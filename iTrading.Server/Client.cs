using iTrading.Client;
using iTrading.Core.Kernel;

namespace TradeMagic.Server
{
    using System;
    using System.Diagnostics;
    using System.Net.Sockets;
    using System.Threading;

    internal class Client
    {
        internal int clientRequestId = -1;
        private int clientVersion = 1;
        internal Connection connection = null;
        internal int id = -1;
        private string listBoxItem = "";
        private static int nextClientId = 1;
        internal TradeMagic.Server.Server server;
        private int serverVersion = 1;
        internal ITradingSocket socket;
        internal object[] syncOrderEvents = new object[0];
        private bool terminated = false;
        private Thread thread;

        internal Client(TradeMagic.Server.Server server, Socket socket)
        {
            lock (typeof(Client))
            {
                this.id = nextClientId++;
            }
            Trace.WriteLine("(" + this.id + ") Server.Client ctor");
            this.server = server;
            this.socket = new ITradingSocket(socket, new ServerBytes());
            this.thread = new Thread(new ThreadStart(this.Handler));
            this.thread.Name = "Client" + this.id;
            this.thread.Start();
        }

        internal void Account(object sender, AccountEventArgs e)
        {
            e.Account.Execution += new ExecutionUpdateEventHandler(this.Execution);
            e.Account.OrderStatus += new OrderStatusEventHandler(this.OrderStatus);
            e.Account.PositionUpdate += new PositionUpdateEventHandler(this.PositionUpdate);
            this.Send(e);
        }

        internal void AccountItemType(object sender, AccountItemTypeEventArgs e)
        {
            this.Send(e);
        }

        internal void AccountUpdate(object sender, AccountUpdateEventArgs e)
        {
            this.Send(e);
        }

        internal void ActionType(object sender, ActionTypeEventArgs e)
        {
            this.Send(e);
        }

        internal void Close()
        {
            if (this.connection == null)
            {
                this.server.Error(ErrorCode.Panic, "", "(" + this.id + ") Client.Close: connection is null");
            }
            else if (!this.terminated)
            {
                this.terminated = true;
                this.connection.Accounts.Account -= new AccountEventHandler(this.Account);
                this.connection.AccountItemTypes.AccountItemType -= new AccountItemTypeEventHandler(this.AccountItemType);
                this.connection.ActionTypes.ActionType -= new ActionTypeEventHandler(this.ActionType);
                this.connection.ConnectionStatus -= new ConnectionStatusEventHandler(this.ConnectionStatus);
                this.connection.Currencies.Currency -= new CurrencyEventHandler(this.Currency);
                this.connection.Error -= new ErrorArgsEventHandler(this.Error);
                this.connection.Exchanges.Exchange -= new ExchangeEventHandler(this.Exchange);
                this.connection.FeatureTypes.FeatureType -= new FeatureTypeEventHandler(this.FeatureType);
                this.connection.MarketDataTypes.MarketDataType -= new MarketDataTypeEventHandler(this.MarketDataType);
                this.connection.MarketPositions.MarketPosition -= new MarketPositionEventHandler(this.MarketPosition);
                this.connection.News.News -= new NewsEventHandler(this.News);
                this.connection.NewsItemTypes.NewsItemType -= new NewsItemTypeEventHandler(this.NewsItemType);
                this.connection.OrderStates.OrderState -= new OrderStateEventHandler(this.OrderState);
                this.connection.OrderTypes.OrderType -= new OrderTypeEventHandler(this.OrderType);
                this.connection.Symbol -= new SymbolEventHandler(this.Symbol);
                this.connection.SymbolTypes.SymbolType -= new SymbolTypeEventHandler(this.SymbolType);
                this.connection.TimeInForces.TimeInForce -= new TimeInForceEventHandler(this.TimeInForce);
                foreach (Account account in this.connection.Accounts)
                {
                    account.AccountUpdate -= new AccountUpdateEventHandler(this.AccountUpdate);
                    account.Execution -= new ExecutionUpdateEventHandler(this.Execution);
                    account.OrderStatus -= new OrderStatusEventHandler(this.OrderStatus);
                    account.PositionUpdate -= new PositionUpdateEventHandler(this.PositionUpdate);
                }
                this.server.connectionPool.Unregister(this);
                this.server.mainForm.HandleClient(this, false);
                lock (this.server.clients)
                {
                    if (this.server.clients.Contains(this))
                    {
                        this.server.clients.Remove(this);
                    }
                }
                this.socket.Close();
                Trace.WriteLine("(" + this.connection.IdPlus + ") Terminated");
            }
        }

