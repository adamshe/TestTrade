namespace Steema.TeeChart.Export
{
    using Steema.TeeChart;
    using System;
    using System.Drawing.Imaging;

    public class TIFFFormat : ImageExportFormat
    {
        private TIFFCompression compression;

        public TIFFFormat(Chart c) : base(c)
        {
            base.FileExtension = "tif";
        }

        internal override string FilterFiles()
        {
            return Texts.TIFFFilter;
        }

        internal override ImageFormat GetFormat()
        {
            return ImageFormat.Tiff;
        }

        public static void SaveToFile(Chart c, string fileName)
        {
            new TIFFFormat(c).Save(fileName);
        }

        public TIFFCompression Compression
        {
            get
            {
                return this.compression;
            }
            set
            {
                this.compression = value;
            }
        }

        internal override EncoderParameters EncoderParams
        {
            get
            {
                EncoderValue compressionLZW;
                if (this.Compression == TIFFCompression.LZW)
                {
                    compressionLZW = EncoderValue.CompressionLZW;
                }
                else
                {
                    compressionLZW = EncoderValue.CompressionRle;
                }
                EncoderParameters parameters = new EncoderParameters(1);
                parameters.Param[0] = new EncoderParameter(Encoder.Compression, (long) compressionLZW);
                return parameters;
            }
        }

        public enum TIFFCompression
        {
            LZW,
            RLE
        }
    }
}

