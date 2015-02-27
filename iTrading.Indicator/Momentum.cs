using iTrading.Core.IndicatorBase;

namespace iTrading.Indicator
{
    using System;
    using iTrading.Core.Data;

    public class Momentum : IndicatorBase
    {
        private int period;

        public Momentum(IDoubleSeries source) : base(source)
        {
            this.period = 14;
        }

        public Momentum(Quotes quotes) : base(quotes)
        {
            this.period = 14;
        }

        protected override double Calculate(int current)
        {
            if (current == 0)
            {
                return 0.0;
            }
            return (base.Source[current] - base.Source[current - Math.Min(current, this.Period)]);
        }

        public override string ToString()
        {
            return string.Concat(new object[] { "MOM(", this.Period, ", ", PriceType.All[this.BasedOn].Name, ")" });
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
                return "Momentum";
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
                    throw new ArgumentOutOfRangeException("Period", value, "Indicator.Momentum: property out of range");
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

