namespace Steema.TeeChart
{
    using Steema.TeeChart.Drawing;
    using Steema.TeeChart.Editors;
    using System;
    using System.ComponentModel;
    using System.Drawing.Design;

    [Editor(typeof(WallEditor), typeof(UITypeEditor))]
    public class Wall : Shape
    {
        protected bool bApplyDark;
        protected internal int iSize;
        protected Walls walls;

        public Wall(Chart c, Walls w) : base(c)
        {
            this.bApplyDark = true;
            this.walls = w;
        }

        protected void PrepareGraphics(Graphics3D g)
        {
            if (base.bTransparent)
            {
                g.Brush.Visible = false;
            }
            else
            {
                g.Brush = base.Brush;
            }
            g.Pen = base.Pen;
        }

        [Description("Applies a darker shade to 3D Chart Walls when true."), DefaultValue(true)]
        public bool ApplyDark
        {
            get
            {
                return this.bApplyDark;
            }
            set
            {
                base.SetBooleanProperty(ref this.bApplyDark, value);
            }
        }

        protected bool ShouldDark
        {
            get
            {
                return ((this.bApplyDark && (base.bBrush != null)) && base.Brush.Visible);
            }
        }

        [DefaultValue(0), Description("Sets the Chart Wall thickness.")]
        public int Size
        {
            get
            {
                return this.iSize;
            }
            set
            {
                base.SetIntegerProperty(ref this.iSize, value);
            }
        }

        internal class WallEditor : UITypeEditor
        {
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                Wall w = (Wall) value;
                bool flag = EditorUtils.ShowFormModal(new Steema.TeeChart.Editors.WallEditor(w, null));
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

