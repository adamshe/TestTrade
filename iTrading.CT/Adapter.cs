using iTrading.Core.Data;

namespace iTrading.CT
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Text;
    using System.Threading;
   using iTrading.Core.Kernel;
    using iTrading.Core.Data;

    internal class Adapter : IAdapter, IMarketData, IMarketDepth, IQuotes
    {
        internal ASCIIEncoding asciiEncoding = new ASCIIEncoding();
        private Callback callback = null;
        internal Connection connection = null;
        private Api.DataCallback dataCallback;
        private int dataDllHandle = 0;
        private Api.ErrorCallback errorCallback;
        private Exec exec = null;
        private Api.OnConnect onConnect;
        private Api.OnConnectFail onConnectFail;
        private Api.OnDisconnect onDisconnect;
        private Api.OnServerConnect onServerConnect;
        private Api.OnServerDisconnect onServerDisconnect;
        private Api.OnUserLogoff onUserLogoff;
        private Api.OnUserLogon onUserLogon;
        private Api.OnUserLogonFail onUserLogonFail;
        private int orderDllHandle = 0;
        internal Quotes quotes2Lookup = null;
        private Api.StatusCallback statusCallback;
        internal Hashtable symbol2MarketData = new Hashtable();
        internal Hashtable symbol2MarketDepth = new Hashtable();

        internal unsafe Adapter(Connection connection)
        {
            this.callback = new Callback(this);
            this.connection = connection;
            this.exec = new Exec(this);
            this.dataCallback = new Api.DataCallback(this.callback.DataCallback);
            this.errorCallback = new Api.ErrorCallback(this.callback.ErrorCallback);
            this.statusCallback = new Api.StatusCallback(this.callback.StatusCallback);
            this.onConnect = new Api.OnConnect(this.exec.OnConnect);
            this.onConnectFail = new Api.OnConnectFail(this.exec.OnConnectFail);
            this.onDisconnect = new Api.OnDisconnect(this.exec.OnDisconnect);
            this.onServerConnect = new Api.OnServerConnect(this.exec.OnServerConnect);
            this.onServerDisconnect = new Api.OnServerDisconnect(this.exec.OnServerDisconnect);
            this.onUserLogoff = new Api.OnUserLogoff(this.exec.OnUserLogoff);
            this.onUserLogon = new Api.OnUserLogon(this.exec.OnUserLogon);
            this.onUserLogonFail = new Api.OnUserLogonFail(this.exec.OnUserLogonFail);
        }

        private void Cleanup()
        {
            if (this.dataDllHandle != 0)
            {
                Api.FreeLibrary(this.dataDllHandle);
                this.dataDllHandle = 0;
            }
            if (this.orderDllHandle != 0)
            {
                Api.FreeLibrary(this.orderDllHandle);
                this.orderDllHandle = 0;
            }
        }

        public void Clear()
        {
        }

        public void Connect()
        {
            if (Globals.TraceSwitch.Connect)
            {
                Trace.WriteLine(string.Concat(new object[] { "(", this.connection.IdPlus, ") CT.Adapter.ConnectNow:  IPAddress='", this.Options.IPAddress, "' IPAddressAlternate=", this.Options.IPAddressAlternate, "' NewsDaysBack=", this.Options.NewsDaysBack }));
            }
            if (this.connection.Options.Mode.Id == ModeTypeId.Simulation)
            {
                new Thread(new ThreadStart(this.ConnectSimulation)).Start();
            }
            else
            {
                try
                {
                    string str = Globals.InstallDir + @"\bin\CT";
                    this.dataDllHandle = Api.LoadLibrary(str + @"\MidAPI.dll");
                    if (this.dataDllHandle != 0)
                    {
                        try
                        {
                            string str2 = Globals.InstallDir + @"\bin\CT";
                            this.orderDllHandle = Api.LoadLibrary(str2 + @"\CXPDLL.dll");
                            if (this.orderDllHandle != 0)
                            {
                                RetCode success = RetCode.Success;
                                if ((success = Api.InitMidApi(this.Options.AuthorizationCode, this.statusCallback, this.dataCallback, this.errorCallback)) == RetCode.Success)
                                {
                                    if ((success = Api.SetStringMidApi(SetString.User, this.Options.User)) != RetCode.Success)
                                    {
                                        this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, ErrorCode.Panic, "CT.Adapter.Connect.SetStringMidApi.User: error " + success, ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                                    }
                                    else if ((success = Api.SetStringMidApi(SetString.Password, this.Options.Password)) != RetCode.Success)
                                    {
                                        this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, ErrorCode.Panic, "CT.Adapter.Connect.SetStringMidApi.Password: error " + success, ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                                    }
                                    else
                                    {
                                        Api.Initialize();
                                        Api.SetConnectCallback(this.onConnect);
                                        Api.SetConnectFailCallback(this.onConnectFail);
                                        Api.SetDisconnectCallback(this.onDisconnect);
                                        Api.SetServerConnectCallback(this.onServerConnect);
                                        Api.SetServerDisconnectCallback(this.onServerDisconnect);
                                        Api.SetUserLogoffCallback(this.onUserLogoff);
                                        Api.SetUserLogonCallback(this.onUserLogon);
                                        Api.SetUserLogonFailCallback(this.onUserLogonFail);
                                        if ((success = Api.ConnectMidApi(false, (this.Options.Mode.Id == ModeTypeId.Live) ? 'L' : 'D')) != RetCode.Success)
                                        {
                                            this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, ErrorCode.Panic, "CT.Adapter.Connect.SetStringMidApi.ConnectMidApi: error " + success, ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                                        }
                                    }
                                }
                                else
                                {
                                    this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, ErrorCode.Panic, "CT.Adapter.Connect.InitMidApi: error " + success, ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                                }
                            }
                            else
                            {
                                this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, ErrorCode.Panic, "Failed to load '" + str2 + @"\CXPDLL.dll'", ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                            }
                        }
                        catch (Exception exception2)
                        {
                            this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, ErrorCode.Panic, "Failed to locate 'MidAPI.dll': " + exception2.Message, ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                        }
                    }
                    else
                    {
                        this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, ErrorCode.Panic, "Failed to load '" + str + @"\MidAPI.dll'", ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                    }
                }
                catch (Exception exception)
                {
                    this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, ErrorCode.Panic, "Failed to locate 'MidAPI.dll': " + exception.Message, ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                }
            }
        }

        private void ConnectSimulation()
        {
            this.Init();
            this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, ErrorCode.NoError, "", ConnectionStatusId.Connected, ConnectionStatusId.Connected, 0, ""));
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
            return this.connection.CreateSymbol(symbol, maxDate, symbolType, this.connection.Exchanges[ExchangeId.Default], 0.0, RightId.Unknown, this.connection.Currencies[CurrencyId.Unknown], 0.01, 0.0, "", null, 0, null, null, null);
        }

        public void Disconnect()
        {
            if (Globals.TraceSwitch.Connect)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") CT.Adapter.Disconnect");
            }
            if (this.connection.Options.Mode.Id == ModeTypeId.Simulation)
            {
                new Thread(new ThreadStart(this.DisconnectNow)).Start();
            }
            else
            {
                Api.UnadviseDataMidApi("$TIME", Feed.Level1);
                Api.UnadviseDataMidApi("!NEWSHOT", Feed.Level1);
                Api.TerminateMidApi();
                this.Cleanup();
                new Thread(new ThreadStart(this.DisconnectNow)).Start();
            }
        }

        private void DisconnectNow()
        {
            this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, ErrorCode.NoError, "", ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
        }

        internal void Init()
        {
            this.connection.ProcessEventArgs(new CurrencyEventArgs(this.connection, ErrorCode.NoError, "", CurrencyId.Unknown, ""));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Default, ""));
            this.connection.ProcessEventArgs(new FeatureTypeEventArgs(this.connection, ErrorCode.NoError, "", FeatureTypeId.ClockSynchronization, 0.0));
            this.connection.ProcessEventArgs(new FeatureTypeEventArgs(this.connection, ErrorCode.NoError, "", FeatureTypeId.MarketData, 0.0));
            this.connection.ProcessEventArgs(new FeatureTypeEventArgs(this.connection, ErrorCode.NoError, "", FeatureTypeId.MarketDepth, 0.0));
            this.connection.ProcessEventArgs(new FeatureTypeEventArgs(this.connection, ErrorCode.NoError, "", FeatureTypeId.MaxMarketDataStreams, 500.0));
            this.connection.ProcessEventArgs(new FeatureTypeEventArgs(this.connection, ErrorCode.NoError, "", FeatureTypeId.MaxMarketDepthStreams, 500.0));
            this.connection.ProcessEventArgs(new FeatureTypeEventArgs(this.connection, ErrorCode.NoError, "", FeatureTypeId.News, 0.0));
            this.connection.ProcessEventArgs(new FeatureTypeEventArgs(this.connection, ErrorCode.NoError, "", FeatureTypeId.QuotesDaily, 0.0));
            this.connection.ProcessEventArgs(new FeatureTypeEventArgs(this.connection, ErrorCode.NoError, "", FeatureTypeId.SplitsAdjustedDaily, 0.0));
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
            this.connection.ProcessEventArgs(new NewsItemTypeEventArgs(this.connection, ErrorCode.NoError, "", NewsItemTypeId.Default, ""));
            this.connection.ProcessEventArgs(new SymbolTypeEventArgs(this.connection, ErrorCode.NoError, "", SymbolTypeId.Unknown, ""));
            this.connection.ProcessEventArgs(new SymbolTypeEventArgs(this.connection, ErrorCode.NoError, "", SymbolTypeId.Future, ""));
            this.connection.ProcessEventArgs(new SymbolTypeEventArgs(this.connection, ErrorCode.NoError, "", SymbolTypeId.Index, ""));
            this.connection.ProcessEventArgs(new SymbolTypeEventArgs(this.connection, ErrorCode.NoError, "", SymbolTypeId.Stock, ""));
            this.callback.typeAsk = this.connection.MarketDataTypes[MarketDataTypeId.Ask];
            this.callback.typeBid = this.connection.MarketDataTypes[MarketDataTypeId.Bid];
            this.callback.typeDailyHigh = this.connection.MarketDataTypes[MarketDataTypeId.DailyHigh];
            this.callback.typeDailyLow = this.connection.MarketDataTypes[MarketDataTypeId.DailyLow];
            this.callback.typeDailyVolume = this.connection.MarketDataTypes[MarketDataTypeId.DailyVolume];
            this.callback.typeLast = this.connection.MarketDataTypes[MarketDataTypeId.Last];
            this.callback.typeLastClose = this.connection.MarketDataTypes[MarketDataTypeId.LastClose];
            this.callback.typeOpening = this.connection.MarketDataTypes[MarketDataTypeId.Opening];
            RetCode success = RetCode.Success;
            if ((success = Api.RequestDataMidApi("$TIME", Feed.Level1, 0, 0, Api.NextRqn)) != RetCode.Success)
            {
                this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.Panic, success.ToString(), "CT.Adapter.Init.RequestDataMidApi: Failed to start clock sychronization"));
            }
            else if ((success = Api.AdviseDataMidApi("$TIME", Feed.Level1)) != RetCode.Success)
            {
                this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.Panic, success.ToString(), "CT.Adapter.Init.AdviseDataMidApi: Failed to start clock sychronization"));
            }
            else if ((success = Api.AdviseDataMidApi("!NEWSHOT", Feed.NewsHeadline)) != RetCode.Success)
            {
                this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.Panic, success.ToString(), "CT.Adapter.Init.AdviseDataMidApi: Failed to start news data stream"));
            }
        }

        private string MonthCode(DateTime expiry)
        {
            switch (expiry.Month)
            {
                case 1:
                    return "F";

                case 2:
                    return "G";

                case 3:
                    return "H";

                case 4:
                    return "J";

                case 5:
                    return "K";

                case 6:
                    return "M";

                case 7:
                    return "N";

                case 8:
                    return "Q";

                case 9:
                    return "U";

                case 10:
                    return "V";

                case 11:
                    return "X";
            }
            return "Z";
        }

        private void Progress_Aborted(object sender, EventArgs e)
        {
            if (Globals.TraceSwitch.Quotes)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") CT.Adapter.Progress_Aborted " + this.quotes2Lookup.ToString());
            }
            Quotes quotes = this.quotes2Lookup;
            this.quotes2Lookup = null;
            this.connection.ProcessEventArgs(new BarUpdateEventArgs(this.connection, ErrorCode.UserAbort, "Aborted by user", Operation.Insert, quotes, 0, -1));
        }

        public void Request(Quotes quotes)
        {
            Trace.Assert(quotes.Period.Id != PeriodTypeId.Tick, "CT.Adapter.Request");
            this.quotes2Lookup = quotes;
            Globals.Progress.Initialise(0, true);
            Globals.Progress.Aborted += new AbortEventHandler(this.Progress_Aborted);
            Globals.Progress.Message = "Retrieving historical data for '" + quotes.Symbol.FullName + "'";
            RetCode success = RetCode.Success;
            if ((success = Api.RequestDataMidApi(this.ToBrokerName(quotes.Symbol), (quotes.Period.Id == PeriodTypeId.Day) ? Feed.History : Feed.Intraday, ((int) this.connection.Now.Subtract(quotes.From).TotalDays) + 1, 1, Api.NextRqn)) != RetCode.Success)
            {
                this.quotes2Lookup = null;
                this.connection.ProcessEventArgs(new BarUpdateEventArgs(this.connection, ErrorCode.Panic, "CT.Adapter.Request: Unable to retrieve historical data(" + success + ")", Operation.Insert, quotes, 0, -1));
            }
        }

        public void Subscribe(MarketData marketData)
        {
            Hashtable hashtable;
            if (Globals.TraceSwitch.MarketData)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") CT.Adapter.Subscribe.MarketData: " + marketData.Symbol.FullName);
            }
            string key = this.ToBrokerName(marketData.Symbol);
            lock ((hashtable = this.symbol2MarketData))
            {
                this.symbol2MarketData.Add(key, marketData);
            }
            RetCode success = RetCode.Success;
            if ((success = Api.RequestDataMidApi(key, Feed.Level1, 0, 0, Api.NextRqn)) != RetCode.Success)
            {
                this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.Panic, success.ToString(), "CT.Adapter.Subscribe.RequestDataMidApi.Level1: Unable to subscribe to market data"));
                marketData.Cancel();
                lock ((hashtable = this.symbol2MarketData))
                {
                    this.symbol2MarketData.Remove(key);
                }
            }
            else if ((success = Api.AdviseDataMidApi(key, Feed.Level1)) != RetCode.Success)
            {
                this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.Panic, success.ToString(), "CT.Adapter.Subscribe.AdviseDataMidApi.Level1: Unable to subscribe to market data"));
                marketData.Cancel();
                lock ((hashtable = this.symbol2MarketData))
                {
                    this.symbol2MarketData.Remove(key);
                }
            }
            else if ((success = Api.RequestDataMidApi(key, Feed.NewsHeadline, (int) this.connection.Now.AddDays((double) -this.Options.NewsDaysBack).Subtract(new DateTime(0x76c, 1, 1, 0, 0, 0)).TotalSeconds, 0, Api.NextRqn)) != RetCode.Success)
            {
                this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.Panic, success.ToString(), "CT.Adapter.Subscribe.RequestDataMidApi.NewsHeadline: Unable to subscribe to market data"));
                marketData.Cancel();
                lock ((hashtable = this.symbol2MarketData))
                {
                    this.symbol2MarketData.Remove(key);
                }
            }
            else if ((success = Api.AdviseDataMidApi(key, Feed.NewsHeadline)) != RetCode.Success)
            {
                this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.Panic, success.ToString(), "CT.Adapter.Subscribe.AdviseDataMidApi.NewsHeadline: Unable to subscribe to market data"));
                marketData.Cancel();
                lock ((hashtable = this.symbol2MarketData))
                {
                    this.symbol2MarketData.Remove(key);
                }
            }
            else if ((success = Api.RequestDataMidApi(key, Feed.History, 3, 1, Api.NextRqn)) != RetCode.Success)
            {
                this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.Panic, success.ToString(), "CT.Adapter.Subscribe.RequestDataMidApi: Unable to subscribe to market data"));
                marketData.Cancel();
                lock ((hashtable = this.symbol2MarketData))
                {
                    this.symbol2MarketData.Remove(key);
                }
            }
        }

        public void Subscribe(MarketDepth marketDepth)
        {
            Hashtable hashtable;
            if (Globals.TraceSwitch.MarketDepth)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") CT.Adapter.Subscribe.MarketDepth: " + marketDepth.Symbol.FullName);
            }
            string key = this.ToBrokerName(marketDepth.Symbol);
            lock ((hashtable = this.symbol2MarketDepth))
            {
                this.symbol2MarketDepth.Add(key, marketDepth);
            }
            RetCode success = RetCode.Success;
            if ((success = Api.RequestDataMidApi(key, Feed.Level2, 0, 0, Api.NextRqn)) != RetCode.Success)
            {
                this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.Panic, success.ToString(), "CT.Adapter.Subscribe.RequestDataMidApi: Unable to subscribe to market depth data"));
                marketDepth.Cancel();
                lock ((hashtable = this.symbol2MarketDepth))
                {
                    this.symbol2MarketDepth.Remove(key);
                }
            }
            else if ((success = Api.AdviseDataMidApi(key, Feed.Level2)) != RetCode.Success)
            {
                this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.Panic, success.ToString(), "CT.Adapter.Subscribe.AdviseDataMidApi: Unable to subscribe to market depth data"));
                marketDepth.Cancel();
                lock ((hashtable = this.symbol2MarketDepth))
                {
                    this.symbol2MarketDepth.Remove(key);
                }
            }
        }

        public void SymbolLookup(Symbol symbolTemplate)
        {
            this.connection.CreateSymbol(symbolTemplate.Name, symbolTemplate.Expiry, symbolTemplate.SymbolType, symbolTemplate.Exchange, symbolTemplate.StrikePrice, symbolTemplate.Right.Id, this.connection.Currencies[CurrencyId.Unknown], 0.01, 1.0, "", null, 0, null, null, null);
        }

        private string ToBrokerName(Symbol symbol)
        {
            string str = symbol.GetProviderName(ProviderTypeId.CyberTrader).ToUpper();
            if (symbol.SymbolType.Id == SymbolTypeId.Future)
            {
                str = string.Concat(new object[] { "/", str, this.MonthCode(symbol.Expiry), symbol.Expiry.Year % 10 });
            }
            return str;
        }

        public void Unsubscribe(MarketData marketData)
        {
            if (Globals.TraceSwitch.MarketData)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") CT.Adapter.Unsubscribe.MarketData: " + marketData.Symbol.FullName);
            }
            string key = this.ToBrokerName(marketData.Symbol);
            lock (this.symbol2MarketData)
            {
                this.symbol2MarketData.Remove(key);
            }
            RetCode success = RetCode.Success;
            if ((success = Api.UnadviseDataMidApi(key, Feed.Level1)) != RetCode.Success)
            {
                this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.Panic, success.ToString(), "CT.Adapter.Unsubscribe.UnadviseDataMidApi.Level1: Unable to unsubscribe from market data"));
            }
            else if ((success = Api.UnadviseDataMidApi(key, Feed.NewsHeadline)) != RetCode.Success)
            {
                this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.Panic, success.ToString(), "CT.Adapter.Unsubscribe.UnadviseDataMidApi.NewsHeadline: Unable to unsubscribe from market data"));
            }
        }

        public void Unsubscribe(MarketDepth marketDepth)
        {
            if (Globals.TraceSwitch.MarketDepth)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") CT.Adapter.Unsubscribe.MarketDepth: " + marketDepth.Symbol.FullName);
            }
            string key = this.ToBrokerName(marketDepth.Symbol);
            lock (this.symbol2MarketDepth)
            {
                this.symbol2MarketDepth.Remove(key);
            }
            RetCode success = RetCode.Success;
            if ((success = Api.UnadviseDataMidApi(key, Feed.Level2)) != RetCode.Success)
            {
                this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.Panic, success.ToString(), "CT.Adapter.Unsubscribe.UnadviseDataMidApi: Unable to unsubscribe from market depth data"));
            }
        }

        internal CTOptions Options
        {
            get
            {
                return (CTOptions) this.connection.Options;
            }
        }
    }
}

