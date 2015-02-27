namespace iTrading.Track
{
    using System;
    using System.Diagnostics;
    using iTrading.Core.Kernel;

    internal class QuoteDataRequest : iTrading.Track.Request
    {
        private IntradayUpdateRequest intradayUpdateRequest;
        internal MarketData marketData;

        internal QuoteDataRequest(Adapter adapter, MarketData marketData) : base(adapter)
        {
            this.intradayUpdateRequest = null;
            this.marketData = marketData;
        }

        internal void Halt()
        {
            lock (this)
            {
                if (this.intradayUpdateRequest != null)
                {
                    this.intradayUpdateRequest.Halt();
                }
            }
        }

        internal override void Process(MessageCode msgCode, ByteReader reader)
        {
            if (Globals.TraceSwitch.MarketData)
            {
                Trace.WriteLine("(" + base.Adapter.connection.IdPlus + ") Track.QuoteDataRequest.Process: symbol='" + this.marketData.Symbol.FullName + "'");
            }
            iTrading.Track.ErrorCode code = (iTrading.Track.ErrorCode) reader.ReadByte();
            if (code == iTrading.Track.ErrorCode.NoSuchTicker)
            {
                base.Adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(base.Adapter.connection, iTrading.Core.Kernel.ErrorCode.NoSuchSymbol, "", "Symbol '" + base.Adapter.ToBrokerName(this.marketData.Symbol) + "' not supported"));
            }
            else if (code != iTrading.Track.ErrorCode.NoError)
            {
                base.Adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(base.Adapter.connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Track.QuoteDataRequest.Process: error " + code));
            }
            else
            {
                reader.ReadByte();
                reader.ReadByte();
                reader.Skip(2);
                byte format = reader.ReadByte();
                reader.ReadByte();
                reader.ReadByte();
                reader.ReadInteger();
                double price = base.CalculateFormatValue((double) reader.ReadInteger(), format);
                int volume = reader.ReadInteger();
                reader.ReadInteger();
                reader.ReadInteger();
                reader.ReadInteger();
                reader.ReadInteger();
                double num4 = base.CalculateFormatValue((double) reader.ReadInteger(), format);
                double num5 = base.CalculateFormatValue((double) reader.ReadInteger(), format);
                double num6 = base.CalculateFormatValue((double) reader.ReadInteger(), format);
                reader.ReadInteger();
                reader.ReadInteger();
                reader.ReadInteger();
                reader.ReadByte();
                reader.ReadString(2);
                reader.ReadByte();
                reader.ReadByte();
                reader.ReadCharDate();
                reader.ReadInteger();
                reader.ReadInteger();
                int blockFactor = reader.ReadShort();
                if (price > 0.0)
                {
                    base.Adapter.connection.ProcessEventArgs(new MarketDataEventArgs(this.marketData.Symbol.MarketData, iTrading.Core.Kernel.ErrorCode.NoError, "", this.marketData.Symbol, base.Adapter.connection.MarketDataTypes[MarketDataTypeId.LastClose], price, 0, base.Adapter.connection.Now));
                }
                if (volume > 0)
                {
                    base.Adapter.connection.ProcessEventArgs(new MarketDataEventArgs(this.marketData.Symbol.MarketData, iTrading.Core.Kernel.ErrorCode.NoError, "", this.marketData.Symbol, base.Adapter.connection.MarketDataTypes[MarketDataTypeId.DailyVolume], 0.0, volume, base.Adapter.connection.Now));
                }
                if (num4 > 0.0)
                {
                    base.Adapter.connection.ProcessEventArgs(new MarketDataEventArgs(this.marketData.Symbol.MarketData, iTrading.Core.Kernel.ErrorCode.NoError, "", this.marketData.Symbol, base.Adapter.connection.MarketDataTypes[MarketDataTypeId.DailyHigh], num4, 0, base.Adapter.connection.Now));
                }
                if (num5 > 0.0)
                {
                    base.Adapter.connection.ProcessEventArgs(new MarketDataEventArgs(this.marketData.Symbol.MarketData, iTrading.Core.Kernel.ErrorCode.NoError, "", this.marketData.Symbol, base.Adapter.connection.MarketDataTypes[MarketDataTypeId.DailyLow], num5, 0, base.Adapter.connection.Now));
                }
                if (num6 > 0.0)
                {
                    base.Adapter.connection.ProcessEventArgs(new MarketDataEventArgs(this.marketData.Symbol.MarketData, iTrading.Core.Kernel.ErrorCode.NoError, "", this.marketData.Symbol, base.Adapter.connection.MarketDataTypes[MarketDataTypeId.Opening], num6, 0, base.Adapter.connection.Now));
                }
                lock (this)
                {
                    this.intradayUpdateRequest = new IntradayUpdateRequest(base.Adapter, this.marketData.Symbol, volume, num4, (num5 == 0.0) ? double.MaxValue : num5, blockFactor);
                    this.intradayUpdateRequest.Send();
                }
            }
        }

        internal override iTrading.Track.ErrorCode Send()
        {
            if (Globals.TraceSwitch.MarketData)
            {
                Trace.WriteLine("(" + base.Adapter.connection.IdPlus + ") Track.QuoteDataRequest.Send: " + this.marketData.Symbol.FullName);
            }
            iTrading.Track.ErrorCode code = (iTrading.Track.ErrorCode) Api.RequestQuoteData(base.Rqn, base.Adapter.ToBrokerName(this.marketData.Symbol), "");
            if (code != iTrading.Track.ErrorCode.NoError)
            {
                base.Adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(base.Adapter.connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Track.QuoteDataRequest.Send: error " + code));
            }
            return code;
        }
    }
}

