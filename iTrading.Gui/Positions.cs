using iTrading.Core.Kernel;

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
    /// Control to display account positions.
    /// </summary>
    public class Positions : UserControl
    {
        private Account account = null;
        private MenuItem closeAllMenuItem;
        private MenuItem closeMenuItem;
        private const string columnAvgPrice = "Avg. price";
        private const string columnCurrency = "Currency";
        private const string columnMarketPosition = "Market pos.";
        private const string columnQuantity = "#";
        private iTrading.Gui.ColumnStyle[] columnStyles = new iTrading.Gui.ColumnStyle[] { new iTrading.Gui.ColumnStyle("Symbol", typeof(string), 1, true, false, HorizontalAlignment.Right), new iTrading.Gui.ColumnStyle("#", typeof(int), 1, true, false, HorizontalAlignment.Right), new iTrading.Gui.ColumnStyle("Market pos.", typeof(string), 1, true, false, HorizontalAlignment.Right), new iTrading.Gui.ColumnStyle("Currency", typeof(string), 1, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("Avg. price", typeof(string), 1, true, false, HorizontalAlignment.Center) };
        private const string columnSymbol = "Symbol";
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private Container components = null;
        private ContextMenu contextMenu1;
        private Hashtable position2DataRow = new Hashtable();
        private TMDataGrid positionsDataGrid;

        /// <summary>
        /// 
        /// </summary>
        public Positions()
        {
            this.InitializeComponent();
            base.Disposed += new EventHandler(this.Positions_Disposed);
        }

        private void AddPosition(Position position)
        {
            if (position.Account == this.account)
            {
                DataRow row = this.positionsDataGrid.DataTable.NewRow();
                row["Symbol"] = "";
                this.position2DataRow.Add(position, row);
                this.positionsDataGrid.DataTable.Rows.Add(row);
                this.UpdateDataRow(row, position);
            }
        }

        private void closeAllMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Position position in this.position2DataRow.Keys)
            {
                if (this.positionsDataGrid.DataTable.Rows.Find(position.Symbol.FullName) != null)
                {
                    this.ClosePosition(position, position.Symbol.Exchange);
                }
            }
        }

        private void closeMenuItem_Click(object sender, EventArgs e)
        {
            if (this.positionsDataGrid.CurrentRowIndex >= 0)
            {
                DataRow row = this.positionsDataGrid.DataTable.Rows[this.positionsDataGrid.CurrentRowIndex];
                foreach (Position position in this.position2DataRow.Keys)
                {
                    Exchange exchange;
                    if (this.position2DataRow[position] != row)
                    {
                        continue;
                    }
                    if (position.Symbol.Exchanges.Count > 1)
                    {
                        exchange = position.Symbol.Exchanges.Find(((MenuItem) sender).Text);
                    }
                    else
                    {
                        exchange = position.Symbol.Exchange;
                    }
                    if (exchange != null)
                    {
                        this.ClosePosition(position, exchange);
                    }
                    break;
                }
            }
        }

        /// <summary>
        /// Closes the position at market and on the selected Exchange.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="exchange"></param>
        private void ClosePosition(Position position, Exchange exchange)
        {
            if (this.account == null)
            {
                throw new TMException(ErrorCode.GuiNotInitialized, "Positions.Account property is NULL. Control is not initialized properly.");
            }
            if (this.account.Connection.ConnectionStatusId != ConnectionStatusId.Connected)
            {
                this.account.Connection.ProcessEventArgs(new ITradingErrorEventArgs(this.account.Connection, ErrorCode.NotConnected, "", "Connection is closed. Unable to submit order."));
            }
            else if (exchange == null)
            {
                this.account.Connection.ProcessEventArgs(new ITradingErrorEventArgs(this.account.Connection, ErrorCode.Panic, "", "Error in ClosePosition: exchange parameter is null."));
            }
            else if (position == null)
            {
                this.account.Connection.ProcessEventArgs(new ITradingErrorEventArgs(this.account.Connection, ErrorCode.Panic, "", "Error in ClosePosition: position parameter is null."));
            }
            else
            {
                ActionTypeId sell;
                if (position.MarketPosition.Id == MarketPositionId.Long)
                {
                    sell = ActionTypeId.Sell;
                }
                else
                {
                    sell = ActionTypeId.BuyToCover;
                }
                Symbol symbol = this.account.Connection.GetSymbol(position.Symbol.Name, position.Symbol.Expiry, position.Symbol.SymbolType, exchange, position.Symbol.StrikePrice, position.Symbol.Right.Id, LookupPolicyId.RepositoryAndProvider);
                if (symbol == null)
                {
                    this.account.Connection.ProcessEventArgs(new ITradingErrorEventArgs(this.account.Connection, ErrorCode.NoSuchSymbol, "", "Error in ClosePosition:Invalid Exchange"));
                }
                else
                {
                    TimeInForce force = TimeInForce.All[TimeInForceId.Day];
                    if (!this.account.IsSimulation)
                    {
                        force = this.account.Connection.TimeInForces[TimeInForceId.Day];
                        if (force == null)
                        {
                            force = this.account.Connection.TimeInForces[TimeInForceId.Gtc];
                        }
                    }
                    Order order = this.account.CreateOrder(symbol, sell, OrderTypeId.Market, force.Id, position.Quantity, 0.0, 0.0, "", null);
                    if (order != null)
                    {
                        order.Submit();
                    }
                }
            }
        }

        private void contextMenu1_Popup(object sender, EventArgs e)
        {
            this.closeMenuItem.MenuItems.Clear();
            if (this.positionsDataGrid.CurrentRowIndex <= -1)
            {
                this.closeMenuItem.Text = "Close";
                this.closeMenuItem.Enabled = false;
                this.closeAllMenuItem.Enabled = false;
            }
            else
            {
                DataRow row = this.positionsDataGrid.DataTable.Rows[this.positionsDataGrid.CurrentRowIndex];
                Position position = null;
                foreach (Position position2 in this.position2DataRow.Keys)
                {
                    if (!(position2.Symbol.FullName == ((string) row["Symbol"])))
                    {
                        continue;
                    }
                    this.closeMenuItem.Text = "Close " + position2.MarketPosition.Name + " " + position2.Quantity.ToString() + " " + position2.Symbol.FullName + " At Market";
                    if (position2.Symbol.Exchanges.Count > 1)
                    {
                        foreach (Exchange exchange in position2.Symbol.Exchanges.Values)
                        {
                            this.closeMenuItem.MenuItems.Add(new MenuItem(exchange.Name, new EventHandler(this.closeMenuItem_Click)));
                        }
                    }
                    else
                    {
                        this.closeMenuItem.Text = this.closeMenuItem.Text + " on " + position2.Symbol.Exchange.Name;
                    }
                    position = position2;
                    break;
                }
                if (position == null)
                {
                    this.closeMenuItem.Enabled = false;
                    this.closeAllMenuItem.Enabled = false;
                }
                else
                {
                    this.closeMenuItem.Enabled = true;
                    this.closeAllMenuItem.Enabled = true;
                }
            }
        }

        /// <summary> 
        /// Die verwendeten Ressourcen bereinigen.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.positionsDataGrid = new TMDataGrid();
            this.contextMenu1 = new ContextMenu();
            this.closeMenuItem = new MenuItem();
            this.closeAllMenuItem = new MenuItem();
            this.positionsDataGrid.BeginInit();
            base.SuspendLayout();
            this.positionsDataGrid.CaptionVisible = false;
            this.positionsDataGrid.ColumnStyles = null;
            this.positionsDataGrid.ContextMenu = this.contextMenu1;
            this.positionsDataGrid.DataMember = "";
            this.positionsDataGrid.Dock = DockStyle.Fill;
            this.positionsDataGrid.HeaderForeColor = SystemColors.ControlText;
            this.positionsDataGrid.Location = new Point(0, 0);
            this.positionsDataGrid.Name = "positionsDataGrid";
            this.positionsDataGrid.RowHeadersVisible = false;
            this.positionsDataGrid.Size = new Size(0x250, 0x158);
            this.positionsDataGrid.TabIndex = 0;
            this.contextMenu1.MenuItems.AddRange(new MenuItem[] { this.closeMenuItem, this.closeAllMenuItem });
            this.contextMenu1.Popup += new EventHandler(this.contextMenu1_Popup);
            this.closeMenuItem.Index = 0;
            this.closeMenuItem.Shortcut = Shortcut.Del;
            this.closeMenuItem.Text = "Close";
            this.closeMenuItem.Click += new EventHandler(this.closeMenuItem_Click);
            this.closeAllMenuItem.Index = 1;
            this.closeAllMenuItem.Shortcut = Shortcut.CtrlDel;
            this.closeAllMenuItem.Text = "Close All At Market";
            this.closeAllMenuItem.Click += new EventHandler(this.closeAllMenuItem_Click);
            base.Controls.Add(this.positionsDataGrid);
            base.Name = "Positions";
            base.Size = new Size(0x250, 0x158);
            base.Load += new EventHandler(this.Positions_Load);
            this.positionsDataGrid.EndInit();
            base.ResumeLayout(false);
        }

        private void Positions_Disposed(object Sender, EventArgs Args)
        {
            this.account.PositionUpdate -= new PositionUpdateEventHandler(this.positions_PositionUpdate);
        }

        private void Positions_Load(object sender, EventArgs e)
        {
            if (Application.ExecutablePath.ToLower().IndexOf("devenv.exe") <= -1)
            {
                this.positionsDataGrid.ColumnStyles = this.columnStyles;
                this.positionsDataGrid.DataView.AllowNew = false;
                this.positionsDataGrid.DataTable.PrimaryKey = new DataColumn[] { this.positionsDataGrid.DataTable.Columns["Symbol"] };
                if (this.account == null)
                {
                    throw new TMException(ErrorCode.GuiNotInitialized, "Positions.Account property is NULL. Control is not initialized properly.");
                }
                if (this.account.Connection.ConnectionStatusId == ConnectionStatusId.Connected)
                {
                    foreach (Position position in this.account.Positions)
                    {
                        this.AddPosition(position);
                    }
                    this.account.PositionUpdate += new PositionUpdateEventHandler(this.positions_PositionUpdate);
                    this.positionsDataGrid.MouseDown += new MouseEventHandler(this.positionsDataGrid_MouseDown);
                }
            }
        }

        private void positions_PositionUpdate(object sender, PositionUpdateEventArgs e)
        {
            if (e.Operation == Operation.Insert)
            {
                this.AddPosition(e.Position);
            }
            else if (e.Operation == Operation.Update)
            {
                DataRow dataRow = (DataRow) this.position2DataRow[e.Position];
                this.UpdateDataRow(dataRow, e.Position);
            }
            else if (e.Operation == Operation.Delete)
            {
                this.positionsDataGrid.DataTable.Rows.Remove((DataRow) this.position2DataRow[e.Position]);
                this.position2DataRow.Remove(e.Position);
            }
            else
            {
                this.account.Connection.ProcessEventArgs(new ITradingErrorEventArgs(this.account.Connection, ErrorCode.Panic, "", "Unknown Operation in Positions.PositionUpdate: " + ((int) e.Operation)));
            }
        }

        /// <summary>
        /// Make sure the row is selected even with a right mouse click.
        /// This is to ensure proper contextMenu1_Popup functionality.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void positionsDataGrid_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point position = new Point(e.X, e.Y);
                DataGrid.HitTestInfo info = this.positionsDataGrid.HitTest(position);
                if (info.Type == DataGrid.HitTestType.Cell)
                {
                    this.positionsDataGrid.CurrentCell = new DataGridCell(info.Row, info.Column);
                }
            }
        }

        private void UpdateDataRow(DataRow dataRow, Position position)
        {
            dataRow["Avg. price"] = position.Symbol.FormatPrice(position.AvgPrice);
            dataRow["Currency"] = position.Currency.Name;
            dataRow["#"] = position.Quantity;
            dataRow["Symbol"] = position.Symbol.FullName;
            dataRow["Market pos."] = position.MarketPosition.Name;
        }

        /// <summary>
        /// Get/set the account holding the positions. Set the account before this control is loaded.
        /// </summary>
        public Account Account
        {
            get
            {
                return this.account;
            }
            set
            {
                this.account = value;
            }
        }
    }
}

