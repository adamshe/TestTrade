namespace Steema.TeeChart.Export
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors.Export;
    using System;
    using System.ComponentModel;
    using System.Drawing.Design;

    [Description("Export chart picture and data."), Editor(typeof(ComponentEditor), typeof(UITypeEditor))]
    public class Exports
    {
        internal Chart chart;
        private DataExport data;
        private ImageExport image;
        private TemplateExport template;

        public Exports(Chart c)
        {
            this.chart = c;
        }

        public void ShowExportDialog()
        {
            ExportEditor.ShowModal(this.chart);
        }

        public void ShowExportDialog(ExportFormat expFmt)
        {
            ExportEditor.ShowModal(this.chart, expFmt);
        }

        public DataExport Data
        {
            get
            {
                if (this.data == null)
                {
                    this.data = new DataExport(this.chart);
                }
                return this.data;
            }
        }

        public ImageExport Image
        {
            get
            {
                if (this.image == null)
                {
                    this.image = new ImageExport(this.chart);
                }
                return this.image;
            }
        }

        public TemplateExport Template
        {
            get
            {
                if (this.template == null)
                {
                    this.template = new TemplateExport(this.chart);
                }
                return this.template;
            }
        }

        internal class ComponentEditor : UITypeEditor
        {
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                Exports exports = (Exports) value;
                ExportEditor.ShowModal(exports.chart);
                return true;
            }

            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.Modal;
            }
        }
    }
}

