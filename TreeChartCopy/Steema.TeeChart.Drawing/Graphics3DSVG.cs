namespace Steema.TeeChart.Drawing
{
    using Steema.TeeChart;
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.IO;

    public class Graphics3DSVG : Graphics3DVec
    {
        public bool AntiAliasing;
        public string DocType;
        private int fx;
        private int fy;
        private int iClipCount;
        private int iClipStack;
        private int iGradientCount;
        private string m_string;

        public Graphics3DSVG(Stream istream, Chart c) : base(istream, c)
        {
            base.AddToStream("<?xml version=\"1.0\" standalone=\"no\"?>");
            this.DocType = "<!DOCTYPE svg PUBLIC \"-//W3C//DTD SVG 20000303 Stylable//EN\" \"http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd\">";
            this.AntiAliasing = true;
        }

        private void AddEnd(string st)
        {
            base.AddToStream(st + "/>");
        }

        public override void Arc(int x1, int y1, int x2, int y2, float startAngle, float sweepAngle)
        {
            if (base.Pen.Visible)
            {
                int num = (x2 + x1) / 2;
                int num2 = (y2 + y1) / 2;
                int num3 = (x2 - x1) / 2;
                int num4 = (y2 - y1) / 2;
                startAngle = (startAngle * 3.14f) / 180f;
                float num5 = startAngle + sweepAngle;
                int num6 = num + Utils.Round((double) (num3 * Math.Cos((double) startAngle)));
                int num7 = num2 - Utils.Round((double) (num4 * Math.Sin((double) startAngle)));
                int num8 = num + Utils.Round((double) (num3 * Math.Cos((double) num5)));
                int num9 = num2 - Utils.Round((double) (num4 * Math.Sin((double) num5)));
                this.Arc(x1, y1, x2, y2, num6, num7, num8, num9);
            }
        }

        public override void Arc(int x1, int y1, int x2, int y2, int x3, int y3, int x4, int y4)
        {
            if (base.Pen.Visible)
            {
                this.PrepareShape();
                this.m_string = "points=\"" + this.PointToStr(x1, y1) + " " + this.PointToStr(x2, y2) + " ";
                string str = this.m_string;
                this.m_string = str + this.PointToStr(x3, y3) + " " + this.PointToStr(x4, y4) + "\"";
                this.AddEnd(this.m_string);
            }
        }

        public override void ClipEllipse(System.Drawing.Rectangle r)
        {
            this.SVGClip();
            this.AddEnd("<ellipse " + this.SVGEllipse(r.Left, r.Top, r.Right, r.Bottom));
            this.SVGEndClip();
        }

        public override void ClipPolygon(params Point[] p)
        {
            this.SVGClip();
            this.AddEnd("<polygon " + this.SVGPoints(p));
            this.SVGEndClip();
        }

        public override void ClipRectangle(System.Drawing.Rectangle r)
        {
            this.SVGClip();
            this.AddEnd("<rect " + this.SVGRect(r));
            this.SVGEndClip();
        }

        protected override void DoDrawString(int x, int y, string text, ChartBrush aBrush)
        {
            if (base.Font.ShouldDrawShadow())
            {
                this.DoText(x + base.Font.Shadow.Width, y + base.Font.Shadow.Height, text, 0.0, aBrush.Color);
            }
            else
            {
                this.DoText(x, y, text, 0.0, aBrush.Color);
            }
        }

        private void DoText(int x, int y, string text, double degangle, Color c)
        {
            y += Utils.Round((double) (1.25 * base.Font.Size));
            if (degangle != 0.0)
            {
                degangle *= -1.0;
                int num = Utils.Round(degangle);
                this.m_string = "<g transform=\"translate(" + x.ToString() + "," + y.ToString() + ") ";
                this.m_string = this.m_string + "rotate(" + num.ToString() + ")\">";
                base.AddToStream(this.m_string);
                this.m_string = "<text x=\"0\" y=\"0\" ";
            }
            else
            {
                this.m_string = "<text x=\"" + x.ToString() + "\" y=\"" + y.ToString() + "\" ";
            }
            this.m_string = this.m_string + this.FontProperties(base.Font);
            if (base.TextAlign == StringAlignment.Center)
            {
                this.m_string = this.m_string + " text-anchor=\"middle\"";
            }
            else if (base.TextAlign == StringAlignment.Far)
            {
                this.m_string = this.m_string + " text-anchor=\"end\"";
            }
            else
            {
                this.m_string = this.m_string + " text-anchor=\"start\"";
            }
            this.m_string = this.m_string + " fill=" + this.SVGColor(c) + ">";
            base.AddToStream(this.m_string);
            base.AddToStream(text);
            base.AddToStream("</text>");
            if (degangle != 0.0)
            {
                base.AddToStream("</g>");
            }
        }

        public override void Draw(System.Drawing.Rectangle r, Image image, bool transparent)
        {
        }

        public override void Draw(int x, int y, Image image)
        {
        }

        public override void DrawBeziers(params Point[] p)
        {
        }

        private void DrawBrushImage(System.Drawing.Rectangle rect)
        {
        }

        public override void Ellipse(int x1, int y1, int x2, int y2)
        {
            if (base.Brush.Visible || base.Pen.Visible)
            {
                this.m_string = "<ellipse " + this.SVGEllipse(x1, y1, x2, y2);
                this.AddEnd(this.m_string + this.SVGBrushPen(true));
            }
        }

        public override void EraseBackground(int left, int top, int right, int bottom)
        {
        }

        private string FontProperties(ChartFont f)
        {
            this.m_string = "font-family=\"" + f.Name.ToString() + "\" font-size=\"" + f.Size.ToString() + "pt\" ";
            if (f.Bold)
            {
                this.m_string = this.m_string + " font-weight=\"bold\"";
            }
            if (f.Italic)
            {
                this.m_string = this.m_string + " font-style=\"italic\"";
            }
            if (f.Underline)
            {
                this.m_string = this.m_string + " text-decoration=\"underline\"";
            }
            if (f.Strikeout)
            {
                this.m_string = this.m_string + " text-decoration=\"line-through\"";
            }
            return this.m_string;
        }

        public override void GradientFill(int left, int top, int right, int bottom, Color startColor, Color endColor, LinearGradientMode direction)
        {
        }

        public override void HorizontalLine(int left, int right, int y)
        {
            this.MoveTo(left, y);
            this.LineTo(right, y);
        }

        protected internal override void InitWindow(Graphics graphics, Aspect a, System.Drawing.Rectangle r, int MaxDepth)
        {
            base.InitWindow(graphics, a, r, MaxDepth);
            base.AddToStream(this.DocType);
            this.m_string = "<svg " + this.TheBounds();
            if (this.AntiAliasing)
            {
                this.m_string = this.m_string + " style=\"text-antialiasing:true\"";
            }
            base.AddToStream(this.m_string + ">");
        }

        private void InternalRect(ChartBrush b, System.Drawing.Rectangle r, bool UsePen, bool IsRound)
        {
            if (b.Visible || (UsePen && base.Pen.Visible))
            {
                this.m_string = "<rect " + this.SVGRect(r) + this.SVGBrushPen(UsePen);
                if (IsRound)
                {
                    this.m_string = this.m_string + " rx=\"5\"";
                }
                this.AddEnd(this.m_string);
            }
        }

        protected override void Line(ChartPen p, Point a, Point b)
        {
            this.MoveTo(a.X, a.Y);
            this.LineTo(b.X, b.Y);
        }

        public override void Line(int x0, int y0, int x1, int y1)
        {
            this.MoveTo(x0, y0);
            this.LineTo(x1, y1);
        }

        public override void LineTo(int x, int y)
        {
            this.m_string = "<line x1=\"" + this.fx.ToString() + "\" y1=\"" + this.fy.ToString() + "\" ";
            string str = this.m_string;
            this.m_string = str + "x2=\"" + x.ToString() + "\" y2=\"" + y.ToString() + "\" fill=\"none\" " + this.SVGPen();
            this.AddEnd(this.m_string);
            this.fx = x;
            this.fy = y;
        }

        public override SizeF MeasureString(ChartFont f, string text)
        {
            return base.g.MeasureString(text, f.DrawingFont);
        }

        public override void MoveTo(int x, int y)
        {
            this.fx = x;
            this.fy = y;
        }

        private string PenStyle(ChartPen ipen)
        {
            this.m_string = "";
            if (ipen.Style == DashStyle.Dot)
            {
                this.m_string = "2, 2";
            }
            else if (ipen.Style == DashStyle.Dash)
            {
                this.m_string = "4, 2";
            }
            else if (ipen.Style == DashStyle.DashDot)
            {
                this.m_string = "4, 2, 2, 2";
            }
            else if (ipen.Style == DashStyle.DashDotDot)
            {
                this.m_string = "4, 2, 2, 2, 2, 2";
            }
            return this.m_string;
        }

        public override void Pie(int x1, int y1, int x2, int y2, double startAngle, double endAngle)
        {
            if (base.Brush.Visible || base.Pen.Visible)
            {
                int x = (x2 + x1) / 2;
                int y = (y2 + y1) / 2;
                int num3 = (x2 - x1) / 2;
                int num4 = (y2 - y1) / 2;
                startAngle = (startAngle * 3.1415926535897931) / 180.0;
                endAngle = (endAngle * 3.1415926535897931) / 180.0;
                int num5 = x + Utils.Round((double) (num3 * Math.Cos(startAngle)));
                int num6 = y - Utils.Round((double) (num4 * Math.Sin(startAngle)));
                int num7 = x + Utils.Round((double) (num3 * Math.Cos(endAngle)));
                int num8 = y - Utils.Round((double) (num4 * Math.Sin(endAngle)));
                this.PrepareShape();
                this.m_string = " points=\"" + this.PointToStr(x, y) + " ";
                string str = this.m_string;
                this.m_string = str + this.PointToStr(x1, y1) + " " + this.PointToStr(x2, y2) + " ";
                str = this.m_string;
                this.m_string = str + this.PointToStr(num5, num6) + " " + this.PointToStr(num7, num8) + "\"";
                this.AddEnd(this.m_string);
            }
        }

        public override void Pixel(int x, int y, int z, Color color)
        {
        }

        private string PointToStr(int x, int y)
        {
            this.m_string = x.ToString() + "," + y.ToString();
            return this.m_string;
        }

        public override void Polygon(params Point[] p)
        {
            if (base.Brush.Visible || base.Pen.Visible)
            {
                this.PrepareShape();
                this.AddEnd(this.SVGPoints(p));
            }
        }

        public override void Polygon(Brush b, params Point[] p)
        {
        }

        public override void Polyline(int z, params Point[] p)
        {
            if (base.Brush.Visible || base.Pen.Visible)
            {
                base.AddToStream("<polyline fill=\"none\" " + this.SVGPen());
                this.AddEnd(this.SVGPoints(p));
            }
        }

        public override void PrepareDrawImage()
        {
        }

        private void PrepareShape()
        {
            base.AddToStream("<polygon" + this.SVGBrushPen(true));
        }

        public override void Rectangle(System.Drawing.Rectangle r)
        {
            this.InternalRect(base.Brush, r, true, false);
        }

        public override void Rectangle(Brush b, System.Drawing.Rectangle r)
        {
        }

        public override void RotateLabel(int x, int y, string text, double rotDegree)
        {
            if (base.Font.ShouldDrawShadow())
            {
                this.DoText(x + base.Font.Shadow.Width, y + base.Font.Shadow.Height, text, rotDegree, base.Font.Shadow.Color);
            }
            else
            {
                this.DoText(x, y, text, rotDegree, base.Font.Color);
            }
        }

        public override void RoundRectangle(System.Drawing.Rectangle r, int roundWidth, int roundHeight)
        {
            this.InternalRect(base.Brush, r, true, true);
        }

        public override void ShowImage()
        {
            base.AddToStream("</svg>");
        }

        private string SVGBrushPen(bool usePen)
        {
            if (base.Brush.Visible)
            {
                this.m_string = " fill=" + this.SVGColor(base.Brush.Color);
                float num = base.Brush.Transparency * 0.01f;
                if (num > 0f)
                {
                    string str = num.ToString().Replace(',', '.');
                    this.m_string = this.m_string + " fill-opacity=\"" + str + "\"";
                }
            }
            else
            {
                this.m_string = " fill=\"none\"";
            }
            if (usePen)
            {
                this.m_string = this.m_string + this.SVGPen();
            }
            return this.m_string;
        }

        private void SVGClip()
        {
            this.iClipStack++;
            this.iClipCount++;
            string str = "Clip" + this.iClipCount.ToString();
            base.AddToStream("<g clip-path=\"url(#" + str + ")\">");
            base.AddToStream("<defs>");
            base.AddToStream("<clipPath id=\"" + str + "\" style=\"clip-rule:nonzero\">");
        }

        private string SVGColor(Color c)
        {
            this.m_string = "";
            if (c == Color.Black)
            {
                this.m_string = "\"black\"";
            }
            else if (c == Color.Silver)
            {
                this.m_string = "\"silver\"";
            }
            else if (c == Color.Gray)
            {
                this.m_string = "\"gray\"";
            }
            else if (c == Color.White)
            {
                this.m_string = "\"white\"";
            }
            else if (c == Color.Maroon)
            {
                this.m_string = "\"maroon\"";
            }
            else if (c == Color.Red)
            {
                this.m_string = "\"red\"";
            }
            else if (c == Color.Purple)
            {
                this.m_string = "\"purple\"";
            }
            else if (c == Color.Fuchsia)
            {
                this.m_string = "\"fuchsia\"";
            }
            else if (c == Color.Green)
            {
                this.m_string = "\"green\"";
            }
            else if (c == Color.Lime)
            {
                this.m_string = "\"lime\"";
            }
            else if (c == Color.Olive)
            {
                this.m_string = "\"olive\"";
            }
            else if (c == Color.Yellow)
            {
                this.m_string = "\"yellow\"";
            }
            else if (c == Color.Navy)
            {
                this.m_string = "\"navy\"";
            }
            else if (c == Color.Blue)
            {
                this.m_string = "\"blue\"";
            }
            else if (c == Color.Teal)
            {
                this.m_string = "\"teal\"";
            }
            else if (c == Color.Aqua)
            {
                this.m_string = "\"aqua\"";
            }
            else
            {
                this.m_string = this.m_string + "\"rgb(" + c.R.ToString() + ",";
                this.m_string = this.m_string + c.G.ToString() + ",";
                this.m_string = this.m_string + c.B.ToString() + ")\"";
            }
            return this.m_string;
        }

        private string SVGEllipse(int x1, int y1, int x2, int y2)
        {
            int num = (x1 + x2) / 2;
            int num2 = (y1 + y2) / 2;
            int num3 = (x2 - x1) / 2;
            int num4 = (y2 - y1) / 2;
            this.m_string = "cx=\"" + num.ToString() + "\" cy=\"" + num2.ToString();
            string str = this.m_string;
            this.m_string = str + "\" rx=\"" + num3.ToString() + "\" ry=\"" + num4.ToString() + "\"";
            return this.m_string;
        }

        private void SVGEndClip()
        {
            base.AddToStream("</clipPath>");
            base.AddToStream("</defs>");
        }

        private string SVGPen()
        {
            if (base.Pen.Visible)
            {
                this.m_string = " stroke=" + this.SVGColor(base.Pen.Color);
                if (base.Pen.Width > 1)
                {
                    this.m_string = this.m_string + " stroke-width=\"" + base.Pen.Width.ToString() + "\"";
                }
                if (base.Pen.Style != DashStyle.Solid)
                {
                    this.m_string = this.m_string + " stroke-dasharray=\"" + this.PenStyle(base.Pen) + "\" ";
                }
                if (base.Pen.EndCap == LineCap.Square)
                {
                    this.m_string = this.m_string + " stroke-linecap=\"square\"";
                }
                else if (base.Pen.EndCap == LineCap.Flat)
                {
                    this.m_string = this.m_string + " stroke-linecap=\"flat\"";
                }
            }
            else
            {
                this.m_string = " stroke=\"none\"";
            }
            return this.m_string;
        }

        private string SVGPoints(params Point[] p)
        {
            this.m_string = "points=\"";
            for (int i = p.GetLowerBound(0); i <= p.GetUpperBound(0); i++)
            {
                this.m_string = this.m_string + this.PointToStr(p[i].X, p[i].Y) + " ";
            }
            this.m_string = this.m_string + "\"";
            return this.m_string;
        }

        private string SVGRect(System.Drawing.Rectangle r)
        {
            int width = r.Width;
            int height = r.Height;
            this.m_string = "x=\"" + r.Left.ToString() + "\" y=\"" + r.Top.ToString() + "\" ";
            string str = this.m_string;
            this.m_string = str + " width=\"" + width.ToString() + "\" height=\"" + height.ToString() + "\"";
            return this.m_string;
        }

        private string TheBounds()
        {
            this.m_string = "width=\"" + base.chart.ChartBounds.Width.ToString() + "px\" ";
            this.m_string = this.m_string + "height=\"" + base.chart.ChartBounds.Height.ToString() + "px\"";
            return this.m_string;
        }

        internal override void TransparentEllipse(int x1, int y1, int x2, int y2)
        {
        }

        public override void UnClip()
        {
            if (this.iClipStack > 0)
            {
                this.iClipStack--;
                base.AddToStream("</g>");
            }
        }

        public override void VerticalLine(int x, int top, int bottom)
        {
            this.MoveTo(x, bottom);
            this.LineTo(x, top);
        }
    }
}

