namespace iTrading.Track
{
    using System;
    using System.Diagnostics;
    using System.Timers;
    using iTrading.Core.Kernel;

    internal class NasdaqLevel2Request : iTrading.Track.Request
    {
        private DateTime date;
        private bool running;
        private Symbol symbol;
        private Timer timer;
        private MarketDataType typeAsk;
        private MarketDataType typeBid;

        internal NasdaqLevel2Request(Adapter adapter, Symbol symbol) : base(adapter)
        {
            this.running = false;
            this.timer = null;
            this.date = adapter.connection.Now.Date;
            this.symbol = symbol;
            this.typeAsk = adapter.connection.MarketDataTypes[MarketDataTypeId.Ask];
            this.typeBid = adapter.connection.MarketDataTypes[MarketDataTypeId.Bid];
        }

        internal iTrading.Track.ErrorCode Halt()
        {
            if (Globals.TraceSwitch.MarketDepth)
            {
                Trace.WriteLine(string.Concat(new object[] { "(", base.Adapter.connection.IdPlus, ") Track.NasdaqLevel2Request.Halt.", base.Rqn, ": symbol='", this.symbol.FullName, "'" }));
            }
            if (this.timer != null)
            {
                this.timer.Stop();
            }
            this.timer = null;
            iTrading.Track.ErrorCode code = (iTrading.Track.ErrorCode) Api.RequestNasdaqLevel_2(base.Rqn, '\x0002', '\x0018', '\x0002', base.Adapter.ToBrokerName(this.symbol));
            if ((code != iTrading.Track.ErrorCode.NoError) && (code != iTrading.Track.ErrorCode.NotConnected))
            {
                base.Adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(base.Adapter.connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Track.NasdaqLevel2Request.Send: error " + code));
                return code;
            }
            this.running = false;
            return code;
        }

        internal override void Process(MessageCode msgCode, ByteReader reader)
        {
            if (Globals.TraceSwitch.MarketDepth)
            {
                Trace.WriteLine(string.Concat(new object[] { "(", base.Adapter.connection.IdPlus, ") Track.NasdaqLevel2Request.Process.", base.Rqn, ": symbol='", this.symbol.FullName, "'' msgCode=", msgCode, " running=", this.running }));
            }
            if (this.running)
            {
                iTrading.Track.ErrorCode code = (iTrading.Track.ErrorCode) reader.ReadByte();
                if (code == iTrading.Track.ErrorCode.NoSuchTicker)
                {
                    base.Adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(base.Adapter.connection, iTrading.Core.Kernel.ErrorCode.NoSuchSymbol, "", "Symbol '" + base.Adapter.ToBrokerName(this.symbol) + "' does not support market depth data"));
                }
                else if (code != iTrading.Track.ErrorCode.NoError)
                {
                    base.Adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(base.Adapter.connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Track.NasdaqLevel2Request.Process: error " + code));
                }
                else
                {
                    reader.ReadByte();
                    int num = reader.ReadByte();
                    reader.Skip(1);
                    for (int i = 0; i < num; i++)
                    {
                        string mmId = reader.ReadString(4);
                        byte num3 = reader.ReadByte();
                        reader.ReadString(1);
                        byte num4 = reader.ReadByte();
                        reader.ReadByte();
                        double bidPrice = reader.ReadInteger();
                        int bidSize = reader.ReadInteger() * 100;
                        double askPrice = reader.ReadInteger();
                        int askSize = reader.ReadInteger() * 100;
                        int num9 = reader.ReadInteger();
                        reader.ReadUShort();
                        reader.ReadUShort();
                        if (num3 != 1)
                        {
                            this.symbol.MarketDepth.Update(mmId, null, 0.0, 0, 0.0, 0, this.date.AddSeconds((double) num9));
                        }
                        else
                        {
                            switch (num4)
                            {
                                case 0:
                                    bidPrice /= 256.0;
                                    askPrice /= 256.0;
                                    break;

                                case 1:
                                    bidPrice /= 256.0;
                                    askPrice /= 1000.0;
                                    break;

                                case 2:
                                    bidPrice /= 1000.0;
                                    askPrice /= 256.0;
                                    break;

                                case 3:
                                    bidPrice /= 1000.0;
                                    askPrice /= 1000.0;
                                    break;

                                case 4:
                                    bidPrice /= 256.0;
                                    askPrice /= 10000.0;
                                    break;

                                case 5:
                                    bidPrice /= 10000.0;
                                    askPrice /= 256.0;
                                    break;

                                case 6:
                                    bidPrice /= 10000.0;
                                    askPrice /= 10000.0;
                                    break;
                            }
                            this.symbol.MarketDepth.Update(mmId, null, askPrice, askSize, bidPrice, bidSize, this.date.AddSeconds((double) num9));
                        }
                    }
                }
            }
        }

        internal override iTrading.Track.ErrorCode Send()
        {
            iTrading.Track.ErrorCode code = this.SendNow();
            if (code == iTrading.Track.ErrorCode.NoError)
            {
                this.timer = new Timer(600000.0);
                this.timer.AutoReset = true;
                this.timer.Elapsed += new ElapsedEventHandler(this.timer_Elapsed);
                this.timer.Start();
            }
            return code;
        }

        private iTrading.Track.ErrorCode SendNow()
        {
            if (Globals.TraceSwitch.MarketDepth)
            {
                Trace.WriteLine(string.Concat(new object[] { "(", base.Adapter.connection.IdPlus, ") Track.NasdaqLevel2Request.Send.", base.Rqn, ": ", this.symbol.FullName }));
            }
            iTrading.Track.ErrorCode code = (iTrading.Track.ErrorCode) Api.RequestNasdaqLevel_2(base.Rqn, '\x0001', '\x0018', '\x0002', base.Adapter.ToBrokerName(this.symbol));
            if (code != iTrading.Track.ErrorCode.NoError)
            {
                base.Adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(base.Adapter.connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Track.NasdaqLevel2Request.Send: error " + code));
                return code;
            }
            this.running = true;
            return code;
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.SendNow();
        }
    }
}

