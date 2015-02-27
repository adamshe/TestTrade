namespace Steema.TeeChart.Export
{
    using Steema.TeeChart;
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;

    public class PNGFormat : ImageExportFormat
    {
        private bool grayScale;

        public PNGFormat(Chart c) : base(c)
        {
            this.grayScale = false;
            base.FileExtension = "png";
        }

        internal override string FilterFiles()
        {
            return Texts.PNGFilter;
        }

        internal override ImageFormat GetFormat()
        {
            return ImageFormat.Png;
        }

        internal override void GetImageOptions(ref Bitmap b)
        {
            if (this.GrayScale)
            {
                base.ConvertToGrayscale(ref b);
            }
        }

        internal static void SaveToFile(Chart c, string fileName)
        {
            new PNGFormat(c).Save(fileName);
        }

        public bool GrayScale
        {
            get
            {
                return this.grayScale;
            }
            set
            {
                this.grayScale = value;
            }
        }
    }
}

