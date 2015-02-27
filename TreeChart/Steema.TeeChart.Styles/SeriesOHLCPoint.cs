namespace Steema.TeeChart.Styles
{
    using System;
    using System.ComponentModel;

    public class SeriesOHLCPoint : SeriesPoint
    {
        private OHLC ohlc;

        public SeriesOHLCPoint()
        {
        }

        public SeriesOHLCPoint(OHLC s, int i)
        {
            base.index = i;
            base.series = s;
            this.ohlc = s;
        }

        [Description("Indexed Point Close value")]
        public double Close
        {
            get
            {
                return this.ohlc.CloseValues[base.index];
            }
            set
            {
                this.ohlc.CloseValues[base.index] = value;
                this.ohlc.Invalidate();
            }
        }

        [Description("Indexed Point Date value")]
        public double Date
        {
            get
            {
                return this.ohlc.DateValues[base.index];
            }
            set
            {
                this.ohlc.DateValues[base.index] = value;
                this.ohlc.Invalidate();
            }
        }

        [Description("Indexed Point High value")]
        public double High
        {
            get
            {
                return this.ohlc.HighValues[base.index];
            }
            set
            {
                this.ohlc.HighValues[base.index] = value;
                this.ohlc.Invalidate();
            }
        }

        [Description("Indexed Point Low value")]
        public double Low
        {
            get
            {
                return this.ohlc.LowValues[base.index];
            }
            set
            {
                this.ohlc.LowValues[base.index] = value;
                this.ohlc.Invalidate();
            }
        }

        [Description("Indexed Point Open value")]
        public double Open
        {
            get
            {
                return this.ohlc.OpenValues[base.index];
            }
            set
            {
                this.ohlc.OpenValues[base.index] = value;
                this.ohlc.Invalidate();
            }
        }
    }
}

