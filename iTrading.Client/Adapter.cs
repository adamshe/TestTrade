using iTrading.Client;

namespace TradeMagic.Client
{
    using System;
    using System.Diagnostics;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using iTrading.Core.Kernel;

    public class Adapter : IAdapter, IMarketData, IMarketDepth, IOrder, IOrderChange
    {
        private int clientVersion = 1;
        private bool closedByClient = false;
        private Connection connection;
        private Thread messageThread = null;
        private int serverVersion = 1;
        private ITradingSocket socket = null;
        private object[] syncOrderStatusEvent = new object[0];
        private object[] syncSymbolLookup = new object[0];

        internal Adapter(Connection connection)
        {
            this.connection = connection;
        }

        public void Cancel(Order order)
        {
            lock (this.syncOrderStatusEvent)
            {
                ((ClientBytes) this.socket.Bytes).RegisterClientRequest(order.Id, order);
                this.socket.Bytes.WriteSerializable(order);
                this.socket.Send();
            }
        }

        public void Change(Order order)
        {
            lock (this.syncOrderStatusEvent)
            {
                ((ClientBytes) this.socket.Bytes).RegisterClientRequest(order.Id, order);
                this.socket.Bytes.WriteSerializable(order);
                this.socket.Send();
            }
        }

        public void Clear()
        {
            this.socket.Bytes.WriteSerializable(this.connection);
            this.socket.Send();
        }

        public void Connect()
        {
            this.closedByClient = false;
            ThreadStart start = new ThreadStart(this.MessageLoop);
            this.messageThread = new Thread(start);
            this.messageThread.Name = "TM client";
            this.messageThread.Start();
        }

        public void Disconnect()
        {
            this.closedByClient = true;
            ((ClientBytes) this.socket.Bytes).RegisterClientRequest(this.connection.Id, this.connection);
            this.socket.Bytes.WriteSerializable(this.connection);
            this.socket.Send();
        }

        private void HandleMessage(ITradingSerializable serializable)
        {
            Trace.WriteLineIf(Globals.TraceSwitch.Native, "Client.Adapter.HandleMessage: received " + serializable.ClassId.ToString("g"));
            switch (serializable.ClassId)
            {
                case ClassId.AccountEventArgs:
                    this.connection.ProcessEventArgs((AccountEventArgs) serializable);
                    return;

                case ClassId.AccountItemTypeEventArgs:
                    this.connection.ProcessEventArgs((AccountItemTypeEventArgs) serializable);
                    return;

                case ClassId.AccountUpdateEventArgs:
                    this.connection.ProcessEventArgs((AccountUpdateEventArgs) serializable);
                    return;

                case ClassId.ActionTypeEventArgs:
                    this.connection.ProcessEventArgs((ActionTypeEventArgs) serializable);
                    return;

                case ClassId.ConnectionStatusEventArgs:
                {
                    ConnectionStatusEventArgs args = (ConnectionStatusEventArgs) serializable;
                    if (Globals.TraceSwitch.Connect)
                    {
                        Trace.WriteLine(string.Concat(new object[] { "(", this.connection.IdPlus, ") Client.HandleMessage.ConnectionStatusEventArgs ", args.ConnectionStatusId.ToString(), " ", args.Error }));
                    }
                    ((ClientBytes) this.socket.Bytes).RegisterServerRequest(args.Connection.Id, this.connection);
                    this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, args.Error, args.NativeError, args.ConnectionStatusId, args.SecondaryConnectionStatusId, args.ClientId, args.CustomText));
                    return;
                }
                case ClassId.CurrencyEventArgs:
                    this.connection.ProcessEventArgs((CurrencyEventArgs) serializable);
                    return;

                case ClassId.ErrorEventArgs:
                    this.connection.ProcessEventArgs((ITradingErrorEventArgs) serializable);
                    return;

                case ClassId.ExchangeEventArgs:
                    this.connection.ProcessEventArgs((ExchangeEventArgs) serializable);
                    return;

                case ClassId.ExecutionUpdateEventArgs:
                    this.connection.ProcessEventArgs((ExecutionUpdateEventArgs) serializable);
                    return;

                case ClassId.FeatureTypeEventArgs:
                    this.connection.ProcessEventArgs((FeatureTypeEventArgs) serializable);
                    return;

