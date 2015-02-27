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

    [Description("Properties to configure a gradient filling."), Editor(typeof(GradientComponentEditor), typeof(UITypeEditor))]
    public class Gradient : TeeBase
    {
        private double angle;
        private LinearGradientMode direction;
        private Color endColor;
        private bool gammaCorrection;
        private Color middleColor;
        public bool Polygonal;
        private bool sigma;
        private float sigmaFocus;
        private float sigmaScale;
        private Color startColor;
        private int transparency;
        private bool useMiddle;
        internal bool visible;
        private System.Drawing.Drawing2D.WrapMode wrapMode;

        public Gradient() : this(null)
        {
        }

        public Gradient(Chart c) : base(c)
        {
            this.direction = LinearGradientMode.Vertical;
            this.startColor = Color.Gold;
            this.middleColor = Color.Gray;
            this.endColor = Color.White;
            this.sigmaFocus = 0.5f;
            this.sigmaScale = 1f;
            this.wrapMode = System.Drawing.Drawing2D.WrapMode.Clamp;
            this.transparency = 0;
            this.angle = 0.0;
        }

        public void Assign(Gradient value)
        {
            this.angle = value.angle;
            this.direction = value.direction;
            this.startColor = value.startColor;
            this.middleColor = value.middleColor;
            this.endColor = value.endColor;
            this.sigmaFocus = value.sigmaFocus;
            this.sigmaScale = value.sigmaScale;
            this.sigma = value.sigma;
            this.wrapMode = value.wrapMode;
            this.gammaCorrection = value.gammaCorrection;
            this.useMiddle = value.useMiddle;
            this.visible = value.visible;
            this.transparency = value.transparency;
        }

        private Color CorrectColor(Color value)
        {
            if (this.transparency == 0)
            {
                return value;
            }
            return Graphics3D.TransparentColor(this.transparency, value);
        }

        public void Draw(Graphics3D g, Rectangle rect)
        {
            g.Rectangle(this.DrawingBrush(rect), rect);
        }

        public void Draw(Graphics3D g, params Point[] p)
        {
            g.Polygon(this.DrawingBrush(p), p);
        }

        public void Draw(Graphics3D g, int left, int top, int right, int bottom)
        {
            this.Draw(g, Rectangle.FromLTRB(left, top, right, bottom));
        }

        public Brush DrawingBrush(params Point[] p)
        {
            PathGradientBrush brush = new PathGradientBrush(p, this.wrapMode);
            brush.InterpolationColors = this.GetColorBlend();
            if (this.sigma)
            {
                brush.SetSigmaBellShape(this.sigmaFocus, this.sigmaScale);
            }
            return brush;
        }

        public Brush DrawingBrush(Rectangle rect)
        {
            LinearGradientBrush brush;
            if (rect.Width < 1)
            {
                rect.Width = 1;
            }
            if (rect.Height < 1)
            {
                rect.Height = 1;
            }
            if (this.angle == 0.0)
            {
                brush = new LinearGradientBrush(rect, this.CorrectColor(this.startColor), this.CorrectColor(this.endColor), this.direction);
            }
            else
            {
                float angle = Convert.ToSingle(this.angle);
                brush = new LinearGradientBrush(rect, this.CorrectColor(this.startColor), this.CorrectColor(this.endColor), angle);
            }
            if (this.useMiddle && !this.middleColor.IsEmpty)
            {
                brush.InterpolationColors = this.GetColorBlend();
            }
            if (this.sigma)
            {
                brush.SetSigmaBellShape(this.sigmaFocus, this.sigmaScale);
            }
            brush.GammaCorrection = this.gammaCorrection;
            return brush;
        }

        public Brush DrawingBrush(Size size)
        {
            return this.DrawingBrush(new Rectangle(new Point(0, 0), size));
        }

        private ColorBlend GetColorBlend()
        {
            ColorBlend blend = new ColorBlend(3);
            blend.Colors[0] = this.CorrectColor(this.startColor);
            blend.Colors[1] = this.CorrectColor(this.middleColor);
            blend.Colors[2] = this.CorrectColor(this.endColor);
            blend.Positions[0] = 0f;
            blend.Positions[1] = 0.5f;
            blend.Positions[2] = 1f;
            return blend;
        }

        [Description("Angle of gradient filling. When zero, uses Direction property."), DefaultValue((double) 0.0)]
        public double Angle
        {
            get
            {
                return this.angle;
            }
            set
            {
                base.SetDoubleProperty(ref this.angle, value);
            }
        }

        [Description("Specifies the direction the gradient fill will be applied."), DefaultValue(1)]
        public LinearGradientMode Direction
        {
            get
            {
                return this.direction;
            }
            set
            {
                if (this.direction != value)
                {
                    this.direction = value;
                    this.Invalidate();
                }
            }
        }

        [DefaultValue(typeof(Color), "White"), Description("One of the three Colors used to create the gradient fill.")]
        public Color EndColor
        {
            get
            {
                return this.endColor;
            }
            set
            {
                base.SetColorProperty(ref this.endColor, value);
            }
        }

        [DefaultValue(false), Description("Enables fine-tuning of displayed Colors.")]
        public bool GammaCorrection
        {
            get
            {
                return this.gammaCorrection;
            }
            set
            {
                base.SetBooleanProperty(ref this.gammaCorrection, value);
            }
        }

        [DefaultValue(typeof(Color), "Gray"), Description("One of the three Colors used to create the gradient fill.")]
        public Color MiddleColor
        {
            get
            {
                return this.middleColor;
            }
            set
            {
                if (this.middleColor != value)
                {
                    this.middleColor = value;
                    this.useMiddle = !this.middleColor.IsEmpty;
                    this.Invalidate();
                }
            }
        }

        [DefaultValue(false), Description("Enables use of SigmaFocus and SigmaScale.")]
        public bool Sigma
        {
            get
            {
                return this.sigma;
            }
            set
            {
                base.SetBooleanProperty(ref this.sigma, value);
            }
        }

        [DefaultValue((float) 0.5f), Description("Ratio between Start and End Colors.")]
        public float SigmaFocus
        {
            get
            {
                return this.sigmaFocus;
            }
            set
            {
                if (this.sigmaFocus != value)
                {
                    this.sigmaFocus = value;
                    this.Invalidate();
                }
            }
        }

        [Description("Ratio between Colors, from 0 to 1."), DefaultValue((float) 1f)]
        public float SigmaScale
        {
            get
            {
                return this.sigmaScale;
            }
            set
            {
                if (this.sigmaScale != value)
                {
                    this.sigmaScale = value;
                    this.Invalidate();
                }
            }
        }

        [Description("One of the three Colors used to create the gradient fill."), DefaultValue(typeof(Color), "Gold")]
        public Color StartColor
        {
            get
            {
                return this.startColor;
            }
            set
            {
                base.SetColorProperty(ref this.startColor, value);
            }
        }

        [DefaultValue(0), Description("Percentage of transparency.")]
        public int Transparency
        {
            get
            {
                return this.transparency;
            }
            set
            {
                base.SetIntegerProperty(ref this.transparency, value);
            }
        }

        [Description("Uses MiddleColor or not."), DefaultValue(false)]
        public bool UseMiddle
        {
            get
            {
                return this.useMiddle;
            }
            set
            {
                base.SetBooleanProperty(ref this.useMiddle, value);
            }
        }

        [Description("Determines whether the gradient fill appears on screen."), DefaultValue(false)]
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

        [DefaultValue(4), Description("Used only for polygonal gradients, to repeat fillings.")]
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

        internal class GradientComponentEditor : UITypeEditor
        {
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                using (GradientEditor editor = new GradientEditor((Gradient) value))
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
                e.Graphics.FillRectangle(((Gradient) e.Value).DrawingBrush(e.Bounds), e.Bounds);
            }
        }
    }
}

