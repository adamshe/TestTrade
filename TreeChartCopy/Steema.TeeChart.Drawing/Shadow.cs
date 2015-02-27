namespace Steema.TeeChart.Drawing
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;

    [Description("Properties to draw a shadow."), Editor(typeof(ShadowComponentEditor), typeof(UITypeEditor))]
    public class Shadow : TeeBase
    {
        protected ChartBrush bBrush;
        protected internal bool bVisible;
        protected internal int defaultSize;
        protected internal bool defaultVisible;
        private int height;
        private int width;

        public Shadow(Chart c) : base(c)
        {
            this.height = 3;
            this.width = 3;
            this.defaultSize = 3;
            this.bBrush = new ChartBrush(c, System.Drawing.Color.DarkGray);
        }

        public Shadow(Chart c, int size) : this(c)
        {
            this.defaultSize = size;
            this.width = size;
            this.height = size;
        }

        public void Assign(Shadow value)
        {
            this.height = value.height;
            this.width = value.width;
            this.bVisible = value.bVisible;
            this.bBrush.Assign(value.bBrush);
        }

        public void Draw(Graphics3D g, Rectangle rect)
        {
            this.Draw(g, rect, 0, 0);
        }

        public void Draw(Graphics3D g, Rectangle rect, int angle, int aZ)
        {
            if ((this.height != 0) || (this.width != 0))
            {
                g.Pen.bVisible = false;
                g.Brush = this.Brush;
                rect.Offset(this.width, this.height);
                if (angle > 0)
                {
                    g.Polygon(aZ, Graphics3D.RotateRectangle(rect, angle));
                }
                else
                {
                    g.Rectangle(rect);
                }
            }
        }

        protected override void SetChart(Chart c)
        {
            base.SetChart(c);
            if (this.bBrush != null)
            {
                this.bBrush.Chart = base.chart;
            }
        }

        protected virtual bool ShouldSerializeHeight()
        {
            return (this.height != this.defaultSize);
        }

        protected virtual bool ShouldSerializeVisible()
        {
            return (this.bVisible != this.defaultVisible);
        }

        protected virtual bool ShouldSerializeWidth()
        {
            return (this.width != this.defaultSize);
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance"), Description("Defines properties to fill shadow.")]
        public ChartBrush Brush
        {
            get
            {
                return this.bBrush;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Description("Gets or sets Color used to fill shadow.")]
        public System.Drawing.Color Color
        {
            get
            {
                return this.bBrush.Color;
            }
            set
            {
                this.bBrush.Color = value;
            }
        }

        [Description("The vertical shadow size.")]
        public int Height
        {
            get
            {
                return this.height;
            }
            set
            {
                base.SetIntegerProperty(ref this.height, value);
            }
        }

        [Obsolete("Obsolete. Please use Width property"), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public int HorizSize
        {
            get
            {
                return this.Width;
            }
            set
            {
                this.Width = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), Description("Size in pixels of shadow.")]
        public System.Drawing.Size Size
        {
            get
            {
                return new System.Drawing.Size(this.width, this.height);
            }
            set
            {
                this.width = value.Width;
                this.height = value.Height;
                this.Invalidate();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Description("Sets Transparency level from 0 to 100%.")]
        public int Transparency
        {
            get
            {
                return this.bBrush.Transparency;
            }
            set
            {
                this.bBrush.Transparency = value;
            }
        }

        [Obsolete("Obsolete. Please use Height property"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public int VertSize
        {
            get
            {
                return this.Height;
            }
            set
            {
                this.Height = value;
            }
        }

        [Description("Shows or hides shadow.")]
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

        [Description("The horizontal shadow size.")]
        public int Width
        {
            get
            {
                return this.width;
            }
            set
            {
                base.SetIntegerProperty(ref this.width, value);
            }
        }

        internal class ShadowComponentEditor : UITypeEditor
        {
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                Shadow s = (Shadow) value;
                bool flag = EditorUtils.ShowFormModal(new ShadowEditor(s, null));
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

