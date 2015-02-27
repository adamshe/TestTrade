namespace iTrading.Core.Kernel
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>For internal use only.</summary>
    [ComVisible(false)]
    public interface IMarketDepth
    {
        /// <summary>
        /// Subscribe to market depth data.
        /// </summary>
        /// <param name="marketDepth"></param>
        void Subscribe(MarketDepth marketDepth);
        /// <summary>
        /// Unscrubsribe from market depth data.
        /// </summary>
        /// <param name="marketDepth"></param>
        void Unsubscribe(MarketDepth marketDepth);
    }
}

