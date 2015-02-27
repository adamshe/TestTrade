using iTrading.Core.IndicatorBase;

namespace iTrading.Indicator
{
    using System;
    using iTrading.Core.Data;

    public class Dmi : IndicatorBase
    {
        private DoubleSeries dmMinus;
        private DoubleSeries dmPlus;
        private int period;
        private Sma smaDmMinus;
        private Sma smaDmPlus;
        private Sma smaTr;
        private DoubleSeries tr;

        public Dmi(IDoubleSeries source) : base(source)
        {
            this.period = 14;
            this.dmMinus = new DoubleSeries();
            this.dmPlus = new DoubleSeries();
            this.tr = new DoubleSeries();
            this.smaDmMinus = new Sma(this.dmMinus);
            this.smaDmPlus = new Sma(this.dmPlus);
            this.smaTr = new Sma(this.tr);
        }

        public Dmi(Quotes quotes) : base(quotes)
        {
            this.period = 14;
            this.dmMinus = new DoubleSeries();
            this.dmPlus = new DoubleSeries();
            this.tr = new DoubleSeries();
            this.smaDmMinus = new Sma(this.dmMinus);
            this.smaDmPlus = new Sma(this.dmPlus);
            this.smaTr = new Sma(this.tr);
        }

        protected  override double Calculate(int current)
        {
            double num = base.High[current] - base.Low[current];
            if (current == 0)
            {
                this.dmMinus[current] = 0.0;
                this.dmPlus[current] = 0.0;
                this.tr[current] = num;
            }
            else
            {
                this.dmMinus[current] = ((base.Low[current - 1] - base.Low[current]) > (base.High[current] - base.High[current - 1])) ? Math.Max((double) (base.Low[current - 1] - base.Low[current]), (double) 0.0) : 0.0;
                this.dmPlus[current] = ((base.High[current] - base.High[current - 1]) > (base.Low[current - 1] - base.Low[current])) ? Math.Max((double) (base.High[current] - base.High[current - 1]), (double) 0.0) : 0.0;
                this.tr[current] = Math.Max(num, Math.Max(Math.Abs((double) (base.High[current] - base.Close[current - 1])), Math.Abs((double) (base.Low[current] - base.Close[current - 1]))));
            }
            double num2 = (this.smaTr[current] == 0.0) ? 0.0 : (this.smaDmPlus[current] / this.smaTr[current]);
            double num3 = (this.smaTr[current] == 0.0) ? 0.0 : (this.smaDmMinus[current] / this.smaTr[current]);
            if ((num2 + num3) != 0.0)
            {
                return ((num2 - num3) / (num2 + num3));
            }
            return 0.0;
        }

        public override string ToString()
        {
            return ("DMI(" + this.Period + ")");
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
                return "Directional Movement Indicator";
            }
        }

        [Parameter(1, 0x7fffffff, "# periods")]
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
                    throw new ArgumentOutOfRangeException("Period", value, "Indicator.Dmi: property out of range");
                }
                this.period = this.smaDmMinus.Period = this.smaDmPlus.Period = this.smaTr.Period = value;
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

