using iTrading.Core.Kernel;

namespace iTrading.Gui
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Resources;
    using System.Windows.Forms;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Displays an account an propertis liek open order, positions etc.
    /// </summary>
    public class AccountForm : Form
    {
        private Account account = null;
        private TabControl accountTabControl;
        private TabPage accountTabPage;
        private AccountValues accountValuesDataGrid;
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private Container components = null;
        private Executions executionsDataGrid;
        private TabPage executionsTabPage;
        private TabPage historyTabPage;
        private OrderEntry orderEntry;
        private OrderHistory orderHistoryDataGrid;
        private Orders ordersDataGrid;
        private TabPage orderTabPage;
        private Positions positionsDataGrid;
        private TabPage positionsTabPage;

        /// <summary>
        /// 
        /// </summary>
        public AccountForm()
        {
            this.InitializeComponent();
        }

        private void AccountForm_Load(object sender, EventArgs e)
        {
            this.Text = "Account '" + this.account.Name + "'";
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
        /// Erforderliche Methode f|r die Designerunterst|tzung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor gedndert werden.
        /// </summary>
        private void InitializeComponent()
        {
            ResourceManager manager = new ResourceManager(typeof(AccountForm));
            this.orderEntry = new OrderEntry();
            this.accountTabControl = new TabControl();
            this.orderTabPage = new TabPage();
            this.ordersDataGrid = new Orders();
            this.historyTabPage = new TabPage();
            this.orderHistoryDataGrid = new OrderHistory();
            this.executionsTabPage = new TabPage();
            this.executionsDataGrid = new Executions();
            this.positionsTabPage = new TabPage();
            this.positionsDataGrid = new Positions();
            this.accountTabPage = new TabPage();
            this.accountValuesDataGrid = new AccountValues();
            this.accountTabControl.SuspendLayout();
            this.orderTabPage.SuspendLayout();
            this.historyTabPage.SuspendLayout();
            this.executionsTabPage.SuspendLayout();
            this.positionsTabPage.SuspendLayout();
            this.accountTabPage.SuspendLayout();
            base.SuspendLayout();
            this.orderEntry.Account = null;
            this.orderEntry.Dock = DockStyle.Top;
            this.orderEntry.Location = new Point(0, 0);
            this.orderEntry.Name = "orderEntry";
            this.orderEntry.Size = new Size(0x3d0, 0x66);
            this.orderEntry.TabIndex = 0;
            this.accountTabControl.Controls.Add(this.orderTabPage);
            this.accountTabControl.Controls.Add(this.historyTabPage);
            this.accountTabControl.Controls.Add(this.executionsTabPage);
            this.accountTabControl.Controls.Add(this.positionsTabPage);
            this.accountTabControl.Controls.Add(this.accountTabPage);
            this.accountTabControl.Dock = DockStyle.Fill;
            this.accountTabControl.Location = new Point(0, 0x66);
            this.accountTabControl.Name = "accountTabControl";
            this.accountTabControl.Padding = new Point(0, 0);
            this.accountTabControl.SelectedIndex = 0;
            this.accountTabControl.Size = new Size(0x3d0, 0x1c0);
            this.accountTabControl.TabIndex = 1;
            this.orderTabPage.Controls.Add(this.ordersDataGrid);
            this.orderTabPage.Location = new Point(4, 0x19);
            this.orderTabPage.Name = "orderTabPage";
            this.orderTabPage.Size = new Size(0x3c8, 0x1a3);
            this.orderTabPage.TabIndex = 0;
            this.orderTabPage.Text = "Orders";
            this.ordersDataGrid.Account = null;
            this.ordersDataGrid.AutoScroll = true;
            this.ordersDataGrid.Dock = DockStyle.Fill;
            this.ordersDataGrid.Location = new Point(0, 0);
            this.ordersDataGrid.Name = "ordersDataGrid";
            this.ordersDataGrid.Size = new Size(0x3c8, 0x1a3);
            this.ordersDataGrid.TabIndex = 0;
            this.historyTabPage.Controls.Add(this.orderHistoryDataGrid);
            this.historyTabPage.Location = new Point(4, 0x19);
            this.historyTabPage.Name = "historyTabPage";
            this.historyTabPage.Size = new Size(0x3c8, 0x1a3);
            this.historyTabPage.TabIndex = 4;
            this.historyTabPage.Text = "History";
            this.orderHistoryDataGrid.Account = null;
            this.orderHistoryDataGrid.Dock = DockStyle.Fill;
            this.orderHistoryDataGrid.Location = new Point(0, 0);
            this.orderHistoryDataGrid.Name = "orderHistoryDataGrid";
            this.orderHistoryDataGrid.Size = new Size(0x3c8, 0x1a3);
            this.orderHistoryDataGrid.TabIndex = 0;
            this.executionsTabPage.Controls.Add(this.executionsDataGrid);
            this.executionsTabPage.Location = new Point(4, 0x19);
            this.executionsTabPage.Name = "executionsTabPage";
            this.executionsTabPage.Size = new Size(0x3c8, 0x1a3);
            this.executionsTabPage.TabIndex = 2;
            this.executionsTabPage.Text = "Executions";
            this.executionsDataGrid.Account = null;
            this.executionsDataGrid.AutoScroll = true;
            this.executionsDataGrid.Dock = DockStyle.Fill;
            this.executionsDataGrid.Location = new Point(0, 0);
            this.executionsDataGrid.Name = "executionsDataGrid";
            this.executionsDataGrid.Size = new Size(0x3c8, 0x1a3);
            this.executionsDataGrid.TabIndex = 0;
            this.positionsTabPage.Controls.Add(this.positionsDataGrid);
            this.positionsTabPage.Location = new Point(4, 0x19);
            this.positionsTabPage.Name = "positionsTabPage";
            this.positionsTabPage.Size = new Size(0x3c8, 0x1a3);
            this.positionsTabPage.TabIndex = 1;
            this.positionsTabPage.Text = "Positions";
            this.positionsDataGrid.Account = null;
            this.positionsDataGrid.AutoScroll = true;
            this.positionsDataGrid.Dock = DockStyle.Fill;
            this.positionsDataGrid.Location = new Point(0, 0);
            this.positionsDataGrid.Name = "positionsDataGrid";
            this.positionsDataGrid.Size = new Size(0x3c8, 0x1a3);
            this.positionsDataGrid.TabIndex = 0;
            this.accountTabPage.Controls.Add(this.accountValuesDataGrid);
            this.accountTabPage.Location = new Point(4, 0x19);
            this.accountTabPage.Name = "accountTabPage";
            this.accountTabPage.Size = new Size(0x3c8, 0x1a3);
            this.accountTabPage.TabIndex = 3;
            this.accountTabPage.Text = "Account";
            this.accountValuesDataGrid.Account = null;
            this.accountValuesDataGrid.AutoScroll = true;
            this.accountValuesDataGrid.Dock = DockStyle.Fill;
            this.accountValuesDataGrid.Location = new Point(0, 0);
            this.accountValuesDataGrid.Name = "accountValuesDataGrid";
            this.accountValuesDataGrid.Size = new Size(0x3c8, 0x1a3);
            this.accountValuesDataGrid.TabIndex = 0;
            this.AutoScaleBaseSize = new Size(6, 15);
            base.ClientSize = new Size(0x3d0, 550);
            base.Controls.Add(this.accountTabControl);
            base.Controls.Add(this.orderEntry);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.MinimumSize = new Size(0x3d0, 0x1f8);
            base.Name = "AccountForm";
            this.Text = "Account";
            base.Load += new EventHandler(this.AccountForm_Load);
            this.accountTabControl.ResumeLayout(false);
            this.orderTabPage.ResumeLayout(false);
            this.historyTabPage.ResumeLayout(false);
            this.executionsTabPage.ResumeLayout(false);
            this.positionsTabPage.ResumeLayout(false);
            this.accountTabPage.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        /// <summary>
        /// Get/set the account where the order should be placed. Set the account before this form is loaded.
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
                this.accountValuesDataGrid.Account = this.account;
                this.executionsDataGrid.Account = this.account;
                this.orderHistoryDataGrid.Account = this.account;
                this.ordersDataGrid.Account = this.account;
                this.orderEntry.Account = this.account;
                this.positionsDataGrid.Account = this.account;
            }
        }
    }
}

