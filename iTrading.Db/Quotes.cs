namespace iTrading.Db
{
    using System;
    using System.Data;
    using System.Diagnostics;
    using iTrading.Core.Kernel;
    using iTrading.Core.Data;

    internal class Quotes
    {
        private static IDbCommand deleteQuotesDaily = null;
        private static IDbCommand deleteQuotesIntraday = null;
        private static IDbCommand insertQuotesDaily = null;
        private static IDbCommand insertQuotesIntraday = null;
        private static IDbCommand minMaxQuotesDaily = null;
        private static IDbCommand minMaxQuotesIntraday = null;
        private static IDbCommand selectQuotesDaily = null;
        private static IDbCommand selectQuotesIntraday = null;

        internal static void Init()
        {
            deleteQuotesDaily = null;
            deleteQuotesIntraday = null;
            insertQuotesDaily = null;
            insertQuotesIntraday = null;
            minMaxQuotesDaily = null;
            minMaxQuotesIntraday = null;
            selectQuotesDaily = null;
            selectQuotesIntraday = null;
        }

        internal static int Select(iTrading.Core.Data.Quotes quotes)
        {
            if (Globals.TraceSwitch.DataBase)
            {
                Trace.WriteLine(string.Concat(new object[] { "(Db) Db.Quotes.Select symbol='", quotes.Symbol.FullName, "' from='", quotes.From, "' to='", quotes.To, "'" }));
            }
            int symbolId = iTrading.Db.Symbol.GetSymbolId(quotes.Symbol);
            Trace.Assert(symbolId >= 0, "Db.Quotes.Select: symbol ='" + quotes.Symbol.FullName + "'");
            if (quotes.Period.Id == PeriodTypeId.Day)
            {
                if (selectQuotesDaily == null)
                {
                    selectQuotesDaily = iTrading.Db.Db.NewCommand("select quotesdate, o, h, l, c, v from tm_quotesdaily where symbol = @symbol and quotesdate >= @from and quotesdate <= @to and expiry = @expiry order by quotesdate", Adapter.iDbTransaction);
                    iTrading.Db.Db.AddParameter(selectQuotesDaily, "@symbol", "Integer");
                    iTrading.Db.Db.AddParameter(selectQuotesDaily, "@from", "DateTime");
                    iTrading.Db.Db.AddParameter(selectQuotesDaily, "@to", "DateTime");
                    iTrading.Db.Db.AddParameter(selectQuotesDaily, "@expiry", "DateTime");
                    selectQuotesDaily.Prepare();
                }
                selectQuotesDaily.Transaction = Adapter.iDbTransaction;
                iTrading.Db.Db.GetParameter(selectQuotesDaily, "@symbol").Value = symbolId;
                iTrading.Db.Db.GetParameter(selectQuotesDaily, "@from").Value = iTrading.Db.Db.ConvertDateTime(quotes.From);
                iTrading.Db.Db.GetParameter(selectQuotesDaily, "@to").Value = iTrading.Db.Db.ConvertDateTime(quotes.To);
                iTrading.Db.Db.GetParameter(selectQuotesDaily, "@expiry").Value = iTrading.Db.Db.ConvertDateTime(quotes.Symbol.Expiry);
                IDataReader reader = selectQuotesDaily.ExecuteReader();
                while (reader.Read())
                {
                    quotes.Bars.Add((double) reader["o"], (double) reader["h"], (double) reader["l"], (double) reader["c"], (DateTime) reader["quotesdate"], (int) reader["v"], false);
                }
                reader.Close();
                if (minMaxQuotesDaily == null)
                {
                    minMaxQuotesDaily = iTrading.Db.Db.NewCommand("select min(quotesdate), max(quotesdate) from tm_quotesdaily where symbol = @symbol and expiry = @expiry", Adapter.iDbTransaction);
                    iTrading.Db.Db.AddParameter(minMaxQuotesDaily, "@symbol", "Integer");
                    iTrading.Db.Db.AddParameter(minMaxQuotesDaily, "@expiry", "DateTime");
                    minMaxQuotesDaily.Prepare();
                }
                minMaxQuotesDaily.Transaction = Adapter.iDbTransaction;
                iTrading.Db.Db.GetParameter(minMaxQuotesDaily, "@symbol").Value = symbolId;
                iTrading.Db.Db.GetParameter(minMaxQuotesDaily, "@expiry").Value = iTrading.Db.Db.ConvertDateTime(quotes.Symbol.Expiry);
                reader = minMaxQuotesDaily.ExecuteReader();
                if ((!reader.Read() || reader.IsDBNull(0)) || ((((DateTime) reader[0]) > quotes.From) || (quotes.Bars.Count == 0)))
                {
                    reader.Close();
                    return 2;
                }
                if (((DateTime) reader[1]) < quotes.To)
                {
                    reader.Close();
                    return 1;
                }
                reader.Close();
            }
            else
            {
                if (selectQuotesIntraday == null)
                {
                    selectQuotesIntraday = iTrading.Db.Db.NewCommand("select numbytes, quotes from tm_quotesintraday where symbol = @symbol and quotesdate >= @from and quotesdate <= @to and expiry = @expiry and tick = @tick order by quotesdate", Adapter.iDbTransaction);
                    iTrading.Db.Db.AddParameter(selectQuotesIntraday, "@symbol", "Integer");
                    iTrading.Db.Db.AddParameter(selectQuotesIntraday, "@from", "DateTime");
                    iTrading.Db.Db.AddParameter(selectQuotesIntraday, "@to", "DateTime");
                    iTrading.Db.Db.AddParameter(selectQuotesIntraday, "@expiry", "DateTime");
                    iTrading.Db.Db.AddParameter(selectQuotesIntraday, "@tick", "Integer");
                    selectQuotesIntraday.Prepare();
                }
                selectQuotesIntraday.Transaction = Adapter.iDbTransaction;
                iTrading.Db.Db.GetParameter(selectQuotesIntraday, "@symbol").Value = symbolId;
                iTrading.Db.Db.GetParameter(selectQuotesIntraday, "@from").Value = iTrading.Db.Db.ConvertDateTime(quotes.From);
                iTrading.Db.Db.GetParameter(selectQuotesIntraday, "@to").Value = iTrading.Db.Db.ConvertDateTime(quotes.To);
                iTrading.Db.Db.GetParameter(selectQuotesIntraday, "@expiry").Value = iTrading.Db.Db.ConvertDateTime(quotes.Symbol.Expiry);
                iTrading.Db.Db.GetParameter(selectQuotesIntraday, "@tick").Value = quotes.Period.Id == PeriodTypeId.Tick;
                IDataReader reader2 = selectQuotesIntraday.ExecuteReader();
                while (reader2.Read())
                {
                    byte[] buffer = new byte[(int) reader2["numbytes"]];
                    reader2.GetBytes(1, 0L, buffer, 0, buffer.Length);
                    quotes.FromBytes(buffer);
                }
                reader2.Close();
                if (minMaxQuotesIntraday == null)
                {
                    minMaxQuotesIntraday = iTrading.Db.Db.NewCommand("select min(quotesdate), max(quotesdate) from tm_quotesintraday where symbol = @symbol and expiry = @expiry and tick = @tick", Adapter.iDbTransaction);
                    iTrading.Db.Db.AddParameter(minMaxQuotesIntraday, "@symbol", "Integer");
                    iTrading.Db.Db.AddParameter(minMaxQuotesIntraday, "@expiry", "DateTime");
                    iTrading.Db.Db.AddParameter(minMaxQuotesIntraday, "@tick", "Integer");
                    minMaxQuotesIntraday.Prepare();
                }
                minMaxQuotesIntraday.Transaction = Adapter.iDbTransaction;
                iTrading.Db.Db.GetParameter(minMaxQuotesIntraday, "@symbol").Value = symbolId;
                iTrading.Db.Db.GetParameter(minMaxQuotesIntraday, "@expiry").Value = iTrading.Db.Db.ConvertDateTime(quotes.Symbol.Expiry);
                iTrading.Db.Db.GetParameter(minMaxQuotesIntraday, "@tick").Value = quotes.Period.Id == PeriodTypeId.Tick;
                reader2 = minMaxQuotesIntraday.ExecuteReader();
                if ((!reader2.Read() || reader2.IsDBNull(0)) || ((((DateTime) reader2[0]) > quotes.From) || (quotes.Bars.Count == 0)))
                {
                    reader2.Close();
                    return 2;
                }
                if (((DateTime) reader2[1]) < quotes.To)
                {
                    reader2.Close();
                    return 1;
                }
                reader2.Close();
            }
            return 0;
        }

        internal static void Update(iTrading.Core.Data.Quotes quotes)
        {
            if (Globals.TraceSwitch.DataBase)
            {
                Trace.WriteLine("(Db) Db.Quotes.Update symbol='" + quotes.Symbol.FullName + "'");
            }
            Adapter.BeginTransactionNow();
            int symbolId = iTrading.Db.Symbol.GetSymbolId(quotes.Symbol);
            Trace.Assert(symbolId >= 0, "Db.Quotes.Update: symbol ='" + quotes.Symbol.FullName + "'");
            if (quotes.Period.Id == PeriodTypeId.Day)
            {
                if (deleteQuotesDaily == null)
                {
                    deleteQuotesDaily = iTrading.Db.Db.NewCommand("delete from tm_quotesdaily where symbol = @symbol and quotesdate >= @from and quotesdate <= @to and expiry = @expiry", Adapter.iDbTransaction);
                    iTrading.Db.Db.AddParameter(deleteQuotesDaily, "@symbol", "Integer");
                    iTrading.Db.Db.AddParameter(deleteQuotesDaily, "@from", "DateTime");
                    iTrading.Db.Db.AddParameter(deleteQuotesDaily, "@to", "DateTime");
                    iTrading.Db.Db.AddParameter(deleteQuotesDaily, "@expiry", "DateTime");
                    deleteQuotesDaily.Prepare();
                }
                deleteQuotesDaily.Transaction = Adapter.iDbTransaction;
                iTrading.Db.Db.GetParameter(deleteQuotesDaily, "@symbol").Value = symbolId;
                iTrading.Db.Db.GetParameter(deleteQuotesDaily, "@from").Value = iTrading.Db.Db.ConvertDateTime(quotes.From);
                iTrading.Db.Db.GetParameter(deleteQuotesDaily, "@to").Value = iTrading.Db.Db.ConvertDateTime(quotes.To);
                iTrading.Db.Db.GetParameter(deleteQuotesDaily, "@expiry").Value = iTrading.Db.Db.ConvertDateTime(quotes.Symbol.Expiry);
                deleteQuotesDaily.ExecuteNonQuery();
                if (insertQuotesDaily == null)
                {
                    insertQuotesDaily = iTrading.Db.Db.NewCommand("insert into tm_quotesdaily(quotesdate, symbol, expiry, o, h, l, c, v) values(@quotesdate, @symbol, @expiry, @o, @h, @l, @c, @v)", Adapter.iDbTransaction);
                    iTrading.Db.Db.AddParameter(insertQuotesDaily, "@quotesdate", "DateTime");
                    iTrading.Db.Db.AddParameter(insertQuotesDaily, "@symbol", "Integer");
                    iTrading.Db.Db.AddParameter(insertQuotesDaily, "@expiry", "DateTime");
                    iTrading.Db.Db.AddParameter(insertQuotesDaily, "@o", "Double");
                    iTrading.Db.Db.AddParameter(insertQuotesDaily, "@h", "Double");
                    iTrading.Db.Db.AddParameter(insertQuotesDaily, "@l", "Double");
                    iTrading.Db.Db.AddParameter(insertQuotesDaily, "@c", "Double");
                    iTrading.Db.Db.AddParameter(insertQuotesDaily, "@v", "Integer");
                    insertQuotesDaily.Prepare();
                }
                insertQuotesDaily.Transaction = Adapter.iDbTransaction;
                foreach (IBar bar in quotes.Bars)
                {
                    iTrading.Db.Db.GetParameter(insertQuotesDaily, "@quotesdate").Value = iTrading.Db.Db.ConvertDateTime(bar.Time.Date);
                    iTrading.Db.Db.GetParameter(insertQuotesDaily, "@symbol").Value = symbolId;
                    iTrading.Db.Db.GetParameter(insertQuotesDaily, "@expiry").Value = iTrading.Db.Db.ConvertDateTime(quotes.Symbol.Expiry);
                    iTrading.Db.Db.GetParameter(insertQuotesDaily, "@o").Value = bar.Open;
                    iTrading.Db.Db.GetParameter(insertQuotesDaily, "@h").Value = bar.High;
                    iTrading.Db.Db.GetParameter(insertQuotesDaily, "@l").Value = bar.Low;
                    iTrading.Db.Db.GetParameter(insertQuotesDaily, "@c").Value = bar.Close;
                    iTrading.Db.Db.GetParameter(insertQuotesDaily, "@v").Value = bar.Volume;
                    insertQuotesDaily.ExecuteNonQuery();
                }
            }
            else
            {
                if (deleteQuotesIntraday == null)
                {
                    deleteQuotesIntraday = iTrading.Db.Db.NewCommand("delete from tm_quotesintraday where symbol = @symbol and quotesdate >= @from and quotesdate <= @to and expiry = @expiry and tick = @tick", Adapter.iDbTransaction);
                    iTrading.Db.Db.AddParameter(deleteQuotesIntraday, "@symbol", "Integer");
                    iTrading.Db.Db.AddParameter(deleteQuotesIntraday, "@from", "DateTime");
                    iTrading.Db.Db.AddParameter(deleteQuotesIntraday, "@to", "DateTime");
                    iTrading.Db.Db.AddParameter(deleteQuotesIntraday, "@expiry", "DateTime");
                    iTrading.Db.Db.AddParameter(deleteQuotesIntraday, "@tick", "Integer");
                    deleteQuotesIntraday.Prepare();
                }
                deleteQuotesIntraday.Transaction = Adapter.iDbTransaction;
                iTrading.Db.Db.GetParameter(deleteQuotesIntraday, "@symbol").Value = symbolId;
                iTrading.Db.Db.GetParameter(deleteQuotesIntraday, "@from").Value = iTrading.Db.Db.ConvertDateTime(quotes.From);
                iTrading.Db.Db.GetParameter(deleteQuotesIntraday, "@to").Value = iTrading.Db.Db.ConvertDateTime(quotes.To);
                iTrading.Db.Db.GetParameter(deleteQuotesIntraday, "@expiry").Value = iTrading.Db.Db.ConvertDateTime(quotes.Symbol.Expiry);
                iTrading.Db.Db.GetParameter(deleteQuotesIntraday, "@tick").Value = quotes.Period.Id == PeriodTypeId.Tick;
                deleteQuotesIntraday.ExecuteNonQuery();
                int first = 0;
                for (int i = 0; i < quotes.Bars.Count; i++)
                {
                    if (((i + 1) >= quotes.Bars.Count) || (quotes.Bars[i + 1].Time.Date != quotes.Bars[first].Time.Date))
                    {
                        byte[] buffer = quotes.ToBytes(first, i);
                        QuotesDay quotesDay = quotes.Bars.GetQuotesDay(quotes.Bars[first].Time.Date);
                        Trace.Assert(quotesDay != null, "Db.Quotes.Update: " + quotes.Bars[first].Time.Date);
                        if (Adapter.connectionType == ConnectionTypeId.MySql)
                        {
                            if (insertQuotesIntraday == null)
                            {
                                insertQuotesIntraday = iTrading.Db.Db.NewCommand("insert into tm_quotesintraday(quotesdate, symbol, expiry, tick, o, h, l, c, v, numbytes, quotes) values(@quotesdate, @symbol, @expiry, @tick, @o, @h, @l, @c, @v, @numbytes, @quotes)", Adapter.iDbTransaction);
                                iTrading.Db.Db.AddParameter(insertQuotesIntraday, "@quotesdate", "DateTime");
                                iTrading.Db.Db.AddParameter(insertQuotesIntraday, "@symbol", "Integer");
                                iTrading.Db.Db.AddParameter(insertQuotesIntraday, "@expiry", "DateTime");
                                iTrading.Db.Db.AddParameter(insertQuotesIntraday, "@tick", "Integer");
                                iTrading.Db.Db.AddParameter(insertQuotesIntraday, "@o", "Double");
                                iTrading.Db.Db.AddParameter(insertQuotesIntraday, "@h", "Double");
                                iTrading.Db.Db.AddParameter(insertQuotesIntraday, "@l", "Double");
                                iTrading.Db.Db.AddParameter(insertQuotesIntraday, "@c", "Double");
                                iTrading.Db.Db.AddParameter(insertQuotesIntraday, "@v", "Integer");
                                iTrading.Db.Db.AddParameter(insertQuotesIntraday, "@numbytes", "Integer");
                                iTrading.Db.Db.AddParameter(insertQuotesIntraday, "@quotes", "Binary");
                                insertQuotesIntraday.Prepare();
                            }
                            insertQuotesIntraday.Transaction = Adapter.iDbTransaction;
                            iTrading.Db.Db.GetParameter(insertQuotesIntraday, "@quotesdate").Value = iTrading.Db.Db.ConvertDateTime(quotes.Bars[first].Time.Date);
                            iTrading.Db.Db.GetParameter(insertQuotesIntraday, "@symbol").Value = symbolId;
                            iTrading.Db.Db.GetParameter(insertQuotesIntraday, "@expiry").Value = iTrading.Db.Db.ConvertDateTime(quotes.Symbol.Expiry);
                            iTrading.Db.Db.GetParameter(insertQuotesIntraday, "@tick").Value = quotes.Period.Id == PeriodTypeId.Tick;
                            iTrading.Db.Db.GetParameter(insertQuotesIntraday, "@o").Value = quotesDay.Open;
                            iTrading.Db.Db.GetParameter(insertQuotesIntraday, "@h").Value = quotesDay.High;
                            iTrading.Db.Db.GetParameter(insertQuotesIntraday, "@l").Value = quotesDay.Low;
                            iTrading.Db.Db.GetParameter(insertQuotesIntraday, "@c").Value = quotesDay.Close;
                            iTrading.Db.Db.GetParameter(insertQuotesIntraday, "@v").Value = quotesDay.Volume;
                            iTrading.Db.Db.GetParameter(insertQuotesIntraday, "@numbytes").Value = buffer.Length;
                            iTrading.Db.Db.GetParameter(insertQuotesIntraday, "@quotes").Value = buffer;
                            insertQuotesIntraday.ExecuteNonQuery();
                        }
                        else
                        {
                            insertQuotesIntraday = iTrading.Db.Db.NewCommand("insert into tm_quotesintraday(quotesdate, symbol, expiry, tick, o, h, l, c, v, numbytes, quotes) values(@quotesdate, @symbol, @expiry, @tick, @o, @h, @l, @c, @v, @numbytes, @quotes)", Adapter.iDbTransaction);
                            iTrading.Db.Db.AddParameter(insertQuotesIntraday, "@quotesdate", "DateTime");
                            iTrading.Db.Db.AddParameter(insertQuotesIntraday, "@symbol", "Integer");
                            iTrading.Db.Db.AddParameter(insertQuotesIntraday, "@expiry", "DateTime");
                            iTrading.Db.Db.AddParameter(insertQuotesIntraday, "@tick", "Integer");
                            iTrading.Db.Db.AddParameter(insertQuotesIntraday, "@o", "Double");
                            iTrading.Db.Db.AddParameter(insertQuotesIntraday, "@h", "Double");
                            iTrading.Db.Db.AddParameter(insertQuotesIntraday, "@l", "Double");
                            iTrading.Db.Db.AddParameter(insertQuotesIntraday, "@c", "Double");
                            iTrading.Db.Db.AddParameter(insertQuotesIntraday, "@v", "Integer");
                            iTrading.Db.Db.AddParameter(insertQuotesIntraday, "@numbytes", "Integer");
                            iTrading.Db.Db.AddParameter(insertQuotesIntraday, "@quotes", "Binary", buffer.Length);
                            iTrading.Db.Db.GetParameter(insertQuotesIntraday, "@quotesdate").Value = iTrading.Db.Db.ConvertDateTime(quotes.Bars[first].Time.Date);
                            iTrading.Db.Db.GetParameter(insertQuotesIntraday, "@symbol").Value = symbolId;
                            iTrading.Db.Db.GetParameter(insertQuotesIntraday, "@expiry").Value = iTrading.Db.Db.ConvertDateTime(quotes.Symbol.Expiry);
                            iTrading.Db.Db.GetParameter(insertQuotesIntraday, "@tick").Value = quotes.Period.Id == PeriodTypeId.Tick;
                            iTrading.Db.Db.GetParameter(insertQuotesIntraday, "@o").Value = quotesDay.Open;
                            iTrading.Db.Db.GetParameter(insertQuotesIntraday, "@h").Value = quotesDay.High;
                            iTrading.Db.Db.GetParameter(insertQuotesIntraday, "@l").Value = quotesDay.Low;
                            iTrading.Db.Db.GetParameter(insertQuotesIntraday, "@c").Value = quotesDay.Close;
                            iTrading.Db.Db.GetParameter(insertQuotesIntraday, "@v").Value = quotesDay.Volume;
                            iTrading.Db.Db.GetParameter(insertQuotesIntraday, "@numbytes").Value = buffer.Length;
                            iTrading.Db.Db.GetParameter(insertQuotesIntraday, "@quotes").Value = buffer;
                            insertQuotesIntraday.ExecuteNonQuery();
                        }
                        first = i + 1;
                    }
                }
            }
            Adapter.CommitTransactionNow();
        }
    }
}

