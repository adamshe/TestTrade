namespace iTrading.Mbt
{
    using AxMBTGRIDSLib;
    using AxMBTOELib;
    using AxMBTQQLib;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Resources;
    using System.Windows.Forms;

    public class NavigatorForm : Form
    {
        private TabPage alertsTabPage;
        private TabPage balancesTabPage;
        private Container components = null;
        private TabPage level2TabPage;
        private AxMbtAlertsCtrl mbtAlerts;
        private AxMbtBalances mbtBalances;
        private AxMbtL2 mbtLevel2;
        private AxMbtOpenOrdersCtrl mbtOpenOrdersCtrl;
        private AxMbtOrderBook mbtOrderBook;
        private AxMbtPositionsCtrl mbtPositionsCtrl;
        private AxMbtSmOE mbtSmOE;
        private AxMbtWatch mbtWatch;
        private TabPage orderBookTabPage;
        private TabPage ordersTabPage;
        private TabPage positionsTabPage;
        private TabControl tabControl;
        private TabPage watchListTabPage;

        public NavigatorForm()
        {
            this.InitializeComponent();
            this.mbtWatch.OnManualSymbolFocusChanged += new _DMbtWatchEvents_OnManualSymbolFocusChangedEventHandler(this.mbtWatch_OnManualSymbolFocusChanged);
        }

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
            ResourceManager manager = new ResourceManager(typeof(NavigatorForm));
            this.tabControl = new TabControl();
            this.watchListTabPage = new TabPage();
            this.mbtSmOE = new AxMbtSmOE();
            this.mbtWatch = new AxMbtWatch();
            this.level2TabPage = new TabPage();
            this.mbtLevel2 = new AxMbtL2();
            this.ordersTabPage = new TabPage();
            this.mbtOpenOrdersCtrl = new AxMbtOpenOrdersCtrl();
            this.orderBookTabPage = new TabPage();
            this.mbtOrderBook = new AxMbtOrderBook();
            this.positionsTabPage = new TabPage();
            this.mbtPositionsCtrl = new AxMbtPositionsCtrl();
            this.balancesTabPage = new TabPage();
            this.mbtBalances = new AxMbtBalances();
            this.alertsTabPage = new TabPage();
            this.mbtAlerts = new AxMbtAlertsCtrl();
            this.tabControl.SuspendLayout();
            this.watchListTabPage.SuspendLayout();
            this.mbtSmOE.BeginInit();
            this.mbtWatch.BeginInit();
            this.level2TabPage.SuspendLayout();
            this.mbtLevel2.BeginInit();
            this.ordersTabPage.SuspendLayout();
            this.mbtOpenOrdersCtrl.BeginInit();
            this.orderBookTabPage.SuspendLayout();
            this.mbtOrderBook.BeginInit();
            this.positionsTabPage.SuspendLayout();
            this.mbtPositionsCtrl.BeginInit();
            this.balancesTabPage.SuspendLayout();
            this.mbtBalances.BeginInit();
            this.alertsTabPage.SuspendLayout();
            this.mbtAlerts.BeginInit();
            base.SuspendLayout();
            this.tabControl.Controls.Add(this.watchListTabPage);
            this.tabControl.Controls.Add(this.level2TabPage);
            this.tabControl.Controls.Add(this.ordersTabPage);
            this.tabControl.Controls.Add(this.orderBookTabPage);
            this.tabControl.Controls.Add(this.positionsTabPage);
            this.tabControl.Controls.Add(this.balancesTabPage);
            this.tabControl.Controls.Add(this.alertsTabPage);
            this.tabControl.Dock = DockStyle.Fill;
            this.tabControl.Location = new Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new Size(0x250, 0x185);
            this.tabControl.TabIndex = 0;
            this.watchListTabPage.Controls.Add(this.mbtSmOE);
            this.watchListTabPage.Controls.Add(this.mbtWatch);
            this.watchListTabPage.Location = new Point(4, 0x16);
            this.watchListTabPage.Name = "watchListTabPage";
            this.watchListTabPage.Size = new Size(0x248, 0x16b);
            this.watchListTabPage.TabIndex = 5;
            this.watchListTabPage.Text = "WatchList";
            this.mbtSmOE.ContainingControl = this;
            this.mbtSmOE.Dock = DockStyle.Bottom;
            this.mbtSmOE.Enabled = true;
            this.mbtSmOE.Location = new Point(0, 0x107);
            this.mbtSmOE.Name = "mbtSmOE";
            this.mbtSmOE.OcxState = (AxHost.State) manager.GetObject("mbtSmOE.OcxState");
            this.mbtSmOE.Size = new Size(0x248, 100);
            this.mbtSmOE.TabIndex = 1;
            this.mbtSmOE.OnSuggestedClientSizeChanged += new _DMbtSmOEEvents_OnSuggestedClientSizeChangedEventHandler(this.mbtSmOE_OnSuggestedClientSizeChanged);
            this.mbtWatch.ContainingControl = this;
            this.mbtWatch.Dock = DockStyle.Top;
            this.mbtWatch.Enabled = true;
            this.mbtWatch.Location = new Point(0, 0);
            this.mbtWatch.Name = "mbtWatch";
            this.mbtWatch.OcxState = (AxHost.State) manager.GetObject("mbtWatch.OcxState");
            this.mbtWatch.Size = new Size(0x248, 0x100);
            this.mbtWatch.TabIndex = 0;
            this.level2TabPage.Controls.Add(this.mbtLevel2);
            this.level2TabPage.Location = new Point(4, 0x16);
            this.level2TabPage.Name = "level2TabPage";
            this.level2TabPage.Size = new Size(0x248, 0x16b);
            this.level2TabPage.TabIndex = 0;
            this.level2TabPage.Text = "Level 2";
            this.mbtLevel2.ContainingControl = this;
            this.mbtLevel2.Dock = DockStyle.Fill;
            this.mbtLevel2.Enabled = true;
            this.mbtLevel2.Location = new Point(0, 0);
            this.mbtLevel2.Name = "mbtLevel2";
            this.mbtLevel2.OcxState = (AxHost.State) manager.GetObject("mbtLevel2.OcxState");
            this.mbtLevel2.Size = new Size(0x248, 0x16b);
            this.mbtLevel2.TabIndex = 0;
            this.ordersTabPage.Controls.Add(this.mbtOpenOrdersCtrl);
            this.ordersTabPage.Location = new Point(4, 0x16);
            this.ordersTabPage.Name = "ordersTabPage";
            this.ordersTabPage.Size = new Size(0x248, 0x16b);
            this.ordersTabPage.TabIndex = 3;
            this.ordersTabPage.Text = "Orders";
            this.mbtOpenOrdersCtrl.ContainingControl = this;
            this.mbtOpenOrdersCtrl.Dock = DockStyle.Fill;
            this.mbtOpenOrdersCtrl.Enabled = true;
            this.mbtOpenOrdersCtrl.Location = new Point(0, 0);
            this.mbtOpenOrdersCtrl.Name = "mbtOpenOrdersCtrl";
            this.mbtOpenOrdersCtrl.OcxState = (AxHost.State) manager.GetObject("mbtOpenOrdersCtrl.OcxState");
            this.mbtOpenOrdersCtrl.Size = new Size(0x248, 0x16b);
            this.mbtOpenOrdersCtrl.TabIndex = 0;
            this.orderBookTabPage.Controls.Add(this.mbtOrderBook);
            this.orderBookTabPage.Location = new Point(4, 0x16);
            this.orderBookTabPage.Name = "orderBookTabPage";
            this.orderBookTabPage.Size = new Size(0x248, 0x16b);
            this.orderBookTabPage.TabIndex = 6;
            this.orderBookTabPage.Text = "OrderBook";
            this.mbtOrderBook.ContainingControl = this;
            this.mbtOrderBook.Dock = DockStyle.Fill;
            this.mbtOrderBook.Enabled = true;
            this.mbtOrderBook.Location = new Point(0, 0);
            this.mbtOrderBook.Name = "mbtOrderBook";
            this.mbtOrderBook.OcxState = (AxHost.State) manager.GetObject("mbtOrderBook.OcxState");
            this.mbtOrderBook.Size = new Size(0x248, 0x16b);
            this.mbtOrderBook.TabIndex = 0;
            this.positionsTabPage.Controls.Add(this.mbtPositionsCtrl);
            this.positionsTabPage.Location = new Point(4, 0x16);
            this.positionsTabPage.Name = "positionsTabPage";
            this.positionsTabPage.Size = new Size(0x248, 0x16b);
            this.positionsTabPage.TabIndex = 4;
            this.positionsTabPage.Text = "Positions";
            this.mbtPositionsCtrl.ContainingControl = this;
            this.mbtPositionsCtrl.Dock = DockStyle.Fill;
            this.mbtPositionsCtrl.Enabled = true;
            this.mbtPositionsCtrl.Location = new Point(0, 0);
            this.mbtPositionsCtrl.Name = "mbtPositionsCtrl";
            this.mbtPositionsCtrl.OcxState = (AxHost.State) manager.GetObject("mbtPositionsCtrl.OcxState");
            this.mbtPositionsCtrl.Size = new Size(0x248, 0x16b);
            this.mbtPositionsCtrl.TabIndex = 0;
            this.balancesTabPage.Controls.Add(this.mbtBalances);
            this.balancesTabPage.Location = new Point(4, 0x16);
            this.balancesTabPage.Name = "balancesTabPage";
            this.balancesTabPage.Size = new Size(0x248, 0x16b);
            this.balancesTabPage.TabIndex = 2;
            this.balancesTabPage.Text = "Balances";
            this.mbtBalances.ContainingControl = this;
            this.mbtBalances.Dock = DockStyle.Fill;
            this.mbtBalances.Enabled = true;
            this.mbtBalances.Location = new Point(0, 0);
            this.mbtBalances.Name = "mbtBalances";
            this.mbtBalances.OcxState = (AxHost.State) manager.GetObject("mbtBalances.OcxState");
            this.mbtBalances.Size = new Size(0x248, 0x16b);
            this.mbtBalances.TabIndex = 0;
            this.alertsTabPage.Controls.Add(this.mbtAlerts);
            this.alertsTabPage.Location = new Point(4, 0x16);
            this.alertsTabPage.Name = "alertsTabPage";
            this.alertsTabPage.Size = new Size(0x248, 0x16b);
            this.alertsTabPage.TabIndex = 1;
            this.alertsTabPage.Text = "Alerts";
            this.mbtAlerts.ContainingControl = this;
            this.mbtAlerts.Dock = DockStyle.Fill;
            this.mbtAlerts.Enabled = true;
            this.mbtAlerts.Location = new Point(0, 0);
            this.mbtAlerts.Name = "mbtAlerts";
            this.mbtAlerts.OcxState = (AxHost.State) manager.GetObject("mbtAlerts.OcxState");
            this.mbtAlerts.Size = new Size(0x248, 0x16b);
            this.mbtAlerts.TabIndex = 0;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x250, 0x185);
            base.Controls.Add(this.tabControl);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.MinimumSize = new Size(600, 0x1a0);
            base.Name = "NavigatorForm";
            this.Text = "MB Trading Navigator";
            base.Closing += new CancelEventHandler(this.NavigatorForm_Closing);
            base.SizeChanged += new EventHandler(this.NavigatorForm_SizeChanged);
            base.Load += new EventHandler(this.NavigatorForm_Load);
            this.tabControl.ResumeLayout(false);
            this.watchListTabPage.ResumeLayout(false);
            this.mbtSmOE.EndInit();
            this.mbtWatch.EndInit();
            this.level2TabPage.ResumeLayout(false);
            this.mbtLevel2.EndInit();
            this.ordersTabPage.ResumeLayout(false);
            this.mbtOpenOrdersCtrl.EndInit();
            this.orderBookTabPage.ResumeLayout(false);
            this.mbtOrderBook.EndInit();
            this.positionsTabPage.ResumeLayout(false);
            this.mbtPositionsCtrl.EndInit();
            this.balancesTabPage.ResumeLayout(false);
            this.mbtBalances.EndInit();
            this.alertsTabPage.ResumeLayout(false);
            this.mbtAlerts.EndInit();
            base.ResumeLayout(false);
        }

        private void mbtSmOE_OnSuggestedClientSizeChanged(object sender, _DMbtSmOEEvents_OnSuggestedClientSizeChangedEvent e)
        {
            this.mbtSmOE.Height = e.lNewHeight;
            this.mbtSmOE.Width = e.lNewWidth;
            this.mbtSmOE.Location = new Point(0, this.watchListTabPage.Height - e.lNewHeight);
        }

        private void mbtWatch_OnManualSymbolFocusChanged(object sender, _DMbtWatchEvents_OnManualSymbolFocusChangedEvent e)
        {
            this.mbtSmOE.Symbol = e.symbol;
        }

        private void NavigatorForm_Closing(object sender, CancelEventArgs e)
        {
            this.SaveSettings();
        }

        private void NavigatorForm_Load(object sender, EventArgs e)
        {
            if (Adapter.currentAdapter == null)
            {
                MessageBox.Show("Unable to start MBT Navigator. Please connect to your MB trading account first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else
            {
                string[] strArray = Adapter.currentAdapter.prefs.GetWatchlistNames().Split(new char[] { ',' });
                int index = 0;
                while (index < strArray.Length)
                {
                    string str = strArray[index];
                    this.mbtWatch.Watchlist = str;
                    break;
                }
                this.mbtBalances.Account = Adapter.currentAdapter.orderClient.Accounts[0];
                this.mbtOpenOrdersCtrl.Account = Adapter.currentAdapter.orderClient.Accounts[0];
                this.mbtOrderBook.Account = Adapter.currentAdapter.orderClient.Accounts[0];
                this.mbtPositionsCtrl.Account = Adapter.currentAdapter.orderClient.Accounts[0];
                this.mbtSmOE.Account = Adapter.currentAdapter.orderClient.Accounts[0];
                this.mbtWatch.Account = Adapter.currentAdapter.orderClient.Accounts[0];
                this.Rearrange();
            }
        }

        private void NavigatorForm_SizeChanged(object sender, EventArgs e)
        {
            this.Rearrange();
        }

        private void Rearrange()
        {
            this.mbtWatch.Size = new Size(base.Width, base.Height - 100);
            this.mbtSmOE.Size = new Size(base.Width, 100);
            this.mbtSmOE.Location = new Point(0, base.Height - 100);
        }

        public void SaveSettings()
        {
            if (Adapter.currentAdapter.prefs != null)
            {
                Adapter.currentAdapter.prefs.Serialize(true, true);
            }
        }
    }
}

