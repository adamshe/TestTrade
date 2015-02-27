using iTrading.Core.Kernel;

namespace iTrading.Track
{
    using System;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.Globalization;
    using iTrading.Core.Kernel;

    internal class BrokerRequestOrder : iTrading.Track.Request
    {
        private Account account;

        internal BrokerRequestOrder(Adapter adapter, Account account) : base(adapter)
        {
            this.account = account;
        }

        internal override void Process(MessageCode msgCode, ByteReader reader)
        {
            if (Globals.TraceSwitch.Order)
            {
                Trace.WriteLine("(" + base.Adapter.connection.IdPlus + ") Track.BrokerRequestOrder.Process: account='" + this.account.Name + "'");
            }
            char ch = (char) reader.ReadByte();
            reader.Skip(3);
            reader.ReadInteger();
            int countFields = reader.ReadInteger();
            reader.ReadInteger();
            StringCollection fields = reader.ReadBrokerTableFields(countFields);
            switch (ch)
            {
                case 'B':
                    ProcessNow(base.Adapter, fields, null);
                    return;

                case 'E':
                    base.Adapter.Init2();
                    break;
            }
        }

        internal static void ProcessNow(Adapter adapter, StringCollection fields, Order order)
        {
            if (Globals.TraceSwitch.Order)
            {
                Trace.WriteLine("(" + adapter.connection.IdPlus + ") Track.BrokerRequestOrder.ProcessNow: " + iTrading.Track.Request.ToString(fields));
            }
            Account account = adapter.connection.Accounts.FindByName(fields[15]);
            if (account == null)
            {
                adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(adapter.connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Track.BrokerRequestOrder.ProcessNow: unknown account id '" + fields[15] + "'"));
            }
            else
            {
                Symbol symbol = adapter.ToSymbol(fields[5]);
                if (symbol == null)
                {
                    adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(adapter.connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Track.BrokerRequestOrder.ProcessNow: unknown symbol '" + fields[5] + "'"));
                }
                else
                {
                    double limitPrice = 0.0;
                    try
                    {
                        limitPrice = Convert.ToDouble(fields[7], adapter.cultureInfo);
                    }
                    catch
                    {
                        adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(adapter.connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Track.BrokerRequestOrder.ProcessNow: illegal limit price '" + fields[7] + "'"));
                        return;
                    }
                    double stopPrice = 0.0;
                    try
                    {
                        stopPrice = Convert.ToDouble(fields[13], adapter.cultureInfo);
                    }
                    catch
                    {
                        adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(adapter.connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Track.BrokerRequestOrder.ProcessNow: illegal stop price '" + fields[13] + "'"));
                        return;
                    }
                    int quantity = 0;
                    try
                    {
                        quantity = Convert.ToInt32(fields[4], adapter.cultureInfo);
                    }
                    catch
                    {
                        adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(adapter.connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Track.BrokerRequestOrder.ProcessNow: illegal quantity '" + fields[4] + "'"));
                        return;
                    }
                    int filled = 0;
                    try
                    {
                        filled = Convert.ToInt32(fields[8], adapter.cultureInfo);
                    }
                    catch
                    {
                        adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(adapter.connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Track.BrokerRequestOrder.ProcessNow: illegal filled quantity '" + fields[8] + "'"));
                        return;
                    }
                    double avgFillPrice = 0.0;
                    try
                    {
                        avgFillPrice = Convert.ToDouble(fields[0x18], adapter.cultureInfo);
                    }
                    catch
                    {
                        adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(adapter.connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Track.BrokerRequestOrder.ProcessNow: illegal avg fill price '" + fields[0x18] + "'"));
                        return;
                    }
                    ActionType type = null;
                    if (CultureInfo.InvariantCulture.CompareInfo.Compare(fields[3], "Buy", CompareOptions.IgnoreCase) == 0)
                    {
                        type = adapter.connection.ActionTypes[ActionTypeId.Buy];
                    }
                    else if (CultureInfo.InvariantCulture.CompareInfo.Compare(fields[3], "BuyC", CompareOptions.IgnoreCase) == 0)
                    {
                        type = adapter.connection.ActionTypes[ActionTypeId.BuyToCover];
                    }
                    else if (CultureInfo.InvariantCulture.CompareInfo.Compare(fields[3], "Sell", CompareOptions.IgnoreCase) == 0)
                    {
                        type = adapter.connection.ActionTypes[ActionTypeId.Sell];
                    }
                    else if (CultureInfo.InvariantCulture.CompareInfo.Compare(fields[3], "SellS", CompareOptions.IgnoreCase) == 0)
                    {
                        type = adapter.connection.ActionTypes[ActionTypeId.SellShort];
                    }
                    else
                    {
                        adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(adapter.connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Track.BrokerRequestOrder.ProcessNow: illegal action type '" + fields[3] + "'"));
                        return;
                    }
                    OrderType type2 = null;
                    if (CultureInfo.InvariantCulture.CompareInfo.Compare(fields[6], "Market", CompareOptions.IgnoreCase) == 0)
                    {
                        type2 = adapter.connection.OrderTypes[OrderTypeId.Market];
                    }
                    else if (CultureInfo.InvariantCulture.CompareInfo.Compare(fields[6], "Limit", CompareOptions.IgnoreCase) == 0)
                    {
                        type2 = adapter.connection.OrderTypes[OrderTypeId.Limit];
                    }
                    else if (CultureInfo.InvariantCulture.CompareInfo.Compare(fields[6], "Stp Mkt", CompareOptions.IgnoreCase) == 0)
                    {
                        type2 = adapter.connection.OrderTypes[OrderTypeId.Stop];
                    }
                    else if (CultureInfo.InvariantCulture.CompareInfo.Compare(fields[6], "Stp Lmt", CompareOptions.IgnoreCase) == 0)
                    {
                        type2 = adapter.connection.OrderTypes[OrderTypeId.StopLimit];
                    }
                    else
                    {
                        adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(adapter.connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Track.BrokerRequestOrder.ProcessNow: illegal order type '" + fields[6] + "'"));
                        return;
                    }
                    TimeInForce force = null;
                    if (CultureInfo.InvariantCulture.CompareInfo.Compare(fields[12], "Day", CompareOptions.IgnoreCase) == 0)
                    {
                        force = adapter.connection.TimeInForces[TimeInForceId.Day];
                    }
                    else if (CultureInfo.InvariantCulture.CompareInfo.Compare(fields[12], "GTC", CompareOptions.IgnoreCase) == 0)
                    {
                        force = adapter.connection.TimeInForces[TimeInForceId.Gtc];
                    }
                    else
                    {
                        adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(adapter.connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Track.BrokerRequestOrder.ProcessNow: illegal tif type '" + fields[12] + "'"));
                        return;
                    }
                    if (order == null)
                    {
                        order = account.Orders.FindByOrderId(fields[9]);
                    }
                    if (order == null)
                    {
                        order = account.CreateOrder(symbol, type.Id, type2.Id, force.Id, quantity, limitPrice, stopPrice, "", null);
                        adapter.connection.ProcessEventArgs(new OrderStatusEventArgs(order, iTrading.Core.Kernel.ErrorCode.NoError, "", order.OrderId, order.LimitPrice, order.StopPrice, order.Quantity, order.AvgFillPrice, order.Filled, adapter.connection.OrderStates[OrderStateId.PendingSubmit], adapter.connection.Now));
                        adapter.connection.ProcessEventArgs(new OrderStatusEventArgs(order, iTrading.Core.Kernel.ErrorCode.NoError, "", fields[9], limitPrice, stopPrice, quantity, avgFillPrice, filled, adapter.connection.OrderStates[OrderStateId.Accepted], adapter.connection.Now));
                    }
                    OrderStateId initialized = OrderStateId.Initialized;
                    if (CultureInfo.InvariantCulture.CompareInfo.Compare(fields[0], "Acked by Broker", CompareOptions.IgnoreCase) == 0)
                    {
                        initialized = OrderStateId.Working;
                    }
                    else if (CultureInfo.InvariantCulture.CompareInfo.Compare(fields[0], "Acked by Tdc", CompareOptions.IgnoreCase) == 0)
                    {
                        initialized = OrderStateId.Accepted;
                    }
                    else if (CultureInfo.InvariantCulture.CompareInfo.Compare(fields[0], "Canceled by User", CompareOptions.IgnoreCase) == 0)
                    {
                        initialized = OrderStateId.Cancelled;
                    }
                    else if (CultureInfo.InvariantCulture.CompareInfo.Compare(fields[0], "Partial fill", CompareOptions.IgnoreCase) == 0)
                    {
                        initialized = OrderStateId.PartFilled;
                    }
                    else if (CultureInfo.InvariantCulture.CompareInfo.Compare(fields[0], "Order Completed", CompareOptions.IgnoreCase) == 0)
                    {
                        if (order.OrderState.Id == OrderStateId.Accepted)
                        {
                            adapter.connection.ProcessEventArgs(new OrderStatusEventArgs(order, iTrading.Core.Kernel.ErrorCode.NoError, "", fields[9], limitPrice, stopPrice, quantity, avgFillPrice, filled, adapter.connection.OrderStates[OrderStateId.Working], adapter.connection.Now));
                        }
                        initialized = OrderStateId.Filled;
                    }
                    if ((initialized != OrderStateId.Initialized) && ((initialized == OrderStateId.PartFilled) || (initialized != order.OrderState.Id)))
                    {
                        adapter.connection.ProcessEventArgs(new OrderStatusEventArgs(order, iTrading.Core.Kernel.ErrorCode.NoError, "", fields[9], limitPrice, stopPrice, quantity, avgFillPrice, filled, adapter.connection.OrderStates[initialized], adapter.connection.Now));
                    }
                }
            }
        }

        internal override iTrading.Track.ErrorCode Send()
        {
            string startDate = base.Adapter.connection.Now.ToString("MM/dd/yy");
            if (Globals.TraceSwitch.Order)
            {
                Trace.WriteLine("(" + base.Adapter.connection.IdPlus + ") Track.BrokerRequestOrder.Send: account='" + this.account.Name + "' endDate='" + startDate + "'");
            }
            return (iTrading.Track.ErrorCode) Api.BrokerRequestOrder((short) base.Rqn, this.account.Name, startDate, startDate, 1);
        }
    }
}

