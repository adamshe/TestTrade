

using iTrading.Core.Data;

namespace iTrading.Gui
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Windows.Forms;
    using iTrading.Core.Kernel;
    using iTrading.Core.Chart;
    using iTrading.Core.Data;

    /// <summary>
    /// Control to display market data (quotes and trades) for multiple symbols.
    /// Modified by Francis Gingras
    /// </summary>
    public class MarketData : UserControl
    {
        private Button AddButton;
        private MenuItem chartMenuItem;
        internal const string columnAskPrice = "Ask";
        internal const string columnAskSize = "Ask size";
        internal const string columnBidPrice = "Bid";
        internal const string columnBidSize = "Bid size";
        internal const string columnChange = "Change";
        internal const string columnExchange = "Exchange";
        internal const string columnHigh = "High";
        internal const string columnLastClose = "YClose";
        internal const string columnLastPrice = "Last";
        internal const string columnLastSize = "Last size";
        internal const string columnLow = "Low";
        private iTrading.Gui.ColumnStyle[] ColumnStyles = new iTrading.Gui.ColumnStyle[] { new iTrading.Gui.ColumnStyle("Symbol", typeof(string), 1, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("Exchange", typeof(string), 1, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("Time", typeof(string), 1, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("Change", typeof(string), 1, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("Last", typeof(string), 1, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("Last size", typeof(string), 1, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("Bid", typeof(string), 1, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("Bid size", typeof(string), 1, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("Ask", typeof(string), 1, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("Ask size", typeof(string), 1, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("Volume", typeof(string), 1, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("High", typeof(string), 1, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("Low", typeof(string), 1, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("YClose", typeof(string), 1, true, false, HorizontalAlignment.Center) };
        internal const string columnSymbol = "Symbol";
        internal const string columnTime = "Time";
        internal const string columnVolume = "Volume";
        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private Container components = null;
        private ContextMenu contextMenu1;
        private ArrayList gridRows = new ArrayList();
        private TMDataGrid marketDataGrid;
        private MenuItem marketDepthMenuItem;
        private MenuItem newsMenuItem;
        private MenuItem removeMenuItem;
        private SelectSymbol selectSymbol;
        internal static int totalEvents = 0;

        /// <summary>
        /// This event will be thrown, when the "Add" button was clicked.
        /// </summary>
        public event EventHandler AddButtonClick
        {
            add
            {
                this.AddButton.Click += value;
            }
            remove
            {
                this.AddButton.Click -= value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public MarketData()
        {
            this.InitializeComponent();
            base.Disposed += new EventHandler(this.MarketData_Disposed);
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            Symbol symbol = this.selectSymbol.Symbol;
            if (symbol == null)
            {
                this.selectSymbol.Connection.ProcessEventArgs(new ITradingErrorEventArgs(this.selectSymbol.Connection, ErrorCode.NoSuchSymbol, "", "Provider does not support this symbol"));
            }
            else
            {
                foreach (MarketDataGridRow row in this.gridRows)
                {
                    if (row.Symbol == symbol)
                    {
                        MessageBox.Show("This symbol already is selected. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        this.selectSymbol.Focus();
                        return;
                    }
                }
                this.AddSymbol(symbol);
            }
        }

        /// <summary>
        /// Add as symbol to the watch list.
        /// </summary>
        /// <param name="symbol"></param>
        public void AddSymbol(Symbol symbol)
        {
            DataRow row = this.marketDataGrid.DataTable.NewRow();
            row["Exchange"] = symbol.Exchange.Name;
            row["Symbol"] = symbol.FullName;
            this.marketDataGrid.DataTable.Rows.Add(row);
            MarketDataGridRow row2 = new MarketDataGridRow(symbol, row);
            this.gridRows.Add(row2);
            symbol.MarketData.MarketDataItem += new MarketDataItemEventHandler(row2.GridRow_MarketData);
        }

        private void chartMenuItem_Click(object sender, EventArgs e)
        {
            Symbol symbol = null;
            foreach (MarketDataGridRow row in this.gridRows)
            {
                if (row.dataRow == this.marketDataGrid.DataTable.Rows[this.marketDataGrid.CurrentRowIndex])
                {
                    symbol = row.Symbol;
                    break;
                }
            }
            if (symbol != null)
            {
                symbol.Connection.Bar += new BarUpdateEventHandler(this.Connection_Bar);
                symbol.RequestQuotes(new DateTime(0x7d4, 11, 0x11), new DateTime(0x7d4, 11, 0x17), new Period(PeriodTypeId.Minute, 1), false, LookupPolicyId.RepositoryAndProvider, this);
            }
        }

        private void Connection_Bar(object sender, BarUpdateEventArgs e)
        {
            if (e.Quotes.CustomLink == this)
            {
                e.Quotes.Symbol.Connection.Bar -= new BarUpdateEventHandler(this.Connection_Bar);
                if (e.Error != ErrorCode.NoError)
                {
                    MessageBox.Show(string.Concat(new object[] { e.NativeError, "(", e.Error, ")" }), "TradeMagic", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                else
                {
                    ChartForm form = new ChartForm();
                    for (Control control = base.Parent; control != null; control = control.Parent)
                    {
                        if (control is Form)
                        {
                            form.MdiParent = ((Form) control).MdiParent;
                            break;
                        }
                    }
                    form.StartPosition = FormStartPosition.CenterParent;
                    form.ChartControl.Quotes = e.Quotes;
                    form.Show();
                }
            }
        }

        private void contextMenu1_Popup(object sender, EventArgs e)
        {
            if (this.marketDataGrid.CurrentRowIndex > -1)
            {
                MarketDataGridRow row = (MarketDataGridRow) this.gridRows[this.marketDataGrid.CurrentRowIndex];
                this.chartMenuItem.Text = "Chart " + row.Symbol.FullName;
                this.removeMenuItem.Text = "Remove " + row.Symbol.FullName;
                this.marketDepthMenuItem.Text = "Market Depth for " + row.Symbol.FullName;
                this.chartMenuItem.Enabled = true;
                this.removeMenuItem.Enabled = true;
                this.marketDepthMenuItem.Enabled = true;
                this.newsMenuItem.Enabled = true;
            }
            else
            {
                this.chartMenuItem.Text = "Chart";
                this.removeMenuItem.Text = "Remove";
                this.marketDepthMenuItem.Text = "Market Depth";
                this.chartMenuItem.Enabled = false;
                this.removeMenuItem.Enabled = false;
                this.marketDepthMenuItem.Enabled = false;
                this.newsMenuItem.Enabled = false;
            }
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
            this.selectSymbol = new SelectSymbol();
            this.AddButton = new Button();
            this.marketDataGrid = new TMDataGrid();
            this.contextMenu1 = new ContextMenu();
            this.chartMenuItem = new MenuItem();
            this.removeMenuItem = new MenuItem();
            this.marketDepthMenuItem = new MenuItem();
            this.newsMenuItem = new MenuItem();
            this.marketDataGrid.BeginInit();
            base.SuspendLayout();
            this.selectSymbol.Connection = null;
            this.selectSymbol.Location = new Point(0, 0);
            this.selectSymbol.Name = "selectSymbol";
            this.selectSymbol.Size = new Size(0x1a0, 0x30);
            this.selectSymbol.Symbol = null;
            this.selectSymbol.TabIndex = 0;
            this.AddButton.Location = new Point(440, 0x18);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new Size(70, 20);
            this.AddButton.TabIndex = 1;
            this.AddButton.Text = "&Add";
            this.AddButton.Click += new EventHandler(this.AddButton_Click);
            this.marketDataGrid.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.marketDataGrid.CaptionVisible = false;
            this.marketDataGrid.ColumnStyles = null;
            this.marketDataGrid.ContextMenu = this.contextMenu1;
            this.marketDataGrid.DataMember = "";
            this.marketDataGrid.HeaderForeColor = SystemColors.ControlText;
            this.marketDataGrid.Location = new Point(0, 0x38);
            this.marketDataGrid.Name = "marketDataGrid";
            this.marketDataGrid.ReadOnly = true;
            this.marketDataGrid.RowHeadersVisible = false;
            this.marketDataGrid.Size = new Size(800, 0x170);
            this.marketDataGrid.TabIndex = 2;
            this.contextMenu1.MenuItems.AddRange(new MenuItem[] { this.chartMenuItem, this.removeMenuItem, this.marketDepthMenuItem, this.newsMenuItem });
            this.contextMenu1.Popup += new EventHandler(this.contextMenu1_Popup);
            this.chartMenuItem.Index = 0;
            this.chartMenuItem.Text = "Chart";
            this.chartMenuItem.Visible = false;
            this.chartMenuItem.Click += new EventHandler(this.chartMenuItem_Click);
            this.removeMenuItem.Index = 1;
            this.removeMenuItem.Shortcut = Shortcut.Del;
            this.removeMenuItem.Text = "Remove";
            this.removeMenuItem.Click += new EventHandler(this.removeMenuItem_Click);
            this.marketDepthMenuItem.Index = 2;
            this.marketDepthMenuItem.Shortcut = Shortcut.CtrlD;
            this.marketDepthMenuItem.Text = "Market Depth";
            this.marketDepthMenuItem.Click += new EventHandler(this.marketDepthMenuItem_Click);
            this.newsMenuItem.Index = 3;
            this.newsMenuItem.Text = "News";
            this.newsMenuItem.Click += new EventHandler(this.newsMenuItem_Click);
            base.Controls.Add(this.marketDataGrid);
            base.Controls.Add(this.AddButton);
            base.Controls.Add(this.selectSymbol);
            base.Name = "MarketData";
            base.Size = new Size(800, 0x1a8);
            base.Load += new EventHandler(this.MarketData_Load);
            this.marketDataGrid.EndInit();
            base.ResumeLayout(false);
        }

        private void MarketData_Disposed(object Sender, EventArgs Args)
        {
            foreach (MarketDataGridRow row in this.gridRows)
            {
                row.Symbol.MarketData.MarketDataItem -= new MarketDataItemEventHandler(row.GridRow_MarketData);
            }
            this.gridRows.Clear();
            this.marketDataGrid.DataTable.Rows.Clear();
        }

        /// <summary>
        /// Catch the enter key.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MarketData_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                e.Handled = true;
                this.AddButton_Click(this, new EventArgs());
            }
        }

        private void MarketData_Load(object sender, EventArgs e)
        {
            this.marketDataGrid.ColumnStyles = this.ColumnStyles;
            this.marketDataGrid.DataView.AllowNew = false;
            this.marketDataGrid.DataTable.RowDeleting += new DataRowChangeEventHandler(this.MarketDataGrid_RowDeleted);
            this.marketDataGrid.MouseDown += new MouseEventHandler(this.MarketDataGrid_MouseDown);
            this.selectSymbol.SymbolName.KeyPress += new KeyPressEventHandler(this.MarketData_KeyPress);
            this.selectSymbol.SymbolType.KeyPress += new KeyPressEventHandler(this.MarketData_KeyPress);
            this.selectSymbol.Exchange.KeyPress += new KeyPressEventHandler(this.MarketData_KeyPress);
            this.selectSymbol.Expiry.KeyPress += new KeyPressEventHandler(this.MarketData_KeyPress);
            totalEvents = 0;
        }

        /// <summary>
        /// Make sure the row is selected even with a right mouse click.
        /// This is to ensure proper contextMenu1_Popup functionality.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MarketDataGrid_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point position = new Point(e.X, e.Y);
                DataGrid.HitTestInfo info = this.marketDataGrid.HitTest(position);
                if (info.Type == DataGrid.HitTestType.Cell)
                {
                    this.marketDataGrid.CurrentCell = new DataGridCell(info.Row, info.Column);
                }
            }
        }

        private void MarketDataGrid_RowDeleted(object Sender, DataRowChangeEventArgs Args)
        {
            if (Args.Action == DataRowAction.Delete)
            {
                foreach (MarketDataGridRow row in this.gridRows)
                {
                    if (row.dataRow == Args.Row)
                    {
                        row.Symbol.MarketData.MarketDataItem -= new MarketDataItemEventHandler(row.GridRow_MarketData);
                        this.gridRows.Remove(row);
                        break;
                    }
                }
            }
        }

        private void marketDepthMenuItem_Click(object sender, EventArgs e)
        {
            MarketDepthForm form = new MarketDepthForm();
            form.Connection = this.selectSymbol.Connection;
            if (base.ParentForm.IsMdiChild)
            {
                form.MdiParent = base.ParentForm.MdiParent;
            }
            form.WindowState = FormWindowState.Normal;
            form.Show();
            this.selectSymbol.SymbolName.Text = ((MarketDataGridRow) this.gridRows[this.marketDataGrid.CurrentRowIndex]).Symbol.Name;
            form.Symbol = this.selectSymbol.Symbol;
        }

        private void newsMenuItem_Click(object sender, EventArgs e)
        {
            NewsItemsForm form = new NewsItemsForm();
            form.Connection = this.selectSymbol.Connection;
            if (base.ParentForm.IsMdiChild)
            {
                form.MdiParent = base.ParentForm.MdiParent;
            }
            form.WindowState = FormWindowState.Normal;
            form.Show();
        }

        private void removeMenuItem_Click(object sender, EventArgs e)
        {
            this.marketDataGrid.DataTable.Rows[this.marketDataGrid.CurrentRowIndex].Delete();
        }

        /// <summary>
        /// Get/set the connection for retrieving market data. Set the connection before this control is loaded.
        /// </summary>
        public Connection Connection
        {
            get
            {
                return this.selectSymbol.Connection;
            }
            set
            {
                this.selectSymbol.Connection = value;
            }
        }

        /// <summary>
        /// The collection of all <see cref="T:iTrading.Gui.MarketDataGridRow" /> objects.
        /// </summary>
        public ArrayList GridRows
        {
            get
            {
                return this.gridRows;
            }
        }
    }
}

