namespace iTrading.Db
{
    using System;
    using System.Data;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Text.RegularExpressions;
    using iTrading.Core.Kernel;

    internal class Create
    {
        private static IDbCommand insertVersion = null;
        private static IDbCommand selectVersion = null;

        internal static void Init()
        {
            insertVersion = null;
            selectVersion = null;
        }

        internal static void Update(int version)
        {
            if (selectVersion == null)
            {
                selectVersion = iTrading.Db.Db.NewCommand("select version from tm_version where version = @version", Adapter.iDbTransaction);
                iTrading.Db.Db.AddParameter(selectVersion, "@version", "Integer");
                selectVersion.Prepare();
            }
            iTrading.Db.Db.GetParameter(selectVersion, "@version").Value = version;
            IDataReader reader = selectVersion.ExecuteReader();
            if (reader.Read())
            {
                reader.Close();
                return;
            }
            reader.Close();
            if (Globals.TraceSwitch.DataBase)
            {
                Trace.WriteLine("(Db) Db.Create.Update " + version);
            }
            switch (version)
            {
                case 200:
                    iTrading.Db.Db.Create("create table tm_accounts(id %counter%,account %varchar%(50) not null,broker %long% not null,primary key(id))");
                    iTrading.Db.Db.Create("create unique index tm_accounts_ui on tm_accounts(account, broker)");
                    iTrading.Db.Db.Create("create table tm_symbols(id %counter%,companyname %varchar%(100),customtext %memo%,expiry datetime,name %varchar%(50) not null,pointvalue %double%,rollovermonths %long%,ticksize %double%,type %long% not null,url %varchar%(250),primary key(id))");
                    iTrading.Db.Db.Create("create unique index tm_symbols_ui on tm_symbols(name, type)");
                    iTrading.Db.Db.Create("create table tm_brokersymbols(broker %long% not null,brokername %varchar%(50) not null,symbol %long% not null,primary key(symbol, broker),foreign key(symbol) references tm_symbols(id))");
                    if (Adapter.connectionType != ConnectionTypeId.MySql)
                    {
                        iTrading.Db.Db.Create("create table tm_exchangesymbols(id %counter%,exchange %long% not null,symbol %long% not null,primary key(id),foreign key(symbol) references tm_symbols(id))");
                        iTrading.Db.Db.Create("create unique index tm_exchangesymbols_ui on tm_exchangesymbols(symbol, exchange)");
                        break;
                    }
                    iTrading.Db.Db.Create("create table tm_exchangesymbols(id %counter%,exchange %long% not null,symbol %long% not null,primary key(id))");
                    iTrading.Db.Db.Create("create unique index tm_exchangesymbols_ui on tm_exchangesymbols(symbol, exchange)");
                    iTrading.Db.Db.Create("alter table tm_exchangesymbols add constraint foreign key(symbol) references tm_symbols(id)");
                    break;

                case 0xc9:
                    iTrading.Db.Db.Create("alter table tm_accounts add simulation %long%");
                    iTrading.Db.Db.Create("update tm_accounts set simulation = 0");
                    goto Label_0489;

                case 0xca:
                    iTrading.Db.Db.DropIndex("tm_accounts", "tm_accounts_ui");
                    iTrading.Db.Db.Create("alter table tm_accounts add mode %long%");
                    iTrading.Db.Db.Create("update tm_accounts set mode = 0");
                    iTrading.Db.Db.Create("create unique index tm_accounts_ui on tm_accounts(account, broker, mode)");
                    goto Label_0489;

                case 0xcb:
                    iTrading.Db.Db.DropIndex("tm_executions", "tm_executions_i0");
                    iTrading.Db.Db.Create("alter table tm_executions %alter% column executionid %varchar%(100) not null");
                    iTrading.Db.Db.Create("create index tm_executions_i0 on tm_executions(executionid)");
                    goto Label_0489;

                case 300:
                    iTrading.Db.Db.Create("alter table tm_symbols add symbolcurrency %long%");
                    iTrading.Db.Db.Create("update tm_symbols set symbolcurrency = 7");
                    if (Adapter.connectionType != ConnectionTypeId.MySql)
                    {
                        iTrading.Db.Db.Create("create table tm_splits(id %counter%,splitdate datetime not null,splitfactor %double%,symbol %long% not null,primary key(id),foreign key(symbol) references tm_symbols(id))");
                        iTrading.Db.Db.Create("create unique index tm_splits_ui on tm_splits(symbol, splitdate)");
                    }
                    else
                    {
                        iTrading.Db.Db.Create("create table tm_splits(id %counter%,splitdate datetime not null,splitfactor %double%,symbol %long% not null,primary key(id))");
                        iTrading.Db.Db.Create("create unique index tm_splits_ui on tm_splits(symbol, splitdate)");
                        iTrading.Db.Db.Create("alter table tm_splits add constraint foreign key(symbol) references tm_symbols(id)");
                    }
                    goto Label_0489;

                case 0x12d:
                    if (Adapter.connectionType != ConnectionTypeId.MySql)
                    {
                        iTrading.Db.Db.Create("create table tm_dividends(id %counter%,dividenddate datetime not null,dividend %double%,symbol %long% not null,primary key(id),foreign key(symbol) references tm_symbols(id))");
                        iTrading.Db.Db.Create("create unique index tm_dividends_ui on tm_dividends(symbol, dividenddate)");
                    }
                    else
                    {
                        iTrading.Db.Db.Create("create table tm_dividends(id %counter%,dividenddate datetime not null,dividend %double%,symbol %long% not null,primary key(id))");
                        iTrading.Db.Db.Create("create unique index tm_dividends_ui on tm_dividends(symbol, dividenddate)");
                        iTrading.Db.Db.Create("alter table tm_dividends add constraint foreign key(symbol) references tm_symbols(id)");
                    }
                    goto Label_0489;

                case 0x12e:
                    iTrading.Db.Db.Create("create table tm_quotesdaily(quotesdate datetime not null,symbol %long% not null,expiry datetime not null,o %double%,h %double%,l %double%,c %double%,v %long%,primary key(symbol, quotesdate, expiry),foreign key(symbol) references tm_symbols(id))");
                    iTrading.Db.Db.Create("create table tm_quotesintraday(quotesdate datetime not null,symbol %long% not null,expiry datetime not null,tick %long% not null,o %double%,h %double%,l %double%,c %double%,v %long%,numbytes %long%,quotes %binary%,primary key(symbol, quotesdate, expiry, tick),foreign key(symbol) references tm_symbols(id))");
                    goto Label_0489;

                case 0x12f:
                    iTrading.Db.Db.DropIndex("tm_symbols", "tm_symbols_ui");
                    iTrading.Db.Db.Create("alter table tm_symbols add strikeprice %double%");
                    iTrading.Db.Db.Create("alter table tm_symbols add optionright %long%");
                    iTrading.Db.Db.Create("update tm_symbols set strikeprice = 0");
                    iTrading.Db.Db.Create("update tm_symbols set optionright = 2");
                    iTrading.Db.Db.Create("alter table tm_symbols %alter% column strikeprice %double% not null");
                    iTrading.Db.Db.Create("alter table tm_symbols %alter% column optionright %long% not null");
                    iTrading.Db.Db.Create("create unique index tm_symbols_ui on tm_symbols(name, type, strikeprice, optionright)");
                    goto Label_0489;

                case 0x130:
                    iTrading.Db.Db.Create("alter table tm_symbols add commission %double%");
                    iTrading.Db.Db.Create("alter table tm_symbols add margin %double%");
                    iTrading.Db.Db.Create("alter table tm_symbols add slippage %double%");
                    iTrading.Db.Db.Create("update tm_symbols set commission = 0");
                    iTrading.Db.Db.Create("update tm_symbols set margin = 0");
                    iTrading.Db.Db.Create("update tm_symbols set slippage = 0");
                    goto Label_0489;

                case 0x131:
                {
                    iTrading.Db.Db.Create("alter table tm_accounts add numbytes %long%");
                    iTrading.Db.Db.Create("alter table tm_accounts add customtext %binary%");
                    IDbCommand cmd = iTrading.Db.Db.NewCommand("update tm_accounts set numbytes = @numbytes, customtext = @customtext", Adapter.iDbTransaction);
                    iTrading.Db.Db.AddParameter(cmd, "@numbytes", "Integer");
                    iTrading.Db.Db.AddParameter(cmd, "@customtext", "Binary", 0);
                    iTrading.Db.Db.GetParameter(cmd, "@numbytes").Value = 0;
                    iTrading.Db.Db.GetParameter(cmd, "@customtext").Value = new byte[0];
                    cmd.ExecuteNonQuery();
                    goto Label_0489;
                }
                default:
                    goto Label_0489;
            }
            if (Adapter.connectionType == ConnectionTypeId.MySql)
            {
                iTrading.Db.Db.Create("create table tm_executions(id %counter%,executionid %varchar%(50) not null,account %long% not null,avgprice %double%,expiry datetime,orderid %varchar%(50),marketposition %long%,quantity %long%,executiontime datetime,symbol %long% not null,primary key(id))");
                iTrading.Db.Db.Create("create index tm_executions_i0 on tm_executions(executionid)");
                iTrading.Db.Db.Create("create index tm_executions_i1 on tm_executions(symbol)");
                iTrading.Db.Db.Create("create index tm_executions_i2 on tm_executions(account)");
                iTrading.Db.Db.Create("alter table tm_executions add constraint foreign key(symbol) references tm_exchangesymbols(id)");
                iTrading.Db.Db.Create("alter table tm_executions add constraint foreign key(account) references tm_accounts(id)");
            }
            else
            {
                iTrading.Db.Db.Create("create table tm_executions(id %counter%,executionid %varchar%(50) not null,account %long% not null,avgprice %double%,expiry datetime,orderid %varchar%(50),marketposition %long%,quantity %long%,executiontime datetime,symbol %long% not null,primary key(id),foreign key(symbol) references tm_exchangesymbols(id),foreign key(account) references tm_accounts(id))");
                iTrading.Db.Db.Create("create index tm_executions_i0 on tm_executions(executionid)");
            }
            if (Adapter.connectionType == ConnectionTypeId.MySql)
            {
                iTrading.Db.Db.Create("create table tm_orders(actiontype %long%,account %long% not null,avgfillprice %double%,customtext %memo%,expiry datetime,filled %long%,orderid %varchar%(50),limitprice %double%,ocagroup %varchar%(50),ordertype %long%,quantity %long%,orderstate %long%,stopprice %double%,symbol %long% not null,updatetime datetime,timeinforce %long%,token %varchar%(50) not null,primary key(token))");
                iTrading.Db.Db.Create("create index tm_orders_i0 on tm_orders(orderid, account)");
                iTrading.Db.Db.Create("create index tm_orders_i1 on tm_orders(updatetime)");
                iTrading.Db.Db.Create("create index tm_orders_i2 on tm_orders(account)");
                iTrading.Db.Db.Create("create index tm_orders_i3 on tm_orders(symbol)");
                iTrading.Db.Db.Create("alter table tm_orders add constraint foreign key(account) references tm_accounts(id)");
                iTrading.Db.Db.Create("alter table tm_orders add constraint foreign key(symbol) references tm_exchangesymbols(id)");
            }
            else
            {
                iTrading.Db.Db.Create("create table tm_orders(actiontype %long%,account %long% not null,avgfillprice %double%,customtext %memo%,expiry datetime,filled %long%,orderid %varchar%(50),limitprice %double%,ocagroup %varchar%(50),ordertype %long%,quantity %long%,orderstate %long%,stopprice %double%,symbol %long% not null,updatetime datetime,timeinforce %long%,token %varchar%(50) not null,primary key(token),foreign key(account) references tm_accounts(id),foreign key(symbol) references tm_exchangesymbols(id))");
                iTrading.Db.Db.Create("create index tm_orders_i0 on tm_orders(orderid, account)");
                iTrading.Db.Db.Create("create index tm_orders_i1 on tm_orders(updatetime)");
            }
            if (Adapter.connectionType == ConnectionTypeId.MySql)
            {
                iTrading.Db.Db.Create("create table tm_orderhistories(id %counter%,avgfillprice %double%,error %long%,filled %long%,limitprice %double%,nativeerror %varchar%(200),token %varchar%(50),orderid %varchar%(50),orderstate %long%,quantity %long%,stopprice %double%,updatetime datetime,primary key(id))");
                iTrading.Db.Db.Create("create index tm_orderhistories_i on tm_orderhistories(token)");
                iTrading.Db.Db.Create("alter table tm_orders add constraint foreign key(token) references tm_orders(token)");
            }
            else
            {
                iTrading.Db.Db.Create("create table tm_orderhistories(id %counter%,avgfillprice %double%,error %long%,filled %long%,limitprice %double%,nativeerror %varchar%(200),token %varchar%(50),orderid %varchar%(50),orderstate %long%,quantity %long%,stopprice %double%,updatetime datetime,primary key(id),foreign key(token) references tm_orders(token))");
                iTrading.Db.Db.Create("create index tm_orderhistories_i on tm_orderhistories(token)");
            }
        Label_0489:
            if (insertVersion == null)
            {
                insertVersion = iTrading.Db.Db.NewCommand("insert into tm_version (version, updated) values (@version, @updated)");
                iTrading.Db.Db.AddParameter(insertVersion, "@version", "Integer");
                iTrading.Db.Db.AddParameter(insertVersion, "@updated", "DateTime");
            }
            iTrading.Db.Db.GetParameter(insertVersion, "@version").Value = version;
            iTrading.Db.Db.GetParameter(insertVersion, "@updated").Value = iTrading.Db.Db.ConvertDateTime(DateTime.Now);
            insertVersion.ExecuteNonQuery();
        }

