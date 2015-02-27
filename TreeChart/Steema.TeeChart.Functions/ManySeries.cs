namespace Steema.TeeChart.Functions
{
    using Steema.TeeChart.Styles;
    using System;
    using System.Collections;

    public class ManySeries : Function
    {
        public override double CalculateMany(ArrayList sourceSeries, int valueIndex)
        {
            bool flag = true;
            double result = 0.0;
            for (int i = 0; i < sourceSeries.Count; i++)
            {
                Steema.TeeChart.Styles.ValueList list = base.ValueList((Series) sourceSeries[i]);
                if (list.Count > valueIndex)
                {
                    if (flag)
                    {
                        result = list[valueIndex];
                        flag = false;
                    }
                    else
                    {
                        result = this.CalculateValue(result, list[valueIndex]);
                    }
                }
            }
            return result;
        }

        protected virtual double CalculateValue(double result, double value)
        {
            return 0.0;
        }
    }
}

