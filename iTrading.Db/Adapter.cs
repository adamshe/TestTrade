namespace iTrading.Db
{
    using System;
    using System.Collections;
    using System.Data;
    using System.Diagnostics;
    using System.Threading;
    using System.Xml;
    using iTrading.Core.Kernel;
    using iTrading.Core.Data;

    internal class Adapter : IDb
    {
        private static Queue commands = new Queue();
        internal static ConnectionTypeId connectionType = ConnectionTypeId.OleDb;
        private static int countAdapters = 0;
        private static int countTransactions = 0;
        internal static IDbConnection iDbConnection = null;
        internal static IDbTransaction iDbTransaction = null;
        private object syncConnect = new object[0];
        private int[] versions = new int[] { 200, 0xc9, 0xca, 0xcb, 300, 0x12d, 0x12e, 0x12f, 0x130, 0x131 };
        private static Thread workerThread = null;

        public void BeginTransaction()
        {
            lock (SyncRootNow)
            {
                BeginTransactionNow();
            }
        }

        internal static void BeginTransactionNow()
        {
            if (countTransactions++ == 0)
            {
                if (Globals.TraceSwitch.DataBase)
                {
                    Trace.WriteLine("(Db) Db.Adapter.BeginTransaction");
                }
                iDbTransaction = iDbConnection.BeginTransaction(IsolationLevel.ReadUncommitted);
            }
        }

        public void CommitTransaction()
        {
            lock (SyncRootNow)
            {
                CommitTransactionNow();
            }
        }

        internal static void CommitTransactionNow()
        {
            if (iDbTransaction == null)
            {
                throw new TMException(ErrorCode.Panic, "Db.Adapter.CommitTransaction: not in transaction");
            }
            if (--countTransactions == 0)
            {
                if (Globals.TraceSwitch.DataBase)
                {
                    Trace.WriteLine("(Db) Db.Adapter.CommitTransaction");
                }
                iDbTransaction.Commit();
                iDbTransaction = null;
            }
        }

        public void Connect(int historyMaintained)
        {
            lock (SyncRootNow)
            {
                if (Globals.TraceSwitch.DataBase)
                {
                    Trace.WriteLine("(Db) Db.Adapter.Connect");
                }
                if (workerThread == null)
                {
                    this.Init();
                    workerThread = new Thread(new ThreadStart(this.Start));
                    workerThread.Name = "TM DB";
                    workerThread.Start();
                }
                if (countAdapters++ == 0)
                {
                    string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;User ID=Admin;Data Source=" + Globals.InstallDir + @"\db\TradeMagic.mdb";
                    connectionType = ConnectionTypeId.OleDb;
                    try
                    {
                        XmlDocument document = new XmlDocument();
                        XmlTextReader reader = new XmlTextReader(Globals.InstallDir + @"\Config.xml");
                        document.Load(reader);
                        reader.Close();
                        connectionString = document["TradeMagic"]["Db"]["ConnectionString"].InnerText;
                        if (document["TradeMagic"]["Db"]["ConnectionType"].InnerText == "SqlClient")
                        {
                            connectionType = ConnectionTypeId.SqlClient;
                        }
                        else if (document["TradeMagic"]["Db"]["ConnectionType"].InnerText == "MySql")
                        {
                            connectionType = ConnectionTypeId.MySql;
                        }
                    }
                    catch (Exception)
                    {
                    }
                    try
                    {
                        iDbConnection = iTrading.Db.Db.NewConnection(connectionString);
                        iDbConnection.Open();
                    }
                    catch (Exception exception)
                    {
                        throw new TMException(ErrorCode.Panic, "Unable to open database, most likely TradeMagic is not installed properly: " + exception.Message);
                    }
                    if (!iTrading.Db.Db.TableExists("tm_version"))
                    {
                        iTrading.Db.Db.Create("create table tm_version(version %long% not null,updated datetime,primary key(version))");
                    }
                    IDataReader reader2 = iTrading.Db.Db.NewCommand("select max(version) from tm_version", iDbTransaction).ExecuteReader();
                    reader2.Read();
                    int num = 0;
                    if (!(reader2[0] is DBNull))
                    {
                        num = (int) reader2[0];
                        if (this.versions[this.versions.Length - 1] < num)
                        {
                            reader2.Close();
                            throw new TMException(ErrorCode.Panic, "Database must be used with a more current version of TradeMagic");
                        }
                    }
                    reader2.Close();
                    if (num != this.versions[this.versions.Length - 1])
                    {
                        Globals.Progress.Initialise(this.versions.Length, false);
                        foreach (int num2 in this.versions)
                        {
                            Globals.Progress.PerformStep();
                            Globals.Progress.Message = "Updating repository";
                            Create.Update(num2);
                        }
                        Globals.Progress.Terminate();
                    }
                    if (historyMaintained > 0)
                    {
                        IDbCommand cmd = iTrading.Db.Db.NewCommand("delete from tm_orderhistories where updatetime < @updatetime");
                        iTrading.Db.Db.AddParameter(cmd, "@updatetime", "DateTime");
                        iTrading.Db.Db.GetParameter(cmd, "@updatetime").Value = iTrading.Db.Db.ConvertDateTime(DateTime.Now.AddDays((double) -historyMaintained));
                        cmd.ExecuteNonQuery();
                    }
                    Create.UpdateSymbols();
                }
            }
        }

        public bool Delete(iTrading.Core.Kernel.Account account)
        {
            lock (SyncRootNow)
            {
                return iTrading.Db.Account.Delete(account);
            }
        }

        public bool Delete(iTrading.Core.Kernel.Symbol symbol)
        {
            lock (SyncRootNow)
            {
                if (symbol.Connection != null)
                {
                    symbol.Connection.Symbols.RemoveFamily(symbol);
                }
                return iTrading.Db.Symbol.Delete(symbol);
            }
        }

        public void Disconnect()
        {
            lock (this.syncConnect)
            {
                if (iDbConnection != null)
                {
                    lock (commands)
                    {
                        commands.Enqueue(new object[] { this, Command.Disconnect });
                        Monitor.Pulse(commands);
                    }
                    Monitor.Wait(this.syncConnect);
                }
            }
        }

        private void DisconnectNow()
        {
            if (Globals.TraceSwitch.DataBase)
            {
                Trace.WriteLine("(Db) Db.Adapter.DisconnectNow");
            }
            if (--countAdapters == 0)
            {
                iDbConnection.Close();
                iDbConnection = null;
                workerThread = null;
            }
            lock (this.syncConnect)
            {
                Monitor.Pulse(this.syncConnect);
            }
            if (countAdapters == 0)
            {
                Thread.CurrentThread.Abort();
            }
        }

        private void Init()
        {
            countAdapters = 0;
            countTransactions = 0;
            commands = new Queue();
            iDbConnection = null;
            iDbTransaction = null;
            workerThread = null;
            iTrading.Db.Account.Init();
            Create.Init();
            iTrading.Db.Execution.Init();
            iTrading.Db.Order.Init();
            iTrading.Db.Quotes.Init();
            iTrading.Db.Symbol.Init();
        }

        public void Insert(OrderStatusEventArgs e, bool synchronous)
        {
            if (synchronous)
            {
                lock (SyncRootNow)
                {
                    iTrading.Db.Order.Insert(e);
                }
            }
            else
            {
                lock (commands)
                {
                    commands.Enqueue(new object[] { this, Command.InsertHistory, e });
                    Monitor.Pulse(commands);
                }
            }
        }

        public void Recover(iTrading.Core.Kernel.Account account)
        {
            lock (SyncRootNow)
            {
                if (Globals.TraceSwitch.DataBase)
                {
                    Trace.WriteLine("(Db) Db.Adapter.RecoverOnConnect");
                }
                this.BeginTransaction();
                iTrading.Db.Account.Restore(account);
                foreach (iTrading.Core.Kernel.Execution execution in account.Executions)
                {
                    if (iTrading.Db.Execution.GetExecutionById(account, execution.Id).Count == 0)
                    {
                        iTrading.Db.Execution.Update(execution);
                    }
                }
                foreach (iTrading.Core.Kernel.Execution execution2 in iTrading.Db.Execution.GetExecutions(account, account.Connection.Now.Date, account.Connection.Now.AddDays(1.0).Date))
                {
                    if (account.Executions.FindByExecId(execution2.Id) == null)
                    {
                        account.Connection.ProcessEventArgs(new ExecutionUpdateEventArgs(account.Connection, ErrorCode.NoError, "", Operation.Insert, execution2.Id, execution2.Account, execution2.Symbol, execution2.Time, execution2.MarketPosition, execution2.OrderId, execution2.Quantity, execution2.AvgPrice));
                    }
                }
                OrderCollection orders = new OrderCollection();
                foreach (iTrading.Core.Kernel.Order order in account.Orders)
                {
                    orders.Add(order);
                }
                account.Orders.Clear();
                foreach (iTrading.Core.Kernel.Order order2 in orders)
                {
                    iTrading.Core.Kernel.Order orderById = iTrading.Db.Order.GetOrderById(order2);
                    if (((orderById != null) && (order2.TimeInForce.Id == TimeInForceId.Day)) && (order2.Time.Date != orderById.Time.Date))
                    {
                        orderById = null;
                    }
                    iTrading.Core.Kernel.Order order4 = new iTrading.Core.Kernel.Order(account, order2.Symbol, order2.Action, order2.OrderType, order2.TimeInForce, order2.Quantity, ((orderById != null) && (orderById.OrderType.Id != order2.OrderType.Id)) ? orderById.LimitPrice : order2.LimitPrice, ((orderById != null) && (orderById.OrderType.Id != order2.OrderType.Id)) ? orderById.StopPrice : order2.StopPrice, (orderById != null) ? orderById.Token : order2.Token, order2.OrderState, order2.OrderId, (orderById != null) ? orderById.OcaGroup : order2.OcaGroup, order2.Filled, order2.AvgFillPrice, order2.Time);
                    order2.CustomText = order4.CustomText = (orderById != null) ? orderById.CustomText : order2.CustomText;
                    foreach (OrderStatusEventArgs args in order2.History)
                    {
                        order4.History.Add(new OrderStatusEventArgs(order4, args.Error, args.NativeError, args.OrderId, args.LimitPrice, args.StopPrice, args.Quantity, args.AvgFillPrice, args.Filled, args.OrderState, args.Time));
                    }
                    if (orderById != null)
                    {
                        iTrading.Db.Order.UpdateByToken(order4);
                    }
                    else
                    {
                        iTrading.Db.Order.Insert(order4);
                    }
                }
                foreach (iTrading.Core.Kernel.Order order5 in iTrading.Db.Order.GetOrders(account, account.Connection.Now.Date, account.Connection.Now.AddDays(1.0).Date))
                {
                    iTrading.Core.Kernel.Order order6 = orders.FindByOrderId(order5.OrderId);
                    account.Orders.Add((order6 != null) ? order6 : order5);
                    if (order6 != null)
                    {
                        order6.CustomText = order5.CustomText;
                        order6.SetToken(order5.Token);
                        if ((account.Connection.FeatureTypes[FeatureTypeId.NativeOcaOrders] == null) || (account.Connection.Options.Provider.Id == ProviderTypeId.TrackData))
                        {
                            order6.SetOcaGroup(order5.OcaGroup);
                        }
                    }
                    if ((((order5.OrderState.Id != OrderStateId.Cancelled) && (order5.OrderState.Id != OrderStateId.Filled)) && (order5.OrderState.Id != OrderStateId.Rejected)) && (((order6 == null) || (order6.OrderState.Id == OrderStateId.Cancelled)) || ((order6.OrderState.Id == OrderStateId.Filled) || (order6.OrderState.Id == OrderStateId.Rejected))))
                    {
                        OrderStatusEventArgs eventArgs = null;
                        ExecutionCollection executionsByOrderId = iTrading.Db.Execution.GetExecutionsByOrderId(account, order5.OrderId);
                        if (executionsByOrderId.Count > 0)
                        {
                            double avgFillPrice = 0.0;
                            int filled = 0;
                            DateTime now = account.Connection.Now;
                            foreach (iTrading.Core.Kernel.Execution execution3 in executionsByOrderId)
                            {
                                avgFillPrice = ((avgFillPrice * filled) + (execution3.AvgPrice * execution3.Quantity)) / ((double) (filled + execution3.Quantity));
                                filled += execution3.Quantity;
                                now = execution3.Time;
                            }
                            if (filled == order5.Quantity)
                            {
                                eventArgs = new OrderStatusEventArgs(order5, ErrorCode.NoError, "", order5.OrderId, order5.LimitPrice, order5.StopPrice, order5.Quantity, avgFillPrice, filled, account.Connection.OrderStates[OrderStateId.Filled], now);
                                account.Connection.ProcessEventArgs(eventArgs);
                                iTrading.Db.Order.Insert(eventArgs);
                                continue;
                            }
                        }
                        foreach (OrderStatusEventArgs args3 in iTrading.Db.Order.GetOrderHistory(order5))
                        {
                            if (((args3.OrderState.Id == OrderStateId.PendingSubmit) && (args3.Time.Date == account.Connection.Now.Date)) && !account.IsSimulation)
                            {
                                eventArgs = new OrderStatusEventArgs(order5, ErrorCode.NoError, "", order5.OrderId, order5.LimitPrice, order5.StopPrice, order5.Quantity, order5.AvgFillPrice, order5.Filled, account.Connection.OrderStates[OrderStateId.Cancelled], account.Connection.Now);
                            }
                        }
                        if ((eventArgs == null) && !account.IsSimulation)
                        {
                            eventArgs = new OrderStatusEventArgs(order5, ErrorCode.NoError, "Order unrecoverable. Probably cancelled by broker", order5.OrderId, order5.LimitPrice, order5.StopPrice, order5.Quantity, order5.AvgFillPrice, order5.Filled, account.Connection.OrderStates[OrderStateId.Unknown], account.Connection.Now);
                        }
                        if (eventArgs != null)
                        {
                            account.Connection.ProcessEventArgs(eventArgs);
                            iTrading.Db.Order.Insert(eventArgs);
                        }
                    }
                }
                foreach (iTrading.Core.Kernel.Order order7 in account.Orders)
                {
                    order7.History.Clear();
                    foreach (OrderStatusEventArgs args4 in iTrading.Db.Order.GetOrderHistory(order7))
                    {
                        order7.History.Add(new OrderStatusEventArgs(order7, args4.Error, args4.NativeError, args4.OrderId, args4.LimitPrice, args4.StopPrice, args4.Quantity, args4.AvgFillPrice, args4.Filled, args4.OrderState, args4.Time));
                    }
                    iTrading.Core.Kernel.Order order8 = orders.FindByOrderId(order7.OrderId);
                    if (order8 != null)
                    {
                        foreach (OrderStatusEventArgs args5 in order8.History)
                        {
                            bool flag = false;
                            if ((args5.OrderState.Id == OrderStateId.Filled) || (args5.OrderState.Id == OrderStateId.PartFilled))
                            {
                                foreach (OrderStatusEventArgs args6 in order7.History)
                                {
                                    if (((args6.OrderState.Id == args5.OrderState.Id) && (args6.Filled == args5.Filled)) && (args6.AvgFillPrice == args5.AvgFillPrice))
                                    {
                                        flag = true;
                                    }
                                }
                                if (!flag)
                                {
                                    account.Connection.ProcessEventArgs(new OrderStatusEventArgs(order7, args5.Error, args5.NativeError, args5.OrderId, args5.LimitPrice, args5.StopPrice, args5.Quantity, args5.AvgFillPrice, args5.Filled, args5.OrderState, args5.Time));
                                }
                                continue;
                            }
                            if ((args5.OrderState.Id == OrderStateId.Cancelled) || (args5.OrderState.Id == OrderStateId.Rejected))
                            {
                                foreach (OrderStatusEventArgs args7 in order7.History)
                                {
                                    if (args7.OrderState.Id == args5.OrderState.Id)
                                    {
                                        flag = true;
                                    }
                                }
                                if (!flag)
                                {
                                    account.Connection.ProcessEventArgs(new OrderStatusEventArgs(order7, args5.Error, args5.NativeError, args5.OrderId, args5.LimitPrice, args5.StopPrice, args5.Quantity, args5.AvgFillPrice, args5.Filled, args5.OrderState, args5.Time));
                                }
                            }
                        }
                        continue;
                    }
                }
                if (account.Connection.FeatureTypes[FeatureTypeId.NativeOcaOrders] == null)
                {
                    ArrayList list = new ArrayList();
                    foreach (iTrading.Core.Kernel.Order order9 in account.Orders)
                    {
                        if ((order9.OcaGroup.Length > 0) && (((order9.OrderState.Id == OrderStateId.Cancelled) || (order9.OrderState.Id == OrderStateId.Filled)) || (order9.OrderState.Id == OrderStateId.Rejected)))
                        {
                            list.Add(order9.OcaGroup);
                        }
                    }
                    foreach (iTrading.Core.Kernel.Order order10 in account.Orders)
                    {
                        if (order10.OcaGroup.Length > 0)
                        {
                            foreach (string str in list)
                            {
                                if ((((order10.OcaGroup == str) && (order10.OrderState.Id != OrderStateId.Cancelled)) && ((order10.OrderState.Id != OrderStateId.Filled) && (order10.OrderState.Id != OrderStateId.Initialized))) && (((order10.OrderState.Id != OrderStateId.PendingCancel) && (order10.OrderState.Id != OrderStateId.Rejected)) && (order10.OrderState.Id != OrderStateId.Unknown)))
                                {
                                    Trace.WriteLine("WARNING: order '" + order10.OrderId + "' cancelled, since an order of the OCA group '" + order10.OcaGroup + "' has been closed");
                                    order10.Cancel();
                                }
                            }
                            continue;
                        }
                    }
                }
                if (account.Connection.Options.Provider.Id == ProviderTypeId.TrackData)
                {
                    ArrayList list2 = new ArrayList();
                    foreach (iTrading.Core.Kernel.Order order11 in account.Orders)
                    {
                        if ((order11.OcaGroup.Length > 0) && ((order11.OrderState.Id == OrderStateId.Filled) || (order11.OrderState.Id == OrderStateId.PartFilled)))
                        {
                            list2.Add(order11.OcaGroup);
                        }
                    }
                    foreach (iTrading.Core.Kernel.Order order12 in account.Orders)
                    {
                        if (order12.OcaGroup.Length > 0)
                        {
                            foreach (string str2 in list2)
                            {
                                if ((((order12.OcaGroup == str2) && (order12.OrderState.Id != OrderStateId.Cancelled)) && ((order12.OrderState.Id != OrderStateId.Filled) && (order12.OrderState.Id != OrderStateId.Initialized))) && (((order12.OrderState.Id != OrderStateId.PendingCancel) && (order12.OrderState.Id != OrderStateId.Rejected)) && (order12.OrderState.Id != OrderStateId.Unknown)))
                                {
                                    Trace.WriteLine("WARNING: order '" + order12.OrderId + "' cancelled, since an of the OCA group '" + order12.OcaGroup + "' has been filled");
                                    order12.Cancel();
                                }
                            }
                            continue;
                        }
                    }
                }
                this.CommitTransaction();
            }
        }

        public int Select(iTrading.Core.Data.Quotes quotes)
        {
            lock (SyncRootNow)
            {
                return iTrading.Db.Quotes.Select(quotes);
            }
        }

        public ExecutionCollection Select(iTrading.Core.Kernel.Account account, DateTime minDate, DateTime maxDate)
        {
            lock (SyncRootNow)
            {
                return iTrading.Db.Execution.GetExecutions(account, minDate, maxDate);
            }
        }

        public SymbolCollection Select(string name, ProviderType brokertype, string companyName, DateTime expiry, SymbolType symbolType, Exchange exchange, double strikePrice, RightId rightId, string customText)
        {
            lock (SyncRootNow)
            {
                return iTrading.Db.Symbol.Select(name, brokertype, companyName, expiry, symbolType, exchange, strikePrice, rightId, customText);
            }
        }


        private void Start()
        {
            object[] objArray;
            object obj2;
            SymbolDictionary dictionary;
            if (Globals.TraceSwitch.DataBase)
            {
                Trace.WriteLine("(Db) Db.Adapter.Start");
            }
        Label_0016:
            objArray = null;
            lock (commands)
            {
                if (commands.Count > 0)
                {
                    objArray = (object[]) commands.Dequeue();
                }
                else
                {
                    Monitor.Wait(commands);
                }
            }
            if (objArray != null)
            {
                switch (((Command) objArray[1]))
                {
                    case Command.Disconnect:
                        lock ((obj2 = SyncRootNow))
                        {
                            ((Adapter) objArray[0]).DisconnectNow();
                            goto Label_0016;
                        }
                        goto Label_00AD;

                    case Command.InsertHistory:
                        goto Label_00AD;

                    case Command.UpdateAccount:
                        goto Label_00D2;

                    case Command.UpdateExecution:
                        goto Label_00F7;

                    case Command.UpdateOrder:
                        goto Label_0145;

                    case Command.UpdateQuotes:
                        goto Label_0193;

                    case Command.UpdateSymbol:
                        goto Label_01E1;
                }
            }
            goto Label_0016;
        Label_00AD:
            Monitor.Enter(obj2 = SyncRootNow);
            try
            {
                iTrading.Db.Order.Insert((OrderStatusEventArgs) objArray[2]);
                goto Label_0016;
            }
            finally
            {
                Monitor.Exit(obj2);
            }
        Label_00D2:
            Monitor.Enter(obj2 = SyncRootNow);
            try
            {
                iTrading.Db.Account.Update((iTrading.Core.Kernel.Account) objArray[2]);
                goto Label_0016;
            }
            finally
            {
                Monitor.Exit(obj2);
            }
        Label_00F7:
            Monitor.Enter(dictionary = ((iTrading.Core.Kernel.Execution) objArray[2]).Symbol.Connection.Symbols);
            try
            {
                lock ((obj2 = SyncRootNow))
                {
                    iTrading.Db.Execution.Update((iTrading.Core.Kernel.Execution) objArray[2]);
                }
                goto Label_0016;
            }
            finally
            {
                Monitor.Exit(dictionary);
            }
        Label_0145:
            Monitor.Enter(dictionary = ((iTrading.Core.Kernel.Order) objArray[2]).Symbol.Connection.Symbols);
            try
            {
                lock ((obj2 = SyncRootNow))
                {
                    iTrading.Db.Order.UpdateByToken((iTrading.Core.Kernel.Order) objArray[2]);
                }
                goto Label_0016;
            }
            finally
            {
                Monitor.Exit(dictionary);
            }
        Label_0193:
            Monitor.Enter(dictionary = ((iTrading.Core.Data.Quotes) objArray[2]).Symbol.Connection.Symbols);
            try
            {
                lock ((obj2 = SyncRootNow))
                {
                    iTrading.Db.Quotes.Update((iTrading.Core.Data.Quotes)objArray[2]);
                }
                goto Label_0016;
            }
            finally
            {
                Monitor.Exit(dictionary);
            }
        Label_01E1:
            Monitor.Enter(obj2 = SyncRootNow);
            try
            {
                iTrading.Db.Symbol.Update((iTrading.Core.Kernel.Symbol) objArray[2]);
                goto Label_0016;
            }
            finally
            {
                Monitor.Exit(obj2);
            }
        }

        public void Update(iTrading.Core.Kernel.Account account, bool synchronous)
        {
            if (synchronous)
            {
                lock (SyncRootNow)
                {
                    iTrading.Db.Account.Update(account);
                }
            }
            else
            {
                lock (commands)
                {
                    commands.Enqueue(new object[] { this, Command.UpdateAccount, account });
                    Monitor.Pulse(commands);
                }
            }
        }

        public void Update(iTrading.Core.Kernel.Execution execution, bool synchronous)
        {
            if (synchronous)
            {
                lock (SyncRootNow)
                {
                    iTrading.Db.Execution.Update(execution);
                }
            }
            else
            {
                lock (commands)
                {
                    commands.Enqueue(new object[] { this, Command.UpdateExecution, execution });
                    Monitor.Pulse(commands);
                }
            }
        }

        public void Update(iTrading.Core.Kernel.Order order, bool synchronous)
        {
            if (synchronous)
            {
                lock (SyncRootNow)
                {
                    iTrading.Db.Order.UpdateByToken(order);
                }
            }
            else
            {
                lock (commands)
                {
                    commands.Enqueue(new object[] { this, Command.UpdateOrder, order });
                    Monitor.Pulse(commands);
                }
            }
        }



        public void Update(iTrading.Core.Kernel.Symbol symbol, bool synchronous)
        {
            if (synchronous)
            {
                lock (SyncRootNow)
                {
                    iTrading.Db.Symbol.Update(symbol);
                }
            }
            else
            {
                lock (commands)
                {
                    commands.Enqueue(new object[] { this, Command.UpdateSymbol, symbol });
                    Monitor.Pulse(commands);
                }
            }
        }

        public void Update(iTrading.Core.Data.Quotes quotes, bool synchronous)
        {
            if (synchronous)
            {
                lock (SyncRootNow)
                {
                    iTrading.Db.Quotes.Update(quotes);
                }
            }
            else
            {
                lock (commands)
                {
                    commands.Enqueue(new object[] { this, Command.UpdateQuotes, quotes });
                    Monitor.Pulse(commands);
                }
            }
        }

        public IDbConnection Connection
        {
            get
            {
                return iDbConnection;
            }
        }

        public object SyncRoot
        {
            get
            {
                return SyncRootNow;
            }
        }

        internal static object SyncRootNow
        {
            get
            {
                return typeof(Adapter);
            }
        }

        private enum Command
        {
            Disconnect,
            InsertHistory,
            UpdateAccount,
            UpdateExecution,
            UpdateOrder,
            UpdateQuotes,
            UpdateSymbol
        }
    }
}

