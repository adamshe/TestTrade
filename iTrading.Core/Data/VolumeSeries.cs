namespace iTrading.Core.Data
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// A collection of bar volume  values.
    /// </summary>
    [ClassInterface(ClassInterfaceType.None), Guid("844199CB-0521-49e7-9967-3E7F68E6E790")]
    public class VolumeSeries : IComVolumeSeries
    {
        private Quotes quotes;

        internal VolumeSeries(Quotes quotes)
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
        /// Get bar volume value at index.
        /// </summary>
        public int this[int idx]
        {
            get
            {
                return this.quotes.Bars[idx].Volume;
            }
        }
    }
}

