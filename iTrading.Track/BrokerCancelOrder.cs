namespace iTrading.Track
{
    using System;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using iTrading.Core.Kernel;

    internal class BrokerCancelOrder : iTrading.Track.Request
    {
        private Order order;

        internal BrokerCancelOrder(Adapter adapter, Order order) : base(adapter)
        {
            this.order = order;
        }

        internal override void Process(MessageCode msgCode, ByteReader reader)
        {
            if (Globals.TraceSwitch.Order)
            {
                Trace.WriteLine("(" + base.Adapter.connection.IdPlus + ") Track.BrokerCancelOrder.Process: " + this.order.ToString());
            }
            if (msgCode == MessageCode.BrokerErrorMessage)
            {
                reader.ReadByte();
                string str = reader.ReadString(80).Trim();
                OrderState orderState = this.order.OrderState;
                int quantity = this.order.Quantity;
                double limitPrice = this.order.LimitPrice;
                double stopPrice = this.order.StopPrice;
                if (this.order.History.Count >= 2)
                {
                    limitPrice = this.order.History[this.order.History.Count - 2].LimitPrice;
                    stopPrice = this.order.History[this.order.History.Count - 2].StopPrice;
                    orderState = this.order.History[this.order.History.Count - 2].OrderState;
                    quantity = this.order.History[this.order.History.Count - 2].Quantity;
                }
                base.Adapter.connection.ProcessEventArgs(new OrderStatusEventArgs(this.order, iTrading.Core.Kernel.ErrorCode.UnableToCancelOrder, "Order '" + this.order.OrderId + "' can't be cancelled now: " + str, this.order.OrderId, limitPrice, stopPrice, quantity, this.order.AvgFillPrice, this.order.Filled, orderState, base.Adapter.connection.Now));
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
                Trace.WriteLine("(" + base.Adapter.connection.IdPlus + ") Track.BrokerCancelOrder.Send: " + this.order.ToString());
            }
            return (iTrading.Track.ErrorCode) Api.BrokerCancelOrder((short) base.Rqn, this.order.Account.Name, this.order.OrderId);
        }
    }
}

