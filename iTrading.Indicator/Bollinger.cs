using iTrading.Core.IndicatorBase;

namespace iTrading.Indicator
{
    using Steema.TeeChart.Styles;
    using System;
    using System.Drawing;
    using iTrading.Core.Data;

    public class Bollinger : IndicatorBase
    {
        private iTrading.Core.Chart.SeriesCollection chartSeries;
        private double numStdDev;
        private int period;
        private Sma sma;
        private StdDev stdDev;

        public Bollinger(IDoubleSeries source) : base(2, source, PriceTypeId.Close)
        {
            this.chartSeries = new iTrading.Core.Chart.SeriesCollection();
            this.numStdDev = 2.0;
            this.period = 14;
            this.sma = new Sma(source);
            this.stdDev = new StdDev(source);
        }

        public Bollinger(Quotes quotes) : base(2, quotes, PriceTypeId.Close)
        {
            this.chartSeries = new iTrading.Core.Chart.SeriesCollection();
            this.numStdDev = 2.0;
            this.period = 14;
            this.sma = new Sma(quotes);
            this.stdDev = new StdDev(quotes);
        }

        protected override double Calculate(int current)
        {
            base.Add(1, this.sma[current] - (this.NumStdDev * this.stdDev[current]));
            return (this.sma[current] + (this.NumStdDev * this.stdDev[current]));
        }

        public override string ToString()
        {
            return string.Concat(new object[] { "BB(", this.Period, ", ", PriceType.All[this.BasedOn].Name, ")" });
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
                        series.Brush.Color = Color.DarkOrange;
                        Line line2 = new Line();
                        line2.Brush.Color = Color.DarkOrange;
                        this.chartSeries.Add(series);
                        this.chartSeries.Add(line2);
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

        public IDoubleSeries Lower
        {
            get
            {
                return base.GetSeries(1);
            }
        }

        public override string Name
        {
            get
            {
                return "Bollinger Bands";
            }
        }

        [Parameter(0.0, double.MaxValue, "# std. dev.")]
        public double NumStdDev
        {
            get
            {
                return this.numStdDev;
            }
            set
            {
                if (this.numStdDev <= 0.0)
                {
                    throw new ArgumentOutOfRangeException("Period", value, "Indicator.Bollinger: property out of range");
                }
                this.numStdDev = value;
            }
        }

        [Parameter(1, 0x7fffffff, "# periods")]
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
                    throw new ArgumentOutOfRangeException("Period", value, "Indicator.Bollinger: property out of range");
                }
                this.period = this.stdDev.Period = this.sma.Period = value;
            }
        }

        public override int UnstablePeriod
        {
            get
            {
                return this.Period;
            }
        }

        public IDoubleSeries Upper
        {
            get
            {
                return base.GetSeries(0);
            }
        }
    }
}

