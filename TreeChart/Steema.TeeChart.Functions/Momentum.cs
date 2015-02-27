namespace Steema.TeeChart.Functions
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;

    public class Momentum : Moving
    {
        public override double Calculate(Series s, int firstIndex, int lastIndex)
        {
            if (firstIndex == -1)
            {
                firstIndex = 0;
                lastIndex = s.Count - 1;
            }
            ValueList list = base.ValueList(s);
            return (list[lastIndex] - list[firstIndex]);
        }

        public override string Description()
        {
            return Texts.FunctionMomentum;
        }
    }
}

