using System;
using System.ComponentModel;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;
using iTrading.Core.Kernel;
using TradeMagic.Pats;

namespace iTrading.Gui
{
    
    /// <summary>
    /// Patsystems conformance test wizard.
    /// </summary>
    public class PatsWizard : Form
    {
        private AccountForm accountForm = null;
        private TextBox accounts;
        private TextBox applicationId;
        private Button bidOfferButton;
        private TextBox commodities;
        private Container components = null;
        private Button connectButton;
        private Connection connection = new Connection();
        private TabPage connectTabPage;
        private Button disconnectButton;
        private TabPage downloadTabPage;
        private NumericUpDown enable;
        private TextBox exchanges;
        private TextBox hostAddress;
        private NumericUpDown hostHandShakeInterval;
        private NumericUpDown hostHandShakeTimeout;
        private NumericUpDown hostPort;
        private Label label1;
        private Label label10;
        private Label label11;
        private Label label12;
        private Label label13;
        private Label label14;
        private Label label15;
        private Label label16;
        private Label label17;
        private Label label18;
        private Label label19;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private TextBox licenseKey;
        private MarketDataForm marketDataForm = null;
        private Button marketDepthButton;
        private MarketDepthForm marketDepthForm = null;
        private ComboBox mode;
        private TextBox newPassword;
        private Button ordersButton;
        private TextBox password;
        private PatsOptions patsOptions = new PatsOptions();
        private TextBox priceAddress;
        private NumericUpDown priceHandShakeInterval;
        private NumericUpDown priceHandShakeTimeout;
        private NumericUpDown pricePort;
        private TextBox retypeNewPassword;
        private TextBox statusTextBox;
        private CheckBox superTas;
        private TabControl tabControl1;
        private TextBox user;

        /// <summary>
        /// </summary>
        public PatsWizard()
        {
            this.InitializeComponent();
        }

