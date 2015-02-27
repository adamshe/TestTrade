namespace Steema.TeeChart.Drawing
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;

    [Description("Font properties used at several objects."), Editor(typeof(FontComponentEditor), typeof(UITypeEditor))]
    public sealed class ChartFont : TeeBase
    {
        private ChartBrush bBrush;
        private bool bold;
        [NonSerialized]
        private Font font;
        private bool italic;
        private string name;
        internal Steema.TeeChart.Drawing.Shadow shadow;
        private int size;
        private bool strikeout;
        private bool underline;

        public ChartFont(Chart c) : base(c)
        {
            this.font = new Font("Arial", 8f);
            this.name = "Arial";
            this.size = 8;
        }

        internal void Assign(ChartFont f)
        {
            if (f.bBrush == null)
            {
                this.bBrush = null;
            }
            else
            {
                this.Brush.Assign(f.bBrush);
            }
            if (f.shadow == null)
            {
                this.shadow = null;
            }
            else
            {
                this.Shadow.Assign(f.shadow);
            }
            this.bold = f.bold;
            this.strikeout = f.strikeout;
            this.underline = f.underline;
            this.italic = f.italic;
            this.name = f.name;
            this.size = f.size;
            this.font = null;
            this.Changed();
        }

        private void Changed()
        {
            if (base.chart != null)
            {
                base.chart.graphics3D.Changed(base.chart.graphics3D.Font);
            }
        }

        public override void Invalidate()
        {
            base.Invalidate();
            if (this.font != null)
            {
                this.font.Dispose();
                this.font = null;
            }
            this.Changed();
        }

        protected override void SetChart(Chart c)
        {
            base.SetChart(c);
            if (this.shadow != null)
            {
                this.shadow.chart = base.chart;
            }
            if (this.bBrush != null)
            {
                this.bBrush.chart = base.chart;
            }
        }

        public bool ShouldDrawShadow()
        {
            if ((this.shadow == null) || !this.shadow.bVisible)
            {
                return false;
            }
            if (this.shadow.Width == 0)
            {
                return (this.shadow.Height != 0);
            }
            return true;
        }

        [Description("Sets Font bold for text."), DefaultValue(false)]
        public bool Bold
        {
            get
            {
                return this.bold;
            }
            set
            {
                base.SetBooleanProperty(ref this.bold, value);
            }
        }

        [Description("Sets the Brush characteristics of the font"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance")]
        public ChartBrush Brush
        {
            get
            {
                if (this.bBrush == null)
                {
                    this.bBrush = new ChartBrush(base.chart, System.Drawing.Color.Black);
                }
                return this.bBrush;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Description("Defines a Font colour for text.")]
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

        public Font DrawingFont
        {
            get
            {
                if (this.font == null)
                {
                    FontStyle regular = FontStyle.Regular;
                    if (this.bold)
                    {
                        regular |= FontStyle.Bold;
                    }
                    if (this.italic)
                    {
                        regular |= FontStyle.Italic;
                    }
                    if (this.underline)
                    {
                        regular |= FontStyle.Underline;
                    }
                    if (this.strikeout)
                    {
                        regular |= FontStyle.Strikeout;
                    }
                    this.font = new Font(this.name, (float) this.size, regular);
                }
                return this.font;
            }
        }

        [Description("Applies a gradient fill to the font."), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Category("Appearance")]
        public Steema.TeeChart.Drawing.Gradient Gradient
        {
            get
            {
                return this.Brush.Gradient;
            }
        }

        [DefaultValue(false), Description("Sets Font italic (true or false) for text.")]
        public bool Italic
        {
            get
            {
                return this.italic;
            }
            set
            {
                base.SetBooleanProperty(ref this.italic, value);
            }
        }

        [DefaultValue("Arial"), Description("Defines a Font type for text.")]
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                base.SetStringProperty(ref this.name, value);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Accesses the shadow properties of the font.")]
        public Steema.TeeChart.Drawing.Shadow Shadow
        {
            get
            {
                if (this.shadow == null)
                {
                    this.shadow = new Steema.TeeChart.Drawing.Shadow(base.chart, 1);
                }
                return this.shadow;
            }
        }

        [DefaultValue(8), Description("Sets Font sizing (in points) for text.")]
        public int Size
        {
            get
            {
                return this.size;
            }
            set
            {
                base.SetIntegerProperty(ref this.size, value);
            }
        }

        [Description("Sets Font Strikeout on/off."), DefaultValue(false)]
        public bool Strikeout
        {
            get
            {
                return this.strikeout;
            }
            set
            {
                base.SetBooleanProperty(ref this.strikeout, value);
            }
        }

        [DefaultValue(false), Description("Sets Font underline on/off.")]
        public bool Underline
        {
            get
            {
                return this.underline;
            }
            set
            {
                base.SetBooleanProperty(ref this.underline, value);
            }
        }

        internal class FontComponentEditor : UITypeEditor
        {
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                ChartFont font = (ChartFont) value;
                bool flag = EditorUtils.EditFont(font);
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
        }
    }
}

