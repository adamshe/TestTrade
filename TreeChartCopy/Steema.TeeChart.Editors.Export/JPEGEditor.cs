namespace Steema.TeeChart.Editors.Export
{
    using Steema.TeeChart.Export;
    using System;
    using System.Windows.Forms;

    public class JPEGEditor : ExportEditors
    {
        public override Form Options()
        {
            if (base.fOptions == null)
            {
                base.fOptions = new JPEGOptions();
            }
            return base.fOptions;
        }

        protected override void UpdateOptions(ImageExportFormat format)
        {
            JPEGFormat format2 = (JPEGFormat) format;
            JPEGOptions fOptions = (JPEGOptions) base.fOptions;
            format2.GrayScale = fOptions.Grayscale;
            format2.Quality = fOptions.Quality;
        }
    }
}

