namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;

    [ToolboxBitmap(typeof(GridBand), "ToolsIcons.GridBand.bmp"), Description("Grid Band tool, use it to display a coloured rectangles (bands) at the grid lines of the specified axis and position.")]
    public class GridBand : ToolAxis
    {
        private ChartBrush FBand1;
        private ChartBrush FBand2;
        private ChartBrush tmpBand;

        public GridBand() : this(null)
        {
        }

        public GridBand(Chart c) : base(c)
        {
            this.Band1.Color = Color.Black;
            this.Band2.Color = Color.Black;
        }

        protected internal override void ChartEvent(EventArgs e)
        {
            base.ChartEvent(e);
            if (((e is BeforeDrawSeriesEventArgs) && (base.chart != null)) && (base.Axis != null))
            {
                this.DrawGrids();
            }
        }

        private void DrawBand(int tmpPos1, int tmpPos2)
        {
            Rectangle rectangle;
            Rectangle chartRect = base.chart.ChartRect;
            Graphics3D graphicsd = base.chart.graphics3D;
            graphicsd.Brush = this.tmpBand;
            graphicsd.Pen.Visible = false;
            if (base.iAxis.horizontal)
            {
                rectangle = Rectangle.FromLTRB(tmpPos1, chartRect.Top, tmpPos2, chartRect.Bottom);
            }
            else
            {
                rectangle = Rectangle.FromLTRB(chartRect.Left + 1, tmpPos1, chartRect.Right, tmpPos2 + 1);
            }
            if (base.chart.Aspect.View3D)
            {
                base.chart.Graphics3D.Rectangle(rectangle, base.chart.Aspect.Width3D);
            }
            else
            {
                rectangle.Inflate(1, 0);
                base.chart.Graphics3D.Rectangle(rectangle);
            }
        }

        private void DrawGrids()
        {
            if (base.Active)
            {
                int tmpNumTicks = base.iAxis.FAxisDraw.tmpNumTicks;
                if (tmpNumTicks > 0)
                {
                    this.tmpBand = this.Band1;
                    if (base.iAxis.horizontal)
                    {
                        if (base.iAxis.FAxisDraw.tmpTicks[0] < base.iAxis.IEndPos)
                        {
                            this.DrawBand(base.iAxis.IEndPos - 1, base.iAxis.FAxisDraw.tmpTicks[0]);
                            this.tmpBand = this.Band2;
                        }
                    }
                    else if (base.iAxis.FAxisDraw.tmpTicks[0] > base.iAxis.IStartPos)
                    {
                        this.DrawBand(base.iAxis.IStartPos + 1, base.iAxis.FAxisDraw.tmpTicks[0]);
                        this.tmpBand = this.Band2;
                    }
                    for (int i = 1; i < tmpNumTicks; i++)
                    {
                        this.DrawBand(base.iAxis.FAxisDraw.tmpTicks[i - 1], base.iAxis.FAxisDraw.tmpTicks[i]);
                        if (this.tmpBand == this.Band1)
                        {
                            this.tmpBand = this.Band2;
                        }
                        else
                        {
                            this.tmpBand = this.Band1;
                        }
                    }
                    if (base.iAxis.horizontal)
                    {
                        if (base.iAxis.FAxisDraw.tmpTicks[tmpNumTicks - 1] > base.iAxis.IStartPos)
                        {
                            this.DrawBand(base.iAxis.FAxisDraw.tmpTicks[tmpNumTicks - 1], base.iAxis.IStartPos);
                        }
                    }
                    else if (base.iAxis.FAxisDraw.tmpTicks[tmpNumTicks - 1] < base.iAxis.IEndPos)
                    {
                        this.DrawBand(base.iAxis.FAxisDraw.tmpTicks[tmpNumTicks - 1], base.iAxis.IEndPos);
                    }
                }
            }
        }

        [Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("The Brush characteristics of the first GridBand tool Band.")]
        public ChartBrush Band1
        {
            get
            {
                if (this.FBand1 == null)
                {
                    this.FBand1 = new ChartBrush(base.chart);
                }
                if (this.FBand1.Solid)
                {
                    this.FBand1.Style = HatchStyle.BackwardDiagonal;
                }
                if (base.chart != null)
                {
                    base.chart.Invalidate();
                }
                return this.FBand1;
            }
        }

        [Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("The Brush characteristics of the second GridBand tool Band.")]
        public ChartBrush Band2
        {
            get
            {
                if (this.FBand2 == null)
                {
                    this.FBand2 = new ChartBrush(base.chart);
                }
                if (this.FBand2.Solid)
                {
                    this.FBand2.Style = HatchStyle.BackwardDiagonal;
                }
                if (base.chart != null)
                {
                    base.chart.Invalidate();
                }
                return this.FBand2;
            }
        }

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.GridBandTool;
            }
        }

        [Description("Gets detailed descriptive text.")]
        public override string Summary
        {
            get
            {
                return Texts.GridBandSummary;
            }
        }
    }
}

