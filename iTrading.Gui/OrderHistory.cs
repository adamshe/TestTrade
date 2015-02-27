using iTrading.Core.Kernel;

namespace iTrading.Gui
{
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Globalization;
    using System.Windows.Forms;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Control for displaying order history.
    /// </summary>
    public class OrderHistory : UserControl
    {
        private Account account = null;
        private const string columnAction = "Action";
        private const string columnAvgFillPrice = "Fill Price";
        private const string columnExchange = "Exchange";
        private const string columnFilled = "Filled";
        private const string columnLimitPrice = "Limit";
        private const string columnMessage = "Message";
        private const string columnOrderId = "Id";
        private const string columnOrderState = "State";
        private const string columnQuantity = "Qty";
        private const string columnStopPrice = "Stop";
        private iTrading.Gui.ColumnStyle[] columnStyles = new iTrading.Gui.ColumnStyle[] { new iTrading.Gui.ColumnStyle("Time", typeof(string), 1, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("Id", typeof(string), 2, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("Symbol", typeof(string), 2, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("Action", typeof(string), 1, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("Qty", typeof(string), 1, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("State", typeof(string), 2, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("Limit", typeof(string), 1, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("Stop", typeof(string), 1, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("Filled", typeof(string), 1, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("Fill Price", typeof(string), 1, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("Exchange", typeof(string), 2, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("Message", typeof(string), 1, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("Token", typeof(string), 3, true, false, HorizontalAlignment.Center) };
        private const string columnSymbol = "Symbol";
        private const string columnTime = "Time";
        private const string columnToken = "Token";
        private IContainer components;
        private int currentRow;
        private TMDataGrid orderHistoryDataGrid;
        private ToolTip toolTip;

        /// <summary>
        /// 
        /// </summary>
        public OrderHistory()
        {
            this.InitializeComponent();
            base.Disposed += new EventHandler(this.OrderHistory_Disposed);
        }

        private void AddOrderHistory(OrderStatusEventArgs e)
        {
            if (e.Order.Account == this.account)
            {
                DataRow row = this.orderHistoryDataGrid.DataTable.NewRow();
                this.orderHistoryDataGrid.DataTable.Rows.Add(row);
                row["Action"] = e.Order.Action.Name;
                row["Fill Price"] = e.Order.Symbol.FormatPrice(e.AvgFillPrice);
                row["Exchange"] = e.Order.Symbol.Exchange.Name;
                row["Filled"] = e.Filled;
                row["Limit"] = e.Order.Symbol.FormatPrice(e.LimitPrice);
                row["Message"] = (e.NativeError.Length == 0) ? "" : string.Concat(new object[] { e.NativeError, " (", e.Error, ")" });
                row["Id"] = e.OrderId;
                row["State"] = e.OrderState.Name;
                row["Qty"] = e.Quantity;
                row["Stop"] = e.Order.Symbol.FormatPrice(e.StopPrice);
                row["Symbol"] = e.Order.Symbol.FullName;
                row["Time"] = e.Time.ToString(CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern);
                lock (e.Order.History)
                {
                    int num = 0;
                    while (true)
                    {
                        if (num == e.Order.History.Count)
                        {
                            row["Token"] = e.Order.Token;
                            break;
                        }
                        if (e.Order.History[num] == e)
                        {
                            row["Token"] = e.Order.Token + "/" + num;
                            break;
                        }
                        num++;
                    }
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
            this.components = new Container();
            this.orderHistoryDataGrid = new TMDataGrid();
            this.toolTip = new ToolTip(this.components);
            this.orderHistoryDataGrid.BeginInit();
            base.SuspendLayout();
            this.orderHistoryDataGrid.CaptionVisible = false;
            this.orderHistoryDataGrid.ColumnStyles = null;
            this.orderHistoryDataGrid.DataMember = "";
            this.orderHistoryDataGrid.Dock = DockStyle.Fill;
            this.orderHistoryDataGrid.HeaderForeColor = SystemColors.ControlText;
            this.orderHistoryDataGrid.Location = new Point(0, 0);
            this.orderHistoryDataGrid.Name = "orderHistoryDataGrid";
            this.orderHistoryDataGrid.RowHeadersVisible = false;
            this.orderHistoryDataGrid.Size = new Size(0x250, 0x158);
            this.orderHistoryDataGrid.TabIndex = 0;
            this.toolTip.SetToolTip(this.orderHistoryDataGrid, "TEST");
            this.toolTip.AutoPopDelay = 0x1388;
            this.toolTip.InitialDelay = 500;
            this.toolTip.ReshowDelay = 100;
            this.toolTip.ShowAlways = true;
            base.Controls.Add(this.orderHistoryDataGrid);
            base.Name = "OrderHistory";
            base.Size = new Size(0x250, 0x158);
            base.Load += new EventHandler(this.OrderHistory_Load);
            this.orderHistoryDataGrid.EndInit();
            base.ResumeLayout(false);
        }

        private void OrderHistory_Disposed(object Sender, EventArgs Args)
        {
            this.account.OrderStatus -= new OrderStatusEventHandler(this.OrderStatusEventArgs);
        }

        private void OrderHistory_Load(object sender, EventArgs e)
        {
            if (Application.ExecutablePath.ToLower().IndexOf("devenv.exe") <= -1)
            {
                this.orderHistoryDataGrid.RowHeadersVisible = false;
                this.orderHistoryDataGrid.ColumnStyles = this.columnStyles;
                this.orderHistoryDataGrid.DataView.AllowNew = false;
                this.orderHistoryDataGrid.DataView.Sort = "Token ASC";
                if (this.account == null)
                {
                    throw new TMException(ErrorCode.GuiNotInitialized, "OrderHistory.Account property is NULL. Control is not initialized properly.");
                }
                if (this.Account.Connection.ConnectionStatusId == ConnectionStatusId.Connected)
                {
                    this.orderHistoryDataGrid.DataTable.BeginInit();
                    lock (this.Account.Orders)
                    {
                        foreach (Order order in this.Account.Orders)
                        {
                            lock (order.History)
                            {
                                foreach (OrderStatusEventArgs args in order.History)
                                {
                                    this.AddOrderHistory(args);
                                }
                                continue;
                            }
                        }
                    }
                    this.orderHistoryDataGrid.DataTable.EndInit();
                    this.account.OrderStatus += new OrderStatusEventHandler(this.OrderStatusEventArgs);
                    this.orderHistoryDataGrid.RowDeleting += new RowDeletingEventHandler(this.RowDeleting);
                    this.orderHistoryDataGrid.MouseMove += new MouseEventHandler(this.orderHistoryDataGrid_MouseMove);
                }
            }
        }

        private void orderHistoryDataGrid_MouseMove(object sender, MouseEventArgs e)
        {
            DataGrid.HitTestInfo info = this.orderHistoryDataGrid.HitTest(e.X, e.Y);
            if (info.Type == DataGrid.HitTestType.Cell)
            {
                if (info.Row != this.currentRow)
                {
                    DataRow row = this.orderHistoryDataGrid.DataTable.Rows[info.Row];
                    this.toolTip.SetToolTip(this.orderHistoryDataGrid, (string) row["Message"]);
                    this.currentRow = info.Row;
                }
            }
            else
            {
                this.toolTip.SetToolTip(this.orderHistoryDataGrid, "");
                this.currentRow = -1;
            }
        }

        private void OrderStatusEventArgs(object sender, OrderStatusEventArgs e)
        {
            this.AddOrderHistory(e);
        }

        private void RowDeleting(object sender, RowDeletingEventArgs e)
        {
            e.Cancel = true;
        }

        /// <summary>
        /// Get/set the account holding the order histories. Set the account before this control is loaded.
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

