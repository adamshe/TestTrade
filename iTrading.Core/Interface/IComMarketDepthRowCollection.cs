namespace iTrading.Core.Interface
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.MarketDepthRowCollection" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("0209F961-ED6C-4c31-B290-E195634B3D70")]
    public interface IComMarketDepthRowCollection
    {
        /// <summary>
        /// The number of available <see cref="T:iTrading.Core.Kernel.MarketDepthRow" /> instances.
        /// </summary>
        int Count { get; }
        /// <summary>
        /// Get the n-th <see cref="T:iTrading.Core.Kernel.MarketDepth" /> item of the container.
        /// </summary>
        MarketDepthRow this[int index] { get; }
        /// <summary>
        /// Checks if the exection exists in this container.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        bool Contains(MarketDepthRow value);
    }
}

