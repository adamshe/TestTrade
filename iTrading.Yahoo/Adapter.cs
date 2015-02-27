using iTrading.Core.Data;

namespace iTrading.Yahoo
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Timers;
    using iTrading.Core.Kernel;
    using iTrading.Core.Data;

    internal class Adapter : IAdapter, IQuotes, IMarketData
    {
        private const string amountPattern = @"[0-9]+\.[0-9]*";
        private Regex amountRegex = new Regex(@"[0-9]+\.[0-9]*");
        private Connection connection = null;
        private CultureInfo cultureInfo = new CultureInfo("en-US");
        private const string datePattern = @"[0-9]+\-[A-Z][a-z][a-z]\-[0-9]+";
        private Regex dateRegex = new Regex(@"[0-9]+\-[A-Z][a-z][a-z]\-[0-9]+");
        private Regex dividendRegex = new Regex(@"[0-9]+\-[A-Z][a-z][a-z]\-[0-9]+[^@]*[0-9]+\.[0-9]*\ [Cc][Aa][Ss][Hh]\ [Dd][Ii][Vv][Ii][Dd][Ee][Nn][Dd]");
        private Regex invalidTicker = new Regex(@"[Ii]nvalid\ [Tt]icker\ [Ss]ymbol");
        private DateTime lastTime;
        private System.Collections.Queue requests = new System.Collections.Queue();
        private Thread requestThread = null;
        private Regex spaceRegex = new Regex(@"[\ \n]+");
        private char[] splitChar = new char[] { ',' };
        private Regex splitFactorRegex = new Regex(@"[0-9]+\ :\ [0-9]+");
        private char[] splitLines = new char[] { '\n' };
        private const string splitPattern = @"[0-9]+\ :\ [0-9]+";
        private Regex splitRegex = new Regex(@"[0-9]+\-[A-Z][a-z][a-z]\-[0-9]+[^@]*[0-9]+\ :\ [0-9]+\ [Ss][Tt][Oo][Cc][Kk]\ [Ss][Pp][Ll][Ii][Tt]");
        private char[] splitSpace = new char[] { ' ' };
        private Hashtable subscribed = new Hashtable();
        private System.Timers.Timer subscribeTimer = null;
        private object[] syncRequest = new object[0];
        private DateTime timeLastRequest = Globals.MaxDate;
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
            this.lastTime = connection.Now;
            this.requestThread = new Thread(new ThreadStart(this.WorkerThread));
            this.requestThread.Name = "TM Yahoo";
            this.requestThread.Start();
        }

        public void Clear()
        {
        }

        public void Connect()
        {
            lock (this.requests)
            {
                object[] objArray = new object[2];
                objArray[0] = new WaitCallback(this.ConnectNow);
                this.requests.Enqueue(objArray);
                Monitor.Pulse(this.requests);
            }
        }

        private void ConnectNow(object state)
        {
            if (Globals.TraceSwitch.Connect)
            {
                Trace.WriteLine(string.Concat(new object[] { "(", this.connection.IdPlus, ") Yahoo.Adapter.ConnectNow:  retryTimesOnWebError=", ((YahooOptions) this.connection.Options).RetryTimesOnWebError, " updateMarketDataSeconds=", ((YahooOptions) this.connection.Options).UpdateMarketDataSeconds, " webTimeoutSeconds=", ((YahooOptions) this.connection.Options).WebTimeoutSeconds }));
            }
            this.subscribeTimer = new System.Timers.Timer(1000.0);
            this.subscribeTimer.AutoReset = true;
            this.subscribeTimer.Elapsed += new ElapsedEventHandler(this.subscribeTimer_Elapsed);
            this.subscribeTimer.Start();
            this.Init();
            this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, ErrorCode.NoError, "", ConnectionStatusId.Connected, ConnectionStatusId.Connected, 0, ""));
        }

        public void Disconnect()
        {
            if (Globals.TraceSwitch.Connect)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") Yahoo.Adapter.Disconnect");
            }
            this.requestThread.Abort();
            if (this.subscribeTimer != null)
            {
                this.subscribeTimer.Stop();
            }
            this.subscribeTimer = null;
            this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, ErrorCode.NoError, "", ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
        }

        private HttpWebResponse DoRequest(string requestString)
        {
            HttpWebResponse response = null;
            lock (this.syncRequest)
            {
                int num = 0;
                while (num < 5)
                {
                    try
                    {
                        if (Globals.TraceSwitch.Native)
                        {
                            Trace.WriteLine("(" + this.connection.IdPlus + ") Yahoo.Adapter.DoRequest1: request='" +
                                            requestString + "'");
                        }
                        WebRequest request = WebRequest.Create(requestString);
                        request.Timeout = ((YahooOptions) this.connection.Options).WebTimeoutSeconds*0x3e8;
                        response = (HttpWebResponse) request.GetResponse();
                        this.timeLastRequest = DateTime.Now;
                        return response;
                    }
                    catch (WebException exception)
                    {
                        if (num >= ((YahooOptions) this.connection.Options).RetryTimesOnWebError)
                        {
                            return null;
                        }
                        if (Globals.TraceSwitch.Native)
                        {
                            Trace.WriteLine("(" + this.connection.IdPlus +
                                            ") Yahoo.Adapter.DoRequest2: retrying on error '" + exception.Message + "'");
                        }
                        this.timeLastRequest = DateTime.Now;
                        int milliseconds = DateTime.Now.Subtract(this.timeLastRequest).Milliseconds;
                        if ((this.timeLastRequest != Globals.MaxDate) &&
                            (milliseconds < ((YahooOptions) this.connection.Options).WaitMilliSecondsRequest))
                        {
                            Thread.Sleep(
                                (int) (((YahooOptions) this.connection.Options).WaitMilliSecondsRequest - milliseconds));
                        }
                        num++;
                    }                  
                }
                return null;
            }          
        }

        internal void Init()
        {
            this.connection.ProcessEventArgs(new CurrencyEventArgs(this.connection, ErrorCode.NoError, "", CurrencyId.Unknown, ""));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Default, ""));
            this.connection.ProcessEventArgs(new FeatureTypeEventArgs(this.connection, ErrorCode.NoError, "", FeatureTypeId.MarketData, 0.0));
            this.connection.ProcessEventArgs(new FeatureTypeEventArgs(this.connection, ErrorCode.NoError, "", FeatureTypeId.MaxMarketDataStreams, 999.0));
            this.connection.ProcessEventArgs(new FeatureTypeEventArgs(this.connection, ErrorCode.NoError, "", FeatureTypeId.QuotesDaily, 0.0));
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

        public void Request(Quotes quotes)
        {
            if (quotes.To >= this.connection.Now.Date)
            {
                this.connection.ProcessEventArgs(new BarUpdateEventArgs(this.connection, ErrorCode.Panic, "Historical data only can be requested up to the previous day", Operation.Insert, quotes, 0, -1));
            }
            else
            {
                lock (this.requests)
                {
                    this.requests.Enqueue(new object[] { new WaitCallback(this.RequestNow), quotes });
                    Monitor.Pulse(this.requests);
                }
            }
        }

        private void RequestNow(object state)
        {
            Quotes quotes = (Quotes) state;
            if (Globals.TraceSwitch.Quotes)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") Yahoo.Adapter.RequestNow1: " + quotes.ToString());
            }
            string providerName = quotes.Symbol.GetProviderName(ProviderTypeId.Yahoo);
            string requestString = string.Concat(new object[] { "http://ichart.yahoo.com/table.csv?s=", providerName, "&a=", quotes.From.Month - 1, "&b=", quotes.From.Day, "&c=", quotes.From.Year, "&d=", quotes.To.Month - 1, "&e=", quotes.To.Day, "&f=", quotes.To.Year, "&g=d&ignore=.csv" });
            HttpWebResponse response = this.DoRequest(requestString);
            if (response == null)
            {
                this.connection.ProcessEventArgs(new BarUpdateEventArgs(this.connection, ErrorCode.Panic, "Failed to retrieve data from YAHOO", Operation.Insert, quotes, 0, -1));
            }
            else
            {
                string input = new StreamReader(response.GetResponseStream()).ReadToEnd();
                response.Close();
                if (Globals.TraceSwitch.Quotes)
                {
                    Trace.WriteLine("(" + this.connection.IdPlus + ") Yahoo.Adapter.RequestNow2: response='" + input + "'");
                }
                if (this.invalidTicker.Match(input).Length > 0)
                {
                    this.connection.ProcessEventArgs(new BarUpdateEventArgs(this.connection, ErrorCode.NoSuchSymbol, "Unknown ticker", Operation.Insert, quotes, 0, -1));
                }
                else
                {
                    string[] strArray = input.Split(this.splitLines);
                    for (int i = strArray.Length - 1; i >= 0; i--)
                    {
                        string str4 = strArray[i];
                        if ((str4.Length != 0) && char.IsNumber(str4[0]))
                        {
                            string[] strArray2 = str4.Split(this.splitChar);
                            if (strArray2.Length != 7)
                            {
                                this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.Panic, "", "Yahoo.Adapter.RequestNow: unexpected # of fields '" + str4 + "'"));
                                break;
                            }
                            try
                            {
                                DateTime time = Convert.ToDateTime(strArray2[0], this.cultureInfo);
                                double open = Convert.ToDouble(strArray2[1], this.cultureInfo);
                                double high = Convert.ToDouble(strArray2[2], this.cultureInfo);
                                double low = Convert.ToDouble(strArray2[3], this.cultureInfo);
                                double close = Convert.ToDouble(strArray2[4], this.cultureInfo);
                                int volume = Convert.ToInt32(strArray2[5], this.cultureInfo);
                                if (volume < 0)
                                {
                                    volume = 0;
                                }
                                if (((((quotes.From <= time) && (time <= quotes.To)) && ((open > 0.0) && (high > 0.0))) && (((low > 0.0) && (close > 0.0)) && ((open >= low) && (high >= low)))) && (((close >= low) && (open <= high)) && ((low <= high) && (close <= high))))
                                {
                                    quotes.Bars.Add(open, high, low, close, time, volume, false);
                                }
                            }
                            catch (Exception exception)
                            {
                                this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.Panic, "", "Yahoo.Adapter.RequestNow: error on processing line '" + str4 + "': " + exception.Message));
                            }
                        }
                    }
                    this.connection.ProcessEventArgs(new BarUpdateEventArgs(this.connection, ErrorCode.NoError, "", Operation.Insert, quotes, 0, quotes.Bars.Count - 1));
                }
            }
        }

        public void Subscribe(MarketData marketData)
        {
            if (Globals.TraceSwitch.MarketData)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") Yahoo.Adapter.Subscribe: symbol='" + marketData.Symbol.FullName + "'");
            }
            string providerName = marketData.Symbol.GetProviderName(ProviderTypeId.Yahoo);
            lock (this.subscribed)
            {
                this.subscribed.Add(providerName, marketData.Symbol);
            }
            lock (this.requests)
            {
                object[] objArray = new object[2];
                objArray[0] = new WaitCallback(this.SubscribeNow);
                this.requests.Enqueue(objArray);
                Monitor.Pulse(this.requests);
            }
        }

        private void SubscribeNow(object state)
        {
            string str5;
            StreamReader reader;
            this.lastTime = this.connection.Now;
            if (Globals.TraceSwitch.MarketData)
            {
                Trace.WriteLine(string.Concat(new object[] { "(", this.connection.IdPlus, ") Yahoo.Adapter.SubscribeNow: count=", this.subscribed.Keys.Count, "" }));
            }
            string str = "";
            string str2 = "";
            lock (this.subscribed)
            {
                foreach (string str3 in this.subscribed.Keys)
                {
                    str = str + ((str.Length == 0) ? "" : "+") + str3;
                }
                if (str.Length == 0)
                {
                    return;
                }
                foreach (string str4 in this.subscribed.Keys)
                {
                    if (((Symbol) this.subscribed[str4]).SymbolType.Id != SymbolTypeId.Index)
                    {
                        str2 = str2 + ((str.Length == 0) ? "" : "+") + str4;
                    }
                }
            }
            if (Globals.TraceSwitch.MarketData)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") Yahoo.Adapter.SubscribeNow1: all='" + str + "'");
            }
            HttpWebResponse response = this.DoRequest("http://finance.yahoo.com/d/quotes.csv?s=" + str + "&f=sl1k3");
            if (response != null)
            {
                str5 = "";
                reader = new StreamReader(response.GetResponseStream());
                while ((str5 = reader.ReadLine()) != null)
                {
                    int index = str5.IndexOf(',', 0);
                    if (index < 0)
                    {
                        this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.Panic, "", "Yahoo.Adapter.SubscribeNow: unexpected # of fields '" + str5 + "'"));
                        break;
                    }
                    int num2 = str5.IndexOf(',', index + 1);
                    if (num2 < 0)
                    {
                        this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.Panic, "", "Yahoo.Adapter.SubscribeNow: unexpected # of fields '" + str5 + "'"));
                        break;
                    }
                    Symbol symbol = (Symbol) this.subscribed[str5.Substring(0, index - 1).Replace("\"", "")];
                    if (symbol != null)
                    {
                        double price = 0.0;
                        try
                        {
                            price = symbol.Round2TickSize(Convert.ToDouble(str5.Substring(index + 1, (num2 - index) - 1), this.cultureInfo));
                        }
                        catch
                        {
                            continue;
                        }
                        int volume = 0;
                        try
                        {
                            volume = Convert.ToInt32(str5.Substring(num2 + 1).Replace(",", ""), this.cultureInfo);
                        }
                        catch
                        {
                            volume = (symbol.SymbolType.Id == SymbolTypeId.Stock) ? 100 : 1;
                        }
                        if (volume <= 0)
                        {
                            volume = 0;
                        }
                        if ((price > 0.0) && (((symbol.MarketData.Last == null) || (symbol.MarketData.Last.Price != price)) || (symbol.MarketData.Last.Volume != volume)))
                        {
                            this.connection.ProcessEventArgs(new MarketDataEventArgs(symbol.MarketData, ErrorCode.NoError, "", symbol, this.typeLast, price, volume, this.connection.Now));
                        }
                    }
                }
            }
            else
            {
                return;
            }
            response.Close();
            response = this.DoRequest("http://finance.yahoo.com/d/quotes.csv?s=" + str + "&f=spohgv");
            if (response != null)
            {
                reader = new StreamReader(response.GetResponseStream());
                while ((str5 = reader.ReadLine()) != null)
                {
                    string[] strArray = str5.Split(this.splitChar);
                    if (strArray.Length != 6)
                    {
                        this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.Panic, "", "Yahoo.Adapter.SubscribeNow: unexpected # of fields '" + str5 + "'"));
                        break;
                    }
                    Symbol symbol2 = (Symbol) this.subscribed[strArray[0].Replace("\"", "")];
                    if (symbol2 != null)
                    {
                        try
                        {
                            double num5 = symbol2.Round2TickSize(Convert.ToDouble(strArray[3], this.cultureInfo));
                            if ((num5 > 0.0) && ((symbol2.MarketData.DailyHigh == null) || (num5 > symbol2.MarketData.DailyHigh.Price)))
                            {
                                this.connection.ProcessEventArgs(new MarketDataEventArgs(symbol2.MarketData, ErrorCode.NoError, "", symbol2, this.typeDailyHigh, num5, 0, this.connection.Now));
                            }
                        }
                        catch
                        {
                        }
                        try
                        {
                            double num6 = symbol2.Round2TickSize(Convert.ToDouble(strArray[4], this.cultureInfo));
                            if ((num6 > 0.0) && ((symbol2.MarketData.DailyLow == null) || (num6 < symbol2.MarketData.DailyLow.Price)))
                            {
                                this.connection.ProcessEventArgs(new MarketDataEventArgs(symbol2.MarketData, ErrorCode.NoError, "", symbol2, this.typeDailyLow, num6, 0, this.connection.Now));
                            }
                        }
                        catch
                        {
                        }
                        try
                        {
                            int num7 = Convert.ToInt32(strArray[5], this.cultureInfo);
                            if (num7 > 0)
                            {
                                this.connection.ProcessEventArgs(new MarketDataEventArgs(symbol2.MarketData, ErrorCode.NoError, "", symbol2, this.typeDailyVolume, 0.0, num7, this.connection.Now));
                            }
                        }
                        catch
                        {
                        }
                        try
                        {
                            double num8 = symbol2.Round2TickSize(Convert.ToDouble(strArray[1], this.cultureInfo));
                            if ((num8 > 0.0) && (symbol2.MarketData.LastClose == null))
                            {
                                this.connection.ProcessEventArgs(new MarketDataEventArgs(symbol2.MarketData, ErrorCode.NoError, "", symbol2, this.typeLastClose, num8, 0, this.connection.Now));
                            }
                        }
                        catch
                        {
                        }
                        try
                        {
                            double num9 = symbol2.Round2TickSize(Convert.ToDouble(strArray[2], this.cultureInfo));
                            if ((num9 > 0.0) && (symbol2.MarketData.Opening == null))
                            {
                                this.connection.ProcessEventArgs(new MarketDataEventArgs(symbol2.MarketData, ErrorCode.NoError, "", symbol2, this.typeOpening, num9, 0, this.connection.Now));
                            }
                            continue;
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }
                response.Close();
                if (str2.Length > 0)
                {
                    response = this.DoRequest("http://finance.yahoo.com/d/quotes.csv?s=" + str2 + "&f=saa5");
                    if (response != null)
                    {
                        str5 = "";
                        reader = new StreamReader(response.GetResponseStream());
                        while ((str5 = reader.ReadLine()) != null)
                        {
                            int num10 = str5.IndexOf(',', 0);
                            if (num10 < 0)
                            {
                                this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.Panic, "", "Yahoo.Adapter.SubscribeNow: unexpected # of fields '" + str5 + "'"));
                                break;
                            }
                            int num11 = str5.IndexOf(',', num10 + 1);
                            if (num11 < 0)
                            {
                                this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.Panic, "", "Yahoo.Adapter.SubscribeNow: unexpected # of fields '" + str5 + "'"));
                                break;
                            }
                            Symbol symbol3 = (Symbol) this.subscribed[str5.Substring(0, num10 - 1).Replace("\"", "")];
                            if (symbol3 != null)
                            {
                                double num12 = 0.0;
                                try
                                {
                                    num12 = symbol3.Round2TickSize(Convert.ToDouble(str5.Substring(num10 + 1, (num11 - num10) - 1), this.cultureInfo));
                                }
                                catch
                                {
                                    continue;
                                }
                                int num13 = 0;
                                try
                                {
                                    num13 = Convert.ToInt32(str5.Substring(num11 + 1).Replace(",", ""), this.cultureInfo);
                                }
                                catch
                                {
                                    num13 = (symbol3.SymbolType.Id == SymbolTypeId.Stock) ? 100 : 1;
                                }
                                if (((num12 > 0.0) && (num13 > 0)) && (((symbol3.MarketData.Ask == null) || (symbol3.MarketData.Ask.Price != num12)) || (symbol3.MarketData.Ask.Volume != num13)))
                                {
                                    this.connection.ProcessEventArgs(new MarketDataEventArgs(symbol3.MarketData, ErrorCode.NoError, "", symbol3, this.typeAsk, num12, num13, this.connection.Now));
                                }
                            }
                        }
                        response.Close();
                        response = this.DoRequest("http://finance.yahoo.com/d/quotes.csv?s=" + str2 + "&f=sbb6");
                        if (response != null)
                        {
                            str5 = "";
                            reader = new StreamReader(response.GetResponseStream());
                            while ((str5 = reader.ReadLine()) != null)
                            {
                                int num14 = str5.IndexOf(',', 0);
                                if (num14 < 0)
                                {
                                    this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.Panic, "", "Yahoo.Adapter.SubscribeNow: unexpected # of fields '" + str5 + "'"));
                                    break;
                                }
                                int num15 = str5.IndexOf(',', num14 + 1);
                                if (num15 < 0)
                                {
                                    this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.Panic, "", "Yahoo.Adapter.SubscribeNow: unexpected # of fields '" + str5 + "'"));
                                    break;
                                }
                                Symbol symbol4 = (Symbol) this.subscribed[str5.Substring(0, num14 - 1).Replace("\"", "")];
                                if (symbol4 != null)
                                {
                                    double num16 = 0.0;
                                    try
                                    {
                                        num16 = symbol4.Round2TickSize(Convert.ToDouble(str5.Substring(num14 + 1, (num15 - num14) - 1), this.cultureInfo));
                                    }
                                    catch
                                    {
                                        continue;
                                    }
                                    int num17 = 0;
                                    try
                                    {
                                        num17 = Convert.ToInt32(str5.Substring(num15 + 1).Replace(",", ""), this.cultureInfo);
                                    }
                                    catch
                                    {
                                        num17 = (symbol4.SymbolType.Id == SymbolTypeId.Stock) ? 100 : 1;
                                    }
                                    if (((num16 > 0.0) && (num17 > 0)) && (((symbol4.MarketData.Bid == null) || (symbol4.MarketData.Bid.Price != num16)) || (symbol4.MarketData.Bid.Volume != num17)))
                                    {
                                        this.connection.ProcessEventArgs(new MarketDataEventArgs(symbol4.MarketData, ErrorCode.NoError, "", symbol4, this.typeBid, num16, num17, this.connection.Now));
                                    }
                                }
                            }
                            response.Close();
                        }
                    }
                }
            }
        }

        private void subscribeTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (this.connection.Now.Subtract(this.lastTime).TotalSeconds >= ((YahooOptions) this.connection.Options).UpdateMarketDataSeconds)
            {
                lock (this.requests)
                {
                    object[] objArray = new object[2];
                    objArray[0] = new WaitCallback(this.SubscribeNow);
                    this.requests.Enqueue(objArray);
                    Monitor.Pulse(this.requests);
                }
            }
        }

        public void SymbolLookup(Symbol template)
        {
            lock (this.requests)
            {
                this.requests.Enqueue(new object[] { new WaitCallback(this.SymbolLookupNow), template });
                Monitor.Pulse(this.requests);
            }
        }

        private void SymbolLookupNow(object state)
        {
            Symbol symbol = (Symbol) state;
            string providerName = symbol.GetProviderName(ProviderTypeId.Yahoo);
            string requestString = string.Concat(new object[] { "http://finance.yahoo.com/q/hp?s=", providerName, "&a=00&b=1&c=1900&d=", this.connection.Now.Month - 1, "&e=", this.connection.Now.Day, "&f=", this.connection.Now.Year, "&g=v" });
            if (Globals.TraceSwitch.SymbolLookup)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") Yahoo.Adapter.SymbolLookup1: symbol='" + symbol.FullName + "' request='" + requestString + "'");
            }
            HttpWebResponse response = this.DoRequest(requestString);
            if (response == null)
            {
                this.connection.ProcessEventArgs(new SymbolEventArgs(this.connection, ErrorCode.NoSuchSymbol, "No such symbol", null));
            }
            else
            {
                string input = new StreamReader(response.GetResponseStream()).ReadToEnd();
                input = this.spaceRegex.Replace(input, " ").Replace("</tr>", "@");
                response.Close();
                if (Globals.TraceSwitch.SymbolLookup)
                {
                    Trace.WriteLine("(" + this.connection.IdPlus + ") Yahoo.Adapter.SymbolLookup: res='" + input + "'");
                }
                if (this.invalidTicker.Match(input).Length > 0)
                {
                    this.connection.ProcessEventArgs(new SymbolEventArgs(this.connection, ErrorCode.NoSuchSymbol, "No such symbol", null));
                }
                else
                {
                    DividendDictionary dividends = new DividendDictionary();
                    SplitDictionary splits = new SplitDictionary();
                    if (symbol.SymbolType.Id == SymbolTypeId.Stock)
                    {
                        Match match;
                        SymbolCollection symbols = Globals.DB.Select(symbol.Name, null, null, symbol.Expiry, symbol.SymbolType, symbol.Exchange, 0.0, RightId.Unknown, null);
                        if (symbols.Count > 0)
                        {
                            dividends = symbols[0].Dividends;
                            splits = symbols[0].Splits;
                        }
                        for (match = this.dividendRegex.Match(input); match.Success; match = match.NextMatch())
                        {
                            string str4 = input.Substring(match.Index, match.Length);
                            Match match2 = this.dateRegex.Match(str4);
                            string str5 = str4.Substring(match2.Index, match2.Length);
                            match2 = this.amountRegex.Match(str4);
                            string str6 = str4.Substring(match2.Index, match2.Length);
                            try
                            {
                                double num = Convert.ToDouble(str6, this.cultureInfo);
                                DateTime time = Convert.ToDateTime(str5, this.cultureInfo);
                                dividends[time] = num;
                            }
                            catch
                            {
                            }
                        }
                        for (match = this.splitRegex.Match(input); match.Success; match = match.NextMatch())
                        {
                            string str7 = input.Substring(match.Index, match.Length);
                            Match match3 = this.dateRegex.Match(str7);
                            string str8 = str7.Substring(match3.Index, match3.Length);
                            match3 = this.splitFactorRegex.Match(str7);
                            string[] strArray = str7.Substring(match3.Index, match3.Length).Split(this.splitSpace);
                            try
                            {
                                DateTime time2 = Convert.ToDateTime(str8, this.cultureInfo);
                                double num2 = Convert.ToDouble(strArray[0]) / Convert.ToDouble(strArray[2]);
                                splits[time2] = num2;
                            }
                            catch
                            {
                            }
                        }
                    }
                    this.connection.CreateSymbol(providerName, symbol.Expiry, symbol.SymbolType, symbol.Exchange, symbol.StrikePrice, symbol.Right.Id, this.connection.Currencies[CurrencyId.Unknown], 0.01, 1.0, "", null, 0, null, splits, dividends);
                }
            }
        }

        public void Unsubscribe(MarketData marketData)
        {
            if (Globals.TraceSwitch.MarketData)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") Yahoo.Adapter.Unsubscribe: symbol='" + marketData.Symbol.FullName + "'");
            }
            string providerName = marketData.Symbol.GetProviderName(ProviderTypeId.Yahoo);
            lock (this.subscribed)
            {
                this.subscribed.Remove(providerName);
                Trace.Assert(this.subscribed[providerName] == null, "Yahoo.Adapter.Unsubsribe: symbol='" + marketData.Symbol.FullName + "'");
            }
        }

        private void WorkerThread()
        {
            if (Globals.TraceSwitch.Connect)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") Yahoo.Adapter.WorkerThread");
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
                this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.Panic, "", "Yahoo.Adapter.WorkerThread: " + exception.Message));
            }
        }
    }
}

