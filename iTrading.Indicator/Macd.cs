using iTrading.Core.Chart;
using iTrading.Core.IndicatorBase;

namespace iTrading.Indicator
{
    using Steema.TeeChart.Drawing;
    using Steema.TeeChart.Styles;
    using System;
    using System.Drawing;
    using iTrading.Core.Data;

    public class Macd : IndicatorBase
    {
        private iTrading.Core.Chart.SeriesCollection chartSeries;
        private int fast;
        private DoubleSeries fastEma;
        private int slow;
        private DoubleSeries slowEma;
        private int smooth;

        public Macd(IDoubleSeries source) : base(3, source, PriceTypeId.Close)
        {
            this.chartSeries = new iTrading.Core.Chart.SeriesCollection();
            this.fast = 12;
            this.slow = 0x1a;
            this.smooth = 9;
            this.fastEma = new DoubleSeries();
            this.slowEma = new DoubleSeries();
        }

        public Macd(Quotes quotes) : base(3, quotes, PriceTypeId.Close)
        {
            this.chartSeries = new iTrading.Core.Chart.SeriesCollection();
            this.fast = 12;
            this.slow = 0x1a;
            this.smooth = 9;
            this.fastEma = new DoubleSeries();
            this.slowEma = new DoubleSeries();
        }

        protected  override double Calculate(int current)
        {
            if (current == 0)
            {
                this.fastEma[current] = base.Source[current];
                this.slowEma[current] = base.Source[current];
                base.Add(1, 0.0);
                base.Add(2, 0.0);
                return 0.0;
            }
            this.fastEma[current] = (IndicatorBase.Alpha(this.Fast) * base.Source[current]) + ((1.0 - IndicatorBase.Alpha(this.Fast)) * this.fastEma[current - 1]);
            this.slowEma[current] = (IndicatorBase.Alpha(this.Slow) * base.Source[current]) + ((1.0 - IndicatorBase.Alpha(this.Slow)) * this.slowEma[current - 1]);
            double num = this.fastEma[current] - this.slowEma[current];
            double indicatorValue = (IndicatorBase.Alpha(this.Smooth) * num) + ((1.0 - IndicatorBase.Alpha(this.Smooth)) * this.MacdAvg[current - 1]);
            base.Add(1, indicatorValue);
            base.Add(2, num - indicatorValue);
            return num;
        }

        public override string ToString()
        {
            return string.Concat(new object[] { "MACD(", this.Fast, ",", this.Slow, ",", this.Smooth, ")" });
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
                        series.Brush.Color = Color.Green;
                        Line line2 = new Line();
                        line2.Brush.Color = Color.DarkViolet;
                        Steema.TeeChart.Styles.Bar bar = new Steema.TeeChart.Styles.Bar();
                        bar.BarWidthPercent = 0;
                        bar.Pen.Visible = false;
                        bar.Brush.Color = Color.Yellow;
                        bar.Marks.Visible = false;
                        this.chartSeries.Add(series);
                        this.chartSeries.Add(line2);
                        this.chartSeries.Add(bar);
                    }
                    return this.chartSeries;
                }
            }
        }

        [Parameter(1, 0x7fffffff, "Fast EMA")]
        public int Fast
        {
            get
            {
                return this.fast;
            }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("Fast", value, "Indicator.Macd: property out of range");
                }
                this.fast = value;
            }
        }

        public override bool IsPriceIndicator
        {
            get
            {
                return false;
            }
        }

        public override LevelCollection Levels
        {
            get
            {
                LevelCollection levels = new LevelCollection();
                levels.Add(new Level(new ChartPen(Color.DarkGray), 0.0));
                return levels;
            }
        }

        public IDoubleSeries MacdAvg
        {
            get
            {
                return base.GetSeries(1);
            }
        }

        public IDoubleSeries MacdDiff
        {
            get
            {
                return base.GetSeries(2);
            }
        }

        public override string Name
        {
            get
            {
                return "MACD";
            }
        }

        [Parameter(1, 0x7fffffff, "Slow EMA")]
        public int Slow
        {
            get
            {
                return this.slow;
            }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("Slow", value, "Indicator.Macd: property out of range");
                }
                this.slow = value;
            }
        }

        [Parameter(1, 0x7fffffff, "Smooth")]
        public int Smooth
        {
            get
            {
                return this.smooth;
            }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("Smooth", value, "Indicator.Macd: property out of range");
                }
                this.smooth = value;
            }
        }

        public override int UnstablePeriod
        {
            get
            {
                return Math.Max(this.Fast, this.Slow);
            }
        }
    }
}

