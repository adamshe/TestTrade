using iTrading.Core.IndicatorBase;

namespace iTrading.Indicator
{
    using System;
    using iTrading.Core.Data;

    public class Atr : IndicatorBase
    {
        private int period;

        public Atr(Quotes quotes) : base(quotes)
        {
            this.period = 14;
        }

        protected override double Calculate(int current)
        {
            double num = base.High[current] - base.Low[current];
            if (current == 0)
            {
                return num;
            }
            num = Math.Max(Math.Abs((double) (base.Low[current] - base.Close[current - 1])), Math.Max(num, Math.Abs((double) (base.High[current] - base.Close[current - 1]))));
            return ((((Math.Min(current + 1, this.Period) - 1) * base[current - 1]) + num) / ((double) Math.Min(current + 1, this.Period)));
        }

        public override string ToString()
        {
            return ("ATR(" + this.Period + ")");
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
                return "Average True Range";
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
                    throw new ArgumentOutOfRangeException("Period", value, "Indicator.Atr: property out of range");
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

