namespace iTrading.IB
{
    using System;
    using iTrading.Core.Kernel;

    internal class MarketDataRequest : iTrading.IB.StreamingRequest
    {
        private Adapter adapter;
        private Contract contract;
        private double priceAsk;
        private double priceBid;
        private double priceLast;
        private Symbol symbol;
        internal int tickerId;
        private MarketDataType typeAsk;
        private MarketDataType typeBid;
        private MarketDataType typeDailyHigh;
        private MarketDataType typeDailyLow;
        private MarketDataType typeDailyVolume;
        private MarketDataType typeLast;
        private MarketDataType typeLastClose;
        private int volumeAsk;
        private int volumeBid;
        private int volumeLast;

        internal MarketDataRequest(Adapter adapter, MarketData tmRequest, int tickerId) : base(tmRequest)
        {
            this.priceAsk = 0.0;
            this.priceBid = 0.0;
            this.priceLast = 0.0;
            this.tickerId = 0;
            this.volumeAsk = 0;
            this.volumeBid = 0;
            this.volumeLast = 0;
            this.adapter = adapter;
            this.contract = adapter.Convert(tmRequest.Symbol);
            this.symbol = tmRequest.Symbol;
            this.tickerId = tickerId;
            this.typeAsk = base.Connection.MarketDataTypes[MarketDataTypeId.Ask];
            this.typeBid = base.Connection.MarketDataTypes[MarketDataTypeId.Bid];
            this.typeDailyHigh = base.Connection.MarketDataTypes[MarketDataTypeId.DailyHigh];
            this.typeDailyLow = base.Connection.MarketDataTypes[MarketDataTypeId.DailyLow];
            this.typeDailyVolume = base.Connection.MarketDataTypes[MarketDataTypeId.DailyVolume];
            this.typeLast = base.Connection.MarketDataTypes[MarketDataTypeId.Last];
            this.typeLastClose = base.Connection.MarketDataTypes[MarketDataTypeId.LastClose];
        }

        protected override void DoCancel()
        {
            this.adapter.ibSocket.Send(2);
            this.adapter.ibSocket.Send(2);
            this.adapter.ibSocket.Send(this.tickerId);
        }

