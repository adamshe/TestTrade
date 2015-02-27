namespace iTrading.Core.Interface
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Daily overview. Used mainy for intraday quotes.
    /// </summary>
    [Guid("B86985A9-6BC0-4473-B568-43A8E49A18EB")]
    public interface IComQuotesDay
    {
        /// <summary>
        /// Clsoe of the day.
        /// </summary>
        double Close { get; }
        /// <summary>
        /// Index of first bar of the day.
        /// </summary>
        int FirstBar { get; }
        /// <summary>
        /// High of the day.
        /// </summary>
        double High { get; }
        /// <summary>
        /// Index of last bar of day.
        /// </summary>
        int LastBar { get; }
        /// <summary>
        /// Low of the day.
        /// </summary>
        double Low { get; }
        /// <summary>
        /// Open of the day.
        /// </summary>
        double Open { get; }
        /// <summary>
        /// Date.
        /// </summary>
        DateTime Time { get; }
        /// <summary>
        /// Volume of the day.
        /// </summary>
        int Volume { get; }
    }
}

