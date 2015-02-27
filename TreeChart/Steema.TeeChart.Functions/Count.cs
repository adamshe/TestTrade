namespace Steema.TeeChart.Functions
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.Collections;

    public class Count : Function
    {
        public override double Calculate(Series sourceSeries, int firstIndex, int lastIndex)
        {
            return ((firstIndex == -1) ? ((double) base.ValueList(sourceSeries).Count) : ((double) ((lastIndex - firstIndex) + 1)));
        }

        public override double CalculateMany(ArrayList sourceSeriesList, int valueIndex)
        {
            double num = 0.0;
            for (int i = 0; i < sourceSeriesList.Count; i++)
            {
                if (base.ValueList((Series) sourceSeriesList[i]).Count > valueIndex)
                {
                    num++;
                }
            }
            return num;
        }

        public override string Description()
        {
            return Texts.FunctionCount;
        }
    }
}

