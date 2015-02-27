namespace Steema.TeeChart.Functions
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;

    public class MomentumDivision : Moving
    {
        public override double Calculate(Series s, int firstIndex, int lastIndex)
        {
            if (firstIndex == -1)
            {
                firstIndex = 0;
                lastIndex = s.Count - 1;
            }
            ValueList list = base.ValueList(s);
            if (list[firstIndex] != 0.0)
            {
                return ((100.0 * list[lastIndex]) / list[firstIndex]);
            }
            return 0.0;
        }

        public override string Description()
        {
            return Texts.FunctionMomentumDiv;
        }
    }
}

