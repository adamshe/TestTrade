using iTrading.Core.Kernel;

namespace iTrading.Gui
{
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Windows.Forms;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Control for displaying executions.
    /// </summary>
    public class Executions : UserControl
    {
        private Account account = null;
        private const string columnExchange = "Exchange";
        private const string columnId = "Execution id";
        private const string columnMarketPosition = "Market pos.";
        private const string columnOrderId = "Order id";
        private const string columnPrice = "Avg. price";
        private const string columnSize = "#";
        private iTrading.Gui.ColumnStyle[] columnStyles = new iTrading.Gui.ColumnStyle[] { new iTrading.Gui.ColumnStyle("Execution id", typeof(string), 2, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("Symbol", typeof(string), 2, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("Exchange", typeof(string), 1, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("Order id", typeof(string), 2, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("Market pos.", typeof(string), 1, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("#", typeof(int), 1, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("Avg. price", typeof(string), 1, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("Time", typeof(string), 2, true, false, HorizontalAlignment.Center) };
        private const string columnSymbol = "Symbol";
        private const string columnTime = "Time";
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private Container components = null;
        private TMDataGrid executionsDataGrid;

        /// <summary>
        /// 
        /// </summary>
        public Executions()
        {
            this.InitializeComponent();
            base.Disposed += new EventHandler(this.Executions_Disposed);
        }

        private void AddExecution(Execution execution)
        {
            if (execution.Account == this.account)
            {
                DataRow row = this.executionsDataGrid.DataTable.NewRow();
                this.executionsDataGrid.DataTable.Rows.Add(row);
                this.UpdateDataRow(row, execution);
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

        private void Executions_Disposed(object Sender, EventArgs Args)
        {
            this.account.Execution -= new ExecutionUpdateEventHandler(this.Executions_Execution);
        }

        private void Executions_Execution(object sender, ExecutionUpdateEventArgs e)
        {
            if (e.Operation == Operation.Insert)
            {
                this.AddExecution(e.Execution);
            }
            else
            {
                foreach (DataRow row in this.executionsDataGrid.DataTable.Rows)
                {
                    if (((string) row["Execution id"]) == e.Execution.Id)
                    {
                        this.UpdateDataRow(row, e.Execution);
                    }
                }
            }
        }

        private void Executions_Load(object sender, EventArgs e)
        {
            if (Application.ExecutablePath.ToLower().IndexOf("devenv.exe") <= -1)
            {
                this.executionsDataGrid.RowHeadersVisible = false;
                this.executionsDataGrid.ColumnStyles = this.columnStyles;
                this.executionsDataGrid.DataView.AllowNew = false;
                this.executionsDataGrid.DataView.Sort = "Time ASC";
                if (this.account == null)
                {
                    throw new TMException(ErrorCode.GuiNotInitialized, "Executions.Account property is NULL. Control is not initialized properly.");
                }
                if (this.Account.Connection.ConnectionStatusId == ConnectionStatusId.Connected)
                {
                    foreach (Execution execution in this.Account.Executions)
                    {
                        this.AddExecution(execution);
                    }
                    this.account.Execution += new ExecutionUpdateEventHandler(this.Executions_Execution);
                    this.executionsDataGrid.RowDeleting += new RowDeletingEventHandler(this.executionsDataGrid_RowDeleting);
                    this.executionsDataGrid.MouseDown += new MouseEventHandler(this.executionsDataGrid_MouseDown);
                }
            }
        }

        private void executionsDataGrid_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point position = new Point(e.X, e.Y);
                DataGrid.HitTestInfo info = this.executionsDataGrid.HitTest(position);
                if (info.Type == DataGrid.HitTestType.Cell)
                {
                    this.executionsDataGrid.CurrentCell = new DataGridCell(info.Row, info.Column);
                }
            }
        }

        private void executionsDataGrid_RowDeleting(object sender, RowDeletingEventArgs e)
        {
            e.Cancel = true;
        }

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.executionsDataGrid = new TMDataGrid();
            this.executionsDataGrid.BeginInit();
            base.SuspendLayout();
            this.executionsDataGrid.CaptionVisible = false;
            this.executionsDataGrid.ColumnStyles = null;
            this.executionsDataGrid.DataMember = "";
            this.executionsDataGrid.Dock = DockStyle.Fill;
            this.executionsDataGrid.HeaderForeColor = SystemColors.ControlText;
            this.executionsDataGrid.Location = new Point(0, 0);
            this.executionsDataGrid.Name = "executionsDataGrid";
            this.executionsDataGrid.RowHeadersVisible = false;
            this.executionsDataGrid.Size = new Size(0x250, 0x158);
            this.executionsDataGrid.TabIndex = 0;
            base.Controls.Add(this.executionsDataGrid);
            base.Name = "Executions";
            base.Size = new Size(0x250, 0x158);
            base.Load += new EventHandler(this.Executions_Load);
            this.executionsDataGrid.EndInit();
            base.ResumeLayout(false);
        }

        private void UpdateDataRow(DataRow dataRow, Execution execution)
        {
            dataRow["Exchange"] = execution.Symbol.Exchange.Name;
            dataRow["Execution id"] = execution.Id;
            dataRow["Order id"] = execution.OrderId;
            dataRow["Market pos."] = execution.MarketPosition.Name;
            dataRow["Avg. price"] = execution.Symbol.FormatPrice(execution.AvgPrice);
            dataRow["#"] = execution.Quantity;
            dataRow["Symbol"] = execution.Symbol.FullName;
            dataRow["Time"] = execution.Time.ToString("G");
        }

        /// <summary>
        /// Get/set the account holding the executions. Set the account before this control is loaded.
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

