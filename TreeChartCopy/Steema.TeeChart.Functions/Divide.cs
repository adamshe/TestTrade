namespace Steema.TeeChart.Functions
{
    using Steema.TeeChart;
    using System;

    public class Divide : ManySeries
    {
        protected override double CalculateValue(double result, double value)
        {
            if (value != 0.0)
            {
                return (result / value);
            }
            return result;
        }

        public override string Description()
        {
            return Texts.FunctionDivide;
        }
    }
}

