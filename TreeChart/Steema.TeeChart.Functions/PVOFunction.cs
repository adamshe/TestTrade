namespace Steema.TeeChart.Functions
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;

    public class PVOFunction : Function
    {
        private ExpMovAverage FExpMovAve1;
        private ExpMovAverage FExpMovAve2;
        private Line FExpSeries1;
        private Line FExpSeries2;
        private bool FPercent;

        public PVOFunction() : this(null)
        {
        }

        public PVOFunction(Chart c) : base(c)
        {
            base.Period = 12.0;
            base.CanUsePeriod = false;
            this.FPercent = true;
            this.FExpSeries1 = new Line();
            this.FExpSeries2 = new Line();
            this.FExpMovAve1 = new ExpMovAverage();
            this.FExpMovAve1.Period = base.Period;
            this.FExpSeries1.Function = this.FExpMovAve1;
            this.FExpMovAve2 = new ExpMovAverage();
            this.FExpMovAve2.Period = 26.0;
            this.FExpSeries2.Function = this.FExpMovAve2;
        }

        public override void AddPoints(Array source)
        {
            if ((!base.updating && (source != null)) && ((source.Length > 0) && (base.Period > 0.0)))
            {
                base.Series.Clear();
                this.FExpMovAve1.Period = base.Period;
                this.FExpMovAve1.AddPoints(source);
                this.FExpMovAve2.AddPoints(source);
                Series series = (Series) source.GetValue(0);
                for (int i = 0; i < series.Count; i++)
                {
                    double num2 = this.FExpSeries1.mandatory[i];
                    if (this.FPercent)
                    {
                        if (num2 != 0.0)
                        {
                            num2 = (num2 - this.FExpSeries2.mandatory[i]) / num2;
                        }
                        else
                        {
                            num2 = this.FExpSeries2.mandatory[i];
                        }
                    }
                    else
                    {
                        num2 -= this.FExpSeries2.mandatory[i];
                    }
                    base.Series.Add(series.notMandatory[i], (double) (100.0 * num2));
                }
            }
        }

        public override string Description()
        {
            return Texts.FunctionPVO;
        }

        [Description("Controls if the difference of two moving averages will be percentual or not."), DefaultValue(true)]
        public bool Percentage
        {
            get
            {
                return this.FPercent;
            }
            set
            {
                if (this.FPercent != value)
                {
                    this.FPercent = value;
                }
                base.Recalculate();
            }
        }

        [Description("Defines the period of the second exponential moving average."), DefaultValue(0x1a)]
        public int Period2
        {
            get
            {
                return (int) this.FExpMovAve2.Period;
            }
            set
            {
                this.FExpMovAve2.Period = value;
                base.Recalculate();
            }
        }
    }
}

