namespace iTrading.Core.Data
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Interface for double value container.
    /// </summary>
    public interface IDoubleSeries
    {
        /// <summary>
        /// Get # of available values.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Get value at index.
        /// </summary>
        double this[int index] { get; }
    }
}

