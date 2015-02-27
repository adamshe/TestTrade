namespace iTrading.Mbt
{
    using System;

    internal enum TimeInForce
    {
        Day = 0x271b,
        DayPlus = 0x2719,
        Gtc = 0x2718,
        Ioc = 0x271a,
        Session = 0x271c
    }
}

