namespace Steema.TeeChart.Editors.Export
{
    using Steema.TeeChart.Export;
    using System;
    using System.Windows.Forms;

    public class PNGEditor : ExportEditors
    {
        public override Form Options()
        {
            if (base.fOptions == null)
            {
                base.fOptions = new PNGOptions();
            }
            return base.fOptions;
        }

        protected override void UpdateOptions(ImageExportFormat format)
        {
            PNGFormat format2 = (PNGFormat) format;
            PNGOptions fOptions = (PNGOptions) base.fOptions;
            format2.GrayScale = fOptions.GrayScale;
        }
    }
}

