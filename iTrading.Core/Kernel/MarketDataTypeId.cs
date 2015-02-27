namespace iTrading.Core.Kernel
{
    using System;

    /// <summary>
    /// Identifies the market data type.
    /// </summary>
    public enum MarketDataTypeId
    {
        Ask,
        Bid,
        Last,
        DailyHigh,
        DailyLow,
        DailyVolume,
        LastClose,
        Opening,
        Unknown
    }
}

