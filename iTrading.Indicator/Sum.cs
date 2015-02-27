using iTrading.Core.IndicatorBase;
using System;
using iTrading.Core.Data;

namespace iTrading.Indicator
{

    public class Sum : IndicatorBase
    {
        private int period;

        public Sum(IDoubleSeries source) : base(source)
        {
            this.period = 14;
        }

        public Sum(Quotes quotes) : base(quotes)
        {
            this.period = 14;
        }

        protected  override double Calculate(int current)
        {
            return ((base.Source[current] + ((current > 0) ? base[current - 1] : 0.0)) - ((current >= this.Period) ? base.Source[current - this.Period] : 0.0));
        }

        public override string ToString()
        {
            return string.Concat(new object[] { "SUM(", this.Period, ", ", PriceType.All[this.BasedOn].Name, ")" });
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
                return "Sum";
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
                    throw new ArgumentOutOfRangeException("Period", value, "Indicator.Sum: property out of range");
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

