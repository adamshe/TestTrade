namespace iTrading.Core.Interface
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.MarketDepthEventArgs" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("AE00920B-8520-498b-83A5-451A5412DAC0")]
    public interface IComMarketDepthEventArgs
    {
        /// <summary>
        /// Market maker.
        /// </summary>
        string MarketMaker { get; }
        /// <summary>
        /// Type of actual market depth item.
        /// </summary>
        iTrading.Core.Kernel.Operation Operation { get; }
        /// <summary>
        /// Position of market depth item. Can be used to e.g. display the <see cref="T:iTrading.Core.Kernel.MarketDepthEventArgs" /> in a datagrid.
        /// </summary>
        int Position { get; }
        /// <summary>
        /// Price of market depth item.
        /// </summary>
        double Price { get; }
        /// <summary>
        /// Side of market depth item.
        /// </summary>
        iTrading.Core.Kernel.MarketDataType MarketDataType { get; }
        /// <summary>
        /// The affected symbol.
        /// </summary>
        iTrading.Core.Kernel.Symbol Symbol { get; }
        /// <summary>
        /// Time value.
        /// </summary>
        DateTime Time { get; }
        /// <summary>
        /// Volume of market depth item.
        /// </summary>
        int Volume { get; }
        /// <summary>
        /// Returns the TradeMagic error code of the Args. 
        /// If the asyncronous call has been successful, the value will be <seealso cref="F:iTrading.Core.Kernel.ErrorCode.NoError" />.
        /// </summary>
        ErrorCode Error { get; }
        /// <summary>
        /// Native error code of underlying broker or data provider.
        /// </summary>
        string NativeError { get; }
        /// <summary>
        /// Request causing the Args.
        /// </summary>
        iTrading.Core.Kernel.Request Request { get; }
    }
}

