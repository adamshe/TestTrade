namespace Steema.TeeChart.Functions
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.Collections;
    using System.ComponentModel;

    public class RootMeanSquare : Function
    {
        private bool complete;
        private int INumPoints;
        private double ISum2;

        public RootMeanSquare() : this(null)
        {
        }

        public RootMeanSquare(Chart c) : base(c)
        {
        }

        private void Accumulate(double value)
        {
            this.ISum2 += value * value;
        }

        public override double Calculate(Series s, int firstIndex, int lastIndex)
        {
            if (firstIndex == -1)
            {
                firstIndex = 0;
                this.INumPoints = s.Count;
                lastIndex = this.INumPoints - 1;
            }
            else
            {
                this.INumPoints = (lastIndex - firstIndex) + 1;
            }
            if (this.INumPoints <= 1)
            {
                return 0.0;
            }
            this.ISum2 = 0.0;
            Steema.TeeChart.Styles.ValueList list = base.ValueList(s);
            for (int i = firstIndex; i <= lastIndex; i++)
            {
                this.Accumulate(list.Value[i]);
            }
            return this.CalculateRMS();
        }

        public override double CalculateMany(ArrayList sourceSeries, int valueIndex)
        {
            if (sourceSeries.Count <= 0)
            {
                return 0.0;
            }
            this.INumPoints = 0;
            this.ISum2 = 0.0;
            for (int i = 0; i < sourceSeries.Count; i++)
            {
                Steema.TeeChart.Styles.ValueList list = base.ValueList((Series) sourceSeries[i]);
                if (list.Count > valueIndex)
                {
                    this.Accumulate(list.Value[valueIndex]);
                    this.INumPoints++;
                }
            }
            if (this.INumPoints <= 1)
            {
                return 0.0;
            }
            return this.CalculateRMS();
        }

        private double CalculateRMS()
        {
            return Math.Sqrt(this.ISum2 / (this.Complete ? ((double) this.INumPoints) : ((double) (this.INumPoints - 1))));
        }

        public override string Description()
        {
            return Texts.FunctionRMS;
        }

        [DefaultValue(false), Description("Defines the RMS formula.")]
        public bool Complete
        {
            get
            {
                return this.complete;
            }
            set
            {
                if (this.complete != value)
                {
                    this.complete = value;
                    base.Recalculate();
                }
            }
        }
    }
}

