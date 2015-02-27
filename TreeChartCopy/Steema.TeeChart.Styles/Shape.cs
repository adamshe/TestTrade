namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    [ToolboxBitmap(typeof(Steema.TeeChart.Styles.Shape), "SeriesIcons.Shape.bmp")]
    public class Shape : Series
    {
        private TextShape shape;
        private ShapeStyles style;
        private ShapeTextHorizAlign textHorizAlign;
        private ShapeTextVertAlign textVertAlign;
        private int tmpMidX;
        private int tmpMidY;
        private ShapeXYStyles xyStyle;

        public Shape() : this(null)
        {
        }

        public Shape(Chart c) : base(c)
        {
            this.style = ShapeStyles.Circle;
            this.xyStyle = ShapeXYStyles.Axis;
            this.textVertAlign = ShapeTextVertAlign.Center;
            this.textHorizAlign = ShapeTextHorizAlign.Center;
            this.shape = new TextShape(c);
            this.AddDefaultPoints();
        }

        private void AddDefaultPoints()
        {
            base.Add((double) 0.0, (double) 0.0);
            base.Add((double) 100.0, (double) 100.0);
        }

        protected override void AddSampleValues(int numValues)
        {
            Series.SeriesRandom random = base.RandomBounds(1);
            if (random.StepX == 0.0)
            {
                this.AddDefaultPoints();
            }
            else
            {
                base.Add((double) (random.tmpX + (random.StepX / 8.0)), (double) (random.tmpY / 2.0));
                base.Add((double) ((random.tmpX + random.StepX) - (random.StepX / 8.0)), (double) (random.tmpY + Utils.Round((double) (random.DifY * random.Random()))));
            }
        }

        protected internal override void CalcZOrder()
        {
            if (base.UseAxis)
            {
                base.CalcZOrder();
            }
        }

        public override int Clicked(int x, int y)
        {
            int num;
            int num2;
            bool flag;
            if (base.chart != null)
            {
                base.chart.graphics3D.Calculate2DPosition(ref x, ref y, base.StartZ);
            }
            Point p = new Point(x, y);
            Rectangle shapeRectangle = this.GetShapeRectangle();
            Graphics3D.RectCenter(shapeRectangle, out num, out num2);
            switch (this.style)
            {
                case ShapeStyles.Circle:
                    flag = Graphics3D.PointInEllipse(p, shapeRectangle);
                    break;

                case ShapeStyles.VertLine:
                    flag = Graphics3D.PointInLineTolerance(p, num, shapeRectangle.Y, num, shapeRectangle.Bottom, 3);
                    break;

                case ShapeStyles.HorizLine:
                    flag = Graphics3D.PointInLineTolerance(p, shapeRectangle.X, num2, shapeRectangle.Right, num2, 3);
                    break;

                case ShapeStyles.Triangle:
                case ShapeStyles.Pyramid:
                    flag = Graphics3D.PointInTriangle(p, shapeRectangle.X, shapeRectangle.Right, shapeRectangle.Bottom, shapeRectangle.Y);
                    break;

                case ShapeStyles.InvertTriangle:
                case ShapeStyles.InvertPyramid:
                    flag = Graphics3D.PointInTriangle(p, shapeRectangle.X, shapeRectangle.Right, shapeRectangle.Y, shapeRectangle.Bottom);
                    break;

                case ShapeStyles.Line:
                    flag = Graphics3D.PointInLineTolerance(p, shapeRectangle.X, shapeRectangle.Y, shapeRectangle.Right, shapeRectangle.Bottom, 3);
                    break;

                case ShapeStyles.Diamond:
                {
                    Point[] poly = new Point[] { new Point(num, shapeRectangle.Y), new Point(shapeRectangle.Right, num2), new Point(num, shapeRectangle.Bottom), new Point(shapeRectangle.X, num2) };
                    flag = Graphics3D.PointInPolygon(p, poly);
                    break;
                }
                default:
                    flag = shapeRectangle.Contains(x, y);
                    break;
            }
            if (!flag)
            {
                return -1;
            }
            return 0;
        }

        protected internal override void CreateSubGallery(Series.SubGalleryEventHandler AddSubChart)
        {
            base.CreateSubGallery(AddSubChart);
            AddSubChart(Texts.Rectangle);
            AddSubChart(Texts.VertLine);
            AddSubChart(Texts.HorizLine);
            AddSubChart(Texts.Ellipse);
            AddSubChart(Texts.DownTri);
            AddSubChart(Texts.Line);
            AddSubChart(Texts.Diamond);
            AddSubChart(Texts.Cube);
            AddSubChart(Texts.Cross);
            AddSubChart(Texts.DiagCross);
            AddSubChart(Texts.Star);
            AddSubChart(Texts.Pyramid);
            AddSubChart(Texts.InvPyramid);
            AddSubChart(Texts.Hollow);
        }

        private void DoGradient(bool Is3D, Rectangle r)
        {
            if (!this.Transparent && this.Gradient.Visible)
            {
                Rectangle rectangle = Is3D ? base.chart.graphics3D.CalcRect3D(r, base.MiddleZ) : r;
                if (this.style == ShapeStyles.Circle)
                {
                    base.chart.graphics3D.ClipEllipse(rectangle);
                }
                this.Gradient.Draw(base.chart.graphics3D, rectangle);
                if (this.style == ShapeStyles.Circle)
                {
                    base.chart.graphics3D.UnClip();
                }
                base.chart.graphics3D.Brush.Visible = false;
            }
        }

        private void DrawCross2D(Rectangle r)
        {
            base.chart.graphics3D.VerticalLine(this.tmpMidX, r.Y, r.Bottom + 1);
            base.chart.graphics3D.HorizontalLine(r.X, r.Right + 1, this.tmpMidY);
        }

        private void DrawCross3D(Rectangle r)
        {
            base.chart.Graphics3D.VerticalLine(this.tmpMidX, r.Y, r.Bottom, base.MiddleZ);
            base.chart.Graphics3D.HorizontalLine(r.X, r.Right, this.tmpMidY, base.MiddleZ);
        }

        private void DrawDiagonalCross2D(Rectangle r)
        {
            base.chart.graphics3D.Line(r.X, r.Y, r.Right + 1, r.Bottom + 1);
            base.chart.graphics3D.Line(r.X, r.Bottom, r.Right + 1, r.Y - 1);
        }

        private void DrawDiagonalCross3D(Rectangle r)
        {
            base.chart.graphics3D.Line(r.X, r.Y, r.Right, r.Bottom, base.MiddleZ);
            base.chart.graphics3D.Line(r.X, r.Bottom, r.Right, r.Y, base.MiddleZ);
        }

        protected override void DrawLegendShape(Graphics3D g, int valueIndex, Rectangle rect)
        {
            this.DrawShape(g, false, rect);
        }

        private void DrawShape(bool Is3D, Rectangle r)
        {
            this.DrawShape(base.chart.graphics3D, Is3D, r);
        }

        private void DrawShape(Graphics3D g, bool Is3D, Rectangle r)
        {
            g.Pen = this.Pen;
            if (this.Transparent)
            {
                g.Brush.Visible = false;
            }
            else
            {
                g.Brush = this.Brush;
                g.Brush.Color = base.Color;
            }
            Graphics3D.RectCenter(r, out this.tmpMidX, out this.tmpMidY);
            if (Is3D)
            {
                switch (this.style)
                {
                    case ShapeStyles.Rectangle:
                        this.DoGradient(Is3D, r);
                        g.Rectangle(r, base.MiddleZ);
                        return;

                    case ShapeStyles.Circle:
                        this.DoGradient(Is3D, r);
                        g.Ellipse(r, base.MiddleZ);
                        return;

                    case ShapeStyles.VertLine:
                        g.VerticalLine(this.tmpMidX, r.Y, r.Bottom, base.MiddleZ);
                        return;

                    case ShapeStyles.HorizLine:
                        g.HorizontalLine(r.X, r.Right, this.tmpMidY, base.MiddleZ);
                        return;

                    case ShapeStyles.Triangle:
                        g.Triangle(new Point(r.X, r.Bottom), new Point(this.tmpMidX, r.Y), new Point(r.Right, r.Bottom), base.MiddleZ);
                        return;

                    case ShapeStyles.InvertTriangle:
                        g.Triangle(new Point(r.X, r.Y), new Point(this.tmpMidX, r.Bottom), new Point(r.Right, r.Y), base.MiddleZ);
                        return;

                    case ShapeStyles.Line:
                        g.Line(r.X, r.Y, r.Right, r.Bottom, base.MiddleZ);
                        return;

                    case ShapeStyles.Diamond:
                        g.Plane(new Point(r.X, this.tmpMidY), new Point(this.tmpMidX, r.Y), new Point(r.Right, this.tmpMidY), new Point(this.tmpMidX, r.Bottom), base.MiddleZ);
                        return;

                    case ShapeStyles.Cube:
                        g.Cube(r, base.StartZ, base.EndZ, !this.Transparent);
                        return;

                    case ShapeStyles.Cross:
                        this.DrawCross3D(r);
                        return;

                    case ShapeStyles.DiagCross:
                        this.DrawDiagonalCross3D(r);
                        return;

                    case ShapeStyles.Star:
                        this.DrawCross3D(r);
                        this.DrawDiagonalCross3D(r);
                        return;

                    case ShapeStyles.Pyramid:
                        g.Pyramid(true, r, base.StartZ, base.EndZ, !this.Transparent);
                        return;

                    case ShapeStyles.InvertPyramid:
                        g.Pyramid(true, r.X, r.Bottom, r.Right, r.Y, base.StartZ, base.EndZ, !this.Transparent);
                        return;
                }
            }
            else
            {
                Point[] pointArray;
                switch (this.style)
                {
                    case ShapeStyles.Rectangle:
                    {
                        if (this.Format.ShapeStyle != TextShapeStyle.RoundRectangle)
                        {
                            this.DoGradient(Is3D, r);
                            g.Rectangle(Rectangle.FromLTRB(r.X, r.Y, r.Right + 1, r.Bottom + 1));
                            return;
                        }
                        int roundWidth = 12;
                        Rectangle rectangle = new Rectangle(r.Left, r.Top, r.Width - roundWidth, r.Height - roundWidth);
                        g.RoundRectangle(rectangle, roundWidth, roundWidth);
                        return;
                    }
                    case ShapeStyles.Circle:
                        this.DoGradient(Is3D, r);
                        g.Ellipse(r);
                        return;

                    case ShapeStyles.VertLine:
                        g.VerticalLine(this.tmpMidX, r.Y, r.Bottom);
                        return;

                    case ShapeStyles.HorizLine:
                        g.HorizontalLine(r.X, r.Right + 1, this.tmpMidY);
                        return;

                    case ShapeStyles.Triangle:
                    case ShapeStyles.Pyramid:
                        pointArray = new Point[] { new Point(r.X, r.Bottom), new Point(this.tmpMidX, r.Y), new Point(r.Right, r.Bottom) };
                        g.Polygon(pointArray);
                        return;

                    case ShapeStyles.InvertTriangle:
                    case ShapeStyles.InvertPyramid:
                        pointArray = new Point[] { new Point(r.X, r.Y), new Point(this.tmpMidX, r.Bottom), new Point(r.Right, r.Y) };
                        g.Polygon(pointArray);
                        return;

                    case ShapeStyles.Line:
                        g.Line(r.X, r.Y, r.Right, r.Bottom);
                        return;

                    case ShapeStyles.Diamond:
                        pointArray = new Point[] { new Point(r.X, this.tmpMidY), new Point(this.tmpMidX, r.Y), new Point(r.Right, this.tmpMidY), new Point(this.tmpMidX, r.Bottom) };
                        g.Polygon(pointArray);
                        return;

                    case ShapeStyles.Cube:
                        g.Rectangle(r);
                        return;

                    case ShapeStyles.Cross:
                        this.DrawCross2D(r);
                        return;

                    case ShapeStyles.DiagCross:
                        this.DrawDiagonalCross2D(r);
                        return;

                    case ShapeStyles.Star:
                        this.DrawCross2D(r);
                        this.DrawDiagonalCross2D(r);
                        return;
                }
            }
        }

        private void DrawText(Rectangle r)
        {
            int num = 4;
            int x = 0;
            int num3 = 0;
            int num4 = 0;
            int y = 0;
            int top = 0;
            int num7 = 0;
            Graphics3D graphicsd = base.Chart.Graphics3D;
            if (this.Text.Length > 0)
            {
                graphicsd.Font = this.Format.Font;
                num3 = (int) Math.Round((double) graphicsd.TextHeight(this.Font, "H"));
                Graphics3D.RectCenter(r, out num4, out y);
                switch (this.textVertAlign)
                {
                    case ShapeTextVertAlign.Top:
                        top = r.Top;
                        break;

                    case ShapeTextVertAlign.Center:
                        top = y - ((int) Math.Round((double) (((double) (num3 * this.Text.Length)) / 2.0)));
                        break;

                    default:
                        top = r.Bottom - (num3 * this.Text.Length);
                        break;
                }
                for (int i = 0; i < this.Text.Length; i++)
                {
                    num7 = (int) graphicsd.TextWidth(this.Text[i]);
                    switch (this.textHorizAlign)
                    {
                        case ShapeTextHorizAlign.Left:
                            x = (r.Left + this.Pen.Width) + num;
                            break;

                        case ShapeTextHorizAlign.Center:
                            x = num4 - (num7 / 2);
                            break;

                        default:
                            x = ((r.Right - this.Pen.Width) - num7) - num;
                            break;
                    }
                    if (this.XYStyle == ShapeXYStyles.Pixels)
                    {
                        graphicsd.TextOut(x, top, this.Text[i]);
                    }
                    else
                    {
                        graphicsd.TextOut(x, top, base.StartZ, this.Text[i]);
                    }
                    top += num3;
                }
            }
        }

        public override void DrawValue(int valueIndex)
        {
            if ((base.Count == 2) && (valueIndex == 0))
            {
                Rectangle adjustedRectangle = this.GetAdjustedRectangle();
                if (adjustedRectangle.IntersectsWith(base.chart.ChartRect))
                {
                    bool flag = (this.xyStyle != ShapeXYStyles.Pixels) && base.chart.aspect.View3D;
                    this.DrawShape(flag, (this.style == ShapeStyles.Line) ? this.GetShapeRectangle() : adjustedRectangle);
                    this.DrawText(adjustedRectangle);
                }
            }
        }

        private Rectangle GetAdjustedRectangle()
        {
            Rectangle shapeRectangle = this.GetShapeRectangle();
            if (shapeRectangle.Y == shapeRectangle.Bottom)
            {
                shapeRectangle.Height = 1;
            }
            else if (shapeRectangle.Y > shapeRectangle.Bottom)
            {
                int height = shapeRectangle.Height;
                shapeRectangle.Y = shapeRectangle.Bottom;
                shapeRectangle.Height = -height;
            }
            if (shapeRectangle.X == shapeRectangle.Right)
            {
                shapeRectangle.Width = 1;
                return shapeRectangle;
            }
            if (shapeRectangle.X > shapeRectangle.Right)
            {
                int width = shapeRectangle.Width;
                shapeRectangle.X = shapeRectangle.Right;
                shapeRectangle.Width = -width;
            }
            return shapeRectangle;
        }

        private Rectangle GetShapeRectangle()
        {
            int num;
            int num2;
            int num3;
            int num4;
            switch (this.xyStyle)
            {
                case ShapeXYStyles.Pixels:
                    num = (int) this.X0;
                    num3 = (int) this.Y0;
                    num2 = (int) this.X1;
                    num4 = (int) this.Y1;
                    break;

                case ShapeXYStyles.Axis:
                    num = this.CalcXPos(0);
                    num3 = this.CalcYPos(0);
                    num2 = this.CalcXPos(1);
                    num4 = this.CalcYPos(1);
                    break;

                default:
                    num = this.CalcXPos(0);
                    num3 = this.CalcYPos(0);
                    num2 = num + ((int) this.X1);
                    num4 = num3 + ((int) this.Y1);
                    break;
            }
            return Rectangle.FromLTRB(num, num3, num2, num4);
        }

        public override bool IsValidSourceOf(Series s)
        {
            return (s is Steema.TeeChart.Styles.Shape);
        }

        protected override bool MoreSameZOrder()
        {
            return false;
        }

        internal override void PrepareForGallery(bool IsEnabled)
        {
            base.PrepareForGallery(IsEnabled);
            this.Font.Color = Color.White;
            this.Font.Size = 12;
            this.shape.Lines = new string[1];
            if (base.chart.Series.IndexOf(this) == 1)
            {
                this.style = ShapeStyles.Circle;
                this.Brush.Color = IsEnabled ? Color.Blue : Color.Silver;
                this.Text[0] = Texts.ShapeGallery1;
            }
            else
            {
                this.style = ShapeStyles.Triangle;
                this.Brush.Color = IsEnabled ? Color.Red : Color.Silver;
                this.Text[0] = Texts.ShapeGallery2;
            }
        }

        protected override void SetChart(Chart c)
        {
            base.SetChart(c);
            if (this.shape != null)
            {
                this.shape.Chart = c;
            }
        }

        protected internal override void SetSubGallery(int index)
        {
            switch (index)
            {
                case 1:
                    this.Style = ShapeStyles.Rectangle;
                    return;

                case 2:
                    this.Style = ShapeStyles.VertLine;
                    return;

                case 3:
                    this.Style = ShapeStyles.HorizLine;
                    return;

                case 4:
                    this.Style = ShapeStyles.Circle;
                    return;

                case 5:
                    this.Style = ShapeStyles.InvertTriangle;
                    return;

                case 6:
                    this.Style = ShapeStyles.Line;
                    return;

                case 7:
                    this.Style = ShapeStyles.Diamond;
                    return;

                case 8:
                    this.Style = ShapeStyles.Cube;
                    return;

                case 9:
                    this.Style = ShapeStyles.Cross;
                    return;

                case 10:
                    this.Style = ShapeStyles.DiagCross;
                    return;

                case 11:
                    this.Style = ShapeStyles.Star;
                    return;

                case 12:
                    this.Style = ShapeStyles.Pyramid;
                    return;

                case 13:
                    this.Style = ShapeStyles.InvertPyramid;
                    return;

                case 14:
                    this.Transparent = !this.Transparent;
                    return;
            }
            base.SetSubGallery(index);
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Defines the brush used to fill shape background."), Category("Appearance")]
        public ChartBrush Brush
        {
            get
            {
                return this.shape.Brush;
            }
        }

        [Description("")]
        public override string Description
        {
            get
            {
                return Texts.GalleryShape;
            }
        }

        [Description("Determines the font attributes used to output ShapeSeries."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ChartFont Font
        {
            get
            {
                return this.shape.Font;
            }
        }

        public TextShape Format
        {
            get
            {
                return this.shape;
            }
        }

        [Category("Appearance"), Description("Gets Gradient fill characteristics for the ShapeSeries Shape."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Steema.TeeChart.Drawing.Gradient Gradient
        {
            get
            {
                return this.shape.Gradient;
            }
        }

        [Description("Horizontally aligns the text.")]
        public ShapeTextHorizAlign HorizAlignment
        {
            get
            {
                return this.textHorizAlign;
            }
            set
            {
                if (this.textHorizAlign != value)
                {
                    this.textHorizAlign = value;
                }
                base.Repaint();
            }
        }

        [Description("Defines pen to draw Series Shape."), Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ChartPen Pen
        {
            get
            {
                return this.shape.Pen;
            }
        }

        [Description("Defines how a TChartShape component appears on a Chart.")]
        public ShapeStyles Style
        {
            get
            {
                return this.style;
            }
            set
            {
                this.style = value;
                base.Chart.Invalidate();
            }
        }

        [Description("Displays customized strings inside Shapes.")]
        public string[] Text
        {
            get
            {
                if (this.shape.Lines == null)
                {
                    this.shape.Lines = new string[0];
                }
                return this.shape.Lines;
            }
            set
            {
                this.shape.Lines = value;
            }
        }

        [DefaultValue(false), Description("Allows Shape Brush attributes to fill the Shape interior.")]
        public bool Transparent
        {
            get
            {
                return this.shape.Transparent;
            }
            set
            {
                this.shape.Transparent = value;
            }
        }

        [Description("Sets the vertical alignment of Text within a TChartShape Series shape.")]
        public ShapeTextVertAlign VertAlignment
        {
            get
            {
                return this.textVertAlign;
            }
            set
            {
                if (this.textVertAlign != value)
                {
                    this.textVertAlign = value;
                }
                base.Repaint();
            }
        }

        [Description("Coordinate used to define the englobing ShapeSeries rectangle.")]
        public double X0
        {
            get
            {
                return base.vxValues[0];
            }
            set
            {
                base.vxValues[0] = value;
                this.Invalidate();
            }
        }

        [Description("Coordinate used to define the englobing ShapeSeries rectangle.")]
        public double X1
        {
            get
            {
                return base.vxValues[1];
            }
            set
            {
                base.vxValues[1] = value;
                this.Invalidate();
            }
        }

        [Description("")]
        public ShapeXYStyles XYStyle
        {
            get
            {
                return this.xyStyle;
            }
            set
            {
                if (this.xyStyle != value)
                {
                    this.xyStyle = value;
                    base.Repaint();
                }
            }
        }

        [Description("Coordinate used to define the englobing ShapeSeries rectangle.")]
        public double Y0
        {
            get
            {
                return base.vyValues[0];
            }
            set
            {
                base.vyValues[0] = value;
                this.Invalidate();
            }
        }

        [Description("Coordinate used to define the englobing ShapeSeries rectangle.")]
        public double Y1
        {
            get
            {
                return base.vyValues[1];
            }
            set
            {
                base.vyValues[1] = value;
                this.Invalidate();
            }
        }
    }
}

