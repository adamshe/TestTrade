using iTrading.Core.Data;
using iTrading.Core.Kernel;    
using IESignal;
using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;

namespace iTrading.ESignal
{

    internal class Adapter : IAdapter, ILoader, IMarketData, IQuotes
    {
        private Connection connection;
        private QuotesRequest currentRequest = null;
        private HooksClass eSignal = null;
        private Hashtable handle2Request = new Hashtable();
        private Hashtable quote2Symbol = new Hashtable();
        private Queue requests = new Queue();
        private Thread requestThread = null;
        private MarketDataType typeAsk = null;
        private MarketDataType typeBid = null;
        private MarketDataType typeDailyHigh = null;
        private MarketDataType typeDailyLow = null;
        private MarketDataType typeDailyVolume = null;
        private MarketDataType typeLast = null;
        private MarketDataType typeLastClose = null;
        private MarketDataType typeOpening = null;

        internal Adapter(Connection connection)
        {
            this.connection = connection;
            this.requestThread = new Thread(new ThreadStart(this.WorkerThread));
            this.requestThread.Name = "TM eSignal";
            this.requestThread.Start();
        }

        public void Clear()
        {
        }

        public void Connect()
        {
            if (Globals.TraceSwitch.Connect)
            {
                Trace.WriteLine(string.Concat(new object[] { "(", this.connection.IdPlus, ") ESignal.Adapter.Connect:  checkStatusSeconds=", ((ESignalOptions) this.connection.Options).CheckStatusSeconds, " connectTimeoutSeconds=", ((ESignalOptions) this.connection.Options).ConnectTimeoutSeconds }));
            }
            lock (this.requests)
            {
                object[] objArray = new object[2];
                objArray[0] = new WaitCallback(this.ConnectNow);
                this.requests.Enqueue(objArray);
                Monitor.Pulse(this.requests);
            }
        }

