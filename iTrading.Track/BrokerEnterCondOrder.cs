namespace iTrading.Track
{
    using System;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.Globalization;
    using iTrading.Core.Kernel;

    internal class BrokerEnterCondOrder : iTrading.Track.Request
    {
        private Order order1;
        private Order order2;

        internal BrokerEnterCondOrder(Adapter adapter, Order order1, Order order2) : base(adapter)
        {
            this.order1 = order1;
            this.order2 = order2;
        }

        internal override void Process(MessageCode msgCode, ByteReader reader)
        {
            if (Globals.TraceSwitch.Order)
            {
                Trace.WriteLine("(" + base.Adapter.connection.IdPlus + ") Track.BrokerEnterCondOrder.Process: " + this.order1.ToString() + "/" + this.order2.ToString());
            }
            if (msgCode == MessageCode.BrokerErrorMessage)
            {
                reader.ReadByte();
                string nativeError = reader.ReadString(80).Trim();
                base.Adapter.connection.ProcessEventArgs(new OrderStatusEventArgs(this.order1, iTrading.Core.Kernel.ErrorCode.OrderRejected, nativeError, this.order1.OrderId, this.order1.LimitPrice, this.order1.StopPrice, this.order1.Quantity, this.order1.AvgFillPrice, this.order1.Filled, base.Adapter.connection.OrderStates[OrderStateId.Rejected], base.Adapter.connection.Now));
                base.Adapter.connection.ProcessEventArgs(new OrderStatusEventArgs(this.order2, iTrading.Core.Kernel.ErrorCode.OrderRejected, nativeError, this.order2.OrderId, this.order2.LimitPrice, this.order2.StopPrice, this.order2.Quantity, this.order2.AvgFillPrice, this.order2.Filled, base.Adapter.connection.OrderStates[OrderStateId.Rejected], base.Adapter.connection.Now));
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
                    Order order = null;
                    if (CultureInfo.InvariantCulture.CompareInfo.Compare(fields[0x10], "OCO_A", CompareOptions.IgnoreCase) == 0)
                    {
                        order = this.order1;
                    }
                    else if (CultureInfo.InvariantCulture.CompareInfo.Compare(fields[0x10], "OCO_B", CompareOptions.IgnoreCase) == 0)
                    {
                        order = this.order2;
                    }
                    else
                    {
                        base.Adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(base.Adapter.connection, iTrading.Core.Kernel.ErrorCode.Panic, fields[0x10], "Track.BrokerEnterCondOrder.Process: Unexpected OCO type in BROKER_TABLE_MESSAGE"));
                    }
                    if (order != null)
                    {
                        BrokerRequestOrder.ProcessNow(base.Adapter, fields, order);
                    }
                }
                else
                {
                    base.Adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(base.Adapter.connection, iTrading.Core.Kernel.ErrorCode.Panic, msgCode.ToString(), "Track.BrokerEnterCondOrder.Process: Unexpected message type"));
                }
            }
        }

        internal override iTrading.Track.ErrorCode Send()
        {
            if (Globals.TraceSwitch.Order)
            {
                Trace.WriteLine("(" + base.Adapter.connection.IdPlus + ") Track.BrokerEnterCondOrder.Send: " + this.order1.ToString() + "/" + this.order2.ToString());
            }
            return (iTrading.Track.ErrorCode) Api.BrokerEnterCondOrder((short) base.Rqn, base.Adapter.ToBrokerOrder(this.order1), base.Adapter.ToBrokerOrder(this.order2), 2);
        }
    }
}

