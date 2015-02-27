namespace TradeMagic.Pats
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;
    using iTrading.Core.Kernel;

    public class Adapter : IAdapter, IMarketData, IMarketDepth, IOrder, IOrderChange
    {
        internal ASCIIEncoding asciiEncoding = new ASCIIEncoding();
        private Callback callback;
        internal Connection connection;
        private PatsApi.ContractCallback contractAddCallback;
        private PatsApi.ContractCallback contractDeleteCallback;
        internal static Adapter currentAdapter = null;
        private PatsApi.PatsCallback dataDLCompleteCallback;
        private static bool demoInitialised = false;
        internal Hashtable exchange2OrderTypes = new Hashtable();
        private PatsApi.FillCallback fillCallback;
        private PatsApi.PatsCallback forcedLogoutCallback;
        private PatsApi.LinkCallback hostLinkStateCallback;
        private static bool liveInitialised = false;
        private PatsApi.PatsCallback logonStatusCallback;
        private PatsApi.MsgCallback messageCallback;
        internal NumberFormatInfo numberFormatInfo = new NumberFormatInfo();
        private PatsApi.OrderCallback orderCallback;
        internal ArrayList orders2Cancel = new ArrayList();
        internal ArrayList orderStubs = new ArrayList();
        internal PatsApi patsApi;
        private PatsApi.LinkCallback priceLinkStateCallback;
        private PatsApi.PriceUpdateCallback priceUpdateCallback;
        internal ArrayList subscribedPriceData = new ArrayList();
        private ArrayList symbols = new ArrayList();
        internal object[] syncOrderStatusEvent = new object[0];
        private static bool testInitialised = false;

        internal Adapter(Connection connection)
        {
            currentAdapter = this;
            this.connection = connection;
            this.callback = new Callback(this);
            this.numberFormatInfo.NumberDecimalSeparator = ".";
            this.patsApi = new PatsApi(this);
            this.contractAddCallback = new PatsApi.ContractCallback(this.callback.ContractAdded);
            this.contractDeleteCallback = new PatsApi.ContractCallback(this.callback.ContractDeleted);
            this.dataDLCompleteCallback = new PatsApi.PatsCallback(this.callback.DataDLComplete);
            this.fillCallback = new PatsApi.FillCallback(this.callback.Fill);
            this.forcedLogoutCallback = new PatsApi.PatsCallback(this.callback.ForcedLogout);
            this.hostLinkStateCallback = new PatsApi.LinkCallback(this.callback.HostLinkStateCallback);
            this.logonStatusCallback = new PatsApi.PatsCallback(this.callback.LogonStatus);
            this.messageCallback = new PatsApi.MsgCallback(this.callback.MessageCallback);
            this.orderCallback = new PatsApi.OrderCallback(this.callback.Order);
            this.priceLinkStateCallback = new PatsApi.LinkCallback(this.callback.PriceLinkStateCallback);
            this.priceUpdateCallback = new PatsApi.PriceUpdateCallback(this.callback.PriceUpdate);
        }

        public void Cancel(iTrading.Core.Kernel.Order order)
        {
            lock (this.syncOrderStatusEvent)
            {
                if (Globals.TraceSwitch.Order)
                {
                    Trace.WriteLine("(" + this.connection.IdPlus + ") Pats.Adapter.Cancel0: " + order.ToString());
                }
                OrderStub adapterLink = (OrderStub) order.AdapterLink;
                if ((adapterLink == null) || (adapterLink.orderId.Length == 0))
                {
                    this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.UnableToCancelOrder, "", "Unable to cancel order now. Please try again later"));
                }
                else
                {
                    ArrayList list;
                    if (order.OrderState.Id == OrderStateId.PendingChange)
                    {
                        lock ((list = this.orders2Cancel))
                        {
                            if (!this.orders2Cancel.Contains(adapterLink))
                            {
                                if (Globals.TraceSwitch.Order)
                                {
                                    Trace.WriteLine("(" + this.connection.IdPlus + ") Pats.Adapter.Cancel1: " + order.ToString());
                                }
                                this.orders2Cancel.Add(adapterLink);
                            }
                        }
                    }
                    else
                    {
                        TradeMagic.Pats.ErrorCode code = this.patsApi.CancelOrder(adapterLink.orderId);
                        if ((code == TradeMagic.Pats.ErrorCode.ErrInvalidState) || (((code == TradeMagic.Pats.ErrorCode.ErrUnknownOrder) && (order.OrderState.Id != OrderStateId.Working)) && ((order.OrderState.Id != OrderStateId.PendingCancel) && (order.OrderState.Id != OrderStateId.PartFilled))))
                        {
                            lock ((list = this.orders2Cancel))
                            {
                                if (!this.orders2Cancel.Contains(adapterLink))
                                {
                                    if (Globals.TraceSwitch.Order)
                                    {
                                        Trace.WriteLine("(" + this.connection.IdPlus + ") Pats.Adapter.Cancel2: " + order.ToString());
                                    }
                                    this.orders2Cancel.Add(adapterLink);
                                }
                                goto Label_0306;
                            }
                        }
                        if (code == TradeMagic.Pats.ErrorCode.ErrUnknownOrder)
                        {
                            this.connection.ProcessEventArgs(new OrderStatusEventArgs(order, iTrading.Core.Kernel.ErrorCode.UnableToCancelOrder, "Unknown order: unable to cancel order '" + order.OrderId + "'", order.OrderId, order.LimitPrice, order.StopPrice, order.Quantity, order.AvgFillPrice, order.Filled, order.OrderState, this.connection.Now));
                        }
                        else if (code != TradeMagic.Pats.ErrorCode.Success)
                        {
                            this.connection.ProcessEventArgs(new OrderStatusEventArgs(order, iTrading.Core.Kernel.ErrorCode.UnableToCancelOrder, "Unable to cancel order '" + order.OrderId + "': (" + code.ToString() + ")", order.OrderId, order.LimitPrice, order.StopPrice, order.Quantity, order.AvgFillPrice, order.Filled, order.OrderState, this.connection.Now));
                        }
                        else
                        {
                            this.connection.ProcessEventArgs(new OrderStatusEventArgs(order, iTrading.Core.Kernel.ErrorCode.NoError, "", order.OrderId, order.LimitPrice, order.StopPrice, order.Quantity, order.AvgFillPrice, order.Filled, this.connection.OrderStates[OrderStateId.PendingCancel], this.connection.Now));
                        }
                    Label_0306:;
                    }
                }
            }
        }

        public void Change(iTrading.Core.Kernel.Order order)
        {
            lock (this.syncOrderStatusEvent)
            {
                if (Globals.TraceSwitch.Order)
                {
                    Trace.WriteLine("(" + this.connection.IdPlus + ") Pats.Adapter.Change0: " + order.ToString());
                }
                OrderStub adapterLink = (OrderStub) order.AdapterLink;
                if ((adapterLink == null) || (adapterLink.orderId.Length == 0))
                {
                    this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.UnableToChangeOrder, "", "Unable to update order now. Please try again later."));
                }
                else
                {
                    iTrading.Core.Kernel.OrderType currentOrderType = this.GetCurrentOrderType(order);
                    if (currentOrderType == null)
                    {
                        this.connection.ProcessEventArgs(new OrderStatusEventArgs(order, iTrading.Core.Kernel.ErrorCode.UnableToChangeOrder, "Pats.Adapter.Change.GetCurrentOrderType failed", order.OrderId, order.LimitPrice, order.StopPrice, order.Quantity, order.AvgFillPrice, order.Filled, order.OrderState, order.Connection.Now));
                    }
                    else
                    {
                        PatsApi.AmendOrderStruct amendOrder = new PatsApi.AmendOrderStruct();
                        amendOrder.trader = adapterLink.patsOrder.traderAccount;
                        amendOrder.price = new char[0x15];
                        amendOrder.price2 = new char[0x15];
                        amendOrder.linkedOrder = new char[11];
                        if (currentOrderType.Id == OrderTypeId.Limit)
                        {
                            PatsApi.PriceToChars(this.numberFormatInfo, order.Symbol, order.LimitPrice, amendOrder.price);
                        }
                        else if (currentOrderType.Id == OrderTypeId.Stop)
                        {
                            PatsApi.PriceToChars(this.numberFormatInfo, order.Symbol, order.StopPrice, amendOrder.price);
                        }
                        else if (currentOrderType.Id == OrderTypeId.StopLimit)
                        {
                            PatsApi.PriceToChars(this.numberFormatInfo, order.Symbol, order.LimitPrice, amendOrder.price2);
                            PatsApi.PriceToChars(this.numberFormatInfo, order.Symbol, order.StopPrice, amendOrder.price);
                        }
                        amendOrder.lots = order.Quantity;
                        if ((order.Action.Id == ActionTypeId.Buy) || (order.Action.Id == ActionTypeId.SellShort))
                        {
                            amendOrder.openOrClose = 'O';
                        }
                        else
                        {
                            amendOrder.openOrClose = 'C';
                        }
                        TradeMagic.Pats.ErrorCode code = this.patsApi.AmendOrder(adapterLink.orderId, ref amendOrder);
                        if (code == TradeMagic.Pats.ErrorCode.ErrAmendDisabled)
                        {
                            this.connection.ProcessEventArgs(new OrderStatusEventArgs(order, iTrading.Core.Kernel.ErrorCode.UnableToChangeOrder, "Exchange does not support order changes (" + code.ToString() + ")", order.OrderId, order.LimitPrice, order.StopPrice, order.Quantity, order.AvgFillPrice, order.Filled, order.OrderState, this.connection.Now));
                        }
                        else if (code != TradeMagic.Pats.ErrorCode.Success)
                        {
                            this.connection.ProcessEventArgs(new OrderStatusEventArgs(order, iTrading.Core.Kernel.ErrorCode.UnableToChangeOrder, "Order '" + order.OrderId + "' can't be changed (" + code.ToString() + ")", order.OrderId, order.LimitPrice, order.StopPrice, order.Quantity, order.AvgFillPrice, order.Filled, order.OrderState, this.connection.Now));
                        }
                        else
                        {
                            this.connection.ProcessEventArgs(new OrderStatusEventArgs(order, iTrading.Core.Kernel.ErrorCode.NoError, "", order.OrderId, order.LimitPrice, order.StopPrice, order.Quantity, order.AvgFillPrice, order.Filled, this.connection.OrderStates[OrderStateId.PendingChange], this.connection.Now));
                        }
                    }
                }
            }
        }

        public void Clear()
        {
        }

        public void Connect()
        {
            if (Globals.TraceSwitch.Connect)
            {
                Trace.WriteLine(string.Concat(new object[] { 
                    "(", this.connection.IdPlus, ") Pats.Adapter.Connect0: host='", ((PatsOptions) this.connection.Options).HostAddress, "/", ((PatsOptions) this.connection.Options).HostPort, "' price='", ((PatsOptions) this.connection.Options).PriceAddress, "/", ((PatsOptions) this.connection.Options).PricePort, "' supertas=", ((PatsOptions) this.connection.Options).SuperTas, " enable=", ((PatsOptions) this.connection.Options).Enable, " host-handshake=", ((PatsOptions) this.connection.Options).HostHandShakeInterval, 
                    "/", ((PatsOptions) this.connection.Options).HostHandShakeTimeout, " price-handshake=", ((PatsOptions) this.connection.Options).PriceHandShakeInterval, "/", ((PatsOptions) this.connection.Options).PriceHandShakeTimeout
                 }));
            }
            if (this.connection.Options.Mode.Id == ModeTypeId.Simulation)
            {
                if (Globals.TraceSwitch.Connect)
                {
                    Trace.WriteLine("(" + this.connection.IdPlus + ") Pats.Adapter.Connect1");
                }
                new Thread(new ThreadStart(this.ConnectSimulation)).Start();
            }
            else
            {
                this.callback.InitVars();
                PatsOptions options = (PatsOptions) this.connection.Options;
                if (this.connection.Options.License.Id != LicenseTypeId.Professional)
                {
                    this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.InvalidLicense, "The Patsystems adapter requires professional license", ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                }
                else if (this.connection.Options.Password.Length > 10)
                {
                    this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.LoginFailed, "Password string too long, max = " + 10, ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                }
                else if (this.connection.Options.User.Length > 10)
                {
                    this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.LoginFailed, "User ID string too long, max = " + 10, ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                }
                else
                {
                    this.patsApi.Enable(options.Enable);
                    this.patsApi.SetClientPath(Globals.InstallDir + @"\bin\Pats\");
                    TradeMagic.Pats.ErrorCode success = TradeMagic.Pats.ErrorCode.Success;
                    if ((options.Mode.Id == ModeTypeId.Demo) && !demoInitialised)
                    {
                        success = this.patsApi.Initialise(0x44, "v2.8.3", " ", " ", " ");
                        if (success != TradeMagic.Pats.ErrorCode.Success)
                        {
                            this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.Panic, "Pats.Adapter.Connect.ptInitialise: can't initialise Patsystem (" + success + ")", ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                            return;
                        }
                        success = this.patsApi.SetInternetUser(false);
                        if (success != TradeMagic.Pats.ErrorCode.Success)
                        {
                            this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.Panic, "Pats.Adapter.Connect.ptSetInternetUser: error " + success, ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                            return;
                        }
                        demoInitialised = true;
                    }
                    else if ((options.Mode.Id == ModeTypeId.Test) && !testInitialised)
                    {
                        success = this.patsApi.Initialise(0x54, "v2.8.3", ((PatsOptions) this.connection.Options).ApplicationId, "1.0", ((PatsOptions) this.connection.Options).LicenseKey);
                        if (success != TradeMagic.Pats.ErrorCode.Success)
                        {
                            this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.Panic, "Pats.Adapter.Connect.ptInitialise: can't initialise Patsystem (" + success + ")", ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                            return;
                        }
                        success = this.patsApi.SetInternetUser(true);
                        if (success != TradeMagic.Pats.ErrorCode.Success)
                        {
                            this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.Panic, "Pats.Adapter.Connect.ptSetInternetUser: error " + success, ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                            return;
                        }
                        testInitialised = true;
                    }
                    else if ((options.Mode.Id == ModeTypeId.Live) && !liveInitialised)
                    {
                        success = this.patsApi.Initialise(0x43, "v2.8.3", ((PatsOptions) this.connection.Options).ApplicationId, "1.0", ((PatsOptions) this.connection.Options).LicenseKey);
                        if (success != TradeMagic.Pats.ErrorCode.Success)
                        {
                            this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.Panic, "Pats.Adapter.Connect.ptInitialise: can't initialise Patsystem (" + success + ")", ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                            return;
                        }
                        success = this.patsApi.SetInternetUser(true);
                        if (success != TradeMagic.Pats.ErrorCode.Success)
                        {
                            this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.Panic, "Pats.Adapter.Connect.ptSetInternetUser: error " + success, ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                            return;
                        }
                        liveInitialised = true;
                    }
                    if (options.Mode.Id != ModeTypeId.Demo)
                    {
                        success = this.patsApi.SetSuperTAS(options.SuperTas ? 'Y' : 'N');
                        if (success != TradeMagic.Pats.ErrorCode.Success)
                        {
                            this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.Panic, "Pats.Adapter.Connect.SetSuperTAS: error " + success, ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                            return;
                        }
                        if ((options.HostHandShakeInterval > 0) && (options.HostHandShakeTimeout > 0))
                        {
                            success = this.patsApi.SetHostHandshake(options.HostHandShakeInterval, options.HostHandShakeTimeout);
                            if (success != TradeMagic.Pats.ErrorCode.Success)
                            {
                                this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.Panic, "Pats.Adapter.Connect.SetHostHandshake: error " + success, ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                                return;
                            }
                        }
                        if ((options.PriceHandShakeInterval > 0) && (options.PriceHandShakeTimeout > 0))
                        {
                            success = this.patsApi.SetPriceHandshake(options.PriceHandShakeInterval, options.PriceHandShakeTimeout);
                            if (success != TradeMagic.Pats.ErrorCode.Success)
                            {
                                this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.Panic, "Pats.Adapter.Connect.SetPriceHandshake: error " + success, ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                                return;
                            }
                        }
                    }
                    success = this.patsApi.RegisterCallback(3, this.logonStatusCallback);
                    if (success != TradeMagic.Pats.ErrorCode.Success)
                    {
                        this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.Panic, "Pats.Adapter.Connect.ptRegisterCallback: error " + success, ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                    }
                    else
                    {
                        success = this.patsApi.RegisterCallback(7, this.dataDLCompleteCallback);
                        if (success != TradeMagic.Pats.ErrorCode.Success)
                        {
                            this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.Panic, "Pats.Adapter.Connect.ptRegisterCallback: error " + success, ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                        }
                        else
                        {
                            success = this.patsApi.RegisterCallback(6, this.forcedLogoutCallback);
                            if (success != TradeMagic.Pats.ErrorCode.Success)
                            {
                                this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.Panic, "Pats.Adapter.Connect.ptRegisterCallback: error " + success, ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                            }
                            else
                            {
                                success = this.patsApi.RegisterContractCallback(11, this.contractAddCallback);
                                if (success != TradeMagic.Pats.ErrorCode.Success)
                                {
                                    this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.Panic, "Pats.Adapter.Connect.RegisterContractCallback: error " + success, ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                                }
                                else
                                {
                                    success = this.patsApi.RegisterContractCallback(12, this.contractDeleteCallback);
                                    if (success != TradeMagic.Pats.ErrorCode.Success)
                                    {
                                        this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.Panic, "Pats.Adapter.Connect.RegisterContractCallback: error " + success, ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                                    }
                                    else
                                    {
                                        success = this.patsApi.RegisterMsgCallback(4, this.messageCallback);
                                        if (success != TradeMagic.Pats.ErrorCode.Success)
                                        {
                                            this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.Panic, "Pats.Adapter.Connect.ptRegisterMsgCallback: error " + success, ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                                        }
                                        else
                                        {
                                            success = this.patsApi.RegisterPriceCallback(8, this.priceUpdateCallback);
                                            if (success != TradeMagic.Pats.ErrorCode.Success)
                                            {
                                                this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.Panic, "Pats.Adapter.Connect.ptRegisterPriceCallback: error " + success, ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                                            }
                                            else
                                            {
                                                success = this.patsApi.RegisterOrderCallback(5, this.orderCallback);
                                                if (success != TradeMagic.Pats.ErrorCode.Success)
                                                {
                                                    this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.Panic, "Pats.Adapter.Connect.ptRegisterOrderCallback: error " + success, ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                                                }
                                                else
                                                {
                                                    success = this.patsApi.RegisterFillCallback(9, this.fillCallback);
                                                    if (success != TradeMagic.Pats.ErrorCode.Success)
                                                    {
                                                        this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.Panic, "Pats.Adapter.Connect.ptRegisterFillCallback: error " + success, ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                                                    }
                                                    else
                                                    {
                                                        success = this.patsApi.RegisterLinkStateCallback(1, this.hostLinkStateCallback);
                                                        if (success != TradeMagic.Pats.ErrorCode.Success)
                                                        {
                                                            this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.Panic, "Pats.Adapter.Connect.ptRegisterLinkStateCallback: error " + success, ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                                                        }
                                                        else
                                                        {
                                                            success = this.patsApi.RegisterLinkStateCallback(2, this.priceLinkStateCallback);
                                                            if (success != TradeMagic.Pats.ErrorCode.Success)
                                                            {
                                                                this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.Panic, "Pats.Adapter.Connect.ptRegisterLinkStateCallback: error " + success, ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                                                            }
                                                            else
                                                            {
                                                                success = this.patsApi.SetHostAddress(((PatsOptions) this.connection.Options).HostAddress, ((PatsOptions) this.connection.Options).HostPort.ToString());
                                                                if (success != TradeMagic.Pats.ErrorCode.Success)
                                                                {
                                                                    this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.Panic, "Pats.Adapter.Connect.ptSetHostAddress: error " + success, ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                                                                }
                                                                else
                                                                {
                                                                    success = this.patsApi.SetPriceAddress(((PatsOptions) this.connection.Options).PriceAddress, ((PatsOptions) this.connection.Options).PricePort.ToString());
                                                                    if (success != TradeMagic.Pats.ErrorCode.Success)
                                                                    {
                                                                        this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.Panic, "Pats.Adapter.Connect.ptSetPriceAddress: error " + success, ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                                                                    }
                                                                    else
                                                                    {
                                                                        success = this.patsApi.Ready();
                                                                        if (success != TradeMagic.Pats.ErrorCode.Success)
                                                                        {
                                                                            this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.Panic, "Pats.Adapter.Connect.ptReady: error " + success, ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
                                                                        }
                                                                        else if (Globals.TraceSwitch.Connect)
                                                                        {
                                                                            Trace.WriteLine("(" + this.connection.IdPlus + ") Pats.Adapter.Connect9");
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void ConnectSimulation()
        {
            this.callback.Init();
            this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", ConnectionStatusId.Connected, ConnectionStatusId.Connected, 0, ""));
        }

        internal iTrading.Core.Kernel.Symbol Convert(char[] exchangeName, char[] contractName, char[] contractDate)
        {
            iTrading.Core.Kernel.Exchange exchange = null;
            PatsApi.Commodity commodity;
            if ((PatsApi.ToString(exchangeName).ToUpper() == "CBOT") || (PatsApi.ToString(exchangeName).ToUpper() == "ECBOT"))
            {
                exchange = this.connection.Exchanges[ExchangeId.ECbot];
            }
            else if ((PatsApi.ToString(exchangeName).ToUpper() == "EUREX") || (PatsApi.ToString(exchangeName).ToUpper() == "XEUREX"))
            {
                exchange = this.connection.Exchanges[ExchangeId.Eurex];
            }
            else if ((PatsApi.ToString(exchangeName).ToUpper() == "MEFF") || (PatsApi.ToString(exchangeName).ToUpper() == "MEFFRV"))
            {
                exchange = this.connection.Exchanges[ExchangeId.Meff];
            }
            else
            {
                exchange = this.connection.Exchanges.FindByMapId(PatsApi.ToString(exchangeName));
            }
            if (exchange == null)
            {
                this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Pats.Adapter.Convert: can't convert TradeMagic.Pats exchange '" + PatsApi.ToString(exchangeName) + "'"));
                return null;
            }
            TradeMagic.Pats.Symbol adapterLink = this.GetPatsSymbol(PatsApi.ToString(exchangeName), PatsApi.ToString(contractName), PatsApi.ToString(contractDate), false);
            if (adapterLink == null)
            {
                this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Pats.Adapter.Convert: unknown symbol '" + PatsApi.ToString(contractName) + "'"));
                return null;
            }
            TradeMagic.Pats.ErrorCode code = currentAdapter.patsApi.GetCommodityByName(PatsApi.ToString(exchangeName), PatsApi.ToString(contractName), out commodity);
            if (code != TradeMagic.Pats.ErrorCode.Success)
            {
                currentAdapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(currentAdapter.connection, iTrading.Core.Kernel.ErrorCode.Panic, code.ToString(), "Pats.Adapter.Convert"));
                return null;
            }
            double tickSize = System.Convert.ToDouble(PatsApi.ToString(commodity.tickSize), this.numberFormatInfo);
            double pointValue = System.Convert.ToDouble(PatsApi.ToString(commodity.onePoint), this.numberFormatInfo);
            if ((tickSize == 0.2) && (commodity.ticksPerPoint == 8))
            {
                tickSize = Globals.TickSize8;
            }
            else if ((tickSize == 0.01) && (commodity.ticksPerPoint == 0x20))
            {
                tickSize = Globals.TickSize32;
            }
            else if ((tickSize == 0.005) && (commodity.ticksPerPoint == 320))
            {
                tickSize = Globals.TickSize64;
            }
            iTrading.Core.Kernel.Symbol symbol2 = this.connection.GetSymbolByProviderName(PatsApi.ToString(adapterLink.contract.leg0ExternalName).ToUpper(), PatsApi.ToExpiryDate(PatsApi.ToString(contractDate)), this.connection.SymbolTypes[SymbolTypeId.Future], exchange, 0.0, RightId.Unknown, LookupPolicyId.RepositoryOnly);
            if (symbol2 == null)
            {
                symbol2 = this.connection.CreateSymbol(PatsApi.ToString(adapterLink.contract.leg0ExternalName).ToUpper(), PatsApi.ToExpiryDate(PatsApi.ToString(contractDate)), this.connection.SymbolTypes[SymbolTypeId.Future], exchange, 0.0, RightId.Unknown, this.connection.Currencies[CurrencyId.Unknown], tickSize, pointValue, "", adapterLink, 0, null, null, null);
                if (symbol2 == null)
                {
                    this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Pats.Adapter.Convert: unknown symbol '" + PatsApi.ToString(contractName) + "'"));
                    return null;
                }
            }
            return symbol2;
        }

        private bool Convert(Order order, TradeMagic.Pats.Symbol patsSymbol, out PatsApi.Order ret)
        {
            ret = new PatsApi.Order();
            ret.traderAccount = new char[0x15];
            ret.orderType = new char[11];
            ret.exchangeName = new char[11];
            ret.contractName = new char[11];
            ret.contractDate = new char[0x33];
            ret.price = new char[0x15];
            ret.price2 = new char[0x15];
            ret.linkedOrder = new char[11];
            ret.goodTillDate = new char[9];
            order.Account.Name.CopyTo(0, ret.traderAccount, 0, order.Account.Name.Length);
            patsSymbol.contract.exchangeName.CopyTo(ret.exchangeName, 0);
            patsSymbol.contract.contractName.CopyTo(ret.contractName, 0);
            patsSymbol.contract.contractDate.CopyTo(ret.contractDate, 0);
            string str = PatsApi.ToString(patsSymbol.contract.exchangeName);
            Hashtable hashtable = (Hashtable) this.exchange2OrderTypes[str];
            if (hashtable == null)
            {
                this.connection.ProcessEventArgs(new OrderStatusEventArgs(order, iTrading.Core.Kernel.ErrorCode.OrderRejected, "Order can't be submitted. Exchange '" + order.Symbol.Exchange.Name + "' does not have any valid order types", order.OrderId, order.LimitPrice, order.StopPrice, order.Quantity, order.AvgFillPrice, order.Filled, this.connection.OrderStates[OrderStateId.Rejected], this.connection.Now));
                return false;
            }
            Trace.Assert(hashtable != null, "Pats.Adapter.Convert: unexpected exchange '" + str + "'");
            string str2 = (string)hashtable[(((int)order.OrderType.Id * 10) + ((int)order.TimeInForce.Id))];
            if (str2 == null)
            {
                this.connection.ProcessEventArgs(new OrderStatusEventArgs(order, iTrading.Core.Kernel.ErrorCode.OrderRejected, string.Concat(new object[] { "Combination order type '", order.OrderType.Id, "' and time-in-force '", order.TimeInForce.Id, "' not supported by exchange '", order.Symbol.Exchange.Name, "'" }), order.OrderId, order.LimitPrice, order.StopPrice, order.Quantity, order.AvgFillPrice, order.Filled, this.connection.OrderStates[OrderStateId.Rejected], this.connection.Now));
                return false;
            }
            str2.CopyTo(0, ret.orderType, 0, str2.Length);
            if ((order.Action.Id == ActionTypeId.Buy) || (order.Action.Id == ActionTypeId.BuyToCover))
            {
                ret.buyOrSell = 'B';
            }
            else
            {
                ret.buyOrSell = 'S';
            }
            if (order.OrderType.Id == OrderTypeId.Limit)
            {
                PatsApi.PriceToChars(this.numberFormatInfo, order.Symbol, order.LimitPrice, ret.price);
            }
            else if (order.OrderType.Id == OrderTypeId.Stop)
            {
                PatsApi.PriceToChars(this.numberFormatInfo, order.Symbol, order.StopPrice, ret.price);
            }
            else if (order.OrderType.Id == OrderTypeId.StopLimit)
            {
                PatsApi.PriceToChars(this.numberFormatInfo, order.Symbol, order.LimitPrice, ret.price2);
                PatsApi.PriceToChars(this.numberFormatInfo, order.Symbol, order.StopPrice, ret.price);
            }
            ret.lots = order.Quantity;
            if ((order.Action.Id == ActionTypeId.Buy) || (order.Action.Id == ActionTypeId.SellShort))
            {
                ret.openOrClose = 'O';
            }
            else
            {
                ret.openOrClose = 'C';
            }
            ret.triggerNow = 'Y';
            if (order.TimeInForce.Id == TimeInForceId.Day)
            {
                this.connection.Now.ToString("yyyyMMdd").ToCharArray().CopyTo(ret.goodTillDate, 0);
                ret.goodTillDate[8] = '\0';
            }
            return true;
        }

        public void Disconnect()
        {
            if (this.connection.Options.Mode.Id == ModeTypeId.Simulation)
            {
                new Thread(new ThreadStart(this.DisconnectNow)).Start();
            }
            else
            {
                TradeMagic.Pats.ErrorCode code;
                this.callback.InitVars();
                if (((PatsOptions) this.connection.Options).LogoffOnConnectionClose)
                {
                    code = this.patsApi.LogOff();
                    if (this.callback.patsThread != null)
                    {
                        this.callback.patsThread.Abort();
                        this.callback.patsThread.Join();
                        this.callback.patsThread = null;
                    }
                }
                else
                {
                    code = this.patsApi.Disconnect();
                    if (code != TradeMagic.Pats.ErrorCode.Success)
                    {
                        this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.Panic, code.ToString(), "Pats.Adapter.Disconnect"));
                        return;
                    }
                }
                new Thread(new ThreadStart(this.DisconnectNow)).Start();
            }
        }

        private void DisconnectNow()
        {
            this.connection.ProcessEventArgs(new ConnectionStatusEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", ConnectionStatusId.Disconnected, ConnectionStatusId.Disconnected, 0, ""));
        }

        internal iTrading.Core.Kernel.OrderType GetCurrentOrderType(iTrading.Core.Kernel.Order order)
        {
            PatsApi.OrderDetail detail;
            TradeMagic.Pats.ErrorCode code;
            OrderStub adapterLink = (OrderStub) order.AdapterLink;
            if ((adapterLink == null) || (adapterLink.orderId.Length == 0))
            {
                this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Pats.Adapter.GetCurrentOrderType: no order stub found"));
                return null;
            }
            if ((code = this.patsApi.GetOrderByID(adapterLink.orderId, out detail)) != TradeMagic.Pats.ErrorCode.Success)
            {
                this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.Panic, code.ToString(), "Pats.Adapter.GetCurrentOrderType.GetOrderByID failed"));
                return null;
            }
            string str = PatsApi.ToString(detail.exchangeName);
            Hashtable hashtable = (Hashtable) this.exchange2OrderTypes[str];
            if (hashtable == null)
            {
                this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Pats.Adapter.GetCurrentOrderType: Exchange '" + order.Symbol.Exchange.Name + "' does not have any valid order types"));
                return null;
            }
            Trace.Assert(hashtable != null, "Pats.Adapter.Change: unexpected exchange '" + str + "'");
            OrderTypeId market = order.OrderType.Id;
            string str2 = PatsApi.ToString(detail.orderType);
            if (((string) hashtable[0]) == str2)
            {
                market = OrderTypeId.Market;
            }
            else if (((string) hashtable[1]) == str2)
            {
                market = OrderTypeId.Market;
            }
            else if (((string) hashtable[10]) == str2)
            {
                market = OrderTypeId.Limit;
            }
            else if (((string) hashtable[11]) == str2)
            {
                market = OrderTypeId.Limit;
            }
            else if (((string) hashtable[0x15]) == str2)
            {
                market = OrderTypeId.Stop;
            }
            else if (((string) hashtable[20]) == str2)
            {
                market = OrderTypeId.Stop;
            }
            else if (((string) hashtable[30]) == str2)
            {
                market = OrderTypeId.StopLimit;
            }
            return order.Connection.OrderTypes[market];
        }

        internal TradeMagic.Pats.Symbol GetPatsSymbol(iTrading.Core.Kernel.Symbol symbol)
        {
            return this.GetPatsSymbol(symbol.Exchange.MapId, symbol.GetProviderName(ProviderTypeId.Patsystems), PatsApi.ToApiDate(symbol.Expiry), true);
        }

        internal TradeMagic.Pats.Symbol GetPatsSymbol(string exchangeName, string name, string contractDate, bool useExternalName)
        {
            lock (this.symbols)
            {
                if (this.symbols.Count == 0)
                {
                    int num;
                    TradeMagic.Pats.ErrorCode code = this.patsApi.CountContracts(out num);
                    if (code != TradeMagic.Pats.ErrorCode.Success)
                    {
                        this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.Panic, code.ToString(), "Pats.Adapter.GetPatsSymbol"));
                        return null;
                    }
                    for (int j = 0; j < num; j++)
                    {
                        PatsApi.Contract contract;
                        code = this.patsApi.GetContract(j, out contract);
                        if (code != TradeMagic.Pats.ErrorCode.Success)
                        {
                            this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.Panic, code.ToString(), "Pats.Adapter.GetPatsSymbol"));
                            return null;
                        }
                        this.symbols.Add(new TradeMagic.Pats.Symbol(j, contract));
                    }
                }
                for (int i = 0; i < this.symbols.Count; i++)
                {
                    TradeMagic.Pats.Symbol symbol = (TradeMagic.Pats.Symbol) this.symbols[i];
                    if (((useExternalName && (PatsApi.ToString(symbol.contract.leg0ExternalName).ToUpper() == name.ToUpper())) || (!useExternalName && (PatsApi.ToString(symbol.contract.contractName).ToUpper() == name.ToUpper()))) && (PatsApi.ToString(symbol.contract.contractDate).ToUpper() == contractDate.ToUpper()))
                    {
                        if (PatsApi.ToString(symbol.contract.exchangeName).ToUpper() == exchangeName.ToUpper())
                        {
                            return symbol;
                        }
                        if (((exchangeName.ToUpper() == "CBOT") || (exchangeName.ToUpper() == "ECBOT")) && ((PatsApi.ToString(symbol.contract.exchangeName).ToUpper() == "CBOT") || (PatsApi.ToString(symbol.contract.exchangeName).ToUpper() == "ECBOT")))
                        {
                            return symbol;
                        }
                        if (((exchangeName.ToUpper() == "EUREX") || (exchangeName.ToUpper() == "XEUREX")) && ((PatsApi.ToString(symbol.contract.exchangeName).ToUpper() == "EUREX") || (PatsApi.ToString(symbol.contract.exchangeName).ToUpper() == "XEUREX")))
                        {
                            return symbol;
                        }
                        if (((exchangeName.ToUpper() == "MEFF") || (exchangeName.ToUpper() == "MEFFRV")) && ((PatsApi.ToString(symbol.contract.exchangeName).ToUpper() == "MEFF") || (PatsApi.ToString(symbol.contract.exchangeName).ToUpper() == "MEFFRV")))
                        {
                            return symbol;
                        }
                    }
                }
                return null;
            }
        }

        public void Submit(iTrading.Core.Kernel.Order order)
        {
            lock (this.syncOrderStatusEvent)
            {
                if (Globals.TraceSwitch.Order)
                {
                    Trace.WriteLine("(" + this.connection.IdPlus + ") Pats.Adapter.Submit0: " + order.ToString());
                }
                TradeMagic.Pats.Symbol patsSymbol = this.GetPatsSymbol(order.Symbol);
                if (patsSymbol == null)
                {
                    this.connection.ProcessEventArgs(new OrderStatusEventArgs(order, iTrading.Core.Kernel.ErrorCode.OrderRejected, "Unknown symbol. Unable to submit order", order.OrderId, order.LimitPrice, order.StopPrice, order.Quantity, order.AvgFillPrice, order.Filled, this.connection.OrderStates[OrderStateId.Rejected], this.connection.Now));
                }
                else
                {
                    PatsApi.Order order2;
                    if (this.Convert(order, patsSymbol, out order2))
                    {
                        ArrayList list;
                        if (Globals.TraceSwitch.Order)
                        {
                            Trace.WriteLine("(" + this.connection.IdPlus + ") Pats.Adapter.Submit: exchange='" + PatsApi.ToString(order2.exchangeName) + "' ordertype='" + PatsApi.ToString(order2.orderType) + "' " + order.ToString());
                        }
                        StringBuilder orderId = new StringBuilder(11);
                        OrderStub stub = new OrderStub(order, order2);
                        stub.cbiOrder.AdapterLink = stub;
                        lock ((list = this.orderStubs))
                        {
                            this.orderStubs.Add(stub);
                        }
                        TradeMagic.Pats.ErrorCode code = this.patsApi.AddOrder(ref order2, orderId);
                        stub.orderId = orderId.ToString();
                        if (Globals.TraceSwitch.Order)
                        {
                            Trace.WriteLine("(" + this.connection.IdPlus + ") Pats.Adapter.SubmitNow0: " + stub.orderId + "/" + order.OrderId);
                        }
                        if (code == TradeMagic.Pats.ErrorCode.ErrInvalidState)
                        {
                            lock ((list = this.orderStubs))
                            {
                                this.orderStubs.Remove(stub);
                            }
                            this.connection.ProcessEventArgs(new OrderStatusEventArgs(order, iTrading.Core.Kernel.ErrorCode.UnableToSubmitOrder, "Order '" + order.OrderId + "' can't be submitted", order.OrderId, order.LimitPrice, order.StopPrice, order.Quantity, order.AvgFillPrice, order.Filled, this.connection.OrderStates[OrderStateId.Rejected], this.connection.Now));
                        }
                        else if (code != TradeMagic.Pats.ErrorCode.Success)
                        {
                            lock ((list = this.orderStubs))
                            {
                                this.orderStubs.Remove(stub);
                            }
                            this.connection.ProcessEventArgs(new OrderStatusEventArgs(order, iTrading.Core.Kernel.ErrorCode.OrderRejected, code.ToString(), order.OrderId, order.LimitPrice, order.StopPrice, order.Quantity, order.AvgFillPrice, order.Filled, this.connection.OrderStates[OrderStateId.Rejected], this.connection.Now));
                        }
                        else
                        {
                            this.connection.ProcessEventArgs(new OrderStatusEventArgs(order, iTrading.Core.Kernel.ErrorCode.NoError, "", stub.orderId, order.LimitPrice, order.StopPrice, order.Quantity, order.AvgFillPrice, order.Filled, this.connection.OrderStates[OrderStateId.PendingSubmit], this.connection.Now));
                        }
                    }
                }
            }
        }

        public void Subscribe(MarketData marketData)
        {
            if (Globals.TraceSwitch.Connect || Globals.TraceSwitch.MarketData)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") Pats.Adapter.Subscribe.MarketData: " + marketData.Symbol.FullName);
            }
            TradeMagic.Pats.Symbol patsSymbol = this.GetPatsSymbol(marketData.Symbol);
            if (patsSymbol == null)
            {
                this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoSuchSymbol, "Unknown symbol", "Unable to subsribe to market data"));
            }
            else
            {
                PatsApi.Contract contract;
                TradeMagic.Pats.ErrorCode code = this.patsApi.GetContract(patsSymbol.index, out contract);
                if (code != TradeMagic.Pats.ErrorCode.Success)
                {
                    this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.Panic, code.ToString(), "Pats.Adapter.MarketDataSubscribe"));
                }
                else
                {
                    lock (this.subscribedPriceData)
                    {
                        PriceStub stub = null;
                        foreach (PriceStub stub2 in this.subscribedPriceData)
                        {
                            if (stub2.cbiSymbol == marketData.Symbol)
                            {
                                stub = stub2;
                            }
                        }
                        if (stub == null)
                        {
                            this.subscribedPriceData.Add(new PriceStub(marketData.Symbol, patsSymbol, true));
                            code = this.patsApi.SubscribePrice(PatsApi.ToString(contract.exchangeName), PatsApi.ToString(contract.contractName), PatsApi.ToString(contract.contractDate));
                            if (code != TradeMagic.Pats.ErrorCode.Success)
                            {
                                this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.Panic, code.ToString(), "Pats.Adapter.MarketDataSubscribe"));
                            }
                        }
                        else
                        {
                            stub.marketData = true;
                        }
                    }
                }
            }
        }

        public void Subscribe(MarketDepth marketDepth)
        {
            if (Globals.TraceSwitch.Connect || Globals.TraceSwitch.MarketDepth)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") Pats.Adapter.Subscribe.MarketDepth: " + marketDepth.Symbol.FullName);
            }
            TradeMagic.Pats.Symbol patsSymbol = this.GetPatsSymbol(marketDepth.Symbol);
            if (patsSymbol == null)
            {
                this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoSuchSymbol, "Unknown symbol", "Unable to subsribe to market depth data"));
            }
            else
            {
                PatsApi.Contract contract;
                TradeMagic.Pats.ErrorCode code = this.patsApi.GetContract(patsSymbol.index, out contract);
                if (code != TradeMagic.Pats.ErrorCode.Success)
                {
                    this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.Panic, code.ToString(), "Pats.Adapter.MarketDepthSubscribe"));
                }
                else
                {
                    lock (this.subscribedPriceData)
                    {
                        PriceStub stub = null;
                        foreach (PriceStub stub2 in this.subscribedPriceData)
                        {
                            if (stub2.cbiSymbol == marketDepth.Symbol)
                            {
                                stub = stub2;
                            }
                        }
                        if (stub == null)
                        {
                            this.subscribedPriceData.Add(new PriceStub(marketDepth.Symbol, patsSymbol, false));
                            code = this.patsApi.SubscribePrice(PatsApi.ToString(contract.exchangeName), PatsApi.ToString(contract.contractName), PatsApi.ToString(contract.contractDate));
                            if (code != TradeMagic.Pats.ErrorCode.Success)
                            {
                                this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.Panic, code.ToString(), "Pats.Adapter.MarketDepthSubscribe"));
                            }
                        }
                        else
                        {
                            stub.marketDepth = true;
                        }
                    }
                }
            }
        }

        public void SymbolLookup(iTrading.Core.Kernel.Symbol template)
        {
            if (template.Name.Length > 10)
            {
                this.connection.ProcessEventArgs(new SymbolEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoSuchSymbol, "Symbol name exceeded max length", null));
            }
            else
            {
                TradeMagic.Pats.Symbol patsSymbol = this.GetPatsSymbol(template);
                if (patsSymbol == null)
                {
                    this.connection.ProcessEventArgs(new SymbolEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoSuchSymbol, "", null));
                }
                else
                {
                    PatsApi.Commodity commodity;
                    TradeMagic.Pats.ErrorCode code = currentAdapter.patsApi.GetCommodityByName(PatsApi.ToString(patsSymbol.contract.exchangeName), PatsApi.ToString(patsSymbol.contract.contractName), out commodity);
                    if (code != TradeMagic.Pats.ErrorCode.Success)
                    {
                        currentAdapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(currentAdapter.connection, iTrading.Core.Kernel.ErrorCode.Panic, code.ToString(), "Pats.Adapter.SymbolLookup"));
                        this.connection.ProcessEventArgs(new SymbolEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoSuchSymbol, "", null));
                    }
                    else
                    {
                        double tickSize = System.Convert.ToDouble(PatsApi.ToString(commodity.tickSize), this.numberFormatInfo);
                        double pointValue = System.Convert.ToDouble(PatsApi.ToString(commodity.onePoint), this.numberFormatInfo);
                        if ((tickSize == 0.2) && (commodity.ticksPerPoint == 8))
                        {
                            tickSize = Globals.TickSize8;
                        }
                        else if ((tickSize == 0.01) && (commodity.ticksPerPoint == 0x20))
                        {
                            tickSize = Globals.TickSize32;
                        }
                        else if ((tickSize == 0.005) && (commodity.ticksPerPoint == 320))
                        {
                            tickSize = Globals.TickSize64;
                        }
                        this.connection.CreateSymbol(template.Name, template.Expiry, template.SymbolType, template.Exchange, template.StrikePrice, template.Right.Id, this.connection.Currencies[CurrencyId.Unknown], tickSize, pointValue, "", patsSymbol, 0, null, null, null);
                    }
                }
            }
        }

        public void Unsubscribe(MarketData marketData)
        {
            if (Globals.TraceSwitch.Connect || Globals.TraceSwitch.MarketData)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") Pats.Adapter.Unsubscribe.MarketData: " + marketData.Symbol.FullName);
            }
            TradeMagic.Pats.Symbol patsSymbol = this.GetPatsSymbol(marketData.Symbol);
            if (patsSymbol == null)
            {
                this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoSuchSymbol, "Unknown symbol", "Unable to unsubsribe from market data"));
            }
            else
            {
                lock (this.subscribedPriceData)
                {
                    for (int i = 0; i < this.subscribedPriceData.Count; i++)
                    {
                        PriceStub stub = (PriceStub) this.subscribedPriceData[i];
                        if (stub.marketData && (stub.cbiSymbol == marketData.Symbol))
                        {
                            if (stub.marketDepth)
                            {
                                stub.marketData = false;
                            }
                            else
                            {
                                TradeMagic.Pats.ErrorCode code = this.patsApi.UnsubscribePrice(PatsApi.ToString(patsSymbol.contract.exchangeName), PatsApi.ToString(patsSymbol.contract.contractName), PatsApi.ToString(patsSymbol.contract.contractDate));
                                if (code != TradeMagic.Pats.ErrorCode.Success)
                                {
                                    this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.Panic, code.ToString(), "Pats.Adapter.Unsubscribe"));
                                }
                                this.subscribedPriceData.RemoveAt(i);
                            }
                            break;
                        }
                    }
                }
            }
        }

        public void Unsubscribe(MarketDepth marketDepth)
        {
            if (Globals.TraceSwitch.Connect || Globals.TraceSwitch.MarketDepth)
            {
                Trace.WriteLine("(" + this.connection.IdPlus + ") Pats.Adapter.Unsubscribe.MarketDepth: " + marketDepth.Symbol.FullName);
            }
            TradeMagic.Pats.Symbol patsSymbol = this.GetPatsSymbol(marketDepth.Symbol);
            if (patsSymbol == null)
            {
                this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.NoSuchSymbol, "Unknown symbol", "Unable to unsubsribe from market depth data"));
            }
            else
            {
                lock (this.subscribedPriceData)
                {
                    for (int i = 0; i < this.subscribedPriceData.Count; i++)
                    {
                        PriceStub stub = (PriceStub) this.subscribedPriceData[i];
                        if (stub.marketDepth && (stub.cbiSymbol == marketDepth.Symbol))
                        {
                            if (stub.marketData)
                            {
                                stub.marketDepth = false;
                            }
                            else
                            {
                                TradeMagic.Pats.ErrorCode code = this.patsApi.UnsubscribePrice(PatsApi.ToString(patsSymbol.contract.exchangeName), PatsApi.ToString(patsSymbol.contract.contractName), PatsApi.ToString(patsSymbol.contract.contractDate));
                                if (code != TradeMagic.Pats.ErrorCode.Success)
                                {
                                    this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, iTrading.Core.Kernel.ErrorCode.Panic, code.ToString(), "Pats.Adapter.Unsubscribe"));
                                }
                                this.subscribedPriceData.RemoveAt(i);
                            }
                            break;
                        }
                    }
                }
            }
        }

        public static StringCollection Contracts
        {
            get
            {
                int num;
                if (currentAdapter == null)
                {
                    throw new TMException(iTrading.Core.Kernel.ErrorCode.Panic, "Patsystems adapter is not yet connected");
                }
                StringCollection strings = new StringCollection();
                TradeMagic.Pats.ErrorCode code = currentAdapter.patsApi.CountContracts(out num);
                if (code != TradeMagic.Pats.ErrorCode.Success)
                {
                    currentAdapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(currentAdapter.connection, iTrading.Core.Kernel.ErrorCode.Panic, code.ToString(), "Pats.Adapter.Contracts1"));
                    return new StringCollection();
                }
                for (int i = 0; i < num; i++)
                {
                    PatsApi.Contract contract;
                    code = currentAdapter.patsApi.GetContract(i, out contract);
                    if (code != TradeMagic.Pats.ErrorCode.Success)
                    {
                        currentAdapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(currentAdapter.connection, iTrading.Core.Kernel.ErrorCode.Panic, code.ToString(), "Pats.Adapter.Contracts2"));
                        return new StringCollection();
                    }
                    strings.Add(PatsApi.ToString(contract.contractName) + "/" + PatsApi.ToString(contract.exchangeName) + "/" + PatsApi.ToString(contract.contractDate) + "/" + PatsApi.ToString(contract.leg0ExternalName));
                }
                return strings;
            }
        }

        public static StringCollection Exchanges
        {
            get
            {
                int num;
                if (currentAdapter == null)
                {
                    throw new TMException(iTrading.Core.Kernel.ErrorCode.Panic, "Patsystems adapter is not yet connected");
                }
                StringCollection strings = new StringCollection();
                TradeMagic.Pats.ErrorCode code = currentAdapter.patsApi.CountExchanges(out num);
                if (code != TradeMagic.Pats.ErrorCode.Success)
                {
                    currentAdapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(currentAdapter.connection, iTrading.Core.Kernel.ErrorCode.Panic, code.ToString(), "Pats.Adapter.Exchanges"));
                    return new StringCollection();
                }
                for (int i = 0; i < num; i++)
                {
                    PatsApi.Exchange exchange;
                    code = currentAdapter.patsApi.GetExchange(i, out exchange);
                    if (code != TradeMagic.Pats.ErrorCode.Success)
                    {
                        currentAdapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(currentAdapter.connection, iTrading.Core.Kernel.ErrorCode.Panic, code.ToString(), "Pats.Adapter.Exchanges"));
                        return new StringCollection();
                    }
                    strings.Add(PatsApi.ToString(exchange.name));
                }
                return strings;
            }
        }

        internal class OrderStub
        {
            internal iTrading.Core.Kernel.Order cbiOrder;
            internal int lastCountOrderHistory = 0;
            internal string orderId;
            internal PatsApi.Order patsOrder;

            internal OrderStub(iTrading.Core.Kernel.Order cbiOrder, PatsApi.Order patsOrder)
            {
                this.cbiOrder = cbiOrder;
                this.patsOrder = patsOrder;
            }
        }

        internal class PriceStub
        {
            internal iTrading.Core.Kernel.Symbol cbiSymbol;
            internal DateTime lastNonZeroMaskEvent = Globals.MinDate;
            internal bool marketData;
            internal bool marketDepth;
            internal TradeMagic.Pats.Symbol symbol;

            internal PriceStub(iTrading.Core.Kernel.Symbol cbiSymbol, TradeMagic.Pats.Symbol symbol, bool isMarketData)
            {
                this.cbiSymbol = cbiSymbol;
                this.marketData = isMarketData;
                this.marketDepth = !isMarketData;
                this.symbol = symbol;
            }
        }

        internal delegate void Process(object param);

        private delegate ArrayList ProcessRet(object param);
    }
}

