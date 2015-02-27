namespace iTrading.Core.Data
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;
    using iTrading.Core.Interface;

    /// <summary>
    /// A series of quote bars. <seealso cref="T:iTrading.Core.Data.Bar" />
    /// </summary>
    [Guid("C121B918-62F6-45e1-992E-C6E9E58ACA59"), ClassInterface(ClassInterfaceType.None)]
    public class Quotes : Request, IComQuotes
    {
        internal bool initialized; 
        internal DateTime from; 
        internal DateTime to;
        private Bars bars;
        private PriceSeries open;
        private PriceSeries high;
        private PriceSeries low;
        private PriceSeries close;
        private PriceSeries median;
        private PriceSeries typical;
        private Period period;
        private bool splitAdjusted;
        private iTrading.Core.Kernel.Symbol symbol;
        private TimeSeries time;
        private VolumeSeries volume;

        internal Quotes(iTrading.Core.Kernel.Symbol symbol, DateTime from, DateTime to, Period period, bool splitAdjusted) : base(symbol.connection)
        {
            this.initialized = false;
            this.bars = new Bars(this);
            this.close = new PriceSeries(this, PriceTypeId.Close);
            this.from = from.Date;
            this.high = new PriceSeries(this, PriceTypeId.High);
            this.low = new PriceSeries(this, PriceTypeId.Low);
            this.median = new PriceSeries(this, PriceTypeId.Median);
            this.open = new PriceSeries(this, PriceTypeId.Open);
            this.period = period;
            this.splitAdjusted = splitAdjusted;
            this.symbol = symbol;
            this.time = new TimeSeries(this);
            this.to = to.Date;
            this.typical = new PriceSeries(this, PriceTypeId.Typical);
            this.volume = new VolumeSeries(this);
        }

        /// <summary>
        /// Create a new <see cref="T:Quotes" /> object of different periodicy.
        /// Note: Only <see cref="T:Quotes" /> objects of the same <see cref="T:TradeMagic.Data.PeriodTypeId" /> or a larger
        /// sacle periodicy may be created and their <see cref="P:TradeMagic.Data.Period.Value" />.
        /// In case the perdiodicy type is the same, then its value must be a multiple of the
        /// original value.
        /// </summary>
        /// <param name="newPeriod"></param>
        /// <returns></returns>
        public Quotes Copy(Period newPeriod)
        {
            if (((this.Period.Id == PeriodTypeId.Minute) && (newPeriod.Id == PeriodTypeId.Tick)) || ((this.Period.Id == PeriodTypeId.Day) && (newPeriod.Id != PeriodTypeId.Day)))
            {
                throw new TMException(ErrorCode.Panic, string.Concat(new object[] { "Cbi.Quotes.Copy: mismatch in periodicy types ", this.Period.Id, " > ", newPeriod.Id }));
            }
            if ((this.Period.Id == newPeriod.Id) && ((newPeriod.Value < this.Period.Value) || ((newPeriod.Value % this.Period.Value) != 0)))
            {
                throw new TMException(ErrorCode.Panic, string.Concat(new object[] { "Cbi.Quotes.Copy: new periodicy value '", newPeriod.Value, "' is not a multiple of one '", this.Period.Value, "'" }));
            }
            Quotes quotes = new Quotes(this.Symbol, this.From, this.To, newPeriod, this.SplitAdjusted);
            foreach (Bar bar in this.Bars)
            {
                quotes.Bars.Add(bar.Open, bar.High, bar.Low, bar.Close, bar.Time, bar.Volume, false);
            }
            return quotes;
        }

        /// <summary>
        /// Dump quotes to a file.
        /// </summary>
        /// <param name="path">File path. Any existing file will be overriden.</param>
        /// <param name="seperator">Character to seperate the output fields.</param>
        /// <returns></returns>
        public void Dump(string path, char seperator)
        {
            CultureInfo formatProvider = new CultureInfo("en-US");
            string str = seperator.ToString();
            StreamWriter writer = null;
            try
            {
                writer = new StreamWriter(path);
            }
            catch (Exception)
            {
                throw new TMException(ErrorCode.Panic, "Unable to create file '" + path + "'");
            }
            foreach (IBar bar in this.bars)
            {
                writer.WriteLine(string.Concat(new object[] { bar.Time.ToString("yyyyMMdd"), (this.Period.Id == PeriodTypeId.Day) ? "" : (str + bar.Time.ToString("HHmmss")), str, this.symbol.FormatPrice(bar.Open, formatProvider), str, this.symbol.FormatPrice(bar.High, formatProvider), str, this.symbol.FormatPrice(bar.Low, formatProvider), str, this.symbol.FormatPrice(bar.Close, formatProvider), str, bar.Volume }));
            }
            writer.Close();
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        /// <param name="buf"></param>
        public void FromBytes(byte[] buf)
        {
            Bytes bytes = new Bytes();
            bytes.Reset(buf);
            double tickSize = bytes.ReadDouble();
            bytes.ReadInt32();
            this.Bars.Add(bytes.ReadDouble(), bytes.ReadDouble(), bytes.ReadDouble(), bytes.ReadDouble(), bytes.ReadDateTime(), bytes.ReadInt32(), false);
            int inLength = bytes.InLength;
            while (inLength < buf.Length)
            {
                IBar bar = this.Bars[this.Bars.Count - 1];
                byte num3 = buf[inLength];
                double open = bar.Open;
                double high = bar.High;
                double low = bar.Low;
                double close = bar.Close;
                DateTime time = bar.Time;
                int volume = 0;
                if (this.Period.Id == PeriodTypeId.Tick)
                {
                    if ((num3 & 3) != 0)
                    {
                        if ((num3 & 3) == 1)
                        {
                            time = time.AddSeconds((double) buf[++inLength]);
                        }
                        else if ((num3 & 3) == 2)
                        {
                            time = time.AddSeconds((double) ((buf[++inLength] * 0x100) + buf[++inLength]));
                        }
                        else if ((num3 & 3) == 3)
                        {
                            time = time.AddSeconds((double) ((((buf[++inLength] * 0x100) * 0x100) + (buf[++inLength] * 0x100)) + buf[++inLength]));
                        }
                    }
                    if ((num3 & 12) == 4)
                    {
                        open += (buf[++inLength] - 0x80) * tickSize;
                    }
                    else if ((num3 & 12) == 8)
                    {
                        open += (((buf[++inLength] * 0x100) + buf[++inLength]) - 0x4000) * tickSize;
                    }
                    else if ((num3 & 12) == 12)
                    {
                        open += (((((((buf[++inLength] * 0x100) * 0x100) * 0x100) + ((buf[++inLength] * 0x100) * 0x100)) + (buf[++inLength] * 0x100)) + buf[++inLength]) - 0x40000000L) * tickSize;
                    }
                    high = low = close = open;
                }
                else if (this.Period.Id == PeriodTypeId.Minute)
                {
                    byte num9 = buf[++inLength];
                    if ((num3 & 3) == 0)
                    {
                        time = time.AddMinutes(1.0);
                    }
                    else if ((num3 & 3) == 1)
                    {
                        time = time.AddMinutes((double) buf[++inLength]);
                    }
                    else if ((num3 & 3) == 2)
                    {
                        time = time.AddMinutes((double) ((buf[++inLength] * 0x100) + buf[++inLength]));
                    }
                    else if ((num3 & 3) == 3)
                    {
                        time = time.AddMinutes((double) ((((buf[++inLength] * 0x100) * 0x100) + (buf[++inLength] * 0x100)) + buf[++inLength]));
                    }
                    if ((num3 & 12) == 4)
                    {
                        open += (buf[++inLength] - 0x80) * tickSize;
                    }
                    else if ((num3 & 12) == 8)
                    {
                        open += (((buf[++inLength] * 0x100) + buf[++inLength]) - 0x4000) * tickSize;
                    }
                    else if ((num3 & 12) == 12)
                    {
                        open += (((((((buf[++inLength] * 0x100) * 0x100) * 0x100) + ((buf[++inLength] * 0x100) * 0x100)) + (buf[++inLength] * 0x100)) + buf[++inLength]) - 0x40000000L) * tickSize;
                    }
                    high = open;
                    if ((num9 & 0x30) == 0x10)
                    {
                        high += buf[++inLength] * tickSize;
                    }
                    else if ((num9 & 0x30) == 0x20)
                    {
                        high += ((buf[++inLength] * 0x100) + buf[++inLength]) * tickSize;
                    }
                    else if ((num9 & 0x30) == 0x30)
                    {
                        high += ((((((buf[++inLength] * 0x100) * 0x100) * 0x100) + ((buf[++inLength] * 0x100) * 0x100)) + (buf[++inLength] * 0x100)) + buf[++inLength]) * tickSize;
                    }
                    low = open;
                    if ((num9 & 0xc0) == 0x40)
                    {
                        low -= buf[++inLength] * tickSize;
                    }
                    else if ((num9 & 0xc0) == 0x80)
                    {
                        low -= ((buf[++inLength] * 0x100) + buf[++inLength]) * tickSize;
                    }
                    else if ((num9 & 0xc0) == 0xc0)
                    {
                        low -= ((((((buf[++inLength] * 0x100) * 0x100) * 0x100) + ((buf[++inLength] * 0x100) * 0x100)) + (buf[++inLength] * 0x100)) + buf[++inLength]) * tickSize;
                    }
                    close = low;
                    if ((num9 & 3) == 1)
                    {
                        close += buf[++inLength] * tickSize;
                    }
                    else if ((num9 & 3) == 2)
                    {
                        close += ((buf[++inLength] * 0x100) + buf[++inLength]) * tickSize;
                    }
                    else if ((num9 & 3) == 3)
                    {
                        close += ((((((buf[++inLength] * 0x100) * 0x100) * 0x100) + ((buf[++inLength] * 0x100) * 0x100)) + (buf[++inLength] * 0x100)) + buf[++inLength]) * tickSize;
                    }
                }
                else
                {
                    Trace.Assert(false, "Cbi.Quotes.FromBytes: unexpected quotes period type: " + this.Period.Id);
                }
                if ((num3 & 240) == 0)
                {
                    volume = 0;
                }
                else if ((num3 & 240) == 0x10)
                {
                    volume = buf[++inLength];
                }
                else if ((num3 & 240) == 0x20)
                {
                    volume = 50 * buf[++inLength];
                }
                else if ((num3 & 240) == 0x30)
                {
                    volume = 100 * buf[++inLength];
                }
                else if ((num3 & 240) == 0x40)
                {
                    volume = 500 * buf[++inLength];
                }
                else if ((num3 & 240) == 80)
                {
                    volume = 0x3e8 * buf[++inLength];
                }
                else if ((num3 & 240) == 0x60)
                {
                    volume = (buf[++inLength] * 0x100) + buf[++inLength];
                }
                else if ((num3 & 240) == 0x70)
                {
                    volume = (((((buf[++inLength] * 0x100) * 0x100) * 0x100) + ((buf[++inLength] * 0x100) * 0x100)) + (buf[++inLength] * 0x100)) + buf[++inLength];
                }
                inLength++;
                open = iTrading.Core.Kernel.Symbol.Round2TickSize(open, tickSize);
                high = iTrading.Core.Kernel.Symbol.Round2TickSize(high, tickSize);
                low = iTrading.Core.Kernel.Symbol.Round2TickSize(low, tickSize);
                close = iTrading.Core.Kernel.Symbol.Round2TickSize(close, tickSize);
                this.Bars.Add(open, high, low, close, time, volume, false);
            }
        }

        /// <summary>
        /// A market data stream may be attached to update this <see cref="T:Quotes" /> object: 
        /// the last bar is updated and/or new bars are added.
        /// Please note: This is effective only after receiving the initial <see cref="T:TradeMagic.Data.BarUpdateEventArgs" /> event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MarketDataItem(object sender, MarketDataEventArgs e)
        {
            if (this.initialized && (e.MarketDataType.Id == MarketDataTypeId.Last))
            {
                this.bars.Add(e.Price, e.Price, e.Price, e.Price, e.Time, e.Volume, true);
            }
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="last"></param>
        /// <returns></returns>
        public byte[] ToBytes(int first, int last)
        {
            byte[] buffer = new byte[0x8000];
            Bytes bytes = new Bytes();
            int index = -1;
            IBar bar = this.Bars[first];
            bytes.Write(this.symbol.TickSize);
            bytes.Write((int) ((last - first) + 1));
            bytes.Write(bar.Open);
            bytes.Write(bar.High);
            bytes.Write(bar.Low);
            bytes.Write(bar.Close);
            bytes.Write(bar.Time);
            bytes.Write(bar.Volume);
            for (int i = 0; i < bytes.OutLength; i++)
            {
                buffer[i] = bytes.Out[i];
            }
            index += bytes.OutLength;
            for (int j = first + 1; j <= last; j++)
            {
                byte[] buffer4;
                IntPtr ptr;
                if ((index + 0x63) > buffer.Length)
                {
                    byte[] array = new byte[buffer.Length * 2];
                    buffer.CopyTo(array, 0);
                    buffer = array;
                }
                IBar bar2 = this.Bars[j];
                int num4 = ++index;
                long volume = 0L;
                bar = this.Bars[j - 1];
                if (this.Period.Id == PeriodTypeId.Tick)
                {
                    volume = (long) Math.Round(bar2.Time.Subtract(bar.Time).TotalSeconds, 0);
                    Trace.Assert(volume >= 0L, "Cbi.Quotes.ToBytes");
                    if (volume != 0L)
                    {
                        if (volume > 0xffL)
                        {
                            if (volume > 0xffffL)
                            {
                                if (volume > 0xffffffL)
                                {
                                    this.symbol.connection.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this.symbol.connection, ErrorCode.Panic, "", string.Concat(new object[] { "Cbi.Quotes.ToBytes: data for '", this.symbol.FullName, "' at ", bar2.Time, " exceeds time gap maximum" })));
                                    return null;
                                }
                                (buffer4 = buffer)[(int) (ptr = (IntPtr) num4)] = (byte) (buffer4[(int) ptr] | 3);
                                buffer[++index] = (byte) (volume / 0x10000L);
                                volume -= (buffer[index] * 0x100) * 0x100;
                                buffer[++index] = (byte) (volume / 0x100L);
                                volume -= buffer[index] * 0x100;
                                buffer[++index] = (byte) volume;
                            }
                            else
                            {
                                (buffer4 = buffer)[(int) (ptr = (IntPtr) num4)] = (byte) (buffer4[(int) ptr] | 2);
                                buffer[++index] = (byte) (volume / 0x100L);
                                volume -= buffer[index] * 0x100;
                                buffer[++index] = (byte) volume;
                            }
                        }
                        else
                        {
                            (buffer4 = buffer)[(int) (ptr = (IntPtr) num4)] = (byte) (buffer4[(int) ptr] | 1);
                            buffer[++index] = (byte) volume;
                        }
                    }
                    volume = (int) Math.Round((double) ((bar2.Open - bar.Open) / this.symbol.TickSize), 0);
                    if (volume != 0L)
                    {
                        if ((-128L >= volume) || (volume >= 0x80L))
                        {
                            if ((-16384L >= volume) || (volume >= 0x4000L))
                            {
                                if ((-1073741824L >= volume) || (volume >= 0x40000000L))
                                {
                                    this.symbol.connection.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this.symbol.connection, ErrorCode.Panic, "", string.Concat(new object[] { "Cbi.Quotes.ToBytes: data for '", this.symbol.FullName, "' at ", bar2.Time, " exceeds max price gap (", bar2.Open, ")" })));
                                    return null;
                                }
                                volume += 0x40000000L;
                                (buffer4 = buffer)[(int) (ptr = (IntPtr) num4)] = (byte) (buffer4[(int) ptr] | 12);
                                buffer[++index] = (byte) (volume / 0x1000000L);
                                volume -= ((buffer[index] * 0x100) * 0x100) * 0x100;
                                buffer[++index] = (byte) (volume / 0x10000L);
                                volume -= (buffer[index] * 0x100) * 0x100;
                                buffer[++index] = (byte) (volume / 0x100L);
                                volume -= buffer[index] * 0x100;
                                buffer[++index] = (byte) volume;
                            }
                            else
                            {
                                volume += 0x4000L;
                                (buffer4 = buffer)[(int) (ptr = (IntPtr) num4)] = (byte) (buffer4[(int) ptr] | 8);
                                buffer[++index] = (byte) (volume / 0x100L);
                                volume -= buffer[index] * 0x100;
                                buffer[++index] = (byte) volume;
                            }
                        }
                        else
                        {
                            volume += 0x80L;
                            (buffer4 = buffer)[(int) (ptr = (IntPtr) num4)] = (byte) (buffer4[(int) ptr] | 4);
                            buffer[++index] = (byte) volume;
                        }
                    }
                }
                else if (this.Period.Id == PeriodTypeId.Minute)
                {
                    int num7 = ++index;
                    volume = (long) Math.Round(bar2.Time.Subtract(bar.Time).TotalMinutes, 0);
                    Trace.Assert(volume >= 0L);
                    if (volume != 1L)
                    {
                        if (volume > 0xffL)
                        {
                            if (volume > 0xffffL)
                            {
                                if (volume > 0xffffffL)
                                {
                                    this.symbol.connection.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this.symbol.connection, ErrorCode.Panic, "", string.Concat(new object[] { "Cbi.Quotes.ToBytes: data for '", this.symbol.FullName, "' at ", bar2.Time, " exceeds time gap maximum" })));
                                    return null;
                                }
                                (buffer4 = buffer)[(int) (ptr = (IntPtr) num4)] = (byte) (buffer4[(int) ptr] | 3);
                                buffer[++index] = (byte) (volume / 0x10000L);
                                volume -= (buffer[index] * 0x100) * 0x100;
                                buffer[++index] = (byte) (volume / 0x100L);
                                volume -= buffer[index] * 0x100;
                                buffer[++index] = (byte) volume;
                            }
                            else
                            {
                                (buffer4 = buffer)[(int) (ptr = (IntPtr) num4)] = (byte) (buffer4[(int) ptr] | 2);
                                buffer[++index] = (byte) (volume / 0x100L);
                                volume -= buffer[index] * 0x100;
                                buffer[++index] = (byte) volume;
                            }
                        }
                        else
                        {
                            (buffer4 = buffer)[(int) (ptr = (IntPtr) num4)] = (byte) (buffer4[(int) ptr] | 1);
                            buffer[++index] = (byte) volume;
                        }
                    }
                    double open = bar2.Open;
                    volume = (int) Math.Round((double) ((open - bar.Open) / this.symbol.TickSize), 0);
                    if (volume != 0L)
                    {
                        if ((-128L >= volume) || (volume >= 0x80L))
                        {
                            if ((-16384L >= volume) || (volume >= 0x4000L))
                            {
                                if ((-1073741824L >= volume) || (volume >= 0x40000000L))
                                {
                                    this.symbol.connection.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this.symbol.connection, ErrorCode.Panic, "", string.Concat(new object[] { "Cbi.Quotes.ToBytes: data for '", this.symbol.FullName, "' at ", bar2.Time, " exceeds max price gap (", bar2.Open, ")" })));
                                    return null;
                                }
                                volume += 0x40000000L;
                                (buffer4 = buffer)[(int) (ptr = (IntPtr) num4)] = (byte) (buffer4[(int) ptr] | 12);
                                buffer[++index] = (byte) (volume / 0x1000000L);
                                volume -= ((buffer[index] * 0x100) * 0x100) * 0x100;
                                buffer[++index] = (byte) (volume / 0x10000L);
                                volume -= (buffer[index] * 0x100) * 0x100;
                                buffer[++index] = (byte) (volume / 0x100L);
                                volume -= buffer[index] * 0x100;
                                buffer[++index] = (byte) volume;
                            }
                            else
                            {
                                volume += 0x4000L;
                                (buffer4 = buffer)[(int) (ptr = (IntPtr) num4)] = (byte) (buffer4[(int) ptr] | 8);
                                buffer[++index] = (byte) (volume / 0x100L);
                                volume -= buffer[index] * 0x100;
                                buffer[++index] = (byte) volume;
                            }
                        }
                        else
                        {
                            volume += 0x80L;
                            (buffer4 = buffer)[(int) (ptr = (IntPtr) num4)] = (byte) (buffer4[(int) ptr] | 4);
                            buffer[++index] = (byte) volume;
                        }
                    }
                    volume = (long) Math.Round((double) ((bar2.High - open) / this.symbol.TickSize), 0);
                    if (volume != 0L)
                    {
                        if (volume >= 0xffL)
                        {
                            if (volume >= 0x8000L)
                            {
                                if (volume >= 0x7fffffffL)
                                {
                                    this.symbol.connection.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this.symbol.connection, ErrorCode.Panic, "", string.Concat(new object[] { "Cbi.Quotes.ToBytes: data for '", this.symbol.FullName, "' at ", bar2.Time, " exceeds max price gap (", bar2.Open, ")" })));
                                    return null;
                                }
                                (buffer4 = buffer)[(int) (ptr = (IntPtr) num7)] = (byte) (buffer4[(int) ptr] | 0x30);
                                buffer[++index] = (byte) (volume / 0x1000000L);
                                volume -= ((buffer[index] * 0x100) * 0x100) * 0x100;
                                buffer[++index] = (byte) (volume / 0x10000L);
                                volume -= (buffer[index] * 0x100) * 0x100;
                                buffer[++index] = (byte) (volume / 0x100L);
                                volume -= buffer[index] * 0x100;
                                buffer[++index] = (byte) volume;
                            }
                            else
                            {
                                (buffer4 = buffer)[(int) (ptr = (IntPtr) num7)] = (byte) (buffer4[(int) ptr] | 0x20);
                                buffer[++index] = (byte) (volume / 0x100L);
                                volume -= buffer[index] * 0x100;
                                buffer[++index] = (byte) volume;
                            }
                        }
                        else
                        {
                            (buffer4 = buffer)[(int) (ptr = (IntPtr) num7)] = (byte) (buffer4[(int) ptr] | 0x10);
                            buffer[++index] = (byte) volume;
                        }
                    }
                    double low = bar2.Low;
                    volume = (long) Math.Round((double) ((open - low) / this.symbol.TickSize), 0);
                    if (volume != 0L)
                    {
                        if (volume >= 0xffL)
                        {
                            if (volume >= 0x8000L)
                            {
                                if (volume >= 0x7fffffffL)
                                {
                                    this.symbol.connection.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this.symbol.connection, ErrorCode.Panic, "", string.Concat(new object[] { "Cbi.Quotes.ToBytes: data for '", this.symbol.FullName, "' at ", bar2.Time, " exceeds max price gap (", bar2.Open, ")" })));
                                    return null;
                                }
                                (buffer4 = buffer)[(int) (ptr = (IntPtr) num7)] = (byte) (buffer4[(int) ptr] | 0xc0);
                                buffer[++index] = (byte) (volume / 0x1000000L);
                                volume -= ((buffer[index] * 0x100) * 0x100) * 0x100;
                                buffer[++index] = (byte) (volume / 0x10000L);
                                volume -= (buffer[index] * 0x100) * 0x100;
                                buffer[++index] = (byte) (volume / 0x100L);
                                volume -= buffer[index] * 0x100;
                                buffer[++index] = (byte) volume;
                            }
                            else
                            {
                                (buffer4 = buffer)[(int) (ptr = (IntPtr) num7)] = (byte) (buffer4[(int) ptr] | 0x80);
                                buffer[++index] = (byte) (volume / 0x100L);
                                volume -= buffer[index] * 0x100;
                                buffer[++index] = (byte) volume;
                            }
                        }
                        else
                        {
                            (buffer4 = buffer)[(int) (ptr = (IntPtr) num7)] = (byte) (buffer4[(int) ptr] | 0x40);
                            buffer[++index] = (byte) volume;
                        }
                    }
                    volume = (long) Math.Round((double) ((bar2.Close - low) / this.symbol.TickSize), 0);
                    if (volume != 0L)
                    {
                        if (volume >= 0xffL)
                        {
                            if (volume >= 0x8000L)
                            {
                                if (volume >= 0x7fffffffL)
                                {
                                    this.symbol.connection.ProcessEventArgs(new iTrading.Core.Kernel.ITradingErrorEventArgs(this.symbol.connection, ErrorCode.Panic, "", string.Concat(new object[] { "Cbi.Quotes.ToBytes: data for '", this.symbol.FullName, "' at ", bar2.Time, " exceeds max price gap (", bar2.Open, ")" })));
                                    return null;
                                }
                                (buffer4 = buffer)[(int) (ptr = (IntPtr) num7)] = (byte) (buffer4[(int) ptr] | 3);
                                buffer[++index] = (byte) (volume / 0x1000000L);
                                volume -= ((buffer[index] * 0x100) * 0x100) * 0x100;
                                buffer[++index] = (byte) (volume / 0x10000L);
                                volume -= (buffer[index] * 0x100) * 0x100;
                                buffer[++index] = (byte) (volume / 0x100L);
                                volume -= buffer[index] * 0x100;
                                buffer[++index] = (byte) volume;
                            }
                            else
                            {
                                (buffer4 = buffer)[(int) (ptr = (IntPtr) num7)] = (byte) (buffer4[(int) ptr] | 2);
                                buffer[++index] = (byte) (volume / 0x100L);
                                volume -= buffer[index] * 0x100;
                                buffer[++index] = (byte) volume;
                            }
                        }
                        else
                        {
                            (buffer4 = buffer)[(int) (ptr = (IntPtr) num7)] = (byte) (buffer4[(int) ptr] | 1);
                            buffer[++index] = (byte) volume;
                        }
                    }
                }
                else
                {
                    Trace.Assert(false, "Cbi.Quotes.ToBytes: unexpected quotes period type: " + this.Period.Id);
                }
                if (bar2.Volume != 0)
                {
                    if (bar2.Volume <= 0xff)
                    {
                        volume = bar2.Volume;
                        (buffer4 = buffer)[(int) (ptr = (IntPtr) num4)] = (byte) (buffer4[(int) ptr] | 0x10);
                        buffer[++index] = (byte) volume;
                    }
                    else if (((((int) (((double) bar2.Volume) / 50.0)) * 50) == bar2.Volume) && ((bar2.Volume / 50) <= 0xff))
                    {
                        volume = bar2.Volume / 50;
                        (buffer4 = buffer)[(int) (ptr = (IntPtr) num4)] = (byte) (buffer4[(int) ptr] | 0x20);
                        buffer[++index] = (byte) volume;
                    }
                    else if (((((int) (((double) bar2.Volume) / 100.0)) * 100) == bar2.Volume) && ((bar2.Volume / 100) <= 0xff))
                    {
                        volume = bar2.Volume / 100;
                        (buffer4 = buffer)[(int) (ptr = (IntPtr) num4)] = (byte) (buffer4[(int) ptr] | 0x30);
                        buffer[++index] = (byte) volume;
                    }
                    else if (((((int) (((double) bar2.Volume) / 500.0)) * 500) == bar2.Volume) && ((bar2.Volume / 500) <= 0xff))
                    {
                        volume = bar2.Volume / 500;
                        (buffer4 = buffer)[(int) (ptr = (IntPtr) num4)] = (byte) (buffer4[(int) ptr] | 0x40);
                        buffer[++index] = (byte) volume;
                    }
                    else if (((((int) (((double) bar2.Volume) / 1000.0)) * 0x3e8) == bar2.Volume) && ((bar2.Volume / 0x3e8) <= 0xff))
                    {
                        volume = bar2.Volume / 0x3e8;
                        (buffer4 = buffer)[(int) (ptr = (IntPtr) num4)] = (byte) (buffer4[(int) ptr] | 80);
                        buffer[++index] = (byte) volume;
                    }
                    else if (bar2.Volume <= 0xffff)
                    {
                        volume = bar2.Volume;
                        (buffer4 = buffer)[(int) (ptr = (IntPtr) num4)] = (byte) (buffer4[(int) ptr] | 0x60);
                        buffer[++index] = (byte) (volume / 0x100L);
                        volume -= buffer[index] * 0x100;
                        buffer[++index] = (byte) volume;
                    }
                    else
                    {
                        volume = bar2.Volume;
                        (buffer4 = buffer)[(int) (ptr = (IntPtr) num4)] = (byte) (buffer4[(int) ptr] | 0x70);
                        buffer[++index] = (byte) (volume / 0x1000000L);
                        volume -= ((buffer[index] * 0x100) * 0x100) * 0x100;
                        buffer[++index] = (byte) (volume / 0x10000L);
                        volume -= (buffer[index] * 0x100) * 0x100;
                        buffer[++index] = (byte) (volume / 0x100L);
                        volume -= buffer[index] * 0x100;
                        buffer[++index] = (byte) volume;
                    }
                }
            }
            byte[] buffer3 = new byte[index + 1];
            for (int k = 0; k <= index; k++)
            {
                buffer3[k] = buffer[k];
            }
            return buffer3;
        }

        /// <summary>
        /// Get a string describing the current <see cref="T:Quotes" /> object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Concat(new object[] { "symbol='", this.symbol.FullName, "' from='", this.from.ToString("yyyy-MM-dd"), "' to='", this.to.ToString("yyyy-MM-dd"), "' period=", this.Period.ToString(), " splitAdjusted=", this.splitAdjusted, " bars=", this.bars.Count });
        }

        /// <summary>
        /// Get the collection of <see cref="T:TradeMagic.Data.Bar" /> objects.
        /// </summary>
        public Bars Bars
        {
            get
            {
                return this.bars;
            }
        }

        /// <summary>
        /// Get the collection of close values.
        /// </summary>
        public PriceSeries Close
        {
            get
            {
                return this.close;
            }
        }

        /// <summary>
        /// Date of first quote bar.
        /// </summary>
        public DateTime From
        {
            get
            {
                return this.from;
            }
        }

        /// <summary>
        /// Get the collection of high values.
        /// </summary>
        public PriceSeries High
        {
            get
            {
                return this.high;
            }
        }

        /// <summary>
        /// Get the collection of low values.
        /// </summary>
        public PriceSeries Low
        {
            get
            {
                return this.low;
            }
        }

        /// <summary>
        /// Get the collection of median prices = (H + L) /2
        /// </summary>
        public PriceSeries Median
        {
            get
            {
                return this.median;
            }
        }

        /// <summary>
        /// Get the collection of open values.
        /// </summary>
        public PriceSeries Open
        {
            get
            {
                return this.open;
            }
        }

        /// <summary>
        /// Periodicy of <see cref="T:Quotes" /> object.
        /// </summary>
        public Period Period
        {
            get
            {
                return this.period;
            }
        }

        /// <summary>
        /// </summary>
        public bool SplitAdjusted
        {
            get
            {
                return this.splitAdjusted;
            }
        }

        /// <summary>
        /// Get the <see cref="P:Quotes.Symbol" /> object.
        /// </summary>
        public iTrading.Core.Kernel.Symbol Symbol
        {
            get
            {
                return this.symbol;
            }
        }

        /// <summary>
        /// Get the collection of time values.
        /// </summary>
        public TimeSeries Time
        {
            get
            {
                return this.time;
            }
        }

        /// <summary>
        /// Date of last quote bar.
        /// </summary>
        public DateTime To
        {
            get
            {
                return this.to;
            }
        }

        /// <summary>
        /// Get the collection of typical prices = (H + L + C) /3
        /// </summary>
        public PriceSeries Typical
        {
            get
            {
                return this.typical;
            }
        }

        /// <summary>
        /// Get the collection of volume values.
        /// </summary>
        public VolumeSeries Volume
        {
            get
            {
                return this.volume;
            }
        }
    }
}

