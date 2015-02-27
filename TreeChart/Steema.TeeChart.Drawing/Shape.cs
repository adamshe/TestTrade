using Steema.TeeChart.Editors.Export;

namespace Steema.TeeChart.Drawing
{
    using Steema.TeeChart;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;

    [Serializable]
    public class Shape : TeeBase
    {
        protected Steema.TeeChart.Drawing.Bevel bBevel;
        protected int bBorderRound;
        protected ChartBrush bBrush;
        protected bool bTransparent;
        protected internal bool defaultVisible;
        protected int iBottom;
        protected int iLeft;
        protected int iRight;
        protected int iTop;
        protected internal ChartPen pPen;
        internal Steema.TeeChart.Drawing.Shadow shadow;
        internal bool visible;

        public Shape() : this(null)
        {
        }

        public Shape(Chart c) : base(c)
        {
            this.defaultVisible = true;
            this.visible = true;
        }

        [Description("Assign all properties from a shape to another.")]
        public void Assign(Shape s)
        {
            if (s != null)
            {
                if (s.bBevel != null)
                {
                    this.Bevel.Assign(s.bBevel);
                }
                if (s.bBrush != null)
                {
                    this.Brush.Assign(s.bBrush);
                }
                this.Left = s.Left;
                this.Right = s.Right;
                this.Top = s.Top;
                this.Bottom = s.Bottom;
                if (s.pPen != null)
                {
                    this.Pen.Assign(s.pPen);
                }
                if (s.shadow != null)
                {
                    this.Shadow.Assign(s.shadow);
                }
                this.Visible = s.Visible;
                this.Transparent = s.Transparent;
            }
        }

        public virtual void Paint(Graphics3D g, Rectangle rect)
        {
            if ((this.shadow != null) && this.shadow.Visible)
            {
                Rectangle rectangle = rect;
                if (this.Pen.bVisible)
                {
                    rectangle.Inflate(this.pPen.Width, this.pPen.Width);
                }
                this.shadow.Draw(g, rectangle);
            }
            if (!this.bTransparent)
            {
                g.Brush = this.Brush;
                g.Pen = this.Pen;
                if (this.BorderRound > 0)
                {
                    g.RoundRectangle(rect, this.BorderRound, this.BorderRound);
                }
                else
                {
                    g.Rectangle(rect);
                }
                if (this.Pen.Visible)
                {
                    rect.Inflate(-this.Pen.Width, -this.Pen.Width);
                }
                if (this.bBevel != null)
                {
                    this.bBevel.Draw(g, rect);
                }
            }
        }

        protected override void SetChart(Chart c)
        {
            base.SetChart(c);
            if (this.pPen != null)
            {
                this.pPen.Chart = base.chart;
            }
            if (this.shadow != null)
            {
                this.shadow.Chart = base.chart;
            }
            if (this.bBrush != null)
            {
                this.bBrush.Chart = base.chart;
            }
            if (this.bBevel != null)
            {
                this.bBevel.Chart = base.chart;
            }
        }

        protected virtual bool ShouldSerializeBottom()
        {
            return false;
        }

        protected virtual bool ShouldSerializeLeft()
        {
            return false;
        }

        protected virtual bool ShouldSerializeRight()
        {
            return false;
        }

        protected virtual bool ShouldSerializeTop()
        {
            return false;
        }

        protected virtual bool ShouldSerializeTransparent()
        {
            return this.bTransparent;
        }

