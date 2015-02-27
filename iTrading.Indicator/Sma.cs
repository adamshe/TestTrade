using iTrading.Core.IndicatorBase;

namespace iTrading.Indicator
{
    using Steema.TeeChart.Styles;
    using System;
    using System.Drawing;
    using iTrading.Core.Data;

    public class Sma : IndicatorBase
    {
        private iTrading.Core.Chart.SeriesCollection chartSeries;
        private int period;

        public Sma(IDoubleSeries source) : base(source)
        {
            this.chartSeries = new iTrading.Core.Chart.SeriesCollection();
            this.period = 14;
        }

        public Sma(Quotes quotes) : base(quotes)
        {
            this.chartSeries = new iTrading.Core.Chart.SeriesCollection();
            this.period = 14;
        }

        protected  override double Calculate(int current)
        {
            double num = 0.0;
            if (current == 0)
            {
                return base.Source[current];
            }
            num = base[current - 1] * Math.Min(current, this.Period);
            if (current >= this.Period)
            {
                return (((num + base.Source[current]) - base.Source[current - this.Period]) / ((double) Math.Min(current, this.Period)));
            }
            return ((num + base.Source[current]) / ((double) (Math.Min(current, this.Period) + 1)));
        }

        public override string ToString()
        {
            return string.Concat(new object[] { "SMA(", this.Period, ", ", PriceType.All[this.BasedOn].Name, ")" });
        }

        [Parameter(PriceTypeId.Close, "Based on")]
        public PriceTypeId BasedOn
        {
            get
            {
                return base.DefaultBasedOn;
            }
            set
            {
                base.DefaultBasedOn = value;
            }
        }

        public override iTrading.Core.Chart.SeriesCollection ChartSeries
        {
            get
            {
                lock (this)
                {
                    if (this.chartSeries.Count == 0)
                    {
                        Line series = new Line();
                        series.Brush.Color = Color.DarkViolet;
                        this.chartSeries.Add(series);
                    }
                    return this.chartSeries;
                }
            }
        }

        public override bool IsPriceIndicator
        {
            get
            {
                return true;
            }
        }

        public override string Name
        {
            get
            {
                return "Simple Moving Average";
            }
        }

        [Parameter(1, 0x7fffffff, "# of periods")]
        public int Period
        {
            get
            {
                return this.period;
            }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("Period", value, "Indicator.Sma: property out of range");
                }
                this.period = value;
            }
        }

        public override int UnstablePeriod
        {
            get
            {
                return this.Period;
            }
        }
    }
}

