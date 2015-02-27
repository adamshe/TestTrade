namespace iTrading.Core.Kernel
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>For internal use only.</summary>
    [ComVisible(false)]
    public interface IMarketData
    {
        /// <summary>
        /// Subscribe to market data.
        /// </summary>
        /// <param name="marketData"></param>
        void Subscribe(MarketData marketData);
        /// <summary>
        /// Unsubsribe from market data.
        /// </summary>
        /// <param name="marketData"></param>
        void Unsubscribe(MarketData marketData);
    }
}

