using iTrading.Core.IndicatorBase;

namespace iTrading.Indicator
{
    using System;
    using iTrading.Core.Data;

    public class Adx : IndicatorBase
    {
        private DoubleSeries dmMinus;
        private DoubleSeries dmPlus;
        private int period;
        private DoubleSeries sumDmMinus;
        private DoubleSeries sumDmPlus;
        private DoubleSeries sumTr;
        private DoubleSeries tr;

        public Adx(Quotes quotes) : base(quotes)
        {
            this.period = 14;
            this.dmPlus = new DoubleSeries();
            this.dmMinus = new DoubleSeries();
            this.sumDmPlus = new DoubleSeries();
            this.sumDmMinus = new DoubleSeries();
            this.sumTr = new DoubleSeries();
            this.tr = new DoubleSeries();
        }

        protected override double Calculate(int current)
        {
            double num = base.High[current] - base.Low[current];
            if (current == 0)
            {
                this.tr[current] = num;
                this.dmPlus[current] = 0.0;
                this.dmMinus[current] = 0.0;
                this.sumTr[current] = this.tr[current];
                this.sumDmPlus[current] = this.dmPlus[current];
                this.sumDmMinus[current] = this.dmMinus[current];
                return 50.0;
            }
            this.tr[current] = Math.Max(Math.Abs((double) (base.Low[current] - base.Close[current - 1])), Math.Max(num, Math.Abs((double) (base.High[current] - base.Close[current - 1]))));
            this.dmPlus[current] = ((base.High[current] - base.High[current - 1]) > (base.Low[current - 1] - base.Low[current])) ? Math.Max((double) (base.High[current] - base.High[current - 1]), (double) 0.0) : 0.0;
            this.dmMinus[current] = ((base.Low[current - 1] - base.Low[current]) > (base.High[current] - base.High[current - 1])) ? Math.Max((double) (base.Low[current - 1] - base.Low[current]), (double) 0.0) : 0.0;
            if (current < this.Period)
            {
                this.sumTr[current] = this.sumTr[current - 1] + this.tr[current];
                this.sumDmPlus[current] = this.sumDmPlus[current - 1] + this.dmPlus[current];
                this.sumDmMinus[current] = this.sumDmMinus[current - 1] + this.dmMinus[current];
            }
            else
            {
                this.sumTr[current] = (this.sumTr[current - 1] - (this.sumTr[current - 1] / ((double) this.Period))) + this.tr[current];
                this.sumDmPlus[current] = (this.sumDmPlus[current - 1] - (this.sumDmPlus[current - 1] / ((double) this.Period))) + this.dmPlus[current];
                this.sumDmMinus[current] = (this.sumDmMinus[current - 1] - (this.sumDmMinus[current - 1] / ((double) this.Period))) + this.dmMinus[current];
            }
            double num2 = 100.0 * ((this.sumTr[current] == 0.0) ? 0.0 : (this.sumDmPlus[current] / this.sumTr[current]));
            double num3 = 100.0 * ((this.sumTr[current] == 0.0) ? 0.0 : (this.sumDmMinus[current] / this.sumTr[current]));
            double num4 = Math.Abs((double) (num2 - num3));
            double num5 = num2 + num3;
            if (num5 != 0.0)
            {
                return ((100.0 * num4) / num5);
            }
            return 50.0;
        }

        public override string ToString()
        {
            return ("ADX(" + this.Period + ")");
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
                return "ADX";
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
                    throw new ArgumentOutOfRangeException("Period", value, "Indicator.Adx: property out of range");
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

