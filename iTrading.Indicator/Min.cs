using iTrading.Core.IndicatorBase;

namespace iTrading.Indicator
{
    using System;
    using iTrading.Core.Data;

    public class Min : IndicatorBase
    {
        private int period;

        public Min(IDoubleSeries source) : base(source)
        {
            this.period = 14;
        }

        public Min(Quotes quotes) : base(quotes)
        {
            this.period = 14;
        }

        protected  override double Calculate(int current)
        {
            double maxValue = double.MaxValue;
            for (int i = Math.Max(0, (current - this.Period) + 1); i <= current; i++)
            {
                maxValue = Math.Min(maxValue, base.Source[i]);
            }
            return maxValue;
        }

        public override string ToString()
        {
            return string.Concat(new object[] { "MIN(", this.Period, ", ", PriceType.All[this.BasedOn].Name, ")" });
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
                return true;
            }
        }

        public override string Name
        {
            get
            {
                return "Minimum";
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
                if (this.period <= 0)
                {
                    throw new ArgumentOutOfRangeException("Period", value, "Indicator.Min: property out of range");
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

