namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    public abstract class BaseLine : Series
    {
        protected ChartPen pLinePen;

        protected BaseLine() : this(null)
        {
        }

        protected BaseLine(Chart c) : base(c)
        {
            this.pLinePen = new ChartPen(Color.Empty);
        }

        protected override void SetChart(Chart c)
        {
            base.SetChart(c);
            this.LinePen.Chart = base.chart;
        }

        [Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Determines pen to draw the line connecting all points.")]
        public ChartPen LinePen
        {
            get
            {
                return this.pLinePen;
            }
        }
    }
}

