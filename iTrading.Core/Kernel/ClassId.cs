namespace iTrading.Core.Kernel
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Id to identify TradeMagic classes. Used for serialization.
    /// </summary>
    [ComVisible(false)]
    public enum ClassId
    {
        AccountEventArgs,
        AccountItemTypeEventArgs,
        AccountUpdateEventArgs,
        ActionTypeEventArgs,
        Connection,
        ConnectionStatusEventArgs,
        CurrencyEventArgs,
        ErrorEventArgs,
        ExchangeEventArgs,
        Execution,
        ExecutionUpdateEventArgs,
        FeatureTypeEventArgs,
        IBOptions,
        MarketData,
        MarketDataEventArgs,
        MarketDataTypeEventArgs,
        MarketDepth,
        MarketDepthEventArgs,
        MarketMakerEventArgs,
        MarketPositionEventArgs,
        MbtOptions,
        NewsEventArgs,
        NewsItemTypeEventArgs,
        OptionsBase,
        Order,
        OrderStateEventArgs,
        OrderStatusEventArgs,
        OrderTypeEventArgs,
        PatsOptions,
        PositionUpdateEventArgs,
        Request,
        StreamingRequest,
        Symbol,
        SymbolEventArgs,
        SymbolTypeEventArgs,
        TimeInForceEventArgs,
        TMEventArgs,
        TrackOptions,
        SimulationAccountOptions,
        SimulationSymbolOptions,
        DataFileHeader,
        TimerEventArgs,
        DtnOptions,
        ESignalOptions,
        BarUpdateEventArgs,
        YahooOptions,
        CTOptions
    }
}

