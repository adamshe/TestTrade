namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using Steema.TeeChart.Editors.Tools;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    [Designer(typeof(ToolDesigner)), DefaultProperty("Active"), DesignTimeVisible(true), ToolboxBitmap(typeof(Bitmap))]
    public class Tool : TeeBase
    {
        private bool active;
        protected ChartBrush bBrush;
        public static int ClickTolerance = 3;
        protected internal ChartPen pPen;

        protected Tool() : this((Chart) null)
        {
        }

        protected Tool(Chart c) : base(c)
        {
            this.active = true;
            if (base.chart != null)
            {
                base.chart.Tools.Add(this);
            }
        }

        public Tool(IContainer container) : this((Chart) null)
        {
            container.Add(this);
        }

        protected internal virtual void ChartEvent(EventArgs e)
        {
        }

        protected override void Dispose(bool disposing)
        {
            base.Chart = null;
            base.Dispose(disposing);
        }

        public Image GetBitmapEditor()
        {
            return Utils.GetBitmapResource(base.GetType().Namespace + ".ToolsIcons." + base.GetType().Name + ".bmp");
        }

        internal static bool GetFirstLastSeries(Series s, out int AMin, out int AMax)
        {
            AMin = s.firstVisible;
            if (AMin < 0)
            {
                AMin = 0;
            }
            AMax = s.lastVisible;
            if (AMax < 0)
            {
                AMax = s.Count - 1;
            }
            else if (AMax >= s.Count)
            {
                AMax = s.Count - 1;
            }
            return (((s.Count > 0) && (AMin <= s.Count)) && (AMax <= s.Count));
        }

        protected internal virtual void MouseEvent(MouseEventKinds kind, MouseEventArgs e, ref Cursor c)
        {
        }

        protected virtual void SetActive(bool value)
        {
            base.SetBooleanProperty(ref this.active, value);
        }

        protected override void SetChart(Chart value)
        {
            if (base.chart != value)
            {
                if (base.chart != null)
                {
                    base.chart.Tools.Remove(this);
                }
                base.SetChart(value);
                if (base.chart != null)
                {
                    base.chart.Tools.Add(this);
                }
                if (this.pPen != null)
                {
                    this.pPen.Chart = base.chart;
                }
                if (this.bBrush != null)
                {
                    this.bBrush.Chart = base.chart;
                }
                if (base.chart != null)
                {
                    base.chart.Invalidate();
                }
            }
        }

        public override string ToString()
        {
            return this.Description;
        }

        [DefaultValue(true), Description("Enables/Disables the indexed Tool.")]
        public bool Active
        {
            get
            {
                return this.active;
            }
            set
            {
                this.SetActive(value);
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string Description
        {
            get
            {
                return "";
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public virtual string Summary
        {
            get
            {
                return "";
            }
        }

        internal class ToolComponentEditor : System.ComponentModel.ComponentEditor
        {
            public override bool EditComponent(ITypeDescriptorContext context, object aobject)
            {
                ToolsEditor.ShowEditor((Steema.TeeChart.Tools.Tool) aobject);
                return true;
            }
        }

        internal class ToolDesigner : ComponentDesigner
        {
            public ToolDesigner()
            {
                this.Verbs.Add(new DesignerVerb(Texts.Edit, new EventHandler(this.OnEdit)));
            }

            private void OnEdit(object sender, EventArgs e)
            {
                if (ToolsEditor.ShowEditor((Steema.TeeChart.Tools.Tool) base.Component))
                {
                    base.RaiseComponentChanged(null, null, null);
                }
            }
        }
    }
}

