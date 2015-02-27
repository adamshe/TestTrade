namespace iTrading.IB
{
    using System;

    internal enum MessageId
    {
        AccountUpdate = 6,
        AccountUpdateTime = 8,
        ContractData = 10,
        ErrMsg = 4,
        ExecutionData = 11,
        ManagedAccts = 15,
        MarketDepth = 12,
        MarketDepth2 = 13,
        NewBulletins = 14,
        NextValidId = 9,
        OpenOrders = 5,
        OrderStatus = 3,
        PortfolioUpdate = 7,
        ReceiveFa = 0x10,
        TickPrice = 1,
        TickSize = 2
    }
}

