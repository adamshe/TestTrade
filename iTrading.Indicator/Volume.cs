using iTrading.Core.IndicatorBase;

namespace iTrading.Indicator
{
    using Steema.TeeChart.Styles;
    using System;
    using System.Drawing;
    using iTrading.Core.Data;

    public class Volume : IndicatorBase
    {
        private iTrading.Core.Chart.SeriesCollection chartSeries;

        public Volume(Quotes quotes) : base(quotes)
        {
            this.chartSeries = new iTrading.Core.Chart.SeriesCollection();
        }

        protected  override double Calculate(int current)
        {
            return (double) base.Volume[current];
        }

        public override string ToString()
        {
            return "VOL";
        }

        public override iTrading.Core.Chart.SeriesCollection ChartSeries
        {
            get
            {
                lock (this)
                {
                    if (this.chartSeries.Count == 0)
                    {
                        Steema.TeeChart.Styles.Bar series = new Steema.TeeChart.Styles.Bar();
                        series.BarWidthPercent = 0;
                        series.Pen.Visible = false;
                        series.Brush.Color = Color.Yellow;
                        series.Marks.Visible = false;
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
                return false;
            }
        }

        public override string Name
        {
            get
            {
                return "Volume";
            }
        }

        public override int UnstablePeriod
        {
            get
            {
                return 0;
            }
        }
    }
}

