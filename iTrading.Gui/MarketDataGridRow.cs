namespace iTrading.Gui
{
    using System;
    using System.Data;
    using System.Diagnostics;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Represents a row of the market data grid.
    /// </summary>
    public class MarketDataGridRow
    {
        internal readonly DataRow dataRow;
        private double lastClose;
        private int lastEventID = 0;
        private double lastPrice;
        private Symbol symbol;

        /// <summary>
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="dataRow"></param>
        public MarketDataGridRow(Symbol symbol, DataRow dataRow)
        {
            this.dataRow = dataRow;
            this.symbol = symbol;
        }

        internal void GridRow_MarketData(object sender, MarketDataEventArgs e)
        {
            iTrading.Gui.MarketData.totalEvents++;
            if (Globals.TraceSwitch.Strict)
            {
                if ((this.lastEventID != 0) && ((this.lastEventID + 1) != e.EventId))
                {
                    Trace.Assert(false, string.Concat(new object[] { "Gui.MarketDataGridRow.GridRow_MarketData: ", this.symbol.FullName, " ", this.lastEventID + 1, " != ", e.EventId }));
                }
                this.lastEventID = e.EventId;
            }
            if (e.Error == ErrorCode.NoError)
            {
                if (e.MarketDataType.Id == MarketDataTypeId.Ask)
                {
                    this.dataRow["Ask"] = this.symbol.FormatPrice(e.Price);
                    this.dataRow["Ask size"] = e.Volume;
                }
                else if (e.MarketDataType.Id == MarketDataTypeId.Bid)
                {
                    this.dataRow["Bid"] = this.symbol.FormatPrice(e.Price);
                    this.dataRow["Bid size"] = e.Volume;
                }
                else if (e.MarketDataType.Id == MarketDataTypeId.Last)
                {
                    this.dataRow["Last"] = this.symbol.FormatPrice(e.Price);
                    this.dataRow["Last size"] = e.Volume;
                    this.dataRow["Time"] = e.Time.ToLongTimeString();
                    this.lastPrice = e.Price;
                    this.dataRow["Change"] = this.symbol.FormatPrice(this.lastPrice - this.lastClose);
                }
                else if (e.MarketDataType.Id == MarketDataTypeId.DailyHigh)
                {
                    this.dataRow["High"] = e.Price;
                }
                else if (e.MarketDataType.Id == MarketDataTypeId.DailyLow)
                {
                    this.dataRow["Low"] = e.Price;
                }
                else if (e.MarketDataType.Id == MarketDataTypeId.DailyVolume)
                {
                    this.dataRow["Volume"] = e.Volume;
                }
                else if (e.MarketDataType.Id == MarketDataTypeId.LastClose)
                {
                    this.dataRow["YClose"] = this.symbol.FormatPrice(e.Price);
                    this.lastClose = e.Price;
                    if (this.lastPrice > 0.0)
                    {
                        this.dataRow["Change"] = this.symbol.FormatPrice(this.lastPrice - this.lastClose);
                    }
                }
            }
        }

        /// <summar>
        /// The <see cref="T: iTrading.Core.Kernel.Symbol" /> object of this grid row.
        /// </summary>
        public Symbol Symbol
        {
            get
            {
                return this.symbol;
            }
        }
    }
}

