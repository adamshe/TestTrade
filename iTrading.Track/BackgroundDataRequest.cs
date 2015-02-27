namespace iTrading.Track
{
    using System;
    using System.Diagnostics;
    using iTrading.Core.Kernel;

    internal class BackgroundDataRequest : iTrading.Track.Request
    {
        private Symbol symbol;

        internal BackgroundDataRequest(Adapter adapter, Symbol symbol) : base(adapter)
        {
            this.symbol = symbol;
        }

        internal override void Process(MessageCode msgCode, ByteReader reader)
        {
            if (Globals.TraceSwitch.SymbolLookup)
            {
                Trace.WriteLine("(" + base.Adapter.connection.IdPlus + ") Track.BackgroundDataRequest.Process: symbol='" + this.symbol.FullName + "'");
            }
            iTrading.Track.ErrorCode code = (iTrading.Track.ErrorCode) reader.ReadByte();
            if (code == iTrading.Track.ErrorCode.NoData)
            {
                base.Adapter.connection.ProcessEventArgs(new SymbolEventArgs(base.Adapter.connection, iTrading.Core.Kernel.ErrorCode.NoSuchSymbol, "No such symbol", null));
            }
            else if (code != iTrading.Track.ErrorCode.NoError)
            {
                base.Adapter.connection.ProcessEventArgs(new SymbolEventArgs(base.Adapter.connection, iTrading.Core.Kernel.ErrorCode.NoSuchSymbol, "Error '" + code + "' on symbol lookup", null));
            }
            else
            {
                reader.ReadByte();
                string companyName = reader.ReadString(0x20);
                base.Adapter.connection.CreateSymbol(this.symbol.Name, this.symbol.Expiry, this.symbol.SymbolType, this.symbol.Exchange, this.symbol.StrikePrice, this.symbol.Right.Id, base.Adapter.connection.Currencies[CurrencyId.UsDollar], 0.01, 1.0, companyName, null, 0, null, null, null);
            }
        }

        internal override iTrading.Track.ErrorCode Send()
        {
            if (Globals.TraceSwitch.SymbolLookup)
            {
                Trace.WriteLine("(" + base.Adapter.connection.IdPlus + ") Track.BackgroundDataRequest.Send: " + this.symbol.FullName);
            }
            iTrading.Track.ErrorCode code = (iTrading.Track.ErrorCode) Api.RequestBackgroundData(base.Rqn, base.Adapter.ToBrokerName(this.symbol), "");
            if (code != iTrading.Track.ErrorCode.NoError)
            {
                base.Adapter.connection.ProcessEventArgs(new SymbolEventArgs(base.Adapter.connection, iTrading.Core.Kernel.ErrorCode.NoSuchSymbol, "Error '" + code + "' on symbol lookup", null));
            }
            return code;
        }
    }
}

