namespace Steema.TeeChart.Drawing
{
    using Steema.TeeChart;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;

    public class Graphics3DGdiPlus : Graphics3D
    {
        protected int currentX;
        protected int currentY;

        public Graphics3DGdiPlus(Chart c) : base(c)
        {
        }

        public override void Arc(int x1, int y1, int x2, int y2, float startAngle, float sweepAngle)
        {
            if (base.Pen.bVisible)
            {
                System.Drawing.Rectangle rect = System.Drawing.Rectangle.FromLTRB(x1, y1, x2, y2);
                base.g.DrawArc(base.Pen.DrawingPen, rect, startAngle, sweepAngle);
            }
        }

        public override void Arc(int x1, int y1, int x2, int y2, int x3, int y3, int x4, int y4)
        {
            if (base.Pen.bVisible)
            {
                float num7;
                int num = x2 - x1;
                int num2 = y2 - y1;
                double num3 = (num * 0.5) + x1;
                double num4 = (num2 * 0.5) + y1;
                float startAngle = 360f - Graphics3D.Rad2Deg(Math.Atan2((y2 - num4) - ((y3 - num4) - (y1 - num4)), x3 - num3));
                float num6 = 360f - Graphics3D.Rad2Deg(Math.Atan2((y2 - num4) - ((y4 - num4) - (y1 - num4)), x4 - num3));
                System.Drawing.Rectangle rect = System.Drawing.Rectangle.FromLTRB(x1, y1, x2, y2);
                if (startAngle < num6)
                {
                    num7 = Math.Abs((float) (num6 - startAngle));
                }
                else
                {
                    num7 = Math.Abs((float) (num6 + (360f - startAngle)));
                }
                base.g.DrawArc(base.Pen.DrawingPen, rect, startAngle, num7);
            }
        }

        public override void ClipEllipse(System.Drawing.Rectangle r)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(r);
            base.g.Clip = new Region(path);
        }

