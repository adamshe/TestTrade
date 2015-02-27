namespace iTrading.Db
{
    using System;
    using System.Collections;
    using System.Data;
    using System.Diagnostics;
    using iTrading.Core.Kernel;

    internal class Order
    {
        private static IDbCommand insertOrder = null;
        private static IDbCommand insertOrderHistory = null;
        private static IDbCommand selectOpenOrders = null;
        private static IDbCommand selectOrderById = null;
        private static IDbCommand selectOrderHistory = null;
        private static IDbCommand selectOrders = null;
        private static IDbCommand selectOrderToken = null;
        private static IDbCommand updateOrder = null;

        internal static iTrading.Core.Kernel.Order GetOrderById(iTrading.Core.Kernel.Order order)
        {
            if (Globals.TraceSwitch.DataBase)
            {
                Trace.WriteLine("(Db) Db.Order.GetOrderById '" + order.OrderId + "'");
            }
            if (selectOrderById == null)
            {
                selectOrderById = iTrading.Db.Db.NewCommand("select actiontype, avgfillprice, tm_orders.customtext as customtext, filled, orderid, limitprice, ocagroup, ordertype,quantity, orderstate, stopprice, updatetime, timeinforce, token,tm_exchangesymbols.exchange as exchange, tm_symbols.name as name, tm_orders.expiry as expiry, tm_symbols.strikeprice as strikeprice, tm_symbols.optionright as optionright, tm_symbols.type as type from tm_orders, tm_accounts, tm_exchangesymbols, tm_symbols where orderid = @orderid and tm_orders.symbol = tm_exchangesymbols.id and tm_exchangesymbols.symbol = tm_symbols.id and tm_orders.account = tm_accounts.id and tm_accounts.account = @account and tm_accounts.broker = @broker and tm_accounts.mode = @mode order by updatetime", Adapter.iDbTransaction);
                iTrading.Db.Db.AddParameter(selectOrderById, "@orderid", "VarChar", 50);
                iTrading.Db.Db.AddParameter(selectOrderById, "@account", "VarChar", 50);
                iTrading.Db.Db.AddParameter(selectOrderById, "@broker", "Integer");
                iTrading.Db.Db.AddParameter(selectOrderById, "@mode", "Integer");
                selectOrderById.Prepare();
            }
            selectOrderById.Transaction = Adapter.iDbTransaction;
            iTrading.Db.Db.GetParameter(selectOrderById, "@orderid").Value = order.OrderId;
            iTrading.Db.Db.GetParameter(selectOrderById, "@account").Value = order.Account.Name;
            iTrading.Db.Db.GetParameter(selectOrderById, "@broker").Value = order.Account.Connection.Options.Provider.Id;
            iTrading.Db.Db.GetParameter(selectOrderById, "@mode").Value = order.Account.Connection.Options.Mode.Id;
            OrderCollection orderCollection = GetOrderCollection(order.Account, selectOrderById);
            if (orderCollection.Count != 0)
            {
                return orderCollection[orderCollection.Count - 1];
            }
            return null;
        }

        internal static OrderCollection GetOrderCollection(iTrading.Core.Kernel.Account account, IDbCommand cmd)
        {
            OrderCollection orders = new OrderCollection();
            IDataReader reader = cmd.ExecuteReader();
            ArrayList list = new ArrayList();
            ArrayList list2 = new ArrayList();
            while (reader.Read())
            {
                list2.Add(new SymbolStore((string) reader["name"], (DateTime) reader["expiry"], account.Connection.SymbolTypes[(SymbolTypeId) reader["type"]], account.Connection.Exchanges[(ExchangeId) reader["exchange"]], (double) reader["strikeprice"], (RightId) reader["optionright"]));
                iTrading.Core.Kernel.Order order = new iTrading.Core.Kernel.Order(account, null, account.IsSimulation ? ActionType.All[(ActionTypeId) reader["actiontype"]] : account.Connection.ActionTypes[(ActionTypeId) reader["actiontype"]], account.IsSimulation ? OrderType.All[(OrderTypeId) reader["ordertype"]] : account.Connection.OrderTypes[(OrderTypeId) reader["ordertype"]], account.IsSimulation ? TimeInForce.All[(TimeInForceId) reader["timeinforce"]] : account.Connection.TimeInForces[(TimeInForceId) reader["timeinforce"]], (int) reader["quantity"], (double) reader["limitprice"], (double) reader["stopprice"], (string) reader["token"], account.IsSimulation ? OrderState.All[(OrderStateId) ((int) reader["orderstate"])] : account.Connection.OrderStates[(OrderStateId) ((int) reader["orderstate"])], (string) reader["orderid"], (string) reader["ocagroup"], (int) reader["filled"], (double) reader["avgfillprice"], (DateTime) reader["updatetime"]);
                order.CustomText = (string) reader["customtext"];
                list.Add(order);
            }
            reader.Close();
            for (int i = 0; i < list.Count; i++)
            {
                iTrading.Core.Kernel.Order order2 = (iTrading.Core.Kernel.Order) list[i];
                SymbolStore store = (SymbolStore) list2[i];
                iTrading.Core.Kernel.Symbol symbol = account.Connection.GetSymbol(store.name, store.expiry, store.symbolType, store.exchange, store.strikePrice, store.rightId, LookupPolicyId.RepositoryAndProvider);
                Trace.Assert(symbol != null, string.Concat(new object[] { "Db.Quotes.GetOrderCollection: ", store.name, " ", store.expiry, " ", store.symbolType.Name, " ", store.exchange.Name }));
                iTrading.Core.Kernel.Order order3 = new iTrading.Core.Kernel.Order(order2.Account, symbol, order2.Action, order2.OrderType, order2.TimeInForce, order2.Quantity, order2.LimitPrice, order2.StopPrice, order2.Token, order2.OrderState, order2.OrderId, order2.OcaGroup, order2.Filled, order2.AvgFillPrice, order2.Time);
                order3.CustomText = order2.CustomText;
                orders.Add(order3);
            }
            return orders;
        }

        internal static OrderStatusEventCollection GetOrderHistory(iTrading.Core.Kernel.Order order)
        {
            if (Globals.TraceSwitch.DataBase)
            {
                Trace.WriteLine("(Db) Db.Order.GetOrderHistory '" + order.OrderId + "'");
            }
            if (selectOrderHistory == null)
            {
                selectOrderHistory = iTrading.Db.Db.NewCommand("select tm_orderhistories.avgfillprice as avgfillprice, error, tm_orderhistories.filled as filled,tm_orderhistories.limitprice as limitprice, tm_orderhistories.nativeerror as nativeerror, tm_orderhistories.orderid as orderid,tm_orderhistories.orderstate as orderstate, tm_orderhistories.quantity as quantity, tm_orderhistories.stopprice as stopprice, tm_orderhistories.updatetime as updatetime from tm_orderhistories, tm_orders where tm_orders.token = @token and tm_orderhistories.token = tm_orders.token order by tm_orderhistories.id", Adapter.iDbTransaction);
                iTrading.Db.Db.AddParameter(selectOrderHistory, "@token", "VarChar", 50);
                selectOrderHistory.Prepare();
            }
            selectOrderHistory.Transaction = Adapter.iDbTransaction;
            iTrading.Db.Db.GetParameter(selectOrderHistory, "@token").Value = order.Token;
            OrderStatusEventCollection events = new OrderStatusEventCollection();
            IDataReader reader = selectOrderHistory.ExecuteReader();
            while (reader.Read())
            {
                OrderState orderState = OrderState.All[(OrderStateId) ((int) reader["orderstate"])];
                if (orderState != null)
                {
                    events.Add(new OrderStatusEventArgs(order, (ErrorCode) reader["error"], (string) reader["nativeerror"], (string) reader["orderid"], (double) reader["limitprice"], (double) reader["stopprice"], (int) reader["quantity"], (double) reader["avgfillprice"], (int) reader["filled"], orderState, (DateTime) reader["updatetime"]));
                }
            }
            reader.Close();
            return events;
        }

        internal static OrderCollection GetOrders(iTrading.Core.Kernel.Account account, DateTime minDate, DateTime maxDate)
        {
            if (Globals.TraceSwitch.DataBase)
            {
                Trace.WriteLine(string.Concat(new object[] { "(Db) Db.Order.GetOrders account='", account.Name, "' '", minDate, "' '", maxDate, "'" }));
            }
            if (selectOpenOrders == null)
            {
                selectOpenOrders = iTrading.Db.Db.NewCommand("select actiontype, avgfillprice, tm_orders.customtext as customtext, filled, orderid, limitprice, ocagroup, ordertype,quantity, orderstate, stopprice, updatetime, timeinforce, token,tm_exchangesymbols.exchange as exchange, tm_symbols.name as name, tm_orders.expiry as expiry, tm_symbols.strikeprice as strikeprice, tm_symbols.optionright as optionright, tm_symbols.type as type from tm_orders, tm_accounts, tm_exchangesymbols, tm_symbols where @minDate <= updatetime and updatetime <= @maxDate and tm_orders.symbol = tm_exchangesymbols.id and tm_exchangesymbols.symbol = tm_symbols.id and tm_orders.account = tm_accounts.id and tm_accounts.account = @account and tm_accounts.broker = @broker and tm_accounts.mode = @mode order by updatetime", Adapter.iDbTransaction);
                iTrading.Db.Db.AddParameter(selectOpenOrders, "@minDate", "DateTime");
                iTrading.Db.Db.AddParameter(selectOpenOrders, "@maxDate", "DateTime");
                iTrading.Db.Db.AddParameter(selectOpenOrders, "@account", "VarChar", 50);
                iTrading.Db.Db.AddParameter(selectOpenOrders, "@broker", "Integer");
                iTrading.Db.Db.AddParameter(selectOpenOrders, "@mode", "Integer");
                selectOpenOrders.Prepare();
            }
            selectOpenOrders.Transaction = Adapter.iDbTransaction;
            iTrading.Db.Db.GetParameter(selectOpenOrders, "@minDate").Value = iTrading.Db.Db.ConvertDateTime(minDate);
            iTrading.Db.Db.GetParameter(selectOpenOrders, "@maxDate").Value = iTrading.Db.Db.ConvertDateTime(maxDate);
            iTrading.Db.Db.GetParameter(selectOpenOrders, "@account").Value = account.Name;
            iTrading.Db.Db.GetParameter(selectOpenOrders, "@broker").Value = account.Connection.Options.Provider.Id;
            iTrading.Db.Db.GetParameter(selectOpenOrders, "@mode").Value = account.Connection.Options.Mode.Id;
            return GetOrderCollection(account, selectOpenOrders);
        }

        internal static bool HasOrders(iTrading.Core.Kernel.Symbol symbol)
        {
            if (Globals.TraceSwitch.DataBase)
            {
                Trace.WriteLine("(Db) Db.Order.HasOrders symbol='" + symbol.FullName + "'");
            }
            if (selectOrders == null)
            {
                selectOrders = iTrading.Db.Db.NewCommand("select token from tm_orders, tm_accounts, tm_exchangesymbols, tm_symbols where tm_symbols.name = @name and tm_symbols.type = @type and tm_orders.symbol = tm_exchangesymbols.id and tm_exchangesymbols.symbol = tm_symbols.id order by updatetime", Adapter.iDbTransaction);
                iTrading.Db.Db.AddParameter(selectOrders, "@name", "VarChar", 50);
                iTrading.Db.Db.AddParameter(selectOrders, "@type", "Integer");
                selectOrders.Prepare();
            }
            selectOrders.Transaction = Adapter.iDbTransaction;
            iTrading.Db.Db.GetParameter(selectOrders, "@name").Value = symbol.Name;
            iTrading.Db.Db.GetParameter(selectOrders, "@type").Value = symbol.SymbolType.Id;
            IDataReader reader = selectOrders.ExecuteReader();
            bool flag = reader.Read();
            reader.Close();
            return flag;
        }

        internal static void Init()
        {
            insertOrder = null;
            insertOrderHistory = null;
            selectOpenOrders = null;
            selectOrderHistory = null;
            selectOrderById = null;
            selectOrderToken = null;
            selectOrders = null;
            updateOrder = null;
        }

        internal static void Insert(iTrading.Core.Kernel.Order order)
        {
            if (Globals.TraceSwitch.DataBase)
            {
                Trace.WriteLine("(Db) Db.Order.Insert order='" + order.OrderId + "'");
            }
            if (insertOrder == null)
            {
                insertOrder = iTrading.Db.Db.NewCommand("insert into tm_orders(actiontype, account, avgfillprice, customtext, expiry, filled, orderid, limitprice, ocagroup, ordertype, quantity, orderstate, stopprice, symbol, updatetime, timeinforce, token)values(@actiontype, @account, @avgfillprice, @customtext, @expiry, @filled, @orderid, @limitprice, @ocagroup, @ordertype, @quantity, @orderstate, @stopprice, @symbol, @updatetime, @timeinforce, @token)", Adapter.iDbTransaction);
                iTrading.Db.Db.AddParameter(insertOrder, "@actiontype", "Integer");
                iTrading.Db.Db.AddParameter(insertOrder, "@account", "Integer");
                iTrading.Db.Db.AddParameter(insertOrder, "@avgfillprice", "Double");
                iTrading.Db.Db.AddParameter(insertOrder, "@customtext", "LongVarChar", 0xffff);
                iTrading.Db.Db.AddParameter(insertOrder, "@expiry", "DateTime");
                iTrading.Db.Db.AddParameter(insertOrder, "@filled", "Integer");
                iTrading.Db.Db.AddParameter(insertOrder, "@orderid", "VarChar", 50);
                iTrading.Db.Db.AddParameter(insertOrder, "@limitprice", "Double");
                iTrading.Db.Db.AddParameter(insertOrder, "@ocagroup", "VarChar", 50);
                iTrading.Db.Db.AddParameter(insertOrder, "@ordertype", "Integer");
                iTrading.Db.Db.AddParameter(insertOrder, "@quantity", "Integer");
                iTrading.Db.Db.AddParameter(insertOrder, "@orderstate", "Integer");
                iTrading.Db.Db.AddParameter(insertOrder, "@stopprice", "Double");
                iTrading.Db.Db.AddParameter(insertOrder, "@symbol", "Integer");
                iTrading.Db.Db.AddParameter(insertOrder, "@updatetime", "DateTime");
                iTrading.Db.Db.AddParameter(insertOrder, "@timeinforce", "Integer");
                iTrading.Db.Db.AddParameter(insertOrder, "@token", "VarChar", 50);
            }
            int accountId = iTrading.Db.Account.GetAccountId(order.Account);
            int exchangeSymbolId = iTrading.Db.Symbol.GetExchangeSymbolId(order.Symbol);
            insertOrder.Transaction = Adapter.iDbTransaction;
            iTrading.Db.Db.GetParameter(insertOrder, "@actiontype").Value = order.Action.Id;
            iTrading.Db.Db.GetParameter(insertOrder, "@account").Value = accountId;
            iTrading.Db.Db.GetParameter(insertOrder, "@avgfillprice").Value = order.AvgFillPrice;
            iTrading.Db.Db.GetParameter(insertOrder, "@customtext").Value = order.CustomText;
            iTrading.Db.Db.GetParameter(insertOrder, "@expiry").Value = iTrading.Db.Db.ConvertDateTime(order.Symbol.Expiry);
            iTrading.Db.Db.GetParameter(insertOrder, "@filled").Value = order.Filled;
            iTrading.Db.Db.GetParameter(insertOrder, "@orderid").Value = order.OrderId;
            iTrading.Db.Db.GetParameter(insertOrder, "@limitprice").Value = order.LimitPrice;
            iTrading.Db.Db.GetParameter(insertOrder, "@ocagroup").Value = order.OcaGroup;
            iTrading.Db.Db.GetParameter(insertOrder, "@ordertype").Value = order.OrderType.Id;
            iTrading.Db.Db.GetParameter(insertOrder, "@quantity").Value = order.Quantity;
            iTrading.Db.Db.GetParameter(insertOrder, "@orderstate").Value = order.OrderState.Id;
            iTrading.Db.Db.GetParameter(insertOrder, "@stopprice").Value = order.StopPrice;
            iTrading.Db.Db.GetParameter(insertOrder, "@symbol").Value = exchangeSymbolId;
            iTrading.Db.Db.GetParameter(insertOrder, "@updatetime").Value = iTrading.Db.Db.ConvertDateTime(order.Time);
            iTrading.Db.Db.GetParameter(insertOrder, "@timeinforce").Value = order.TimeInForce.Id;
            iTrading.Db.Db.GetParameter(insertOrder, "@token").Value = order.Token;
            insertOrder.ExecuteNonQuery();
        }

        internal static void Insert(OrderStatusEventArgs e)
        {
            if (Globals.TraceSwitch.DataBase)
            {
                Trace.WriteLine(string.Concat(new object[] { "(Db) Db.Order.Insert order='", e.OrderId, "' ", e.OrderState.Id }));
            }
            Adapter.BeginTransactionNow();
            if (insertOrderHistory == null)
            {
                insertOrderHistory = iTrading.Db.Db.NewCommand("insert into tm_orderhistories(avgfillprice, error, filled, limitprice, nativeerror, orderid, orderstate, quantity, stopprice, token, updatetime)values(@avgfillprice, @error, @filled, @limitprice, @nativeerror, @orderid, @orderstate, @quantity, @stopprice, @token, @updatetime)", Adapter.iDbTransaction);
                iTrading.Db.Db.AddParameter(insertOrderHistory, "@avgfillprice", "Double");
                iTrading.Db.Db.AddParameter(insertOrderHistory, "@error", "Integer");
                iTrading.Db.Db.AddParameter(insertOrderHistory, "@filled", "Integer");
                iTrading.Db.Db.AddParameter(insertOrderHistory, "@limitprice", "Double");
                iTrading.Db.Db.AddParameter(insertOrderHistory, "@nativeerror", "VarChar", 200);
                iTrading.Db.Db.AddParameter(insertOrderHistory, "@orderid", "VarChar", 50);
                iTrading.Db.Db.AddParameter(insertOrderHistory, "@orderstate", "Integer");
                iTrading.Db.Db.AddParameter(insertOrderHistory, "@quantity", "Integer");
                iTrading.Db.Db.AddParameter(insertOrderHistory, "@stopprice", "Double");
                iTrading.Db.Db.AddParameter(insertOrderHistory, "@token", "VarChar", 50);
                iTrading.Db.Db.AddParameter(insertOrderHistory, "@updatetime", "DateTime");
            }
            insertOrderHistory.Transaction = Adapter.iDbTransaction;
            iTrading.Db.Db.GetParameter(insertOrderHistory, "@avgfillprice").Value = e.AvgFillPrice;
            iTrading.Db.Db.GetParameter(insertOrderHistory, "@error").Value = e.Error;
            iTrading.Db.Db.GetParameter(insertOrderHistory, "@filled").Value = e.Filled;
            iTrading.Db.Db.GetParameter(insertOrderHistory, "@limitprice").Value = e.LimitPrice;
            iTrading.Db.Db.GetParameter(insertOrderHistory, "@nativeerror").Value = e.NativeError.Substring(0, Math.Min(e.NativeError.Length, 0xc7));
            iTrading.Db.Db.GetParameter(insertOrderHistory, "@orderid").Value = e.OrderId;
            iTrading.Db.Db.GetParameter(insertOrderHistory, "@orderstate").Value = e.OrderState.Id;
            iTrading.Db.Db.GetParameter(insertOrderHistory, "@quantity").Value = e.Quantity;
            iTrading.Db.Db.GetParameter(insertOrderHistory, "@stopprice").Value = e.StopPrice;
            iTrading.Db.Db.GetParameter(insertOrderHistory, "@token").Value = e.Order.Token;
            iTrading.Db.Db.GetParameter(insertOrderHistory, "@updatetime").Value = iTrading.Db.Db.ConvertDateTime(e.Time);
            insertOrderHistory.ExecuteNonQuery();
            UpdateByToken1(e.Order);
            Adapter.CommitTransactionNow();
        }

        internal static void UpdateByToken(iTrading.Core.Kernel.Order order)
        {
            if (Globals.TraceSwitch.DataBase)
            {
                Trace.WriteLine("(Db) Db.Order.UpdateByTokenNow order='" + order.OrderId + "'");
            }
            Adapter.BeginTransactionNow();
            if (selectOrderToken == null)
            {
                selectOrderToken = iTrading.Db.Db.NewCommand("select token from tm_orders where token = @token", Adapter.iDbTransaction);
                iTrading.Db.Db.AddParameter(selectOrderToken, "@token", "VarChar", 50);
                selectOrderToken.Prepare();
            }
            selectOrderToken.Transaction = Adapter.iDbTransaction;
            iTrading.Db.Db.GetParameter(selectOrderToken, "@token").Value = order.Token;
            IDataReader reader = selectOrderToken.ExecuteReader();
            if (reader.Read())
            {
                reader.Close();
            }
            else
            {
                reader.Close();
                Insert(order);
            }
            UpdateByToken1(order);
            Adapter.CommitTransactionNow();
        }

        internal static void UpdateByToken1(iTrading.Core.Kernel.Order order)
        {
            if (updateOrder == null)
            {
                updateOrder = iTrading.Db.Db.NewCommand("update tm_orders set avgfillprice = @avgfillprice, customtext = @customtext, expiry = @expiry, filled = @filled, orderid = @orderid,limitprice = @limitprice, ocagroup = @ocagroup, quantity = @quantity, orderstate = @orderstate, stopprice = @stopprice, updatetime = @updatetime where token = @token", Adapter.iDbTransaction);
                iTrading.Db.Db.AddParameter(updateOrder, "@avgfillprice", "Double");
                iTrading.Db.Db.AddParameter(updateOrder, "@customtext", "LongVarChar", 0xffff);
                iTrading.Db.Db.AddParameter(updateOrder, "@expiry", "DateTime");
                iTrading.Db.Db.AddParameter(updateOrder, "@filled", "Integer");
                iTrading.Db.Db.AddParameter(updateOrder, "@orderid", "VarChar", 50);
                iTrading.Db.Db.AddParameter(updateOrder, "@limitprice", "Double");
                iTrading.Db.Db.AddParameter(updateOrder, "@ocagroup", "VarChar", 50);
                iTrading.Db.Db.AddParameter(updateOrder, "@quantity", "Integer");
                iTrading.Db.Db.AddParameter(updateOrder, "@orderstate", "Integer");
                iTrading.Db.Db.AddParameter(updateOrder, "@stopprice", "Double");
                iTrading.Db.Db.AddParameter(updateOrder, "@updatetime", "DateTime");
                iTrading.Db.Db.AddParameter(updateOrder, "@token", "VarChar", 50);
            }
            updateOrder.Transaction = Adapter.iDbTransaction;
            iTrading.Db.Db.GetParameter(updateOrder, "@avgfillprice").Value = order.AvgFillPrice;
            iTrading.Db.Db.GetParameter(updateOrder, "@customtext").Value = order.CustomText;
            iTrading.Db.Db.GetParameter(updateOrder, "@expiry").Value = iTrading.Db.Db.ConvertDateTime(order.Symbol.Expiry);
            iTrading.Db.Db.GetParameter(updateOrder, "@filled").Value = order.Filled;
            iTrading.Db.Db.GetParameter(updateOrder, "@orderid").Value = order.OrderId;
            iTrading.Db.Db.GetParameter(updateOrder, "@limitprice").Value = order.LimitPrice;
            iTrading.Db.Db.GetParameter(updateOrder, "@ocagroup").Value = order.OcaGroup;
            iTrading.Db.Db.GetParameter(updateOrder, "@quantity").Value = order.Quantity;
            iTrading.Db.Db.GetParameter(updateOrder, "@orderstate").Value = order.OrderState.Id;
            iTrading.Db.Db.GetParameter(updateOrder, "@stopprice").Value = order.StopPrice;
            iTrading.Db.Db.GetParameter(updateOrder, "@updatetime").Value = iTrading.Db.Db.ConvertDateTime(order.Time);
            iTrading.Db.Db.GetParameter(updateOrder, "@token").Value = order.Token;
            updateOrder.ExecuteNonQuery();
        }
    }
}

