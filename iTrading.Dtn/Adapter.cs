using iTrading.Core.Data;

namespace iTrading.Dtn
{
    using Microsoft.Win32;
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;
    using System.Xml;
    using iTrading.Core.Kernel;
    using iTrading.Core.Data ;

    internal class ImplementationDetails
    {
        // Fields
        internal static structPacket1 Structure1; // data size: 6 bytes
        internal static structPacket2 Structure2; // data size: 6 bytes
        internal static Hashtable HashTable1;

        // Nested Types
        [StructLayout(LayoutKind.Explicit, Size=6, Pack=1)]
        internal struct structPacket1
        {
        }
        [StructLayout(LayoutKind.Explicit, Size=6, Pack=1)]
        internal struct structPacket2
        {
        }
    }

 

    public class Adapter : IAdapter, IMarketData, IMarketDepth, IQuotes
    {
        private bool clientAppRegistered = false;
        private bool closedByClient = false;
        private char[] commaSplitter = new char[] { ',' };
        internal Connection connection = null;
        private Control control = null;
        private CultureInfo cultureInfo = new CultureInfo("en-US");
        private string currentLookupRequest = "-";
        private char[] dataSplitter = new char[] { ',', ':', ' ' };
        private int dllHandle = 0;
        private const string endMsgTag = "!ENDMSG!";
        private int lastTotalVolume = 0;
        private Hashtable level1Tickers = new Hashtable();
        private Hashtable level2Tickers = new Hashtable();
        private StringCollection lookupRequests = new StringCollection();
        private Hashtable newsIds = new Hashtable();
        private int newsIntervalSeconds = 0;
        private ArrayList newsProviders = new ArrayList();
        private ArrayList newsStories = new ArrayList();
        private Quotes quotes2Lookup = null;
        private ArrayList quotesBuf = new ArrayList();
        private DtnSocket socketFeed = null;
        private DtnSocket socketLevel2 = null;
        private DtnSocket socketLookup = null;
        private char[] splitSplitter = new char[] { ' ', '/' };
        private char[] splitter2 = new char[] { '-', ':', ' ' };
        private Symbol symbol2Lookup = null;
        private Thread threadFeed = null;
        private Thread threadLevel2 = null;
        private Thread threadLookup = null;
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
            this.control = new Control();
            this.control.CreateControl();
            this.control.MouseMove += new MouseEventHandler(this.MouseMove);
        }

