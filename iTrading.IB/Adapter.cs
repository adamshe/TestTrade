using iTrading.Core.Kernel;

namespace iTrading.IB
{
    using Microsoft.Win32;
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Timers;
    using System.Windows.Forms;
    using System.Xml;
    using iTrading.Core.Kernel;

    internal class Adapter : IAdapter, IMarketData, IMarketDepth, IOrder, IOrderChange
    {
        internal ArrayList accountUpdateRequests = new ArrayList();
        internal System.Timers.Timer accountUpdateTimer = null;
        internal StringCollection approvedPermIds = null;
        private AutoOpenOrdersRequest autoOpenOrdersRequest = null;
        private const int clientVersion = 0x10;
        private bool closedByClient = false;
        internal Connection connection = null;
        internal System.Timers.Timer connectTimer = null;
        internal const string defaultAccountName = "default";
        private DateTime dteLastSend = Globals.MaxDate;
        private ExecutionsRequest executionsRequest = null;
        internal string faCustom = "";
        private static Thread handleRuntimePopups = null;
        internal IBSocket ibSocket = null;
        private bool inSymbolLookup = false;
        internal bool isFaAccount = false;
        private ManagedAcctsRequest managedAcctsRequest = null;
        private Thread messageThread = null;
        internal int nextOrderId = -1;
        private int nextTickerId = -1;
        private OpenOrdersRequest openOrdersRequest = null;
        internal Hashtable orders = new Hashtable();
        internal ArrayList postponedOrderStatus = new ArrayList();
        private static int runningConnections = 0;
        private object[] syncOrderStatusEvent = new object[0];
        private object[] syncPopupHandlerThread = new object[0];
        private Hashtable tickerId2Request = new Hashtable();
        private Process twsProcess = null;
        private string twsTempDir = "";

        internal Adapter(Connection connection)
        {
            this.connection = connection;
            this.ibSocket = new IBSocket(this);
        }

        internal void AccountUpdateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (this.accountUpdateRequests)
            {
                AccountUpdatesRequest request = (AccountUpdatesRequest) this.accountUpdateRequests[0];
                if ((++request.elapsed > this.Options.AccountsUpdateSeconds) && (this.accountUpdateRequests.Count != 1))
                {
                    this.accountUpdateRequests.Remove(request);
                    request.subscribe = false;
                    request.Send(this);
                    request = (AccountUpdatesRequest) this.accountUpdateRequests[0];
                    request.elapsed = 0;
                    request.subscribe = true;
                    request.Send(this);
                }
            }
        }

        public void Cancel(iTrading.Core.Kernel.Order order)
        {
            lock (this.syncOrderStatusEvent)
            {
                bool flag = false;
                this.Delay();
                lock (this.ibSocket)
                {
                    flag = ((OrderRequest) order.AdapterLink).Cancel();
                }
                if (flag)
                {
                    this.connection.ProcessEventArgs(new OrderStatusEventArgs(order, ErrorCode.NoError, "", order.OrderId, order.LimitPrice, order.StopPrice, order.Quantity, order.AvgFillPrice, order.Filled, this.connection.OrderStates[OrderStateId.PendingCancel], this.connection.Now));
                }
            }
        }

        public void Change(iTrading.Core.Kernel.Order order)
        {
            lock (this.syncOrderStatusEvent)
            {
                this.Delay();
                if (this.Options.CheckForMarketData && (((order.Symbol.MarketData.Last == null) || (order.Symbol.MarketData.Ask == null)) || (order.Symbol.MarketData.Bid == null)))
                {
                    this.connection.ProcessEventArgs(new OrderStatusEventArgs(order, ErrorCode.UnableToChangeOrder, "Unable to submit order now: no market data available", order.OrderId, order.LimitPrice, order.StopPrice, order.Quantity, order.AvgFillPrice, order.Filled, order.OrderState, this.connection.Now));
                }
                else
                {
                    OrderRequest adapterLink = (OrderRequest) order.AdapterLink;
                    adapterLink.order = new iTrading.IB.Order(this, order.Account.Name, order.Quantity, order.Action.MapId, order.OrderType.MapId, order.TimeInForce.MapId, order.LimitPrice, order.StopPrice, order.OcaGroup, "");
                    lock (this.ibSocket)
                    {
                        ((OrderRequest) order.AdapterLink).Send(this);
                    }
                    this.connection.ProcessEventArgs(new OrderStatusEventArgs(order, ErrorCode.NoError, "", order.OrderId, order.LimitPrice, order.StopPrice, order.Quantity, order.AvgFillPrice, order.Filled, this.connection.OrderStates[OrderStateId.PendingChange], this.connection.Now));
                }
            }
        }

