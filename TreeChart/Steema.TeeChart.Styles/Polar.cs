namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using System;
    using System.Drawing;

    [ToolboxBitmap(typeof(Polar), "SeriesIcons.Polar.bmp")]
    public class Polar : CustomPolar
    {
        public Polar() : this(null)
        {
        }

        public Polar(Chart c) : base(c)
        {
        }

        protected override void AddSampleValues(int numValues)
        {
            double num = 360.0 / ((double) numValues);
            Series.SeriesRandom random = base.RandomBounds(numValues);
            for (int i = 1; i <= numValues; i++)
            {
                base.Add((double) (i * num), (double) (1.0 + (1000.0 * random.Random())));
            }
        }

        public override string Description
        {
            get
            {
                return Texts.GalleryPolar;
            }
        }
    }
}

