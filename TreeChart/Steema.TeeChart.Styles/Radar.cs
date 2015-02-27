namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using System;
    using System.Drawing;

    [ToolboxBitmap(typeof(Radar), "SeriesIcons.Radar.bmp")]
    public class Radar : CustomPolar
    {
        public Radar() : this(null)
        {
        }

        public Radar(Chart c) : base(c)
        {
        }

        protected override void AddSampleValues(int numValues)
        {
            string[] strArray = new string[] { Texts.PieSample1, Texts.PieSample2, Texts.PieSample3, Texts.PieSample4, Texts.PieSample5 };
            Series.SeriesRandom random = base.RandomBounds(numValues);
            for (int i = 0; i < numValues; i++)
            {
                base.Add(1000.0 * random.Random(), strArray[i % 5]);
            }
        }

        protected internal override void DoBeforeDrawChart()
        {
            base.DoBeforeDrawChart();
            base.SetRotationAngle(90);
            base.IMaxValuesCount = base.chart.GetMaxValuesCount();
            if (base.IMaxValuesCount > 0)
            {
                base.AngleIncrement = 360.0 / ((double) base.IMaxValuesCount);
            }
        }

        protected override string GetCircleLabel(double angle, int index)
        {
            string str = base.sLabels[index];
            if (str.Length == 0)
            {
                return index.ToString();
            }
            return str;
        }

        protected override double GetXValue(int valueIndex)
        {
            if (base.IMaxValuesCount <= 0)
            {
                return 0.0;
            }
            return (valueIndex * (360.0 / ((double) base.IMaxValuesCount)));
        }

        protected internal override int NumSampleValues()
        {
            return 5;
        }

        internal override void PrepareForGallery(bool IsEnabled)
        {
            base.PrepareForGallery(IsEnabled);
            base.FillSampleValues();
        }

        public override string Description
        {
            get
            {
                return Texts.GalleryRadar;
            }
        }
    }
}