        private void bidOfferButton_Click(object sender, EventArgs e)
        {
            if ((this.marketDataForm == null) || !this.marketDataForm.Visible)
            {
                this.marketDataForm = new MarketDataForm();
                this.marketDataForm.Connection = this.connection;
                this.marketDataForm.WindowState = FormWindowState.Normal;
                this.marketDataForm.Show();
            }
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            if ((this.newPassword.Text.Length > 0) && (this.newPassword.Text != this.retypeNewPassword.Text))
            {
                MessageBox.Show("Retyped password does not match.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else
            {
                this.Gui2Data();
                this.patsOptions.Save();
                this.patsOptions.RunAtServer = false;
                this.connectButton.Enabled = false;
                this.connection.Connect(this.patsOptions);
            }
        }

        private void connection_ConnectionStatus(object sender, ConnectionStatusEventArgs e)
        {
            this.statusTextBox.Text = (e.ConnectionStatusId == ConnectionStatusId.Connected) ? "connected" : ("not connected" + ((e.NativeError.Length == 0) ? "" : (": " + e.NativeError)));
            if (e.ConnectionStatusId == ConnectionStatusId.Connected)
            {
                this.bidOfferButton.Enabled = true;
                this.connectButton.Enabled = false;
                this.disconnectButton.Enabled = true;
                this.marketDepthButton.Enabled = true;
                this.ordersButton.Enabled = true;
            }
            else
            {
                this.bidOfferButton.Enabled = false;
                this.connectButton.Enabled = true;
                this.disconnectButton.Enabled = false;
                this.marketDepthButton.Enabled = false;
                this.ordersButton.Enabled = false;
            }
            if (e.ConnectionStatusId == ConnectionStatusId.Connected)
            {
                this.accounts.Text = "";
                foreach (Account account in this.connection.Accounts)
                {
                    this.accounts.AppendText(account.Name + "\r\n");
                }
                this.exchanges.Text = "";
                foreach (string str in Adapter.Exchanges)
                {
                    this.exchanges.AppendText(str + "\r\n");
                }
                this.commodities.Text = "";
                foreach (string str2 in Adapter.Contracts)
                {
                    this.commodities.AppendText(str2 + "\r\n");
                }
            }
        }

        private void Data2Gui()
        {
            this.applicationId.Text = this.patsOptions.ApplicationId;
            this.hostAddress.Text = this.patsOptions.HostAddress;
            this.hostHandShakeInterval.Value = this.patsOptions.HostHandShakeInterval;
            this.hostHandShakeTimeout.Value = this.patsOptions.HostHandShakeTimeout;
            this.hostPort.Value = this.patsOptions.HostPort;
            this.licenseKey.Text = this.patsOptions.LicenseKey;
            this.password.Text = this.patsOptions.Password;
            this.priceAddress.Text = this.patsOptions.PriceAddress;
            this.priceHandShakeInterval.Value = this.patsOptions.PriceHandShakeInterval;
            this.priceHandShakeTimeout.Value = this.patsOptions.PriceHandShakeTimeout;
            this.pricePort.Value = this.patsOptions.PricePort;
            this.superTas.Checked = this.patsOptions.SuperTas;
            this.user.Text = this.patsOptions.User;
        }

        private void disconnectButton_Click(object sender, EventArgs e)
        {
            this.connection.Close();
        }

        /// <summary>
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void Gui2Data()
        {
            this.patsOptions.ApplicationId = this.applicationId.Text;
            this.patsOptions.Enable = (int) this.enable.Value;
            this.patsOptions.HostAddress = this.hostAddress.Text;
            this.patsOptions.HostHandShakeInterval = (int) this.hostHandShakeInterval.Value;
            this.patsOptions.HostHandShakeTimeout = (int) this.hostHandShakeTimeout.Value;
            this.patsOptions.HostPort = (int) this.hostPort.Value;
            this.patsOptions.LicenseKey = this.licenseKey.Text;
            this.patsOptions.Mode = ModeType.All.Find(this.mode.Text);
            this.patsOptions.NewPassword = this.newPassword.Text;
            this.patsOptions.Password = this.password.Text;
            this.patsOptions.PriceAddress = this.priceAddress.Text;
            this.patsOptions.PriceHandShakeInterval = (int) this.priceHandShakeInterval.Value;
            this.patsOptions.PriceHandShakeTimeout = (int) this.priceHandShakeTimeout.Value;
            this.patsOptions.PricePort = (int) this.pricePort.Value;
            this.patsOptions.SuperTas = this.superTas.Checked;
            this.patsOptions.User = this.user.Text;
        }

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            ResourceManager manager = new ResourceManager(typeof(PatsWizard));
            this.tabControl1 = new TabControl();
            this.connectTabPage = new TabPage();
            this.enable = new NumericUpDown();
            this.label19 = new Label();
            this.priceHandShakeTimeout = new NumericUpDown();
            this.label15 = new Label();
            this.priceHandShakeInterval = new NumericUpDown();
            this.label16 = new Label();
            this.hostHandShakeTimeout = new NumericUpDown();
            this.label17 = new Label();
            this.hostHandShakeInterval = new NumericUpDown();
            this.label18 = new Label();
            this.superTas = new CheckBox();
            this.mode = new ComboBox();
            this.label14 = new Label();
            this.ordersButton = new Button();
            this.marketDepthButton = new Button();
            this.bidOfferButton = new Button();
            this.retypeNewPassword = new TextBox();
            this.label13 = new Label();
            this.disconnectButton = new Button();
            this.newPassword = new TextBox();
            this.label12 = new Label();
            this.statusTextBox = new TextBox();
            this.connectButton = new Button();
            this.password = new TextBox();
            this.label7 = new Label();
            this.user = new TextBox();
            this.label8 = new Label();
            this.licenseKey = new TextBox();
            this.label6 = new Label();
            this.applicationId = new TextBox();
            this.label5 = new Label();
            this.pricePort = new NumericUpDown();
            this.label3 = new Label();
            this.priceAddress = new TextBox();
            this.label4 = new Label();
            this.hostPort = new NumericUpDown();
            this.label2 = new Label();
            this.hostAddress = new TextBox();
            this.label1 = new Label();
            this.downloadTabPage = new TabPage();
            this.exchanges = new TextBox();
            this.label11 = new Label();
            this.commodities = new TextBox();
            this.label10 = new Label();
            this.accounts = new TextBox();
            this.label9 = new Label();
            this.tabControl1.SuspendLayout();
            this.connectTabPage.SuspendLayout();
            this.enable.BeginInit();
            this.priceHandShakeTimeout.BeginInit();
            this.priceHandShakeInterval.BeginInit();
            this.hostHandShakeTimeout.BeginInit();
            this.hostHandShakeInterval.BeginInit();
            this.pricePort.BeginInit();
            this.hostPort.BeginInit();
            this.downloadTabPage.SuspendLayout();
            base.SuspendLayout();
            this.tabControl1.Controls.Add(this.connectTabPage);
            this.tabControl1.Controls.Add(this.downloadTabPage);
            this.tabControl1.Dock = DockStyle.Fill;
            this.tabControl1.Location = new Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new Size(610, 0x147);
            this.tabControl1.TabIndex = 0;
            this.connectTabPage.Controls.Add(this.enable);
            this.connectTabPage.Controls.Add(this.label19);
            this.connectTabPage.Controls.Add(this.priceHandShakeTimeout);
            this.connectTabPage.Controls.Add(this.label15);
            this.connectTabPage.Controls.Add(this.priceHandShakeInterval);
            this.connectTabPage.Controls.Add(this.label16);
            this.connectTabPage.Controls.Add(this.hostHandShakeTimeout);
            this.connectTabPage.Controls.Add(this.label17);
            this.connectTabPage.Controls.Add(this.hostHandShakeInterval);
            this.connectTabPage.Controls.Add(this.label18);
            this.connectTabPage.Controls.Add(this.superTas);
            this.connectTabPage.Controls.Add(this.mode);
            this.connectTabPage.Controls.Add(this.label14);
            this.connectTabPage.Controls.Add(this.ordersButton);
            this.connectTabPage.Controls.Add(this.marketDepthButton);
            this.connectTabPage.Controls.Add(this.bidOfferButton);
            this.connectTabPage.Controls.Add(this.retypeNewPassword);
            this.connectTabPage.Controls.Add(this.label13);
            this.connectTabPage.Controls.Add(this.disconnectButton);
            this.connectTabPage.Controls.Add(this.newPassword);
            this.connectTabPage.Controls.Add(this.label12);
            this.connectTabPage.Controls.Add(this.statusTextBox);
            this.connectTabPage.Controls.Add(this.connectButton);
            this.connectTabPage.Controls.Add(this.password);
            this.connectTabPage.Controls.Add(this.label7);
            this.connectTabPage.Controls.Add(this.user);
            this.connectTabPage.Controls.Add(this.label8);
            this.connectTabPage.Controls.Add(this.licenseKey);
            this.connectTabPage.Controls.Add(this.label6);
            this.connectTabPage.Controls.Add(this.applicationId);
            this.connectTabPage.Controls.Add(this.label5);
            this.connectTabPage.Controls.Add(this.pricePort);
            this.connectTabPage.Controls.Add(this.label3);
            this.connectTabPage.Controls.Add(this.priceAddress);
            this.connectTabPage.Controls.Add(this.label4);
            this.connectTabPage.Controls.Add(this.hostPort);
            this.connectTabPage.Controls.Add(this.label2);
            this.connectTabPage.Controls.Add(this.hostAddress);
            this.connectTabPage.Controls.Add(this.label1);
            this.connectTabPage.Location = new Point(4, 0x16);
            this.connectTabPage.Name = "connectTabPage";
            this.connectTabPage.Size = new Size(0x25a, 0x12d);
            this.connectTabPage.TabIndex = 0;
            this.connectTabPage.Text = "Connect";
            this.enable.Location = new Point(520, 0x98);
            int[] bits = new int[4];
            bits[0] = 0xff;
            this.enable.Maximum = new decimal(bits);
            this.enable.Name = "enable";
            this.enable.ReadOnly = true;
            this.enable.Size = new Size(0x38, 20);
            this.enable.TabIndex = 0x36;
            bits = new int[4];
            bits[0] = 0xff;
            this.enable.Value = new decimal(bits);
            this.label19.Location = new Point(440, 0x98);
            this.label19.Name = "label19";
            this.label19.Size = new Size(80, 0x17);
            this.label19.TabIndex = 0x37;
            this.label19.Text = "Enable:";
            this.label19.TextAlign = ContentAlignment.MiddleLeft;
            this.priceHandShakeTimeout.Location = new Point(520, 120);
            bits = new int[4];
            bits[0] = 0x708;
            this.priceHandShakeTimeout.Maximum = new decimal(bits);
            this.priceHandShakeTimeout.Name = "priceHandShakeTimeout";
            this.priceHandShakeTimeout.Size = new Size(0x38, 20);
            this.priceHandShakeTimeout.TabIndex = 14;
            this.label15.Location = new Point(440, 120);
            this.label15.Name = "label15";
            this.label15.Size = new Size(80, 0x17);
            this.label15.TabIndex = 0x35;
            this.label15.Text = "Price timeout:";
            this.label15.TextAlign = ContentAlignment.MiddleLeft;
            this.priceHandShakeInterval.Location = new Point(520, 0x58);
            bits = new int[4];
            bits[0] = 900;
            this.priceHandShakeInterval.Maximum = new decimal(bits);
            this.priceHandShakeInterval.Name = "priceHandShakeInterval";
            this.priceHandShakeInterval.Size = new Size(0x38, 20);
            this.priceHandShakeInterval.TabIndex = 13;
            this.label16.Location = new Point(440, 0x58);
            this.label16.Name = "label16";
            this.label16.Size = new Size(80, 0x17);
            this.label16.TabIndex = 0x34;
            this.label16.Text = "Price interval:";
            this.label16.TextAlign = ContentAlignment.MiddleLeft;
            this.hostHandShakeTimeout.Location = new Point(520, 0x38);
            bits = new int[4];
            bits[0] = 0x708;
            this.hostHandShakeTimeout.Maximum = new decimal(bits);
            this.hostHandShakeTimeout.Name = "hostHandShakeTimeout";
            this.hostHandShakeTimeout.Size = new Size(0x38, 20);
            this.hostHandShakeTimeout.TabIndex = 12;
            this.label17.Location = new Point(440, 0x38);
            this.label17.Name = "label17";
            this.label17.Size = new Size(0x48, 0x17);
            this.label17.TabIndex = 0x33;
            this.label17.Text = "Host timeout:";
            this.label17.TextAlign = ContentAlignment.MiddleLeft;
            this.hostHandShakeInterval.Location = new Point(520, 0x18);
            bits = new int[4];
            bits[0] = 900;
            this.hostHandShakeInterval.Maximum = new decimal(bits);
            this.hostHandShakeInterval.Name = "hostHandShakeInterval";
            this.hostHandShakeInterval.Size = new Size(0x38, 20);
            this.hostHandShakeInterval.TabIndex = 11;
            this.label18.Location = new Point(440, 0x18);
            this.label18.Name = "label18";
            this.label18.Size = new Size(0x48, 0x17);
            this.label18.TabIndex = 50;
            this.label18.Text = "Host interval:";
            this.label18.TextAlign = ContentAlignment.MiddleLeft;
            this.superTas.Location = new Point(0xe8, 0x98);
            this.superTas.Name = "superTas";
            this.superTas.RightToLeft = RightToLeft.Yes;
            this.superTas.Size = new Size(0x60, 0x18);
            this.superTas.TabIndex = 10;
            this.superTas.Text = "Super TAS";
            this.superTas.TextAlign = ContentAlignment.MiddleRight;
            this.mode.DropDownStyle = ComboBoxStyle.DropDownList;
            this.mode.Location = new Point(0x68, 0x98);
            this.mode.Name = "mode";
            this.mode.Size = new Size(0x58, 0x15);
            this.mode.Sorted = true;
            this.mode.TabIndex = 0x2c;
            this.mode.SelectedIndexChanged += new EventHandler(this.mode_SelectedIndexChanged);
            this.label14.Location = new Point(0x18, 0x98);
            this.label14.Name = "label14";
            this.label14.Size = new Size(0x40, 0x17);
            this.label14.TabIndex = 0x2d;
            this.label14.Text = "&Mode:";
            this.label14.TextAlign = ContentAlignment.MiddleLeft;
            this.ordersButton.Enabled = false;
            this.ordersButton.Location = new Point(0x1d8, 0xd0);
            this.ordersButton.Name = "ordersButton";
            this.ordersButton.TabIndex = 0x1a;
            this.ordersButton.Text = "Ord&ers ...";
            this.ordersButton.Click += new EventHandler(this.ordersButton_Click);
            this.marketDepthButton.Enabled = false;
            this.marketDepthButton.Location = new Point(0x178, 240);
            this.marketDepthButton.Name = "marketDepthButton";
            this.marketDepthButton.TabIndex = 0x19;
            this.marketDepthButton.Text = "&Mkt depth ...";
            this.marketDepthButton.Click += new EventHandler(this.marketDepthButton_Click);
            this.bidOfferButton.Enabled = false;
            this.bidOfferButton.Location = new Point(0x178, 0xd0);
            this.bidOfferButton.Name = "bidOfferButton";
            this.bidOfferButton.TabIndex = 0x18;
            this.bidOfferButton.Text = "&Bid/Offer ...";
            this.bidOfferButton.Click += new EventHandler(this.bidOfferButton_Click);
            this.retypeNewPassword.Location = new Point(0x68, 240);
            this.retypeNewPassword.Name = "retypeNewPassword";
            this.retypeNewPassword.PasswordChar = '*';
            this.retypeNewPassword.Size = new Size(0x58, 20);
            this.retypeNewPassword.TabIndex = 0x15;
            this.retypeNewPassword.Text = "";
            this.label13.Location = new Point(0x18, 240);
            this.label13.Name = "label13";
            this.label13.Size = new Size(0x48, 0x17);
            this.label13.TabIndex = 0x27;
            this.label13.Text = "Ret&ype pwd:";
            this.label13.TextAlign = ContentAlignment.MiddleLeft;
            this.disconnectButton.Enabled = false;
            this.disconnectButton.Location = new Point(280, 240);
            this.disconnectButton.Name = "disconnectButton";
            this.disconnectButton.TabIndex = 0x17;
            this.disconnectButton.Text = "&Disconnect";
            this.disconnectButton.Click += new EventHandler(this.disconnectButton_Click);
            this.newPassword.Location = new Point(0x68, 0xd0);
            this.newPassword.Name = "newPassword";
            this.newPassword.PasswordChar = '*';
            this.newPassword.Size = new Size(0x58, 20);
            this.newPassword.TabIndex = 20;
            this.newPassword.Text = "";
            this.label12.Location = new Point(0x18, 0xd0);
            this.label12.Name = "label12";
            this.label12.Size = new Size(0x48, 0x17);
            this.label12.TabIndex = 0x24;
            this.label12.Text = "&New pwd:";
            this.label12.TextAlign = ContentAlignment.MiddleLeft;
            this.statusTextBox.Dock = DockStyle.Bottom;
            this.statusTextBox.Location = new Point(0, 0x119);
            this.statusTextBox.Name = "statusTextBox";
            this.statusTextBox.ReadOnly = true;
            this.statusTextBox.Size = new Size(0x25a, 20);
            this.statusTextBox.TabIndex = 0x22;
            this.statusTextBox.Text = "not connected";
            this.connectButton.Location = new Point(280, 0xd0);
            this.connectButton.Name = "connectButton";
            this.connectButton.TabIndex = 0x16;
            this.connectButton.Text = "&Connect";
            this.connectButton.Click += new EventHandler(this.connectButton_Click);
            this.password.Location = new Point(0x68, 0x38);
            this.password.Name = "password";
            this.password.PasswordChar = '*';
            this.password.Size = new Size(0x58, 20);
            this.password.TabIndex = 2;
            this.password.Text = "";
            this.label7.Location = new Point(0x18, 0x38);
            this.label7.Name = "label7";
            this.label7.Size = new Size(0x40, 0x17);
            this.label7.TabIndex = 0x1f;
            this.label7.Text = "&Password:";
            this.label7.TextAlign = ContentAlignment.MiddleLeft;
            this.user.Location = new Point(0x68, 0x18);
            this.user.Name = "user";
            this.user.Size = new Size(0x58, 20);
            this.user.TabIndex = 1;
            this.user.Text = "";
            this.label8.Location = new Point(0x18, 0x18);
            this.label8.Name = "label8";
            this.label8.Size = new Size(0x40, 0x17);
            this.label8.TabIndex = 0x1c;
            this.label8.Text = "&User:";
            this.label8.TextAlign = ContentAlignment.MiddleLeft;
            this.licenseKey.Location = new Point(0x68, 120);
            this.licenseKey.Name = "licenseKey";
            this.licenseKey.Size = new Size(0x58, 20);
            this.licenseKey.TabIndex = 4;
            this.licenseKey.Text = "";
            this.label6.Location = new Point(0x18, 120);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x48, 0x17);
            this.label6.TabIndex = 0x1b;
            this.label6.Text = "&License key:";
            this.label6.TextAlign = ContentAlignment.MiddleLeft;
            this.applicationId.Location = new Point(0x68, 0x58);
            this.applicationId.Name = "applicationId";
            this.applicationId.Size = new Size(0x58, 20);
            this.applicationId.TabIndex = 3;
            this.applicationId.Text = "";
            this.label5.Location = new Point(0x18, 0x58);
            this.label5.Name = "label5";
            this.label5.Size = new Size(80, 0x17);
            this.label5.TabIndex = 0x1a;
            this.label5.Text = "&Application ID:";
            this.label5.TextAlign = ContentAlignment.MiddleLeft;
            this.pricePort.Location = new Point(0x138, 120);
            bits = new int[4];
            bits[0] = 0x98967f;
            this.pricePort.Maximum = new decimal(bits);
            this.pricePort.Name = "pricePort";
            this.pricePort.Size = new Size(0x58, 20);
            this.pricePort.TabIndex = 9;
            this.label3.Location = new Point(0xe8, 120);
            this.label3.Name = "label3";
            this.label3.Size = new Size(80, 0x17);
            this.label3.TabIndex = 0x19;
            this.label3.Text = "Price po&rt:";
            this.label3.TextAlign = ContentAlignment.MiddleLeft;
            this.priceAddress.Location = new Point(0x138, 0x58);
            this.priceAddress.Name = "priceAddress";
            this.priceAddress.Size = new Size(0x58, 20);
            this.priceAddress.TabIndex = 8;
            this.priceAddress.Text = "";
            this.label4.Location = new Point(0xe8, 0x58);
            this.label4.Name = "label4";
            this.label4.Size = new Size(80, 0x17);
            this.label4.TabIndex = 0x18;
            this.label4.Text = "Price &address:";
            this.label4.TextAlign = ContentAlignment.MiddleLeft;
            this.hostPort.Location = new Point(0x138, 0x38);
            bits = new int[4];
            bits[0] = 0x98967f;
            this.hostPort.Maximum = new decimal(bits);
            this.hostPort.Name = "hostPort";
            this.hostPort.Size = new Size(0x58, 20);
            this.hostPort.TabIndex = 7;
            this.label2.Location = new Point(0xe8, 0x38);
            this.label2.Name = "label2";
            this.label2.Size = new Size(80, 0x17);
            this.label2.TabIndex = 0x11;
            this.label2.Text = "Host &port:";
            this.label2.TextAlign = ContentAlignment.MiddleLeft;
            this.hostAddress.Location = new Point(0x138, 0x18);
            this.hostAddress.Name = "hostAddress";
            this.hostAddress.Size = new Size(0x58, 20);
            this.hostAddress.TabIndex = 6;
            this.hostAddress.Text = "";
            this.label1.Location = new Point(0xe8, 0x18);
            this.label1.Name = "label1";
            this.label1.Size = new Size(80, 0x17);
            this.label1.TabIndex = 15;
            this.label1.Text = "&Host address:";
            this.label1.TextAlign = ContentAlignment.MiddleLeft;
            this.downloadTabPage.Controls.Add(this.exchanges);
            this.downloadTabPage.Controls.Add(this.label11);
            this.downloadTabPage.Controls.Add(this.commodities);
            this.downloadTabPage.Controls.Add(this.label10);
            this.downloadTabPage.Controls.Add(this.accounts);
            this.downloadTabPage.Controls.Add(this.label9);
            this.downloadTabPage.Location = new Point(4, 0x16);
            this.downloadTabPage.Name = "downloadTabPage";
            this.downloadTabPage.Size = new Size(0x25a, 0x12d);
            this.downloadTabPage.TabIndex = 1;
            this.downloadTabPage.Text = "Download";
            this.exchanges.Location = new Point(400, 0x10);
            this.exchanges.Multiline = true;
            this.exchanges.Name = "exchanges";
            this.exchanges.ScrollBars = ScrollBars.Both;
            this.exchanges.Size = new Size(0xc0, 0x110);
            this.exchanges.TabIndex = 0x21;
            this.exchanges.Text = "";
            this.exchanges.WordWrap = false;
            this.label11.Location = new Point(320, 0x10);
            this.label11.Name = "label11";
            this.label11.Size = new Size(0x40, 0x17);
            this.label11.TabIndex = 0x22;
            this.label11.Text = "Exchanges:";
            this.label11.TextAlign = ContentAlignment.MiddleLeft;
            this.commodities.Location = new Point(0x60, 0x70);
            this.commodities.Multiline = true;
            this.commodities.Name = "commodities";
            this.commodities.ScrollBars = ScrollBars.Both;
            this.commodities.Size = new Size(0xc0, 0xb0);
            this.commodities.TabIndex = 0x1f;
            this.commodities.Text = "";
            this.commodities.WordWrap = false;
            this.label10.Location = new Point(0x10, 0x70);
            this.label10.Name = "label10";
            this.label10.Size = new Size(80, 0x17);
            this.label10.TabIndex = 0x20;
            this.label10.Text = "Commodities:";
            this.label10.TextAlign = ContentAlignment.MiddleLeft;
            this.accounts.Location = new Point(0x60, 0x10);
            this.accounts.Multiline = true;
            this.accounts.Name = "accounts";
            this.accounts.ScrollBars = ScrollBars.Both;
            this.accounts.Size = new Size(0xc0, 80);
            this.accounts.TabIndex = 0x1d;
            this.accounts.Text = "";
            this.accounts.WordWrap = false;
            this.label9.Location = new Point(0x10, 0x10);
            this.label9.Name = "label9";
            this.label9.Size = new Size(0x40, 0x17);
            this.label9.TabIndex = 30;
            this.label9.Text = "Accounts:";
            this.label9.TextAlign = ContentAlignment.MiddleLeft;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(610, 0x147);
            base.Controls.Add(this.tabControl1);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "PatsWizard";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Patsystems conformance test wizard";
            base.Closing += new CancelEventHandler(this.Wizard_Closing);
            base.Load += new EventHandler(this.Wizard_Load);
            this.tabControl1.ResumeLayout(false);
            this.connectTabPage.ResumeLayout(false);
            this.enable.EndInit();
            this.priceHandShakeTimeout.EndInit();
            this.priceHandShakeInterval.EndInit();
            this.hostHandShakeTimeout.EndInit();
            this.hostHandShakeInterval.EndInit();
            this.pricePort.EndInit();
            this.hostPort.EndInit();
            this.downloadTabPage.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void marketDepthButton_Click(object sender, EventArgs e)
        {
            if ((this.marketDepthForm == null) || !this.marketDepthForm.Visible)
            {
                this.marketDepthForm = new MarketDepthForm();
                this.marketDepthForm.Connection = this.connection;
                this.marketDepthForm.WindowState = FormWindowState.Normal;
                this.marketDepthForm.Show();
            }
        }

        private void mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            OptionsBase base2 = OptionsBase.Restore(ProviderType.All[ProviderTypeId.Patsystems], ModeType.All.Find(this.mode.Text));
            if ((base2 != null) && (base2 is PatsOptions))
            {
                this.patsOptions = (PatsOptions) base2;
            }
            this.Data2Gui();
        }

