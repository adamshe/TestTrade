namespace Steema.TeeChart.Export
{
    using Steema.TeeChart;
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;

    public class GIFFormat : ImageExportFormat
    {
        private GIFColorReduction colorReduction;

        public GIFFormat(Chart c) : base(c)
        {
            base.FileExtension = "gif";
        }

        internal override string FilterFiles()
        {
            return Texts.GIFFilter;
        }

        internal override ImageFormat GetFormat()
        {
            return ImageFormat.Gif;
        }

        internal override void GetImageOptions(ref Bitmap b)
        {
            switch (this.ColorReduction)
            {
                case GIFColorReduction.None:
                    break;

                case GIFColorReduction.GrayScale:
                    this.MakeWithNewColorTable(ref b);
                    break;

                default:
                    return;
            }
        }

        private void MakeWithNewColorTable(ref Bitmap b)
        {
            ImageExportFormat.MakeGrayScale(b);
        }

        public static void SaveToFile(Chart c, string fileName)
        {
            new GIFFormat(c).Save(fileName);
        }

        public GIFColorReduction ColorReduction
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

        public enum GIFColorReduction
        {
            None,
            GrayScale
        }
    }
}

