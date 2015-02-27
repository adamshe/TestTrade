using iTrading.Core.Data;

namespace iTrading.Track
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using iTrading.Core.Kernel;
    using iTrading.Core.Data;

    internal class HistoricalDataRequest : iTrading.Track.Request
    {
        private ArrayList bars;
        private bool isAborted;
        private Quotes quotes;

        internal HistoricalDataRequest(Adapter adapter, Quotes quotes) : base(adapter)
        {
            this.bars = new ArrayList();
            this.isAborted = false;
            this.quotes = quotes;
        }

        internal override void Process(MessageCode msgCode, ByteReader reader)
        {
            if (!this.isAborted)
            {
                if (Globals.TraceSwitch.Quotes)
                {
                    Trace.WriteLine("(" + base.Adapter.connection.IdPlus + ") Track.HistoricalDataRequest.Process: " + this.quotes.ToString());
                }
                iTrading.Track.ErrorCode code = (iTrading.Track.ErrorCode) reader.ReadByte();
                if (code == iTrading.Track.ErrorCode.NoSuchTicker)
                {
                    Globals.Progress.Terminate();
                    if (!this.isAborted)
                    {
                        base.Adapter.connection.ProcessEventArgs(new BarUpdateEventArgs(base.Adapter.connection, iTrading.Core.Kernel.ErrorCode.NoSuchSymbol, "", Operation.Insert, this.quotes, 0, -1));
                    }
                }
                else if (code != iTrading.Track.ErrorCode.NoError)
                {
                    base.Adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(base.Adapter.connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Track.HistoricalDataRequest.Process: error " + code));
                }
                else
                {
                    reader.ReadByte();
                    int num = reader.ReadByte();
                    int num2 = reader.ReadByte();
                    reader.ReadInteger();
                    bool flag = false;
                    for (int i = 0; i < num2; i++)
                    {
                        bool flag2 = false;
                        DateTime time = reader.ReadShortDate();
                        if (time == Globals.MaxDate)
                        {
                            string str = "";
                            if (this.bars.Count > 0)
                            {
                                str = " around " + ((iTrading.Track.Bar) this.bars[this.bars.Count - 1]).time.Date.ToString("yyyy-MM-dd");
                            }
                            Trace.WriteLine("WARNING: Track.HistoricalDataRequest.Process: Invalid data for '" + this.quotes.Symbol.FullName + "' at '" + str + "'");
                            flag2 = true;
                        }
                        else if (time < this.quotes.From)
                        {
                            flag = true;
                            flag2 = true;
                        }
                        else if (this.quotes.To < time)
                        {
                            flag2 = true;
                        }
                        reader.Skip(1);
                        byte format = reader.ReadByte();
                        double open = base.CalculateFormatValue((double) reader.ReadInteger(), format);
                        double high = base.CalculateFormatValue((double) reader.ReadInteger(), format);
                        double low = base.CalculateFormatValue((double) reader.ReadInteger(), format);
                        double close = base.CalculateFormatValue((double) reader.ReadInteger(), format);
                        int volume = reader.ReadInteger();
                        reader.ReadInteger();
                        if (volume < 0)
                        {
                            volume = 0;
                        }
                        if ((((!flag2 && (open > 0.0)) && ((high > 0.0) && (low > 0.0))) && (((close > 0.0) && (open >= low)) && ((high >= low) && (close >= low)))) && (((open <= high) && (low <= high)) && (close <= high)))
                        {
                            this.bars.Add(new iTrading.Track.Bar(open, high, low, close, time, volume));
                        }
                    }
                    bool flag3 = true;
                    if (num != 0)
                    {
                        if (flag)
                        {
                            flag3 = false;
                        }
                        else if (num == 2)
                        {
                            flag3 = false;
                        }
                        else if ((this.bars.Count == 0) || (((iTrading.Track.Bar) this.bars[this.bars.Count - 1]).time.Date <= this.quotes.From))
                        {
                            flag3 = false;
                        }
                        if (flag3)
                        {
                            Globals.Progress.Message = this.quotes.Symbol.FullName + " " + ((this.bars.Count > 0) ? ((iTrading.Track.Bar) this.bars[this.bars.Count - 1]).time.ToString("yyyy-MM-dd") : "");
                            if (this.SendNow(((iTrading.Track.Bar) this.bars[this.bars.Count - 1]).time.Date.AddDays(-1.0)) != iTrading.Track.ErrorCode.NoError)
                            {
                                Globals.Progress.Terminate();
                                if (!this.isAborted)
                                {
                                    base.Adapter.connection.ProcessEventArgs(new BarUpdateEventArgs(base.Adapter.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", Operation.Insert, this.quotes, 0, -1));
                                }
                            }
                        }
                        else
                        {
                            for (int j = this.bars.Count - 1; j >= 0; j--)
                            {
                                iTrading.Track.Bar bar = (iTrading.Track.Bar) this.bars[j];
                                this.quotes.Bars.Add(bar.open, bar.high, bar.low, bar.close, bar.time, bar.volume, false);
                            }
                            this.bars.Clear();
                            Globals.Progress.Terminate();
                            if (!this.isAborted)
                            {
                                base.Adapter.connection.ProcessEventArgs(new BarUpdateEventArgs(base.Adapter.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", Operation.Insert, this.quotes, 0, this.quotes.Bars.Count - 1));
                            }
                        }
                    }
                }
            }
        }

        private void Progress_Aborted(object sender, EventArgs e)
        {
            this.isAborted = true;
            base.Adapter.connection.ProcessEventArgs(new BarUpdateEventArgs(base.Adapter.connection, iTrading.Core.Kernel.ErrorCode.UserAbort, "Aborted by user", Operation.Insert, this.quotes, 0, -1));
        }

        internal override iTrading.Track.ErrorCode Send()
        {
            Globals.Progress.Initialise(0, true);
            Globals.Progress.Aborted += new AbortEventHandler(this.Progress_Aborted);
            Globals.Progress.Message = "Retrieving data for '" + this.quotes.Symbol.FullName + "'";
            return this.SendNow(this.quotes.To);
        }

        internal iTrading.Track.ErrorCode SendNow(DateTime next)
        {
            base.Adapter.Delay();
            if (Globals.TraceSwitch.Quotes)
            {
                Trace.WriteLine("(" + base.Adapter.connection.IdPlus + ") Track.HistoricalDataRequest.Send: " + this.quotes.Symbol.FullName);
            }
            iTrading.Track.ErrorCode code = (iTrading.Track.ErrorCode) Api.RequestHistoricalData(base.Rqn, base.Adapter.ToBrokerName(this.quotes.Symbol), new short[] { (short) next.Year, (short) next.Month, (short) next.Day }, (short) Math.Min((double) 255.0, (double) (next.Subtract(this.quotes.From).TotalDays + 1.0)), 1);
            if (code != iTrading.Track.ErrorCode.NoError)
            {
                Globals.Progress.Terminate();
                base.Adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(base.Adapter.connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Track.HistoricalDataRequest.Send: error " + code));
            }
            return code;
        }
    }
}

