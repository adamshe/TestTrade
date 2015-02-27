using iTrading.Core.Data;

namespace iTrading.Track
{
    using System;
    using System.Diagnostics;
    using iTrading.Core.Kernel;
    using iTrading.Core.Data;

    internal class IntradayDataRequest : iTrading.Track.Request
    {
        private bool isAborted;
        private int last;
        private Quotes quotes;

        internal IntradayDataRequest(Adapter adapter, Quotes quotes) : base(adapter)
        {
            this.isAborted = false;
            this.quotes = quotes;
        }

        internal override void Process(MessageCode msgCode, ByteReader reader)
        {
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
                        Globals.Progress.Message = this.quotes.Symbol.FullName + " " + ((this.quotes.Bars.Count > 0) ? this.quotes.Bars[this.quotes.Bars.Count - 1].Time.ToString("yyyy-MM-dd") : "");
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
            reader.Skip(2);
            DateTime time = reader.ReadCharDate();
            bool flag = reader.ReadByte() == 0;
            int hour = reader.ReadByte();
            int minute = reader.ReadByte();
            int second = reader.ReadByte();
            byte format = reader.ReadByte();
            reader.ReadInteger();
            double open = base.CalculateFormatValue((double) reader.ReadInteger(), format);
            double high = base.CalculateFormatValue((double) reader.ReadInteger(), format);
            double low = base.CalculateFormatValue((double) reader.ReadInteger(), format);
            double close = base.CalculateFormatValue((double) reader.ReadInteger(), format);
            int volume = reader.ReadInteger();
            try
            {
                time = new DateTime(time.Year, time.Month, time.Day, hour, minute, second);
            }
            catch
            {
                Trace.WriteLine(string.Concat(new object[] { "WARNING: Track.IntradayDataRequest.Process: Invalid timestamp on '", this.quotes.Symbol.FullName, "' at '", time, "/", hour, "/", minute, "/", second, "'" }));
                return;
            }
            if (volume < 0)
            {
                volume = 0;
            }
            if (((((this.quotes.From <= time.Date) && (time.Date <= this.quotes.To)) && ((open > 0.0) && (high > 0.0))) && (((low > 0.0) && (close > 0.0)) && ((open >= low) && (high >= low)))) && (((close >= low) && (open <= high)) && ((low <= high) && (close <= high))))
            {
                this.quotes.Bars.Add(open, high, low, close, time, volume, false);
            }
            if (!flag)
            {
                if ((this.last == 0) || (time.Date > this.quotes.To))
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
                    Globals.Progress.Message = this.quotes.Symbol.FullName + " " + ((this.quotes.Bars.Count > 0) ? this.quotes.Bars[this.quotes.Bars.Count - 1].Time.ToString("yyyy-MM-dd") : "");
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
            this.last = Math.Min(14, (int) base.Adapter.connection.Now.Date.Subtract(this.quotes.From).TotalDays);
            return this.SendNow();
        }

        private iTrading.Track.ErrorCode SendNow()
        {
            base.Adapter.Delay();
            if (Globals.TraceSwitch.Quotes)
            {
                Trace.WriteLine(string.Concat(new object[] { "(", base.Adapter.connection.IdPlus, ") Track.IntradayDataRequest.SendNow: ", this.quotes.Symbol.FullName, " back=", Math.Max(0, this.last) }));
            }
            iTrading.Track.ErrorCode code = (iTrading.Track.ErrorCode) Api.RequestIntradayData(base.Rqn, base.Adapter.ToBrokerName(this.quotes.Symbol), (byte) Math.Max(0, this.last));
            if (code != iTrading.Track.ErrorCode.NoError)
            {
                base.Adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(base.Adapter.connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Track.IntradayDataRequest.Send: error " + code));
            }
            return code;
        }
    }
}

