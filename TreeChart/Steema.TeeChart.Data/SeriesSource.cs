namespace Steema.TeeChart.Data
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Design;

    [Serializable, ToolboxItem(false), Designer(typeof(Designer))]
    public abstract class SeriesSource : Component
    {
        private Steema.TeeChart.Styles.Series iSeries;

        protected SeriesSource()
        {
        }

        internal void AddToContainer(IContainer c)
        {
            c.Add(this);
        }

        protected override void Dispose(bool disposing)
        {
            if ((disposing && (this.Series != null)) && (this.Series.DataSource == this))
            {
                this.Series.DataSource = null;
            }
            base.Dispose(disposing);
        }

        public virtual void RefreshData()
        {
        }

        public Steema.TeeChart.Styles.Series Series
        {
            get
            {
                return this.iSeries;
            }
            set
            {
                this.iSeries = value;
            }
        }

        internal sealed class Designer : ComponentDesigner
        {
            public Designer()
            {
                this.Verbs.Add(new DesignerVerb(Texts.Edit, new EventHandler(this.OnEdit)));
                this.Verbs.Add(new DesignerVerb(Texts.RefreshData, new EventHandler(this.OnRefresh)));
            }

            private void OnEdit(object sender, EventArgs e)
            {
                SeriesSource component = (SeriesSource) base.Component;
                if (((component != null) && (component.Series != null)) && SeriesEditor.ShowEditor(component.Series, ChartEditorTabs.SeriesDataSource))
                {
                    base.RaiseComponentChanged(null, null, null);
                }
            }

            private void OnRefresh(object sender, EventArgs e)
            {
                ((SeriesSource) base.Component).RefreshData();
            }
        }
    }
}

