namespace iTrading.Mbt
{
    using System;

    internal enum OrderType
    {
        Limit = 0x272e,
        Market = 0x272f,
        Normal = 0x273a,
        StopLimit = 0x2731,
        StopMarket = 0x2730
    }
}

