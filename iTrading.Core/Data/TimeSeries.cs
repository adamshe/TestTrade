namespace iTrading.Core.Data
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// A collection of bar timestamp values.
    /// </summary>
    [ClassInterface(ClassInterfaceType.None), Guid("33FCFFB1-B763-4a41-ADBD-B7F137F4FC3D")]
    public class TimeSeries : IComTimeSeries
    {
        private Quotes quotes;

        internal TimeSeries(Quotes quotes)
        {
            this.quotes = quotes;
        }

        /// <summary>
        /// Number of items.
        /// </summary>
        public int Count
        {
            get
            {
                return this.quotes.Bars.Count;
            }
        }

        /// <summary>
        /// Get bar timestamp value at index.
        /// </summary>
        public DateTime this[int idx]
        {
            get
            {
                return this.quotes.Bars[idx].Time;
            }
        }
    }
}

