namespace Steema.TeeChart.Drawing
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    [Description("Brush (pattern) used to fill polygons and rectangles."), Editor(typeof(BrushComponentEditor), typeof(UITypeEditor))]
    public sealed class ChartBrush : TeeBase
    {
        internal System.Drawing.Color color;
        internal System.Drawing.Color defaultColor;
        internal bool defaultVisible;
        private System.Drawing.Color foregroundColor;
        private Steema.TeeChart.Drawing.Gradient gradient;
        [NonSerialized]
        private Brush handle;
        private System.Drawing.Image image;
        private Steema.TeeChart.Drawing.ImageMode imageMode;
        private bool imageTransparent;
        internal int internalTransparency;
        private bool solid;
        private HatchStyle style;
        internal bool visible;
        private System.Drawing.Drawing2D.WrapMode wrapMode;

        public ChartBrush(Chart c) : this(c, System.Drawing.Color.White, true)
        {
        }

        public ChartBrush(Chart c, bool startVisible) : this(c, System.Drawing.Color.White, startVisible)
        {
        }

        public ChartBrush(Chart c, System.Drawing.Color aColor) : this(c, aColor, true)
        {
        }

        public ChartBrush(Chart c, System.Drawing.Color aColor, bool startVisible) : base(c)
        {
            this.foregroundColor = System.Drawing.Color.Silver;
            this.color = System.Drawing.Color.White;
            this.defaultColor = System.Drawing.Color.White;
            this.visible = true;
            this.defaultVisible = true;
            this.style = HatchStyle.BackwardDiagonal;
            this.solid = true;
            this.wrapMode = System.Drawing.Drawing2D.WrapMode.Tile;
            this.imageMode = Steema.TeeChart.Drawing.ImageMode.Stretch;
            this.internalTransparency = 0;
            this.color = aColor;
            this.defaultColor = this.color;
            this.visible = startVisible;
            this.defaultVisible = this.visible;
        }

        internal void ApplyDark(byte quantity)
        {
            Graphics3D.ApplyDark(ref this.color, quantity);
            this.SetNullHandle();
        }

        internal void ApplyDark(System.Drawing.Color c, byte quantity)
        {
            this.color = c;
            this.ApplyDark(quantity);
        }

        internal void Assign(ChartBrush b)
        {
            this.foregroundColor = b.foregroundColor;
            this.color = b.color;
            this.visible = b.visible;
            this.style = b.style;
            this.image = b.image;
            this.imageTransparent = b.imageTransparent;
            this.solid = b.solid;
            this.wrapMode = b.wrapMode;
            this.imageMode = b.imageMode;
            if (b.gradient != null)
            {
                this.Gradient.Assign(b.gradient);
            }
            else
            {
                this.gradient = null;
            }
            this.SetNullHandle();
        }

        public void ClearImage()
        {
            if (this.image != null)
            {
                this.image.Dispose();
            }
        }

        public override void Invalidate()
        {
            this.SetNullHandle();
            base.Invalidate();
        }

        public void LoadImage(string fileName)
        {
            this.ClearImage();
            this.image = System.Drawing.Image.FromFile(fileName);
        }

        protected override void SetChart(Chart c)
        {
            base.SetChart(c);
            if (this.gradient != null)
            {
                this.gradient.Chart = base.chart;
            }
        }

        private void SetNullHandle()
        {
            this.handle = null;
            if ((base.chart != null) && (this == base.chart.graphics3D.Brush))
            {
                base.chart.graphics3D.Changed(base.chart.graphics3D.Brush);
            }
        }

        private bool ShouldSerializeColor()
        {
            return (this.color != this.defaultColor);
        }

        private bool ShouldSerializeVisible()
        {
            return (this.visible != this.defaultVisible);
        }

        [Description("Determines the color used to fill a zone.")]
        public System.Drawing.Color Color
        {
            get
            {
                return this.color;
            }
            set
            {
                base.SetColorProperty(ref this.color, value);
            }
        }

        [Browsable(false), Description("Access the internal Drawing.Brush"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Brush DrawingBrush
        {
            get
            {
                if (this.handle == null)
                {
                    if ((this.image != null) && (this.imageMode == Steema.TeeChart.Drawing.ImageMode.Tile))
                    {
                        this.handle = new TextureBrush(this.image, this.wrapMode);
                    }
                    else if (this.solid)
                    {
                        this.handle = new SolidBrush(this.color);
                    }
                    else
                    {
                        this.handle = new HatchBrush(this.style, this.foregroundColor, this.color);
                    }
                }
                return this.handle;
            }
        }

        [Description("Color to fill inner portions of Brush, when Solid is false."), DefaultValue(typeof(System.Drawing.Color), "Silver")]
        public System.Drawing.Color ForegroundColor
        {
            get
            {
                return this.foregroundColor;
            }
            set
            {
                base.SetColorProperty(ref this.foregroundColor, value);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), DefaultValue((string) null), Description("Fill Gradient."), Category("Appearance")]
        public Steema.TeeChart.Drawing.Gradient Gradient
        {
            get
            {
                if (this.gradient == null)
                {
                    this.gradient = new Steema.TeeChart.Drawing.Gradient(base.chart);
                }
                return this.gradient;
            }
            set
            {
                if (value == null)
                {
                    this.gradient = null;
                }
                else
                {
                    this.Gradient.Assign(value);
                }
            }
        }

        internal bool GradientVisible
        {
            get
            {
                return ((this.gradient != null) && this.gradient.visible);
            }
        }

        [Description("Image to use for fill."), DefaultValue((string) null)]
        public System.Drawing.Image Image
        {
            get
            {
                return this.image;
            }
            set
            {
                this.image = value;
                this.solid = this.image == null;
                this.Invalidate();
            }
        }

        [Description("Style of drawing brush Image."), DefaultValue(typeof(Steema.TeeChart.Drawing.ImageMode), "Stretch")]
        public Steema.TeeChart.Drawing.ImageMode ImageMode
        {
            get
            {
                return this.imageMode;
            }
            set
            {
                if (this.imageMode != value)
                {
                    this.imageMode = value;
                    this.Invalidate();
                }
            }
        }

        [DefaultValue(false), Description("Sets the Brush image to Transparent.")]
        public bool ImageTransparent
        {
            get
            {
                return this.imageTransparent;
            }
            set
            {
                base.SetBooleanProperty(ref this.imageTransparent, value);
            }
        }

        [Description("Fills using Color only. Does not use Foreground color."), DefaultValue(true)]
        public bool Solid
        {
            get
            {
                return this.solid;
            }
            set
            {
                base.SetBooleanProperty(ref this.solid, value);
            }
        }

        [DefaultValue(3), Description("Determines the style in which the zone is filled or patterned using both Color and ForegroundColor.")]
        public HatchStyle Style
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
                    this.solid = false;
                    this.Invalidate();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(0), Description("Sets Transparency level from 0 to 100%.")]
        public int Transparency
        {
            get
            {
                return Graphics3D.Transparency(this.color);
            }
            set
            {
                if (this.internalTransparency != value)
                {
                    this.internalTransparency = value;
                }
                this.Color = Graphics3D.TransparentColor(value, this.color);
                if (this.gradient != null)
                {
                    this.gradient.Transparency = value;
                }
            }
        }

        [Description("Empty of Brush when False.")]
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

        [Description("Style of drawing brush Image."), DefaultValue(typeof(System.Drawing.Drawing2D.WrapMode), "Tile")]
        public System.Drawing.Drawing2D.WrapMode WrapMode
        {
            get
            {
                return this.wrapMode;
            }
            set
            {
                if (this.wrapMode != value)
                {
                    this.wrapMode = value;
                    this.Invalidate();
                }
            }
        }

        internal class BrushComponentEditor : UITypeEditor
        {
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                using (BrushEditor editor = new BrushEditor((ChartBrush) value))
                {
                    bool flag = editor.ShowDialog() == DialogResult.OK;
                    if ((context != null) && flag)
                    {
                        context.OnComponentChanged();
                    }
                    return flag;
                }
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
                base.PaintValue(e);
                e.Graphics.FillRectangle(((ChartBrush) e.Value).DrawingBrush, e.Bounds);
            }
        }
    }
}

