namespace iTrading.Core.Interface
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.MarketDepthRow" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("7AEDC736-1E04-4689-BCBB-392E4F32E7A5")]
    public interface IComMarketDepthRow
    {
        /// <summary>
        /// Market maker id.
        /// </summary>
        string MarketMaker { get; }
        /// <summary>
        /// Price of position.
        /// </summary>
        double Price { get; }
        /// <summary>
        /// Time last update.
        /// </summary>
        DateTime Time { get; }
        /// <summary>
        /// Siez of position.
        /// </summary>
        int Volume { get; }
    }
}

