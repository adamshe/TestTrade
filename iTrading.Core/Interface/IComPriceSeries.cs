namespace iTrading.Core.Interface
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;

    /// <summary>
    /// A collection of price values (open/high/low/close).
    /// </summary>
    [Guid("7414D7F1-7372-4c8c-9578-FA9EDFC86B6A")]
    public interface IComPriceSeries
    {
        /// <summary>
        /// Number of items.
        /// </summary>
        int Count { get; }
        /// <summary>
        /// Get Open/High/Low/Close value at index.
        /// </summary>
        double this[int idx] { get; }
    }
}

