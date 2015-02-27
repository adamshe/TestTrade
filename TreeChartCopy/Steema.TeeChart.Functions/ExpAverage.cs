namespace Steema.TeeChart.Functions
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;

    public class ExpAverage : Moving
    {
        private double weight = 0.2;

        public override double Calculate(Series s, int firstIndex, int lastIndex)
        {
            ValueList list = base.ValueList(s);
            double num = list[lastIndex];
            if (lastIndex <= 0)
            {
                return num;
            }
            return ((list[lastIndex - 1] * (1.0 - this.weight)) + (num * this.weight));
        }

        public override string Description()
        {
            return Texts.FunctionExpAverage;
        }

        [Description("Performs an Exponential average based on its DataSource values."), DefaultValue((double) 0.2)]
        public double Weight
        {
            get
            {
                return this.weight;
            }
            set
            {
                if ((value < 0.0) || (value > 1.0))
                {
                    throw new TeeChartException(Texts.ExpAverageWeight);
                }
                if (this.weight != value)
                {
                    this.weight = value;
                    base.Recalculate();
                }
            }
        }
    }
}

