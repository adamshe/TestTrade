namespace iTrading.Track
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Security;

    internal class Api
    {
        internal static Adapter adapter;

        internal static int BrokerCancelOrder(short rqn, string account, string orderId)
        {
            return (int) Invoke(new Delegate3(Api.BrokerCancelOrderNow), new object[] { rqn, account, orderId });
        }

        private static object BrokerCancelOrderNow(object rqn, object account, object orderId)
        {
            return mtBrokerCancelOrder((short) rqn, (string) account, (string) orderId);
        }

        internal static int BrokerEnterCondOrder(short rqn, BrokerOrder order1, BrokerOrder order2, int condFlag)
        {
            return (int) Invoke(new Delegate4(Api.BrokerEnterCondOrderNow), new object[] { rqn, order1, order2, condFlag });
        }

        private static object BrokerEnterCondOrderNow(object rqn, object order1, object order2, object condFlag)
        {
            BrokerOrder order = (BrokerOrder) order1;
            BrokerOrder order3 = (BrokerOrder) order2;
            return mtBrokerEnterCondOrder((short) rqn, ref order, ref order3, (int) condFlag);
        }

        internal static int BrokerEnterOrder(short rqn, BrokerOrder brokerOrder)
        {
            return (int) Invoke(new Delegate2(Api.BrokerEnterOrderNow), new object[] { rqn, brokerOrder });
        }

        private static object BrokerEnterOrderNow(object rqn, object brokerOrder)
        {
            BrokerOrder order = (BrokerOrder) brokerOrder;
            return mtBrokerEnterOrder((short) rqn, ref order);
        }

        internal static int BrokerRequestAcctIds(int rqn)
        {
            return (int) Invoke(new Delegate1(Api.BrokerRequestAcctIdsNow), new object[] { rqn });
        }

        private static object BrokerRequestAcctIdsNow(object rqn)
        {
            return mtBrokerRequestAcctIds((int) rqn);
        }

        internal static int BrokerRequestAcctPositions(short rqn, string account, char flag)
        {
            return (int) Invoke(new Delegate3(Api.BrokerRequestAcctPositionsNow), new object[] { rqn, account, flag });
        }

        private static object BrokerRequestAcctPositionsNow(object rqn, object account, object flag)
        {
            return mtBrokerRequestAcctPositions((short) rqn, (string) account, (char) flag);
        }

        internal static int BrokerRequestAcctSummary(int rqn, string account)
        {
            return (int) Invoke(new Delegate2(Api.BrokerRequestAcctSummaryNow), new object[] { rqn, account });
        }

        private static object BrokerRequestAcctSummaryNow(object rqn, object account)
        {
            return mtBrokerRequestAcctSummary((int) rqn, (string) account);
        }

        internal static int BrokerRequestOrder(int rqn, string account, string startDate, string endDate, byte type)
        {
            return (int) Invoke(new Delegate5(Api.BrokerRequestOrderNow), new object[] { rqn, account, startDate, endDate, type });
        }

        private static object BrokerRequestOrderNow(object rqn, object account, object startDate, object endDate, object type)
        {
            return mtBrokerRequestOrder((int) rqn, (string) account, (string) startDate, (string) endDate, (byte) type);
        }

        internal static int BrokerRequestTransactionData(int rqn, string account, string symbol, string startDate, string endDate)
        {
            return (int) Invoke(new Delegate5(Api.BrokerRequestTransactionDataNow), new object[] { rqn, account, symbol, startDate, endDate });
        }

        private static object BrokerRequestTransactionDataNow(object rqn, object account, object symbol, object startDate, object endDate)
        {
            return mtBrokerRequestTransactionData((int) rqn, (string) account, (string) symbol, (string) startDate, (string) endDate);
        }

        internal static int BrokerUseContest()
        {
            return (int) Invoke(new Delegate0(Api.BrokerUseContestNow), new object[0]);
        }

        private static object BrokerUseContestNow()
        {
            return mtBrokerUseContest();
        }

        internal static int BrokerUseRealAcct()
        {
            return (int) Invoke(new Delegate0(Api.BrokerUseRealAcctNow), new object[0]);
        }

        private static object BrokerUseRealAcctNow()
        {
            return mtBrokerUseRealAcct();
        }

        internal static int ConnectToServer()
        {
            return (int) Invoke(new Delegate0(Api.ConnectToServerNow), new object[0]);
        }

        internal static object ConnectToServerNow()
        {
            return mtConnectToServer();
        }

        internal static int DeInit()
        {
            return (int) Invoke(new Delegate0(Api.DeInitNow), new object[0]);
        }

        private static object DeInitNow()
        {
            return mtDeInit();
        }

        internal static int Disconnect()
        {
            return (int) Invoke(new Delegate0(Api.DisconnectNow), new object[0]);
        }

        private static object DisconnectNow()
        {
            return mtDisconnect();
        }

        [SuppressUnmanagedCodeSecurity, DllImport("Kernel32")]
        internal static extern bool FreeLibrary(int handle);
        internal static int GetMessage(byte[] msgBytes, int flag)
        {
            return mtGetMessage(msgBytes, flag);
        }

        internal static int Init()
        {
            return (int) Invoke(new Delegate0(Api.InitNow), new object[0]);
        }

        private static object InitNow()
        {
            return mtInit();
        }

        internal static object Invoke(Delegate method, object[] args)
        {
            return method.Method.Invoke(method, args);
        }

        internal static bool IsConnected()
        {
            return (bool) Invoke(new Delegate0(Api.IsConnectedNow), new object[0]);
        }

        private static object IsConnectedNow()
        {
            return mtIsConnected();
        }

        [SuppressUnmanagedCodeSecurity, DllImport("Kernel32")]
        internal static extern int LoadLibrary(string dllName);
        [SuppressUnmanagedCodeSecurity, DllImport("mytrackDLL.dll")]
        private static extern int mtBrokerCancelOrder(short rqn, string account, string orderId);
        [SuppressUnmanagedCodeSecurity, DllImport("mytrackDLL.dll")]
        private static extern int mtBrokerEnterCondOrder(short rqn, ref BrokerOrder order1, ref BrokerOrder order2, int condFlag);
        [SuppressUnmanagedCodeSecurity, DllImport("mytrackDLL.dll")]
        private static extern int mtBrokerEnterOrder(short rqn, ref BrokerOrder brokerOrder);
        [SuppressUnmanagedCodeSecurity, DllImport("mytrackDLL.dll")]
        private static extern int mtBrokerRequestAcctIds(int rqn);
        [SuppressUnmanagedCodeSecurity, DllImport("mytrackDLL.dll")]
        private static extern int mtBrokerRequestAcctPositions(short rqn, string account, char flag);
        [SuppressUnmanagedCodeSecurity, DllImport("mytrackDLL.dll")]
        private static extern int mtBrokerRequestAcctSummary(int rqn, string account);
        [SuppressUnmanagedCodeSecurity, DllImport("mytrackDLL.dll")]
        private static extern int mtBrokerRequestOrder(int rqn, string account, string startDate, string endDate, byte type);
        [SuppressUnmanagedCodeSecurity, DllImport("mytrackDLL.dll")]
        private static extern int mtBrokerRequestTransactionData(int rqn, string account, string symbol, string startDate, string endDate);
        [SuppressUnmanagedCodeSecurity, DllImport("mytrackDLL.dll")]
        private static extern int mtBrokerUseContest();
        [SuppressUnmanagedCodeSecurity, DllImport("mytrackDLL.dll")]
        private static extern int mtBrokerUseRealAcct();
        [SuppressUnmanagedCodeSecurity, DllImport("mytrackDLL.dll")]
        private static extern int mtConnectToServer();
        [SuppressUnmanagedCodeSecurity, DllImport("mytrackDLL.dll")]
        private static extern int mtDeInit();
        [SuppressUnmanagedCodeSecurity, DllImport("mytrackDLL.dll")]
        private static extern int mtDisconnect();
        [SuppressUnmanagedCodeSecurity, DllImport("mytrackDLL.dll")]
        private static extern int mtGetMessage([Out] byte[] msgBytes, int flag);
        [SuppressUnmanagedCodeSecurity, DllImport("mytrackDll.dll")]
        private static extern int mtInit();
        [SuppressUnmanagedCodeSecurity, DllImport("mytrackDll.dll")]
        private static extern bool mtIsConnected();
        [SuppressUnmanagedCodeSecurity, DllImport("mytrackDll.dll")]
        private static extern int mtRequestBackgroundData(int rqn, string symbol, string optionCode);
        [SuppressUnmanagedCodeSecurity, DllImport("mytrackDll.dll")]
        private static extern int mtRequestHistoricalData(int rqn, string symbol, short[] day, short numPkts, short periods);
        [SuppressUnmanagedCodeSecurity, DllImport("mytrackDll.dll")]
        private static extern int mtRequestIntradayData(int rqn, string symbol, byte dateFlag);
        [SuppressUnmanagedCodeSecurity, DllImport("mytrackDll.dll")]
        private static extern int mtRequestIntradayHistory(int rqn, string symbol, byte dateFlag, int type, int lastTickerSeq, int lastControl);
        [SuppressUnmanagedCodeSecurity, DllImport("mytrackDll.dll")]
        private static extern int mtRequestIntradayUpdate(int rqn, string symbol, string optionCode, char flag);
        [SuppressUnmanagedCodeSecurity, DllImport("mytrackDll.dll")]
        private static extern int mtRequestLogoff(int rqn);
        [SuppressUnmanagedCodeSecurity, DllImport("mytrackDLL.dll")]
        private static extern int mtRequestLogon(int rqn, string user, string password, string productName, byte revHigh, byte revLow);
        [SuppressUnmanagedCodeSecurity, DllImport("mytrackDLL.dll")]
        private static extern int mtRequestNasdaqLevel_2(int rqn, char flag, char nol, char showClosed, string symbol);
        [SuppressUnmanagedCodeSecurity, DllImport("mytrackDll.dll")]
        private static extern int mtRequestNewsHeadlines(int rqn, char flag);
        [SuppressUnmanagedCodeSecurity, DllImport("mytrackDLL.dll")]
        private static extern int mtRequestNewsStory(int rqn, string storyNumber);
        [SuppressUnmanagedCodeSecurity, DllImport("mytrackDll.dll")]
        private static extern int mtRequestNewsVendors(int rqn);
        [SuppressUnmanagedCodeSecurity, DllImport("mytrackDLL.dll")]
        private static extern int mtRequestQuoteData(int rqn, string ticker, string optionCode);
        [SuppressUnmanagedCodeSecurity, DllImport("mytrackDLL.dll")]
        private static extern int mtSetHost(string host, int port);
        internal static int RequestBackgroundData(int rqn, string symbol, string optionCode)
        {
            return (int) Invoke(new Delegate3(Api.RequestBackgroundDataNow), new object[] { rqn, symbol, optionCode });
        }

        private static object RequestBackgroundDataNow(object rqn, object symbol, object optionCode)
        {
            return mtRequestBackgroundData((int) rqn, (string) symbol, (string) optionCode);
        }

        internal static int RequestHistoricalData(int rqn, string symbol, short[] day, short numPkts, short periods)
        {
            return (int) Invoke(new Delegate5(Api.RequestHistoricalDataNow), new object[] { rqn, symbol, day, numPkts, periods });
        }

        private static object RequestHistoricalDataNow(object rqn, object symbol, object day, object numPkts, object periods)
        {
            return mtRequestHistoricalData((int) rqn, (string) symbol, (short[]) day, (short) numPkts, (short) periods);
        }

        internal static int RequestIntradayData(int rqn, string symbol, byte dateFlag)
        {
            return (int) Invoke(new Delegate3(Api.RequestIntradayDataNow), new object[] { rqn, symbol, dateFlag });
        }

        private static object RequestIntradayDataNow(object rqn, object symbol, object dateFlag)
        {
            return mtRequestIntradayData((int) rqn, (string) symbol, (byte) dateFlag);
        }

        internal static int RequestIntradayHistory(int rqn, string symbol, byte dateFlag, int type, int lastTickerSeq, int lastControl)
        {
            return (int) Invoke(new Delegate6(Api.RequestIntradayHistoryNow), new object[] { rqn, symbol, dateFlag, type, lastTickerSeq, lastControl });
        }

        private static object RequestIntradayHistoryNow(object rqn, object symbol, object dateFlag, object type, object lastTickerSeq, object lastControl)
        {
            return mtRequestIntradayHistory((int) rqn, (string) symbol, (byte) dateFlag, (int) type, (int) lastTickerSeq, (int) lastControl);
        }

        internal static int RequestIntradayUpdate(int rqn, string symbol, string optionCode, char flag)
        {
            return (int) Invoke(new Delegate4(Api.RequestIntradayUpdateNow), new object[] { rqn, symbol, optionCode, flag });
        }

        private static object RequestIntradayUpdateNow(object rqn, object symbol, object optionCode, object flag)
        {
            return mtRequestIntradayUpdate((int) rqn, (string) symbol, (string) optionCode, (char) flag);
        }

        internal static int RequestLogoff(int rqn)
        {
            return (int) Invoke(new Delegate1(Api.RequestLogoffNow), new object[] { rqn });
        }

        private static object RequestLogoffNow(object rqn)
        {
            return mtRequestLogoff((int) rqn);
        }

        internal static int RequestLogon(int rqn, string user, string password, string productName, byte revHigh, byte revLow)
        {
            return (int) Invoke(new Delegate6(Api.RequestLogonNow), new object[] { rqn, user, password, productName, revHigh, revLow });
        }

        private static object RequestLogonNow(object rqn, object user, object password, object productName, object revHigh, object revLow)
        {
            return mtRequestLogon((int) rqn, (string) user, (string) password, (string) productName, (byte) revHigh, (byte) revLow);
        }

        internal static int RequestNasdaqLevel_2(int rqn, char flag, char nol, char showClosed, string symbol)
        {
            return (int) Invoke(new Delegate5(Api.RequestNasdaqLevel_2Now), new object[] { rqn, flag, nol, showClosed, symbol });
        }

        private static object RequestNasdaqLevel_2Now(object rqn, object flag, object nol, object showClosed, object symbol)
        {
            return mtRequestNasdaqLevel_2((int) rqn, (char) flag, (char) nol, (char) showClosed, (string) symbol);
        }

        internal static int RequestNewsHeadlines(int rqn, char flag)
        {
            return (int) Invoke(new Delegate2(Api.RequestNewsHeadlinesNow), new object[] { rqn, flag });
        }

        private static object RequestNewsHeadlinesNow(object rqn, object flag)
        {
            return mtRequestNewsHeadlines((int) rqn, (char) flag);
        }

        internal static int RequestNewsStory(int rqn, string storyNumber)
        {
            return (int) Invoke(new Delegate2(Api.RequestNewsStoryNow), new object[] { rqn, storyNumber });
        }

        private static object RequestNewsStoryNow(object rqn, object storyNumber)
        {
            return mtRequestNewsStory((int) rqn, (string) storyNumber);
        }

        internal static int RequestNewsVendors(int rqn)
        {
            return (int) Invoke(new Delegate1(Api.RequestNewsVendorsNow), new object[] { rqn });
        }

        private static object RequestNewsVendorsNow(object rqn)
        {
            return mtRequestNewsVendors((int) rqn);
        }

        internal static int RequestQuoteData(int rqn, string ticker, string optionCode)
        {
            return (int) Invoke(new Delegate3(Api.RequestQuoteDataNow), new object[] { rqn, ticker, optionCode });
        }

        private static object RequestQuoteDataNow(object rqn, object ticker, object optionCode)
        {
            return mtRequestQuoteData((int) rqn, (string) ticker, (string) optionCode);
        }

        internal static int SetHost(string host, int port)
        {
            return (int) Invoke(new Delegate2(Api.SetHostNow), new object[] { host, port });
        }

        private static object SetHostNow(object host, object port)
        {
            return mtSetHost((string) host, (int) port);
        }

        [StructLayout(LayoutKind.Sequential, Pack=1)]
        internal struct BrokerOrder
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=4)]
            public char[] turnaround;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=0x10)]
            public char[] acct;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=10)]
            public char[] ticker;
            public char actionCode;
            public char instrumentType;
            public char fillKill;
            public char AllNone;
            public char orderType;
            public char dayGTC;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=4)]
            public char[] marketMaker;
            public int orderQty;
            public float limitPrice;
            public float stopPrice;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=12)]
            public char[] spare1;
            public ushort ecnMap;
            public ushort excludeMap;
            public char mmListed;
            public char mmOption;
            public char editOverride;
            public char origin;
            public char optionMarket;
            public char condInd;
            public char mmNasdaq;
            public char mmOtc;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=0x10)]
            public char[] orderId;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=3)]
            public short[] orderDate;
            public short spare2;
            public int orderTime;
            public int filledQty;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=12)]
            public char[] spare3;
            internal void Init()
            {
                this.turnaround = new char[4];
                this.acct = new char[0x10];
                this.ticker = new char[10];
                this.marketMaker = new char[4];
                this.spare1 = new char[12];
                this.orderId = new char[0x10];
                this.orderDate = new short[3];
                this.spare3 = new char[12];
                for (int i = 0; i < this.turnaround.Length; i++)
                {
                    this.turnaround[i] = '\0';
                }
                for (int j = 0; j < this.acct.Length; j++)
                {
                    this.acct[j] = ' ';
                }
            }
        }

        private delegate object Delegate0();

        private delegate object Delegate1(object args1);

        private delegate object Delegate2(object args1, object args2);

        private delegate object Delegate3(object args1, object args2, object args3);

        private delegate object Delegate4(object args1, object args2, object args3, object args4);

        private delegate object Delegate5(object args1, object args2, object args3, object args4, object args5);

        private delegate object Delegate6(object args1, object args2, object args3, object args4, object args5, object args6);
    }
}

