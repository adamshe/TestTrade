namespace iTrading.Db
{
    using System;
    using System.Collections;
    using System.Data;
    using System.Diagnostics;
    using iTrading.Core.Kernel;

    internal class Symbol
    {
        private static IDbCommand deleteBrokerSymbol = null;
        private static IDbCommand deleteDividends = null;
        private static IDbCommand deleteExchangeSymbol = null;
        private static IDbCommand deleteQuotesDaily = null;
        private static IDbCommand deleteQuotesIntraday = null;
        private static IDbCommand deleteSplits = null;
        private static IDbCommand deleteSymbol = null;
        private static IDbCommand insertBrokerSymbol = null;
        private static IDbCommand insertDividends = null;
        private static IDbCommand insertExchangeSymbol = null;
        private static IDbCommand insertSplits = null;
        private static IDbCommand insertSymbol = null;
        private static IDbCommand selectBrokerSymbol = null;
        private static IDbCommand selectDividends = null;
        private static IDbCommand selectExchangeSymbol = null;
        private static IDbCommand selectSplits = null;
        private static IDbCommand selectSymbolId = null;
        private static IDbCommand selectSymbolId2 = null;
        private static IDbCommand updateBrokerSymbol = null;
        private static IDbCommand updateSymbol = null;

        internal static bool Delete(iTrading.Core.Kernel.Symbol symbol)
        {
            int symbolId = GetSymbolId(symbol);
            if (symbolId >= 0)
            {
                if (iTrading.Db.Execution.HasExecutions(symbol))
                {
                    if (symbol.Connection != null)
                    {
                        symbol.Connection.ProcessEventArgs(new ITradingErrorEventArgs(symbol.Connection, ErrorCode.UnableToDeleteSymbol, "", "Symbol can't be deleted, since there are executions stored in the repository for this symbol"));
                    }
                    return false;
                }
                if (iTrading.Db.Order.HasOrders(symbol))
                {
                    if (symbol.Connection != null)
                    {
                        symbol.Connection.ProcessEventArgs(new ITradingErrorEventArgs(symbol.Connection, ErrorCode.UnableToDeleteSymbol, "", "Symbol can't be deleted, since there are orders stored in the repository for this symbol"));
                    }
                    return false;
                }
                if (deleteBrokerSymbol == null)
                {
                    deleteBrokerSymbol = iTrading.Db.Db.NewCommand("delete from tm_brokersymbols where symbol = @symbol", Adapter.iDbTransaction);
                    iTrading.Db.Db.AddParameter(deleteBrokerSymbol, "@symbol", "Integer");
                    deleteBrokerSymbol.Prepare();
                }
                deleteBrokerSymbol.Transaction = Adapter.iDbTransaction;
                iTrading.Db.Db.GetParameter(deleteBrokerSymbol, "@symbol").Value = symbolId;
                deleteBrokerSymbol.ExecuteNonQuery();
                if (deleteDividends == null)
                {
                    deleteDividends = iTrading.Db.Db.NewCommand("delete from tm_dividends where symbol = @symbol", Adapter.iDbTransaction);
                    iTrading.Db.Db.AddParameter(deleteDividends, "@symbol", "Integer");
                    deleteDividends.Prepare();
                }
                deleteDividends.Transaction = Adapter.iDbTransaction;
                iTrading.Db.Db.GetParameter(deleteDividends, "@symbol").Value = symbolId;
                deleteDividends.ExecuteNonQuery();
                if (deleteExchangeSymbol == null)
                {
                    deleteExchangeSymbol = iTrading.Db.Db.NewCommand("delete from tm_exchangesymbols where symbol = @symbol", Adapter.iDbTransaction);
                    iTrading.Db.Db.AddParameter(deleteExchangeSymbol, "@symbol", "Integer");
                    deleteExchangeSymbol.Prepare();
                }
                deleteExchangeSymbol.Transaction = Adapter.iDbTransaction;
                iTrading.Db.Db.GetParameter(deleteExchangeSymbol, "@symbol").Value = symbolId;
                deleteExchangeSymbol.ExecuteNonQuery();
                if (deleteQuotesDaily == null)
                {
                    deleteQuotesDaily = iTrading.Db.Db.NewCommand("delete from tm_quotesdaily where symbol = @symbol", Adapter.iDbTransaction);
                    iTrading.Db.Db.AddParameter(deleteQuotesDaily, "@symbol", "Integer");
                    deleteQuotesDaily.Prepare();
                }
                deleteQuotesDaily.Transaction = Adapter.iDbTransaction;
                iTrading.Db.Db.GetParameter(deleteQuotesDaily, "@symbol").Value = symbolId;
                deleteQuotesDaily.ExecuteNonQuery();
                if (deleteQuotesIntraday == null)
                {
                    deleteQuotesIntraday = iTrading.Db.Db.NewCommand("delete from tm_quotesintraday where symbol = @symbol", Adapter.iDbTransaction);
                    iTrading.Db.Db.AddParameter(deleteQuotesIntraday, "@symbol", "Integer");
                    deleteQuotesIntraday.Prepare();
                }
                deleteQuotesIntraday.Transaction = Adapter.iDbTransaction;
                iTrading.Db.Db.GetParameter(deleteQuotesIntraday, "@symbol").Value = symbolId;
                deleteQuotesIntraday.ExecuteNonQuery();
                if (deleteSplits == null)
                {
                    deleteSplits = iTrading.Db.Db.NewCommand("delete from tm_splits where symbol = @symbol", Adapter.iDbTransaction);
                    iTrading.Db.Db.AddParameter(deleteSplits, "@symbol", "Integer");
                    deleteSplits.Prepare();
                }
                deleteSplits.Transaction = Adapter.iDbTransaction;
                iTrading.Db.Db.GetParameter(deleteSplits, "@symbol").Value = symbolId;
                deleteSplits.ExecuteNonQuery();
                if (deleteSymbol == null)
                {
                    deleteSymbol = iTrading.Db.Db.NewCommand("delete from tm_symbols where name = @name and type = @type and strikeprice = @strikeprice and optionright = @optionright", Adapter.iDbTransaction);
                    iTrading.Db.Db.AddParameter(deleteSymbol, "@name", "VarChar", 50);
                    iTrading.Db.Db.AddParameter(deleteSymbol, "@type", "Integer");
                    iTrading.Db.Db.AddParameter(deleteSymbol, "@strikeprice", "Double");
                    iTrading.Db.Db.AddParameter(deleteSymbol, "@optionright", "Integer");
                    deleteSymbol.Prepare();
                }
                deleteSymbol.Transaction = Adapter.iDbTransaction;
                iTrading.Db.Db.GetParameter(deleteSymbol, "@name").Value = symbol.Name;
                iTrading.Db.Db.GetParameter(deleteSymbol, "@type").Value = symbol.SymbolType.Id;
                iTrading.Db.Db.GetParameter(deleteSymbol, "@strikeprice").Value = symbol.StrikePrice;
                iTrading.Db.Db.GetParameter(deleteSymbol, "@optionright").Value = symbol.Right.Id;
                deleteSymbol.ExecuteNonQuery();
            }
            return true;
        }

        private static string GetBrokerName(ProviderTypeId broker, int symbolId)
        {
            if (selectBrokerSymbol == null)
            {
                selectBrokerSymbol = iTrading.Db.Db.NewCommand("select brokername from tm_brokersymbols where symbol = @symbol and broker = @broker", Adapter.iDbTransaction);
                iTrading.Db.Db.AddParameter(selectBrokerSymbol, "@symbol", "Integer");
                iTrading.Db.Db.AddParameter(selectBrokerSymbol, "@broker", "Integer");
                selectBrokerSymbol.Prepare();
            }
            selectBrokerSymbol.Transaction = Adapter.iDbTransaction;
            iTrading.Db.Db.GetParameter(selectBrokerSymbol, "@symbol").Value = symbolId;
            iTrading.Db.Db.GetParameter(selectBrokerSymbol, "@broker").Value = broker;
            IDataReader reader = selectBrokerSymbol.ExecuteReader();
            string str = "";
            if (reader.Read())
            {
                str = (string) reader["brokername"];
            }
            reader.Close();
            return str;
        }

        private static DividendDictionary GetDividends(int symbolId)
        {
            if (selectDividends == null)
            {
                selectDividends = iTrading.Db.Db.NewCommand("select dividenddate, dividend from tm_dividends where symbol = @symbol order by dividenddate", Adapter.iDbTransaction);
                iTrading.Db.Db.AddParameter(selectDividends, "@symbol", "Integer");
                selectDividends.Prepare();
            }
            selectDividends.Transaction = Adapter.iDbTransaction;
            iTrading.Db.Db.GetParameter(selectDividends, "@symbol").Value = symbolId;
            IDataReader reader = selectDividends.ExecuteReader();
            DividendDictionary dictionary = new DividendDictionary();
            while (reader.Read())
            {
                dictionary.Add((DateTime) reader["dividenddate"], (double) reader["dividend"]);
            }
            reader.Close();
            return dictionary;
        }

        internal static int GetExchangeSymbolId(iTrading.Core.Kernel.Symbol symbol)
        {
            if (Globals.TraceSwitch.DataBase)
            {
                Trace.WriteLine("(Db) Db.Symbol.GetSymbolId '" + symbol.FullName + "'");
            }
            if (selectSymbolId == null)
            {
                selectSymbolId = iTrading.Db.Db.NewCommand("select tm_exchangesymbols.id from tm_symbols, tm_exchangesymbols where name = @name and type = @type and strikeprice = @strikeprice and optionright = @optionright and tm_symbols.id = tm_exchangesymbols.symbol and exchange = @exchange", Adapter.iDbTransaction);
                iTrading.Db.Db.AddParameter(selectSymbolId, "@name", "VarChar", 50);
                iTrading.Db.Db.AddParameter(selectSymbolId, "@type", "Integer");
                iTrading.Db.Db.AddParameter(selectSymbolId, "@strikeprice", "Double");
                iTrading.Db.Db.AddParameter(selectSymbolId, "@optionright", "Integer");
                iTrading.Db.Db.AddParameter(selectSymbolId, "@exchange", "Integer");
                selectSymbolId.Prepare();
            }
            selectSymbolId.Transaction = Adapter.iDbTransaction;
            iTrading.Db.Db.GetParameter(selectSymbolId, "@name").Value = symbol.Name;
            iTrading.Db.Db.GetParameter(selectSymbolId, "@type").Value = symbol.SymbolType.Id;
            iTrading.Db.Db.GetParameter(selectSymbolId, "@strikeprice").Value = symbol.StrikePrice;
            iTrading.Db.Db.GetParameter(selectSymbolId, "@optionright").Value = symbol.Right.Id;
            iTrading.Db.Db.GetParameter(selectSymbolId, "@exchange").Value = symbol.Exchange.Id;
            IDataReader reader = selectSymbolId.ExecuteReader();
            int num = -1;
            if (reader.Read())
            {
                num = (int) reader["id"];
                reader.Close();
                return num;
            }
            reader.Close();
            Update(symbol);
            reader = selectSymbolId.ExecuteReader();
            reader.Read();
            num = (int) reader["id"];
            reader.Close();
            return num;
        }

        private static SplitDictionary GetSplits(int symbolId)
        {
            if (selectSplits == null)
            {
                selectSplits = iTrading.Db.Db.NewCommand("select splitdate, splitfactor from tm_splits where symbol = @symbol order by splitdate", Adapter.iDbTransaction);
                iTrading.Db.Db.AddParameter(selectSplits, "@symbol", "Integer");
                selectSplits.Prepare();
            }
            selectSplits.Transaction = Adapter.iDbTransaction;
            iTrading.Db.Db.GetParameter(selectSplits, "@symbol").Value = symbolId;
            IDataReader reader = selectSplits.ExecuteReader();
            SplitDictionary dictionary = new SplitDictionary();
            while (reader.Read())
            {
                dictionary.Add((DateTime) reader["splitdate"], (double) reader["splitfactor"]);
            }
            reader.Close();
            return dictionary;
        }

        internal static int GetSymbolId(iTrading.Core.Kernel.Symbol symbol)
        {
            if (selectSymbolId2 == null)
            {
                selectSymbolId2 = iTrading.Db.Db.NewCommand("select id from tm_symbols where name = @name and type = @type and strikeprice = @strikeprice and optionright = @optionright", Adapter.iDbTransaction);
                iTrading.Db.Db.AddParameter(selectSymbolId2, "@name", "VarChar", 50);
                iTrading.Db.Db.AddParameter(selectSymbolId2, "@type", "Integer");
                iTrading.Db.Db.AddParameter(selectSymbolId2, "@strikeprice", "Double");
                iTrading.Db.Db.AddParameter(selectSymbolId2, "@optionright", "Integer");
                selectSymbolId2.Prepare();
            }
            selectSymbolId2.Transaction = Adapter.iDbTransaction;
            iTrading.Db.Db.GetParameter(selectSymbolId2, "@name").Value = symbol.Name;
            iTrading.Db.Db.GetParameter(selectSymbolId2, "@type").Value = symbol.SymbolType.Id;
            iTrading.Db.Db.GetParameter(selectSymbolId2, "@strikeprice").Value = symbol.StrikePrice;
            iTrading.Db.Db.GetParameter(selectSymbolId2, "@optionright").Value = symbol.Right.Id;
            IDataReader reader = selectSymbolId2.ExecuteReader();
            int num = -1;
            if (reader.Read())
            {
                num = (int) reader["id"];
                reader.Close();
                return num;
            }
            reader.Close();
            return num;
        }

        internal static void Init()
        {
            deleteBrokerSymbol = null;
            deleteDividends = null;
            deleteExchangeSymbol = null;
            deleteQuotesDaily = null;
            deleteQuotesIntraday = null;
            deleteSplits = null;
            deleteSymbol = null;
            insertBrokerSymbol = null;
            insertDividends = null;
            insertExchangeSymbol = null;
            insertSplits = null;
            insertSymbol = null;
            selectBrokerSymbol = null;
            selectDividends = null;
            selectExchangeSymbol = null;
            selectSplits = null;
            selectSymbolId = null;
            selectSymbolId2 = null;
            updateBrokerSymbol = null;
            updateSymbol = null;
        }

        internal static SymbolCollection Select(string name, ProviderType brokerType, string companyName, DateTime expiry, SymbolType symbolType, Exchange exchange, double strikePrice, RightId rightId, string customText)
        {
            bool flag;
            DateTime time2;
            int num9;
            if (Globals.TraceSwitch.DataBase)
            {
                Trace.WriteLine("(Db) Db.Symbol.Select '" + name + "'");
            }
            if ((symbolType != null) && (symbolType.Id != SymbolTypeId.Option))
            {
                strikePrice = 0.0;
                rightId = RightId.Unknown;
            }
            string str = "select tm_symbols.id as id, commission, companyname, customtext, exchange, expiry, margin, name, optionright, pointvalue,rollovermonths, slippage, strikePrice, symbolcurrency, ticksize, type, url ";
            str = str + "from tm_symbols, tm_exchangesymbols ";
            if ((name != null) && (brokerType != null))
            {
                str = str + ", tm_brokersymbols ";
            }
            str = str + "where 1 = 1 ";
            if ((name != null) && (brokerType == null))
            {
                str = str + "and name like @name ";
            }
            if ((name != null) && (brokerType != null))
            {
                str = str + "and brokername like @name ";
            }
            if (symbolType != null)
            {
                str = str + "and type = @type ";
            }
            if (strikePrice != 0.0)
            {
                str = str + "and strikePrice = @strikePrice ";
            }
            if (rightId != RightId.Unknown)
            {
                str = str + "and optionright = @optionright ";
            }
            if (companyName != null)
            {
                str = str + "and companyname like @companyname ";
            }
            if (customText != null)
            {
                str = str + "and customText like @customText ";
            }
            if ((name != null) && (brokerType != null))
            {
                str = str + "and broker = @broker and tm_brokersymbols.symbol = tm_symbols.id ";
            }
            IDbCommand cmd = iTrading.Db.Db.NewCommand(str + "and tm_exchangesymbols.symbol = tm_symbols.id " + "order by tm_symbols.name, tm_symbols.type", Adapter.iDbTransaction);
            if (name != null)
            {
                iTrading.Db.Db.AddParameter(cmd, "@name", "VarChar", 50);
            }
            if (symbolType != null)
            {
                iTrading.Db.Db.AddParameter(cmd, "@type", "Integer");
            }
            if (strikePrice != 0.0)
            {
                iTrading.Db.Db.AddParameter(cmd, "@strikePrice", "Double");
            }
            if (rightId != RightId.Unknown)
            {
                iTrading.Db.Db.AddParameter(cmd, "@optionright", "Integer");
            }
            if (companyName != null)
            {
                iTrading.Db.Db.AddParameter(cmd, "@companyname", "VarChar", 100);
            }
            if (customText != null)
            {
                iTrading.Db.Db.AddParameter(cmd, "@customText", "LongVarChar", 0xffff);
            }
            if ((name != null) && (brokerType != null))
            {
                iTrading.Db.Db.AddParameter(cmd, "@broker", "Integer");
            }
            if (name != null)
            {
                iTrading.Db.Db.GetParameter(cmd, "@name").Value = name;
            }
            if (symbolType != null)
            {
                iTrading.Db.Db.GetParameter(cmd, "@type").Value = symbolType.Id;
            }
            if (strikePrice != 0.0)
            {
                iTrading.Db.Db.GetParameter(cmd, "@strikePrice").Value = strikePrice;
            }
            if (rightId != RightId.Unknown)
            {
                iTrading.Db.Db.GetParameter(cmd, "@optionright").Value = rightId;
            }
            if (companyName != null)
            {
                iTrading.Db.Db.GetParameter(cmd, "@companyName").Value = companyName;
            }
            if (customText != null)
            {
                iTrading.Db.Db.GetParameter(cmd, "@customText").Value = customText;
            }
            if ((name != null) && (brokerType != null))
            {
                iTrading.Db.Db.GetParameter(cmd, "@broker").Value = brokerType.Id;
            }
            double num = 0.0;
            string str2 = "";
            CurrencyId unknown = CurrencyId.Unknown;
            string str3 = "";
            ExchangeDictionary exchanges = new ExchangeDictionary();
            DateTime maxDate = Globals.MaxDate;
            int num2 = -1;
            double num3 = 0.0;
            string str4 = "";
            double pointValue = 0.0;
            RightId id2 = RightId.Unknown;
            int num5 = 0;
            double num6 = 0.0;
            double num7 = 0.0;
            double tickSize = 0.0;
            SymbolTypeId stock = SymbolTypeId.Stock;
            string str5 = "";
            Hashtable hashtable = new Hashtable();
            IDataReader reader = cmd.ExecuteReader();
            SymbolCollection symbols = new SymbolCollection();
        Label_030D:
            do
            {
                flag = reader.Read();
                if (((!flag || !(expiry != Globals.MaxDate)) || !(expiry != Globals.ContinousContractExpiry)) || ((SymbolType.All[(SymbolTypeId) reader["type"]].Id != SymbolTypeId.Future) && (SymbolType.All[(SymbolTypeId) reader["type"]].Id != SymbolTypeId.Option)))
                {
                    break;
                }
                time2 = (DateTime) reader["expiry"];
                num9 = (int) reader["rollovermonths"];
            }
            while ((((num9 == 0) && (expiry != time2)) || (expiry < time2)) || ((num9 != 0) && ((((double) Math.Abs((int) (expiry.Month - time2.Month))) / ((double) num9)) != Math.Floor((double) (((double) Math.Abs((int) (expiry.Month - time2.Month))) / ((double) num9))))));
            if (flag || (num2 >= 0))
            {
                if (!flag || ((((int) reader["id"]) != num2) && (num2 >= 0)))
                {
                    if ((exchange == null) || (exchanges[exchange.Id] != null))
                    {
                        Exchange exchange2 = null;
                        foreach (Exchange exchange3 in exchanges.Values)
                        {
                            exchange2 = exchange3;
                            break;
                        }
                        iTrading.Core.Kernel.Symbol newSymbol = new iTrading.Core.Kernel.Symbol(null, str4, (expiry != Globals.MaxDate) ? expiry : maxDate, SymbolType.All[stock], exchange2, num7, id2, iTrading.Core.Kernel.Currency.All[unknown], tickSize, pointValue, str2, exchanges, null, null);
                        newSymbol.Commission = num;
                        newSymbol.CustomText = str3;
                        newSymbol.Margin = num3;
                        newSymbol.RolloverMonths = num5;
                        newSymbol.Slippage = num6;
                        newSymbol.Url = str5;
                        hashtable[num2] = newSymbol;
                        symbols.Add(newSymbol);
                    }
                    exchanges = new ExchangeDictionary();
                }
                if (flag)
                {
                    exchanges.Add(Exchange.All[(ExchangeId) reader["exchange"]]);
                    num = (double) reader["commission"];
                    str2 = (string) reader["companyname"];
                    unknown = (CurrencyId) reader["symbolcurrency"];
                    str3 = (string) reader["customtext"];
                    maxDate = (DateTime) reader["expiry"];
                    num2 = (int) reader["id"];
                    num3 = (double) reader["margin"];
                    str4 = (string) reader["name"];
                    id2 = (RightId) reader["optionright"];
                    pointValue = (double) reader["pointvalue"];
                    num5 = (int) reader["rollovermonths"];
                    num6 = (double) reader["slippage"];
                    num7 = (double) reader["strikeprice"];
                    tickSize = (double) reader["ticksize"];
                    stock = (SymbolTypeId) reader["type"];
                    str5 = (string) reader["url"];
                    goto Label_030D;
                }
            }
            reader.Close();
            foreach (int num10 in hashtable.Keys)
            {
                foreach (ProviderType type in ProviderType.All.Values)
                {
                    ((iTrading.Core.Kernel.Symbol) hashtable[num10]).SetProviderName(type.Id, GetBrokerName(type.Id, num10));
                }
                DividendDictionary dividends = GetDividends(num10);
                foreach (DateTime time3 in dividends.Keys)
                {
                    ((iTrading.Core.Kernel.Symbol) hashtable[num10]).Dividends.Add(time3, dividends[time3]);
                }
                SplitDictionary splits = GetSplits(num10);
                foreach (DateTime time4 in splits.Keys)
                {
                    ((iTrading.Core.Kernel.Symbol) hashtable[num10]).Splits.Add(time4, splits[time4]);
                }
            }
            return symbols;
        }

        internal static void Update(iTrading.Core.Kernel.Symbol symbol)
        {
            if (Globals.TraceSwitch.DataBase)
            {
                Trace.WriteLine("(Db) Db.Symbol.UpdateNow symbol='" + symbol.FullName + "'");
            }
            Adapter.BeginTransactionNow();
            int symbolId = GetSymbolId(symbol);
            if (symbolId < 0)
            {
                if (insertSymbol == null)
                {
                    insertSymbol = iTrading.Db.Db.NewCommand("insert into tm_symbols(commission, companyname, customtext, expiry, margin, name, optionright, pointvalue, rollovermonths, slippage, strikeprice, symbolcurrency, ticksize, type, url) values(@commission, @companyname, @customtext, @expiry, @margin, @name, @optionright, @pointvalue, @rollovermonths, @slippage, @strikeprice, @symbolcurrency, @ticksize, @type, @url)", Adapter.iDbTransaction);
                    iTrading.Db.Db.AddParameter(insertSymbol, "@commission", "Double");
                    iTrading.Db.Db.AddParameter(insertSymbol, "@companyname", "VarChar", 100);
                    iTrading.Db.Db.AddParameter(insertSymbol, "@customtext", "LongVarChar", 0xffff);
                    iTrading.Db.Db.AddParameter(insertSymbol, "@expiry", "DateTime");
                    iTrading.Db.Db.AddParameter(insertSymbol, "@margin", "Double");
                    iTrading.Db.Db.AddParameter(insertSymbol, "@name", "VarChar", 50);
                    iTrading.Db.Db.AddParameter(insertSymbol, "@optionright", "Integer");
                    iTrading.Db.Db.AddParameter(insertSymbol, "@pointvalue", "Double");
                    iTrading.Db.Db.AddParameter(insertSymbol, "@rollovermonths", "Integer");
                    iTrading.Db.Db.AddParameter(insertSymbol, "@slippage", "Double");
                    iTrading.Db.Db.AddParameter(insertSymbol, "@strikeprice", "Double");
                    iTrading.Db.Db.AddParameter(insertSymbol, "@symbolcurrency", "Integer");
                    iTrading.Db.Db.AddParameter(insertSymbol, "@ticksize", "Double");
                    iTrading.Db.Db.AddParameter(insertSymbol, "@type", "Integer");
                    iTrading.Db.Db.AddParameter(insertSymbol, "@url", "VarChar", 250);
                    insertSymbol.Prepare();
                }
                insertSymbol.Transaction = Adapter.iDbTransaction;
                iTrading.Db.Db.GetParameter(insertSymbol, "@commission").Value = symbol.Commission;
                iTrading.Db.Db.GetParameter(insertSymbol, "@companyname").Value = symbol.CompanyName;
                iTrading.Db.Db.GetParameter(insertSymbol, "@customtext").Value = symbol.CustomText;
                iTrading.Db.Db.GetParameter(insertSymbol, "@expiry").Value = iTrading.Db.Db.ConvertDateTime(symbol.Expiry.Date);
                iTrading.Db.Db.GetParameter(insertSymbol, "@margin").Value = symbol.Margin;
                iTrading.Db.Db.GetParameter(insertSymbol, "@name").Value = symbol.Name;
                iTrading.Db.Db.GetParameter(insertSymbol, "@optionright").Value = symbol.Right.Id;
                iTrading.Db.Db.GetParameter(insertSymbol, "@pointvalue").Value = symbol.PointValue;
                iTrading.Db.Db.GetParameter(insertSymbol, "@rollovermonths").Value = symbol.RolloverMonths;
                iTrading.Db.Db.GetParameter(insertSymbol, "@slippage").Value = symbol.Slippage;
                iTrading.Db.Db.GetParameter(insertSymbol, "@strikeprice").Value = symbol.StrikePrice;
                iTrading.Db.Db.GetParameter(insertSymbol, "@symbolcurrency").Value = symbol.Currency.Id;
                iTrading.Db.Db.GetParameter(insertSymbol, "@ticksize").Value = symbol.TickSize;
                iTrading.Db.Db.GetParameter(insertSymbol, "@type").Value = symbol.SymbolType.Id;
                iTrading.Db.Db.GetParameter(insertSymbol, "@url").Value = symbol.Url;
                insertSymbol.ExecuteNonQuery();
                symbolId = GetSymbolId(symbol);
            }
            if (updateSymbol == null)
            {
                updateSymbol = iTrading.Db.Db.NewCommand("update tm_symbols set commission = @commission, companyname = @companyname, customtext = @customtext, margin = @margin, pointvalue = @pointvalue, rollovermonths = @rollovermonths, slippage = @slippage, symbolcurrency = @symbolcurrency, ticksize = @ticksize, url = @url where id = @id", Adapter.iDbTransaction);
                iTrading.Db.Db.AddParameter(updateSymbol, "@commission", "Double");
                iTrading.Db.Db.AddParameter(updateSymbol, "@companyname", "VarChar", 100);
                iTrading.Db.Db.AddParameter(updateSymbol, "@customtext", "LongVarChar", 0xffff);
                iTrading.Db.Db.AddParameter(updateSymbol, "@margin", "Double");
                iTrading.Db.Db.AddParameter(updateSymbol, "@pointvalue", "Double");
                iTrading.Db.Db.AddParameter(updateSymbol, "@rollovermonths", "Integer");
                iTrading.Db.Db.AddParameter(updateSymbol, "@slippage", "Double");
                iTrading.Db.Db.AddParameter(updateSymbol, "@symbolcurrency", "Integer");
                iTrading.Db.Db.AddParameter(updateSymbol, "@ticksize", "Double");
                iTrading.Db.Db.AddParameter(updateSymbol, "@url", "VarChar", 250);
                iTrading.Db.Db.AddParameter(updateSymbol, "@id", "Integer");
                updateSymbol.Prepare();
            }
            updateSymbol.Transaction = Adapter.iDbTransaction;
            iTrading.Db.Db.GetParameter(updateSymbol, "@commission").Value = symbol.Commission;
            iTrading.Db.Db.GetParameter(updateSymbol, "@companyname").Value = symbol.CompanyName;
            iTrading.Db.Db.GetParameter(updateSymbol, "@customtext").Value = symbol.CustomText;
            iTrading.Db.Db.GetParameter(updateSymbol, "@margin").Value = symbol.Margin;
            iTrading.Db.Db.GetParameter(updateSymbol, "@pointvalue").Value = symbol.PointValue;
            iTrading.Db.Db.GetParameter(updateSymbol, "@rollovermonths").Value = symbol.RolloverMonths;
            iTrading.Db.Db.GetParameter(updateSymbol, "@slippage").Value = symbol.Slippage;
            iTrading.Db.Db.GetParameter(updateSymbol, "@symbolcurrency").Value = symbol.Currency.Id;
            iTrading.Db.Db.GetParameter(updateSymbol, "@ticksize").Value = symbol.TickSize;
            iTrading.Db.Db.GetParameter(updateSymbol, "@url").Value = symbol.Url;
            iTrading.Db.Db.GetParameter(updateSymbol, "@id").Value = symbolId;
            updateSymbol.ExecuteNonQuery();
            if (selectExchangeSymbol == null)
            {
                selectExchangeSymbol = iTrading.Db.Db.NewCommand("select id from tm_exchangesymbols where symbol = @symbol and exchange = @exchange", Adapter.iDbTransaction);
                iTrading.Db.Db.AddParameter(selectExchangeSymbol, "@symbol", "Integer");
                iTrading.Db.Db.AddParameter(selectExchangeSymbol, "@exchange", "Integer");
                selectExchangeSymbol.Prepare();
            }
            ArrayList list = new ArrayList();
            foreach (Exchange exchange in symbol.Exchanges.Values)
            {
                list.Add(exchange.Id);
            }
            foreach (ExchangeId id in list)
            {
                selectExchangeSymbol.Transaction = Adapter.iDbTransaction;
                iTrading.Db.Db.GetParameter(selectExchangeSymbol, "@symbol").Value = symbolId;
                iTrading.Db.Db.GetParameter(selectExchangeSymbol, "@exchange").Value = id;
                IDataReader reader = selectExchangeSymbol.ExecuteReader();
                if (!reader.Read())
                {
                    reader.Close();
                    if (insertExchangeSymbol == null)
                    {
                        insertExchangeSymbol = iTrading.Db.Db.NewCommand("insert into tm_exchangesymbols(exchange, symbol) values(@exchange, @symbol)", Adapter.iDbTransaction);
                        iTrading.Db.Db.AddParameter(insertExchangeSymbol, "@exchange", "Integer");
                        iTrading.Db.Db.AddParameter(insertExchangeSymbol, "@symbol", "Integer");
                        insertExchangeSymbol.Prepare();
                    }
                    insertExchangeSymbol.Transaction = Adapter.iDbTransaction;
                    iTrading.Db.Db.GetParameter(insertExchangeSymbol, "@exchange").Value = id;
                    iTrading.Db.Db.GetParameter(insertExchangeSymbol, "@symbol").Value = symbolId;
                    insertExchangeSymbol.ExecuteNonQuery();
                    continue;
                }
                reader.Close();
            }
            foreach (ProviderType type in ProviderType.All.Values)
            {
                if (GetBrokerName(type.Id, symbolId).Length == 0)
                {
                    if (insertBrokerSymbol == null)
                    {
                        insertBrokerSymbol = iTrading.Db.Db.NewCommand("insert into tm_brokersymbols(broker, brokername, symbol) values (@broker, @brokername, @symbol)", Adapter.iDbTransaction);
                        iTrading.Db.Db.AddParameter(insertBrokerSymbol, "@broker", "Integer");
                        iTrading.Db.Db.AddParameter(insertBrokerSymbol, "@brokername", "VarChar", 100);
                        iTrading.Db.Db.AddParameter(insertBrokerSymbol, "@symbol", "Integer");
                        insertBrokerSymbol.Prepare();
                    }
                    string str = symbol.GetProviderName(type.Id);
                    if (str.Length > 0)
                    {
                        insertBrokerSymbol.Transaction = Adapter.iDbTransaction;
                        iTrading.Db.Db.GetParameter(insertBrokerSymbol, "@broker").Value = type.Id;
                        iTrading.Db.Db.GetParameter(insertBrokerSymbol, "@brokername").Value = str;
                        iTrading.Db.Db.GetParameter(insertBrokerSymbol, "@symbol").Value = symbolId;
                        insertBrokerSymbol.ExecuteNonQuery();
                    }
                    continue;
                }
                if (updateBrokerSymbol == null)
                {
                    updateBrokerSymbol = iTrading.Db.Db.NewCommand("update tm_brokersymbols set brokername = @brokername where symbol = @symbol and broker = @broker", Adapter.iDbTransaction);
                    iTrading.Db.Db.AddParameter(updateBrokerSymbol, "@brokername", "VarChar", 100);
                    iTrading.Db.Db.AddParameter(updateBrokerSymbol, "@symbol", "Integer");
                    iTrading.Db.Db.AddParameter(updateBrokerSymbol, "@broker", "Integer");
                    updateBrokerSymbol.Prepare();
                }
                string providerName = symbol.GetProviderName(type.Id);
                if (providerName.Length > 0)
                {
                    updateBrokerSymbol.Transaction = Adapter.iDbTransaction;
                    iTrading.Db.Db.GetParameter(updateBrokerSymbol, "@brokername").Value = providerName;
                    iTrading.Db.Db.GetParameter(updateBrokerSymbol, "@symbol").Value = symbolId;
                    iTrading.Db.Db.GetParameter(updateBrokerSymbol, "@broker").Value = type.Id;
                    updateBrokerSymbol.ExecuteNonQuery();
                }
            }
            if (deleteDividends == null)
            {
                deleteDividends = iTrading.Db.Db.NewCommand("delete from tm_dividends where symbol = @symbol", Adapter.iDbTransaction);
                iTrading.Db.Db.AddParameter(deleteDividends, "@symbol", "Integer");
                deleteDividends.Prepare();
            }
            deleteDividends.Transaction = Adapter.iDbTransaction;
            iTrading.Db.Db.GetParameter(deleteDividends, "@symbol").Value = symbolId;
            deleteDividends.ExecuteNonQuery();
            foreach (DateTime time in symbol.Dividends.KeyCollection)
            {
                if (insertDividends == null)
                {
                    insertDividends = iTrading.Db.Db.NewCommand("insert into tm_dividends(dividenddate, dividend, symbol) values (@dividenddate, @dividend, @symbol)", Adapter.iDbTransaction);
                    iTrading.Db.Db.AddParameter(insertDividends, "@dividenddate", "DateTime");
                    iTrading.Db.Db.AddParameter(insertDividends, "@dividend", "Double");
                    iTrading.Db.Db.AddParameter(insertDividends, "@symbol", "Integer");
                    insertDividends.Prepare();
                }
                insertDividends.Transaction = Adapter.iDbTransaction;
                iTrading.Db.Db.GetParameter(insertDividends, "@dividenddate").Value = iTrading.Db.Db.ConvertDateTime(time);
                iTrading.Db.Db.GetParameter(insertDividends, "@dividend").Value = symbol.Dividends[time];
                iTrading.Db.Db.GetParameter(insertDividends, "@symbol").Value = symbolId;
                insertDividends.ExecuteNonQuery();
            }
            if (deleteSplits == null)
            {
                deleteSplits = iTrading.Db.Db.NewCommand("delete from tm_splits where symbol = @symbol", Adapter.iDbTransaction);
                iTrading.Db.Db.AddParameter(deleteSplits, "@symbol", "Integer");
                deleteSplits.Prepare();
            }
            deleteSplits.Transaction = Adapter.iDbTransaction;
            iTrading.Db.Db.GetParameter(deleteSplits, "@symbol").Value = symbolId;
            deleteSplits.ExecuteNonQuery();
            foreach (DateTime time2 in symbol.Splits.KeyCollection)
            {
                if (insertSplits == null)
                {
                    insertSplits = iTrading.Db.Db.NewCommand("insert into tm_splits(splitdate, splitfactor, symbol) values (@splitdate, @splitfactor, @symbol)", Adapter.iDbTransaction);
                    iTrading.Db.Db.AddParameter(insertSplits, "@splitdate", "DateTime");
                    iTrading.Db.Db.AddParameter(insertSplits, "@splitfactor", "Double");
                    iTrading.Db.Db.AddParameter(insertSplits, "@symbol", "Integer");
                    insertSplits.Prepare();
                }
                insertSplits.Transaction = Adapter.iDbTransaction;
                iTrading.Db.Db.GetParameter(insertSplits, "@splitdate").Value = iTrading.Db.Db.ConvertDateTime(time2);
                iTrading.Db.Db.GetParameter(insertSplits, "@splitfactor").Value = symbol.Splits[time2];
                iTrading.Db.Db.GetParameter(insertSplits, "@symbol").Value = symbolId;
                insertSplits.ExecuteNonQuery();
            }
            if (symbol.Connection != null)
            {
                symbol.Connection.Symbols.RemoveFamily(symbol);
            }
            Adapter.CommitTransactionNow();
        }
    }
}

