using System;

namespace iTrading.Core.Data
{
    /// <summary>
    /// Quote bar.
    /// </summary>
    public interface IBar
    {
        /// <summary></summary>
        string ToString();

        /// <summary></summary>
        double Close { get; }

        /// <summary></summary>
        double High { get; }

        /// <summary></summary>
        double Low { get; }

        /// <summary></summary>
        double Open { get; }

        /// <summary></summary>
        DateTime Time { get; }

        /// <summary></summary>
        int Volume { get; }
    }
}