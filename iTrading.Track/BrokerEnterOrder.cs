namespace iTrading.Track
{
    using System;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using iTrading.Core.Kernel;

    internal class BrokerEnterOrder : iTrading.Track.Request
    {
        private Order order;

        internal BrokerEnterOrder(Adapter adapter, Order order) : base(adapter)
        {
            this.order = order;
        }

        internal override void Process(MessageCode msgCode, ByteReader reader)
        {
            if (Globals.TraceSwitch.Order)
            {
                Trace.WriteLine("(" + base.Adapter.connection.IdPlus + ") Track.BrokerEnterOrder.Process: " + this.order.ToString());
            }
            if (msgCode == MessageCode.BrokerErrorMessage)
            {
                reader.ReadByte();
                string nativeError = reader.ReadString(80).Trim();
                if (this.order.OrderState.Id == OrderStateId.PendingSubmit)
                {
                    base.Adapter.connection.ProcessEventArgs(new OrderStatusEventArgs(this.order, iTrading.Core.Kernel.ErrorCode.NoError, "", this.order.OrderId, this.order.LimitPrice, this.order.StopPrice, this.order.Quantity, this.order.AvgFillPrice, this.order.Filled, base.Adapter.connection.OrderStates[OrderStateId.Accepted], base.Adapter.connection.Now));
                }
                base.Adapter.connection.ProcessEventArgs(new OrderStatusEventArgs(this.order, iTrading.Core.Kernel.ErrorCode.OrderRejected, nativeError, this.order.OrderId, this.order.LimitPrice, this.order.StopPrice, this.order.Quantity, this.order.AvgFillPrice, this.order.Filled, base.Adapter.connection.OrderStates[OrderStateId.Rejected], base.Adapter.connection.Now));
            }
            else if (msgCode != MessageCode.BrokerMessage)
            {
                if (msgCode == MessageCode.BrokerTableMessage)
                {
                    reader.ReadByte();
                    reader.Skip(3);
                    reader.ReadInteger();
                    int countFields = reader.ReadInteger();
                    reader.ReadInteger();
                    StringCollection fields = reader.ReadBrokerTableFields(countFields);
                    BrokerRequestOrder.ProcessNow(base.Adapter, fields, this.order);
                }
                else
                {
                    base.Adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(base.Adapter.connection, iTrading.Core.Kernel.ErrorCode.Panic, msgCode.ToString(), "Track.BrokerEnterOrder.Process: Unexpected message type"));
                }
            }
        }

        internal override iTrading.Track.ErrorCode Send()
        {
            if (Globals.TraceSwitch.Order)
            {
                Trace.WriteLine("(" + base.Adapter.connection.IdPlus + ") Track.BrokerEnterOrder.Send: " + this.order.ToString());
            }
            return (iTrading.Track.ErrorCode) Api.BrokerEnterOrder((short) base.Rqn, base.Adapter.ToBrokerOrder(this.order));
        }
    }
}