        private void ordersButton_Click(object sender, EventArgs e)
        {
            if ((this.accountForm == null) || !this.accountForm.Visible)
            {
                this.accountForm = new AccountForm();
                this.accountForm.Account = this.connection.Accounts[0];
                this.accountForm.WindowState = FormWindowState.Normal;
                this.accountForm.Show();
            }
        }

        private void Wizard_Closing(object sender, CancelEventArgs e)
        {
            if (this.accountForm != null)
            {
                this.accountForm.Close();
            }
            if (this.marketDataForm != null)
            {
                this.marketDataForm.Close();
            }
            if (this.marketDepthForm != null)
            {
                this.marketDepthForm.Close();
            }
            this.connection.Close();
        }

        private void Wizard_Load(object sender, EventArgs e)
        {
            foreach (ModeType type in ModeType.All.Values)
            {
                this.mode.Items.Add(type.Name);
            }
            this.mode.SelectedItem = ModeType.All[ModeTypeId.Demo].Name;
            this.connection.ConnectionStatus += new ConnectionStatusEventHandler(this.connection_ConnectionStatus);
            this.connection.Error += Globals.DefaultErrorArgs;
            OptionsBase base2 = OptionsBase.Restore(ProviderType.All[ProviderTypeId.Patsystems], ModeType.All.Find(this.mode.Text));
            if ((base2 != null) && (base2 is PatsOptions))
            {
                this.patsOptions = (PatsOptions) base2;
            }
            this.Data2Gui();
        }
    }
}

