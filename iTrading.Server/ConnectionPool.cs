using iTrading.Core.Kernel;

namespace TradeMagic.Server
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;
    using iTrading.Core.Kernel;

    internal class ConnectionPool
    {
        private bool busy = false;
        private Hashtable client2Connection = new Hashtable();
        private TradeMagic.Server.Server server;

        internal ConnectionPool(TradeMagic.Server.Server server)
        {
            this.server = server;
        }

        private void ConnectionStatus(object sender, ConnectionStatusEventArgs e)
        {
            if (Globals.TraceSwitch.Connect)
            {
                Trace.WriteLine(string.Concat(new object[] { "(", e.Connection.IdPlus, ") Server.ConnectionPool.ConnectionStatus ", e.ConnectionStatusId, " ", e.Error }));
            }
            this.busy = false;
        }

        internal void Register(Client client, OptionsBase options)
        {
            if (Globals.TraceSwitch.Connect)
            {
                Trace.WriteLine("(" + client.id + ") Server.ConnectionPool.Register1");
            }
            lock (typeof(ConnectionPool))
            {
                while (this.busy)
                {
                    Application.DoEvents();
                    Thread.Sleep(500);
                }
                this.busy = true;
            }
            Client client2 = null;
            foreach (Client client3 in this.client2Connection.Keys)
            {
                if (((Connection) this.client2Connection[client3]).Options.SameConnection(options))
                {
                    client.connection = (Connection) this.client2Connection[client3];
                    client2 = client3;
                }
            }
            bool flag = true;
            if (client.connection == null)
            {
                foreach (Client client4 in this.client2Connection.Keys)
                {
                    if ((((Connection) this.client2Connection[client4]).Options.Provider.Id == options.Provider.Id) && (((Connection) this.client2Connection[client4]).FeatureTypes[FeatureTypeId.MultipleConnections] == null))
                    {
                        client.connection = (Connection) this.client2Connection[client4];
                        client2 = client4;
                        flag = false;
                    }
                }
            }
            bool flag2 = false;
            if (client.connection == null)
            {
                if (Globals.TraceSwitch.Connect)
                {
                    Trace.WriteLine("(" + client.id + ") Server.ConnectionPool.Register2 new connection");
                }
                client.connection = new Connection();
                client.connection.ConnectionStatus += new ConnectionStatusEventHandler(this.ConnectionStatus);
                client.connection.CustomLink = 1;
                client.connection.SynchronizeInvoke = client.server.synchronizeInvoke;
                this.client2Connection.Add(client, client.connection);
                flag2 = true;
            }
            else
            {
                if (flag)
                {
                    AccountItemTypeDictionary dictionary2;
                    AccountCollection accounts;
                    if (Globals.TraceSwitch.Connect)
                    {
                        Trace.WriteLine(string.Concat(new object[] { "(", client.id, ") Server.ConnectionPool.Register2 old connection ", client.connection.IdPlus }));
                    }
                    client.connection.CustomLink = ((int) client.connection.CustomLink) + 1;
                    this.client2Connection.Add(client, client.connection);
                    ((ServerBytes) client.socket.Bytes).serverId2Request = (Hashtable) ((ServerBytes) client2.socket.Bytes).serverId2Request.Clone();
                    ((ServerBytes) client.socket.Bytes).RegisterClientRequest(client.clientRequestId, client.connection);
                    lock (client.connection.Currencies)
                    {
                        foreach (Currency currency in client.connection.Currencies.Values)
                        {
                            client.Currency(this, new CurrencyEventArgs(client.connection, ErrorCode.NoError, "", currency.Id, currency.MapId));
                        }
                    }
                    lock ((dictionary2 = client.connection.AccountItemTypes))
                    {
                        foreach (AccountItemType type in client.connection.AccountItemTypes.Values)
                        {
                            client.AccountItemType(this, new AccountItemTypeEventArgs(client.connection, ErrorCode.NoError, "", type.Id, type.MapId));
                        }
                    }
                    lock (client.connection.ActionTypes)
                    {
                        foreach (ActionType type2 in client.connection.ActionTypes.Values)
                        {
                            client.ActionType(this, new ActionTypeEventArgs(client.connection, ErrorCode.NoError, "", type2.Id, type2.MapId));
                        }
                    }
                    lock (client.connection.Exchanges)
                    {
                        foreach (Exchange exchange in client.connection.Exchanges.Values)
                        {
                            client.Exchange(this, new ExchangeEventArgs(client.connection, ErrorCode.NoError, "", exchange.Id, exchange.MapId));
                        }
                    }
                    lock (client.connection.FeatureTypes)
                    {
                        foreach (FeatureType type3 in client.connection.FeatureTypes.Values)
                        {
                            client.FeatureType(this, new FeatureTypeEventArgs(client.connection, ErrorCode.NoError, "", type3.Id, type3.Value));
                        }
                    }
                    lock (client.connection.MarketDataTypes)
                    {
                        foreach (MarketDataType type4 in client.connection.MarketDataTypes.Values)
                        {
                            client.MarketDataType(this, new MarketDataTypeEventArgs(client.connection, ErrorCode.NoError, "", type4.Id, type4.MapId));
                        }
                    }
                    lock (client.connection.MarketPositions)
                    {
                        foreach (MarketPosition position in client.connection.MarketPositions.Values)
                        {
                            client.MarketPosition(this, new MarketPositionEventArgs(client.connection, ErrorCode.NoError, "", position.Id, position.MapId));
                        }
                    }
                    lock (client.connection.NewsItemTypes)
                    {
                        foreach (NewsItemType type5 in client.connection.NewsItemTypes.Values)
                        {
                            client.NewsItemType(this, new NewsItemTypeEventArgs(client.connection, ErrorCode.NoError, "", type5.Id, type5.MapId));
                        }
                    }
                    lock (client.connection.OrderStates)
                    {
                        foreach (OrderState state in client.connection.OrderStates.Values)
                        {
                            client.OrderState(this, new OrderStateEventArgs(client.connection, ErrorCode.NoError, "", state.Id, state.MapId));
                        }
                    }
                    lock (client.connection.OrderTypes)
                    {
                        foreach (OrderType type6 in client.connection.OrderTypes.Values)
                        {
                            client.OrderType(this, new OrderTypeEventArgs(client.connection, ErrorCode.NoError, "", type6.Id, type6.MapId));
                        }
                    }
                    lock (client.connection.SymbolTypes)
                    {
                        foreach (SymbolType type7 in client.connection.SymbolTypes.Values)
                        {
                            client.SymbolType(this, new SymbolTypeEventArgs(client.connection, ErrorCode.NoError, "", type7.Id, type7.MapId));
                        }
                    }
                    lock (client.connection.TimeInForces)
                    {
                        foreach (TimeInForce force in client.connection.TimeInForces.Values)
                        {
                            client.TimeInForce(this, new TimeInForceEventArgs(client.connection, ErrorCode.NoError, "", force.Id, force.MapId));
                        }
                    }
                    lock ((accounts = client.connection.Accounts))
                    {
                        foreach (Account account in client.connection.Accounts)
                        {
                            client.Account(this, new AccountEventArgs(client.connection, ErrorCode.NoError, "", account.Name, account.SimulationAccountOptions));
                        }
                    }
                    client.ConnectionStatus(this, new ConnectionStatusEventArgs(client.connection, ErrorCode.NoError, "", ConnectionStatusId.Connected, ConnectionStatusId.Connected, client.id, client.connection.CustomText));
                    lock (client.connection.News)
                    {
                        foreach (NewsEventArgs args in client.connection.News)
                        {
                            client.News(this, new NewsEventArgs(client.connection, ErrorCode.NoError, "", args.Id, args.ItemType, args.Time, args.HeadLine, args.Text));
                        }
                    }
                    lock (client.connection.Symbols)
                    {
                        foreach (Symbol symbol in client.connection.Symbols.Values)
                        {
                            client.Symbol(this, new SymbolEventArgs(client.connection, ErrorCode.NoError, "", symbol));
                        }
                    }
                    Monitor.Enter(client.syncOrderEvents);
                    lock ((accounts = client.connection.Accounts))
                    {
                        foreach (Account account2 in client.connection.Accounts)
                        {
                            lock ((dictionary2 = client.connection.AccountItemTypes))
                            {
                                foreach (Currency currency2 in client.connection.Currencies.Values)
                                {
                                    foreach (AccountItemType type8 in client.connection.AccountItemTypes.Values)
                                    {
                                        if (type8.Id != AccountItemTypeId.Unknown)
                                        {
                                            client.AccountUpdate(this, new AccountUpdateEventArgs(client.connection, ErrorCode.NoError, "", account2, type8, account2.GetItem(type8, currency2).Currency, account2.GetItem(type8, currency2).Value, account2.LastUpdate));
                                        }
                                    }
                                }
                            }
                            lock (account2.Executions)
                            {
                                foreach (Execution execution in account2.Executions)
                                {
                                    client.Execution(this, new ExecutionUpdateEventArgs(client.connection, ErrorCode.NoError, "", Operation.Insert, execution.Id, execution.Account, execution.Symbol, execution.Time, execution.MarketPosition, execution.OrderId, execution.Quantity, execution.AvgPrice));
                                }
                            }
                            lock (account2.Positions)
                            {
                                foreach (Position position2 in account2.Positions)
                                {
                                    client.PositionUpdate(this, new PositionUpdateEventArgs(client.connection, ErrorCode.NoError, "", Operation.Insert, account2, position2.Symbol, position2.MarketPosition, position2.Quantity, position2.Currency, position2.AvgPrice));
                                }
                            }
                            lock (account2.Orders)
                            {
                                foreach (Order order in account2.Orders)
                                {
                                    client.OrderStatus(this, new OrderStatusEventArgs(order, ErrorCode.NoError, "", order.OrderId, order.LimitPrice, order.StopPrice, order.Quantity, order.AvgFillPrice, order.Filled, client.connection.OrderStates[OrderStateId.Initialized], order.Time));
                                }
                                continue;
                            }
                        }
                        goto Label_0F28;
                    }
                }
                client.ConnectionStatus(this, new ConnectionStatusEventArgs(client.connection, ErrorCode.MultipleConnectionsNotSupported, "There already is a connection (using different connect options) to this broker", ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, client.id, ""));
                client.connection = null;
                return;
            }
        Label_0F28:
            ((ServerBytes) client.socket.Bytes).RegisterClientRequest(client.clientRequestId, client.connection);
            client.socket.Bytes.Connection = client.connection;
            client.connection.Accounts.Account += new AccountEventHandler(client.Account);
            client.connection.AccountItemTypes.AccountItemType += new AccountItemTypeEventHandler(client.AccountItemType);
            client.connection.ActionTypes.ActionType += new ActionTypeEventHandler(client.ActionType);
            client.connection.ConnectionStatus += new ConnectionStatusEventHandler(client.ConnectionStatus);
            client.connection.Currencies.Currency += new CurrencyEventHandler(client.Currency);
            client.connection.Error += new ErrorArgsEventHandler(client.Error);
            client.connection.Exchanges.Exchange += new ExchangeEventHandler(client.Exchange);
            client.connection.FeatureTypes.FeatureType += new FeatureTypeEventHandler(client.FeatureType);
            client.connection.MarketDataTypes.MarketDataType += new MarketDataTypeEventHandler(client.MarketDataType);
            client.connection.MarketPositions.MarketPosition += new MarketPositionEventHandler(client.MarketPosition);
            client.connection.News.News += new NewsEventHandler(client.News);
            client.connection.NewsItemTypes.NewsItemType += new NewsItemTypeEventHandler(client.NewsItemType);
            client.connection.OrderStates.OrderState += new OrderStateEventHandler(client.OrderState);
            client.connection.OrderTypes.OrderType += new OrderTypeEventHandler(client.OrderType);
            client.connection.Symbol += new SymbolEventHandler(client.Symbol);
            client.connection.SymbolTypes.SymbolType += new SymbolTypeEventHandler(client.SymbolType);
            client.connection.TimeInForces.TimeInForce += new TimeInForceEventHandler(client.TimeInForce);
            foreach (Account account3 in client.connection.Accounts)
            {
                account3.AccountUpdate += new AccountUpdateEventHandler(client.AccountUpdate);
                account3.Execution += new ExecutionUpdateEventHandler(client.Execution);
                account3.OrderStatus += new OrderStatusEventHandler(client.OrderStatus);
                account3.PositionUpdate += new PositionUpdateEventHandler(client.PositionUpdate);
            }
            if (flag2)
            {
                client.connection.Connect(options);
            }
            else
            {
                Monitor.Exit(client.syncOrderEvents);
                this.busy = false;
            }
            if (Globals.TraceSwitch.Connect)
            {
                Trace.WriteLine("(" + client.id + ") Server.ConnectionPool.Register9");
            }
        }

        internal void Unregister(Client client)
        {
            if (Globals.TraceSwitch.Connect)
            {
                Trace.WriteLine("(" + client.id + ") Server.ConnectionPool.Unregister1");
            }
            Connection currentConnection = (Connection) this.client2Connection[client];
            if ((currentConnection != null) && this.client2Connection.Contains(client))
            {
                lock (typeof(ConnectionPool))
                {
                    while (this.busy)
                    {
                        Application.DoEvents();
                        Thread.Sleep(500);
                    }
                    this.busy = true;
                }
                this.client2Connection.Remove(client);
                currentConnection.CustomLink = ((int) currentConnection.CustomLink) - 1;
                if (((int) currentConnection.CustomLink) == 0)
                {
                    if (Globals.TraceSwitch.Connect)
                    {
                        Trace.WriteLine(string.Concat(new object[] { "(", client.id, ") Server.ConnectionPool.Unregister2 close connection ", currentConnection.IdPlus }));
                    }
                    currentConnection.Close();
                }
                client.ConnectionStatus(this, new ConnectionStatusEventArgs(currentConnection, ErrorCode.NoError, "", ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, client.id, ""));
                this.busy = false;
                if (Globals.TraceSwitch.Connect)
                {
                    Trace.WriteLine("(" + client.id + ") Server.ConnectionPool.Unregister9");
                }
            }
        }

        private delegate void LockHandler(bool setLock);
    }
}

