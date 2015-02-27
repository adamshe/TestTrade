namespace TradeMagic.Pats
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Text;
    using iTrading.Core.Kernel;

    internal class PatsApi
    {
        private Adapter adapter;
        internal const string apiVersion = "v2.8.3";
        internal const int array10Length = 10;
        internal const int array20Length = 20;
        internal const int array25Length = 0x19;
        internal const int contractDateLength = 50;
        internal const int contractNameLength = 10;
        internal const int dateLength = 8;
        internal const string demoDll = @"Pats\demoapi.dll";
        internal const int exchangeNameLength = 10;
        internal const int exchangeOrderIdLength = 30;
        internal const int fillIdLength = 70;
        internal const int floatStringLength = 20;
        internal const int gtdLength = 8;
        internal bool isDemo;
        internal const string liveDll = @"Pats\patsapi.dll";
        internal const int orderIdLength = 10;
        internal const int orderTypeLength = 10;
        internal const int priceStringLength = 20;
        internal const int sizeofInt = 4;
        internal const int sizeofPriceDetailed = 30;
        internal const int textTypeLength = 60;
        internal const int timeLength = 6;
        internal const int traderStringLength = 20;

        internal PatsApi(Adapter adapter)
        {
            this.adapter = adapter;
            this.isDemo = adapter.connection.Options.Mode.Id == ModeTypeId.Demo;
        }

        public TradeMagic.Pats.ErrorCode AcknowledgeUsrMsg(string msgId)
        {
            if (!this.isDemo)
            {
                return LiveAcknowledgeUsrMsg(msgId);
            }
            return DemoAcknowledgeUsrMsg(msgId);
        }

        public TradeMagic.Pats.ErrorCode AddOrder(ref Order order, StringBuilder orderId)
        {
            if (!this.isDemo)
            {
                return LiveAddOrder(ref order, orderId);
            }
            return DemoAddOrder(ref order, orderId);
        }

        public TradeMagic.Pats.ErrorCode AmendOrder(string orderId, ref AmendOrderStruct amendOrder)
        {
            if (!this.isDemo)
            {
                return LiveAmendOrder(orderId, ref amendOrder);
            }
            return DemoAmendOrder(orderId, ref amendOrder);
        }

        public TradeMagic.Pats.ErrorCode BuyingPowerRemaining(string exchangeName, string contractName, string contractDate, string traderAccount, StringBuilder bpRemaining)
        {
            if (!this.isDemo)
            {
                return LiveBuyingPowerRemaining(exchangeName, contractName, contractDate, traderAccount, bpRemaining);
            }
            return DemoBuyingPowerRemaining(exchangeName, contractName, contractDate, traderAccount, bpRemaining);
        }

        public TradeMagic.Pats.ErrorCode CancelOrder(string orderId)
        {
            if (!this.isDemo)
            {
                return LiveCancelOrder(orderId);
            }
            return DemoCancelOrder(orderId);
        }

        internal static double CharsToPrice(IFormatProvider formatProvider, iTrading.Core.Kernel.Symbol symbol, char[] chars)
        {
            double d = ToDouble(chars, formatProvider);
            Trace.Assert(d >= 0.0, "Pats.PatsApi.CharsToPrice");
            if (symbol.TickSize == Globals.TickSize8)
            {
                double num2 = Math.Floor(d);
                return symbol.Round2TickSize(num2 + (((d - num2) * 10.0) * Globals.TickSize8));
            }
            if (symbol.TickSize == Globals.TickSize32)
            {
                double num3 = Math.Floor(d);
                return symbol.Round2TickSize(num3 + (((d - num3) * 100.0) * Globals.TickSize32));
            }
            if (symbol.TickSize == Globals.TickSize64)
            {
                double num4 = Math.Floor(d);
                d = symbol.Round2TickSize(num4 + ((((d - num4) * 1000.0) * Globals.TickSize32) / 10.0));
            }
            return d;
        }

        public TradeMagic.Pats.ErrorCode CountContracts(out int count)
        {
            if (!this.isDemo)
            {
                return LiveCountContracts(out count);
            }
            return DemoCountContracts(out count);
        }

        public TradeMagic.Pats.ErrorCode CountExchanges(out int count)
        {
            if (!this.isDemo)
            {
                return LiveCountExchanges(out count);
            }
            return DemoCountExchanges(out count);
        }

        public TradeMagic.Pats.ErrorCode CountFills(out int count)
        {
            if (!this.isDemo)
            {
                return LiveCountFills(out count);
            }
            return DemoCountFills(out count);
        }

        public TradeMagic.Pats.ErrorCode CountOrderHistory(int orderIdx, out int count)
        {
            if (!this.isDemo)
            {
                return LiveCountOrderHistory(orderIdx, out count);
            }
            return DemoCountOrderHistory(orderIdx, out count);
        }

        public TradeMagic.Pats.ErrorCode CountOrders(out int count)
        {
            if (!this.isDemo)
            {
                return LiveCountOrders(out count);
            }
            return DemoCountOrders(out count);
        }

        public TradeMagic.Pats.ErrorCode CountOrderTypes(out int count)
        {
            if (!this.isDemo)
            {
                return LiveCountOrderTypes(out count);
            }
            return DemoCountOrderTypes(out count);
        }

        public TradeMagic.Pats.ErrorCode CountTraders(out int count)
        {
            if (!this.isDemo)
            {
                return LiveCountTraders(out count);
            }
            return DemoCountTraders(out count);
        }

        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\demoapi.dll", EntryPoint="ptAcknowledgeUsrMsg")]
        private static extern TradeMagic.Pats.ErrorCode DemoAcknowledgeUsrMsg(string msgId);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\demoapi.dll", EntryPoint="ptAddOrder")]
        private static extern TradeMagic.Pats.ErrorCode DemoAddOrder(ref Order order, [MarshalAs(UnmanagedType.LPStr)] StringBuilder orderId);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\demoapi.dll", EntryPoint="ptAmendOrder")]
        private static extern TradeMagic.Pats.ErrorCode DemoAmendOrder(string orderId, ref AmendOrderStruct amendOrder);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\demoapi.dll", EntryPoint="ptBuyingPowerRemaining")]
        private static extern TradeMagic.Pats.ErrorCode DemoBuyingPowerRemaining(string exchangeName, string contractName, string contractDate, string traderAccount, [MarshalAs(UnmanagedType.LPStr)] StringBuilder bpRemaining);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\demoapi.dll", EntryPoint="ptCancelOrder")]
        private static extern TradeMagic.Pats.ErrorCode DemoCancelOrder(string orderId);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\demoapi.dll", EntryPoint="ptCountContracts")]
        private static extern TradeMagic.Pats.ErrorCode DemoCountContracts(out int count);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\demoapi.dll", EntryPoint="ptCountExchanges")]
        private static extern TradeMagic.Pats.ErrorCode DemoCountExchanges(out int count);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\demoapi.dll", EntryPoint="ptCountFills")]
        private static extern TradeMagic.Pats.ErrorCode DemoCountFills(out int count);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\demoapi.dll", EntryPoint="ptCountOrderHistory")]
        private static extern TradeMagic.Pats.ErrorCode DemoCountOrderHistory(int orderIdx, out int count);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\demoapi.dll", EntryPoint="ptCountOrders")]
        private static extern TradeMagic.Pats.ErrorCode DemoCountOrders(out int count);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\demoapi.dll", EntryPoint="ptCountOrderTypes")]
        private static extern TradeMagic.Pats.ErrorCode DemoCountOrderTypes(out int count);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\demoapi.dll", EntryPoint="ptCountTraders")]
        private static extern TradeMagic.Pats.ErrorCode DemoCountTraders(out int count);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\demoapi.dll", EntryPoint="ptDisconnect")]
        private static extern TradeMagic.Pats.ErrorCode DemoDisconnect();
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\demoapi.dll", EntryPoint="ptEnable")]
        private static extern void DemoEnable(int options);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\demoapi.dll", EntryPoint="ptEnabledFunctionality")]
        private static extern TradeMagic.Pats.ErrorCode DemoEnabledFunctionality(out int functionality, out int software);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\demoapi.dll", EntryPoint="ptGetCommodityByName")]
        private static extern TradeMagic.Pats.ErrorCode DemoGetCommodityByName(string exchangeName, string contractName, out Commodity commodity);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\demoapi.dll", EntryPoint="ptGetContract")]
        private static extern TradeMagic.Pats.ErrorCode DemoGetContract(int count, out Contract contract);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\demoapi.dll", EntryPoint="ptGetContractByName")]
        private static extern TradeMagic.Pats.ErrorCode DemoGetContractByName(string exchangeName, string contractName, string contractDate, out Contract contract);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\demoapi.dll", EntryPoint="ptGetContractPosition")]
        private static extern TradeMagic.Pats.ErrorCode DemoGetContractPosition(string exchangeName, string contractName, string contractDate, string traderAccount, out PositionStruct positionStruct);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\demoapi.dll", EntryPoint="ptGetExchange")]
        private static extern TradeMagic.Pats.ErrorCode DemoGetExchange(int count, out Exchange exchange);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\demoapi.dll", EntryPoint="ptGetFill")]
        private static extern TradeMagic.Pats.ErrorCode DemoGetFill(int count, out Fill fill);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\demoapi.dll", EntryPoint="ptGetFillByID")]
        private static extern TradeMagic.Pats.ErrorCode DemoGetFillByID(string fillId, out Fill fill);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\demoapi.dll", EntryPoint="ptGetLogonStatus")]
        private static extern TradeMagic.Pats.ErrorCode DemoGetLogonStatus(out LogonStatus status);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\demoapi.dll", EntryPoint="ptGetOpenPosition")]
        private static extern TradeMagic.Pats.ErrorCode DemoGetOpenPosition(string exchangeName, string contractName, string contractDate, string traderAccount, out PositionStruct positionStruct);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\demoapi.dll", EntryPoint="ptGetOrder")]
        private static extern TradeMagic.Pats.ErrorCode DemoGetOrder(int count, out OrderDetail orderDetail);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\demoapi.dll", EntryPoint="ptGetOrderByID")]
        private static extern TradeMagic.Pats.ErrorCode DemoGetOrderByID(string orderId, out OrderDetail orderDetail);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\demoapi.dll", EntryPoint="ptGetOrderHistory")]
        private static extern TradeMagic.Pats.ErrorCode DemoGetOrderHistory(int orderIdx, int pos, out OrderDetail orderDetail);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\demoapi.dll", EntryPoint="ptGetOrderType")]
        private static extern TradeMagic.Pats.ErrorCode DemoGetOrderType(int count, out OrderType orderType);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\demoapi.dll", EntryPoint="ptGetPrice")]
        private static extern TradeMagic.Pats.ErrorCode DemoGetPrice(int count, [Out] byte[] buf);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\demoapi.dll", EntryPoint="ptGetTotalPosition")]
        private static extern TradeMagic.Pats.ErrorCode DemoGetTotalPosition(string traderAccount, out PositionStruct positionStruct);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\demoapi.dll", EntryPoint="ptGetTrader")]
        private static extern TradeMagic.Pats.ErrorCode DemoGetTrader(int count, out TraderAccount traderAccount);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\demoapi.dll", EntryPoint="ptGetUsrMsgByID")]
        private static extern TradeMagic.Pats.ErrorCode DemoGetUsrMsgByID(string msgId, out Message msg);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\demoapi.dll", EntryPoint="ptHostLinkStateChange")]
        private static extern TradeMagic.Pats.ErrorCode DemoHostLinkStateChange(string host, string port);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\demoapi.dll", EntryPoint="ptInitialise")]
        private static extern TradeMagic.Pats.ErrorCode DemoInitialise(byte env, string apiVersion, string appId, string appVersion, string license);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\demoapi.dll", EntryPoint="ptLogOff")]
        private static extern TradeMagic.Pats.ErrorCode DemoLogOff();
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\demoapi.dll", EntryPoint="ptLogOn")]
        private static extern TradeMagic.Pats.ErrorCode DemoLogOn(ref Logon logon);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\demoapi.dll", EntryPoint="ptReady")]
        private static extern TradeMagic.Pats.ErrorCode DemoReady();
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\demoapi.dll", EntryPoint="ptRegisterCallback")]
        private static extern TradeMagic.Pats.ErrorCode DemoRegisterCallback(int callbackId, PatsCallback callback);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\demoapi.dll", EntryPoint="ptRegisterContractCallback")]
        private static extern TradeMagic.Pats.ErrorCode DemoRegisterContractCallback(int callbackId, ContractCallback callback);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\demoapi.dll", EntryPoint="ptRegisterFillCallback")]
        private static extern TradeMagic.Pats.ErrorCode DemoRegisterFillCallback(int callbackId, FillCallback callback);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\demoapi.dll", EntryPoint="ptRegisterLinkStateCallback")]
        private static extern TradeMagic.Pats.ErrorCode DemoRegisterLinkStateCallback(int callbackId, LinkCallback callback);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\demoapi.dll", EntryPoint="ptRegisterMsgCallback")]
        private static extern TradeMagic.Pats.ErrorCode DemoRegisterMsgCallback(int callbackId, MsgCallback callback);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\demoapi.dll", EntryPoint="ptRegisterOrderCallback")]
        private static extern TradeMagic.Pats.ErrorCode DemoRegisterOrderCallback(int callbackId, OrderCallback callback);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\demoapi.dll", EntryPoint="ptRegisterPriceCallback")]
        private static extern TradeMagic.Pats.ErrorCode DemoRegisterPriceCallback(int callbackId, PriceUpdateCallback callback);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\demoapi.dll", EntryPoint="ptSetClientPath")]
        private static extern void DemoSetClientPath(string clientPath);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\demoapi.dll", EntryPoint="ptSetHostAddress")]
        private static extern TradeMagic.Pats.ErrorCode DemoSetHostAddress(string host, string port);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\demoapi.dll", EntryPoint="ptSetHostHandShake")]
        private static extern TradeMagic.Pats.ErrorCode DemoSetHostHandShake(int interval, int timeout);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\demoapi.dll", EntryPoint="ptSetInternetUser")]
        private static extern TradeMagic.Pats.ErrorCode DemoSetInternetUser(string yesNo);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\demoapi.dll", EntryPoint="ptSetPriceAddress")]
        private static extern TradeMagic.Pats.ErrorCode DemoSetPriceAddress(string host, string port);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\demoapi.dll", EntryPoint="ptSetPriceAgeCounter")]
        private static extern TradeMagic.Pats.ErrorCode DemoSetPriceAgeCounter(int age);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\demoapi.dll", EntryPoint="ptSetPriceHandShake")]
        private static extern TradeMagic.Pats.ErrorCode DemoSetPriceHandShake(int interval, int timeout);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\demoapi.dll", EntryPoint="ptSetSuperTAS")]
        private static extern TradeMagic.Pats.ErrorCode DemoSetSuperTAS(char enable);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\demoapi.dll", EntryPoint="ptSubscribePrice")]
        private static extern TradeMagic.Pats.ErrorCode DemoSubscribePrice(string exchangeName, string contractName, string contractDate);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\demoapi.dll", EntryPoint="ptUnsubscribePrice")]
        private static extern TradeMagic.Pats.ErrorCode DemoUnsubscribePrice(string exchangeName, string contractName, string contractDate);
        public TradeMagic.Pats.ErrorCode Disconnect()
        {
            if (!this.isDemo)
            {
                return LiveDisconnect();
            }
            return DemoDisconnect();
        }

        public void Enable(int options)
        {
            if (this.isDemo)
            {
                DemoEnable(options);
            }
            else
            {
                LiveEnable(options);
            }
        }

        public TradeMagic.Pats.ErrorCode EnabledFunctionality(out int functionality, out int software)
        {
            if (!this.isDemo)
            {
                return LiveEnabledFunctionality(out functionality, out software);
            }
            return DemoEnabledFunctionality(out functionality, out software);
        }

        public TradeMagic.Pats.ErrorCode GetCommodityByName(string exchangeName, string contractName, out Commodity commodity)
        {
            if (!this.isDemo)
            {
                return LiveGetCommodityByName(exchangeName, contractName, out commodity);
            }
            return DemoGetCommodityByName(exchangeName, contractName, out commodity);
        }

        public TradeMagic.Pats.ErrorCode GetContract(int count, out Contract contract)
        {
            if (!this.isDemo)
            {
                return LiveGetContract(count, out contract);
            }
            return DemoGetContract(count, out contract);
        }

        public TradeMagic.Pats.ErrorCode GetContractByName(string exchangeName, string contractName, string contractDate, out Contract contract)
        {
            if (!this.isDemo)
            {
                return LiveGetContractByName(exchangeName, contractName, contractDate, out contract);
            }
            return DemoGetContractByName(exchangeName, contractName, contractDate, out contract);
        }

        public TradeMagic.Pats.ErrorCode GetContractPosition(string exchangeName, string contractName, string contractDate, string traderAccount, out PositionStruct positionStruct)
        {
            if (!this.isDemo)
            {
                return LiveGetContractPosition(exchangeName, contractName, contractDate, traderAccount, out positionStruct);
            }
            return DemoGetContractPosition(exchangeName, contractName, contractDate, traderAccount, out positionStruct);
        }

        public TradeMagic.Pats.ErrorCode GetExchange(int count, out Exchange exchange)
        {
            if (!this.isDemo)
            {
                return LiveGetExchange(count, out exchange);
            }
            return DemoGetExchange(count, out exchange);
        }

        public TradeMagic.Pats.ErrorCode GetFill(int count, out Fill fill)
        {
            if (!this.isDemo)
            {
                return LiveGetFill(count, out fill);
            }
            return DemoGetFill(count, out fill);
        }

        public TradeMagic.Pats.ErrorCode GetFillByID(string fillId, out Fill fill)
        {
            if (!this.isDemo)
            {
                return LiveGetFillByID(fillId, out fill);
            }
            return DemoGetFillByID(fillId, out fill);
        }

        public TradeMagic.Pats.ErrorCode GetLogonStatus(out LogonStatus status)
        {
            if (!this.isDemo)
            {
                return LiveGetLogonStatus(out status);
            }
            return DemoGetLogonStatus(out status);
        }

        public TradeMagic.Pats.ErrorCode GetOpenPosition(string exchangeName, string contractName, string contractDate, string traderAccount, out PositionStruct positionStruct)
        {
            if (!this.isDemo)
            {
                return LiveGetOpenPosition(exchangeName, contractName, contractDate, traderAccount, out positionStruct);
            }
            return DemoGetOpenPosition(exchangeName, contractName, contractDate, traderAccount, out positionStruct);
        }

        public TradeMagic.Pats.ErrorCode GetOrder(int count, out OrderDetail orderDetail)
        {
            if (!this.isDemo)
            {
                return LiveGetOrder(count, out orderDetail);
            }
            return DemoGetOrder(count, out orderDetail);
        }

        public TradeMagic.Pats.ErrorCode GetOrderByID(string orderId, out OrderDetail orderDetail)
        {
            if (!this.isDemo)
            {
                return LiveGetOrderByID(orderId, out orderDetail);
            }
            return DemoGetOrderByID(orderId, out orderDetail);
        }

        public TradeMagic.Pats.ErrorCode GetOrderHistory(int orderIdx, int pos, out OrderDetail orderDetail)
        {
            if (!this.isDemo)
            {
                return LiveGetOrderHistory(orderIdx, pos, out orderDetail);
            }
            return DemoGetOrderHistory(orderIdx, pos, out orderDetail);
        }

        public TradeMagic.Pats.ErrorCode GetOrderType(int count, out OrderType orderType)
        {
            if (!this.isDemo)
            {
                return LiveGetOrderType(count, out orderType);
            }
            return DemoGetOrderType(count, out orderType);
        }

        public TradeMagic.Pats.ErrorCode GetPrice(int count, byte[] buf)
        {
            if (!this.isDemo)
            {
                return LiveGetPrice(count, buf);
            }
            return DemoGetPrice(count, buf);
        }

        public TradeMagic.Pats.ErrorCode GetTotalPosition(string traderAccount, out PositionStruct positionStruct)
        {
            if (!this.isDemo)
            {
                return LiveGetTotalPosition(traderAccount, out positionStruct);
            }
            return DemoGetTotalPosition(traderAccount, out positionStruct);
        }

        public TradeMagic.Pats.ErrorCode GetTrader(int count, out TraderAccount traderAccount)
        {
            if (!this.isDemo)
            {
                return LiveGetTrader(count, out traderAccount);
            }
            return DemoGetTrader(count, out traderAccount);
        }

        public TradeMagic.Pats.ErrorCode GetUsrMsgByID(string msgId, out Message msg)
        {
            if (!this.isDemo)
            {
                return LiveGetUsrMsgByID(msgId, out msg);
            }
            return DemoGetUsrMsgByID(msgId, out msg);
        }

        public TradeMagic.Pats.ErrorCode HostLinkStateChange(string host, string port)
        {
            if (!this.isDemo)
            {
                return LiveHostLinkStateChange(host, port);
            }
            return DemoHostLinkStateChange(host, port);
        }

        public TradeMagic.Pats.ErrorCode Initialise(byte env, string apiVersion, string appId, string appVersion, string license)
        {
            if (!this.isDemo)
            {
                return LiveInitialise(env, apiVersion, appId, appVersion, license);
            }
            return DemoInitialise(env, apiVersion, appId, appVersion, license);
        }

        internal static bool IsEqual(char[] buf1, char[] buf2)
        {
            for (int i = 0; i < buf1.Length; i++)
            {
                if (i >= buf2.Length)
                {
                    return false;
                }
                if (buf1[i] != buf2[i])
                {
                    return false;
                }
                if (buf1[i] == '\0')
                {
                    return true;
                }
            }
            return true;
        }

        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\patsapi.dll", EntryPoint="ptAcknowledgeUsrMsg")]
        private static extern TradeMagic.Pats.ErrorCode LiveAcknowledgeUsrMsg(string msgId);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\patsapi.dll", EntryPoint="ptAddOrder")]
        private static extern TradeMagic.Pats.ErrorCode LiveAddOrder(ref Order order, [MarshalAs(UnmanagedType.LPStr)] StringBuilder orderId);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\patsapi.dll", EntryPoint="ptAmendOrder")]
        private static extern TradeMagic.Pats.ErrorCode LiveAmendOrder(string orderId, ref AmendOrderStruct amendOrder);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\patsapi.dll", EntryPoint="ptBuyingPowerRemaining")]
        private static extern TradeMagic.Pats.ErrorCode LiveBuyingPowerRemaining(string exchangeName, string contractName, string contractDate, string traderAccount, [MarshalAs(UnmanagedType.LPStr)] StringBuilder bpRemaining);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\patsapi.dll", EntryPoint="ptCancelOrder")]
        private static extern TradeMagic.Pats.ErrorCode LiveCancelOrder(string orderId);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\patsapi.dll", EntryPoint="ptCountContracts")]
        private static extern TradeMagic.Pats.ErrorCode LiveCountContracts(out int count);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\patsapi.dll", EntryPoint="ptCountExchanges")]
        private static extern TradeMagic.Pats.ErrorCode LiveCountExchanges(out int count);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\patsapi.dll", EntryPoint="ptCountFills")]
        private static extern TradeMagic.Pats.ErrorCode LiveCountFills(out int count);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\patsapi.dll", EntryPoint="ptCountOrderHistory")]
        private static extern TradeMagic.Pats.ErrorCode LiveCountOrderHistory(int orderIdx, out int count);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\patsapi.dll", EntryPoint="ptCountOrders")]
        private static extern TradeMagic.Pats.ErrorCode LiveCountOrders(out int count);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\patsapi.dll", EntryPoint="ptCountOrderTypes")]
        private static extern TradeMagic.Pats.ErrorCode LiveCountOrderTypes(out int count);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\patsapi.dll", EntryPoint="ptCountTraders")]
        private static extern TradeMagic.Pats.ErrorCode LiveCountTraders(out int count);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\patsapi.dll", EntryPoint="ptDisconnect")]
        private static extern TradeMagic.Pats.ErrorCode LiveDisconnect();
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\patsapi.dll", EntryPoint="ptEnable")]
        private static extern void LiveEnable(int options);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\patsapi.dll", EntryPoint="ptEnabledFunctionality")]
        private static extern TradeMagic.Pats.ErrorCode LiveEnabledFunctionality(out int functionality, out int software);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\patsapi.dll", EntryPoint="ptGetCommodityByName")]
        private static extern TradeMagic.Pats.ErrorCode LiveGetCommodityByName(string exchangeName, string contractName, out Commodity commodity);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\patsapi.dll", EntryPoint="ptGetContract")]
        private static extern TradeMagic.Pats.ErrorCode LiveGetContract(int count, out Contract contract);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\patsapi.dll", EntryPoint="ptGetContractByName")]
        private static extern TradeMagic.Pats.ErrorCode LiveGetContractByName(string exchangeName, string contractName, string contractDate, out Contract contract);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\patsapi.dll", EntryPoint="ptGetContractPosition")]
        private static extern TradeMagic.Pats.ErrorCode LiveGetContractPosition(string exchangeName, string contractName, string contractDate, string traderAccount, out PositionStruct positionStruct);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\patsapi.dll", EntryPoint="ptGetExchange")]
        private static extern TradeMagic.Pats.ErrorCode LiveGetExchange(int count, out Exchange exchange);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\patsapi.dll", EntryPoint="ptGetFill")]
        private static extern TradeMagic.Pats.ErrorCode LiveGetFill(int count, out Fill fill);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\patsapi.dll", EntryPoint="ptGetFillByID")]
        private static extern TradeMagic.Pats.ErrorCode LiveGetFillByID(string fillId, out Fill fill);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\patsapi.dll", EntryPoint="ptGetLogonStatus")]
        private static extern TradeMagic.Pats.ErrorCode LiveGetLogonStatus(out LogonStatus status);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\patsapi.dll", EntryPoint="ptGetOpenPosition")]
        private static extern TradeMagic.Pats.ErrorCode LiveGetOpenPosition(string exchangeName, string contractName, string contractDate, string traderAccount, out PositionStruct positionStruct);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\patsapi.dll", EntryPoint="ptGetOrder")]
        private static extern TradeMagic.Pats.ErrorCode LiveGetOrder(int count, out OrderDetail orderDetail);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\patsapi.dll", EntryPoint="ptGetOrderByID")]
        private static extern TradeMagic.Pats.ErrorCode LiveGetOrderByID(string orderId, out OrderDetail orderDetail);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\patsapi.dll", EntryPoint="ptGetOrderHistory")]
        private static extern TradeMagic.Pats.ErrorCode LiveGetOrderHistory(int orderIdx, int pos, out OrderDetail orderDetail);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\patsapi.dll", EntryPoint="ptGetOrderType")]
        private static extern TradeMagic.Pats.ErrorCode LiveGetOrderType(int count, out OrderType orderType);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\patsapi.dll", EntryPoint="ptGetPrice")]
        private static extern TradeMagic.Pats.ErrorCode LiveGetPrice(int count, [Out] byte[] buf);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\patsapi.dll", EntryPoint="ptGetTotalPosition")]
        private static extern TradeMagic.Pats.ErrorCode LiveGetTotalPosition(string traderAccount, out PositionStruct positionStruct);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\patsapi.dll", EntryPoint="ptGetTrader")]
        private static extern TradeMagic.Pats.ErrorCode LiveGetTrader(int count, out TraderAccount traderAccount);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\patsapi.dll", EntryPoint="ptGetUsrMsgByID")]
        private static extern TradeMagic.Pats.ErrorCode LiveGetUsrMsgByID(string msgId, out Message msg);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\patsapi.dll", EntryPoint="ptHostLinkStateChange")]
        private static extern TradeMagic.Pats.ErrorCode LiveHostLinkStateChange(string host, string port);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\patsapi.dll", EntryPoint="ptInitialise")]
        private static extern TradeMagic.Pats.ErrorCode LiveInitialise(byte env, string apiVersion, string appId, string appVersion, string license);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\patsapi.dll", EntryPoint="ptLogOff")]
        private static extern TradeMagic.Pats.ErrorCode LiveLogOff();
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\patsapi.dll", EntryPoint="ptLogOn")]
        private static extern TradeMagic.Pats.ErrorCode LiveLogOn(ref Logon logon);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\patsapi.dll", EntryPoint="ptReady")]
        private static extern TradeMagic.Pats.ErrorCode LiveReady();
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\patsapi.dll", EntryPoint="ptRegisterCallback")]
        private static extern TradeMagic.Pats.ErrorCode LiveRegisterCallback(int callbackId, PatsCallback callback);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\patsapi.dll", EntryPoint="ptRegisterContractCallback")]
        private static extern TradeMagic.Pats.ErrorCode LiveRegisterContractCallback(int callbackId, ContractCallback callback);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\patsapi.dll", EntryPoint="ptRegisterFillCallback")]
        private static extern TradeMagic.Pats.ErrorCode LiveRegisterFillCallback(int callbackId, FillCallback callback);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\patsapi.dll", EntryPoint="ptRegisterLinkStateCallback")]
        private static extern TradeMagic.Pats.ErrorCode LiveRegisterLinkStateCallback(int callbackId, LinkCallback callback);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\patsapi.dll", EntryPoint="ptRegisterMsgCallback")]
        private static extern TradeMagic.Pats.ErrorCode LiveRegisterMsgCallback(int callbackId, MsgCallback callback);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\patsapi.dll", EntryPoint="ptRegisterOrderCallback")]
        private static extern TradeMagic.Pats.ErrorCode LiveRegisterOrderCallback(int callbackId, OrderCallback callback);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\patsapi.dll", EntryPoint="ptRegisterPriceCallback")]
        private static extern TradeMagic.Pats.ErrorCode LiveRegisterPriceCallback(int callbackId, PriceUpdateCallback callback);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\patsapi.dll", EntryPoint="ptSetClientPath")]
        private static extern void LiveSetClientPath(string clientPath);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\patsapi.dll", EntryPoint="ptSetHostAddress")]
        private static extern TradeMagic.Pats.ErrorCode LiveSetHostAddress(string host, string port);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\patsapi.dll", EntryPoint="ptSetHostHandShake")]
        private static extern TradeMagic.Pats.ErrorCode LiveSetHostHandShake(int interval, int timeout);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\patsapi.dll", EntryPoint="ptSetInternetUser")]
        private static extern TradeMagic.Pats.ErrorCode LiveSetInternetUser(string yesNo);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\patsapi.dll", EntryPoint="ptSetPriceAddress")]
        private static extern TradeMagic.Pats.ErrorCode LiveSetPriceAddress(string host, string port);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\patsapi.dll", EntryPoint="ptSetPriceAgeCounter")]
        private static extern TradeMagic.Pats.ErrorCode LiveSetPriceAgeCounter(int age);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\patsapi.dll", EntryPoint="ptSetPriceHandShake")]
        private static extern TradeMagic.Pats.ErrorCode LiveSetPriceHandShake(int interval, int timeout);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\patsapi.dll", EntryPoint="ptSetSuperTAS")]
        private static extern TradeMagic.Pats.ErrorCode LiveSetSuperTAS(char enable);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\patsapi.dll", EntryPoint="ptSubscribePrice")]
        private static extern TradeMagic.Pats.ErrorCode LiveSubscribePrice(string exchangeName, string contractName, string contractDate);
        [SuppressUnmanagedCodeSecurity, DllImport(@"Pats\patsapi.dll", EntryPoint="ptUnsubscribePrice")]
        private static extern TradeMagic.Pats.ErrorCode LiveUnsubscribePrice(string exchangeName, string contractName, string contractDate);
        public TradeMagic.Pats.ErrorCode LogOff()
        {
            if (!this.isDemo)
            {
                return LiveLogOff();
            }
            return DemoLogOff();
        }

        public TradeMagic.Pats.ErrorCode LogOn(ref Logon logon)
        {
            if (!this.isDemo)
            {
                return LiveLogOn(ref logon);
            }
            return DemoLogOn(ref logon);
        }

        internal static void PriceToChars(IFormatProvider formatProvider, iTrading.Core.Kernel.Symbol symbol, double price, char[] chars)
        {
            Trace.Assert(price >= 0.0, "Pats.PatsApi.PriceToChars");
            string str = "";
            if (symbol.TickSize == Globals.TickSize8)
            {
                double num = Math.Floor(price);
                double num2 = (price - num) / Globals.TickSize8;
                str = num.ToString() + ((num2 == 0.0) ? "" : ("." + num2.ToString("0")));
            }
            else if (symbol.TickSize == Globals.TickSize32)
            {
                double num3 = Math.Floor(price);
                double num4 = (price - num3) / Globals.TickSize32;
                str = num3.ToString() + ((num4 == 0.0) ? "" : ("." + num4.ToString("00")));
            }
            else if (symbol.TickSize == Globals.TickSize64)
            {
                double num5 = Math.Floor(price);
                double num6 = (10.0 * (price - num5)) / Globals.TickSize32;
                str = num5.ToString() + ((num6 == 0.0) ? "" : ("." + num6.ToString("000")));
            }
            else
            {
                str = price.ToString(formatProvider);
            }
            str.CopyTo(0, chars, 0, str.Length);
        }

        public TradeMagic.Pats.ErrorCode Ready()
        {
            if (!this.isDemo)
            {
                return LiveReady();
            }
            return DemoReady();
        }

        public TradeMagic.Pats.ErrorCode RegisterCallback(int callbackId, PatsCallback callback)
        {
            if (!this.isDemo)
            {
                return LiveRegisterCallback(callbackId, callback);
            }
            return DemoRegisterCallback(callbackId, callback);
        }

        public TradeMagic.Pats.ErrorCode RegisterContractCallback(int callbackId, ContractCallback callback)
        {
            if (!this.isDemo)
            {
                return LiveRegisterContractCallback(callbackId, callback);
            }
            return DemoRegisterContractCallback(callbackId, callback);
        }

        public TradeMagic.Pats.ErrorCode RegisterFillCallback(int callbackId, FillCallback callback)
        {
            if (!this.isDemo)
            {
                return LiveRegisterFillCallback(callbackId, callback);
            }
            return DemoRegisterFillCallback(callbackId, callback);
        }

        public TradeMagic.Pats.ErrorCode RegisterLinkStateCallback(int callbackId, LinkCallback callback)
        {
            if (!this.isDemo)
            {
                return LiveRegisterLinkStateCallback(callbackId, callback);
            }
            return DemoRegisterLinkStateCallback(callbackId, callback);
        }

        public TradeMagic.Pats.ErrorCode RegisterMsgCallback(int callbackId, MsgCallback callback)
        {
            if (!this.isDemo)
            {
                return LiveRegisterMsgCallback(callbackId, callback);
            }
            return DemoRegisterMsgCallback(callbackId, callback);
        }

        public TradeMagic.Pats.ErrorCode RegisterOrderCallback(int callbackId, OrderCallback callback)
        {
            if (!this.isDemo)
            {
                return LiveRegisterOrderCallback(callbackId, callback);
            }
            return DemoRegisterOrderCallback(callbackId, callback);
        }

        public TradeMagic.Pats.ErrorCode RegisterPriceCallback(int callbackId, PriceUpdateCallback callback)
        {
            if (!this.isDemo)
            {
                return LiveRegisterPriceCallback(callbackId, callback);
            }
            return DemoRegisterPriceCallback(callbackId, callback);
        }

        public void SetClientPath(string clientPath)
        {
            if (this.isDemo)
            {
                DemoSetClientPath(clientPath);
            }
            else
            {
                LiveSetClientPath(clientPath);
            }
        }

        public TradeMagic.Pats.ErrorCode SetHostAddress(string host, string port)
        {
            if (!this.isDemo)
            {
                return LiveSetHostAddress(host, port);
            }
            return DemoSetHostAddress(host, port);
        }

        public TradeMagic.Pats.ErrorCode SetHostHandshake(int interval, int timeout)
        {
            if (!this.isDemo)
            {
                return LiveSetHostHandShake(interval, timeout);
            }
            return DemoSetHostHandShake(interval, timeout);
        }

        public TradeMagic.Pats.ErrorCode SetInternetUser(bool yes)
        {
            if (!this.isDemo)
            {
                return LiveSetInternetUser(yes ? "Y" : "N");
            }
            return DemoSetInternetUser(yes ? "Y" : "N");
        }

        public TradeMagic.Pats.ErrorCode SetPriceAddress(string host, string port)
        {
            if (!this.isDemo)
            {
                return LiveSetPriceAddress(host, port);
            }
            return DemoSetPriceAddress(host, port);
        }

        public TradeMagic.Pats.ErrorCode SetPriceAgeCounter(int age)
        {
            if (!this.isDemo)
            {
                return LiveSetPriceAgeCounter(age);
            }
            return DemoSetPriceAgeCounter(age);
        }

        public TradeMagic.Pats.ErrorCode SetPriceHandshake(int interval, int timeout)
        {
            if (!this.isDemo)
            {
                return LiveSetPriceHandShake(interval, timeout);
            }
            return DemoSetPriceHandShake(interval, timeout);
        }

        public TradeMagic.Pats.ErrorCode SetSuperTAS(char enable)
        {
            if (!this.isDemo)
            {
                return LiveSetSuperTAS(enable);
            }
            return DemoSetSuperTAS(enable);
        }

        public TradeMagic.Pats.ErrorCode SubscribePrice(string exchangeName, string contractName, string contractDate)
        {
            if (!this.isDemo)
            {
                return LiveSubscribePrice(exchangeName, contractName, contractDate);
            }
            return DemoSubscribePrice(exchangeName, contractName, contractDate);
        }

        internal static string ToApiDate(DateTime date)
        {
            return date.ToString("MMMyy", CultureInfo.CreateSpecificCulture("en-US").DateTimeFormat);
        }

        internal static DateTime ToDateTime(char[] date, char[] time, Connection connection)
        {
            string str = ToString(date);
            string str2 = ToString(time);
            try
            {
                return new DateTime(Convert.ToInt32(str.Substring(0, 4)), Convert.ToInt32(str.Substring(4, 2)), Convert.ToInt32(str.Substring(6, 2)), Convert.ToInt32(str2.Substring(0, 2)), Convert.ToInt32(str2.Substring(2, 2)), Convert.ToInt32(str2.Substring(4, 2)));
            }
            catch (Exception exception)
            {
                connection.ProcessEventArgs(new ITradingErrorEventArgs(connection, iTrading.Core.Kernel.ErrorCode.Panic, "", exception.Message + " " + str + ":" + str2));
                return Globals.MaxDate;
            }
        }

        internal static double ToDouble(char[] array, IFormatProvider format)
        {
            double num = 0.0;
            try
            {
                num = Convert.ToDouble(ToString(array), format);
            }
            catch (Exception exception)
            {
                if (Adapter.currentAdapter != null)
                {
                    Adapter.currentAdapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(Adapter.currentAdapter.connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Pats.PatsApi: unable to convert '" + ToString(array) + "' to double: " + exception.Message));
                    return num;
                }
                Trace.Assert(false, "Pats.PatsApi.ToDouble: unable to convert '" + ToString(array) + "' to double: " + exception.Message);
            }
            return num;
        }

        internal static DateTime ToExpiryDate(string expiry)
        {
            int month = 0;
            if (expiry.Substring(0, 3).ToUpper() == "JAN")
            {
                month = 1;
            }
            else if (expiry.Substring(0, 3).ToUpper() == "FEB")
            {
                month = 2;
            }
            else if (expiry.Substring(0, 3).ToUpper() == "MAR")
            {
                month = 3;
            }
            else if (expiry.Substring(0, 3).ToUpper() == "APR")
            {
                month = 4;
            }
            else if (expiry.Substring(0, 3).ToUpper() == "MAY")
            {
                month = 5;
            }
            else if (expiry.Substring(0, 3).ToUpper() == "JUN")
            {
                month = 6;
            }
            else if (expiry.Substring(0, 3).ToUpper() == "JUL")
            {
                month = 7;
            }
            else if (expiry.Substring(0, 3).ToUpper() == "AUG")
            {
                month = 8;
            }
            else if (expiry.Substring(0, 3).ToUpper() == "SEP")
            {
                month = 9;
            }
            else if (expiry.Substring(0, 3).ToUpper() == "OCT")
            {
                month = 10;
            }
            else if (expiry.Substring(0, 3).ToUpper() == "NOV")
            {
                month = 11;
            }
            else if (expiry.Substring(0, 3).ToUpper() == "DEC")
            {
                month = 12;
            }
            return new DateTime(Convert.ToInt32("20" + expiry.Substring(3, 2)), month, 1);
        }

        internal static uint ToInt(byte[] array, int offset)
        {
            return (uint) BitConverter.ToInt32(array, offset);
        }

        internal static string ToString(char[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == '\0')
                {
                    return new string(array, 0, i);
                }
            }
            return new string(array);
        }

        public TradeMagic.Pats.ErrorCode UnsubscribePrice(string exchangeName, string contractName, string contractDate)
        {
            if (!this.isDemo)
            {
                return LiveUnsubscribePrice(exchangeName, contractName, contractDate);
            }
            return DemoUnsubscribePrice(exchangeName, contractName, contractDate);
        }

        [StructLayout(LayoutKind.Sequential, Pack=1)]
        internal struct AmendOrderStruct
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=0x15)]
            public char[] price;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=0x15)]
            public char[] price2;
            public int lots;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=11)]
            public char[] linkedOrder;
            public char openOrClose;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=0x15)]
            public char[] trader;
        }

        [StructLayout(LayoutKind.Sequential, Pack=1)]
        internal struct Commodity
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=11)]
            public char[] exchangeName;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=11)]
            public char[] contractName;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=11)]
            public char[] currency;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=11)]
            public char[] group;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=11)]
            public char[] onePoint;
            public int ticksPerPoint;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=11)]
            public char[] tickSize;
        }

        [StructLayout(LayoutKind.Sequential, Pack=1)]
        internal struct Contract
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=11)]
            public char[] contractName;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=0x33)]
            public char[] contractDate;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=11)]
            public char[] exchangeName;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=9)]
            public char[] expiryDate;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=9)]
            public char[] lastTradeDate;
            public int numberOfLegs;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=11)]
            public char[] leg0ContractType;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=11)]
            public char[] leg0ExternalName;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=11)]
            public char[] leg0Maturity;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=11)]
            public char[] leg0StricePrice;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=11)]
            public char[] leg0Identity;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=11)]
            public char[] leg1ContractType;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=11)]
            public char[] leg1ExternalName;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=11)]
            public char[] leg1Maturity;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=11)]
            public char[] leg1StricePrice;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=11)]
            public char[] leg1Identity;
        }

        internal delegate void ContractCallback(ref PatsApi.ContractUpdate update);

        [StructLayout(LayoutKind.Sequential, Pack=1)]
        internal struct ContractUpdate
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=11)]
            public char[] exchangeName;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=11)]
            public char[] contractName;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=0x33)]
            public char[] contractDate;
            public ContractUpdate(PatsApi.Contract contract)
            {
                this.contractDate = new char[0x33];
                this.contractName = new char[11];
                this.exchangeName = new char[11];
                contract.contractDate.CopyTo(this.contractDate, 0);
                contract.contractName.CopyTo(this.contractName, 0);
                contract.exchangeName.CopyTo(this.exchangeName, 0);
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack=1)]
        internal struct Exchange
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=11)]
            public char[] name;
            public char queryEnabled;
            public char amendEnabled;
            public int strategy;
        }

        [StructLayout(LayoutKind.Sequential, Pack=1)]
        internal struct Fill
        {
            public int index;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=0x47)]
            public char[] fillId;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=11)]
            public char[] exchangeName;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=11)]
            public char[] contractName;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=0x33)]
            public char[] contractDate;
            public char buyOrSell;
            public int lots;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=0x15)]
            public char[] price;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=11)]
            public char[] orderId;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=9)]
            public char[] dateFilled;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=7)]
            public char[] timeFilled;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=9)]
            public char[] dateHostRecd;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=7)]
            public char[] timeHostRecd;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=0x1f)]
            public char[] exchangeOrderId;
            public byte fillType;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=0x15)]
            public char[] traderAccount;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=11)]
            public char[] userName;
        }

        internal delegate void FillCallback(ref PatsApi.FillUpdate update);

        [StructLayout(LayoutKind.Sequential, Pack=1)]
        internal struct FillUpdate
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=11)]
            public char[] orderId;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=0x47)]
            public char[] fillId;
        }

        internal delegate void LinkCallback(ref PatsApi.LinkState state);

        [StructLayout(LayoutKind.Sequential, Pack=1)]
        internal struct LinkState
        {
            public byte oldState;
            public byte newState;
        }

        [StructLayout(LayoutKind.Sequential, Pack=1)]
        internal struct Logon
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=11)]
            public char[] userId;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=11)]
            public char[] password;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=11)]
            public char[] newpassword;
            public char reset;
            public char reports;
            public Logon(string user, string pwd, string newPwd)
            {
                this.password = new char[11];
                this.newpassword = new char[11];
                this.userId = new char[11];
                pwd.CopyTo(0, this.password, 0, pwd.Length);
                user.CopyTo(0, this.userId, 0, user.Length);
                this.reset = 'Y';
                this.reports = 'Y';
                if (newPwd.Length > 0)
                {
                    newPwd.CopyTo(0, this.newpassword, 0, newPwd.Length);
                }
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack=1)]
        internal struct LogonStatus
        {
            public byte status;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=0x3d)]
            public char[] reason;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=0x15)]
            public char[] defaultTraderAccount;
            public char showMessage;
        }

        [StructLayout(LayoutKind.Sequential, Pack=1)]
        internal struct Message
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=11)]
            public char[] msgId;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=0x3d)]
            public char[] msgText;
            public char isAlert;
            public char status;
        }

        internal delegate void MsgCallback([MarshalAs(UnmanagedType.LPStr)] string msg);

        [StructLayout(LayoutKind.Sequential, Pack=1)]
        internal struct Order
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=0x15)]
            public char[] traderAccount;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=11)]
            public char[] orderType;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=11)]
            public char[] exchangeName;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=11)]
            public char[] contractName;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=0x33)]
            public char[] contractDate;
            public char buyOrSell;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=0x15)]
            public char[] price;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=0x15)]
            public char[] price2;
            public int lots;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=11)]
            public char[] linkedOrder;
            public char openOrClose;
            public int xref;
            public int xrefP;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=9)]
            public char[] goodTillDate;
            public char triggerNow;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=0x1a)]
            public char[] reference;
        }

        internal delegate void OrderCallback(ref PatsApi.OrderUpdate update);

        [StructLayout(LayoutKind.Sequential, Pack=1)]
        internal struct OrderDetail
        {
            public int index;
            public char historic;
            public char isChecked;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=11)]
            public char[] orderId;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=11)]
            public char[] displayId;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=0x1f)]
            public char[] exchangeOrderId;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=11)]
            public char[] userName;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=0x15)]
            public char[] traderAccount;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=11)]
            public char[] orderType;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=11)]
            public char[] exchangeName;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=11)]
            public char[] contractName;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=0x33)]
            public char[] contractDate;
            public char buyOrSell;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=0x15)]
            public char[] price;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=0x15)]
            public char[] price2;
            public int lots;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=11)]
            public char[] linkedOrder;
            public int amountFilled;
            public int noOfFills;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=0x15)]
            public char[] averagePrice;
            public byte status;
            public char openOrClose;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=9)]
            public char[] dateSent;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=7)]
            public char[] timeSent;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=9)]
            public char[] dateHostRecd;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=7)]
            public char[] timeHostRecd;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=9)]
            public char[] dateExchRecd;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=7)]
            public char[] timeExchRecd;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=9)]
            public char[] dateExchAckn;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=7)]
            public char[] timeExchAckn;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=0x3d)]
            public char[] nonExecReason;
            public int xref;
            public int xrefP;
            public int updateSeq;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=9)]
            public char[] goodTillDate;
        }

        [StructLayout(LayoutKind.Sequential, Pack=1)]
        internal struct OrderType
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=11)]
            public char[] orderType;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=11)]
            public char[] exchangeName;
            public int orderTypeId;
            public byte numPricesReqd;
            public byte numVolumesReqd;
            public byte numDatesReqd;
            public char autoCreated;
        }

        [StructLayout(LayoutKind.Sequential, Pack=1)]
        internal struct OrderUpdate
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=11)]
            public char[] orderId;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=11)]
            public char[] oldOrderId;
        }

        internal delegate void PatsCallback();

        [StructLayout(LayoutKind.Sequential, Pack=1)]
        internal struct PositionStruct
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=0x15)]
            public char[] profit;
            public int buys;
            public int sells;
        }

        internal class Price
        {
            private Adapter adapter;
            private byte[] buf;
            private iTrading.Core.Kernel.Symbol symbol;

            public Price(Adapter adapter, iTrading.Core.Kernel.Symbol symbol, byte[] buf)
            {
                this.adapter = adapter;
                this.buf = buf;
                this.symbol = symbol;
            }

            public PatsApi.PriceDetailed GetBidDOM(int idx)
            {
                return PatsApi.PriceDetailed.ReadFromBytes(this.adapter, this.symbol, this.buf, (idx + 30) * 30);
            }

            public PatsApi.PriceDetailed GetLast(int idx)
            {
                return PatsApi.PriceDetailed.ReadFromBytes(this.adapter, this.symbol, this.buf, (idx + 5) * 30);
            }

            public PatsApi.PriceDetailed GetOfferDOM(int idx)
            {
                return PatsApi.PriceDetailed.ReadFromBytes(this.adapter, this.symbol, this.buf, (idx + 50) * 30);
            }

            public PatsApi.PriceDetailed Bid
            {
                get
                {
                    return PatsApi.PriceDetailed.ReadFromBytes(this.adapter, this.symbol, this.buf, 0);
                }
            }

            public uint ChangeMask
            {
                get
                {
                    return PatsApi.ToInt(this.buf, 0x838);
                }
            }

            public PatsApi.PriceDetailed Closing
            {
                get
                {
                    return PatsApi.PriceDetailed.ReadFromBytes(this.adapter, this.symbol, this.buf, 870);
                }
            }

            public PatsApi.PriceDetailed High
            {
                get
                {
                    return PatsApi.PriceDetailed.ReadFromBytes(this.adapter, this.symbol, this.buf, 780);
                }
            }

            public PatsApi.PriceDetailed Low
            {
                get
                {
                    return PatsApi.PriceDetailed.ReadFromBytes(this.adapter, this.symbol, this.buf, 810);
                }
            }

            public PatsApi.PriceDetailed Offer
            {
                get
                {
                    return PatsApi.PriceDetailed.ReadFromBytes(this.adapter, this.symbol, this.buf, 30);
                }
            }

            public PatsApi.PriceDetailed Opening
            {
                get
                {
                    return PatsApi.PriceDetailed.ReadFromBytes(this.adapter, this.symbol, this.buf, 840);
                }
            }

            public PatsApi.PriceDetailed Total
            {
                get
                {
                    return PatsApi.PriceDetailed.ReadFromBytes(this.adapter, this.symbol, this.buf, 750);
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct PriceDetailed
        {
            public double price;
            public int volume;
            public byte ageCounter;
            public byte direction;
            public byte hour;
            public byte minute;
            public byte second;
            internal static PatsApi.PriceDetailed ReadFromBytes(Adapter adapter, iTrading.Core.Kernel.Symbol symbol, byte[] buffer, int offset)
            {
                PatsApi.PriceDetailed detailed;
                detailed.price = PatsApi.CharsToPrice(adapter.numberFormatInfo, symbol, adapter.asciiEncoding.GetChars(buffer, offset, 20));
                detailed.volume = (int) PatsApi.ToInt(buffer, (offset + 20) + 1);
                detailed.ageCounter = buffer[((offset + 20) + 1) + 4];
                detailed.direction = buffer[((offset + 20) + 1) + 5];
                detailed.hour = buffer[((offset + 20) + 1) + 6];
                detailed.minute = buffer[((offset + 20) + 1) + 7];
                detailed.second = buffer[((offset + 20) + 1) + 8];
                return detailed;
            }
        }

        internal delegate void PriceUpdateCallback(ref PatsApi.ContractUpdate update);

        [StructLayout(LayoutKind.Sequential, Pack=1)]
        internal struct TraderAccount
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=0x15)]
            public char[] traderAccount;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=0x15)]
            public char[] backOfficeID;
            public char tradable;
            public int lossLimit;
        }
    }
}

