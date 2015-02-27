namespace Steema.TeeChart.Editors.Export
{
    using Steema.TeeChart.Export;
    using System;
    using System.Windows.Forms;

    public class TIFFEditor : ExportEditors
    {
        public override Form Options()
        {
            if (base.fOptions == null)
            {
                base.fOptions = new TIFFOptions();
            }
            return base.fOptions;
        }

        protected override void UpdateOptions(ImageExportFormat format)
        {
            TIFFFormat format2 = (TIFFFormat) format;
            TIFFOptions fOptions = (TIFFOptions) base.fOptions;
            format2.Compression = fOptions.CompressionLZW ? TIFFFormat.TIFFCompression.LZW : TIFFFormat.TIFFCompression.RLE;
        }
    }
}

