using iTrading.Core.IndicatorBase;

namespace iTrading.Indicator
{
    using System;
    using iTrading.Core.Data;

    public class StdDev : IndicatorBase
    {
        private int period;

        public StdDev(IDoubleSeries source) : base(source)
        {
            this.period = 14;
        }

        public StdDev(Quotes quotes) : base(quotes)
        {
            this.period = 14;
        }

        protected override double Calculate(int current)
        {
            if (current == 0)
            {
                return 0.0;
            }
            double num = 0.0;
            for (int i = Math.Max(0, (current - this.Period) + 1); i <= current; i++)
            {
                num += base.Source[i];
            }
            double num3 = num / ((double) Math.Min(current + 1, this.Period));
            num = 0.0;
            for (int j = Math.Max(0, (current - this.Period) + 1); j <= current; j++)
            {
                num += (base.Source[j] - num3) * (base.Source[j] - num3);
            }
            return Math.Sqrt(num / ((double) Math.Min(current + 1, this.Period)));
        }

        public override string ToString()
        {
            return string.Concat(new object[] { "StdDev(", this.Period, ", ", PriceType.All[this.BasedOn].Name, ")" });
        }

        [Parameter(PriceTypeId.Close, "Based on")]
        public PriceTypeId BasedOn
        {
            get
            {
                return base.DefaultBasedOn;
            }
            set
            {
                base.DefaultBasedOn = value;
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
                return "Standard Deviation";
            }
        }

        [Parameter(1, 0x7fffffff, "# of periods")]
        public int Period
        {
            get
            {
                return this.period;
            }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("Period", value, "Indicator.StdDev: property out of range");
                }
                this.period = value;
            }
        }

        public override int UnstablePeriod
        {
            get
            {
                return this.Period;
            }
        }
    }
}

