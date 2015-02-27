using iTrading.Core.Data;

namespace iTrading.Core.Interface
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;
    using iTrading.Core.Data;

    /// <summary>
    /// A series of quote bars. <seealso cref="T:TradeMagic.Data.Bar" />
    /// </summary>
    [Guid("E544CABF-96AD-4bc3-9D07-8B20771E5AC7")]
    public interface IComQuotes
    {
        /// <summary>
        /// Get the collection of <see cref="T:TradeMagic.Data.Bar" /> objects.
        /// </summary>
        Bars Bars { get; }
        /// <summary>
        /// Get the collection of close values.
        /// </summary>
        PriceSeries Close { get; }
        /// <summary>
        /// Create a new <see cref="T:Quotes" /> object of different periodicy.
        /// Please note: Only <see cref="T:Quotes" /> objects of the same <see cref="T:TradeMagic.Data.PeriodTypeId" /> may be
        /// created and their <see cref="P:TradeMagic.Data.Period.Value" /> must be a multiple of the
        /// original value.
        /// </summary>
        /// <param name="newQPeriod"></param>
        /// <returns></returns>
        Quotes Copy(Period newQPeriod);
        /// <summary>
        /// Dump quotes to a file.
        /// </summary>
        /// <param name="path">File path. Any existing file will be overriden.</param>
        /// <param name="seperator">Character to seperate the output fields.</param>
        /// <returns></returns>
        void Dump(string path, char seperator);
        /// <summary>
        /// Date of first quote bar.
        /// </summary>
        DateTime From { get; }
        /// <summary>
        /// Get the collection of high values.
        /// </summary>
        PriceSeries High { get; }
        /// <summary>
        /// Get the collection of low values.
        /// </summary>
        PriceSeries Low { get; }
        /// <summary>
        /// Get the collection of open values.
        /// </summary>
        PriceSeries Open { get; }
        /// <summary>
        /// Periodicy of <see cref="T:Quotes" /> object.
        /// </summary>
        Period Period { get; }
        /// <summary>
        /// </summary>
        bool SplitAdjusted { get; }
        /// <summary>
        /// Get the <see cref="P:iTrading.Core.Interface.IComQuotes.Symbol" /> object.
        /// </summary>
        iTrading.Core.Kernel.Symbol Symbol { get; }
        /// <summary>
        /// Get the collection of time values.
        /// </summary>
        TimeSeries Time { get; }
        /// <summary>
        /// Date of last quote bar.
        /// </summary>
        DateTime To { get; }
        /// <summary>
        /// Symbol name including exchange.
        /// </summary>
        /// <returns></returns>
        string ToString();
        /// <summary>
        /// Get the collection of volume values.
        /// </summary>
        VolumeSeries Volume { get; }
        /// <summary>
        /// Get the collection of typical price values = (H + L + C) /3
        /// </summary>
        PriceSeries Typical { get; }
    }
}

