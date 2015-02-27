namespace iTrading.Mbt
{
    using MBTORDERSLib;
    using System;
    using System.Collections;
    using System.Diagnostics;
    using iTrading.Core.Kernel;

    internal class Callback
    {
        private Adapter adapter;

        internal Callback(Adapter adapter)
        {
            this.adapter = adapter;
        }

        internal void AddOrder(MbtAccount mbtAccount, string symbol, iTrading.Mbt.ActionType action, iTrading.Mbt.OrderType orderType, string orderNumber, int quantity, double price, double stopLimit, ArrayList histories)
        {
            MbtOrderHistory history;
            double limitPrice;
            double stopPrice;
            ErrorCode noError;
            string message;
            OrderState state;
            int num6;
            string str2;
            Account account = this.adapter.connection.Accounts.FindByName(mbtAccount.Account);
            if (account == null)
            {
                this.Connection.ProcessEventArgs(new ITradingErrorEventArgs(this.Connection, ErrorCode.Panic, "", "Mbt.Callback.AddOrder: unknown account '" + mbtAccount.Account + "'"));
                return;
            }
            ActionTypeId buy = ActionTypeId.Buy;
            switch (action)
            {
                case iTrading.Mbt.ActionType.Buy:
                    buy = ActionTypeId.Buy;
                    break;

                case iTrading.Mbt.ActionType.Sell:
                    buy = ActionTypeId.Sell;
                    break;

                case iTrading.Mbt.ActionType.SellShort:
                    buy = ActionTypeId.SellShort;
                    break;

                default:
                    this.Connection.ProcessEventArgs(new ITradingErrorEventArgs(this.Connection, ErrorCode.Panic, "", "Mbt.Callback.AddOrder: unknown action type '" + action + "'"));
                    return;
            }
            OrderTypeId market = OrderTypeId.Market;
            switch (orderType)
            {
                case iTrading.Mbt.OrderType.Limit:
                    market = OrderTypeId.Limit;
                    break;

                case iTrading.Mbt.OrderType.Market:
                    market = OrderTypeId.Market;
                    break;

                case iTrading.Mbt.OrderType.StopMarket:
                    market = OrderTypeId.Stop;
                    break;

                case iTrading.Mbt.OrderType.StopLimit:
                    market = OrderTypeId.StopLimit;
                    break;

                default:
                    this.Connection.ProcessEventArgs(new ITradingErrorEventArgs(this.Connection, ErrorCode.Panic, "", "Mbt.Callback.AddOrder: unknown order type '" + orderType + "'"));
                    return;
            }
            double avgFillPrice = 0.0;
            int num2 = 0;
            int filled = 0;
            OrderStatusEventCollection events = new OrderStatusEventCollection();
            this.adapter.orderClient.OrderHistories.LockItems();
        Label_0146:
            do
            {
                do
                {
                    do
                    {
                        history = null;
                        if (histories == null)
                        {
                            if (num2 >= this.adapter.orderClient.OrderHistories.Count)
                            {
                                goto Label_0409;
                            }
                            history = this.adapter.orderClient.OrderHistories[num2];
                        }
                        else
                        {
                            if (num2 >= histories.Count)
                            {
                                goto Label_0409;
                            }
                            history = (MbtOrderHistory) histories[num2];
                        }
                        num2++;
                    }
                    while (!(history.OrderNumber == orderNumber));
                    if (history.Event == "Executed")
                    {
                        avgFillPrice = ((filled + history.Quantity) == 0) ? 0.0 : (((filled * avgFillPrice) + (history.Quantity * history.Price)) / ((double) (filled + history.Quantity)));
                        filled += history.Quantity;
                    }
                    limitPrice = history.Price;
                    stopPrice = history.StopLimit;
                    if ((market != OrderTypeId.Limit) && (market != OrderTypeId.StopLimit))
                    {
                        limitPrice = 0.0;
                    }
                    noError = ErrorCode.NoError;
                    message = "";
                    state = this.adapter.connection.OrderStates[OrderStateId.Initialized];
                    num6 = history.Quantity;
                }
                while ((str2 = history.Event) == null);
                str2 = string.IsInterned(str2);
                if (str2 == "Enter")
                {
                    state = this.Connection.OrderStates[OrderStateId.Accepted];
                    goto Label_03CE;
                }
                if (str2 == "Executed")
                {
                    limitPrice = events[events.Count - 1].LimitPrice;
                    stopPrice = events[events.Count - 1].StopPrice;
                    num6 = quantity;
                    state = this.Connection.OrderStates[(filled >= quantity) ? OrderStateId.Filled : OrderStateId.PartFilled];
                    goto Label_03CE;
                }
                if (str2 == "Live")
                {
                    stopPrice = events[events.Count - 1].StopPrice;
                    state = this.Connection.OrderStates[OrderStateId.Working];
                    goto Label_03CE;
                }
                if (str2 == "Order Reject")
                {
                    noError = ErrorCode.OrderRejected;
                    message = history.Message;
                    state = this.Connection.OrderStates[OrderStateId.Rejected];
                    goto Label_03CE;
                }
            }
            while (str2 != "Order Cancelled");
            limitPrice = events[events.Count - 1].LimitPrice;
            stopPrice = events[events.Count - 1].StopPrice;
            state = this.Connection.OrderStates[OrderStateId.Cancelled];
        Label_03CE:
            events.Add(new OrderStatusEventArgs(null, noError, message, history.OrderNumber, limitPrice, stopPrice, num6, avgFillPrice, filled, state, Adapter.Convert(history.Date, history.Time)));
            goto Label_0146;
        Label_0409:
            this.adapter.orderClient.OrderHistories.UnlockItems();
            Order order = account.Orders.FindByOrderId(orderNumber);
            if (order != null)
            {
                lock (order.History)
                {
                    foreach (OrderStatusEventArgs args in events)
                    {
                        OrderStatusEventArgs args2 = null;
                        foreach (OrderStatusEventArgs args3 in order.History)
                        {
                            if ((args.OrderState.Id == args3.OrderState.Id) && (args.Time == args3.Time))
                            {
                                args2 = args3;
                                break;
                            }
                        }
                        if (args2 == null)
                        {
                            this.adapter.connection.ProcessEventArgs(new OrderStatusEventArgs(order, args.Error, args.NativeError, args.OrderId, args.LimitPrice, args.StopPrice, args.Quantity, args.AvgFillPrice, args.Filled, args.OrderState, args.Time));
                        }
                    }
                    return;
                }
            }
            account.CreateOrder(this.adapter.Convert(symbol), buy, market, TimeInForceId.Day, quantity, price, stopLimit, "", (events.Count == 0) ? this.adapter.connection.OrderStates[OrderStateId.Initialized] : events[0].OrderState, orderNumber, "", events, null);
        }

        private void CancelOrderNow(object order)
        {
            this.adapter.Cancel((Order) order);
        }

        private Order FindOrder(MbtAccount mbtAccount, string token, string orderId, bool errorOnFail)
        {
            Account account = this.adapter.connection.Accounts.FindByName(mbtAccount.Account);
            if (account == null)
            {
                this.Connection.ProcessEventArgs(new ITradingErrorEventArgs(this.Connection, ErrorCode.Panic, "", "Mbt.Callback.Order: unknown account '" + mbtAccount.Account + "'"));
                return null;
            }
            Order order = account.Orders.FindByOrderId(token);
            if (order != null)
            {
                return order;
            }
            order = account.Orders.FindByOrderId(orderId);
            if (order != null)
            {
                return order;
            }
            if (errorOnFail)
            {
                this.Connection.ProcessEventArgs(new ITradingErrorEventArgs(this.Connection, ErrorCode.Panic, "", "Mbt.Callback.FindOrder: unknown order '" + orderId + "'"));
            }
            return null;
        }

        internal void OnHistoryAdded(MbtOrderHistory orderHistory)
        {
            if (this.adapter.connection.ConnectionStatusId == ConnectionStatusId.Connected)
            {
                try
                {
                    if (Globals.TraceSwitch.Order)
                    {
                        Trace.WriteLine("(" + this.Connection.IdPlus + ") Mbt.Callback.OnHistoryAdded: " + orderHistory.OrderNumber + " " + orderHistory.Event);
                    }
                    Order order = this.FindOrder(orderHistory.Account, orderHistory.Token, orderHistory.OrderNumber, true);
                    if (order != null)
                    {
                        if (orderHistory.Event == "Enter")
                        {
                            double price = orderHistory.Price;
                            double stopLimit = orderHistory.StopLimit;
                            if ((order.OrderType.Id != OrderTypeId.Limit) && (order.OrderType.Id != OrderTypeId.StopLimit))
                            {
                                price = 0.0;
                            }
                            if (order.OrderState.Id == OrderStateId.Initialized)
                            {
                                this.Connection.ProcessEventArgs(new OrderStatusEventArgs(order, ErrorCode.NoError, "", orderHistory.OrderNumber, price, stopLimit, orderHistory.Quantity, order.AvgFillPrice, order.Filled, this.Connection.OrderStates[OrderStateId.PendingSubmit], Adapter.Convert(orderHistory.Date, orderHistory.Time)));
                                this.Connection.ProcessEventArgs(new OrderStatusEventArgs(order, ErrorCode.NoError, "", orderHistory.OrderNumber, price, stopLimit, orderHistory.Quantity, order.AvgFillPrice, order.Filled, this.Connection.OrderStates[OrderStateId.Accepted], Adapter.Convert(orderHistory.Date, orderHistory.Time)));
                            }
                        }
                        else if (orderHistory.Event == "Executed")
                        {
                            int num3 = this.adapter.orderClient.OrderHistories.Count - 1;
                            DateTime time = Adapter.Convert(orderHistory.Date, orderHistory.Time);
                            string executionId = "TM_" + time.ToString("yyyyMMdd") + "_" + num3.ToString("0000");
                            this.Connection.ProcessEventArgs(new ExecutionUpdateEventArgs(this.Connection, ErrorCode.NoError, "", Operation.Insert, executionId, order.Account, order.Symbol, time, this.Connection.MarketPositions[(orderHistory.BuySell == 0x2710) ? MarketPositionId.Long : MarketPositionId.Short], orderHistory.OrderNumber, orderHistory.Quantity, orderHistory.Price));
                            this.UpdateAccount(orderHistory.Account, time);
                            this.Connection.ProcessEventArgs(new OrderStatusEventArgs(order, ErrorCode.NoError, "", orderHistory.OrderNumber, order.LimitPrice, order.StopPrice, order.Quantity, ((order.Filled + orderHistory.Quantity) == 0) ? 0.0 : (((order.AvgFillPrice * order.Filled) + (orderHistory.Price * orderHistory.Quantity)) / ((double) (order.Filled + orderHistory.Quantity))), order.Filled + orderHistory.Quantity, this.Connection.OrderStates[((orderHistory.Quantity + order.Filled) >= order.Quantity) ? OrderStateId.Filled : OrderStateId.PartFilled], Adapter.Convert(orderHistory.Date, orderHistory.Time)));
                        }
                        else if (orderHistory.Event == "Live")
                        {
                            double limitPrice = orderHistory.Price;
                            double stopPrice = order.StopPrice;
                            if ((order.OrderType.Id != OrderTypeId.Limit) && (order.OrderType.Id != OrderTypeId.StopLimit))
                            {
                                limitPrice = 0.0;
                            }
                            if (order.OrderState.Id != OrderStateId.Accepted)
                            {
                                this.Connection.ProcessEventArgs(new OrderStatusEventArgs(order, ErrorCode.NoError, "", orderHistory.OrderNumber, limitPrice, stopPrice, orderHistory.Quantity, order.AvgFillPrice, order.Filled, this.Connection.OrderStates[OrderStateId.Accepted], Adapter.Convert(orderHistory.Date, orderHistory.Time)));
                            }
                            this.Connection.ProcessEventArgs(new OrderStatusEventArgs(order, ErrorCode.NoError, "", orderHistory.OrderNumber, limitPrice, stopPrice, orderHistory.Quantity, order.AvgFillPrice, order.Filled, this.Connection.OrderStates[OrderStateId.Working], Adapter.Convert(orderHistory.Date, orderHistory.Time)));
                        }
                        else if (orderHistory.Event == "Order Reject")
                        {
                            this.Connection.ProcessEventArgs(new OrderStatusEventArgs(order, ErrorCode.OrderRejected, orderHistory.Message, orderHistory.OrderNumber, order.LimitPrice, order.StopPrice, order.Quantity, order.AvgFillPrice, order.Filled, this.Connection.OrderStates[OrderStateId.Rejected], Adapter.Convert(orderHistory.Date, orderHistory.Time)));
                        }
                        else if (orderHistory.Event == "Order Cancelled")
                        {
                            this.Connection.ProcessEventArgs(new OrderStatusEventArgs(order, ErrorCode.NoError, "", orderHistory.OrderNumber, order.LimitPrice, order.StopPrice, order.Quantity, order.AvgFillPrice, order.Filled, this.Connection.OrderStates[OrderStateId.Cancelled], Adapter.Convert(orderHistory.Date, orderHistory.Time)));
                        }
                    }
                }
                catch (Exception exception)
                {
                    this.Connection.ProcessEventArgs(new ITradingErrorEventArgs(this.Connection, ErrorCode.Panic, "", "Mbt.Callback.OnHistoryAdded: exception caught: " + exception.Message));
                }
            }
        }

        internal void OnPositionAdded(MbtPosition position)
        {
            if ((this.adapter.connection.ConnectionStatusId == ConnectionStatusId.Connected) || (this.adapter.connection.ConnectionStatusId == ConnectionStatusId.Connecting))
            {
                try
                {
                    if (Math.Abs((int) (position.IntradayPosition + position.OvernightPosition)) != 0)
                    {
                        Account account = this.adapter.connection.Accounts.FindByName(position.Account.Account);
                        if (account == null)
                        {
                            this.adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.adapter.connection, ErrorCode.Panic, "", "Mbt.Adapter.ConnectNow: unknown account '" + position.Account.Account + "'"));
                        }
                        else
                        {
                            Operation insert = Operation.Insert;
                            Symbol symbol = this.adapter.Convert(position.Symbol);
                            if (account.Positions.FindBySymbol(symbol) != null)
                            {
                                insert = Operation.Update;
                            }
                            this.adapter.connection.ProcessEventArgs(new PositionUpdateEventArgs(this.adapter.connection, ErrorCode.NoError, "", insert, account, symbol, this.adapter.connection.MarketPositions[((position.IntradayPosition + position.OvernightPosition) > 0) ? MarketPositionId.Long : MarketPositionId.Short], Math.Abs((int) (position.IntradayPosition + position.OvernightPosition)), this.adapter.connection.Currencies[CurrencyId.Unknown], Symbol.Round2TickSize(((position.IntradayPosition * position.IntradayPrice) + (position.OvernightPosition * position.OvernightPrice)) / ((double) (position.IntradayPosition + position.OvernightPosition)), 0.0001)));
                        }
                    }
                }
                catch (Exception exception)
                {
                    this.Connection.ProcessEventArgs(new ITradingErrorEventArgs(this.Connection, ErrorCode.Panic, "", "Mbt.Callback.OnPositionAdded: exception caught: " + exception.Message));
                }
            }
        }

        internal void OnPositionUpdated(MbtPosition position)
        {
            if ((this.adapter.connection.ConnectionStatusId == ConnectionStatusId.Connected) || (this.adapter.connection.ConnectionStatusId == ConnectionStatusId.Connecting))
            {
                try
                {
                    Account account = this.adapter.connection.Accounts.FindByName(position.Account.Account);
                    if (account == null)
                    {
                        this.adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.adapter.connection, ErrorCode.Panic, "", "Mbt.Adapter.ConnectNow: unknown account '" + position.Account.Account + "'"));
                    }
                    else
                    {
                        Symbol symbol = this.adapter.Convert(position.Symbol);
                        Position position2 = account.Positions.FindBySymbol(symbol);
                        Operation operation = (position2 == null) ? Operation.Insert : (((position.IntradayPosition + position.OvernightPosition) == 0) ? Operation.Delete : Operation.Update);
                        if (((position2 != null) || ((position.IntradayPosition + position.OvernightPosition) != 0)) && (((position2 == null) || (position2.Quantity != Math.Abs((int) (position.IntradayPosition + position.OvernightPosition)))) || (position2.MarketPosition.Id != this.adapter.connection.MarketPositions[((position.IntradayPosition + position.OvernightPosition) > 0) ? MarketPositionId.Long : MarketPositionId.Short].Id)))
                        {
                            this.adapter.connection.ProcessEventArgs(new PositionUpdateEventArgs(this.adapter.connection, ErrorCode.NoError, "", operation, account, symbol, this.adapter.connection.MarketPositions[((position.IntradayPosition + position.OvernightPosition) > 0) ? MarketPositionId.Long : MarketPositionId.Short], (operation == Operation.Delete) ? 0 : Math.Abs((int) (position.IntradayPosition + position.OvernightPosition)), this.adapter.connection.Currencies[CurrencyId.Unknown], Symbol.Round2TickSize(((position.IntradayPosition * position.IntradayPrice) + (position.OvernightPosition * position.OvernightPrice)) / ((double) (position.IntradayPosition + position.OvernightPosition)), 0.0001)));
                        }
                    }
                }
                catch (Exception exception)
                {
                    this.Connection.ProcessEventArgs(new ITradingErrorEventArgs(this.Connection, ErrorCode.Panic, "", "Mbt.Callback.OnPositionUpdated: exception caught: " + exception.Message));
                }
            }
        }

        internal void OnSubmit(MbtOpenOrder openOrder)
        {
            if (this.adapter.connection.ConnectionStatusId == ConnectionStatusId.Connected)
            {
                try
                {
                    if (Globals.TraceSwitch.Order)
                    {
                        Trace.WriteLine("(" + this.Connection.IdPlus + ") Mbt.Callback.OnSubmit0: " + openOrder.OrderNumber);
                    }
                    lock (this.adapter.orders2Cancel)
                    {
                        if (this.adapter.orders2Cancel.Count > 0)
                        {
                            ArrayList list = this.adapter.orders2Cancel;
                            this.adapter.orders2Cancel = new ArrayList();
                            foreach (Order order in list)
                            {
                                if (Globals.TraceSwitch.Order)
                                {
                                    Trace.WriteLine("(" + this.adapter.connection.IdPlus + ") Mbt.Callback.OnSubmit1: " + order.ToString());
                                }
                                this.adapter.connection.SynchronizeInvoke.Invoke(new Adapter.Process(this.CancelOrderNow), new object[] { order });
                            }
                        }
                    }
                    Order order2 = this.FindOrder(openOrder.Account, openOrder.Token, openOrder.OrderNumber, false);
                    if (order2 == null)
                    {
                        this.AddOrder(openOrder.Account, openOrder.Symbol, (iTrading.Mbt.ActionType) openOrder.BuySell, (iTrading.Mbt.OrderType) openOrder.OrderType, openOrder.OrderNumber, openOrder.Quantity, openOrder.Price, openOrder.StopLimit, null);
                    }
                    else
                    {
                        double price = openOrder.Price;
                        if ((order2.OrderType.Id != OrderTypeId.Limit) && (order2.OrderType.Id != OrderTypeId.StopLimit))
                        {
                            price = 0.0;
                        }
                        this.Connection.ProcessEventArgs(new OrderStatusEventArgs(order2, ErrorCode.NoError, "", openOrder.OrderNumber, price, openOrder.StopLimit, openOrder.Quantity, order2.AvgFillPrice, order2.Filled, this.Connection.OrderStates[(((order2.OrderState.Id == OrderStateId.PendingSubmit) || (order2.OrderState.Id == OrderStateId.PendingCancel)) || (order2.OrderState.Id == OrderStateId.PendingChange)) ? OrderStateId.Accepted : order2.OrderState.Id], Adapter.Convert(openOrder.Date, openOrder.Time)));
                    }
                }
                catch (Exception exception)
                {
                    this.Connection.ProcessEventArgs(new ITradingErrorEventArgs(this.Connection, ErrorCode.Panic, "", "Mbt.Callback.OnSubmit: exception caught: " + exception.Message));
                }
            }
        }

        internal void UpdateAccount(MbtAccount account, DateTime time)
        {
            this.Connection.ProcessEventArgs(new AccountUpdateEventArgs(this.Connection, ErrorCode.NoError, "", this.Connection.Accounts.FindByName(account.Account), this.Connection.AccountItemTypes[AccountItemTypeId.BuyingPower], this.Connection.Currencies[CurrencyId.Unknown], account.CurrentBP, time));
            this.Connection.ProcessEventArgs(new AccountUpdateEventArgs(this.Connection, ErrorCode.NoError, "", this.Connection.Accounts.FindByName(account.Account), this.Connection.AccountItemTypes[AccountItemTypeId.CashValue], this.Connection.Currencies[CurrencyId.Unknown], account.CurrentEquity, time));
        }

        private Connection Connection
        {
            get
            {
                return this.adapter.connection;
            }
        }
    }
}

