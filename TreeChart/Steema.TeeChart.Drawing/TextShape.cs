namespace Steema.TeeChart.Drawing
{
    using Steema.TeeChart;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    public class TextShape : Shape
    {
        protected internal string defaultText;
        protected internal bool drawText;
        private ChartFont font;
        private string[] lines;
        private TextShapeStyle shapeStyle;
        public ShapeSize Size;

        public TextShape() : this(null)
        {
        }

        public TextShape(Chart c) : base(c)
        {
            this.Size = new ShapeSize();
            this.shapeStyle = TextShapeStyle.Rectangle;
            this.drawText = true;
            this.defaultText = "";
        }

        [Description("Assign all properties from a TextShape to another.")]
        public void Assign(TextShape s)
        {
            if (s != null)
            {
                base.Assign(s);
                if (s.font != null)
                {
                    this.Font.Assign(s.font);
                }
                this.lines = s.lines;
                this.shapeStyle = s.shapeStyle;
                this.Size = s.Size;
            }
        }

        public void DrawRectRotated(Graphics3D g, Rectangle rect, int angle, int aZ)
        {
            if (!base.bTransparent)
            {
                if ((base.Shadow.bVisible && base.bBrush.visible) && !g.SupportsFullRotation)
                {
                    base.shadow.Draw(g, rect, angle, aZ);
                }
                if (base.Gradient.Visible && (angle == 0))
                {
                    base.Gradient.Draw(g, rect);
                    g.Brush.Visible = false;
                }
                else
                {
                    g.Brush = base.bBrush;
                }
                g.Pen = base.Pen;
                this.InternalDrawShape(g, rect, 0x10, angle, aZ);
            }
            if (base.bBevel != null)
            {
                base.bBevel.Draw(g, rect);
            }
        }

        private void InternalDrawShape(Graphics3D g, Rectangle aRect, int teeDefaultRoundSize, int angle, int aZ)
        {
            if (angle > 0)
            {
                g.Polygon(aZ, Graphics3D.RotateRectangle(aRect, angle));
            }
            else if (g.SupportsFullRotation)
            {
                g.Rectangle(aRect, aZ);
            }
            else if (this.shapeStyle == TextShapeStyle.Rectangle)
            {
                g.Rectangle(aRect);
            }
            else
            {
                g.RoundRectangle(aRect);
            }
        }

        public void Paint()
        {
            this.Paint(base.chart.graphics3D, base.ShapeBounds);
        }

        public override void Paint(Graphics3D g, Rectangle rect)
        {
            if (this.drawText && (this.lines != null))
            {
                g.Font = this.font;
                System.Drawing.Size size = g.MeasureString(g.Font, this.Text).ToSize();
                this.Size.Width = size.Width;
                this.Size.Height = size.Height;
                int right = rect.Right;
                int bottom = rect.Bottom;
                rect.Width = this.Size.Width;
                rect.Height = this.Size.Height;
                rect.X = ((rect.X + right) / 2) - this.Size.Width;
            }
            if (base.bBevel != null)
            {
                if (base.bBevel.inner != BevelStyles.None)
                {
                    rect.Inflate(1, 1);
                }
                if (base.bBevel.outer != BevelStyles.None)
                {
                    rect.Inflate(1, 1);
                }
            }
            switch (this.shapeStyle)
            {
                case TextShapeStyle.Rectangle:
                    base.BorderRound = 0;
                    break;

                case TextShapeStyle.RoundRectangle:
                    base.BorderRound = 8;
                    break;
            }
            base.Paint(g, rect);
            if (this.drawText && (this.lines != null))
            {
                g.TextOut(rect.X, rect.Y + 2, this.Text);
            }
        }

        protected override void SetChart(Chart c)
        {
            base.SetChart(c);
            if (this.font != null)
            {
                this.font.Chart = base.chart;
            }
        }

        protected virtual bool ShouldSerializeLines()
        {
            return (this.Text != this.defaultText);
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Determines the font attributes used to output TextShape strings.")]
        public ChartFont Font
        {
            get
            {
                if (this.font == null)
                {
                    this.font = new ChartFont(base.chart);
                }
                return this.font;
            }
        }

        [DefaultValue((string) null)]
        public string[] Lines
        {
            get
            {
                return this.lines;
            }
            set
            {
                this.lines = value;
                this.Invalidate();
            }
        }

        protected int LinesLength
        {
            get
            {
                if (this.lines != null)
                {
                    return this.lines.Length;
                }
                return 0;
            }
        }

        [Obsolete("Please use Shadow.Size property."), DefaultValue(3), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public int ShadowSize
        {
            get
            {
                return base.Shadow.Width;
            }
            set
            {
                base.Shadow.Width = value;
            }
        }

        [DefaultValue(0), Description("Shape may be rectagular or rounded rectangular.")]
        public TextShapeStyle ShapeStyle
        {
            get
            {
                return this.shapeStyle;
            }
            set
            {
                if (this.shapeStyle != value)
                {
                    this.shapeStyle = value;
                    this.Invalidate();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Description("The Text property is used to display customized strings inside Shapes.")]
        public string Text
        {
            get
            {
                if (this.LinesLength <= 0)
                {
                    return "";
                }
                return this.lines[0];
            }
            set
            {
                if (this.Text != value)
                {
                    if ((this.lines == null) || (this.lines.Length == 0))
                    {
                        this.lines = new string[1];
                    }
                    this.lines[0] = value;
                    this.Invalidate();
                }
            }
        }
    }
}

