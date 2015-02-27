namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    [Description("Displays draggable Cursor lines on top of Series."), ToolboxBitmap(typeof(CursorTool), "ToolsIcons.CursorTool.bmp")]
    public class CursorTool : ToolSeries
    {
        private CursorClicked dragging;
        private bool followMouse;
        private Point IPoint;
        private double IXValue;
        private double IYValue;
        private bool snap;
        private CursorToolStyles style;
        private bool useChartRect;

        public event CursorChangeEventHandler Change;

        public event CursorGetAxisRectEventHandler GetAxisRect;

        public CursorTool() : this(null)
        {
        }

        public CursorTool(Chart c) : base(c)
        {
            this.style = CursorToolStyles.Both;
            this.dragging = CursorClicked.None;
            this.IPoint = new Point(-1, -1);
        }

        protected void CalcScreenPositions()
        {
            if ((this.IPoint.X == -1) || (this.IPoint.Y == -1))
            {
                this.IPoint.X = (base.GetHorizAxis.IStartPos + base.GetHorizAxis.IEndPos) / 2;
                this.IPoint.Y = (base.GetVertAxis.IStartPos + base.GetVertAxis.IEndPos) / 2;
                this.CalcValuePositions(this.IPoint.X, this.IPoint.Y);
                int snapPoint = this.SnapToPoint();
                this.CalcScreenPositions();
                this.Changed(snapPoint);
            }
            else
            {
                this.IPoint.X = base.GetHorizAxis.CalcPosValue(this.IXValue);
                this.IPoint.Y = base.GetVertAxis.CalcPosValue(this.IYValue);
            }
        }

        private void CalcValuePositions(int x, int y)
        {
            switch (this.dragging)
            {
                case CursorClicked.Horizontal:
                    this.IYValue = base.GetVertAxis.CalcPosPoint(y);
                    return;

                case CursorClicked.Vertical:
                    this.IXValue = base.GetHorizAxis.CalcPosPoint(x);
                    return;
            }
            this.IXValue = base.GetHorizAxis.CalcPosPoint(x);
            this.IYValue = base.GetVertAxis.CalcPosPoint(y);
        }

        protected void Changed(int snapPoint)
        {
            if (this.Change != null)
            {
                CursorChangeEventArgs e = new CursorChangeEventArgs();
                e.x = this.IPoint.X;
                e.y = this.IPoint.Y;
                e.XValue = this.IXValue;
                e.YValue = this.IYValue;
                e.Series = base.iSeries;
                e.SnapPoint = snapPoint;
                this.Change(this, e);
            }
        }

        protected internal override void ChartEvent(EventArgs e)
        {
            base.ChartEvent(e);
            if (e is AfterDrawEventArgs)
            {
                this.CalcScreenPositions();
                this.RedrawCursor();
            }
        }

        public CursorClicked Clicked(int x, int y)
        {
            CursorClicked none = CursorClicked.None;
            if (this.InternalGetAxisRect().Contains(x, y))
            {
                if (((this.style == CursorToolStyles.Both) && (Math.Abs((int) (y - this.IPoint.Y)) < Steema.TeeChart.Tools.Tool.ClickTolerance)) && (Math.Abs((int) (x - this.IPoint.X)) < Steema.TeeChart.Tools.Tool.ClickTolerance))
                {
                    return CursorClicked.Both;
                }
                if (((this.style == CursorToolStyles.Horizontal) || (this.style == CursorToolStyles.Both)) && (Math.Abs((int) (y - this.IPoint.Y)) < Steema.TeeChart.Tools.Tool.ClickTolerance))
                {
                    return CursorClicked.Horizontal;
                }
                if ((this.style != CursorToolStyles.Vertical) && (this.style != CursorToolStyles.Both))
                {
                    return none;
                }
                if (Math.Abs((int) (x - this.IPoint.X)) < Steema.TeeChart.Tools.Tool.ClickTolerance)
                {
                    none = CursorClicked.Vertical;
                }
            }
            return none;
        }

        protected void DoGetAxisRect(ref Rectangle tmpResult)
        {
            if (this.GetAxisRect != null)
            {
                CursorGetAxisRectEventArgs e = new CursorGetAxisRectEventArgs(tmpResult);
                this.GetAxisRect(this, e);
                tmpResult = e.Rectangle;
            }
        }

        private void DrawCursorLines(bool Draw3D, Rectangle R, int X, int Y)
        {
            switch (this.style)
            {
                case CursorToolStyles.Horizontal:
                    if (Y < 0)
                    {
                        break;
                    }
                    this.DrawHorizontal(R, Y);
                    return;

                case CursorToolStyles.Vertical:
                    if (X < 0)
                    {
                        break;
                    }
                    this.DrawVertical(R, X);
                    return;

                default:
                    if (Y >= 0)
                    {
                        this.DrawHorizontal(R, Y);
                    }
                    if (X >= 0)
                    {
                        this.DrawVertical(R, X);
                    }
                    break;
            }
        }

        private void DrawHorizontal(Rectangle r, int Y)
        {
            if (base.chart.aspect.View3D)
            {
                base.chart.graphics3D.HorizontalLine(r.X, r.Right, Y, 0);
            }
            else
            {
                base.chart.graphics3D.HorizontalLine(r.X, r.Right, Y);
            }
        }

        private void DrawVertical(Rectangle r, int X)
        {
            if (base.chart.aspect.View3D)
            {
                base.chart.graphics3D.VerticalLine(X, r.Y, r.Bottom, 0);
            }
            else
            {
                base.chart.graphics3D.VerticalLine(X, r.Y, r.Bottom);
            }
        }

        private Rectangle InternalGetAxisRect()
        {
            Rectangle chartRect;
            if (this.useChartRect)
            {
                chartRect = base.chart.ChartRect;
            }
            else
            {
                chartRect = new Rectangle();
                chartRect.X = base.GetHorizAxis.IStartPos;
                chartRect.Width = base.GetHorizAxis.IEndPos - chartRect.X;
                chartRect.Y = base.GetVertAxis.IStartPos;
                chartRect.Height = base.GetVertAxis.IEndPos - chartRect.Y;
            }
            this.DoGetAxisRect(ref chartRect);
            return chartRect;
        }

        protected internal override void MouseEvent(MouseEventKinds kind, MouseEventArgs e, ref Cursor c)
        {
            if (kind == MouseEventKinds.Up)
            {
                this.dragging = CursorClicked.None;
            }
            else if (kind == MouseEventKinds.Move)
            {
                this.MouseMove(e, ref c);
            }
            else if ((kind == MouseEventKinds.Down) && !this.followMouse)
            {
                this.dragging = this.Clicked(e.X, e.Y);
                if (this.dragging != CursorClicked.None)
                {
                    base.chart.CancelMouse = true;
                }
            }
        }

        private void MouseMove(MouseEventArgs e, ref Cursor c)
        {
            if (((this.dragging != CursorClicked.None) || this.followMouse) && this.InternalGetAxisRect().Contains(e.X, e.Y))
            {
                this.RedrawCursor();
                this.CalcValuePositions(e.X, e.Y);
                int snapPoint = this.SnapToPoint();
                this.CalcScreenPositions();
                this.RedrawCursor();
                this.Changed(snapPoint);
            }
            else
            {
                CursorClicked clicked = this.Clicked(e.X, e.Y);
                switch (clicked)
                {
                    case CursorClicked.Horizontal:
                        c = Cursors.HSplit;
                        break;

                    case CursorClicked.Vertical:
                        c = Cursors.VSplit;
                        break;

                    case CursorClicked.Both:
                        c = Cursors.SizeAll;
                        break;
                }
                base.chart.CancelMouse = clicked != CursorClicked.None;
            }
        }

        [Description("Returns nearest point to Cursor and smallest distance value.")]
        public int NearestPoint(CursorToolStyles style, out double difference)
        {
            int num2;
            int num3;
            double num = 0.0;
            int num4 = -1;
            difference = -1.0;
            if (Steema.TeeChart.Tools.Tool.GetFirstLastSeries(base.iSeries, out num2, out num3))
            {
                for (int i = num2; i <= num3; i++)
                {
                    switch (style)
                    {
                        case CursorToolStyles.Horizontal:
                            num = Math.Abs((double) (this.IYValue - base.iSeries.YValues[i]));
                            break;

                        case CursorToolStyles.Vertical:
                            num = Math.Abs((double) (this.IXValue - base.iSeries.XValues[i]));
                            break;

                        default:
                            num = Math.Sqrt(Utils.Sqr(this.IXValue - base.iSeries.XValues[i]) + Utils.Sqr(this.IYValue - base.iSeries.vyValues[i]));
                            break;
                    }
                    if ((difference == -1.0) || (num < difference))
                    {
                        difference = num;
                        num4 = i;
                    }
                }
            }
            return num4;
        }

        private void RedrawCursor()
        {
            Graphics3D graphicsd = base.chart.graphics3D;
            if ((base.Chart != null) && graphicsd.ValidState())
            {
                if (base.chart.Panel.Color.IsEmpty)
                {
                    Color control = SystemColors.Control;
                }
                graphicsd.Pen = this.Pen;
                graphicsd.Brush.Visible = false;
                this.DrawCursorLines(base.chart.Aspect.View3D, this.InternalGetAxisRect(), this.IPoint.X, this.IPoint.Y);
                this.Invalidate();
            }
        }

        protected override void SetSeries(Series value)
        {
            if (base.iSeries != value)
            {
                if (base.Active && (base.iSeries != null))
                {
                    this.RedrawCursor();
                }
                base.SetSeries(value);
                if (base.iSeries != null)
                {
                    this.SnapToPoint();
                    this.CalcScreenPositions();
                    if (base.Active)
                    {
                        this.RedrawCursor();
                    }
                }
            }
        }

        [Description("Moves cursor to nearest Series point and returns point index.")]
        public int SnapToPoint()
        {
            int num2;
            if ((base.iSeries != null) && this.snap)
            {
                double num;
                num2 = this.NearestPoint(this.style, out num);
            }
            else
            {
                num2 = -1;
            }
            if (num2 != -1)
            {
                switch (this.style)
                {
                    case CursorToolStyles.Horizontal:
                        this.IYValue = base.iSeries.YValues[num2];
                        return num2;

                    case CursorToolStyles.Vertical:
                        this.IXValue = base.iSeries.XValues[num2];
                        return num2;
                }
                this.IXValue = base.iSeries.XValues[num2];
                this.IYValue = base.iSeries.YValues[num2];
            }
            return num2;
        }

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.CursorTool;
            }
        }

        [DefaultValue(false), Description("Moves Cursor when moving the mouse.")]
        public bool FollowMouse
        {
            get
            {
                return this.followMouse;
            }
            set
            {
                this.followMouse = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Element Pen characteristics."), Category("Appearance")]
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

        [Description("Aligns the TCursorTool with the nearest series point."), DefaultValue(false)]
        public bool Snap
        {
            get
            {
                return this.snap;
            }
            set
            {
                this.snap = value;
            }
        }

        [Description("Defines which lines of the CursorTool are shown."), DefaultValue(2)]
        public CursorToolStyles Style
        {
            get
            {
                return this.style;
            }
            set
            {
                if (this.style != value)
                {
                    this.style = value;
                    this.SnapToPoint();
                    this.Invalidate();
                }
            }
        }

        [Description("Gets detailed descriptive text.")]
        public override string Summary
        {
            get
            {
                return Texts.CursorToolSummary;
            }
        }

        [Description("Sets full Chart rectangle instead of boundaries defined by Series axis."), DefaultValue(false)]
        public bool UseChartRect
        {
            get
            {
                return this.useChartRect;
            }
            set
            {
                base.SetBooleanProperty(ref this.useChartRect, value);
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Description("Sets X Value for vertical component of Cursor Tool.")]
        public double XValue
        {
            get
            {
                return this.IXValue;
            }
            set
            {
                if (this.IXValue != value)
                {
                    this.IXValue = value;
                    this.CalcScreenPositions();
                    this.Invalidate();
                }
            }
        }

        [Browsable(false), Description("Sets Y Value for horizontal component of Cursor Tool."), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double YValue
        {
            get
            {
                return this.IYValue;
            }
            set
            {
                if (this.IYValue != value)
                {
                    this.IYValue = value;
                    this.CalcScreenPositions();
                    this.Invalidate();
                }
            }
        }
    }
}

