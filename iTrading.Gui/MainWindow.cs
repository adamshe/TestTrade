namespace iTrading.Gui
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Resources;
    using System.Windows.Forms;
    using iTrading.Core.Kernel;
    using iTrading.Mbt;

    /// <summary>
    /// TradeMagic demo application main window.
    /// </summary>
    public class MainWindow : Form
    {
        private MenuItem aboutMenuItem;
        private MenuItem accountsMenuItem;
        private bool boolFirstLogonDone = false;
        private MenuItem CascadeMenuItem;
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private Container components = null;
        private const string connected = "Connected";
        private const string connectedLost = "Connection lost";
        private const string connectedLost2 = "Secondary connection lost";
        internal static Connection Connection = new Connection();
        private MenuItem connectMenuItem;
        private MenuItem controlCenterMenuItem;
        private MenuItem dataMenuItem;
        private DataRecorderForm dataRecorderForm = null;
        private MenuItem dataRecorderMenuItem;
        private MenuItem disconnectMenuItem;
        private MenuItem exitMenuItem;
        private MenuItem exportHistoricalDataMenuItem;
        private MenuItem exportMarketDataMenuItem;
        private MenuItem exportMarketDepthMenuItem;
        private MenuItem fileMenuItem;
        private MenuItem historicalDataMenuItem;
        private MenuItem importHistoricalDataMenuItem;
        private MenuItem importMarketDataMenuItem;
        private MenuItem importMarketDepthMenuItem;
        private MainMenu mainMenu1;
        private MenuItem marketDataMenuItem;
        private MenuItem marketDepthMenuItem;
        private MenuItem mbtNavigatorMenuItem;
        private MenuItem menuItem1;
        private MenuItem menuItem2;
        private MenuItem menuItem3;
        private MenuItem menuItem4;
        private MenuItem menuItem5;
        private MenuItem menuItem6;
        private MenuItem menuItem7;
        private MenuItem newsMenuItem;
        private const string notConnected = "Not Connected";
        private MenuItem optionsMenuItem;
        private StatusBarPanel PanelConnected = new StatusBarPanel();
        private StatusBarPanel PanelFill = new StatusBarPanel();
        private StatusBarPanel PanelMode = new StatusBarPanel();
        private StatusBarPanel PanelTime = new StatusBarPanel();
        private MenuItem patsWizardMenuItem;
        private MenuItem registrationMenuItem;
        private MenuItem resetSimulationAccountMenuItem;
        private SimulationControlCenter simulationControlCenter = null;
        private MenuItem startStopMenuItem;
        private System.Windows.Forms.StatusBar StatusBar;
        private MenuItem symbolsMenuItem;
        private MenuItem TileHorizontalMenuItem;
        private MenuItem TileVerticalMenuItem;
        private TraceLogForm traceLog;
        private MenuItem traceLogMenuItem;
        private MenuItem windowMenuItem;

        /// <summary>
        /// 
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void aboutMenuItem_Click(object sender, EventArgs e)
        {
            new AboutForm().ShowDialog();
        }

        private void CascadeMenuItem_Click(object sender, EventArgs e)
        {
            base.LayoutMdi(MdiLayout.Cascade);
        }

        private void ConnectMenuItem_Click(object sender, EventArgs e)
        {
            ConnectForm form = new ConnectForm();
            form.Connection = Connection;
            form.ShowDialog();
            Connection = form.Connection;
        }

        private void controlCenterMenuItem_Click(object sender, EventArgs e)
        {
            this.ShowSimulationControlCenter();
        }

        private void DisconnectMenuItem_Click(object sender, EventArgs e)
        {
            if (Connection != null)
            {
                Connection.Close();
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

        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            if (Connection != null)
            {
                Connection.Close();
            }
            Environment.Exit(0);
        }

        private void exportHistoricalDataMenuItem_Click(object sender, EventArgs e)
        {
            ExportHistoricalDataForm form = new ExportHistoricalDataForm();
            form.Connection = Connection;
            form.MdiParent = this;
            form.WindowState = FormWindowState.Normal;
            form.Show();
        }

        private void exportMarketDataMenuItem_Click(object sender, EventArgs e)
        {
            ExportMarketDataForm form = new ExportMarketDataForm();
            form.Connection = Connection;
            form.MdiParent = this;
            form.WindowState = FormWindowState.Normal;
            form.Show();
        }

        private void exportMarketDepthMenuItem_Click(object sender, EventArgs e)
        {
            ExportMarketDepthForm form = new ExportMarketDepthForm();
            form.Connection = Connection;
            form.MdiParent = this;
            form.WindowState = FormWindowState.Normal;
            form.Show();
        }

        private void historicalDataMenuItem_Click(object sender, EventArgs e)
        {
            HistoricalDataForm form = new HistoricalDataForm();
            form.Connection = Connection;
            form.MdiParent = this;
            form.WindowState = FormWindowState.Normal;
            form.Show();
        }

        private void importHistoricalDataMenuItem_Click(object sender, EventArgs e)
        {
            ImportHistoricalDataForm form = new ImportHistoricalDataForm();
            form.Connection = Connection;
            form.MdiParent = this;
            form.WindowState = FormWindowState.Normal;
            form.Show();
        }

        private void importMarketDataMenuItem_Click(object sender, EventArgs e)
        {
            ImportMarketDataForm form = new ImportMarketDataForm();
            form.Connection = Connection;
            form.MdiParent = this;
            form.WindowState = FormWindowState.Normal;
            form.Show();
        }

        private void importMarketDepthMenuItem_Click(object sender, EventArgs e)
        {
            ImportMarketDepthForm form = new ImportMarketDepthForm();
            form.Connection = Connection;
            form.MdiParent = this;
            form.WindowState = FormWindowState.Normal;
            form.Show();
        }

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            ResourceManager manager = new ResourceManager(typeof(MainWindow));
            this.mainMenu1 = new MainMenu();
            this.fileMenuItem = new MenuItem();
            this.connectMenuItem = new MenuItem();
            this.disconnectMenuItem = new MenuItem();
            this.menuItem2 = new MenuItem();
            this.exitMenuItem = new MenuItem();
            this.dataMenuItem = new MenuItem();
            this.symbolsMenuItem = new MenuItem();
            this.marketDataMenuItem = new MenuItem();
            this.marketDepthMenuItem = new MenuItem();
            this.historicalDataMenuItem = new MenuItem();
            this.newsMenuItem = new MenuItem();
            this.accountsMenuItem = new MenuItem();
            this.menuItem3 = new MenuItem();
            this.exportHistoricalDataMenuItem = new MenuItem();
            this.importHistoricalDataMenuItem = new MenuItem();
            this.menuItem7 = new MenuItem();
            this.dataRecorderMenuItem = new MenuItem();
            this.startStopMenuItem = new MenuItem();
            this.menuItem5 = new MenuItem();
            this.exportMarketDataMenuItem = new MenuItem();
            this.exportMarketDepthMenuItem = new MenuItem();
            this.menuItem6 = new MenuItem();
            this.importMarketDataMenuItem = new MenuItem();
            this.importMarketDepthMenuItem = new MenuItem();
            this.controlCenterMenuItem = new MenuItem();
            this.menuItem4 = new MenuItem();
            this.traceLogMenuItem = new MenuItem();
            this.optionsMenuItem = new MenuItem();
            this.resetSimulationAccountMenuItem = new MenuItem();
            this.patsWizardMenuItem = new MenuItem();
            this.mbtNavigatorMenuItem = new MenuItem();
            this.windowMenuItem = new MenuItem();
            this.CascadeMenuItem = new MenuItem();
            this.TileHorizontalMenuItem = new MenuItem();
            this.TileVerticalMenuItem = new MenuItem();
            this.menuItem1 = new MenuItem();
            this.registrationMenuItem = new MenuItem();
            this.aboutMenuItem = new MenuItem();
            this.StatusBar = new System.Windows.Forms.StatusBar();
            base.SuspendLayout();
            this.mainMenu1.MenuItems.AddRange(new MenuItem[] { this.fileMenuItem, this.dataMenuItem, this.accountsMenuItem, this.menuItem3, this.windowMenuItem, this.menuItem1 });
            this.fileMenuItem.Index = 0;
            this.fileMenuItem.MenuItems.AddRange(new MenuItem[] { this.connectMenuItem, this.disconnectMenuItem, this.menuItem2, this.exitMenuItem });
            this.fileMenuItem.Text = "&File";
            this.connectMenuItem.Index = 0;
            this.connectMenuItem.Text = "&Connect ...";
            this.connectMenuItem.Click += new EventHandler(this.ConnectMenuItem_Click);
            this.disconnectMenuItem.Index = 1;
            this.disconnectMenuItem.Text = "&Disconnect";
            this.disconnectMenuItem.Click += new EventHandler(this.DisconnectMenuItem_Click);
            this.menuItem2.Index = 2;
            this.menuItem2.Text = "-";
            this.exitMenuItem.Index = 3;
            this.exitMenuItem.Text = "&Exit";
            this.exitMenuItem.Click += new EventHandler(this.ExitMenuItem_Click);
            this.dataMenuItem.Index = 1;
            this.dataMenuItem.MenuItems.AddRange(new MenuItem[] { this.symbolsMenuItem, this.marketDataMenuItem, this.marketDepthMenuItem, this.historicalDataMenuItem, this.newsMenuItem });
            this.dataMenuItem.Text = "&Data";
            this.symbolsMenuItem.Index = 0;
            this.symbolsMenuItem.Text = "&Symbols";
            this.symbolsMenuItem.Click += new EventHandler(this.symbolsMenuItem_Click);
            this.marketDataMenuItem.Index = 1;
            this.marketDataMenuItem.Text = "&Market data";
            this.marketDataMenuItem.Click += new EventHandler(this.MarketDataMenuItem_Click);
            this.marketDepthMenuItem.Index = 2;
            this.marketDepthMenuItem.Text = "Market &depth";
            this.marketDepthMenuItem.Click += new EventHandler(this.MarketDepthMenuItem_Click);
            this.historicalDataMenuItem.Index = 3;
            this.historicalDataMenuItem.Text = "&Historical data";
            this.historicalDataMenuItem.Click += new EventHandler(this.historicalDataMenuItem_Click);
            this.newsMenuItem.Index = 4;
            this.newsMenuItem.Text = "&News";
            this.newsMenuItem.Click += new EventHandler(this.NewsMenuItem_Click);
            this.accountsMenuItem.Index = 2;
            this.accountsMenuItem.Text = "&Accounts";
            this.menuItem3.Index = 3;
            this.menuItem3.MenuItems.AddRange(new MenuItem[] { this.exportHistoricalDataMenuItem, this.importHistoricalDataMenuItem, this.menuItem7, this.dataRecorderMenuItem, this.controlCenterMenuItem, this.menuItem4, this.traceLogMenuItem, this.optionsMenuItem, this.resetSimulationAccountMenuItem, this.patsWizardMenuItem, this.mbtNavigatorMenuItem });
            this.menuItem3.Text = "E&xtras";
            this.exportHistoricalDataMenuItem.Index = 0;
            this.exportHistoricalDataMenuItem.Text = "&Export historical data";
            this.exportHistoricalDataMenuItem.Click += new EventHandler(this.exportHistoricalDataMenuItem_Click);
            this.importHistoricalDataMenuItem.Index = 1;
            this.importHistoricalDataMenuItem.Text = "&Import historical data";
            this.importHistoricalDataMenuItem.Click += new EventHandler(this.importHistoricalDataMenuItem_Click);
            this.menuItem7.Index = 2;
            this.menuItem7.Text = "-";
            this.dataRecorderMenuItem.Enabled = false;
            this.dataRecorderMenuItem.Index = 3;
            this.dataRecorderMenuItem.MenuItems.AddRange(new MenuItem[] { this.startStopMenuItem, this.menuItem5, this.exportMarketDataMenuItem, this.exportMarketDepthMenuItem, this.menuItem6, this.importMarketDataMenuItem, this.importMarketDepthMenuItem });
            this.dataRecorderMenuItem.Text = "&Data recorder";
            this.startStopMenuItem.Index = 0;
            this.startStopMenuItem.Text = "&Start / Stop";
            this.startStopMenuItem.Click += new EventHandler(this.startStopMenuItem_Click);
            this.menuItem5.Index = 1;
            this.menuItem5.Text = "-";
            this.exportMarketDataMenuItem.Index = 2;
            this.exportMarketDataMenuItem.Text = "&Export market data";
            this.exportMarketDataMenuItem.Click += new EventHandler(this.exportMarketDataMenuItem_Click);
            this.exportMarketDepthMenuItem.Index = 3;
            this.exportMarketDepthMenuItem.Text = "E&xport market depth data";
            this.exportMarketDepthMenuItem.Click += new EventHandler(this.exportMarketDepthMenuItem_Click);
            this.menuItem6.Index = 4;
            this.menuItem6.Text = "-";
            this.importMarketDataMenuItem.Index = 5;
            this.importMarketDataMenuItem.Text = "&Import market data";
            this.importMarketDataMenuItem.Click += new EventHandler(this.importMarketDataMenuItem_Click);
            this.importMarketDepthMenuItem.Index = 6;
            this.importMarketDepthMenuItem.Text = "I&mport market depth data";
            this.importMarketDepthMenuItem.Click += new EventHandler(this.importMarketDepthMenuItem_Click);
            this.controlCenterMenuItem.Enabled = false;
            this.controlCenterMenuItem.Index = 4;
            this.controlCenterMenuItem.Text = "&Simulation control center";
            this.controlCenterMenuItem.Click += new EventHandler(this.controlCenterMenuItem_Click);
            this.menuItem4.Index = 5;
            this.menuItem4.Text = "-";
            this.traceLogMenuItem.Index = 6;
            this.traceLogMenuItem.Text = "&Trace log";
            this.traceLogMenuItem.Click += new EventHandler(this.traceLogMenuItem_Click);
            this.optionsMenuItem.Index = 7;
            this.optionsMenuItem.Text = "&Options ...";
            this.optionsMenuItem.Click += new EventHandler(this.optionsMenuItem_Click);
            this.resetSimulationAccountMenuItem.Index = 8;
            this.resetSimulationAccountMenuItem.Text = "&Reset simulation account ...";
            this.resetSimulationAccountMenuItem.Click += new EventHandler(this.resetSimulationAccountMenuItem_Click);
            this.patsWizardMenuItem.Index = 9;
            this.patsWizardMenuItem.Text = "&Patsystems wizard";
            this.patsWizardMenuItem.Click += new EventHandler(this.patsWizardMenuItem_Click);
            this.mbtNavigatorMenuItem.Index = 10;
            this.mbtNavigatorMenuItem.Text = "&MBT Navigator";
            this.mbtNavigatorMenuItem.Click += new EventHandler(this.mbtNavigatorMenuItem_Click);
            this.windowMenuItem.Index = 4;
            this.windowMenuItem.MdiList = true;
            this.windowMenuItem.MenuItems.AddRange(new MenuItem[] { this.CascadeMenuItem, this.TileHorizontalMenuItem, this.TileVerticalMenuItem });
            this.windowMenuItem.Text = "&Window";
            this.CascadeMenuItem.Index = 0;
            this.CascadeMenuItem.Text = "&Cascade";
            this.CascadeMenuItem.Click += new EventHandler(this.CascadeMenuItem_Click);
            this.TileHorizontalMenuItem.Index = 1;
            this.TileHorizontalMenuItem.Text = "Tile &horizontal";
            this.TileHorizontalMenuItem.Click += new EventHandler(this.TileHorizontalMenuItem_Click);
            this.TileVerticalMenuItem.Index = 2;
            this.TileVerticalMenuItem.Text = "Tile &vertical";
            this.TileVerticalMenuItem.Click += new EventHandler(this.TileVerticalMenuItem_Click);
            this.menuItem1.Index = 5;
            this.menuItem1.MenuItems.AddRange(new MenuItem[] { this.registrationMenuItem, this.aboutMenuItem });
            this.menuItem1.Text = "&Help";
            this.registrationMenuItem.Index = 0;
            this.registrationMenuItem.Text = "Registration ...";
            this.registrationMenuItem.Click += new EventHandler(this.registrationMenuItem_Click);
            this.aboutMenuItem.Index = 1;
            this.aboutMenuItem.Text = "&About ...";
            this.aboutMenuItem.Click += new EventHandler(this.aboutMenuItem_Click);
            this.StatusBar.Location = new Point(0, 0x259);
            this.StatusBar.Name = "StatusBar";
            this.StatusBar.ShowPanels = true;
            this.StatusBar.Size = new Size(0x403, 0x1a);
            this.StatusBar.SizingGrip = false;
            this.StatusBar.TabIndex = 1;
            this.AutoScaleBaseSize = new Size(6, 15);
            base.ClientSize = new Size(0x403, 0x273);
            base.Controls.Add(this.StatusBar);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.IsMdiContainer = true;
            base.Menu = this.mainMenu1;
            base.Name = "MainWindow";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "TradeMagic";
            base.WindowState = FormWindowState.Maximized;
            base.Load += new EventHandler(this.MainWindow_Load);
            base.Closed += new EventHandler(this.MainWindow_Closed);
            base.ResumeLayout(false);
        }

        private void MainWindow_Account(object sender, AccountEventArgs e)
        {
            int num = this.accountsMenuItem.MenuItems.Count + 1;
            MenuItem item = new MenuItem("&" + num.ToString() + " " + e.Account.Name);
            item.Index = this.accountsMenuItem.MenuItems.Count;
            item.Click += new EventHandler(this.MainWindow_accountClick);
            this.accountsMenuItem.MenuItems.Add(this.accountsMenuItem.MenuItems.Count, item);
        }

        private void MainWindow_accountClick(object sender, EventArgs e)
        {
            foreach (Form form in base.MdiChildren)
            {
                if ((form is AccountForm) && (((AccountForm) form).Account.Name == Connection.Accounts[((MenuItem) sender).Index].Name))
                {
                    form.Focus();
                    return;
                }
            }
            AccountForm form2 = new AccountForm();
            form2.Account = Connection.Accounts[((MenuItem) sender).Index];
            form2.MdiParent = this;
            form2.WindowState = FormWindowState.Normal;
            form2.Show();
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            this.traceLog.Dispose();
            if (Connection.Options is PatsOptions)
            {
                ((PatsOptions) Connection.Options).LogoffOnConnectionClose = true;
            }
            Connection.Close();
        }

        private void MainWindow_ConnectionStatus(object sender, ConnectionStatusEventArgs e)
        {
            base.Activate();
            if (e.Connection.ConnectionStatusId == ConnectionStatusId.Connected)
            {
                this.PanelConnected.Text = "Connected";
                if (e.OldConnectionStatusId != ConnectionStatusId.Connecting)
                {
                    if (e.SecondaryConnectionStatusId == ConnectionStatusId.ConnectionLost)
                    {
                        this.PanelConnected.Text = "Secondary connection lost";
                    }
                }
                else
                {
                    this.accountsMenuItem.Enabled = true;
                    this.connectMenuItem.Enabled = false;
                    this.dataMenuItem.Enabled = true;
                    this.dataRecorderMenuItem.Enabled = true;
                    this.disconnectMenuItem.Enabled = true;
                    this.marketDepthMenuItem.Enabled = e.Connection.FeatureTypes[FeatureTypeId.MarketDepth] != null;
                    this.newsMenuItem.Enabled = e.Connection.FeatureTypes[FeatureTypeId.News] != null;
                    this.mbtNavigatorMenuItem.Enabled = Connection.Options.Provider.Id == ProviderTypeId.MBTrading;
                    this.patsWizardMenuItem.Enabled = false;
                    this.startStopMenuItem.Enabled = e.Connection.Options.Mode.Id != ModeTypeId.Simulation;
                    this.resetSimulationAccountMenuItem.Enabled = true;
                    if (((e.Connection.FeatureTypes[FeatureTypeId.Quotes1Minute] != null) || (e.Connection.FeatureTypes[FeatureTypeId.QuotesDaily] != null)) || (e.Connection.FeatureTypes[FeatureTypeId.QuotesTick] != null))
                    {
                        this.historicalDataMenuItem.Enabled = true;
                    }
                    else
                    {
                        this.historicalDataMenuItem.Enabled = false;
                    }
                    if (e.Connection.Accounts.FindByName(e.Connection.SimulationAccountName) == null)
                    {
                        e.Connection.CreateSimulationAccount(e.Connection.SimulationAccountName, new SimulationAccountOptions());
                    }
                    AccountForm form = new AccountForm();
                    form.Account = Connection.Accounts[0];
                    form.MdiParent = this;
                    form.WindowState = FormWindowState.Normal;
                    form.Show();
                    this.Text = "TradeMagic - " + e.Connection.Options.Provider.Name;
                    this.PanelConnected.Text = "Connected";
                    this.PanelMode.Text = Connection.Options.Mode.Name;
                    if (e.Connection.Options.Mode.Id == ModeTypeId.Simulation)
                    {
                        this.controlCenterMenuItem.Enabled = true;
                        this.ShowSimulationControlCenter();
                    }
                }
            }
            else if (e.Connection.ConnectionStatusId == ConnectionStatusId.ConnectionLost)
            {
                this.PanelConnected.Text = "Connection lost";
            }
            else if (e.Connection.ConnectionStatusId == ConnectionStatusId.Disconnected)
            {
                this.accountsMenuItem.MenuItems.Clear();
                this.connectMenuItem.Enabled = true;
                this.controlCenterMenuItem.Enabled = false;
                this.dataMenuItem.Enabled = false;
                this.dataRecorderMenuItem.Enabled = false;
                this.disconnectMenuItem.Enabled = false;
                this.accountsMenuItem.Enabled = false;
                this.patsWizardMenuItem.Enabled = true;
                this.mbtNavigatorMenuItem.Enabled = false;
                this.resetSimulationAccountMenuItem.Enabled = false;
                foreach (Form form2 in base.MdiChildren)
                {
                    form2.Close();
                }
                if (e.Error != ErrorCode.NoError)
                {
                    Connection.ProcessEventArgs(new ITradingErrorEventArgs(Connection, ErrorCode.NotConnected, e.NativeError, "Unable to connect"));
                }
                this.Text = "TradeMagic";
                this.PanelConnected.Text = "Not Connected";
                this.PanelMode.Text = "";
                if (this.simulationControlCenter != null)
                {
                    this.simulationControlCenter.Close();
                    this.simulationControlCenter = null;
                }
            }
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            this.traceLog = new TraceLogForm();
            this.traceLog.MdiParent = this;
            this.PanelConnected.Alignment = HorizontalAlignment.Center;
            this.PanelConnected.AutoSize = StatusBarPanelAutoSize.Contents;
            this.PanelConnected.Style = StatusBarPanelStyle.Text;
            this.PanelFill.AutoSize = StatusBarPanelAutoSize.Spring;
            this.PanelMode.Alignment = HorizontalAlignment.Center;
            this.PanelMode.Style = StatusBarPanelStyle.Text;
            this.PanelTime.Alignment = HorizontalAlignment.Center;
            this.PanelTime.Style = StatusBarPanelStyle.Text;
            this.StatusBar.Panels.Add(this.PanelConnected);
            this.StatusBar.Panels.Add(this.PanelFill);
            this.StatusBar.Panels.Add(this.PanelMode);
            this.StatusBar.Panels.Add(this.PanelTime);
            this.PanelConnected.Text = "Not Connected";
            Connection.Accounts.Account += new AccountEventHandler(this.MainWindow_Account);
            Connection.ConnectionStatus += new ConnectionStatusEventHandler(this.MainWindow_ConnectionStatus);
            Connection.Error += Globals.DefaultErrorArgs;
            Connection.Timer += new TimerEventHandler(this.MainWindow_Timer);
            this.connectMenuItem.Enabled = true;
            this.disconnectMenuItem.Enabled = false;
            this.dataMenuItem.Enabled = false;
            this.accountsMenuItem.Enabled = false;
            this.mbtNavigatorMenuItem.Enabled = false;
            this.resetSimulationAccountMenuItem.Enabled = false;
            if (!this.boolFirstLogonDone)
            {
                if (Connection.GetLicense("Startup").Id == LicenseTypeId.NotRegistered)
                {
                    if (this.boolFirstLogonDone)
                    {
                        Application.Exit();
                    }
                    if (MessageBox.Show("There is no valid TradeMagic registration.\r\n\r\nTradeMagic will not start without prior registration. Do you want to register now ?", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1) == DialogResult.No)
                    {
                        Application.Exit();
                    }
                    RegisterForm form = new RegisterForm();
                    form.WindowState = FormWindowState.Normal;
                    if (form.ShowDialog() != DialogResult.OK)
                    {
                        Application.Exit();
                    }
                }
                this.boolFirstLogonDone = true;
                ConnectForm form2 = new ConnectForm();
                form2.Connection = Connection;
                form2.ShowDialog();
            }
        }

        private void MainWindow_Timer(object sender, TimerEventArgs e)
        {
            this.PanelTime.Text = e.Time.ToString("HH:mm:ss");
        }

        private void MarketDataMenuItem_Click(object sender, EventArgs e)
        {
            MarketDataForm form = new MarketDataForm();
            form.Connection = Connection;
            form.MdiParent = this;
            form.WindowState = FormWindowState.Normal;
            form.Show();
        }

        private void MarketDepthMenuItem_Click(object sender, EventArgs e)
        {
            MarketDepthForm form = new MarketDepthForm();
            form.Connection = Connection;
            form.MdiParent = this;
            form.WindowState = FormWindowState.Normal;
            form.Show();
        }

        private void mbtNavigatorMenuItem_Click(object sender, EventArgs e)
        {
            NavigatorForm form = new NavigatorForm();
            form.MdiParent = this;
            form.WindowState = FormWindowState.Normal;
            form.Show();
        }

        private void NewsMenuItem_Click(object sender, EventArgs e)
        {
            NewsItemsForm form = new NewsItemsForm();
            form.Connection = Connection;
            form.MdiParent = this;
            form.WindowState = FormWindowState.Normal;
            form.Show();
        }

        private void optionsMenuItem_Click(object sender, EventArgs e)
        {
            new OptionsForm().ShowDialog();
        }

        private void patsWizardMenuItem_Click(object sender, EventArgs e)
        {
            PatsWizard wizard = new PatsWizard();
            wizard.WindowState = FormWindowState.Normal;
            wizard.ShowDialog();
        }

        private void registrationMenuItem_Click(object sender, EventArgs e)
        {
            RegisterForm form = new RegisterForm();
            form.WindowState = FormWindowState.Normal;
            form.ShowDialog();
        }

        private void resetSimulationAccountMenuItem_Click(object sender, EventArgs e)
        {
            ResetSimAccountForm form = new ResetSimAccountForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                foreach (Account account in Connection.Accounts)
                {
                    account.SimulationReset();
                }
            }
        }

        private void ShowSimulationControlCenter()
        {
            if (this.simulationControlCenter == null)
            {
                this.simulationControlCenter = new SimulationControlCenter();
                this.simulationControlCenter.Connection = Connection;
                this.simulationControlCenter.MdiParent = this;
            }
            this.simulationControlCenter.Show();
        }

        private void startStopMenuItem_Click(object sender, EventArgs e)
        {
            if (this.dataRecorderForm == null)
            {
                this.dataRecorderForm = new DataRecorderForm();
                this.dataRecorderForm.Connection = Connection;
                this.dataRecorderForm.MdiParent = this;
                this.dataRecorderForm.WindowState = FormWindowState.Normal;
            }
            this.dataRecorderForm.Show();
        }

        private void symbolsMenuItem_Click(object sender, EventArgs e)
        {
            SymbolsForm form = new SymbolsForm();
            form.Connection = Connection;
            form.MdiParent = this;
            form.WindowState = FormWindowState.Normal;
            form.Show();
        }

        private void TileHorizontalMenuItem_Click(object sender, EventArgs e)
        {
            base.LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void TileVerticalMenuItem_Click(object sender, EventArgs e)
        {
            base.LayoutMdi(MdiLayout.TileVertical);
        }

        private void traceLogMenuItem_Click(object sender, EventArgs e)
        {
            this.traceLog.Show();
            this.traceLog.Focus();
        }
    }
}

