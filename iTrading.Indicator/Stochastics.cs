using iTrading.Core.IndicatorBase;

namespace iTrading.Indicator
{
    using System;
    using iTrading.Core.Data;

    public class Stochastics : IndicatorBase
    {
        private DoubleSeries den;
        private DoubleSeries fastK;
        private Max max;
        private Min min;
        private DoubleSeries nom;
        private int periodD;
        private int periodK;
        private Sma smaD;
        private int smooth;
        private Sum sumDen;
        private Sum sumNom;

        public Stochastics(Quotes quotes) : base(2, quotes)
        {
            this.periodD = 3;
            this.periodK = 14;
            this.smooth = 7;
            this.den = new DoubleSeries();
            this.fastK = new DoubleSeries();
            this.max = new Max(base.Quotes.High);
            this.min = new Min(base.Quotes.Low);
            this.nom = new DoubleSeries();
            this.smaD = new Sma(base.GetSeries(1));
            this.sumDen = new Sum(this.den);
            this.sumNom = new Sum(this.nom);
        }

        protected  override double Calculate(int current)
        {
            this.den[current] = this.max[current] - this.min[current];
            this.nom[current] = base.Close[current] - this.min[current];
            this.fastK[current] = (100.0 * this.nom[current]) / this.den[current];
            base.Add(1, (100.0 * this.sumNom[current]) / this.sumDen[current]);
            return this.smaD[current];
        }

        public override string ToString()
        {
            return string.Concat(new object[] { "Stoch(", this.PeriodK, ", ", this.PeriodD, ",", this.Smooth, ")" });
        }

        public IDoubleSeries D
        {
            get
            {
                return base.GetSeries(0);
            }
        }

        public override bool IsPriceIndicator
        {
            get
            {
                return false;
            }
        }

        public IDoubleSeries K
        {
            get
            {
                return base.GetSeries(1);
            }
        }

        public override string Name
        {
            get
            {
                return "Stochastics";
            }
        }

        [Parameter(1, 0x7fffffff, "D periods")]
        public int PeriodD
        {
            get
            {
                return this.periodD;
            }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("PeriodD", value, "Indicator.Stochastics: property out of range");
                }
                this.periodD = this.sumDen.Period = this.sumNom.Period = value;
            }
        }

        [Parameter(1, 0x7fffffff, "K periods")]
        public int PeriodK
        {
            get
            {
                return this.periodK;
            }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("PeriodK", value, "Indicator.Stochastics: property out of range");
                }
                this.periodK = this.max.Period = this.min.Period = value;
            }
        }

        [Parameter(1, 0x7fffffff, "Smooth")]
        public int Smooth
        {
            get
            {
                return this.smooth;
            }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("Smooth", value, "Indicator.Stochastics: property out of range");
                }
                this.smooth = this.smaD.Period = value;
            }
        }

        public override int UnstablePeriod
        {
            get
            {
                return Math.Max(this.Smooth, Math.Max(this.PeriodK, this.PeriodD));
            }
        }
    }
}