        protected virtual bool ShouldSerializeVisible()
        {
            return (this.visible != this.defaultVisible);
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Sets the bevel characteristics of the Shape.")]
        public Steema.TeeChart.Drawing.Bevel Bevel
        {
            get
            {
                if (this.bBevel == null)
                {
                    this.bBevel = new Steema.TeeChart.Drawing.Bevel(base.chart);
                }
                return this.bBevel;
            }
        }

        [Obsolete("Please use Bevel.Inner property."), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DefaultValue(0)]
        public BevelStyles BevelInner
        {
            get
            {
                return this.Bevel.Inner;
            }
            set
            {
                this.Bevel.Inner = value;
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), Obsolete("Please use Bevel.Outer property."), DefaultValue(0), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public BevelStyles BevelOuter
        {
            get
            {
                return this.Bevel.Outer;
            }
            set
            {
                this.Bevel.Outer = value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false), Obsolete("Please use Bevel.Width property."), DefaultValue(1), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int BevelWidth
        {
            get
            {
                return this.Bevel.Width;
            }
            set
            {
                this.Bevel.Width = value;
            }
        }

        [Description("Rounds the Border of the Chart Panel."), DefaultValue(0)]
        public int BorderRound
        {
            get
            {
                return this.bBorderRound;
            }
            set
            {
                base.SetIntegerProperty(ref this.bBorderRound, value);
            }
        }

        [Browsable(false)]
        public int Bottom
        {
            get
            {
                return this.iBottom;
            }
            set
            {
                base.SetIntegerProperty(ref this.iBottom, value);
            }
        }

        [Description("Defines the kind of brush used to fill shape background."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ChartBrush Brush
        {
            get
            {
                if (this.bBrush == null)
                {
                    this.bBrush = new ChartBrush(base.chart);
                }
                return this.bBrush;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Description("Defines the shape Color.")]
        public System.Drawing.Color Color
        {
            get
            {
                return this.Brush.color;
            }
            set
            {
                this.Brush.Color = value;
                if (this.Transparency != 0)
                {
                    this.Brush.Color = Graphics3D.TransparentColor(this.Transparency, value);
                    if (base.chart.parent != null)
                    {
                        base.chart.parent.DoSetControlStyle();
                    }
                }
                this.bBrush.Solid = true;
            }
        }

        [Description("Calls the  Gradient characteristics for the shape."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Steema.TeeChart.Drawing.Gradient Gradient
        {
            get
            {
                return this.Brush.Gradient;
            }
        }

        [Editor(typeof(BitmapEditor), typeof(UITypeEditor)), DefaultValue((string) null)]
        public System.Drawing.Image Image
        {
            get
            {
                return this.Brush.Image;
            }
            set
            {
                this.Brush.Image = value;
            }
        }

        [Description("Gets or sets how Image will be displayed."), DefaultValue(1)]
        public Steema.TeeChart.Drawing.ImageMode ImageMode
        {
            get
            {
                return this.Brush.ImageMode;
            }
            set
            {
                this.Brush.ImageMode = value;
            }
        }

        [Description("Sets the shape image to transparent."), DefaultValue(false)]
        public bool ImageTransparent
        {
            get
            {
                return this.Brush.ImageTransparent;
            }
            set
            {
                this.Brush.ImageTransparent = value;
            }
        }

        [Browsable(false)]
        public int Left
        {
            get
            {
                return this.iLeft;
            }
            set
            {
                base.SetIntegerProperty(ref this.iLeft, value);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Specifies the pen used to draw the shape."), Category("Appearance")]
        public ChartPen Pen
        {
            get
            {
                if (this.pPen == null)
                {
                    this.pPen = new ChartPen(base.chart, System.Drawing.Color.Black);
                }
                return this.pPen;
            }
        }

        [Browsable(false)]
        public int Right
        {
            get
            {
                return this.iRight;
            }
            set
            {
                base.SetIntegerProperty(ref this.iRight, value);
            }
        }

        [Description("Defines the shape's Shadow characteristics."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Steema.TeeChart.Drawing.Shadow Shadow
        {
            get
            {
                if (this.shadow == null)
                {
                    this.shadow = new Steema.TeeChart.Drawing.Shadow(base.chart);
                }
                return this.shadow;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Description("Defines the boundaries of the Shape."), Browsable(false)]
        public Rectangle ShapeBounds
        {
            get
            {
                return Rectangle.FromLTRB(this.iLeft, this.iTop, this.iRight, this.iBottom);
            }
            set
            {
                this.iLeft = value.X;
                this.iTop = value.Y;
                this.iRight = value.Right;
                this.iBottom = value.Bottom;
            }
        }

        [Browsable(false)]
        public int Top
        {
            get
            {
                return this.iTop;
            }
            set
            {
                base.SetIntegerProperty(ref this.iTop, value);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(0), Description("Sets Transparency level from 0 to 100%."), Category("Appearance")]
        public int Transparency
        {
            get
            {
                return this.Brush.Transparency;
            }
            set
            {
                this.bBrush.Transparency = value;
            }
        }

        [Description("Enables/disables transparency of shape.")]
        public bool Transparent
        {
            get
            {
                return this.bTransparent;
            }
            set
            {
                base.SetBooleanProperty(ref this.bTransparent, value);
            }
        }

        [Description("Shows or hides the Shape.")]
        public bool Visible
        {
            get
            {
                return this.visible;
            }
            set
            {
                base.SetBooleanProperty(ref this.visible, value);
            }
        }
    }
}

