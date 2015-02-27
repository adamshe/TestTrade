namespace iTrading.Core.Interface
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.MarketDataEventArgs" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("553B0D92-7F94-4615-831D-BBB6A93DCA67")]
    public interface IComMarketDataEventArgs
    {
        /// <summary>
        /// Type of actual data item.
        /// </summary>
        iTrading.Core.Kernel.MarketDataType MarketDataType { get; }
        /// <summary>
        /// Price of actual data item.
        /// </summary>
        double Price { get; }
        /// <summary>
        /// The affected symbol.
        /// </summary>
        iTrading.Core.Kernel.Symbol Symbol { get; }
        /// <summary>
        /// Time of actual data item.
        /// </summary>
        DateTime Time { get; }
        /// <summary>
        /// Volume of actual data item.
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