        private void ConnectNow()
        {
            if (Globals.TraceSwitch.Connect)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") ESignal.Adapter.ConnectNow");
            }
            this.Init();
            this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, ErrorCode.NoError, "", ConnectionStatusId.Connected, ConnectionStatusId.Connected, 0, ""));
        }

        public void ConnectNow(object state)
        {
            if (this.connection.Options.Mode.Id == ModeTypeId.Simulation)
            {
                new Thread(new ThreadStart(this.ConnectNow)).Start();
            }
            else
            {
                this.eSignal = new HooksClass();
                this.eSignal.OnEntitledValid += (new _IHooksEvents_OnEntitledValidEventHandler(this.eSignal_OnEntitledValid));
                this.eSignal.OnQuoteChanged += (new _IHooksEvents_OnQuoteChangedEventHandler(this.eSignal_OnQuoteChanged));
                this.eSignal.OnBarsChanged += (new _IHooksEvents_OnBarsChangedEventHandler(this.eSignal_OnBarsChanged));
                this.eSignal.OnBarsReceived += (new _IHooksEvents_OnBarsReceivedEventHandler(this.eSignal_OnBarsReceived));
                this.eSignal.SetApplication(this.connection.Options.User);
            }
        }

        public IAdapter Create(Connection connection)
        {
            return new Adapter(connection);
        }

        public void Disconnect()
        {
            if (this.connection.Options.Mode.Id == ModeTypeId.Simulation)
            {
                new Thread(new ThreadStart(this.DisconnectNow)).Start();
            }
            else
            {
                if (Globals.TraceSwitch.Connect)
                {
                    Trace.WriteLine("(" + this.connection.IdPlus + ") ESignal.Adapter.Disconnect");
                }
                this.requestThread.Abort();
                this.eSignal.ReleaseAllHistory();
                this.eSignal.ReleaseAllTimeSales();
                this.eSignal = null;
                new Thread(new ThreadStart(this.DisconnectNow)).Start();
            }
        }

        private void DisconnectNow()
        {
            if (Globals.TraceSwitch.Connect)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") ESignal.Adapter.DisconnectNow");
            }
            this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, ErrorCode.NoError, "", ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
        }

        private void eSignal_OnBarsChanged(int lHandle)
        {
        }

        private void eSignal_OnBarsReceived(int lHandle)
        {
            if (Globals.TraceSwitch.Native)
            {
                Trace.WriteLine(string.Concat(new object[] { "(", this.connection.IdPlus, ") ESignal.Adapter.OnBarsReceived: handle=", lHandle }));
            }
            try
            {
                QuotesRequest request = (QuotesRequest) this.handle2Request[lHandle];
                if (request != null)
                {
                    if (request.intern)
                    {
                        this.HandleMarketData(request);
                    }
                    else
                    {
                        this.History2Quotes(request);
                    }
                }
            }
            catch (Exception exception)
            {
                Trace.WriteLine("ERROR: ESignal.Adapter.eSignal_OnBarsReceived: " + exception.Message);
            }
        }

        private void eSignal_OnEntitledValid()
        {
            try
            {
                if (this.connection.ConnectionStatusId == ConnectionStatusId.Connecting)
                {
                    if (this.eSignal.IsEntitled == 0)
                    {
                        this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, ErrorCode.NativeError, "Not entitled to use eSignal ActiveX API", ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                    }
                    else
                    {
                        this.eSignal.ClearSymbolCache();
                        this.eSignal.ReleaseAllHistory();
                        this.eSignal.ReleaseAllTimeSales();
                        new Thread(new ThreadStart(this.ConnectNow)).Start();
                    }
                }
            }
            catch (Exception exception)
            {
                Trace.WriteLine("ERROR: ESignal.Adapter.eSignal_OnEntitledValid: " + exception.Message);
            }
        }

        private void eSignal_OnQuoteChanged(string sSymbol)
        {
            if (Globals.TraceSwitch.MarketData)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") ESignal.Adapter.OnQuoteChanged: symbol'=" + sSymbol + "'");
            }
            try
            {
                BasicQuote quote = this.eSignal.get_GetBasicQuote(sSymbol);
                Symbol symbol = (Symbol) this.quote2Symbol[sSymbol];
                if (symbol != null)
                {
                    double price = symbol.Round2TickSize(quote.dAsk);
                    int volume = quote.lAskSize * ((symbol.SymbolType.Id == SymbolTypeId.Stock) ? 100 : 1);
                    if ((price > 0.0) && (((symbol.MarketData.Ask == null) || (symbol.MarketData.Ask.Price != quote.dAsk)) || (symbol.MarketData.Ask.Volume != volume)))
                    {
                        this.connection.ProcessEventArgs(new MarketDataEventArgs(symbol.MarketData, ErrorCode.NoError, "", symbol, this.typeAsk, price, volume, this.connection.Now));
                    }
                    double num3 = symbol.Round2TickSize(quote.dBid);
                    int num4 = quote.lBidSize * ((symbol.SymbolType.Id == SymbolTypeId.Stock) ? 100 : 1);
                    if ((num3 > 0.0) && (((symbol.MarketData.Bid == null) || (symbol.MarketData.Bid.Price != quote.dBid)) || (symbol.MarketData.Bid.Volume != quote.lBidSize)))
                    {
                        this.connection.ProcessEventArgs(new MarketDataEventArgs(symbol.MarketData, ErrorCode.NoError, "", symbol, this.typeBid, num3, num4, this.connection.Now));
                    }
                    double num5 = symbol.Round2TickSize(quote.dLast);
                    if ((num5 > 0.0) && (((symbol.MarketData.Last == null) || (symbol.MarketData.Last.Price != quote.dLast)) || (symbol.MarketData.Last.Volume != quote.lLastSize)))
                    {
                        int lLastSize = quote.lLastSize;
                        if (lLastSize < 0)
                        {
                            lLastSize = 0;
                        }
                        this.connection.ProcessEventArgs(new MarketDataEventArgs(symbol.MarketData, ErrorCode.NoError, "", symbol, this.typeLast, num5, lLastSize, this.connection.Now));
                        if (lLastSize > 0)
                        {
                            this.connection.ProcessEventArgs(new MarketDataEventArgs(symbol.MarketData, ErrorCode.NoError, "", symbol, this.typeDailyVolume, 0.0, lLastSize + ((symbol.MarketData.DailyVolume != null) ? symbol.MarketData.DailyVolume.Volume : 0), this.connection.Now));
                        }
                    }
                    if ((num5 > 0.0) && ((symbol.MarketData.DailyHigh == null) || (symbol.MarketData.DailyHigh.Price < quote.dLast)))
                    {
                        this.connection.ProcessEventArgs(new MarketDataEventArgs(symbol.MarketData, ErrorCode.NoError, "", symbol, this.typeDailyHigh, num5, 0, this.connection.Now));
                    }
                    if ((num5 > 0.0) && ((symbol.MarketData.DailyLow == null) || (symbol.MarketData.DailyLow.Price > quote.dLast)))
                    {
                        this.connection.ProcessEventArgs(new MarketDataEventArgs(symbol.MarketData, ErrorCode.NoError, "", symbol, this.typeDailyLow, num5, 0, this.connection.Now));
                    }
                }
            }
            catch (Exception exception)
            {
                Trace.WriteLine("ERROR: ESignal.Adapter.eSignal_OnQuoteChanged: " + exception.Message);
            }
        }

        private void HandleMarketData(QuotesRequest request)
        {
            double price = 0.0;
            double num2 = 0.0;
            double num3 = 0.0;
            double num4 = 0.0;
            int volume = 0;
            if (Globals.TraceSwitch.MarketData)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") ESignal.Adapter.HandleMarketData: symbol'=" + request.symbol.FullName + "'");
            }
            if (this.eSignal.get_GetNumBars(request.handle) >= 2)
            {
                num3 = request.symbol.Round2TickSize(this.eSignal.get_GetBar(request.handle, -1).dClose);
            }
            if (this.eSignal.get_GetNumBars(request.handle) >= 1)
            {
                num2 = request.symbol.Round2TickSize(this.eSignal.get_GetBar(request.handle, 0).dHigh);
                num4 = request.symbol.Round2TickSize(this.eSignal.get_GetBar(request.handle, 0).dLow);
                volume = (int) this.eSignal.get_GetBar(request.handle, 0).dVolume;
                price = request.symbol.Round2TickSize(this.eSignal.get_GetBar(request.handle, 0).dOpen);
            }
            this.eSignal.ReleaseHistory(request.handle);
            lock (this.handle2Request)
            {
                this.handle2Request.Remove(request.handle);
            }
            if (num2 > 0.0)
            {
                if ((price > 0.0) && (request.symbol.MarketData.Opening == null))
                {
                    this.connection.ProcessEventArgs(new MarketDataEventArgs(request.symbol.MarketData, ErrorCode.NoError, "", request.symbol, this.typeOpening, price, 0, this.connection.Now));
                }
                if ((num2 > 0.0) && ((request.symbol.MarketData.DailyHigh == null) || (request.symbol.MarketData.DailyHigh.Price < num2)))
                {
                    this.connection.ProcessEventArgs(new MarketDataEventArgs(request.symbol.MarketData, ErrorCode.NoError, "", request.symbol, this.typeDailyHigh, num2, 0, this.connection.Now));
                }
                if ((num4 > 0.0) && ((request.symbol.MarketData.DailyLow == null) || (request.symbol.MarketData.DailyLow.Price > num4)))
                {
                    this.connection.ProcessEventArgs(new MarketDataEventArgs(request.symbol.MarketData, ErrorCode.NoError, "", request.symbol, this.typeDailyLow, num4, 0, this.connection.Now));
                }
                if ((volume > 0) && ((request.symbol.MarketData.DailyVolume == null) || (request.symbol.MarketData.DailyVolume.Volume < volume)))
                {
                    this.connection.ProcessEventArgs(new MarketDataEventArgs(request.symbol.MarketData, ErrorCode.NoError, "", request.symbol, this.typeDailyVolume, 0.0, volume, this.connection.Now));
                }
            }
            if (num3 > 0.0)
            {
                this.eSignal.get_GetBar(request.handle, -1);
                if (request.symbol.MarketData.LastClose == null)
                {
                    this.connection.ProcessEventArgs(new MarketDataEventArgs(request.symbol.MarketData, ErrorCode.NoError, "", request.symbol, this.typeLastClose, num3, 0, this.connection.Now));
                }
            }
        }

        private void History2Quotes(QuotesRequest request)
        {
            if (Globals.TraceSwitch.Quotes)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") ESignal.Adapter.History2Quotes: symbol'=" + request.symbol.FullName + "'");
            }
            Quotes quotes = request.quotes;
            lock (request)
            {
                if (request.quotes == null)
                {
                    return;
                }
                for (int i = this.eSignal.get_GetNumBars(request.handle) - 1; i >= 0; i--)
                {
                    if (Globals.Progress.IsAborted)
                    {
                        return;
                    }
                    BarData data = this.eSignal.get_GetBar(request.handle, -i);
                    DateTime dtTime = data.dtTime;
                    if (request.quotes.Period.Id != PeriodTypeId.Day)
                    {
                        dtTime = dtTime.AddHours((double) -this.connection.Options.TimerDelayHours);
                    }
                    if (request.quotes.Period.Id == PeriodTypeId.Minute)
                    {
                        dtTime = dtTime.AddMinutes(1.0);
                    }
                    if (((request.quotes.Period.Id == PeriodTypeId.Tick) && (request.quotes.Bars.Count > 0)) && (dtTime < request.quotes.Bars[request.quotes.Bars.Count - 1].Time))
                    {
                        dtTime = request.quotes.Bars[request.quotes.Bars.Count - 1].Time;
                    }
                    double tickSize = quotes.Symbol.TickSize;
                    if (quotes.Period.Id == PeriodTypeId.Day)
                    {
                        tickSize /= quotes.Symbol.Splits.GetSplitFactor(dtTime.Date);
                    }
                    double close = Symbol.Round2TickSize(data.dClose, tickSize);
                    double high = Symbol.Round2TickSize(data.dHigh, tickSize);
                    double low = Symbol.Round2TickSize(data.dLow, tickSize);
                    double open = Symbol.Round2TickSize(data.dOpen, tickSize);
                    int dVolume = (int) data.dVolume;
                    if ((dtTime.Date >= request.quotes.From) && (dtTime.Date <= request.quotes.To))
                    {
                        if (dVolume < 0)
                        {
                            dVolume = 0;
                        }
                        else if (((((open <= 0.0) || (high <= 0.0)) || ((low <= 0.0) || (close <= 0.0))) || (((open < low) || (high < low)) || ((close < low) || (open > high)))) || ((low > high) || (close > high)))
                        {
                            goto Label_02BC;
                        }
                        request.quotes.Bars.Add(open, high, low, close, dtTime, dVolume, false);
                    Label_02BC:;
                    }
                }
                Globals.Progress.Terminate();
                this.eSignal.ReleaseHistory(request.handle);
                lock (this.handle2Request)
                {
                    this.handle2Request.Remove(request.handle);
                }
                request.quotes = null;
            }
            this.connection.ProcessEventArgs(new BarUpdateEventArgs(this.connection, ErrorCode.NoError, "", Operation.Insert, quotes, 0, quotes.Bars.Count - 1));
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
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Cfe, "CF"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Default, ""));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Eurex, "DT"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.EurexUS, "EUS"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Hkfe, "HKF"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Idem, "IDM"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Liffe, "LF"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Monep, "MA"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Sfe, "SFE"));
            this.connection.ProcessEventArgs(new FeatureTypeEventArgs(this.connection, ErrorCode.NoError, "", FeatureTypeId.MarketData, 0.0));
            this.connection.ProcessEventArgs(new FeatureTypeEventArgs(this.connection, ErrorCode.NoError, "", FeatureTypeId.MaxMarketDataStreams, 999.0));
            this.connection.ProcessEventArgs(new FeatureTypeEventArgs(this.connection, ErrorCode.NoError, "", FeatureTypeId.Quotes1Minute, 0.0));
            this.connection.ProcessEventArgs(new FeatureTypeEventArgs(this.connection, ErrorCode.NoError, "", FeatureTypeId.QuotesDaily, 0.0));
            this.connection.ProcessEventArgs(new FeatureTypeEventArgs(this.connection, ErrorCode.NoError, "", FeatureTypeId.QuotesTick, 0.0));
            this.connection.ProcessEventArgs(new FeatureTypeEventArgs(this.connection, ErrorCode.NoError, "", FeatureTypeId.SplitsAdjustedDaily, 0.0));
            this.connection.ProcessEventArgs(new FeatureTypeEventArgs(this.connection, ErrorCode.NoError, "", FeatureTypeId.SymbolLookup, 0.0));
            this.connection.ProcessEventArgs(new MarketDataTypeEventArgs(this.connection, ErrorCode.NoError, "", MarketDataTypeId.Ask, ""));
            this.connection.ProcessEventArgs(new MarketDataTypeEventArgs(this.connection, ErrorCode.NoError, "", MarketDataTypeId.Bid, ""));
            this.connection.ProcessEventArgs(new MarketDataTypeEventArgs(this.connection, ErrorCode.NoError, "", MarketDataTypeId.DailyHigh, ""));
            this.connection.ProcessEventArgs(new MarketDataTypeEventArgs(this.connection, ErrorCode.NoError, "", MarketDataTypeId.DailyLow, ""));
            this.connection.ProcessEventArgs(new MarketDataTypeEventArgs(this.connection, ErrorCode.NoError, "", MarketDataTypeId.DailyVolume, ""));
            this.connection.ProcessEventArgs(new MarketDataTypeEventArgs(this.connection, ErrorCode.NoError, "", MarketDataTypeId.Last, ""));
            this.connection.ProcessEventArgs(new MarketDataTypeEventArgs(this.connection, ErrorCode.NoError, "", MarketDataTypeId.LastClose, ""));
            this.connection.ProcessEventArgs(new MarketDataTypeEventArgs(this.connection, ErrorCode.NoError, "", MarketDataTypeId.Opening, ""));
            this.connection.ProcessEventArgs(new MarketDataTypeEventArgs(this.connection, ErrorCode.NoError, "", MarketDataTypeId.Unknown, ""));
            this.connection.ProcessEventArgs(new SymbolTypeEventArgs(this.connection, ErrorCode.NoError, "", SymbolTypeId.Unknown, ""));
            this.connection.ProcessEventArgs(new SymbolTypeEventArgs(this.connection, ErrorCode.NoError, "", SymbolTypeId.Future, ""));
            this.connection.ProcessEventArgs(new SymbolTypeEventArgs(this.connection, ErrorCode.NoError, "", SymbolTypeId.Index, ""));
            this.connection.ProcessEventArgs(new SymbolTypeEventArgs(this.connection, ErrorCode.NoError, "", SymbolTypeId.Stock, ""));
            this.typeAsk = this.connection.MarketDataTypes[MarketDataTypeId.Ask];
            this.typeBid = this.connection.MarketDataTypes[MarketDataTypeId.Bid];
            this.typeDailyHigh = this.connection.MarketDataTypes[MarketDataTypeId.DailyHigh];
            this.typeDailyLow = this.connection.MarketDataTypes[MarketDataTypeId.DailyLow];
            this.typeDailyVolume = this.connection.MarketDataTypes[MarketDataTypeId.DailyVolume];
            this.typeLast = this.connection.MarketDataTypes[MarketDataTypeId.Last];
            this.typeLastClose = this.connection.MarketDataTypes[MarketDataTypeId.LastClose];
            this.typeOpening = this.connection.MarketDataTypes[MarketDataTypeId.Opening];
        }

        private string MonthCode(Symbol symbol)
        {
            if (symbol.Expiry == Globals.ContinousContractExpiry)
            {
                return "##";
            }
            switch (symbol.Expiry.Month)
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

        private void Quotes_Aborted(object sender, EventArgs e)
        {
            if (Globals.TraceSwitch.Quotes)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") ESignal.Adapter.Quotes_Aborted: symbol'=" + ((this.currentRequest == null) ? "null" : this.currentRequest.symbol.FullName) + "'");
            }
            QuotesRequest currentRequest = this.currentRequest;
            this.currentRequest = null;
            this.eSignal.ReleaseHistory(currentRequest.handle);
            lock (this.handle2Request)
            {
                this.handle2Request.Remove(currentRequest.handle);
            }
            Quotes quotes = currentRequest.quotes;
            currentRequest.quotes = null;
            this.connection.ProcessEventArgs(new BarUpdateEventArgs(this.connection, ErrorCode.UserAbort, "Retrieving historical data aborted", Operation.Insert, quotes, 0, -1));
        }

        public void Request(Quotes quotes)
        {
            lock (this.requests)
            {
                this.requests.Enqueue(new object[] { new WaitCallback(this.RequestNow), quotes });
                Monitor.Pulse(this.requests);
            }
        }

        private void RequestNow(object q)
        {
            Quotes quotes = (Quotes) q;
            if (Globals.TraceSwitch.Quotes)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") ESignal.Adapter.RequestNow: " + quotes.ToString());
            }
            if (this.eSignal.IsValidSymbol(this.ToProviderName(quotes.Symbol)) == 0)
            {
                this.connection.ProcessEventArgs(new BarUpdateEventArgs(this.connection, ErrorCode.NoSuchSymbol, "No such symbol", Operation.Insert, quotes, 0, quotes.Bars.Count - 1));
            }
            else
            {
                Globals.Progress.Initialise(0, true);
                Globals.Progress.Aborted += new AbortEventHandler(this.Quotes_Aborted);
                Globals.Progress.Message = "Retrieving historical data for '" + quotes.Symbol.FullName + "'";
                int handle = -1;
                if (quotes.Period.Id == PeriodTypeId.Day)
                {
                    handle = this.eSignal.get_RequestHistory(this.ToProviderName(quotes.Symbol), "D", barType.btDAYS, ((int) this.connection.Now.Date.Subtract(quotes.From).TotalDays) + 1, -1, -1);
                }
                else if (quotes.Period.Id == PeriodTypeId.Minute)
                {
                    handle = this.eSignal.get_RequestHistory(this.ToProviderName(quotes.Symbol), "1", barType.btDAYS, ((int) this.connection.Now.Date.Subtract(quotes.From).TotalDays) + 1, -1, -1);
                }
                else if (quotes.Period.Id == PeriodTypeId.Tick)
                {
                    handle = this.eSignal.get_RequestHistory(this.ToProviderName(quotes.Symbol), "1T", barType.btDAYS, ((int) this.connection.Now.Date.Subtract(quotes.From).TotalDays) + 1, -1, -1);
                }
                this.currentRequest = new QuotesRequest(quotes, quotes.Symbol, handle, false);
                lock (this.handle2Request)
                {
                    this.handle2Request.Add(handle, this.currentRequest);
                }
                if (this.eSignal.get_IsHistoryReady(handle) == 1)
                {
                    this.eSignal_OnBarsReceived(handle);
                }
            }
        }

        public void Subscribe(MarketData marketData)
        {
            lock (this.requests)
            {
                this.requests.Enqueue(new object[] { new WaitCallback(this.SubscribeNow), marketData });
                Monitor.Pulse(this.requests);
            }
        }

        private void SubscribeNow(object m)
        {
            Hashtable hashtable;
            MarketData data = (MarketData) m;
            if (Globals.TraceSwitch.MarketData)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") ESignal.Adapter.Subscribe.SubscribeNow: symbol='" + data.Symbol.FullName + "'");
            }
            string key = this.ToProviderName(data.Symbol);
            lock ((hashtable = this.quote2Symbol))
            {
                this.quote2Symbol.Add(key, data.Symbol);
            }
            int handle = this.eSignal.get_RequestHistory(this.ToProviderName(data.Symbol), "D", barType.btDAYS, 10, -1, -1);
            QuotesRequest request = new QuotesRequest(null, data.Symbol, handle, true);
            lock ((hashtable = this.handle2Request))
            {
                this.handle2Request.Add(handle, request);
            }
            if (this.eSignal.get_IsHistoryReady(handle) == 1)
            {
                this.eSignal_OnBarsReceived(handle);
            }
            this.eSignal.RequestSymbol(key, 1);
        }

        public void SymbolLookup(Symbol template)
        {
            lock (this.requests)
            {
                this.requests.Enqueue(new object[] { new WaitCallback(this.SymbolLookupNow), template });
                Monitor.Pulse(this.requests);
            }
        }

        private void SymbolLookupNow(object t)
        {
            Symbol symbol = (Symbol) t;
            if (Globals.TraceSwitch.SymbolLookup)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") ESignal.Adapter.SymbolLookupNow: symbol='" + symbol.FullName + "'");
            }
            if (this.eSignal.IsValidSymbol(this.ToProviderName(symbol)) == 0)
            {
                this.connection.ProcessEventArgs(new SymbolEventArgs(this.connection, ErrorCode.NoSuchSymbol, "No such symbol", symbol));
            }
            else
            {
                string providerName = symbol.GetProviderName(ProviderTypeId.MBTrading);
                Symbol symbol2 = this.connection.GetSymbolByProviderName(providerName, symbol.Expiry, symbol.SymbolType, symbol.Exchange, symbol.StrikePrice, symbol.Right.Id, LookupPolicyId.RepositoryOnly);
                if (symbol2 != null)
                {
                    symbol = symbol2;
                }
                this.connection.CreateSymbol(providerName, symbol.Expiry, symbol.SymbolType, symbol.Exchange, symbol.StrikePrice, symbol.Right.Id, this.connection.Currencies[CurrencyId.Unknown], 0.01, 1.0, "", null, 0, null, null, null);
            }
        }

        internal string ToProviderName(Symbol symbol)
        {
            string str = symbol.GetProviderName(ProviderTypeId.ESignal).ToUpper();
            if (symbol.SymbolType.Id == SymbolTypeId.Future)
            {
                str = str + ' ' + ((symbol.Expiry == Globals.ContinousContractExpiry) ? "#F" : (this.MonthCode(symbol) + (symbol.Expiry.Year % 10)));
            }
            if (symbol.Exchange.MapId.Length != 0)
            {
                str = str + "-" + symbol.Exchange.MapId;
            }
            return str;
        }

        private void UnsubcribeNow(object m)
        {
            MarketData data = (MarketData) m;
            if (Globals.TraceSwitch.MarketData)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") ESignal.Adapter.UnsubcribeNow.MarketData: symbol='" + data.Symbol.FullName + "'");
            }
            string key = this.ToProviderName(data.Symbol);
            lock (this.quote2Symbol)
            {
                this.quote2Symbol.Remove(key);
            }
            this.eSignal.ReleaseSymbol(key);
        }

        public void Unsubscribe(MarketData marketData)
        {
            lock (this.requests)
            {
                this.requests.Enqueue(new object[] { new WaitCallback(this.UnsubcribeNow), marketData });
                Monitor.Pulse(this.requests);
            }
        }

        private void WorkerThread()
        {
            if (Globals.TraceSwitch.Connect)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") ESignal.Adapter.WorkerThread");
            }
            try
            {
                while (true)
                {
                    object[] objArray = null;
                    lock (this.requests)
                    {
                        if (this.requests.Count > 0)
                        {
                            objArray = (object[]) this.requests.Dequeue();
                        }
                        else
                        {
                            Monitor.Wait(this.requests);
                        }
                    }
                    if (objArray != null)
                    {
                        ((WaitCallback) objArray[0]).DynamicInvoke(new object[] { objArray[1] });
                    }
                }
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception exception)
            {
                this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.Panic, "", "ESignal.Adapter.WorkerThread: " + exception.Message));
            }
        }

        private class QuotesRequest
        {
            public int handle = -1;
            public bool intern = true;
            public Quotes quotes = null;
            public Symbol symbol = null;

            public QuotesRequest(Quotes quotes, Symbol symbol, int handle, bool intern)
            {
                this.intern = intern;
                this.handle = handle;
                this.quotes = quotes;
                this.symbol = symbol;
            }
        }
    }
}

