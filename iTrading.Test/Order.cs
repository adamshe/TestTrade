using iTrading.Core.Kernel;

namespace iTrading.Test
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.IO;
    using System.Threading;
    using System.Windows.Forms;
    using System.Xml;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Test order placement. Orders are executed only on the demo account.
    /// </summary>
    public class Order : TestBase
    {
        private Account account = null;
        private ActionTypeId actionType;
        private ErrorCode expectedError = ErrorCode.NoError;
        private double fillPrice;
        private static int futuresToOrder = 0;
        private double limitPrice;
        private const int maxFuturesToOrder = 3;
        private int maxQuantity = 0xf4240;
        private iTrading.Core.Kernel.Order order;
        private iTrading.Core.Kernel.Order order1;
        private iTrading.Core.Kernel.Order order2;
        private OrderTypeId orderType;
        private int quantity;
        private const int quantityStocks = 1;
        private int state;
        internal static bool stopOrders = false;
        private double stopPrice;
        private Symbol symbol;
        private bool testError;
        private bool updateCancelOrder;

        /// <summary>
        /// 
        /// </summary>
        public Order()
        {
            base.ExecuteInLiveMode = false;
        }

        private void AccountUpdate(object sender, AccountUpdateEventArgs e)
        {
             iTrading.Test.Globals.Assert(base.GetType().FullName + " a00", e.Account != null);
             iTrading.Test.Globals.Assert(base.GetType().FullName + " a01", e.Currency != null);
             iTrading.Test.Globals.Assert(base.GetType().FullName + " a02", e.ItemType != null);
        }

        private bool CheckExecutions(iTrading.Core.Kernel.Order order)
        {
            int num = 0;
            lock (order.Account.Executions)
            {
                for (int i = order.Account.Executions.Count - 1; i >= 0; i--)
                {
                    if (order.Account.Executions[i].OrderId == order.OrderId)
                    {
                        num += order.Account.Executions[i].Quantity;
                    }
                    if (num == order.Quantity)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Creates an instance of this class.
        /// </summary>
        /// <returns></returns>
        protected override TestBase CreateInstance()
        {
            return new  iTrading.Test.Order();
        }

        /// <summary>
        /// Execute test.
        /// </summary>
        protected override void DoTest()
        {
            if (base.Connection.FeatureTypes[FeatureTypeId.Order] != null)
            {
                ArrayList list = new ArrayList();
                OrderType type = base.Connection.OrderTypes[OrderTypeId.Market];
                if (type == null)
                {
                     iTrading.Test.Globals.Assert(base.GetType().FullName + " 200", false);
                }
                list.Add(type.Id);
                type = base.Connection.OrderTypes[OrderTypeId.Limit];
                if (type != null)
                {
                    list.Add(type.Id);
                }
                if (stopOrders)
                {
                    type = base.Connection.OrderTypes[OrderTypeId.Stop];
                    if (type != null)
                    {
                        list.Add(type.Id);
                    }
                    type = base.Connection.OrderTypes[OrderTypeId.StopLimit];
                    if (type != null)
                    {
                        list.Add(type.Id);
                    }
                }
                if (this.account == null)
                {
                    this.account = base.Connection.Accounts[0];
                }
                this.account.AccountUpdate += new AccountUpdateEventHandler(this.AccountUpdate);
                this.account.Execution += new ExecutionUpdateEventHandler(this.Executions);
                this.account.OrderStatus += new OrderStatusEventHandler(this.OrderStatus);
                this.account.PositionUpdate += new PositionUpdateEventHandler(this.Positions_PositionUpdate);
                if (!base.RunMultiple && !base.RunMultiThread)
                {
                    base.Connection.Symbols.Clear();
                }
                if ((base.Connection.Options.Provider.Id == ProviderTypeId.MBTrading) || this.account.IsSimulation)
                {
                    foreach (SymbolTemplate template in base.SymbolTemplates)
                    {
                        if (template.Valid)
                        {
                            this.symbol = base.Connection.GetSymbol(template.Name, template.Expiry, base.Connection.SymbolTypes[template.SymbolTypeId], base.Connection.Exchanges[template.ExchangeId], 0.0, RightId.Unknown, LookupPolicyId.RepositoryAndProvider);
                            this.symbol.MarketData.MarketDataItem += new MarketDataItemEventHandler(this.MarketData_MarketDataItem);
                        }
                    }
                    for (int i = 0; i < 500; i++)
                    {
                        Thread.Sleep(10);
                        Application.DoEvents();
                    }
                    foreach (SymbolTemplate template2 in base.SymbolTemplates)
                    {
                        if (template2.Valid)
                        {
                            this.symbol = base.Connection.GetSymbol(template2.Name, template2.Expiry, base.Connection.SymbolTypes[template2.SymbolTypeId], base.Connection.Exchanges[template2.ExchangeId], 0.0, RightId.Unknown, LookupPolicyId.RepositoryAndProvider);
                            if ((this.symbol.SymbolType.Id != SymbolTypeId.Index) && !this.account.IsSimulation)
                            {
                                 iTrading.Test.Globals.Assert(base.GetType().FullName + " start market data 01 " + template2.Name, this.symbol.MarketData.Ask != null);
                                 iTrading.Test.Globals.Assert(base.GetType().FullName + " start market data 02 " + template2.Name, this.symbol.MarketData.Bid != null);
                                 iTrading.Test.Globals.Assert(base.GetType().FullName + " start market data 03 " + template2.Name, this.symbol.MarketData.Last != null);
                            }
                        }
                    }
                }
                foreach (SymbolTemplate template3 in base.SymbolTemplates)
                {
                    if ((template3.SymbolTypeId != SymbolTypeId.Index) && ((template3.SymbolTypeId != SymbolTypeId.Future) || (base.Connection.Options.Provider.Id != ProviderTypeId.TrackData)))
                    {
                        this.updateCancelOrder = false;
                        this.fillPrice = 0.0;
                        this.order = null;
                        this.testError = false;
                        foreach (OrderTypeId id in list)
                        {
                            this.orderType = id;
                            if (template3.Valid && ((this.orderType == OrderTypeId.Market) || (this.fillPrice != 0.0)))
                            {
                                if (iTrading.Core.Kernel.Globals.TraceSwitch.SymbolLookup)
                                {
                                    Trace.WriteLine(string.Concat(new object[] { "(", base.Connection.IdPlus, ") Test.Order.DoTest.Find: ", template3.Name, " ", template3.Expiry.ToString("yy"), "/", template3.Expiry.ToString("MM"), " ", template3.ExchangeId }));
                                }
                                this.symbol = base.Connection.GetSymbol(template3.Name, template3.Expiry, base.Connection.SymbolTypes[template3.SymbolTypeId], base.Connection.Exchanges[template3.ExchangeId], 0.0, RightId.Unknown, LookupPolicyId.RepositoryAndProvider);
                                if (((base.Broker.Id != ProviderTypeId.MBTrading) || !base.RunMultiThread) || (this.symbol.SymbolType.Id != SymbolTypeId.Stock))
                                {
                                    this.state = 0;
                                    this.actionType = ActionTypeId.Buy;
                                    this.quantity = (this.symbol.SymbolType.Id == SymbolTypeId.Stock) ? 1 : 1;
                                    if (iTrading.Core.Kernel.Globals.TraceSwitch.SymbolLookup)
                                    {
                                        Trace.WriteLine("(" + base.Connection.IdPlus + ") Test.Order.DoTest.Found: " + this.symbol.FullName);
                                    }
                                    switch (this.orderType)
                                    {
                                        case OrderTypeId.Market:
                                            this.limitPrice = 0.0;
                                            this.stopPrice = 0.0;
                                            break;

                                        case OrderTypeId.Limit:
                                            this.limitPrice = this.fillPrice * 1.01;
                                            this.stopPrice = 0.0;
                                            break;

                                        case OrderTypeId.Stop:
                                            this.limitPrice = 0.0;
                                            this.stopPrice = this.fillPrice * 0.99;
                                            break;

                                        case OrderTypeId.StopLimit:
                                            this.limitPrice = this.fillPrice * 1.01;
                                            this.stopPrice = this.fillPrice * 0.99;
                                            break;
                                    }
                                    if (iTrading.Core.Kernel.Globals.TraceSwitch.Test)
                                    {
                                        Trace.WriteLine("(" + base.Connection.IdPlus + ") Test.Order.DoTest.1: " + this.symbol.FullName);
                                    }
                                    this.ProcessState(null);
                                    while (this.state < 10)
                                    {
                                         iTrading.Test.Globals.Sleep( iTrading.Test.Globals.MilliSeconds2Sleep);
                                    }
                                    this.state = 0;
                                    this.actionType = ActionTypeId.Sell;
                                    switch (this.orderType)
                                    {
                                        case OrderTypeId.Market:
                                            this.limitPrice = 0.0;
                                            this.stopPrice = 0.0;
                                            break;

                                        case OrderTypeId.Limit:
                                            this.limitPrice = this.fillPrice * 0.99;
                                            this.stopPrice = 0.0;
                                            break;

                                        case OrderTypeId.Stop:
                                            this.limitPrice = 0.0;
                                            this.stopPrice = this.fillPrice * 1.01;
                                            break;

                                        case OrderTypeId.StopLimit:
                                            this.limitPrice = this.fillPrice * 0.99;
                                            this.stopPrice = this.fillPrice * 1.01;
                                            break;
                                    }
                                    if (iTrading.Core.Kernel.Globals.TraceSwitch.Test)
                                    {
                                        Trace.WriteLine("(" + base.Connection.IdPlus + ") Test.Order.DoTest.2: " + this.symbol.FullName);
                                    }
                                    this.ProcessState(null);
                                    while (this.state < 10)
                                    {
                                         iTrading.Test.Globals.Sleep( iTrading.Test.Globals.MilliSeconds2Sleep);
                                    }
                                    if (base.Connection.Options.Provider.Id != ProviderTypeId.TrackData)
                                    {
                                        this.state = 0;
                                        this.actionType = ActionTypeId.SellShort;
                                        this.quantity = (this.symbol.SymbolType.Id == SymbolTypeId.Stock) ? 1 : 1;
                                        switch (this.orderType)
                                        {
                                            case OrderTypeId.Market:
                                                this.limitPrice = 0.0;
                                                this.stopPrice = 0.0;
                                                break;

                                            case OrderTypeId.Limit:
                                                this.limitPrice = this.fillPrice * 0.99;
                                                this.stopPrice = 0.0;
                                                break;

                                            case OrderTypeId.Stop:
                                                this.limitPrice = 0.0;
                                                this.stopPrice = this.fillPrice * 1.01;
                                                break;

                                            case OrderTypeId.StopLimit:
                                                this.limitPrice = this.fillPrice * 0.99;
                                                this.stopPrice = this.fillPrice * 1.01;
                                                break;
                                        }
                                        if (iTrading.Core.Kernel.Globals.TraceSwitch.Test)
                                        {
                                            Trace.WriteLine("(" + base.Connection.IdPlus + ") Test.Order.DoTest.3: " + this.symbol.FullName);
                                        }
                                        this.ProcessState(null);
                                        while (this.state < 10)
                                        {
                                             iTrading.Test.Globals.Sleep( iTrading.Test.Globals.MilliSeconds2Sleep);
                                        }
                                        this.state = 0;
                                        this.actionType = ActionTypeId.BuyToCover;
                                        switch (this.orderType)
                                        {
                                            case OrderTypeId.Market:
                                                this.limitPrice = 0.0;
                                                this.stopPrice = 0.0;
                                                break;

                                            case OrderTypeId.Limit:
                                                this.limitPrice = this.fillPrice * 1.01;
                                                this.stopPrice = 0.0;
                                                break;

                                            case OrderTypeId.Stop:
                                                this.limitPrice = 0.0;
                                                this.stopPrice = this.fillPrice * 0.99;
                                                break;

                                            case OrderTypeId.StopLimit:
                                                this.limitPrice = this.fillPrice * 1.01;
                                                this.stopPrice = this.fillPrice * 0.99;
                                                break;
                                        }
                                        if (iTrading.Core.Kernel.Globals.TraceSwitch.Test)
                                        {
                                            Trace.WriteLine("(" + base.Connection.IdPlus + ") Test.Order.DoTest.4: " + this.symbol.FullName);
                                        }
                                        this.ProcessState(null);
                                        while (this.state < 10)
                                        {
                                             iTrading.Test.Globals.Sleep( iTrading.Test.Globals.MilliSeconds2Sleep);
                                        }
                                    }
                                }
                            }
                        }
                        type = base.Connection.OrderTypes[OrderTypeId.StopLimit];
                        if (type != null)
                        {
                            type = base.Connection.OrderTypes[OrderTypeId.Limit];
                        }
                        if ((this.fillPrice != 0.0) && (type != null))
                        {
                            this.state = 0;
                            this.actionType = ActionTypeId.Buy;
                            this.updateCancelOrder = true;
                            this.testError = false;
                            this.orderType = type.Id;
                            this.quantity = (this.symbol.SymbolType.Id == SymbolTypeId.Stock) ? 1 : 1;
                            this.limitPrice = this.fillPrice * 0.95;
                            this.stopPrice = this.fillPrice * 1.1;
                            if (iTrading.Core.Kernel.Globals.TraceSwitch.Test)
                            {
                                Trace.WriteLine("(" + base.Connection.IdPlus + ") Test.Order.DoTest.5: " + this.symbol.FullName);
                            }
                            this.ProcessState(null);
                            while (this.state < 20)
                            {
                                 iTrading.Test.Globals.Sleep( iTrading.Test.Globals.MilliSeconds2Sleep);
                            }
                        }
                        if ((this.fillPrice != 0.0) && (type != null))
                        {
                            this.state = 0;
                            this.actionType = ActionTypeId.Buy;
                            this.updateCancelOrder = false;
                            this.testError = true;
                            this.orderType = type.Id;
                            this.quantity = this.maxQuantity;
                            this.limitPrice = this.fillPrice;
                            this.stopPrice = this.fillPrice;
                            if (iTrading.Core.Kernel.Globals.TraceSwitch.Test)
                            {
                                Trace.WriteLine("(" + base.Connection.IdPlus + ") Test.Order.DoTest.6: " + this.symbol.FullName);
                            }
                            this.ProcessState(null);
                            while (this.state < 30)
                            {
                                 iTrading.Test.Globals.Sleep( iTrading.Test.Globals.MilliSeconds2Sleep);
                            }
                        }
                        if (((base.Connection.Options.Provider.Id != ProviderTypeId.TrackData) && (this.fillPrice != 0.0)) && ((base.Connection.OrderTypes[OrderTypeId.Limit] != null) && (base.Connection.OrderTypes[OrderTypeId.Stop] != null)))
                        {
                            this.state = 30;
                            this.updateCancelOrder = false;
                            this.testError = false;
                            this.quantity = (this.symbol.SymbolType.Id == SymbolTypeId.Stock) ? 1 : 1;
                            this.limitPrice = this.fillPrice * 1.1;
                            this.stopPrice = this.fillPrice * 0.95;
                            if (iTrading.Core.Kernel.Globals.TraceSwitch.Test)
                            {
                                Trace.WriteLine("(" + base.Connection.IdPlus + ") Test.Order.DoTest.7: " + this.symbol.FullName);
                            }
                            this.ProcessState(null);
                            while (this.state < 40)
                            {
                                 iTrading.Test.Globals.Sleep( iTrading.Test.Globals.MilliSeconds2Sleep);
                            }
                        }
                    }
                }
                if ((base.Connection.Options.Provider.Id == ProviderTypeId.MBTrading) && this.account.IsSimulation)
                {
                    foreach (SymbolTemplate template4 in base.SymbolTemplates)
                    {
                        if (template4.Valid)
                        {
                            this.symbol = base.Connection.GetSymbol(template4.Name, template4.Expiry, base.Connection.SymbolTypes[template4.SymbolTypeId], base.Connection.Exchanges[template4.ExchangeId], 0.0, RightId.Unknown, LookupPolicyId.RepositoryAndProvider);
                            this.symbol.MarketData.MarketDataItem -= new MarketDataItemEventHandler(this.MarketData_MarketDataItem);
                        }
                    }
                }
                this.account.AccountUpdate -= new AccountUpdateEventHandler(this.AccountUpdate);
                this.account.Execution -= new ExecutionUpdateEventHandler(this.Executions);
                this.account.OrderStatus -= new OrderStatusEventHandler(this.OrderStatus);
                this.account.PositionUpdate -= new PositionUpdateEventHandler(this.Positions_PositionUpdate);
            }
        }

        private void Executions(object sender, ExecutionUpdateEventArgs e)
        {
            if (iTrading.Core.Kernel.Globals.TraceSwitch.Order)
            {
                Trace.WriteLine("(" + base.Connection.IdPlus + ") Test.Order.Executions: " + e.Execution.OrderId + " " + ((this.order == null) ? "null" : this.order.OrderId.ToString()));
            }
            if ((this.order != null) && (this.order.OrderId == e.Execution.OrderId))
            {
                 iTrading.Test.Globals.Assert(base.GetType().FullName + " 400 " + e.Error, e.Error == ErrorCode.NoError);
                 iTrading.Test.Globals.Assert(base.GetType().FullName + " 401 " + e.Error, e.NativeError.Length == 0);
                 iTrading.Test.Globals.Assert(base.GetType().FullName + " 402 " + e.Execution.Account.Name, e.Execution.Account.Name == this.order.Account.Name);
            }
             iTrading.Test.Globals.Assert(base.GetType().FullName + " 403", e.Execution.Id.Length > 0);
             iTrading.Test.Globals.Assert(base.GetType().FullName + " 404", e.Execution.MarketPosition != null);
             iTrading.Test.Globals.Assert(base.GetType().FullName + " 405", e.Execution.Quantity != 0);
             iTrading.Test.Globals.Assert(base.GetType().FullName + " 406", e.Execution.Symbol != null);
        }

        private void MarketData_MarketDataItem(object sender, MarketDataEventArgs e)
        {
        }

        private void OrderStatus(object sender, OrderStatusEventArgs e)
        {
            if (e.OrderState.Id == OrderStateId.Initialized)
            {
                 iTrading.Test.Globals.Assert(base.GetType().FullName + " 300 " + e.Error, e.Error == ErrorCode.NoError);
                 iTrading.Test.Globals.Assert(base.GetType().FullName + " 301 " + e.Error, e.NativeError.Length == 0);
            }
            else
            {
                if ((((this.order != null) && (this.order.Id == e.Order.Id)) || ((this.order1 != null) && (this.order1.Id == e.Order.Id))) || ((this.order2 != null) && (this.order2.Id == e.Order.Id)))
                {
                     iTrading.Test.Globals.Assert(base.GetType().FullName + " 100 " + e.Error, e.Error == this.expectedError);
                     iTrading.Test.Globals.Assert(base.GetType().FullName + " 101 " + e.Error, (e.Error == ErrorCode.NoError) ? (e.NativeError.Length == 0) : (e.NativeError.Length > 0));
                    this.ProcessState(e);
                }
                 iTrading.Test.Globals.Assert(base.GetType().FullName + " 109", e.OrderState != null);
            }
        }

        private void Positions_PositionUpdate(object sender, PositionUpdateEventArgs e)
        {
             iTrading.Test.Globals.Assert(base.GetType().FullName + " 500 " + e.Error, e.Error == ErrorCode.NoError);
             iTrading.Test.Globals.Assert(base.GetType().FullName + " 501 " + e.Error, e.NativeError.Length == 0);
             iTrading.Test.Globals.Assert(base.GetType().FullName + " 502", e.Account != null);
             iTrading.Test.Globals.Assert(base.GetType().FullName + " 503", e.Currency != null);
             iTrading.Test.Globals.Assert(base.GetType().FullName + " 504", e.MarketPosition != null);
             iTrading.Test.Globals.Assert(base.GetType().FullName + " 505", e.Position != null);
             iTrading.Test.Globals.Assert(base.GetType().FullName + " 506", e.Symbol != null);
        }

        private void ProcessState(OrderStatusEventArgs e)
        {
            if (iTrading.Core.Kernel.Globals.TraceSwitch.Order)
            {
                Trace.WriteLine(string.Concat(new object[] { "(", base.Connection.IdPlus, ") Test.Order.ProcessState: ", this.state, (e == null) ? "" : string.Concat(new object[] { ", order ", e.Order.OrderId, " ", e.OrderState.Id }) }));
            }
            switch (this.state)
            {
                case 0:
                {
                    this.WaitForTicket();
                    SimulationSymbolOptions simulationSymbolOptions = new SimulationSymbolOptions();
                    simulationSymbolOptions.Margin = 1.0;
                    this.state = 1;
                    this.expectedError = ErrorCode.NoError;
                    this.order = this.account.CreateOrder(this.symbol, this.actionType, this.orderType, TimeInForceId.Day, this.quantity, this.limitPrice, this.stopPrice, "", null);
                    this.SetCustomText();
                    if (!this.Account.IsSimulation)
                    {
                        this.order.Submit();
                        return;
                    }
                    this.order.Submit(simulationSymbolOptions);
                    return;
                }
                case 1:
                    this.state = 2;
                     iTrading.Test.Globals.Assert(string.Concat(new object[] { base.GetType().FullName, " 000 ", e.Order.OrderId, " ", e.OrderState.Id }), e.OrderState.Id == OrderStateId.PendingSubmit);
                    return;

                case 2:
                    this.state = 3;
                     iTrading.Test.Globals.Assert(string.Concat(new object[] { base.GetType().FullName, " 001 ", e.Order.OrderId, " ", e.OrderState.Id }), e.OrderState.Id == OrderStateId.Accepted);
                    if (this.testError)
                    {
                        this.expectedError = ErrorCode.OrderRejected;
                        this.state = 0x15;
                    }
                    return;

                case 3:
                    this.state = 4;
                     iTrading.Test.Globals.Assert(string.Concat(new object[] { base.GetType().FullName, " 002 ", e.Order.OrderId, " ", e.OrderState.Id }), e.OrderState.Id == OrderStateId.Working);
                    if (this.updateCancelOrder)
                    {
                        this.state = 12;
                        if (base.Connection.FeatureTypes[FeatureTypeId.OrderChange] == null)
                        {
                            this.state = 15;
                            this.order.Cancel();
                            return;
                        }
                        this.limitPrice = this.order.LimitPrice * 0.98;
                        this.stopPrice = this.order.StopPrice * 1.03;
                        this.order.Change();
                    }
                    return;

                case 4:
                    if (e.OrderState.Id != OrderStateId.Filled)
                    {
                        if (e.OrderState.Id == OrderStateId.Cancelled)
                        {
                            this.state = 10;
                            return;
                        }
                        if (e.OrderState.Id != OrderStateId.PartFilled)
                        {
                             iTrading.Test.Globals.Assert(string.Concat(new object[] { base.GetType().FullName, " 004 ", e.Order.OrderId, " ", e.OrderState.Id }), false);
                        }
                        return;
                    }
                    while (!this.CheckExecutions(this.order))
                    {
                         iTrading.Test.Globals.Sleep( iTrading.Test.Globals.MilliSeconds2Sleep);
                    }
                     iTrading.Test.Globals.Assert(base.GetType().FullName + " 003 " + e.Order.OrderId, e.AvgFillPrice != 0.0);
                    if (this.orderType == OrderTypeId.Market)
                    {
                        this.fillPrice = e.AvgFillPrice;
                    }
                    this.state = 10;
                    return;

                case 12:
                    this.state = 13;
                     iTrading.Test.Globals.Assert(string.Concat(new object[] { base.GetType().FullName, " 005 ", e.Order.OrderId, " ", e.OrderState.Id }), e.OrderState.Id == OrderStateId.PendingChange);
                    return;

                case 13:
                    this.state = 14;
                     iTrading.Test.Globals.Assert(string.Concat(new object[] { base.GetType().FullName, " 006 ", e.Order.OrderId, " ", e.OrderState.Id }), e.OrderState.Id == OrderStateId.Accepted);
                    return;

                case 14:
                    this.state = 15;
                     iTrading.Test.Globals.Assert(string.Concat(new object[] { base.GetType().FullName, " 007 ", e.Order.OrderId, " ", e.OrderState.Id }), e.OrderState.Id == OrderStateId.Working);
                    this.order.Cancel();
                    return;

                case 15:
                    this.state = 0x10;
                     iTrading.Test.Globals.Assert(string.Concat(new object[] { base.GetType().FullName, " 008 ", e.Order.OrderId, " ", e.OrderState.Id }), e.OrderState.Id == OrderStateId.PendingCancel);
                    return;

                case 0x10:
                    this.state = 20;
                     iTrading.Test.Globals.Assert(string.Concat(new object[] { base.GetType().FullName, " 009 ", e.Order.OrderId, " ", e.OrderState.Id }), e.OrderState.Id == OrderStateId.Cancelled);
                    if (this.symbol.SymbolType.Id != SymbolTypeId.Stock)
                    {
                        futuresToOrder -= this.quantity;
                    }
                    return;

                case 0x15:
                    this.state = 30;
                     iTrading.Test.Globals.Assert(string.Concat(new object[] { base.GetType().FullName, " 010 ", e.Order.OrderId, " ", e.OrderState.Id }), e.OrderState.Id == OrderStateId.Rejected);
                    if (this.symbol.SymbolType.Id != SymbolTypeId.Stock)
                    {
                        futuresToOrder -= this.quantity;
                    }
                    this.expectedError = ErrorCode.NoError;
                    return;

                case 30:
                {
                    this.WaitForTicket();
                    this.expectedError = ErrorCode.NoError;
                    string ocaGroup = "TM_" + Guid.NewGuid().ToString();
                    this.state = 0x1f;
                    this.order = this.account.CreateOrder(this.symbol, base.Connection.ActionTypes[ActionTypeId.Sell].Id, base.Connection.OrderTypes[OrderTypeId.Market].Id, TimeInForceId.Day, this.quantity, 0.0, 0.0, ocaGroup, null);
                    this.order1 = this.account.CreateOrder(this.symbol, base.Connection.ActionTypes[ActionTypeId.Sell].Id, base.Connection.OrderTypes[OrderTypeId.Stop].Id, TimeInForceId.Day, this.quantity, 0.0, this.stopPrice, ocaGroup, null);
                    this.order1.Submit();
                    this.order2 = this.account.CreateOrder(this.symbol, base.Connection.ActionTypes[ActionTypeId.Sell].Id, base.Connection.OrderTypes[OrderTypeId.Limit].Id, TimeInForceId.Day, this.quantity, this.limitPrice, 0.0, ocaGroup, null);
                    this.order2.Submit();
                    this.order.Submit();
                    return;
                }
                case 0x1f:
                    if ((((e.Order == this.order) && (e.OrderState.Id == OrderStateId.Filled)) || ((e.Order == this.order1) && (e.OrderState.Id == OrderStateId.Cancelled))) || ((e.Order == this.order2) && (e.OrderState.Id == OrderStateId.Cancelled)))
                    {
                        this.state = 0x20;
                    }
                    return;

                case 0x20:
                    if ((((e.Order == this.order) && (e.OrderState.Id == OrderStateId.Filled)) || ((e.Order == this.order1) && (e.OrderState.Id == OrderStateId.Cancelled))) || ((e.Order == this.order2) && (e.OrderState.Id == OrderStateId.Cancelled)))
                    {
                        this.state = 0x21;
                    }
                    return;

                case 0x21:
                    if ((((e.Order != this.order) || (e.OrderState.Id != OrderStateId.Filled)) && ((e.Order != this.order1) || (e.OrderState.Id != OrderStateId.Cancelled))) && ((e.Order != this.order2) || (e.OrderState.Id != OrderStateId.Cancelled)))
                    {
                        this.state = this.state;
                        return;
                    }
                    this.state = 0x23;
                    this.order = this.account.CreateOrder(this.symbol, base.Connection.ActionTypes[ActionTypeId.Buy].Id, base.Connection.OrderTypes[OrderTypeId.Market].Id, TimeInForceId.Day, this.quantity, 0.0, 0.0, "", null);
                    this.order.Submit();
                    return;

                case 0x23:
                    this.state++;
                    return;

                case 0x24:
                    this.state++;
                    return;

                case 0x25:
                    this.state++;
                    return;

                case 0x26:
                    if (this.symbol.SymbolType.Id != SymbolTypeId.Stock)
                    {
                        futuresToOrder -= this.quantity;
                    }
                    this.state = 40;
                    return;
            }
             iTrading.Test.Globals.Assert(base.GetType().FullName + ": unexpected test driver state " + this.state, false);
        }

        private void SetCustomText()
        {
            if (this.order != null)
            {
                XmlDocument document = new XmlDocument();
                document.AppendChild(document.CreateElement("TradeMagic"));
                if (base.Connection.Options.Provider.Id == ProviderTypeId.InteractiveBrokers)
                {
                    string str = base.Connection.Options.Provider.Id.ToString();
                    document["TradeMagic"].AppendChild(document.CreateElement(str));
                    document["TradeMagic"][str].AppendChild(document.CreateElement("AccountGroup"));
                    document["TradeMagic"][str]["AccountGroup"].InnerText = "cesar";
                    document["TradeMagic"][str].AppendChild(document.CreateElement("DefaultMethod"));
                    document["TradeMagic"][str]["DefaultMethod"].InnerText = "EqualQuantity";
                }
                string name = "MyProductName";
                document["TradeMagic"].AppendChild(document.CreateElement(name));
                string str3 = "subkey1";
                document["TradeMagic"][name].AppendChild(document.CreateElement(str3));
                document["TradeMagic"][name][str3].InnerText = "any sub value 1";
                string str4 = "subkey2";
                document["TradeMagic"][name].AppendChild(document.CreateElement(str4));
                document["TradeMagic"][name][str4].InnerText = "any sub value 2";
                StringWriter writer = new StringWriter();
                document.Save(writer);
                this.order.CustomText = writer.ToString();
                iTrading.Core.Kernel.Globals.DB.Update(this.order, true);
            }
        }

        private bool WaitForTicket()
        {
            bool flag = false;
            if (this.symbol.SymbolType.Id != SymbolTypeId.Stock)
            {
                if ((this.actionType == ActionTypeId.Buy) || (this.actionType == ActionTypeId.SellShort))
                {
                    while (!flag)
                    {
                        lock (typeof( iTrading.Test.Order))
                        {
                            if (futuresToOrder < 3)
                            {
                                futuresToOrder += this.quantity;
                                return true;
                            }
                        }
                         iTrading.Test.Globals.Sleep( iTrading.Test.Globals.MilliSeconds2Sleep);
                    }
                }
                else
                {
                    futuresToOrder -= this.quantity;
                }
            }
            return false;
        }

        /// <summary>
        /// Test account.
        /// </summary>
        protected Account Account
        {
            get
            {
                return this.account;
            }
            set
            {
                this.account = value;
            }
        }
    }
}

