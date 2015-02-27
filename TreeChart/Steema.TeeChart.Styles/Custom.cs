namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;

    public class Custom : CustomPoint
    {
        protected ChartBrush bAreaBrush;
        protected bool bClickableLine;
        protected bool bDark3D;
        private int BottomPos;
        private bool colorEachLine;
        protected bool drawArea;
        protected bool drawLine;
        private bool invertedStairs;
        private bool IsLastValue;
        private int lineHeight;
        private int OldBottomPos;
        private Color OldColor;
        private int OldX;
        private int OldY;
        private ChartPen outLine;
        protected ChartPen pAreaLines;
        private bool stairs;
        private Color tmpColor;
        private double tmpDark3DRatio;

        public Custom() : this(null)
        {
        }

        public Custom(Chart c) : base(c)
        {
            this.bClickableLine = true;
            this.colorEachLine = true;
            this.bDark3D = true;
            this.drawLine = true;
            base.pLinePen.defaultColor = Color.Black;
            base.pLinePen.Color = Color.Black;
        }

        protected internal override void CalcHorizMargins(ref int LeftMargin, ref int RightMargin)
        {
            base.CalcHorizMargins(ref LeftMargin, ref RightMargin);
            this.InternalCalcMargin(!base.yMandatory, true, ref LeftMargin, ref RightMargin);
        }

        protected internal override void CalcVerticalMargins(ref int TopMargin, ref int BottomMargin)
        {
            base.CalcVerticalMargins(ref TopMargin, ref BottomMargin);
            this.InternalCalcMargin(base.yMandatory, false, ref TopMargin, ref BottomMargin);
            if (((this.lineHeight > 0) && !this.drawArea) && (base.chart.aspect.view3D && (this.lineHeight > BottomMargin)))
            {
                BottomMargin = this.lineHeight;
            }
        }

        private int CalcYPosLeftRight(double yLimit, int anotherIndex, int valueIndex)
        {
            double num = base.vxValues[anotherIndex];
            double num2 = base.vxValues[valueIndex] - num;
            if (num2 == 0.0)
            {
                return this.CalcYPos(anotherIndex);
            }
            double num3 = base.vyValues[anotherIndex];
            return base.GetVertAxis.CalcYPosValue((1.0 * num3) + (((yLimit - num) * (base.vyValues[valueIndex] - num3)) / num2));
        }

        private bool CheckPointInLine(Point P, int tmpX, int tmpY, int OldXPos, int OldYPos)
        {
            if (base.chart.Aspect.View3D)
            {
                Point[] poly = new Point[] { new Point(tmpX, tmpY), new Point(tmpX + base.chart.seriesWidth3D, tmpY - base.chart.seriesHeight3D), new Point(OldXPos + base.chart.seriesWidth3D, OldYPos - base.chart.seriesHeight3D), new Point(OldXPos, OldYPos) };
                return Graphics3D.PointInPolygon(P, poly);
            }
            if (!this.stairs)
            {
                return Graphics3D.PointInLineTolerance(P, tmpX, tmpY, OldXPos, OldYPos, 3);
            }
            if (this.invertedStairs)
            {
                if (!this.PointInVertLine(P, OldXPos, OldYPos, tmpY))
                {
                    return this.PointInHorizLine(P, OldXPos, tmpY, tmpX);
                }
                return true;
            }
            if (!this.PointInHorizLine(P, OldXPos, OldYPos, tmpX))
            {
                return this.PointInVertLine(P, tmpX, OldYPos, tmpY);
            }
            return true;
        }

        public override int Clicked(int x, int y)
        {
            if (base.chart != null)
            {
                base.chart.graphics3D.Calculate2DPosition(ref x, ref y, base.StartZ);
            }
            int num = base.Clicked(x, y);
            if (((num == -1) && (base.firstVisible > -1)) && (base.lastVisible > -1))
            {
                int oldXPos = 0;
                int oldYPos = 0;
                Point p = new Point(x, y);
                for (int i = base.firstVisible; i <= base.lastVisible; i++)
                {
                    int tmpX = this.CalcXPos(i);
                    int tmpY = this.CalcYPos(i);
                    if (base.Pointer.Visible && this.ClickedPointer(i, tmpX, tmpY, x, y))
                    {
                        base.OnClickPointer(i, x, y);
                        return i;
                    }
                    if ((tmpX == x) && (tmpY == y))
                    {
                        return i;
                    }
                    if ((i > base.firstVisible) && this.bClickableLine)
                    {
                        if (!this.CheckPointInLine(p, tmpX, tmpY, oldXPos, oldYPos))
                        {
                            if (!this.drawArea)
                            {
                                goto Label_0158;
                            }
                            Point[] poly = new Point[] { new Point(oldXPos, oldYPos), new Point(tmpX, tmpY), new Point(tmpX, this.GetOriginPos(i)), new Point(oldXPos, this.GetOriginPos(i - 1)) };
                            if (!Graphics3D.PointInPolygon(p, poly))
                            {
                                goto Label_0158;
                            }
                        }
                        return (i - 1);
                    }
                Label_0158:
                    oldXPos = tmpX;
                    oldYPos = tmpY;
                }
            }
            return num;
        }

        protected internal override void Draw()
        {
            if ((this.outLine != null) && this.outLine.Visible)
            {
                Color color = base.Color;
                base.Color = this.outLine.Color;
                int width = base.pLinePen.Width;
                ChartPen pLinePen = base.pLinePen;
                base.pLinePen = this.outLine;
                base.pLinePen.Width = (width + this.outLine.Width) + 2;
                base.Draw();
                base.pLinePen.Width = width;
                base.pLinePen = pLinePen;
                base.Color = color;
            }
            base.Draw();
        }

        private void DrawArea(Color BrushColor, int x, int y)
        {
            Rectangle rectangle;
            Graphics3D g = base.chart.graphics3D;
            if (!this.bAreaBrush.Color.IsEmpty)
            {
                this.bAreaBrush.Transparency = this.Transparency;
            }
            base.chart.SetBrushCanvas(BrushColor, this.bAreaBrush, this.bAreaBrush.Color.IsEmpty ? base.Color : this.bAreaBrush.Color);
            if (base.chart.Aspect.View3D && this.IsLastValue)
            {
                if (base.yMandatory)
                {
                    g.RectangleZ(x, y, this.BottomPos, base.StartZ, base.EndZ);
                }
                else
                {
                    g.RectangleY(x, y, this.BottomPos, base.StartZ, base.EndZ);
                }
            }
            if (this.stairs)
            {
                int num;
                int bottomPos;
                if (this.invertedStairs)
                {
                    num = base.yMandatory ? y : x;
                    bottomPos = this.BottomPos;
                }
                else
                {
                    num = base.yMandatory ? this.OldY : this.OldX;
                    bottomPos = this.OldBottomPos;
                }
                if (base.yMandatory)
                {
                    rectangle = new Rectangle(this.OldX, num, x - this.OldX, bottomPos - num);
                }
                else
                {
                    rectangle = new Rectangle(bottomPos, y, (num - bottomPos) - 1, this.OldY - y);
                }
                if (!base.chart.Aspect.View3D)
                {
                    g.Rectangle(rectangle);
                }
                else
                {
                    g.Rectangle(rectangle, base.StartZ);
                    if (g.SupportsFullRotation)
                    {
                        g.Rectangle(rectangle, base.EndZ);
                    }
                }
            }
            else
            {
                Point point;
                Point point4;
                if (base.yMandatory)
                {
                    point = new Point(this.OldX, this.OldBottomPos);
                    point4 = new Point(x, this.BottomPos);
                }
                else
                {
                    point = new Point(this.OldBottomPos, this.OldY);
                    point4 = new Point(this.BottomPos, y);
                }
                Point point2 = new Point(this.OldX, this.OldY);
                Point point3 = new Point(x, y);
                if (base.chart.Aspect.View3D)
                {
                    g.Plane(point, point2, point3, point4, base.startZ);
                }
                else
                {
                    Point[] pointArray;
                    if ((this.Brush != null) && this.Brush.GradientVisible)
                    {
                        pointArray = new Point[] { point, point2, point3, point4 };
                        g.ClipPolygon(pointArray);
                        int num3 = base.CalcPosValue(base.mandatory.Maximum);
                        int height = base.CalcPosValue(base.mandatory.Minimum);
                        if (base.yMandatory)
                        {
                            rectangle = new Rectangle(this.OldX, num3, x, height);
                        }
                        else
                        {
                            rectangle = new Rectangle(height, this.OldY, num3, y);
                        }
                        this.Brush.Gradient.Draw(g, rectangle);
                        g.UnClip();
                        this.Brush.Visible = false;
                        if (this.pAreaLines.bVisible)
                        {
                            if (base.yMandatory)
                            {
                                g.VerticalLine(this.OldX, this.OldY, this.OldBottomPos);
                            }
                            else
                            {
                                g.HorizontalLine(this.OldBottomPos, this.OldX, this.OldY);
                            }
                        }
                    }
                    else
                    {
                        pointArray = new Point[] { point, point2, point3, point4 };
                        g.Polygon(pointArray);
                    }
                }
                if (g.SupportsFullRotation)
                {
                    g.Plane(point, point2, point3, point4, base.EndZ);
                }
                if (base.pLinePen.bVisible)
                {
                    g.Pen = base.LinePen;
                    g.Line(this.OldX, this.OldY, x, y, base.StartZ);
                }
            }
        }

        protected override void DrawLegendShape(Graphics3D g, int valueIndex, Rectangle rect)
        {
            Color tmpColor = (valueIndex == -1) ? base.Color : this.ValueColor(valueIndex);
            if (base.Pointer.Visible)
            {
                if (this.drawLine)
                {
                    this.DrawLine(g, false, tmpColor, rect);
                }
                base.point.DrawLegendShape(g, tmpColor, rect, base.LinePen.bVisible);
            }
            else if (this.drawLine && !this.drawArea)
            {
                this.DrawLine(g, base.chart.aspect.view3D, tmpColor, rect);
            }
            else
            {
                base.DrawLegendShape(g, valueIndex, rect);
            }
        }

        private void DrawLine(Graphics3D g, bool DrawRectangle, Color tmpColor, Rectangle Rect)
        {
            if (base.Chart.Legend.Symbol.DefaultPen)
            {
                this.LinePrepareCanvas(g, tmpColor);
            }
            if (DrawRectangle)
            {
                g.Rectangle(Rect);
            }
            else
            {
                g.HorizontalLine(Rect.X, Rect.Right, (Rect.Y + Rect.Bottom) / 2);
            }
        }

        private void DrawPoint(bool drawOldPointer, int valueIndex, int x, int y)
        {
            Point[] p = new Point[4];
            Graphics3D g = base.chart.graphics3D;
            if ((((x != this.OldX) || (y != this.OldY)) && (!this.tmpColor.IsEmpty && base.chart.aspect.view3D)) && (this.drawArea || this.drawLine))
            {
                Color areaBrushColor;
                g.Pen = base.LinePen;
                if (this.tmpColor == Color.Transparent)
                {
                    g.Pen.Color = this.tmpColor;
                }
                g.Brush = this.Brush;
                if (this.colorEachLine || this.drawArea)
                {
                    areaBrushColor = this.GetAreaBrushColor(this.tmpColor);
                }
                else
                {
                    areaBrushColor = base.Color;
                }
                Color color2 = g.Brush.Color;
                g.Brush.Color = areaBrushColor;
                Point point = new Point(x, y);
                Point point2 = new Point(this.OldX, this.OldY);
                if (this.stairs)
                {
                    if (this.invertedStairs)
                    {
                        if (this.bDark3D)
                        {
                            g.Brush.ApplyDark(0x40);
                        }
                        g.RectangleZ(point2.X, point2.Y, y, base.StartZ, base.EndZ);
                        if (this.bDark3D)
                        {
                            g.Brush.Color = areaBrushColor;
                        }
                        g.RectangleY(point.X, point.Y, this.OldX, base.StartZ, base.EndZ);
                    }
                    else
                    {
                        g.RectangleY(point2.X, point2.Y, x, base.StartZ, base.EndZ);
                        if (this.bDark3D)
                        {
                            g.Brush.ApplyDark(0x40);
                        }
                        g.RectangleZ(point.X, point.Y, this.OldY, base.StartZ, base.EndZ);
                        if (this.bDark3D)
                        {
                            g.Brush.Color = areaBrushColor;
                        }
                    }
                }
                else
                {
                    if ((this.lineHeight > 0) && !this.drawArea)
                    {
                        p[0] = point;
                        p[1] = point2;
                        p[2].X = point2.X;
                        p[2].Y = point2.Y + this.lineHeight;
                        p[3].X = point.X;
                        p[3].Y = point.Y + this.lineHeight;
                        g.Plane(base.StartZ, base.StartZ, p);
                        if (this.IsLastValue)
                        {
                            g.RectangleZ(point.X, point.Y, point.Y + this.lineHeight, base.StartZ, base.EndZ);
                        }
                    }
                    bool flag = this.bDark3D && !g.SupportsFullRotation;
                    if (flag)
                    {
                        int num = point.X - point2.X;
                        if (((num != 0) && (this.tmpDark3DRatio != 0.0)) && (((point2.Y - point.Y) / num) > this.tmpDark3DRatio))
                        {
                            g.Brush.ApplyDark(0x40);
                            if ((this.lineHeight > 0) && !this.drawArea)
                            {
                                point.Y += this.lineHeight;
                                point2.Y += this.lineHeight;
                            }
                        }
                    }
                    if (g.Monochrome)
                    {
                        g.Brush.Color = Color.White;
                    }
                    g.Plane(point, point2, base.StartZ, base.EndZ);
                    if (flag)
                    {
                        g.Brush.Color = areaBrushColor;
                    }
                }
                g.Brush.Color = color2;
            }
            if (this.drawArea)
            {
                g.Brush.Color = this.GetAreaBrushColor(this.tmpColor);
                g.Pen = this.pAreaLines;
                if (this.pAreaLines.Color.IsEmpty || !this.pAreaLines.Visible)
                {
                    g.Pen.Color = this.tmpColor;
                }
                else
                {
                    g.Pen = this.pAreaLines;
                }
                this.DrawArea(g.Brush.Color, x, y);
            }
            else if (!base.chart.Aspect.View3D && this.drawLine)
            {
                this.LinePrepareCanvas(g, this.colorEachLine ? this.tmpColor : base.Color);
                if (this.stairs)
                {
                    if (this.invertedStairs)
                    {
                        g.VerticalLine(this.OldX, this.OldY, y);
                    }
                    else
                    {
                        g.HorizontalLine(this.OldX, x, this.OldY);
                    }
                    g.LineTo(x, y);
                }
                else
                {
                    g.Line(this.OldX, this.OldY, x, y);
                }
            }
            if (base.point.Visible && drawOldPointer)
            {
                if (!this.OldColor.IsEmpty)
                {
                    base.DrawPointer(this.OldX, this.OldY, this.OldColor, valueIndex - 1);
                }
                if (this.IsLastValue && !this.tmpColor.IsEmpty)
                {
                    base.DrawPointer(x, y, this.tmpColor, valueIndex);
                }
            }
        }

        public override void DrawValue(int valueIndex)
        {
            int firstVisible;
            Graphics3D graphicsd = base.chart.graphics3D;
            this.tmpColor = this.ValueColor(valueIndex);
            int x = this.CalcXPos(valueIndex);
            int y = this.CalcYPos(valueIndex);
            graphicsd.Pen.Color = Color.Black;
            graphicsd.Brush.Color = this.tmpColor;
            if (this.OldColor.IsEmpty)
            {
                this.OldX = x;
                this.OldY = y;
            }
            this.BottomPos = this.GetOriginPos(valueIndex);
            if (this.DrawValuesForward())
            {
                firstVisible = base.firstVisible;
                this.IsLastValue = valueIndex == base.lastVisible;
            }
            else
            {
                firstVisible = base.lastVisible;
                this.IsLastValue = valueIndex == base.firstVisible;
            }
            if (valueIndex == firstVisible)
            {
                if (this.bDark3D)
                {
                    if (base.chart.seriesWidth3D != 0)
                    {
                        this.tmpDark3DRatio = Math.Abs((int) (base.chart.seriesHeight3D / base.chart.seriesWidth3D));
                    }
                    else
                    {
                        this.tmpDark3DRatio = 1.0;
                    }
                }
                if ((firstVisible == base.firstVisible) && (valueIndex > 0))
                {
                    if (this.drawArea)
                    {
                        this.OldX = this.CalcXPos(valueIndex - 1);
                        this.OldY = this.CalcYPos(valueIndex - 1);
                        this.OldBottomPos = this.GetOriginPos(valueIndex - 1);
                    }
                    else
                    {
                        Rectangle chartRect = base.chart.ChartRect;
                        this.OldX = base.GetHorizAxis.Inverted ? chartRect.Right : chartRect.X;
                        if (this.stairs)
                        {
                            this.OldY = this.CalcYPos(valueIndex - 1);
                        }
                        else
                        {
                            this.OldY = this.CalcYPosLeftRight(base.XScreenToValue(this.OldX), valueIndex - 1, valueIndex);
                        }
                    }
                    if (!base.IsNull(valueIndex - 1))
                    {
                        this.DrawPoint(false, valueIndex, x, y);
                    }
                }
                if (this.IsLastValue && base.point.Visible)
                {
                    base.DrawPointer(x, y, this.tmpColor, valueIndex);
                }
                if ((graphicsd.SupportsFullRotation && this.drawArea) && base.chart.aspect.view3D)
                {
                    graphicsd.RectangleZ(x, y, this.BottomPos, base.StartZ, base.EndZ);
                }
            }
            else if (!base.IsNull(valueIndex - 1))
            {
                this.DrawPoint(true, valueIndex, x, y);
            }
            this.OldX = x;
            this.OldY = y;
            this.OldBottomPos = this.BottomPos;
            this.OldColor = this.tmpColor;
        }

        protected Color GetAreaBrushColor(Color c)
        {
            if (!base.bColorEach)
            {
                if (this.bAreaBrush == null)
                {
                    return c;
                }
                if (!this.bAreaBrush.Color.IsEmpty)
                {
                    return this.bAreaBrush.Color;
                }
            }
            return c;
        }

        private void InternalCalcMargin(bool sameSide, bool horizontal, ref int a, ref int b)
        {
            if (horizontal)
            {
                base.Pointer.CalcHorizMargins(ref a, ref b);
            }
            else
            {
                base.Pointer.CalcVerticalMargins(ref a, ref b);
            }
            if (this.drawLine)
            {
                if (this.stairs)
                {
                    a = Math.Max(a, base.pLinePen.Width);
                    b = Math.Max(b, base.pLinePen.Width + 1);
                }
                if ((this.outLine != null) && this.outLine.Visible)
                {
                    a = Math.Max(a, this.outLine.Width);
                    b = Math.Max(b, this.outLine.Width);
                }
            }
            if (base.marks.visible && sameSide)
            {
                if (base.yMandatory)
                {
                    a = Math.Max(a, base.marks.ArrowLength);
                }
                else
                {
                    b = Math.Max(b, base.marks.ArrowLength);
                }
            }
            if (base.marks.visible && sameSide)
            {
                int num = base.marks.Callout.Length + base.marks.Callout.Distance;
                if (base.yMandatory)
                {
                    a = Math.Max(b, num);
                }
                else
                {
                    b = Math.Max(a, num);
                }
            }
        }

        private void LinePrepareCanvas(Graphics3D g, Color tmpColor)
        {
            if (g.Monochrome)
            {
                tmpColor = Color.White;
            }
            if (base.chart.Aspect.View3D)
            {
                g.Brush = this.Brush;
                if (base.bBrush.Image != null)
                {
                    g.Brush.Image = base.bBrush.Image;
                }
                else
                {
                    g.Brush.Style = base.bBrush.Style;
                    g.Brush.Color = tmpColor;
                }
                g.Pen = base.LinePen;
                if (tmpColor == Color.Transparent)
                {
                    g.Pen.Color = tmpColor;
                }
            }
            else
            {
                g.Brush.Visible = false;
                g.Pen = base.LinePen;
                g.Pen.Color = tmpColor;
            }
        }

        private bool PointInHorizLine(Point P, int x0, int y0, int x1)
        {
            return Graphics3D.PointInLineTolerance(P, x0, y0, x1, y0, 3);
        }

        private bool PointInVertLine(Point P, int x0, int y0, int y1)
        {
            return Graphics3D.PointInLineTolerance(P, x0, y0, x0, y1, 3);
        }

        protected override void SetChart(Chart c)
        {
            base.SetChart(c);
            if (this.bAreaBrush != null)
            {
                this.bAreaBrush.Chart = c;
            }
            if (this.pAreaLines != null)
            {
                this.pAreaLines.Chart = c;
            }
            if (this.outLine != null)
            {
                this.outLine.Chart = c;
            }
        }

        protected override void SetSeriesColor(Color value)
        {
            base.SetSeriesColor(value);
            if (this.bAreaBrush != null)
            {
                this.bAreaBrush.Color = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance"), Description("Sets Brush characteristics.")]
        public ChartBrush Brush
        {
            get
            {
                return base.bBrush;
            }
        }

        [DefaultValue(true), Description("Allows mouse clicks over the line drawn between points.")]
        public bool ClickableLine
        {
            get
            {
                return this.bClickableLine;
            }
            set
            {
                base.SetBooleanProperty(ref this.bClickableLine, value);
            }
        }

        [DefaultValue(true), Category("Appearance"), Description("Enables/Disables the coloring of each connecting line of a series.")]
        public bool ColorEachLine
        {
            get
            {
                return this.colorEachLine;
            }
            set
            {
                base.SetBooleanProperty(ref this.colorEachLine, value);
            }
        }

        [Category("Appearance"), Description("Darkens parts of 3D Line Series to add depth."), DefaultValue(true)]
        public bool Dark3D
        {
            get
            {
                return this.bDark3D;
            }
            set
            {
                base.SetBooleanProperty(ref this.bDark3D, value);
            }
        }

        [Description("Changes the direction of the step, when true."), DefaultValue(false)]
        public bool InvertedStairs
        {
            get
            {
                return this.invertedStairs;
            }
            set
            {
                base.SetBooleanProperty(ref this.invertedStairs, value);
            }
        }

        [DefaultValue(0), Description("Defines the Height of the line in pixels.")]
        public int LineHeight
        {
            get
            {
                return this.lineHeight;
            }
            set
            {
                base.SetIntegerProperty(ref this.lineHeight, value);
            }
        }

        [Description("Sets Opacity level from 0 to 100%"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public int Opacity
        {
            get
            {
                return (100 - this.Transparency);
            }
            set
            {
                this.Transparency = 100 - value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), DefaultValue((string) null), Description("Pen for Series Line's outer pen.")]
        public ChartPen OutLine
        {
            get
            {
                if (this.outLine == null)
                {
                    this.outLine = new ChartPen(base.chart, Color.Black, false, LineCap.Round);
                }
                return this.outLine;
            }
        }

        [DefaultValue(false), Description("Steps line joining adjacent points.")]
        public bool Stairs
        {
            get
            {
                return this.stairs;
            }
            set
            {
                base.SetBooleanProperty(ref this.stairs, value);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Description("Sets Transparency level from 0 to 100%"), Category("Appearance"), DefaultValue(0)]
        public int Transparency
        {
            get
            {
                return base.bBrush.Transparency;
            }
            set
            {
                base.bBrush.Transparency = value;
            }
        }
    }
}

