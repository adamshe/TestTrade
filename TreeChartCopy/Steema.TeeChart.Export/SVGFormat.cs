namespace Steema.TeeChart.Export
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.Drawing;
    using System.IO;

    public class SVGFormat : ImageExportFormat
    {
        public SVGFormat(Chart c) : base(c)
        {
            base.FileExtension = "svg";
        }

        internal override string FilterFiles()
        {
            return Texts.SVGFilter;
        }

        public override void Save(Stream stream)
        {
            Graphics3D graphicsd = base.chart.Graphics3D;
            try
            {
                base.chart.Graphics3D = new Graphics3DSVG(stream, base.chart);
                base.chart.Draw(null, new Rectangle(0, 0, base.Width, base.Height));
            }
            finally
            {
                base.chart.Graphics3D = graphicsd;
            }
        }

        public static void SaveToFile(Chart c, string fileName)
        {
            new SVGFormat(c).Save(fileName);
        }
    }
}

