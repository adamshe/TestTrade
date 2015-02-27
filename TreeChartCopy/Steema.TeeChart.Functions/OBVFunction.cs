namespace Steema.TeeChart.Functions
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;

    public class OBVFunction : Function
    {
        private Series FVolume;

        public OBVFunction() : this(null)
        {
        }

        public OBVFunction(Chart c) : base(c)
        {
            base.Period = 1.0;
            base.CanUsePeriod = false;
        }

        public override void AddPoints(Array source)
        {
            if ((!base.updating && (source != null)) && (source.Length > 0))
            {
                Series series = (Series) source.GetValue(0);
                if ((series.Count > 0) && (series is OHLC))
                {
                    base.Series.Clear();
                    if (this.FVolume != null)
                    {
                        for (int i = 0; i < series.Count; i++)
                        {
                            if (this.FVolume.Count > i)
                            {
                                double y = this.FVolume.mandatory[i];
                                if (((OHLC) series).CloseValues[i] > ((OHLC) series).OpenValues[i])
                                {
                                    if (i > 0)
                                    {
                                        y += base.Series.mandatory.Last;
                                    }
                                }
                                else if (i > 0)
                                {
                                    y = base.Series.mandatory.Last - y;
                                }
                                base.Series.Add(((OHLC) series).DateValues[i], y);
                            }
                        }
                    }
                }
            }
        }

        public override string Description()
        {
            return Texts.FunctionOBV;
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

