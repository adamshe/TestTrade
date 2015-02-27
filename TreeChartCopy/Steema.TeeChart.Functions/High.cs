namespace Steema.TeeChart.Functions
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.Collections;

    public class High : Function
    {
        public override double Calculate(Series sourceSeries, int firstIndex, int lastIndex)
        {
            Steema.TeeChart.Styles.ValueList list = base.ValueList(sourceSeries);
            if (firstIndex == -1)
            {
                return list.Maximum;
            }
            double num = list[firstIndex];
            for (int i = firstIndex + 1; i <= lastIndex; i++)
            {
                double num3 = list[i];
                if (num3 > num)
                {
                    num = num3;
                }
            }
            return num;
        }

        public override double CalculateMany(ArrayList sourceSeriesList, int valueIndex)
        {
            double num = 0.0;
            for (int i = 0; i < sourceSeriesList.Count; i++)
            {
                Steema.TeeChart.Styles.ValueList list = base.ValueList((Series) sourceSeriesList[i]);
                if (list.Count > valueIndex)
                {
                    double num3 = list[valueIndex];
                    if ((i == 0) || (num3 > num))
                    {
                        num = num3;
                    }
                }
            }
            return num;
        }

        public override string Description()
        {
            return Texts.FunctionHigh;
        }
    }
}

