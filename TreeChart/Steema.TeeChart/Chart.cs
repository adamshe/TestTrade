namespace Steema.TeeChart
{
    using Steema.TeeChart.Data;
    using Steema.TeeChart.Drawing;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Export;
    using Steema.TeeChart.Import;
    using Steema.TeeChart.Styles;
    using Steema.TeeChart.Tools;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization;
    using System.Security.Permissions;
    using System.Windows.Forms;

    [Serializable]
    public class Chart : TeeBase, ISerializable
    {
        internal Steema.TeeChart.Drawing.Aspect aspect;
        public bool AutoRepaint;
        internal Steema.TeeChart.Axes axes;
        public bool CancelMouse;
        internal Rectangle chartBounds;
        public Rectangle ChartRect;
        public bool ClipWhenMetafiling;
        public bool ClipWhenPrinting;
        private Exports export;
        private Steema.TeeChart.Footer footer;
        internal Steema.TeeChart.Drawing.Graphics3D graphics3D;
        private Steema.TeeChart.Header header;
        internal bool IClicked;
        private Imports import;
        protected internal Steema.TeeChart.Page iPage;
        internal Steema.TeeChart.Legend legend;
        internal ChartPen legendPen;
        private TeeEventListeners listeners;
        internal int maxZOrder;
        private Steema.TeeChart.Panel panel;
        private Scroll panning;
        internal IChart parent;
        private Steema.TeeChart.Printer printer;
        internal bool printing;
        internal bool restoredAxisScales;
        private AllAxisSavedScales savedScales;
        internal SeriesCollection series;
        internal int seriesHeight3D;
        internal int seriesWidth3D;
        private Steema.TeeChart.Footer subFooter;
        private Steema.TeeChart.Header subHeader;
        private ToolsCollection tools;
        private ChartToolTip toolTip;
        private Steema.TeeChart.Walls walls;
        internal Steema.TeeChart.Zoom zoom;

        public Chart()
        {
            this.AutoRepaint = true;
            this.restoredAxisScales = true;
            this.ClipWhenMetafiling = true;
            this.ClipWhenPrinting = true;
            this.maxZOrder = 0;
            this.chartBounds = new Rectangle(0, 0, 0, 0);
            if (!base.DesignMode)
            {
                DesignTimeOptions.InitLanguage(false, false);
            }
            this.listeners = new TeeEventListeners();
            base.chart = this;
            this.series = new SeriesCollection(this);
            this.tools = new ToolsCollection(this);
            this.aspect = new Steema.TeeChart.Drawing.Aspect(this);
            this.graphics3D = new Graphics3DGdiPlus(this);
            this.panel = new Steema.TeeChart.Panel(this);
            this.legend = new Steema.TeeChart.Legend(this);
            this.header = new Steema.TeeChart.Header(this);
            this.header.defaultText = "TeeChart";
            this.header.Text = this.header.defaultText;
            this.subHeader = new Steema.TeeChart.Header(this);
            this.footer = new Steema.TeeChart.Footer(this);
            this.subFooter = new Steema.TeeChart.Footer(this);
            this.walls = new Steema.TeeChart.Walls(this);
            this.axes = new Steema.TeeChart.Axes(this);
            this.iPage = new Steema.TeeChart.Page(this);
            this.export = new Exports(this);
            this.import = new Imports(this);
            this.printer = new Steema.TeeChart.Printer(this);
        }

        public Chart(SerializationInfo info, StreamingContext context) : this()
        {
            this.import.DeserializeFrom(info, context);
        }

        public Steema.TeeChart.Styles.Series ActiveSeriesLegend(int itemIndex)
        {
            return this.SeriesLegend(itemIndex, true);
        }

        private bool ActiveSeriesUseAxis()
        {
            foreach (Steema.TeeChart.Styles.Series series in this.series)
            {
                if (series.Active && series.UseAxis)
                {
                    return true;
                }
            }
            return false;
        }

        private void AxisRect(Axis a, ref Rectangle r)
        {
            if (this.IsAxisVisible(a))
            {
                Rectangle rectangle = r;
                a.CalcRect(ref rectangle, true);
                r.Intersect(rectangle);
            }
        }

        public System.Drawing.Bitmap Bitmap()
        {
            return this.Bitmap(this.Width, this.Height);
        }

        public System.Drawing.Bitmap Bitmap(int width, int height)
        {
            return this.Bitmap(width, height, PixelFormat.Undefined);
        }

        public System.Drawing.Bitmap Bitmap(int width, int height, PixelFormat pixelformat)
        {
            System.Drawing.Bitmap bitmap;
            if (pixelformat == PixelFormat.Undefined)
            {
                bitmap = new System.Drawing.Bitmap(width, height);
            }
            else
            {
                bitmap = new System.Drawing.Bitmap(width, height, pixelformat);
            }
            this.chartBounds.Width = width;
            this.chartBounds.Height = height;
            this.Draw(Graphics.FromImage(bitmap));
            return bitmap;
        }

        internal TeeEvent BroadcastEvent(TeeEvent Event)
        {
            TeeEvent event2 = Event;
            Event.sender = base.Chart;
            foreach (ITeeEventListener listener in this.Listeners)
            {
                listener.TeeEvent(Event);
                if ((Event is TeeMouseEvent) && base.chart.CancelMouse)
                {
                    return event2;
                }
            }
            return event2;
        }

        protected internal void BroadcastEvent(Steema.TeeChart.Styles.Series s, SeriesEventStyle e)
        {
            SeriesEvent event2 = new SeriesEvent();
            event2.Event = e;
            event2.Series = s;
            this.BroadcastEvent(event2);
        }

        protected internal virtual void BroadcastMouseEvent(MouseEventKinds kind, MouseEventArgs e)
        {
            if ((this.parent != null) && !base.DesignMode)
            {
                if (this.Listeners.Count > 0)
                {
                    TeeMouseEvent event2 = new TeeMouseEvent();
                    try
                    {
                        event2.Event = kind;
                        event2.Button = e.Button;
                        event2.mArgs = new MouseEventArgs(e.Button, 1, e.X, e.Y, 0);
                        this.BroadcastEvent(event2);
                    }
                    finally
                    {
                        event2 = null;
                    }
                }
                Cursor c = this.parent.GetCursor();
                if (c != null)
                {
                    this.BroadcastMouseEvent(kind, e, ref c);
                }
            }
        }

        protected internal virtual void BroadcastMouseEvent(MouseEventKinds kind, MouseEventArgs e, ref Cursor c)
        {
            foreach (Steema.TeeChart.Tools.Tool tool in this.tools)
            {
                if (tool.Active)
                {
                    tool.MouseEvent(kind, e, ref c);
                }
            }
            foreach (Steema.TeeChart.Styles.Series series in this.series)
            {
                if (series.Active)
                {
                    series.MouseEvent(kind, e, ref c);
                }
            }
        }

        protected internal virtual void BroadcastToolEvent(EventArgs e)
        {
            foreach (Steema.TeeChart.Tools.Tool tool in this.tools)
            {
                if (tool.Active)
                {
                    tool.ChartEvent(e);
                }
            }
        }

        private void CalcAxisRect()
        {
            Rectangle chartRect = this.ChartRect;
            this.axes.AdjustMaxMin();
            this.axes.InternalCalcPositions();
            Rectangle rect = chartRect;
            this.AxisRect(this.axes.Left, ref chartRect);
            this.AxisRect(this.axes.Top, ref chartRect);
            this.AxisRect(this.axes.Right, ref chartRect);
            this.AxisRect(this.axes.Bottom, ref chartRect);
            this.AxisRect(this.axes.Depth, ref chartRect);
            foreach (Axis axis in this.axes.custom)
            {
                if (this.IsAxisVisible(axis))
                {
                    rect = chartRect;
                    axis.CalcRect(ref chartRect, false);
                    chartRect.Intersect(rect);
                }
            }
            this.RecalcWidthHeight(ref chartRect);
            this.ChartRect = chartRect;
            this.axes.InternalCalcPositions();
        }

        private ClickedPart CalcNeedClickedPart(Point Pos, bool Needed)
        {
            ClickedPart result = new ClickedPart();
            result.Part = ClickedParts.None;
            result.PointIndex = -1;
            result.Series = null;
            result.Axis = null;
            if (this.Legend.Visible)
            {
                result.PointIndex = this.Legend.Clicked(Pos.X, Pos.Y);
                if (result.PointIndex != -1)
                {
                    result.Part = ClickedParts.Legend;
                    return result;
                }
            }
            for (int i = this.Series.Count - 1; i >= 0; i--)
            {
                Steema.TeeChart.Styles.Series series = this.Series[i];
                bool flag = (this.parent != null) && this.parent.CheckClickSeries();
                if (series.Active && ((!Needed || series.HasClickEvents()) || flag))
                {
                    result.PointIndex = series.Clicked(Pos);
                    if (result.PointIndex != -1)
                    {
                        result.Series = this.Series[i];
                        result.Part = ClickedParts.Series;
                        return result;
                    }
                    if (this.Series[i].Marks.Visible)
                    {
                        result.PointIndex = this.Series[i].Marks.Clicked(Pos);
                        if (result.PointIndex != -1)
                        {
                            result.Series = this.Series[i];
                            result.Part = ClickedParts.SeriesMarks;
                            return result;
                        }
                    }
                }
                for (int j = 0; j < 5; j++)
                {
                    this.ClickedAxis(this.Axes[j], Pos, ref result);
                    if (result.Part == ClickedParts.Axis)
                    {
                        return result;
                    }
                }
                for (int k = 0; k < this.Axes.Custom.Count; k++)
                {
                    this.ClickedAxis(this.Axes.Custom[k], Pos, ref result);
                    if (result.Part == ClickedParts.Axis)
                    {
                        return result;
                    }
                }
                if (this.Header.Clicked(Pos))
                {
                    result.Part = ClickedParts.Header;
                }
                else if (this.SubHeader.Clicked(Pos))
                {
                    result.Part = ClickedParts.SubHeader;
                }
                else if (this.Footer.Clicked(Pos))
                {
                    result.Part = ClickedParts.Foot;
                }
                else if (this.SubFooter.Clicked(Pos))
                {
                    result.Part = ClickedParts.SubFoot;
                }
                else if (this.ChartRect.Contains(Pos.X, Pos.Y) && (this.CountActiveSeries() > 0))
                {
                    result.Part = ClickedParts.ChartRect;
                }
            }
            return result;
        }

        private int CalcNumPages(Axis a)
        {
            int num = 1;
            int count = 0;
            bool flag = true;
            foreach (Steema.TeeChart.Styles.Series series in this.series)
            {
                if ((series.Active && series.AssociatedToAxis(a)) && (flag || (series.Count > count)))
                {
                    count = series.Count;
                    flag = false;
                }
                if (count > 0)
                {
                    num = count / this.iPage.MaxPointsPerPage;
                    if ((count % this.iPage.MaxPointsPerPage) <= 0)
                    {
                        continue;
                    }
                    num++;
                }
            }
            return num;
        }

        private void CalcSeriesAxisRect(Axis axis)
        {
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            int num4 = 0;
            int leftMargin = 0;
            int rightMargin = 0;
            foreach (Steema.TeeChart.Styles.Series series in this.series)
            {
                if (!series.bActive || !series.AssociatedToAxis(axis))
                {
                    continue;
                }
                if (axis.horizontal)
                {
                    series.CalcHorizMargins(ref leftMargin, ref rightMargin);
                    if (axis.AutomaticMinimum)
                    {
                        num = Math.Max(num, leftMargin);
                    }
                    if (axis.AutomaticMaximum)
                    {
                        num3 = Math.Max(num3, rightMargin);
                    }
                    continue;
                }
                series.CalcVerticalMargins(ref leftMargin, ref rightMargin);
                if (axis.AutomaticMaximum)
                {
                    num2 = Math.Max(num2, leftMargin);
                }
                if (axis.AutomaticMinimum)
                {
                    num4 = Math.Max(num4, rightMargin);
                }
            }
            if (axis.Horizontal)
            {
                num += axis.MinimumOffset;
                num3 += axis.MaximumOffset;
            }
            else
            {
                num2 += axis.MaximumOffset;
                num4 += axis.MinimumOffset;
            }
            axis.AdjustMaxMinRect(Rectangle.FromLTRB(num, num2, num3, num4));
        }

        private void CalcSeriesRect()
        {
            for (int i = 0; i < this.axes.Count; i++)
            {
                this.CalcSeriesAxisRect(this.axes[i]);
            }
        }

        private void CalcSize3DWalls()
        {
            if (this.aspect.View3D)
            {
                double num = 0.001 * this.aspect.Chart3DPercent;
                if (!this.aspect.Orthogonal)
                {
                    num *= 2.0;
                }
                this.seriesWidth3D = (int) (num * this.chartBounds.Width);
                if (this.aspect.Orthogonal)
                {
                    double num2 = Math.Sin(this.aspect.OrthoAngle * 0.017453292519943295);
                    double num3 = Math.Cos(this.aspect.OrthoAngle * 0.017453292519943295);
                    num = num2 / num3;
                }
                else
                {
                    num = 1.0;
                }
                if (num > 1.0)
                {
                    this.seriesWidth3D = (int) (((double) this.seriesWidth3D) / num);
                }
                this.seriesHeight3D = (int) (this.seriesWidth3D * num);
                int num4 = this.aspect.ApplyZOrder ? Math.Max(1, this.maxZOrder + 1) : 1;
                this.aspect.Height3D = this.seriesHeight3D * num4;
                this.aspect.Width3D = this.seriesWidth3D * num4;
            }
            else
            {
                this.seriesWidth3D = 0;
                this.seriesHeight3D = 0;
                this.aspect.Width3D = 0;
                this.aspect.Height3D = 0;
            }
        }

        private int CalcString(int tmpResult, string St, ref int numLines)
        {
            tmpResult = Math.Max(tmpResult, (int) this.Graphics3D.TextWidth(St));
            numLines++;
            return tmpResult;
        }

        private void CalcWallsRect(ref Rectangle r)
        {
            this.CalcSize3DWalls();
            if (this.aspect.View3D && this.aspect.Orthogonal)
            {
                int size;
                if (this.ActiveSeriesUseAxis())
                {
                    size = this.Walls.Back.Size;
                }
                else
                {
                    size = 0;
                }
                r.Width -= Math.Abs(this.aspect.Width3D) + size;
                int num2 = Math.Abs(this.aspect.Height3D) + size;
                r.Height -= num2;
                r.Y += num2;
                if (this.Walls.Right.Visible)
                {
                    r.Width -= this.Walls.Right.Size + 1;
                }
            }
            this.RecalcWidthHeight(ref r);
        }

        public bool CanClip()
        {
            if (this.graphics3D.SupportsFullRotation)
            {
                return false;
            }
            if ((this.printing || this.graphics3D.metafiling) && (!this.printing || !this.ClipWhenPrinting))
            {
                return (this.graphics3D.metafiling && this.ClipWhenMetafiling);
            }
            return true;
        }

        private bool CheckMouseSeries(ref Cursor c, int X, int Y)
        {
            foreach (Steema.TeeChart.Styles.Series series in this.series)
            {
                if (series.bActive && series.CheckMouse(ref c, X, Y))
                {
                    return true;
                }
            }
            return false;
        }

        private void CheckTitle(Steema.TeeChart.Title t, MouseEventArgs e)
        {
            if (this.parent != null)
            {
                this.parent.CheckTitle(t, e);
            }
        }

        protected internal virtual void CheckZoomPanning(MouseEventArgs e)
        {
            if (this.ActiveSeriesUseAxis())
            {
                if (this.Zoom.Allow && (e.Button == this.Zoom.MouseButton))
                {
                    int x = e.X;
                    int y = e.Y;
                    if (this.Zoom.Direction == ZoomDirections.Vertical)
                    {
                        x = this.ChartRect.X;
                    }
                    if (this.Zoom.Direction == ZoomDirections.Horizontal)
                    {
                        y = this.ChartRect.Y;
                    }
                    this.Zoom.Activate(x, y);
                    if (this.Zoom.Direction == ZoomDirections.Vertical)
                    {
                        this.Zoom.x1 = this.ChartRect.Right;
                    }
                    if (this.Zoom.Direction == ZoomDirections.Horizontal)
                    {
                        this.Zoom.y1 = this.ChartRect.Bottom;
                    }
                    this.Zoom.Draw();
                    this.IClicked = true;
                }
                if ((this.panning.Allow != ScrollModes.None) && (e.Button == this.panning.MouseButton))
                {
                    this.panning.Activate(e.X, e.Y);
                    this.IClicked = true;
                }
            }
        }

        private void ClickedAxis(Axis a, Point p, ref ClickedPart result)
        {
            if (a.Clicked(p))
            {
                result.Part = ClickedParts.Axis;
                result.Axis = a;
            }
        }

        public int CountActiveSeries()
        {
            int num = 0;
            foreach (Steema.TeeChart.Styles.Series series in this.series)
            {
                if (series.bActive)
                {
                    num++;
                }
            }
            return num;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.RemoveAllComponents();
            }
            base.Dispose(disposing);
        }

        internal void DoDrawLegend(ref Rectangle tmp)
        {
            if (this.legend.Visible)
            {
                this.legend.Paint(this.graphics3D, tmp);
                if (this.legend.iLastValue >= this.legend.firstValue)
                {
                    this.legend.ResizeChartRect(ref tmp);
                }
            }
        }

        protected internal virtual void DoMouseDown(bool IsDoubleClick, MouseEventArgs e)
        {
            if ((!this.CancelMouse && !this.Zoom.Active) && !this.Panning.Active)
            {
                this.CancelMouse = false;
                this.BroadcastMouseEvent(MouseEventKinds.Down, e);
                if (!this.CancelMouse)
                {
                    ClickedPart part = this.CalcNeedClickedPart(new Point(e.X, e.Y), true);
                    this.IClicked = false;
                    switch (part.Part)
                    {
                        case ClickedParts.Legend:
                            this.IClicked = this.legend.DoMouseDown(e.X, e.Y);
                            if (this.parent != null)
                            {
                                this.parent.DoClickLegend(this, e);
                            }
                            break;

                        case ClickedParts.Series:
                            this.CancelMouse = false;
                            if ((e.Clicks == 2) && (this.parent != null))
                            {
                                part.Series.OnDblClick(e);
                            }
                            if (e.Clicks == 1)
                            {
                                part.Series.OnClick(e);
                            }
                            this.IClicked = this.CancelMouse;
                            if ((this.parent != null) && this.parent.CheckClickSeries())
                            {
                                this.CancelMouse = true;
                                if (this.parent != null)
                                {
                                    this.parent.DoClickSeries(this, part.Series, part.PointIndex, e);
                                }
                                this.IClicked = this.CancelMouse;
                            }
                            if (!this.IClicked)
                            {
                                this.CheckZoomPanning(e);
                            }
                            break;

                        case ClickedParts.Axis:
                            if (this.parent != null)
                            {
                                this.parent.DoClickAxis(part.Axis, e);
                            }
                            this.IClicked = this.CancelMouse;
                            if (!this.IClicked)
                            {
                                this.CheckZoomPanning(e);
                            }
                            break;

                        case ClickedParts.Foot:
                            this.CheckTitle(this.footer, e);
                            break;

                        case ClickedParts.Header:
                            this.CheckTitle(this.header, e);
                            break;

                        case ClickedParts.SubHeader:
                            this.CheckTitle(this.subHeader, e);
                            break;

                        case ClickedParts.SubFoot:
                            this.CheckTitle(this.subFooter, e);
                            break;

                        case ClickedParts.ChartRect:
                            if (this.parent != null)
                            {
                                this.parent.CheckBackground(this, e);
                            }
                            if (!this.IClicked)
                            {
                                this.CheckZoomPanning(e);
                            }
                            break;
                    }
                    if (!this.IClicked && (this.parent != null))
                    {
                        this.parent.CheckBackground(this, e);
                    }
                }
                this.CancelMouse = false;
            }
        }

        public virtual void DoMouseMove(int x, int y, ref Cursor c)
        {
            if (!this.CancelMouse)
            {
                if ((this.zoom != null) && this.zoom.Active)
                {
                    if (this.zoom.Direction == ZoomDirections.Vertical)
                    {
                        x = this.ChartRect.Right;
                    }
                    if (this.zoom.Direction == ZoomDirections.Horizontal)
                    {
                        y = this.ChartRect.Bottom;
                    }
                    if ((x != this.zoom.x1) || (y != this.zoom.y1))
                    {
                        this.zoom.Draw();
                        this.zoom.x1 = x;
                        this.zoom.y1 = y;
                        this.zoom.Draw();
                    }
                }
                else if ((this.panning != null) && this.panning.Active)
                {
                    if (!this.ChartRect.Contains(x, y))
                    {
                        this.panning.Active = false;
                    }
                    else if ((x != this.panning.x1) || (y != this.panning.y1))
                    {
                        bool panned = false;
                        this.panning.Check();
                        if (this.restoredAxisScales)
                        {
                            this.savedScales = this.SaveScales();
                            this.restoredAxisScales = false;
                        }
                        this.PanAxis(true, x, this.panning.x1, ref panned);
                        this.PanAxis(false, y, this.panning.y1, ref panned);
                        this.panning.x1 = x;
                        this.panning.y1 = y;
                        if (panned)
                        {
                            if (this.parent != null)
                            {
                                this.parent.DoScroll(this, EventArgs.Empty);
                            }
                            this.Invalidate();
                        }
                    }
                }
                else
                {
                    this.CheckMouseSeries(ref c, x, y);
                }
            }
        }

        public virtual void DoMouseUp(MouseEventArgs e)
        {
            this.CancelMouse = false;
            if (this.Zoom.Active && (e.Button == this.Zoom.MouseButton))
            {
                this.zoom.Active = false;
                this.zoom.Draw();
                int x = e.X;
                int y = e.Y;
                if (this.zoom.Direction == ZoomDirections.Vertical)
                {
                    x = this.ChartRect.Right;
                }
                if (this.zoom.Direction == ZoomDirections.Horizontal)
                {
                    y = this.ChartRect.Bottom;
                }
                this.zoom.x1 = x;
                this.zoom.y1 = y;
                if ((Math.Abs((int) (this.zoom.x1 - this.zoom.x0)) > this.zoom.MinPixels) && (Math.Abs((int) (this.zoom.y1 - this.zoom.y0)) > this.zoom.MinPixels))
                {
                    if ((this.zoom.x1 > this.zoom.x0) && (this.zoom.y1 > this.zoom.y0))
                    {
                        this.zoom.CalcZoomPoints();
                    }
                    else
                    {
                        this.zoom.Undo();
                    }
                    this.Invalidate();
                }
            }
            if (this.panning != null)
            {
                this.panning.Active = false;
            }
            this.BroadcastMouseEvent(MouseEventKinds.Up, e);
        }

        internal void DoPanelPaint(Graphics g, Rectangle r)
        {
            this.ChartRect = r;
            this.chartBounds = r;
            this.graphics3D.InitWindow(g, this.aspect, this.ChartRect, 100);
            this.panel.Draw(this.graphics3D, ref this.ChartRect);
        }

        internal void DoZoom(double topi, double topf, double boti, double botf, double lefi, double leff, double rigi, double rigf)
        {
            if (this.restoredAxisScales)
            {
                this.savedScales = this.SaveScales();
                this.restoredAxisScales = false;
            }
            if (this.Zoom.Animated)
            {
                this.DoZoomAnimated(topi, topf, boti, botf, lefi, leff, rigi, rigf);
            }
            this.Axes.Left.SetMinMax(lefi, leff);
            this.Axes.Right.SetMinMax(rigi, rigf);
            this.Axes.Top.SetMinMax(topi, topf);
            this.Axes.Bottom.SetMinMax(boti, botf);
            this.zoom.Zoomed = true;
            if (this.parent != null)
            {
                this.parent.DoZoomed(this, EventArgs.Empty);
            }
        }

        private void DoZoomAnimated(double topi, double topf, double boti, double botf, double lefi, double leff, double rigi, double rigf)
        {
            for (int i = 1; i < this.Zoom.AnimatedSteps; i++)
            {
                this.ZoomAxis(this.Axes.Left, lefi, leff);
                this.ZoomAxis(this.Axes.Right, rigi, rigf);
                this.ZoomAxis(this.Axes.Top, topi, topf);
                this.ZoomAxis(this.Axes.Bottom, boti, botf);
                this.Invalidate();
            }
        }

        public void Draw(Graphics g)
        {
            this.Draw(g, new Rectangle(0, 0, this.chartBounds.Width, this.chartBounds.Height));
        }

        public void Draw(Graphics g, Rectangle r)
        {
            bool autoRepaint = this.AutoRepaint;
            this.AutoRepaint = false;
            try
            {
                this.DoPanelPaint(g, r);
                if (this.parent != null)
                {
                    this.parent.DoBeforeDraw();
                }
                this.InternalDraw(g);
                if (this.parent != null)
                {
                    this.parent.DoAfterDraw();
                }
            }
            finally
            {
                this.graphics3D.ShowImage();
                this.AutoRepaint = autoRepaint;
            }
        }

        private bool DrawRightWallAfter()
        {
            Point point = this.graphics3D.Calc3DPoint(this.ChartRect.Right, this.ChartRect.Y, 0);
            Point point2 = this.graphics3D.Calc3DPoint(this.ChartRect.Right, this.ChartRect.Bottom + this.walls.CalcWallSize(this.axes.Bottom), this.Aspect.Width3D + this.walls.Back.Size);
            return (point.X <= point2.X);
        }

        private void DrawTitleFoot(ref Rectangle rect, bool CustomOnly)
        {
            this.header.DoDraw(this.graphics3D, ref rect, CustomOnly);
            this.subHeader.DoDraw(this.graphics3D, ref rect, CustomOnly);
            this.footer.DoDraw(this.graphics3D, ref rect, CustomOnly);
            this.subFooter.DoDraw(this.graphics3D, ref rect, CustomOnly);
        }

        private void DrawTitlesAndLegend(Graphics g, ref Rectangle tmp, bool BeforeSeries)
        {
            if (BeforeSeries)
            {
                if (!this.legend.CustomPosition && this.ShouldDrawLegend())
                {
                    if (this.legend.Vertical)
                    {
                        this.DoDrawLegend(ref tmp);
                        this.DrawTitleFoot(ref tmp, false);
                    }
                    else
                    {
                        this.DrawTitleFoot(ref tmp, false);
                        this.DoDrawLegend(ref tmp);
                    }
                }
                else
                {
                    this.DrawTitleFoot(ref tmp, false);
                }
            }
            else
            {
                if (this.legend.CustomPosition && this.ShouldDrawLegend())
                {
                    this.DoDrawLegend(ref tmp);
                }
                this.DrawTitleFoot(ref tmp, true);
            }
            if ((!BeforeSeries && this.ActiveSeriesUseAxis()) && (this.Aspect.View3D && this.walls.View3D))
            {
                if (this.walls.Right.Visible && this.DrawRightWallAfter())
                {
                    this.walls.Right.Paint(this.graphics3D, tmp);
                }
                if ((this.walls.Left.Visible && !this.Aspect.Orthogonal) && (this.Aspect.Rotation < 270))
                {
                    this.walls.Left.Paint(this.graphics3D, tmp);
                    this.axes.Left.iHideBackGrid = true;
                    this.axes.Left.Draw(false);
                    this.axes.Left.iHideBackGrid = false;
                }
            }
        }

        public string FormattedLegend(int seriesOrValueIndex)
        {
            string text = this.legend.FormattedLegend(seriesOrValueIndex);
            if (this.parent != null)
            {
                this.parent.DoGetLegendText(this, this.legend.iLegendStyle, seriesOrValueIndex, ref text);
            }
            return text;
        }

        public string FormattedValueLegend(Steema.TeeChart.Styles.Series aSeries, int valueIndex)
        {
            if (aSeries == null)
            {
                return "";
            }
            return this.legend.FormattedValue(aSeries, valueIndex);
        }

        public Color FreeSeriesColor(bool checkBackground)
        {
            int index = 0;
            do
            {
                Color defaultColor = Steema.TeeChart.Drawing.Graphics3D.GetDefaultColor(index);
                if (this.IsFreeSeriesColor(defaultColor, checkBackground))
                {
                    return defaultColor;
                }
                index++;
            }
            while (index < Steema.TeeChart.Drawing.Graphics3D.ColorPalette.Length);
            return Steema.TeeChart.Drawing.Graphics3D.ColorPalette[0];
        }

        public Steema.TeeChart.Styles.Series GetASeries()
        {
            foreach (Steema.TeeChart.Styles.Series series in this.series)
            {
                if (series.bActive)
                {
                    return series;
                }
            }
            return null;
        }

        public Steema.TeeChart.Styles.Series GetAxisSeries(Axis axis)
        {
            foreach (Steema.TeeChart.Styles.Series series in this.series)
            {
                if ((series.bActive || this.NoActiveSeries(axis)) && series.AssociatedToAxis(axis))
                {
                    return series;
                }
            }
            return null;
        }

        internal int GetMaxValuesCount()
        {
            int count = 0;
            bool flag = true;
            foreach (Steema.TeeChart.Styles.Series series in this.series)
            {
                if (series.bActive && (flag || (series.Count > count)))
                {
                    count = series.Count;
                    flag = false;
                }
            }
            return count;
        }

        [SecurityPermission(SecurityAction.Demand)]//,SecurityPermissionFlag.NoFlags)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            this.export.Template.Serialize(info, context);
        }

        internal void InternalDraw(Graphics g)
        {
            Rectangle chartRect = this.ChartRect;
            foreach (Steema.TeeChart.Styles.Series series in this.series)
            {
                if (series.bActive)
                {
                    series.DoBeforeDrawChart();
                }
            }
            this.DrawTitlesAndLegend(g, ref chartRect, true);
            this.ChartRect = chartRect;
            this.SetSeriesZOrder();
            this.CalcWallsRect(ref chartRect);
            this.ChartRect = chartRect;
            this.CalcAxisRect();
            chartRect = this.ChartRect;
            this.SetSeriesZPositions();
            this.CalcSeriesRect();
            this.graphics3D.Projection(this.aspect.Width3D, this.ChartRect);
            if (this.series.ActiveUseAxis() && this.walls.Visible)
            {
                this.walls.Paint(this.graphics3D, chartRect);
            }
            if (this.axes.DrawBehind)
            {
                this.axes.Draw(this.graphics3D);
            }
            this.BroadcastToolEvent(new BeforeDrawSeriesEventArgs());
            if (this.parent != null)
            {
                this.parent.DoBeforeDrawSeries();
            }
            StringAlignment textAlign = this.graphics3D.TextAlign;
            if (this.axes.depth.inverted)
            {
                for (int i = this.series.Count - 1; i >= 0; i--)
                {
                    if (this.Series[i].Active)
                    {
                        this.Series[i].DrawSeries();
                    }
                }
            }
            else
            {
                foreach (Steema.TeeChart.Styles.Series series2 in this.series)
                {
                    if (series2.bActive)
                    {
                        series2.DrawSeries();
                    }
                }
            }
            this.graphics3D.TextAlign = textAlign;
            if (!this.axes.DrawBehind)
            {
                this.axes.Draw(this.graphics3D);
            }
            this.DrawTitlesAndLegend(g, ref chartRect, false);
            this.BroadcastToolEvent(new AfterDrawEventArgs());
        }

        protected internal double InternalMinMax(Axis aAxis, bool isMin, bool isX)
        {
            bool flag;
            double num;
            double num2;
            if (aAxis.IsDepthAxis)
            {
                if (aAxis.CalcLabelStyle() == AxisLabelStyle.Value)
                {
                    num2 = 0.0;
                    flag = true;
                    foreach (Steema.TeeChart.Styles.Series series in this.series)
                    {
                        if (!series.bActive)
                        {
                            continue;
                        }
                        num = isMin ? series.MinZValue() : series.MaxZValue();
                        if ((flag || (isMin && (num < num2))) || (!isMin && (num > num2)))
                        {
                            flag = false;
                            num2 = num;
                        }
                    }
                    return num2;
                }
                return (isMin ? -0.5 : (this.maxZOrder + 0.5));
            }
            num2 = 0.0;
            Steema.TeeChart.Styles.Series axisSeries = this.GetAxisSeries(aAxis);
            bool flag2 = (axisSeries != null) ? (axisSeries.yMandatory ? isX : !isX) : isX;
            if ((this.iPage.MaxPointsPerPage > 0) && flag2)
            {
                if ((axisSeries != null) && (axisSeries.Count > 0))
                {
                    ValueList list = isX ? axisSeries.XValues : axisSeries.YValues;
                    int index = (this.iPage.Current - 1) * this.iPage.MaxPointsPerPage;
                    int count = axisSeries.Count;
                    if (count <= index)
                    {
                        index = Math.Max(0, (count / this.iPage.MaxPointsPerPage) - 1) * this.iPage.MaxPointsPerPage;
                    }
                    int num5 = (index + this.iPage.MaxPointsPerPage) - 1;
                    if (count <= num5)
                    {
                        num5 = (index + (count % this.iPage.MaxPointsPerPage)) - 1;
                    }
                    if (isMin)
                    {
                        return list.Value[index];
                    }
                    num2 = list.Value[num5];
                    if (!this.iPage.ScaleLastPage)
                    {
                        int num6 = (num5 - index) + 1;
                        if (num6 < this.iPage.MaxPointsPerPage)
                        {
                            num = list.Value[index];
                            num2 = num + ((this.iPage.MaxPointsPerPage * (num2 - num)) / ((double) num6));
                        }
                    }
                }
                return num2;
            }
            flag = true;
            foreach (Steema.TeeChart.Styles.Series series3 in this.series)
            {
                if (((!series3.bActive && !this.NoActiveSeries(aAxis)) || (series3.Count <= 0)) || ((!isX || ((series3.HorizAxis != HorizontalAxis.Both) && (series3.GetHorizAxis != aAxis))) && (isX || ((series3.VertAxis != VerticalAxis.Both) && (series3.GetVertAxis != aAxis)))))
                {
                    continue;
                }
                if (isMin)
                {
                    num = isX ? series3.MinXValue() : series3.MinYValue();
                }
                else
                {
                    num = isX ? series3.MaxXValue() : series3.MaxYValue();
                }
                if ((flag || (isMin && (num < num2))) || (!isMin && (num > num2)))
                {
                    num2 = num;
                    flag = false;
                }
            }
            return num2;
        }

        public bool IsAxisVisible(Axis a)
        {
            bool flag = this.axes.Visible && a.Visible;
            if (flag)
            {
                if (a.IsDepthAxis)
                {
                    return this.aspect.view3D;
                }
                foreach (Steema.TeeChart.Styles.Series series in this.series)
                {
                    if (series.bActive)
                    {
                        if (series.UseAxis)
                        {
                            flag = series.AssociatedToAxis(a);
                            if (flag)
                            {
                                return true;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            return flag;
        }

        public bool IsFreeSeriesColor(Color color, bool checkBackground)
        {
            bool flag = checkBackground && ((color == this.panel.Color) || (color == this.walls.Back.Color));
            foreach (Steema.TeeChart.Styles.Series series in this.series)
            {
                if ((series.Color == color) || flag)
                {
                    return false;
                }
            }
            return !flag;
        }

        protected internal bool IsValidDataSource(Steema.TeeChart.Styles.Series s, object source)
        {
            bool flag = ((s != source) && (source is Steema.TeeChart.Styles.Series)) && s.IsValidSourceOf((Steema.TeeChart.Styles.Series) source);
            if (!flag)
            {
                flag = DataSeriesSource.IsValidSource(source);
            }
            return flag;
        }

        public int MaxMarkWidth()
        {
            int num = 0;
            foreach (Steema.TeeChart.Styles.Series series in this.series)
            {
                if (series.Active)
                {
                    num = Math.Max(num, series.MaxMarkWidth());
                }
            }
            return num;
        }

        public int MaxTextWidth()
        {
            int num = 0;
            foreach (Steema.TeeChart.Styles.Series series in this.series)
            {
                if (series.Labels.Count > 0)
                {
                    for (int i = 0; i < series.Count; i++)
                    {
                        num = Math.Max(num, this.MultiLineTextWidth(series.sLabels[i]));
                    }
                }
            }
            return num;
        }

        public double MaxXValue(Axis axis)
        {
            return this.InternalMinMax(axis, false, true);
        }

        public double MaxYValue(Axis axis)
        {
            return this.InternalMinMax(axis, false, false);
        }

        public System.Drawing.Imaging.Metafile Metafile(Chart aChart, int width, int height)
        {
            return this.Metafile(new MemoryStream(), aChart, width, height);
        }

        public System.Drawing.Imaging.Metafile Metafile(Stream stream, Chart aChart, int width, int height)
        {
            System.Drawing.Bitmap image = new System.Drawing.Bitmap(1, 1, PixelFormat.Format24bppRgb);
            Graphics gRef = Graphics.FromImage(image);
            return this.Metafile(gRef, new MemoryStream(), aChart, width, height);
        }

        public System.Drawing.Imaging.Metafile Metafile(Graphics gRef, Stream stream, Chart aChart, int width, int height)
        {
            IntPtr hdc = gRef.GetHdc();
            System.Drawing.Imaging.Metafile image = new System.Drawing.Imaging.Metafile(stream, hdc);
            gRef.ReleaseHdc(hdc);
            gRef.Dispose();
            gRef = Graphics.FromImage(image);
            aChart.graphics3D.metafiling = true;
            try
            {
                aChart.Draw(gRef, new Rectangle(0, 0, width, height));
            }
            finally
            {
                aChart.graphics3D.metafiling = false;
                gRef.Dispose();
            }
            return image;
        }

        public double MinXValue(Axis axis)
        {
            return this.InternalMinMax(axis, true, true);
        }

        public double MinYValue(Axis axis)
        {
            return this.InternalMinMax(axis, true, false);
        }

        protected internal int MultiLineTextWidth(string s)
        {
            int num;
            return this.MultiLineTextWidth(s, out num);
        }

        protected internal int MultiLineTextWidth(string s, out int numLines)
        {
            int index = s.IndexOf('\n');
            if (index == -1)
            {
                numLines = 1;
                return (int) this.graphics3D.TextWidth(s);
            }
            int tmpResult = 0;
            numLines = 0;
            while (index != -1)
            {
                tmpResult = this.CalcString(tmpResult, s.Substring(0, index + 1), ref numLines);
                s = s.Remove(0, index + 1);
                index = s.IndexOf('\n');
            }
            if (s.Length != 0)
            {
                numLines++;
                tmpResult = Math.Max(tmpResult, (int) this.graphics3D.TextWidth(s));
            }
            return tmpResult;
        }

        private bool NoActiveSeries(Axis a)
        {
            for (int i = 0; i < this.series.Count; i++)
            {
                Steema.TeeChart.Styles.Series series = this.series[i];
                if (series.Active && series.AssociatedToAxis(a))
                {
                    return false;
                }
            }
            return true;
        }

        internal int NumPages()
        {
            if ((this.iPage.MaxPointsPerPage <= 0) || (this.Series.Count <= 0))
            {
                return 1;
            }
            if (this.Series[0].yMandatory)
            {
                return Math.Max(this.CalcNumPages(this.Axes.Top), this.CalcNumPages(this.Axes.Bottom));
            }
            return Math.Max(this.CalcNumPages(this.Axes.Left), this.CalcNumPages(this.Axes.Right));
        }

        private void PanAxis(bool AxisHorizontal, int Pos1, int Pos2, ref bool Panned)
        {
            ScrollModes modes = AxisHorizontal ? ScrollModes.Horizontal : ScrollModes.Vertical;
            if ((Pos1 != Pos2) && ((this.panning.Allow == modes) || (this.panning.Allow == ScrollModes.Both)))
            {
                if (AxisHorizontal)
                {
                    this.ProcessPanning(this.Axes.Top, Pos2, Pos1);
                    this.ProcessPanning(this.Axes.Bottom, Pos2, Pos1);
                }
                else
                {
                    this.ProcessPanning(this.Axes.Left, Pos2, Pos1);
                    this.ProcessPanning(this.Axes.Right, Pos2, Pos1);
                }
                for (int i = 0; i < this.Axes.Custom.Count; i++)
                {
                    Axis a = this.Axes.Custom[i];
                    if (!a.IsDepthAxis && ((AxisHorizontal && a.Horizontal) || (!AxisHorizontal && !a.Horizontal)))
                    {
                        this.ProcessPanning(a, Pos2, Pos1);
                    }
                }
                Panned = true;
            }
        }

        private void ProcessPanning(Axis a, int IniPos, int EndPos)
        {
            double delta = a.CalcPosPoint(IniPos) - a.CalcPosPoint(EndPos);
            double min = a.Minimum + delta;
            double max = a.Maximum + delta;
            if ((this.parent != null) && this.parent.DoAllowScroll(a, delta, ref min, ref max))
            {
                a.SetMinMax(min, max);
            }
        }

        internal void RecalcWidthHeight(ref Rectangle r)
        {
            if (r.X < this.chartBounds.X)
            {
                r.X = this.chartBounds.X;
            }
            if (r.Y < this.chartBounds.Y)
            {
                r.Y = this.chartBounds.Y;
            }
            if (r.Right < this.chartBounds.X)
            {
                r.Width = 1;
            }
            else if (r.Right == this.chartBounds.X)
            {
                r.Width = this.chartBounds.Width;
            }
            if (r.Bottom < this.chartBounds.Y)
            {
                r.Height = 1;
            }
            else if (r.Bottom == this.chartBounds.Y)
            {
                r.Height = this.chartBounds.Height;
            }
            this.graphics3D.XCenter = (r.X + r.Right) / 2;
            this.graphics3D.YCenter = (r.Y + r.Bottom) / 2;
        }

        internal void RemoveAllComponents()
        {
            this.series.Clear();
            this.tools.Clear();
            this.axes.custom.Clear();
        }

        internal void RemoveListener(ITeeEventListener sender)
        {
            if (this.listeners != null)
            {
                this.listeners.Remove(sender);
            }
        }

        internal void RestoreAxisScales()
        {
            if (!this.restoredAxisScales)
            {
                this.RestoreScales(this.savedScales);
                this.restoredAxisScales = true;
            }
        }

        private void RestoreAxisScales(Axis a, AxisSavedScales tmp)
        {
            a.Automatic = tmp.Auto;
            a.AutomaticMinimum = tmp.AutoMin;
            a.AutomaticMaximum = tmp.AutoMax;
            if (!a.Automatic)
            {
                a.SetMinMax(tmp.Min, tmp.Max);
            }
        }

        private void RestoreScales(AllAxisSavedScales s)
        {
            this.RestoreAxisScales(this.Axes.Top, s.Top);
            this.RestoreAxisScales(this.Axes.Bottom, s.Bottom);
            this.RestoreAxisScales(this.Axes.Left, s.Left);
            this.RestoreAxisScales(this.Axes.Right, s.Right);
        }

        private void SaveAxisScales(Axis a, ref AxisSavedScales tmp)
        {
            tmp.Auto = a.Automatic;
            tmp.AutoMin = a.AutomaticMinimum;
            tmp.AutoMax = a.AutomaticMaximum;
            tmp.Min = a.Minimum;
            tmp.Max = a.Maximum;
        }

        private AllAxisSavedScales SaveScales()
        {
            AllAxisSavedScales scales = new AllAxisSavedScales();
            this.SaveAxisScales(this.Axes.Top, ref scales.Top);
            this.SaveAxisScales(this.Axes.Bottom, ref scales.Bottom);
            this.SaveAxisScales(this.Axes.Left, ref scales.Left);
            this.SaveAxisScales(this.Axes.Right, ref scales.Right);
            return scales;
        }

        public Steema.TeeChart.Styles.Series SeriesLegend(int itemIndex, bool onlyActive)
        {
            int num = 0;
            for (int i = 0; i < this.series.Count; i++)
            {
                Steema.TeeChart.Styles.Series series = this.series[i];
                if (series.ShowInLegend && (!onlyActive || series.bActive))
                {
                    if (num == itemIndex)
                    {
                        return series;
                    }
                    num++;
                }
            }
            return null;
        }

        public string SeriesTitleLegend(int seriesIndex, bool onlyActive)
        {
            Steema.TeeChart.Styles.Series series;
            if (onlyActive)
            {
                series = this.ActiveSeriesLegend(seriesIndex);
            }
            else
            {
                series = this.SeriesLegend(seriesIndex, false);
            }
            if (series == null)
            {
                return "";
            }
            return series.ToString();
        }

        internal void SetBrushCanvas(Color AColor, ChartBrush ABrush, Color ABackColor)
        {
            this.graphics3D.Brush = ABrush;
            this.graphics3D.Brush.Color = AColor;
        }

        private void SetSeriesZOrder()
        {
            this.maxZOrder = 0;
            bool flag = this.aspect.ApplyZOrder && this.aspect.View3D;
            if (flag)
            {
                this.maxZOrder = -1;
                foreach (Steema.TeeChart.Styles.Series series in this.series)
                {
                    if (series.Active)
                    {
                        series.CalcZOrder();
                    }
                }
            }
            if (!this.axes.Depth.inverted)
            {
                foreach (Steema.TeeChart.Styles.Series series2 in this.series)
                {
                    if (series2.bActive)
                    {
                        series2.iZOrder = flag ? (this.maxZOrder - series2.ZOrder) : 0;
                    }
                }
            }
        }

        private void SetSeriesZPositions()
        {
            foreach (Steema.TeeChart.Styles.Series series in this.series)
            {
                if (!series.Active)
                {
                    continue;
                }
                series.startZ = series.iZOrder * this.seriesWidth3D;
                if (series.Depth == -1)
                {
                    series.endZ = series.startZ + this.seriesWidth3D;
                }
                else
                {
                    series.endZ = series.startZ + series.Depth;
                }
                series.middleZ = (series.startZ + series.endZ) / 2;
                series.Marks.zPosition = series.middleZ;
            }
        }

        private bool ShouldDrawLegend()
        {
            if (!this.legend.Visible)
            {
                return false;
            }
            if (!this.legend.HasCheckBoxes())
            {
                return (this.CountActiveSeries() > 0);
            }
            return true;
        }

        [Obsolete("Please use tChart1.Zoom.Undo method."), EditorBrowsable(EditorBrowsableState.Never)]
        public void UndoZoom()
        {
            this.zoom.Undo();
        }

        private void ZoomAxis(Axis a, double tmpA, double tmpB)
        {
            a.SetMinMax((double) (a.Minimum + ((tmpA - a.Minimum) / Steema.TeeChart.Zoom.AnimatedFactor)), (double) (a.Maximum - ((a.Maximum - tmpB) / Steema.TeeChart.Zoom.AnimatedFactor)));
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("3D view parameters.")]
        public Steema.TeeChart.Drawing.Aspect Aspect
        {
            get
            {
                return this.aspect;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Collection of predefined and custom axis objects.")]
        public Steema.TeeChart.Axes Axes
        {
            get
            {
                return this.axes;
            }
        }

        [Description("Read only. Used to get the four sides of the Chart.")]
        public Rectangle ChartBounds
        {
            get
            {
                return this.chartBounds;
            }
        }

        internal IContainer ChartContainer
        {
            get
            {
                if (this.parent != null)
                {
                    if (this.parent.GetContainer() != null)
                    {
                        return this.parent.GetContainer();
                    }
                    Form form = this.parent.FindParentForm();
                    if (form != null)
                    {
                        return form.Container;
                    }
                }
                return null;
            }
        }

        internal int ChartRectBottom
        {
            get
            {
                return this.ChartRect.Bottom;
            }
        }

        internal int ChartRectHeight
        {
            get
            {
                return this.ChartRect.Height;
            }
        }

        internal int ChartRectWidth
        {
            get
            {
                return this.ChartRect.Width;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), Description("Accesses Chart export properties and methods.")]
        public Exports Export
        {
            get
            {
                return this.export;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Titles"), Description("Defines Text shown at the bottom of the Chart.")]
        public Steema.TeeChart.Footer Footer
        {
            get
            {
                return this.footer;
            }
        }

        [Description("Accesses TeeChart Draw properties and methods."), Browsable(false)]
        public Steema.TeeChart.Drawing.Graphics3D Graphics3D
        {
            get
            {
                return this.graphics3D;
            }
            set
            {
                this.graphics3D = value;
                this.Invalidate();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Titles"), Description("Defines Text shown at top of the Chart.")]
        public Steema.TeeChart.Header Header
        {
            get
            {
                return this.header;
            }
        }

        [Description("Sets the Chart Height in pixels.")]
        public int Height
        {
            get
            {
                return this.chartBounds.Height;
            }
            set
            {
                this.chartBounds.Height = value;
                this.Invalidate();
            }
        }

        [Description("Accesses Chart import properties and methods."), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Imports Import
        {
            get
            {
                return this.import;
            }
        }

        public Steema.TeeChart.Styles.Series this[int index]
        {
            get
            {
                return this.series[index];
            }
            set
            {
                this.series[index] = value;
            }
        }

        internal int Left
        {
            get
            {
                return this.chartBounds.Left;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Properties for Chart Legend.")]
        public Steema.TeeChart.Legend Legend
        {
            get
            {
                return this.legend;
            }
        }

        public TeeEventListeners Listeners
        {
            get
            {
                return this.listeners;
            }
        }

        [Description("Properties to define multiple pages."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Steema.TeeChart.Page Page
        {
            get
            {
                return this.iPage;
            }
        }

        [Description("Background visible attributes."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Steema.TeeChart.Panel Panel
        {
            get
            {
                return this.panel;
            }
        }

        [Description("Sets the scrolling direction or denies scrolling."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Scroll Panning
        {
            get
            {
                if (this.panning == null)
                {
                    this.panning = new Scroll(this);
                }
                return this.panning;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Printing related properties.")]
        public Steema.TeeChart.Printer Printer
        {
            get
            {
                return this.printer;
            }
        }

        internal int Right
        {
            get
            {
                return this.chartBounds.Right;
            }
        }

        [Description("Collection of Series contained in this Chart."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public SeriesCollection Series
        {
            get
            {
                return this.series;
            }
        }

        [Category("Titles"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Defines Text shown directly above Footer.")]
        public Steema.TeeChart.Footer SubFooter
        {
            get
            {
                return this.subFooter;
            }
        }

        [Category("Titles"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Defines Text shown directly below Header.")]
        public Steema.TeeChart.Header SubHeader
        {
            get
            {
                return this.subHeader;
            }
        }

        [Browsable(false), Obsolete("Please use SubHeader property."), EditorBrowsable(EditorBrowsableState.Never)]
        public Steema.TeeChart.Header SubTitle
        {
            get
            {
                return this.subHeader;
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), Obsolete("Please use Header property.")]
        public Steema.TeeChart.Header Title
        {
            get
            {
                return this.header;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Collection of Tool components contained in this Chart.")]
        public ToolsCollection Tools
        {
            get
            {
                return this.tools;
            }
        }

        [Description("Displays a text box at the cursor."), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ChartToolTip ToolTip
        {
            get
            {
                if (this.toolTip == null)
                {
                    this.toolTip = new ChartToolTip(this);
                }
                return this.toolTip;
            }
        }

        internal int Top
        {
            get
            {
                return this.chartBounds.Top;
            }
        }

        [Description("Accesses wall characteristics of the Chart."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Steema.TeeChart.Walls Walls
        {
            get
            {
                return this.walls;
            }
        }

        [Description("Sets the Chart Width in pixels.")]
        public int Width
        {
            get
            {
                return this.chartBounds.Width;
            }
            set
            {
                this.chartBounds.Width = value;
                this.Invalidate();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Accesses the Zoom characteristics of the Chart.")]
        public Steema.TeeChart.Zoom Zoom
        {
            get
            {
                if (this.zoom == null)
                {
                    this.zoom = new Steema.TeeChart.Zoom(this);
                }
                return this.zoom;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct AllAxisSavedScales
        {
            public Chart.AxisSavedScales Left;
            public Chart.AxisSavedScales Top;
            public Chart.AxisSavedScales Right;
            public Chart.AxisSavedScales Bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct AxisSavedScales
        {
            public bool Auto;
            public bool AutoMin;
            public bool AutoMax;
            public double Min;
            public double Max;
        }

        public sealed class ChartToolTip
        {
            private Control chartControl;
            public string Text;
            private ToolTip toolTip;

            public ChartToolTip(Chart c)
            {
                this.chartControl = c.parent.GetControl();
                this.toolTip = new ToolTip();
                this.toolTip.InitialDelay = 500;
            }

            public void Hide()
            {
                this.toolTip.Active = false;
            }

            public void Show()
            {
                this.toolTip.SetToolTip(this.chartControl, this.Text);
                this.toolTip.Active = true;
            }

            [DefaultValue(500), Description("Sets the time lag before the Tool Tip appears.")]
            public int InitialDelay
            {
                get
                {
                    return this.toolTip.InitialDelay;
                }
                set
                {
                    this.toolTip.InitialDelay = value;
                }
            }
        }

        internal class ClickedPart
        {
            public Steema.TeeChart.Axis Axis;
            public ClickedParts Part;
            public int PointIndex;
            public Steema.TeeChart.Styles.Series Series;
        }
    }
}

