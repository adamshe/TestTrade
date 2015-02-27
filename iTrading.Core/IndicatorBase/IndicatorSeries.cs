using System;
using iTrading.Core.Kernel;
using iTrading.Core.Data;

namespace iTrading.Core.IndicatorBase
{
    internal class IndicatorSeries : IDoubleSeries
    {
        private double[] buffer = new double[0x400];
        internal int current = -1;
        private IndicatorBase indicator;
        private int maxIdx = -1;

        internal IndicatorSeries(IndicatorBase indicator)
        {
            this.indicator = indicator;
        }

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
        /// </summary>
        public IndicatorBase Indicator
        {
            get
            {
                return this.indicator;
            }
        }

        /// <summary>
        /// Get/set double value at index.
        /// </summary>
        public double this[int index]
        {
            get
            {
                if ((index < 0) || (index >= this.indicator.Source.Count))
                {
                    throw new ArgumentOutOfRangeException("index", index, "IndicatorSeries.get_item: index out of range 0 - " + (this.indicator.Source.Count - 1));
                }
                if (index > this.current)
                {
                    for (int i = this.current + 1; i <= index; i++)
                    {
                        if (!this.indicator.initialized)
                        {
                            try
                            {
                                this.indicator.Init();
                            }
                            catch (Exception exception)
                            {
                                throw new TMException(ErrorCode.IndicatorInit, exception.Message);
                            }
                            this.indicator.initialized = true;
                        }
                        double indicatorValue = 0.0;
                        try
                        {
                            indicatorValue = this.indicator.Calculate(i);
                        }
                        catch (Exception exception2)
                        {
                            throw new TMException(ErrorCode.IndicatorCalculate, exception2.Message);
                        }
                        this.indicator.Add(0, indicatorValue);
                        for (int j = 0; j < this.indicator.resultSeries.Count; j++)
                        {
                            ((IndicatorSeries) this.indicator.resultSeries[j]).current = i;
                        }
                    }
                }
                return this.buffer[index];
            }
            set
            {
                if (index < 0)
                {
                    throw new ArgumentOutOfRangeException("index", index, "TradeMagic.Indicator.IndicatorSeries.Item: index must be greater/equal");
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