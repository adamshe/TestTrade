namespace Steema.TeeChart.Functions
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    public class Smoothing : Function
    {
        private int factor;
        private bool interpolate;

        public Smoothing() : this(null)
        {
        }

        public Smoothing(Chart c) : base(c)
        {
            this.interpolate = true;
            this.factor = 4;
            base.CanUsePeriod = false;
            base.dPeriod = 1.0;
        }

        public override void AddPoints(Array source)
        {
            if ((!base.updating && (source != null)) && (source.Length > 0))
            {
                Series s = (Series) source.GetValue(0);
                base.Series.Clear();
                if (s.Count > 0)
                {
                    ValueList list = base.ValueList(s);
                    Spline spline = new Spline();
                    for (int i = 0; i < s.Count; i++)
                    {
                        spline.AddPoint(s.XValues.Value[i], list.Value[i]);
                        spline.SetKnuckle(i, false);
                    }
                    spline.Interpolated = this.interpolate;
                    spline.Fragments = s.Count * this.factor;
                    for (int j = 0; j <= spline.Fragments; j++)
                    {
                        PointF tf = spline.Value(((double) j) / ((double) spline.Fragments));
                        if (base.Series.yMandatory)
                        {
                            base.Series.Add((double) tf.X, (double) tf.Y);
                        }
                        else
                        {
                            base.Series.Add((double) tf.Y, (double) tf.X);
                        }
                    }
                }
            }
        }

        public override string Description()
        {
            return Texts.FunctionSmooth;
        }

        [DefaultValue(4), Description("Gets or sets number of times the resulting smooth points are in quantity compared to source points.")]
        public int Factor
        {
            get
            {
                return this.factor;
            }
            set
            {
                if (this.factor != value)
                {
                    this.factor = Math.Max(1, value);
                    base.Recalculate();
                }
            }
        }

        [DefaultValue(true), Description("When true, resulting smooth curves will pass exactly over source points.")]
        public bool Interpolate
        {
            get
            {
                return this.interpolate;
            }
            set
            {
                if (this.interpolate != value)
                {
                    this.interpolate = value;
                    base.Recalculate();
                }
            }
        }
    }
}

