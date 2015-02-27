namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    [ToolboxBitmap(typeof(ExtraLegend), "ToolsIcons.ExtraLegend.bmp"), Description("Displays a custom legend at any location inside Chart.")]
    public class ExtraLegend : ToolSeries
    {
        private Steema.TeeChart.Legend FLegend;

        public ExtraLegend() : this(null)
        {
        }

        public ExtraLegend(Chart c) : base(c)
        {
        }

        protected internal override void ChartEvent(EventArgs e)
        {
            base.ChartEvent(e);
            if (((e is AfterDrawEventArgs) && (base.chart != null)) && (base.Series != null))
            {
                this.Legend.Series = base.Series;
                if (this.Legend.Visible)
                {
                    this.DrawExtraLegend();
                }
            }
        }

        private void DrawExtraLegend()
        {
            Rectangle chartRect = base.chart.ChartRect;
            Steema.TeeChart.Legend legend = base.chart.Legend;
            base.chart.legend = this.Legend;
            base.chart.DoDrawLegend(ref chartRect);
            base.chart.legend = legend;
        }

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.ExtraLegendTool;
            }
        }

        [Description("Gets or sets the Legend characteristics.")]
        public Steema.TeeChart.Legend Legend
        {
            get
            {
                if (this.FLegend == null)
                {
                    this.FLegend = new Steema.TeeChart.Legend(base.chart);
                    this.FLegend.CustomPosition = true;
                    this.FLegend.LegendStyle = LegendStyles.Values;
                }
                return this.FLegend;
            }
            set
            {
                this.FLegend.Assign(value);
            }
        }

        [Description("Gets detailed descriptive text.")]
        public override string Summary
        {
            get
            {
                return Texts.ExtraLegendSummary;
            }
        }
    }
}

