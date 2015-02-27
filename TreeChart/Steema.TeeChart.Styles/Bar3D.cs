namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using System;
    using System.Drawing;

    [ToolboxBitmap(typeof(Bar3D), "SeriesIcons.Bar3D.bmp")]
    public class Bar3D : Bar
    {
        private ValueList offsetValues;

        public Bar3D() : this(null)
        {
        }

        public Bar3D(Chart c) : base(c)
        {
            this.offsetValues = new ValueList(this, Texts.ValuesOffset);
        }

        public int Add(double x, double y, double offset)
        {
            return this.Add(x, y, offset, "", Color.Empty);
        }

        public int Add(double x, double y, double offset, Color color)
        {
            return this.Add(x, y, offset, "", color);
        }

        public int Add(double x, double y, double offset, string text)
        {
            return this.Add(x, y, offset, text, Color.Empty);
        }

        public int Add(double x, double y, double offset, string text, Color color)
        {
            this.offsetValues.TempValue = offset;
            return base.Add(x, y, text, color);
        }

        protected override void AddSampleValues(int numValues)
        {
            Series.SeriesRandom random = base.RandomBounds(numValues);
            for (int i = 1; i <= numValues; i++)
            {
                random.tmpY = random.DifY * random.Random();
                this.Add(random.tmpX, 10.0 + Math.Abs(random.tmpY), Math.Abs((double) (random.DifY / (1.0 + (5.0 * random.Random())))));
                random.tmpX += random.StepX;
            }
        }

        protected internal override double GetOriginValue(int valueIndex)
        {
            return (base.GetOriginValue(valueIndex) + this.offsetValues[valueIndex]);
        }

        public override double MaxYValue()
        {
            double num = base.MaxYValue();
            if ((base.iMultiBar != MultiBars.None) && (base.iMultiBar != MultiBars.Side))
            {
                return num;
            }
            return Math.Max(num, this.offsetValues.Maximum);
        }

        public override double MinYValue()
        {
            double num = base.MinYValue();
            if ((base.iMultiBar == MultiBars.None) || (base.iMultiBar == MultiBars.Side))
            {
                for (int i = 0; i < base.Count; i++)
                {
                    if (this.offsetValues[i] < 0.0)
                    {
                        num = Math.Min(num, base.vyValues[i] + this.offsetValues[i]);
                    }
                }
            }
            return num;
        }

        public override double PointOrigin(int valueIndex, bool sumAll)
        {
            return this.offsetValues[valueIndex];
        }

        protected override bool SubGalleryStack()
        {
            return false;
        }

        public override string Description
        {
            get
            {
                return Texts.GalleryBar3D;
            }
        }

        public ValueList OffsetValues
        {
            get
            {
                return this.offsetValues;
            }
            set
            {
                base.SetValueList(this.offsetValues, value);
            }
        }
    }
}

