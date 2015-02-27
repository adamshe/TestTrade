namespace Steema.TeeChart.Export
{
    using Steema.TeeChart;
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;

    public class BitmapFormat : ImageExportFormat
    {
        private BMPColorReduction colorReduction;

        public BitmapFormat(Chart c) : base(c)
        {
            base.FileExtension = "bmp";
        }

        internal override string FilterFiles()
        {
            return Texts.BMPFilter;
        }

        internal override ImageFormat GetFormat()
        {
            return ImageFormat.Bmp;
        }

        internal override void GetImageOptions(ref Bitmap b)
        {
            switch (this.ColorReduction)
            {
                case BMPColorReduction.Default:
                    break;

                case BMPColorReduction.GrayScale:
                    ImageExportFormat.MakeGrayScale(b);
                    break;

                default:
                    return;
            }
        }

        public BMPColorReduction ColorReduction
        {
            get
            {
                return this.colorReduction;
            }
            set
            {
                this.colorReduction = value;
            }
        }

        public enum BMPColorReduction
        {
            Default,
            GrayScale
        }
    }
}

