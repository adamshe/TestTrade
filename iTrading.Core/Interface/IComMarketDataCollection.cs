namespace iTrading.Core.Interface
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.MarketDataCollection" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("CC625923-9B3F-4ce0-8CC6-4DC7BDD68369")]
    public interface IComMarketDataCollection
    {
        /// <summary>
        /// Get the n-th <see cref="T:iTrading.Core.Kernel.MarketData" /> item of the container.
        /// </summary>
        MarketData this[int index] { get; }
        /// <summary>
        /// Checks if the exection exists in this container.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        bool Contains(MarketData value);
        /// <summary>
        /// The number of available <see cref="T:iTrading.Core.Kernel.MarketData" /> instances.
        /// </summary>
        int Count { get; }
    }
}

