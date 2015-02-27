using System;
using System.Collections;
using System.Runtime.InteropServices;
using iTrading.Core.Kernel;
using iTrading.Core.Interface;

namespace iTrading.Core.Data
{
    /// <summary>
    /// A collection of <see cref="T:iTrading.Core.Data.Bar" /> objects.
    /// </summary>
    [Guid("3446F834-4D60-4fb4-9C5E-3899AED81DA8"), ClassInterface(ClassInterfaceType.None)]
    public class Bars : IComBars, IEnumerable
    {
        internal ArrayList bars = new ArrayList();
        internal int barsAdded = 0;
        private Hashtable daily = new Hashtable();
        private Quotes quotes;

        internal Bars(Quotes quotes)
        {
            this.quotes = quotes;
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        /// <param name="open"></param>
        /// <param name="high"></param>
        /// <param name="low"></param>
        /// <param name="close"></param>
        /// <param name="time"></param>
        /// <param name="volume"></param>
        /// <param name="notify"></param>
        public void Add(double open, double high, double low, double close, DateTime time, int volume, bool notify)
        {
            if (open <= 0.0)
            {
                throw new TMException(ErrorCode.Panic, "Cbi.Quotes.Add: open value (" + open + ") must be greater than 0");
            }
            if (high <= 0.0)
            {
                throw new TMException(ErrorCode.Panic, "Cbi.Quotes.Add: high value (" + high + ") must be greater than 0");
            }
            if (low <= 0.0)
            {
                throw new TMException(ErrorCode.Panic, "Cbi.Quotes.Add: low value (" + low + ") must be greater than 0");
            }
            if (close <= 0.0)
            {
                throw new TMException(ErrorCode.Panic, "Cbi.Quotes.Add: close value (" + close + ") must be greater than 0");
            }
            if (volume < 0)
            {
                throw new TMException(ErrorCode.Panic, "Cbi.Quotes.Add: volume value (" + volume + ") must be greater than 0");
            }
            Operation insert = Operation.Insert;
            if (this.bars.Count == 0)
            {
                if (this.quotes.Period.Id == PeriodTypeId.Minute)
                {
                    time = Globals.MinDate.AddMinutes(Math.Ceiling((double) (Math.Ceiling(time.Subtract(Globals.MinDate).TotalMinutes) / ((double) this.quotes.Period.Value))) * this.quotes.Period.Value);
                }
                else if (this.quotes.Period.Id == PeriodTypeId.Day)
                {
                    time = time.AddDays((double) (this.quotes.Period.Value - 1));
                }
                this.bars.Add(new Bar(open, high, low, close, time, volume));
                this.barsAdded++;
            }
            else if (this.quotes.Period.Id == PeriodTypeId.Tick)
            {
                if (this.barsAdded++ < this.quotes.Period.Value)
                {
                    ((Bar) this.bars[this.bars.Count - 1]).Update(high, low, close, time, volume);
                    insert = Operation.Update;
                }
                else
                {
                    this.barsAdded = 1;
                    this.bars.Add(new Bar(open, high, low, close, time, volume));
                }
            }
            else
            {
                Bar bar = (Bar) this.bars[this.bars.Count - 1];
                if ((this.quotes.Period.Id == PeriodTypeId.Minute) ? (time <= bar.Time) : (time.Date <= bar.Time.Date))
                {
                    bar.Update(high, low, close, bar.Time, volume);
                    insert = Operation.Update;
                }
                else
                {
                    DateTime date;
                    if (this.quotes.Period.Id == PeriodTypeId.Minute)
                    {
                        date = bar.Time.AddMinutes(Math.Ceiling((double) (Math.Ceiling(time.Subtract(bar.Time).TotalMinutes) / ((double) this.quotes.Period.Value))) * this.quotes.Period.Value);
                    }
                    else
                    {
                        date = bar.Time.Date.AddDays(Math.Ceiling((double) (Math.Ceiling(time.Date.Subtract(bar.Time.Date).TotalDays) / ((double) this.quotes.Period.Value))) * this.quotes.Period.Value).Date;
                    }
                    this.bars.Add(new Bar(open, high, low, close, date, volume));
                }
            }
            QuotesDay day = (QuotesDay) this.daily[time.Date];
            if (day == null)
            {
                this.daily.Add(time.Date, new QuotesDay(open, high, low, close, time, volume, this.Count - 1));
            }
            else
            {
                day.Update(high, low, close, volume, this.Count - 1);
            }
            if (notify)
            {
                this.quotes.connection.ProcessEventArgs(new BarUpdateEventArgs(this.quotes.connection, ErrorCode.NoError, "", insert, this.quotes, this.bars.Count - 1, this.bars.Count - 1));
            }
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            return this.bars.GetEnumerator();
        }

        /// <summary>
        /// Get daily summary.
        /// </summary>
        /// <param name="atDate"></param>
        /// <returns>NULL, if the requested day is not included in the current quotes object.</returns>
        public QuotesDay GetQuotesDay(DateTime atDate)
        {
            return (QuotesDay) this.daily[atDate.Date];
        }

        /// <summary>
        /// Number of items.
        /// </summary>
        public int Count
        {
            get
            {
                return this.bars.Count;
            }
        }

        /// <summary>
        /// Get <see cref="T:iTrading.Core.Data.IBar" /> at index.
        /// </summary>
        public IBar this[int index]
        {
            get
            {
                if (this.bars.Count == 0)
                {
                    throw new TMException(ErrorCode.Panic, "Cbi.Bars.Item: Bar collection is empty");
                }
                if ((index < 0) || (index >= this.bars.Count))
                {
                    throw new ArgumentOutOfRangeException("index", index, "Cbi.Bars.Item: Index out of range from 0 to " + this.bars.Count);
                }
                return (IBar) this.bars[index];
            }
        }
    }
}