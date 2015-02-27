namespace iTrading.Mbt
{
    using MBTCOMLib;
    using MBTORDERSLib;
    using MBTQUOTELib;
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;
    using iTrading.Core.Kernel;

    internal class Adapter : IAdapter, IMarketData, IMarketDepth, IOrder, IOrderChange
    {
        private iTrading.Mbt.Callback callback;
        private static MbtComMgr comMgr = null;
        private bool connectedOrders = false;
        private bool connectedPerms = false;
        private bool connectedQuotes = false;
        internal Connection connection;
        internal static Adapter currentAdapter = null;
        private static bool initialConnect = true;
        private static string initialUser = "";
        internal MbtOrderClient orderClient = null;
        internal ArrayList orders2Cancel = new ArrayList();
        internal MbtPrefs prefs = null;
        internal MbtQuotes quotesMgr = null;
        private const int tmHostId = 9;

        internal Adapter(Connection connection)
        {
            currentAdapter = this;
            this.callback = new iTrading.Mbt.Callback(this);
            this.connection = connection;
            lock (typeof(Adapter))
            {
                if (comMgr == null)
                {
                    comMgr = new MbtComMgrClass();
                }
            }
        }

        public void Cancel(Order order)
        {
            if (Globals.TraceSwitch.Order)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") Mbt.Adapter.Cancel0: " + order.ToString());
            }
            string pbstrRetMsg = "";
            if (order.OrderState.Id == OrderStateId.PendingSubmit)
            {
                lock (this.orders2Cancel)
                {
                    if (!this.orders2Cancel.Contains(order))
                    {
                        if (Globals.TraceSwitch.Order)
                        {
                            Trace.WriteLine("(" + this.connection.IdPlus + ") Mbt.Adapter.Cancel1: " + order.ToString());
                        }
                        this.orders2Cancel.Add(order);
                    }
                }
            }
            else if (!this.orderClient.Cancel(order.OrderId, ref pbstrRetMsg))
            {
                this.connection.ProcessEventArgs(new OrderStatusEventArgs(order, ErrorCode.UnableToCancelOrder, "Order '" + order.OrderId + "' can't be cancelled now", order.OrderId, order.LimitPrice, order.StopPrice, order.Quantity, order.AvgFillPrice, order.Filled, order.OrderState, this.connection.Now));
            }
            else
            {
                this.connection.ProcessEventArgs(new OrderStatusEventArgs(order, ErrorCode.NoError, "", order.OrderId, order.LimitPrice, order.StopPrice, order.Quantity, order.AvgFillPrice, order.Filled, this.connection.OrderStates[OrderStateId.PendingCancel], this.connection.Now));
            }
        }

        public void Change(Order order)
        {
            if (Globals.TraceSwitch.Order)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") Mbt.Adapter.Change: " + order.ToString());
            }
            double limitPrice = order.LimitPrice;
            string bstrSymbol = this.Convert(order.Symbol);
            if ((order.OrderType.Id != OrderTypeId.Limit) && (order.OrderType.Id != OrderTypeId.StopLimit))
            {
                if (this.quotesMgr.GetCachedQuote(bstrSymbol).dAsk == 0.0)
                {
                    this.connection.ProcessEventArgs(new OrderStatusEventArgs(order, ErrorCode.OrderRejected, "Unable to change order now: no quotes available", order.OrderId, order.LimitPrice, order.StopPrice, order.Quantity, order.AvgFillPrice, order.Filled, this.connection.OrderStates[OrderStateId.Rejected], this.connection.Now));
                    return;
                }
                limitPrice = this.quotesMgr.GetCachedQuote(bstrSymbol).dAsk;
            }
            string pbstrRetMsg = "";
            if (!this.orderClient.Replace(order.OrderId, order.Quantity, order.Quantity, System.Convert.ToInt32(order.OrderType.MapId), System.Convert.ToInt32(order.TimeInForce.MapId), limitPrice, order.StopPrice, ref pbstrRetMsg))
            {
                this.connection.ProcessEventArgs(new OrderStatusEventArgs(order, ErrorCode.UnableToChangeOrder, "Order '" + order.OrderId + "' can't be changed now", order.OrderId, order.LimitPrice, order.StopPrice, order.Quantity, order.AvgFillPrice, order.Filled, order.OrderState, this.connection.Now));
            }
            else
            {
                this.connection.ProcessEventArgs(new OrderStatusEventArgs(order, ErrorCode.NoError, "", order.OrderId, order.LimitPrice, order.StopPrice, order.Quantity, order.AvgFillPrice, order.Filled, this.connection.OrderStates[OrderStateId.PendingChange], this.connection.Now));
            }
        }

        public void Clear()
        {
            foreach (Symbol symbol in this.connection.Symbols.Values)
            {
                if (symbol.MarketData.AdapterLink != null)
                {
                    this.quotesMgr.UnadviseSymbol((Quotes) symbol.MarketData.AdapterLink, this.Convert(symbol), 1);
                    symbol.MarketData.AdapterLink = null;
                }
                if (symbol.MarketDepth.AdapterLink != null)
                {
                    this.quotesMgr.UnadviseSymbol((Quotes) symbol.MarketDepth.AdapterLink, this.Convert(symbol), 2);
                    symbol.MarketDepth.AdapterLink = null;
                }
            }
        }

        private void comMgr_OnAlertAdded(MbtAlert alert)
        {
            if (alert.Severity == 0x7531)
            {
                this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.CriticalProviderMessage, "", alert.Message));
            }
            this.connection.ProcessEventArgs(new NewsEventArgs(this.connection, ErrorCode.NoError, "", "TM_" + Guid.NewGuid().ToString(), this.connection.NewsItemTypes[NewsItemTypeId.Default], Convert(alert.Date, alert.Time), alert.Message, alert.Message));
        }

        public void Connect()
        {
            if (Globals.TraceSwitch.Connect)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") Mbt.Adapter.Connect0");
            }
            if (this.connection.Options.Mode.Id == ModeTypeId.Simulation)
            {
                new Thread(new ThreadStart(this.ConnectSimulation)).Start();
            }
            else
            {
                if (Globals.TraceSwitch.Connect)
                {
                    Trace.WriteLine("(" + this.connection.IdPlus + ") Mbt.Adapter.Connect1");
                }
                Thread thread2 = null;
                ThreadStart start = new ThreadStart(this.HandleFailedLogin);
                thread2 = new Thread(start);
                thread2.Name = "MBT login popup handler";
                thread2.Start();
                if (Globals.TraceSwitch.Connect)
                {
                    Trace.WriteLine("(" + this.connection.IdPlus + ") Mbt.Adapter.Connect2");
                }
                this.orderClient = comMgr.OrderClient;
                this.quotesMgr = comMgr.Quotes;
                this.prefs = comMgr.Prefs;
                comMgr.EnableSplash(false);
                if (Globals.TraceSwitch.Connect)
                {
                    Trace.WriteLine("(" + this.connection.IdPlus + ") Mbt.Adapter.Connect3");
                }
                this.orderClient.OnLogonSucceed += (new _IMbtOrderClientEvents_OnLogonSucceedEventHandler(this.OnLogonSucceed));
                if (initialConnect)
                {
                    if (Globals.TraceSwitch.Connect)
                    {
                        Trace.WriteLine("(" + this.connection.IdPlus + ") Mbt.Adapter.Connect4");
                    }
                    if (!comMgr.DoLogin(9, this.connection.Options.User, this.connection.Options.Password, ""))
                    {
                        this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, ErrorCode.LoginFailed, "MBT Navigator login failed, please check username/password", ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                        thread2.Abort();
                        return;
                    }
                    thread2.Abort();
                    WinApi.ShowWindow(WinApi.FindWindow(null, "MbtNavSplash"), 0);
                    initialConnect = false;
                    initialUser = this.connection.Options.User;
                }
                else
                {
                    if (initialUser != this.connection.Options.User)
                    {
                        this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, ErrorCode.LoginFailed, "MBT Navigator does not support subsequent logins with different user ids. Please restart your application.", ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                        return;
                    }
                    if (Globals.TraceSwitch.Connect)
                    {
                        Trace.WriteLine("(" + this.connection.IdPlus + ") Mbt.Adapter.Connect5");
                    }
                    thread2.Abort();
                    this.orderClient.Connect();
                    if (Globals.TraceSwitch.Connect)
                    {
                        Trace.WriteLine("(" + this.connection.IdPlus + ") Mbt.Adapter.Connect6");
                    }
                    this.quotesMgr.Connect();
                    if (Globals.TraceSwitch.Connect)
                    {
                        Trace.WriteLine("(" + this.connection.IdPlus + ") Mbt.Adapter.Connect7");
                    }
                }
                this.prefs.ConfirmOrderCancel = false;
                this.prefs.ConfirmOrderChange = false;
                this.prefs.ConfirmOrderPlace = false;
                if (Globals.TraceSwitch.Connect)
                {
                    Trace.WriteLine("(" + this.connection.IdPlus + ") Mbt.Adapter.Connect9");
                }
            }
        }

        private void ConnectSimulation()
        {
            if (this.connection.Options.Mode.Id == ModeTypeId.Simulation)
            {
                this.Init();
                this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, ErrorCode.NoError, "", ConnectionStatusId.Connected, ConnectionStatusId.Connected, 0, ""));
            }
        }

        internal Symbol Convert(string symbol)
        {
            DateTime maxDate = Globals.MaxDate;
            SymbolType symbolType = this.connection.SymbolTypes[SymbolTypeId.Stock];
            if (symbol.ToCharArray()[0] != '/')
            {
                if (symbol.ToCharArray()[0] == '$')
                {
                    symbolType = this.connection.SymbolTypes[SymbolTypeId.Index];
                    symbol = symbol.Substring(1, symbol.Length - 1);
                }
            }
            else
            {
                symbolType = this.connection.SymbolTypes[SymbolTypeId.Future];
                string str = symbol.Substring(symbol.Length - 2, 1);
                int year = System.Convert.ToInt32(symbol.Substring(symbol.Length - 1, 1));
                if ((DateTime.Now.Year % 100) <= year)
                {
                    year += ((int) Math.Floor((double) (((double) DateTime.Now.Year) / 100.0))) * 100;
                }
                else
                {
                    year += ((int) Math.Ceiling((double) (((double) DateTime.Now.Year) / 100.0))) * 100;
                }
                switch (str)
                {
                    case "F":
                        maxDate = new DateTime(year, 1, 1);
                        break;

                    case "G":
                        maxDate = new DateTime(year, 2, 1);
                        break;

                    case "H":
                        maxDate = new DateTime(year, 3, 1);
                        break;

                    case "J":
                        maxDate = new DateTime(year, 4, 1);
                        break;

                    case "K":
                        maxDate = new DateTime(year, 5, 1);
                        break;

                    case "M":
                        maxDate = new DateTime(year, 6, 1);
                        break;

                    case "N":
                        maxDate = new DateTime(year, 7, 1);
                        break;

                    case "Q":
                        maxDate = new DateTime(year, 8, 1);
                        break;

                    case "U":
                        maxDate = new DateTime(year, 9, 1);
                        break;

                    case "V":
                        maxDate = new DateTime(year, 10, 1);
                        break;

                    case "X":
                        maxDate = new DateTime(year, 11, 1);
                        break;

                    case "Z":
                        maxDate = new DateTime(year, 12, 1);
                        break;
                }
                symbol = symbol.Substring(1, symbol.Length - 3);
            }
            Symbol symbol2 = this.connection.GetSymbolByProviderName(symbol, maxDate, symbolType, this.connection.Exchanges[ExchangeId.Default], 0.0, RightId.Unknown, LookupPolicyId.RepositoryOnly);
            if (symbol2 != null)
            {
                return symbol2;
            }
            return this.SymbolLookup(symbol, maxDate, symbolType, this.connection.Exchanges[ExchangeId.Default], 0.0, RightId.Unknown);
        }

        internal string Convert(Symbol symbol)
        {
            if (symbol.SymbolType.Id != SymbolTypeId.Future)
            {
                return symbol.GetProviderName(ProviderTypeId.MBTrading);
            }
            string str = "";
            switch (symbol.Expiry.Month)
            {
                case 1:
                    str = "F";
                    break;

                case 2:
                    str = "G";
                    break;

                case 3:
                    str = "H";
                    break;

                case 4:
                    str = "J";
                    break;

                case 5:
                    str = "K";
                    break;

                case 6:
                    str = "M";
                    break;

                case 7:
                    str = "N";
                    break;

                case 8:
                    str = "Q";
                    break;

                case 9:
                    str = "U";
                    break;

                case 10:
                    str = "V";
                    break;

                case 11:
                    str = "X";
                    break;

                case 12:
                    str = "Z";
                    break;
            }
            return string.Concat(new object[] { "/", symbol.GetProviderName(ProviderTypeId.MBTrading), str, symbol.Expiry.Year % 100 });
        }

        internal static DateTime Convert(string date, string time)
        {
            string[] strArray = date.Split(new char[] { '/' });
            string[] strArray2 = time.Split(new char[] { ':' });
            return new DateTime(System.Convert.ToInt32(strArray[2]), System.Convert.ToInt32(strArray[0]), System.Convert.ToInt32(strArray[1]), System.Convert.ToInt32(strArray2[0]), System.Convert.ToInt32(strArray2[1]), System.Convert.ToInt32(strArray2[2]));
        }

        public void Disconnect()
        {
            if (this.connection.Options.Mode.Id == ModeTypeId.Simulation)
            {
                new Thread(new ThreadStart(this.DisconnectNow)).Start();
            }
            else
            {
                comMgr.OnAlertAdded -=(new IMbtComMgrEvents_OnAlertAddedEventHandler(this.comMgr_OnAlertAdded));
                comMgr.OnCriticalShutdown -= (new IMbtComMgrEvents_OnCriticalShutdownEventHandler(this.OnCriticalShutdown));
                comMgr.OnHealthUpdate -= (new IMbtComMgrEvents_OnHealthUpdateEventHandler(this.OnHealthUpdate));
                this.orderClient.OnHistoryAdded -= (new _IMbtOrderClientEvents_OnHistoryAddedEventHandler(this.callback.OnHistoryAdded));
                this.orderClient.OnLogonSucceed -= (new _IMbtOrderClientEvents_OnLogonSucceedEventHandler(this.OnLogonSucceed));
                this.orderClient.OnPositionAdded -= (new _IMbtOrderClientEvents_OnPositionAddedEventHandler(this.callback.OnPositionAdded));
                this.orderClient.OnPositionUpdated -= (new _IMbtOrderClientEvents_OnPositionUpdatedEventHandler(this.callback.OnPositionUpdated));
                this.orderClient.OnSubmit -= (new _IMbtOrderClientEvents_OnSubmitEventHandler(this.callback.OnSubmit));
                if (this.quotesMgr != null)
                {
                    this.connection.Symbols.Clear();
                }
                this.prefs = null;
                if (this.quotesMgr != null)
                {
                    this.quotesMgr.Disconnect();
                    this.quotesMgr = null;
                }
                if (this.orderClient != null)
                {
                    this.orderClient.Disconnect();
                    this.orderClient = null;
                }
                this.connectedOrders = false;
                this.connectedPerms = false;
                this.connectedQuotes = false;
                new Thread(new ThreadStart(this.DisconnectNow)).Start();
            }
        }

        private void DisconnectNow()
        {
            this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, ErrorCode.NoError, "", ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
        }

        private void HandleFailedLogin()
        {
            try
            {
                iTrading.Mbt.HandleFailedLogin login = new iTrading.Mbt.HandleFailedLogin();
                while (true)
                {
                    login.Run();
                    Thread.Sleep(250);
                }
            }
            catch (Exception)
            {
            }
        }

        private void Init()
        {
            this.connection.ProcessEventArgs(new CurrencyEventArgs(this.connection, ErrorCode.NoError, "", CurrencyId.AustralianDollar, ""));
            this.connection.ProcessEventArgs(new CurrencyEventArgs(this.connection, ErrorCode.NoError, "", CurrencyId.BritishPound, ""));
            this.connection.ProcessEventArgs(new CurrencyEventArgs(this.connection, ErrorCode.NoError, "", CurrencyId.CanadianDollar, ""));
            this.connection.ProcessEventArgs(new CurrencyEventArgs(this.connection, ErrorCode.NoError, "", CurrencyId.Euro, ""));
            this.connection.ProcessEventArgs(new CurrencyEventArgs(this.connection, ErrorCode.NoError, "", CurrencyId.HongKongDollar, ""));
            this.connection.ProcessEventArgs(new CurrencyEventArgs(this.connection, ErrorCode.NoError, "", CurrencyId.JapaneseYen, ""));
            this.connection.ProcessEventArgs(new CurrencyEventArgs(this.connection, ErrorCode.NoError, "", CurrencyId.SwissFranc, ""));
            this.connection.ProcessEventArgs(new CurrencyEventArgs(this.connection, ErrorCode.NoError, "", CurrencyId.Unknown, ""));
            this.connection.ProcessEventArgs(new CurrencyEventArgs(this.connection, ErrorCode.NoError, "", CurrencyId.UsDollar, ""));
            this.connection.ProcessEventArgs(new AccountItemTypeEventArgs(this.connection, ErrorCode.NoError, "", AccountItemTypeId.BuyingPower, ""));
            this.connection.ProcessEventArgs(new AccountItemTypeEventArgs(this.connection, ErrorCode.NoError, "", AccountItemTypeId.CashValue, ""));
            if (this.Options.Mode.Id == ModeTypeId.Simulation)
            {
                this.connection.CreateSimulationAccount(this.connection.SimulationAccountName, new SimulationAccountOptions());
            }
            else
            {
                this.orderClient.Accounts.LockItems();
                foreach (MbtAccount account in this.orderClient.Accounts)
                {
                    this.connection.ProcessEventArgs(new AccountEventArgs(this.connection, ErrorCode.NoError, "", account.Account, null));
                    this.callback.UpdateAccount(account, this.connection.Now);
                }
                this.orderClient.Accounts.UnlockItems();
            }
            int num = 0x2710;
            this.connection.ProcessEventArgs(new ActionTypeEventArgs(this.connection, ErrorCode.NoError, "", ActionTypeId.Buy, num.ToString()));
            num = 0x2710;
            this.connection.ProcessEventArgs(new ActionTypeEventArgs(this.connection, ErrorCode.NoError, "", ActionTypeId.BuyToCover, num.ToString()));
            num = 0x2711;
            this.connection.ProcessEventArgs(new ActionTypeEventArgs(this.connection, ErrorCode.NoError, "", ActionTypeId.Sell, num.ToString()));
            num = 0x2712;
            this.connection.ProcessEventArgs(new ActionTypeEventArgs(this.connection, ErrorCode.NoError, "", ActionTypeId.SellShort, num.ToString()));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Default, "MBTX"));
            if (this.Options.Mode.Id == ModeTypeId.Live)
            {
                this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Arca, "ARCA"));
                this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Amex, "AMEX"));
                this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Brut, "BRUT"));
                this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Globex, "CME"));
                this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Inca, "INCA"));
                this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.SDot, "ISI"));
                this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Island, "ISLD"));
                this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Nnm, "NNM"));
                this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Nyse, "NYSE"));
                this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Oes, "OES"));
                this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Opra, "OPRA"));
                this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.OtcBB, "OTCBB"));
                this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Nscm, "SCM"));
                this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Soes, "SOES"));
                this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Redi, "REDI"));
            }
            this.connection.ProcessEventArgs(new FeatureTypeEventArgs(this.connection, ErrorCode.NoError, "", FeatureTypeId.MarketData, 0.0));
            this.connection.ProcessEventArgs(new FeatureTypeEventArgs(this.connection, ErrorCode.NoError, "", FeatureTypeId.MaxMarketDataStreams, 100.0));
            this.connection.ProcessEventArgs(new FeatureTypeEventArgs(this.connection, ErrorCode.NoError, "", FeatureTypeId.MarketDepth, 0.0));
            this.connection.ProcessEventArgs(new FeatureTypeEventArgs(this.connection, ErrorCode.NoError, "", FeatureTypeId.MaxMarketDepthStreams, 100.0));
            this.connection.ProcessEventArgs(new FeatureTypeEventArgs(this.connection, ErrorCode.NoError, "", FeatureTypeId.News, 0.0));
            this.connection.ProcessEventArgs(new FeatureTypeEventArgs(this.connection, ErrorCode.NoError, "", FeatureTypeId.Order, 0.0));
            this.connection.ProcessEventArgs(new FeatureTypeEventArgs(this.connection, ErrorCode.NoError, "", FeatureTypeId.OrderChange, 0.0));
            this.connection.ProcessEventArgs(new FeatureTypeEventArgs(this.connection, ErrorCode.NoError, "", FeatureTypeId.SynchronousSymbolLookup, 0.0));
            this.connection.ProcessEventArgs(new MarketDataTypeEventArgs(this.connection, ErrorCode.NoError, "", MarketDataTypeId.Ask, ""));
            this.connection.ProcessEventArgs(new MarketDataTypeEventArgs(this.connection, ErrorCode.NoError, "", MarketDataTypeId.Bid, ""));
            this.connection.ProcessEventArgs(new MarketDataTypeEventArgs(this.connection, ErrorCode.NoError, "", MarketDataTypeId.DailyHigh, ""));
            this.connection.ProcessEventArgs(new MarketDataTypeEventArgs(this.connection, ErrorCode.NoError, "", MarketDataTypeId.DailyLow, ""));
            this.connection.ProcessEventArgs(new MarketDataTypeEventArgs(this.connection, ErrorCode.NoError, "", MarketDataTypeId.DailyVolume, ""));
            this.connection.ProcessEventArgs(new MarketDataTypeEventArgs(this.connection, ErrorCode.NoError, "", MarketDataTypeId.Last, ""));
            this.connection.ProcessEventArgs(new MarketDataTypeEventArgs(this.connection, ErrorCode.NoError, "", MarketDataTypeId.LastClose, ""));
            this.connection.ProcessEventArgs(new MarketDataTypeEventArgs(this.connection, ErrorCode.NoError, "", MarketDataTypeId.Opening, ""));
            this.connection.ProcessEventArgs(new MarketDataTypeEventArgs(this.connection, ErrorCode.NoError, "", MarketDataTypeId.Unknown, ""));
            this.connection.ProcessEventArgs(new MarketPositionEventArgs(this.connection, ErrorCode.NoError, "", MarketPositionId.Long, ""));
            this.connection.ProcessEventArgs(new MarketPositionEventArgs(this.connection, ErrorCode.NoError, "", MarketPositionId.Short, ""));
            this.connection.ProcessEventArgs(new MarketPositionEventArgs(this.connection, ErrorCode.NoError, "", MarketPositionId.Unknown, ""));
            this.connection.ProcessEventArgs(new NewsItemTypeEventArgs(this.connection, ErrorCode.NoError, "", NewsItemTypeId.Default, ""));
            this.connection.ProcessEventArgs(new OrderStateEventArgs(this.connection, ErrorCode.NoError, "", OrderStateId.Cancelled, ""));
            this.connection.ProcessEventArgs(new OrderStateEventArgs(this.connection, ErrorCode.NoError, "", OrderStateId.Filled, ""));
            this.connection.ProcessEventArgs(new OrderStateEventArgs(this.connection, ErrorCode.NoError, "", OrderStateId.Initialized, ""));
            this.connection.ProcessEventArgs(new OrderStateEventArgs(this.connection, ErrorCode.NoError, "", OrderStateId.PartFilled, ""));
            this.connection.ProcessEventArgs(new OrderStateEventArgs(this.connection, ErrorCode.NoError, "", OrderStateId.PendingCancel, ""));
            this.connection.ProcessEventArgs(new OrderStateEventArgs(this.connection, ErrorCode.NoError, "", OrderStateId.PendingSubmit, ""));
            this.connection.ProcessEventArgs(new OrderStateEventArgs(this.connection, ErrorCode.NoError, "", OrderStateId.PendingChange, ""));
            this.connection.ProcessEventArgs(new OrderStateEventArgs(this.connection, ErrorCode.NoError, "", OrderStateId.Rejected, ""));
            this.connection.ProcessEventArgs(new OrderStateEventArgs(this.connection, ErrorCode.NoError, "", OrderStateId.Accepted, ""));
            this.connection.ProcessEventArgs(new OrderStateEventArgs(this.connection, ErrorCode.NoError, "", OrderStateId.Unknown, ""));
            this.connection.ProcessEventArgs(new OrderStateEventArgs(this.connection, ErrorCode.NoError, "", OrderStateId.Working, ""));
            num = 0x272f;
            this.connection.ProcessEventArgs(new OrderTypeEventArgs(this.connection, ErrorCode.NoError, "", OrderTypeId.Market, num.ToString()));
            num = 0x272e;
            this.connection.ProcessEventArgs(new OrderTypeEventArgs(this.connection, ErrorCode.NoError, "", OrderTypeId.Limit, num.ToString()));
            num = 0x2730;
            this.connection.ProcessEventArgs(new OrderTypeEventArgs(this.connection, ErrorCode.NoError, "", OrderTypeId.Stop, num.ToString()));
            num = 0x2731;
            this.connection.ProcessEventArgs(new OrderTypeEventArgs(this.connection, ErrorCode.NoError, "", OrderTypeId.StopLimit, num.ToString()));
            this.connection.ProcessEventArgs(new SymbolTypeEventArgs(this.connection, ErrorCode.NoError, "", SymbolTypeId.Unknown, ""));
            this.connection.ProcessEventArgs(new SymbolTypeEventArgs(this.connection, ErrorCode.NoError, "", SymbolTypeId.Future, ""));
            this.connection.ProcessEventArgs(new SymbolTypeEventArgs(this.connection, ErrorCode.NoError, "", SymbolTypeId.Index, ""));
            this.connection.ProcessEventArgs(new SymbolTypeEventArgs(this.connection, ErrorCode.NoError, "", SymbolTypeId.Stock, ""));
            num = 0x271b;
            this.connection.ProcessEventArgs(new TimeInForceEventArgs(this.connection, ErrorCode.NoError, "", TimeInForceId.Day, num.ToString()));
        }

        private void LogonSucceed()
        {
            if (Globals.TraceSwitch.Connect)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") Mbt.Adapter.LogonSucceed0");
            }
            while (!this.orderClient.GotInitialLogonSucceed())
            {
                Application.DoEvents();
                Thread.Sleep(10);
            }
            if (Globals.TraceSwitch.Connect)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") Mbt.Adapter.LogonSucceed1");
            }
            if (this.connection.ConnectionStatusId == ConnectionStatusId.Connecting)
            {
                if (Globals.TraceSwitch.Connect)
                {
                    Trace.WriteLine("(" + this.connection.IdPlus + ") Mbt.Adapter.LogonSucceed2");
                }
                while (this.orderClient.Accounts.Count == 0)
                {
                    Application.DoEvents();
                    Thread.Sleep(10);
                }
                if (Globals.TraceSwitch.Connect)
                {
                    Trace.WriteLine("(" + this.connection.IdPlus + ") Mbt.Adapter.LogonSucceed3");
                }
                this.prefs.Serialize(false, true);
                if (Globals.TraceSwitch.Connect)
                {
                    Trace.WriteLine("(" + this.connection.IdPlus + ") Mbt.Adapter.LogonSucceed4");
                }
                comMgr.OnAlertAdded += new IMbtComMgrEvents_OnAlertAddedEventHandler(this.comMgr_OnAlertAdded);
                comMgr.OnCriticalShutdown+=new IMbtComMgrEvents_OnCriticalShutdownEventHandler(this.OnCriticalShutdown);
                comMgr.OnHealthUpdate+=(new IMbtComMgrEvents_OnHealthUpdateEventHandler(this.OnHealthUpdate));
                this.orderClient.OnHistoryAdded += (new _IMbtOrderClientEvents_OnHistoryAddedEventHandler(this.callback.OnHistoryAdded));
                this.orderClient.OnPositionAdded += (new _IMbtOrderClientEvents_OnPositionAddedEventHandler(this.callback.OnPositionAdded));
                this.orderClient.OnPositionUpdated += (new _IMbtOrderClientEvents_OnPositionUpdatedEventHandler(this.callback.OnPositionUpdated));
                this.orderClient.OnSubmit += (new _IMbtOrderClientEvents_OnSubmitEventHandler(this.callback.OnSubmit));
                this.Init();
                comMgr.Alerts.LockItems();
                foreach (MbtAlert alert in comMgr.Alerts)
                {
                    this.comMgr_OnAlertAdded(alert);
                }
                comMgr.Alerts.UnlockItems();
                this.RecoverOrdersPositions();
                this.connectedOrders = true;
                this.connectedPerms = true;
                this.connectedQuotes = true;
                this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, ErrorCode.NoError, "", ConnectionStatusId.Connected, ConnectionStatusId.Connected, 0, ""));
                if (Globals.TraceSwitch.Connect)
                {
                    Trace.WriteLine("(" + this.connection.IdPlus + ") Mbt.Adapter.LogonSucceed9");
                }
            }
        }

        private void OnCriticalShutdown()
        {
            if (Globals.TraceSwitch.Connect)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") Mbt.Adapter.OnCriticalShutdown");
            }
            this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.NativeError, "", "Got critical shutdown event"));
            this.Disconnect();
        }

        private void OnHealthUpdate(enumServerIndex index, enumConnectionState state)
        {
            if (Globals.TraceSwitch.Connect)
            {
                Trace.WriteLine(string.Concat(new object[] { "(", this.connection.IdPlus, ") Mbt.Adapter.OnHealthUpdate: ", index, " ", state }));
            }
            if ((index == enumServerIndex.siOrders) && (state == enumConnectionState.csLoggedIn))
            {
                this.connectedOrders = true;
            }
            else if ((index == enumServerIndex.siPerms) && (state == enumConnectionState.csLoggedIn))
            {
                this.connectedPerms = true;
            }
            else if ((index == enumServerIndex.siQuotes) && (state == enumConnectionState.csLoggedIn))
            {
                this.connectedQuotes = true;
            }
            else if ((index == enumServerIndex.siOrders) && (state == enumConnectionState.csDisconnected))
            {
                this.connectedOrders = false;
            }
            else if ((index == enumServerIndex.siPerms) && (state == enumConnectionState.csDisconnected))
            {
                this.connectedPerms = false;
            }
            else if ((index == enumServerIndex.siQuotes) && (state == enumConnectionState.csDisconnected))
            {
                this.connectedQuotes = false;
            }
            if ((this.connection.ConnectionStatusId == ConnectionStatusId.Connected) && !this.connectedOrders)
            {
                this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, ErrorCode.NoError, "Unexpected disconnect from server. Retrying connect ...", ConnectionStatusId.ConnectionLost, this.connection.SecondaryConnectionStatusId, 0, ""));
            }
            else if ((this.connection.ConnectionStatusId != ConnectionStatusId.Connected) && this.connectedOrders)
            {
                this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, ErrorCode.NoError, "", ConnectionStatusId.Connected, this.connection.SecondaryConnectionStatusId, 0, ""));
                this.RecoverOrdersPositions();
                this.orderClient.Accounts.LockItems();
                foreach (MbtAccount account in this.orderClient.Accounts)
                {
                    this.callback.UpdateAccount(account, this.connection.Now);
                }
                this.orderClient.Accounts.UnlockItems();
            }
            else if ((this.connection.SecondaryConnectionStatusId == ConnectionStatusId.Connected) && (!this.connectedQuotes || !this.connectedPerms))
            {
                this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, ErrorCode.NoError, "Unexpected disconnect from secondary server(s). Retrying connect ...", this.connection.ConnectionStatusId, ConnectionStatusId.ConnectionLost, 0, ""));
            }
            else if (((this.connection.SecondaryConnectionStatusId != ConnectionStatusId.Connected) && this.connectedQuotes) && this.connectedPerms)
            {
                this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, ErrorCode.NoError, "", this.connection.ConnectionStatusId, ConnectionStatusId.Connected, 0, ""));
            }
        }

        private void OnLogonSucceed()
        {
            this.LogonSucceed();
        }

        private void RecoverOrdersPositions()
        {
            this.orderClient.Positions.LockItems();
            foreach (MbtPosition position in this.orderClient.Positions)
            {
                this.callback.OnPositionAdded(position);
            }
            this.orderClient.Positions.UnlockItems();
            this.orderClient.OrderHistories.LockItems();
            int num = -1;
            foreach (MbtOrderHistory history in this.orderClient.OrderHistories)
            {
                num++;
                if (history.Event == "Executed")
                {
                    Account account = this.connection.Accounts.FindByName(history.Account.Account);
                    if (account == null)
                    {
                        this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.Panic, "", "Mbt.Adapter.ConnectNow: unknown account '" + history.Account.Account + "'"));
                    }
                    else
                    {
                        Symbol symbol = this.Convert(history.Symbol);
                        DateTime time = Convert(history.Date, history.Time);
                        string execId = "TM_" + time.ToString("yyyyMMdd") + "_" + num.ToString("0000");
                        Execution execution = account.Executions.FindByExecId(execId);
                        if (execution != null)
                        {
                            this.connection.ProcessEventArgs(new ExecutionUpdateEventArgs(this.connection, ErrorCode.NoError, "", Operation.Update, execution.Id, execution.Account, execution.Symbol, execution.Time, this.connection.MarketPositions[(history.BuySell == 0x2710) ? MarketPositionId.Long : MarketPositionId.Short], execution.OrderId, history.Quantity, history.Price));
                            continue;
                        }
                        this.connection.ProcessEventArgs(new ExecutionUpdateEventArgs(this.connection, ErrorCode.NoError, "", Operation.Insert, execId, account, symbol, time, this.connection.MarketPositions[(history.BuySell == 0x2710) ? MarketPositionId.Long : MarketPositionId.Short], history.OrderNumber, history.Quantity, history.Price));
                    }
                }
            }
            this.orderClient.OrderHistories.UnlockItems();
            Hashtable hashtable = new Hashtable();
            this.orderClient.OrderHistories.LockItems();
            foreach (MbtOrderHistory history2 in this.orderClient.OrderHistories)
            {
                if (this.connection.Accounts.FindByName(history2.Account.Account) == null)
                {
                    this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.Panic, "", "Mbt.Adapter.LogonSucceed: unknown account '" + history2.Account + "'"));
                }
                else
                {
                    ArrayList list = null;
                    list = (ArrayList) hashtable[history2.OrderNumber];
                    if (list == null)
                    {
                        hashtable.Add(history2.OrderNumber, list = new ArrayList());
                    }
                    list.Add(history2);
                }
            }
            this.orderClient.OrderHistories.UnlockItems();
            foreach (ArrayList list2 in hashtable.Values)
            {
                MbtOrderHistory history3 = (MbtOrderHistory) list2[0];
                this.callback.AddOrder(history3.Account, history3.Symbol, (iTrading.Mbt.ActionType) history3.BuySell, (iTrading.Mbt.OrderType) history3.OrderType, history3.OrderNumber, history3.Quantity, history3.Price, history3.StopLimit, list2);
            }
        }

        public void Submit(Order order)
        {
            if (Globals.TraceSwitch.Order)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") Mbt.Adapter.Submit: " + order.ToString());
            }
            MbtAccount pAcct = null;
            this.orderClient.Accounts.LockItems();
            foreach (MbtAccount account2 in this.orderClient.Accounts)
            {
                if (account2.Account == order.Account.Name)
                {
                    pAcct = account2;
                }
            }
            this.orderClient.Accounts.UnlockItems();
            Trace.Assert(pAcct != null, "Mbt.Adapter.Submit: account='" + order.Account.Name + "'");
            double limitPrice = order.LimitPrice;
            string bstrSymbol = this.Convert(order.Symbol);
            if ((order.OrderType.Id != OrderTypeId.Limit) && (order.OrderType.Id != OrderTypeId.StopLimit))
            {
                if (this.quotesMgr.GetCachedQuote(bstrSymbol).dAsk == 0.0)
                {
                    this.connection.ProcessEventArgs(new OrderStatusEventArgs(order, ErrorCode.OrderRejected, "Unable to submit order now: no quotes available", order.OrderId, order.LimitPrice, order.StopPrice, order.Quantity, order.AvgFillPrice, order.Filled, this.connection.OrderStates[OrderStateId.Rejected], this.connection.Now));
                    return;
                }
                limitPrice = this.quotesMgr.GetCachedQuote(bstrSymbol).dAsk;
            }
            string pbstrRetMsg = "";
            if (!this.orderClient.Submit(System.Convert.ToInt32(order.Action.MapId), order.Quantity, bstrSymbol, limitPrice, order.StopPrice, System.Convert.ToInt32(order.TimeInForce.MapId), 0x2724, System.Convert.ToInt32(order.OrderType.MapId), 0x273a, order.Quantity, pAcct, order.Symbol.Exchange.MapId, "", 0.0, 0.0, "", 0, 0, 0, 0, ref pbstrRetMsg))
            {
                this.connection.ProcessEventArgs(new OrderStatusEventArgs(order, ErrorCode.OrderRejected, pbstrRetMsg, order.OrderId, order.LimitPrice, order.StopPrice, order.Quantity, order.AvgFillPrice, order.Filled, this.connection.OrderStates[OrderStateId.Rejected], this.connection.Now));
            }
            else
            {
                this.connection.ProcessEventArgs(new OrderStatusEventArgs(order, ErrorCode.NoError, "", pbstrRetMsg, order.LimitPrice, order.StopPrice, order.Quantity, order.AvgFillPrice, order.Filled, this.connection.OrderStates[OrderStateId.PendingSubmit], this.connection.Now));
            }
        }

        public void Subscribe(MarketData marketData)
        {
            Quotes pNotify = new Quotes(this, marketData.Symbol);
            marketData.AdapterLink = pNotify;
            this.quotesMgr.AdviseSymbol(pNotify, this.Convert(marketData.Symbol), 1);
        }

        public void Subscribe(MarketDepth marketDepth)
        {
            Quotes pNotify = new Quotes(this, marketDepth.Symbol);
            marketDepth.AdapterLink = pNotify;
            this.quotesMgr.AdviseSymbol(pNotify, this.Convert(marketDepth.Symbol), 2);
        }

        public void SymbolLookup(Symbol template)
        {
            this.SymbolLookup(template.GetProviderName(ProviderTypeId.MBTrading), template.Expiry, template.SymbolType, template.Exchange, template.StrikePrice, template.Right.Id);
        }

        private Symbol SymbolLookup(string name, DateTime expiry, SymbolType symbolType, Exchange exchange, double strikePrice, RightId rightId)
        {
            Symbol symbol = this.connection.GetSymbolByProviderName(name, expiry, symbolType, exchange, strikePrice, rightId, LookupPolicyId.RepositoryOnly);
            if (symbol != null)
            {
                return symbol;
            }
            double pointValue = 1.0;
            double tickSize = 0.01;
            if (symbolType.Id == SymbolTypeId.Future)
            {
                switch (name)
                {
                    case "ES":
                        pointValue = 50.0;
                        tickSize = 0.25;
                        goto Label_0108;

                    case "ER2":
                        pointValue = 100.0;
                        tickSize = 0.1;
                        goto Label_0108;

                    case "FE":
                        pointValue = 20.0;
                        tickSize = 0.5;
                        goto Label_0108;

                    case "NQ":
                        pointValue = 20.0;
                        tickSize = 0.5;
                        break;

                    case "YM":
                        pointValue = 5.0;
                        tickSize = 1.0;
                        break;
                }
            }
        Label_0108:
            this.connection.CreateSymbol(name, expiry, symbolType, exchange, strikePrice, rightId, this.connection.Currencies[CurrencyId.Unknown], tickSize, pointValue, "", null, 0, null, null, null);
            symbol = this.connection.GetSymbolByProviderName(name, expiry, symbolType, exchange, strikePrice, rightId, LookupPolicyId.RepositoryOnly);
            Trace.Assert(symbol != null, string.Concat(new object[] { "Mbt.Adapter.SymbolLooup: name='", name, "' expiry=", expiry, " symbolType=", symbolType, " exchange=", exchange, " strikePrice=", strikePrice, " rightId=", rightId }));
            Quotes pNotify = new Quotes(this, symbol);
            symbol.MarketData.AdapterLink = pNotify;
            this.quotesMgr.AdviseSymbol(pNotify, this.Convert(symbol), 1);
            return symbol;
        }

        public void Unsubscribe(MarketData marketData)
        {
            if (marketData.AdapterLink != null)
            {
                Trace.Assert(marketData.AdapterLink is Quotes, "MbtAdapter.Unsubscribe.MarketData");
                this.quotesMgr.UnadviseSymbol((Quotes) marketData.AdapterLink, this.Convert(marketData.Symbol), 1);
            }
        }

        public void Unsubscribe(MarketDepth marketDepth)
        {
            if (marketDepth.AdapterLink != null)
            {
                Trace.Assert(marketDepth.AdapterLink is Quotes, "MbtAdapter.Unsubscribe.MarketDepth");
                this.quotesMgr.UnadviseSymbol((Quotes) marketDepth.AdapterLink, this.Convert(marketDepth.Symbol), 2);
            }
        }

        private MbtOptions Options
        {
            get
            {
                return (MbtOptions) this.connection.Options;
            }
        }

        internal delegate void Process(object param);
    }
}

