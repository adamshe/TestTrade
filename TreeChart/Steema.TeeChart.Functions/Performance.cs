namespace Steema.TeeChart.Functions
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;

    public class Performance : Moving
    {
        public Performance() : this(null)
        {
        }

        public Performance(Chart c) : base(c)
        {
        }

        public override double Calculate(Series s, int firstIndex, int lastIndex)
        {
            if (firstIndex == -1)
            {
                lastIndex = s.Count - 1;
            }
            ValueList list = base.ValueList(s);
            if (list.Value[0] != 0.0)
            {
                return (((list.Value[lastIndex] - list.Value[0]) * 100.0) / list.Value[0]);
            }
            return 0.0;
        }

        public override string Description()
        {
            return Texts.FunctionPerf;
        }
    }
}

