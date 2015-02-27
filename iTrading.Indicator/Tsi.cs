using iTrading.Core.IndicatorBase;

namespace iTrading.Indicator
{
    using System;
    using iTrading.Core.Data;

    public class Tsi : IndicatorBase
    {
        private int fast;
        private Core.Data.DoubleSeries fastAbsEma;
        private Core.Data.DoubleSeries fastEma;
        private int slow;
        private DoubleSeries slowAbsEma;
        private DoubleSeries slowEma;

        public Tsi(IDoubleSeries source) : base(1, source, PriceTypeId.Close)
        {
            this.fast = 3;
            this.slow = 14;
            this.fastAbsEma = new DoubleSeries();
            this.fastEma = new DoubleSeries();
            this.slowAbsEma = new DoubleSeries();
            this.slowEma = new DoubleSeries();
        }

        public Tsi(Quotes quotes) : base(1, quotes, PriceTypeId.Close)
        {
            this.fast = 3;
            this.slow = 14;
            this.fastAbsEma = new DoubleSeries();
            this.fastEma = new DoubleSeries();
            this.slowAbsEma = new DoubleSeries();
            this.slowEma = new DoubleSeries();
        }

        protected  override double Calculate(int current)
        {
            if (current == 0)
            {
                this.fastEma[current] = 0.0;
                this.fastAbsEma[current] = 0.0;
                this.slowEma[current] = 0.0;
                this.slowAbsEma[current] = 0.0;
                return 0.0;
            }
            double num = base.Source[current] - base.Source[current - 1];
            this.slowEma[current] = (num * IndicatorBase.Alpha(this.Slow)) + ((1.0 - IndicatorBase.Alpha(this.Slow)) * this.slowEma[current - 1]);
            this.fastEma[current] = (this.slowEma[current] * IndicatorBase.Alpha(this.Fast)) + ((1.0 - IndicatorBase.Alpha(this.Fast)) * this.fastEma[current - 1]);
            this.slowAbsEma[current] = (Math.Abs(num) * IndicatorBase.Alpha(this.Slow)) + ((1.0 - IndicatorBase.Alpha(this.Slow)) * this.slowAbsEma[current - 1]);
            this.fastAbsEma[current] = (this.slowAbsEma[current] * IndicatorBase.Alpha(this.Fast)) + ((1.0 - IndicatorBase.Alpha(this.Fast)) * this.fastAbsEma[current - 1]);
            return ((100.0 * this.fastEma[current]) / this.fastAbsEma[current]);
        }

        public override string ToString()
        {
            return string.Concat(new object[] { "TSI(", this.Slow, ",", this.Fast, ")" });
        }

        [Parameter(1, 0x7fffffff, "Fast")]
        public int Fast
        {
            get
            {
                return this.fast;
            }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("Fast", value, "Indicator.Tsi: property out of range");
                }
                this.fast = value;
            }
        }

        public override bool IsPriceIndicator
        {
            get
            {
                return false;
            }
        }

        public override string Name
        {
            get
            {
                return "TSI";
            }
        }

        [Parameter(1, 0x7fffffff, "Slow")]
        public int Slow
        {
            get
            {
                return this.slow;
            }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("Slow", value, "Indicator.Tsi: property out of range");
                }
                this.slow = value;
            }
        }

        public override int UnstablePeriod
        {
            get
            {
                return Math.Max(this.Fast, this.Slow);
            }
        }
    }
}

