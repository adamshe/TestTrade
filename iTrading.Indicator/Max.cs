using System;
using iTrading.Core.IndicatorBase;
using iTrading.Core.Data;

namespace iTrading.Indicator
{
    public class Max : IndicatorBase
    {
        private int period;

        public Max(IDoubleSeries source) : base(source)
        {
            period = 14;
        }

        public Max(Quotes quotes) : base(quotes)
        {
            period = 14;
        }

        [Parameter(PriceTypeId.Close, "Based on")]
        public PriceTypeId BasedOn
        {
            get { return base.DefaultBasedOn; }
            set { base.DefaultBasedOn = value; }
        }

        public override bool IsPriceIndicator
        {
            get { return true; }
        }

        public override string Name
        {
            get { return "Maximum"; }
        }

        [Parameter(1, 0x7fffffff, "# of periods")]
        public int Period
        {
            get { return period; }
            set
            {
                if (period <= 0)
                {
                    throw new ArgumentOutOfRangeException("Period", value, "Indicator.Max: property out of range");
                }
                period = value;
            }
        }

        public override int UnstablePeriod
        {
            get { return Period; }
        }

        protected override double Calculate(int current)
        {
            double minValue = double.MinValue;
            for (int i = Math.Max(0, (current - Period) + 1); i <= current; i++)
            {
                minValue = Math.Max(minValue, base.Source[i]);
            }
            return minValue;
        }

        public override string ToString()
        {
            return string.Concat(new object[] {"MAX(", Period, ", ", PriceType.All[BasedOn].Name, ")"});
        }
    }
}