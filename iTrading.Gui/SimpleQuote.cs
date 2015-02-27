namespace iTrading.Gui
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Windows.Forms;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Control to display market data (quotes and trades) for multiple symbols.
    /// Input a ticker in Symbol and the grid will fill with streaming data.
    /// Based on MarketData.cs
    /// </summary>
    public class SimpleQuote : UserControl
    {
        private const string columnAskPrice = "Ask";
        private const string columnAskSize = "Ask size";
        private const string columnBidPrice = "Bid";
        private const string columnBidSize = "Bid size";
        private const string columnChange = "Change";
        private const string columnExchange = "Exchange";
        private const string columnHigh = "High";
        private const string columnLastClose = "Last Close";
        private const string columnLastPrice = "Last";
        private const string columnLastSize = "Last size";
        private const string columnLow = "Low";
        private iTrading.Gui.ColumnStyle[] columnStyles = new iTrading.Gui.ColumnStyle[] { new iTrading.Gui.ColumnStyle("Symbol", typeof(string), 2, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("Time", typeof(string), 1, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("Change", typeof(string), 1, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("Last", typeof(string), 1, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("Last size", typeof(string), 1, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("Bid", typeof(string), 1, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("Ask", typeof(string), 1, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("Volume", typeof(string), 1, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("High", typeof(string), 1, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("Low", typeof(string), 1, true, false, HorizontalAlignment.Center) };
        private const string columnSymbol = "Symbol";
        private const string columnTime = "Time";
        private const string columnVolume = "Volume";
        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private Container components = null;
        private Connection connection = null;
        private ArrayList gridRows = new ArrayList(1);
        private TMDataGrid marketDataGrid;
        private Symbol symbol = null;

        /// <summary>
        /// Displays a quote on a separate thread.
        /// </summary>
        public SimpleQuote()
        {
            this.InitializeComponent();
            base.Disposed += new EventHandler(this.MarketData_Disposed);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.marketDataGrid = new TMDataGrid();
            this.marketDataGrid.BeginInit();
            base.SuspendLayout();
            this.marketDataGrid.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.marketDataGrid.CaptionVisible = false;
            this.marketDataGrid.ColumnStyles = null;
            this.marketDataGrid.DataMember = "";
            this.marketDataGrid.FlatMode = true;
            this.marketDataGrid.HeaderForeColor = SystemColors.ControlText;
            this.marketDataGrid.Location = new Point(0, 0);
            this.marketDataGrid.Name = "marketDataGrid";
            this.marketDataGrid.ParentRowsVisible = false;
            this.marketDataGrid.PreferredColumnWidth = 50;
            this.marketDataGrid.RowHeadersVisible = false;
            this.marketDataGrid.Size = new Size(0x2d8, 0x2d);
            this.marketDataGrid.TabIndex = 0;
            this.marketDataGrid.TabStop = false;
            base.Controls.Add(this.marketDataGrid);
            base.Name = "SimpleQuote";
            base.Size = new Size(0x2d8, 0x30);
            base.Load += new EventHandler(this.SimpleQuote_Load);
            this.marketDataGrid.EndInit();
            base.ResumeLayout(false);
        }

        private void MarketData_Disposed(object sender, EventArgs args)
        {
            foreach (GridRow row in this.gridRows)
            {
                if (row.dataRow["Symbol"].ToString().Length != 0)
                {
                    row.symbol.MarketData.MarketDataItem -= new MarketDataItemEventHandler(row.GridRow_MarketData);
                }
            }
            this.gridRows.Clear();
            this.marketDataGrid.DataTable.Rows.Clear();
        }

        /// <summary>
        /// Start a data stream and fill the grid with the symbol data.
        /// </summary>
        /// <param name="symbol"></param>
        public void SelectSymbol(Symbol symbol)
        {
            if (this.connection == null)
            {
                throw new TMException(ErrorCode.GuiNotInitialized, "SimpleQuote.Connection property is NULL. Control is not initialized properly.");
            }
            GridRow row = (GridRow) this.gridRows[0];
            if (row.dataRow["Symbol"].ToString().Length > 0)
            {
                row.symbol.MarketData.MarketDataItem -= new MarketDataItemEventHandler(row.GridRow_MarketData);
                for (int i = 0; i < row.dataRow.Table.Columns.Count; i++)
                {
                    row.dataRow[i] = DBNull.Value;
                }
            }
            if (symbol != null)
            {
                row.dataRow["Symbol"] = symbol.FullName;
                this.gridRows.Clear();
                GridRow row2 = new GridRow(symbol, this.marketDataGrid.DataTable.Rows[0]);
                this.gridRows.Add(row2);
                symbol.MarketData.MarketDataItem += new MarketDataItemEventHandler(((GridRow) this.gridRows[0]).GridRow_MarketData);
            }
        }

        private void SimpleQuote_Load(object sender, EventArgs e)
        {
            this.marketDataGrid.ColumnStyles = this.columnStyles;
            this.marketDataGrid.DataView.AllowNew = false;
            DataRow row = this.marketDataGrid.DataTable.NewRow();
            this.marketDataGrid.DataTable.Rows.Add(row);
            this.gridRows.Add(new GridRow(null, row));
        }

        /// <summary>
        /// get/Set the connection for the control.
        /// This must be set before using the control.
        /// </summary>
        public Connection Connection
        {
            get
            {
                return this.connection;
            }
            set
            {
                this.connection = value;
            }
        }

        /// <summary>
        /// The current symbol. NULL, if symbol is not supported by this broker / data provider.
        /// </summary>
        public Symbol Symbol
        {
            get
            {
                return this.symbol;
            }
        }

        private class GridRow
        {
            public readonly DataRow dataRow;
            private double lastClose;
            private double lastPrice;
            public readonly Symbol symbol;

            public GridRow(Symbol symbol, DataRow dataRow)
            {
                this.dataRow = dataRow;
                this.symbol = symbol;
            }

            /// <summary>
            /// Process MarketData here.
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            public void GridRow_MarketData(object sender, MarketDataEventArgs e)
            {
                if (e.Error == ErrorCode.NoError)
                {
                    if (e.MarketDataType.Id == MarketDataTypeId.Ask)
                    {
                        this.dataRow["Ask"] = this.symbol.FormatPrice(e.Price);
                    }
                    else if (e.MarketDataType.Id == MarketDataTypeId.Bid)
                    {
                        this.dataRow["Bid"] = this.symbol.FormatPrice(e.Price);
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
                        this.dataRow["High"] = this.symbol.FormatPrice(e.Price);
                    }
                    else if (e.MarketDataType.Id == MarketDataTypeId.DailyLow)
                    {
                        this.dataRow["Low"] = this.symbol.FormatPrice(e.Price);
                    }
                    else if (e.MarketDataType.Id == MarketDataTypeId.DailyVolume)
                    {
                        this.dataRow["Volume"] = e.Volume;
                    }
                    else if (e.MarketDataType.Id == MarketDataTypeId.LastClose)
                    {
                        this.lastClose = e.Price;
                        if (this.lastPrice > 0.0)
                        {
                            this.dataRow["Change"] = this.symbol.FormatPrice(this.lastPrice - this.lastClose);
                        }
                    }
                }
            }
        }
    }
}

