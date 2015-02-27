namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    [Description("Allows drawing custom lines by dragging the mouse."), ToolboxBitmap(typeof(DrawLine), "ToolsIcons.DrawLine.bmp")]
    public class DrawLine : ToolSeries
    {
        private MouseButtons button;
        private bool drawing;
        private bool enableDraw;
        private bool enableSelect;
        public Point FromPoint;
        protected DrawLineHandle IHandle;
        private DrawLines lines;
        private Point point;
        private DrawLineItem selected;
        public Point ToPoint;

        public event DrawLineEventHandler DraggedLine;

        public event DrawLineEventHandler DragLine;

        public event DrawLineEventHandler NewLine;

        public event DrawLineEventHandler Select;

        public event DrawLineSelectingEventHandler Selecting;

        public DrawLine() : this(null)
        {
        }

        public DrawLine(Chart c) : base(c)
        {
            this.button = MouseButtons.Left;
            this.enableDraw = true;
            this.enableSelect = true;
            this.lines = new DrawLines();
            this.IHandle = DrawLineHandle.None;
            this.lines.tool = this;
        }

        public Point AxisPoint(PointDouble p)
        {
            return new Point(base.GetHorizAxis.CalcPosValue(p.X), base.GetVertAxis.CalcPosValue(p.Y));
        }

        protected internal override void ChartEvent(EventArgs e)
        {
            base.ChartEvent(e);
            if ((e is AfterDrawEventArgs) && (this.lines.Count > 0))
            {
                this.ClipDrawingRegion();
                foreach (DrawLineItem item in this.lines)
                {
                    base.chart.graphics3D.Pen = this.Pen;
                    this.DoDrawLine(this.AxisPoint(item.StartPos), this.AxisPoint(item.EndPos), item.Style);
                }
                if (this.selected != null)
                {
                    this.selected.DrawHandles();
                }
                base.chart.graphics3D.UnClip();
            }
        }

        private bool CheckCursor(int x, int y, ref Cursor c)
        {
            if ((this.selected != null) && ((this.InternalClicked(x, y, DrawLineHandle.Start) == this.selected) || (this.InternalClicked(x, y, DrawLineHandle.End) == this.selected)))
            {
                c = Cursors.Cross;
                return true;
            }
            if (this.Clicked(x, y) != null)
            {
                c = Cursors.Hand;
                return true;
            }
            return false;
        }

        public DrawLineItem Clicked(int x, int y)
        {
            DrawLineItem item = this.InternalClicked(x, y, DrawLineHandle.None);
            if (item == null)
            {
                item = this.InternalClicked(x, y, DrawLineHandle.Start);
            }
            if (item == null)
            {
                item = this.InternalClicked(x, y, DrawLineHandle.End);
            }
            return item;
        }

        private bool ClickedLine(DrawLineItem line, DrawLineHandle handle, Point P)
        {
            Point point = this.AxisPoint(line.StartPos);
            Point point2 = this.AxisPoint(line.EndPos);
            switch (handle)
            {
                case DrawLineHandle.Start:
                    return line.StartHandle.Contains(P.X, P.Y);

                case DrawLineHandle.End:
                    return line.EndHandle.Contains(P.X, P.Y);
            }
            switch (line.Style)
            {
                case DrawLineStyle.Line:
                    return Graphics3D.PointInLineTolerance(P, point.X, point.Y, point2.X, point2.Y, Steema.TeeChart.Tools.Tool.ClickTolerance);

                case DrawLineStyle.HorizParallel:
                {
                    bool flag = Graphics3D.PointInLineTolerance(P, point.X, point.Y, point2.X, point.Y, Steema.TeeChart.Tools.Tool.ClickTolerance);
                    if (!flag)
                    {
                        flag = Graphics3D.PointInLineTolerance(P, point.X, point2.Y, point2.X, point2.Y, Steema.TeeChart.Tools.Tool.ClickTolerance);
                    }
                    return flag;
                }
            }
            bool flag2 = Graphics3D.PointInLineTolerance(P, point.X, point.Y, point.X, point2.Y, Steema.TeeChart.Tools.Tool.ClickTolerance);
            if (!flag2)
            {
                flag2 = Graphics3D.PointInLineTolerance(P, point2.X, point.Y, point2.X, point2.Y, Steema.TeeChart.Tools.Tool.ClickTolerance);
            }
            return flag2;
        }

        protected void ClipDrawingRegion()
        {
            Rectangle chartRect;
            if (base.iSeries != null)
            {
                chartRect = new Rectangle();
                chartRect.X = base.GetHorizAxis.IStartPos;
                chartRect.Y = base.GetVertAxis.IStartPos;
                chartRect.Width = base.GetHorizAxis.IEndPos - chartRect.X;
                chartRect.Height = base.GetVertAxis.IEndPos - chartRect.Y;
            }
            else
            {
                chartRect = base.chart.ChartRect;
            }
            if (base.chart.CanClip())
            {
                base.chart.graphics3D.ClipCube(chartRect, 0, base.chart.Aspect.Width3D);
            }
        }

        public void DeleteSelected()
        {
            if (this.selected != null)
            {
                this.drawing = false;
                this.IHandle = DrawLineHandle.None;
                this.lines.Remove(this.selected);
            }
        }

        private void DoDrawLine(Point StartPos, Point EndPos, DrawLineStyle s)
        {
            Graphics3D graphicsd = base.chart.graphics3D;
            if (base.chart.aspect.view3D)
            {
                switch (s)
                {
                    case DrawLineStyle.Line:
                        graphicsd.MoveTo(StartPos, 0);
                        graphicsd.LineTo(EndPos, 0);
                        return;

                    case DrawLineStyle.HorizParallel:
                        graphicsd.HorizontalLine(StartPos.X, EndPos.X, StartPos.Y, 0);
                        graphicsd.HorizontalLine(StartPos.X, EndPos.X, EndPos.Y, 0);
                        return;
                }
                graphicsd.VerticalLine(StartPos.X, StartPos.Y, EndPos.Y, 0);
                graphicsd.VerticalLine(EndPos.X, StartPos.Y, EndPos.Y, 0);
            }
            else
            {
                switch (s)
                {
                    case DrawLineStyle.Line:
                        graphicsd.Line(StartPos.X, StartPos.Y, EndPos.X, EndPos.Y);
                        return;

                    case DrawLineStyle.HorizParallel:
                        graphicsd.HorizontalLine(StartPos.X, EndPos.X, StartPos.Y);
                        graphicsd.HorizontalLine(StartPos.X, EndPos.X, EndPos.Y);
                        return;
                }
                graphicsd.VerticalLine(StartPos.X, StartPos.Y, EndPos.Y);
                graphicsd.VerticalLine(EndPos.X, StartPos.Y, EndPos.Y);
            }
        }

        private void DoMouseDown(MouseEventArgs e, ref Cursor c)
        {
            if (e.Button == this.button)
            {
                DrawLineItem line = this.enableSelect ? this.Clicked(e.X, e.Y) : null;
                if (line != null)
                {
                    this.FromPoint = this.AxisPoint(line.StartPos);
                    this.ToPoint = this.AxisPoint(line.EndPos);
                    this.IHandle = DrawLineHandle.Series;
                    this.point = new Point(e.X, e.Y);
                    if (line != this.selected)
                    {
                        bool allow = true;
                        if (this.Selecting != null)
                        {
                            this.Selecting(this, line, ref allow);
                        }
                        if (allow)
                        {
                            if (this.selected != null)
                            {
                                this.selected = line;
                                this.Invalidate();
                            }
                            else
                            {
                                this.selected = line;
                                this.selected.DrawHandles();
                            }
                            if (this.Select != null)
                            {
                                this.Select(this);
                            }
                        }
                    }
                    else if (this.InternalClicked(e.X, e.Y, DrawLineHandle.Start) != null)
                    {
                        this.IHandle = DrawLineHandle.Start;
                    }
                    else if (this.InternalClicked(e.X, e.Y, DrawLineHandle.End) != null)
                    {
                        this.IHandle = DrawLineHandle.End;
                    }
                    base.chart.CancelMouse = true;
                }
                else
                {
                    this.selected = null;
                    if (this.enableDraw)
                    {
                        this.drawing = true;
                        this.FromPoint = new Point(e.X, e.Y);
                        this.ToPoint = this.FromPoint;
                        this.RedrawLine(this.selected);
                        base.chart.CancelMouse = true;
                    }
                }
            }
        }

        private void DoMouseMove(MouseEventArgs e, ref Cursor c)
        {
            if (this.drawing || (this.IHandle != DrawLineHandle.None))
            {
                this.Invalidate();
                Point point = new Point(e.X, e.Y);
                if (this.drawing)
                {
                    this.ToPoint = point;
                }
                else if (this.IHandle == DrawLineHandle.Start)
                {
                    this.FromPoint = point;
                }
                else if (this.IHandle == DrawLineHandle.End)
                {
                    this.ToPoint = point;
                }
                else if (this.IHandle == DrawLineHandle.Series)
                {
                    this.FromPoint.X += e.X - this.point.X;
                    this.FromPoint.Y += e.Y - this.point.Y;
                    this.ToPoint.X += e.X - this.point.X;
                    this.ToPoint.Y += e.Y - this.point.Y;
                    this.point = point;
                }
                this.RedrawLine(this.selected);
                base.chart.CancelMouse = true;
                if (this.DragLine != null)
                {
                    this.DragLine(this);
                }
            }
            else if (this.enableSelect)
            {
                base.chart.CancelMouse = this.CheckCursor(e.X, e.Y, ref c);
            }
        }

        private void DoMouseUp(MouseEventArgs e, ref Cursor c)
        {
            if (e.Button == this.button)
            {
                if (this.IHandle != DrawLineHandle.None)
                {
                    if ((this.IHandle == DrawLineHandle.Start) || (this.IHandle == DrawLineHandle.Series))
                    {
                        this.selected.StartPos = this.ScreenPoint(this.FromPoint);
                    }
                    if ((this.IHandle == DrawLineHandle.End) || (this.IHandle == DrawLineHandle.Series))
                    {
                        this.selected.EndPos = this.ScreenPoint(this.ToPoint);
                    }
                    this.IHandle = DrawLineHandle.None;
                    this.Invalidate();
                    if (this.DraggedLine != null)
                    {
                        this.DraggedLine(this);
                    }
                }
                else if (this.drawing)
                {
                    if ((this.FromPoint.X != this.ToPoint.X) || (this.FromPoint.Y != this.ToPoint.Y))
                    {
                        DrawLineItem item = new DrawLineItem(this);
                        item.StartPos = this.ScreenPoint(this.FromPoint);
                        item.EndPos = this.ScreenPoint(this.ToPoint);
                        this.Invalidate();
                        if (this.NewLine != null)
                        {
                            this.NewLine(this);
                        }
                    }
                    this.drawing = false;
                }
            }
        }

        private DrawLineItem InternalClicked(int X, int Y, DrawLineHandle handle)
        {
            base.chart.graphics3D.Calculate2DPosition(ref X, ref Y, 0);
            Point p = new Point(X, Y);
            if ((this.selected != null) && this.ClickedLine(this.selected, handle, p))
            {
                return this.selected;
            }
            foreach (DrawLineItem item in this.lines)
            {
                if (this.ClickedLine(item, handle, p))
                {
                    return item;
                }
            }
            return null;
        }

        protected internal override void MouseEvent(MouseEventKinds kind, MouseEventArgs e, ref Cursor c)
        {
            if (kind == MouseEventKinds.Down)
            {
                this.DoMouseDown(e, ref c);
            }
            else if (kind == MouseEventKinds.Move)
            {
                this.DoMouseMove(e, ref c);
            }
            else if (kind == MouseEventKinds.Up)
            {
                this.DoMouseUp(e, ref c);
            }
        }

        private void RedrawLine(DrawLineItem line)
        {
            Graphics3D graphicsd = base.chart.graphics3D;
            if ((base.Chart != null) && graphicsd.ValidState())
            {
                graphicsd.Pen = this.Pen;
                DrawLineStyle s = (line == null) ? DrawLineStyle.Line : line.Style;
                this.DoDrawLine(this.FromPoint, this.ToPoint, s);
            }
        }

        public PointDouble ScreenPoint(Point p)
        {
            return new PointDouble(base.GetHorizAxis.CalcPosPoint(p.X), base.GetVertAxis.CalcPosPoint(p.Y));
        }

        [DefaultValue(0x100000), Description("Defines which mousebutton activates the DrawLineTool.")]
        public MouseButtons Button
        {
            get
            {
                return this.button;
            }
            set
            {
                this.button = value;
            }
        }

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.DrawLineTool;
            }
        }

        [Description("Enables/Disables drawing of lines on the chart by the user."), DefaultValue(true)]
        public bool EnableDraw
        {
            get
            {
                return this.enableDraw;
            }
            set
            {
                this.enableDraw = value;
            }
        }

        [Description("Enables selection of lines for repositioning on the Chart."), DefaultValue(true)]
        public bool EnableSelect
        {
            get
            {
                return this.enableSelect;
            }
            set
            {
                this.enableSelect = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Description("Returns the list of lines drawn on the Chart."), Browsable(false)]
        public DrawLines Lines
        {
            get
            {
                return this.lines;
            }
        }

        [Description("Element Pen characteristics."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance")]
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

        [Browsable(false), Description("Returns the line or lines that are currently selected."), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DrawLineItem Selected
        {
            get
            {
                return this.selected;
            }
            set
            {
                if (this.selected != value)
                {
                    this.selected = value;
                    this.Invalidate();
                }
            }
        }

        [Description("Gets detailed descriptive text.")]
        public override string Summary
        {
            get
            {
                return Texts.DrawLineSummary;
            }
        }
    }
}