        public override void ClipPolygon(params Point[] p)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddPolygon(p);
            base.g.Clip = new Region(path);
        }

        public override void ClipRectangle(System.Drawing.Rectangle r)
        {
            base.g.Clip = new Region(base.CorrectRectangle(r));
        }

        protected override void DoDrawString(int x, int y, string text, ChartBrush aBrush)
        {
            Brush drawingBrush;
            if (aBrush.GradientVisible)
            {
                drawingBrush = aBrush.Gradient.DrawingBrush(base.g.MeasureString(text, base.Font.DrawingFont).ToSize());
            }
            else
            {
                drawingBrush = aBrush.DrawingBrush;
            }
            base.g.DrawString(text, base.Font.DrawingFont, drawingBrush, (float) x, (float) y, base.stringFormat);
        }

        public override void Draw(System.Drawing.Rectangle r, Image image, bool transparent)
        {
            if (transparent)
            {
                ImageAttributes imageAttr = new ImageAttributes();
                Color pixel = new Bitmap(image).GetPixel(1, 1);
                imageAttr.SetColorKey(pixel, pixel);
                base.g.DrawImage(image, r, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttr);
            }
            else
            {
                base.g.DrawImage(image, r);
            }
        }

        public override void Draw(int x, int y, Image image)
        {
            base.g.DrawImageUnscaled(image, x, y);
        }

        public override void DrawBeziers(params Point[] p)
        {
            base.g.DrawBeziers(base.Pen.DrawingPen, p);
        }

        private void DrawBrushImage(System.Drawing.Rectangle rect)
        {
            if ((base.Brush.ImageMode == ImageMode.Normal) || (base.Brush.ImageMode == ImageMode.Center))
            {
                base.Brush.Solid = true;
                base.g.FillRectangle(base.Brush.DrawingBrush, rect);
                base.Brush.Solid = false;
            }
            if (base.Brush.ImageTransparent)
            {
                base.Draw(rect, base.Brush.Image, base.Brush.ImageMode, true);
            }
            else
            {
                base.Draw(rect, base.Brush.Image, base.Brush.ImageMode, false);
            }
        }

        public override void Ellipse(int x1, int y1, int x2, int y2)
        {
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(x1, y1, x2 - x1, y2 - y1);
            if (base.Brush.visible)
            {
                if (base.Brush.GradientVisible)
                {
                    base.g.FillEllipse(base.Brush.Gradient.DrawingBrush(rect), rect);
                }
                else
                {
                    base.g.FillEllipse(base.Brush.DrawingBrush, rect);
                }
            }
            if (base.Pen.bVisible)
            {
                base.g.DrawEllipse(base.Pen.DrawingPen, rect);
            }
        }

        public override void EraseBackground(int left, int top, int right, int bottom)
        {
            System.Drawing.Rectangle rect = System.Drawing.Rectangle.FromLTRB(left, top, right, bottom);
            if (base.Brush.GradientVisible)
            {
                base.g.FillRectangle(base.Brush.Gradient.DrawingBrush(rect), rect);
            }
            else
            {
                base.g.FillRectangle(base.Brush.DrawingBrush, rect);
            }
        }

        public override void GradientFill(int left, int top, int right, int bottom, Color startColor, Color endColor, LinearGradientMode direction)
        {
            System.Drawing.Rectangle rect = System.Drawing.Rectangle.FromLTRB(left, top, right, bottom);
            using (LinearGradientBrush brush = new LinearGradientBrush(rect, startColor, endColor, direction))
            {
                base.g.FillRectangle(brush, rect);
            }
        }

        public override void HorizontalLine(int left, int right, int y)
        {
            base.g.DrawLine(base.Pen.DrawingPen, left, y, right, y);
            this.MoveTo(right, y);
        }

        protected internal override void InitWindow(System.Drawing.Graphics graphics, Aspect a, System.Drawing.Rectangle r, int MaxDepth)
        {
            base.g = graphics;
            base.InitWindow(graphics, a, r, MaxDepth);
            if (!base.metafiling)
            {
                base.g.SmoothingMode = base.aSmoothingMode;
                base.g.TextRenderingHint = base.aTextRenderingHint;
            }
        }

        protected override void Line(ChartPen p, Point a, Point b)
        {
            base.g.DrawLine(p.DrawingPen, a, b);
            base.MoveTo(b);
        }

        public override void Line(int x0, int y0, int x1, int y1)
        {
            base.g.DrawLine(base.Pen.DrawingPen, x0, y0, x1, y1);
            this.MoveTo(x1, y1);
        }

        public override void LineTo(int x, int y)
        {
            base.g.DrawLine(base.Pen.DrawingPen, this.currentX, this.currentY, x, y);
            this.MoveTo(x, y);
        }

        public override SizeF MeasureString(ChartFont f, string text)
        {
            if (base.g == null)
            {
                Bitmap image = new Bitmap(1, 1);
                base.g = System.Drawing.Graphics.FromImage(image);
            }
            return base.g.MeasureString(text, f.DrawingFont);
        }

        public override void MoveTo(int x, int y)
        {
            this.currentX = x;
            this.currentY = y;
        }

        public override void Pie(int x1, int y1, int x2, int y2, double startAngle, double endAngle)
        {
            int num = -Utils.Round(startAngle);
            int num2 = -Utils.Round((double) (endAngle - startAngle));
            System.Drawing.Rectangle rect = System.Drawing.Rectangle.FromLTRB(x1, y1, x2, y2);
            if (base.Brush.visible)
            {
                if (base.Brush.GradientVisible)
                {
                    base.g.FillPie(base.Brush.Gradient.DrawingBrush(rect), rect, (float) num, (float) num2);
                }
                else
                {
                    base.g.FillPie(base.Brush.DrawingBrush, rect, (float) num, (float) num2);
                }
            }
            if (base.Pen.bVisible)
            {
                base.g.DrawPie(base.Pen.DrawingPen, rect, (float) num, (float) num2);
            }
        }

        public override void Pixel(int x, int y, int z, Color color)
        {
            base.Pen.Color = color;
            base.Calc3DPos(ref x, ref y, z);
            base.g.DrawLine(base.Pen.DrawingPen, x, y, x + 1, y + 1);
        }

        public override void Polygon(params Point[] p)
        {
            if (base.Brush.visible)
            {
                if (base.Brush.GradientVisible)
                {
                    if (base.Brush.Gradient.Polygonal)
                    {
                        base.g.FillPolygon(base.Brush.Gradient.DrawingBrush(p), p);
                    }
                    else
                    {
                        base.g.FillPolygon(base.Brush.Gradient.DrawingBrush(base.PolygonRect(p)), p);
                    }
                }
                else
                {
                    base.g.FillPolygon(base.Brush.DrawingBrush, p);
                }
            }
            if (base.Pen.bVisible)
            {
                base.g.DrawPolygon(base.Pen.DrawingPen, p);
            }
        }

        public override void Polygon(Brush b, params Point[] p)
        {
            base.g.FillPolygon(b, p);
        }

        public override void Polyline(int z, params Point[] p)
        {
            int length = p.Length;
            Point[] points = new Point[length];
            for (int i = 0; i < length; i++)
            {
                points[i] = base.Calc3DPoint(p[i], z);
            }
            base.g.DrawLines(base.Pen.DrawingPen, points);
        }

        public override void PrepareDrawImage()
        {
            base.g.InterpolationMode = InterpolationMode.NearestNeighbor;
            base.g.PixelOffsetMode = PixelOffsetMode.HighQuality;
        }

        public override void Rectangle(System.Drawing.Rectangle r)
        {
            if (base.Brush.visible)
            {
                if (base.Brush.GradientVisible)
                {
                    base.g.FillRectangle(base.Brush.Gradient.DrawingBrush(r), r);
                }
                else if (base.Brush.Image != null)
                {
                    this.DrawBrushImage(r);
                }
                else
                {
                    base.g.FillRectangle(base.Brush.DrawingBrush, r);
                }
            }
            if (base.Pen.bVisible)
            {
                r.Width--;
                r.Height--;
                base.g.DrawRectangle(base.Pen.DrawingPen, r);
            }
        }

        public override void Rectangle(Brush b, System.Drawing.Rectangle r)
        {
            base.g.FillRectangle(b, r);
        }

        public override void RotateLabel(int x, int y, string text, double rotDegree)
        {
            Matrix transform = base.g.Transform;
            transform.RotateAt((float) (360.0 - rotDegree), (PointF) new Point(x, y));
            base.g.MultiplyTransform(transform);
            if (base.Font.ShouldDrawShadow())
            {
                this.DoDrawString(x + base.Font.shadow.Width, y + base.Font.shadow.Height, text, base.Font.shadow.Brush);
            }
            this.DoDrawString(x, y, text, base.Font.Brush);
            base.g.ResetTransform();
        }

        public override void RoundRectangle(System.Drawing.Rectangle r, int roundWidth, int roundHeight)
        {
            base.RoundRectangle(r, roundWidth, roundHeight, base.g);
        }

        public override void ShowImage()
        {
            base.ShowImage();
            base.g = null;
        }

        internal override void TransparentEllipse(int x1, int y1, int x2, int y2)
        {
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(x1, y1, x2 - x1, y2 - y1);
            SolidBrush brush = new SolidBrush(base.chart.Panel.Color);
            base.g.FillEllipse(brush, rect);
        }

        [Description("Removes any clipping region applied to Chart Drawing.")]
        public override void UnClip()
        {
            base.g.Clip.Dispose();
            base.g.ResetClip();
        }

        public override bool ValidState()
        {
            return (base.g != null);
        }

        public override void VerticalLine(int x, int top, int bottom)
        {
            base.g.DrawLine(base.Pen.DrawingPen, x, top, x, bottom);
            this.MoveTo(x, bottom);
        }

        public System.Drawing.Graphics Graphics
        {
            get
            {
                return base.g;
            }
        }
    }
}

