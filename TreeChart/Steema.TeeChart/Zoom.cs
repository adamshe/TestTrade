namespace Steema.TeeChart
{
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class Zoom : ZoomScroll
    {
        private bool allow;
        private bool animated;
        public static double AnimatedFactor = 3.0;
        private int animatedSteps;
        private ChartBrush bBrush;
        private ZoomDirections direction;
        private Keys keyShift;
        private int minPixels;
        private MouseButtons mouseButton;
        internal ChartPen pen;
        internal bool zoomed;

        public Zoom(Chart c) : base(c)
        {
            this.allow = true;
            this.animatedSteps = 8;
            this.direction = ZoomDirections.Both;
            this.mouseButton = MouseButtons.Left;
            this.keyShift = Keys.None;
            this.minPixels = 0x10;
        }

        private void CalcAxisScale(Axis axis, double percentZoom, out double tmpA, out double tmpB)
        {
            double minimum = axis.Minimum;
            double maximum = axis.Maximum;
            axis.CalcMinMax(ref minimum, ref maximum);
            double num3 = (maximum - minimum) * percentZoom;
            tmpA = minimum + num3;
            tmpB = maximum - num3;
        }

        internal void CalcZoomPoints()
        {
            base.Check();
            base.chart.Axes.DoZoom(base.x0, base.y0, base.x1, base.y1);
        }

        private Rectangle ClipZoomReversibleRectangle()
        {
            Point point = base.chart.parent.PointToScreen(new Point(0, 0));
            Rectangle rectangle = new Rectangle(point.X + base.x0, point.Y + base.y0, base.x1 - base.x0, base.y1 - base.y0);
            Rectangle chartBounds = base.chart.ChartBounds;
            chartBounds.X = point.X;
            chartBounds.Y = point.Y;
            rectangle.Intersect(chartBounds);
            return rectangle;
        }

        public void Draw()
        {
            if ((this.pen != null) && (this.bBrush != null))
            {
                Rectangle rectangle = this.ClipZoomReversibleRectangle();
                rectangle.Inflate(1, 1);
                int argb = -1 ^ this.bBrush.Color.ToArgb();
                int num2 = -1 ^ this.pen.Color.ToArgb();
                if (this.pen.Style == DashStyle.Solid)
                {
                    ControlPaint.DrawReversibleFrame(rectangle, Color.FromArgb(num2), FrameStyle.Thick);
                }
                else
                {
                    ControlPaint.DrawReversibleFrame(rectangle, Color.FromArgb(num2), FrameStyle.Dashed);
                }
                ControlPaint.FillReversibleRectangle(this.ClipZoomReversibleRectangle(), Color.FromArgb(argb));
            }
            else if (this.pen != null)
            {
                base.chart.Invalidate();
                Rectangle rect = new Rectangle(base.x0, base.y0, base.x1 - base.x0, base.y1 - base.y0);
                if (base.chart.graphics3D is Graphics3DGdiPlus)
                {
                    ((Graphics3DGdiPlus) base.chart.graphics3D).Graphics.DrawRectangle(this.Pen.DrawingPen, rect);
                }
            }
            else
            {
                ControlPaint.DrawReversibleFrame(this.ClipZoomReversibleRectangle(), Color.Black, FrameStyle.Dashed);
            }
        }

        protected override void SetChart(Chart c)
        {
            base.SetChart(c);
            if (this.pen != null)
            {
                this.pen.Chart = c;
            }
            if (this.bBrush != null)
            {
                this.bBrush.Chart = c;
            }
        }

        public void Undo()
        {
            base.chart.RestoreAxisScales();
            this.Zoomed = false;
        }

        public void ZoomPercent(double percentZoom)
        {
            double num;
            double num2;
            double num3;
            double num4;
            double num5;
            double num6;
            double num7;
            double num8;
            percentZoom = (percentZoom - 100.0) * 0.01;
            this.CalcAxisScale(base.chart.Axes.Left, percentZoom, out num, out num2);
            this.CalcAxisScale(base.chart.Axes.Right, percentZoom, out num3, out num4);
            this.CalcAxisScale(base.chart.Axes.Top, percentZoom, out num5, out num6);
            this.CalcAxisScale(base.chart.Axes.Bottom, percentZoom, out num7, out num8);
            base.chart.DoZoom(num5, num6, num7, num8, num, num2, num3, num4);
            this.Invalidate();
        }

        [Description("Zooms the Chart rectangle. Units pixels")]
        public void ZoomRect(Rectangle r)
        {
            base.x0 = r.Left;
            base.y0 = r.Top;
            base.x1 = r.Right;
            base.y1 = r.Bottom;
            this.CalcZoomPoints();
        }

        [DefaultValue(true), Description("Allows runtime Zoom by dragging the mouse when True.")]
        public bool Allow
        {
            get
            {
                return this.allow;
            }
            set
            {
                this.allow = value;
            }
        }

        [DefaultValue(false), Description("Animates Zoom in sequenced steps when True.")]
        public bool Animated
        {
            get
            {
                return this.animated;
            }
            set
            {
                this.animated = value;
            }
        }

        [Description("Determines number of steps for animated zooming sequence."), DefaultValue(8)]
        public int AnimatedSteps
        {
            get
            {
                return this.animatedSteps;
            }
            set
            {
                this.animatedSteps = value;
            }
        }

        [Description("Brush used to fill mousedragged zoom area."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance")]
        public ChartBrush Brush
        {
            get
            {
                if (this.bBrush == null)
                {
                    this.bBrush = new ChartBrush(base.chart);
                }
                return this.bBrush;
            }
        }

        [DefaultValue(2), Description("Sets the direction of the zoom on a selected area.")]
        public ZoomDirections Direction
        {
            get
            {
                return this.direction;
            }
            set
            {
                this.direction = value;
            }
        }

        [DefaultValue(0), Description("Sets a keyboard button as an extra condition to initiate the zoom.")]
        public Keys KeyShift
        {
            get
            {
                return this.keyShift;
            }
            set
            {
                this.keyShift = value;
            }
        }

        [Description("Sets min. number of pixels to actuate zoom action."), DefaultValue(0x10)]
        public int MinPixels
        {
            get
            {
                return this.minPixels;
            }
            set
            {
                this.minPixels = value;
            }
        }

        [Description("Sets the mousebutton to use for the zoom action."), DefaultValue(0x100000)]
        public MouseButtons MouseButton
        {
            get
            {
                return this.mouseButton;
            }
            set
            {
                this.mouseButton = value;
            }
        }

        [Description("Pen used to draw surrounding rectangle of zoom area."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance")]
        public ChartPen Pen
        {
            get
            {
                if (this.pen == null)
                {
                    this.pen = new ChartPen(Color.Black);
                }
                return this.pen;
            }
        }

        [Description("Determines if Chart axis scales fit all Chart points."), DefaultValue(false), Browsable(false)]
        public bool Zoomed
        {
            get
            {
                return this.zoomed;
            }
            set
            {
                this.zoomed = value;
                if ((base.chart.parent != null) && !this.zoomed)
                {
                    base.chart.parent.DoUndoneZoom(this, EventArgs.Empty);
                }
                this.Invalidate();
            }
        }
    }
}

