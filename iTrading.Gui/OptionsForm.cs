using iTrading.Core.Kernel;

namespace iTrading.Gui
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Globalization;
    using System.Resources;
    using System.Windows.Forms;
    using System.Xml;
    using iTrading.Core.Kernel;

    /// <summary>
    /// </summary>
    public class OptionsForm : Form
    {
        private Button cancel;
        private NumericUpDown CommissionFutures;
        private NumericUpDown CommissionMinFutures;
        private NumericUpDown CommissionMinOptions;
        private NumericUpDown CommissionMinStocks;
        private NumericUpDown CommissionOptions;
        private NumericUpDown CommissionStocks;
        private Container components = null;
        private NumericUpDown DelayCommunication;
        private NumericUpDown DelayExchange;
        private TabPage futuresSimulatorTabPage;
        private TabPage generalSimulatorTabPage;
        private NumericUpDown InitialCash;
        private Label label1;
        private Label label10;
        private Label label11;
        private Label label12;
        private Label label13;
        private Label label14;
        private Label label15;
        private Label label16;
        private Label label17;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private NumericUpDown MaintenanceMargin;
        private NumericUpDown Margin;
        private NumericUpDown MarginFutures;
        private Button ok;
        private TabPage optionsSimulatorTabPage;
        private NumericUpDown SlippageFutures;
        private NumericUpDown SlippageOptions;
        private NumericUpDown SlippageStocks;
        private TabPage stocksSimulatorTabPage;
        private TabControl tabControl1;
        private TabControl tabControl2;
        private TabPage tabPage1;
        private NumericUpDown WaitForMarketDataSeconds;

        /// <summary>
        /// </summary>
        public OptionsForm()
        {
            this.InitializeComponent();
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
            ResourceManager manager = new ResourceManager(typeof(OptionsForm));
            this.tabControl1 = new TabControl();
            this.tabPage1 = new TabPage();
            this.tabControl2 = new TabControl();
            this.generalSimulatorTabPage = new TabPage();
            this.label17 = new Label();
            this.WaitForMarketDataSeconds = new NumericUpDown();
            this.label16 = new Label();
            this.MaintenanceMargin = new NumericUpDown();
            this.label15 = new Label();
            this.Margin = new NumericUpDown();
            this.label14 = new Label();
            this.InitialCash = new NumericUpDown();
            this.label13 = new Label();
            this.DelayExchange = new NumericUpDown();
            this.label10 = new Label();
            this.DelayCommunication = new NumericUpDown();
            this.futuresSimulatorTabPage = new TabPage();
            this.label1 = new Label();
            this.CommissionMinFutures = new NumericUpDown();
            this.label2 = new Label();
            this.MarginFutures = new NumericUpDown();
            this.label3 = new Label();
            this.SlippageFutures = new NumericUpDown();
            this.label8 = new Label();
            this.CommissionFutures = new NumericUpDown();
            this.stocksSimulatorTabPage = new TabPage();
            this.label7 = new Label();
            this.CommissionMinStocks = new NumericUpDown();
            this.label12 = new Label();
            this.SlippageStocks = new NumericUpDown();
            this.label11 = new Label();
            this.CommissionStocks = new NumericUpDown();
            this.optionsSimulatorTabPage = new TabPage();
            this.label4 = new Label();
            this.CommissionMinOptions = new NumericUpDown();
            this.label6 = new Label();
            this.SlippageOptions = new NumericUpDown();
            this.label9 = new Label();
            this.CommissionOptions = new NumericUpDown();
            this.cancel = new Button();
            this.ok = new Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.generalSimulatorTabPage.SuspendLayout();
            this.WaitForMarketDataSeconds.BeginInit();
            this.MaintenanceMargin.BeginInit();
            this.Margin.BeginInit();
            this.InitialCash.BeginInit();
            this.DelayExchange.BeginInit();
            this.DelayCommunication.BeginInit();
            this.futuresSimulatorTabPage.SuspendLayout();
            this.CommissionMinFutures.BeginInit();
            this.MarginFutures.BeginInit();
            this.SlippageFutures.BeginInit();
            this.CommissionFutures.BeginInit();
            this.stocksSimulatorTabPage.SuspendLayout();
            this.CommissionMinStocks.BeginInit();
            this.SlippageStocks.BeginInit();
            this.CommissionStocks.BeginInit();
            this.optionsSimulatorTabPage.SuspendLayout();
            this.CommissionMinOptions.BeginInit();
            this.SlippageOptions.BeginInit();
            this.CommissionOptions.BeginInit();
            base.SuspendLayout();
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = DockStyle.Fill;
            this.tabControl1.Location = new Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new Size(0x112, 0x119);
            this.tabControl1.TabIndex = 0;
            this.tabPage1.Controls.Add(this.tabControl2);
            this.tabPage1.Controls.Add(this.cancel);
            this.tabPage1.Controls.Add(this.ok);
            this.tabPage1.Location = new Point(4, 0x16);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new Size(0x10a, 0xff);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Simulator";
            this.tabControl2.Controls.Add(this.generalSimulatorTabPage);
            this.tabControl2.Controls.Add(this.futuresSimulatorTabPage);
            this.tabControl2.Controls.Add(this.stocksSimulatorTabPage);
            this.tabControl2.Controls.Add(this.optionsSimulatorTabPage);
            this.tabControl2.Dock = DockStyle.Top;
            this.tabControl2.Location = new Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new Size(0x10a, 0xd0);
            this.tabControl2.TabIndex = 0;
            this.generalSimulatorTabPage.Controls.Add(this.label17);
            this.generalSimulatorTabPage.Controls.Add(this.WaitForMarketDataSeconds);
            this.generalSimulatorTabPage.Controls.Add(this.label16);
            this.generalSimulatorTabPage.Controls.Add(this.MaintenanceMargin);
            this.generalSimulatorTabPage.Controls.Add(this.label15);
            this.generalSimulatorTabPage.Controls.Add(this.Margin);
            this.generalSimulatorTabPage.Controls.Add(this.label14);
            this.generalSimulatorTabPage.Controls.Add(this.InitialCash);
            this.generalSimulatorTabPage.Controls.Add(this.label13);
            this.generalSimulatorTabPage.Controls.Add(this.DelayExchange);
            this.generalSimulatorTabPage.Controls.Add(this.label10);
            this.generalSimulatorTabPage.Controls.Add(this.DelayCommunication);
            this.generalSimulatorTabPage.Location = new Point(4, 0x16);
            this.generalSimulatorTabPage.Name = "generalSimulatorTabPage";
            this.generalSimulatorTabPage.Size = new Size(0x102, 0xb6);
            this.generalSimulatorTabPage.TabIndex = 3;
            this.generalSimulatorTabPage.Text = "General";
            this.label17.Location = new Point(0x21, 0x99);
            this.label17.Name = "label17";
            this.label17.Size = new Size(0x72, 0x17);
            this.label17.TabIndex = 0x2f;
            this.label17.Text = "Wait for data (secs):";
            this.label17.TextAlign = ContentAlignment.MiddleLeft;
            this.WaitForMarketDataSeconds.Location = new Point(160, 0x99);
            this.WaitForMarketDataSeconds.Name = "WaitForMarketDataSeconds";
            this.WaitForMarketDataSeconds.Size = new Size(0x49, 20);
            this.WaitForMarketDataSeconds.TabIndex = 6;
            this.label16.Location = new Point(0x21, 0x7d);
            this.label16.Name = "label16";
            this.label16.Size = new Size(0x72, 0x17);
            this.label16.TabIndex = 0x2d;
            this.label16.Text = "M&aintenance margin:";
            this.label16.TextAlign = ContentAlignment.MiddleLeft;
            this.MaintenanceMargin.DecimalPlaces = 2;
            int[] bits = new int[4];
            bits[0] = 5;
            bits[3] = 0x20000;
            this.MaintenanceMargin.Increment = new decimal(bits);
            this.MaintenanceMargin.Location = new Point(160, 0x7d);
            bits = new int[4];
            bits[0] = 1;
            this.MaintenanceMargin.Maximum = new decimal(bits);
            this.MaintenanceMargin.Name = "MaintenanceMargin";
            this.MaintenanceMargin.Size = new Size(0x49, 20);
            this.MaintenanceMargin.TabIndex = 5;
            bits = new int[4];
            bits[0] = 3;
            bits[3] = 0x10000;
            this.MaintenanceMargin.Value = new decimal(bits);
            this.label15.Location = new Point(0x21, 0x61);
            this.label15.Name = "label15";
            this.label15.Size = new Size(0x72, 0x17);
            this.label15.TabIndex = 0x2b;
            this.label15.Text = "&Margin:";
            this.label15.TextAlign = ContentAlignment.MiddleLeft;
            this.Margin.DecimalPlaces = 2;
            bits = new int[4];
            bits[0] = 5;
            bits[3] = 0x20000;
            this.Margin.Increment = new decimal(bits);
            this.Margin.Location = new Point(160, 0x61);
            bits = new int[4];
            bits[0] = 1;
            this.Margin.Maximum = new decimal(bits);
            this.Margin.Name = "Margin";
            this.Margin.Size = new Size(0x49, 20);
            this.Margin.TabIndex = 4;
            bits = new int[4];
            bits[0] = 5;
            bits[3] = 0x10000;
            this.Margin.Value = new decimal(bits);
            this.label14.Location = new Point(0x21, 0x45);
            this.label14.Name = "label14";
            this.label14.Size = new Size(0x7f, 0x18);
            this.label14.TabIndex = 0x29;
            this.label14.Text = "&Initial cash:";
            this.label14.TextAlign = ContentAlignment.MiddleLeft;
            bits = new int[4];
            bits[0] = 0x3e8;
            this.InitialCash.Increment = new decimal(bits);
            this.InitialCash.Location = new Point(160, 0x45);
            bits = new int[4];
            bits[0] = 0x540be3ff;
            bits[1] = 2;
            this.InitialCash.Maximum = new decimal(bits);
            this.InitialCash.Name = "InitialCash";
            this.InitialCash.Size = new Size(0x49, 20);
            this.InitialCash.TabIndex = 3;
            this.InitialCash.ThousandsSeparator = true;
            bits = new int[4];
            bits[0] = 0x186a0;
            this.InitialCash.Value = new decimal(bits);
            this.label13.Location = new Point(0x21, 0x2a);
            this.label13.Name = "label13";
            this.label13.Size = new Size(0x7f, 0x17);
            this.label13.TabIndex = 0x27;
            this.label13.Text = "Delay &exchange (msec):";
            this.label13.TextAlign = ContentAlignment.MiddleLeft;
            this.DelayExchange.Location = new Point(160, 0x2a);
            bits = new int[4];
            bits[0] = 0x1869f;
            this.DelayExchange.Maximum = new decimal(bits);
            this.DelayExchange.Name = "DelayExchange";
            this.DelayExchange.Size = new Size(0x49, 20);
            this.DelayExchange.TabIndex = 2;
            bits = new int[4];
            bits[0] = 500;
            this.DelayExchange.Value = new decimal(bits);
            this.label10.Location = new Point(0x21, 14);
            this.label10.Name = "label10";
            this.label10.Size = new Size(0x72, 0x17);
            this.label10.TabIndex = 0x25;
            this.label10.Text = "&Delay comm. (msec):";
            this.label10.TextAlign = ContentAlignment.MiddleLeft;
            this.DelayCommunication.Location = new Point(160, 14);
            bits = new int[4];
            bits[0] = 0x1869f;
            this.DelayCommunication.Maximum = new decimal(bits);
            this.DelayCommunication.Name = "DelayCommunication";
            this.DelayCommunication.Size = new Size(0x49, 20);
            this.DelayCommunication.TabIndex = 1;
            bits = new int[4];
            bits[0] = 150;
            this.DelayCommunication.Value = new decimal(bits);
            this.futuresSimulatorTabPage.Controls.Add(this.label1);
            this.futuresSimulatorTabPage.Controls.Add(this.CommissionMinFutures);
            this.futuresSimulatorTabPage.Controls.Add(this.label2);
            this.futuresSimulatorTabPage.Controls.Add(this.MarginFutures);
            this.futuresSimulatorTabPage.Controls.Add(this.label3);
            this.futuresSimulatorTabPage.Controls.Add(this.SlippageFutures);
            this.futuresSimulatorTabPage.Controls.Add(this.label8);
            this.futuresSimulatorTabPage.Controls.Add(this.CommissionFutures);
            this.futuresSimulatorTabPage.Location = new Point(4, 0x16);
            this.futuresSimulatorTabPage.Name = "futuresSimulatorTabPage";
            this.futuresSimulatorTabPage.Size = new Size(0x102, 0xb6);
            this.futuresSimulatorTabPage.TabIndex = 1;
            this.futuresSimulatorTabPage.Text = "Futures";
            this.label1.Location = new Point(40, 0x37);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x6b, 0x18);
            this.label1.TabIndex = 0x2f;
            this.label1.Text = "&Commission min.:";
            this.label1.TextAlign = ContentAlignment.MiddleLeft;
            this.CommissionMinFutures.DecimalPlaces = 2;
            this.CommissionMinFutures.Location = new Point(0x99, 0x37);
            this.CommissionMinFutures.Name = "CommissionMinFutures";
            this.CommissionMinFutures.Size = new Size(0x4a, 20);
            this.CommissionMinFutures.TabIndex = 2;
            this.label2.Location = new Point(40, 0x7d);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x6b, 0x17);
            this.label2.TabIndex = 0x2e;
            this.label2.Text = "&Margin req. per unit:";
            this.label2.TextAlign = ContentAlignment.MiddleLeft;
            this.MarginFutures.Location = new Point(0x99, 0x7d);
            bits = new int[4];
            bits[0] = 0x98967f;
            this.MarginFutures.Maximum = new decimal(bits);
            this.MarginFutures.Name = "MarginFutures";
            this.MarginFutures.Size = new Size(0x4a, 20);
            this.MarginFutures.TabIndex = 4;
            this.label3.Location = new Point(40, 90);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x6b, 0x18);
            this.label3.TabIndex = 0x2d;
            this.label3.Text = "&Slippage as ticks:";
            this.label3.TextAlign = ContentAlignment.MiddleLeft;
            this.SlippageFutures.Location = new Point(0x99, 90);
            this.SlippageFutures.Name = "SlippageFutures";
            this.SlippageFutures.Size = new Size(0x4a, 20);
            this.SlippageFutures.TabIndex = 3;
            this.label8.Location = new Point(40, 0x15);
            this.label8.Name = "label8";
            this.label8.Size = new Size(0x71, 0x17);
            this.label8.TabIndex = 0x2c;
            this.label8.Text = "&Commission per unit:";
            this.label8.TextAlign = ContentAlignment.MiddleLeft;
            this.CommissionFutures.DecimalPlaces = 2;
            this.CommissionFutures.Location = new Point(0x99, 0x15);
            this.CommissionFutures.Name = "CommissionFutures";
            this.CommissionFutures.Size = new Size(0x4a, 20);
            this.CommissionFutures.TabIndex = 1;
            this.stocksSimulatorTabPage.Controls.Add(this.label7);
            this.stocksSimulatorTabPage.Controls.Add(this.CommissionMinStocks);
            this.stocksSimulatorTabPage.Controls.Add(this.label12);
            this.stocksSimulatorTabPage.Controls.Add(this.SlippageStocks);
            this.stocksSimulatorTabPage.Controls.Add(this.label11);
            this.stocksSimulatorTabPage.Controls.Add(this.CommissionStocks);
            this.stocksSimulatorTabPage.Location = new Point(4, 0x16);
            this.stocksSimulatorTabPage.Name = "stocksSimulatorTabPage";
            this.stocksSimulatorTabPage.Size = new Size(0x102, 0xb6);
            this.stocksSimulatorTabPage.TabIndex = 0;
            this.stocksSimulatorTabPage.Text = "Stocks";
            this.label7.Location = new Point(40, 0x45);
            this.label7.Name = "label7";
            this.label7.Size = new Size(0x6b, 0x18);
            this.label7.TabIndex = 0x27;
            this.label7.Text = "&Commission min.:";
            this.label7.TextAlign = ContentAlignment.MiddleLeft;
            this.CommissionMinStocks.DecimalPlaces = 2;
            this.CommissionMinStocks.Location = new Point(0x99, 0x45);
            this.CommissionMinStocks.Name = "CommissionMinStocks";
            this.CommissionMinStocks.Size = new Size(0x4a, 20);
            this.CommissionMinStocks.TabIndex = 2;
            this.label12.Location = new Point(40, 0x68);
            this.label12.Name = "label12";
            this.label12.Size = new Size(0x6b, 0x17);
            this.label12.TabIndex = 0x24;
            this.label12.Text = "&Slippage as ticks:";
            this.label12.TextAlign = ContentAlignment.MiddleLeft;
            this.SlippageStocks.Location = new Point(0x99, 0x68);
            this.SlippageStocks.Name = "SlippageStocks";
            this.SlippageStocks.Size = new Size(0x4a, 20);
            this.SlippageStocks.TabIndex = 3;
            this.label11.Location = new Point(40, 0x23);
            this.label11.Name = "label11";
            this.label11.Size = new Size(0x71, 0x17);
            this.label11.TabIndex = 0x23;
            this.label11.Text = "&Commission per unit:";
            this.label11.TextAlign = ContentAlignment.MiddleLeft;
            this.CommissionStocks.DecimalPlaces = 2;
            this.CommissionStocks.Location = new Point(0x99, 0x23);
            this.CommissionStocks.Name = "CommissionStocks";
            this.CommissionStocks.Size = new Size(0x4a, 20);
            this.CommissionStocks.TabIndex = 1;
            this.optionsSimulatorTabPage.Controls.Add(this.label4);
            this.optionsSimulatorTabPage.Controls.Add(this.CommissionMinOptions);
            this.optionsSimulatorTabPage.Controls.Add(this.label6);
            this.optionsSimulatorTabPage.Controls.Add(this.SlippageOptions);
            this.optionsSimulatorTabPage.Controls.Add(this.label9);
            this.optionsSimulatorTabPage.Controls.Add(this.CommissionOptions);
            this.optionsSimulatorTabPage.Location = new Point(4, 0x16);
            this.optionsSimulatorTabPage.Name = "optionsSimulatorTabPage";
            this.optionsSimulatorTabPage.Size = new Size(0x102, 0xb6);
            this.optionsSimulatorTabPage.TabIndex = 2;
            this.optionsSimulatorTabPage.Text = "Options";
            this.label4.Location = new Point(40, 0x45);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x6b, 0x18);
            this.label4.TabIndex = 0x37;
            this.label4.Text = "&Commission min.:";
            this.label4.TextAlign = ContentAlignment.MiddleLeft;
            this.CommissionMinOptions.DecimalPlaces = 2;
            this.CommissionMinOptions.Location = new Point(0x99, 0x45);
            this.CommissionMinOptions.Name = "CommissionMinOptions";
            this.CommissionMinOptions.Size = new Size(0x4a, 20);
            this.CommissionMinOptions.TabIndex = 2;
            this.CommissionMinOptions.ValueChanged += new EventHandler(this.numericUpDown1_ValueChanged);
            this.label6.Location = new Point(40, 0x68);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x6b, 0x18);
            this.label6.TabIndex = 0x35;
            this.label6.Text = "&Slippage as ticks:";
            this.label6.TextAlign = ContentAlignment.MiddleLeft;
            this.SlippageOptions.Location = new Point(0x99, 0x68);
            this.SlippageOptions.Name = "SlippageOptions";
            this.SlippageOptions.Size = new Size(0x4a, 20);
            this.SlippageOptions.TabIndex = 3;
            this.label9.Location = new Point(40, 0x23);
            this.label9.Name = "label9";
            this.label9.Size = new Size(0x71, 0x17);
            this.label9.TabIndex = 0x34;
            this.label9.Text = "&Commission per unit:";
            this.label9.TextAlign = ContentAlignment.MiddleLeft;
            this.CommissionOptions.DecimalPlaces = 2;
            this.CommissionOptions.Location = new Point(0x99, 0x23);
            this.CommissionOptions.Name = "CommissionOptions";
            this.CommissionOptions.Size = new Size(0x4a, 20);
            this.CommissionOptions.TabIndex = 1;
            this.cancel.DialogResult = DialogResult.Cancel;
            this.cancel.Location = new Point(0x99, 0xde);
            this.cancel.Name = "cancel";
            this.cancel.TabIndex = 11;
            this.cancel.Text = "&Cancel";
            this.ok.DialogResult = DialogResult.OK;
            this.ok.Location = new Point(40, 0xde);
            this.ok.Name = "ok";
            this.ok.TabIndex = 10;
            this.ok.Text = "&Ok";
            this.ok.Click += new EventHandler(this.ok_Click);
            base.AcceptButton = this.ok;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this.cancel;
            base.ClientSize = new Size(0x112, 0x119);
            base.Controls.Add(this.tabControl1);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "OptionsForm";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Options";
            base.Load += new EventHandler(this.OptionsForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.generalSimulatorTabPage.ResumeLayout(false);
            this.WaitForMarketDataSeconds.EndInit();
            this.MaintenanceMargin.EndInit();
            this.Margin.EndInit();
            this.InitialCash.EndInit();
            this.DelayExchange.EndInit();
            this.DelayCommunication.EndInit();
            this.futuresSimulatorTabPage.ResumeLayout(false);
            this.CommissionMinFutures.EndInit();
            this.MarginFutures.EndInit();
            this.SlippageFutures.EndInit();
            this.CommissionFutures.EndInit();
            this.stocksSimulatorTabPage.ResumeLayout(false);
            this.CommissionMinStocks.EndInit();
            this.SlippageStocks.EndInit();
            this.CommissionStocks.EndInit();
            this.optionsSimulatorTabPage.ResumeLayout(false);
            this.CommissionMinOptions.EndInit();
            this.SlippageOptions.EndInit();
            this.CommissionOptions.EndInit();
            base.ResumeLayout(false);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
        }

        private void ok_Click(object sender, EventArgs e)
        {
            XmlDocument document = new XmlDocument();
            XmlTextReader reader = new XmlTextReader(Globals.InstallDir + @"\Config.xml");
            document.Load(reader);
            reader.Close();
            if (document["TradeMagic"] == null)
            {
                document.AppendChild(document.CreateElement("TradeMagic"));
            }
            if (document["TradeMagic"]["Options"] == null)
            {
                document["TradeMagic"].AppendChild(document.CreateElement("Options"));
            }
            if (document["TradeMagic"]["Options"]["Simulator"] == null)
            {
                document["TradeMagic"]["Options"].AppendChild(document.CreateElement("Simulator"));
            }
            if (document["TradeMagic"]["Options"]["Simulator"]["General"] == null)
            {
                document["TradeMagic"]["Options"]["Simulator"].AppendChild(document.CreateElement("General"));
            }
            if (document["TradeMagic"]["Options"]["Simulator"]["General"]["DelayCommunication"] == null)
            {
                document["TradeMagic"]["Options"]["Simulator"]["General"].AppendChild(document.CreateElement("DelayCommunication"));
            }
            if (document["TradeMagic"]["Options"]["Simulator"]["General"]["DelayExchange"] == null)
            {
                document["TradeMagic"]["Options"]["Simulator"]["General"].AppendChild(document.CreateElement("DelayExchange"));
            }
            if (document["TradeMagic"]["Options"]["Simulator"]["General"]["InitialCash"] == null)
            {
                document["TradeMagic"]["Options"]["Simulator"]["General"].AppendChild(document.CreateElement("InitialCash"));
            }
            if (document["TradeMagic"]["Options"]["Simulator"]["General"]["Margin"] == null)
            {
                document["TradeMagic"]["Options"]["Simulator"]["General"].AppendChild(document.CreateElement("Margin"));
            }
            if (document["TradeMagic"]["Options"]["Simulator"]["General"]["MaintenanceMargin"] == null)
            {
                document["TradeMagic"]["Options"]["Simulator"]["General"].AppendChild(document.CreateElement("MaintenanceMargin"));
            }
            if (document["TradeMagic"]["Options"]["Simulator"]["General"]["WaitForMarketDataSeconds"] == null)
            {
                document["TradeMagic"]["Options"]["Simulator"]["General"].AppendChild(document.CreateElement("WaitForMarketDataSeconds"));
            }
            if (document["TradeMagic"]["Options"]["Simulator"]["Stocks"] == null)
            {
                document["TradeMagic"]["Options"]["Simulator"].AppendChild(document.CreateElement("Stocks"));
            }
            if (document["TradeMagic"]["Options"]["Simulator"]["Stocks"]["Commission"] == null)
            {
                document["TradeMagic"]["Options"]["Simulator"]["Stocks"].AppendChild(document.CreateElement("Commission"));
            }
            if (document["TradeMagic"]["Options"]["Simulator"]["Stocks"]["CommissionMin"] == null)
            {
                document["TradeMagic"]["Options"]["Simulator"]["Stocks"].AppendChild(document.CreateElement("CommissionMin"));
            }
            if (document["TradeMagic"]["Options"]["Simulator"]["Stocks"]["Slippage"] == null)
            {
                document["TradeMagic"]["Options"]["Simulator"]["Stocks"].AppendChild(document.CreateElement("Slippage"));
            }
            if (document["TradeMagic"]["Options"]["Simulator"]["Futures"] == null)
            {
                document["TradeMagic"]["Options"]["Simulator"].AppendChild(document.CreateElement("Futures"));
            }
            if (document["TradeMagic"]["Options"]["Simulator"]["Futures"]["Commission"] == null)
            {
                document["TradeMagic"]["Options"]["Simulator"]["Futures"].AppendChild(document.CreateElement("Commission"));
            }
            if (document["TradeMagic"]["Options"]["Simulator"]["Futures"]["CommissionMin"] == null)
            {
                document["TradeMagic"]["Options"]["Simulator"]["Futures"].AppendChild(document.CreateElement("CommissionMin"));
            }
            if (document["TradeMagic"]["Options"]["Simulator"]["Futures"]["Margin"] == null)
            {
                document["TradeMagic"]["Options"]["Simulator"]["Futures"].AppendChild(document.CreateElement("Margin"));
            }
            if (document["TradeMagic"]["Options"]["Simulator"]["Futures"]["Slippage"] == null)
            {
                document["TradeMagic"]["Options"]["Simulator"]["Futures"].AppendChild(document.CreateElement("Slippage"));
            }
            if (document["TradeMagic"]["Options"]["Simulator"]["Options"] == null)
            {
                document["TradeMagic"]["Options"]["Simulator"].AppendChild(document.CreateElement("Options"));
            }
            if (document["TradeMagic"]["Options"]["Simulator"]["Options"]["Commission"] == null)
            {
                document["TradeMagic"]["Options"]["Simulator"]["Options"].AppendChild(document.CreateElement("Commission"));
            }
            if (document["TradeMagic"]["Options"]["Simulator"]["Options"]["CommissionMin"] == null)
            {
                document["TradeMagic"]["Options"]["Simulator"]["Options"].AppendChild(document.CreateElement("CommissionMin"));
            }
            if (document["TradeMagic"]["Options"]["Simulator"]["Options"]["Slippage"] == null)
            {
                document["TradeMagic"]["Options"]["Simulator"]["Options"].AppendChild(document.CreateElement("Slippage"));
            }
            document["TradeMagic"]["Options"]["Simulator"]["General"]["DelayCommunication"].InnerText = Convert.ToString(this.DelayCommunication.Value, CultureInfo.InvariantCulture);
            document["TradeMagic"]["Options"]["Simulator"]["General"]["DelayExchange"].InnerText = Convert.ToString(this.DelayExchange.Value, CultureInfo.InvariantCulture);
            document["TradeMagic"]["Options"]["Simulator"]["General"]["InitialCash"].InnerText = Convert.ToString(this.InitialCash.Value, CultureInfo.InvariantCulture);
            document["TradeMagic"]["Options"]["Simulator"]["General"]["Margin"].InnerText = Convert.ToString(this.Margin.Value, CultureInfo.InvariantCulture);
            document["TradeMagic"]["Options"]["Simulator"]["General"]["MaintenanceMargin"].InnerText = Convert.ToString(this.MaintenanceMargin.Value, CultureInfo.InvariantCulture);
            document["TradeMagic"]["Options"]["Simulator"]["General"]["WaitForMarketDataSeconds"].InnerText = Convert.ToString(this.WaitForMarketDataSeconds.Value, CultureInfo.InvariantCulture);
            document["TradeMagic"]["Options"]["Simulator"]["Stocks"]["Commission"].InnerText = Convert.ToString(this.CommissionStocks.Value, CultureInfo.InvariantCulture);
            document["TradeMagic"]["Options"]["Simulator"]["Stocks"]["CommissionMin"].InnerText = Convert.ToString(this.CommissionMinStocks.Value, CultureInfo.InvariantCulture);
            document["TradeMagic"]["Options"]["Simulator"]["Stocks"]["Slippage"].InnerText = Convert.ToString(this.SlippageStocks.Value, CultureInfo.InvariantCulture);
            document["TradeMagic"]["Options"]["Simulator"]["Futures"]["Commission"].InnerText = Convert.ToString(this.CommissionFutures.Value, CultureInfo.InvariantCulture);
            document["TradeMagic"]["Options"]["Simulator"]["Futures"]["CommissionMin"].InnerText = Convert.ToString(this.CommissionMinFutures.Value, CultureInfo.InvariantCulture);
            document["TradeMagic"]["Options"]["Simulator"]["Futures"]["Margin"].InnerText = Convert.ToString(this.MarginFutures.Value, CultureInfo.InvariantCulture);
            document["TradeMagic"]["Options"]["Simulator"]["Futures"]["Slippage"].InnerText = Convert.ToString(this.SlippageFutures.Value, CultureInfo.InvariantCulture);
            document["TradeMagic"]["Options"]["Simulator"]["Options"]["Commission"].InnerText = Convert.ToString(this.CommissionOptions.Value, CultureInfo.InvariantCulture);
            document["TradeMagic"]["Options"]["Simulator"]["Options"]["CommissionMin"].InnerText = Convert.ToString(this.CommissionMinOptions.Value, CultureInfo.InvariantCulture);
            document["TradeMagic"]["Options"]["Simulator"]["Options"]["Slippage"].InnerText = Convert.ToString(this.SlippageOptions.Value, CultureInfo.InvariantCulture);
            document.Save(Globals.InstallDir + @"\Config.xml");
            foreach (Account account in MainWindow.Connection.Accounts)
            {
                if (account.IsSimulation)
                {
                    account.SimulationAccountOptions = new SimulationAccountOptions();
                }
            }
        }

        private void OptionsForm_Load(object sender, EventArgs e)
        {
            XmlDocument document = new XmlDocument();
            XmlTextReader reader = new XmlTextReader(Globals.InstallDir + @"\Config.xml");
            document.Load(reader);
            reader.Close();
            try
            {
                this.DelayCommunication.Value = Convert.ToDecimal(document["TradeMagic"]["Options"]["Simulator"]["General"]["DelayCommunication"].InnerText, CultureInfo.InvariantCulture);
                this.DelayExchange.Value = Convert.ToDecimal(document["TradeMagic"]["Options"]["Simulator"]["General"]["DelayExchange"].InnerText, CultureInfo.InvariantCulture);
                this.InitialCash.Value = Convert.ToDecimal(document["TradeMagic"]["Options"]["Simulator"]["General"]["InitialCash"].InnerText, CultureInfo.InvariantCulture);
                this.Margin.Value = Convert.ToDecimal(document["TradeMagic"]["Options"]["Simulator"]["General"]["Margin"].InnerText, CultureInfo.InvariantCulture);
                this.MaintenanceMargin.Value = Convert.ToDecimal(document["TradeMagic"]["Options"]["Simulator"]["General"]["MaintenanceMargin"].InnerText, CultureInfo.InvariantCulture);
                this.WaitForMarketDataSeconds.Value = Convert.ToDecimal(document["TradeMagic"]["Options"]["Simulator"]["General"]["WaitForMarketDataSeconds"].InnerText, CultureInfo.InvariantCulture);
                this.CommissionStocks.Value = Convert.ToDecimal(document["TradeMagic"]["Options"]["Simulator"]["Stocks"]["Commission"].InnerText, CultureInfo.InvariantCulture);
                this.CommissionMinStocks.Value = Convert.ToDecimal(document["TradeMagic"]["Options"]["Simulator"]["Stocks"]["CommissionMin"].InnerText, CultureInfo.InvariantCulture);
                this.SlippageStocks.Value = Convert.ToDecimal(document["TradeMagic"]["Options"]["Simulator"]["Stocks"]["Slippage"].InnerText, CultureInfo.InvariantCulture);
                this.CommissionFutures.Value = Convert.ToDecimal(document["TradeMagic"]["Options"]["Simulator"]["Futures"]["Commission"].InnerText, CultureInfo.InvariantCulture);
                this.CommissionMinFutures.Value = Convert.ToDecimal(document["TradeMagic"]["Options"]["Simulator"]["Futures"]["CommissionMin"].InnerText, CultureInfo.InvariantCulture);
                this.MarginFutures.Value = Convert.ToDecimal(document["TradeMagic"]["Options"]["Simulator"]["Futures"]["Margin"].InnerText, CultureInfo.InvariantCulture);
                this.SlippageFutures.Value = Convert.ToDecimal(document["TradeMagic"]["Options"]["Simulator"]["Futures"]["Slippage"].InnerText, CultureInfo.InvariantCulture);
                this.CommissionOptions.Value = Convert.ToDecimal(document["TradeMagic"]["Options"]["Simulator"]["Options"]["Commission"].InnerText, CultureInfo.InvariantCulture);
                this.CommissionMinOptions.Value = Convert.ToDecimal(document["TradeMagic"]["Options"]["Simulator"]["Options"]["CommissionMin"].InnerText, CultureInfo.InvariantCulture);
                this.SlippageOptions.Value = Convert.ToDecimal(document["TradeMagic"]["Options"]["Simulator"]["Options"]["Slippage"].InnerText, CultureInfo.InvariantCulture);
            }
            catch
            {
            }
        }
    }
}

