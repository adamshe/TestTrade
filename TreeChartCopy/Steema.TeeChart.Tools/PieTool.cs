namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    [ToolboxBitmap(typeof(PieTool), "ToolsIcons.PieTool.bmp"), Description("Outlines or expands Pie slices when moving or clicking with mouse.")]
    public class PieTool : ToolSeries
    {
        private int iSlice;
        private PieToolStyle style;

        public PieTool() : this(null)
        {
        }

        public PieTool(Chart c) : base(c)
        {
            this.style = PieToolStyle.Focus;
            this.iSlice = -1;
            this.Pen.Width = 3;
        }

        private void FocusSlice(int valueIndex, bool focused)
        {
            Pie series = (Pie) base.Series;
            if (this.style == PieToolStyle.Explode)
            {
                int num = focused ? 1 : -1;
                for (int i = 1; i <= 20; i++)
                {
                    series.ExplodedSlice[valueIndex] = (2 * i) * num;
                    base.Chart.Invalidate();
                }
            }
            else if (!focused)
            {
                base.Chart.parent.RefreshControl();
            }
            else
            {
                int num4;
                int num5;
                Color white = this.Pen.Color;
                if (base.Series.ValueColor(valueIndex) == this.Pen.Color)
                {
                    if (this.Pen.Color == Color.Black)
                    {
                        white = Color.White;
                    }
                    else
                    {
                        white = Color.Black;
                    }
                }
                if (!base.chart.graphics3D.ValidState())
                {
                    Control control = base.chart.parent.GetControl();
                    if ((control == null) || !(base.chart.graphics3D is Graphics3DGdiPlus))
                    {
                        return;
                    }
                    ((Graphics3DGdiPlus) base.chart.graphics3D).g = control.CreateGraphics();
                }
                base.chart.graphics3D.Pen = this.Pen;
                base.chart.graphics3D.Pen.Color = white;
                int index = (valueIndex == 0) ? (base.Series.Count - 1) : (valueIndex - 1);
                series.AngleToPos(series.Angles[index].EndAngle, (double) series.XRadius, (double) series.YRadius, out num4, out num5);
                series.DrawPie(valueIndex);
            }
        }

        protected internal override void MouseEvent(MouseEventKinds kind, MouseEventArgs e, ref Cursor c)
        {
            if ((kind == MouseEventKinds.Move) && (base.Series != null))
            {
                int num = base.Series.Clicked(e.X, e.Y);
                if (this.iSlice != num)
                {
                    if (this.iSlice != -1)
                    {
                        this.FocusSlice(this.iSlice, false);
                    }
                    this.iSlice = num;
                    if (this.iSlice != -1)
                    {
                        this.FocusSlice(this.iSlice, true);
                    }
                }
            }
        }

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.PieTool;
            }
        }

        [Description("Element Pen characteristics."), Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ChartPen Pen
        {
            get
            {
                if (base.pPen == null)
                {
                    base.pPen = new ChartPen(base.chart, Color.Black);
                }
                return base.pPen;
            }
        }

        public int Slice
        {
            get
            {
                return this.iSlice;
            }
        }

        [DefaultValue(0)]
        public PieToolStyle Style
        {
            get
            {
                return this.style;
            }
            set
            {
                this.style = value;
            }
        }

        [Description("Gets detailed descriptive text.")]
        public override string Summary
        {
            get
            {
                return Texts.PieToolSummary;
            }
        }
    }
}

