namespace Steema.TeeChart.Editors.Export
{
    using Steema.TeeChart.Export;
    using System;
    using System.Windows.Forms;

    public class MetafileEditor : ExportEditors
    {
        public override Form Options()
        {
            if (base.fOptions == null)
            {
                base.fOptions = new EmfOptions();
            }
            return base.fOptions;
        }

        protected override void UpdateOptions(ImageExportFormat format)
        {
            MetafileFormat format2 = (MetafileFormat) format;
            EmfOptions fOptions = (EmfOptions) base.fOptions;
            format2.Enhanced = fOptions.Enhanced;
            format2.UpdateFileExtension();
        }
    }
}