                case ClassId.MarketDataEventArgs:
                {
                    MarketDataEventArgs eventArgs = (MarketDataEventArgs) serializable;
                    lock (eventArgs.Symbol.MarketData)
                    {
                        if (eventArgs.Symbol.MarketData.IsRunning)
                        {
                            this.connection.ProcessEventArgs(eventArgs);
                        }
                        return;
                    }
                    
                }
                break;
                case ClassId.MarketDataTypeEventArgs:
                    break;

                case ClassId.MarketDepthEventArgs:
                {
                    MarketDepthEventArgs args3 = (MarketDepthEventArgs) serializable;
                    lock (args3.Symbol.MarketDepth)
                    {
                        if (args3.Symbol.MarketDepth.IsRunning)
                        {
                            this.connection.ProcessEventArgs(args3);
                        }
                        return;
                    }
                }
                case ClassId.MarketPositionEventArgs:
                    goto Label_02EF;

                case ClassId.NewsEventArgs:
                    this.connection.ProcessEventArgs((NewsEventArgs) serializable);
                    return;

                case ClassId.NewsItemTypeEventArgs:
                    this.connection.ProcessEventArgs((NewsItemTypeEventArgs) serializable);
                    return;

                case ClassId.OrderStateEventArgs:
                    this.connection.ProcessEventArgs((OrderStateEventArgs) serializable);
                    return;

                case ClassId.OrderStatusEventArgs:
                    lock (this.syncOrderStatusEvent)
                    {
                        OrderStatusEventArgs args4 = (OrderStatusEventArgs) serializable;
                        lock (args4.Order.Account.Orders)
                        {
                            if (args4.OrderState.Id == OrderStateId.Initialized)
                            {
                                Order clientRequest = args4.Order.Account.Orders.FindByOrderId(args4.Order.OrderId);
                                if (clientRequest == null)
                                {
                                    clientRequest = args4.Order.Account.CreateOrder(args4.Order.Symbol, args4.Order.Action.Id, args4.Order.OrderType.Id, args4.Order.TimeInForce.Id, args4.Order.Quantity, args4.Order.LimitPrice, args4.Order.StopPrice, args4.Order.Token, args4.OrderState, args4.Order.OrderId, args4.Order.OcaGroup, args4.Order.History, null);
                                    ((ClientBytes) this.socket.Bytes).RegisterClientRequest(clientRequest.Id, clientRequest);
                                }
                                ((ClientBytes) this.socket.Bytes).RegisterServerRequest(args4.Order.Id, clientRequest);
                            }
                            else
                            {
                                this.connection.ProcessEventArgs(new OrderStatusEventArgs(args4.Order, args4.Error, args4.NativeError, args4.OrderId, args4.LimitPrice, args4.StopPrice, args4.Quantity, args4.AvgFillPrice, args4.Filled, args4.OrderState, args4.Time));
                            }
                        }
                        return;
                    }
                case ClassId.OrderTypeEventArgs:
                    goto Label_04EA;

                case ClassId.PositionUpdateEventArgs:
                    this.connection.ProcessEventArgs((PositionUpdateEventArgs) serializable);
                    return;

                case ClassId.SymbolEventArgs:
                {
                    SymbolEventArgs args5 = (SymbolEventArgs) serializable;
                    if (Globals.TraceSwitch.SymbolLookup)
                    {
                        Trace.WriteLine("(" + this.connection.IdPlus + ") Client.HandleMessage.SymbolEventArgs " + ((args5.Error == ErrorCode.NoError) ? args5.Symbol.FullName : "null"));
                    }
                    if (args5.Error == ErrorCode.NoError)
                    {
                        lock (this.connection.Symbols)
                        {
                            Symbol symbol = this.connection.CreateSymbol(args5.Symbol.Name, args5.Symbol.Expiry, args5.Symbol.SymbolType, args5.Symbol.Exchange, args5.Symbol.StrikePrice, args5.Symbol.Right.Id, args5.Symbol.Currency, args5.Symbol.TickSize, args5.Symbol.PointValue, args5.Symbol.CompanyName, null, args5.Symbol.Id, args5.Symbol.Exchanges, args5.Symbol.Splits, args5.Symbol.Dividends);
                            ((ClientBytes) this.socket.Bytes).RegisterServerRequest(args5.Symbol.Id, symbol);
                            return;
                        }
                    }
                    this.connection.ProcessEventArgs(new SymbolEventArgs(this.connection, args5.Error, args5.NativeError, args5.Symbol));
                    return;
                }
                case ClassId.SymbolTypeEventArgs:
                    this.connection.ProcessEventArgs((SymbolTypeEventArgs) serializable);
                    return;

