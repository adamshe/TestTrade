namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    [Description("Displays a picture using the specified Series axes as boundaries. When the axis are zoomed or scrolled, the picture will follow the new boundaries."), ToolboxBitmap(typeof(ChartImage), "ToolsIcons.ChartImage.bmp")]
    public class ChartImage : ToolSeries
    {
        private System.Drawing.Image image;
        private Steema.TeeChart.Drawing.ImageMode imageMode;

        public ChartImage() : this(null)
        {
        }

        public ChartImage(Chart c) : base(c)
        {
            this.imageMode = Steema.TeeChart.Drawing.ImageMode.Stretch;
        }

        protected internal override void ChartEvent(EventArgs e)
        {
            base.ChartEvent(e);
            if ((e is BeforeDrawSeriesEventArgs) && (this.image != null))
            {
                Rectangle r = new Rectangle();
                if (base.Series != null)
                {
                    r.X = base.Series.CalcXPosValue(base.Series.MinXValue());
                    r.Width = base.Series.CalcXPosValue(base.Series.MaxXValue()) - r.X;
                    r.Y = base.Series.CalcYPosValue(base.Series.MaxYValue());
                    r.Height = base.Series.CalcYPosValue(base.Series.MinYValue()) - r.Y;
                }
                else
                {
                    r.X = base.GetHorizAxis.CalcPosValue(base.GetHorizAxis.Minimum);
                    r.Width = base.GetHorizAxis.CalcPosValue(base.GetHorizAxis.Maximum) - r.X;
                    r.Y = base.GetVertAxis.CalcYPosValue(base.GetVertAxis.Maximum);
                    r.Height = base.GetVertAxis.CalcYPosValue(base.GetVertAxis.Minimum) - r.Y;
                }
                Graphics3D graphicsd = base.chart.graphics3D;
                if (base.chart.CanClip())
                {
                    graphicsd.ClipCube(base.chart.ChartRect, 0, base.chart.Aspect.Width3D);
                }
                graphicsd.Draw(graphicsd.CalcRect3D(r, 0), this.image, this.imageMode, false);
                graphicsd.UnClip();
            }
        }

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.ImageTool;
            }
        }

        [Description("Image to draw"), DefaultValue((string) null)]
        public System.Drawing.Image Image
        {
            get
            {
                return this.image;
            }
            set
            {
                this.image = value;
                this.Invalidate();
            }
        }

        [Description("Determines how to draw image."), DefaultValue(1)]
        public Steema.TeeChart.Drawing.ImageMode ImageMode
        {
            get
            {
                return this.imageMode;
            }
            set
            {
                if (this.imageMode != value)
                {
                    this.imageMode = value;
                    this.Invalidate();
                }
            }
        }

        [Description("Gets detailed descriptive text.")]
        public override string Summary
        {
            get
            {
                return Texts.ImageToolSummary;
            }
        }
    }
}

