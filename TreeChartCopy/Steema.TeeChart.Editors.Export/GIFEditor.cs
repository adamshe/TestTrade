namespace Steema.TeeChart.Editors.Export
{
    using Steema.TeeChart.Export;
    using System;
    using System.Windows.Forms;

    public class GIFEditor : ExportEditors
    {
        public override Form Options()
        {
            if (base.fOptions == null)
            {
                base.fOptions = new GIFOptions();
            }
            return base.fOptions;
        }

        protected override void UpdateOptions(ImageExportFormat format)
        {
            GIFFormat format2 = (GIFFormat) format;
            GIFOptions fOptions = (GIFOptions) base.fOptions;
            format2.ColorReduction = (GIFFormat.GIFColorReduction) fOptions.ColorReduction;
        }
    }
}

