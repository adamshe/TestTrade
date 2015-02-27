namespace iTrading.Mbt
{
    using MBTQUOTELib;
    using System;
    using iTrading.Core.Kernel;

    internal class Quotes : IMbtQuotesNotify
    {
        private Adapter adapter;
        private DateTime lastQuoteTime = Globals.MinDate;
        private Symbol symbol;
        internal double tickSize = double.MaxValue;
        private MarketDataType typeAsk;
        private MarketDataType typeBid;
        private MarketDataType typeDailyHigh;
        private MarketDataType typeDailyLow;
        private MarketDataType typeDailyVolume;
        private MarketDataType typeLast;
        private MarketDataType typeLastClose;
        private MarketDataType typeOpening;

        internal Quotes(Adapter adapter, Symbol symbol)
        {
            this.adapter = adapter;
            this.symbol = symbol;
            this.typeAsk = adapter.connection.MarketDataTypes[MarketDataTypeId.Ask];
            this.typeBid = adapter.connection.MarketDataTypes[MarketDataTypeId.Bid];
            this.typeDailyHigh = adapter.connection.MarketDataTypes[MarketDataTypeId.DailyHigh];
            this.typeDailyLow = adapter.connection.MarketDataTypes[MarketDataTypeId.DailyLow];
            this.typeDailyVolume = adapter.connection.MarketDataTypes[MarketDataTypeId.DailyVolume];
            this.typeLast = adapter.connection.MarketDataTypes[MarketDataTypeId.Last];
            this.typeLastClose = adapter.connection.MarketDataTypes[MarketDataTypeId.LastClose];
            this.typeOpening = adapter.connection.MarketDataTypes[MarketDataTypeId.Opening];
        }

        public void OnLevel2Data(ref LEVEL2RECORD e)
        {
            int length = 0;
            MarketDepthRowCollection rows = null;
            string bstrMMID = e.bstrMMID;
            length = bstrMMID.IndexOf(':');
            if (length > 0)
            {
                bstrMMID = bstrMMID.Substring(0, length);
                switch (bstrMMID)
                {
                    case "ARCA":
                        bstrMMID = "ARCHIP";
                        break;

                    case "INET":
                        bstrMMID = "INETKB";
                        break;
                }
            }
            DateTime now = this.adapter.connection.Now;
            double price = this.symbol.Round2TickSize(e.dPrice);
            string[] strArray = e.bstrTime.Split(new char[] { ':' });
            DateTime time = new DateTime(now.Year, now.Month, now.Day, Convert.ToInt32(strArray[0]), Convert.ToInt32(strArray[1]), Convert.ToInt32(strArray[2]));
            if (time < now.AddSeconds(-10.0))
            {
                time = now;
            }
            if (time < this.lastQuoteTime)
            {
                time = this.lastQuoteTime;
            }
            this.lastQuoteTime = time;
            rows = (e.side == enumMarketSide.msBid) ? this.symbol.MarketDepth.Bid : this.symbol.MarketDepth.Ask;
            for (int i = 0; i < rows.Count; i++)
            {
                MarketDepthRow row = rows[i];
                if (((price == 0.0) || (e.lSize == 0)) || (e.bClosed != 0))
                {
                    if (e.bstrMMID == ((string) row.AdapterLink))
                    {
                        this.adapter.connection.ProcessEventArgs(new MarketDepthEventArgs(this.symbol.MarketDepth, ErrorCode.NoError, "", this.symbol, i, bstrMMID, Operation.Delete, (e.side == enumMarketSide.msBid) ? this.typeBid : this.typeAsk, price, e.lSize, time));
                        return;
                    }
                }
                else
                {
                    if (e.bstrMMID == ((string) row.AdapterLink))
                    {
                        this.adapter.connection.ProcessEventArgs(new MarketDepthEventArgs(this.symbol.MarketDepth, ErrorCode.NoError, "", this.symbol, i, bstrMMID, Operation.Update, (e.side == enumMarketSide.msBid) ? this.typeBid : this.typeAsk, price, e.lSize, time));
                        return;
                    }
                    if ((e.side == enumMarketSide.msBid) ? (price > row.Price) : (price < row.Price))
                    {
                        this.adapter.connection.ProcessEventArgs(new MarketDepthEventArgs(this.symbol.MarketDepth, ErrorCode.NoError, "", this.symbol, i, bstrMMID, Operation.Insert, (e.side == enumMarketSide.msBid) ? this.typeBid : this.typeAsk, price, e.lSize, time));
                        rows[i].AdapterLink = e.bstrMMID;
                        return;
                    }
                }
            }
            if (((price != 0.0) && (e.lSize != 0)) && (e.bClosed == 0))
            {
                int count = rows.Count;
                this.adapter.connection.ProcessEventArgs(new MarketDepthEventArgs(this.symbol.MarketDepth, ErrorCode.NoError, "", this.symbol, count, bstrMMID, Operation.Insert, (e.side == enumMarketSide.msBid) ? this.typeBid : this.typeAsk, price, e.lSize, time));
                rows[count].AdapterLink = e.bstrMMID;
            }
        }

        public void OnOptionsData(ref OPTIONSRECORD e)
        {
        }

        public void OnQuoteData(ref QUOTERECORD e)
        {
            if ((this.adapter.connection.ConnectionStatusId == ConnectionStatusId.Connected) && this.symbol.MarketData.IsRunning)
            {
                double price = this.symbol.Round2TickSize(e.dAsk);
                if (((this.symbol.MarketData.Ask == null) || (this.symbol.MarketData.Ask.Price != price)) || (this.symbol.MarketData.Ask.Volume != e.lAskSize))
                {
                    this.adapter.connection.ProcessEventArgs(new MarketDataEventArgs(this.symbol.MarketData, ErrorCode.NoError, "", this.symbol, this.typeAsk, price, e.lAskSize, this.adapter.connection.Now));
                }
                price = this.symbol.Round2TickSize(e.dBid);
                if (((this.symbol.MarketData.Bid == null) || (this.symbol.MarketData.Bid.Price != price)) || (this.symbol.MarketData.Bid.Volume != e.lBidSize))
                {
                    this.adapter.connection.ProcessEventArgs(new MarketDataEventArgs(this.symbol.MarketData, ErrorCode.NoError, "", this.symbol, this.typeBid, price, e.lBidSize, this.adapter.connection.Now));
                }
                price = this.symbol.Round2TickSize(e.dLast);
                if (((this.symbol.MarketData.Last == null) || (this.symbol.MarketData.Last.Price != price)) || (this.symbol.MarketData.Last.Volume != e.lLastSize))
                {
                    this.adapter.connection.ProcessEventArgs(new MarketDataEventArgs(this.symbol.MarketData, ErrorCode.NoError, "", this.symbol, this.typeLast, price, e.lLastSize, this.adapter.connection.Now));
                }
                price = this.symbol.Round2TickSize(e.dHigh);
                if ((this.symbol.MarketData.DailyHigh == null) || (this.symbol.MarketData.DailyHigh.Price != price))
                {
                    this.adapter.connection.ProcessEventArgs(new MarketDataEventArgs(this.symbol.MarketData, ErrorCode.NoError, "", this.symbol, this.typeDailyHigh, price, 0, this.adapter.connection.Now));
                }
                price = this.symbol.Round2TickSize(e.dLow);
                if ((this.symbol.MarketData.DailyLow == null) || (this.symbol.MarketData.DailyLow.Price != price))
                {
                    this.adapter.connection.ProcessEventArgs(new MarketDataEventArgs(this.symbol.MarketData, ErrorCode.NoError, "", this.symbol, this.typeDailyLow, price, 0, this.adapter.connection.Now));
                }
                if ((this.symbol.MarketData.DailyVolume == null) || (this.symbol.MarketData.DailyVolume.Volume != e.lVolume))
                {
                    this.adapter.connection.ProcessEventArgs(new MarketDataEventArgs(this.symbol.MarketData, ErrorCode.NoError, "", this.symbol, this.typeDailyVolume, 0.0, e.lVolume, this.adapter.connection.Now));
                }
                price = this.symbol.Round2TickSize(e.dPrevClose);
                if ((this.symbol.MarketData.LastClose == null) || (this.symbol.MarketData.LastClose.Price != price))
                {
                    this.adapter.connection.ProcessEventArgs(new MarketDataEventArgs(this.symbol.MarketData, ErrorCode.NoError, "", this.symbol, this.typeLastClose, price, 0, this.adapter.connection.Now));
                }
                price = this.symbol.Round2TickSize(e.dOpen);
                if ((this.symbol.MarketData.Opening == null) || (this.symbol.MarketData.Opening.Price != price))
                {
                    this.adapter.connection.ProcessEventArgs(new MarketDataEventArgs(this.symbol.MarketData, ErrorCode.NoError, "", this.symbol, this.typeOpening, price, 0, this.adapter.connection.Now));
                }
            }
        }

        public void OnTSData(ref TSRECORD e)
        {
        }
    }
}

