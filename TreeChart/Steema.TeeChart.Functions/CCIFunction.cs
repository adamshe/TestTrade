namespace Steema.TeeChart.Functions
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;

    public class CCIFunction : Function
    {
        private Line FAveSeries;
        private double FConstant;
        private MovingAverage FMovAve;
        private Line FTypical;
        private int tmpPeriod;

        public CCIFunction() : this(null)
        {
        }

        public CCIFunction(Chart c) : base(c)
        {
            base.Period = 20.0;
            base.CanUsePeriod = false;
            this.FConstant = 0.015;
            this.FTypical = new Line();
            this.FMovAve = new MovingAverage();
            this.FAveSeries = new Line();
            this.FAveSeries.Function = this.FMovAve;
        }

        public override void AddPoints(Array source)
        {
            if ((!base.updating && (source != null)) && (source.Length > 0))
            {
                Series series = (Series) source.GetValue(0);
                if ((series.Count > 0) && (series is OHLC))
                {
                    base.Series.Clear();
                    this.tmpPeriod = (int) Math.Round(base.Period, 0);
                    if (this.tmpPeriod > 0)
                    {
                        this.CalculateMovAve(series);
                        double constant = this.Constant;
                        if (constant == 0.0)
                        {
                            constant = 1.0;
                        }
                        for (int i = this.tmpPeriod; i < series.Count; i++)
                        {
                            double y = (this.FTypical.mandatory[i] - this.FAveSeries.mandatory[i]) / (constant * this.MeanDeviation(i));
                            base.Series.Add(series.notMandatory[i], y);
                        }
                    }
                }
            }
        }

        private void CalculateMovAve(Series Source)
        {
            int num;
            this.FTypical.Clear();
            this.FMovAve.Period = this.tmpPeriod;
            this.FTypical.BeginUpdate();
            for (num = 0; num < Source.Count; num++)
            {
                this.FTypical.Add((double) (((((OHLC) Source).HighValues[num] + ((OHLC) Source).LowValues[num]) + ((OHLC) Source).CloseValues[num]) * 0.33333333333333331));
            }
            this.FTypical.EndUpdate();
            for (num = this.tmpPeriod; num < this.FTypical.Count; num++)
            {
                this.FAveSeries.Add(this.FMovAve.Calculate(this.FTypical, num - this.tmpPeriod, num));
            }
        }

        public override string Description()
        {
            return Texts.FunctionCCI;
        }

        private double MeanDeviation(int Index)
        {
            double num = 0.0;
            for (int i = Index; i >= (Index - this.tmpPeriod); i--)
            {
                num += Math.Abs((double) (this.FAveSeries.mandatory[Index - this.tmpPeriod] - this.FTypical.mandatory[i]));
            }
            return (num / ((double) this.tmpPeriod));
        }

        [Description("Value by which to multiply mean deviation."), DefaultValue((double) 0.015)]
        public double Constant
        {
            get
            {
                return this.FConstant;
            }
            set
            {
                if (this.FConstant != value)
                {
                    this.FConstant = value;
                }
                base.Recalculate();
            }
        }
    }
}

