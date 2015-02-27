namespace Steema.TeeChart.Editors.Export
{
    using Steema.TeeChart.Export;
    using System;
    using System.Windows.Forms;

    public class BitmapEditor : ExportEditors
    {
        public override Form Options()
        {
            if (base.fOptions == null)
            {
                base.fOptions = new BMPOptions();
            }
            return base.fOptions;
        }

        protected override void UpdateOptions(ImageExportFormat format)
        {
            BitmapFormat format2 = (BitmapFormat) format;
            BMPOptions fOptions = (BMPOptions) base.fOptions;
            format2.ColorReduction = (BitmapFormat.BMPColorReduction) fOptions.ColorReduction;
        }
    }
}

