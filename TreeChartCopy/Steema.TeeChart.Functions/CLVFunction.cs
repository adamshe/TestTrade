namespace Steema.TeeChart.Functions
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;

    public class CLVFunction : Function
    {
        private bool FAccumulate;
        private Series FVolume;

        public CLVFunction() : this(null)
        {
        }

        public CLVFunction(Chart c) : base(c)
        {
            base.Period = 1.0;
            base.CanUsePeriod = false;
            this.FAccumulate = true;
        }

        public override void AddPoints(Array source)
        {
            bool flag = this.FVolume != null;
            if ((!base.updating && (source != null)) && (source.Length > 0))
            {
                Series series = (Series) source.GetValue(0);
                if ((series.Count > 0) && (series is OHLC))
                {
                    base.Series.Clear();
                    for (int i = 0; i < series.Count; i++)
                    {
                        double num2 = ((OHLC) series).HighValues[i];
                        double num3 = ((OHLC) series).CloseValues[i];
                        double num4 = ((OHLC) series).LowValues[i];
                        double y = ((num3 - num4) - (num2 - num3)) / (num2 - num4);
                        if (flag && (this.FVolume.Count > i))
                        {
                            y *= this.FVolume.mandatory[i];
                        }
                        if (this.Accumulate && (i > 0))
                        {
                            y += base.Series.mandatory.Last;
                        }
                        base.Series.Add(((OHLC) series).DateValues[i], y);
                    }
                }
            }
        }

        public override string Description()
        {
            return Texts.FunctionCLV;
        }

        [Description("Accumulates the CLV Function results.")]
        public bool Accumulate
        {
            get
            {
                return this.FAccumulate;
            }
            set
            {
                if (this.FAccumulate != value)
                {
                    this.FAccumulate = value;
                }
                base.Recalculate();
            }
        }

        [Description("Gets/sets the Volume series by which CLV results are multiplied.")]
        public Series Volume
        {
            get
            {
                return this.FVolume;
            }
            set
            {
                if (this.FVolume != value)
                {
                    this.FVolume = value;
                }
                base.Recalculate();
            }
        }
    }
}

