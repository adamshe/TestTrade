namespace Steema.TeeChart.Functions
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;

    public class DownSampling : Function
    {
        private DownSamplingMethod method;
        private int reducedsize;
        private double tolerance;

        public DownSampling() : this(null)
        {
        }

        public DownSampling(Chart c) : base(c)
        {
            base.CanUsePeriod = false;
            base.dPeriod = 1.0;
            this.tolerance = 1.0;
            this.method = DownSamplingMethod.Average;
        }

        public override void AddPoints(Array source)
        {
            if ((!base.updating && (source != null)) && (source.Length > 0))
            {
                Series series = (Series) source.GetValue(0);
                base.Series.Clear();
                int count = series.Count;
                if (series.yMandatory == base.Series.yMandatory)
                {
                    base.Series.notMandatory.Order = ValueListOrder.Ascending;
                    base.Series.mandatory.Order = ValueListOrder.None;
                }
                else
                {
                    base.Series.notMandatory.Order = ValueListOrder.None;
                    base.Series.mandatory.Order = ValueListOrder.Ascending;
                }
                double[] rx = new double[count];
                double[] ry = new double[count];
                this.Reduce(series.notMandatory.Value, series.mandatory.Value, count, ref rx, ref ry);
                base.Series.notMandatory.Value = rx;
                base.Series.notMandatory.Count = this.reducedsize;
                base.Series.mandatory.Value = ry;
                base.Series.mandatory.Count = this.reducedsize;
            }
        }

        public override string Description()
        {
            return Texts.FunctionDownSampling;
        }

        private void Reduce(double[] x, double[] y, int N, ref double[] rx, ref double[] ry)
        {
            int index = 0;
            int num2 = index;
            this.reducedsize = 0;
            double num3 = 0.0;
            double num4 = 0.0;
            double num5 = 0.0;
            while (index < N)
            {
                num2 = index;
                num3 = y[index];
                num4 = y[index];
                num5 = y[index];
                while (Math.Abs((double) (x[num2 + 1] - x[index])) < this.tolerance)
                {
                    num2++;
                    num3 += y[num2];
                    if (y[num2] > num4)
                    {
                        num4 = y[num2];
                    }
                    if (y[num2] < num5)
                    {
                        num5 = y[num2];
                    }
                }
                if (this.method != DownSamplingMethod.MinMax)
                {
                    rx[this.reducedsize] = (x[num2] + x[index]) * 0.5;
                    if (this.method == DownSamplingMethod.Average)
                    {
                        ry[this.reducedsize] = num3 / ((double) ((num2 - index) + 1));
                    }
                    else if (this.method == DownSamplingMethod.Max)
                    {
                        ry[this.reducedsize] = num4;
                    }
                    else if (this.method == DownSamplingMethod.Min)
                    {
                        ry[this.reducedsize] = num5;
                    }
                    this.reducedsize++;
                }
                else if (this.reducedsize < (rx.GetUpperBound(0) - 1))
                {
                    rx[this.reducedsize] = x[index];
                    rx[this.reducedsize + 1] = x[num2];
                    ry[this.reducedsize] = num5;
                    ry[this.reducedsize + 1] = num4;
                    this.reducedsize += 2;
                }
                index = num2 + 1;
            }
        }

        [Description("Defines reduction/downsampling method.")]
        public DownSamplingMethod Method
        {
            get
            {
                return this.method;
            }
            set
            {
                if (this.method != value)
                {
                    this.method = value;
                    base.Recalculate();
                }
            }
        }

        public int ReducedSize
        {
            get
            {
                return this.reducedsize;
            }
        }

        public double Tolerance
        {
            get
            {
                return this.tolerance;
            }
            set
            {
                if (this.tolerance != value)
                {
                    this.tolerance = Math.Max(0.0, value);
                    base.Recalculate();
                }
            }
        }
    }
}