        private void Cleanup()
        {
            if (Globals.TraceSwitch.Connect)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") Dtn.Adapter.Cleanup");
            }
            this.connection.Timer -= new TimerEventHandler(this.ConnectionTimer);
            if (this.socketFeed != null)
            {
                this.socketFeed.Close();
            }
            this.socketFeed = null;
            if (this.socketLevel2 != null)
            {
                this.socketLevel2.Close();
            }
            this.socketLevel2 = null;
            this.socketLookup = null;
            if (this.threadFeed != null)
            {
                this.threadFeed.Abort();
            }
            this.threadFeed = null;
            if (this.threadLevel2 != null)
            {
                this.threadLevel2.Abort();
            }
            this.threadLevel2 = null;
            if (this.threadLookup != null)
            {
                this.threadLookup.Abort();
            }
            this.threadLookup = null;
            if (this.clientAppRegistered)
            {
                RemoveClientApp(this.control.Handle);
                if (this.dllHandle != 0)
                {
                    FreeLibrary(this.dllHandle);
                    this.dllHandle = 0;
                }
            }
        }

        public void Clear()
        {
        }

        public void Connect()
        {
            if (Globals.TraceSwitch.Connect)
            {
                Trace.WriteLine(string.Concat(new object[] { "(", this.connection.IdPlus, ") Dtn.Adapter.Connect:  host=", this.Options.Host, " newsDaysBack=", this.Options.NewsDaysBack, " newsIntervalSeconds=", this.Options.NewsIntervalSeconds, " newsMaxBack=", this.Options.NewsMaxBack, " portFeed=", this.Options.PortFeed, " portLevel2=", this.Options.PortLevel2, " portLookup=", this.Options.PortLookup }));
            }
            if (this.connection.Options.Mode.Id == ModeTypeId.Simulation)
            {
                new Thread(new ThreadStart(this.ConnectSimulation)).Start();
            }
            else
            {
                this.connection.Timer += new TimerEventHandler(this.ConnectionTimer);
                if (!this.TryConnect(false))
                {
                    try
                    {
                        string str = "";
                        RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE");
                        if (key != null)
                        {
                            RegistryKey key2 = key.OpenSubKey("DTN");
                            if (key2 != null)
                            {
                                key2 = key2.OpenSubKey("IQFeed");
                                if (key2 != null)
                                {
                                    str = (string) key2.GetValue("EXEDIR");
                                }
                            }
                        }
                        if (str.Length == 0)
                        {
                            str = Globals.InstallDir + @"\bin\Dtn";
                        }
                        if (Globals.TraceSwitch.Connect)
                        {
                            Trace.WriteLine("(" + this.connection.IdPlus + ") Dtn.Adapter.Connect: iqPath='" + str + "'");
                        }
                        this.dllHandle = LoadLibrary(str + @"\IQ32.dll");
                        if (this.dllHandle != 0)
                        {
                            if (RegisterClientApp(this.control.Handle, "TRADEMAGIC", "1.0", "0.11111111") == 0)
                            {
                                this.clientAppRegistered = true;
                            }
                            else
                            {
                                this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, ErrorCode.InvalidLicense, "Failed to call 'RegisterClientApp'", ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                            }
                        }
                        else
                        {
                            this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, ErrorCode.Panic, "Failed to load '" + str + @"\IQ32.dll'", ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                        }
                    }
                    catch (Exception exception)
                    {
                        this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, ErrorCode.Panic, "Failed to locate 'IQ32.dll': " + exception.Message, ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                    }
                }
            }
        }

        private void ConnectionTimer(object sender, TimerEventArgs e)
        {
            if (--this.newsIntervalSeconds <= 0)
            {
                this.RequestNewsHeadlines(false);
                this.newsIntervalSeconds = ((DtnOptions) this.connection.Options).NewsIntervalSeconds;
            }
        }

        private void ConnectSimulation()
        {
            this.Init();
            this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, ErrorCode.NoError, "", ConnectionStatusId.Connected, ConnectionStatusId.Connected, 0, ""));
        }

        public void Disconnect()
        {
            if (this.connection.Options.Mode.Id == ModeTypeId.Simulation)
            {
                new Thread(new ThreadStart(this.DisconnectNow)).Start();
            }
            else
            {
                this.closedByClient = true;
                this.Cleanup();
                new Thread(new ThreadStart(this.DisconnectNow)).Start();
            }
        }

        private void DisconnectNow()
        {
            if (Globals.TraceSwitch.Connect)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") Dtn.Adapter.DisconnectNow");
            }
            this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, ErrorCode.NoError, "", ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
        }

        [DllImport("Kernel32")]
        private static extern bool FreeLibrary(int handle);
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
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Ace, "60"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Amex, "6"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Box, "17"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Cboe, "13"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Default, ""));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.ECbot, "30"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Eurex, "65"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Globex, "34"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Liffe, "61"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Me, "51"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Nybot, "38"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Nymex, "36"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Nyse, "7"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.One, "40"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.OtcBB, "4"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Phlx, "9"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Pse, "11"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.Tsx, "50"));
            this.connection.ProcessEventArgs(new ExchangeEventArgs(this.connection, ErrorCode.NoError, "", ExchangeId.TsxV, "52"));
            this.connection.ProcessEventArgs(new FeatureTypeEventArgs(this.connection, ErrorCode.NoError, "", FeatureTypeId.ClockSynchronization, 0.0));
            this.connection.ProcessEventArgs(new FeatureTypeEventArgs(this.connection, ErrorCode.NoError, "", FeatureTypeId.MarketData, 0.0));
            this.connection.ProcessEventArgs(new FeatureTypeEventArgs(this.connection, ErrorCode.NoError, "", FeatureTypeId.MarketDepth, 0.0));
            this.connection.ProcessEventArgs(new FeatureTypeEventArgs(this.connection, ErrorCode.NoError, "", FeatureTypeId.MaxMarketDataStreams, 500.0));
            this.connection.ProcessEventArgs(new FeatureTypeEventArgs(this.connection, ErrorCode.NoError, "", FeatureTypeId.MaxMarketDepthStreams, 500.0));
            this.connection.ProcessEventArgs(new FeatureTypeEventArgs(this.connection, ErrorCode.NoError, "", FeatureTypeId.News, 0.0));
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
            this.connection.ProcessEventArgs(new NewsItemTypeEventArgs(this.connection, ErrorCode.NoError, "", NewsItemTypeId.AssociatedPress, "AP"));
            this.connection.ProcessEventArgs(new NewsItemTypeEventArgs(this.connection, ErrorCode.NoError, "", NewsItemTypeId.BusinessWire, "CBW"));
            this.connection.ProcessEventArgs(new NewsItemTypeEventArgs(this.connection, ErrorCode.NoError, "", NewsItemTypeId.Catalog, "CAT"));
            this.connection.ProcessEventArgs(new NewsItemTypeEventArgs(this.connection, ErrorCode.NoError, "", NewsItemTypeId.CbsMarketWatch, "MW"));
            this.connection.ProcessEventArgs(new NewsItemTypeEventArgs(this.connection, ErrorCode.NoError, "", NewsItemTypeId.Commercial, "C"));
            this.connection.ProcessEventArgs(new NewsItemTypeEventArgs(this.connection, ErrorCode.NoError, "", NewsItemTypeId.DowJones, "DJS"));
            this.connection.ProcessEventArgs(new NewsItemTypeEventArgs(this.connection, ErrorCode.NoError, "", NewsItemTypeId.Default, "RT8"));
            this.connection.ProcessEventArgs(new NewsItemTypeEventArgs(this.connection, ErrorCode.NoError, "", NewsItemTypeId.Dtn, "DTN"));
            this.connection.ProcessEventArgs(new NewsItemTypeEventArgs(this.connection, ErrorCode.NoError, "", NewsItemTypeId.DtnNewsBreak, "DNB"));
            this.connection.ProcessEventArgs(new NewsItemTypeEventArgs(this.connection, ErrorCode.NoError, "", NewsItemTypeId.FuturesWorld, "FW"));
            this.connection.ProcessEventArgs(new NewsItemTypeEventArgs(this.connection, ErrorCode.NoError, "", NewsItemTypeId.InternetWire, "CIW"));
            this.connection.ProcessEventArgs(new NewsItemTypeEventArgs(this.connection, ErrorCode.NoError, "", NewsItemTypeId.MarketGuide, "MKG"));
            this.connection.ProcessEventArgs(new NewsItemTypeEventArgs(this.connection, ErrorCode.NoError, "", NewsItemTypeId.MidnightTrader, "MNT"));
            this.connection.ProcessEventArgs(new NewsItemTypeEventArgs(this.connection, ErrorCode.NoError, "", NewsItemTypeId.PrimeZone, "CPZ"));
            this.connection.ProcessEventArgs(new NewsItemTypeEventArgs(this.connection, ErrorCode.NoError, "", NewsItemTypeId.PRNewswire, "CPR"));
            this.connection.ProcessEventArgs(new NewsItemTypeEventArgs(this.connection, ErrorCode.NoError, "", NewsItemTypeId.RealTimeTrader, "RTT"));
            this.connection.ProcessEventArgs(new NewsItemTypeEventArgs(this.connection, ErrorCode.NoError, "", NewsItemTypeId.Reuters, "BFA"));
            this.connection.ProcessEventArgs(new NewsItemTypeEventArgs(this.connection, ErrorCode.NoError, "", NewsItemTypeId.ReutersBasic, "RB"));
            this.connection.ProcessEventArgs(new NewsItemTypeEventArgs(this.connection, ErrorCode.NoError, "", NewsItemTypeId.ReutersPremium, "RP"));
            this.connection.ProcessEventArgs(new NewsItemTypeEventArgs(this.connection, ErrorCode.NoError, "", NewsItemTypeId.TheFlyOnTheWall, "FLY"));
            this.connection.ProcessEventArgs(new NewsItemTypeEventArgs(this.connection, ErrorCode.NoError, "", NewsItemTypeId.Usda, "US"));
            this.connection.ProcessEventArgs(new NewsItemTypeEventArgs(this.connection, ErrorCode.NoError, "", NewsItemTypeId.ZacksTrader, "ZAK"));
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
            Trace.Assert(this.typeAsk != null, "Dtn.Adapter.Init.typeAsk");
            Trace.Assert(this.typeBid != null, "Dtn.Adapter.Init.typeBid");
            Trace.Assert(this.typeDailyHigh != null, "Dtn.Adapter.Init.typeDailyHigh");
            Trace.Assert(this.typeDailyLow != null, "Dtn.Adapter.Init.typeDailyLow");
            Trace.Assert(this.typeDailyVolume != null, "Dtn.Adapter.Init.typeDailyVolume");
            Trace.Assert(this.typeLast != null, "Dtn.Adapter.Init.typeLast");
            Trace.Assert(this.typeLastClose != null, "Dtn.Adapter.Init.typeLastClose");
            Trace.Assert(this.typeOpening != null, "Dtn.Adapter.Init.typeOpening");
        }

        [DllImport("Kernel32")]
        private static extern int LoadLibrary(string dllName);
        private void MessageLoopFeed()
        {
            string str;
        Label_00CA:
            str = "";
            try
            {
                str = this.socketFeed.Read();
                if (str[0] == '.')
                {
                    this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.Panic, "", "Dtn.Adapter.MessageLoopFeed.: msg='" + str + "'"));
                }
                else if (str[0] == 'F')
                {
                    if (Globals.TraceSwitch.SymbolLookup)
                    {
                        Trace.WriteLine("(" + this.connection.IdPlus + ") Dtn.Adapter.MessageLoopFeed.F: msg='" + str + "'");
                    }
                    string[] strArray = str.Split(this.commaSplitter);
                    if (this.symbol2Lookup != null)
                    {
                        object obj2;
                        double tickSize = 0.01;
                        if (((obj2 = strArray[0x27]) != null) && ((obj2 = ImplementationDetails.HashTable1[obj2]) != null))
                        {
                            switch (((int) obj2))
                            {
                                case 0:
                                    tickSize = 0.125;
                                    break;

                                case 1:
                                    tickSize = 0.25;
                                    break;

                                case 2:
                                    tickSize = 0.03125;
                                    break;

                                case 3:
                                    tickSize = 0.015625;
                                    break;

                                case 4:
                                    tickSize = 0.0078125;
                                    break;

                                case 5:
                                    tickSize = 0.015625;
                                    break;

                                case 6:
                                    tickSize = 0.0078125;
                                    break;

                                case 7:
                                    tickSize = 0.015625;
                                    break;

                                case 8:
                                    tickSize = 0.0079365079365079361;
                                    break;

                                case 9:
                                    try
                                    {
                                        tickSize = 1.0 / Math.Pow(10.0, (double) Convert.ToInt32(strArray[40]));
                                    }
                                    catch
                                    {
                                    }
                                    break;
                            }
                        }
                        SplitDictionary splits = new SplitDictionary();
                        if (this.symbol2Lookup.SymbolType.Id == SymbolTypeId.Stock)
                        {
                            SymbolCollection symbols = Globals.DB.Select(this.symbol2Lookup.Name, null, null, this.symbol2Lookup.Expiry, this.symbol2Lookup.SymbolType, this.symbol2Lookup.Exchange, 0.0, RightId.Unknown, null);
                            if (symbols.Count > 0)
                            {
                                splits = symbols[0].Splits;
                            }
                            foreach (string str2 in new string[] { strArray[0x23], strArray[0x24] })
                            {
                                if (str2.Length != 0)
                                {
                                    string[] strArray2 = str2.Split(this.splitSplitter);
                                    int num2 = Convert.ToInt32(strArray2[3], this.cultureInfo);
                                    DateTime splitDate = new DateTime(num2 + ((num2 > 50) ? 1900 : 2000), Convert.ToInt32(strArray2[1], this.cultureInfo), Convert.ToInt32(strArray2[2], this.cultureInfo));
                                    if (!splits.Contains(splitDate))
                                    {
                                        splits.Add(splitDate, Convert.ToDouble(strArray2[0], this.cultureInfo));
                                    }
                                }
                            }
                        }
                        this.connection.CreateSymbol(this.symbol2Lookup.Name, this.symbol2Lookup.Expiry, this.symbol2Lookup.SymbolType, this.symbol2Lookup.Exchange, this.symbol2Lookup.StrikePrice, this.symbol2Lookup.Right.Id, this.connection.Currencies[CurrencyId.Unknown], tickSize, 1.0, strArray[0x18], null, 0, null, splits, null);
                        this.Unsubscribe(this.symbol2Lookup.MarketData);
                        this.symbol2Lookup = null;
                    }
                }
                else if (str[0] == 'Q')
                {
                    string[] strArray3 = str.Split(this.commaSplitter);
                    Symbol symbol = null;
                    if (strArray3[3] == "Not Found")
                    {
                        if (this.symbol2Lookup != null)
                        {
                            if (Globals.TraceSwitch.SymbolLookup)
                            {
                                Trace.WriteLine("(" + this.connection.IdPlus + ") Dtn.Adapter.MessageLoopFeed.Q: msg='" + str + "'");
                            }
                            this.connection.ProcessEventArgs(new SymbolEventArgs(this.connection, ErrorCode.NoSuchSymbol, "No such symbol", null));
                            this.Unsubscribe(this.symbol2Lookup.MarketData);
                            this.symbol2Lookup = null;
                        }
                    }
                    else
                    {
                        symbol = (Symbol) this.level1Tickers[strArray3[1]];
                        if (symbol != null)
                        {
                            if (Globals.TraceSwitch.MarketData)
                            {
                                Trace.WriteLine("(" + this.connection.IdPlus + ") Dtn.Adapter.MessageLoopFeed.Q: msg='" + str + "'");
                            }
                            double price = 0.0;
                            double num4 = 0.0;
                            int volume = 0;
                            int num6 = 0;
                            double num7 = 0.0;
                            double num8 = 0.0;
                            int num9 = 0;
                            DateTime now = this.connection.Now;
                            try
                            {
                                price = symbol.Round2TickSize((strArray3[11].Length > 0) ? Convert.ToDouble(strArray3[11], this.cultureInfo) : 0.0);
                            }
                            catch (Exception exception)
                            {
                                if (Globals.TraceSwitch.Strict)
                                {
                                    Trace.WriteLine("(" + this.connection.IdPlus + ") Dtn.Adapter.MessageLoopFeed: field[11]='" + strArray3[11] + "' msg='" + str + "':" + exception.Message);
                                }
                            }
                            try
                            {
                                num4 = symbol.Round2TickSize((strArray3[10].Length > 0) ? Convert.ToDouble(strArray3[10], this.cultureInfo) : 0.0);
                            }
                            catch (Exception exception2)
                            {
                                if (Globals.TraceSwitch.Strict)
                                {
                                    Trace.WriteLine("(" + this.connection.IdPlus + ") Dtn.Adapter.MessageLoopFeed: field[10]='" + strArray3[10] + "' msg='" + str + "':" + exception2.Message);
                                }
                            }
                            try
                            {
                                volume = (strArray3[13].Length > 0) ? Convert.ToInt32(strArray3[13], this.cultureInfo) : 0;
                            }
                            catch (Exception exception3)
                            {
                                if (Globals.TraceSwitch.Strict)
                                {
                                    Trace.WriteLine("(" + this.connection.IdPlus + ") Dtn.Adapter.MessageLoopFeed: field[13]='" + strArray3[13] + "' msg='" + str + "':" + exception3.Message);
                                }
                            }
                            try
                            {
                                num6 = (strArray3[12].Length > 0) ? Convert.ToInt32(strArray3[12], this.cultureInfo) : 0;
                            }
                            catch (Exception exception4)
                            {
                                if (Globals.TraceSwitch.Strict)
                                {
                                    Trace.WriteLine("(" + this.connection.IdPlus + ") Dtn.Adapter.MessageLoopFeed: field[12]='" + strArray3[12] + "' msg='" + str + "':" + exception4.Message);
                                }
                            }
                            try
                            {
                                num7 = symbol.Round2TickSize((strArray3[8].Length > 0) ? Convert.ToDouble(strArray3[8], this.cultureInfo) : 0.0);
                            }
                            catch (Exception exception5)
                            {
                                if (Globals.TraceSwitch.Strict)
                                {
                                    Trace.WriteLine("(" + this.connection.IdPlus + ") Dtn.Adapter.MessageLoopFeed: field[8]='" + strArray3[8] + "' msg='" + str + "':" + exception5.Message);
                                }
                            }
                            try
                            {
                                num8 = symbol.Round2TickSize((strArray3[9].Length > 0) ? Convert.ToDouble(strArray3[9], this.cultureInfo) : 0.0);
                            }
                            catch (Exception exception6)
                            {
                                if (Globals.TraceSwitch.Strict)
                                {
                                    Trace.WriteLine("(" + this.connection.IdPlus + ") Dtn.Adapter.MessageLoopFeed: field[9]='" + strArray3[9] + "' msg='" + str + "':" + exception6.Message);
                                }
                            }
                            try
                            {
                                num9 = (strArray3[6].Length > 0) ? Convert.ToInt32(strArray3[6], this.cultureInfo) : 0;
                            }
                            catch (Exception exception7)
                            {
                                if (Globals.TraceSwitch.Strict)
                                {
                                    Trace.WriteLine("(" + this.connection.IdPlus + ") Dtn.Adapter.MessageLoopFeed: field[6]='" + strArray3[6] + "' msg='" + str + "':" + exception7.Message);
                                }
                            }
                            if (strArray3[0x11].Length != 6)
                            {
                                if ((strArray3[0x11].Length > 0) && Globals.TraceSwitch.Strict)
                                {
                                    Trace.WriteLine("(" + this.connection.IdPlus + ") Dtn.Adapter.MessageLoopFeed: field[17]='" + strArray3[0x11] + "' msg='" + str + "'");
                                }
                            }
                            else if (strArray3[0x11].Substring(5, 1) == "t")
                            {
                                double num10 = 0.0;
                                int num11 = 0;
                                try
                                {
                                    num10 = symbol.Round2TickSize((strArray3[3].Length > 0) ? Convert.ToDouble(strArray3[3], this.cultureInfo) : 0.0);
                                }
                                catch (Exception exception8)
                                {
                                    if (Globals.TraceSwitch.Strict)
                                    {
                                        Trace.WriteLine("(" + this.connection.IdPlus + ") Dtn.Adapter.MessageLoopFeed: field[3]='" + strArray3[3] + "' msg='" + str + "':" + exception8.Message);
                                    }
                                }
                                try
                                {
                                    if (symbol.SymbolType.Id != SymbolTypeId.Index)
                                    {
                                        num11 = (strArray3[7].Length > 0) ? Convert.ToInt32(strArray3[7], this.cultureInfo) : 0;
                                    }
                                }
                                catch (Exception exception9)
                                {
                                    if (Globals.TraceSwitch.Strict)
                                    {
                                        Trace.WriteLine("(" + this.connection.IdPlus + ") Dtn.Adapter.MessageLoopFeed: field[7]='" + strArray3[7] + "' msg='" + str + "':" + exception9.Message);
                                    }
                                }
                                if (num11 < 0)
                                {
                                    num11 = 0;
                                }
                                if (num10 > 0.0)
                                {
                                    this.connection.ProcessEventArgs(new MarketDataEventArgs(symbol.MarketData, ErrorCode.NoError, "", symbol, this.typeLast, num10, num11, now));
                                }
                            }
                            if ((price > 0.0) && (((symbol.MarketData.Ask == null) || (symbol.MarketData.Ask.Price != price)) || (symbol.MarketData.Ask.Volume != volume)))
                            {
                                this.connection.ProcessEventArgs(new MarketDataEventArgs(symbol.MarketData, ErrorCode.NoError, "", symbol, this.typeAsk, price, volume, now));
                            }
                            if ((num4 > 0.0) && (((symbol.MarketData.Bid == null) || (symbol.MarketData.Bid.Price != num4)) || (symbol.MarketData.Bid.Volume != num6)))
                            {
                                this.connection.ProcessEventArgs(new MarketDataEventArgs(symbol.MarketData, ErrorCode.NoError, "", symbol, this.typeBid, num4, num6, now));
                            }
                            if ((symbol.MarketData.Opening == null) && (strArray3[0x13].Length > 0))
                            {
                                double num12 = 0.0;
                                try
                                {
                                    num12 = symbol.Round2TickSize(Convert.ToDouble(strArray3[0x13], this.cultureInfo));
                                }
                                catch (Exception)
                                {
                                }
                                if (num12 > 0.0)
                                {
                                    this.connection.ProcessEventArgs(new MarketDataEventArgs(symbol.MarketData, ErrorCode.NoError, "", symbol, this.typeOpening, num12, 0, now));
                                }
                            }
                            if ((symbol.MarketData.LastClose == null) && (strArray3[20].Length > 0))
                            {
                                double num13 = 0.0;
                                try
                                {
                                    num13 = symbol.Round2TickSize(Convert.ToDouble(strArray3[20], this.cultureInfo));
                                }
                                catch (Exception)
                                {
                                }
                                if (num13 > 0.0)
                                {
                                    this.connection.ProcessEventArgs(new MarketDataEventArgs(symbol.MarketData, ErrorCode.NoError, "", symbol, this.typeLastClose, num13, 0, now));
                                }
                            }
                            if ((num7 > 0.0) && ((symbol.MarketData.DailyHigh == null) || (symbol.MarketData.DailyHigh.Price < num7)))
                            {
                                this.connection.ProcessEventArgs(new MarketDataEventArgs(symbol.MarketData, ErrorCode.NoError, "", symbol, this.typeDailyHigh, num7, 0, now));
                            }
                            if ((num8 > 0.0) && ((symbol.MarketData.DailyLow == null) || (symbol.MarketData.DailyLow.Price < num8)))
                            {
                                this.connection.ProcessEventArgs(new MarketDataEventArgs(symbol.MarketData, ErrorCode.NoError, "", symbol, this.typeDailyLow, num8, 0, now));
                            }
                            if ((num9 > 0) && ((symbol.MarketData.DailyVolume == null) || (symbol.MarketData.DailyVolume.Volume < num9)))
                            {
                                this.connection.ProcessEventArgs(new MarketDataEventArgs(symbol.MarketData, ErrorCode.NoError, "", symbol, this.typeDailyVolume, 0.0, num9, now));
                            }
                        }
                    }
                }
                else if (str[0] == 'S')
                {
                    if (Globals.TraceSwitch.Connect)
                    {
                        Trace.WriteLine("(" + this.connection.IdPlus + ") Dtn.Adapter.MessageLoopFeed.S: msg='" + str + "'");
                    }
                    string[] strArray4 = str.Split(this.dataSplitter);
                    if (((strArray4[1] == "SERVER") && (strArray4[2] == "DISCONNECTED")) && (this.connection.ConnectionStatusId == ConnectionStatusId.Connected))
                    {
                        this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, ErrorCode.NoError, "Unexpected disconnect from server. Retrying connect ...", ConnectionStatusId.ConnectionLost, ConnectionStatusId.ConnectionLost, 0, ""));
                    }
                    else if (((strArray4[1] == "SERVER") && (strArray4[2] == "CONNECTED")) && (this.connection.ConnectionStatusId == ConnectionStatusId.ConnectionLost))
                    {
                        this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, ErrorCode.NoError, "", ConnectionStatusId.Connected, ConnectionStatusId.Connected, 0, ""));
                    }
                }
                else if (str[0] == 'T')
                {
                    if (Globals.TraceSwitch.Timer)
                    {
                        Trace.WriteLine("(" + this.connection.IdPlus + ") Dtn.Adapter.MessageLoopFeed.T: msg='" + str + "'");
                    }
                    string[] strArray5 = str.Split(this.dataSplitter);
                    DateTime time = this.connection.Now;
                    try
                    {
                        time = new DateTime(Convert.ToInt32(strArray5[1].Substring(0, 4), this.cultureInfo), Convert.ToInt32(strArray5[1].Substring(4, 2), this.cultureInfo), Convert.ToInt32(strArray5[1].Substring(6, 2), this.cultureInfo), Convert.ToInt32(strArray5[2], this.cultureInfo), Convert.ToInt32(strArray5[3], this.cultureInfo), 0);
                    }
                    catch (Exception exception10)
                    {
                        this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.Panic, "", "Dtn.Adapter.MessageLoopFeedT1 msg='" + str + "': " + exception10.Message));
                        goto Label_00CA;
                    }
                    this.connection.ProcessEventArgs(new TimerEventArgs(this.connection, ErrorCode.NoError, "", time, true));
                }
                goto Label_00CA;
            }
            catch (SocketClosedException exception11)
            {
                if (!this.closedByClient)
                {
                    this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, ErrorCode.ServerConnectionIsBroken, "DTN closed socket (" + exception11.Message + ")", ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                    this.Cleanup();
                }
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception exception12)
            {
                this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.Panic, "", "Dtn.Adapter.MessageLoopFeed msg='" + str + "': " + exception12.Message));
                goto Label_00CA;
            }
        }

        private void MessageLoopLevel2()
        {
            string str;
        Label_0000:
            str = "";
            try
            {
                str = this.socketLevel2.Read();
                if (str[0] == '.')
                {
                    this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.Panic, "", "Dtn.Adapter.MessageLoopLevel2: msg='" + str + "'"));
                    goto Label_0000;
                }
                if (str[0] == 'E')
                {
                    string[] strArray = str.Split(this.commaSplitter);
                    this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.Panic, "", strArray[2] + "(" + strArray[1] + ")"));
                    goto Label_0000;
                }
                if (str[0] != 'U')
                {
                    goto Label_0000;
                }
                if (Globals.TraceSwitch.MarketDepth)
                {
                    Trace.WriteLine("(" + this.connection.IdPlus + ") Dtn.Adapter.MessageLoopLevel2: msg='" + str + "'");
                }
                string[] strArray2 = str.Split(this.commaSplitter);
                Symbol symbol = (Symbol) this.level2Tickers[strArray2[1]];
                if (symbol != null)
                {
                    double askPrice = 0.0;
                    double bidPrice = 0.0;
                    int askSize = 0;
                    int bidSize = 0;
                    DateTime now = this.connection.Now;
                    if (strArray2[10] != "L")
                    {
                        try
                        {
                            askPrice = Convert.ToDouble(strArray2[4], this.cultureInfo);
                            bidPrice = Convert.ToDouble(strArray2[3], this.cultureInfo);
                            askSize = Convert.ToInt32(strArray2[6], this.cultureInfo);
                            bidSize = Convert.ToInt32(strArray2[5], this.cultureInfo);
                            now = new DateTime(Convert.ToInt32(strArray2[8].Substring(0, 4), this.cultureInfo), Convert.ToInt32(strArray2[8].Substring(5, 2), this.cultureInfo), Convert.ToInt32(strArray2[8].Substring(8, 2), this.cultureInfo), Convert.ToInt32(strArray2[7].Substring(0, 2), this.cultureInfo), Convert.ToInt32(strArray2[7].Substring(3, 2), this.cultureInfo), Convert.ToInt32(strArray2[7].Substring(6, 2), this.cultureInfo));
                        }
                        catch (Exception exception)
                        {
                            Trace.WriteLine("WARNING: Dtn.Adapter.MessageLoopLevel2 msg='" + str + "': " + exception.Message);
                        }
                    }
                    if (Globals.TraceSwitch.MarketDepth)
                    {
                        Trace.WriteLine(string.Concat(new object[] { "(", this.connection.IdPlus, ") Dtn.Adapter.MessageLoopLevel2: symbol='", symbol.FullName, "'  time='", now, "' ask=", askPrice, "/", askSize, " bid=", bidPrice, "/", bidSize }));
                    }
                    symbol.MarketDepth.Update(strArray2[2], null, askPrice, askSize, bidPrice, bidSize, now);
                    goto Label_0000;
                }
            }
            catch (SocketClosedException exception2)
            {
                if (!this.closedByClient)
                {
                    this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, ErrorCode.ServerConnectionIsBroken, "DTN closed socket (" + exception2.Message + ")", ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                    this.Cleanup();
                }
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception exception3)
            {
                this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.Panic, "", "Dtn.Adapter.MessageLoopLevel2 msg='" + str + "': " + exception3.Message));
                goto Label_0000;
            }
        }

        private void MessageLoopLookup()
        {
            string[] strArray = null;
            string[] strArray2 = null;
            string str;
            DateTime now = DateTime.Now;
            double high = 0.0;
            double low = 0.0;
            double open = 0.0;
            double close = 0.0;
            int volume = 0;
        Label_0038:
            str = "";
            try
            {
                Quotes quotes2;
                str = this.socketLookup.Read();
                if (str[0] != '.')
                {
                    goto Label_01C9;
                }
                if (str.IndexOf("..Error") >= 0)
                {
                    while (this.socketLookup.Read() != "!ENDMSG!")
                    {
                    }
                    if (this.currentLookupRequest[0] == 'H')
                    {
                        Globals.Progress.Terminate();
                        Quotes quotes = this.quotes2Lookup;
                        this.quotes2Lookup = null;
                        this.NextLookupRequest("-");
                        if (str.IndexOf("unavailable") >= 0)
                        {
                            this.connection.ProcessEventArgs(new BarUpdateEventArgs(this.connection, ErrorCode.Panic, str, Operation.Insert, quotes, 0, -1));
                            goto Label_0038;
                        }
                        if (Globals.TraceSwitch.Quotes)
                        {
                            Trace.WriteLine("(" + this.connection.IdPlus + ") Dtn.Adapter.MessageLoopLookup.Error: retrying, " + this.quotes2Lookup.ToString() + " msg='" + str + "'");
                        }
                        this.Request(quotes);
                    }
                }
                else
                {
                    if (!(str != "..NONE.."))
                    {
                        goto Label_01AB;
                    }
                    this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.Panic, "", "Dtn.Adapter.MessageLoopLookup0: msg='" + str + "'"));
                }
                goto Label_01B9;
            Label_019E:
                str = this.socketLookup.Read();
            Label_01AB:
                if (str != "!ENDMSG!")
                {
                    goto Label_019E;
                }
            Label_01B9:
                this.NextLookupRequest("-");
                goto Label_0038;
            Label_01C9:
                if ((this.currentLookupRequest[0] == 'N') || (str[0] == '<'))
                {
                    ArrayList list;
                    if (Globals.TraceSwitch.News)
                    {
                        Trace.WriteLine("(" + this.connection.IdPlus + ") Dtn.Adapter.MessageLoopLookup.<: request='" + this.currentLookupRequest + "' msg='" + str + "'");
                    }
                    string s = "";
                    while (str != "!ENDMSG!")
                    {
                        s = s + str;
                        str = this.socketLookup.Read();
                    }
                    this.NextLookupRequest("-");
                    XmlDocument document = new XmlDocument();
                    try
                    {
                        XmlTextReader reader = new XmlTextReader(new StringReader(s));
                        document.Load(reader);
                        reader.Close();
                    }
                    catch (Exception exception)
                    {
                        this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.Panic, "", "Dtn.Adapter.MessageLoopLookup.Xml: msg='" + s + "': " + exception.Message));
                        goto Label_0038;
                    }
                    if (document["DynamicNewsConf"] != null)
                    {
                        foreach (XmlNode node in document["DynamicNewsConf"])
                        {
                            foreach (XmlNode node2 in node)
                            {
                                NewsItemType type = this.connection.NewsItemTypes.FindByMapId(node2.Attributes["type"].Value);
                                if (type == null)
                                {
                                    Trace.WriteLine("WARNING: news provider '" + node2.Attributes["type"].Value + "' not supported by TradeMagic");
                                }
                                else
                                {
                                    this.newsProviders.Add(type);
                                }
                            }
                        }
                        this.RequestNewsHeadlines(true);
                        this.newsIntervalSeconds = this.Options.NewsIntervalSeconds;
                        goto Label_0038;
                    }
                    if (document["news_headlines"] != null)
                    {
                        foreach (XmlNode node3 in document["news_headlines"])
                        {
                            string innerText = node3["id"].InnerText;
                            string mapId = node3["source"].InnerText;
                            string str5 = node3["timestamp"].InnerText;
                            string headLine = node3["text"].InnerText;
                            if (this.newsIds[innerText] == null)
                            {
                                try
                                {
                                    now = new DateTime(Convert.ToInt32(str5.Substring(0, 4)), Convert.ToInt32(str5.Substring(4, 2)), Convert.ToInt32(str5.Substring(6, 2)), Convert.ToInt32(str5.Substring(8, 2)), Convert.ToInt32(str5.Substring(10, 2)), Convert.ToInt32(str5.Substring(12, 2)));
                                }
                                catch (Exception exception2)
                                {
                                    Trace.WriteLine("WARNING: invalid timestamp on news item '" + innerText + "': " + exception2.Message);
                                }
                                NewsItemType itemType = this.connection.NewsItemTypes.FindByMapId(mapId);
                                if (itemType == null)
                                {
                                    Trace.WriteLine("WARNING: news provider '" + mapId + "' not supported by TradeMagic");
                                }
                                else
                                {
                                    this.newsIds.Add(innerText, innerText);
                                    lock ((list = this.newsStories))
                                    {
                                        this.newsStories.Add(new NewsEventArgs(this.connection, ErrorCode.NoError, "", innerText, itemType, now, headLine, ""));
                                        if (this.newsStories.Count == 1)
                                        {
                                            this.RetrieveNewsStory();
                                        }
                                        continue;
                                    }
                                }
                            }
                        }
                        goto Label_0038;
                    }
                    if (document["news_stories"] == null)
                    {
                        goto Label_0038;
                    }
                    string newsText = "";
                    foreach (XmlNode node4 in document["news_stories"])
                    {
                        newsText = newsText + node4["story_text"].InnerText;
                    }
                    lock ((list = this.newsStories))
                    {
                        NewsEventArgs args = (NewsEventArgs) this.newsStories[0];
                        this.newsStories.RemoveAt(0);
                        if (newsText.Length > 0)
                        {
                            this.connection.ProcessEventArgs(new NewsEventArgs(this.connection, ErrorCode.NoError, "", args.Id, args.ItemType, args.Time, args.HeadLine, newsText));
                        }
                        this.RetrieveNewsStory();
                        goto Label_0038;
                    }
                }
                if (this.currentLookupRequest[0] == 'H')
                {
                    if (Globals.TraceSwitch.Quotes)
                    {
                        Trace.WriteLine("(" + this.connection.IdPlus + ") Dtn.Adapter.MessageLoopLookup.Q: request='" + this.currentLookupRequest + "' msg='" + str + "'");
                    }
                    quotes2 = null;
                    if (this.quotes2Lookup != null)
                    {
                        goto Label_076F;
                    }
                }
                goto Label_0038;
            Label_0754:
                this.quotesBuf.Add(str);
                str = this.socketLookup.Read();
            Label_076F:
                if (str != "!ENDMSG!")
                {
                    goto Label_0754;
                }
                this.NextLookupRequest("-");
                for (int i = this.quotesBuf.Count - 1; i >= 0; i--)
                {
                    try
                    {
                        strArray = ((string) this.quotesBuf[i]).Split(this.commaSplitter);
                        strArray2 = strArray[0].Split(this.splitter2);
                        if (this.quotes2Lookup.Period.Id == PeriodTypeId.Day)
                        {
                            now = new DateTime(Convert.ToInt32(strArray2[0]), Convert.ToInt32(strArray2[1]), Convert.ToInt32(strArray2[2]));
                            double tickSize = this.quotes2Lookup.Symbol.TickSize / this.quotes2Lookup.Symbol.Splits.GetSplitFactor(now.Date);
                            high = Symbol.Round2TickSize(Convert.ToDouble(strArray[1], this.cultureInfo), tickSize);
                            low = Symbol.Round2TickSize(Convert.ToDouble(strArray[2], this.cultureInfo), tickSize);
                            open = Symbol.Round2TickSize(Convert.ToDouble(strArray[3], this.cultureInfo), tickSize);
                            close = Symbol.Round2TickSize(Convert.ToDouble(strArray[4], this.cultureInfo), tickSize);
                            volume = Convert.ToInt32(strArray[5], this.cultureInfo);
                        }
                        else if (this.quotes2Lookup.Period.Id == PeriodTypeId.Minute)
                        {
                            now = new DateTime(Convert.ToInt32(strArray2[0]), Convert.ToInt32(strArray2[1]), Convert.ToInt32(strArray2[2]), Convert.ToInt32(strArray2[3]), Convert.ToInt32(strArray2[4]), Convert.ToInt32(strArray2[5]));
                            high = Symbol.Round2TickSize(Convert.ToDouble(strArray[1], this.cultureInfo), this.quotes2Lookup.Symbol.TickSize);
                            low = Symbol.Round2TickSize(Convert.ToDouble(strArray[2], this.cultureInfo), this.quotes2Lookup.Symbol.TickSize);
                            open = Symbol.Round2TickSize(Convert.ToDouble(strArray[3], this.cultureInfo), this.quotes2Lookup.Symbol.TickSize);
                            close = Symbol.Round2TickSize(Convert.ToDouble(strArray[4], this.cultureInfo), this.quotes2Lookup.Symbol.TickSize);
                            volume = Convert.ToInt32(strArray[6], this.cultureInfo);
                        }
                        else
                        {
                            now = new DateTime(Convert.ToInt32(strArray2[0]), Convert.ToInt32(strArray2[1]), Convert.ToInt32(strArray2[2]), Convert.ToInt32(strArray2[3]), Convert.ToInt32(strArray2[4]), Convert.ToInt32(strArray2[5]));
                            high = Symbol.Round2TickSize(Convert.ToDouble(strArray[1], this.cultureInfo), this.quotes2Lookup.Symbol.TickSize);
                            low = high;
                            open = high;
                            close = high;
                            int num8 = Convert.ToInt32(strArray[3], this.cultureInfo);
                            if (this.lastTotalVolume == 0)
                            {
                                volume = (this.quotes2Lookup.Symbol.SymbolType.Id == SymbolTypeId.Stock) ? 100 : 1;
                            }
                            else
                            {
                                volume = num8 - this.lastTotalVolume;
                            }
                            this.lastTotalVolume = num8;
                        }
                    }
                    catch (Exception exception3)
                    {
                        this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.Panic, "", "Dtn.Adapter.MessageLoopLookup msg='" + ((string) this.quotesBuf[i]) + "': " + exception3.Message));
                    }
                    if (volume < 0)
                    {
                        volume = 0;
                    }
                    if (((((now.Date >= this.quotes2Lookup.From) && (this.quotes2Lookup.To >= now.Date)) && ((open > 0.0) && (high > 0.0))) && (((low > 0.0) && (close > 0.0)) && ((open >= low) && (high >= low)))) && (((close >= low) && (open <= high)) && ((low <= high) && (close <= high))))
                    {
                        this.quotes2Lookup.Bars.Add(open, high, low, close, now, volume, false);
                        if ((this.quotes2Lookup.Bars.Count % 100) == 0)
                        {
                            Globals.Progress.Message = this.quotes2Lookup.Symbol.FullName + " " + this.quotes2Lookup.Bars[this.quotes2Lookup.Bars.Count - 1].Time.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                    }
                }
                Globals.Progress.Terminate();
                quotes2 = this.quotes2Lookup;
                this.quotes2Lookup = null;
                this.connection.ProcessEventArgs(new BarUpdateEventArgs(this.connection, ErrorCode.NoError, "", Operation.Insert, quotes2, 0, quotes2.Bars.Count - 1));
                goto Label_0038;
            }
            catch (SocketClosedException exception4)
            {
                if (!this.closedByClient)
                {
                    this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, ErrorCode.ServerConnectionIsBroken, "DTN closed socket (" + exception4.Message + ")", ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                    this.Cleanup();
                }
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception exception5)
            {
                this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.Panic, "", "Dtn.Adapter.MessageLoopLookup msg='" + str + "': " + exception5.Message));
                goto Label_0038;
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

        private void MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.X == 0) && (e.Y == 0))
            {
                if (Globals.TraceSwitch.Connect)
                {
                    Trace.WriteLine("(" + this.connection.IdPlus + ") Dtn.Adapter.MouseMove");
                }
                if (this.socketFeed == null)
                {
                    this.TryConnect(true);
                }
            }
        }

        internal void NextLookupRequest(string request)
        {
            lock (this.lookupRequests)
            {
                if (request != "-")
                {
                    this.lookupRequests.Add(request);
                }
                else
                {
                    this.currentLookupRequest = "-";
                }
                if ((this.currentLookupRequest == "-") && (this.lookupRequests.Count > 0))
                {
                    this.currentLookupRequest = this.lookupRequests[0];
                    this.lookupRequests.RemoveAt(0);
                    this.socketLookup.Write(this.currentLookupRequest);
                }
            }
        }

        private void Progress_Aborted(object sender, EventArgs e)
        {
            if (Globals.TraceSwitch.Quotes)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") Dtn.Adapter.Progress_Aborted " + this.quotes2Lookup.ToString());
            }
            Quotes quotes = this.quotes2Lookup;
            this.quotes2Lookup = null;
            this.NextLookupRequest("-");
            this.connection.ProcessEventArgs(new BarUpdateEventArgs(this.connection, ErrorCode.UserAbort, "Aborted by user", Operation.Insert, quotes, 0, -1));
        }

        [DllImport("IQ32.dll")]
        private static extern int RegisterClientApp(IntPtr hWnd, string productName, string productKey, string productVersion);
        [DllImport("IQ32.dll")]
        private static extern void RemoveClientApp(IntPtr hWnd);
        public void Request(Quotes quotes)
        {
            int num = ((int) DateTime.Now.Date.Subtract(quotes.From).TotalDays) + 1;
            string request = "";
            string str2 = "";
            if (quotes.Period.Id == PeriodTypeId.Day)
            {
                request = string.Concat(new object[] { "HD,", this.ToBrokerName(quotes.Symbol), ",", num, ";" });
                str2 = "Retrieving daily data for '" + quotes.Symbol.FullName + "'";
            }
            else if (quotes.Period.Id == PeriodTypeId.Minute)
            {
                request = string.Concat(new object[] { "HM,", this.ToBrokerName(quotes.Symbol), ",", num, ",1;" });
                str2 = "Retrieving minute data for '" + quotes.Symbol.FullName + "'";
            }
            else if (quotes.Period.Id == PeriodTypeId.Tick)
            {
                request = string.Concat(new object[] { "HT,", this.ToBrokerName(quotes.Symbol), ",", num, ";" });
                str2 = "Retrieving tick data for '" + quotes.Symbol.FullName + "'";
            }
            this.lastTotalVolume = 0;
            this.quotes2Lookup = quotes;
            this.quotesBuf.Clear();
            Globals.Progress.Initialise(0, true);
            Globals.Progress.Aborted += new AbortEventHandler(this.Progress_Aborted);
            Globals.Progress.Message = str2;
            if (Globals.TraceSwitch.Quotes)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") Dtn.Adapter.Request: msg='" + request + "' " + quotes.ToString());
            }
            this.NextLookupRequest(request);
        }

        private void RequestNewsHeadlines(bool initial)
        {
            foreach (NewsItemType type in this.newsProviders)
            {
                string request = string.Concat(new object[] { "NH:", type.MapId, ":", this.Options.NewsMaxBack, ":", initial ? this.connection.Now.AddDays((double) -this.Options.NewsDaysBack).ToString("yyyyMMdd") : "0", ";" });
                if (Globals.TraceSwitch.News)
                {
                    Trace.WriteLine("(" + this.connection.IdPlus + ") Dtn.Adapter.RequestNewsHeadlines: msg='" + request + "'");
                }
                this.NextLookupRequest(request);
            }
        }

        private void RetrieveNewsStory()
        {
            lock (this.newsStories)
            {
                if (this.newsStories.Count != 0)
                {
                    NewsEventArgs args = (NewsEventArgs) this.newsStories[0];
                    string request = "NN:" + args.Id + ":" + args.ItemType.MapId + ":" + args.Time.ToString("yyyyMMdd") + ";";
                    if (Globals.TraceSwitch.News)
                    {
                        Trace.WriteLine("(" + this.connection.IdPlus + ") Dtn.Adapter.RetrieveNewsStory: msg='" + request + "'");
                    }
                    this.NextLookupRequest(request);
                }
            }
        }

        public void Subscribe(MarketData marketData)
        {
            if (Globals.TraceSwitch.MarketData)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") Dtn.Adapter.Subscribe.MarketData: symbol='" + marketData.Symbol.FullName + "'");
            }
            string key = this.ToBrokerName(marketData.Symbol);
            lock (this.level1Tickers)
            {
                this.level1Tickers.Add(key, marketData.Symbol);
            }
            lock (this.socketFeed)
            {
                this.socketFeed.Write("w" + key + "\n");
            }
        }

        public void Subscribe(MarketDepth marketDepth)
        {
            if (Globals.TraceSwitch.MarketDepth)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") Dtn.Adapter.Subscribe.MarketDepth: symbol='" + marketDepth.Symbol.FullName + "'");
            }
            string key = this.ToBrokerName(marketDepth.Symbol);
            lock (this.level2Tickers)
            {
                this.level2Tickers.Add(key, marketDepth.Symbol);
            }
            lock (this.socketLevel2)
            {
                this.socketLevel2.Write("w" + key + "\r\n");
            }
        }

        public void SymbolLookup(Symbol template)
        {
            if (Globals.TraceSwitch.SymbolLookup)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") Dtn.Adapter.SymbolLookup: symbol='" + template.FullName + "'");
            }
            lock (this.level1Tickers)
            {
                this.symbol2Lookup = (Symbol) this.level1Tickers[this.ToBrokerName(template)];
                if (this.symbol2Lookup != null)
                {
                    this.symbol2Lookup = new Symbol(null, template.Name, template.Expiry, template.SymbolType, template.Exchange, template.StrikePrice, template.Right.Id, this.symbol2Lookup.Currency, this.symbol2Lookup.TickSize, this.symbol2Lookup.PointValue, this.symbol2Lookup.CompanyName, this.symbol2Lookup.Exchanges, this.symbol2Lookup.Splits, this.symbol2Lookup.Dividends);
                    new Thread(new ThreadStart(this.SymbolLookupNow)).Start();
                }
                else
                {
                    this.symbol2Lookup = template;
                    this.Subscribe(template.MarketData);
                }
            }
        }

        private void SymbolLookupNow()
        {
            this.connection.CreateSymbol(this.symbol2Lookup.Name, this.symbol2Lookup.Expiry, this.symbol2Lookup.SymbolType, this.symbol2Lookup.Exchange, this.symbol2Lookup.StrikePrice, this.symbol2Lookup.Right.Id, this.connection.Currencies[CurrencyId.Unknown], this.symbol2Lookup.TickSize, this.symbol2Lookup.PointValue, this.symbol2Lookup.CompanyName, null, 0, this.symbol2Lookup.Exchanges, this.symbol2Lookup.Splits, this.symbol2Lookup.Dividends);
            this.symbol2Lookup = null;
        }

        private string ToBrokerName(Symbol symbol)
        {
            string str = symbol.GetProviderName(ProviderTypeId.Dtn).ToUpper();
            if ((symbol.SymbolType.Id == SymbolTypeId.Stock) && ((symbol.Exchange.Id == ExchangeId.Tsx) || (symbol.Exchange.Id == ExchangeId.TsxV)))
            {
                str = "C." + str;
            }
            if (symbol.SymbolType.Id == SymbolTypeId.Future)
            {
                str = str + ((symbol.Expiry == Globals.ContinousContractExpiry) ? "#" : (this.MonthCode(symbol.Expiry) + (symbol.Expiry.Year % 10)));
            }
            return str;
        }

        private bool TryConnect(bool statusEvent)
        {
            if (Globals.TraceSwitch.Connect)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") Dtn.Adapter.TryConnect");
            }
            try
            {
                if (this.socketFeed != null)
                {
                    this.socketFeed.Close();
                }
            }
            finally
            {
                this.socketFeed = null;
            }
            try
            {
                if (this.socketLevel2 != null)
                {
                    this.socketLevel2.Close();
                }
            }
            finally
            {
                this.socketLevel2 = null;
            }
            try
            {
                if (this.socketLookup != null)
                {
                    this.socketLookup.Close();
                }
            }
            finally
            {
                this.socketLookup = null;
            }
            try
            {
                this.socketFeed = new DtnSocket(this, this.Options.Host, this.Options.PortFeed);
                this.socketLevel2 = new DtnSocket(this, this.Options.Host, this.Options.PortLevel2);
                this.socketLookup = new DtnSocket(this, this.Options.Host, this.Options.PortLookup);
                this.socketFeed.Connect();
                this.socketLevel2.Connect();
                this.socketLookup.Connect();
                this.socketFeed.Read();
                this.socketFeed.Read();
                this.socketFeed.Read();
                this.socketFeed.Read();
                this.socketFeed.Read();
                lock (this.socketFeed)
                {
                    this.socketFeed.Write("S,KEY\r\n");
                }
                this.threadFeed = new Thread(new ThreadStart(this.MessageLoopFeed));
                this.threadFeed.Name = "TM DTN Feed";
                this.threadFeed.Start();
                this.threadLevel2 = new Thread(new ThreadStart(this.MessageLoopLevel2));
                this.threadLevel2.Name = "TM DTN Level2";
                this.threadLevel2.Start();
                this.threadLookup = new Thread(new ThreadStart(this.MessageLoopLookup));
                this.threadLookup.Name = "TM DTN Lookup";
                this.threadLookup.Start();
            }
            catch (Exception exception)
            {
                try
                {
                    if (this.socketFeed != null)
                    {
                        this.socketFeed.Close();
                    }
                }
                finally
                {
                    this.socketFeed = null;
                }
                try
                {
                    if (this.socketLevel2 != null)
                    {
                        this.socketLevel2.Close();
                    }
                }
                finally
                {
                    this.socketLevel2 = null;
                }
                try
                {
                    if (this.socketLookup != null)
                    {
                        this.socketLookup.Close();
                    }
                }
                finally
                {
                    this.socketLookup = null;
                }
                if (statusEvent)
                {
                    this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, ErrorCode.InvalidLicense, "Failed to create sockets: " + exception.Message, ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                }
                return false;
            }
            this.Init();
            this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, ErrorCode.NoError, "", ConnectionStatusId.Connected, ConnectionStatusId.Connected, 0, ""));
            this.NextLookupRequest("NC;");
            return true;
        }

        public void Unsubscribe(MarketData marketData)
        {
            if (Globals.TraceSwitch.MarketData)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") Dtn.Adapter.Unsubscribe.MarketData: symbol='" + marketData.Symbol.FullName + "'");
            }
            string key = this.ToBrokerName(marketData.Symbol);
            lock (this.level1Tickers)
            {
                this.level1Tickers.Remove(key);
            }
            lock (this.socketFeed)
            {
                this.socketFeed.Write("r" + key + "\n");
            }
        }

        public void Unsubscribe(MarketDepth marketDepth)
        {
            if (Globals.TraceSwitch.MarketDepth)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") Dtn.Adapter.Unsubscribe.MarketDepth: symbol='" + marketDepth.Symbol.FullName + "'");
            }
            string key = this.ToBrokerName(marketDepth.Symbol);
            lock (this.level2Tickers)
            {
                this.level2Tickers.Remove(key);
            }
            lock (this.socketLevel2)
            {
                this.socketLevel2.Write("r" + key + "\r\n");
            }
        }

        private DtnOptions Options
        {
            get
            {
                return (DtnOptions) this.connection.Options;
            }
        }
    }
}

