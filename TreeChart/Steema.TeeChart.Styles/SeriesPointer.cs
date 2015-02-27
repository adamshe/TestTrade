namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using Steema.TeeChart.Editors;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;

    [Serializable, Editor(typeof(PointerEditor), typeof(UITypeEditor)), Description("Series Pointer.")]
    public class SeriesPointer : TeeBase
    {
        protected internal bool AllowChangeSize;
        private ChartBrush bBrush;
        protected internal bool bVisible;
        private bool dark3D;
        internal bool defaultVisible;
        private bool draw3D;
        private int horizSize;
        private bool inflate;
        private ChartPen pen;
        private int PXMinus;
        private int PXPlus;
        private int PYMinus;
        private int PYPlus;
        private Steema.TeeChart.Styles.Series series;
        private PointerStyles style;
        private int vertSize;

        public SeriesPointer(Chart c, Steema.TeeChart.Styles.Series s) : base(c)
        {
            this.dark3D = true;
            this.draw3D = true;
            this.inflate = true;
            this.horizSize = 4;
            this.vertSize = 4;
            this.bVisible = true;
            this.defaultVisible = true;
            this.style = PointerStyles.Rectangle;
            this.series = s;
            this.Transparency = 0;
        }

        internal void CalcHorizMargins(ref int LeftMargin, ref int RightMargin)
        {
            if (this.bVisible && this.inflate)
            {
                LeftMargin = Math.Max(LeftMargin, this.horizSize + 1);
                RightMargin = Math.Max(RightMargin, this.horizSize + 1);
            }
        }

        internal void CalcVerticalMargins(ref int TopMargin, ref int BottomMargin)
        {
            if (this.bVisible && this.inflate)
            {
                TopMargin = Math.Max(TopMargin, this.vertSize + 1);
                BottomMargin = Math.Max(BottomMargin, this.vertSize + 1);
            }
        }

        private void DoHorizTriangle3D(Graphics3D g, int DeltaX, int px, int py)
        {
            if (this.draw3D)
            {
                g.Pyramid(false, px + DeltaX, this.PYMinus, px - DeltaX, this.PYPlus, this.StartZ, this.EndZ, this.dark3D);
            }
            else
            {
                g.Triangle(new Point(px + DeltaX, this.PYMinus), new Point(px + DeltaX, this.PYPlus), new Point(px - DeltaX, py), this.StartZ);
            }
        }

        private void DoTriangle3D(Graphics3D g, int DeltaY, int px, int py)
        {
            if (this.draw3D)
            {
                g.Pyramid(true, this.PXMinus, py - DeltaY, this.PXPlus, py + DeltaY, this.StartZ, this.EndZ, this.dark3D);
            }
            else
            {
                g.Triangle(new Point(this.PXMinus, py + DeltaY), new Point(this.PXPlus, py + DeltaY), new Point(px, py - DeltaY), this.StartZ);
            }
        }

        public void Draw(int px, int py, System.Drawing.Color colorValue)
        {
            this.Draw(base.chart.graphics3D, base.chart.Aspect.View3D, px, py, this.horizSize, this.vertSize, colorValue, this.style);
        }

        public void Draw(int px, int py, System.Drawing.Color colorValue, PointerStyles aStyle)
        {
            this.Draw(base.chart.graphics3D, base.chart.Aspect.View3D, px, py, this.horizSize, this.vertSize, colorValue, aStyle);
        }

        public void Draw(Graphics3D g, bool is3D, int px, int py, int tmpHoriz, int tmpVert, System.Drawing.Color colorValue, PointerStyles aStyle)
        {
            g.Brush.Transparency = this.Brush.Transparency;
            g.Brush.Color = Graphics3D.TransparentColor(g.Brush.Transparency, colorValue);
            this.PXMinus = px - tmpHoriz;
            this.PXPlus = px + tmpHoriz;
            this.PYMinus = py - tmpVert;
            this.PYPlus = py + tmpVert;
            if (!is3D)
            {
                Point[] pointArray6;
                switch (aStyle)
                {
                    case PointerStyles.Rectangle:
                        g.Rectangle(this.PXMinus, this.PYMinus, this.PXPlus + 1, this.PYPlus + 1);
                        return;

                    case PointerStyles.Circle:
                        g.Ellipse(this.PXMinus, this.PYMinus, this.PXPlus, this.PYPlus);
                        return;

                    case PointerStyles.Triangle:
                    {
                        pointArray6 = new Point[] { new Point(this.PXMinus, this.PYPlus), new Point(this.PXPlus, this.PYPlus), new Point(px, this.PYMinus) };
                        Point[] p = pointArray6;
                        g.Polygon(p);
                        return;
                    }
                    case PointerStyles.DownTriangle:
                    {
                        pointArray6 = new Point[] { new Point(this.PXMinus, this.PYMinus), new Point(this.PXPlus, this.PYMinus), new Point(px, this.PYPlus) };
                        Point[] pointArray2 = pointArray6;
                        g.Polygon(pointArray2);
                        return;
                    }
                    case PointerStyles.Cross:
                        this.DrawCross(g, px, py, colorValue);
                        return;

                    case PointerStyles.DiagCross:
                        this.DrawDiagonalCross(g, colorValue);
                        return;

                    case PointerStyles.Star:
                        this.DrawCross(g, px, py, colorValue);
                        this.DrawDiagonalCross(g, colorValue);
                        return;

                    case PointerStyles.Diamond:
                    {
                        pointArray6 = new Point[] { new Point(this.PXMinus, py), new Point(px, this.PYMinus), new Point(this.PXPlus, py), new Point(px, this.PYPlus) };
                        Point[] pointArray5 = pointArray6;
                        g.Polygon(pointArray5);
                        return;
                    }
                    case PointerStyles.SmallDot:
                        g.Pixel(px, py, this.MiddleZ, colorValue);
                        return;

                    case PointerStyles.Nothing:
                        return;

                    case PointerStyles.LeftTriangle:
                    {
                        pointArray6 = new Point[] { new Point(this.PXMinus, py), new Point(this.PXPlus, this.PYMinus), new Point(this.PXPlus, this.PYPlus) };
                        Point[] pointArray3 = pointArray6;
                        g.Polygon(pointArray3);
                        return;
                    }
                    case PointerStyles.RightTriangle:
                    {
                        pointArray6 = new Point[] { new Point(this.PXMinus, this.PYMinus), new Point(this.PXMinus, this.PYPlus), new Point(this.PXPlus, py) };
                        Point[] pointArray4 = pointArray6;
                        g.Polygon(pointArray4);
                        return;
                    }
                    case PointerStyles.Sphere:
                        g.SphereEnh(this.PXMinus, this.PYMinus, this.PXPlus, this.PYPlus);
                        return;

                    case PointerStyles.PolishedSphere:
                        g.EllipseEnh(this.PXMinus, this.PYMinus, this.PXPlus, this.PYPlus);
                        return;
                }
            }
            else
            {
                switch (aStyle)
                {
                    case PointerStyles.Rectangle:
                        if (!this.draw3D)
                        {
                            g.Rectangle(this.PXMinus, this.PYMinus, this.PXPlus + 1, this.PYPlus + 1, this.StartZ);
                            return;
                        }
                        g.Cube(this.PXMinus, this.PYMinus, this.PXPlus, this.PYPlus, this.StartZ, this.EndZ, this.dark3D);
                        return;

                    case PointerStyles.Circle:
                        if (!this.draw3D || !g.SupportsFullRotation)
                        {
                            g.Ellipse(this.PXMinus, this.PYMinus, this.PXPlus, this.PYPlus, this.StartZ);
                            return;
                        }
                        g.Sphere(px, py, this.MiddleZ, (double) tmpHoriz);
                        return;

                    case PointerStyles.Triangle:
                        this.DoTriangle3D(g, tmpVert, px, py);
                        return;

                    case PointerStyles.DownTriangle:
                        this.DoTriangle3D(g, -tmpVert, px, py);
                        return;

                    case PointerStyles.Cross:
                        this.DrawCross(g, px, py, colorValue);
                        return;

                    case PointerStyles.DiagCross:
                        this.DrawDiagonalCross(g, colorValue);
                        return;

                    case PointerStyles.Star:
                        this.DrawCross(g, px, py, colorValue);
                        this.DrawDiagonalCross(g, colorValue);
                        return;

                    case PointerStyles.Diamond:
                        g.Plane(new Point(this.PXMinus, py), new Point(px, this.PYMinus), new Point(this.PXPlus, py), new Point(px, this.PYPlus), this.StartZ);
                        return;

                    case PointerStyles.SmallDot:
                        g.Pixel(px, py, this.MiddleZ, colorValue);
                        return;

                    case PointerStyles.Nothing:
                        return;

                    case PointerStyles.LeftTriangle:
                        this.DoHorizTriangle3D(g, tmpHoriz, px, py);
                        return;

                    case PointerStyles.RightTriangle:
                        this.DoHorizTriangle3D(g, -tmpHoriz, px, py);
                        return;

                    case PointerStyles.Sphere:
                        g.SphereEnh(this.PXMinus, this.PYMinus, this.PXPlus, this.PYPlus, this.StartZ);
                        return;

                    case PointerStyles.PolishedSphere:
                        g.EllipseEnh(this.PXMinus, this.PYMinus, this.PXPlus, this.PYPlus, this.StartZ);
                        return;

                    default:
                        return;
                }
            }
        }

        private void DrawCross(Graphics3D g, int px, int py, System.Drawing.Color ColorValue)
        {
            g.VerticalLine(px, this.PYMinus, this.PYPlus + 1, this.StartZ);
            g.HorizontalLine(this.PXMinus, this.PXPlus + 1, py, this.StartZ);
        }

        private void DrawDiagonalCross(Graphics3D g, System.Drawing.Color ColorValue)
        {
            g.Line(this.PXMinus, this.PYMinus, this.PXPlus + 1, this.PYPlus + 1, this.StartZ);
            g.Line(this.PXPlus, this.PYMinus, this.PXMinus - 1, this.PYPlus + 1, this.StartZ);
        }

        internal void DrawLegendShape(System.Drawing.Color color, Rectangle rect, bool drawPen)
        {
            this.DrawLegendShape(base.chart.graphics3D, color, rect, drawPen);
        }

        internal void DrawLegendShape(Graphics3D g, System.Drawing.Color color, Rectangle rect, bool drawPen)
        {
            int num;
            int num2;
            this.PrepareCanvas(g, color);
            if (drawPen)
            {
                num = rect.Width / 3;
                num2 = rect.Height / 3;
            }
            else
            {
                num = 1 + (rect.Width / 2);
                num2 = 1 + (rect.Height / 2);
            }
            this.Draw(g, false, (rect.X + rect.Right) / 2, (rect.Y + rect.Bottom) / 2, Math.Min(this.horizSize, num), Math.Min(this.vertSize, num2), color, this.style);
        }

        internal void PrepareCanvas(Graphics3D g, System.Drawing.Color colorValue)
        {
            g.Pen = this.Pen;
            if (this.pen.Color.IsEmpty || (colorValue == System.Drawing.Color.Transparent))
            {
                g.Pen.Color = colorValue;
            }
            g.Brush = this.Brush;
            if (this.bBrush.Color.IsEmpty)
            {
                g.Brush.ForegroundColor = this.bBrush.Solid ? colorValue : System.Drawing.Color.Black;
                g.Brush.Color = colorValue;
            }
            else if (this.series != null)
            {
                if (this.series.ColorEach || (colorValue == System.Drawing.Color.Transparent))
                {
                    g.Brush.Color = colorValue;
                }
                else
                {
                    g.Brush.Color = this.bBrush.Color;
                }
            }
            else
            {
                g.Brush.Color = colorValue;
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

        protected virtual bool ShouldSerializeVisible()
        {
            return (this.bVisible != this.defaultVisible);
        }

        [Description("Brush used to fill Series Pointers."), Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ChartBrush Brush
        {
            get
            {
                if (this.bBrush == null)
                {
                    this.bBrush = new ChartBrush(base.chart);
                    this.bBrush.Transparency = 0;
                    if (this.series != null)
                    {
                        this.bBrush.Color = this.series.Color;
                    }
                }
                return this.bBrush;
            }
        }

        [Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Description("Sets the pointer color.")]
        public System.Drawing.Color Color
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

        [DefaultValue(true), Description("Fills pointer sides in 3D mode with darker color."), Category("Appearance")]
        public bool Dark3D
        {
            get
            {
                return this.dark3D;
            }
            set
            {
                base.SetBooleanProperty(ref this.dark3D, value);
            }
        }

        [Description("Draws pointer in 3D mode."), DefaultValue(true)]
        public bool Draw3D
        {
            get
            {
                return this.draw3D;
            }
            set
            {
                base.SetBooleanProperty(ref this.draw3D, value);
            }
        }

        [Browsable(false), Description(""), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int EndZ
        {
            get
            {
                if (this.series != null)
                {
                    return this.series.EndZ;
                }
                return 0;
            }
        }

        [Category("Appearance"), Description("Configures Gradient filling attributes."), DefaultValue((string) null), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Steema.TeeChart.Drawing.Gradient Gradient
        {
            get
            {
                return this.bBrush.Gradient;
            }
        }

        [DefaultValue(4), Description("Horizontal size of pointer in pixels.")]
        public int HorizSize
        {
            get
            {
                return this.horizSize;
            }
            set
            {
                base.SetIntegerProperty(ref this.horizSize, value);
            }
        }

        [Description("Expands axes to fit pointers."), DefaultValue(true)]
        public bool InflateMargins
        {
            get
            {
                return this.inflate;
            }
            set
            {
                base.SetBooleanProperty(ref this.inflate, value);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), Description("")]
        public int MiddleZ
        {
            get
            {
                if (this.series != null)
                {
                    return this.series.MiddleZ;
                }
                return 0;
            }
        }

        [Category("Appearance"), Description("Pen used to draw a frame around Series Pointers."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ChartPen Pen
        {
            get
            {
                if (this.pen == null)
                {
                    this.pen = new ChartPen(base.chart, System.Drawing.Color.Black);
                }
                return this.pen;
            }
        }

        [Browsable(false)]
        public Steema.TeeChart.Styles.Series Series
        {
            get
            {
                return this.series;
            }
        }

        [Description(""), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public int StartZ
        {
            get
            {
                if (this.series != null)
                {
                    return this.series.StartZ;
                }
                return 0;
            }
        }

        [Description("Pointer style.")]
        public PointerStyles Style
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
                    this.Invalidate();
                }
            }
        }

        [Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Description("Sets Transparency level from 0 to 100%."), DefaultValue(0)]
        public int Transparency
        {
            get
            {
                return this.Brush.Transparency;
            }
            set
            {
                this.Brush.Transparency = value;
            }
        }

        [DefaultValue(4), Description("Horizontal size of pointer in pixels.")]
        public int VertSize
        {
            get
            {
                return this.vertSize;
            }
            set
            {
                base.SetIntegerProperty(ref this.vertSize, value);
            }
        }

        [Description("Shows or hides the pointer.")]
        public bool Visible
        {
            get
            {
                return this.bVisible;
            }
            set
            {
                base.SetBooleanProperty(ref this.bVisible, value);
            }
        }

        internal sealed class PointerEditor : UITypeEditor
        {
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                bool flag = EditorUtils.ShowFormModal(new Steema.TeeChart.Editors.SeriesPointer((Steema.TeeChart.Styles.SeriesPointer) value));
                if ((context != null) && flag)
                {
                    context.OnComponentChanged();
                }
                return value;
            }

            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.Modal;
            }

            public override bool GetPaintValueSupported(ITypeDescriptorContext context)
            {
                return true;
            }

            public override void PaintValue(PaintValueEventArgs e)
            {
                Chart chart;
                base.PaintValue(e);
                Steema.TeeChart.Styles.SeriesPointer pointer = (Steema.TeeChart.Styles.SeriesPointer) e.Value;
                if (pointer.chart == null)
                {
                    chart = new Chart();
                    chart.AutoRepaint = false;
                    pointer.Chart = chart;
                }
                else
                {
                    chart = null;
                }
                Graphics3DGdiPlus g = new Graphics3DGdiPlus(pointer.chart);
                g.g = e.Graphics;
                int px = e.Bounds.X + (e.Bounds.Width / 2);
                int py = e.Bounds.Y + (e.Bounds.Height / 2);
                int tmpHoriz = Math.Min(e.Bounds.Width, e.Bounds.Height) / 2;
                pointer.Draw(g, pointer.draw3D, px, py, tmpHoriz, tmpHoriz, pointer.Color, pointer.style);
                if (pointer.chart == chart)
                {
                    pointer.Chart = null;
                    chart.Dispose();
                }
            }
        }
    }
}

