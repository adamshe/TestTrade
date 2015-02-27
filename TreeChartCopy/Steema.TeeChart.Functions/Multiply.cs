namespace Steema.TeeChart.Functions
{
    using Steema.TeeChart;
    using System;

    public class Multiply : ManySeries
    {
        protected override double CalculateValue(double result, double value)
        {
            return (result * value);
        }

        public override string Description()
        {
            return Texts.FunctionMultiply;
        }
    }
}

