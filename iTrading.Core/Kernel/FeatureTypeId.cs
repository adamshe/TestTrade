namespace iTrading.Core.Kernel
{
    using System;

    /// <summary>
    /// Identifies the order Feature type.
    /// </summary>
    public enum FeatureTypeId
    {
        ClockSynchronization,
        MarketData,
        MarketDepth,
        MaxMarketDataStreams,
        MaxMarketDepthStreams,
        MultipleConnections,
        NativeOcaOrders,
        News,
        Order,
        OrderChange,
        Quotes1Minute,
        QuotesDaily,
        QuotesTick,
        SplitsAdjustedDaily,
        SymbolLookup,
        SynchronousSymbolLookup
    }
}

