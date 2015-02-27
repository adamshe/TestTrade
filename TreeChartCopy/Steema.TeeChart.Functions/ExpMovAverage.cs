namespace Steema.TeeChart.Functions
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;

    public class ExpMovAverage : Function
    {
        public ExpMovAverage() : this(null)
        {
        }

        public ExpMovAverage(Chart c) : base(c)
        {
            base.CanUsePeriod = false;
            base.dPeriod = 10.0;
        }

        public override string Description()
        {
            return Texts.FunctionExpMovAve;
        }

        protected override void DoCalculation(Series source, ValueList notMandatorySource)
        {
            base.Series.Clear();
            if ((base.Period > 0.0) && (source.Count > 1))
            {
                ValueList list = base.ValueList(source);
                double num = 2.0 / (base.Period + 1.0);
                double y = list.Value[0];
                base.Series.Add(notMandatorySource.Value[0], y);
                for (int i = 1; i < source.Count; i++)
                {
                    y = (list.Value[i] * num) + (y * (1.0 - num));
                    base.Series.Add(notMandatorySource.Value[i], y);
                }
            }
        }
    }
}

