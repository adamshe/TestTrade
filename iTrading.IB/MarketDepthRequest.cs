namespace iTrading.IB
{
    using System;
    using iTrading.Core.Kernel;

    internal class MarketDepthRequest : iTrading.IB.StreamingRequest
    {
        private Adapter adapter;
        private readonly Contract contract;
        private Symbol symbol;
        internal int tickerId;
        private MarketDataType typeAsk;
        private MarketDataType typeBid;

        internal MarketDepthRequest(Adapter adapter, MarketDepth tmRequest, int tickerId) : base(tmRequest)
        {
            this.tickerId = 0;
            this.adapter = adapter;
            this.contract = adapter.Convert(tmRequest.Symbol);
            this.symbol = tmRequest.Symbol;
            this.tickerId = tickerId;
            this.typeAsk = base.Connection.MarketDataTypes[MarketDataTypeId.Ask];
            this.typeBid = base.Connection.MarketDataTypes[MarketDataTypeId.Bid];
        }

        protected override void DoCancel()
        {
            if (this.adapter.ibSocket.ServerVersion >= 6)
            {
                this.adapter.ibSocket.Send(11);
                this.adapter.ibSocket.Send(1);
                this.adapter.ibSocket.Send(this.tickerId);
            }
        }

        internal void Process(Adapter adapter, int version, MessageId messageId)
        {
            MarketDataType typeAsk;
            string marketMaker = "";
            DateTime now = adapter.connection.Now;
            int position = adapter.ibSocket.ReadInteger();
            if (messageId == MessageId.MarketDepth2)
            {
                marketMaker = adapter.ibSocket.ReadString();
            }
            iTrading.Core.Kernel.Operation operation = adapter.Convert((iTrading.IB.Operation)adapter.ibSocket.ReadInteger());
            int num3 = adapter.ibSocket.ReadInteger();
            double price = adapter.ibSocket.ReadDouble();
            int num4 = adapter.ibSocket.ReadInteger();
            if (operation == iTrading.Core.Kernel.Operation.Delete)
            {
                price = 0.0;
                num4 = 0;
            }
            switch (num3)
            {
                case 0:
                    typeAsk = this.typeAsk;
                    break;

                case 1:
                    typeAsk = this.typeBid;
                    break;

                default:
                    typeAsk = base.Connection.MarketDataTypes[MarketDataTypeId.Unknown];
                    adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(adapter.connection, ErrorCode.Panic, "", "IB.MarketDepthRequest.Process: can't convert internal IB side type " + num3));
                    break;
            }
            adapter.connection.ProcessEventArgs(new MarketDepthEventArgs((MarketDepth) base.TMRequest, ErrorCode.NoError, "", this.symbol, position, marketMaker, operation, typeAsk, price, (this.symbol.SymbolType.Id == SymbolTypeId.Stock) ? (num4 * 100) : num4, now));
        }

        internal override void Send(Adapter adapter)
        {
            if (adapter.ibSocket.ServerVersion >= 6)
            {
                adapter.ibSocket.Send(10);
                adapter.ibSocket.Send(1);
                adapter.ibSocket.Send(this.tickerId);
                adapter.ibSocket.Send(this.contract.Symbol);
                adapter.ibSocket.Send(this.contract.SecType);
                adapter.ibSocket.Send((this.contract.Expiry == Globals.MaxDate) ? "" : this.contract.Expiry.ToString("yyyyMM"));
                adapter.ibSocket.Send(this.contract.Strike);
                adapter.ibSocket.Send((string) Names.Rights[this.contract.Right]);
                adapter.ibSocket.Send(this.contract.Exchange);
                adapter.ibSocket.Send(this.contract.Currency);
                adapter.ibSocket.Send(this.contract.LocalSymbol);
            }
        }
    }
}

