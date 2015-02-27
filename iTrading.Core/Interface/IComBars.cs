namespace iTrading.Core.Interface
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Data;

    /// <summary>
    /// A collection of <see cref="T:TradeMagic.Data.Bar" /> objects.
    /// </summary>
    [Guid("68FB4CBA-4D9F-4f2a-BD23-22A8C4FD6B2B")]
    public interface IComBars
    {
        /// <summary>
        /// Number of items.
        /// </summary>
        int Count { get; }
        /// <summary>
        /// Get daily summary.
        /// </summary>
        /// <param name="atDate"></param>
        /// <returns>NULL, if the requested day is not included in the current quotes object.</returns>
        QuotesDay GetQuotesDay(DateTime atDate);
    }
}

