namespace Steema.TeeChart.Functions
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.Collections;

    public class Add : Function
    {
        public override double Calculate(Series sourceSeries, int firstIndex, int lastIndex)
        {
            Steema.TeeChart.Styles.ValueList list = base.ValueList(sourceSeries);
            if (firstIndex == -1)
            {
                return list.Total;
            }
            double num = 0.0;
            for (int i = firstIndex; i <= lastIndex; i++)
            {
                num += list[i];
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
                    num += list[valueIndex];
                }
            }
            return num;
        }

        public override string Description()
        {
            return Texts.FunctionAdd;
        }
    }
}

