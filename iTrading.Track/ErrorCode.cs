namespace iTrading.Track
{
    using System;

    internal enum ErrorCode
    {
        AlreadyCalled = -23,
        AlreadyLoggedOn = -13,
        ArgLength = -14,
        DbError = 13,
        DbNotAvialable = 11,
        DeInitCalled = -22,
        Disconnect = -18,
        File = 0x27,
        FindIpAddr = -5,
        InitNotCalled = -10,
        InMsgTooLarge = -17,
        InputThread = -7,
        InvalidAccountId = -21,
        InvalidAcct = 5,
        InvalidArgument = -20,
        InvalidProductName = -24,
        InvalidRequest = 0x10,
        InvalidRqn = 20,
        InvalidVersion = 0x16,
        LogonAlready = 9,
        NoActivity = 12,
        NoClientHeartBeat = -19,
        NoComposite = 0x21,
        NoConnect = -6,
        NoData = 14,
        NoError = 0,
        NoFutures = 0x20,
        NoHeartBeat = 4,
        NoMalloc = -3,
        NoMessage = -12,
        NoOptions = 0x1f,
        NoOtcBb = 0x22,
        NoPriorRequest = 15,
        NoQueues = -2,
        NoRoom = 1,
        NoSdkFeature = 0x2a,
        NoSdkRegistration = 0x2b,
        NoSocket = -4,
        NoSuchTicker = 7,
        NotConnected = -11,
        NotLogon = 8,
        NotOutputQueueLinks = -15,
        NoTrade = 0x23,
        OutMsgTooLarge = -16,
        OutputThread = -8,
        Profile = 30,
        TdsNotAvailable = 0x19,
        TimerThread = -9,
        TooManyConcurrent = 0x15,
        TooManyConnections = 3,
        TooManyLines = 0x12,
        TooManyRequests = 10,
        TunnelClose = -27,
        TunnelDestroy = -28,
        TunnelShutdown = -26,
        TunnelStartup = -25,
        Unknown = -33,
        UnknownConnection = 2,
        UnknownRequest = 0x11,
        UnknowRqn = 0x13,
        Warehouse = 0x1b,
        WarehouseGet = 0x1a,
        WarehouseNotAvailable = 0x17,
        WarehousePut = 0x1c,
        WsaStartup = -1
    }
}
