namespace TradeMagic.Pats
{
    using System;

    internal enum OrderType
    {
        GTCLimit = 0x33,
        GTCMarket = 50,
        GTCStop = 0x34,
        GTDLimit = 0x36,
        GTDMarket = 0x35,
        GTDStop = 0x37,
        LimitAtOpen = 0x11,
        MLM = 0x12,
        OrderTypeIOC = 12,
        OrderTypeLimit = 2,
        OrderTypeLimitFAK = 3,
        OrderTypeLimitFOK = 4,
        OrderTypeMarket = 1,
        OrderTypeMarketFOK = 10,
        OrderTypeMIT = 8,
        OrderTypeMOO = 11,
        OrderTypeRFQ = 15,
        OrderTypeStop = 5,
        OrderTypeStopFall = 14,
        OrderTypeStopLoss = 0x10,
        OrderTypeStopRise = 13,
        OrderTypeSynthMIT = 9,
        OrderTypeSynthStop = 6,
        OrderTypeSynthStopLimit = 7,
        SETSCurDel = 0x5f,
        SETSInstDel = 0x5e,
        SETSRepcancel = 0x5b,
        SETSRepenter = 90,
        SETSRepprerel = 0x5c,
        SETSSectDel = 0x5d
    }
}

