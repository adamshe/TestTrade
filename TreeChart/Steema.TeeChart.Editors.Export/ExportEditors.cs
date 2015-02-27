namespace Steema.TeeChart.Editors.Export
{
    using Steema.TeeChart.Export;
    using System;
    using System.Windows.Forms;

    public class ExportEditors
    {
        protected Form fOptions;

        public virtual Form Options()
        {
            return null;
        }

        public void SetOptions(ImageExportFormat format)
        {
            this.UpdateOptions(format);
            format.Width = ((ExportEditor) this.fOptions.ParentForm).ImageWidth();
            format.Height = ((ExportEditor) this.fOptions.ParentForm).ImageHeight();
        }

        protected virtual void UpdateOptions(ImageExportFormat format)
        {
        }
    }
}

