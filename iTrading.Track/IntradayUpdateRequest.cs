namespace iTrading.Track
{
    using System;
    using System.Diagnostics;
    using iTrading.Core.Kernel;

    internal class IntradayUpdateRequest : iTrading.Track.Request
    {
        internal int blockFactor;
        internal double dailyHigh;
        internal double dailyLow;
        internal int dailyVolume;
        private bool running;
        internal Symbol symbol;
        private MarketDataType typeAsk;
        private MarketDataType typeBid;
        private MarketDataType typeDailyHigh;
        private MarketDataType typeDailyLow;
        private MarketDataType typeDailyVolume;
        private MarketDataType typeLast;

        internal IntradayUpdateRequest(Adapter adapter, Symbol symbol, int dailyVolume, double dailyHigh, double dailyLow, int blockFactor) : base(adapter)
        {
            this.running = false;
            this.typeAsk = null;
            this.typeBid = null;
            this.typeDailyHigh = null;
            this.typeDailyLow = null;
            this.typeDailyVolume = null;
            this.typeLast = null;
            this.blockFactor = blockFactor;
            this.dailyHigh = dailyHigh;
            this.dailyLow = dailyLow;
            this.dailyVolume = dailyVolume;
            this.symbol = symbol;
            this.typeAsk = adapter.connection.MarketDataTypes[MarketDataTypeId.Ask];
            this.typeBid = adapter.connection.MarketDataTypes[MarketDataTypeId.Bid];
            this.typeDailyHigh = adapter.connection.MarketDataTypes[MarketDataTypeId.DailyHigh];
            this.typeDailyLow = adapter.connection.MarketDataTypes[MarketDataTypeId.DailyLow];
            this.typeDailyVolume = adapter.connection.MarketDataTypes[MarketDataTypeId.DailyVolume];
            this.typeLast = adapter.connection.MarketDataTypes[MarketDataTypeId.Last];
        }

        internal iTrading.Track.ErrorCode Halt()
        {
            if (Globals.TraceSwitch.MarketData)
            {
                Trace.WriteLine(string.Concat(new object[] { "(", base.Adapter.connection.IdPlus, ") Track.IntradayUpdateRequest.Halt.", base.Rqn, ": symbol='", this.symbol.FullName, "'" }));
            }
            iTrading.Track.ErrorCode code = (iTrading.Track.ErrorCode) Api.RequestIntradayUpdate(base.Rqn, base.Adapter.ToBrokerName(this.symbol), "", '\x0002');
            if ((code != iTrading.Track.ErrorCode.NoError) && (code != iTrading.Track.ErrorCode.NotConnected))
            {
                base.Adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(base.Adapter.connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Track.IntradayUpdateRequest.Halt: error " + code));
                return code;
            }
            this.running = false;
            return code;
        }

        internal override void Process(MessageCode msgCode, ByteReader reader)
        {
            if (msgCode == MessageCode.IntradayUpdate)
            {
                if (Globals.TraceSwitch.MarketData)
                {
                    Trace.WriteLine(string.Concat(new object[] { "(", base.Adapter.connection.IdPlus, ") Track.IntradayUpdateRequest.Process.", base.Rqn, ": symbol='", this.symbol.FullName, "' msgCode=", msgCode, " running=", this.running }));
                }
                if (this.running)
                {
                    iTrading.Track.ErrorCode code = (iTrading.Track.ErrorCode) reader.ReadByte();
                    if (code != iTrading.Track.ErrorCode.NoError)
                    {
                        base.Adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(base.Adapter.connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Track.IntradayUpdateRequest.Process: error " + code));
                    }
                    else
                    {
                        reader.ReadByte();
                        byte num = reader.ReadByte();
                        byte num2 = reader.ReadByte();
                        DateTime time = reader.ReadCharTime();
                        byte format = reader.ReadByte();
                        if (num == 0)
                        {
                            reader.ReadInteger();
                            if (num2 == 0)
                            {
                                double price = base.CalculateFormatValue((double) reader.ReadInteger(), format);
                                int volume = reader.ReadInteger() * this.blockFactor;
                                if (volume < 0)
                                {
                                    volume = 0;
                                }
                                if (price > 0.0)
                                {
                                    base.Adapter.connection.ProcessEventArgs(new MarketDataEventArgs(this.symbol.MarketData, iTrading.Core.Kernel.ErrorCode.NoError, "", this.symbol, this.typeLast, price, volume, time));
                                    this.dailyVolume += volume;
                                    if (this.dailyVolume > 0)
                                    {
                                        base.Adapter.connection.ProcessEventArgs(new MarketDataEventArgs(this.symbol.MarketData, iTrading.Core.Kernel.ErrorCode.NoError, "", this.symbol, this.typeDailyVolume, 0.0, this.dailyVolume, time));
                                    }
                                    if (price > this.dailyHigh)
                                    {
                                        this.dailyHigh = price;
                                        base.Adapter.connection.ProcessEventArgs(new MarketDataEventArgs(this.symbol.MarketData, iTrading.Core.Kernel.ErrorCode.NoError, "", this.symbol, this.typeDailyHigh, this.dailyHigh, 0, time));
                                    }
                                    else if (price < this.dailyLow)
                                    {
                                        this.dailyLow = price;
                                        base.Adapter.connection.ProcessEventArgs(new MarketDataEventArgs(this.symbol.MarketData, iTrading.Core.Kernel.ErrorCode.NoError, "", this.symbol, this.typeDailyLow, this.dailyLow, 0, time));
                                    }
                                }
                            }
                            else
                            {
                                double num6 = base.CalculateFormatValue((double) reader.ReadInteger(), format);
                                double num7 = base.CalculateFormatValue((double) reader.ReadInteger(), format);
                                int num8 = reader.ReadUShort();
                                int num9 = reader.ReadUShort();
                                if (((num6 > 0.0) && (num8 > 0)) && (((this.symbol.MarketData.Bid == null) || (this.symbol.MarketData.Bid.Price != num6)) || (this.symbol.MarketData.Bid.Volume != num8)))
                                {
                                    base.Adapter.connection.ProcessEventArgs(new MarketDataEventArgs(this.symbol.MarketData, iTrading.Core.Kernel.ErrorCode.NoError, "", this.symbol, this.typeBid, num6, num8, time));
                                }
                                if (((num7 > 0.0) && (num9 > 0)) && (((this.symbol.MarketData.Ask == null) || (this.symbol.MarketData.Ask.Price != num7)) || (this.symbol.MarketData.Ask.Volume != num9)))
                                {
                                    base.Adapter.connection.ProcessEventArgs(new MarketDataEventArgs(this.symbol.MarketData, iTrading.Core.Kernel.ErrorCode.NoError, "", this.symbol, this.typeAsk, num7, num9, time));
                                }
                            }
                        }
                    }
                }
            }
        }

        internal override iTrading.Track.ErrorCode Send()
        {
            if (Globals.TraceSwitch.MarketData)
            {
                Trace.WriteLine(string.Concat(new object[] { "(", base.Adapter.connection.IdPlus, ") Track.IntradayUpdateRequest.Send.", base.Rqn, ": ", this.symbol.FullName }));
            }
            iTrading.Track.ErrorCode code = (iTrading.Track.ErrorCode) Api.RequestIntradayUpdate(base.Rqn, base.Adapter.ToBrokerName(this.symbol), "", '\x0001');
            if (code != iTrading.Track.ErrorCode.NoError)
            {
                base.Adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(base.Adapter.connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Track.IntradayUpdateRequest.Send: error " + code));
                return code;
            }
            this.running = true;
            return code;
        }
    }
}