        internal static void UpdateSymbols()
        {
            if (File.Exists(Globals.InstallDir + @"\db\Symbols.txt"))
            {
                CultureInfo provider = new CultureInfo("en-US");
                bool flag = false;
                string input = null;
                StreamReader reader = File.OpenText(Globals.InstallDir + @"\db\Symbols.txt");
                Globals.Progress.Initialise(200, true);
                while ((input = reader.ReadLine()) != null)
                {
                    input = Regex.Replace(Regex.Replace(input, @"^[\ \t]*", ""), @"[\ \t]*$", "");
                    if ((input.Length != 0) && (input[0] != '#'))
                    {
                        string[] strArray = input.Split(new char[] { ';' });
                        if (strArray.Length != 13)
                        {
                            flag = true;
                            Trace.WriteLine(string.Concat(new object[] { "ERROR on processing 'Symbols.txt' file, wrong number of fields (", strArray.Length, "): ", input }));
                        }
                        else
                        {
                            if (strArray[0].Length == 0)
                            {
                                flag = true;
                                Trace.WriteLine("ERROR on processing 'Symbols.txt' file, symbol name field is mandatory: " + input);
                                continue;
                            }
                            if (strArray[2].Length == 0)
                            {
                                flag = true;
                                Trace.WriteLine("ERROR on processing 'Symbols.txt' file, symbol type field is mandatory: " + input);
                                continue;
                            }
                            if (strArray[5].Length == 0)
                            {
                                flag = true;
                                Trace.WriteLine("ERROR on processing 'Symbols.txt' file, currency field is mandatory: " + input);
                                continue;
                            }
                            if (strArray[7].Length == 0)
                            {
                                flag = true;
                                Trace.WriteLine("ERROR on processing 'Symbols.txt' file, pointvalue field is mandatory: " + input);
                                continue;
                            }
                            if (strArray[8].Length == 0)
                            {
                                flag = true;
                                Trace.WriteLine("ERROR on processing 'Symbols.txt' file, ticksize field is mandatory: " + input);
                                continue;
                            }
                            if (strArray[11].Length == 0)
                            {
                                flag = true;
                                Trace.WriteLine("ERROR on processing 'Symbols.txt' file, exchanges is mandatory: " + input);
                                continue;
                            }
                            try
                            {
                                string name = strArray[0].Trim();
                                string companyName = (strArray[1].Trim().Length == 0) ? "" : strArray[1].Trim();
                                SymbolType symbolType = SymbolType.All[(SymbolTypeId) Convert.ToInt32(strArray[2].Trim(), provider)];
                                double strikePrice = (strArray[3].Trim().Length == 0) ? 0.0 : Convert.ToDouble(strArray[3].Trim(), provider);
                                Right right = Right.All[(strArray[4].Trim().Length == 0) ? RightId.Unknown : ((RightId) Convert.ToInt32(strArray[4].Trim(), provider))];
                                iTrading.Core.Kernel.Currency currency = iTrading.Core.Kernel.Currency.All[(CurrencyId) Convert.ToInt32(strArray[5].Trim(), provider)];
                                DateTime expiry = (strArray[6].Trim().Length == 0) ? Globals.MaxDate : new DateTime(Convert.ToInt32(strArray[6].Trim().Substring(0, 4), provider), Convert.ToInt32(strArray[6].Trim().Substring(4, 2), provider), 1);
                                double pointValue = Convert.ToDouble(strArray[7].Trim(), provider);
                                double tickSize = Convert.ToDouble(strArray[8].Trim(), provider);
                                int num4 = (strArray[9].Trim().Length == 0) ? 0 : Convert.ToInt32(strArray[9].Trim(), provider);
                                string str4 = (strArray[10].Trim().Length == 0) ? "" : strArray[10].Trim();
                                string str5 = strArray[11].Trim();
                                string[] strArray2 = (strArray[12].Trim().Length == 0) ? null : strArray[12].Trim().Split(new char[] { ',' });
                                if ((strArray2 != null) && (strArray2.Length < 4))
                                {
                                    flag = true;
                                    Trace.WriteLine("ERROR on processing 'Symbols.txt' file, wrong number of values in field 'brokernames' (" + strArray2.Length + ")");
                                }
                                else
                                {
                                    Exchange exchange = null;
                                    ExchangeDictionary exchanges = new ExchangeDictionary();
                                    foreach (string str6 in str5.Split(new char[] { ',' }))
                                    {
                                        Exchange exchange2 = Exchange.All.Find(str6);
                                        if (exchange2 == null)
                                        {
                                            flag = true;
                                            Trace.WriteLine("ERROR on processing 'Symbols.txt' file, exchange '" + str6 + "' is not supported: " + input);
                                        }
                                        else
                                        {
                                            exchange = exchange2;
                                            exchanges.Add(exchange2);
                                        }
                                    }
                                    if (currency == null)
                                    {
                                        flag = true;
                                        Trace.WriteLine("ERROR on processing 'Symbols.txt' file, no valid currency found: " + input);
                                    }
                                    else if (exchange == null)
                                    {
                                        flag = true;
                                        Trace.WriteLine("ERROR on processing 'Symbols.txt' file, no valid exchanges found: " + input);
                                    }
                                    else if (symbolType == null)
                                    {
                                        flag = true;
                                        Trace.WriteLine("ERROR on processing 'Symbols.txt' file, no valid symbol type found: " + input);
                                    }
                                    else if (right == null)
                                    {
                                        flag = true;
                                        Trace.WriteLine("ERROR on processing 'Symbols.txt' file, no valid option right found: " + input);
                                    }
                                    else
                                    {
                                        SymbolCollection symbols = Globals.DB.Select(name, null, null, expiry, symbolType, null, 0.0, RightId.Unknown, null);
                                        iTrading.Core.Kernel.Symbol symbol = null;
                                        if (symbols.Count > 0)
                                        {
                                            symbol = symbols[0];
                                        }
                                        iTrading.Core.Kernel.Symbol symbol2 = new iTrading.Core.Kernel.Symbol(null, name, expiry, symbolType, exchange, strikePrice, right.Id, currency, tickSize, pointValue, companyName, exchanges, (symbol == null) ? null : symbol.Splits, (symbol == null) ? null : symbol.Dividends);
                                        symbol2.CustomText = (symbol == null) ? "" : symbol.CustomText;
                                        symbol2.RolloverMonths = num4;
                                        symbol2.Url = str4;
                                        for (int i = 0; i < strArray2.Length; i++)
                                        {
                                            string providerName = (strArray2[i].Length == 0) ? name : strArray2[i];
                                            switch (i)
                                            {
                                                case 0:
                                                    symbol2.SetProviderName(ProviderTypeId.InteractiveBrokers, providerName);
                                                    break;

                                                case 1:
                                                    symbol2.SetProviderName(ProviderTypeId.MBTrading, providerName);
                                                    break;

                                                case 2:
                                                    symbol2.SetProviderName(ProviderTypeId.Patsystems, providerName);
                                                    break;

                                                case 3:
                                                    symbol2.SetProviderName(ProviderTypeId.TrackData, providerName);
                                                    break;

                                                case 4:
                                                    symbol2.SetProviderName(ProviderTypeId.Dtn, providerName);
                                                    break;

                                                case 5:
                                                    symbol2.SetProviderName(ProviderTypeId.ESignal, providerName);
                                                    break;

                                                case 6:
                                                    symbol2.SetProviderName(ProviderTypeId.Yahoo, providerName);
                                                    break;

                                                case 7:
                                                    symbol2.SetProviderName(ProviderTypeId.CyberTrader, providerName);
                                                    break;
                                            }
                                        }
                                        Globals.Progress.PerformStep();
                                        Globals.Progress.Message = "Loading/updating symbol '" + symbol2.FullName + "'";
                                        if (Globals.Progress.IsAborted)
                                        {
                                            break;
                                        }
                                        Globals.DB.BeginTransaction();
                                        Globals.DB.Update(symbol2, true);
                                        Globals.DB.CommitTransaction();
                                    }
                                }
                                continue;
                            }
                            catch (Exception exception)
                            {
                                Trace.WriteLine("ERROR on processing 'Symbols.txt' file, exception caught: " + input + ": " + exception.Message);
                                continue;
                            }
                        }
                    }
                }
                reader.Close();
                Globals.Progress.Terminate();
                if (!flag)
                {
                    File.Delete(Globals.InstallDir + @"\db\Symbols.txt");
                }
            }
        }
    }
}

