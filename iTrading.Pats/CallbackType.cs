namespace TradeMagic.Pats
{
    using System;

    internal enum CallbackType
    {
        AtBestUpdate = 0x10,
        ConnectivityStatus = 14,
        ContractAdded = 11,
        ContractDeleted = 12,
        DataDLComplete = 7,
        ExchangeRate = 13,
        Fill = 9,
        ForcedLogout = 6,
        HostLinkStateChange = 1,
        LogonStatus = 3,
        MemoryWarning = 0x12,
        Message = 4,
        Order = 5,
        OrderCancelFailure = 15,
        PriceLinkStateChange = 2,
        PriceUpdate = 8,
        StatusChange = 10,
        SubscriberDepthUpdate = 0x13,
        TickerUpdate = 0x11
    }
}

