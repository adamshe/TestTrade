namespace Steema.TeeChart.Export
{
    using Steema.TeeChart;
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;

    public class JPEGFormat : ImageExportFormat
    {
        private bool grayScale;
        private int quality;

        public JPEGFormat(Chart c) : base(c)
        {
            this.quality = 0x5f;
            this.grayScale = false;
            base.FileExtension = "jpg";
        }

        internal override string FilterFiles()
        {
            return Texts.JPEGFilter;
        }

        internal override ImageFormat GetFormat()
        {
            return ImageFormat.Jpeg;
        }

        internal override void GetImageOptions(ref Bitmap b)
        {
            if (this.GrayScale)
            {
                base.ConvertToGrayscale(ref b);
            }
        }

        public static void SaveToFile(Chart c, string fileName)
        {
            new JPEGFormat(c).Save(fileName);
        }

        internal override EncoderParameters EncoderParams
        {
            get
            {
                EncoderParameters parameters = new EncoderParameters(1);
                parameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)this.Quality);
                return parameters;
            }
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

        public int Quality
        {
            get
            {
                return this.quality;
            }
            set
            {
                this.quality = value;
            }
        }
    }
}

