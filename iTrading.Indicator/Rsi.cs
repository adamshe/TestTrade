using iTrading.Core.Chart;
using iTrading.Core.IndicatorBase;
using PriceTypeId=iTrading.Core.Data.PriceTypeId;

namespace iTrading.Indicator
{
    using Steema.TeeChart.Drawing;
    using Steema.TeeChart.Styles;
    using System;
    using System.Drawing;
    using iTrading.Core.Data;

    public class Rsi : IndicatorBase
    {
        private iTrading.Core.Chart.SeriesCollection chartSeries;
        private DoubleSeries down;
        private double lowerLevel;
        private int period;
        private Sma smaDown;
        private Sma smaUp;
        private int smooth;
        private DoubleSeries up;
        private double upperLevel;

        public Rsi(IDoubleSeries source) : base(2, source)
        {
            this.chartSeries = new iTrading.Core.Chart.SeriesCollection();
            this.lowerLevel = 30.0;
            this.period = 14;
            this.smooth = 3;
            this.upperLevel = 70.0;
            this.down = new DoubleSeries();
            this.up = new DoubleSeries();
            this.smaDown = new Sma(this.down);
            this.smaUp = new Sma(this.up);
        }

        public Rsi(Quotes quotes) : base(2, quotes)
        {
            this.chartSeries = new iTrading.Core.Chart.SeriesCollection();
            this.lowerLevel = 30.0;
            this.period = 14;
            this.smooth = 3;
            this.upperLevel = 70.0;
            this.down = new DoubleSeries();
            this.up = new DoubleSeries();
            this.smaDown = new Sma(this.down);
            this.smaUp = new Sma(this.up);
        }

        protected  override double Calculate(int current)
        {
            if (current == 0)
            {
                this.down[current] = 0.0;
                this.up[current] = 0.0;
                base.Add(1, 50.0);
                return 50.0;
            }
            this.down[current] = Math.Max((double) (base.Source[current - 1] - base.Source[current]), (double) 0.0);
            this.up[current] = Math.Max((double) (base.Source[current] - base.Source[current - 1]), (double) 0.0);
            double num = 100.0 - (100.0 / (1.0 + (((this.smaUp[current] == 0.0) || (this.smaDown[current] == 0.0)) ? 1.0 : (this.smaUp[current] / this.smaDown[current]))));
            double indicatorValue = (IndicatorBase.Alpha(this.Smooth) * num) + ((1.0 - IndicatorBase.Alpha(this.Smooth)) * this.RsiAvg[current - 1]);
            base.Add(1, indicatorValue);
            return num;
        }

        public override string ToString()
        {
            return string.Concat(new object[] { "RSI(", this.Period, ", ", PriceType.All[this.BasedOn].Name, ")" });
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
                        series.Brush.Color = Color.Green;
                        Line line2 = new Line();
                        line2.Brush.Color = Color.Orange;
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
                return false;
            }
        }

        public override LevelCollection Levels
        {
            get
            {
                LevelCollection levels = new LevelCollection();
                levels.Add(new Level(new ChartPen(Color.DarkViolet), this.LowerLevel));
                levels.Add(new Level(new ChartPen(Color.YellowGreen), this.UpperLevel));
                return levels;
            }
        }

        [Parameter((double) 0.0, (double) 100.0, "Lower level")]
        public double LowerLevel
        {
            get
            {
                return this.lowerLevel;
            }
            set
            {
                if ((value < 0.0) || (value > 100.0))
                {
                    throw new ArgumentOutOfRangeException("LowerLevel", value, "Indicator.Rsi: property out of range");
                }
                this.lowerLevel = value;
            }
        }

        public override string Name
        {
            get
            {
                return "RSI";
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
                    throw new ArgumentOutOfRangeException("Period", value, "Indicator.Rsi: property out of range");
                }
                this.period = this.smaDown.Period = this.smaUp.Period = value;
            }
        }

        public IDoubleSeries RsiAvg
        {
            get
            {
                return base.GetSeries(1);
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
                    throw new ArgumentOutOfRangeException("Smooth", value, "Indicator.Rsi: property out of range");
                }
                this.smooth = value;
            }
        }

        public override int UnstablePeriod
        {
            get
            {
                return this.Period;
            }
        }

        [Parameter((double) 0.0, (double) 100.0, "Upper level")]
        public double UpperLevel
        {
            get
            {
                return this.upperLevel;
            }
            set
            {
                if ((value < 0.0) || (value > 100.0))
                {
                    throw new ArgumentOutOfRangeException("UpperLevel", value, "Indicator.Rsi: property out of range");
                }
                this.upperLevel = value;
            }
        }
    }
}

