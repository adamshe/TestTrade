namespace Steema.TeeChart
{
    using Steema.TeeChart.Drawing;
    using Steema.TeeChart.Editors;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;

    [Serializable, Editor(typeof(PanelEditor), typeof(UITypeEditor)), Description("Background properties.")]
    public class Panel : Shape
    {
        private bool backInside;
        private double marginBottom;
        private double marginLeft;
        private double marginRight;
        private double marginTop;
        private PanelMarginUnits marginUnits;

        public Panel(Chart c) : base(c)
        {
            this.marginLeft = 3.0;
            this.marginTop = 4.0;
            this.marginRight = 3.0;
            this.marginBottom = 4.0;
            this.marginUnits = PanelMarginUnits.Percent;
            base.Pen.defaultVisible = false;
            base.Pen.Visible = false;
            base.Color = SystemColors.Control;
            base.Brush.defaultColor = SystemColors.Control;
            base.Bevel.Outer = BevelStyles.Raised;
            base.bBevel.defaultOuter = BevelStyles.Raised;
        }

        internal void ApplyMargins(ref Rectangle r)
        {
            int height = r.Height;
            if (this.marginTop != 0.0)
            {
                int num2 = (this.marginUnits == PanelMarginUnits.Percent) ? Utils.Round((double) ((height * this.marginTop) * 0.01)) : ((int) this.marginTop);
                r.Y += num2;
                r.Height -= num2;
            }
            if (this.marginBottom != 0.0)
            {
                int num3 = (this.marginUnits == PanelMarginUnits.Percent) ? Utils.Round((double) ((height * this.marginBottom) * 0.01)) : ((int) this.marginBottom);
                r.Height -= num3;
            }
            int width = r.Width;
            if (this.marginLeft != 0.0)
            {
                int num5 = (this.marginUnits == PanelMarginUnits.Percent) ? Utils.Round((double) ((width * this.marginLeft) * 0.01)) : ((int) this.marginLeft);
                r.X += num5;
                r.Width -= num5;
            }
            if (this.marginRight != 0.0)
            {
                int num6 = (this.marginUnits == PanelMarginUnits.Percent) ? Utils.Round((double) ((width * this.marginRight) * 0.01)) : ((int) this.marginRight);
                r.Width -= num6;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), Obsolete("Set Panel.Image=null")]
        public void BackImageClear()
        {
            base.Image = null;
        }

        public void Draw(Graphics3D g, ref Rectangle r)
        {
            if (!base.chart.printing || base.chart.Printer.PrintPanelBackground)
            {
                if (base.Shadow.Visible)
                {
                    g.Rectangle(base.Brush.DrawingBrush, r);
                    int width = base.Shadow.Width;
                    if (width > 0)
                    {
                        r.Width -= width;
                    }
                    else
                    {
                        r.X += width;
                    }
                    int height = base.Shadow.Height;
                    if (height > 0)
                    {
                        r.Height -= height;
                    }
                    else
                    {
                        r.Y += height;
                    }
                }
                this.Paint(g, r);
            }
            this.ApplyMargins(ref r);
        }

        [Description("Fills panel using Walls.Back.Image."), DefaultValue(false), EditorBrowsable(EditorBrowsableState.Never), Obsolete("Use Walls.Back.Image property.")]
        public bool BackImageInside
        {
            get
            {
                return this.backInside;
            }
            set
            {
                base.SetBooleanProperty(ref this.backInside, value);
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("Use the Pen property."), EditorBrowsable(EditorBrowsableState.Never)]
        public ChartPen BorderPen
        {
            get
            {
                return base.Pen;
            }
        }

        [DefaultValue((double) 4.0), Description("Sets Bottom margin as percentage of Chart Drawing.")]
        public double MarginBottom
        {
            get
            {
                return this.marginBottom;
            }
            set
            {
                base.SetDoubleProperty(ref this.marginBottom, value);
            }
        }

        [DefaultValue((double) 3.0), Description("Sets Left margin as percentage of Chart Drawing.")]
        public double MarginLeft
        {
            get
            {
                return this.marginLeft;
            }
            set
            {
                base.SetDoubleProperty(ref this.marginLeft, value);
            }
        }

        [DefaultValue((double) 3.0), Description("Sets Right margin as percentage of Chart Drawing.")]
        public double MarginRight
        {
            get
            {
                return this.marginRight;
            }
            set
            {
                base.SetDoubleProperty(ref this.marginRight, value);
            }
        }

        [Description("Sets Top margin as percentage of Chart Drawing."), DefaultValue((double) 4.0)]
        public double MarginTop
        {
            get
            {
                return this.marginTop;
            }
            set
            {
                base.SetDoubleProperty(ref this.marginTop, value);
            }
        }

        [DefaultValue(0), Description("Sets the units in which the Margins are expressed.")]
        public PanelMarginUnits MarginUnits
        {
            get
            {
                return this.marginUnits;
            }
            set
            {
                if (this.marginUnits != value)
                {
                    this.marginUnits = value;
                    this.Invalidate();
                }
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced), Browsable(false), Description("This property shows or hides the Panel.")]
        public bool Visible
        {
            get
            {
                return base.Visible;
            }
            set
            {
                base.Visible = value;
            }
        }

        internal class PanelEditor : UITypeEditor
        {
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                Panel p = (Panel) value;
                bool flag = EditorUtils.ShowFormModal(new Steema.TeeChart.Editors.PanelEditor(p, null));
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

