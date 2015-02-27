namespace Steema.TeeChart.Styles
{
    using System;
    using System.ComponentModel;
    using System.Drawing;

    public class SeriesPoint
    {
        internal int index;
        internal Series series;

        public SeriesPoint()
        {
        }

        public SeriesPoint(Series s, int i)
        {
            this.index = i;
            this.series = s;
        }

        [Description("Indexed Point Color")]
        public System.Drawing.Color Color
        {
            get
            {
                return this.series.Colors[this.index];
            }
            set
            {
                this.series.Colors[this.index] = value;
                this.series.Invalidate();
            }
        }

        [Description("Indexed Point Label")]
        public string Label
        {
            get
            {
                return this.series.Labels[this.index];
            }
            set
            {
                this.series.Labels[this.index] = value;
                this.series.Invalidate();
            }
        }
    }
}

