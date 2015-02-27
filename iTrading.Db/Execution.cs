namespace iTrading.Db
{
    using System;
    using System.Collections;
    using System.Data;
    using System.Diagnostics;
    using iTrading.Core.Kernel;

    internal class Execution
    {
        private static IDbCommand insertExecution = null;
        private static IDbCommand selectExecutionById = null;
        private static IDbCommand selectExecutionBySymbol = null;
        private static IDbCommand selectExecutions = null;
        private static IDbCommand selectExecutionsByOrderId = null;
        private static IDbCommand updateExecution = null;

        internal static ExecutionCollection GetExecutionById(iTrading.Core.Kernel.Account account, string execId)
        {
            if (Globals.TraceSwitch.DataBase)
            {
                Trace.WriteLine("(Db) Db.Execution.GetExecutionById account='" + account.Name + "' '" + execId + "'");
            }
            if (selectExecutionById == null)
            {
                selectExecutionById = iTrading.Db.Db.NewCommand("select executionid, avgprice, orderid, marketposition, quantity, executiontime, tm_exchangesymbols.exchange as exchange, tm_symbols.name as name, tm_executions.expiry as expiry, tm_symbols.type as type, tm_symbols.strikeprice as strikeprice, tm_symbols.optionright as optionright from tm_executions, tm_accounts, tm_symbols, tm_exchangesymbols where executionid = @executionid and tm_executions.symbol = tm_exchangesymbols.id and tm_exchangesymbols.symbol = tm_symbols.id and tm_executions.account = tm_accounts.id and tm_accounts.account = @account and tm_accounts.broker = @broker and tm_accounts.mode = @mode order by executiontime", Adapter.iDbTransaction);
                iTrading.Db.Db.AddParameter(selectExecutionById, "@executionid", "VarChar", 100);
                iTrading.Db.Db.AddParameter(selectExecutionById, "@account", "VarChar", 50);
                iTrading.Db.Db.AddParameter(selectExecutionById, "@broker", "Integer");
                iTrading.Db.Db.AddParameter(selectExecutionById, "@mode", "Integer");
                selectExecutionById.Prepare();
            }
            selectExecutionById.Transaction = Adapter.iDbTransaction;
            iTrading.Db.Db.GetParameter(selectExecutionById, "@executionid").Value = execId;
            iTrading.Db.Db.GetParameter(selectExecutionById, "@account").Value = account.Name;
            iTrading.Db.Db.GetParameter(selectExecutionById, "@broker").Value = account.Connection.Options.Provider.Id;
            iTrading.Db.Db.GetParameter(selectExecutionById, "@mode").Value = account.Connection.Options.Mode.Id;
            return GetExecutionCollection(account, selectExecutionById);
        }

        internal static ExecutionCollection GetExecutionCollection(iTrading.Core.Kernel.Account account, IDbCommand cmd)
        {
            ExecutionCollection executions = new ExecutionCollection();
            IDataReader reader = cmd.ExecuteReader();
            ArrayList list = new ArrayList();
            ArrayList list2 = new ArrayList();
            while (reader.Read())
            {
                list2.Add(new SymbolStore((string) reader["name"], (DateTime) reader["expiry"], account.Connection.SymbolTypes[(SymbolTypeId) reader["type"]], account.Connection.Exchanges[(ExchangeId) reader["exchange"]], (double) reader["strikeprice"], (RightId) reader["optionright"]));
                list.Add(new iTrading.Core.Kernel.Execution((string) reader["executionid"], account, null, (DateTime) reader["executiontime"], account.IsSimulation ? MarketPosition.All[(MarketPositionId) reader["marketposition"]] : account.Connection.MarketPositions[(MarketPositionId) reader["marketposition"]], (string) reader["orderid"], (int) reader["quantity"], (double) reader["avgprice"]));
            }
            reader.Close();
            for (int i = 0; i < list2.Count; i++)
            {
                iTrading.Core.Kernel.Execution execution = (iTrading.Core.Kernel.Execution) list[i];
                SymbolStore store = (SymbolStore) list2[i];
                iTrading.Core.Kernel.Symbol symbol = account.Connection.GetSymbol(store.name, store.expiry, store.symbolType, store.exchange, store.strikePrice, store.rightId, LookupPolicyId.RepositoryAndProvider);
                Trace.Assert(symbol != null, string.Concat(new object[] { "Db.Execution.GetExecutionCollection: ", store.name, " ", store.expiry, " ", store.symbolType.Name, " ", store.exchange.Name }));
                executions.Add(new iTrading.Core.Kernel.Execution(execution.Id, execution.Account, symbol, execution.Time, execution.MarketPosition, execution.OrderId, execution.Quantity, execution.AvgPrice));
            }
            return executions;
        }

        internal static ExecutionCollection GetExecutions(iTrading.Core.Kernel.Account account, DateTime minDate, DateTime maxDate)
        {
            if (Globals.TraceSwitch.DataBase)
            {
                Trace.WriteLine(string.Concat(new object[] { "(Db) Db.Execution.GetExecutions account='", account.Name, "' '", minDate, "' '", maxDate, "'" }));
            }
            if (selectExecutions == null)
            {
                selectExecutions = iTrading.Db.Db.NewCommand("select executionid, avgprice, orderid, marketposition, quantity, executiontime, tm_exchangesymbols.exchange as exchange, tm_symbols.name as name, tm_executions.expiry as expiry, tm_symbols.type as type, tm_symbols.strikeprice as strikeprice, tm_symbols.optionright as optionright from tm_executions, tm_accounts, tm_symbols, tm_exchangesymbols where @mindate <= executiontime and executiontime <= @maxdate and tm_executions.symbol = tm_exchangesymbols.id and tm_exchangesymbols.symbol = tm_symbols.id and tm_executions.account = tm_accounts.id and tm_accounts.account = @account and tm_accounts.broker = @broker and tm_accounts.mode = @mode order by executiontime", Adapter.iDbTransaction);
                iTrading.Db.Db.AddParameter(selectExecutions, "@mindate", "DateTime");
                iTrading.Db.Db.AddParameter(selectExecutions, "@maxdate", "DateTime");
                iTrading.Db.Db.AddParameter(selectExecutions, "@account", "VarChar", 50);
                iTrading.Db.Db.AddParameter(selectExecutions, "@broker", "Integer");
                iTrading.Db.Db.AddParameter(selectExecutions, "@mode", "Integer");
                selectExecutions.Prepare();
            }
            selectExecutions.Transaction = Adapter.iDbTransaction;
            iTrading.Db.Db.GetParameter(selectExecutions, "@minDate").Value = iTrading.Db.Db.ConvertDateTime(minDate);
            iTrading.Db.Db.GetParameter(selectExecutions, "@maxDate").Value = iTrading.Db.Db.ConvertDateTime(maxDate);
            iTrading.Db.Db.GetParameter(selectExecutions, "@account").Value = account.Name;
            iTrading.Db.Db.GetParameter(selectExecutions, "@broker").Value = account.Connection.Options.Provider.Id;
            iTrading.Db.Db.GetParameter(selectExecutions, "@mode").Value = account.Connection.Options.Mode.Id;
            return GetExecutionCollection(account, selectExecutions);
        }

        internal static ExecutionCollection GetExecutionsByOrderId(iTrading.Core.Kernel.Account account, string orderId)
        {
            if (Globals.TraceSwitch.DataBase)
            {
                Trace.WriteLine("(Db) Db.Execution.GetExecutionsByOrderId account='" + account.Name + "' '" + orderId + "'");
            }
            if (selectExecutionsByOrderId == null)
            {
                selectExecutionsByOrderId = iTrading.Db.Db.NewCommand("select executionid, avgprice, orderid, marketposition, quantity, executiontime, tm_exchangesymbols.exchange as exchange, tm_symbols.name as name, tm_executions.expiry as expiry, tm_symbols.type as type, tm_symbols.strikeprice as strikeprice, tm_symbols.optionright as optionright from tm_executions, tm_accounts, tm_symbols, tm_exchangesymbols where orderid = @orderid and tm_executions.symbol = tm_exchangesymbols.id and tm_exchangesymbols.symbol = tm_symbols.id and tm_executions.account = tm_accounts.id and tm_accounts.account = @account and tm_accounts.broker = @broker and tm_accounts.mode = @mode order by executiontime", Adapter.iDbTransaction);
                iTrading.Db.Db.AddParameter(selectExecutionsByOrderId, "@orderid", "VarChar", 50);
                iTrading.Db.Db.AddParameter(selectExecutionsByOrderId, "@account", "VarChar", 50);
                iTrading.Db.Db.AddParameter(selectExecutionsByOrderId, "@broker", "Integer");
                iTrading.Db.Db.AddParameter(selectExecutionsByOrderId, "@mode", "Integer");
                selectExecutionsByOrderId.Prepare();
            }
            selectExecutionsByOrderId.Transaction = Adapter.iDbTransaction;
            iTrading.Db.Db.GetParameter(selectExecutionsByOrderId, "@orderid").Value = orderId;
            iTrading.Db.Db.GetParameter(selectExecutionsByOrderId, "@account").Value = account.Name;
            iTrading.Db.Db.GetParameter(selectExecutionsByOrderId, "@broker").Value = account.Connection.Options.Provider.Id;
            iTrading.Db.Db.GetParameter(selectExecutionsByOrderId, "@mode").Value = account.Connection.Options.Mode.Id;
            return GetExecutionCollection(account, selectExecutionsByOrderId);
        }

        internal static bool HasExecutions(iTrading.Core.Kernel.Symbol symbol)
        {
            if (Globals.TraceSwitch.DataBase)
            {
                Trace.WriteLine("(Db) Db.Execution.HasExecutions symbol='" + symbol.FullName + "'");
            }
            if (selectExecutionBySymbol == null)
            {
                selectExecutionBySymbol = iTrading.Db.Db.NewCommand("select executionid from tm_executions, tm_accounts, tm_symbols, tm_exchangesymbols where tm_symbols.name = @name and tm_symbols.type = @type and tm_executions.symbol = tm_exchangesymbols.id and tm_exchangesymbols.symbol = tm_symbols.id", Adapter.iDbTransaction);
                iTrading.Db.Db.AddParameter(selectExecutionBySymbol, "@name", "VarChar", 50);
                iTrading.Db.Db.AddParameter(selectExecutionBySymbol, "@type", "Integer");
                selectExecutionBySymbol.Prepare();
            }
            selectExecutionBySymbol.Transaction = Adapter.iDbTransaction;
            iTrading.Db.Db.GetParameter(selectExecutionBySymbol, "@name").Value = symbol.Name;
            iTrading.Db.Db.GetParameter(selectExecutionBySymbol, "@type").Value = symbol.SymbolType.Id;
            IDataReader reader = selectExecutionBySymbol.ExecuteReader();
            bool flag = reader.Read();
            reader.Close();
            return flag;
        }

        internal static void Init()
        {
            insertExecution = null;
            selectExecutions = null;
            selectExecutionById = null;
            selectExecutionBySymbol = null;
            selectExecutionsByOrderId = null;
            updateExecution = null;
        }

        internal static void Update(iTrading.Core.Kernel.Execution execution)
        {
            if (Globals.TraceSwitch.DataBase)
            {
                Trace.WriteLine("(Db) Db.Execution.UpdateNow execution='" + execution.Id + "'");
            }
            Adapter.BeginTransactionNow();
            if (GetExecutionById(execution.Account, execution.Id).Count == 0)
            {
                if (insertExecution == null)
                {
                    insertExecution = iTrading.Db.Db.NewCommand("insert into tm_executions(executionid, account, avgprice, expiry, orderid, marketposition, quantity, executiontime, symbol)values(@executionid, @account, @avgprice, @expiry, @orderid, @marketposition, @quantity, @executiontime, @symbol)", Adapter.iDbTransaction);
                    iTrading.Db.Db.AddParameter(insertExecution, "@executionid", "VarChar", 100);
                    iTrading.Db.Db.AddParameter(insertExecution, "@account", "Integer");
                    iTrading.Db.Db.AddParameter(insertExecution, "@avgprice", "Double");
                    iTrading.Db.Db.AddParameter(insertExecution, "@expiry", "DateTime");
                    iTrading.Db.Db.AddParameter(insertExecution, "@orderid", "VarChar", 50);
                    iTrading.Db.Db.AddParameter(insertExecution, "@marketposition", "Integer");
                    iTrading.Db.Db.AddParameter(insertExecution, "@quantity", "Double");
                    iTrading.Db.Db.AddParameter(insertExecution, "@executiontime", "DateTime");
                    iTrading.Db.Db.AddParameter(insertExecution, "@symbol", "Integer");
                }
                int accountId = iTrading.Db.Account.GetAccountId(execution.Account);
                int exchangeSymbolId = iTrading.Db.Symbol.GetExchangeSymbolId(execution.Symbol);
                insertExecution.Transaction = Adapter.iDbTransaction;
                iTrading.Db.Db.GetParameter(insertExecution, "@executionid").Value = execution.Id;
                iTrading.Db.Db.GetParameter(insertExecution, "@account").Value = accountId;
                iTrading.Db.Db.GetParameter(insertExecution, "@avgprice").Value = execution.AvgPrice;
                iTrading.Db.Db.GetParameter(insertExecution, "@expiry").Value = iTrading.Db.Db.ConvertDateTime(execution.Symbol.Expiry);
                iTrading.Db.Db.GetParameter(insertExecution, "@orderid").Value = execution.OrderId;
                iTrading.Db.Db.GetParameter(insertExecution, "@marketposition").Value = execution.MarketPosition.Id;
                iTrading.Db.Db.GetParameter(insertExecution, "@quantity").Value = execution.Quantity;
                iTrading.Db.Db.GetParameter(insertExecution, "@executiontime").Value = iTrading.Db.Db.ConvertDateTime(execution.Time);
                iTrading.Db.Db.GetParameter(insertExecution, "@symbol").Value = exchangeSymbolId;
                insertExecution.ExecuteNonQuery();
            }
            else
            {
                if (updateExecution == null)
                {
                    updateExecution = iTrading.Db.Db.NewCommand("update tm_executions set account = @account, avgprice= @avgprice, expiry = @expiry, orderid = @orderid, marketposition = @marketposition,quantity = @quantity, executiontime = @executiontime, symbol = @symbol where executionid = @executionid", Adapter.iDbTransaction);
                    iTrading.Db.Db.AddParameter(updateExecution, "@account", "Integer");
                    iTrading.Db.Db.AddParameter(updateExecution, "@avgprice", "Double");
                    iTrading.Db.Db.AddParameter(updateExecution, "@expiry", "DateTime");
                    iTrading.Db.Db.AddParameter(updateExecution, "@orderid", "VarChar", 50);
                    iTrading.Db.Db.AddParameter(updateExecution, "@marketposition", "Integer");
                    iTrading.Db.Db.AddParameter(updateExecution, "@quantity", "Double");
                    iTrading.Db.Db.AddParameter(updateExecution, "@executiontime", "DateTime");
                    iTrading.Db.Db.AddParameter(updateExecution, "@symbol", "Integer");
                    iTrading.Db.Db.AddParameter(updateExecution, "@executionid", "VarChar", 100);
                }
                int num3 = iTrading.Db.Account.GetAccountId(execution.Account);
                int num4 = iTrading.Db.Symbol.GetExchangeSymbolId(execution.Symbol);
                updateExecution.Transaction = Adapter.iDbTransaction;
                iTrading.Db.Db.GetParameter(updateExecution, "@executionid").Value = execution.Id;
                iTrading.Db.Db.GetParameter(updateExecution, "@account").Value = num3;
                iTrading.Db.Db.GetParameter(updateExecution, "@avgprice").Value = execution.AvgPrice;
                iTrading.Db.Db.GetParameter(updateExecution, "@expiry").Value = iTrading.Db.Db.ConvertDateTime(execution.Symbol.Expiry);
                iTrading.Db.Db.GetParameter(updateExecution, "@orderid").Value = execution.OrderId;
                iTrading.Db.Db.GetParameter(updateExecution, "@marketposition").Value = execution.MarketPosition.Id;
                iTrading.Db.Db.GetParameter(updateExecution, "@quantity").Value = execution.Quantity;
                iTrading.Db.Db.GetParameter(updateExecution, "@executiontime").Value = iTrading.Db.Db.ConvertDateTime(execution.Time);
                iTrading.Db.Db.GetParameter(updateExecution, "@symbol").Value = num4;
                updateExecution.ExecuteNonQuery();
            }
            Adapter.CommitTransactionNow();
        }
    }
}

