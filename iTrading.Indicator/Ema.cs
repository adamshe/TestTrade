using iTrading.Core.IndicatorBase;

namespace iTrading.Indicator
{
    using Steema.TeeChart.Styles;
    using System;
    using System.Drawing;
    using iTrading.Core.Data;

    public class Ema : IndicatorBase
    {
        private iTrading.Core.Chart.SeriesCollection chartSeries;
        private int period;

        public Ema(IDoubleSeries source) : base(source)
        {
            this.chartSeries = new iTrading.Core.Chart.SeriesCollection();
            this.period = 14;
        }

        public Ema(Quotes quotes) : base(quotes)
        {
            this.chartSeries = new iTrading.Core.Chart.SeriesCollection();
            this.period = 14;
        }

        protected  override double Calculate(int current)
        {
            if (current != 0)
            {
                return ((base.Source[current] * IndicatorBase.Alpha(this.Period)) + ((1.0 - IndicatorBase.Alpha(this.Period)) * base[current - 1]));
            }
            return base.Source[current];
        }

        public override string ToString()
        {
            return string.Concat(new object[] { "EMA(", this.Period, ", ", PriceType.All[this.BasedOn].Name, ")" });
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
                return "Exponential Moving Average";
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
                    throw new ArgumentOutOfRangeException("Period", value, "Indicator.Ema: property out of range");
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

