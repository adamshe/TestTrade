namespace iTrading.Core.Interface
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.MarketPosition" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("87831F1E-2368-423c-9E5D-BBF65BC1F1E0")]
    public interface IComMarketPosition
    {
        /// <summary>
        /// The TradeMagic id of the MarketPosition. This id is independent from the underlying provider system.
        /// </summary>
        MarketPositionId Id { get; }
        /// <summary>
        /// The name of the MarketPosition.
        /// </summary>
        string Name { get; }
    }
}

