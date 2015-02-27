using iTrading.Core.Data;

namespace iTrading.CT
{
    using System;
    using System.Collections;
    using System.Diagnostics;
   using iTrading.Core.Kernel;
    using iTrading.Core.Data;

    internal class Callback
    {
        private Adapter adapter;
        private Hashtable newsHeadlines = new Hashtable();
        private char[] timeSplitter = new char[] { '/', ':' };
        internal MarketDataType typeAsk = null;
        internal MarketDataType typeBid = null;
        internal MarketDataType typeDailyHigh = null;
        internal MarketDataType typeDailyLow = null;
        internal MarketDataType typeDailyVolume = null;
        internal MarketDataType typeLast = null;
        internal MarketDataType typeLastClose = null;
        internal MarketDataType typeOpening = null;

        internal Callback(Adapter adapter)
        {
            this.adapter = adapter;
        }

        internal unsafe void DataCallback(Feed feed, DataType type, byte* bytes)
        {
            Hashtable hashtable;
            if (Globals.TraceSwitch.Native)
            {
                Trace.WriteLine(string.Concat(new object[] { "(", this.adapter.connection.IdPlus, ") CT.Callback.DataCallback: feed=", feed, " type=", type }));
            }
            ByteReader reader = new ByteReader(this.adapter, bytes);
            if (feed == Feed.Level1)
            {
                try
                {
                    reader.ReadInteger();
                    short num = reader.ReadShort();
                    string str = reader.ReadString(9);
                    reader.ReadByte();
                    double num2 = reader.ReadDouble();
                    int num3 = reader.ReadInteger();
                    reader.Skip(4);
                    double price = reader.ReadDouble();
                    reader.ReadDouble();
                    double num5 = reader.ReadDouble();
                    int num6 = reader.ReadInteger();
                    reader.Skip(4);
                    double num7 = reader.ReadDouble();
                    int num8 = reader.ReadInteger();
                    reader.Skip(4);
                    double num9 = reader.ReadDouble();
                    double num10 = reader.ReadDouble();
                    int volume = reader.ReadInteger();
                    reader.ReadChar();
                    reader.Skip(3);
                    reader.ReadDouble();
                    if (str == "$TIME")
                    {
                        DateTime time = reader.ReadTime();
                        this.adapter.connection.ProcessEventArgs(new TimerEventArgs(this.adapter.connection, ErrorCode.NoError, "", time, true));
                    }
                    else
                    {
                        MarketData marketData = null;
                        lock ((hashtable = this.adapter.symbol2MarketData))
                        {
                            marketData = (MarketData) this.adapter.symbol2MarketData[str];
                        }
                        if (marketData != null)
                        {
                            if (Globals.TraceSwitch.MarketData)
                            {
                                Trace.WriteLine("(" + this.adapter.connection.IdPlus + ") CT.Callback.DataCallback.MarketData: " + marketData.Symbol.FullName);
                            }
                            double num12 = ((num & 0x20) != 0) ? num7 : ((marketData.Ask == null) ? 0.0 : marketData.Ask.Price);
                            int num13 = ((num & 0x40) != 0) ? num8 : ((marketData.Ask == null) ? 0 : marketData.Ask.Volume);
                            double num14 = ((num & 8) != 0) ? num5 : ((marketData.Bid == null) ? 0.0 : marketData.Bid.Price);
                            int num15 = ((num & 0x10) != 0) ? num6 : ((marketData.Bid == null) ? 0 : marketData.Bid.Volume);
                            double num16 = ((num & 1) != 0) ? num2 : ((marketData.Last == null) ? 0.0 : marketData.Last.Price);
                            int num17 = ((num & 0x800) != 0) ? num3 : ((marketData.Last == null) ? 0 : marketData.Last.Volume);
                            if (((num16 > 0.0) && (num17 >= 0)) && (((num & 1) != 0) || ((num & 0x800) != 0)))
                            {
                                this.adapter.connection.ProcessEventArgs(new MarketDataEventArgs(marketData, ErrorCode.NoError, "", marketData.Symbol, this.typeLast, marketData.Symbol.Round2TickSize(num16), num17, this.adapter.connection.Now));
                            }
                            if (((num12 > 0.0) && (num13 >= 0)) && (((num & 0x20) != 0) || ((num & 0x40) != 0)))
                            {
                                this.adapter.connection.ProcessEventArgs(new MarketDataEventArgs(marketData, ErrorCode.NoError, "", marketData.Symbol, this.typeAsk, marketData.Symbol.Round2TickSize(num12), num13, this.adapter.connection.Now));
                            }
                            if (((num14 > 0.0) && (num15 >= 0)) && (((num & 8) != 0) || ((num & 0x10) != 0)))
                            {
                                this.adapter.connection.ProcessEventArgs(new MarketDataEventArgs(marketData, ErrorCode.NoError, "", marketData.Symbol, this.typeBid, marketData.Symbol.Round2TickSize(num14), num15, this.adapter.connection.Now));
                            }
                            if ((price > 0.0) && ((num & 2) != 0))
                            {
                                this.adapter.connection.ProcessEventArgs(new MarketDataEventArgs(marketData, ErrorCode.NoError, "", marketData.Symbol, this.typeOpening, marketData.Symbol.Round2TickSize(price), 0, this.adapter.connection.Now));
                            }
                            if ((num9 > 0.0) && ((num & 0x80) != 0))
                            {
                                this.adapter.connection.ProcessEventArgs(new MarketDataEventArgs(marketData, ErrorCode.NoError, "", marketData.Symbol, this.typeDailyHigh, marketData.Symbol.Round2TickSize(num9), 0, this.adapter.connection.Now));
                            }
                            if ((num10 > 0.0) && ((num & 0x100) != 0))
                            {
                                this.adapter.connection.ProcessEventArgs(new MarketDataEventArgs(marketData, ErrorCode.NoError, "", marketData.Symbol, this.typeDailyLow, marketData.Symbol.Round2TickSize(num10), 0, this.adapter.connection.Now));
                            }
                            if ((volume > 0) && ((num & 0x200) != 0))
                            {
                                this.adapter.connection.ProcessEventArgs(new MarketDataEventArgs(marketData, ErrorCode.NoError, "", marketData.Symbol, this.typeDailyVolume, 0.0, volume, this.adapter.connection.Now));
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    this.adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.adapter.connection, ErrorCode.Panic, "", "CT.Callback.Level1: " + exception.Message));
                }
            }
            else if ((feed == Feed.Level2) || (feed == Feed.Futures))
            {
                try
                {
                    reader.ReadInteger();
                    int num18 = reader.ReadInteger();
                    string str2 = reader.ReadString(9);
                    MarketDepth marketDepthData = null;
                    lock ((hashtable = this.adapter.symbol2MarketDepth))
                    {
                        marketDepthData = (MarketDepth) this.adapter.symbol2MarketDepth[str2];
                    }
                    if (marketDepthData != null)
                    {
                        if (Globals.TraceSwitch.MarketDepth)
                        {
                            Trace.WriteLine("(" + this.adapter.connection.IdPlus + ") CT.Callback.DataCallback.MarketDepth: " + marketDepthData.Symbol.FullName);
                        }
                        if (marketDepthData.Symbol.SymbolType.Id == SymbolTypeId.Future)
                        {
                            reader.Skip(5);
                            reader.Skip(10);
                            for (int i = 0; i < num18; i++)
                            {
                                short num20 = reader.ReadShort();
                                reader.Skip(2);
                                int position = reader.ReadInteger();
                                double num22 = reader.ReadDouble();
                                int num23 = reader.ReadInteger();
                                reader.Skip(4);
                                double num24 = reader.ReadDouble();
                                int num25 = reader.ReadInteger();
                                reader.Skip(0x24);
                                reader.Skip(0x24);
                                reader.Skip(10);
                                reader.Skip(2);
                                position--;
                                if (((num20 & 8) != 0) || ((num20 & 0x10) != 0))
                                {
                                    Operation update = Operation.Update;
                                    if (position >= marketDepthData.Ask.Count)
                                    {
                                        update = Operation.Insert;
                                    }
                                    else if (num25 == -1)
                                    {
                                        update = Operation.Delete;
                                    }
                                    else
                                    {
                                        num24 = ((num20 & 8) != 0) ? num24 : marketDepthData.Ask[position].Price;
                                        num25 = ((num20 & 0x10) != 0) ? num25 : marketDepthData.Ask[position].Volume;
                                    }
                                    this.adapter.connection.ProcessEventArgs(new MarketDepthEventArgs(marketDepthData, ErrorCode.NoError, "", marketDepthData.Symbol, position, "", update, this.typeAsk, num24, num25, this.adapter.connection.Now));
                                }
                                if (((num20 & 2) != 0) || ((num20 & 4) != 0))
                                {
                                    Operation operation = Operation.Update;
                                    if (position >= marketDepthData.Bid.Count)
                                    {
                                        operation = Operation.Insert;
                                    }
                                    else if (num23 == -1)
                                    {
                                        operation = Operation.Delete;
                                    }
                                    else
                                    {
                                        num22 = ((num20 & 2) != 0) ? num22 : marketDepthData.Bid[position].Price;
                                        num23 = ((num20 & 4) != 0) ? num23 : marketDepthData.Bid[position].Volume;
                                    }
                                    this.adapter.connection.ProcessEventArgs(new MarketDepthEventArgs(marketDepthData, ErrorCode.NoError, "", marketDepthData.Symbol, position, "", operation, this.typeBid, num22, num23, this.adapter.connection.Now));
                                }
                            }
                        }
                        else
                        {
                            reader.ReadByte();
                            reader.Skip(4);
                            reader.Skip(10);
                            for (int j = 0; j < num18; j++)
                            {
                                short num27 = reader.ReadShort();
                                string mmId = reader.ReadString(5);
                                reader.Skip(1);
                                double bidPrice = reader.ReadDouble();
                                int bidSize = reader.ReadInteger();
                                reader.Skip(4);
                                double askPrice = reader.ReadDouble();
                                int askSize = reader.ReadInteger();
                                char ch = reader.ReadChar();
                                reader.Skip(3);
                                reader.Skip(0x24);
                                reader.Skip(10);
                                reader.Skip(2);
                                if (((num27 & 8) != 0) && ((num27 & 0x10) == 0))
                                {
                                    askSize = -1;
                                }
                                else if (((num27 & 8) == 0) && ((num27 & 0x10) != 0))
                                {
                                    askPrice = -1.0;
                                }
                                if (((num27 & 2) != 0) && ((num27 & 4) == 0))
                                {
                                    bidSize = -1;
                                }
                                else if (((num27 & 2) == 0) && ((num27 & 4) != 0))
                                {
                                    bidPrice = -1.0;
                                }
                                if (((num27 & 8) != 0) || ((num27 & 0x10) != 0))
                                {
                                    if (((num27 & 0x40) != 0) && ((ch == 'K') || (ch == 'H')))
                                    {
                                        marketDepthData.Update(mmId, this.typeAsk, 0.0, 0, 0.0, 0, this.adapter.connection.Now);
                                    }
                                    else
                                    {
                                        marketDepthData.Update(mmId, this.typeAsk, askPrice, askSize, 0.0, 0, this.adapter.connection.Now);
                                    }
                                }
                                if (((num27 & 2) != 0) || ((num27 & 4) != 0))
                                {
                                    if (((num27 & 0x40) != 0) && ((ch == 'K') || (ch == 'H')))
                                    {
                                        marketDepthData.Update(mmId, this.typeBid, 0.0, 0, 0.0, 0, this.adapter.connection.Now);
                                    }
                                    else
                                    {
                                        marketDepthData.Update(mmId, this.typeBid, 0.0, 0, bidPrice, bidSize, this.adapter.connection.Now);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception exception2)
                {
                    this.adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.adapter.connection, ErrorCode.Panic, "", "CT.Callback.Level2: " + exception2.Message));
                }
            }
            else if (feed == Feed.NewsHeadline)
            {
                try
                {
                    reader.ReadInteger();
                    int num32 = reader.ReadInteger();
                    string str4 = reader.ReadString(9);
                    reader.Skip(1);
                    reader.Skip(10);
                    for (int k = 0; k < num32; k++)
                    {
                        string key = reader.ReadString(15);
                        reader.Skip(1);
                        reader.ReadTime();
                        DateTime time2 = reader.ReadTime();
                        reader.Skip(10);
                        string str6 = reader.ReadString(500);
                        reader.Skip(2);
                        if (Globals.TraceSwitch.News)
                        {
                            Trace.WriteLine("(" + this.adapter.connection.IdPlus + ") CT.Callback.DataCallback.NewsHeadline: id='" + key + "' symbol='" + str4 + "'");
                        }
                        lock ((hashtable = this.newsHeadlines))
                        {
                            if (!this.newsHeadlines.Contains(key))
                            {
                                this.newsHeadlines.Add(key, new NewsHeadline(key, time2, ((str4 == "!NEWSHOT") ? "" : (str4 + ": ")) + str6));
                            }
                        }
                    }
                }
                catch (Exception exception3)
                {
                    this.adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.adapter.connection, ErrorCode.Panic, "", "CT.Callback.NewsHeadline: " + exception3.Message));
                }
            }
            else if (feed == Feed.NewsStory)
            {
                try
                {
                    int num34 = reader.ReadInteger();
                    string str7 = reader.ReadString(15);
                    reader.Skip(10);
                    string newsText = reader.ReadString(num34 - 0x1d);
                    if (Globals.TraceSwitch.News)
                    {
                        Trace.WriteLine("(" + this.adapter.connection.IdPlus + ") CT.Callback.DataCallback.NewsStory: id='" + str7 + "'");
                    }
                    lock ((hashtable = this.newsHeadlines))
                    {
                        if (this.newsHeadlines.Contains(str7))
                        {
                            NewsHeadline headline = (NewsHeadline) this.newsHeadlines[str7];
                            this.adapter.connection.ProcessEventArgs(new NewsEventArgs(this.adapter.connection, ErrorCode.NoError, "", str7, this.adapter.connection.NewsItemTypes[NewsItemTypeId.Default], headline.time, headline.headline, newsText));
                        }
                    }
                }
                catch (Exception exception4)
                {
                    this.adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.adapter.connection, ErrorCode.Panic, "", "CT.Callback.NewsStory: " + exception4.Message));
                }
            }
            else if ((feed == Feed.History) || (feed == Feed.Intraday))
            {
                try
                {
                    if (Globals.TraceSwitch.Quotes)
                    {
                        Trace.WriteLine("(" + this.adapter.connection.IdPlus + ") CT.Callback.DataCallback.History: " + this.adapter.quotes2Lookup.ToString());
                    }
                    reader.ReadInteger();
                    int num35 = reader.ReadInteger();
                    string str9 = reader.ReadString(9);
                    reader.Skip(1);
                    reader.ReadShort();
                    reader.ReadInteger();
                    reader.ReadShort();
                    reader.Skip(10);
                    reader.Skip(4);
                    if (this.adapter.quotes2Lookup == null)
                    {
                        double num36 = 0.0;
                        for (int m = 0; m < num35; m++)
                        {
                            reader.ReadDouble();
                            reader.ReadDouble();
                            reader.ReadDouble();
                            double num38 = reader.ReadDouble();
                            reader.ReadInteger();
                            string str10 = reader.ReadString(10);
                            string str11 = reader.ReadString(10);
                            reader.Skip(6);
                            reader.Skip(2);
                            DateTime now = this.adapter.connection.Now;
                            try
                            {
                                string[] strArray = str11.Split(this.timeSplitter);
                                now = new DateTime(0x7d0 + Convert.ToInt32(strArray[2]), Convert.ToInt32(strArray[0]), Convert.ToInt32(strArray[1]));
                            }
                            catch (Exception exception5)
                            {
                                if (Globals.TraceSwitch.Strict)
                                {
                                    Trace.WriteLine("(" + this.adapter.connection.IdPlus + ") CT.Callback.DataCallback.History1: " + str9 + " time='" + str10 + "' date='" + str11 + "': " + exception5.Message);
                                }
                                return;
                            }
                            if (now.Date < this.adapter.connection.Now.Date)
                            {
                                num36 = num38;
                            }
                        }
                        Symbol symbol = this.adapter.Convert(str9);
                        if ((((str9 != null) && symbol.MarketData.IsRunning) && (symbol.MarketData.LastClose == null)) && (num36 != 0.0))
                        {
                            this.adapter.connection.ProcessEventArgs(new MarketDataEventArgs(symbol.MarketData, ErrorCode.NoError, "", symbol, this.typeLastClose, num36, 0, this.adapter.connection.Now));
                        }
                    }
                    else
                    {
                        for (int n = 0; n < num35; n++)
                        {
                            double open = reader.ReadDouble();
                            double high = reader.ReadDouble();
                            double low = reader.ReadDouble();
                            double close = reader.ReadDouble();
                            int num44 = reader.ReadInteger();
                            string str12 = reader.ReadString(10);
                            string str13 = reader.ReadString(10);
                            reader.Skip(6);
                            reader.Skip(2);
                            DateTime time4 = this.adapter.connection.Now;
                            try
                            {
                                if (this.adapter.quotes2Lookup.Period.Id == PeriodTypeId.Day)
                                {
                                    string[] strArray2 = str13.Split(this.timeSplitter);
                                    time4 = new DateTime(0x7d0 + Convert.ToInt32(strArray2[2]), Convert.ToInt32(strArray2[0]), Convert.ToInt32(strArray2[1]));
                                }
                            }
                            catch (Exception exception6)
                            {
                                if (Globals.TraceSwitch.Strict)
                                {
                                    Trace.WriteLine("(" + this.adapter.connection.IdPlus + ") CT.Callback.DataCallback.History2: " + this.adapter.quotes2Lookup.ToString() + " time='" + str12 + "' date='" + str13 + "': " + exception6.Message);
                                }
                                continue;
                            }
                            if (num44 < 0)
                            {
                                num44 = 0;
                            }
                            if (((((time4.Date >= this.adapter.quotes2Lookup.From) && (this.adapter.quotes2Lookup.To >= time4.Date)) && ((open > 0.0) && (high > 0.0))) && (((low > 0.0) && (close > 0.0)) && ((open >= low) && (high >= low)))) && (((close >= low) && (open <= high)) && ((low <= high) && (close <= high))))
                            {
                                this.adapter.quotes2Lookup.Bars.Add(open, high, low, close, time4, num44, false);
                            }
                        }
                        Globals.Progress.Terminate();
                        Quotes quotes = this.adapter.quotes2Lookup;
                        this.adapter.quotes2Lookup = null;
                        this.adapter.connection.ProcessEventArgs(new BarUpdateEventArgs(this.adapter.connection, ErrorCode.NoError, "", Operation.Insert, quotes, 0, quotes.Bars.Count - 1));
                    }
                }
                catch (Exception exception7)
                {
                    this.adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.adapter.connection, ErrorCode.Panic, "", "CT.Callback.History: " + exception7.Message));
                }
            }
        }

        internal void ErrorCallback(Feed feed, Failure type, string symbol)
        {
            try
            {
                Hashtable hashtable;
                if (feed == Feed.Level1)
                {
                    if (Globals.TraceSwitch.News)
                    {
                        Trace.WriteLine("(" + this.adapter.connection.IdPlus + ") CT.Callback.ErrorCallback.Level1: symbol='" + symbol + "'");
                    }
                    lock ((hashtable = this.adapter.symbol2MarketData))
                    {
                        if (this.adapter.symbol2MarketData.Contains(symbol))
                        {
                            this.adapter.symbol2MarketData.Remove(symbol);
                        }
                    }
                    this.adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.adapter.connection, ErrorCode.NoSuchSymbol, "", "Unknown symbol (" + symbol + ")"));
                }
                else
                {
                    if (feed == Feed.NewsStory)
                    {
                        if (Globals.TraceSwitch.News)
                        {
                            Trace.WriteLine("(" + this.adapter.connection.IdPlus + ") CT.Callback.ErrorCallback.NewsStory: symbol='" + symbol + "'");
                        }
                        lock ((hashtable = this.newsHeadlines))
                        {
                            if (this.newsHeadlines.Contains(symbol))
                            {
                                NewsHeadline headline = (NewsHeadline) this.newsHeadlines[symbol];
                                this.adapter.connection.ProcessEventArgs(new NewsEventArgs(this.adapter.connection, ErrorCode.NoError, "", symbol, this.adapter.connection.NewsItemTypes[NewsItemTypeId.Default], headline.time, headline.headline, ""));
                            }
                            return;
                        }
                    }
                    if ((feed == Feed.History) || (feed == Feed.Intraday))
                    {
                        if (Globals.TraceSwitch.Quotes)
                        {
                            Trace.WriteLine("(" + this.adapter.connection.IdPlus + ") CT.Callback.ErrorCallback.History: " + ((this.adapter.quotes2Lookup == null) ? "(null)" : this.adapter.quotes2Lookup.ToString()));
                        }
                        if (this.adapter.quotes2Lookup != null)
                        {
                            Globals.Progress.Terminate();
                            Quotes quotes = this.adapter.quotes2Lookup;
                            this.adapter.quotes2Lookup = null;
                            this.adapter.connection.ProcessEventArgs(new BarUpdateEventArgs(this.adapter.connection, ErrorCode.Panic, "No historical data available", Operation.Insert, quotes, 0, quotes.Bars.Count - 1));
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                this.adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.adapter.connection, ErrorCode.Panic, "", "CT.Callback.ErrorCallback: " + exception.Message));
            }
        }

        internal void StatusCallback(Status status, string msg)
        {
            try
            {
                if (Globals.TraceSwitch.Connect)
                {
                    Trace.WriteLine(string.Concat(new object[] { "(", this.adapter.connection.IdPlus, ") CT.Callback.StatusCallback: status=", status }));
                }
                if ((status == Status.Ready) && (this.adapter.connection.ConnectionStatusId == ConnectionStatusId.Connecting))
                {
                    Api.OpenConnection(this.adapter.Options.User, this.adapter.Options.Password, (this.adapter.Options.Mode.Id == ModeTypeId.Live) ? 0 : 1, this.adapter.Options.IPAddress, this.adapter.Options.IPAddressAlternate);
                }
            }
            catch (Exception exception)
            {
                this.adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.adapter.connection, ErrorCode.Panic, "", "CT.Callback.StatusCallback: " + exception.Message));
            }
        }

        private class NewsHeadline
        {
            public string headline;
            public string id;
            public DateTime time;

            public NewsHeadline(string id, DateTime time, string headline)
            {
                this.headline = headline;
                this.id = id;
                this.time = time;
            }
        }
    }
}