        internal void Process(Adapter adapter, int version, MessageId messageId)
        {
            ItemType lastPrice = ItemType.LastPrice;
            bool flag = false;
            string str = adapter.ibSocket.ReadString();
            MarketData tMRequest = (MarketData) base.TMRequest;
            try
            {
                lastPrice = (ItemType) Convert.ToInt32(str, adapter.ibSocket.numberFormatInfo);
            }
            catch (Exception exception)
            {
                adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(adapter.connection, ErrorCode.Panic, "", "IB.MarketData.Process: unable to convert market data type '" + str + "': " + exception.Message));
                flag = true;
            }
            if (messageId != MessageId.TickPrice)
            {
                if (messageId != MessageId.TickSize)
                {
                    return;
                }
                int volume = adapter.ibSocket.ReadInteger();
                if (volume <= 0)
                {
                    return;
                }
                if (flag)
                {
                    return;
                }
                if (flag)
                {
                    return;
                }
                int num5 = 1;
                if (this.contract.SecType == "STK")
                {
                    num5 = 100;
                }
                switch (lastPrice)
                {
                    case ItemType.AskSize:
                        this.volumeAsk = volume;
                        if ((this.priceAsk > 0.0) && (((tMRequest.Ask == null) || (tMRequest.Ask.Price != this.priceAsk)) || (tMRequest.Ask.Volume != this.volumeAsk)))
                        {
                            base.Connection.ProcessEventArgs(new MarketDataEventArgs(tMRequest, ErrorCode.NoError, "", this.symbol, this.typeAsk, this.priceAsk, this.volumeAsk, adapter.connection.Now));
                        }
                        return;

                    case ItemType.LastPrice:
                        goto Label_0580;

                    case ItemType.LastSize:
                        this.volumeLast = volume * num5;
                        if (this.priceLast > 0.0)
                        {
                            base.Connection.ProcessEventArgs(new MarketDataEventArgs(tMRequest, ErrorCode.NoError, "", this.symbol, this.typeLast, this.priceLast, this.volumeLast, adapter.connection.Now));
                        }
                        return;

                    case ItemType.DailyVolume:
                        base.Connection.ProcessEventArgs(new MarketDataEventArgs(tMRequest, ErrorCode.NoError, "", this.symbol, this.typeDailyVolume, 0.0, volume, adapter.connection.Now));
                        return;

                    case ItemType.BidSize:
                        this.volumeBid = volume;
                        if ((this.priceBid > 0.0) && (((tMRequest.Bid == null) || (tMRequest.Bid.Price != this.priceBid)) || (tMRequest.Bid.Volume != this.volumeBid)))
                        {
                            base.Connection.ProcessEventArgs(new MarketDataEventArgs(tMRequest, ErrorCode.NoError, "", this.symbol, this.typeBid, this.priceBid, this.volumeBid, adapter.connection.Now));
                        }
                        return;
                }
            }
            else
            {
                double price = adapter.ibSocket.ReadDouble();
                int num2 = 0;
                if (version >= 2)
                {
                    num2 = adapter.ibSocket.ReadInteger();
                    int num3 = 1;
                    if (this.contract.SecType == "STK")
                    {
                        num3 = 100;
                    }
                    switch (lastPrice)
                    {
                        case ItemType.BidPrice:
                            this.volumeBid = num2;
                            break;

                        case ItemType.AskPrice:
                            this.volumeAsk = num2;
                            break;

                        case ItemType.LastPrice:
                            this.volumeLast = num2 * num3;
                            break;
                    }
                }
                if (price >= 0.0)
                {
                    if (flag)
                    {
                        return;
                    }
                    switch (lastPrice)
                    {
                        case ItemType.BidPrice:
                            this.priceBid = price;
                            if ((this.volumeBid > 0) && (((tMRequest.Bid == null) || (tMRequest.Bid.Price != this.priceBid)) || (tMRequest.Bid.Volume != this.volumeBid)))
                            {
                                base.Connection.ProcessEventArgs(new MarketDataEventArgs(tMRequest, ErrorCode.NoError, "", this.symbol, this.typeBid, this.priceBid, this.volumeBid, adapter.connection.Now));
                            }
                            return;

                        case ItemType.AskPrice:
                            this.priceAsk = price;
                            if ((this.volumeAsk > 0) && (((tMRequest.Ask == null) || (tMRequest.Ask.Price != this.priceAsk)) || (tMRequest.Ask.Volume != this.volumeAsk)))
                            {
                                base.Connection.ProcessEventArgs(new MarketDataEventArgs(tMRequest, ErrorCode.NoError, "", this.symbol, this.typeAsk, this.priceAsk, this.volumeAsk, adapter.connection.Now));
                            }
                            return;

                        case ItemType.LastPrice:
                            this.priceLast = price;
                            if (this.volumeLast > 0)
                            {
                                base.Connection.ProcessEventArgs(new MarketDataEventArgs(tMRequest, ErrorCode.NoError, "", this.symbol, this.typeLast, this.priceLast, this.volumeLast, adapter.connection.Now));
                            }
                            return;

                        case ItemType.DailyHigh:
                            base.Connection.ProcessEventArgs(new MarketDataEventArgs(tMRequest, ErrorCode.NoError, "", this.symbol, this.typeDailyHigh, price, 0, adapter.connection.Now));
                            return;

                        case ItemType.DailyLow:
                            base.Connection.ProcessEventArgs(new MarketDataEventArgs(tMRequest, ErrorCode.NoError, "", this.symbol, this.typeDailyLow, price, 0, adapter.connection.Now));
                            return;

                        case ItemType.LastClose:
                            base.Connection.ProcessEventArgs(new MarketDataEventArgs(tMRequest, ErrorCode.NoError, "", this.symbol, this.typeLastClose, price, 0, adapter.connection.Now));
                            return;
                    }
                    base.Connection.ProcessEventArgs(new ITradingErrorEventArgs(base.Connection, ErrorCode.Panic, "", "IB.MarketDataRequest.Process invalid price data type =" + ((int) lastPrice)));
                }
                return;
            }
        Label_0580:
            base.Connection.ProcessEventArgs(new ITradingErrorEventArgs(base.Connection, ErrorCode.Panic, "", "IB.MarketDataRequest.Process invalid volume data type =" + ((int) lastPrice)));
        }

        internal override void Send(Adapter adapter)
        {
            adapter.ibSocket.Send(1);
            adapter.ibSocket.Send(3);
            adapter.ibSocket.Send(this.tickerId);
            adapter.ibSocket.Send(this.contract.Symbol);
            adapter.ibSocket.Send(this.contract.SecType);
            adapter.ibSocket.Send((this.contract.Expiry == Globals.MaxDate) ? "" : this.contract.Expiry.ToString("yyyyMM"));
            adapter.ibSocket.Send(this.contract.Strike);
            adapter.ibSocket.Send((string) Names.Rights[this.contract.Right]);
            adapter.ibSocket.Send(this.contract.Exchange);
            adapter.ibSocket.Send(this.contract.Currency);
            if (adapter.ibSocket.ServerVersion >= 2)
            {
                adapter.ibSocket.Send(this.contract.LocalSymbol);
            }
            if ((adapter.ibSocket.ServerVersion >= 8) && (this.contract.SecType == "BAG"))
            {
                adapter.ibSocket.Send(this.contract.ComboLegs.Count);
                foreach (ComboLeg leg in this.contract.ComboLegs)
                {
                    adapter.ibSocket.Send(leg.ConId);
                    adapter.ibSocket.Send(leg.Ratio);
                    adapter.ibSocket.Send(leg.Action);
                    adapter.ibSocket.Send(leg.ConId);
                }
            }
        }

        internal enum ItemType
        {
            BidSize,
            BidPrice,
            AskPrice,
            AskSize,
            LastPrice,
            LastSize,
            DailyHigh,
            DailyLow,
            DailyVolume,
            LastClose
        }
    }
}

