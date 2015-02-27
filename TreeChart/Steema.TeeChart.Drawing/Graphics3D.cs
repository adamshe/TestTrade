namespace Steema.TeeChart.Drawing
{
    using Steema.TeeChart;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Text;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public abstract class Graphics3D : TeeBase
    {
        protected System.Drawing.Drawing2D.SmoothingMode aSmoothingMode;
        protected internal Aspect aspect;
        protected System.Drawing.Text.TextRenderingHint aTextRenderingHint;
        private ChartBrush bBrush;
        private System.Drawing.Rectangle bounds;
        private bool buffered;
        private double c1;
        private double c2c1;
        private double c2c3;
        private double c2s1;
        private double c2s3;
        private double c3;
        public static Color[] ColorPalette = new Color[] { 
            Color.Red, Color.Green, Color.Yellow, Color.Blue, Color.White, Color.Gray, Color.Fuchsia, Color.Teal, Color.Navy, Color.Maroon, Color.Lime, Color.Olive, Color.Purple, Color.Silver, Color.Aqua, Color.Black, 
            Color.GreenYellow, Color.SkyBlue, Color.Bisque, Color.Indigo
         };
        internal const int DarkColorQuantity = 0x40;
        internal const int DarkerColorQuantity = 0x80;
        internal bool Dirty;
        private ChartFont font;
        public Point[] FourPoints;
        protected internal Graphics g;
        private double IOrthoX;
        private double IOrthoY;
        private double IPerspec;
        private Point[] IPoints;
        private double IZoomFactor;
        private bool IZoomText;
        private SizeF layoutArea;
        internal bool metafiling;
        public bool Monochrome;
        private ChartPen pen;
        private Pie3D pie3D;
        public const double PiStep = 0.017453292519943295;
        private PointXYZ rotationCenter;
        private double s1;
        private double s2;
        private double s3;
        protected StringFormat stringFormat;
        private bool supports3DText;
        private double tempXX;
        private double tempXZ;
        private double tempYX;
        private double tempYZ;
        private int xCenter;
        private int xCenterOffset;
        private int yCenter;
        private int yCenterOffset;
        private int zCenter;

        public Graphics3D(Chart c) : base(c)
        {
            this.FourPoints = new Point[4];
            this.aSmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
            this.aTextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.stringFormat = new StringFormat();
            this.pen = new ChartPen(Color.Black);
            this.bBrush = new ChartBrush(null);
            this.font = new ChartFont(null);
            this.buffered = true;
            this.rotationCenter = new PointXYZ();
            this.IPoints = new Point[4];
            this.Dirty = true;
            this.layoutArea = new SizeF(1000f, 1000f);
            this.pen.chart = c;
            this.bBrush.chart = c;
            this.font.chart = c;
            this.aspect = base.chart.aspect;
        }

        private Region AddRightRegion(System.Drawing.Rectangle rect, int minZ, int maxZ)
        {
            Point[] points = new Point[6];
            points[0] = this.Calc3DPoint(rect.X, rect.Bottom, minZ);
            points[1] = this.Calc3DPoint(rect.X, rect.Y, minZ);
            Point point = this.Calc3DPoint(rect.X, rect.Y, maxZ);
            Point point2 = this.Calc3DPoint(rect.Right, rect.Y, minZ);
            points[2] = (point2.Y < point.Y) ? point2 : point;
            points[3] = this.Calc3DPoint(rect.Right, rect.Y, maxZ);
            point = this.Calc3DPoint(rect.Right, rect.Bottom, maxZ);
            point2 = this.Calc3DPoint(rect.Right, rect.Y, minZ);
            points[4] = (point2.X > point.X) ? point2 : point;
            points[5] = this.Calc3DPoint(rect.Right, rect.Bottom, minZ);
            if (points[5].X < points[0].X)
            {
                points[0].X = points[5].X;
                if (point2.Y < points[0].Y)
                {
                    points[0].Y = point2.Y;
                }
            }
            GraphicsPath path = new GraphicsPath();
            path.AddPolygon(points);
            return new Region(path);
        }

        public static void ApplyBright(ref Color c, byte howMuch)
        {
            byte r = c.R;
            byte g = c.G;
            byte b = c.B;
            if ((r + howMuch) < 0x100)
            {
                r = (byte) (r + howMuch);
            }
            else
            {
                r = 0xff;
            }
            if ((g + howMuch) < 0x100)
            {
                g = (byte) (g + howMuch);
            }
            else
            {
                g = 0xff;
            }
            if ((b + howMuch) < 0x100)
            {
                b = (byte) (b + howMuch);
            }
            else
            {
                b = 0xff;
            }
            c = Color.FromArgb(c.A, r, g, b);
        }

        public static void ApplyDark(ref Color c, byte howMuch)
        {
            byte r = c.R;
            byte g = c.G;
            byte b = c.B;
            if (r > howMuch)
            {
                r = (byte) (r - howMuch);
            }
            else
            {
                r = 0;
            }
            if (g > howMuch)
            {
                g = (byte) (g - howMuch);
            }
            else
            {
                g = 0;
            }
            if (b > howMuch)
            {
                b = (byte) (b - howMuch);
            }
            else
            {
                b = 0;
            }
            c = Color.FromArgb(c.A, r, g, b);
        }

        public abstract void Arc(int x1, int y1, int x2, int y2, float startAngle, float sweepAngle);
        public abstract void Arc(int x1, int y1, int x2, int y2, int x3, int y3, int x4, int y4);
        public void Arrow(bool filled, Point fromPoint, Point toPoint, int headWidth, int headHeight, int z)
        {
            int num = toPoint.X - fromPoint.X;
            int num2 = fromPoint.Y - toPoint.Y;
            double num3 = Math.Sqrt((double) ((num * num) + (num2 * num2)));
            if (num3 > 0.0)
            {
                ArrowPoint point = new ArrowPoint();
                point.z = z;
                point.g = this;
                int num4 = headWidth;
                int num5 = Math.Min(Utils.Round(num3), headHeight);
                point.SinA = ((double) num2) / num3;
                point.CosA = ((double) num) / num3;
                double num6 = (toPoint.X * point.CosA) - (toPoint.Y * point.SinA);
                double num7 = (toPoint.X * point.SinA) + (toPoint.Y * point.CosA);
                point.x = num6 - num5;
                point.y = num7 - (num4 * 0.5);
                Point p = point.Calc();
                point.y = num7 + (num4 * 0.5);
                Point point3 = point.Calc();
                if (filled)
                {
                    double num8 = num4 * 0.25;
                    point.y = num7 - num8;
                    Point point4 = point.Calc();
                    point.y = num7 + num8;
                    Point point5 = point.Calc();
                    point.x = (fromPoint.X * point.CosA) - (fromPoint.Y * point.SinA);
                    point.y = num7 - num8;
                    Point point6 = point.Calc();
                    point.y = num7 + num8;
                    Point point7 = point.Calc();
                    Point[] pointArray = new Point[] { point7, point6, point4, p, this.Calc3DPoint(toPoint, z), point3, point5 };
                    this.Polygon(pointArray);
                }
                else
                {
                    this.MoveTo(fromPoint, z);
                    this.LineTo(toPoint, z);
                    this.LineTo(point3, z);
                    this.MoveTo(toPoint, z);
                    this.LineTo(p, z);
                }
            }
        }

        public Point Calc3DPoint(Point3D p)
        {
            return this.Calc3DPoint(p.X, p.Y, p.Z);
        }

        public Point Calc3DPoint(Point p, int z)
        {
            this.Calc3DPos(ref p, p.X, p.Y, z);
            return p;
        }

        public Point Calc3DPoint(int x, int y, int z)
        {
            this.Calc3DPos(ref x, ref y, z);
            return new Point(x, y);
        }

        public void Calc3DPos(ref Point p, Point source, int z)
        {
            this.Calc3DPos(ref p, source.X, source.Y, z);
        }

        public void Calc3DPos(ref int x, ref int y, int z)
        {
            if (this.aspect.orthogonal)
            {
                x = ((int) (this.IZoomFactor * ((double) ((x - this.xCenter) + z)))) + this.xCenterOffset;
                y = ((int) (this.IZoomFactor * (((double) (y - this.yCenter)) - (this.IOrthoY * z)))) + this.yCenterOffset;
            }
            else if (this.aspect.view3D)
            {
                double iZoomFactor;
                z -= this.zCenter;
                x -= this.xCenter;
                y -= this.yCenter;
                if (this.IPerspec > 0.0)
                {
                    iZoomFactor = this.IZoomFactor * (1.0 - ((((((double) x) * this.c2s1) - (((double) y) * this.s2)) + (z * this.c2c1)) * this.IPerspec));
                }
                else
                {
                    iZoomFactor = this.IZoomFactor;
                }
                int num2 = x;
                x = ((int) ((((((double) x) * this.tempXX) + (((double) y) * this.c2s3)) + (z * this.tempXZ)) * iZoomFactor)) + this.xCenterOffset;
                y = ((int) ((((num2 * this.tempYX) + (((double) y) * this.c2c3)) + (z * this.tempYZ)) * iZoomFactor)) + this.yCenterOffset;
            }
        }

        public void Calc3DPos(ref Point p, int x, int y, int z)
        {
            this.Calc3DPos(ref x, ref y, z);
            p.X = x;
            p.Y = y;
        }

        private static LineOrientations CalcLineParameters(Point PointA, Point PointB, out double Slope, out double Intercept)
        {
            Point point = new Point(PointA.X - PointB.X, PointA.Y - PointB.Y);
            if ((point.X == 0) && (point.Y == 0))
            {
                Slope = 0.0;
                Intercept = 0.0;
                return LineOrientations.Point;
            }
            if (Math.Abs(point.X) >= Math.Abs(point.Y))
            {
                try
                {
                    Slope = Convert.ToDouble(point.Y) / Convert.ToDouble(point.X);
                }
                catch
                {
                    Slope = 0.0;
                }
                Intercept = PointA.Y - (PointA.X * Slope);
                return LineOrientations.Horizontal;
            }
            try
            {
                Slope = Convert.ToDouble(point.X) / Convert.ToDouble(point.Y);
            }
            catch
            {
                Slope = 0.0;
            }
            Intercept = PointA.X - (PointA.Y * Slope);
            return LineOrientations.Vertical;
        }

        internal void CalcPerspective(System.Drawing.Rectangle r)
        {
            this.IPerspec = (this.aspect.Perspective > 0) ? ((((double) this.aspect.Perspective) / 150.0) / ((double) r.Width)) : 0.0;
        }

        public System.Drawing.Rectangle CalcRect3D(System.Drawing.Rectangle r, int z)
        {
            int x = r.X;
            int y = r.Y;
            this.Calc3DPos(ref x, ref y, z);
            int right = r.Right;
            int bottom = r.Bottom;
            this.Calc3DPos(ref right, ref bottom, z);
            r.X = x;
            r.Y = y;
            r.Width = right - r.X;
            r.Height = bottom - r.Y;
            return r;
        }

        internal void CalcTrigValues()
        {
            double elevation;
            double tilt;
            double rotation = 0.0;
            if (!this.aspect.orthogonal)
            {
                rotation = this.aspect.rotation;
                elevation = this.aspect.Elevation;
                tilt = this.aspect.Tilt;
            }
            else
            {
                rotation = 0.0;
                elevation = 0.0;
                tilt = 0.0;
            }
            this.s1 = Math.Sin(rotation * 0.017453292519943295);
            this.c1 = Math.Cos(rotation * 0.017453292519943295);
            this.s2 = Math.Sin(elevation * 0.017453292519943295);
            double num4 = Math.Cos(elevation * 0.017453292519943295);
            this.s3 = Math.Sin(tilt * 0.017453292519943295);
            this.c3 = Math.Cos(tilt * 0.017453292519943295);
            this.c2s3 = num4 * this.s3;
            this.c2c3 = Math.Max((double) 1E-05, (double) (num4 * this.c3));
            this.tempXX = Math.Max((double) 1E-05, (double) (((this.s1 * this.s2) * this.s3) + (this.c1 * this.c3)));
            this.tempYX = ((this.c3 * this.s1) * this.s2) - (this.c1 * this.s3);
            this.tempXZ = ((this.c1 * this.s2) * this.s3) - (this.c3 * this.s1);
            this.tempYZ = ((this.c1 * this.c3) * this.s2) + (this.s1 * this.s3);
            this.c2s1 = num4 * this.s1;
            this.c2c1 = this.c1 * num4;
        }

        public void Calculate2DPosition(ref int x, ref int y, int z)
        {
            if (this.IZoomFactor != 0.0)
            {
                double num2 = 1.0 / this.IZoomFactor;
                if (this.aspect.orthogonal)
                {
                    x = ((int) Math.Round((double) ((((double) (x - this.xCenterOffset)) * num2) - (this.IOrthoX * z)))) + this.xCenter;
                    y = ((int) Math.Round((double) ((((double) (y - this.yCenterOffset)) * num2) + (this.IOrthoY * z)))) + this.yCenter;
                }
                else if ((this.aspect.view3D && (this.tempXX != 0.0)) && (this.c2c3 != 0.0))
                {
                    int num = x;
                    z -= this.zCenter;
                    x = ((int) Math.Round((double) (((((num - this.xCenterOffset) * num2) - (z * this.tempXZ)) - (((double) (y - this.yCenter)) * this.c2s3)) / this.tempXX))) + this.xCenter;
                    y = ((int) Math.Round((double) ((((((double) (y - this.yCenterOffset)) * num2) - (z * this.tempYZ)) - ((num - this.xCenter) * this.tempYX)) / this.c2c3))) + this.yCenter;
                }
            }
        }

        public virtual void Changed(object o)
        {
        }

        public virtual void ClipCube(System.Drawing.Rectangle rect, int minZ, int maxZ)
        {
            if (this.aspect.View3D)
            {
                Aspect aspect = this.aspect;
                if ((aspect.Elevation == 270) && ((aspect.Rotation == 270) || (aspect.Rotation == 360)))
                {
                    Point point = this.Calc3DPoint(rect.X, rect.Y, minZ);
                    Point point2 = this.Calc3DPoint(rect.Right, rect.Y, maxZ);
                    this.ClipRectangle(point.X, point.Y, point2.X, point2.Y);
                }
                else if (aspect.Orthogonal)
                {
                    if (aspect.OrthoAngle > 90)
                    {
                        this.ClipToLeft(rect, minZ, maxZ);
                    }
                    else
                    {
                        this.ClipToRight(rect, minZ, maxZ);
                    }
                }
                else if (aspect.Rotation >= 270)
                {
                    this.ClipToRight(rect, minZ, maxZ);
                }
            }
            else
            {
                this.ClipRectangle(rect.X + 1, rect.Y + 1, rect.Right - 1, rect.Bottom - 1);
            }
        }

        public abstract void ClipEllipse(System.Drawing.Rectangle r);
        public abstract void ClipPolygon(params Point[] p);
        public abstract void ClipRectangle(System.Drawing.Rectangle r);
        public void ClipRectangle(int left, int top, int right, int bottom)
        {
            this.ClipRectangle(System.Drawing.Rectangle.FromLTRB(left, top, right, bottom));
        }

        private void ClipToLeft(System.Drawing.Rectangle rect, int minZ, int maxZ)
        {
            Point[] p = new Point[] { this.Calc3DPoint(rect.X, rect.Bottom, minZ), this.Calc3DPoint(rect.X, rect.Bottom, maxZ), this.Calc3DPoint(rect.X, rect.Y, maxZ), this.Calc3DPoint(rect.Right, rect.Y, maxZ), this.Calc3DPoint(rect.Right, rect.Y, minZ), this.Calc3DPoint(rect.Right, rect.Bottom, minZ) };
            this.ClipPolygon(p);
        }

        private void ClipToRight(System.Drawing.Rectangle rect, int minZ, int maxZ)
        {
            Point[] p = new Point[6];
            p[0] = this.Calc3DPoint(rect.X, rect.Bottom, minZ);
            p[1] = this.Calc3DPoint(rect.X, rect.Y, minZ);
            Point point = this.Calc3DPoint(rect.X, rect.Y, maxZ);
            Point point2 = this.Calc3DPoint(rect.Right, rect.Y, minZ);
            p[2] = (point2.Y < point.Y) ? point2 : point;
            p[3] = this.Calc3DPoint(rect.Right, rect.Y, maxZ);
            point = this.Calc3DPoint(rect.Right, rect.Bottom, maxZ);
            point2 = this.Calc3DPoint(rect.Right, rect.Y, minZ);
            p[4] = (point2.X > point.X) ? point2 : point;
            p[5] = this.Calc3DPoint(rect.Right, rect.Bottom, minZ);
            if (p[5].X < p[0].X)
            {
                p[0].X = p[5].X;
                if (point2.Y < p[0].Y)
                {
                    p[0].Y = point2.Y;
                }
            }
            this.ClipPolygon(p);
        }

        public void Cone(bool vertical, System.Drawing.Rectangle r, int z0, int z1, bool darkSides)
        {
            this.InternalCylinder(vertical, r, z0, z1, darkSides, 0);
        }

        public void Cone(bool vertical, System.Drawing.Rectangle r, int z0, int z1, bool darkSides, int conePercent)
        {
            this.InternalCylinder(vertical, r, z0, z1, darkSides, conePercent);
        }

        public void Cone(bool vertical, int left, int top, int right, int bottom, int z0, int z1, bool darkSides)
        {
            this.InternalCylinder(vertical, System.Drawing.Rectangle.FromLTRB(left, top, right, bottom), z0, z1, darkSides, 0);
        }

        protected System.Drawing.Rectangle CorrectRectangle(System.Drawing.Rectangle r)
        {
            if (r.Height < 0)
            {
                r.Y = r.Bottom;
                r.Height = -r.Height;
            }
            if (r.Width < 0)
            {
                r.X = r.Right;
                r.Width = -r.Width;
            }
            return r;
        }

        public static bool CrossingLines(double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4, out double x, out double y)
        {
            double num = y2 - y1;
            double num2 = y4 - y3;
            double num3 = x2 - x1;
            double num4 = x4 - x3;
            double num5 = num / num3;
            double num6 = num2 / num4;
            double num7 = y1 - (num5 * x1);
            double num8 = y3 - (num6 * x3);
            double num9 = num6 - num5;
            if (Math.Abs(num9) > 1E-15)
            {
                x = (num7 - num8) / num9;
            }
            else
            {
                x = x2;
            }
            y = (num5 * x) + num7;
            double num10 = (x1 * y2) - (x2 * y1);
            double num11 = (x3 * y4) - (x4 * y3);
            return ((((((num * x3) - (num3 * y3)) - num10) > 0.0) ^ ((((num * x4) - (num3 * y4)) - num10) > 0.0)) && (((((num2 * x1) - (num4 * y1)) - num11) > 0.0) ^ ((((num2 * x2) - (num4 * y2)) - num11) > 0.0)));
        }

        public void Cube(System.Drawing.Rectangle r, int z0, int z1)
        {
            this.Cube(r.X, r.Y, r.Right, r.Bottom, z0, z1, true);
        }

        public void Cube(System.Drawing.Rectangle r, int z0, int z1, bool darkSides)
        {
            this.Cube(r.X, r.Y, r.Right, r.Bottom, z0, z1, darkSides);
        }

        public void Cube(int left, int top, int right, int bottom, int z0, int z1, bool darkSides)
        {
            Color c = this.Brush.Color;
            Point point = this.Calc3DPoint(left, top, z0);
            Point point2 = this.Calc3DPoint(right, top, z0);
            Point point3 = this.Calc3DPoint(right, bottom, z0);
            Point point4 = this.Calc3DPoint(right, top, z1);
            this.IPoints[0] = point;
            this.IPoints[1] = point2;
            this.IPoints[2] = point3;
            this.IPoints[3] = this.Calc3DPoint(left, bottom, z0);
            if (this.Culling() > 0.0)
            {
                this.PolygonFour();
            }
            else
            {
                this.Calc3DPos(ref this.IPoints[0], left, top, z1);
                this.Calc3DPos(ref this.IPoints[1], right, top, z1);
                this.Calc3DPos(ref this.IPoints[2], right, bottom, z1);
                this.Calc3DPos(ref this.IPoints[3], left, bottom, z1);
                this.PolygonFour();
            }
            this.Calc3DPos(ref this.IPoints[2], right, bottom, z1);
            this.IPoints[0] = point2;
            this.IPoints[1] = point4;
            this.IPoints[3] = point3;
            if (this.Culling() > 0.0)
            {
                if (darkSides)
                {
                    this.InternalApplyDark(c, 0x80);
                }
                this.PolygonFour();
            }
            this.IPoints[0] = point;
            this.Calc3DPos(ref this.IPoints[1], left, top, z1);
            this.Calc3DPos(ref this.IPoints[2], left, bottom, z1);
            this.Calc3DPos(ref this.IPoints[3], left, bottom, z0);
            double num = ((this.IPoints[3].X - this.IPoints[0].X) * (this.IPoints[1].Y - this.IPoints[0].Y)) - ((this.IPoints[1].X - this.IPoints[0].X) * (this.IPoints[3].Y - this.IPoints[0].Y));
            if (num > 0.0)
            {
                if (darkSides)
                {
                    this.InternalApplyDark(c, 0x80);
                }
                this.PolygonFour();
            }
            this.Calc3DPos(ref this.IPoints[3], left, top, z1);
            num = ((point.X - point2.X) * (point4.Y - point2.Y)) - ((point4.X - point2.X) * (point.Y - point2.Y));
            if (num > 0.0)
            {
                this.IPoints[0] = point;
                this.IPoints[1] = point2;
                this.IPoints[2] = point4;
                if (darkSides)
                {
                    this.InternalApplyDark(c, 0x40);
                }
                this.PolygonFour();
            }
            this.Calc3DPos(ref this.IPoints[0], left, bottom, z0);
            this.Calc3DPos(ref this.IPoints[2], right, bottom, z1);
            this.Calc3DPos(ref this.IPoints[1], left, bottom, z1);
            this.IPoints[3] = point3;
            if (this.Culling() < 0.0)
            {
                if (darkSides)
                {
                    this.InternalApplyDark(c, 0x40);
                }
                this.PolygonFour();
            }
            this.Brush.Color = c;
        }

        internal static bool Cull(Point[] p)
        {
            return Cull(p[0], p[1], p[2]);
        }

        internal static bool Cull(Point p0, Point p1, Point p2)
        {
            return ((((p0.X - p1.X) * (p2.Y - p1.Y)) - ((p2.X - p1.X) * (p0.Y - p1.Y))) < 0);
        }

        private double Culling()
        {
            return (double) (((this.IPoints[3].X - this.IPoints[2].X) * (this.IPoints[1].Y - this.IPoints[2].Y)) - ((this.IPoints[1].X - this.IPoints[2].X) * (this.IPoints[3].Y - this.IPoints[2].Y)));
        }

        public void Cylinder(bool vertical, System.Drawing.Rectangle r, int z0, int z1, bool darkSides)
        {
            this.InternalCylinder(vertical, r, z0, z1, darkSides, 100);
        }

        private void DoBevelRect(System.Drawing.Rectangle rect, ChartPen a, ChartPen b)
        {
            Point point = new Point(rect.Right - 1, rect.Y);
            Point point2 = new Point(rect.X, rect.Bottom - 1);
            Point point3 = new Point(rect.Right - 1, rect.Bottom - 1);
            this.Line(a, rect.Location, point);
            this.Line(a, rect.Location, point2);
            this.Line(b, point2, point3);
            this.Line(b, point3, point);
        }

        protected abstract void DoDrawString(int x, int y, string text, ChartBrush aBrush);
        public void Donut(int xCenter, int yCenter, int xRadius, int yRadius, double startAngle, double endAngle, double holePercent)
        {
            Point[] pointArray = new Point[0x100];
            double num = (holePercent * xRadius) * 0.01;
            double num2 = (holePercent * yRadius) * 0.01;
            int num3 = Utils.Round((double) ((128.0 * (endAngle - startAngle)) / 3.1415926535897931));
            if (num3 < 2)
            {
                num3 = 2;
            }
            else if (num3 > 0x80)
            {
                num3 = 0x80;
            }
            double num4 = (endAngle - startAngle) / ((double) (num3 - 1));
            double a = startAngle;
            for (int i = 1; i <= num3; i++)
            {
                double num6 = Math.Sin(a);
                double num7 = Math.Cos(a);
                pointArray[i].X = xCenter + Utils.Round((double) (xRadius * num7));
                pointArray[i].Y = yCenter - Utils.Round((double) (yRadius * num6));
                int index = (i == 1) ? 0 : (((2 * num3) - i) + 1);
                pointArray[index].X = xCenter + Utils.Round((double) (num * num7));
                pointArray[index].Y = yCenter - Utils.Round((double) (num2 * num6));
                a += num4;
            }
            Array source = pointArray;
            this.Polygon(SliceArray(ref source, 2 * num3));
        }

        public abstract void Draw(System.Drawing.Rectangle r, Image image, bool transparent);
        public abstract void Draw(int x, int y, Image image);
        public void Draw(System.Drawing.Rectangle r, Image image, ImageMode mode, bool transparent)
        {
            if (mode == ImageMode.Center)
            {
                r.X += (r.Width - image.Width) / 2;
                r.Y += (r.Height - image.Height) / 2;
                r.Width = image.Width;
                r.Height = image.Height;
            }
            else if (mode == ImageMode.Normal)
            {
                r.Width = image.Width;
                r.Height = image.Height;
            }
            else if (mode == ImageMode.Tile)
            {
                if ((image.Width > 0) && (image.Height > 0))
                {
                    int num = 0;
                    do
                    {
                        int num2 = 0;
                        do
                        {
                            this.Draw(new System.Drawing.Rectangle(r.X + num2, r.Y + num, image.Width, image.Height), image, transparent);
                            num2 += image.Width;
                        }
                        while (num2 < r.Width);
                        num += image.Height;
                    }
                    while (num < r.Height);
                }
                return;
            }
            this.Draw(r, image, transparent);
        }

        public abstract void DrawBeziers(params Point[] p);
        public void DrawBeziers(int z, params Point[] p)
        {
            for (int i = 0; i <= p.GetUpperBound(0); i++)
            {
                p[i] = this.Calc3DPoint(p[i], z);
            }
            this.DrawBeziers(p);
        }

        public void Ellipse(System.Drawing.Rectangle r)
        {
            this.Ellipse(r.X, r.Y, r.Right, r.Bottom);
        }

        public void Ellipse(System.Drawing.Rectangle r, int z)
        {
            this.Ellipse(r.X, r.Y, r.Right, r.Bottom, z);
        }

        public void Ellipse(System.Drawing.Rectangle r, int z, double angle)
        {
            this.Ellipse(r.Left, r.Top, r.Right, r.Bottom, z, angle);
        }

        public abstract void Ellipse(int x1, int y1, int x2, int y2);
        public void Ellipse(int x1, int y1, int x2, int y2, int z)
        {
            this.Calc3DPos(ref x1, ref y1, z);
            this.Calc3DPos(ref x2, ref y2, z);
            this.Ellipse(x1, y1, x2, y2);
        }

        public void Ellipse(int left, int top, int right, int bottom, int z, double angle)
        {
            Point[] p = new Point[0x40];
            Point[] pointArray2 = new Point[3];
            double a = (right + left) * 0.5;
            double num6 = (bottom + top) * 0.5;
            double num7 = (right - left) * 0.5;
            double num8 = (bottom - top) * 0.5;
            angle *= 0.017453292519943295;
            double num9 = 0.099733100113961692;
            double num10 = Math.Sin(angle);
            double num11 = Math.Cos(angle);
            for (int i = 0; i < 0x40; i++)
            {
                double num3 = Math.Sin(i * num9);
                double num4 = Math.Cos(i * num9);
                double num = num7 * num3;
                double num2 = num8 * num4;
                p[i].X = (int) Math.Round((double) (a + ((num * num11) + (num2 * num10))));
                p[i].Y = (int) Math.Round((double) (num6 + ((-num * num10) + (num2 * num11))));
            }
            if (this.Brush.Visible)
            {
                bool visible = this.Pen.Visible;
                this.Pen.Visible = false;
                int num13 = (int) Math.Round(a);
                int num14 = (int) Math.Round(num6);
                for (int j = 1; j < 0x40; j++)
                {
                    pointArray2[0].X = num13;
                    pointArray2[0].Y = num14;
                    pointArray2[1] = p[j - 1];
                    pointArray2[2] = p[j];
                    this.Polygon(z, pointArray2);
                }
                pointArray2[0].X = num13;
                pointArray2[0].Y = num14;
                pointArray2[1] = p[0x3f];
                pointArray2[2] = p[0];
                this.Polygon(z, pointArray2);
                this.Pen.Visible = visible;
            }
            if (this.Pen.Visible)
            {
                this.Polyline(z, p);
            }
        }

        public void EllipseEnh(int x1, int y1, int x2, int y2)
        {
            Color baseColor = this.Brush.Color;
            this.Brush.Color = this.SpecialColor(baseColor);
            int transparency = this.Brush.Transparency;
            System.Drawing.Rectangle r = new System.Drawing.Rectangle(x1, y1, x2 - x1, y2 - y1);
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(r.Left + ((int) (r.Width * 0.08)), r.Top + ((int) (r.Height * 0.03)), (int) (r.Width * 0.84), (int) (r.Height * 0.94));
            this.Pen.Visible = false;
            System.Drawing.Rectangle chartRect = base.chart.ChartRect;
            Region region = this.AddRightRegion(chartRect, 0, base.chart.Aspect.Width3D);
            this.g.Clip = region;
            this.Ellipse(r);
            if (((x2 - x1) < 5) || ((y2 - y1) < 5))
            {
                this.UnClip();
            }
            else
            {
                GraphicsPath path = new GraphicsPath();
                path.AddEllipse(r);
                Region region2 = new Region(path);
                region2.Intersect(region);
                this.g.Clip = region2;
                GraphicsPath path2 = new GraphicsPath();
                System.Drawing.Rectangle rectangle4 = new System.Drawing.Rectangle(r.X + ((int) ((r.Width / 10) * 0.5)), r.Y + ((r.Height / 10) * 4), (r.Width / 10) * 9, (r.Height / 10) * 7);
                if ((rectangle4.Width < 5) || (rectangle4.Height < 5))
                {
                    this.UnClip();
                }
                else
                {
                    path2.AddEllipse(rectangle4);
                    PathGradientBrush brush = new PathGradientBrush(path2);
                    Color c = baseColor;
                    ApplyBright(ref c, 0x80);
                    brush.CenterColor = c;
                    Color[] colorArray = new Color[] { Color.Transparent };
                    brush.SurroundColors = colorArray;
                    this.FillRectangle(brush, r.X, r.Y, r.Width, r.Height);
                    this.UnClip();
                    this.Brush.Transparency = 0;
                    this.Brush.Gradient.Visible = true;
                    this.Brush.Gradient.StartColor = Color.White;
                    this.Brush.Gradient.MiddleColor = Color.Transparent;
                    this.Brush.Gradient.EndColor = Color.Transparent;
                    path = new GraphicsPath();
                    path.AddEllipse(rect);
                    Region region3 = new Region(new System.Drawing.Rectangle(rect.X, rect.Y, rect.Width, (int) (rect.Height * 0.7)));
                    Region region4 = new Region(path);
                    region4.Intersect(region3);
                    region4.Intersect(region);
                    this.g.Clip = region4;
                    this.Rectangle(rect.X, rect.Y, rect.Right, rect.Y + ((int) (rect.Height * 0.9)));
                    this.UnClip();
                }
            }
        }

        public void EllipseEnh(int x1, int y1, int x2, int y2, int z)
        {
            this.Calc3DPos(ref x1, ref y1, z);
            this.Calc3DPos(ref x2, ref y2, z);
            this.EllipseEnh(x1, y1, x2, y2);
        }

        public abstract void EraseBackground(int left, int top, int right, int bottom);
        public void FillRectangle(System.Drawing.Brush brush, int x, int y, int width, int height)
        {
            this.g.FillRectangle(brush, x, y, width, height);
        }

        public void FillRegion(System.Drawing.Brush brush, Region region)
        {
            this.g.FillRegion(brush, region);
        }

        public int FontTextHeight(ChartFont f)
        {
            return (int) this.TextHeight(f, "W");
        }

        public Point[] FourPointsFromRect(System.Drawing.Rectangle r, int z)
        {
            return new Point[] { this.Calc3DPoint(r.Location, z), this.Calc3DPoint(r.Right, r.Top, z), this.Calc3DPoint(r.Right, r.Bottom, z), this.Calc3DPoint(r.Left, r.Bottom, z) };
        }

        private GraphicsPath GetCapsule(RectangleF baseRect)
        {
            GraphicsPath path = new GraphicsPath();
            try
            {
                float height;
                RectangleF ef;
                if (baseRect.Width > baseRect.Height)
                {
                    height = baseRect.Height;
                    SizeF size = new SizeF(height, height);
                    ef = new RectangleF(baseRect.Location, size);
                    path.AddArc(ef, 90f, 180f);
                    ef.X = baseRect.Right - height;
                    path.AddArc(ef, 270f, 180f);
                    return path;
                }
                if (baseRect.Width < baseRect.Height)
                {
                    height = baseRect.Width;
                    SizeF ef3 = new SizeF(height, height);
                    ef = new RectangleF(baseRect.Location, ef3);
                    path.AddArc(ef, 180f, 180f);
                    ef.Y = baseRect.Bottom - height;
                    path.AddArc(ef, 0f, 180f);
                    return path;
                }
                path.AddEllipse(baseRect);
            }
            catch
            {
                path.AddEllipse(baseRect);
            }
            finally
            {
                path.CloseFigure();
            }
            return path;
        }

        public virtual Region GetChartPolygon(System.Drawing.Rectangle rect, int minZ, int maxZ)
        {
            if (this.aspect.View3D)
            {
                Point point = this.Calc3DPoint(rect.X, rect.Y, minZ);
                Point point2 = this.Calc3DPoint(rect.Right, rect.Y, maxZ);
                return new Region(new System.Drawing.Rectangle(point.X, point.Y, point2.X - point.X, point2.Y - point.Y));
            }
            return new Region(new System.Drawing.Rectangle(rect.X + 1, rect.Y + 1, ((rect.Right - 1) - rect.X) + 1, ((rect.Bottom - 1) - rect.Y) + 1));
        }

        internal GraphicsPath GetClipRoundRectangle(System.Drawing.Rectangle rect, int RoundRadius)
        {
            float left = Convert.ToSingle(rect.Left);
            float top = Convert.ToSingle(rect.Top);
            float right = Convert.ToSingle(rect.Right);
            float bottom = Convert.ToSingle(rect.Bottom);
            RectangleF ef = RectangleF.FromLTRB(left, top, right, bottom);
            float num5 = Convert.ToSingle(RoundRadius);
            if (num5 <= 0f)
            {
                GraphicsPath path = new GraphicsPath();
                path.AddRectangle(ef);
                path.CloseFigure();
                return path;
            }
            if (num5 >= (((double) Math.Min(ef.Width, ef.Height)) / 2.0))
            {
                return this.GetCapsule(ef);
            }
            float width = num5 * 2f;
            SizeF size = new SizeF(width, width);
            RectangleF ef3 = new RectangleF(ef.Location, size);
            GraphicsPath path2 = new GraphicsPath();
            path2.AddArc(ef3, 180f, 90f);
            ef3.X = ef.Right - width;
            path2.AddArc(ef3, 270f, 90f);
            ef3.Y = ef.Bottom - width;
            path2.AddArc(ef3, 0f, 90f);
            ef3.X = ef.Left;
            path2.AddArc(ef3, 90f, 90f);
            path2.CloseFigure();
            return path2;
        }

        public static Color GetDefaultColor(int index)
        {
            return ColorPalette[index % 0x13];
        }

        public static HatchStyle GetDefaultPattern(int index)
        {
            HatchStyle[] styleArray2 = new HatchStyle[0x12];
            styleArray2[1] = HatchStyle.Vertical;
            styleArray2[2] = HatchStyle.ForwardDiagonal;
            styleArray2[3] = HatchStyle.BackwardDiagonal;
            styleArray2[4] = HatchStyle.Cross;
            styleArray2[5] = HatchStyle.DiagonalCross;
            styleArray2[6] = HatchStyle.DiagonalBrick;
            styleArray2[7] = HatchStyle.Divot;
            styleArray2[8] = HatchStyle.LargeConfetti;
            styleArray2[9] = HatchStyle.OutlinedDiamond;
            styleArray2[10] = HatchStyle.Plaid;
            styleArray2[11] = HatchStyle.Shingle;
            styleArray2[12] = HatchStyle.SolidDiamond;
            styleArray2[13] = HatchStyle.Sphere;
            styleArray2[14] = HatchStyle.Trellis;
            styleArray2[15] = HatchStyle.Wave;
            styleArray2[0x10] = HatchStyle.Weave;
            styleArray2[0x11] = HatchStyle.ZigZag;
            HatchStyle[] styleArray = styleArray2;
            return styleArray[index % styleArray.Length];
        }

        private bool GetSupports3DText()
        {
            return false;
        }

        public abstract void GradientFill(int left, int top, int right, int bottom, Color startColor, Color endColor, LinearGradientMode direction);
        public abstract void HorizontalLine(int left, int right, int y);
        public void HorizontalLine(int left, int right, int y, int z)
        {
            int num = y;
            this.Calc3DPos(ref left, ref num, z);
            this.Calc3DPos(ref right, ref y, z);
            this.Line(left, num, right, y);
        }

        protected internal virtual void InitWindow(Graphics graphics, Aspect a, System.Drawing.Rectangle r, int MaxDepth)
        {
            this.bounds = r;
            this.aspect = a;
            this.IZoomFactor = 1.0;
            if (this.aspect.view3D)
            {
                if (this.aspect.orthogonal)
                {
                    double orthoAngle = this.aspect.OrthoAngle;
                    if (orthoAngle > 90.0)
                    {
                        this.IOrthoX = -1.0;
                        orthoAngle = 180.0 - orthoAngle;
                    }
                    else
                    {
                        this.IOrthoX = 1.0;
                    }
                    double num2 = Math.Sin(this.aspect.OrthoAngle * 0.017453292519943295);
                    double num3 = Math.Cos(this.aspect.OrthoAngle * 0.017453292519943295);
                    this.IOrthoY = (num3 < 0.01) ? 1.0 : (num2 / num3);
                }
                this.IZoomFactor = 0.01 * this.aspect.Zoom;
                this.IZoomText = this.aspect.ZoomText;
            }
            this.CalcTrigValues();
        }

        private void InternalApplyDark(Color c, byte quantity)
        {
            this.bBrush.ApplyDark(c, quantity);
        }

        private void InternalCylinder(bool vertical, System.Drawing.Rectangle r, int z0, int z1, bool dark3D, int conePercent)
        {
            Color backColor;
            int num;
            int num2;
            int num4;
            int num6;
            int num7;
            double num8;
            double num9;
            Point[] pointArray4;
            Point3D[] pointdArray = new Point3D[0x10];
            Point[] p = new Point[0x10];
            if (this.Brush.Solid)
            {
                backColor = this.Brush.Color;
            }
            else
            {
                backColor = this.BackColor;
            }
            int num5 = (z1 - z0) / 2;
            int num3 = (z1 + z0) / 2;
            if (vertical)
            {
                num4 = (r.Right - r.X) / 2;
                num2 = (r.Right + r.X) / 2;
                num = Math.Abs((int) (r.Bottom - r.Y));
                for (num6 = 0; num6 < 0x10; num6++)
                {
                    Utils.SinCos((num6 - 3) * 0.39269908169872414, out num8, out num9);
                    pointdArray[num6].X = num2 + Utils.Round((double) (num8 * num4));
                    if (r.Y < r.Bottom)
                    {
                        pointdArray[num6].Y = r.Y;
                    }
                    else
                    {
                        pointdArray[num6].Y = r.Bottom;
                    }
                    pointdArray[num6].Z = num3 - Utils.Round((double) (num9 * num5));
                }
                num4 = Utils.Round((double) ((num4 * conePercent) * 0.01));
                num5 = Utils.Round((double) ((num5 * conePercent) * 0.01));
                p[1] = this.Calc3DPoint(pointdArray[0].X, pointdArray[0].Y + num, pointdArray[0].Z);
                Utils.SinCos(-1.1780972450961724, out num8, out num9);
                pointdArray[0].X = num2 + Utils.Round((double) (num8 * num4));
                pointdArray[0].Z = num3 - Utils.Round((double) (num9 * num5));
                p[0] = this.Calc3DPoint(pointdArray[0]);
                num7 = 0;
                for (num6 = 1; num6 < 0x10; num6++)
                {
                    p[2] = this.Calc3DPoint(pointdArray[num6].X, pointdArray[num6].Y + num, pointdArray[num6].Z);
                    Utils.SinCos((num6 - 3) * 0.39269908169872414, out num8, out num9);
                    pointdArray[num6].X = num2 + Utils.Round((double) (num8 * num4));
                    pointdArray[num6].Z = num3 - Utils.Round((double) (num9 * num5));
                    p[3] = this.Calc3DPoint(pointdArray[num6]);
                    if ((((p[0].X - p[2].X) + p[1].X) - p[3].X) < 0)
                    {
                        if (dark3D)
                        {
                            this.InternalApplyDark(backColor, Convert.ToByte((int) (0x10 * num7)));
                        }
                        pointArray4 = new Point[] { p[0], p[1], p[2], p[3] };
                        Point[] pointArray2 = pointArray4;
                        this.Polygon(pointArray2);
                    }
                    p[0] = p[3];
                    p[1] = p[2];
                    num7++;
                }
            }
            else
            {
                num4 = (r.Bottom - r.Y) / 2;
                num2 = (r.Bottom + r.Y) / 2;
                for (num6 = 0; num6 < 0x10; num6++)
                {
                    Utils.SinCos((num6 - 4) * 0.39269908169872414, out num8, out num9);
                    if (r.X < r.Right)
                    {
                        pointdArray[num6].X = r.Right;
                    }
                    else
                    {
                        pointdArray[num6].X = r.X;
                    }
                    pointdArray[num6].Y = num2 + Utils.Round((double) (num8 * num4));
                    pointdArray[num6].Z = num3 - Utils.Round((double) (num9 * num5));
                }
                num4 = Utils.Round((double) ((num4 * conePercent) * 0.01));
                num5 = Utils.Round((double) ((num5 * conePercent) * 0.01));
                num = Math.Abs((int) (r.Right - r.X));
                p[1] = this.Calc3DPoint(pointdArray[0].X - num, pointdArray[0].Y, pointdArray[0].Z);
                Utils.SinCos(-1.5707963267948966, out num8, out num9);
                pointdArray[0].Y = num2 + Utils.Round((double) (num8 * num4));
                pointdArray[0].Z = num3 - Utils.Round((double) (num9 * num5));
                p[0] = this.Calc3DPoint(pointdArray[0]);
                num7 = 0;
                for (num6 = 1; num6 < 0x10; num6++)
                {
                    p[2] = this.Calc3DPoint(pointdArray[num6].X - num, pointdArray[num6].Y, pointdArray[num6].Z);
                    Utils.SinCos((num6 - 4) * 0.39269908169872414, out num8, out num9);
                    pointdArray[num6].Y = num2 + Utils.Round((double) (num8 * num4));
                    pointdArray[num6].Z = num3 - Utils.Round((double) (num9 * num5));
                    p[3] = this.Calc3DPoint(pointdArray[num6]);
                    if ((((p[0].Y - p[2].Y) + p[1].Y) - p[3].Y) < 0)
                    {
                        if (dark3D)
                        {
                            this.InternalApplyDark(backColor, Convert.ToByte((int) (0x10 * num7)));
                        }
                        pointArray4 = new Point[] { p[0], p[1], p[2], p[3] };
                        Point[] pointArray3 = pointArray4;
                        this.Polygon(pointArray3);
                    }
                    p[0] = p[3];
                    p[1] = p[2];
                    num7++;
                }
            }
            for (num6 = 0; num6 < 0x10; num6++)
            {
                p[num6] = this.Calc3DPoint(pointdArray[num6]);
            }
            if (dark3D)
            {
                this.InternalApplyDark(backColor, 0x40);
            }
            this.Polygon(p);
        }

        public void Line(Point p0, Point p1)
        {
            this.Line(p0.X, p0.Y, p1.X, p1.Y);
        }

        protected abstract void Line(ChartPen p, Point a, Point b);
        public void Line(Point p0, Point p1, int z)
        {
            this.Line(p0.X, p0.Y, p1.X, p1.Y, z);
        }

        public abstract void Line(int x0, int y0, int x1, int y1);
        public void Line(int x0, int y0, int x1, int y1, int z)
        {
            this.Calc3DPos(ref x0, ref y0, z);
            this.Calc3DPos(ref x1, ref y1, z);
            this.Line(x0, y0, x1, y1);
        }

        public void Line(int x0, int y0, int z0, int x1, int y1, int z1)
        {
            int x = x0;
            int y = y0;
            this.Calc3DPos(ref x, ref y, z0);
            this.Calc3DPos(ref x1, ref y1, z1);
            this.Line(x, y, x1, y1);
        }

        public void LineTo(Point3D p)
        {
            this.Calc3DPos(ref p.X, ref p.Y, p.Z);
            this.LineTo(p.X, p.Y);
        }

        public void LineTo(Point p, int z)
        {
            this.LineTo(p.X, p.Y, z);
        }

        public abstract void LineTo(int x, int y);
        public void LineTo(int x, int y, int z)
        {
            this.Calc3DPos(ref x, ref y, z);
            this.LineTo(x, y);
        }

        [Obsolete("Use Line method."), EditorBrowsable(EditorBrowsableState.Never)]
        public void LineWithZ(int x, int y, int x1, int y1, int z)
        {
            this.Line(x, y, x1, y1, z);
        }

        public abstract SizeF MeasureString(ChartFont f, string text);
        public void MoveTo(Point3D p)
        {
            this.Calc3DPos(ref p.X, ref p.Y, p.Z);
            this.MoveTo(p.X, p.Y);
        }

        public void MoveTo(Point p)
        {
            this.MoveTo(p.X, p.Y);
        }

        public void MoveTo(Point p, int z)
        {
            int x = p.X;
            int y = p.Y;
            this.Calc3DPos(ref x, ref y, z);
            this.MoveTo(x, y);
        }

        public abstract void MoveTo(int x, int y);
        public void MoveTo(int x, int y, int z)
        {
            this.Calc3DPos(ref x, ref y, z);
            this.MoveTo(x, y);
        }

        public void PaintBevel(BevelStyles bevel, System.Drawing.Rectangle rect, int width, Color one, Color two)
        {
            ChartPen pen;
            ChartPen pen2;
            if (bevel == BevelStyles.Raised)
            {
                pen = new ChartPen(one);
                pen2 = new ChartPen(two);
            }
            else
            {
                pen = new ChartPen(two);
                pen2 = new ChartPen(one);
            }
            int num = width;
            while (num > 0)
            {
                num--;
                this.DoBevelRect(rect, pen, pen2);
                rect.Inflate(-1, -1);
            }
            pen.Dispose();
            pen2.Dispose();
        }

        public abstract void Pie(int x1, int y1, int x2, int y2, double startAngle, double endAngle);
        public void Pie(int x1, int y1, int x2, int y2, int x3, int y3, int x4, int y4)
        {
            int num = x2 - x1;
            int num2 = y2 - y1;
            int num3 = x1 + (num / 2);
            int num4 = y1 + (num2 / 2);
            double startAngle = Math.Atan2((double) (num4 - y3), (double) (x3 - num3));
            if (startAngle < 0.0)
            {
                startAngle = 6.2831853071795862 + startAngle;
            }
            startAngle *= 57.295779513082323;
            double num6 = Math.Atan2((double) (num4 - y4), (double) (x4 - num3));
            if (num6 < 0.0)
            {
                num6 = 6.2831853071795862 + num6;
            }
            num6 *= 57.295779513082323;
            this.Pie(x1, y1, x2, y2, startAngle, num6 - startAngle);
        }

        public void Pie(int xCenter, int yCenter, int xRadius, int yRadius, int z0, int z1, double startAngle, double endAngle, bool darkSides, bool drawSides)
        {
            this.Pie(xCenter, yCenter, xRadius, yRadius, z0, z1, startAngle, endAngle, darkSides, drawSides, 0);
        }

        public void Pie(int xCenter, int yCenter, int xRadius, int yRadius, int z0, int z1, double startAngle, double endAngle, bool darkSides, bool drawSides, int donutPercent)
        {
            if (this.pie3D == null)
            {
                this.pie3D = new Pie3D(this);
            }
            this.pie3D.Pie(xCenter, yCenter, xRadius, yRadius, z0, z1, startAngle, endAngle, darkSides, drawSides, donutPercent);
        }

        public abstract void Pixel(int x, int y, int z, Color color);
        public void Plane(int z0, int z1, params Point[] p)
        {
            this.Calc3DPos(ref this.IPoints[0], p[0].X, p[0].Y, z0);
            this.Calc3DPos(ref this.IPoints[1], p[1].X, p[1].Y, z0);
            this.Calc3DPos(ref this.IPoints[2], p[2].X, p[2].Y, z1);
            this.Calc3DPos(ref this.IPoints[3], p[3].X, p[3].Y, z1);
            this.PolygonFour();
        }

        public void Plane(Point p1, Point p2, int z0, int z1)
        {
            this.Calc3DPos(ref this.IPoints[0], p1.X, p1.Y, z0);
            this.Calc3DPos(ref this.IPoints[1], p2.X, p2.Y, z0);
            this.Calc3DPos(ref this.IPoints[2], p2.X, p2.Y, z1);
            this.Calc3DPos(ref this.IPoints[3], p1.X, p1.Y, z1);
            this.PolygonFour();
        }

        public void Plane(Point p1, Point p2, Point p3, Point p4, int z)
        {
            this.Calc3DPos(ref this.IPoints[0], p1.X, p1.Y, z);
            this.Calc3DPos(ref this.IPoints[1], p2.X, p2.Y, z);
            this.Calc3DPos(ref this.IPoints[2], p3.X, p3.Y, z);
            this.Calc3DPos(ref this.IPoints[3], p4.X, p4.Y, z);
            this.PolygonFour();
        }

        public void PlaneFour3D(int z0, int z1, params Point[] p)
        {
            this.IPoints = p;
            this.IPoints[0] = this.Calc3DPoint(this.IPoints[0].X, this.IPoints[0].Y, z0);
            this.IPoints[1] = this.Calc3DPoint(this.IPoints[1].X, this.IPoints[1].Y, z0);
            this.IPoints[2] = this.Calc3DPoint(this.IPoints[2].X, this.IPoints[2].Y, z1);
            this.IPoints[3] = this.Calc3DPoint(this.IPoints[3].X, this.IPoints[3].Y, z1);
            this.PolygonFour();
        }

        public static bool PointInEllipse(Point p, System.Drawing.Rectangle rect)
        {
            int num;
            int num2;
            RectCenter(rect, out num, out num2);
            int num3 = (int) Utils.Sqr((double) (num - rect.X));
            int num4 = (int) Utils.Sqr((double) (num2 - rect.Y));
            return (((num3 != 0) && (num4 != 0)) && (((Utils.Sqr((double) (p.X - num)) / ((double) num3)) + (Utils.Sqr((double) (p.Y - num2)) / ((double) num4))) <= 1.0));
        }

        public static bool PointInEllipse(Point p, int left, int top, int right, int bottom)
        {
            return PointInEllipse(p, System.Drawing.Rectangle.FromLTRB(left, top, right, bottom));
        }

        public static bool PointInHorizTriangle(Point p, int y0, int y1, int x0, int x1)
        {
            Point[] poly = new Point[] { new Point(x0, y0), new Point(x1, (y0 + y1) / 2), new Point(x0, y1) };
            return PointInPolygon(p, poly);
        }

        public static bool PointInLineTolerance(Point p, int px, int py, int qx, int qy, int tolerance)
        {
            double num;
            double num2;
            Point pointA = new Point(px, py);
            Point pointB = new Point(qx, qy);
            switch (CalcLineParameters(pointA, pointB, out num2, out num))
            {
                case LineOrientations.Point:
                    tolerance /= 2;
                    return System.Drawing.Rectangle.FromLTRB(pointA.X - tolerance, pointA.Y - tolerance, pointB.X + tolerance, pointB.Y + tolerance).Contains(p.X, p.Y);

                case LineOrientations.Horizontal:
                {
                    if ((p.X < Math.Min(pointA.X, pointB.X)) || (p.X > Math.Max(pointA.X, pointB.X)))
                    {
                        break;
                    }
                    int num3 = (int) Math.Round((double) ((num2 * p.X) + num));
                    return (Math.Abs((int) (num3 - p.Y)) <= tolerance);
                }
                case LineOrientations.Vertical:
                {
                    if ((p.Y < Math.Min(pointA.Y, pointB.Y)) || (p.Y > Math.Max(pointA.Y, pointB.Y)))
                    {
                        break;
                    }
                    int num4 = (int) Math.Round((double) ((num2 * p.Y) + num));
                    return (Math.Abs((int) (num4 - p.X)) <= tolerance);
                }
            }
            return false;
        }

        public static bool PointInPolygon(Point p, params Point[] poly)
        {
            bool flag = false;
            int upperBound = poly.GetUpperBound(0);
            int num2 = upperBound;
            for (int i = 0; i <= num2; i++)
            {
                if ((((poly[i].Y <= p.Y) && (p.Y < poly[upperBound].Y)) || ((poly[upperBound].Y <= p.Y) && (p.Y < poly[i].Y))) && (p.X < ((((poly[upperBound].X - poly[i].X) * (p.Y - poly[i].Y)) / (poly[upperBound].Y - poly[i].Y)) + poly[i].X)))
                {
                    flag = !flag;
                }
                upperBound = i;
            }
            return flag;
        }

        [EditorBrowsable(EditorBrowsableState.Never), Obsolete("Please use Rectangle.Contains method.")]
        public static bool PointInRect(System.Drawing.Rectangle rect, int x, int y)
        {
            return rect.Contains(x, y);
        }

        public static bool PointInTriangle(Point p, int x0, int x1, int y0, int y1)
        {
            Point[] poly = new Point[] { new Point(x0, y0), new Point((x0 + x1) / 2, y1), new Point(x1, y0) };
            return PointInPolygon(p, poly);
        }

        public abstract void Polygon(params Point[] p);
        public abstract void Polygon(System.Drawing.Brush b, params Point[] p);
        public void Polygon(int z, params Point[] p)
        {
            for (int i = 0; i <= p.GetUpperBound(0); i++)
            {
                p[i] = this.Calc3DPoint(p[i], z);
            }
            this.Polygon(p);
        }

        private void PolygonFour()
        {
            this.Polygon(this.IPoints);
        }

        public System.Drawing.Rectangle PolygonRect(params Point[] p)
        {
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle();
            if (p.GetUpperBound(0) > 1)
            {
                rectangle.X = p[0].X;
                rectangle.Width = 0;
                rectangle.Y = p[0].Y;
                rectangle.Height = 0;
                for (int i = 1; i <= p.GetUpperBound(0); i++)
                {
                    if (p[i].X < rectangle.X)
                    {
                        rectangle.Width = rectangle.Right - p[i].X;
                        rectangle.X = p[i].X;
                    }
                    if (p[i].X > rectangle.Right)
                    {
                        rectangle.Width += p[i].X - rectangle.Right;
                    }
                    if (p[i].Y < rectangle.Y)
                    {
                        rectangle.Height = rectangle.Bottom - p[i].Y;
                        rectangle.Y = p[i].Y;
                    }
                    if (p[i].Y > rectangle.Bottom)
                    {
                        rectangle.Height += p[i].Y - rectangle.Bottom;
                    }
                }
            }
            return rectangle;
        }

        public abstract void Polyline(int z, params Point[] p);
        public abstract void PrepareDrawImage();
        public void Projection(int maxDepth, System.Drawing.Rectangle r)
        {
            this.xCenter = (r.X + ((int) (r.Width * 0.5))) + this.RotationCenter.X;
            this.yCenter = (r.Y + ((int) (r.Height * 0.5))) + this.RotationCenter.Y;
            this.zCenter = ((int) (maxDepth * 0.5)) + this.RotationCenter.Z;
            this.xCenterOffset = this.xCenter + this.aspect.HorizOffset;
            this.yCenterOffset = this.yCenter + this.aspect.VertOffset;
            this.CalcPerspective(r);
        }

        public void Pyramid(bool vertical, System.Drawing.Rectangle r, int z0, int z1, bool darkSides)
        {
            this.Pyramid(vertical, r.X, r.Y, r.Right, r.Bottom, z0, z1, darkSides);
        }

        public void Pyramid(bool vertical, int left, int top, int right, int bottom, int z0, int z1, bool darkSides)
        {
            Point point;
            Point point2;
            Point point3;
            Point point5;
            Point[] pointArray;
            Color c = this.Brush.Solid ? this.Brush.Color : this.BackColor;
            if (vertical)
            {
                if (top != bottom)
                {
                    point = this.Calc3DPoint(left, bottom, z0);
                    point2 = this.Calc3DPoint(right, bottom, z0);
                    point5 = this.Calc3DPoint((left + right) / 2, top, (z0 + z1) / 2);
                    pointArray = new Point[] { point, point5, point2 };
                    this.Polygon(pointArray);
                    point3 = this.Calc3DPoint(left, bottom, z1);
                    if ((top < bottom) && (point3.Y < point5.Y))
                    {
                        pointArray = new Point[] { point, point5, point3 };
                        this.Polygon(pointArray);
                    }
                    if (darkSides)
                    {
                        this.InternalApplyDark(c, 0x80);
                    }
                    Point point4 = this.Calc3DPoint(right, bottom, z1);
                    pointArray = new Point[] { point2, point5, point4 };
                    this.Polygon(pointArray);
                    if ((top < bottom) && (point3.Y < point5.Y))
                    {
                        pointArray = new Point[] { point5, point3, point4 };
                        this.Polygon(pointArray);
                    }
                }
                if (top >= bottom)
                {
                    if (darkSides)
                    {
                        this.InternalApplyDark(c, 0x40);
                    }
                    this.RectangleY(left, bottom, right, z0, z1);
                }
            }
            else
            {
                if (left != right)
                {
                    point = this.Calc3DPoint(left, top, z0);
                    point2 = this.Calc3DPoint(left, bottom, z0);
                    point5 = this.Calc3DPoint(right, (top + bottom) / 2, (z0 + z1) / 2);
                    pointArray = new Point[] { point, point5, point2 };
                    this.Polygon(pointArray);
                    if (darkSides)
                    {
                        this.InternalApplyDark(c, 0x40);
                    }
                    point3 = this.Calc3DPoint(left, top, z1);
                    pointArray = new Point[] { point, point5, point3 };
                    this.Polygon(pointArray);
                }
                if (left >= right)
                {
                    if (darkSides)
                    {
                        this.InternalApplyDark(c, 0x80);
                    }
                    this.RectangleZ(left, top, bottom, z0, z1);
                }
            }
        }

        public void PyramidTrunc(System.Drawing.Rectangle r, int startZ, int endZ, int truncX, int truncZ)
        {
            InternalPyramidTrunc trunc = new InternalPyramidTrunc();
            trunc.r = r;
            trunc.StartZ = startZ;
            trunc.EndZ = endZ;
            trunc.TruncX = truncX;
            trunc.TruncZ = truncZ;
            trunc.Draw(this);
        }

        protected static float Rad2Deg(double radian)
        {
            return (float) ((180.0 * radian) / 3.1415926535897931);
        }

        public abstract void Rectangle(System.Drawing.Rectangle r);
        public abstract void Rectangle(System.Drawing.Brush b, System.Drawing.Rectangle r);
        public void Rectangle(System.Drawing.Rectangle r, int z)
        {
            this.Calc3DPos(ref this.IPoints[0], r.X, r.Y, z);
            this.Calc3DPos(ref this.IPoints[1], r.Right, r.Y, z);
            this.Calc3DPos(ref this.IPoints[2], r.Right, r.Bottom, z);
            this.Calc3DPos(ref this.IPoints[3], r.X, r.Bottom, z);
            this.PolygonFour();
        }

        public void Rectangle(int left, int top, int right, int bottom)
        {
            int num;
            int num2;
            if (left > right)
            {
                num = left - right;
                left = right;
            }
            else
            {
                num = right - left;
            }
            if (top > bottom)
            {
                num2 = top - bottom;
                top = bottom;
            }
            else
            {
                num2 = bottom - top;
            }
            this.Rectangle(new System.Drawing.Rectangle(left, top, num, num2));
        }

        public void Rectangle(int left, int top, int right, int bottom, int z)
        {
            this.Rectangle(System.Drawing.Rectangle.FromLTRB(left, top, right, bottom), z);
        }

        [EditorBrowsable(EditorBrowsableState.Never), Obsolete("Please use Rectangle method.")]
        public void RectangleWithZ(System.Drawing.Rectangle r, int z)
        {
            this.Rectangle(r, z);
        }

        public void RectangleY(int left, int top, int right, int z0, int z1)
        {
            this.Calc3DPos(ref this.IPoints[0], left, top, z0);
            this.Calc3DPos(ref this.IPoints[1], right, top, z0);
            this.Calc3DPos(ref this.IPoints[2], right, top, z1);
            this.Calc3DPos(ref this.IPoints[3], left, top, z1);
            this.PolygonFour();
        }

        public void RectangleZ(int left, int top, int bottom, int z0, int z1)
        {
            this.Calc3DPos(ref this.IPoints[0], left, top, z0);
            this.Calc3DPos(ref this.IPoints[1], left, top, z1);
            this.Calc3DPos(ref this.IPoints[2], left, bottom, z1);
            this.Calc3DPos(ref this.IPoints[3], left, bottom, z0);
            this.PolygonFour();
        }

        internal static void RectCenter(System.Drawing.Rectangle r, out int x, out int y)
        {
            x = (r.Left + r.Right) / 2;
            y = (r.Top + r.Bottom) / 2;
        }

        public System.Drawing.Rectangle RectFromPolygon(int num, params Point[] p)
        {
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(p[0].X, p[0].Y, 0, 0);
            for (int i = 1; i < num; i++)
            {
                if (p[i].X < rectangle.X)
                {
                    rectangle.X = p[i].X;
                }
                else if (p[i].X > rectangle.Right)
                {
                    rectangle.Width = p[i].X - rectangle.X;
                }
                if (p[i].Y < rectangle.Y)
                {
                    rectangle.Y = p[i].Y;
                }
                else if (p[i].Y > rectangle.Bottom)
                {
                    rectangle.Height = p[i].Y - rectangle.Y;
                }
            }
            rectangle.Width++;
            rectangle.Height++;
            return rectangle;
        }

        public System.Drawing.Rectangle RectFromRectZ(System.Drawing.Rectangle r, int z)
        {
            return this.RectFromPolygon(4, this.FourPointsFromRect(r, z));
        }

        public abstract void RotateLabel(int x, int y, string text, double rotDegree);
        public void RotateLabel(int x, int y, int z, string text, double rotDegree)
        {
            this.Calc3DPos(ref x, ref y, z);
            this.RotateLabel(x, y, text, rotDegree);
        }

        internal static void RotatePoint(ref Point p, int ax, int ay, Point tmpCenter, double tmpCos, double tmpSin)
        {
            p.X = tmpCenter.X + ((int) Math.Round((double) ((ax * tmpCos) + (ay * tmpSin))));
            p.Y = tmpCenter.Y + ((int) Math.Round((double) ((-ax * tmpSin) + (ay * tmpCos))));
        }

        internal static Point[] RotateRectangle(System.Drawing.Rectangle r, int angle)
        {
            int num;
            int num2;
            RectCenter(r, out num, out num2);
            Point tmpCenter = new Point(num, num2);
            double a = angle * Utils.PiStep;
            double tmpSin = Math.Sin(a);
            double tmpCos = Math.Cos(a);
            System.Drawing.Rectangle rectangle = r;
            rectangle.Offset(-tmpCenter.X, -tmpCenter.Y);
            Point[] pointArray = new Point[4];
            RotatePoint(ref pointArray[0], rectangle.X, rectangle.Y, tmpCenter, tmpCos, tmpSin);
            RotatePoint(ref pointArray[1], rectangle.Right, rectangle.Y, tmpCenter, tmpCos, tmpSin);
            RotatePoint(ref pointArray[2], rectangle.Right, rectangle.Bottom, tmpCenter, tmpCos, tmpSin);
            RotatePoint(ref pointArray[3], rectangle.X, rectangle.Bottom, tmpCenter, tmpCos, tmpSin);
            return pointArray;
        }

        public void RoundRectangle(System.Drawing.Rectangle r)
        {
            this.RoundRectangle(r, 8, 8);
        }

        public abstract void RoundRectangle(System.Drawing.Rectangle r, int roundWidth, int roundHeight);
        protected void RoundRectangle(System.Drawing.Rectangle r, int roundWidth, int roundHeight, Graphics g)
        {
            if (roundWidth == roundHeight)
            {
                using (GraphicsPath path = this.GetClipRoundRectangle(r, roundWidth))
                {
                    if (this.Brush.Visible)
                    {
                        if (this.Brush.GradientVisible)
                        {
                            g.FillPath(this.Brush.Gradient.DrawingBrush(r), path);
                        }
                        else
                        {
                            g.FillPath(this.Brush.DrawingBrush, path);
                        }
                    }
                    if (this.Pen.Visible)
                    {
                        g.DrawPath(this.Pen.DrawingPen, path);
                    }
                    return;
                }
            }
            int x = (r.X + r.Width) - roundWidth;
            int y = (r.Y + r.Height) - roundHeight;
            using (GraphicsPath path2 = new GraphicsPath())
            {
                path2.AddArc(r.X, y, roundWidth, roundHeight, 180f, -90f);
                path2.AddArc(x, y, roundWidth, roundHeight, 90f, -90f);
                path2.AddArc(x, r.Y, roundWidth, roundHeight, 360f, -90f);
                path2.AddArc(r.X, r.Y, roundWidth, roundHeight, 270f, -90f);
                path2.CloseFigure();
                if (this.Brush.Visible)
                {
                    if (this.Brush.GradientVisible)
                    {
                        g.FillPath(this.Brush.Gradient.DrawingBrush(r), path2);
                    }
                    else
                    {
                        g.FillPath(this.Brush.DrawingBrush, path2);
                    }
                }
                if (this.Pen.Visible)
                {
                    g.DrawPath(this.Pen.DrawingPen, path2);
                }
            }
        }

        public virtual void ShowImage()
        {
            this.Dirty = false;
        }

        internal static Point[] SliceArray(ref Array source, int length)
        {
            Point[] destinationArray = new Point[length];
            Array.Copy(source, destinationArray, length);
            return destinationArray;
        }

        private Color SpecialColor(Color baseColor)
        {
            if (baseColor == Color.FromArgb(baseColor.A, 0xff, 0xff, 0xff))
            {
                baseColor = Color.FromArgb(baseColor.A, 0xd4, 0xd4, 0xd4);
            }
            if (baseColor == Color.FromArgb(baseColor.A, 0xff, 0xff, 0))
            {
                baseColor = Color.FromArgb(baseColor.A, 0xd4, 0xd4, 0);
            }
            return baseColor;
        }

        public void Sphere(int x, int y, int z, double radius)
        {
            int num = Utils.Round(radius);
            this.Ellipse(x - num, y - num, x + num, y + num, z);
        }

        public void SphereEnh(int x1, int y1, int x2, int y2)
        {
            Color baseColor = this.Brush.Color;
            this.Brush.Color = this.SpecialColor(baseColor);
            int transparency = this.Brush.Transparency;
            System.Drawing.Rectangle r = new System.Drawing.Rectangle(x1, y1, x2 - x1, y2 - y1);
            new System.Drawing.Rectangle(r.Left + ((int) (r.Width * 0.08)), r.Top + ((int) (r.Height * 0.03)), (int) (r.Width * 0.84), (int) (r.Height * 0.94));
            this.Pen.Visible = false;
            System.Drawing.Rectangle chartRect = base.chart.ChartRect;
            Region region = this.AddRightRegion(chartRect, 0, base.chart.Aspect.Width3D);
            this.g.Clip = region;
            this.Ellipse(r);
            if (((x2 - x1) < 5) || ((y2 - y1) < 5))
            {
                this.UnClip();
            }
            else
            {
                GraphicsPath path = new GraphicsPath();
                path.AddEllipse(r);
                Region region2 = new Region(path);
                region2.Intersect(region);
                this.g.Clip = region2;
                GraphicsPath path2 = new GraphicsPath();
                System.Drawing.Rectangle rect = new System.Drawing.Rectangle(r.X - (r.Width / 10), r.Y - (r.Height / 10), r.Width, r.Height);
                if ((rect.Width < 5) || (rect.Height < 5))
                {
                    this.UnClip();
                }
                else
                {
                    path2.AddEllipse(rect);
                    PathGradientBrush brush = new PathGradientBrush(path2);
                    Color c = baseColor;
                    ApplyBright(ref c, 0x80);
                    brush.CenterColor = c;
                    Color[] colorArray = new Color[] { Color.Transparent };
                    brush.SurroundColors = colorArray;
                    this.FillRectangle(brush, r.X, r.Y, r.Width, r.Height);
                    this.UnClip();
                }
            }
        }

        public void SphereEnh(int x1, int y1, int x2, int y2, int z)
        {
            this.Calc3DPos(ref x1, ref y1, z);
            this.Calc3DPos(ref x2, ref y2, z);
            this.SphereEnh(x1, y1, x2, y2);
        }

        public float TextHeight(string text)
        {
            return this.MeasureString(this.Font, text).Height;
        }

        public float TextHeight(ChartFont f, string text)
        {
            return this.MeasureString(f, text).Height;
        }

        public void TextOut(int x, int y, string text)
        {
            this.TextOut(this.font, x, y, text);
        }

        public void TextOut(ChartFont f, int x, int y, string text)
        {
            if (f.ShouldDrawShadow())
            {
                this.DoDrawString(x + f.shadow.Width, y + f.shadow.Height, text, f.shadow.Brush);
            }
            this.DoDrawString(x, y, text, f.Brush);
        }

        public void TextOut(int x, int y, int z, string text)
        {
            this.Calc3DPos(ref x, ref y, z);
            if (this.IZoomText && (this.IZoomFactor != -1.0))
            {
                float size = this.font.DrawingFont.Size;
                int num2 = (int) Math.Max((double) 1.0, (double) (this.IZoomFactor * size));
                if (size != num2)
                {
                    this.font.Size = num2;
                }
            }
            this.TextOut(x, y, text);
        }

        public float TextWidth(string text)
        {
            return this.MeasureString(this.Font, text).Width;
        }

        public float TextWidth(ChartFont f, string text)
        {
            return this.MeasureString(f, text).Width;
        }

        public static int Transparency(Color color)
        {
            int a = color.A;
            if (a != 0xff)
            {
                return (int) (0.39216 * (0xff - a));
            }
            return 0;
        }

        public static Color TransparentColor(int transparency, Color color)
        {
            return Color.FromArgb(Utils.Round((double) ((100 - transparency) * 2.55)), color);
        }

        internal abstract void TransparentEllipse(int x1, int y1, int x2, int y2);
        internal void TransparentEllipse(int x1, int y1, int x2, int y2, int z)
        {
            this.Calc3DPos(ref x1, ref y1, z);
            this.Calc3DPos(ref x2, ref y2, z);
            this.TransparentEllipse(x1, y1, x2, y2);
        }

        public void Triangle(Triangle3D p)
        {
            Point[] pointArray = new Point[] { this.Calc3DPoint(p.p0), this.Calc3DPoint(p.p1), this.Calc3DPoint(p.p2) };
            this.Polygon(pointArray);
        }

        public void Triangle(Point p0, Point p1, Point p2, int z)
        {
            p0 = this.Calc3DPoint(p0.X, p0.Y, z);
            p1 = this.Calc3DPoint(p1.X, p1.Y, z);
            p2 = this.Calc3DPoint(p2.X, p2.Y, z);
            Point[] p = new Point[] { p0, p1, p2 };
            this.Polygon(p);
        }

        public abstract void UnClip();
        [Obsolete("Please use UnClip() method")]
        public void UnClipRectangle()
        {
            this.UnClip();
        }

        public virtual bool ValidState()
        {
            return true;
        }

        public abstract void VerticalLine(int x, int top, int bottom);
        public void VerticalLine(int x, int top, int bottom, int z)
        {
            int num = x;
            this.Calc3DPos(ref num, ref top, z);
            this.Calc3DPos(ref x, ref bottom, z);
            this.Line(num, top, x, bottom);
        }

        public void ZLine(int x, int y, int z0, int z1)
        {
            int num = x;
            int num2 = y;
            this.Calc3DPos(ref x, ref y, z0);
            this.Calc3DPos(ref num, ref num2, z1);
            this.Line(x, y, num, num2);
        }

        [Description("Sets / returns the color used to fill behind text or non-solid brush styles.")]
        public Color BackColor
        {
            get
            {
                return this.Brush.Color;
            }
            set
            {
                this.Brush.Color = value;
            }
        }

        [Description("Determines Brush used to fill the Canvas draw rectangle background.")]
        public ChartBrush Brush
        {
            get
            {
                return this.bBrush;
            }
            set
            {
                this.bBrush.Assign(value);
            }
        }

        [Description("Returns the centre Horizontal co-ordinate of the Chart.")]
        public int ChartXCenter
        {
            get
            {
                return this.XCenter;
            }
        }

        [Description("Returns the middle Vertical coordinate of the Chart.")]
        public int ChartYCenter
        {
            get
            {
                return this.YCenter;
            }
        }

        [Description("Determines Font for outputted text when using the Drawing.")]
        public ChartFont Font
        {
            get
            {
                return this.font;
            }
            set
            {
                this.font.Assign(value);
            }
        }

        [Description("Defines the Height of the Font in pixels.")]
        public int FontHeight
        {
            get
            {
                return (int) this.TextHeight("W");
            }
        }

        [Description("Gets and sets the Brush.Gradient properties of the Canvas.")]
        public Steema.TeeChart.Drawing.Gradient Gradient
        {
            get
            {
                return this.bBrush.Gradient;
            }
            set
            {
                this.bBrush.Gradient = value;
            }
        }

        internal double IZoomfactor
        {
            get
            {
                return this.IZoomFactor;
            }
            set
            {
                this.IZoomFactor = value;
            }
        }

        [Description("Indicates the kind of pen used to draw Canvas lines.")]
        public ChartPen Pen
        {
            get
            {
                return this.pen;
            }
            set
            {
                this.pen.Assign(value);
            }
        }

        [Description("Sets the Pixel location (using X,Y,Z) of the centre of rotation.")]
        public PointXYZ RotationCenter
        {
            get
            {
                return this.rotationCenter;
            }
            set
            {
                this.rotationCenter = value;
                this.Invalidate();
            }
        }

        [Description("Returns the height, in pixels, of the Chart Panel.")]
        public static int ScreenHeight
        {
            get
            {
                return Screen.PrimaryScreen.Bounds.Height;
            }
        }

        [Description("Returns the width, in pixels, of the Chart Panel.")]
        public static int ScreenWidth
        {
            get
            {
                return Screen.PrimaryScreen.Bounds.Width;
            }
        }

        public System.Drawing.Drawing2D.SmoothingMode SmoothingMode
        {
            get
            {
                return this.aSmoothingMode;
            }
            set
            {
                if (this.aSmoothingMode != value)
                {
                    this.aSmoothingMode = value;
                    this.Invalidate();
                }
            }
        }

        [Description("Returns if Canvas supports 3D Text or not.")]
        public bool Supports3DText
        {
            get
            {
                return this.GetSupports3DText();
            }
            set
            {
                this.supports3DText = value;
            }
        }

        [Description("Returns if Canvas can do rotation and elevation of more than 90 degree.")]
        public bool SupportsFullRotation
        {
            get
            {
                return false;
            }
        }

        [Description("Sets the alignment used when displaying text using TextOut or TextOut3D."), DefaultValue(0)]
        public StringAlignment TextAlign
        {
            get
            {
                return this.stringFormat.Alignment;
            }
            set
            {
                this.stringFormat.Alignment = value;
            }
        }

        public System.Drawing.Text.TextRenderingHint TextRenderingHint
        {
            get
            {
                return this.aTextRenderingHint;
            }
            set
            {
                if (this.aTextRenderingHint != value)
                {
                    this.aTextRenderingHint = value;
                    this.Invalidate();
                }
            }
        }

        [Description("Draws items to an internal canvas to prevent flickering on screen.")]
        public bool UseBuffer
        {
            get
            {
                return this.buffered;
            }
            set
            {
                this.buffered = value;
                if (base.chart.parent != null)
                {
                    base.chart.parent.DoSetControlStyle();
                }
            }
        }

        [Description("Obtain the X coordinate of the pixel location of the center of the 3D Canvas.")]
        public int XCenter
        {
            get
            {
                return this.xCenter;
            }
            set
            {
                this.xCenter = value;
            }
        }

        [Description("Obtain the Y coordinate of the pixel location of the center of the 3D Canvas.")]
        public int YCenter
        {
            get
            {
                return this.yCenter;
            }
            set
            {
                this.yCenter = value;
            }
        }

        private class ArrowPoint
        {
            internal double CosA;
            internal Graphics3D g;
            internal double SinA;
            internal double x;
            internal double y;
            internal int z;

            public Point Calc()
            {
                Point p = new Point(Utils.Round((double) ((this.x * this.CosA) + (this.y * this.SinA))), Utils.Round((double) ((-this.x * this.SinA) + (this.y * this.CosA))));
                return this.g.Calc3DPoint(p, this.z);
            }
        }

        private class InternalPyramidTrunc
        {
            internal int EndZ;
            internal Graphics3D g;
            internal int MidX;
            internal int MidZ;
            internal Rectangle r;
            internal int StartZ;
            internal int TruncX;
            internal int TruncZ;

            private void BottomCover()
            {
                this.g.RectangleY(this.r.X, this.r.Bottom, this.r.Right, this.StartZ, this.EndZ);
            }

            public void Draw(Graphics3D gr)
            {
                this.g = gr;
                this.MidX = (this.r.X + this.r.Right) / 2;
                this.MidZ = (this.StartZ + this.EndZ) / 2;
                if (this.r.Bottom > this.r.Y)
                {
                    this.BottomCover();
                }
                else
                {
                    this.TopCover();
                }
                this.FrontWall(this.MidZ + this.TruncZ, this.EndZ);
                this.SideWall(this.r.X, this.MidX - this.TruncX, this.StartZ, this.EndZ);
                this.FrontWall(this.MidZ - this.TruncZ, this.StartZ);
                this.SideWall(this.r.Right, this.MidX + this.TruncX, this.StartZ, this.EndZ);
                if (this.r.Bottom > this.r.Y)
                {
                    this.TopCover();
                }
                else
                {
                    this.BottomCover();
                }
            }

            private void FrontWall(int StartZ, int EndZ)
            {
                Point[] p = new Point[4];
                p[0].X = this.MidX - this.TruncX;
                p[0].Y = this.r.Y;
                p[1].X = this.MidX + this.TruncX;
                p[1].Y = this.r.Y;
                p[2].X = this.r.Right;
                p[2].Y = this.r.Bottom;
                p[3].X = this.r.X;
                p[3].Y = this.r.Bottom;
                this.g.PlaneFour3D(StartZ, EndZ, p);
            }

            private void SideWall(int HorizPos1, int HorizPos2, int StartZ, int EndZ)
            {
                this.g.IPoints[0] = this.g.Calc3DPoint(HorizPos2, this.r.Y, this.MidZ - this.TruncZ);
                this.g.IPoints[1] = this.g.Calc3DPoint(HorizPos2, this.r.Y, this.MidZ + this.TruncZ);
                this.g.IPoints[2] = this.g.Calc3DPoint(HorizPos1, this.r.Bottom, EndZ);
                this.g.IPoints[3] = this.g.Calc3DPoint(HorizPos1, this.r.Bottom, StartZ);
                this.g.PolygonFour();
            }

            private void TopCover()
            {
                if (this.TruncX != 0)
                {
                    this.g.RectangleY(this.MidX - this.TruncX, this.r.Y, this.MidX + this.TruncX, this.MidZ - this.TruncZ, this.MidZ + this.TruncZ);
                }
            }
        }

        private enum LineOrientations
        {
            Point,
            Horizontal,
            Vertical
        }

        private class Pie3D
        {
            private Point center = Point.Empty;
            public int CircleSteps;
            private bool dark;
            private int donut;
            public int End3D;
            private Graphics3D g;
            public const int MaxCircleSteps = 0x20;
            public Color OldColor;
            public Point[] Points = new Point[0x41];
            public Point[] Points3D = new Point[0x41];
            public Point[] Points3D2 = new Point[0x41];
            public int Start3D;
            private double tmpXRadius;
            private double tmpYRadius;

            public Pie3D(Graphics3D graphics)
            {
                this.g = graphics;
            }

            private void CalcCenter(ref Point p, double angle, int z)
            {
                if (this.donut > 0)
                {
                    double num = Math.Sin(angle);
                    double num2 = Math.Cos(angle);
                    int x = this.center.X + Utils.Round((double) (this.tmpXRadius * num2));
                    int y = this.center.Y - Utils.Round((double) (this.tmpYRadius * num));
                    this.g.Calc3DPos(ref p, x, y, z);
                }
                else
                {
                    this.g.Calc3DPos(ref p, this.center, z);
                }
            }

            public void Draw3DPie()
            {
                if (this.dark)
                {
                    this.g.InternalApplyDark(this.OldColor, 0x20);
                }
                try
                {
                    int num;
                    int circleSteps;
                    if ((this.Start3D == 1) && (this.End3D == this.CircleSteps))
                    {
                        for (num = 1; num <= this.CircleSteps; num++)
                        {
                            this.Points3D[num - 1] = this.Points[num];
                        }
                        circleSteps = this.CircleSteps;
                    }
                    else
                    {
                        circleSteps = 0;
                        for (num = this.Start3D; num <= this.End3D; num++)
                        {
                            this.Points3D[circleSteps] = this.Points[num];
                            this.Points3D[((this.End3D - this.Start3D) + 1) + circleSteps] = this.Points3D[((2 * this.CircleSteps) - this.End3D) + circleSteps];
                            circleSteps++;
                        }
                    }
                    Array source = this.Points3D;
                    this.g.Polygon(Graphics3D.SliceArray(ref source, 2 * circleSteps));
                }
                catch (IndexOutOfRangeException exception)
                {
                    Console.WriteLine("{0} Caught exception #1.", exception);
                }
            }

            private void FinishSide(double angle, int z)
            {
                this.CalcCenter(ref this.g.IPoints[3], angle, z);
                if (this.dark)
                {
                    this.g.InternalApplyDark(this.OldColor, 0x20);
                }
                this.g.PolygonFour();
            }

            public void Pie(int xCenter, int yCenter, int xRadius, int yRadius, int z0, int z1, double startAngle, double endAngle, bool darkSides, bool drawSides, int donutPercent)
            {
                int num3;
                this.center.X = xCenter;
                this.center.Y = yCenter;
                this.dark = darkSides;
                this.CircleSteps = 2 + Math.Min(30, Utils.Round((double) (((180.0 * Math.Abs((double) (endAngle - startAngle))) / 3.1415926535897931) / 10.0)));
                this.donut = donutPercent;
                if (donutPercent > 0)
                {
                    this.tmpXRadius = (donutPercent * xRadius) * 0.01;
                    this.tmpYRadius = (donutPercent * yRadius) * 0.01;
                }
                this.CalcCenter(ref this.Points[0], startAngle, z1);
                double num7 = (endAngle - startAngle) / ((double) (this.CircleSteps - 1));
                double a = startAngle;
                for (num3 = 1; num3 <= this.CircleSteps; num3++)
                {
                    double num = Math.Sin(a);
                    double num2 = Math.Cos(a);
                    int x = xCenter + Utils.Round((double) (xRadius * num2));
                    int y = yCenter - Utils.Round((double) (yRadius * num));
                    this.g.Calc3DPos(ref this.Points[num3], x, y, z1);
                    this.g.Calc3DPos(ref this.Points3D[(2 * this.CircleSteps) - num3], x, y, z0);
                    a += num7;
                }
                if (this.g.Brush.Solid)
                {
                    this.OldColor = this.g.Brush.Color;
                }
                else
                {
                    this.OldColor = this.g.BackColor;
                }
                if (donutPercent > 0)
                {
                    this.CalcCenter(ref this.Points[(2 * this.CircleSteps) + 1], endAngle, z1);
                    this.Points3D2[0] = this.Points[0];
                    a = endAngle;
                    for (num3 = 1; num3 <= this.CircleSteps; num3++)
                    {
                        this.CalcCenter(ref this.Points[this.CircleSteps + num3], a, z1);
                        this.Points3D2[num3 - 1] = this.Points[this.CircleSteps + num3];
                        this.CalcCenter(ref this.Points3D2[(2 * this.CircleSteps) - num3], a, z0);
                        a -= num7;
                    }
                    if (darkSides)
                    {
                        this.g.InternalApplyDark(this.OldColor, 0x20);
                    }
                    Array source = this.Points3D2;
                    this.g.Polygon(Graphics3D.SliceArray(ref source, 2 * this.CircleSteps));
                }
                if (drawSides)
                {
                    if (this.Points[this.CircleSteps].X < xCenter)
                    {
                        if (donutPercent > 0)
                        {
                            this.g.IPoints[0] = this.Points[(2 * this.CircleSteps) + 1];
                        }
                        else
                        {
                            this.g.IPoints[0] = this.Points[0];
                        }
                        this.g.IPoints[1] = this.Points[this.CircleSteps];
                        this.g.IPoints[2] = this.Points3D[this.CircleSteps];
                        this.FinishSide(endAngle, z0);
                    }
                    if (this.Points[1].X > xCenter)
                    {
                        this.g.IPoints[0] = this.Points[0];
                        this.g.IPoints[1] = this.Points[1];
                        this.g.IPoints[2] = this.Points3D[(2 * this.CircleSteps) - 1];
                        this.FinishSide(startAngle, z0);
                    }
                }
                if (this.g.Brush.Solid)
                {
                    this.g.Brush.Color = this.OldColor;
                }
                else
                {
                    this.g.BackColor = this.OldColor;
                }
                Array points = this.Points;
                if (donutPercent > 0)
                {
                    this.g.Polygon(Graphics3D.SliceArray(ref points, (2 * this.CircleSteps) + 1));
                }
                else
                {
                    this.g.Polygon(Graphics3D.SliceArray(ref points, this.CircleSteps + 1));
                }
                bool flag2 = false;
                this.Start3D = 0;
                this.End3D = 0;
                for (num3 = 2; num3 <= this.CircleSteps; num3++)
                {
                    if (this.Points[num3].X <= this.Points[num3 - 1].X)
                    {
                        continue;
                    }
                    this.Start3D = num3 - 1;
                    bool flag = true;
                    int index = num3 + 1;
                    while (index < this.CircleSteps)
                    {
                        if (this.Points[index + 1].X < this.Points[index].X)
                        {
                            this.End3D = index;
                            flag2 = true;
                            break;
                        }
                        index++;
                    }
                    if (!flag2 && (this.Points[this.CircleSteps].X >= this.Points[this.CircleSteps - 1].X))
                    {
                        this.End3D = this.CircleSteps;
                        flag2 = true;
                    }
                    if (flag && flag2)
                    {
                        this.Draw3DPie();
                    }
                    if ((this.End3D == this.CircleSteps) || (this.Points[this.CircleSteps].X <= this.Points[this.CircleSteps - 1].X))
                    {
                        break;
                    }
                    this.End3D = this.CircleSteps;
                    index = this.CircleSteps - 1;
                    while (this.Points[index].X > this.Points[index - 1].X)
                    {
                        index--;
                        if (index == 1)
                        {
                            break;
                        }
                    }
                    if (index <= 1)
                    {
                        break;
                    }
                    this.Start3D = index;
                    this.Draw3DPie();
                    return;
                }
            }
        }
    }
}

