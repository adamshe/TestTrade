namespace Steema.TeeChart.Functions
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;

    public class Moving : Function
    {
        public Moving() : this(null)
        {
        }

        public Moving(Chart c) : base(c)
        {
            base.dPeriod = 1.0;
        }

        protected override void DoCalculation(Series source, ValueList notMandatorySource)
        {
            int num = Utils.Round(base.dPeriod);
            for (int i = num - 1; i < source.Count; i++)
            {
                base.AddFunctionXY(source.yMandatory, notMandatorySource[i], this.Calculate(source, (i - num) + 1, i));
            }
        }
    }
}

