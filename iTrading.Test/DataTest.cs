namespace iTrading.Test
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using iTrading.Core.Kernel;
   using iTrading.Core.Data;

    /// <summary>
    /// Test data retrieval.
    /// </summary>
    public class DataTest : TestBase
    {
        private static int concurrentMarketDepth = 0;
        private bool marketDataSeen = false;
        private bool marketDepthSeen = false;
        private ArrayList quotesHolders = new ArrayList();
        private int state = 0;

        /// <summary>
        /// Creates an instance of this class.
        /// </summary>
        /// <returns></returns>
        protected override TestBase CreateInstance()
        {
            return new DataTest();
        }

        /// <summary>
        /// Execute test.
        /// </summary>
        protected override void DoTest()
        {
            Type type;
            this.state = this.state;
            Random random = new Random((int) DateTime.Now.Ticks);
            base.Connection.News.News += new NewsEventHandler(this.NewsData_NewsItem);
            if (iTrading.Core.Kernel.Globals.TraceSwitch.Test)
            {
                Trace.WriteLine("(" + base.Connection.IdPlus + ") Test.Data.DoTest1");
            }
            this.state = 1;
            foreach (SymbolTemplate template in base.SymbolTemplates)
            {
                Symbol symbol = base.Connection.GetSymbol(template.Name, template.Expiry, base.Connection.SymbolTypes[template.SymbolTypeId], base.Connection.Exchanges[template.ExchangeId], 0.0, RightId.Unknown, LookupPolicyId.RepositoryAndProvider);
                 iTrading.Test.Globals.Assert(string.Concat(new object[] { base.GetType().FullName, " 000a ", template.Name, " ", template.Expiry, " ", template.ExchangeId }), template.Valid ? (symbol != null) : (symbol == null));
                if (symbol != null)
                {
                    double tickSize = symbol.TickSize;
                }
                 iTrading.Test.Globals.Sleep( iTrading.Test.Globals.MilliSeconds2Sleep);
                if (((symbol != null) && !base.RunMultiple) && !base.RunMultiThread)
                {
                    lock ((type = typeof(DataTest)))
                    {
                        iTrading.Core.Kernel.Globals.DB.BeginTransaction();
                        string customText = Guid.NewGuid().ToString();
                        symbol.CustomText = customText;
                        string providerName = symbol.GetProviderName(base.Connection.Options.Provider.Id);
                        symbol.SetProviderName(base.Connection.Options.Provider.Id, "xx" + symbol.Name);
                        iTrading.Core.Kernel.Globals.DB.Update(symbol, true);
                        SymbolCollection symbols = iTrading.Core.Kernel.Globals.DB.Select(symbol.Name, null, null, symbol.Expiry, symbol.SymbolType, symbol.Exchange, 0.0, RightId.Unknown, null);
                         iTrading.Test.Globals.Assert(string.Concat(new object[] { base.GetType().FullName, " 000b ", template.Name, " ", template.Expiry, " ", template.ExchangeId }), symbols.Count == 1);
                         iTrading.Test.Globals.Assert(string.Concat(new object[] { base.GetType().FullName, " 000c ", template.Name, " ", template.Expiry, " ", template.ExchangeId }), symbols[0].CustomText == customText);
                         iTrading.Test.Globals.Assert(string.Concat(new object[] { base.GetType().FullName, " 000d ", template.Name, " ", template.Expiry, " ", template.ExchangeId }), iTrading.Core.Kernel.Globals.DB.Select(symbol.Name.Substring(0, 1) + "%", null, null, symbol.Expiry, symbol.SymbolType, symbol.Exchange, 0.0, RightId.Unknown, null).Count >= 1);
                         iTrading.Test.Globals.Assert(string.Concat(new object[] { base.GetType().FullName, " 000e ", template.Name, " ", template.Expiry, " ", template.ExchangeId }), iTrading.Core.Kernel.Globals.DB.Select("%", null, null, symbol.Expiry, symbol.SymbolType, symbol.Exchange, 0.0, RightId.Unknown, null).Count >= 1);
                         iTrading.Test.Globals.Assert(string.Concat(new object[] { base.GetType().FullName, " 000f ", template.Name, " ", template.Expiry, " ", template.ExchangeId }), iTrading.Core.Kernel.Globals.DB.Select(symbol.Name, null, null, iTrading.Core.Kernel.Globals.MaxDate, symbol.SymbolType, symbol.Exchange, 0.0, RightId.Unknown, null).Count >= 1);
                         iTrading.Test.Globals.Assert(string.Concat(new object[] { base.GetType().FullName, " 000g ", template.Name, " ", template.Expiry, " ", template.ExchangeId }), iTrading.Core.Kernel.Globals.DB.Select(symbol.Name, null, null, symbol.Expiry, null, symbol.Exchange, 0.0, RightId.Unknown, null).Count >= 1);
                         iTrading.Test.Globals.Assert(string.Concat(new object[] { base.GetType().FullName, " 000h ", template.Name, " ", template.Expiry, " ", template.ExchangeId }), iTrading.Core.Kernel.Globals.DB.Select(symbol.Name, null, null, symbol.Expiry, symbol.SymbolType, null, 0.0, RightId.Unknown, null).Count >= 1);
                         iTrading.Test.Globals.Assert(string.Concat(new object[] { base.GetType().FullName, " 000i ", template.Name, " ", template.Expiry, " ", template.ExchangeId }), iTrading.Core.Kernel.Globals.DB.Select(symbol.Name, null, null, symbol.Expiry, symbol.SymbolType, symbol.Exchange, 0.0, RightId.Unknown, null).Count >= 1);
                         iTrading.Test.Globals.Assert(string.Concat(new object[] { base.GetType().FullName, " 000j ", template.Name, " ", template.Expiry, " ", template.ExchangeId }), iTrading.Core.Kernel.Globals.DB.Select(symbol.Name, null, null, symbol.Expiry, symbol.SymbolType, symbol.Exchange, 0.0, RightId.Unknown, customText).Count >= 1);
                         iTrading.Test.Globals.Assert(string.Concat(new object[] { base.GetType().FullName, " 000k ", template.Name, " ", template.Expiry, " ", template.ExchangeId }), iTrading.Core.Kernel.Globals.DB.Select("%", null, null, iTrading.Core.Kernel.Globals.MaxDate, null, null, 0.0, RightId.Unknown, null).Count >= 1);
                         iTrading.Test.Globals.Assert(string.Concat(new object[] { base.GetType().FullName, " 000l ", template.Name, " ", template.Expiry, " ", template.ExchangeId }), iTrading.Core.Kernel.Globals.DB.Select(null, null, symbol.CompanyName, iTrading.Core.Kernel.Globals.MaxDate, null, null, 0.0, RightId.Unknown, null).Count >= 1);
                         iTrading.Test.Globals.Assert(string.Concat(new object[] { base.GetType().FullName, " 000m ", template.Name, " ", template.Expiry, " ", template.ExchangeId }), iTrading.Core.Kernel.Globals.DB.Select(null, null, "%", iTrading.Core.Kernel.Globals.MaxDate, null, null, 0.0, RightId.Unknown, null).Count >= 1);
                        base.Connection.Symbols.Clear();
                        Symbol symbol2 = base.Connection.GetSymbolByProviderName("xx" + symbol.Name, symbol.Expiry, symbol.SymbolType, symbol.Exchange, 0.0, RightId.Unknown, LookupPolicyId.RepositoryOnly);
                         iTrading.Test.Globals.Assert(string.Concat(new object[] { base.GetType().FullName, " 000n ", template.Name, " ", template.Expiry, " ", template.ExchangeId }), symbol2 != null);
                         iTrading.Test.Globals.Assert(string.Concat(new object[] { base.GetType().FullName, " 000o ", template.Name, " ", template.Expiry, " ", template.ExchangeId }), symbol2.Name == symbol.Name);
                         iTrading.Test.Globals.Assert(string.Concat(new object[] { base.GetType().FullName, " 000p ", template.Name, " ", template.Expiry, " ", template.ExchangeId }), symbol2.GetProviderName(base.Connection.Options.Provider.Id) == ("xx" + symbol.Name));
                        symbol2 = base.Connection.GetSymbolByProviderName("xx" + symbol.Name, symbol.Expiry, symbol.SymbolType, symbol.Exchange, 0.0, RightId.Unknown, LookupPolicyId.RepositoryOnly);
                         iTrading.Test.Globals.Assert(string.Concat(new object[] { base.GetType().FullName, " 000q ", template.Name, " ", template.Expiry, " ", template.ExchangeId }), symbol2 != null);
                        symbol.SetProviderName(base.Connection.Options.Provider.Id, providerName);
                        iTrading.Core.Kernel.Globals.DB.Update(symbol, true);
                        string name = "@@" + symbol.Name;
                        Symbol symbol3 = new Symbol(null, name, symbol.Expiry, symbol.SymbolType, symbol.Exchange, symbol.StrikePrice, symbol.Right.Id, symbol.Currency, symbol.TickSize, symbol.PointValue, symbol.CompanyName, symbol.Exchanges, symbol.Splits, symbol.Dividends);
                        iTrading.Core.Kernel.Globals.DB.Delete(symbol3);
                        iTrading.Core.Kernel.Globals.DB.CommitTransaction();
                    }
                }
            }
            if (iTrading.Core.Kernel.Globals.TraceSwitch.Test)
            {
                Trace.WriteLine("(" + base.Connection.IdPlus + ") Test.Data.DoTest2");
            }
            this.state = 2;
            for (int i = 0; i < 100; i++)
            {
                SymbolTemplate template2 = base.SymbolTemplates[random.Next(base.SymbolTemplates.Length)];
                Symbol symbol4 = base.Connection.GetSymbol(template2.Name, template2.Expiry, base.Connection.SymbolTypes[template2.SymbolTypeId], base.Connection.Exchanges[template2.ExchangeId], 0.0, RightId.Unknown, LookupPolicyId.RepositoryAndProvider);
                 iTrading.Test.Globals.Assert(string.Concat(new object[] { base.GetType().FullName, " 001 ", template2.Name, " ", template2.Expiry, " ", template2.ExchangeId }), template2.Valid ? (symbol4 != null) : (symbol4 == null));
                if (symbol4 != null)
                {
                    double num9 = symbol4.TickSize;
                }
                 iTrading.Test.Globals.Sleep( iTrading.Test.Globals.MilliSeconds2Sleep);
            }
            if (base.Connection.FeatureTypes[FeatureTypeId.MarketData] != null)
            {
                if (iTrading.Core.Kernel.Globals.TraceSwitch.Test)
                {
                    Trace.WriteLine("(" + base.Connection.IdPlus + ") Test.Data.DoTest3");
                }
                this.state = 3;
                foreach (SymbolTemplate template3 in base.SymbolTemplates)
                {
                    if (template3.Valid)
                    {
                        Symbol symbol5 = base.Connection.GetSymbol(template3.Name, template3.Expiry, base.Connection.SymbolTypes[template3.SymbolTypeId], base.Connection.Exchanges[template3.ExchangeId], 0.0, RightId.Unknown, LookupPolicyId.RepositoryAndProvider);
                        this.marketDataSeen = false;
                        symbol5.MarketData.MarketDataItem += new MarketDataItemEventHandler(this.MarketData_MarketDataItem);
                        for (int k = 0; !this.marketDataSeen && (k < (0x1388 /  iTrading.Test.Globals.MilliSeconds2Sleep)); k++)
                        {
                             iTrading.Test.Globals.Sleep( iTrading.Test.Globals.MilliSeconds2Sleep);
                        }
                        symbol5.MarketData.MarketDataItem -= new MarketDataItemEventHandler(this.MarketData_MarketDataItem);
                    }
                }
                if (iTrading.Core.Kernel.Globals.TraceSwitch.Test)
                {
                    Trace.WriteLine("(" + base.Connection.IdPlus + ") Test.Data.DoTest4");
                }
                this.state = 4;
                for (int j = 0; j < 100; j++)
                {
                    SymbolTemplate template4 = base.SymbolTemplates[random.Next(base.SymbolTemplates.Length)];
                    if (template4.Valid)
                    {
                        Symbol symbol6 = base.Connection.GetSymbol(template4.Name, template4.Expiry, base.Connection.SymbolTypes[template4.SymbolTypeId], base.Connection.Exchanges[template4.ExchangeId], 0.0, RightId.Unknown, LookupPolicyId.RepositoryAndProvider);
                        this.marketDataSeen = false;
                        symbol6.MarketData.MarketDataItem += new MarketDataItemEventHandler(this.MarketData_MarketDataItem);
                        for (int m = 0; !this.marketDataSeen && (m < (0x1388 /  iTrading.Test.Globals.MilliSeconds2Sleep)); m++)
                        {
                             iTrading.Test.Globals.Sleep( iTrading.Test.Globals.MilliSeconds2Sleep);
                        }
                        symbol6.MarketData.MarketDataItem -= new MarketDataItemEventHandler(this.MarketData_MarketDataItem);
                    }
                }
            }
            if (base.Connection.FeatureTypes[FeatureTypeId.MarketDepth] != null)
            {
                if (iTrading.Core.Kernel.Globals.TraceSwitch.Test)
                {
                    Trace.WriteLine("(" + base.Connection.IdPlus + ") Test.Data.DoTest5");
                }
                this.state = 5;
                foreach (SymbolTemplate template5 in base.SymbolTemplates)
                {
                    if (template5.Valid && ((base.Connection.Options.Provider.Id != ProviderTypeId.TrackData) || (template5.SymbolTypeId == SymbolTypeId.Stock)))
                    {
                        Symbol symbol7 = base.Connection.GetSymbol(template5.Name, template5.Expiry, base.Connection.SymbolTypes[template5.SymbolTypeId], base.Connection.Exchanges[template5.ExchangeId], 0.0, RightId.Unknown, LookupPolicyId.RepositoryAndProvider);
                        bool flag = false;
                        while (!flag)
                        {
                            lock ((type = typeof(DataTest)))
                            {
                                if (concurrentMarketDepth < base.Connection.FeatureTypes[FeatureTypeId.MaxMarketDepthStreams].Value)
                                {
                                    flag = true;
                                    concurrentMarketDepth++;
                                }
                            }
                             iTrading.Test.Globals.Sleep( iTrading.Test.Globals.MilliSeconds2Sleep);
                        }
                        this.marketDepthSeen = false;
                        symbol7.MarketDepth.MarketDepthItem += new MarketDepthItemEventHandler(this.MarketDepth_MarketDepthItem);
                        for (int num5 = 0; !this.marketDepthSeen && (num5 < (0x1388 /  iTrading.Test.Globals.MilliSeconds2Sleep)); num5++)
                        {
                             iTrading.Test.Globals.Sleep( iTrading.Test.Globals.MilliSeconds2Sleep);
                        }
                        symbol7.MarketDepth.MarketDepthItem -= new MarketDepthItemEventHandler(this.MarketDepth_MarketDepthItem);
                        concurrentMarketDepth--;
                    }
                }
                if (iTrading.Core.Kernel.Globals.TraceSwitch.Test)
                {
                    Trace.WriteLine("(" + base.Connection.IdPlus + ") Test.Data.DoTest6");
                }
                this.state = 6;
                for (int n = 0; n < 100; n++)
                {
                    SymbolTemplate template6 = base.SymbolTemplates[random.Next(base.SymbolTemplates.Length)];
                    if (template6.Valid && ((base.Connection.Options.Provider.Id != ProviderTypeId.TrackData) || (template6.SymbolTypeId == SymbolTypeId.Stock)))
                    {
                        Symbol symbol8 = base.Connection.GetSymbol(template6.Name, template6.Expiry, base.Connection.SymbolTypes[template6.SymbolTypeId], base.Connection.Exchanges[template6.ExchangeId], 0.0, RightId.Unknown, LookupPolicyId.RepositoryAndProvider);
                        bool flag2 = false;
                        while (!flag2)
                        {
                            lock ((type = typeof(DataTest)))
                            {
                                if (concurrentMarketDepth < base.Connection.FeatureTypes[FeatureTypeId.MaxMarketDepthStreams].Value)
                                {
                                    flag2 = true;
                                    concurrentMarketDepth++;
                                }
                            }
                             iTrading.Test.Globals.Sleep( iTrading.Test.Globals.MilliSeconds2Sleep);
                        }
                        this.marketDepthSeen = false;
                        symbol8.MarketDepth.MarketDepthItem += new MarketDepthItemEventHandler(this.MarketDepth_MarketDepthItem);
                        for (int num7 = 0; !this.marketDepthSeen && (num7 < (0x1388 /  iTrading.Test.Globals.MilliSeconds2Sleep)); num7++)
                        {
                             iTrading.Test.Globals.Sleep( iTrading.Test.Globals.MilliSeconds2Sleep);
                        }
                        symbol8.MarketDepth.MarketDepthItem -= new MarketDepthItemEventHandler(this.MarketDepth_MarketDepthItem);
                        concurrentMarketDepth--;
                    }
                }
                this.state = 0;
            }
            base.Connection.News.News -= new NewsEventHandler(this.NewsData_NewsItem);
        }

        private void MarketData_MarketDataItem(object sender, MarketDataEventArgs e)
        {
            this.marketDataSeen = true;
             iTrading.Test.Globals.Assert(base.GetType().FullName + " 100 " + e.Error, e.Error == ErrorCode.NoError);
             iTrading.Test.Globals.Assert(base.GetType().FullName + " 101 " + e.Error, e.NativeError.Length == 0);
             iTrading.Test.Globals.Assert(base.GetType().FullName + " 102", e.MarketDataType != null);
             iTrading.Test.Globals.Assert(base.GetType().FullName + " 103", e.Symbol != null);
             iTrading.Test.Globals.Assert(base.GetType().FullName + " 103a", e.Price == e.Symbol.Round2TickSize(e.Price));
            switch (e.MarketDataType.Id)
            {
                case MarketDataTypeId.Ask:
                     iTrading.Test.Globals.Assert(base.GetType().FullName + " 104a", e.Price > 0.0);
                     iTrading.Test.Globals.Assert(base.GetType().FullName + " 105a", e.Volume >= 0);
                    return;

                case MarketDataTypeId.Bid:
                     iTrading.Test.Globals.Assert(base.GetType().FullName + " 104b", e.Price > 0.0);
                     iTrading.Test.Globals.Assert(base.GetType().FullName + " 105b", e.Volume >= 0);
                    return;

                case MarketDataTypeId.Last:
                     iTrading.Test.Globals.Assert(base.GetType().FullName + " 104f", e.Price > 0.0);
                     iTrading.Test.Globals.Assert(base.GetType().FullName + " 105f", e.Volume >= 0);
                    return;

                case MarketDataTypeId.DailyHigh:
                     iTrading.Test.Globals.Assert(base.GetType().FullName + " 104c", e.Price > 0.0);
                     iTrading.Test.Globals.Assert(base.GetType().FullName + " 105c", e.Volume >= 0);
                    return;

                case MarketDataTypeId.DailyLow:
                     iTrading.Test.Globals.Assert(base.GetType().FullName + " 104d", e.Price > 0.0);
                     iTrading.Test.Globals.Assert(base.GetType().FullName + " 105d", e.Volume >= 0);
                    return;

                case MarketDataTypeId.DailyVolume:
                     iTrading.Test.Globals.Assert(base.GetType().FullName + " 104e", e.Price >= 0.0);
                     iTrading.Test.Globals.Assert(base.GetType().FullName + " 105e", e.Volume > 0);
                    return;

                case MarketDataTypeId.LastClose:
                     iTrading.Test.Globals.Assert(base.GetType().FullName + " 104g", e.Price > 0.0);
                     iTrading.Test.Globals.Assert(base.GetType().FullName + " 105g", e.Volume >= 0);
                    return;

                case MarketDataTypeId.Opening:
                     iTrading.Test.Globals.Assert(base.GetType().FullName + " 104h", e.Price > 0.0);
                     iTrading.Test.Globals.Assert(base.GetType().FullName + " 105h", e.Volume >= 0);
                    return;
            }
        }

        private void MarketDepth_MarketDepthItem(object sender, MarketDepthEventArgs e)
        {
            this.marketDepthSeen = true;
             iTrading.Test.Globals.Assert(base.GetType().FullName + " 200 " + e.Error, e.Error == ErrorCode.NoError);
             iTrading.Test.Globals.Assert(base.GetType().FullName + " 201 " + e.Error, e.NativeError.Length == 0);
             iTrading.Test.Globals.Assert(base.GetType().FullName + " 202", e.MarketDataType != null);
             iTrading.Test.Globals.Assert(base.GetType().FullName + " 203", e.Position >= 0);
             iTrading.Test.Globals.Assert(base.GetType().FullName + " 204", e.Symbol != null);
             iTrading.Test.Globals.Assert(base.GetType().FullName + " 204a", e.Price == e.Symbol.Round2TickSize(e.Price));
            if (e.Operation == Operation.Delete)
            {
                 iTrading.Test.Globals.Assert(base.GetType().FullName + " 205a", e.Price >= 0.0);
                 iTrading.Test.Globals.Assert(base.GetType().FullName + " 206a", e.Volume >= 0);
            }
            else
            {
                 iTrading.Test.Globals.Assert(base.GetType().FullName + " 205b", e.Price > 0.0);
                 iTrading.Test.Globals.Assert(base.GetType().FullName + " 206b", e.Volume > 0);
            }
        }

        private void NewsData_NewsItem(object sender, NewsEventArgs e)
        {
             iTrading.Test.Globals.Assert(base.GetType().FullName + " 300 " + e.Error, e.Error == ErrorCode.NoError);
             iTrading.Test.Globals.Assert(base.GetType().FullName + " 301 " + e.Error, e.NativeError.Length == 0);
             iTrading.Test.Globals.Assert(base.GetType().FullName + " 302", e.HeadLine.Length > 0);
             iTrading.Test.Globals.Assert(base.GetType().FullName + " 303", e.Id.Length > 0);
             iTrading.Test.Globals.Assert(base.GetType().FullName + " 304", e.ItemType != null);
             iTrading.Test.Globals.Assert(base.GetType().FullName + " 305", e.Text.Length > 0);
        }

        private class QuotesHolder
        {
            public iTrading.Core.Data.Quotes quotes = null;
        }
    }
}

