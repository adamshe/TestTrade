using iTrading.Core.Data;

namespace iTrading.Track
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using iTrading.Core.Kernel;
    using iTrading.Core .Data ;

    internal class IntradayHistoryRequest : iTrading.Track.Request
    {
        private ArrayList bars;
        private int control;
        private bool isAborted;
        private int last;
        private Quotes quotes;
        private int seqNum;

        internal IntradayHistoryRequest(Adapter adapter, Quotes quotes) : base(adapter)
        {
            this.bars = new ArrayList();
            this.control = 0;
            this.isAborted = false;
            this.seqNum = 0;
            this.quotes = quotes;
        }

        private void CopyBars()
        {
            for (int i = this.bars.Count - 1; i >= 0; i--)
            {
                iTrading.Track.Bar bar = (iTrading.Track.Bar) this.bars[i];
                this.quotes.Bars.Add(bar.open, bar.high, bar.low, bar.close, bar.time, bar.volume, false);
            }
            this.bars.Clear();
        }

        internal override void Process(MessageCode msgCode, ByteReader reader)
        {
            if (Globals.TraceSwitch.Quotes)
            {
                Trace.WriteLine("(" + base.Adapter.connection.IdPlus + ") Track.IntradayHistoryRequest.Process: " + this.quotes.ToString());
            }
            iTrading.Track.ErrorCode code = (iTrading.Track.ErrorCode) reader.ReadByte();
            switch (code)
            {
                case iTrading.Track.ErrorCode.NoSuchTicker:
                    Globals.Progress.Terminate();
                    if (!this.isAborted)
                    {
                        base.Adapter.connection.ProcessEventArgs(new BarUpdateEventArgs(base.Adapter.connection, iTrading.Core.Kernel.ErrorCode.NoSuchSymbol, "", Operation.Insert, this.quotes, 0, -1));
                    }
                    return;

                case iTrading.Track.ErrorCode.NoData:
                case iTrading.Track.ErrorCode.TdsNotAvailable:
                    if (this.last == 0)
                    {
                        Globals.Progress.Terminate();
                        if (!this.isAborted)
                        {
                            base.Adapter.connection.ProcessEventArgs(new BarUpdateEventArgs(base.Adapter.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", Operation.Insert, this.quotes, 0, this.quotes.Bars.Count - 1));
                        }
                    }
                    else
                    {
                        this.last--;
                        this.control = 0;
                        this.seqNum = 0;
                        this.CopyBars();
                        if (this.bars.Count > 0)
                        {
                            Globals.Progress.Message = this.quotes.Symbol.FullName + " " + ((iTrading.Track.Bar) this.bars[this.bars.Count - 1]).time.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        if (Globals.Progress.IsAborted || (this.SendNow() != iTrading.Track.ErrorCode.NoError))
                        {
                            Globals.Progress.Terminate();
                            if (!this.isAborted)
                            {
                                base.Adapter.connection.ProcessEventArgs(new BarUpdateEventArgs(base.Adapter.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", Operation.Insert, this.quotes, 0, this.quotes.Bars.Count - 1));
                            }
                        }
                    }
                    break;

                case iTrading.Track.ErrorCode.NoError:
                    break;

                default:
                    base.Adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(base.Adapter.connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Track.IntradayDataRequest.Send: error " + code));
                    return;
            }
            reader.ReadByte();
            DateTime time = reader.ReadCharDate();
            byte num = reader.ReadByte();
            byte num2 = reader.ReadByte();
            reader.Skip(1);
            this.control = reader.ReadInteger();
            for (int i = 0; i < num2; i++)
            {
                int hour = reader.ReadByte();
                int minute = reader.ReadByte();
                int second = reader.ReadByte();
                byte format = reader.ReadByte();
                this.seqNum = reader.ReadInteger();
                double open = base.CalculateFormatValue((double) reader.ReadInteger(), format);
                int volume = reader.ReadInteger();
                reader.ReadByte();
                reader.ReadString(1);
                reader.ReadByte();
                reader.Skip(1);
                try
                {
                    time = new DateTime(time.Year, time.Month, time.Day, hour, minute, second);
                }
                catch
                {
                    Trace.WriteLine(string.Concat(new object[] { "WARNING: Track.IntradayHistoryRequest.Process: Invalid timestamp on '", this.quotes.Symbol.FullName, "' at '", time, "/", hour, "/", minute, "/", second, "'" }));
                    continue;
                }
                if ((this.bars.Count > 0) && (((iTrading.Track.Bar) this.bars[this.bars.Count - 1]).time < time))
                {
                    time = ((iTrading.Track.Bar) this.bars[this.bars.Count - 1]).time;
                }
                if (volume < 0)
                {
                    volume = 0;
                }
                if (((time.Date < this.quotes.From) || (this.quotes.To < time.Date)) || (open <= 0.0))
                {
                    if (num == 1)
                    {
                        Globals.Progress.Message = this.quotes.Symbol.FullName + " " + time.ToString("yyyy-MM-dd HH:mm:ss") + " (ignored)";
                    }
                }
                else
                {
                    this.bars.Add(new iTrading.Track.Bar(open, open, open, open, time, volume));
                }
            }
            if (num == 1)
            {
                if (this.bars.Count > 0)
                {
                    Globals.Progress.Message = this.quotes.Symbol.FullName + " " + ((iTrading.Track.Bar) this.bars[this.bars.Count - 1]).time.ToString("yyyy-MM-dd HH:mm:ss");
                }
                if (Globals.Progress.IsAborted || (this.SendNow() != iTrading.Track.ErrorCode.NoError))
                {
                    Globals.Progress.Terminate();
                    if (!this.isAborted)
                    {
                        base.Adapter.connection.ProcessEventArgs(new BarUpdateEventArgs(base.Adapter.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", Operation.Insert, this.quotes, 0, this.quotes.Bars.Count - 1));
                    }
                }
            }
            else if ((this.last == 0) || (time.Date > this.quotes.To))
            {
                this.CopyBars();
                Globals.Progress.Terminate();
                if (!this.isAborted)
                {
                    base.Adapter.connection.ProcessEventArgs(new BarUpdateEventArgs(base.Adapter.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", Operation.Insert, this.quotes, 0, this.quotes.Bars.Count - 1));
                }
            }
            else
            {
                this.last--;
                this.control = 0;
                this.seqNum = 0;
                this.CopyBars();
                if (this.bars.Count > 0)
                {
                    Globals.Progress.Message = this.quotes.Symbol.FullName + " " + ((iTrading.Track.Bar) this.bars[this.bars.Count - 1]).time.ToString("yyyy-MM-dd HH:mm:ss");
                }
                if (Globals.Progress.IsAborted || (this.SendNow() != iTrading.Track.ErrorCode.NoError))
                {
                    Globals.Progress.Terminate();
                    if (!this.isAborted)
                    {
                        base.Adapter.connection.ProcessEventArgs(new BarUpdateEventArgs(base.Adapter.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", Operation.Insert, this.quotes, 0, this.quotes.Bars.Count - 1));
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
            this.last = Math.Min(30, (int) base.Adapter.connection.Now.Date.Subtract(this.quotes.From).TotalDays);
            return this.SendNow();
        }

        private iTrading.Track.ErrorCode SendNow()
        {
            base.Adapter.Delay();
            if (Globals.TraceSwitch.Quotes)
            {
                Trace.WriteLine(string.Concat(new object[] { "(", base.Adapter.connection.IdPlus, ") Track.IntradayHistoryRequest.Send: ", this.quotes.Symbol.FullName, " back=", Math.Max(0, this.last), " seqNum=", this.seqNum, " control=", this.control }));
            }
            iTrading.Track.ErrorCode code = (iTrading.Track.ErrorCode) Api.RequestIntradayHistory(base.Rqn, base.Adapter.ToBrokerName(this.quotes.Symbol), (byte) Math.Max(0, this.last), 1, this.seqNum, this.control);
            if (code != iTrading.Track.ErrorCode.NoError)
            {
                base.Adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(base.Adapter.connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Track.IntradayDataRequest.Send: error " + code));
            }
            return code;
        }
    }
}

