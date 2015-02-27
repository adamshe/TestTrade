namespace Steema.TeeChart.Export
{
    using Steema.TeeChart;
    using System;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Windows.Forms;

    public class MetafileFormat : ImageExportFormat
    {
        public bool bEnhanced;

        public MetafileFormat(Chart c) : base(c)
        {
            this.bEnhanced = true;
            base.FileExtension = "emf";
        }

        public override void CopyToClipboard()
        {
            using (Metafile metafile = base.chart.Metafile(base.chart, base.Width, base.Height))
            {
                Clipboard.SetDataObject(new DataObject(this.DataFormat, metafile), true);
            }
        }

        internal override string FilterFiles()
        {
            if (this.Enhanced)
            {
                return Texts.EMFFilter;
            }
            return Texts.WMFFilter;
        }

        internal override ImageFormat GetFormat()
        {
            if (this.Enhanced)
            {
                return ImageFormat.Emf;
            }
            return ImageFormat.Wmf;
        }

        public override void Save(Stream stream)
        {
            base.chart.Metafile(stream, base.chart, base.Width, base.Height);
        }

        public static void SaveToFile(Chart c, string fileName)
        {
            new MetafileFormat(c).Save(fileName);
        }

        internal void UpdateFileExtension()
        {
            if (this.Enhanced)
            {
                base.FileExtension = "emf";
            }
            else
            {
                base.FileExtension = "wmf";
            }
        }

        protected override string DataFormat
        {
            get
            {
                if (this.Enhanced)
                {
                    return DataFormats.EnhancedMetafile;
                }
                return DataFormats.MetafilePict;
            }
        }

        public bool Enhanced
        {
            get
            {
                return this.bEnhanced;
            }
            set
            {
                this.bEnhanced = value;
            }
        }
    }
}

