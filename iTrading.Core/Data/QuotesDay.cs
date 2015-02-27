using iTrading.Core.Data;

namespace iTrading.Core.Data
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Daily overview. Used mainy for intraday quotes.
    /// </summary>
    [Guid("35E55D40-FB95-4be6-ACC0-64A15903A886"), ClassInterface(ClassInterfaceType.None)]
    public class QuotesDay : IBar, IComQuotesDay
    {        
        private int firstBar;        
        private int lastBar;
        private double open;
        private double high;
        private double low;
        private double close;

        private DateTime time;
        private int volume;

        /// <summary>
        /// </summary>
        /// <param name="open"></param>
        /// <param name="high"></param>
        /// <param name="low"></param>
        /// <param name="close"></param>
        /// <param name="time"></param>
        /// <param name="volume"></param>
        /// <param name="firstBar"></param>
        internal QuotesDay(double open, double high, double low, double close, DateTime time, int volume, int firstBar)
        {
            this.close = close;
            this.firstBar = firstBar;
            this.high = high;
            this.lastBar = firstBar;
            this.low = low;
            this.open = open;
            this.time = time.Date;
            this.volume = volume;
        }

        internal void Update(double newHigh, double newLow, double newClose, int addVolume, int lastBar)
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
            lastBar = lastBar;
            if ((this.volume + addVolume) >= 0x7fffffffL)
            {
                this.volume = 0x7ffffffe;
            }
            else
            {
                this.volume += addVolume;
            }
        }

        /// <summary>
        /// Clsoe of the day.
        /// </summary>
        public double Close
        {
            get
            {
                return this.close;
            }
        }

        /// <summary>
        /// Index of first bar of the day.
        /// </summary>
        public int FirstBar
        {
            get
            {
                return this.firstBar;
            }
        }

        /// <summary>
        /// High of the day.
        /// </summary>
        public double High
        {
            get
            {
                return this.high;
            }
        }

        /// <summary>
        /// Index of last bar of day.
        /// </summary>
        public int LastBar
        {
            get
            {
                return this.lastBar;
            }
        }

        /// <summary>
        /// Low of the day.
        /// </summary>
        public double Low
        {
            get
            {
                return this.low;
            }
        }

        /// <summary>
        /// Open of the day.
        /// </summary>
        public double Open
        {
            get
            {
                return this.open;
            }
        }

        /// <summary>
        /// Date.
        /// </summary>
        public DateTime Time
        {
            get
            {
                return this.time;
            }
        }

        /// <summary>
        /// Volume of the day.
        /// </summary>
        public int Volume
        {
            get
            {
                return this.volume;
            }
        }
    }
}