        internal void ConnectionStatus(object sender, ConnectionStatusEventArgs e)
        {
            if (Globals.TraceSwitch.Connect)
            {
                Trace.WriteLine(string.Concat(new object[] { "(", (this.connection == null) ? this.id.ToString() : this.connection.IdPlus, ") Server.Client.ConnectionStatus ", e.ConnectionStatusId, " ", e.Error }));
            }
            this.Send(new ConnectionStatusEventArgs(e.Connection, e.Error, e.NativeError, e.ConnectionStatusId, e.SecondaryConnectionStatusId, this.id, e.CustomText));
            if (e.ConnectionStatusId != ConnectionStatusId.Connected)
            {
                this.Close();
            }
        }

        internal void Currency(object sender, CurrencyEventArgs e)
        {
            this.Send(e);
        }

        internal void Error(object sender, ITradingErrorEventArgs e)
        {
            this.server.Error(e.Error, e.NativeError, "(" + ((this.connection == null) ? this.id.ToString() : this.connection.IdPlus) + ") " + e.Message);
            this.Send(e);
        }

        internal void Exchange(object sender, ExchangeEventArgs e)
        {
            this.Send(e);
        }

        internal void Execution(object sender, ExecutionUpdateEventArgs e)
        {
            lock (this.syncOrderEvents)
            {
                this.Send(e);
            }
        }

        internal void FeatureType(object sender, FeatureTypeEventArgs e)
        {
            if (e.FeatureType.Id != FeatureTypeId.SynchronousSymbolLookup)
            {
                this.Send(e);
            }
        }

