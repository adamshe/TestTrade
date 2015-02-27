namespace Steema.TeeChart.Functions
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;

    public class MovingAverage : Moving
    {
        private bool FWeighted;
        private bool FWeightedIndex;

        public MovingAverage() : this(null)
        {
        }

        public MovingAverage(Chart c) : base(c)
        {
            this.FWeighted = false;
            this.FWeightedIndex = false;
        }

        public override double Calculate(Series sourceSeries, int firstIndex, int lastIndex)
        {
            double num5 = 0.0;
            double num2 = 0.0;
            ValueList list = base.ValueList(sourceSeries);
            for (int i = firstIndex; i <= lastIndex; i++)
            {
                double num4;
                double num3 = list.Value[i];
                if (this.FWeighted)
                {
                    num4 = sourceSeries.XValues.Value[i];
                    num5 += num3 * num4;
                    num2 += num4;
                }
                else if (this.FWeightedIndex)
                {
                    num4 = (i - firstIndex) + 1;
                    num5 += num3 * num4;
                    num2 += num4;
                }
                else
                {
                    num5 += num3;
                }
            }
            if (this.FWeighted || this.FWeightedIndex)
            {
                if (num2 != 0.0)
                {
                    return (num5 / num2);
                }
                return 0.0;
            }
            return (num5 / ((double) ((lastIndex - firstIndex) + 1)));
        }

        public override string Description()
        {
            return Texts.FunctionMovingAverage;
        }

        [DefaultValue(false), Description("Set Weighted to true to calculate Mov. Avg. using the corresponding X point values.")]
        public bool Weighted
        {
            get
            {
                return this.FWeighted;
            }
            set
            {
                if (this.FWeighted != value)
                {
                    this.FWeighted = value;
                }
                base.Recalculate();
            }
        }

        [Description("Set WeightedIndex to true to calculate Mov. Avg. using the source points index as the \"weight\"."), DefaultValue(false)]
        public bool WeightedIndex
        {
            get
            {
                return this.FWeightedIndex;
            }
            set
            {
                if (this.FWeightedIndex != value)
                {
                    this.FWeightedIndex = value;
                }
                base.Recalculate();
            }
        }
    }
}

