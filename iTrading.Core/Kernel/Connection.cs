using System.Windows.Forms;
using iTrading.Core.Data;
using iTrading.Core.Kernel;

namespace iTrading.Core.Kernel
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Timers;
    using System.Xml;
    using iTrading.Core.Interface;
    using iTrading.Core.Data;

    /// <summary>
    /// Represents a connection to a broker / data provider.
    /// </summary>
    [ClassInterface(ClassInterfaceType.None), Guid("7884331C-E8F9-4958-A618-A30BEE6B31AA")]
    public class Connection : Request, IComConnection, ITradingSerializable
    {
        private AccountItemTypeDictionary accountItemTypes;
        private AccountCollection accounts;
        private ActionTypeDictionary actionTypes;
        internal IAdapter adapter;
        internal int clientId;
        private iTrading.Core.Kernel.ConnectionStatusId connectionStatusId;
        internal static CultureInfo cultureInfo = new CultureInfo("en-US");
        private CurrencyDictionary currencies;
        private string customText;
        private bool disconnectedSent;
        internal ProcessEventArgsHandler eventArgsHandler;
        private ExchangeDictionary exchanges;
        private FeatureTypeDictionary featureTypes;
        internal IMarketData marketData;
        internal MarketDataCollection marketDataStreams;
        private MarketDataTypeDictionary marketDataTypes;
        internal IMarketDepth marketDepth;
        internal MarketDepthCollection marketDepthStreams;
        private MarketPositionDictionary marketPositions;
        private NewsEventArgsCollection news;
        private NewsItemTypeDictionary newsItemTypes;
        private double nowDelayMilliSeconds;
        private double nowMilliSeconds;
        private OptionsBase options;
        internal IOrder order;
        internal IOrderChange orderChange;
        private OrderStateDictionary orderStates;
        private OrderTypeDictionary orderTypes;
        internal IQuotes quotes;
        internal ArrayList quotesRequests;
        internal System.Timers.Timer recorderTimer;
        internal System.Timers.Timer replayTimer;
        private iTrading.Core.Kernel.ConnectionStatusId secondaryConnectionStatusId;
        private string simulationAccountName;
        private double simulationSpeed;
        internal StreamingRequestDictionary streamingRequests;
        internal iTrading.Core.Kernel.Symbol symbol2Lookup;
        private SymbolDictionary symbols;
        private object[] symbolsSyncObject;
        private SymbolTypeDictionary symbolTypes;
        internal iTrading.Core.Kernel.SynchronizeInvoke synchronizeInvoke;
        private TimeInForceDictionary timeInForce;
        private System.Timers.Timer timer;

        /// <summary>
        /// This event will be thrown when a <see cref="E:iTrading.Core.Kernel.Connection.Bar" /> object is updated.
        /// </summary>
        public event BarUpdateEventHandler Bar;

        /// <summary>
        /// Indicates updates on the connection status, see <see cref="P:iTrading.Core.Kernel.Connection.ConnectionStatusId" /> = <see cref="F:iTrading.Core.Kernel.ConnectionStatusId.Disconnected" />
        /// </summary>
        public event ConnectionStatusEventHandler ConnectionStatus;

        /// <summary>
        /// An error has occured. <seealso cref="T:iTrading.Core.Kernel.ITradingErrorEventArgs" />.
        /// </summary>
        public event ErrorArgsEventHandler Error;

        /// <summary>
        /// This event will be thrown once for symbol looked up in the broker's system.
        /// </summary>
        public event SymbolEventHandler Symbol;

        /// <summary>
        /// Timer event. Thrown every <see cref="P:iTrading.Core.Kernel.OptionsBase.TimerIntervalMilliSeconds" /> milliseconds.
        /// </summary>
        public event TimerEventHandler Timer;

        /// <summary>
        /// Creates a new <see cref="T:iTrading.Core.Kernel.Connection" /> instance. 
        /// For opening the connection, call <see cref="M:iTrading.Core.Kernel.Connection.Connect(iTrading.Core.Kernel.OptionsBase)" />.
        /// </summary>

        #region Event Invoke


        public void OnConnectionStatusChange(Request pRequest, ConnectionStatusEventArgs pEvent)
        {
            if (ConnectionStatus != null)
            {
                ConnectionStatus(pRequest, pEvent);
            }
        }

        public void OnBarChange(Request pRequest, BarUpdateEventArgs pEvent)
        {
            if (Bar != null)
            {
                Bar(pRequest, pEvent);
            }
        }
        public void OnTimerChange(Request pRequest, TimerEventArgs pEvent)
        {
            if (Timer != null)
            {
                Timer(pRequest, pEvent);
            }
        }

        public void OnSymbolChange(Request pRequest, SymbolEventArgs pEvent)
        {
            if (Symbol != null)
            {
                Symbol(pRequest, pEvent);
            }
        }

        public void OnErrorChange(Request pRequest, ITradingErrorEventArgs pEvent)
        {
            if (Error  != null)
            {
                Error(pRequest, pEvent);
            }
        }

        public void OnExchangeChange(Request pRequest, ExchangeEventArgs pEvent)
        {            
                Exchanges.OnExchangeChange( pRequest, pEvent);           
        }

        public void OnCurrencyChange(Request pRequest, CurrencyEventArgs pEvent)
        {
            Currencies.OnCurrencyChange( pRequest, pEvent);
        }

        public void OnOrderTypesChange(Request pRequest, OrderTypeEventArgs pEvent)
        {
            OrderTypes.OnOrderTypeChange(pRequest, pEvent);
        }
        #endregion
        public Connection() : base(null)
        {
            this.accounts = new AccountCollection();
            this.adapter = null;
            this.Bar = null;
            this.clientId = 0;
            this.ConnectionStatus = null;
            this.connectionStatusId = iTrading.Core.Kernel.ConnectionStatusId.Disconnected;
            this.customText = "";
            this.disconnectedSent = false;
            this.eventArgsHandler = null;
            this.marketData = null;
            this.marketDataStreams = new MarketDataCollection();
            this.marketDepth = null;
            this.marketDepthStreams = new MarketDepthCollection();
            this.nowMilliSeconds = 0.0;
            this.nowDelayMilliSeconds = 0.0;
            this.options = null;
            this.order = null;
            this.orderChange = null;
            this.quotesRequests = new ArrayList();
            this.quotes = null;
            this.recorderTimer = null;
            this.replayTimer = null;
            this.secondaryConnectionStatusId = iTrading.Core.Kernel.ConnectionStatusId.Disconnected;
            this.simulationAccountName = "TM Simulation";
            this.simulationSpeed = 1.0;
            this.streamingRequests = new StreamingRequestDictionary();
            this.Symbol = null;
            this.symbolsSyncObject = new object[0];
            this.symbol2Lookup = null;
            this.synchronizeInvoke = null;
            this.timer = null;
            this.Timer  = null;
            base.connection = this;
            this.actionTypes = new ActionTypeDictionary();
            this.accountItemTypes = new AccountItemTypeDictionary();
            this.currencies = new CurrencyDictionary();
            this.eventArgsHandler = new ProcessEventArgsHandler(this.ProcessEventArgsInThreadContext);
            this.exchanges = new ExchangeDictionary();
            this.featureTypes = new FeatureTypeDictionary();
            this.marketDataTypes = new MarketDataTypeDictionary();
            this.marketPositions = new MarketPositionDictionary();
            this.news = new NewsEventArgsCollection(this);
            this.newsItemTypes = new NewsItemTypeDictionary();
            this.orderStates = new OrderStateDictionary();
            this.orderTypes = new OrderTypeDictionary();
            this.symbols = new SymbolDictionary(this);
            this.symbolTypes = new SymbolTypeDictionary();
            this.timeInForce = new TimeInForceDictionary();
            this.clientId = base.Id;
            this.synchronizeInvoke = new iTrading.Core.Kernel.SynchronizeInvoke();
        }

        /// <summary></summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public Connection(Bytes bytes, int version) : base(bytes, version)
        {
            this.accounts = new AccountCollection();
            this.adapter = null;
            this.Bar  = null;
            this.clientId = 0;
            this.ConnectionStatus= null;
            this.connectionStatusId = iTrading.Core.Kernel.ConnectionStatusId.Disconnected;
            this.customText = "";
            this.disconnectedSent = false;
            this.eventArgsHandler = null;
            this.marketData = null;
            this.marketDataStreams = new MarketDataCollection();
            this.marketDepth = null;
            this.marketDepthStreams = new MarketDepthCollection();
            this.nowMilliSeconds = 0.0;
            this.nowDelayMilliSeconds = 0.0;
            this.options = null;
            this.order = null;
            this.orderChange = null;
            this.quotesRequests = new ArrayList();
            this.quotes = null;
            this.recorderTimer = null;
            this.replayTimer = null;
            this.secondaryConnectionStatusId = iTrading.Core.Kernel.ConnectionStatusId.Disconnected;
            this.simulationAccountName = "TM Simulation";
            this.simulationSpeed = 1.0;
            this.streamingRequests = new StreamingRequestDictionary();
            this.Symbol = null;
            this.symbolsSyncObject = new object[0];
            this.symbol2Lookup = null;
            this.synchronizeInvoke = null;
            this.timer = null;
            this.Timer = null;
            this.connectionStatusId = bytes.ReadConnectionStatus().Id;
            this.options = (OptionsBase) bytes.ReadSerializable();
            if (version >= 2)
            {
                this.simulationSpeed = bytes.ReadDouble();
            }
            this.actionTypes = new ActionTypeDictionary();
            this.accountItemTypes = new AccountItemTypeDictionary();
            this.currencies = new CurrencyDictionary();
            this.eventArgsHandler = new ProcessEventArgsHandler(this.ProcessEventArgsInThreadContext);
            this.exchanges = new ExchangeDictionary();
            this.featureTypes = new FeatureTypeDictionary();
            this.marketDataTypes = new MarketDataTypeDictionary();
            this.marketPositions = new MarketPositionDictionary();
            this.news = new NewsEventArgsCollection(this);
            this.newsItemTypes = new NewsItemTypeDictionary();
            this.orderStates = new OrderStateDictionary();
            this.orderTypes = new OrderTypeDictionary();
            this.symbols = new SymbolDictionary(this);
            this.symbolTypes = new SymbolTypeDictionary();
            this.timeInForce = new TimeInForceDictionary();
            this.clientId = base.Id;
            this.synchronizeInvoke = new iTrading.Core.Kernel.SynchronizeInvoke();
        }

        /// <summary>
        /// Closes the actual connection and its streaming requests.
        /// </summary>
        public void Close()
        {
            if ((this.adapter != null) && (this.connectionStatusId != iTrading.Core.Kernel.ConnectionStatusId.Disconnected))
            {
                lock (this.streamingRequests)
                {
                    foreach (StreamingRequest request in this.streamingRequests.Values)
                    {
                        if (!(request is Order))
                        {
                            request.Cancel();
                        }
                    }
                    this.streamingRequests.Clear();
                }
                base.operation = Operation.Delete;
                this.SynchronizeInvoke.Invoke(new MethodInvoker(this.CloseNow), null);
            }
        }

        private void CloseNow()
        {
            try
            {
                this.adapter.Disconnect();
            }
            catch (Exception exception)
            {
                Trace.WriteLine("ERROR: Cbi.Connection.CloseNow: exception caught: " + exception.Message);
                this.ProcessEventArgs(new ConnectionStatusEventArgs(this, ErrorCode.NoError, "", iTrading.Core.Kernel.ConnectionStatusId.Disconnected, iTrading.Core.Kernel.ConnectionStatusId.Disconnected, 0, ""));
            }
        }

        /// <summary>
        /// Opens the connection. This is an asychronous operation.
        /// Make sure that your registered a <see cref="T:iTrading.Core.Kernel.ConnectionStatusEventHandler" /> callback to indicate success
        /// or failure of this operation.
        /// </summary>
        /// <param name="options">Actual connection options.</param>
        public void Connect(OptionsBase options)
        {
            if (Globals.TraceSwitch.Connect)
            {
                Trace.WriteLine("(" + this.IdPlus + ") Cbi.Connection.Connect " + this.ConnectionStatusId.ToString() + " assembly=" + Assembly.GetAssembly(typeof(Connection)).GetName().Version.ToString());
            }
            if (this.ConnectionStatusId != iTrading.Core.Kernel.ConnectionStatusId.Connected)
            {
                this.connectionStatusId = iTrading.Core.Kernel.ConnectionStatusId.Connecting;
                this.disconnectedSent = false;
                this.nowDelayMilliSeconds = -1.0;
                this.options = options;
                this.SynchronizeInvoke.Invoke(new MethodInvoker(this.ConnectNow), null);
            }
        }

        private void ConnectNow()
        {
            try
            {
                if (Globals.TraceSwitch.Connect)
                {
                    Trace.WriteLine("(" + this.IdPlus + ") Cbi.Connection.ConnectNow1");
                }
                this.options.license = this.GetLicense(this.options.Provider.Name + " " + this.options.Mode.Name);
                if (this.options.License.Id == LicenseTypeId.NotRegistered)
                {
                    this.ProcessEventArgs(new ConnectionStatusEventArgs(this, ErrorCode.InvalidLicense, "There is no valid TradeMagic registration.\r\n\r\nTradeMagic will not run without prior registration.", iTrading.Core.Kernel.ConnectionStatusId.Disconnected, iTrading.Core.Kernel.ConnectionStatusId.Disconnected, 0, ""));
                }
                else
                {
                    string str = "Client";
                    if (!this.Options.RunAtServer)
                    {
                        if (this.Options.Provider.Id == ProviderTypeId.CyberTrader)
                        {
                            str = "CT";
                        }
                        else if (this.Options.Provider.Id == ProviderTypeId.Dtn)
                        {
                            str = "Dtn";
                        }
                        else if (this.Options.Provider.Id == ProviderTypeId.ESignal)
                        {
                            str = "ESignal";
                        }
                        else if (this.Options.Provider.Id == ProviderTypeId.InteractiveBrokers)
                        {
                            str = "IB";
                        }
                        else if (this.Options.Provider.Id == ProviderTypeId.MBTrading)
                        {
                            str = "Mbt";
                        }
                        else if (this.Options.Provider.Id == ProviderTypeId.Patsystems)
                        {
                            str = "Pats";
                        }
                        else if (this.Options.Provider.Id == ProviderTypeId.TrackData)
                        {
                            str = "Track";
                        }
                        else if (this.Options.Provider.Id == ProviderTypeId.Yahoo)
                        {
                            str = "Yahoo";
                        }
                    }
                    string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\TradeMagic." + str + ".dll";
                    if (!File.Exists(path))
                    {
                        this.ProcessEventArgs(new ConnectionStatusEventArgs(this, ErrorCode.Panic, "broker adapter assembly '" + path + "' does not exist", iTrading.Core.Kernel.ConnectionStatusId.Disconnected, iTrading.Core.Kernel.ConnectionStatusId.Disconnected, 0, ""));
                    }
                    else
                    {
                        if (Globals.TraceSwitch.Connect)
                        {
                            Trace.WriteLine("(" + this.IdPlus + ") Cbi.Connection.ConnectNow2");
                        }
                        try
                        {
                            if (!this.Options.RunAtServer)
                            {
                                Globals.DB.Connect(this.Options.HistoryMaintained);
                            }
                        }
                        catch (Exception exception)
                        {
                            this.ProcessEventArgs(new ConnectionStatusEventArgs(this, ErrorCode.Panic, "Cbi.Connection.ConnectNow: Failed to connect to db. " + exception.Message, iTrading.Core.Kernel.ConnectionStatusId.Disconnected, iTrading.Core.Kernel.ConnectionStatusId.Disconnected, 0, ""));
                            return;
                        }
                        try
                        {
                            ILoader loader = null;
                            loader = (ILoader) Assembly.LoadFrom(path).CreateInstance("TradeMagic." + str + ".Loader");
                            if (Globals.TraceSwitch.Connect)
                            {
                                Trace.WriteLine("(" + this.IdPlus + ") Cbi.Connection.ConnectNow.CreateLoader");
                            }
                            base.operation = Operation.Insert;
                            this.adapter = loader.Create(this);
                            if (this.adapter is IMarketData)
                            {
                                this.marketData = (IMarketData) this.adapter;
                            }
                            if (this.adapter is IMarketDepth)
                            {
                                this.marketDepth = (IMarketDepth) this.adapter;
                            }
                            if (this.adapter is IOrder)
                            {
                                this.order = (IOrder) this.adapter;
                            }
                            if (this.adapter is IOrderChange)
                            {
                                this.orderChange = (IOrderChange) this.adapter;
                            }
                            if (this.adapter is IQuotes)
                            {
                                this.quotes = (IQuotes) this.adapter;
                            }
                            if (Globals.TraceSwitch.Connect)
                            {
                                Trace.WriteLine("(" + this.IdPlus + ") Cbi.Connection.ConnectNow.Connect");
                            }
                            this.adapter.Connect();
                        }
                        catch (Exception exception2)
                        {
                            string str3;
                            if (this.Options.Provider.Id == ProviderTypeId.MBTrading)
                            {
                                str3 = "Most likely you do not have installed the recommended version of MBT Navigator. \r\n\r\nPlease download from http://www.trademagic.net.\r\n\r\nException: " + exception2.Message;
                            }
                            else if (this.Options.Provider.Id == ProviderTypeId.ESignal)
                            {
                                str3 = "Most likely you do not have installed the recommended version of eSignal. \r\n\r\nPlease download from http://www.trademagic.net.\r\n\r\nException: " + exception2.Message;
                            }
                            else
                            {
                                str3 = "Cbi.Connection.ConnectNow: Failed to call IAdapter.Connect.  " + exception2.Message;
                            }
                            this.ProcessEventArgs(new ConnectionStatusEventArgs(this, ErrorCode.Panic, str3, iTrading.Core.Kernel.ConnectionStatusId.Disconnected, iTrading.Core.Kernel.ConnectionStatusId.Disconnected, 0, ""));
                            return;
                        }
                        if (Globals.TraceSwitch.Connect)
                        {
                            Trace.WriteLine("(" + this.IdPlus + ") Cbi.Connection.ConnectNow9 ok");
                        }
                    }
                }
            }
            catch (Exception exception3)
            {
                this.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this, ErrorCode.Panic, "", "Cbi.Connection.ConnectNow: exception caught: " + exception3.Message));
            }
        }

        /// <summary>
        /// Create account for order execution simulation.
        /// </summary>
        /// <param name="accountName">Account name, must be unique within the <see cref="P:iTrading.Core.Kernel.Connection.Accounts" /> collection</param>
        /// <param name="simulationAccountOptions">Default simulation options for this account.
        /// may be overriden by <see cref="M:iTrading.Core.Kernel.Order.Submit" />.</param>
        public Account CreateSimulationAccount(string accountName, SimulationAccountOptions simulationAccountOptions)
        {
            Account account = null;
            lock (this.accounts)
            {
                foreach (Account account2 in this.accounts)
                {
                    if (account2.Name == accountName)
                    {
                        throw new TMException(ErrorCode.Panic, "Account name for simulated account already exists");
                    }
                }
                this.ProcessEventArgs(new AccountEventArgs(this, ErrorCode.NoError, "", accountName, (simulationAccountOptions == null) ? (simulationAccountOptions = new SimulationAccountOptions()) : simulationAccountOptions));
                account = this.accounts.FindByName(accountName);
                Trace.Assert(account != null, "Cbi.Connection.CreateSimulationAccount: account='" + accountName + "'");
            }
            Globals.DB.Recover(account);
            XmlDocument document = new XmlDocument();
            XmlTextReader reader = new XmlTextReader(new StringReader(account.CustomText));
            string str = "_" + ModeTypeId.Simulation.ToString();
            try
            {
                document.Load(reader);
                reader.Close();
                foreach (XmlNode node in document["TradeMagic"][str]["AccountItems"])
                {
                    this.ProcessEventArgs(new AccountUpdateEventArgs(this, ErrorCode.NoError, "", account, AccountItemType.All[(AccountItemTypeId) Convert.ToInt32(node["AccountItemType"].InnerText, CultureInfo.InvariantCulture)], iTrading.Core.Kernel.Currency.All[(CurrencyId) Convert.ToInt32(node["Currency"].InnerText, CultureInfo.InvariantCulture)], Convert.ToDouble(node["Value"].InnerText, CultureInfo.InvariantCulture), this.Now));
                }
            }
            catch
            {
                this.ProcessEventArgs(new AccountUpdateEventArgs(this, ErrorCode.NoError, "", account, AccountItemType.All[AccountItemTypeId.BuyingPower], iTrading.Core.Kernel.Currency.All[CurrencyId.Unknown], 2.0 * simulationAccountOptions.InitialCashValue, this.Now));
                this.ProcessEventArgs(new AccountUpdateEventArgs(this, ErrorCode.NoError, "", account, AccountItemType.All[AccountItemTypeId.CashValue], iTrading.Core.Kernel.Currency.All[CurrencyId.Unknown], simulationAccountOptions.InitialCashValue, this.Now));
                this.ProcessEventArgs(new AccountUpdateEventArgs(this, ErrorCode.NoError, "", account, AccountItemType.All[AccountItemTypeId.ExcessEquity], iTrading.Core.Kernel.Currency.All[CurrencyId.Unknown], 2.0 * simulationAccountOptions.InitialCashValue, this.Now));
                this.ProcessEventArgs(new AccountUpdateEventArgs(this, ErrorCode.NoError, "", account, AccountItemType.All[AccountItemTypeId.InitialMargin], iTrading.Core.Kernel.Currency.All[CurrencyId.Unknown], 0.0, this.Now));
            }
            try
            {
                foreach (XmlNode node2 in document["TradeMagic"][str]["Positions"])
                {
                    iTrading.Core.Kernel.Symbol symbolByName = this.GetSymbolByName(node2["Symbol"].InnerText);
                    if (symbolByName == null)
                    {
                        this.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this, ErrorCode.Panic, "", "Unable to restore position for unknown symbol '" + node2["Symbol"].InnerText + "'"));
                    }
                    else
                    {
                        this.ProcessEventArgs(new PositionUpdateEventArgs(this, ErrorCode.NoError, "", Operation.Insert, account, symbolByName, MarketPosition.All[(MarketPositionId) Convert.ToInt32(node2["MarketPosition"].InnerText, CultureInfo.InvariantCulture)], Convert.ToInt32(node2["Quantity"].InnerText, CultureInfo.InvariantCulture), iTrading.Core.Kernel.Currency.All[(CurrencyId) Convert.ToInt32(node2["Currency"].InnerText, CultureInfo.InvariantCulture)], Convert.ToDouble(node2["AvgPrice"].InnerText, CultureInfo.InvariantCulture)));
                    }
                }
            }
            catch
            {
            }
            if (account.Executions.Count == 0)
            {
                foreach (Position position in account.Positions)
                {
                    account.simulationOverNight.Add(new Execution("", account, position.Symbol, this.Now, position.MarketPosition, "", position.Quantity, position.AvgPrice));
                }
            }
            else
            {
                try
                {
                    foreach (XmlNode node3 in document["TradeMagic"][str]["OverNight"])
                    {
                        iTrading.Core.Kernel.Symbol symbol = this.GetSymbolByName(node3["Symbol"].InnerText);
                        if (symbol == null)
                        {
                            this.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this, ErrorCode.Panic, "", "Unable to restore overnight position for unknown symbol '" + node3["Symbol"].InnerText + "'"));
                        }
                        else
                        {
                            account.simulationOverNight.Add(new Execution("", account, symbol, this.Now, MarketPosition.All[(MarketPositionId) Convert.ToInt32(node3["MarketPosition"].InnerText, CultureInfo.InvariantCulture)], "", Convert.ToInt32(node3["Quantity"].InnerText, CultureInfo.InvariantCulture), Convert.ToDouble(node3["AvgPrice"].InnerText, CultureInfo.InvariantCulture)));
                        }
                    }
                }
                catch
                {
                }
            }
            account.SimulationAccountUpdate();
            lock (account.Orders)
            {
                foreach (Order order in account.Orders)
                {
                    order.simulator = new Simulator(order, SimulationSymbolOptions.Restore(order.CustomText));
                }
                return account;
            }
            return account;
        }

        /// <summary>
        /// For internal use only. Symbols created by calling this function will not work properly. 
        /// Use <see cref="M:iTrading.Core.Kernel.Connection.GetSymbol(System.String,System.DateTime,iTrading.Core.Kernel.SymbolType,iTrading.Core.Kernel.Exchange,System.Double,iTrading.Core.Kernel.RightId,iTrading.Core.Kernel.LookupPolicyId)" /> instead.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="expiry"></param>
        /// <param name="symbolType"></param>
        /// <param name="exchange"></param>
        /// <param name="currency"></param>
        /// <param name="adapterLink"></param>
        /// <param name="tickSize"></param>
        /// <param name="strikePrice"></param>
        /// <param name="rightId"></param>
        /// <param name="companyName"></param>
        /// <param name="pointValue"></param>
        /// <param name="mapId"></param>
        /// <param name="exchanges"></param>
        /// <param name="splits"></param>
        /// <param name="dividends"></param>
        /// <returns></returns>
        public iTrading.Core.Kernel.Symbol CreateSymbol(string name, DateTime expiry, SymbolType symbolType, Exchange exchange, double strikePrice, RightId rightId, iTrading.Core.Kernel.Currency currency, double tickSize, double pointValue, string companyName, object adapterLink, int mapId, ExchangeDictionary exchanges, SplitDictionary splits, DividendDictionary dividends)
        {
            if (this.ConnectionStatusId == iTrading.Core.Kernel.ConnectionStatusId.Disconnected)
            {
                return null;
            }
            if (this.Exchanges[exchange.Id] == null)
            {
                throw new TMException(ErrorCode.Panic, "Cbi.Connection.CreateSymbol: Exchange '" + exchange.Name + "' not supported by this broker / data provider");
            }
            exchange = this.Exchanges[exchange.Id];
            if (this.SymbolTypes[symbolType.Id] == null)
            {
                throw new TMException(ErrorCode.Panic, "Cbi.Connection.CreateSymbol: Symbol type '" + symbolType.Name + "' not supported by this broker / data provider");
            }
            symbolType = this.SymbolTypes[symbolType.Id];
            lock (this.symbols)
            {
                iTrading.Core.Kernel.Symbol symbol = new iTrading.Core.Kernel.Symbol(this, name, expiry, symbolType, exchange, strikePrice, rightId, currency, tickSize, pointValue, companyName, exchanges, splits, dividends);
                iTrading.Core.Kernel.Symbol symbol2 = this.symbols[symbol];
                if (symbol2 != null)
                {
                    if (Globals.TraceSwitch.SymbolLookup)
                    {
                        Trace.WriteLine("(" + this.IdPlus + ") Cbi.Connection.CreateSymbol1: " + symbol2.FullName);
                    }
                    symbol2.Update(symbol);
                    if (((this.FeatureTypes[FeatureTypeId.SynchronousSymbolLookup] == null) && (this.symbol2Lookup != null)) && (this.symbol2Lookup == symbol2))
                    {
                        lock (this.Symbols)
                        {
                            Monitor.Pulse(this.Symbols);
                        }
                    }
                    return symbol2;
                }
                symbol = new iTrading.Core.Kernel.Symbol(this, name, expiry, symbolType, exchange, strikePrice, rightId, currency, tickSize, pointValue, companyName, exchanges, splits, dividends);
                if (Globals.TraceSwitch.SymbolLookup)
                {
                    Trace.WriteLine("(" + this.IdPlus + ") Cbi.Connection.CreateSymbol2: " + symbol.FullName);
                }
                symbol.AdapterLink = adapterLink;
                symbol.MapId = mapId;
                this.symbols.Add(symbol);
                this.ProcessEventArgs(new SymbolEventArgs(this, ErrorCode.NoError, "", symbol));
                return symbol;
            }
        }

        /// <summary>
        /// The actual license.
        /// </summary>
        public LicenseType GetLicense(string moduleName)
        {
            return Globals.GetLicense(moduleName);
        }

        internal void GetQuotesNow()
        {
            this.quotes.Request(((QuotesRequest) this.quotesRequests[0]).adapterQuotes);
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        public Request GetRequest(int requestId)
        {
            return this.streamingRequests[requestId];
        }

        /// <summary>
        /// Retrieves a symbol object from the provider's dictionary.
        /// Please note: There exists only one instance for every symbol.
        /// </summary>
        /// <param name="name">Symbol name.</param>
        /// <param name="expiry">Expiry date. Used for futures only.</param>
        /// <param name="symbolType">Type of symbol.</param>
        /// <param name="exchange">Identifies the exchange.</param>
        /// <param name="strikePrice">Strike price. For options only. Set to 0 for any other symbol type.</param>
        /// <param name="rightId">Options right. For options only. Set to <see cref="F:iTrading.Core.Kernel.RightId.Unknown" /> for any other symbol type.</param>
        /// <param name="lookupPolicyId"></param>
        /// <returns>Symbol in the directory. NULL, if provider does not support this symbol or if connection is not
        /// established.</returns>
        public iTrading.Core.Kernel.Symbol GetSymbol(string name, DateTime expiry, SymbolType symbolType, Exchange exchange, double strikePrice, RightId rightId, LookupPolicyId lookupPolicyId)
        {
            if (Globals.TraceSwitch.SymbolLookup)
            {
                Trace.WriteLine(string.Concat(new object[] { "(", this.IdPlus, ") Cbi.Connection.GetSymbol0: name='", (name == null) ? "null" : name, "' expiry=", expiry.ToString("yyyy-MM-dd"), " symbolType=", (symbolType == null) ? "null" : symbolType.Name, " exchange=", (exchange == null) ? "null" : exchange.Name, " lookupPolicyId=", lookupPolicyId }));
            }
            if (this.ConnectionStatusId == iTrading.Core.Kernel.ConnectionStatusId.Disconnected)
            {
                return null;
            }
            if ((symbolType.Id != SymbolTypeId.Future) && (symbolType.Id != SymbolTypeId.Option))
            {
                expiry = Globals.MaxDate;
            }
            if (symbolType.Id != SymbolTypeId.Option)
            {
                rightId = RightId.Unknown;
                strikePrice = 0.0;
            }
            else
            {
                if (rightId == RightId.Unknown)
                {
                    throw new TMException(ErrorCode.Panic, "Cbi.Connection.GetSymbol: RightId '" + rightId + "' not supported for options");
                }
                if (strikePrice <= 0.0)
                {
                    throw new TMException(ErrorCode.Panic, "Cbi.Connection.GetSymbol: Strike prices smaller/equal 0 are not supported for options (=" + strikePrice + ")");
                }
            }
            if (this.Options.Mode.Id == ModeTypeId.Simulation)
            {
                if (lookupPolicyId == LookupPolicyId.ProviderOnly)
                {
                    return null;
                }
                if (lookupPolicyId == LookupPolicyId.RepositoryAndProvider)
                {
                    lookupPolicyId = LookupPolicyId.RepositoryOnly;
                }
            }
            if (this.Exchanges[exchange.Id] == null)
            {
                throw new TMException(ErrorCode.Panic, "Cbi.Connection.GetSymbol: Exchange '" + exchange.Name + "' not supported by this broker / data provider");
            }
            exchange = this.Exchanges[exchange.Id];
            if (this.SymbolTypes[symbolType.Id] == null)
            {
                throw new TMException(ErrorCode.Panic, "Cbi.Connection.GetSymbol: Symbol type '" + symbolType.Name + "' not supported by this broker / data provider");
            }
            symbolType = this.SymbolTypes[symbolType.Id];
            ExchangeDictionary exchanges = new ExchangeDictionary();
            exchanges.Add(exchange);
            iTrading.Core.Kernel.Symbol symbol = null;
            iTrading.Core.Kernel.Symbol symbol2 = new iTrading.Core.Kernel.Symbol(this, name, expiry, symbolType, exchange, strikePrice, rightId, null, 0.01, 1.0, "", exchanges, null, null);
            if (Globals.TraceSwitch.SymbolLookup)
            {
                Trace.WriteLine("(" + this.IdPlus + ") Cbi.Connection.GetSymbol1: " + symbol2.FullName);
            }
            symbol = this.symbols[symbol2];
            if (symbol != null)
            {
                if (Globals.TraceSwitch.SymbolLookup)
                {
                    Trace.WriteLine("(" + this.IdPlus + ") Cbi.Connection.GetSymbol2: " + symbol.FullName);
                }
                return symbol;
            }
            lock (this.symbols)
            {
                symbol = this.symbols[symbol2];
                if (symbol != null)
                {
                    if (Globals.TraceSwitch.SymbolLookup)
                    {
                        Trace.WriteLine("(" + this.IdPlus + ") Cbi.Connection.GetSymbol3: " + symbol.FullName);
                    }
                    return symbol;
                }
                if (lookupPolicyId == LookupPolicyId.CacheOnly)
                {
                    return null;
                }
                if (lookupPolicyId == LookupPolicyId.NoCheck)
                {
                    if (symbolType.Id != SymbolTypeId.Stock)
                    {
                        return null;
                    }
                    if (symbol == null)
                    {
                        this.symbols.Add(symbol2);
                        symbol = this.symbols[symbol2];
                        Trace.Assert(symbol != null, "Cbi.Connection.GetSymbol1: " + symbol2.ToString());
                        this.ProcessEventArgs(new SymbolEventArgs(this, ErrorCode.NoError, "", symbol));
                    }
                    return symbol;
                }
                if (!this.Options.RunAtServer && ((lookupPolicyId == LookupPolicyId.RepositoryAndProvider) || (lookupPolicyId == LookupPolicyId.RepositoryOnly)))
                {
                    SymbolCollection symbols = null;
                    symbols = Globals.DB.Select(name, null, null, expiry, symbolType, exchange, strikePrice, rightId, null);
                    Trace.Assert(symbols.Count <= 1, string.Concat(new object[] { "Cbi.Connection.GetSymbol2: count=", symbols.Count, " name='", name, "' expiry=", expiry, " symbolType=", symbolType.Name, " exchange=", exchange.Name, " strikePrice=", strikePrice, " rightId=", rightId }));
                    if (symbols.Count == 1)
                    {
                        ExchangeDictionary dictionary2 = new ExchangeDictionary();
                        iTrading.Core.Kernel.Symbol symbol3 = symbols[0];
                        foreach (Exchange exchange2 in symbol3.Exchanges.Values)
                        {
                            if (this.Exchanges[exchange2.Id] != null)
                            {
                                dictionary2.Add(this.Exchanges[exchange2.Id]);
                            }
                        }
                        iTrading.Core.Kernel.Symbol symbol4 = new iTrading.Core.Kernel.Symbol(this, symbol3.Name, symbol3.Expiry, this.SymbolTypes[symbol3.SymbolType.Id], exchange, symbol3.StrikePrice, symbol3.Right.Id, symbol3.Currency, symbol3.TickSize, symbol3.PointValue, symbol3.CompanyName, dictionary2, symbol3.Splits, symbol3.Dividends);
                        symbol4.Commission = symbol3.Commission;
                        symbol4.CustomText = symbol3.CustomText;
                        symbol4.Margin = symbol3.Margin;
                        symbol4.RolloverMonths = symbol3.RolloverMonths;
                        symbol4.Slippage = symbol3.Slippage;
                        symbol4.Url = symbol3.Url;
                        foreach (ProviderType type in ProviderType.All.Values)
                        {
                            symbol4.SetProviderName(type.Id, symbol3.GetProviderName(type.Id));
                        }
                        this.symbols.Add(symbol4);
                        symbol = this.symbols[symbol4];
                        Trace.Assert(symbol != null, "Cbi.Connection.GetSymbol3: " + symbol4);
                        this.ProcessEventArgs(new SymbolEventArgs(this, ErrorCode.NoError, "", symbol));
                        return symbol;
                    }
                    if (lookupPolicyId == LookupPolicyId.RepositoryOnly)
                    {
                        return null;
                    }
                }
            }
            lock (this.symbolsSyncObject)
            {
                this.symbol2Lookup = new iTrading.Core.Kernel.Symbol(this, name, expiry, symbolType, exchange, strikePrice, rightId, null, 0.01, 1.0, "", null, null, null);
                if (Globals.TraceSwitch.SymbolLookup)
                {
                    Trace.WriteLine("(" + this.IdPlus + ") Cbi.Connection.GetSymbol4: " + this.symbol2Lookup.FullName);
                }
                symbol = this.symbols[this.symbol2Lookup];
                if (symbol != null)
                {
                    if (Globals.TraceSwitch.SymbolLookup)
                    {
                        Trace.WriteLine("(" + this.IdPlus + ") Cbi.Connection.GetSymbol5: " + symbol.FullName);
                    }
                    return symbol;
                }
                this.SynchronizeInvoke.Invoke(new MethodInvoker(this.GetSymbolNow), null);
                iTrading.Core.Kernel.Symbol symbol5 = this.symbol2Lookup;
                this.symbol2Lookup = null;
                if ((((lookupPolicyId != LookupPolicyId.ProviderOnly) && (symbol5 != null)) && !this.Options.RunAtServer) && ((this.ConnectionStatusId == iTrading.Core.Kernel.ConnectionStatusId.Connected) || (this.ConnectionStatusId == iTrading.Core.Kernel.ConnectionStatusId.ConnectionLost)))
                {
                    Globals.DB.Update(symbol5, true);
                }
                return symbol5;
            }
        }

        /// <summary>
        /// Get a symbol macthing a name created by <see cref="M:iTrading.Core.Kernel.Symbol.ToString" />.
        /// </summary>
        /// <param name="symbolName"></param>
        /// <returns></returns>
        public iTrading.Core.Kernel.Symbol GetSymbolByName(string symbolName)
        {
            Exchange exchange = null;
            string str = "";
            DateTime maxDate = Globals.MaxDate;
            string[] strArray = symbolName.Split(new char[] { ' ' });
            string name = strArray[0];
            RightId unknown = RightId.Unknown;
            double strikePrice = 0.0;
            SymbolType symbolType = null;
            if (symbolName[0] == '^')
            {
                if (strArray.Length != 2)
                {
                    this.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this, ErrorCode.InvalidSymbolName, "", "Symbol name '" + symbolName + "' does not refer to a valid TradeMagic symbol"));
                    return null;
                }
                exchange = this.Exchanges.Find(strArray[1]);
                str = strArray[1];
                name = strArray[0].Substring(1);
                symbolType = this.SymbolTypes[SymbolTypeId.Index];
            }
            else
            {
                if (symbolName[0] == '+')
                {
                    if (strArray.Length != 5)
                    {
                        this.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this, ErrorCode.InvalidSymbolName, "", "Symbol name '" + symbolName + "' does not refer to a valid TradeMagic symbol"));
                        return null;
                    }
                    exchange = this.Exchanges.Find(strArray[4]);
                    str = strArray[4];
                    name = strArray[0].Substring(1);
                    symbolType = this.SymbolTypes[SymbolTypeId.Option];
                    if (strArray[2] == "C")
                    {
                        unknown = RightId.Call;
                    }
                    else if (strArray[2] == "P")
                    {
                        unknown = RightId.Put;
                    }
                    else
                    {
                        this.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this, ErrorCode.InvalidSymbolName, "", "Symbol name '" + symbolName + "' does not contain a valid right component"));
                        return null;
                    }
                    try
                    {
                        strikePrice = Convert.ToDouble(strArray[3], cultureInfo);
                    }
                    catch
                    {
                        this.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this, ErrorCode.InvalidSymbolName, "", "Symbol name '" + symbolName + "' does not contain a strike price component"));
                        return null;
                    }
                    if (strArray[1] == "##/##")
                    {
                        maxDate = Globals.ContinousContractExpiry;
                        goto Label_0493;
                    }
                    string[] strArray2 = strArray[1].Split(new char[] { '-' });
                    if (strArray2.Length != 2)
                    {
                        this.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this, ErrorCode.InvalidSymbolName, "", "Symbol name '" + symbolName + "' does not contain a valid expiry component"));
                        return null;
                    }
                    try
                    {
                        if (strArray2[0].Length != 2)
                        {
                            this.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this, ErrorCode.InvalidSymbolName, "", "Symbol name '" + symbolName + "' does not contain a valid expiry component"));
                            return null;
                        }
                        int month = Convert.ToInt32(strArray2[0]);
                        int year = 0;
                        if (strArray2[1].Length == 4)
                        {
                            year = Convert.ToInt32(strArray2[1]);
                        }
                        else if (strArray2[1].Length == 2)
                        {
                            year = 0x7d0 + Convert.ToInt32(strArray2[1]);
                        }
                        else
                        {
                            this.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this, ErrorCode.InvalidSymbolName, "", "Symbol name '" + symbolName + "' does not contain a valid expiry component"));
                            return null;
                        }
                        maxDate = new DateTime(year, month, 1);
                        goto Label_0493;
                    }
                    catch (Exception)
                    {
                        this.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this, ErrorCode.InvalidSymbolName, "", "Symbol name '" + symbolName + "' does not contain a valid expiry component"));
                        return null;
                    }
                }
                if (strArray.Length == 2)
                {
                    exchange = this.Exchanges.Find(strArray[1]);
                    str = strArray[1];
                    symbolType = this.SymbolTypes[SymbolTypeId.Stock];
                }
                else
                {
                    if (strArray.Length == 3)
                    {
                        exchange = this.Exchanges.Find(strArray[2]);
                        str = strArray[2];
                        symbolType = this.SymbolTypes[SymbolTypeId.Future];
                        if (strArray[1] == "##/##")
                        {
                            maxDate = Globals.ContinousContractExpiry;
                            goto Label_0493;
                        }
                        string[] strArray3 = strArray[1].Split(new char[] { '-' });
                        if (strArray3.Length != 2)
                        {
                            this.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this, ErrorCode.InvalidSymbolName, "", "Symbol name '" + symbolName + "' does not contain a valid expiry component"));
                            return null;
                        }
                        try
                        {
                            if (strArray3[0].Length != 2)
                            {
                                this.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this, ErrorCode.InvalidSymbolName, "", "Symbol name '" + symbolName + "' does not contain a valid expiry component"));
                                return null;
                            }
                            int num4 = Convert.ToInt32(strArray3[0]);
                            int num5 = 0;
                            if (strArray3[1].Length == 4)
                            {
                                num5 = Convert.ToInt32(strArray3[1]);
                            }
                            else if (strArray3[1].Length == 2)
                            {
                                num5 = 0x7d0 + Convert.ToInt32(strArray3[1]);
                            }
                            else
                            {
                                this.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this, ErrorCode.InvalidSymbolName, "", "Symbol name '" + symbolName + "' does not contain a valid expiry component"));
                                return null;
                            }
                            maxDate = new DateTime(num5, num4, 1);
                            goto Label_0493;
                        }
                        catch (Exception)
                        {
                            this.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this, ErrorCode.InvalidSymbolName, "", "Symbol name '" + symbolName + "' does not contain a valid expiry component"));
                            return null;
                        }
                    }
                    this.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this, ErrorCode.InvalidSymbolName, "", "Symbol name '" + symbolName + "' does not refer to a valid TradeMagic symbol"));
                    return null;
                }
            }
        Label_0493:
            if (exchange == null)
            {
                this.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this, ErrorCode.InvalidSymbolName, "", "Exchange '" + str + "' is not supported by this broker / data provider"));
                return null;
            }
            if (symbolType == null)
            {
                this.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this, ErrorCode.InvalidSymbolName, "", "Symbol type for symbol '" + symbolName + "' is not supported by this broker / data provider"));
                return null;
            }
            iTrading.Core.Kernel.Symbol symbol = this.GetSymbol(name, maxDate, symbolType, exchange, strikePrice, unknown, LookupPolicyId.RepositoryAndProvider);
            if (symbol == null)
            {
                this.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this, ErrorCode.InvalidSymbolName, "", "Symbol '" + symbolName + "' is not supported by this broker / data provider"));
            }
            return symbol;
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="expiry"></param>
        /// <param name="symbolType"></param>
        /// <param name="exchange"></param>
        /// <param name="strikePrice"></param>
        /// <param name="rightId"></param>
        /// <param name="lookupPolicyId"></param>
        /// <returns></returns>
        public iTrading.Core.Kernel.Symbol GetSymbolByProviderName(string providerName, DateTime expiry, SymbolType symbolType, Exchange exchange, double strikePrice, RightId rightId, LookupPolicyId lookupPolicyId)
        {
            if ((symbolType.Id != SymbolTypeId.Future) && (symbolType.Id != SymbolTypeId.Option))
            {
                expiry = Globals.MaxDate;
            }
            if (symbolType.Id != SymbolTypeId.Option)
            {
                rightId = RightId.Unknown;
                strikePrice = 0.0;
            }
            if ((lookupPolicyId != LookupPolicyId.CacheOnly) && (lookupPolicyId != LookupPolicyId.RepositoryOnly))
            {
                throw new TMException(ErrorCode.Panic, "Cbi.Connection.GetSymbolByProviderName: lookup policy (" + lookupPolicyId + ") not supported");
            }
            if (this.Options.Mode.Id == ModeTypeId.Simulation)
            {
                if (lookupPolicyId == LookupPolicyId.ProviderOnly)
                {
                    return null;
                }
                if (lookupPolicyId == LookupPolicyId.RepositoryAndProvider)
                {
                    lookupPolicyId = LookupPolicyId.RepositoryOnly;
                }
            }
            if (this.Exchanges[exchange.Id] == null)
            {
                throw new TMException(ErrorCode.Panic, "Cbi.Connection.GetSymbolByBrokerName: Exchange '" + exchange.Name + "' not supported by this broker / data provider");
            }
            exchange = this.Exchanges[exchange.Id];
            if (this.SymbolTypes[symbolType.Id] == null)
            {
                throw new TMException(ErrorCode.Panic, "Cbi.Connection.GetSymbolByBrokerName: Symbol type '" + symbolType.Name + "' not supported by this broker / data provider");
            }
            symbolType = this.SymbolTypes[symbolType.Id];
            iTrading.Core.Kernel.Symbol symbol = null;
            lock (this.Symbols)
            {
                foreach (iTrading.Core.Kernel.Symbol symbol2 in this.Symbols.Values)
                {
                    if ((((symbol2.GetProviderName(this.Options.Provider.Id) == providerName) && (symbol2.SymbolType.Id == symbolType.Id)) && ((symbol2.Exchange.Id == exchange.Id) && (symbol2.Expiry == expiry))) && ((symbol2.StrikePrice == strikePrice) && (symbol2.Right.Id == rightId)))
                    {
                        symbol = symbol2;
                        break;
                    }
                }
                if (symbol != null)
                {
                    if (this.FeatureTypes[FeatureTypeId.SynchronousSymbolLookup] == null)
                    {
                        lock (this.Symbols)
                        {
                            Monitor.Pulse(this.Symbols);
                        }
                    }
                    return symbol;
                }
                if (lookupPolicyId == LookupPolicyId.CacheOnly)
                {
                    return null;
                }
                if (this.Options.RunAtServer)
                {
                    return null;
                }
                SymbolCollection symbols = null;
                symbols = Globals.DB.Select(providerName, this.Options.Provider, null, expiry, symbolType, exchange, strikePrice, rightId, null);
                if (symbols.Count >= 1)
                {
                    ExchangeDictionary exchanges = new ExchangeDictionary();
                    iTrading.Core.Kernel.Symbol symbol3 = symbols[0];
                    foreach (Exchange exchange2 in symbol3.Exchanges.Values)
                    {
                        if (this.Exchanges[exchange2.Id] != null)
                        {
                            exchanges.Add(this.Exchanges[exchange2.Id]);
                        }
                    }
                    iTrading.Core.Kernel.Symbol symbol4 = new iTrading.Core.Kernel.Symbol(this, symbol3.Name, symbol3.Expiry, this.SymbolTypes[symbol3.SymbolType.Id], exchange, symbol3.StrikePrice, symbol3.Right.Id, symbol3.Currency, symbol3.TickSize, symbol3.PointValue, symbol3.CompanyName, exchanges, symbol3.Splits, symbol3.Dividends);
                    symbol4.Commission = symbol3.Commission;
                    symbol4.CustomText = symbol3.CustomText;
                    symbol4.Margin = symbol3.Margin;
                    symbol4.RolloverMonths = symbol3.RolloverMonths;
                    symbol4.Slippage = symbol3.Slippage;
                    symbol4.Url = symbol3.Url;
                    foreach (ProviderType type in ProviderType.All.Values)
                    {
                        symbol4.SetProviderName(type.Id, symbol3.GetProviderName(type.Id));
                    }
                    this.symbols.Add(symbol4);
                    symbol = this.symbols[symbol4];
                    Trace.Assert(symbol != null, "Cbi.Conection.GetSymbolByProviderName:" + symbol4);
                    this.ProcessEventArgs(new SymbolEventArgs(this, ErrorCode.NoError, "", symbol));
                    return symbol;
                }
            }
            return null;
        }

        private void GetSymbolNow()
        {
            try
            {
                lock (this.Symbols)
                {
                    if (Globals.TraceSwitch.SymbolLookup)
                    {
                        Trace.WriteLine("(" + this.IdPlus + ") Cbi.Connection.GetSymbolNow1: " + this.symbol2Lookup.FullName);
                    }
                    this.adapter.SymbolLookup(this.symbol2Lookup);
                    if (this.FeatureTypes[FeatureTypeId.SynchronousSymbolLookup] == null)
                    {
                        Monitor.Wait(this.Symbols);
                    }
                    this.symbol2Lookup = this.symbols[this.symbol2Lookup];
                    if (Globals.TraceSwitch.SymbolLookup)
                    {
                        Trace.WriteLine("(" + this.IdPlus + ") Cbi.Connection.GetSymbolNow2: " + ((this.symbol2Lookup == null) ? "null" : this.symbol2Lookup.FullName));
                    }
                }
            }
            catch (Exception exception)
            {
                this.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this, ErrorCode.Panic, "", "Cbi.Connection.GetSymbolNow: exception caught: " + exception.Message));
            }
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        /// <param name="eventArgs"></param>
        public void ProcessEventArgs(ITradingBaseEventArgs eventArgs)
        {
            AccountEventArgs args;
            BarUpdateEventArgs args2;
            ConnectionStatusEventArgs args3;
            ExecutionUpdateEventArgs args4;
            MarketDataEventArgs args5;
            MarketDepthEventArgs args6;
            AccountItemTypeDictionary dictionary;
            ActionTypeDictionary dictionary2;
            CurrencyDictionary dictionary3;
            ExchangeDictionary dictionary4;
            FeatureTypeDictionary dictionary5;
            MarketDataTypeDictionary dictionary6;
            MarketPositionDictionary dictionary7;
            NewsEventArgsCollection argss;
            NewsItemTypeDictionary dictionary8;
            OrderStateDictionary dictionary9;
            OrderTypeDictionary dictionary10;
            SymbolTypeDictionary dictionary11;
            TimeInForceDictionary dictionary12;
            AccountCollection accounts;
            PositionCollection positions;
            switch (eventArgs.ClassId)
            {
                case iTrading.Core.Kernel.ClassId.AccountEventArgs:
                    goto Label_0340;

                case iTrading.Core.Kernel.ClassId.AccountItemTypeEventArgs:
                    lock ((dictionary = this.AccountItemTypes))
                    {
                        this.AccountItemTypes.Add(((AccountItemTypeEventArgs) eventArgs).AccountItemType);
                        goto Label_16E0;
                    }
                    break;

                case iTrading.Core.Kernel.ClassId.ActionTypeEventArgs:
                    break;

                case iTrading.Core.Kernel.ClassId.ConnectionStatusEventArgs:
                    Connection connection;
                    args3 = (ConnectionStatusEventArgs) eventArgs;
                    if (Globals.TraceSwitch.Connect)
                    {
                        Trace.WriteLine(string.Concat(new object[] { "(", this.IdPlus, ") Cbi.Connection.ProcessEventArgs.ConnectionStatusEventArgs ", args3.ConnectionStatusId.ToString(), " ", args3.SecondaryConnectionStatusId.ToString(), " ", args3.Error, " ", args3.NativeError }));
                    }
                    if (args3.ConnectionStatusId != iTrading.Core.Kernel.ConnectionStatusId.Disconnected)
                    {
                        if ((args3.ConnectionStatusId == iTrading.Core.Kernel.ConnectionStatusId.Connected) && (args3.OldConnectionStatusId == iTrading.Core.Kernel.ConnectionStatusId.Connecting))
                        {
                            this.customText = args3.CustomText;
                            if (args3.ClientId != 0)
                            {
                                this.clientId = args3.ClientId;
                            }
                            if (!this.options.RunAtServer)
                            {
                                lock ((accounts = this.Accounts))
                                {
                                    foreach (Account account2 in this.Accounts)
                                    {
                                        try
                                        {
                                            Globals.DB.Recover(account2);
                                            continue;
                                        }
                                        catch (Exception exception)
                                        {
                                            Trace.WriteLine("WARNING: Cbi.Connection.ProcessEventArgs.ConnectionStatusEventArgs.Recover: " + exception.Message);
                                            try
                                            {
                                                this.connectionStatusId = args3.ConnectionStatusId;
                                                this.disconnectedSent = true;
                                                this.Close();
                                            }
                                            catch (Exception)
                                            {
                                            }
                                            args3 = new ConnectionStatusEventArgs(this, ErrorCode.Panic, "Cbi.Connection.ProcessEventArgs: " + exception.Message, iTrading.Core.Kernel.ConnectionStatusId.Disconnected, iTrading.Core.Kernel.ConnectionStatusId.Disconnected, args3.ClientId, args3.CustomText);
                                            try
                                            {
                                                this.SynchronizeInvoke.AsyncInvoke(this.eventArgsHandler, new object[] { args3 });
                                            }
                                            catch (Exception)
                                            {
                                            }
                                            return;
                                        }
                                    }
                                }
                            }
                            lock ((connection = this))
                            {
                                if (this.timer != null)
                                {
                                    this.timer.Stop();
                                }
                                this.timer = new System.Timers.Timer((double) this.options.TimerIntervalMilliSeconds);
                                this.timer.AutoReset = true;
                                this.timer.Elapsed += new ElapsedEventHandler(this.TimerElapsed);
                                this.timer.Start();
                                if ((this.featureTypes[FeatureTypeId.ClockSynchronization] == null) || (this.Options.Mode.Id == ModeTypeId.Simulation))
                                {
                                    this.nowMilliSeconds = (-this.options.TimerDelayHours * 0xe10) * 0x3e8;
                                }
                            }
                        }
                        goto Label_0E14;
                    }
                    if (!this.Options.RunAtServer)
                    {
                        Globals.DB.Disconnect();
                    }
                    lock ((connection = this))
                    {
                        if (this.timer != null)
                        {
                            this.timer.Stop();
                            this.timer = null;
                        }
                        if (this.recorderTimer != null)
                        {
                            this.recorderTimer.Stop();
                            this.recorderTimer = null;
                        }
                        if (this.replayTimer != null)
                        {
                            this.replayTimer.Stop();
                            this.replayTimer = null;
                        }
                    }
                    lock ((accounts = this.Accounts))
                    {
                        foreach (Account account in this.Accounts)
                        {
                            lock (account.Executions)
                            {
                                account.Executions.Clear();
                            }
                            lock (account.Orders)
                            {
                                account.Orders.Clear();
                            }
                            lock ((positions = account.Positions))
                            {
                                account.Positions.Clear();
                                continue;
                            }
                        }
                        this.Accounts.Clear();
                    }
                    lock ((dictionary2 = this.ActionTypes))
                    {
                        this.ActionTypes.Clear();
                    }
                    lock ((dictionary = this.AccountItemTypes))
                    {
                        this.AccountItemTypes.Clear();
                    }
                    lock ((dictionary3 = this.Currencies))
                    {
                        this.Currencies.Clear();
                    }
                    lock ((dictionary4 = this.Exchanges))
                    {
                        this.Exchanges.Clear();
                    }
                    lock ((dictionary5 = this.FeatureTypes))
                    {
                        this.FeatureTypes.Clear();
                    }
                    lock ((dictionary6 = this.MarketDataTypes))
                    {
                        this.MarketDataTypes.Clear();
                    }
                    lock ((dictionary7 = this.MarketPositions))
                    {
                        this.MarketPositions.Clear();
                    }
                    lock ((argss = this.News))
                    {
                        this.News.Clear();
                    }
                    lock ((dictionary8 = this.NewsItemTypes))
                    {
                        this.NewsItemTypes.Clear();
                    }
                    lock ((dictionary9 = this.OrderStates))
                    {
                        this.OrderStates.Clear();
                    }
                    lock ((dictionary10 = this.OrderTypes))
                    {
                        this.OrderTypes.Clear();
                    }
                    lock ((dictionary11 = this.SymbolTypes))
                    {
                        this.SymbolTypes.Clear();
                    }
                    lock ((dictionary12 = this.TimeInForces))
                    {
                        this.TimeInForces.Clear();
                    }
                    lock (this.Symbols)
                    {
                        foreach (iTrading.Core.Kernel.Symbol symbol in this.Symbols.Values)
                        {
                            symbol.MarketData.CancelRecorder();
                        }
                    }
                    try
                    {
                        this.Symbols.Clear();
                    }
                    catch (InvalidOperationException)
                    {
                    }
                    if (!this.disconnectedSent)
                    {
                        goto Label_0E14;
                    }
                    return;

                case iTrading.Core.Kernel.ClassId.CurrencyEventArgs:
                    goto Label_012A;

                case iTrading.Core.Kernel.ClassId.ExchangeEventArgs:
                    goto Label_015B;

                case iTrading.Core.Kernel.ClassId.ExecutionUpdateEventArgs:
                    if (this.ConnectionStatusId != iTrading.Core.Kernel.ConnectionStatusId.Disconnected)
                    {
                        args4 = (ExecutionUpdateEventArgs) eventArgs;
                        if (Globals.TraceSwitch.Order)
                        {
                            Trace.WriteLine(string.Concat(new object[] { "(", this.IdPlus, ") Cbi.Connection.ProcessEventArgs.ExecutionEventArgs: id='", args4.ExecutionId, "' symbol='", args4.Symbol.FullName, "' orderid='", args4.OrderId, "' #=", args4.Quantity, " price=", args4.AvgPrice }));
                        }
                        switch (args4.Operation)
                        {
                            case Operation.Insert:
                                args4.Account.Executions.Add(args4.execution = new Execution(args4.ExecutionId, args4.Account, args4.Symbol, args4.Time, args4.MarketPosition, args4.OrderId, args4.Quantity, args4.AvgPrice));
                                goto Label_0FD3;

                            case Operation.Update:
                                args4.execution = args4.Account.Executions.FindByExecId(args4.ExecutionId);
                                Trace.Assert(args4.execution != null, "Cbi.Connection.ProcessEventArgs: executionId='" + args4.ExecutionId + "'");
                                args4.execution.Update(args4);
                                goto Label_0FD3;

                            case Operation.Delete:
                                Trace.Assert(false, "Cbi.Connection.ProcessEventArgs: illegal 'delete' operation for Cbi.Execution");
                                goto Label_0FD3;
                        }
                        goto Label_0FD3;
                    }
                    return;

                case iTrading.Core.Kernel.ClassId.FeatureTypeEventArgs:
                    goto Label_018C;

                case iTrading.Core.Kernel.ClassId.MarketDataEventArgs:
                    if (this.ConnectionStatusId != iTrading.Core.Kernel.ConnectionStatusId.Disconnected)
                    {
                        args5 = (MarketDataEventArgs) eventArgs;
                        switch (args5.MarketDataType.Id)
                        {
                            case MarketDataTypeId.Ask:
                                if (args5.Price != 0.0)
                                {
                                    args5.Symbol.MarketData.lastAsk = args5;
                                    goto Label_119D;
                                }
                                return;

                            case MarketDataTypeId.Bid:
                                if (args5.Price != 0.0)
                                {
                                    args5.Symbol.MarketData.lastBid = args5;
                                    goto Label_119D;
                                }
                                return;

                            case MarketDataTypeId.Last:
                                if (args5.Price != 0.0)
                                {
                                    args5.Symbol.MarketData.lastLast = args5;
                                    goto Label_119D;
                                }
                                return;

                            case MarketDataTypeId.DailyHigh:
                                if (args5.Price != 0.0)
                                {
                                    args5.Symbol.MarketData.lastDailyHigh = args5;
                                    goto Label_119D;
                                }
                                return;

                            case MarketDataTypeId.DailyLow:
                                if (args5.Price != 0.0)
                                {
                                    args5.Symbol.MarketData.lastDailyLow = args5;
                                    goto Label_119D;
                                }
                                return;

                            case MarketDataTypeId.DailyVolume:
                                if (args5.Volume != 0)
                                {
                                    args5.Symbol.MarketData.lastDailyVolume = args5;
                                    goto Label_119D;
                                }
                                return;

                            case MarketDataTypeId.LastClose:
                                if (args5.Price != 0.0)
                                {
                                    args5.Symbol.MarketData.lastLastClose = args5;
                                    goto Label_119D;
                                }
                                return;

                            case MarketDataTypeId.Opening:
                                if (args5.Price != 0.0)
                                {
                                    args5.Symbol.MarketData.lastOpening = args5;
                                    goto Label_119D;
                                }
                                return;
                        }
                        goto Label_119D;
                    }
                    return;

                case iTrading.Core.Kernel.ClassId.MarketDataTypeEventArgs:
                    goto Label_01BD;

                case iTrading.Core.Kernel.ClassId.MarketDepthEventArgs:
                    if (this.ConnectionStatusId != iTrading.Core.Kernel.ConnectionStatusId.Disconnected)
                    {
                        args6 = (MarketDepthEventArgs) eventArgs;
                        MarketDepthRowCollection rows = (args6.MarketDataType.Id == MarketDataTypeId.Bid) ? args6.Symbol.MarketDepth.Bid : args6.Symbol.MarketDepth.Ask;
                        lock (args6.Symbol.MarketDepth)
                        {
                            switch (args6.Operation)
                            {
                                case Operation.Insert:
                                    if (args6.Position < rows.Count)
                                    {
                                        goto Label_1287;
                                    }
                                    rows.Add(new MarketDepthRow(args6.MarketMaker, args6.Price, args6.Volume, args6.Time));
                                    goto Label_130A;

                                case Operation.Update:
                                {
                                    MarketDepthRow row = rows[args6.Position];
                                    row.marketMaker = args6.MarketMaker;
                                    row.price = args6.Price;
                                    row.time = args6.Time;
                                    row.volume = args6.Volume;
                                    goto Label_130A;
                                }
                                case Operation.Delete:
                                    if (args6.Position < rows.Count)
                                    {
                                        break;
                                    }
                                    return;

                                default:
                                    goto Label_130A;
                            }
                            rows.RemoveAt(args6.Position);
                            goto Label_130A;
                        Label_1287:
                            rows.Insert(args6.Position, new MarketDepthRow(args6.MarketMaker, args6.Price, args6.Volume, args6.Time));
                        }
                        goto Label_130A;
                    }
                    return;

                case iTrading.Core.Kernel.ClassId.MarketPositionEventArgs:
                    goto Label_01EE;

                case iTrading.Core.Kernel.ClassId.NewsEventArgs:
                    goto Label_021F;

                case iTrading.Core.Kernel.ClassId.NewsItemTypeEventArgs:
                    goto Label_024B;

                case iTrading.Core.Kernel.ClassId.OrderStateEventArgs:
                    goto Label_027C;

                case iTrading.Core.Kernel.ClassId.OrderStatusEventArgs:
                    if (this.ConnectionStatusId != iTrading.Core.Kernel.ConnectionStatusId.Disconnected)
                    {
                        OrderStatusEventArgs args7 = (OrderStatusEventArgs) eventArgs;
                        if (Globals.TraceSwitch.Order)
                        {
                            Trace.WriteLine("(" + this.IdPlus + ") Cbi.Connection.ProcessEventArgs.OrderStatusEventArgs: " + args7.ToString());
                        }
                        int num4 = args7.Filled - args7.order.Filled;
                        args7.Order.Update(args7);
                        if (!this.Options.RunAtServer && ((this.ConnectionStatusId == iTrading.Core.Kernel.ConnectionStatusId.Connected) || (this.ConnectionStatusId == iTrading.Core.Kernel.ConnectionStatusId.ConnectionLost)))
                        {
                            Globals.DB.Insert(args7, false);
                        }
                        if (((((this.connectionStatusId == iTrading.Core.Kernel.ConnectionStatusId.Connected) && !this.options.RunAtServer) && (args7.Order.OcaGroup.Length > 0)) && (args7.Order.Account.IsSimulation || (this.FeatureTypes[FeatureTypeId.NativeOcaOrders] == null))) && (((args7.OrderState.Id == OrderStateId.Rejected) || (args7.OrderState.Id == OrderStateId.PendingCancel)) || (((args7.OrderState.Id == OrderStateId.Cancelled) || (args7.OrderState.Id == OrderStateId.Filled)) || (args7.OrderState.Id == OrderStateId.PartFilled))))
                        {
                            this.SynchronizeInvoke.Invoke(new WorkerArgs2(Order.HandleOcaOrdersNow), new object[] { args7, num4 });
                        }
                        if (this.connectionStatusId == iTrading.Core.Kernel.ConnectionStatusId.Connecting)
                        {
                            return;
                        }
                        goto Label_16E0;
                    }
                    return;

                case iTrading.Core.Kernel.ClassId.OrderTypeEventArgs:
                    goto Label_02AD;

                case iTrading.Core.Kernel.ClassId.PositionUpdateEventArgs:
                    if (this.ConnectionStatusId != iTrading.Core.Kernel.ConnectionStatusId.Disconnected)
                    {
                        PositionUpdateEventArgs args8 = (PositionUpdateEventArgs) eventArgs;
                        if (Globals.TraceSwitch.Order)
                        {
                            Trace.WriteLine(string.Concat(new object[] { "(", this.IdPlus, ") Cbi.Connection.ProcessEventArgs.PositionUpdateEventArgs: symbol='", args8.Symbol.FullName, "' #=", args8.Quantity, " price=", args8.AvgPrice }));
                        }
                        lock ((positions = args8.Account.Positions))
                        {
                            if (args8.Operation == Operation.Insert)
                            {
                                args8.Account.Positions.Add(args8.position = new Position(args8.Account, args8.Symbol, args8.MarketPosition, args8.Quantity, args8.Currency, args8.AvgPrice));
                            }
                            else
                            {
                                foreach (Position position in args8.Account.Positions)
                                {
                                    if (args8.Symbol.IsEqual(position.Symbol))
                                    {
                                        args8.position = position;
                                    }
                                }
                                if (args8.Operation == Operation.Update)
                                {
                                    args8.Position.Update(args8);
                                }
                                else
                                {
                                    args8.Account.Positions.Remove(args8.position);
                                }
                            }
                        }
                        if (this.connectionStatusId == iTrading.Core.Kernel.ConnectionStatusId.Connecting)
                        {
                            return;
                        }
                        goto Label_16E0;
                    }
                    return;

                case iTrading.Core.Kernel.ClassId.SymbolEventArgs:
                    if (this.ConnectionStatusId != iTrading.Core.Kernel.ConnectionStatusId.Disconnected)
                    {
                        ((SymbolEventArgs) eventArgs).Pulse();
                        if (this.connectionStatusId == iTrading.Core.Kernel.ConnectionStatusId.Connecting)
                        {
                            return;
                        }
                        goto Label_16E0;
                    }
                    return;

                case iTrading.Core.Kernel.ClassId.SymbolTypeEventArgs:
                    goto Label_02DE;

                case iTrading.Core.Kernel.ClassId.TimeInForceEventArgs:
                    goto Label_030F;

                case iTrading.Core.Kernel.ClassId.TimerEventArgs:
                {
                    TimerEventArgs args9 = (TimerEventArgs) eventArgs;
                    if (!args9.sync)
                    {
                        goto Label_16E0;
                    }
                    if ((this.nowDelayMilliSeconds >= 0.0) && (args9.Time <= this.Now))
                    {
                        this.nowDelayMilliSeconds = this.Now.Subtract(args9.Time).TotalMilliseconds;
                        return;
                    }
                    this.nowMilliSeconds = -DateTime.Now.Subtract(args9.Time).TotalMilliseconds;
                    this.nowDelayMilliSeconds = 0.0;
                    return;
                }
                case iTrading.Core.Kernel.ClassId.BarUpdateEventArgs:
                    goto Label_03C7;

                default:
                    goto Label_16E0;
            }
            lock ((dictionary2 = this.ActionTypes))
            {
                this.ActionTypes.Add(((ActionTypeEventArgs) eventArgs).ActionType);
                goto Label_16E0;
            }
        Label_012A:
            Monitor.Enter(dictionary3 = this.Currencies);
            try
            {
                this.Currencies.Add(((CurrencyEventArgs) eventArgs).Currency);
                goto Label_16E0;
            }
            finally
            {
                Monitor.Exit(dictionary3);
            }
        Label_015B:
            Monitor.Enter(dictionary4 = this.Exchanges);
            try
            {
                this.Exchanges.Add(((ExchangeEventArgs) eventArgs).Exchange);
                goto Label_16E0;
            }
            finally
            {
                Monitor.Exit(dictionary4);
            }
        Label_018C:
            Monitor.Enter(dictionary5 = this.FeatureTypes);
            try
            {
                this.FeatureTypes.Add(((FeatureTypeEventArgs) eventArgs).FeatureType);
                goto Label_16E0;
            }
            finally
            {
                Monitor.Exit(dictionary5);
            }
        Label_01BD:
            Monitor.Enter(dictionary6 = this.MarketDataTypes);
            try
            {
                this.MarketDataTypes.Add(((MarketDataTypeEventArgs) eventArgs).MarketDataType);
                goto Label_16E0;
            }
            finally
            {
                Monitor.Exit(dictionary6);
            }
        Label_01EE:
            Monitor.Enter(dictionary7 = this.MarketPositions);
            try
            {
                this.MarketPositions.Add(((MarketPositionEventArgs) eventArgs).MarketPosition);
                goto Label_16E0;
            }
            finally
            {
                Monitor.Exit(dictionary7);
            }
        Label_021F:
            Monitor.Enter(argss = this.News);
            try
            {
                this.News.Add((NewsEventArgs) eventArgs);
                goto Label_16E0;
            }
            finally
            {
                Monitor.Exit(argss);
            }
        Label_024B:
            Monitor.Enter(dictionary8 = this.NewsItemTypes);
            try
            {
                this.NewsItemTypes.Add(((NewsItemTypeEventArgs) eventArgs).NewsItemType);
                goto Label_16E0;
            }
            finally
            {
                Monitor.Exit(dictionary8);
            }
        Label_027C:
            Monitor.Enter(dictionary9 = this.OrderStates);
            try
            {
                this.OrderStates.Add(((OrderStateEventArgs) eventArgs).OrderState);
                goto Label_16E0;
            }
            finally
            {
                Monitor.Exit(dictionary9);
            }
        Label_02AD:
            Monitor.Enter(dictionary10 = this.OrderTypes);
            try
            {
                this.OrderTypes.Add(((OrderTypeEventArgs) eventArgs).OrderType);
                goto Label_16E0;
            }
            finally
            {
                Monitor.Exit(dictionary10);
            }
        Label_02DE:
            Monitor.Enter(dictionary11 = this.SymbolTypes);
            try
            {
                this.SymbolTypes.Add(((SymbolTypeEventArgs) eventArgs).SymbolType);
                goto Label_16E0;
            }
            finally
            {
                Monitor.Exit(dictionary11);
            }
        Label_030F:
            Monitor.Enter(dictionary12 = this.TimeInForces);
            try
            {
                this.TimeInForces.Add(((TimeInForceEventArgs) eventArgs).TimeInForce);
                goto Label_16E0;
            }
            finally
            {
                Monitor.Exit(dictionary12);
            }
        Label_0340:
            args = (AccountEventArgs) eventArgs;
            if (Globals.TraceSwitch.Order)
            {
                Trace.WriteLine("(" + this.IdPlus + ") Cbi.Connection.ProcessEventArgs.AccountEventArgs: name='" + args.Account.Name + "'");
            }
            lock ((accounts = this.Accounts))
            {
                this.Accounts.Add(args.Account);
                goto Label_16E0;
            }
        Label_03C7:
            args2 = (BarUpdateEventArgs) eventArgs;
            if (Globals.TraceSwitch.Quotes)
            {
                Trace.WriteLine(string.Concat(new object[] { "(", this.IdPlus, ") Cbi.Connection.ProcessEventArgs.BarUpdateEventArgs: symbol='", args2.Quotes.Symbol.FullName, "' first=", args2.First, " last=", args2.Last, " period=", args2.Quotes.Period.ToString() }));
            }
            Quotes quotes = null;
            lock (this.quotesRequests)
            {
                QuotesRequest request = null;
                foreach (QuotesRequest request2 in base.connection.quotesRequests)
                {
                    if (request2.adapterQuotes == args2.Quotes)
                    {
                        request = request2;
                    }
                }
                if (request != null)
                {
                    if ((args2.Error == ErrorCode.NoError) && (args2.Quotes.Bars.Count > 0))
                    {
                        quotes = args2.Quotes;
                        request.quotes.Bars.barsAdded = 0;
                        int num = request.quotes.Bars.Count - 1;
                        while (num >= 0)
                        {
                            if (request.quotes.Bars[num].Time < args2.Quotes.Bars[0].Time)
                            {
                                break;
                            }
                            num--;
                        }
                        while (((request.quotes.Bars.Count - 1) >= 0) && (request.quotes.Bars.Count > (num + 1)))
                        {
                            request.quotes.Bars.bars.RemoveAt(request.quotes.Bars.Count - 1);
                        }
                        bool flag = (args2.Quotes.Period.Id == PeriodTypeId.Day) && (this.featureTypes[FeatureTypeId.SplitsAdjustedDaily] != null);
                        if (flag)
                        {
                            quotes = new Quotes(args2.Quotes.Symbol, args2.Quotes.From, args2.Quotes.To, args2.Quotes.Period, args2.Quotes.SplitAdjusted);
                        }
                        foreach (Bar bar in args2.Quotes.Bars)
                        {
                            double splitFactor = request.quotes.Symbol.Splits.GetSplitFactor(bar.Time.Date);
                            double num3 = (request.quotes.SplitAdjusted && !flag) ? splitFactor : 1.0;
                            request.quotes.Bars.Add(bar.Open / num3, bar.High / num3, bar.Low / num3, bar.Close / num3, bar.Time, bar.Volume, false);
                            if (flag)
                            {
                                quotes.Bars.Add(bar.Open * splitFactor, bar.High * splitFactor, bar.Low * splitFactor, bar.Close * splitFactor, bar.Time, bar.Volume, false);
                            }
                        }
                    }
                    this.quotesRequests.Remove(request);
                    if (this.quotesRequests.Count > 0)
                    {
                        this.SynchronizeInvoke.AsyncInvoke(new MethodInvoker(this.GetQuotesNow), null);
                    }
                    eventArgs = new BarUpdateEventArgs(this, args2.Error, args2.NativeError, Operation.Insert, request.quotes, 0, request.quotes.Bars.Count - 1);
                }
            }
            if ((quotes != null) && (args2.Error == ErrorCode.NoError))
            {
                Globals.DB.Update(quotes, false);
            }
            goto Label_16E0;
        Label_0E14:
            this.connectionStatusId = args3.ConnectionStatusId;
            this.secondaryConnectionStatusId = args3.SecondaryConnectionStatusId;
            goto Label_16E0;
        Label_0FD3:
            if (!this.Options.RunAtServer && ((this.ConnectionStatusId == iTrading.Core.Kernel.ConnectionStatusId.Connected) || (this.ConnectionStatusId == iTrading.Core.Kernel.ConnectionStatusId.ConnectionLost)))
            {
                Globals.DB.Update(args4.Execution, false);
            }
            if (this.connectionStatusId != iTrading.Core.Kernel.ConnectionStatusId.Connecting)
            {
                goto Label_16E0;
            }
            return;
        Label_119D:
            if (!args5.initOnly)
            {
                goto Label_16E0;
            }
            return;
        Label_130A:
            if (args6.initOnly)
            {
                return;
            }
        Label_16E0:
            try
            {
                this.SynchronizeInvoke.AsyncInvoke(this.eventArgsHandler, new object[] { eventArgs });
            }
            catch (InvalidOperationException)
            {
            }
        }

        internal void ProcessEventArgsInThreadContext(ITradingBaseEventArgs eventArgs)
        {
            try
            {
                eventArgs.Process();
            }
            catch (Exception exception)
            {
                Trace.WriteLine("Cbi.Connection.ProcessEventArgsInThreadContext, exception caught: " + exception.Message);
            }
        }

        internal void RecorderTimerElapsed(object sender, ElapsedEventArgs e)
        {
            this.SynchronizeInvoke.Invoke(new MethodInvoker(this.RecorderTimerElapsedNow), null);
        }

        internal void RecorderTimerElapsedNow()
        {
            ArrayList list = new ArrayList();
            ArrayList list2 = new ArrayList();
            lock (this.MarketDataStreams)
            {
                foreach (MarketData data in this.MarketDataStreams)
                {
                    if (data.marketDataBuf.Recording && (DateTime.Now.Subtract(data.marketDataBuf.TimeLastWrite).TotalMinutes >= (0.95 * this.Options.RecorderWriteMinutes)))
                    {
                        list.Add(data.marketDataBuf);
                    }
                }
            }
            lock (this.MarketDepthStreams)
            {
                foreach (MarketDepth depth in this.MarketDepthStreams)
                {
                    if (depth.marketDepthBuf.Recording && (DateTime.Now.Subtract(depth.marketDepthBuf.TimeLastWrite).TotalMinutes >= (0.95 * this.Options.RecorderWriteMinutes)))
                    {
                        list2.Add(depth.marketDepthBuf);
                    }
                }
            }
            foreach (MarketDataBuf buf in list)
            {
                buf.Write();
            }
            foreach (MarketDepthBuf buf2 in list2)
            {
                buf2.Write();
            }
        }

        /// <summary>
        /// Serialize the current object.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public override void Serialize(Bytes bytes, int version)
        {
            base.Serialize(bytes, version);
            bytes.Write(iTrading.Core.Kernel.ConnectionStatus.All[this.ConnectionStatusId]);
            bytes.WriteSerializable(this.options);
            if (version >= 2)
            {
                bytes.Write(this.simulationSpeed);
            }
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (this.Options.Mode.Id != ModeTypeId.Simulation)
            {
                if ((this.featureTypes[FeatureTypeId.ClockSynchronization] == null) || (this.nowDelayMilliSeconds < 0.0))
                {
                    this.nowMilliSeconds = (-base.connection.Options.TimerDelayHours * 0xe10) * 0x3e8;
                }
                else
                {
                    if (this.nowDelayMilliSeconds > this.options.TimerIntervalMilliSeconds)
                    {
                        this.nowDelayMilliSeconds -= this.options.TimerIntervalMilliSeconds;
                        return;
                    }
                    if (this.nowDelayMilliSeconds > 0.0)
                    {
                        this.nowMilliSeconds -= this.options.TimerIntervalMilliSeconds - this.nowDelayMilliSeconds;
                        this.nowDelayMilliSeconds = 0.0;
                    }
                }
            }
            else if (this.SimulationSpeed != 0.0)
            {
                this.nowMilliSeconds += this.timer.Interval * this.simulationSpeed;
            }
            this.ProcessEventArgs(new TimerEventArgs(this, ErrorCode.NoError, "", this.Now, false));
        }

        /// <summary>
        /// Get a collection of all available account item types. Please note: This collection is empty, when
        /// <see cref="P:iTrading.Core.Kernel.Connection.ConnectionStatusId" /> = <see cref="F:iTrading.Core.Kernel.ConnectionStatusId.Disconnected" />
        /// </summary>
        public AccountItemTypeDictionary AccountItemTypes
        {
            get
            {
                return this.accountItemTypes;
            }
        }

        /// <summary>
        /// Retrieves a container holding all accounts for this connection. Usually there will not be more than one
        /// account. But some brokers support multiple accounts managed by a master connection.
        /// Please note: This collection is empty, when <see cref="P:iTrading.Core.Kernel.Connection.ConnectionStatusId" /> = <see cref="F:iTrading.Core.Kernel.ConnectionStatusId.Disconnected" />
        /// </summary>
        public AccountCollection Accounts
        {
            get
            {
                return this.accounts;
            }
        }

        /// <summary>
        /// Get a collection of all available order action types. Please note: This collection is empty, when
        /// <see cref="P:iTrading.Core.Kernel.Connection.ConnectionStatusId" /> = <see cref="F:iTrading.Core.Kernel.ConnectionStatusId.Disconnected" />
        /// </summary>
        public ActionTypeDictionary ActionTypes
        {
            get
            {
                return this.actionTypes;
            }
        }

        /// <summary>
        /// Gets <see cref="P:iTrading.Core.Kernel.Connection.ClassId" /> of current object.
        /// </summary>
        public override iTrading.Core.Kernel.ClassId ClassId
        {
            get
            {
                return iTrading.Core.Kernel.ClassId.Connection;
            }
        }

        /// <summary>
        /// Identifies the client connection at the TradeMagic server.
        /// 0, when broker adapter is executed locally.
        /// </summary>
        public int ClientId
        {
            get
            {
                return this.clientId;
            }
        }

        /// <summary>
        /// Indicates the current connection status id.
        /// </summary>
        public iTrading.Core.Kernel.ConnectionStatusId ConnectionStatusId
        {
            get
            {
                return this.connectionStatusId;
            }
        }

        /// <summary>
        /// Get a collection of all available currencies. Please note: This collection is empty, when
        /// <see cref="P:iTrading.Core.Kernel.Connection.ConnectionStatusId" /> = <see cref="F:iTrading.Core.Kernel.ConnectionStatusId.Disconnected" />
        /// </summary>
        public CurrencyDictionary Currencies
        {
            get
            {
                return this.currencies;
            }
        }

        /// <summary>
        /// Custom text.
        /// </summary>
        public string CustomText
        {
            get
            {
                if (this.customText != null)
                {
                    return this.customText;
                }
                return "";
            }
        }

        /// <summary>
        /// Get a collection of all available exchanges. Please note: This collection is empty, when
        /// <see cref="P:iTrading.Core.Kernel.Connection.ConnectionStatusId" /> = <see cref="F:iTrading.Core.Kernel.ConnectionStatusId.Disconnected" />
        /// </summary>
        public ExchangeDictionary Exchanges
        {
            get
            {
                return this.exchanges;
            }
        }

        /// <summary>
        /// Get a collection of all available feature types. Please note: This collection is empty, when
        /// <see cref="P:iTrading.Core.Kernel.Connection.ConnectionStatusId" /> = <see cref="F:iTrading.Core.Kernel.ConnectionStatusId.Disconnected" />
        /// </summary>
        public FeatureTypeDictionary FeatureTypes
        {
            get
            {
                return this.featureTypes;
            }
        }

        /// <summary>
        /// Get <see cref="P:iTrading.Core.Kernel.Request.Id" /> plus the current thread's name as string.
        /// </summary>
        public string IdPlus
        {
            get
            {
                string name = Thread.CurrentThread.Name;
                if ((name == null) || (name.Length == 0))
                {
                    name = AppDomain.GetCurrentThreadId().ToString();
                }
                return (base.Id.ToString() + "/" + name);
            }
        }

        /// <summary>
        /// Get a collection of all symbols with running <see cref="T:iTrading.Core.Kernel.MarketData" /> streams. 
        /// Please note: This collection is empty, when <see cref="P:iTrading.Core.Kernel.Connection.ConnectionStatusId" /> = <see cref="F:iTrading.Core.Kernel.ConnectionStatusId.Disconnected" />
        /// </summary>
        public MarketDataCollection MarketDataStreams
        {
            get
            {
                return this.marketDataStreams;
            }
        }

        /// <summary>
        /// Get a collection of all available order market data types. Please note: This collection is empty, when
        /// <see cref="P:iTrading.Core.Kernel.Connection.ConnectionStatusId" /> = <see cref="F:iTrading.Core.Kernel.ConnectionStatusId.Disconnected" />
        /// </summary>
        public MarketDataTypeDictionary MarketDataTypes
        {
            get
            {
                return this.marketDataTypes;
            }
        }

        /// <summary>
        /// Get a collection of all symbols with running <see cref="T:iTrading.Core.Kernel.MarketDepth" /> streams. 
        /// Please note: This collection is empty, when <see cref="P:iTrading.Core.Kernel.Connection.ConnectionStatusId" /> = <see cref="F:iTrading.Core.Kernel.ConnectionStatusId.Disconnected" />
        /// </summary>
        public MarketDepthCollection MarketDepthStreams
        {
            get
            {
                return this.marketDepthStreams;
            }
        }

        /// <summary>
        /// Get a collection of all available market position types. Please note: This collection is empty, when
        /// <see cref="P:iTrading.Core.Kernel.Connection.ConnectionStatusId" /> = <see cref="F:iTrading.Core.Kernel.ConnectionStatusId.Disconnected" />
        /// </summary>
        public MarketPositionDictionary MarketPositions
        {
            get
            {
                return this.marketPositions;
            }
        }

        /// <summary>
        /// Get a collection of all available market position types. Please note: This collection is empty, when
        /// <see cref="P:iTrading.Core.Kernel.Connection.ConnectionStatusId" /> = <see cref="F:iTrading.Core.Kernel.ConnectionStatusId.Disconnected" />
        /// </summary>
        public NewsEventArgsCollection News
        {
            get
            {
                return this.news;
            }
        }

        /// <summary>
        /// Get a collection of all available news item types. Please note: This collection is empty, when
        /// <see cref="P:iTrading.Core.Kernel.Connection.ConnectionStatusId" /> = <see cref="F:iTrading.Core.Kernel.ConnectionStatusId.Disconnected" />
        /// </summary>
        public NewsItemTypeDictionary NewsItemTypes
        {
            get
            {
                return this.newsItemTypes;
            }
        }

        /// <summary>
        /// Get/set current time, synchronized with the provider's "heartbeat" (if provided).
        /// Plase note: Setting the time only is effective in simulation mode.
        /// (<see cref="P:iTrading.Core.Kernel.Connection.Options" />.Mode.Id == <see cref="F:iTrading.Core.Kernel.ModeTypeId.Simulation" />).
        /// </summary>
        public DateTime Now
        {
            get
            {
                return DateTime.Now.AddMilliseconds(this.nowMilliSeconds);
            }
            set
            {
                if (this.Options.Mode.Id == ModeTypeId.Simulation)
                {
                    this.nowMilliSeconds = DateTime.Now.Subtract(value).TotalMilliseconds;
                    if (this.replayTimer != null)
                    {
                        this.replayTimer.Enabled = false;
                    }
                    lock (this.Symbols)
                    {
                        foreach (iTrading.Core.Kernel.Symbol symbol in this.symbols.Values)
                        {
                            if (symbol.MarketData.IsRunning)
                            {
                                symbol.MarketData.Cancel();
                                symbol.MarketData.Start();
                            }
                            if (symbol.MarketDepth.IsRunning)
                            {
                                symbol.MarketDepth.Cancel();
                                symbol.MarketDepth.Start();
                            }
                        }
                    }
                    if (this.replayTimer != null)
                    {
                        this.replayTimer.Enabled = true;
                    }
                }
            }
        }

        /// <summary>
        /// Options of current connection.
        /// </summary>
        public OptionsBase Options
        {
            get
            {
                return this.options;
            }
        }

        /// <summary>
        /// Get a collection of all available order status instances. Please note: This collection is empty, when
        /// <see cref="P:iTrading.Core.Kernel.Connection.ConnectionStatusId" /> = <see cref="F:iTrading.Core.Kernel.ConnectionStatusId.Disconnected" />
        /// </summary>
        public OrderStateDictionary OrderStates
        {
            get
            {
                return this.orderStates;
            }
        }

        /// <summary>
        /// Get a collection of all available order types. Please note: This collection is empty, when
        /// <see cref="P:iTrading.Core.Kernel.Connection.ConnectionStatusId" /> = <see cref="F:iTrading.Core.Kernel.ConnectionStatusId.Disconnected" />
        /// </summary>
        public OrderTypeDictionary OrderTypes
        {
            get
            {
                return this.orderTypes;
            }
        }

        /// <summary>
        /// Indicates the current connection status id of the secondary server(s), e.g. price feed server.
        /// </summary>
        public iTrading.Core.Kernel.ConnectionStatusId SecondaryConnectionStatusId
        {
            get
            {
                return this.secondaryConnectionStatusId;
            }
        }

        /// <summary>
        /// Get/set name of simulation account. Set this property before calling <see cref="M:iTrading.Core.Kernel.Connection.Connect(iTrading.Core.Kernel.OptionsBase)" />.
        /// Default: "TM Simulation".
        /// </summary>
        public string SimulationAccountName
        {
            get
            {
                return this.simulationAccountName;
            }
            set
            {
                this.simulationAccountName = value;
            }
        }

        /// <summary>
        /// Get/set speed of replayed data for simulation. 
        /// Effective when <see cref="P:iTrading.Core.Kernel.Connection.Options" />.Mode.Id == <see cref="F:iTrading.Core.Kernel.ModeTypeId.Simulation" />.
        /// Only values greater/equal zero are accepted
        /// 1 = realtime speed, 0 = stop.
        /// </summary>
        public double SimulationSpeed
        {
            get
            {
                return this.simulationSpeed;
            }
            set
            {
                if (value < 0.0)
                {
                    throw new TMException(ErrorCode.UnableToPerformAction, "SimulationSpeed values must be greater/equal 0");
                }
                this.simulationSpeed = value;
            }
        }

        /// <summary>
        /// Get a collection of all symbols. Please note: This collection is empty, when
        /// <see cref="P:iTrading.Core.Kernel.Connection.ConnectionStatusId" /> = <see cref="F:iTrading.Core.Kernel.ConnectionStatusId.Disconnected" />
        /// </summary>
        public SymbolDictionary Symbols
        {
            get
            {
                return this.symbols;
            }
        }

        /// <summary>
        /// Get a collection of all available symbol types. Please note: This collection is empty, when
        /// <see cref="P:iTrading.Core.Kernel.Connection.ConnectionStatusId" /> = <see cref="F:iTrading.Core.Kernel.ConnectionStatusId.Disconnected" />
        /// </summary>
        public SymbolTypeDictionary SymbolTypes
        {
            get
            {
                return this.symbolTypes;
            }
        }

        /// <summary>
        /// Gets or sets the object used to marshal the event calls.
        /// The incoming events are thrown in 
        /// the thread context of the thread which called <see cref="M:iTrading.Core.Kernel.Connection.Connect(iTrading.Core.Kernel.OptionsBase)" />. 
        /// The default behaviour may be overriden on settings this property.
        /// </summary>
        public iTrading.Core.Kernel.SynchronizeInvoke SynchronizeInvoke
        {
            get
            {
                return this.synchronizeInvoke;
            }
            set
            {
                this.synchronizeInvoke = value;
            }
        }

        /// <summary>
        /// Get a collection of all available time in force values. Please note: This collection is empty, when
        /// <see cref="P:iTrading.Core.Kernel.Connection.ConnectionStatusId" /> = <see cref="F:iTrading.Core.Kernel.ConnectionStatusId.Disconnected" />
        /// </summary>
        public TimeInForceDictionary TimeInForces
        {
            get
            {
                return this.timeInForce;
            }
        }

        /// <summary>
        /// Version number.
        /// </summary>
        public override int Version
        {
            get
            {
                return 2;
            }
        }

        internal delegate void ProcessEventArgsHandler(ITradingBaseEventArgs eventArgs);

        internal delegate void WorkerArgs1(object arg1);

        internal delegate void WorkerArgs2(object arg1, object arg2);
    }
}

