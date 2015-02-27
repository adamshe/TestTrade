namespace Steema.TeeChart.Functions
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using Steema.TeeChart.Styles;
    using System;
    using System.Runtime.InteropServices;

    public class CrossPoints : Function
    {
        private ValueList tmp1;
        private ValueList tmp2;
        private ValueList tmpX1;
        private ValueList tmpX2;

        public CrossPoints() : this(null)
        {
        }

        public CrossPoints(Chart c) : base(c)
        {
            base.CanUsePeriod = false;
        }

        public override void AddPoints(Array source)
        {
            if (source.Length > 0)
            {
                Series series = (Series) source.GetValue(0);
                if (series.Count > 0)
                {
                    this.DoCalculation(series, series.notMandatory);
                }
            }
        }

        public override string Description()
        {
            return Texts.FunctionCross;
        }

        protected override void DoCalculation(Series source, ValueList notMandatorySource)
        {
            if (base.Series.DataSourceArray().Count > 1)
            {
                source = (Series) base.Series.DataSourceArray()[0];
                this.tmp1 = base.ValueList(source);
                this.tmpX1 = notMandatorySource;
                Series s = (Series) base.Series.DataSourceArray()[1];
                this.tmp2 = base.ValueList(s);
                this.tmpX2 = s.notMandatory;
                base.Series.Clear();
                if ((this.tmpX1.Count > 1) && (this.tmpX2.Count > 1))
                {
                    int num = 0;
                    int num2 = 0;
                    do
                    {
                        double num3;
                        double num4;
                        if (this.LinesCross(num, num2, out num3, out num4))
                        {
                            base.Series.Add(num3, num4);
                        }
                        if (this.tmpX2.Value[num2 + 1] < this.tmpX1.Value[num + 1])
                        {
                            num2++;
                        }
                        else
                        {
                            num++;
                        }
                    }
                    while ((num < this.tmpX1.Count) && (num2 < this.tmpX2.Count));
                }
            }
        }

        private bool LinesCross(int index1, int index2, out double x, out double y)
        {
            return Graphics3D.CrossingLines(this.tmpX1.Value[index1], this.tmp1.Value[index1], this.tmpX1.Value[index1 + 1], this.tmp1.Value[index1 + 1], this.tmpX2.Value[index2], this.tmp2.Value[index2], this.tmpX2.Value[index2 + 1], this.tmp2.Value[index2 + 1], out x, out y);
        }
    }
}

