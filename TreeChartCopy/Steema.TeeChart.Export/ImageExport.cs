namespace Steema.TeeChart.Export
{
    using Steema.TeeChart;
    using System;

    public sealed class ImageExport
    {
        private BitmapFormat bitmapFormat;
        internal Chart chart;
        private GIFFormat gifFormat;
        private JPEGFormat jpegFormat;
        private MetafileFormat metafileFormat;
        private PNGFormat pngFormat;
        private SVGFormat svgFormat;
        private TIFFFormat tifFormat;
        private VMLFormat vmlFormat;

        public ImageExport(Chart c)
        {
            this.chart = c;
        }

        internal ImageExportFormat FromFormat(PictureFormats format)
        {
            switch (format)
            {
                case PictureFormats.Metafile:
                    return this.Metafile;

                case PictureFormats.JPEG:
                    return this.JPEG;

                case PictureFormats.PNG:
                    return this.PNG;

                case PictureFormats.GIF:
                    return this.GIF;

                case PictureFormats.TIFF:
                    return this.TIFF;

                case PictureFormats.VML:
                    return this.VML;

                case PictureFormats.SVG:
                    return this.SVG;
            }
            return this.Bitmap;
        }

        public BitmapFormat Bitmap
        {
            get
            {
                if (this.bitmapFormat == null)
                {
                    this.bitmapFormat = new BitmapFormat(this.chart);
                }
                return this.bitmapFormat;
            }
        }

        public GIFFormat GIF
        {
            get
            {
                if (this.gifFormat == null)
                {
                    this.gifFormat = new GIFFormat(this.chart);
                }
                return this.gifFormat;
            }
        }

        public JPEGFormat JPEG
        {
            get
            {
                if (this.jpegFormat == null)
                {
                    this.jpegFormat = new JPEGFormat(this.chart);
                }
                return this.jpegFormat;
            }
        }

        public MetafileFormat Metafile
        {
            get
            {
                if (this.metafileFormat == null)
                {
                    this.metafileFormat = new MetafileFormat(this.chart);
                }
                return this.metafileFormat;
            }
        }

        public PNGFormat PNG
        {
            get
            {
                if (this.pngFormat == null)
                {
                    this.pngFormat = new PNGFormat(this.chart);
                }
                return this.pngFormat;
            }
        }

        public SVGFormat SVG
        {
            get
            {
                if (this.svgFormat == null)
                {
                    this.svgFormat = new SVGFormat(this.chart);
                }
                return this.svgFormat;
            }
        }

        public TIFFFormat TIFF
        {
            get
            {
                if (this.tifFormat == null)
                {
                    this.tifFormat = new TIFFFormat(this.chart);
                }
                return this.tifFormat;
            }
        }

        public VMLFormat VML
        {
            get
            {
                if (this.vmlFormat == null)
                {
                    this.vmlFormat = new VMLFormat(this.chart);
                }
                return this.vmlFormat;
            }
        }
    }
}

