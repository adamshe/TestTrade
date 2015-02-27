using System;

namespace iTrading.Core.Data
{
    internal class Bar : IBar
    {
        private double open;
        private double high;
        private double low;
        private double close;
        private DateTime time;
        private int volume;

        internal Bar(double open, double high, double low, double close, DateTime time, int volume)
        {
            this.close = close;
            this.high = high;
            this.low = low;
            this.open = open;
            this.time = time;
            this.volume = volume;
        }

        public override string ToString()
        {
            return string.Concat(new object[] { "", this.open, "/", this.high, "/", this.low, "/", this.close, "/", this.volume, "/", this.time });
        }

        public string ToCSVString()
        {
            return string.Concat(new object[] { "", this.open, ",", this.high, ",", this.low, ",", this.close, ",", this.volume, ",", this.time });
        }

        internal void Update(double newHigh, double newLow, double newClose, DateTime newTime, int addVolume)
        {
            if (newHigh > this.high)
            {
                this.high = newHigh;
            }
            if (newLow < this.low)
            {
                this.low = newLow;
            }
            this.close = newClose;
            this.time = newTime;
            if ((this.volume + addVolume) >= 0x7fffffffL)
            {
                this.volume = 0x7ffffffe;
            }
            else
            {
                this.volume += addVolume;
            }
        }

        public double Close
        {
            get
            {
                return this.close;
            }
        }

        public double High
        {
            get
            {
                return this.high;
            }
        }

        public double Low
        {
            get
            {
                return this.low;
            }
        }

        public double Open
        {
            get
            {
                return this.open;
            }
        }

        public DateTime Time
        {
            get
            {
                return this.time;
            }
        }

        public int Volume
        {
            get
            {
                return this.volume;
            }
        }
    }
}