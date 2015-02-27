namespace iTrading.Core.Interface
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;

    /// <summary>
    /// A collection of bar timestamp values.
    /// </summary>
    [Guid("3D3117E7-AF0A-4303-A7FF-38166AF96A5D")]
    public interface IComTimeSeries
    {
        /// <summary>
        /// Number of items.
        /// </summary>
        int Count { get; }
        /// <summary>
        /// Get bar timestamp value at index.
        /// </summary>
        DateTime this[int idx] { get; }
    }
}

