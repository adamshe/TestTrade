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

    [Serializable, Description("Pen used to draw lines and borders."), Editor(typeof(PenComponentEditor), typeof(UITypeEditor))]
    public class ChartPen : TeeBase
    {
        protected internal bool bVisible;
        protected System.Drawing.Color cColor;
        internal EventHandler ColorChanged;
        private System.Drawing.Drawing2D.DashCap dashCap;
        protected internal System.Drawing.Color defaultColor;
        protected internal LineCap defaultEndCap;
        internal DashStyle defaultStyle;
        protected internal bool defaultVisible;
        private LineCap endCap;
        [NonSerialized]
        protected Pen handle;
        private DashStyle style;
        private int transparency;
        protected internal bool usesVisible;
        private int width;

        public ChartPen(Chart c) : this(c, System.Drawing.Color.Empty, true)
        {
        }

        public ChartPen(ChartPen source) : this(source.Chart)
        {
            this.Assign(source);
        }

        public ChartPen(System.Drawing.Color c) : this(null, c)
        {
        }

        public ChartPen(Chart c, bool startVisible) : this(c, System.Drawing.Color.Empty, startVisible)
        {
        }

        public ChartPen(Chart c, System.Drawing.Color startColor) : this(c, startColor, true)
        {
        }

        public ChartPen(Chart c, System.Drawing.Color startColor, bool startVisible) : base(c)
        {
            this.defaultVisible = true;
            this.usesVisible = true;
            this.bVisible = true;
            this.width = 1;
            this.endCap = LineCap.Flat;
            this.defaultEndCap = LineCap.Flat;
            this.dashCap = System.Drawing.Drawing2D.DashCap.Flat;
            this.style = DashStyle.Solid;
            this.defaultStyle = DashStyle.Solid;
            this.defaultColor = startColor;
            this.cColor = this.defaultColor;
            this.defaultVisible = startVisible;
            this.bVisible = this.defaultVisible;
        }

        public ChartPen(Chart c, System.Drawing.Color startColor, bool startVisible, LineCap cap) : this(c, startColor, startVisible)
        {
            this.endCap = cap;
            this.defaultEndCap = cap;
        }

        internal void Assign(ChartPen p)
        {
            this.style = p.style;
            this.width = p.width;
            this.cColor = p.cColor;
            this.endCap = p.endCap;
            this.dashCap = p.dashCap;
            this.bVisible = p.bVisible;
            this.transparency = p.transparency;
            if (this.handle != null)
            {
                this.SetDrawingPen();
            }
            this.Changed();
        }

        internal void Assign(ChartPen p, System.Drawing.Color AColor)
        {
            this.style = p.style;
            this.width = p.width;
            this.cColor = AColor;
            this.endCap = p.endCap;
            this.dashCap = p.dashCap;
            this.bVisible = p.bVisible;
            this.transparency = p.transparency;
            if (this.handle != null)
            {
                this.SetDrawingPen();
            }
            this.Changed();
        }

        private void Changed()
        {
            if ((base.chart != null) && (this == base.chart.graphics3D.Pen))
            {
                base.chart.graphics3D.Changed(base.chart.graphics3D.Pen);
            }
        }

        private System.Drawing.Color GetColor()
        {
            if (this.transparency == 0)
            {
                return this.cColor;
            }
            return Graphics3D.TransparentColor(this.transparency, this.cColor);
        }

        public override void Invalidate()
        {
            base.Invalidate();
            this.Changed();
        }

        private void SetDrawingPen()
        {
            this.handle.Color = this.GetColor();
            this.handle.DashStyle = this.style;
            this.handle.Width = this.width;
            this.handle.EndCap = this.endCap;
            this.handle.DashCap = this.dashCap;
        }

        protected virtual bool ShouldSerializeColor()
        {
            return (this.cColor != this.defaultColor);
        }

        protected bool ShouldSerializeEndCap()
        {
            return (this.endCap != this.defaultEndCap);
        }

        protected virtual bool ShouldSerializeStyle()
        {
            return (this.style != this.defaultStyle);
        }

        protected virtual bool ShouldSerializeVisible()
        {
            return (this.bVisible != this.defaultVisible);
        }

        [Description("Determines the color used by the pen to draw lines on the Drawing.")]
        public System.Drawing.Color Color
        {
            get
            {
                return this.cColor;
            }
            set
            {
                if (this.cColor != value)
                {
                    this.cColor = value;
                    if (this.handle != null)
                    {
                        this.handle.Color = this.GetColor();
                    }
                    if (this.ColorChanged != null)
                    {
                        this.ColorChanged(this, EventArgs.Empty);
                    }
                }
            }
        }

        [Description("Defines segment ending style of dashed lines."), DefaultValue(0)]
        public System.Drawing.Drawing2D.DashCap DashCap
        {
            get
            {
                return this.dashCap;
            }
            set
            {
                if (this.dashCap != value)
                {
                    this.dashCap = value;
                    if (this.handle != null)
                    {
                        this.handle.DashCap = this.dashCap;
                    }
                    this.Invalidate();
                }
            }
        }

        [Browsable(false), Description("Accesses the internal Drawing.Pen object."), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Pen DrawingPen
        {
            get
            {
                if (this.handle == null)
                {
                    this.handle = new Pen(this.GetColor());
                    this.SetDrawingPen();
                }
                return this.handle;
            }
        }

        [Description("Style of line endings.")]
        public LineCap EndCap
        {
            get
            {
                return this.endCap;
            }
            set
            {
                if (this.endCap != value)
                {
                    this.endCap = value;
                    if (this.handle != null)
                    {
                        this.handle.EndCap = this.endCap;
                    }
                    this.Invalidate();
                }
            }
        }

        [Description("Determines the style in which the pen draw lines on the Drawing.")]
        public virtual DashStyle Style
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
                    if (this.handle != null)
                    {
                        this.handle.DashStyle = this.style;
                    }
                    this.Invalidate();
                }
            }
        }

        [Description("Sets Transparency level from 0 to 100%."), DefaultValue(0)]
        public int Transparency
        {
            get
            {
                return this.transparency;
            }
            set
            {
                if (this.transparency != value)
                {
                    this.transparency = value;
                    this.Invalidate();
                }
            }
        }

        [Description("Determines if the pen will draw lines or not.")]
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

        [DefaultValue(1), Description("Determines the width of lines the pen draws.")]
        public virtual int Width
        {
            get
            {
                return this.width;
            }
            set
            {
                if (this.width != value)
                {
                    this.width = value;
                    if (this.handle != null)
                    {
                        this.handle.Width = this.width;
                    }
                    this.Invalidate();
                }
            }
        }

        internal class PenComponentEditor : UITypeEditor
        {
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                using (PenEditor editor = new PenEditor((ChartPen) value))
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
                int num = e.Bounds.Top + (e.Bounds.Height / 2);
                ChartPen pen = (ChartPen) e.Value;
                e.Graphics.DrawLine(pen.DrawingPen, e.Bounds.X, num, e.Bounds.Right - 1, num);
            }
        }
    }
}

