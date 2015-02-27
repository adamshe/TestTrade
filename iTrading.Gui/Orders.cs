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
    /// Control to display open orders.
    /// </summary>
    public class Orders : UserControl
    {
        private Account account = null;
        private MenuItem cancelAllMenuItem;
        private MenuItem cancelMenuItem;
        private MenuItem changeMenuItem;
        private const string columnAction = "Action";
        private const string columnAvgFillPrice = "Avg. Price";
        private const string columnExchange = "Exchange";
        private const string columnFilled = "Filled";
        private const string columnLimitPrice = "Limit";
        private const string columnOcaGroup = "Oca Group";
        private const string columnOrderId = "Order Id";
        private const string columnOrderType = "Type";
        private const string columnQuantity = "Units";
        private const string columnStatus = "Status";
        private const string columnStopPrice = "Stop";
        private iTrading.Gui.ColumnStyle[] columnStyles = new iTrading.Gui.ColumnStyle[] { new iTrading.Gui.ColumnStyle("Order Id", typeof(string), 2, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("Symbol", typeof(string), 2, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("Exchange", typeof(string), 1, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("Units", typeof(int), 1, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("Action", typeof(string), 1, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("Oca Group", typeof(string), 1, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("Type", typeof(string), 1, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("Tif", typeof(string), 1, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("Limit", typeof(string), 1, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("Stop", typeof(string), 1, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("Status", typeof(string), 1, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("Filled", typeof(int), 1, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("Avg. Price", typeof(double), 1, true, false, HorizontalAlignment.Center) };
        private const string columnSymbol = "Symbol";
        private const string columnTimeInForce = "Tif";
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private Container components = null;
        private ContextMenu contextMenu1;
        private Hashtable order2DataRow = new Hashtable();
        private TMDataGrid ordersDataGrid;
        private MenuItem seperatorMenuItem;

        /// <summary>
        /// 
        /// </summary>
        public Orders()
        {
            this.InitializeComponent();
            base.Disposed += new EventHandler(this.Orders_Disposed);
        }

        private void AddOrder(Order order)
        {
            if (order.Account == this.account)
            {
                lock (this.order2DataRow)
                {
                    if (!this.order2DataRow.Contains(order))
                    {
                        DataRow row = this.ordersDataGrid.DataTable.NewRow();
                        this.ordersDataGrid.DataTable.Rows.Add(row);
                        this.order2DataRow.Add(order, row);
                        this.UpdateDataRow(row, order);
                    }
                }
            }
        }

        /// <summary>
        /// Cencel all open orders.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelAllMenuItem_Click(object sender, EventArgs e)
        {
            ArrayList list = new ArrayList();
            foreach (Order order in this.order2DataRow.Keys)
            {
                list.Add(order);
            }
            foreach (Order order2 in list)
            {
                order2.Cancel();
            }
        }

        private void cancelMenuItem_Click(object sender, EventArgs e)
        {
            if (this.ordersDataGrid.CurrentRowIndex >= 0)
            {
                DataRow row = this.ordersDataGrid.DataTable.Rows[this.ordersDataGrid.CurrentRowIndex];
                foreach (Order order in this.order2DataRow.Keys)
                {
                    if (this.order2DataRow[order] == row)
                    {
                        order.Cancel();
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Cancel/replace an order.
        /// Displays a OrderEntry dialog.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void changeMenuItem_Click(object sender, EventArgs e)
        {
            if (this.ordersDataGrid.CurrentRowIndex >= 0)
            {
                DataRow row = this.ordersDataGrid.DataTable.Rows[this.ordersDataGrid.CurrentRowIndex];
                foreach (Order order in this.order2DataRow.Keys)
                {
                    if (this.order2DataRow[order] == row)
                    {
                        OrderEntry entry = new OrderEntry(order);
                        entry.LimitPrice.Symbol = order.Symbol;
                        entry.StopPrice.Symbol = order.Symbol;
                        Form form = new Form();
                        form.ClientSize = entry.Size;
                        form.Controls.Add(entry);
                        form.Text = "Change Order";
                        form.ShowInTaskbar = false;
                        form.WindowState = FormWindowState.Normal;
                        form.FormBorderStyle = FormBorderStyle.FixedDialog;
                        form.StartPosition = FormStartPosition.CenterParent;
                        form.ShowDialog();
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Shows/hides popup menu items based on the order.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contextMenu1_Popup(object sender, EventArgs e)
        {
            if (this.ordersDataGrid.CurrentRowIndex <= -1)
            {
                this.cancelMenuItem.Text = "Cancel";
                this.cancelAllMenuItem.Text = "Cancel All";
                this.changeMenuItem.Text = "Change";
                this.cancelMenuItem.Enabled = false;
                this.cancelAllMenuItem.Enabled = false;
                this.changeMenuItem.Enabled = false;
            }
            else
            {
                DataRow row = this.ordersDataGrid.DataTable.Rows[this.ordersDataGrid.CurrentRowIndex];
                Order order = null;
                foreach (Order order2 in this.order2DataRow.Keys)
                {
                    if ((order2.OrderId == ((string) row["Order Id"])) && (order2.Symbol.FullName == ((string) row["Symbol"])))
                    {
                        this.cancelMenuItem.Text = string.Concat(new object[] { "Cancel ", order2.Action.Name, " ", order2.Quantity, " ", order2.Symbol.Name, " at ", order2.OrderType.Name });
                        this.changeMenuItem.Text = string.Concat(new object[] { "Change ", order2.Action.Name, " ", order2.Quantity, " ", order2.Symbol.Name, " at ", order2.OrderType.Name });
                        order = order2;
                        break;
                    }
                }
                if (((order == null) || (order.OrderState.Id == OrderStateId.PendingCancel)) || ((order.OrderState.Id == OrderStateId.Rejected) || (order.OrderState.Id == OrderStateId.Cancelled)))
                {
                    this.cancelMenuItem.Enabled = false;
                    this.cancelAllMenuItem.Enabled = false;
                    this.changeMenuItem.Enabled = false;
                }
                else
                {
                    if (order.OrderId.Length == 0)
                    {
                        this.changeMenuItem.Enabled = false;
                    }
                    else
                    {
                        this.changeMenuItem.Enabled = true;
                    }
                    this.cancelAllMenuItem.Enabled = true;
                    this.cancelMenuItem.Enabled = true;
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
            this.ordersDataGrid = new TMDataGrid();
            this.contextMenu1 = new ContextMenu();
            this.cancelMenuItem = new MenuItem();
            this.cancelAllMenuItem = new MenuItem();
            this.seperatorMenuItem = new MenuItem();
            this.changeMenuItem = new MenuItem();
            this.ordersDataGrid.BeginInit();
            base.SuspendLayout();
            this.ordersDataGrid.CaptionVisible = false;
            this.ordersDataGrid.ColumnStyles = null;
            this.ordersDataGrid.ContextMenu = this.contextMenu1;
            this.ordersDataGrid.DataMember = "";
            this.ordersDataGrid.Dock = DockStyle.Fill;
            this.ordersDataGrid.HeaderForeColor = SystemColors.ControlText;
            this.ordersDataGrid.Location = new Point(0, 0);
            this.ordersDataGrid.Name = "ordersDataGrid";
            this.ordersDataGrid.RowHeadersVisible = false;
            this.ordersDataGrid.RowHeaderWidth = 20;
            this.ordersDataGrid.Size = new Size(0x250, 0x158);
            this.ordersDataGrid.TabIndex = 0;
            this.contextMenu1.MenuItems.AddRange(new MenuItem[] { this.cancelMenuItem, this.cancelAllMenuItem, this.seperatorMenuItem, this.changeMenuItem });
            this.contextMenu1.Popup += new EventHandler(this.contextMenu1_Popup);
            this.cancelMenuItem.Index = 0;
            this.cancelMenuItem.Shortcut = Shortcut.Del;
            this.cancelMenuItem.Text = "Cancel";
            this.cancelMenuItem.Click += new EventHandler(this.cancelMenuItem_Click);
            this.cancelAllMenuItem.Index = 1;
            this.cancelAllMenuItem.Shortcut = Shortcut.CtrlDel;
            this.cancelAllMenuItem.Text = "Cancel All";
            this.cancelAllMenuItem.Click += new EventHandler(this.cancelAllMenuItem_Click);
            this.seperatorMenuItem.Index = 2;
            this.seperatorMenuItem.Text = "-";
            this.changeMenuItem.Index = 3;
            this.changeMenuItem.Text = "Change ...";
            this.changeMenuItem.Click += new EventHandler(this.changeMenuItem_Click);
            base.Controls.Add(this.ordersDataGrid);
            base.Name = "Orders";
            base.Size = new Size(0x250, 0x158);
            base.Load += new EventHandler(this.Orders_Load);
            this.ordersDataGrid.EndInit();
            base.ResumeLayout(false);
        }

        private void Orders_Disposed(object Sender, EventArgs Args)
        {
            this.Account.OrderStatus -= new OrderStatusEventHandler(this.Orders_OrderStatus);
        }

        private void Orders_Load(object sender, EventArgs e)
        {
            if (Application.ExecutablePath.ToLower().IndexOf("devenv.exe") <= -1)
            {
                if (!this.account.IsSimulation && (this.account.Connection.FeatureTypes[FeatureTypeId.OrderChange] == null))
                {
                    this.contextMenu1.MenuItems.Remove(this.seperatorMenuItem);
                    this.contextMenu1.MenuItems.Remove(this.changeMenuItem);
                }
                this.ordersDataGrid.ColumnStyles = this.columnStyles;
                this.ordersDataGrid.DataView.AllowNew = false;
                if (this.account == null)
                {
                    throw new TMException(ErrorCode.GuiNotInitialized, "Orders.Account property is NULL. Control is not initialized properly.");
                }
                if (this.account.Connection.ConnectionStatusId == ConnectionStatusId.Connected)
                {
                    foreach (Order order in this.account.Orders)
                    {
                        if (((order.OrderState.Id != OrderStateId.Cancelled) && (order.OrderState.Id != OrderStateId.Filled)) && (order.OrderState.Id != OrderStateId.Rejected))
                        {
                            this.AddOrder(order);
                        }
                    }
                    this.account.OrderStatus += new OrderStatusEventHandler(this.Orders_OrderStatus);
                    this.ordersDataGrid.MouseDown += new MouseEventHandler(this.OrdersDataGrid_MouseDown);
                }
            }
        }

        private void Orders_OrderStatus(object sender, OrderStatusEventArgs e)
        {
            this.AddOrder(e.Order);
            DataRow row = (DataRow) this.order2DataRow[e.Order];
            if (e.Error != ErrorCode.NoError)
            {
                e.Order.Connection.ProcessEventArgs(new ITradingErrorEventArgs(e.Order.Connection, e.Error, "", e.NativeError));
            }
            if (((e.OrderState.Id == OrderStateId.Cancelled) || (e.OrderState.Id == OrderStateId.Filled)) || (e.OrderState.Id == OrderStateId.Rejected))
            {
                try
                {
                    this.ordersDataGrid.DataTable.Rows.Remove(row);
                }
                catch
                {
                }
                this.order2DataRow.Remove(e.Order);
            }
            else
            {
                this.UpdateDataRow(row, e.Order);
            }
        }

        /// <summary>
        /// Make sure the row is selected even with a right mouse click.
        /// This is to ensure proper contextMenu1_Popup functionality.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OrdersDataGrid_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point position = new Point(e.X, e.Y);
                DataGrid.HitTestInfo info = this.ordersDataGrid.HitTest(position);
                if (info.Type == DataGrid.HitTestType.Cell)
                {
                    this.ordersDataGrid.CurrentCell = new DataGridCell(info.Row, info.Column);
                }
            }
        }

        private void UpdateDataRow(DataRow dataRow, Order order)
        {
            dataRow["Order Id"] = order.OrderId;
            dataRow["Symbol"] = order.Symbol.FullName;
            dataRow["Exchange"] = order.Symbol.Exchange.Name;
            dataRow["Units"] = order.Quantity;
            dataRow["Action"] = order.Action.Name;
            dataRow["Oca Group"] = order.OcaGroup;
            dataRow["Type"] = order.OrderType.Name;
            dataRow["Tif"] = order.TimeInForce.Name;
            dataRow["Limit"] = order.Symbol.FormatPrice(order.LimitPrice);
            dataRow["Stop"] = order.Symbol.FormatPrice(order.StopPrice);
            dataRow["Status"] = order.OrderState.Name;
            dataRow["Filled"] = order.Filled;
            dataRow["Avg. Price"] = order.AvgFillPrice;
        }

        /// <summary>
        /// Get/set the account holding the orders. Set the account before this control is loaded.
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