        private void Cleanup()
        {
            lock (this.syncPopupHandlerThread)
            {
                if ((--runningConnections == 0) && (handleRuntimePopups != null))
                {
                    handleRuntimePopups.Abort();
                    handleRuntimePopups = null;
                }
            }
            lock (this.ibSocket)
            {
                try
                {
                    this.ibSocket.Close();
                }
                catch (Exception exception)
                {
                    this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.Panic, "", "IB.Globals.Cleanup: " + exception.Message));
                }
                if (this.twsProcess != null)
                {
                    this.TerminateTws();
                    this.twsProcess = null;
                }
            }
        }

        public void Clear()
        {
        }

        public void Connect()
        {
            ThreadStart start = new ThreadStart(this.MessageLoop);
            this.messageThread = new Thread(start);
            this.messageThread.Name = "TM IB" + ((IBOptions) this.connection.Options).ClientId;
            this.messageThread.Start();
        }

        internal void ConnectTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.connectTimer.Stop();
            this.connectTimer = null;
            if (this.approvedPermIds == null)
            {
                string customText = "";
                if (this.faCustom.Length > 0)
                {
                    customText = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<TradeMagic>\n";
                    object obj2 = customText;
                    customText = string.Concat(new object[] { obj2, "<", this.Options.Provider.Id, ">", this.faCustom, "</", this.Options.Provider.Id, ">\n</TradeMagic>\n" });
                }
                if (this.connection.Accounts.Count == 0)
                {
                    this.connection.ProcessEventArgs(new AccountEventArgs(this.connection, ErrorCode.NoError, "", "default", null));
                }
                this.connection.SynchronizeInvoke.AsyncInvoke(new ProcessDelegate(this.ConnectTimer_ElapsedNow), new object[] { new ConnectionStatusEventArgs(this.connection, ErrorCode.NoError, "", ConnectionStatusId.Connected, ConnectionStatusId.Connected, 0, customText) });
            }
            else
            {
                ArrayList list = new ArrayList();
                lock (this.approvedPermIds)
                {
                    lock (this.connection.Accounts)
                    {
                    Label_0276:
                        foreach (Account account in this.connection.Accounts)
                        {
                            lock (account.Orders)
                            {
                                foreach (iTrading.Core .Kernel .Order order in account.Orders)
                                {
                                    if ((((order.OrderState.Id != OrderStateId.Cancelled) && (order.OrderState.Id != OrderStateId.Filled)) && ((order.OrderState.Id != OrderStateId.Rejected) && (order.OrderState.Id != OrderStateId.Unknown))) && !this.approvedPermIds.Contains(order.OrderId))
                                    {
                                        list.Add(new OrderStatusEventArgs(order, ErrorCode.NoError, "", order.OrderId, order.LimitPrice, order.StopPrice, order.Quantity, order.AvgFillPrice, order.Filled, this.connection.OrderStates[OrderStateId.Unknown], order.Time));
                                    }
                                }
                                goto Label_0276;
                            }
                        }
                    }
                }
                this.approvedPermIds = null;
                foreach (OrderStatusEventArgs args in list)
                {
                    this.connection.SynchronizeInvoke.AsyncInvoke(new ProcessDelegate(this.ConnectTimer_ElapsedNow), new object[] { args });
                }
            }
        }

        private void ConnectTimer_ElapsedNow(ITradingBaseEventArgs e)
        {
            this.connection.ProcessEventArgs(e);
        }

        internal Contract Convert(Symbol symbol)
        {
            Contract contract = new Contract(symbol.GetProviderName(ProviderTypeId.InteractiveBrokers));
            contract.Exchange = symbol.Exchange.MapId;
            contract.Expiry = symbol.Expiry;
            contract.SecType = symbol.SymbolType.MapId;
            contract.Strike = symbol.StrikePrice;
            switch (symbol.Right.Id)
            {
                case RightId.Call:
                    contract.Right = iTrading.IB.Right.CALL;
                    return contract;

                case RightId.Put:
                    contract.Right = iTrading.IB.Right.PUT;
                    return contract;
            }
            return contract;
        }

        internal iTrading.Core.Kernel.Operation Convert(iTrading.IB.Operation operation)
        {
            switch (operation)
            {
                case iTrading.IB.Operation.Insert:
                    return iTrading.Core.Kernel.Operation.Insert;

                case iTrading.IB.Operation.Update:
                    return iTrading.Core.Kernel.Operation.Update;

                case iTrading.IB.Operation.Delete:
                    return iTrading.Core.Kernel.Operation.Delete;
            }
            this.connection.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this.connection, ErrorCode.Panic, "", "IB.Adapter.Convert: can't convert internal IB operation type " + ((int)operation)));
            return iTrading.Core.Kernel.Operation.Insert;
        }

        internal Symbol Convert(Contract contract, Currency currency, double tickSize, double pointValue, ExchangeDictionary exchanges)
        {
            if (contract.Exchange.Length == 0)
            {
                contract.Exchange = "SMART";
            }
            Exchange exchange = this.connection.Exchanges.FindByMapId(contract.Exchange);
            if (exchange == null)
            {
                exchange = this.connection.Exchanges[ExchangeId.Default];
                this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.Panic, "", "IB.Adapter.Convert: can't convert iTrading.IB exchange '" + contract.Exchange + "'"));
            }
            SymbolType symbolType = this.connection.SymbolTypes.FindByMapId(contract.SecType);
            if (symbolType == null)
            {
                symbolType = this.connection.SymbolTypes[SymbolTypeId.Unknown];
                this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.Panic, "", "IB.Adapter.Convert: can't convert iTrading.IB symbol type '" + contract.SecType + "'"));
            }
            if (currency == null)
            {
                currency = this.connection.Currencies[CurrencyId.Unknown];
            }
            Trace.Assert(currency != null, "IB.Adapter.Convert: currency 'unknown' not registered");
            RightId unknown = RightId.Unknown;
            if ((contract.Right == iTrading.IB.Right.C) || (contract.Right == iTrading.IB.Right.CALL))
            {
                unknown = RightId.Call;
            }
            else if ((contract.Right == iTrading.IB.Right.P) || (contract.Right == iTrading.IB.Right.PUT))
            {
                unknown = RightId.Put;
            }
            else if (contract.Right == iTrading.IB.Right.ANY)
            {
                unknown = RightId.Unknown;
            }
            else
            {
                this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.Panic, "", "IB.Adapter.Convert: can't convert iTrading.IB right type '" + contract.Right + "'"));
            }
            Symbol symbol = this.connection.GetSymbolByProviderName(contract.Symbol, contract.Expiry, symbolType, exchange, contract.Strike, unknown, LookupPolicyId.RepositoryOnly);
            if (symbol != null)
            {
                return symbol;
            }
            return this.connection.CreateSymbol(contract.Symbol, contract.Expiry, symbolType, exchange, contract.Strike, unknown, currency, tickSize, pointValue, "", null, 0, exchanges, null, null);
        }

        private void Delay()
        {
            int milliseconds = DateTime.Now.Subtract(this.dteLastSend).Milliseconds;
            if ((this.dteLastSend != Globals.MaxDate) && (milliseconds < this.Options.WaitMilliSecondsRequest))
            {
                Thread.Sleep((int) (this.Options.WaitMilliSecondsRequest - milliseconds));
            }
            this.dteLastSend = DateTime.Now;
        }

        public void Disconnect()
        {
            if (this.connection.Options.Mode.Id == ModeTypeId.Simulation)
            {
                new Thread(new ThreadStart(this.DisconnectNow)).Start();
            }
            else
            {
                if (this.accountUpdateTimer != null)
                {
                    this.accountUpdateTimer.Stop();
                    this.accountUpdateTimer = null;
                }
                lock (this.accountUpdateRequests)
                {
                    foreach (AccountUpdatesRequest request in this.accountUpdateRequests)
                    {
                        request.subscribe = false;
                        request.Send(this);
                    }
                    this.accountUpdateRequests.Clear();
                }
                this.closedByClient = true;
                this.Cleanup();
                new Thread(new ThreadStart(this.DisconnectNow)).Start();
            }
        }

        private void DisconnectNow()
        {
            this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, ErrorCode.NoError, "", ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
        }

        internal Account GetAccount(string accountName)
        {
            if (Globals.TraceSwitch.Order)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") IB.Adapter.GetAccount: name='" + accountName + "'");
            }
            Account account = this.connection.Accounts.FindByName((this.connection.Accounts.Count == 1) ? this.connection.Accounts[0].Name : accountName);
            if (account != null)
            {
                return account;
            }
            if (this.connection.Currencies.Count != 0)
            {
                this.connection.ProcessEventArgs(new AccountEventArgs(this.connection, ErrorCode.NoError, "", accountName, null));
                account = this.connection.Accounts.FindByName(accountName);
                if (account != null)
                {
                    return account;
                }
                this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.InvalidNativeAccount, "", "IB.Adapter.GetAccount: Account '" + accountName + "'"));
            }
            return null;
        }

        private void HandleMessage(MessageId messageId)
        {
            IBErrorCode code;
            string str2;
            if (Globals.TraceSwitch.Native)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") IB.Adapter.HandleMessage: messageId " + messageId.ToString("g"));
            }
            int version = 0;
            string str = this.ibSocket.ReadString();
            try
            {
                version = System.Convert.ToInt32(str, this.ibSocket.numberFormatInfo);
            }
            catch (Exception)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") IB.Adapter.HandleMessage: unable to convert '" + str + "' to integer");
            }
            int num2 = 0;
            iTrading.Core .Kernel.Request request = null;
            switch (messageId)
            {
                case MessageId.TickPrice:
                case MessageId.TickSize:
                    num2 = this.ibSocket.ReadInteger();
                    request = (iTrading.Core .Kernel.Request) this.tickerId2Request[num2];
                    if (request != null)
                    {
                        ((MarketDataRequest) request.AdapterLink).Process(this, version, messageId);
                        return;
                    }
                    this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.Panic, "", string.Concat(new object[] { "invalid ticker id =", num2, " msg=", messageId })));
                    return;

                case MessageId.OrderStatus:
                    if (this.connectTimer != null)
                    {
                        this.StartConnectTimer();
                    }
                    lock (this.syncOrderStatusEvent)
                    {
                        OrderRequest.Process(this, version);
                        return;
                    }
                    goto Label_0910;

                case MessageId.ErrMsg:
                    if (version >= 2)
                    {
                        SymbolDictionary dictionary;
                        num2 = this.ibSocket.ReadInteger();
                        code = (IBErrorCode) this.ibSocket.ReadInteger();
                        str2 = this.ibSocket.ReadString();
                        switch (code)
                        {
                            case IBErrorCode.ConnectionLost:
                                if (Globals.TraceSwitch.Connect)
                                {
                                    Trace.WriteLine("(" + this.connection.IdPlus + ") IB.Adapter.HandleMessage ConnectionLost");
                                }
                                if (this.connection.ConnectionStatusId != ConnectionStatusId.Connecting)
                                {
                                    this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, ErrorCode.NoError, str2 + "(" + ((int) code).ToString() + ")", ConnectionStatusId.ConnectionLost, ConnectionStatusId.ConnectionLost, 0, ""));
                                }
                                return;

                            case IBErrorCode.ConnectionLostAndRestored:
                                if (Globals.TraceSwitch.Connect)
                                {
                                    Trace.WriteLine("(" + this.connection.IdPlus + ") IB.Adapter.HandleMessage ConnectionLostAndRestored");
                                }
                                if (this.connection.ConnectionStatusId != ConnectionStatusId.Connecting)
                                {
                                    Hashtable hashtable;
                                    Thread.Sleep(0x1388);
                                    foreach (MarketData data in this.connection.MarketDataStreams)
                                    {
                                        this.Delay();
                                        ((MarketDataRequest) data.AdapterLink).tickerId = ++this.nextTickerId;
                                        lock ((hashtable = this.tickerId2Request))
                                        {
                                            this.tickerId2Request.Add(this.nextTickerId, data.AdapterLink);
                                        }
                                        ((MarketDataRequest) data.AdapterLink).Send(this);
                                    }
                                    foreach (MarketDepth depth in this.connection.MarketDepthStreams)
                                    {
                                        this.Delay();
                                        ((MarketDepthRequest) depth.AdapterLink).tickerId = ++this.nextTickerId;
                                        lock ((hashtable = this.tickerId2Request))
                                        {
                                            this.tickerId2Request.Add(this.nextTickerId, depth.AdapterLink);
                                        }
                                        ((MarketDepthRequest) depth.AdapterLink).Send(this);
                                    }
                                    if (this.accountUpdateRequests.Count > 0)
                                    {
                                        this.Delay();
                                        AccountUpdatesRequest request2 = (AccountUpdatesRequest) this.accountUpdateRequests[0];
                                        request2.elapsed = 0;
                                        request2.subscribe = true;
                                        request2.Send(this);
                                    }
                                    this.approvedPermIds = new StringCollection();
                                    this.executionsRequest = null;
                                    this.openOrdersRequest = null;
                                    this.StartConnectTimer();
                                    this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, ErrorCode.NoError, "", ConnectionStatusId.Connected, ConnectionStatusId.Connected, 0, ""));
                                }
                                return;

                            case IBErrorCode.ConnectionLostAndMaintained:
                                if (Globals.TraceSwitch.Connect)
                                {
                                    Trace.WriteLine("(" + this.connection.IdPlus + ") IB.Adapter.HandleMessage ConnectionLostAndMaintained");
                                }
                                if (this.connection.ConnectionStatusId != ConnectionStatusId.Connecting)
                                {
                                    Thread.Sleep(0x1388);
                                    this.approvedPermIds = new StringCollection();
                                    this.executionsRequest = null;
                                    this.openOrdersRequest = null;
                                    this.StartConnectTimer();
                                    this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, ErrorCode.NoError, "", ConnectionStatusId.Connected, ConnectionStatusId.Connected, 0, ""));
                                }
                                return;

                            case IBErrorCode.ServerValidateError:
                                if (this.inSymbolLookup)
                                {
                                    this.connection.ProcessEventArgs(new SymbolEventArgs(this.connection, ErrorCode.NoSuchSymbol, ((int) code).ToString(), null));
                                    this.inSymbolLookup = false;
                                    return;
                                }
                                break;

                            case IBErrorCode.Rescubscribe2MarketDepth:
                                lock ((dictionary = this.connection.Symbols))
                                {
                                    foreach (Symbol symbol in this.connection.Symbols.Values)
                                    {
                                        if (symbol.MarketDepth.IsRunning)
                                        {
                                            symbol.MarketDepth.Cancel();
                                            symbol.MarketDepth.Start();
                                        }
                                    }
                                }
                                return;

                            case IBErrorCode.ResetMarketDepthRows:
                                lock ((dictionary = this.connection.Symbols))
                                {
                                    foreach (Symbol symbol2 in this.connection.Symbols.Values)
                                    {
                                        if (symbol2.MarketDepth.IsRunning)
                                        {
                                            while (symbol2.MarketDepth.Ask.Count > 0)
                                            {
                                                this.connection.ProcessEventArgs(new MarketDepthEventArgs(symbol2.MarketDepth, ErrorCode.NoError, "", symbol2, 0, "", iTrading.Core .Kernel .Operation.Delete, this.connection.MarketDataTypes[MarketDataTypeId.Ask], 0.0, 0, this.connection.Now));
                                            }
                                            while (symbol2.MarketDepth.Bid.Count > 0)
                                            {
                                                this.connection.ProcessEventArgs(new MarketDepthEventArgs(symbol2.MarketDepth, ErrorCode.NoError, "", symbol2, 0, "", iTrading.Core .Kernel .Operation.Delete, this.connection.MarketDataTypes[MarketDataTypeId.Bid], 0.0, 0, this.connection.Now));
                                            }
                                        }
                                    }
                                }
                                return;

                            case IBErrorCode.NoSuchSecurity:
                                this.connection.ProcessEventArgs(new SymbolEventArgs(this.connection, ErrorCode.NoSuchSymbol, ((int) code).ToString(), null));
                                return;
                        }
                        break;
                    }
                    this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.Panic, "", this.ibSocket.ReadString()));
                    return;

                case MessageId.OpenOrders:
                    if (this.connectTimer != null)
                    {
                        this.StartConnectTimer();
                    }
                    OrderRequest.ProcessOpenOrders(this, version);
                    return;

                case MessageId.AccountUpdate:
                    if (this.connectTimer != null)
                    {
                        this.StartConnectTimer();
                    }
                    AccountUpdatesRequest.ProcessUpdate(this, version);
                    return;

                case MessageId.PortfolioUpdate:
                    goto Label_0910;

                case MessageId.AccountUpdateTime:
                    if (this.connectTimer != null)
                    {
                        this.StartConnectTimer();
                    }
                    AccountUpdatesRequest.ProcessUpdateTime(this, version);
                    return;

                case MessageId.NextValidId:
                    num2 = this.ibSocket.ReadInteger();
                    this.nextOrderId = num2 + 1;
                    if (Globals.TraceSwitch.Order)
                    {
                        Trace.WriteLine(string.Concat(new object[] { "(", this.connection.IdPlus, ") IB.Adapter.HandleMessage: nextOrderId=", this.nextOrderId, " twsSend=", num2 }));
                    }
                    return;

                case MessageId.ContractData:
                    this.inSymbolLookup = false;
                    ContractDetailRequest.Process(this, version);
                    return;

                case MessageId.ExecutionData:
                    if (this.connectTimer != null)
                    {
                        this.StartConnectTimer();
                    }
                    ExecutionsRequest.Process(this, version);
                    return;

                case MessageId.MarketDepth:
                case MessageId.MarketDepth2:
                    num2 = this.ibSocket.ReadInteger();
                    request = (iTrading.Core .Kernel.Request) this.tickerId2Request[num2];
                    if (request != null)
                    {
                        ((MarketDepthRequest) request.AdapterLink).Process(this, version, messageId);
                        return;
                    }
                    this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.Panic, "", string.Concat(new object[] { "invalid ticker id =", num2, " msg=", messageId })));
                    return;

                case MessageId.NewBulletins:
                    NewsBulletinsRequest.Process(this);
                    return;

                case MessageId.ManagedAccts:
                    this.isFaAccount = true;
                    ManagedAcctsRequest.Process(this, version);
                    this.managedAcctsRequest = null;
                    return;

                case MessageId.ReceiveFa:
                    FaRequest.Process(this, version);
                    return;

                default:
                    this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.Panic, "", "unknown message id=" + messageId));
                    return;
            }
            this.connection.SynchronizeInvoke.AsyncInvoke(new ProcessErrorDelegate(this.ProcessErrorNow), new object[] { code, num2, str2 });
            return;
        Label_0910:
            if (this.connectTimer != null)
            {
                this.StartConnectTimer();
            }
            AccountUpdatesRequest.ProcessPortfolioUpdate(this, version);
        }

        private void HandlePopups()
        {
            try
            {
                HandleRuntimePopups popups = new HandleRuntimePopups();
                while (true)
                {
                    popups.Run();
                    Thread.Sleep(this.Options.PopupHandlerMilliSeconds);
                }
            }
            catch (Exception)
            {
            }
        }

        private void Init()
        {
            this.connection.ProcessEventArgs(new CurrencyEventArgs(this.connection, ErrorCode.NoError, "", CurrencyId.AustralianDollar, "AUD"));
            this.connection.ProcessEventArgs(new CurrencyEventArgs(this.connection, ErrorCode.NoError, "", CurrencyId.BritishPound, "GBP"));
            this.connection.ProcessEventArgs(new CurrencyEventArgs(this.connection, ErrorCode.NoError, "", CurrencyId.CanadianDollar, "CAD"));
            this.connection.ProcessEventArgs(new CurrencyEventArgs(this.connection, ErrorCode.NoError, "", CurrencyId.Euro, "EUR"));
            this.connection.ProcessEventArgs(new CurrencyEventArgs(this.connection, ErrorCode.NoError, "", CurrencyId.HongKongDollar, "HKD"));
            this.connection.ProcessEventArgs(new CurrencyEventArgs(this.connection, ErrorCode.NoError, "", CurrencyId.JapaneseYen, "JPY"));
            this.connection.ProcessEventArgs(new CurrencyEventArgs(this.connection, ErrorCode.NoError, "", CurrencyId.SwissFranc, "CHF"));
            this.connection.ProcessEventArgs(new CurrencyEventArgs(this.connection, ErrorCode.NoError, "", CurrencyId.Unknown, ""));
            this.connection.ProcessEventArgs(new CurrencyEventArgs(this.connection, ErrorCode.NoError, "", CurrencyId.UsDollar, "USD"));
            this.connection.ProcessEventArgs(new AccountItemTypeEventArgs(this.connection, ErrorCode.NoError, "", AccountItemTypeId.BuyingPower, "BuyingPower"));
            this.connection.ProcessEventArgs(new AccountItemTypeEventArgs(this.connection, ErrorCode.NoError, "", AccountItemTypeId.CashValue, "TotalCashValue"));
            this.connection.ProcessEventArgs(new AccountItemTypeEventArgs(this.connection, ErrorCode.NoError, "", AccountItemTypeId.ExcessEquity, "SMA"));
            this.connection.ProcessEventArgs(new AccountItemTypeEventArgs(this.connection, ErrorCode.NoError, "", AccountItemTypeId.InitialMargin, "InitMarginReq"));
            this.connection.ProcessEventArgs(new AccountItemTypeEventArgs(this.connection, ErrorCode.NoError, "", AccountItemTypeId.InitialMarginOvernight, "LookAheadInitMarginReq"));
            this.connection.ProcessEventArgs(new AccountItemTypeEventArgs(this.connection, ErrorCode.NoError, "", AccountItemTypeId.MaintenanceMargin, "MaintMarginReq"));
            this.connection.ProcessEventArgs(new AccountItemTypeEventArgs(this.connection, ErrorCode.NoError, "", AccountItemTypeId.MaintenanceMarginOvernight, "LookAheadMaintMarginReq"));
            this.connection.ProcessEventArgs(new AccountItemTypeEventArgs(this.connection, ErrorCode.NoError, "", AccountItemTypeId.NetLiquidation, "NetLiquidation"));
            this.connection.ProcessEventArgs(new AccountItemTypeEventArgs(this.connection, ErrorCode.NoError, "", AccountItemTypeId.NetLiquidationByCurrency, "NetLiquidationByCurrency"));
            this.connection.ProcessEventArgs(new AccountItemTypeEventArgs(this.connection, ErrorCode.NoError, "", AccountItemTypeId.RealizedProfitLoss, "RealizedPnL"));
            this.connection.ProcessEventArgs(new AccountItemTypeEventArgs(this.connection, ErrorCode.NoError, "", AccountItemTypeId.TotalCashBalance, "TotalCashBalance"));
            this.connection.ProcessEventArgs(new AccountItemTypeEventArgs(this.connection, ErrorCode.NoError, "", AccountItemTypeId.Unknown, "Unknown"));
            if (this.Options.Mode.Id == ModeTypeId.Simulation)
            {
                this.connection.CreateSimulationAccount(this.connection.SimulationAccountName, new SimulationAccountOptions());
            }
            this.connection.ProcessEventArgs(new ActionTypeEventArgs(this.connection, ErrorCode.NoError, "", ActionTypeId.Buy, "BUY"));
            this.connection.ProcessEventArgs(new ActionTypeEventArgs(this.connection, ErrorCode.NoError, "", ActionTypeId.BuyToCover, "BUY"));
            this.connection.ProcessEventArgs(new ActionTypeEventArgs(this.connection, ErrorCode.NoError, "", ActionTypeId.Sell, "SELL"));
            this.connection.ProcessEventArgs(new ActionTypeEventArgs(this.connection, ErrorCode.NoError, "", ActionTypeId.SellShort, "SELL"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Ace, "ACE"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Amex, "AMEX"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Arca, "ARCA"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Belfox, "BELFOX"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Box, "BOX"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Brut, "BRUT"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.BTrade, "BTRADE"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Caes, "CAES"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Cfe, "CFE"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Cboe, "CBOE"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Default, "SMART"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.ECbot, "ECBOT"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Eurex, "DTB"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.EurexSW, "SOFFEX"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Fta, "FTA"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Globex, "GLOBEX"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Hkfe, "HKFE"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.IBIdeal, "IDEAL"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.IBIdealPro, "IDEALPRO"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.IBTmbr, "TMBR"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Idem, "IDEM"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Ise, "ISE"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Island, "ISLAND"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Liffe, "LIFFE"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Lse, "LSE"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Me, "ME"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Meff, "MEFFRV"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Monep, "MONEP"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Nqlx, "NQLX"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Nymex, "NYMEX"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Nyse, "NYSE"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.One, "ONE"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Ose, "OSE.JPN"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Phlx, "PHLX"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Pse, "PSE"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Snfe, "SNFE"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Soes, "SUPERSOES"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Tse, "TSE.JPN"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Tsx, "TSE"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.TsxV, "VENTURE"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.VirtX, "VIRTX"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Xetra, "IBIS"));
            this.connection.ProcessEventArgs(new FeatureTypeEventArgs(this.connection, ErrorCode.NoError, "", FeatureTypeId.MarketData, 0.0));
            this.connection.ProcessEventArgs(new FeatureTypeEventArgs(this.connection, ErrorCode.NoError, "", FeatureTypeId.MarketDepth, 0.0));
            this.connection.ProcessEventArgs(new FeatureTypeEventArgs(this.connection, ErrorCode.NoError, "", FeatureTypeId.MaxMarketDataStreams, (double) this.Options.MaxMarketDataStreams));
            this.connection.ProcessEventArgs(new FeatureTypeEventArgs(this.connection, ErrorCode.NoError, "", FeatureTypeId.MaxMarketDepthStreams, 3.0));
            this.connection.ProcessEventArgs(new FeatureTypeEventArgs(this.connection, ErrorCode.NoError, "", FeatureTypeId.MultipleConnections, 0.0));
            this.connection.ProcessEventArgs(new FeatureTypeEventArgs(this.connection, ErrorCode.NoError, "", FeatureTypeId.News, 0.0));
            this.connection.ProcessEventArgs(new FeatureTypeEventArgs(this.connection, ErrorCode.NoError, "", FeatureTypeId.Order, 0.0));
            this.connection.ProcessEventArgs(new FeatureTypeEventArgs(this.connection, ErrorCode.NoError, "", FeatureTypeId.OrderChange, 0.0));
            this.connection.ProcessEventArgs(new FeatureTypeEventArgs(this.connection, ErrorCode.NoError, "", FeatureTypeId.SymbolLookup, 0.0));
            if (this.Options.UseNativeOcaOrders && (this.Options.Mode.Id != ModeTypeId.Simulation))
            {
                this.connection.ProcessEventArgs(new FeatureTypeEventArgs(this.connection, ErrorCode.NoError, "", FeatureTypeId.NativeOcaOrders, 0.0));
            }
            this.connection.ProcessEventArgs(new MarketDataTypeEventArgs(this.connection, ErrorCode.NoError, "", MarketDataTypeId.Ask, ""));
            this.connection.ProcessEventArgs(new MarketDataTypeEventArgs(this.connection, ErrorCode.NoError, "", MarketDataTypeId.Bid, ""));
            this.connection.ProcessEventArgs(new MarketDataTypeEventArgs(this.connection, ErrorCode.NoError, "", MarketDataTypeId.DailyHigh, ""));
            this.connection.ProcessEventArgs(new MarketDataTypeEventArgs(this.connection, ErrorCode.NoError, "", MarketDataTypeId.DailyLow, ""));
            this.connection.ProcessEventArgs(new MarketDataTypeEventArgs(this.connection, ErrorCode.NoError, "", MarketDataTypeId.DailyVolume, ""));
            this.connection.ProcessEventArgs(new MarketDataTypeEventArgs(this.connection, ErrorCode.NoError, "", MarketDataTypeId.Last, ""));
            this.connection.ProcessEventArgs(new MarketDataTypeEventArgs(this.connection, ErrorCode.NoError, "", MarketDataTypeId.LastClose, ""));
            this.connection.ProcessEventArgs(new MarketDataTypeEventArgs(this.connection, ErrorCode.NoError, "", MarketDataTypeId.Unknown, ""));
            this.connection.ProcessEventArgs(new MarketPositionEventArgs(this.connection, ErrorCode.NoError, "", MarketPositionId.Long, "BOT"));
            this.connection.ProcessEventArgs(new MarketPositionEventArgs(this.connection, ErrorCode.NoError, "", MarketPositionId.Short, "SLD"));
            this.connection.ProcessEventArgs(new MarketPositionEventArgs(this.connection, ErrorCode.NoError, "", MarketPositionId.Unknown, ""));
            this.connection.ProcessEventArgs(new NewsItemTypeEventArgs(this.connection, ErrorCode.NoError, "", NewsItemTypeId.Default, "1"));
            this.connection.ProcessEventArgs(new OrderStateEventArgs(this.connection, ErrorCode.NoError, "", OrderStateId.Cancelled, "Cancelled"));
            this.connection.ProcessEventArgs(new OrderStateEventArgs(this.connection, ErrorCode.NoError, "", OrderStateId.Filled, "Filled"));
            this.connection.ProcessEventArgs(new OrderStateEventArgs(this.connection, ErrorCode.NoError, "", OrderStateId.Initialized, ""));
            this.connection.ProcessEventArgs(new OrderStateEventArgs(this.connection, ErrorCode.NoError, "", OrderStateId.PartFilled, ""));
            this.connection.ProcessEventArgs(new OrderStateEventArgs(this.connection, ErrorCode.NoError, "", OrderStateId.PendingChange, "PendingChange"));
            this.connection.ProcessEventArgs(new OrderStateEventArgs(this.connection, ErrorCode.NoError, "", OrderStateId.PendingCancel, "PendingCancel"));
            this.connection.ProcessEventArgs(new OrderStateEventArgs(this.connection, ErrorCode.NoError, "", OrderStateId.PendingSubmit, "PendingSubmit"));
            this.connection.ProcessEventArgs(new OrderStateEventArgs(this.connection, ErrorCode.NoError, "", OrderStateId.Accepted, "PreSubmitted"));
            this.connection.ProcessEventArgs(new OrderStateEventArgs(this.connection, ErrorCode.NoError, "", OrderStateId.Rejected, "Inactive"));
            this.connection.ProcessEventArgs(new OrderStateEventArgs(this.connection, ErrorCode.NoError, "", OrderStateId.Unknown, ""));
            this.connection.ProcessEventArgs(new OrderStateEventArgs(this.connection, ErrorCode.NoError, "", OrderStateId.Working, "Submitted"));
            this.connection.ProcessEventArgs(new OrderTypeEventArgs(this.connection, ErrorCode.NoError, "", OrderTypeId.Market, "MKT"));
            this.connection.ProcessEventArgs(new OrderTypeEventArgs(this.connection, ErrorCode.NoError, "", OrderTypeId.Limit, "LMT"));
            this.connection.ProcessEventArgs(new OrderTypeEventArgs(this.connection, ErrorCode.NoError, "", OrderTypeId.Stop, "STP"));
            this.connection.ProcessEventArgs(new OrderTypeEventArgs(this.connection, ErrorCode.NoError, "", OrderTypeId.StopLimit, "STPLMT"));
            this.connection.ProcessEventArgs(new OrderTypeEventArgs(this.connection, ErrorCode.NoError, "", OrderTypeId.Unknown, ""));
            this.connection.ProcessEventArgs(new SymbolTypeEventArgs(this.connection, ErrorCode.NoError, "", SymbolTypeId.Unknown, ""));
            this.connection.ProcessEventArgs(new SymbolTypeEventArgs(this.connection, ErrorCode.NoError, "", SymbolTypeId.Future, "FUT"));
            this.connection.ProcessEventArgs(new SymbolTypeEventArgs(this.connection, ErrorCode.NoError, "", SymbolTypeId.Index, "IND"));
            this.connection.ProcessEventArgs(new SymbolTypeEventArgs(this.connection, ErrorCode.NoError, "", SymbolTypeId.Option, "OPT"));
            this.connection.ProcessEventArgs(new SymbolTypeEventArgs(this.connection, ErrorCode.NoError, "", SymbolTypeId.Stock, "STK"));
            this.connection.ProcessEventArgs(new TimeInForceEventArgs(this.connection, ErrorCode.NoError, "", TimeInForceId.Day, "DAY"));
            this.connection.ProcessEventArgs(new TimeInForceEventArgs(this.connection, ErrorCode.NoError, "", TimeInForceId.Gtc, "GTC"));
        }

        private static string MakeTempDir()
        {
            string str2;
            string environmentVariable = Environment.GetEnvironmentVariable("TEMP");
            Random random = new Random(DateTime.Now.Millisecond);
            do
            {
                str2 = environmentVariable + @"\" + random.Next();
            }
            while (Directory.Exists(str2) || File.Exists(str2));
            Directory.CreateDirectory(str2);
            return str2;
        }

        internal void MessageLoop()
        {
            string str;
            object[] objArray;
            if (this.connection.Options.Mode.Id == ModeTypeId.Simulation)
            {
                this.Init();
                this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, ErrorCode.NoError, "", ConnectionStatusId.Connected, ConnectionStatusId.Connected, 0, ""));
                return;
            }
            if (Globals.TraceSwitch.Connect)
            {
                Trace.WriteLine(string.Concat(new object[] { 
                    "(", this.connection.IdPlus, ") IB.Adapter.MessageLoop1:  Host='", this.Options.Host, "' Port=", this.Options.Port, " ClientId=", this.Options.ClientId, " Mode=", this.Options.Mode.Id, " UseSsl=", this.Options.UseSsl, " AccountsUpdateSeconds=", this.Options.AccountsUpdateSeconds, " CheckForMarketData=", this.Options.CheckForMarketData, 
                    " Connect2RunningTws=", this.Options.Connect2RunningTws, " DontHandleRuntimePopups=", this.Options.DontHandleRuntimePopups, " EnablePopupHandling=", this.Options.EnablePopupHandling, " LogLevel=", this.Options.LogLevel, " PopupHandlerMilliSeconds=", this.Options.PopupHandlerMilliSeconds, " SimOcaPartialFills=", this.Options.SimOcaPartialFills, " UseNativeOcaOrders=", this.Options.UseNativeOcaOrders, " UseUserSettings=", this.Options.UseUserSettings, 
                    " WaitMilliSecondsRequest=", this.Options.WaitMilliSecondsRequest, " WaitOnConnectMilliSeconds=", this.Options.WaitOnConnectMilliSeconds
                 }));
            }
            Monitor.Enter(typeof(Adapter));
            if (((this.Options.Host.Length == 0) || (this.Options.Host == this.Options.DefaultHost)) && !this.StartTws())
            {
                Monitor.Exit(typeof(Adapter));
                return;
            }
            if (Globals.TraceSwitch.Connect)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") IB.Adapter.MessageLoop2");
            }
            this.Init();
            if (Globals.TraceSwitch.Connect)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") IB.Adapter.MessageLoop3");
            }
            ConnectionStatusEventArgs eventArgs = null;
            for (int i = 0; !this.ibSocket.Connected && (i < this.Options.MaxTrysToLogin); i++)
            {
                try
                {
                    this.ibSocket.Connect();
                }
                catch (Exception exception)
                {
                    eventArgs = new ConnectionStatusEventArgs(this.connection, ErrorCode.Panic, "IB.Adapter.MessageLoop " + exception.Message, ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, "");
                    Application.DoEvents();
                    Thread.Sleep(0x3e8);
                }
            }
            if (Globals.TraceSwitch.Connect)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") IB.Adapter.MessageLoop4");
            }
            if (!this.ibSocket.Connected)
            {
                Monitor.Exit(typeof(Adapter));
                if (eventArgs != null)
                {
                    this.connection.ProcessEventArgs(eventArgs);
                }
                return;
            }
            if (Globals.TraceSwitch.Connect)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") IB.Adapter.MessageLoop5");
            }
            lock ((objArray = this.syncPopupHandlerThread))
            {
                if (((this.Options.Host.Length == 0) || (this.Options.Host == this.Options.DefaultHost)) && (this.Options.EnablePopupHandling && (handleRuntimePopups == null)))
                {
                    ThreadStart start = new ThreadStart(this.HandlePopups);
                    handleRuntimePopups = new Thread(start);
                    handleRuntimePopups.Name = "IB popup handler";
                    handleRuntimePopups.Start();
                }
                runningConnections++;
            }
            if (Globals.TraceSwitch.Connect)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") IB.Adapter.MessageLoop6");
            }
            this.Delay();
            this.ibSocket.Send(0x10);
            try
            {
                this.ibSocket.ServerVersion = this.ibSocket.ReadInteger();
            }
            catch (SocketClosedException exception2)
            {
                Monitor.Exit(typeof(Adapter));
                this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, ErrorCode.Panic, "TWS did not accept connection attempt (" + exception2.Message + ")", ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                return;
            }
            lock ((objArray = this.syncPopupHandlerThread))
            {
                if (this.Options.DontHandleRuntimePopups && (handleRuntimePopups != null))
                {
                    handleRuntimePopups.Abort();
                    handleRuntimePopups = null;
                }
            }
            if (this.ibSocket.ServerVersion >= 1)
            {
                if (this.ibSocket.ServerVersion >= 3)
                {
                    this.ibSocket.Send(this.Options.ClientId);
                }
                if (Globals.TraceSwitch.Connect)
                {
                    Trace.WriteLine(string.Concat(new object[] { "(", this.connection.IdPlus, ") IB.Adapter.MessageLoop7: serverversion=", this.ibSocket.ServerVersion }));
                }
                this.Delay();
                this.ibSocket.Send(14);
                this.ibSocket.Send(1);
                this.ibSocket.Send((int) this.Options.LogLevel);
                Monitor.Exit(typeof(Adapter));
                if (Globals.TraceSwitch.Connect)
                {
                    Trace.WriteLine("(" + this.connection.IdPlus + ") IB.Adapter.MessageLoop8");
                }
                this.ibSocket.Clear();
                this.managedAcctsRequest = new ManagedAcctsRequest(this);
                this.Delay();
                new NextValidIdRequest(this, this.Options.RequestIds).Send(this);
                this.Delay();
                this.managedAcctsRequest.Send(this);
                this.Delay();
                new NewsBulletinsRequest(this).Send(this);
                if (Globals.TraceSwitch.Connect)
                {
                    Trace.WriteLine("(" + this.connection.IdPlus + ") IB.Adapter.MessageLoop9");
                }
            }
            else
            {
                this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.Panic, "", "IB.Adapter.MessageLoop: Invalid server version " + this.ibSocket.ServerVersion));
                return;
            }
        Label_0750:
            str = "";
            try
            {
                str = this.ibSocket.ReadString();
                if (str.Length == 0)
                {
                    if (!this.ibSocket.Connected)
                    {
                        return;
                    }
                }
                else
                {
                    MessageId id;
                    try
                    {
                        id = (MessageId) System.Convert.ToInt32(str, this.ibSocket.numberFormatInfo);
                    }
                    catch (Exception)
                    {
                        this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.Panic, "", "IB.Adapter.MessageLoop: Unable to convert msg '" + str + "' to an integer id"));
                        goto Label_0750;
                    }
                    this.HandleMessage(id);
                }
                goto Label_0750;
            }
            catch (SocketClosedException exception3)
            {
                if (!this.closedByClient)
                {
                    this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, ErrorCode.ServerConnectionIsBroken, "TWS closed socket (" + exception3.Message + ")", ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                    this.Cleanup();
                }
                return;
            }
            catch (ThreadAbortException)
            {
                return;
            }
            catch (Exception exception4)
            {
                this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.Panic, "", "IB.Globals.MessageLoop msg='" + str + "': " + exception4.Message));
                goto Label_0750;
            }
        }

        private void ProcessErrorNow(IBErrorCode err, int id, string msg)
        {
            try
            {
                string[] strArray;
                int num7;
                OrderRequest request = (OrderRequest) this.orders[id];
                switch (err)
                {
                    case IBErrorCode.OrderRejected:
                        if (request != null)
                        {
                            goto Label_04DD;
                        }
                        this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.Panic, "", "IB.Adapter.HandlerMessage: can't find order " + id));
                        return;

                    case IBErrorCode.OrderCancelled:
                        if (request != null)
                        {
                            break;
                        }
                        num7 = (int) err;
                        this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.NativeError, num7.ToString(), msg));
                        return;

                    case IBErrorCode.SizeMatchAllocation:
                        if (request != null)
                        {
                            request.AdjustOrderStatus(OrderStateId.Rejected, request.TMOrder.OrderId);
                            num7 = (int) err;
                            this.connection.ProcessEventArgs(new OrderStatusEventArgs(request.TMOrder, ErrorCode.OrderRejected, msg + " (" + num7.ToString() + ")", request.TMOrder.OrderId, request.TMOrder.LimitPrice, request.TMOrder.StopPrice, request.TMOrder.Quantity, request.TMOrder.AvgFillPrice, request.TMOrder.Filled, this.connection.OrderStates[OrderStateId.Rejected], this.connection.Now));
                        }
                        return;

                    case IBErrorCode.PriceVariation:
                        if (request != null)
                        {
                            if (request.TMOrder.OrderState.Id == OrderStateId.PendingChange)
                            {
                                OrderState orderState = request.TMOrder.OrderState;
                                int quantity = request.TMOrder.Quantity;
                                double limitPrice = request.TMOrder.LimitPrice;
                                double stopPrice = request.TMOrder.StopPrice;
                                if (request.TMOrder.History.Count >= 2)
                                {
                                    limitPrice = request.TMOrder.History[request.TMOrder.History.Count - 2].LimitPrice;
                                    stopPrice = request.TMOrder.History[request.TMOrder.History.Count - 2].StopPrice;
                                    orderState = request.TMOrder.History[request.TMOrder.History.Count - 2].OrderState;
                                    quantity = request.TMOrder.History[request.TMOrder.History.Count - 2].Quantity;
                                }
                                strArray = new string[7];
                                strArray[0] = "Order '";
                                strArray[1] = request.TMOrder.OrderId;
                                strArray[2] = "' can't be changed: ";
                                strArray[3] = msg;
                                strArray[4] = " (";
                                num7 = (int) err;
                                strArray[5] = num7.ToString();
                                strArray[6] = ")";
                                this.connection.ProcessEventArgs(new OrderStatusEventArgs(request.TMOrder, ErrorCode.UnableToChangeOrder, string.Concat(strArray), request.TMOrder.OrderId, limitPrice, stopPrice, quantity, request.TMOrder.AvgFillPrice, request.TMOrder.Filled, orderState, this.connection.Now));
                            }
                            else
                            {
                                request.AdjustOrderStatus(OrderStateId.Rejected, request.TMOrder.OrderId);
                                num7 = (int) err;
                                this.connection.ProcessEventArgs(new OrderStatusEventArgs(request.TMOrder, ErrorCode.OrderRejected, msg + " (" + num7.ToString() + ")", request.TMOrder.OrderId, request.TMOrder.LimitPrice, request.TMOrder.StopPrice, request.TMOrder.Quantity, request.TMOrder.AvgFillPrice, request.TMOrder.Filled, this.connection.OrderStates[OrderStateId.Rejected], this.connection.Now));
                            }
                        }
                        return;

                    case IBErrorCode.ServerValidateError:
                        if (this.managedAcctsRequest != null)
                        {
                            this.isFaAccount = false;
                            AccountUpdatesRequest request2 = new AccountUpdatesRequest(this, "", true);
                            request2.Send(this);
                            this.accountUpdateRequests.Add(request2);
                            this.managedAcctsRequest = null;
                            this.StartConnectTimer();
                        }
                        else if (request != null)
                        {
                            request.AdjustOrderStatus(OrderStateId.Rejected, request.TMOrder.OrderId);
                            num7 = (int) err;
                            this.connection.ProcessEventArgs(new OrderStatusEventArgs(request.TMOrder, ErrorCode.OrderRejected, msg + " (" + num7.ToString() + ")", request.TMOrder.OrderId, request.TMOrder.LimitPrice, request.TMOrder.StopPrice, request.TMOrder.Quantity, request.TMOrder.AvgFillPrice, request.TMOrder.Filled, this.connection.OrderStates[OrderStateId.Rejected], this.connection.Now));
                        }
                        else
                        {
                            this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.Panic, IBErrorCode.ServerValidateError.ToString(), msg));
                        }
                        return;

                    case IBErrorCode.Resubscribe2AccountData:
                        if (this.connection.Accounts.Count == 1)
                        {
                            num7 = (int) err;
                            this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.NativeError, num7.ToString(), msg));
                        }
                        else
                        {
                            AccountUpdatesRequest request3 = (AccountUpdatesRequest) this.accountUpdateRequests[0];
                            request3 = (AccountUpdatesRequest) this.accountUpdateRequests[0];
                            request3.elapsed = 0;
                            request3.subscribe = true;
                            request3.Send(this);
                        }
                        return;

                    case IBErrorCode.CantFindEid:
                    case IBErrorCode.CantFindMarketDepth:
                        return;

                    default:
                        goto Label_0924;
                }
                if (((request.TMOrder.OrderState.Id != OrderStateId.PendingCancel) && (request.TMOrder.OcaGroup.Length == 0)) && (request.TMOrder.OrderState.Id != OrderStateId.Filled))
                {
                    request.AdjustOrderStatus(OrderStateId.Cancelled, request.TMOrder.OrderId);
                    num7 = (int) err;
                    this.connection.ProcessEventArgs(new OrderStatusEventArgs(request.TMOrder, ErrorCode.NoError, msg + " (" + num7.ToString() + ")", request.TMOrder.OrderId, request.TMOrder.LimitPrice, request.TMOrder.StopPrice, request.TMOrder.Quantity, request.TMOrder.AvgFillPrice, request.TMOrder.Filled, this.connection.OrderStates[OrderStateId.Cancelled], this.connection.Now));
                }
                return;
            Label_04DD:
                if ((request.TMOrder.OrderState.Id == OrderStateId.PendingChange) || (request.TMOrder.OrderState.Id == OrderStateId.PendingCancel))
                {
                    OrderState state2 = request.TMOrder.OrderState;
                    int num4 = request.TMOrder.Quantity;
                    double num5 = request.TMOrder.LimitPrice;
                    double num6 = request.TMOrder.StopPrice;
                    if (request.TMOrder.History.Count >= 2)
                    {
                        num5 = request.TMOrder.History[request.TMOrder.History.Count - 2].LimitPrice;
                        num6 = request.TMOrder.History[request.TMOrder.History.Count - 2].StopPrice;
                        state2 = request.TMOrder.History[request.TMOrder.History.Count - 2].OrderState;
                        num4 = request.TMOrder.History[request.TMOrder.History.Count - 2].Quantity;
                    }
                    strArray = new string[9];
                    strArray[0] = "Order '";
                    strArray[1] = request.TMOrder.OrderId;
                    strArray[2] = "' can't be ";
                    strArray[3] = (request.TMOrder.OrderState.Id == OrderStateId.PendingChange) ? "changed" : "cancelled";
                    strArray[4] = ": ";
                    strArray[5] = msg;
                    strArray[6] = " (";
                    num7 = (int) err;
                    strArray[7] = num7.ToString();
                    strArray[8] = ")";
                    this.connection.ProcessEventArgs(new OrderStatusEventArgs(request.TMOrder, (request.TMOrder.OrderState.Id == OrderStateId.PendingChange) ? ErrorCode.UnableToChangeOrder : ErrorCode.UnableToCancelOrder, string.Concat(strArray), request.TMOrder.OrderId, num5, num6, num4, request.TMOrder.AvgFillPrice, request.TMOrder.Filled, state2, this.connection.Now));
                }
                else
                {
                    request.AdjustOrderStatus(OrderStateId.Rejected, request.TMOrder.OrderId);
                    num7 = (int) err;
                    this.connection.ProcessEventArgs(new OrderStatusEventArgs(request.TMOrder, ErrorCode.OrderRejected, msg + " (" + num7.ToString() + ")", request.TMOrder.OrderId, request.TMOrder.LimitPrice, request.TMOrder.StopPrice, request.TMOrder.Quantity, request.TMOrder.AvgFillPrice, request.TMOrder.Filled, this.connection.OrderStates[OrderStateId.Rejected], this.connection.Now));
                }
                return;
            Label_0924:
                if (request != null)
                {
                    request.AdjustOrderStatus(OrderStateId.Rejected, request.TMOrder.OrderId);
                    num7 = (int) err;
                    this.connection.ProcessEventArgs(new OrderStatusEventArgs(request.TMOrder, ErrorCode.OrderRejected, msg + " (" + num7.ToString() + ")", request.TMOrder.OrderId, request.TMOrder.LimitPrice, request.TMOrder.StopPrice, request.TMOrder.Quantity, request.TMOrder.AvgFillPrice, request.TMOrder.Filled, this.connection.OrderStates[OrderStateId.Rejected], this.connection.Now));
                }
                else
                {
                    num7 = (int) err;
                    this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.NativeError, num7.ToString(), msg));
                }
            }
            catch (Exception exception)
            {
                this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.Panic, "", "IB.Adapter.ProcessErrorNow: Exception caught : " + exception.Message));
            }
        }

        internal void StartConnectTimer()
        {
            if (this.executionsRequest == null)
            {
                this.Delay();
                this.executionsRequest = new ExecutionsRequest(this);
                this.executionsRequest.Send(this);
            }
            if (this.openOrdersRequest == null)
            {
                this.Delay();
                this.openOrdersRequest = new OpenOrdersRequest(this);
                this.openOrdersRequest.Send(this);
            }
            if ((this.autoOpenOrdersRequest == null) && (this.Options.ClientId == 0))
            {
                this.Delay();
                this.autoOpenOrdersRequest = new AutoOpenOrdersRequest(this, true);
                this.autoOpenOrdersRequest.Send(this);
            }
            if (this.connectTimer != null)
            {
                this.connectTimer.Stop();
            }
            this.connectTimer = new System.Timers.Timer((double) this.Options.WaitOnConnectMilliSeconds);
            this.connectTimer.AutoReset = false;
            this.connectTimer.Elapsed += new ElapsedEventHandler(this.ConnectTimer_Elapsed);
            this.connectTimer.Start();
        }

        private bool StartTws()
        {
            if (Globals.TraceSwitch.Connect)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") IB.Adapter.StartTws0");
            }
            FindTws tws = new FindTws();
            if (tws.Run() && this.Options.Connect2RunningTws)
            {
                return true;
            }
            if (!File.Exists(Environment.SystemDirectory + @"\javaw.exe"))
            {
                this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, ErrorCode.Panic, "Can't find Java executable at '" + Environment.SystemDirectory + @"\javaw.exe'. Unable to start TWS.", ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                return false;
            }
            RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE");
            if (key == null)
            {
                this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, ErrorCode.Panic, "IB.Adapter.StartTws: can't find registry key 'SOFTWARE'", ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                return false;
            }
            RegistryKey key2 = key.OpenSubKey("Trader Workstation");
            if (key2 == null)
            {
                this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, ErrorCode.Panic, "IB.Adapter.StartTws: can't find registry key 'Trader Workstation'", ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                return false;
            }
            object obj2 = key2.GetValue("jtspath");
            if (obj2 == null)
            {
                this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, ErrorCode.Panic, "IB.Adapter.StartTws: can't find registry key 'jtspath'", ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                return false;
            }
            string path = (string) obj2;
            if (!File.Exists(path + @"\jts.ini"))
            {
                this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, ErrorCode.Panic, "IB.Adapter.StartTws: file '" + path + @"\jts.ini' does not exist. Please check TWS installation.", ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                return false;
            }
            StreamReader reader = File.OpenText(path + @"\jts.ini");
            Regex regex = new Regex("");
            string str2 = Regex.Replace(reader.ReadToEnd(), "UseSSL=.*", "UseSSL=" + (this.Options.UseSsl ? "true" : "false"), RegexOptions.None);
            reader.Close();
            string innerText = "";
            try
            {
                XmlDocument document = new XmlDocument();
                XmlTextReader reader2 = new XmlTextReader(Globals.InstallDir + @"\Config.xml");
                document.Load(reader2);
                reader2.Close();
                innerText = document["TradeMagic"]["InteractiveBrokers"]["TwsStartupParameter"].InnerText;
            }
            catch (Exception)
            {
            }
            string str4 = path;
            if (Globals.TraceSwitch.Connect)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") IB.Adapter.StartTws1: '" + str4 + "'");
            }
            if (!this.Options.UseUserSettings)
            {
                this.twsTempDir = MakeTempDir();
                StreamWriter writer = File.CreateText(this.twsTempDir + @"\jts.ini");
                writer.Write(str2);
                writer.Close();
                if (Globals.TraceSwitch.Connect)
                {
                    Trace.WriteLine("(" + this.connection.IdPlus + ") IB.Adapter.StartTws2: '" + this.twsTempDir + "'");
                }
                foreach (string str5 in Directory.GetFileSystemEntries(path))
                {
                    if (File.Exists(str5 + @"\settings.xml"))
                    {
                        if (Globals.TraceSwitch.Connect)
                        {
                            Trace.WriteLine("(" + this.connection.IdPlus + ") IB.Adapter.StartTws30: '" + str5 + @"\settings.xml'");
                        }
                        DirectoryInfo info = new DirectoryInfo(str5);
                        Directory.CreateDirectory(this.twsTempDir + @"\" + info.Name);
                        foreach (FileSystemInfo info2 in new DirectoryInfo(Globals.InstallDir + @"\IB\BlankTws").GetFileSystemInfos())
                        {
                            if (info2 is FileInfo)
                            {
                                File.Copy(info2.FullName, this.twsTempDir + @"\" + info.Name + @"\" + info2.Name, true);
                            }
                        }
                        CultureInfo provider = new CultureInfo("en-US");
                        string str6 = DateTime.Now.AddHours(23.0).ToString("hh:mm,tt", provider);
                        reader = File.OpenText(str5 + @"\settings.xml");
                        regex = new Regex("");
                        string str7 = Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(reader.ReadToEnd(), "<port>[0-9]*</port>", "<port>" + this.Options.Port + "</port>", RegexOptions.None), "<socketClient>.*</socketClient>", "<socketClient>true</socketClient>", RegexOptions.None), "<showTipAtStartup>.*</showTipAtStartup>", "<showTipAtStartup>false</showTipAtStartup>", RegexOptions.None), "<autoLogoffTime>.*</autoLogoffTime>", "<autoLogoffTime>" + str6 + "</autoLogoffTime>", RegexOptions.None), "<autoOpenOrdDonwload>.*</autoOpenOrdDonwload>", "<autoOpenOrdDonwload>false</autoOpenOrdDonwload>", RegexOptions.None), "<fireOpenOrderToAPIOnModify>.*</fireOpenOrderToAPIOnModify>", "<fireOpenOrderToAPIOnModify>true</fireOpenOrderToAPIOnModify>", RegexOptions.None);
                        reader.Close();
                        writer = File.CreateText(this.twsTempDir + @"\" + info.Name + @"\settings.xml");
                        writer.Write(str7);
                        writer.Close();
                        if (Globals.TraceSwitch.Connect)
                        {
                            Trace.WriteLine("(" + this.connection.IdPlus + ") IB.Adapter.StartTws31: '" + this.twsTempDir + @"\" + info.Name + @"\settings.xml'");
                        }
                        if (Globals.TraceSwitch.Connect)
                        {
                            Trace.WriteLine("(" + this.connection.IdPlus + ") IB.Adapter.StartTws32: '" + str5 + @"\user.ini'");
                        }
                        reader = File.OpenText(str5 + @"\user.ini");
                        regex = new Regex("");
                        str2 = reader.ReadToEnd();
                        reader.Close();
                        str2 = Regex.Replace(Regex.Replace(str2, @"Warn\ on\ Exit.*", "Warn on Exit=0", RegexOptions.None), @"Warn\ on\ Exit\ with\ Pos.*", "Warn on Exit with Pos=0", RegexOptions.None);
                        writer = File.CreateText(str5 + @"\user.ini");
                        writer.Write(str2);
                        writer.Close();
                    }
                }
            }
            if (Globals.TraceSwitch.Connect)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") IB.Adapter.StartTws4");
            }
            int num = 0;
        Label_0705:
            this.twsProcess = new Process();
            if (innerText.Length > 0)
            {
                this.twsProcess.StartInfo.Arguments = innerText + " " + str4;
            }
            else
            {
                this.twsProcess.StartInfo.Arguments = "-cp " + path + @"\jts.jar;" + path + @"\jcommon-0.9.0.jar;" + path + @"\jfreechart-0.9.15.jar jclient/LoginFrame " + (this.Options.UseUserSettings ? str4 : this.twsTempDir);
            }
            this.twsProcess.StartInfo.FileName = Environment.SystemDirectory + @"\javaw.exe";
            this.twsProcess.StartInfo.UseShellExecute = false;
            this.twsProcess.StartInfo.WorkingDirectory = path;
            if (Globals.TraceSwitch.Connect)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") IB.Adapter.StartTws5: exec='" + this.twsProcess.StartInfo.FileName + "' args='" + this.twsProcess.StartInfo.Arguments + "'");
            }
            try
            {
                this.twsProcess.Start();
            }
            catch (Exception exception)
            {
                if (this.twsTempDir.Length > 0)
                {
                    Directory.Delete(this.twsTempDir, true);
                }
                this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, ErrorCode.Panic, "Unable to start TWS process '" + this.twsProcess.StartInfo.FileName + "': " + exception.Message, ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                return false;
            }
            if (Globals.TraceSwitch.Connect)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") IB.Adapter.StartTws6");
            }
            HandleStartupPopups popups = new HandleStartupPopups(this.Options.User, this.Options.Password);
            while (!popups.Run())
            {
                if (this.connection.ConnectionStatusId == ConnectionStatusId.Disconnected)
                {
                    return false;
                }
                Thread.Sleep(this.Options.PopupHandlerMilliSeconds);
            }
            if (Globals.TraceSwitch.Connect)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") IB.Adapter.StartTws7");
            }
            tws.IgnoreTwsInstances = tws.RunningTwsInstances;
            bool flag = false;
            HandleFailedLogin login = new HandleFailedLogin();
            for (int i = 0; i < ((this.Options.MaxWaitSecondsToLogin * 0x3e8) / this.Options.PopupHandlerMilliSeconds); i++)
            {
                if (Globals.TraceSwitch.Connect)
                {
                    Trace.WriteLine("(" + this.connection.IdPlus + ") IB.Adapter.StartTws71");
                }
                if (this.connection.ConnectionStatusId == ConnectionStatusId.Disconnected)
                {
                    return false;
                }
                if (tws.Run())
                {
                    flag = true;
                    break;
                }
                if (login.Run())
                {
                    if (Globals.TraceSwitch.Connect)
                    {
                        Trace.WriteLine("(" + this.connection.IdPlus + ") IB.Adapter.StartTws72");
                    }
                    popups = new HandleStartupPopups();
                    while (!popups.Run())
                    {
                        if (this.connection.ConnectionStatusId == ConnectionStatusId.Disconnected)
                        {
                            return false;
                        }
                        Application.DoEvents();
                        Thread.Sleep(this.Options.PopupHandlerMilliSeconds);
                    }
                    if (this.twsTempDir.Length > 0)
                    {
                        Directory.Delete(this.twsTempDir, true);
                    }
                    this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, ErrorCode.LoginFailed, "Invalid user or password, or system not available", ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                    return false;
                }
                Application.DoEvents();
                Thread.Sleep(this.Options.PopupHandlerMilliSeconds);
            }
            if (!flag)
            {
                if (Globals.TraceSwitch.Connect)
                {
                    Trace.WriteLine("(" + this.connection.IdPlus + ") IB.Adapter.StartTws73");
                }
                try
                {
                    this.twsProcess.Kill();
                    this.twsProcess = null;
                }
                catch (Exception)
                {
                }
                if (++num >= this.Options.MaxTrysToLogin)
                {
                    if (this.twsTempDir.Length > 0)
                    {
                        Directory.Delete(this.twsTempDir, true);
                    }
                    this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, ErrorCode.LoginFailed, "Timeout on waiting for TWS", ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                    return false;
                }
                goto Label_0705;
            }
            if (Globals.TraceSwitch.Connect)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") IB.Adapter.StartTws9");
            }
            return true;
        }

        public void Submit(iTrading.Core.Kernel.Order order)
        {
            OrderRequest request = null;
            lock (this.syncOrderStatusEvent)
            {
                this.Delay();
                if (this.Options.CheckForMarketData && (((order.Symbol.MarketData.Last == null) || (order.Symbol.MarketData.Ask == null)) || (order.Symbol.MarketData.Bid == null)))
                {
                    this.connection.ProcessEventArgs(new OrderStatusEventArgs(order, ErrorCode.OrderRejected, "Unable to submit order now: no market data available", order.OrderId, order.LimitPrice, order.StopPrice, order.Quantity, order.AvgFillPrice, order.Filled, this.connection.OrderStates[OrderStateId.Rejected], this.connection.Now));
                }
                else
                {
                    lock (this.ibSocket)
                    {
                        try
                        {
                            request = new OrderRequest(this, order);
                        }
                        catch (TMException exception)
                        {
                            this.connection.ProcessEventArgs(new OrderStatusEventArgs(order, ErrorCode.OrderRejected, exception.Message, order.OrderId, order.LimitPrice, order.StopPrice, order.Quantity, order.AvgFillPrice, order.Filled, this.connection.OrderStates[OrderStateId.Rejected], this.connection.Now));
                            goto Label_01EF;
                        }
                        lock (this.orders)
                        {
                            this.orders.Add(request.orderId = this.nextOrderId++, request);
                        }
                        this.Delay();
                        request.Send(this);
                    }
                    this.connection.ProcessEventArgs(new OrderStatusEventArgs(order, ErrorCode.NoError, "", order.OrderId, order.LimitPrice, order.StopPrice, order.Quantity, order.AvgFillPrice, order.Filled, this.connection.OrderStates[OrderStateId.PendingSubmit], this.connection.Now));
                Label_01EF:;
                }
            }
        }

        public void Subscribe(MarketData marketData)
        {
            this.Delay();
            lock (this.ibSocket)
            {
                int key = ++this.nextTickerId;
                lock (this.tickerId2Request)
                {
                    this.tickerId2Request.Add(key, marketData);
                }
                new MarketDataRequest(this, marketData, key).Send(this);
            }
        }

        public void Subscribe(MarketDepth marketDepth)
        {
            this.Delay();
            lock (this.ibSocket)
            {
                int key = ++this.nextTickerId;
                lock (this.tickerId2Request)
                {
                    this.tickerId2Request.Add(key, marketDepth);
                }
                new MarketDepthRequest(this, marketDepth, key).Send(this);
            }
        }

        public void SymbolLookup(Symbol template)
        {
            this.inSymbolLookup = true;
            lock (this.ibSocket)
            {
                new ContractDetailRequest(this, template).Send(this);
            }
        }

        private void TerminateTws()
        {
            try
            {
                this.twsProcess.Kill();
                this.twsProcess.WaitForExit();
            }
            catch (Exception)
            {
            }
            if (this.twsTempDir.Length > 0)
            {
                Directory.Delete(this.twsTempDir, true);
            }
        }

        public void Unsubscribe(MarketData marketData)
        {
            this.Delay();
            lock (this.ibSocket)
            {
                ((MarketDataRequest) marketData.AdapterLink).Cancel();
            }
        }

        public void Unsubscribe(MarketDepth marketDepth)
        {
            this.Delay();
            lock (this.ibSocket)
            {
                ((MarketDepthRequest) marketDepth.AdapterLink).Cancel();
            }
        }

        internal IBOptions Options
        {
            get
            {
                return (IBOptions) this.connection.Options;
            }
        }

        private delegate void ProcessDelegate(ITradingBaseEventArgs e);

        private delegate void ProcessErrorDelegate(IBErrorCode err, int id, string msg);
    }
}

