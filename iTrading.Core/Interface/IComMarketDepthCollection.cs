namespace iTrading.Core.Interface
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.MarketDepthCollection" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("C0175D31-52E0-4855-9027-11241511C2CA")]
    public interface IComMarketDepthCollection
    {
        /// <summary>
        /// Get the n-th <see cref="T:iTrading.Core.Kernel.MarketDepth" /> item of the container.
        /// </summary>
        MarketDepth this[int index] { get; }
        /// <summary>
        /// Checks if the exection exists in this container.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        bool Contains(MarketDepth value);
        /// <summary>
        /// The number of available <see cref="T:iTrading.Core.Kernel.MarketDepth" /> instances.
        /// </summary>
        int Count { get; }
    }
}

