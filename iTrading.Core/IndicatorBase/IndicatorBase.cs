using System;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using iTrading.Core.Chart;
using iTrading.Core.Data;
using iTrading.Core.Kernel;
using Steema.TeeChart.Styles;
using iTrading.Core.Data;

namespace iTrading.Core.IndicatorBase
{
    /// <summary>
    /// Base class for all indicators.
    /// </summary>
    public abstract class IndicatorBase : IDoubleSeries
    {
        private iTrading.Core.Chart.SeriesCollection chartSeries;
        private IDoubleSeries close;
        private PriceTypeId defaultBasedOn;
        private IDoubleSeries doubleSeries;
        private IDoubleSeries high;
        internal bool initialized;
        private LevelCollection levels;
        private IDoubleSeries low;
        private IDoubleSeries median;
        private IDoubleSeries open;
        private Quotes quotes;
        internal ArrayList resultSeries;
        private IDoubleSeries source;
        private TimeSeries time;
        private IDoubleSeries typical;
        private VolumeSeries volume;

        /// <summary>
        /// </summary>
        internal IndicatorBase()
        {
            this.close = null;
            this.defaultBasedOn = PriceTypeId.Close;
            this.doubleSeries = null;
            this.high = null;
            this.initialized = false;
            this.levels = new LevelCollection();
            this.low = null;
            this.median = null;
            this.open = null;
            this.quotes = null;
            this.resultSeries = new ArrayList();
            this.chartSeries = new iTrading.Core.Chart.SeriesCollection();
            this.source = null;
            this.time = null;
            this.typical = null;
            this.volume = null;
            this.resultSeries.Add(new IndicatorSeries(this));
        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        protected IndicatorBase(IDoubleSeries source)
        {
            this.close = null;
            this.defaultBasedOn = PriceTypeId.Close;
            this.doubleSeries = null;
            this.high = null;
            this.initialized = false;
            this.levels = new LevelCollection();
            this.low = null;
            this.median = null;
            this.open = null;
            this.quotes = null;
            this.resultSeries = new ArrayList();
            this.chartSeries = new iTrading.Core.Chart.SeriesCollection();
            this.source = null;
            this.time = null;
            this.typical = null;
            this.volume = null;
            this.resultSeries.Add(new IndicatorSeries(this));
            this.SetDoubleSeries(source);
        }

        /// <summary>
        /// </summary>
        /// <param name="quotes"></param>
        protected IndicatorBase(Quotes quotes)
        {
            this.close = null;
            this.defaultBasedOn = PriceTypeId.Close;
            this.doubleSeries = null;
            this.high = null;
            this.initialized = false;
            this.levels = new LevelCollection();
            this.low = null;
            this.median = null;
            this.open = null;
            this.quotes = null;
            this.resultSeries = new ArrayList();
            this.chartSeries = new iTrading.Core.Chart.SeriesCollection();
            this.source = null;
            this.time = null;
            this.typical = null;
            this.volume = null;
            this.resultSeries.Add(new IndicatorSeries(this));
            this.SetQuotes(quotes);
        }

        /// <summary>
        /// </summary>
        /// <param name="numSeries"># of result series for this indicator.</param>
        /// <param name="source"></param>
        protected IndicatorBase(int numSeries, IDoubleSeries source)
        {
            this.close = null;
            this.defaultBasedOn = PriceTypeId.Close;
            this.doubleSeries = null;
            this.high = null;
            this.initialized = false;
            this.levels = new LevelCollection();
            this.low = null;
            this.median = null;
            this.open = null;
            this.quotes = null;
            this.resultSeries = new ArrayList();
            this.chartSeries = new iTrading.Core.Chart.SeriesCollection();
            this.source = null;
            this.time = null;
            this.typical = null;
            this.volume = null;
            Trace.Assert(numSeries >= 1, "Indicator.IndicatorBase.ctor: numSeries=" + numSeries);
            for (int i = 0; i < numSeries; i++)
            {
                this.resultSeries.Add(new IndicatorSeries(this));
            }
            this.SetDoubleSeries(source);
        }

        /// <summary>
        /// </summary>
        /// <param name="numSeries"># of result series for this indicator.</param>
        /// <param name="quotes"></param>
        protected IndicatorBase(int numSeries, Quotes quotes)
        {
            this.close = null;
            this.defaultBasedOn = PriceTypeId.Close;
            this.doubleSeries = null;
            this.high = null;
            this.initialized = false;
            this.levels = new LevelCollection();
            this.low = null;
            this.median = null;
            this.open = null;
            this.quotes = null;
            this.resultSeries = new ArrayList();
            this.chartSeries = new iTrading.Core.Chart.SeriesCollection();
            this.source = null;
            this.time = null;
            this.typical = null;
            this.volume = null;
            Trace.Assert(numSeries >= 1, "Indicator.IndicatorBase.ctor: numSeries=" + numSeries);
            for (int i = 0; i < numSeries; i++)
            {
                this.resultSeries.Add(new IndicatorSeries(this));
            }
            this.SetQuotes(quotes);
        }

        /// <summary>
        /// </summary>
        /// <param name="numSeries"># of result series for this indicator.</param>
        /// <param name="source"></param>
        /// <param name="basedOn"></param>
        protected IndicatorBase(int numSeries, IDoubleSeries source, PriceTypeId basedOn)
        {
            this.close = null;
            this.defaultBasedOn = PriceTypeId.Close;
            this.doubleSeries = null;
            this.high = null;
            this.initialized = false;
            this.levels = new LevelCollection();
            this.low = null;
            this.median = null;
            this.open = null;
            this.quotes = null;
            this.resultSeries = new ArrayList();
            this.chartSeries = new iTrading.Core.Chart.SeriesCollection();
            this.source = null;
            this.time = null;
            this.typical = null;
            this.volume = null;
            Trace.Assert(numSeries >= 1, "Indicator.IndicatorBase.ctor: numSeries=" + numSeries);
            for (int i = 0; i < numSeries; i++)
            {
                this.resultSeries.Add(new IndicatorSeries(this));
            }
            this.SetDoubleSeries(source);
            this.DefaultBasedOn = basedOn;
        }

        /// <summary>
        /// </summary>
        /// <param name="numSeries"># of result series for this indicator.</param>
        /// <param name="quotes"></param>
        /// <param name="basedOn"></param>
        protected IndicatorBase(int numSeries, Quotes quotes, PriceTypeId basedOn)
        {
            this.close = null;
            this.defaultBasedOn = PriceTypeId.Close;
            this.doubleSeries = null;
            this.high = null;
            this.initialized = false;
            this.levels = new LevelCollection();
            this.low = null;
            this.median = null;
            this.open = null;
            this.quotes = null;
            this.resultSeries = new ArrayList();
            this.chartSeries = new iTrading.Core.Chart.SeriesCollection();
            this.source = null;
            this.time = null;
            this.typical = null;
            this.volume = null;
            Trace.Assert(numSeries >= 1, "Indicator.IndicatorBase.ctor: numSeries=" + numSeries);
            for (int i = 0; i < numSeries; i++)
            {
                this.resultSeries.Add(new IndicatorSeries(this));
            }
            this.SetQuotes(quotes);
            this.DefaultBasedOn = basedOn;
        }

        /// <summary>
        /// Get value of a series at specified index.
        /// </summary>
        /// <param name="series">Index of selected series.</param>
        /// <param name="indicatorValue">Value to set.</param>
        protected internal void Add(int series, double indicatorValue)
        {
            if ((series < 0) || (series >= this.CountSeries))
            {
                throw new ArgumentOutOfRangeException("series", series, "Indicator.IndicatorBase.SetAt: series out of range 0 - " + (this.CountSeries - 1));
            }
            IndicatorSeries series2 = (IndicatorSeries) this.resultSeries[series];
            series2[++series2.current] = indicatorValue;
        }

        /// <summary>
        /// Alpha value for exponential moving averages:  2.0 / (1 + period).
        /// </summary>
        /// <param name="period"></param>
        /// <returns></returns>
        public static double Alpha(int period)
        {
            return (2.0 / ((double) (1 + period)));
        }

        private void BarUpdate(object sender, BarUpdateEventArgs e)
        {
            if ((e.Quotes == this.quotes) && (((IndicatorSeries) this.resultSeries[0]).current >= e.First))
            {
                int num = Math.Min(((IndicatorSeries) this.resultSeries[0]).current, e.Last);
                if (!this.initialized)
                {
                    try
                    {
                        this.Init();
                    }
                    catch (Exception exception)
                    {
                        throw new TMException(ErrorCode.IndicatorInit, exception.Message);
                    }
                    this.initialized = true;
                }
                if ((e.First > 0) && (e.Operation != Operation.Delete))
                {
                    for (int j = 0; j < this.resultSeries.Count; j++)
                    {
                        ((IndicatorSeries) this.resultSeries[j]).current = e.First - 1;
                    }
                }
                for (int i = e.First; i <= num; i++)
                {
                    double indicatorValue = 0.0;
                    try
                    {
                        indicatorValue = this.Calculate(i);
                    }
                    catch (Exception exception2)
                    {
                        throw new TMException(ErrorCode.IndicatorCalculate, exception2.Message);
                    }
                    this.Add(0, indicatorValue);
                    for (int k = 0; k < this.resultSeries.Count; k++)
                    {
                        ((IndicatorSeries) this.resultSeries[k]).current = i;
                    }
                }
            }
        }

        /// <summary>
        /// Calculates the indicator value(s) at the current index.
        /// Note: The returned value will be stored in the result series = 0. All other result values must be saved within this method.
        /// </summary>
        /// <param name="current"></param>
        protected internal   abstract double Calculate(int current);
        internal IndicatorBase CloneWithQuotes(Quotes quotes)
        {
            IndicatorBase base2 = (IndicatorBase) Globals.IndicatorAssembly.CreateInstance(base.GetType().FullName, false, BindingFlags.CreateInstance, null, new object[] { quotes }, null, new object[0]);
            PropertyType[] typeArray2 = new PropertyType[5];
            typeArray2[0] = PropertyType.BasedOn;
            typeArray2[2] = PropertyType.Param1;
            typeArray2[3] = PropertyType.Param2;
            typeArray2[4] = PropertyType.Param3;
            foreach (PropertyType type in typeArray2)
            {
                PropertyInfo propertyInfo = this.GetPropertyInfo(type);
                if (propertyInfo != null)
                {
                    propertyInfo.SetValue(base2, propertyInfo.GetValue(this, null), null);
                }
            }
            foreach (Series series in this.chartSeries)
            {
                base2.chartSeries.Add(series);
            }
            foreach (Level level in this.levels)
            {
                base2.levels.Add(level);
            }
            return base2;
        }

        /// <summary>
        /// Get value of a series at specified index.
        /// </summary>
        /// <param name="series">Index of selected series.</param>
        /// <param name="index">Index within the series.</param>
        /// <returns></returns>
        internal double GetAt(int series, int index)
        {
            if ((series < 0) || (series >= this.CountSeries))
            {
                throw new ArgumentOutOfRangeException("series", series, "Indicator.IndicatorBase.GetAt: series out of range 0 - " + (this.CountSeries - 1));
            }
            return ((IDoubleSeries) this.resultSeries[series])[index];
        }

        internal ParameterAttribute GetParameterAttribute(PropertyType propertyType)
        {
            int num = 0;
            foreach (PropertyInfo info in base.GetType().GetProperties())
            {
                ParameterAttribute attribute = (ParameterAttribute) Attribute.GetCustomAttribute(info, typeof(ParameterAttribute), false);
                if (((attribute != null) && info.CanWrite) && info.CanRead)
                {
                    if ((propertyType == PropertyType.BasedOn) && (info.PropertyType.Name == "PriceTypeId"))
                    {
                        return attribute;
                    }
                    if ((info.PropertyType.Name == "Double") || (info.PropertyType.Name == "Int32"))
                    {
                        if (num == (int)propertyType)
                        {
                            return attribute;
                        }
                        num++;
                    }
                }
            }
            return null;
        }

        internal PropertyInfo GetPropertyInfo(PropertyType propertyType)
        {
            int num = 0;
            foreach (PropertyInfo info in base.GetType().GetProperties())
            {
                if (((((ParameterAttribute) Attribute.GetCustomAttribute(info, typeof(ParameterAttribute), false)) != null) && info.CanWrite) && info.CanRead)
                {
                    if ((propertyType == PropertyType.BasedOn) && (info.PropertyType.Name == "PriceTypeId"))
                    {
                        return info;
                    }
                    if ((info.PropertyType.Name == "Double") || (info.PropertyType.Name == "Int32"))
                    {
                        if (num == (int)propertyType)
                        {
                            return info;
                        }
                        num++;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// </summary>
        /// <param name="series">Index of selected series.</param>
        protected IDoubleSeries GetSeries(int series)
        {
            if ((series < 0) || (series >= this.CountSeries))
            {
                throw new ArgumentOutOfRangeException("series", series, "Indicator.IndicatorBase.GetSeries: series out of range 0 - " + (this.CountSeries - 1));
            }
            return (IDoubleSeries) this.resultSeries[series];
        }

        /// <summary>
        /// Overridable init method. Is called once before the first <see cref="M:TradeMagic.Indicator.IndicatorBase.Calculate(System.Int32)" /> call.
        /// </summary>
        protected internal virtual void Init()
        {
        }

        private void SetDoubleSeries(IDoubleSeries newDoubleSeries)
        {
            this.doubleSeries = this.source = newDoubleSeries;
            if (this.quotes != null)
            {
                this.quotes.Connection.Bar -= new BarUpdateEventHandler(this.BarUpdate);
            }
            this.quotes = null;
        }

        private void SetQuotes(Quotes newQuotes)
        {
            this.doubleSeries = null;
            if (this.quotes != null)
            {
                this.quotes.Connection.Bar -= new BarUpdateEventHandler(this.BarUpdate);
            }
            this.quotes = null;
            this.close = (newQuotes == null) ? null : ((IDoubleSeries) newQuotes.Close);
            this.high = (newQuotes == null) ? null : ((IDoubleSeries) newQuotes.High);
            this.low = (newQuotes == null) ? null : ((IDoubleSeries) newQuotes.Low);
            this.median = (newQuotes == null) ? null : ((IDoubleSeries) newQuotes.Median);
            this.open = (newQuotes == null) ? null : ((IDoubleSeries) newQuotes.Open);
            this.quotes = newQuotes;
            this.time = (newQuotes == null) ? null : newQuotes.Time;
            this.typical = (newQuotes == null) ? null : ((IDoubleSeries) newQuotes.Typical);
            this.volume = (newQuotes == null) ? null : newQuotes.Volume;
            this.DefaultBasedOn = this.defaultBasedOn;
            if (this.quotes != null)
            {
                this.quotes.Connection.Bar += new BarUpdateEventHandler(this.BarUpdate);
            }
        }

        /// <summary>
        /// Text to describe the indicator.
        /// </summary>
        /// <returns></returns>
        public abstract override string ToString();

        /// <summary>
        /// Each chart series of this containers describes how to paint a series of values.
        /// By default this collection is empty. In case TradeMagic does not find a chart series to paint a value series, a default charting series is generated.
        /// </summary>
        public virtual iTrading.Core.Chart.SeriesCollection ChartSeries
        {
            get
            {
                return this.chartSeries;
            }
        }

        /// <summary>
        /// </summary>
        protected IDoubleSeries Close
        {
            get
            {
                if (this.quotes == null)
                {
                    return this.doubleSeries;
                }
                return this.close;
            }
        }

        /// <summary>
        /// Number of items.
        /// </summary>
        public int Count
        {
            get
            {
                if (this.quotes == null)
                {
                    return this.doubleSeries.Count;
                }
                return this.quotes.Bars.Count;
            }
        }

        /// <summary>
        /// Get the # of series for this indicator.
        /// </summary>
        public int CountSeries
        {
            get
            {
                return this.resultSeries.Count;
            }
        }

        /// <summary>
        /// Get/set the default series.
        /// </summary>
        protected PriceTypeId DefaultBasedOn
        {
            get
            {
                return this.defaultBasedOn;
            }
            set
            {
                this.defaultBasedOn = value;
                switch (this.defaultBasedOn)
                {
                    case PriceTypeId.Close:
                        this.source = (this.quotes != null) ? this.quotes.Close : this.doubleSeries;
                        return;

                    case PriceTypeId.High:
                        this.source = (this.quotes != null) ? this.quotes.High : this.doubleSeries;
                        return;

                    case PriceTypeId.Low:
                        this.source = (this.quotes != null) ? this.quotes.Low : this.doubleSeries;
                        return;

                    case PriceTypeId.Median:
                        this.source = (this.quotes != null) ? this.quotes.Median : this.doubleSeries;
                        return;

                    case PriceTypeId.Open:
                        this.source = (this.quotes != null) ? this.quotes.Open : this.doubleSeries;
                        return;

                    case PriceTypeId.Typical:
                        this.source = (this.quotes != null) ? this.quotes.Typical : this.doubleSeries;
                        return;
                }
                Trace.Assert(false, "Indicator.IndicatorBase,DefaultPriceTypeId.Set: " + ((int) this.defaultBasedOn));
            }
        }

        /// <summary>
        /// Get <see cref="T:TradeMagic.Data.IDoubleSeries" /> data source.
        /// </summary>
        public IDoubleSeries DoubleSeries
        {
            get
            {
                return this.doubleSeries;
            }
        }

        /// <summary>
        /// </summary>
        protected IDoubleSeries High
        {
            get
            {
                if (this.quotes == null)
                {
                    return this.doubleSeries;
                }
                return this.high;
            }
        }

        /// <summary>
        /// Does the indicator have the same price scale as the underlying data?
        /// </summary>
        public abstract bool IsPriceIndicator { get; }

        /// <summary>
        /// Get/set indicator value at index.
        /// </summary>
        public double this[int index]
        {
            get
            {
                return this.GetAt(0, index);
            }
        }

        /// <summary>
        /// Get the collection of levels to display together with this indicator. By default this collection is empty and can be overriden.
        /// </summary>
        public virtual LevelCollection Levels
        {
            get
            {
                return this.levels;
            }
        }

        /// <summary>
        /// </summary>
        protected IDoubleSeries Low
        {
            get
            {
                if (this.quotes == null)
                {
                    return this.doubleSeries;
                }
                return this.low;
            }
        }

        /// <summary>
        /// </summary>
        protected IDoubleSeries Median
        {
            get
            {
                if (this.quotes == null)
                {
                    return this.doubleSeries;
                }
                return this.median;
            }
        }

        /// <summary>
        /// The indicator's printable name.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// </summary>
        protected IDoubleSeries Open
        {
            get
            {
                if (this.quotes == null)
                {
                    return this.doubleSeries;
                }
                return this.open;
            }
        }

        /// <summary>
        /// Get <see cref="T:Quotes" /> data source.
        /// </summary>
        public Quotes Quotes
        {
            get
            {
                return this.quotes;
            }
        }

        /// <summary>
        /// Get the default source.
        /// </summary>
        protected internal IDoubleSeries Source
        {
            get
            {
                return this.source;
            }
        }

        /// <summary>
        /// </summary>
        protected TimeSeries Time
        {
            get
            {
                return this.time;
            }
        }

        /// <summary>
        /// </summary>
        protected IDoubleSeries Typical
        {
            get
            {
                if (this.quotes == null)
                {
                    return this.doubleSeries;
                }
                return this.typical;
            }
        }

        /// <summary>
        /// Usually indicators need some periods to stabilize their values.
        /// </summary>
        public abstract int UnstablePeriod { get; }

        /// <summary>
        /// </summary>
        protected VolumeSeries Volume
        {
            get
            {
                return this.volume;
            }
        }
    }
}