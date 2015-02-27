namespace iTrading.Core.Data
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Buffer for a series of double values.
    /// </summary>
    public class DoubleSeries : IDoubleSeries
    {
        private double[] buffer = new double[0x400];
        private int maxIdx = -1;

        /// <summary>
        /// Number of items.
        /// </summary>
        public int Count
        {
            get
            {
                return (this.maxIdx + 1);
            }
        }

        /// <summary>
        /// Get/set double value at index.
        /// </summary>
        public double this[int index]
        {
            get
            {
                if ((index < 0) || (index > this.maxIdx))
                {
                    throw new ArgumentOutOfRangeException("index", index, "TradeMagic.Indicator.DoubleSeries.Item: index out of range 0 - " + this.maxIdx);
                }
                return this.buffer[index];
            }
            set
            {
                if (index < 0)
                {
                    throw new ArgumentOutOfRangeException("index", index, "TradeMagic.Indicator.DoubleSeries.Item: index must be greater/equal");
                }
                while (this.buffer.Length <= index)
                {
                    double[] array = new double[this.buffer.Length * 2];
                    this.buffer.CopyTo(array, 0);
                    this.buffer = array;
                }
                this.buffer[index] = value;
                this.maxIdx = index;
            }
        }
    }
}

