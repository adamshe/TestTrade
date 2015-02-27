namespace Steema.TeeChart.Functions
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.Collections;

    public class Cumulative : Function
    {
        public Cumulative() : this(null)
        {
        }

        public Cumulative(Chart c) : base(c)
        {
            base.dPeriod = 1.0;
        }

        public override double Calculate(Series sourceSeries, int firstIndex, int lastIndex)
        {
            double num = (firstIndex > 0) ? base.series.mandatory.Last : 0.0;
            if (firstIndex >= 0)
            {
                return (num + base.ValueList(sourceSeries)[firstIndex]);
            }
            return num;
        }

        public override double CalculateMany(ArrayList sourceSeriesList, int valueIndex)
        {
            double num = (valueIndex == 0) ? 0.0 : base.series.mandatory[valueIndex - 1];
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
            return Texts.FunctionCumulative;
        }
    }
}

