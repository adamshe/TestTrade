using iTrading.Core.Data;

namespace iTrading.Test
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.IO;
    using System.Threading;
    using System.Windows.Forms;
    using iTrading.Core.Kernel;
   using iTrading.Core.Data;

    /// <summary>
    /// Quotes data retrieval.
    /// </summary>
    public class Quotes : TestBase
    {
        private ArrayList quotesHolders = new ArrayList();
        private int state = 0;

        private void CompareQuotes(iTrading.Core .Data.Quotes quotes1, iTrading.Core .Data.Quotes quotes2)
        {
             iTrading.Test.Globals.Assert(base.GetType().FullName + " 501 " + quotes1.Symbol.FullName, quotes1.Bars.Count == quotes2.Bars.Count);
            for (int i = 0; i < quotes1.Bars.Count; i++)
            {
                IBar bar = quotes1.Bars[i];
                IBar bar2 = quotes2.Bars[i];
                 iTrading.Test.Globals.Assert(base.GetType().FullName + " 501 " + quotes1.Symbol.FullName + " '" + bar.ToString() + "' '" + bar2.ToString() + "'", bar.Time == bar2.Time);
                if ((base.Connection.Options.Provider.Id == ProviderTypeId.Dtn) || (base.Connection.Options.Provider.Id == ProviderTypeId.TrackData))
                {
                    if (bar.Close != bar2.Close)
                    {
                        Trace.WriteLine(base.GetType().FullName + " 502 " + quotes1.Symbol.FullName + " '" + bar.ToString() + "' '" + bar2.ToString() + "'");
                    }
                    if (bar.High != bar2.High)
                    {
                        Trace.WriteLine(base.GetType().FullName + " 503 " + quotes1.Symbol.FullName + " '" + bar.ToString() + "' '" + bar2.ToString() + "'");
                    }
                    if (bar.Low != bar2.Low)
                    {
                        Trace.WriteLine(base.GetType().FullName + " 504 " + quotes1.Symbol.FullName + " '" + bar.ToString() + "' '" + bar2.ToString() + "'");
                    }
                    if (bar.Open != bar2.Open)
                    {
                        Trace.WriteLine(base.GetType().FullName + " 505 " + quotes1.Symbol.FullName + " '" + bar.ToString() + "' '" + bar2.ToString() + "'");
                    }
                    if (bar.Volume != bar2.Volume)
                    {
                        Trace.WriteLine(base.GetType().FullName + " 507 " + quotes1.Symbol.FullName + " '" + bar.ToString() + "' '" + bar2.ToString() + "'");
                    }
                }
                else
                {
                     iTrading.Test.Globals.Assert(base.GetType().FullName + " 502 " + quotes1.Symbol.FullName + " '" + bar.ToString() + "' '" + bar2.ToString() + "'", bar.Close == bar2.Close);
                     iTrading.Test.Globals.Assert(base.GetType().FullName + " 503 " + quotes1.Symbol.FullName + " '" + bar.ToString() + "' '" + bar2.ToString() + "'", bar.High == bar2.High);
                     iTrading.Test.Globals.Assert(base.GetType().FullName + " 504 " + quotes1.Symbol.FullName + " '" + bar.ToString() + "' '" + bar2.ToString() + "'", bar.Low == bar2.Low);
                     iTrading.Test.Globals.Assert(base.GetType().FullName + " 505 " + quotes1.Symbol.FullName + " '" + bar.ToString() + "' '" + bar2.ToString() + "'", bar.Open == bar2.Open);
                     iTrading.Test.Globals.Assert(base.GetType().FullName + " 506 " + quotes1.Symbol.FullName + " '" + bar.ToString() + "' '" + bar2.ToString() + "'", bar.Volume == bar2.Volume);
                }
                IBar bar1 = quotes1.Bars[i];
            }
        }

        private void Connection_Bar(object sender, BarUpdateEventArgs e)
        {
             iTrading.Test.Globals.Assert(base.GetType().FullName + " 400", e.Error == ErrorCode.NoError);
            for (int i = e.First; i <= e.Last; i++)
            {
                IBar bar = e.Quotes.Bars[i];
                 iTrading.Test.Globals.Assert(base.GetType().FullName + " 401", bar.Close > 0.0);
                 iTrading.Test.Globals.Assert(base.GetType().FullName + " 402", bar.High > 0.0);
                 iTrading.Test.Globals.Assert(base.GetType().FullName + " 403", bar.Low > 0.0);
                 iTrading.Test.Globals.Assert(base.GetType().FullName + " 404", bar.Open > 0.0);
                 iTrading.Test.Globals.Assert(base.GetType().FullName + " 405", bar.Volume >= 0);
                 iTrading.Test.Globals.Assert(base.GetType().FullName + " 406", bar.Low <= bar.Close);
                 iTrading.Test.Globals.Assert(base.GetType().FullName + " 407", bar.Low <= bar.High);
                 iTrading.Test.Globals.Assert(base.GetType().FullName + " 409", bar.High >= bar.Close);
                 iTrading.Test.Globals.Assert(base.GetType().FullName + " 410", bar.High >= bar.Low);
                 iTrading.Test.Globals.Assert(base.GetType().FullName + " 411", bar.High >= bar.Open);
                if (this.state > 1)
                {
                     iTrading.Test.Globals.Assert(base.GetType().FullName + " 412", bar.Open == Symbol.Round2TickSize(bar.Open, 0.0001));
                     iTrading.Test.Globals.Assert(base.GetType().FullName + " 413", bar.High == Symbol.Round2TickSize(bar.High, 0.0001));
                     iTrading.Test.Globals.Assert(base.GetType().FullName + " 414", bar.Low == Symbol.Round2TickSize(bar.Low, 0.0001));
                     iTrading.Test.Globals.Assert(base.GetType().FullName + " 415", bar.Close == Symbol.Round2TickSize(bar.Close, 0.0001));
                }
                if (i > e.First)
                {
                    if (e.Quotes.Period.Id == PeriodTypeId.Tick)
                    {
                         iTrading.Test.Globals.Assert(base.GetType().FullName + " 416 " + bar.Time.ToString("yyyy-MM-dd HH:mm:ss") + " " + e.Quotes.Bars[i - 1].Time.ToString("yyyy-MM-dd HH:mm:ss"), bar.Time >= e.Quotes.Bars[i - 1].Time);
                    }
                    else
                    {
                         iTrading.Test.Globals.Assert(base.GetType().FullName + " 417 " + bar.Time.ToString("yyyy-MM-dd HH:mm:ss") + " " + e.Quotes.Bars[i - 1].Time.ToString("yyyy-MM-dd HH:mm:ss"), bar.Time > e.Quotes.Bars[i - 1].Time);
                    }
                    if (e.Quotes.Period.Id == PeriodTypeId.Minute)
                    {
                         iTrading.Test.Globals.Assert(base.GetType().FullName + " 418", bar.Time.Subtract(e.Quotes.Bars[i - 1].Time).TotalMinutes >= e.Quotes.Period.Value);
                    }
                }
            }
            QuotesHolder customLink = (QuotesHolder) e.Quotes.CustomLink;
            customLink.quotes = e.Quotes;
            lock (this.quotesHolders)
            {
                if (this.quotesHolders.Contains(customLink))
                {
                    this.quotesHolders.Remove(customLink);
                }
            }
        }

        /// <summary>
        /// Creates an instance of this class.
        /// </summary>
        /// <returns></returns>
        protected override TestBase CreateInstance()
        {
            return new  iTrading.Test.Quotes();
        }

        /// <summary>
        /// Execute test.
        /// </summary>
        protected override void DoTest()
        {
            ArrayList list;
            this.state = this.state;
            if (iTrading.Core.Kernel.Globals.TraceSwitch.Test)
            {
                Trace.WriteLine("(" + base.Connection.IdPlus + ") Test.Quotes.DoTest1");
            }
            this.state = 1;
            base.Connection.Symbols.Clear();
            foreach (SymbolTemplate template in base.SymbolTemplates)
            {
                Symbol symbol = base.Connection.GetSymbol(template.Name, template.Expiry, base.Connection.SymbolTypes[template.SymbolTypeId], base.Connection.Exchanges[template.ExchangeId], 0.0, RightId.Unknown, LookupPolicyId.RepositoryAndProvider);
                 iTrading.Test.Globals.Sleep( iTrading.Test.Globals.MilliSeconds2Sleep);
                if (((symbol != null) && !base.RunMultiple) && !base.RunMultiThread)
                {
                    lock (typeof(DataTest))
                    {
                        iTrading.Core.Kernel.Globals.DB.BeginTransaction();
                        string name = "@@" + symbol.Name;
                        Symbol symbol2 = new Symbol(null, name, symbol.Expiry, symbol.SymbolType, symbol.Exchange, symbol.StrikePrice, symbol.Right.Id, symbol.Currency, symbol.TickSize, symbol.PointValue, symbol.CompanyName, symbol.Exchanges, symbol.Splits, symbol.Dividends);
                        iTrading.Core.Kernel.Globals.DB.Delete(symbol2);
                        iTrading.Core.Kernel.Globals.DB.Update(symbol2, true);
                        if ((symbol.Name == "ES") || (symbol.Name == "MSFT"))
                        {
                            string path =  iTrading.Test.Globals.MakeTempFile();
                            Symbol symbol3 = base.Connection.GetSymbol(name, symbol.Expiry, symbol.SymbolType, symbol.Exchange, symbol.StrikePrice, symbol.Right.Id, LookupPolicyId.RepositoryOnly);
                             iTrading.Test.Globals.Assert(string.Concat(new object[] { base.GetType().FullName, " 000r ", template.Name, " ", template.Expiry, " ", template.ExchangeId }), symbol3 != null);
                            if (Directory.Exists(iTrading.Core.Kernel.Globals.InstallDir + @"db\data\" + symbol3.FullName))
                            {
                                Directory.Delete(iTrading.Core.Kernel.Globals.InstallDir + @"db\data\" + symbol3.FullName, true);
                            }
                            symbol3.MarketData.Load(iTrading.Core.Kernel.Globals.InstallDir + @"db\testdata\" + symbol.Name + ".txt");
                            symbol3.MarketData.Dump(new DateTime(0x7d4, 1, 1), new DateTime(0x7d4, 12, 0x1f), path);
                             iTrading.Test.Globals.Assert(string.Concat(new object[] { base.GetType().FullName, " 000s ", template.Name, " ", template.Expiry, " ", template.ExchangeId }),  iTrading.Test.Globals.CompareFiles(path, iTrading.Core.Kernel.Globals.InstallDir + @"db\testdata\Ref" + symbol.Name + ".txt"));
                            symbol3.MarketDepth.Load(iTrading.Core.Kernel.Globals.InstallDir + @"db\testdata\" + symbol.Name + "2.txt");
                            symbol3.MarketDepth.Dump(new DateTime(0x7d4, 1, 1), new DateTime(0x7d4, 12, 0x1f), path);
                             iTrading.Test.Globals.Assert(string.Concat(new object[] { base.GetType().FullName, " 000s ", template.Name, " ", template.Expiry, " ", template.ExchangeId }),  iTrading.Test.Globals.CompareFiles(path, iTrading.Core.Kernel.Globals.InstallDir + @"db\testdata\Ref" + symbol.Name + "2.txt"));
                            string str3 = iTrading.Core.Kernel.Globals.InstallDir + @"db\testdata\QuotesDay" + symbol.Name + ".txt";
                            if (File.Exists(str3))
                            {
                                base.Connection.Bar += new BarUpdateEventHandler(this.Connection_Bar);
                                symbol3.LoadQuotes(str3, ' ', PeriodTypeId.Day);
                                QuotesHolder holder = new QuotesHolder();
                                lock ((list = this.quotesHolders))
                                {
                                    this.quotesHolders.Add(holder);
                                }
                                symbol3.RequestQuotes(new DateTime(0x7d4, 10, 1), new DateTime(0x7d4, 10, 0x13), new Period(PeriodTypeId.Day, 1), false, LookupPolicyId.RepositoryOnly, holder);
                                while (true)
                                {
                                    lock ((list = this.quotesHolders))
                                    {
                                        if (!this.quotesHolders.Contains(holder))
                                        {
                                            break;
                                        }
                                    }
                                    Thread.Sleep(10);
                                    Application.DoEvents();
                                }
                                holder.quotes.Dump(path, ' ');
                                 iTrading.Test.Globals.CompareFiles(path, str3);
                                lock ((list = this.quotesHolders))
                                {
                                    this.quotesHolders.Add(holder);
                                }
                                symbol3.RequestQuotes(new DateTime(0x7d4, 10, 1), new DateTime(0x7d4, 10, 0x13), new Period(PeriodTypeId.Day, 3), false, LookupPolicyId.RepositoryOnly, holder);
                                while (true)
                                {
                                    lock ((list = this.quotesHolders))
                                    {
                                        if (!this.quotesHolders.Contains(holder))
                                        {
                                            break;
                                        }
                                    }
                                    Thread.Sleep(10);
                                    Application.DoEvents();
                                }
                                holder.quotes.Dump(path, ' ');
                                 iTrading.Test.Globals.CompareFiles(path, iTrading.Core.Kernel.Globals.InstallDir + @"db\testdata\QuotesDay" + symbol.Name + "-3.txt");
                                base.Connection.Bar -= new BarUpdateEventHandler(this.Connection_Bar);
                            }
                            str3 = iTrading.Core.Kernel.Globals.InstallDir + @"db\testdata\QuotesMinute" + symbol.Name + ".txt";
                            if (File.Exists(str3))
                            {
                                base.Connection.Bar += new BarUpdateEventHandler(this.Connection_Bar);
                                symbol3.LoadQuotes(str3, ' ', PeriodTypeId.Minute);
                                QuotesHolder holder2 = new QuotesHolder();
                                lock ((list = this.quotesHolders))
                                {
                                    this.quotesHolders.Add(holder2);
                                }
                                symbol3.RequestQuotes(new DateTime(0x7d4, 10, 1), new DateTime(0x7d4, 10, 3), new Period(PeriodTypeId.Minute, 1), false, LookupPolicyId.RepositoryOnly, holder2);
                                while (true)
                                {
                                    lock ((list = this.quotesHolders))
                                    {
                                        if (!this.quotesHolders.Contains(holder2))
                                        {
                                            break;
                                        }
                                    }
                                    Thread.Sleep(10);
                                    Application.DoEvents();
                                }
                                holder2.quotes.Dump(path, ' ');
                                 iTrading.Test.Globals.CompareFiles(path, str3);
                                lock ((list = this.quotesHolders))
                                {
                                    this.quotesHolders.Add(holder2);
                                }
                                symbol3.RequestQuotes(new DateTime(0x7d4, 10, 1), new DateTime(0x7d4, 10, 3), new Period(PeriodTypeId.Minute, 3), false, LookupPolicyId.RepositoryOnly, holder2);
                                while (true)
                                {
                                    lock ((list = this.quotesHolders))
                                    {
                                        if (!this.quotesHolders.Contains(holder2))
                                        {
                                            break;
                                        }
                                    }
                                    Thread.Sleep(10);
                                    Application.DoEvents();
                                }
                                holder2.quotes.Dump(path, ' ');
                                 iTrading.Test.Globals.CompareFiles(path, iTrading.Core.Kernel.Globals.InstallDir + @"db\testdata\QuotesMinute" + symbol.Name + "-3.txt");
                                base.Connection.Bar -= new BarUpdateEventHandler(this.Connection_Bar);
                            }
                            str3 = iTrading.Core.Kernel.Globals.InstallDir + @"db\testdata\QuotesTick" + symbol.Name + ".txt";
                            if (File.Exists(str3))
                            {
                                base.Connection.Bar += new BarUpdateEventHandler(this.Connection_Bar);
                                symbol3.LoadQuotes(str3, ' ', PeriodTypeId.Tick);
                                QuotesHolder holder3 = new QuotesHolder();
                                lock ((list = this.quotesHolders))
                                {
                                    this.quotesHolders.Add(holder3);
                                }
                                symbol3.RequestQuotes(new DateTime(0x7d4, 10, 1), new DateTime(0x7d4, 10, 6), new Period(PeriodTypeId.Tick, 1), false, LookupPolicyId.RepositoryOnly, holder3);
                                while (true)
                                {
                                    lock ((list = this.quotesHolders))
                                    {
                                        if (!this.quotesHolders.Contains(holder3))
                                        {
                                            break;
                                        }
                                    }
                                    Thread.Sleep(10);
                                    Application.DoEvents();
                                }
                                holder3.quotes.Dump(path, ' ');
                                 iTrading.Test.Globals.CompareFiles(path, str3);
                                lock ((list = this.quotesHolders))
                                {
                                    this.quotesHolders.Add(holder3);
                                }
                                symbol3.RequestQuotes(new DateTime(0x7d4, 10, 1), new DateTime(0x7d4, 10, 6), new Period(PeriodTypeId.Tick, 4), false, LookupPolicyId.RepositoryOnly, holder3);
                                while (true)
                                {
                                    lock ((list = this.quotesHolders))
                                    {
                                        if (!this.quotesHolders.Contains(holder3))
                                        {
                                            break;
                                        }
                                    }
                                    Thread.Sleep(10);
                                    Application.DoEvents();
                                }
                                holder3.quotes.Dump(path, ' ');
                                 iTrading.Test.Globals.CompareFiles(path, iTrading.Core.Kernel.Globals.InstallDir + @"db\testdata\QuotesTick" + symbol.Name + "-4.txt");
                                base.Connection.Bar -= new BarUpdateEventHandler(this.Connection_Bar);
                            }
                            File.Delete(path);
                        }
                        iTrading.Core.Kernel.Globals.DB.Delete(symbol2);
                        iTrading.Core.Kernel.Globals.DB.CommitTransaction();
                    }
                }
            }
            if (base.Connection.FeatureTypes[FeatureTypeId.QuotesDaily] != null)
            {
                if (iTrading.Core.Kernel.Globals.TraceSwitch.Test)
                {
                    Trace.WriteLine("(" + base.Connection.IdPlus + ") Test.Quotes.DoTest2");
                }
                this.state = 2;
                base.Connection.Bar += new BarUpdateEventHandler(this.Connection_Bar);
                for (int i = 0; i < ((base.RunMultiple || base.RunMultiThread) ? 2 : 5); i++)
                {
                    base.Connection.Symbols.Clear();
                    foreach (SymbolTemplate template2 in base.SymbolTemplates)
                    {
                        if (template2.Valid)
                        {
                            Symbol symbol4 = base.Connection.GetSymbol(template2.Name, template2.Expiry, base.Connection.SymbolTypes[template2.SymbolTypeId], base.Connection.Exchanges[template2.ExchangeId], 0.0, RightId.Unknown, LookupPolicyId.RepositoryAndProvider);
                            QuotesHolder holder4 = new QuotesHolder();
                            lock ((list = this.quotesHolders))
                            {
                                this.quotesHolders.Add(holder4);
                            }
                            symbol4.RequestQuotes(base.Connection.Now.AddDays(-30.0).Date, base.Connection.Now.Date.AddDays(-1.0).Date, new Period(PeriodTypeId.Day, 1), false, LookupPolicyId.ProviderOnly, holder4);
                            while (true)
                            {
                                lock ((list = this.quotesHolders))
                                {
                                    if (!this.quotesHolders.Contains(holder4))
                                    {
                                        break;
                                    }
                                }
                                Thread.Sleep(10);
                                Application.DoEvents();
                            }
                            QuotesHolder holder5 = new QuotesHolder();
                            lock ((list = this.quotesHolders))
                            {
                                this.quotesHolders.Add(holder5);
                            }
                            symbol4.RequestQuotes(base.Connection.Now.AddDays(-30.0).Date, base.Connection.Now.Date.AddDays(-1.0).Date, new Period(PeriodTypeId.Day, 1), false, LookupPolicyId.RepositoryAndProvider, holder5);
                            while (true)
                            {
                                lock ((list = this.quotesHolders))
                                {
                                    if (!this.quotesHolders.Contains(holder5))
                                    {
                                        break;
                                    }
                                }
                                Thread.Sleep(10);
                                Application.DoEvents();
                            }
                            this.CompareQuotes(holder4.quotes, holder5.quotes);
                        }
                    }
                }
                base.Connection.Bar -= new BarUpdateEventHandler(this.Connection_Bar);
            }
            if (base.Connection.FeatureTypes[FeatureTypeId.Quotes1Minute] != null)
            {
                if (iTrading.Core.Kernel.Globals.TraceSwitch.Test)
                {
                    Trace.WriteLine("(" + base.Connection.IdPlus + ") Test.Quotes.DoTest3");
                }
                this.state = 3;
                base.Connection.Bar += new BarUpdateEventHandler(this.Connection_Bar);
                for (int j = 0; j < ((base.RunMultiple || base.RunMultiThread) ? 2 : 5); j++)
                {
                    base.Connection.Symbols.Clear();
                    foreach (SymbolTemplate template3 in base.SymbolTemplates)
                    {
                        if (template3.Valid)
                        {
                            Symbol symbol5 = base.Connection.GetSymbol(template3.Name, template3.Expiry, base.Connection.SymbolTypes[template3.SymbolTypeId], base.Connection.Exchanges[template3.ExchangeId], 0.0, RightId.Unknown, LookupPolicyId.RepositoryAndProvider);
                            QuotesHolder holder6 = new QuotesHolder();
                            lock ((list = this.quotesHolders))
                            {
                                this.quotesHolders.Add(holder6);
                            }
                            symbol5.RequestQuotes(base.Connection.Now.AddDays(-5.0).Date, base.Connection.Now.Date.AddDays(-1.0).Date, new Period(PeriodTypeId.Minute, 3), false, LookupPolicyId.ProviderOnly, holder6);
                            while (true)
                            {
                                lock ((list = this.quotesHolders))
                                {
                                    if (!this.quotesHolders.Contains(holder6))
                                    {
                                        break;
                                    }
                                }
                                Thread.Sleep(10);
                                Application.DoEvents();
                            }
                            QuotesHolder holder7 = new QuotesHolder();
                            lock ((list = this.quotesHolders))
                            {
                                this.quotesHolders.Add(holder7);
                            }
                            symbol5.RequestQuotes(base.Connection.Now.AddDays(-5.0).Date, base.Connection.Now.Date.AddDays(-1.0).Date, new Period(PeriodTypeId.Minute, 3), false, LookupPolicyId.RepositoryAndProvider, holder7);
                            while (true)
                            {
                                lock ((list = this.quotesHolders))
                                {
                                    if (!this.quotesHolders.Contains(holder7))
                                    {
                                        break;
                                    }
                                }
                                Thread.Sleep(10);
                                Application.DoEvents();
                            }
                            this.CompareQuotes(holder6.quotes, holder7.quotes);
                        }
                    }
                }
                base.Connection.Bar -= new BarUpdateEventHandler(this.Connection_Bar);
            }
            if ((!base.RunMultiple && !base.RunMultiThread) && (base.Connection.FeatureTypes[FeatureTypeId.QuotesTick] != null))
            {
                if (iTrading.Core.Kernel.Globals.TraceSwitch.Test)
                {
                    Trace.WriteLine("(" + base.Connection.IdPlus + ") Test.Quotes.DoTest4");
                }
                this.state = 4;
                base.Connection.Bar += new BarUpdateEventHandler(this.Connection_Bar);
                base.Connection.Symbols.Clear();
                foreach (SymbolTemplate template4 in base.SymbolTemplates)
                {
                    if (template4.Valid)
                    {
                        Symbol symbol6 = base.Connection.GetSymbol(template4.Name, template4.Expiry, base.Connection.SymbolTypes[template4.SymbolTypeId], base.Connection.Exchanges[template4.ExchangeId], 0.0, RightId.Unknown, LookupPolicyId.RepositoryAndProvider);
                        QuotesHolder holder8 = new QuotesHolder();
                        lock ((list = this.quotesHolders))
                        {
                            this.quotesHolders.Add(holder8);
                        }
                        symbol6.RequestQuotes(base.Connection.Now.Date.AddDays(-1.0).Date, base.Connection.Now.Date.AddDays(-1.0).Date, new Period(PeriodTypeId.Tick, 1), false, LookupPolicyId.ProviderOnly, holder8);
                        while (true)
                        {
                            lock ((list = this.quotesHolders))
                            {
                                if (!this.quotesHolders.Contains(holder8))
                                {
                                    break;
                                }
                            }
                            Thread.Sleep(10);
                            Application.DoEvents();
                        }
                        QuotesHolder holder9 = new QuotesHolder();
                        lock ((list = this.quotesHolders))
                        {
                            this.quotesHolders.Add(holder9);
                        }
                        symbol6.RequestQuotes(base.Connection.Now.AddDays(-1.0).Date, base.Connection.Now.Date.AddDays(-1.0).Date, new Period(PeriodTypeId.Tick, 1), false, LookupPolicyId.RepositoryAndProvider, holder9);
                        while (true)
                        {
                            lock ((list = this.quotesHolders))
                            {
                                if (!this.quotesHolders.Contains(holder9))
                                {
                                    break;
                                }
                            }
                            Thread.Sleep(10);
                            Application.DoEvents();
                        }
                        this.CompareQuotes(holder8.quotes, holder9.quotes);
                    }
                }
                base.Connection.Bar -= new BarUpdateEventHandler(this.Connection_Bar);
            }
        }

        private class QuotesHolder
        {
            public iTrading.Core .Data .Quotes quotes = null;
        }
    }
}