                case ClassId.TimeInForceEventArgs:
                    this.connection.ProcessEventArgs((TimeInForceEventArgs) serializable);
                    return;

                default:
                    this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.Panic, "", "TradeMagic.Client.HandleMessage: unknown class id '" + serializable.ClassId + "'"));
                    return;
            }
            this.connection.ProcessEventArgs((MarketDataTypeEventArgs) serializable);
            return;
        Label_02EF:
            this.connection.ProcessEventArgs((MarketPositionEventArgs) serializable);
            return;
        Label_04EA:
            this.connection.ProcessEventArgs((OrderTypeEventArgs) serializable);
        }

        internal void MessageLoop()
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress address = Dns.Resolve(this.connection.Options.TMHost).AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(address, this.connection.Options.TMPort);
            try
            {
                socket.Connect(remoteEP);
            }
            catch (Exception exception)
            {
                this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, ErrorCode.Panic, "TradeMagic.Client.MessageLoop " + exception.Message, ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                return;
            }
            this.socket = new ITradingSocket(socket, new ClientBytes(this.connection));
            this.socket.Bytes.Write(this.clientVersion);
            this.socket.Send();
            this.serverVersion = this.socket.Receive().ReadInt32();
            ((ClientBytes) this.socket.Bytes).RegisterClientRequest(this.connection.Id, this.connection);
            this.socket.Bytes.WriteSerializable(this.connection);
            this.socket.Send();
        Label_0115:
            try
            {
                ITradingSerializable serializable = this.socket.Receive().ReadSerializable();
                this.HandleMessage(serializable);
                if ((serializable.ClassId != ClassId.ConnectionStatusEventArgs) || (((ConnectionStatusEventArgs) serializable).ConnectionStatusId != ConnectionStatusId.Disconnected))
                {
                    goto Label_0115;
                }
                if (!this.closedByClient)
                {
                    this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, ErrorCode.ServerConnectionIsBroken, "Server closed socket", ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                }
                this.socket.Close();
            }
            catch (SocketClosedException)
            {
                if (!this.closedByClient)
                {
                    this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, ErrorCode.ServerConnectionIsBroken, "Server closed socket", ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                }
                Thread.CurrentThread.Abort();
                goto Label_0115;
            }
            catch (ThreadAbortException)
            {
                Thread.CurrentThread.Abort();
                goto Label_0115;
            }
            catch (Exception exception2)
            {
                this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.Panic, "", "Client.Adapter.MessageLoop: " + exception2.Message));
                Thread.CurrentThread.Abort();
                goto Label_0115;
            }
        }

        public void Submit(Order order)
        {
            lock (this.syncOrderStatusEvent)
            {
                ((ClientBytes) this.socket.Bytes).RegisterClientRequest(order.Id, order);
                this.socket.Bytes.WriteSerializable(order);
                this.socket.Send();
            }
        }

        public void Subscribe(MarketData marketData)
        {
            ((ClientBytes) this.socket.Bytes).RegisterClientRequest(marketData.Id, marketData);
            this.socket.Bytes.WriteSerializable(marketData);
            this.socket.Send();
        }

        public void Subscribe(MarketDepth marketDepth)
        {
            ((ClientBytes) this.socket.Bytes).RegisterClientRequest(marketDepth.Id, marketDepth);
            this.socket.Bytes.WriteSerializable(marketDepth);
            this.socket.Send();
        }

        public void SymbolLookup(Symbol template)
        {
            ((ClientBytes) this.socket.Bytes).RegisterClientRequest(template.Id, this.connection);
            this.socket.Bytes.WriteSerializable(template);
            this.socket.Send();
        }

        public void Unsubscribe(MarketData marketData)
        {
            this.socket.Bytes.WriteSerializable(marketData);
            this.socket.Send();
        }

        public void Unsubscribe(MarketDepth marketDepth)
        {
            this.socket.Bytes.WriteSerializable(marketDepth);
            this.socket.Send();
        }

        public object SyncSymbolLookup
        {
            get
            {
                return this.syncSymbolLookup;
            }
        }
    }
}

