namespace iTrading.CT
{
    using System;

    internal enum Level1
    {
        Ask = 0x20,
        AskSize = 0x40,
        Bid = 8,
        BidSize = 0x10,
        ChangeLast = 0x1000,
        Close = 4,
        Exchange = 0x2000,
        High = 0x80,
        Low = 0x100,
        Open = 2,
        Tick = 0x400,
        TodayClose = 0x8000,
        Trade = 1,
        TradeSize = 0x800,
        TradeTime = 0x4000,
        Volume = 0x200
    }
}

