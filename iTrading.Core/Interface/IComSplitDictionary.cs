namespace iTrading.Core.Interface
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Dictionary of stock splits.
    /// </summary>
    [Guid("8C77CBA4-42BF-4ac8-A67C-1D9DD44C73DF")]
    public interface IComSplitDictionary
    {
        /// <summary>
        /// Add a split.
        /// </summary>
        /// <param name="splitDate"></param>
        /// <param name="factor"></param>
        void Add(DateTime splitDate, double factor);
        /// <summary>
        /// Checks if the split exists in this container.
        /// </summary>
        /// <param name="splitDate"></param>
        /// <returns></returns>
        bool Contains(DateTime splitDate);
        /// <summary>
        /// Get a splitfactor for a given date.
        /// </summary>
        /// <param name="atDate"></param>
        /// <returns></returns>
        double GetSplitFactor(DateTime atDate);
        /// <summary>
        /// Retrieves a split factor by a split date. If date is not a split date, 1 is returned.
        /// </summary>
        /// <param name="splitDate"></param>
        double this[DateTime splitDate] { get; set; }
        /// <summary>
        /// The collection of available split dates.
        /// Used by COM clients to enumerate the <see cref="T:iTrading.Core.Kernel.SplitDictionary" />
        /// because Interop does not allow enumeration (for..each) of Dictionaries.
        /// </summary>
        ValuesCollection KeyCollection { get; }
    }
}

