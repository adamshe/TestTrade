namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    [Description("Displays clickable arrows at axis start and ending points, to scroll axis."), ToolboxBitmap(typeof(AxisArrow), "ToolsIcons.AxisArrow.bmp")]
    public class AxisArrow : ToolAxis
    {
        private int length;
        private AxisArrowPosition position;
        private bool scrollInverted;
        private int scrollPercent;

        public AxisArrow() : this((Chart) null)
        {
        }

        public AxisArrow(Axis a) : this(a.Chart)
        {
            base.iAxis = a;
        }

        public AxisArrow(Chart c) : base(c)
        {
            this.length = 0x10;
            this.position = AxisArrowPosition.Both;
            this.scrollPercent = 10;
        }

        protected internal override void ChartEvent(EventArgs e)
        {
            if ((e is AfterDrawEventArgs) && (base.iAxis != null))
            {
                base.chart.graphics3D.Brush = this.Brush;
                base.chart.graphics3D.Pen = base.Pen;
                int z = (base.chart.Aspect.View3D && base.iAxis.OtherSide) ? base.chart.Aspect.Width3D : 0;
                if ((this.position == AxisArrowPosition.Start) || (this.position == AxisArrowPosition.Both))
                {
                    this.DrawArrow(base.iAxis.IStartPos, this.length, z);
                }
                if ((this.position == AxisArrowPosition.End) || (this.position == AxisArrowPosition.Both))
                {
                    this.DrawArrow(base.Axis.IEndPos, -this.length, z);
                }
            }
        }

        private int Check(int Pos1, int Pos2)
        {
            if (Math.Abs((int) (Pos1 - base.iAxis.Position)) < Steema.TeeChart.Tools.Tool.ClickTolerance)
            {
                if (((this.position == AxisArrowPosition.Start) || (this.position == AxisArrowPosition.Both)) && ((Pos2 > base.iAxis.IStartPos) && (Pos2 < (base.iAxis.IStartPos + this.length))))
                {
                    return 0;
                }
                if (((this.position == AxisArrowPosition.End) || (this.position == AxisArrowPosition.Both)) && ((Pos2 < base.iAxis.IEndPos) && (Pos2 > (base.iAxis.IEndPos - this.length))))
                {
                    return 1;
                }
            }
            return -1;
        }

        private int ClickedArrow(int x, int y)
        {
            if (!base.iAxis.Horizontal)
            {
                return this.Check(x, y);
            }
            return this.Check(y, x);
        }

        private void DoScroll(double ADelta)
        {
            double minimum = base.iAxis.Minimum;
            double maximum = base.iAxis.Maximum;
            if ((base.chart.parent != null) && base.chart.parent.DoAllowScroll(base.iAxis, ADelta, ref minimum, ref maximum))
            {
                base.iAxis.Minimum = minimum;
                base.iAxis.Maximum = maximum;
                base.iAxis.Scroll(ADelta, false);
                if (base.chart.parent != null)
                {
                    base.chart.parent.DoScroll(this, EventArgs.Empty);
                }
            }
        }

        private void DrawArrow(int APos, int ALength, int z)
        {
            Point point;
            Point point2;
            if (base.iAxis.Horizontal)
            {
                point = new Point(APos + ALength, base.iAxis.Position);
                point2 = new Point(APos, base.iAxis.Position);
            }
            else
            {
                point = new Point(base.iAxis.Position, APos + ALength);
                point2 = new Point(base.iAxis.Position, APos);
            }
            base.chart.graphics3D.Arrow(true, point, point2, 8, 8, z);
        }

        protected internal override void MouseEvent(MouseEventKinds kind, MouseEventArgs e, ref Cursor c)
        {
            if ((base.iAxis != null) && base.iAxis.Visible)
            {
                if (kind != MouseEventKinds.Down)
                {
                    if ((kind == MouseEventKinds.Move) && (this.ClickedArrow(e.X, e.Y) != -1))
                    {
                        c = Cursors.Hand;
                        base.chart.CancelMouse = true;
                    }
                }
                else if (this.scrollPercent != 0)
                {
                    int num = this.ClickedArrow(e.X, e.Y);
                    double aDelta = ((base.iAxis.Maximum - base.iAxis.Minimum) * this.scrollPercent) * 0.01;
                    if (this.scrollInverted)
                    {
                        aDelta = -aDelta;
                    }
                    switch (num)
                    {
                        case 0:
                            this.DoScroll(aDelta);
                            break;

                        case 1:
                            this.DoScroll(-aDelta);
                            break;
                    }
                    if ((num == 0) || (num == 1))
                    {
                        base.chart.CancelMouse = true;
                    }
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Element Brush characteristics."), Category("Appearance")]
        public ChartBrush Brush
        {
            get
            {
                if (base.bBrush == null)
                {
                    base.bBrush = new ChartBrush(base.chart);
                }
                return base.bBrush;
            }
        }

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.AxisArrowTool;
            }
        }

        [Description("Sets the length of the Arrows."), DefaultValue(0x10)]
        public int Length
        {
            get
            {
                return this.length;
            }
            set
            {
                base.SetIntegerProperty(ref this.length, value);
            }
        }

        [DefaultValue(2), Description("Sets where the arrows are drawn on the Axis.")]
        public AxisArrowPosition Position
        {
            get
            {
                return this.position;
            }
            set
            {
                if (this.position != value)
                {
                    this.position = value;
                    this.Invalidate();
                }
            }
        }

        [DefaultValue(false), Description("Reverses direction of applied Axis Arrow scroll.")]
        public bool ScrollInverted
        {
            get
            {
                return this.scrollInverted;
            }
            set
            {
                this.scrollInverted = value;
            }
        }

        [Description("Sets TChart scroll rate as percentage of the associated axis."), DefaultValue(10)]
        public int ScrollPercent
        {
            get
            {
                return this.scrollPercent;
            }
            set
            {
                this.scrollPercent = value;
            }
        }

        [Description("Gets detailed descriptive text.")]
        public override string Summary
        {
            get
            {
                return Texts.AxisArrowSummary;
            }
        }
    }
}