        private void Handler()
        {
            ITradingSerializable serializable;
            this.server.allDone.Set();
            Trace.WriteLine("(" + this.id + ") Server.Client.Handler");
            try
            {
                this.clientVersion = this.socket.Receive().ReadInt32();
                this.socket.Bytes.Write(this.serverVersion);
                this.socket.Send();
            }
            catch (Exception exception)
            {
                this.server.Error(ErrorCode.Panic, "", string.Concat(new object[] { "(", this.id, ") Server.Client.Handler: ", exception.Message }));
                this.Close();
                return;
            }
            Trace.WriteLine("(" + this.id + ") Server.Client.Handler: starting message loop");
        Label_00E0:
            serializable = null;
            try
            {
                Order order;
                serializable = this.socket.Receive().ReadSerializable();
                if (Globals.TraceSwitch.Native)
                {
                    Trace.WriteLine("(" + ((this.connection == null) ? this.id.ToString() : this.connection.IdPlus) + ") Server.Client.Handler: received " + serializable.ClassId.ToString("g"));
                }
                switch (serializable.ClassId)
                {
                    case ClassId.MarketDepth:
                    {
                        if (this.connection == null)
                        {
                            this.server.Error(ErrorCode.Panic, "", "(" + this.id + ") Server.Client.Handler3: connection is null");
                            return;
                        }
                        MarketDepth depth = (MarketDepth) serializable;
                        Symbol symbol2 = this.connection.GetSymbol(depth.Symbol.Name, depth.Symbol.Expiry, depth.Symbol.SymbolType, depth.Symbol.Exchange, depth.Symbol.StrikePrice, depth.Symbol.Right.Id, LookupPolicyId.RepositoryAndProvider);
                        if (symbol2 == null)
                        {
                            this.server.Error(ErrorCode.Panic, "", "(" + this.connection.IdPlus + ") Server.Client.Handler3: symbol is null");
                            return;
                        }
                        ((ServerBytes) this.socket.Bytes).RegisterClientRequest(depth.Id, symbol2.MarketDepth);
                        if (depth.IsRunning)
                        {
                            symbol2.MarketDepth.MarketDepthItem -= new MarketDepthItemEventHandler(this.MarketDepthItem);
                        }
                        else
                        {
                            symbol2.MarketDepth.MarketDepthItem += new MarketDepthItemEventHandler(this.MarketDepthItem);
                        }
                        goto Label_00E0;
                    }
                    case ClassId.Order:
                    {
                        if (this.connection == null)
                        {
                            this.server.Error(ErrorCode.Panic, "", "(" + this.id + ") Server.Client.Handler4: connection is null");
                            return;
                        }
                        order = (Order) serializable;
                        Symbol symbol3 = this.connection.GetSymbol(order.Symbol.Name, order.Symbol.Expiry, order.Symbol.SymbolType, order.Symbol.Exchange, order.Symbol.StrikePrice, order.Symbol.Right.Id, LookupPolicyId.RepositoryAndProvider);
                        if (symbol3 == null)
                        {
                            this.server.Error(ErrorCode.Panic, "", "(" + this.connection.IdPlus + ") Server.Client.Handler4: symbol is null");
                            return;
                        }
                        if (order.Operation != Operation.Insert)
                        {
                            break;
                        }
                        Order serverRequest = null;
                        lock (this.syncOrderEvents)
                        {
                            serverRequest = order.Account.CreateOrder(symbol3, order.Action.Id, order.OrderType.Id, order.TimeInForce.Id, order.Quantity, order.LimitPrice, order.StopPrice, order.Token, order.OrderState, order.OrderId, order.OcaGroup, null, null);
                            if (serverRequest == null)
                            {
                                this.server.Error(ErrorCode.Panic, "", "(" + this.connection.IdPlus + ") Server.Client.Handler4: order is null");
                                return;
                            }
                            ((ServerBytes) this.socket.Bytes).RegisterClientRequest(order.Id, serverRequest);
                            ((ServerBytes) this.socket.Bytes).RegisterServerRequest(serverRequest.Id, serverRequest);
                        }
                        serverRequest.Submit();
                        goto Label_00E0;
                    }
                    case ClassId.Symbol:
                    {
                        if (this.connection == null)
                        {
                            this.server.Error(ErrorCode.Panic, "", "(" + this.id + ") Client.Symbol1: connection is null");
                            return;
                        }
                        Symbol symbol4 = (Symbol) serializable;
                        Symbol symbol5 = this.connection.GetSymbol(symbol4.Name, symbol4.Expiry, symbol4.SymbolType, symbol4.Exchange, symbol4.StrikePrice, symbol4.Right.Id, LookupPolicyId.CacheOnly);
                        if (symbol5 != null)
                        {
                            this.connection.ProcessEventArgs(new SymbolEventArgs(this.connection, ErrorCode.NoError, "", symbol5));
                        }
                        else if (this.connection.GetSymbol(symbol4.Name, symbol4.Expiry, symbol4.SymbolType, symbol4.Exchange, symbol4.StrikePrice, symbol4.Right.Id, LookupPolicyId.RepositoryAndProvider) == null)
                        {
                            if (Globals.TraceSwitch.SymbolLookup)
                            {
                                Trace.WriteLine("(" + this.connection.IdPlus + ") Client.Symbol2: null");
                            }
                            this.Send(new SymbolEventArgs(this.connection, ErrorCode.NoSuchSymbol, "Symbol '" + symbol4.FullName + "' not supported by this provider", null));
                        }
                        goto Label_00E0;
                    }
                    case ClassId.Connection:
                    {
                        Connection connection = (Connection) serializable;
                        this.clientRequestId = connection.Id;
                        if (connection.Operation == Operation.Insert)
                        {
                            if (Globals.TraceSwitch.Connect)
                            {
                                Trace.WriteLine("(" + ((this.connection == null) ? this.id.ToString() : this.connection.IdPlus) + ") Server.Client.Handler.Connection: Connect " + ((this.connection == null) ? "null" : this.connection.ConnectionStatusId.ToString()));
                            }
                            OptionsBase options = connection.Options;
                            options.RunAtServer = false;
                            this.server.connectionPool.Register(this, options);
                            this.server.mainForm.HandleClient(this, true);
                            goto Label_00E0;
                        }
                        if (connection.Operation != Operation.Delete)
                        {
                            goto Label_00E0;
                        }
                        if (Globals.TraceSwitch.Connect)
                        {
                            Trace.WriteLine("(" + ((this.connection == null) ? this.id.ToString() : this.connection.IdPlus) + ") Server.Client.Handler.Connection: Close " + ((this.connection == null) ? "null" : this.connection.ConnectionStatusId.ToString()));
                        }
                        if (this.connection == null)
                        {
                            this.server.Error(ErrorCode.Panic, "", "(" + this.id + ") Server.Client.Handler1: connection is null");
                        }
                        else
                        {
                            this.Close();
                        }
                        return;
                    }
                    case ClassId.MarketData:
                    {
                        if (this.connection == null)
                        {
                            this.server.Error(ErrorCode.Panic, "", "(" + this.id + ") Server.Client.Handler2: connection is null");
                            return;
                        }
                        MarketData data = (MarketData) serializable;
                        Symbol symbol = this.connection.GetSymbol(data.Symbol.Name, data.Symbol.Expiry, data.Symbol.SymbolType, data.Symbol.Exchange, data.Symbol.StrikePrice, data.Symbol.Right.Id, LookupPolicyId.RepositoryAndProvider);
                        if (symbol == null)
                        {
                            this.server.Error(ErrorCode.Panic, "", "(" + this.connection.IdPlus + ") ClientHandler2: symbol is null");
                            return;
                        }
                        ((ServerBytes) this.socket.Bytes).RegisterClientRequest(data.Id, symbol.MarketData);
                        if (data.IsRunning)
                        {
                            symbol.MarketData.MarketDataItem -= new MarketDataItemEventHandler(this.MarketDataItem);
                        }
                        else
                        {
                            symbol.MarketData.MarketDataItem += new MarketDataItemEventHandler(this.MarketDataItem);
                        }
                        goto Label_00E0;
                    }
                    default:
                        goto Label_094A;
                }
                Order order3 = order.Account.Orders.FindByOrderId(order.OrderId);
                if (order3 == null)
                {
                    this.server.Error(ErrorCode.Panic, "", "(" + this.connection.IdPlus + ") Server.Client.Handler4: order is null");
                    return;
                }
                if (order.Operation == Operation.Delete)
                {
                    order3.Cancel();
                }
                else if (order.Operation == Operation.Update)
                {
                    order3.LimitPrice = order.LimitPrice;
                    order3.Quantity = order.Quantity;
                    order3.StopPrice = order.StopPrice;
                    order3.Change();
                }
                goto Label_00E0;
            Label_094A:
                if (this.connection == null)
                {
                    this.server.Error(ErrorCode.Panic, "", "(" + this.id + ") Server.Client.Handler6: connection is null");
                }
                else
                {
                    this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.Panic, "", "Server.Client.Handler: unknown serializable '" + ((int) serializable.ClassId) + "'"));
                    goto Label_00E0;
                }
            }
            catch (ThreadAbortException)
            {
                this.Close();
            }
            catch (SocketClosedException)
            {
                this.Close();
            }
            catch (Exception exception2)
            {
                this.server.Error(ErrorCode.Panic, "", "(" + ((this.connection == null) ? this.id.ToString() : this.connection.IdPlus) + ") Server.Client.Handler7: " + exception2.Message);
                this.Close();
            }
        }

        internal void MarketDataItem(object sender, MarketDataEventArgs e)
        {
            lock (e.Symbol.MarketData)
            {
                if (e.Symbol.MarketData.IsRunning)
                {
                    this.Send(e);
                }
            }
        }

        internal void MarketDataType(object sender, MarketDataTypeEventArgs e)
        {
            this.Send(e);
        }

        internal void MarketDepthItem(object sender, MarketDepthEventArgs e)
        {
            lock (e.Symbol.MarketDepth)
            {
                if (e.Symbol.MarketDepth.IsRunning)
                {
                    this.Send(e);
                }
            }
        }

        internal void MarketPosition(object sender, MarketPositionEventArgs e)
        {
            this.Send(e);
        }

        internal void News(object sender, NewsEventArgs e)
        {
            this.Send(e);
        }

        internal void NewsItemType(object sender, NewsItemTypeEventArgs e)
        {
            this.Send(e);
        }

        internal void OrderState(object sender, OrderStateEventArgs e)
        {
            this.Send(e);
        }

        internal void OrderStatus(object sender, OrderStatusEventArgs e)
        {
            lock (this.syncOrderEvents)
            {
                if ((e.Error == ErrorCode.NoError) && (e.OrderState.Id == OrderStateId.Initialized))
                {
                    ((ServerBytes) this.socket.Bytes).RegisterServerRequest(e.Order.Id, e.Order);
                }
                this.Send(e);
            }
        }

        internal void OrderType(object sender, OrderTypeEventArgs e)
        {
            this.Send(e);
        }

        internal void PositionUpdate(object sender, PositionUpdateEventArgs e)
        {
            lock (this.syncOrderEvents)
            {
                this.Send(e);
            }
        }

        private void Send(ITradingSerializable serializable)
        {
            if (this.socket.Connected)
            {
                try
                {
                    lock (this.socket.SendSync)
                    {
                        this.socket.Bytes.WriteSerializable(serializable);
                        this.socket.Send();
                    }
                }
                catch (SocketClosedException)
                {
                    this.Close();
                }
                catch (Exception exception)
                {
                    this.server.Error(ErrorCode.Panic, "", "(" + ((this.connection == null) ? this.id.ToString() : this.connection.IdPlus) + ") Client.Send (" + serializable.ClassId.ToString("g") + "): " + exception.Message);
                    this.Close();
                }
            }
        }

        internal void Symbol(object sender, SymbolEventArgs e)
        {
            if (e.Error == ErrorCode.NoError)
            {
                if (Globals.TraceSwitch.SymbolLookup)
                {
                    Trace.WriteLine("(" + this.connection.IdPlus + ") Server.Client.Symbol: " + e.Symbol.FullName);
                }
                ((ServerBytes) this.socket.Bytes).RegisterServerRequest(e.Symbol.Id, e.Symbol);
                this.Send(e);
            }
        }

        internal void SymbolType(object sender, SymbolTypeEventArgs e)
        {
            this.Send(e);
        }

        internal void TimeInForce(object sender, TimeInForceEventArgs e)
        {
            this.Send(e);
        }

        internal string ListBoxItem
        {
            get
            {
                if (this.connection == null)
                {
                    this.server.Error(ErrorCode.Panic, "", "(" + this.id + ") Client.ListBoxItem: connection is null");
                    return "";
                }
                if (this.listBoxItem.Length == 0)
                {
                    this.listBoxItem = string.Concat(new object[] { "(", this.id, ") ", this.connection.Options.Provider.Name });
                }
                return this.listBoxItem;
            }
        }
    }
}

