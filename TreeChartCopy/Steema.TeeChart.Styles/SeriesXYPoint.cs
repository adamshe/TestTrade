namespace Steema.TeeChart.Styles
{
    using System;
    using System.ComponentModel;

    public class SeriesXYPoint : SeriesPoint
    {
        public SeriesXYPoint()
        {
        }

        public SeriesXYPoint(Series s, int i)
        {
            base.index = i;
            base.series = s;
        }

        [Description("Indexed Point X value")]
        public double X
        {
            get
            {
                return base.series.XValues[base.index];
            }
            set
            {
                base.series.XValues[base.index] = value;
                base.series.Invalidate();
            }
        }

        [Description("Indexed Point Y value")]
        public double Y
        {
            get
            {
                return base.series.YValues[base.index];
            }
            set
            {
                base.series.YValues[base.index] = value;
                base.series.Invalidate();
            }
        }
    }
}

