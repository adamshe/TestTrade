namespace iTrading.Core.Interface
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Dictionary of stock dividends.
    /// </summary>
    [Guid("B2FCA760-7151-462e-B872-1C0705C091E4")]
    public interface IComDividendDictionary
    {
        /// <summary>
        /// Add a dividend.
        /// </summary>
        /// <param name="dividendDate"></param>
        /// <param name="dividend"></param>
        void Add(DateTime dividendDate, double dividend);
        /// <summary>
        /// Checks if the dividend exists in this container.
        /// </summary>
        /// <param name="dividendDate"></param>
        /// <returns></returns>
        bool Contains(DateTime dividendDate);
        /// <summary>
        /// Retrieves a dividend by a dividend date. If date is not a dividend date, 0 is returned.
        /// </summary>
        /// <param name="dividendDate"></param>
        double this[DateTime dividendDate] { get; set; }
        /// <summary>
        /// The collection of available dividend dates.
        /// Used by COM clients to enumerate the <see cref="T:iTrading.Core.Kernel.DividendDictionary" />
        /// because Interop does not allow enumeration (for..each) of Dictionaries.
        /// </summary>
        ValuesCollection KeyCollection { get; }
    }
}

