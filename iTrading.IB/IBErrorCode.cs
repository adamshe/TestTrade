namespace iTrading.IB
{
    using System;

    internal enum IBErrorCode
    {
        CantFindEid = 300,
        CantFindMarketDepth = 310,
        ConnectionLost = 0x44c,
        ConnectionLostAndMaintained = 0x44e,
        ConnectionLostAndRestored = 0x44d,
        NoSuchSecurity = 200,
        OrderCancelled = 0xca,
        OrderRejected = 0xc9,
        PriceVariation = 110,
        Rescubscribe2MarketDepth = 0x13c,
        ResetMarketDepthRows = 0x13d,
        Resubscribe2AccountData = 0x834,
        ServerValidateError = 0x141,
        SizeMatchAllocation = 0x90
    }
}

