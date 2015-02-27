namespace Steema.TeeChart
{
    using Steema.TeeChart.Drawing;
    using Steema.TeeChart.Editors;
    using System;
    using System.ComponentModel;
    using System.Drawing.Design;

    [Serializable, Editor(typeof(PageEditor), typeof(UITypeEditor)), Description("Chart Paging properties.")]
    public class Page : TeeBase
    {
        private int current;
        private int maxPoints;
        private bool scaleLast;

        public Page(Chart c) : base(c)
        {
            this.maxPoints = 0;
            this.current = 1;
            this.scaleLast = true;
        }

        public void Next()
        {
            if ((this.maxPoints > 0) && (this.current < this.Count))
            {
                this.Current++;
            }
        }

        public void Previous()
        {
            if ((this.maxPoints > 0) && (this.current > 1))
            {
                this.Current--;
            }
        }

        [Browsable(false), Description("Gets the number of pages according to MaxPointsPerPage property.")]
        public int Count
        {
            get
            {
                return base.Chart.NumPages();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), Description("Gets or sets the current page number.")]
        public int Current
        {
            get
            {
                return this.current;
            }
            set
            {
                if ((value >= 0) && (value <= this.Count))
                {
                    base.SetIntegerProperty(ref this.current, value);
                }
            }
        }

        [DefaultValue(0), Description("Sets the number of points displayed per page.")]
        public int MaxPointsPerPage
        {
            get
            {
                return this.maxPoints;
            }
            set
            {
                base.SetIntegerProperty(ref this.maxPoints, value);
            }
        }

        [Description("Determines how the last Chart page will be displayed."), DefaultValue(true)]
        public bool ScaleLastPage
        {
            get
            {
                return this.scaleLast;
            }
            set
            {
                base.SetBooleanProperty(ref this.scaleLast, value);
            }
        }

        internal class PageEditor : UITypeEditor
        {
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                Page p = (Page) value;
                bool flag = EditorUtils.ShowFormModal(new Steema.TeeChart.Editors.PageEditor(p, null));
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

