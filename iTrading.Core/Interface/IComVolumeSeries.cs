namespace iTrading.Core.Interface
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;

    /// <summary>
    /// A collection of bar volume  values.
    /// </summary>
    [Guid("AE40A761-CF6C-4015-ACC2-835377DEAB72")]
    public interface IComVolumeSeries
    {
        /// <summary>
        /// Number of items.
        /// </summary>
        int Count { get; }
        /// <summary>
        /// Get bar volume value at index.
        /// </summary>
        int this[int idx] { get; }
    }
}

