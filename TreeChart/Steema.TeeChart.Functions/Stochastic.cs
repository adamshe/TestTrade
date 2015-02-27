namespace Steema.TeeChart.Functions
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;

    public class Stochastic : Moving
    {
        private double[] dens;
        private double[] nums;

        public Stochastic() : this(null)
        {
        }

        public Stochastic(Chart c) : base(c)
        {
        }

        public override void AddPoints(Array source)
        {
            this.nums = new double[((Series) source.GetValue(0)).Count];
            this.dens = new double[((Series) source.GetValue(0)).Count];
            base.AddPoints(source);
        }

        public override double Calculate(Series s, int firstIndex, int lastIndex)
        {
            double num = 0.0;
            ValueList yValueList = s.GetYValueList("LOW");
            ValueList list2 = s.GetYValueList("HIGH");
            double num2 = yValueList.Value[firstIndex];
            double num3 = list2.Value[firstIndex];
            for (int i = firstIndex; i <= lastIndex; i++)
            {
                if (yValueList.Value[i] < num2)
                {
                    num2 = yValueList.Value[i];
                }
                if (list2.Value[i] > num3)
                {
                    num3 = list2.Value[i];
                }
            }
            this.nums[lastIndex] = base.ValueList(s).Value[lastIndex] - num2;
            this.dens[lastIndex] = num3 - num2;
            if (num3 != num2)
            {
                num = 100.0 * (this.nums[lastIndex] / this.dens[lastIndex]);
            }
            return num;
        }

        public override string Description()
        {
            return Texts.FunctionStochastic;
        }
    }
}

