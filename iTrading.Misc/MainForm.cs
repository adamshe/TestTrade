namespace TradeMagic.Misc
{
    using Microsoft.Win32;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Resources;
    using System.Runtime.InteropServices;
    using System.Security.Cryptography;
    using System.Timers;
    using System.Windows.Forms;
    using System.Xml;
    using TradeMagic.Cbi;
    using TradeMagic.Com;
    using TradeMagic.Gui;

    public class MainForm : Form
    {
        private ComboBox actionComboBox;
        private IContainer components;
        private ContextMenu contextMenu;
        private static string[] dtnDlls = new string[] { "DTNHistoryLookup.dll", "DTNSymbolLookup.dll", "DTNOptionChainLookup.dll", "DTNNewsLookup.dll", "IQ_API.dll", "IQFeedX.dll" };
        private DateTimePicker endDateTimePicker;
        private MenuItem exitMenuItem;
        private Label label1;
        private Label label10;
        private Label label11;
        private Label label12;
        private Label label13;
        private Label label14;
        private Label label15;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private TabPage licenseTabPage;
        private TextBox licenseTextBox;
        private static MainForm mainForm;
        private Button manageButton;
        private int maxProcessSeconds = 1;
        private double meanSeconds = 0.0;
        private TextBox newPasswordTextBox;
        private NotifyIcon notifyIcon;
        private Panel panel1;
        private Panel panel2;
        private TextBox passwordTextBox;
        private NumericUpDown queryMaxSeconds;
        private NumericUpDown queryMinutes;
        private TextBox retypePwdTextBox;
        private Button startButton;
        private DateTimePicker startDateTimePicker;
        private ComboBox statusComboBox;
        private TabControl tabControl;
        private int testCount = 0;
        private System.Timers.Timer timer = null;
        internal TraceLog traceLog;
        private TabPage traceTabPage;
        private TextBox urlTextBox;
        private TextBox userIDTextBox;
        private TabPage vendor;
        private ComboBox vendorCodeComboBox;
        private TextBox vendorCodeTextBox2;
        private TextBox xmlTextBox;

        public MainForm()
        {
            this.InitializeComponent();
        }

        private void actionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.actionComboBox.Text)
            {
                case "Activity":
                    this.endDateTimePicker.Enabled = true;
                    this.licenseTextBox.Enabled = false;
                    this.newPasswordTextBox.Enabled = false;
                    this.retypePwdTextBox.Enabled = false;
                    this.startDateTimePicker.Enabled = true;
                    this.statusComboBox.Enabled = false;
                    return;

                case "ChangePassword":
                    this.endDateTimePicker.Enabled = false;
                    this.licenseTextBox.Enabled = false;
                    this.newPasswordTextBox.Enabled = true;
                    this.retypePwdTextBox.Enabled = true;
                    this.startDateTimePicker.Enabled = false;
                    this.statusComboBox.Enabled = false;
                    return;

                case "Check":
                    this.endDateTimePicker.Enabled = true;
                    this.licenseTextBox.Enabled = false;
                    this.newPasswordTextBox.Enabled = false;
                    this.retypePwdTextBox.Enabled = false;
                    this.startDateTimePicker.Enabled = true;
                    this.statusComboBox.Enabled = false;
                    return;

                case "LicenseInfo":
                    this.endDateTimePicker.Enabled = false;
                    this.licenseTextBox.Enabled = true;
                    this.newPasswordTextBox.Enabled = false;
                    this.retypePwdTextBox.Enabled = false;
                    this.startDateTimePicker.Enabled = false;
                    this.statusComboBox.Enabled = false;
                    return;

                case "LicenseSummary":
                    this.endDateTimePicker.Enabled = false;
                    this.licenseTextBox.Enabled = false;
                    this.newPasswordTextBox.Enabled = false;
                    this.retypePwdTextBox.Enabled = false;
                    this.startDateTimePicker.Enabled = false;
                    this.statusComboBox.Enabled = false;
                    return;

                case "Report":
                    this.endDateTimePicker.Enabled = true;
                    this.licenseTextBox.Enabled = false;
                    this.newPasswordTextBox.Enabled = false;
                    this.retypePwdTextBox.Enabled = false;
                    this.startDateTimePicker.Enabled = true;
                    this.statusComboBox.Enabled = false;
                    break;

                case "UpdateLicense":
                    this.endDateTimePicker.Enabled = true;
                    this.licenseTextBox.Enabled = true;
                    this.newPasswordTextBox.Enabled = false;
                    this.retypePwdTextBox.Enabled = false;
                    this.startDateTimePicker.Enabled = true;
                    this.statusComboBox.Enabled = true;
                    break;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void exitMenuItem_Click(object sender, EventArgs e)
        {
            if (this.timer != null)
            {
                this.timer.Close();
            }
            Application.Exit();
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ResourceManager manager = new ResourceManager(typeof(MainForm));
            this.notifyIcon = new NotifyIcon(this.components);
            this.contextMenu = new ContextMenu();
            this.exitMenuItem = new MenuItem();
            this.tabControl = new TabControl();
            this.vendor = new TabPage();
            this.panel2 = new Panel();
            this.manageButton = new Button();
            this.retypePwdTextBox = new TextBox();
            this.label13 = new Label();
            this.newPasswordTextBox = new TextBox();
            this.label12 = new Label();
            this.statusComboBox = new ComboBox();
            this.label11 = new Label();
            this.label10 = new Label();
            this.endDateTimePicker = new DateTimePicker();
            this.label9 = new Label();
            this.startDateTimePicker = new DateTimePicker();
            this.licenseTextBox = new TextBox();
            this.label8 = new Label();
            this.actionComboBox = new ComboBox();
            this.label7 = new Label();
            this.passwordTextBox = new TextBox();
            this.label6 = new Label();
            this.label5 = new Label();
            this.xmlTextBox = new TextBox();
            this.panel1 = new Panel();
            this.urlTextBox = new TextBox();
            this.licenseTabPage = new TabPage();
            this.userIDTextBox = new TextBox();
            this.label14 = new Label();
            this.vendorCodeTextBox2 = new TextBox();
            this.label15 = new Label();
            this.label4 = new Label();
            this.queryMaxSeconds = new NumericUpDown();
            this.label3 = new Label();
            this.startButton = new Button();
            this.label2 = new Label();
            this.queryMinutes = new NumericUpDown();
            this.label1 = new Label();
            this.traceTabPage = new TabPage();
            this.traceLog = new TraceLog();
            this.vendorCodeComboBox = new ComboBox();
            this.tabControl.SuspendLayout();
            this.vendor.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.licenseTabPage.SuspendLayout();
            this.queryMaxSeconds.BeginInit();
            this.queryMinutes.BeginInit();
            this.traceTabPage.SuspendLayout();
            base.SuspendLayout();
            this.notifyIcon.ContextMenu = this.contextMenu;
            this.notifyIcon.Icon = (Icon) manager.GetObject("notifyIcon.Icon");
            this.notifyIcon.Text = "TradeMagic Misc";
            this.notifyIcon.Visible = true;
            this.notifyIcon.DoubleClick += new EventHandler(this.notifyIcon_DoubleClick);
            this.contextMenu.MenuItems.AddRange(new MenuItem[] { this.exitMenuItem });
            this.exitMenuItem.Index = 0;
            this.exitMenuItem.Text = "Exit";
            this.exitMenuItem.Click += new EventHandler(this.exitMenuItem_Click);
            this.tabControl.Controls.Add(this.vendor);
            this.tabControl.Controls.Add(this.licenseTabPage);
            this.tabControl.Controls.Add(this.traceTabPage);
            this.tabControl.Dock = DockStyle.Fill;
            this.tabControl.Location = new Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new Size(0x339, 0x1ca);
            this.tabControl.TabIndex = 0;
            this.vendor.Controls.Add(this.panel2);
            this.vendor.Controls.Add(this.panel1);
            this.vendor.Location = new Point(4, 0x19);
            this.vendor.Name = "vendor";
            this.vendor.Size = new Size(0x331, 0x1ad);
            this.vendor.TabIndex = 2;
            this.vendor.Text = "Vendor";
            this.panel2.Controls.Add(this.vendorCodeComboBox);
            this.panel2.Controls.Add(this.manageButton);
            this.panel2.Controls.Add(this.retypePwdTextBox);
            this.panel2.Controls.Add(this.label13);
            this.panel2.Controls.Add(this.newPasswordTextBox);
            this.panel2.Controls.Add(this.label12);
            this.panel2.Controls.Add(this.statusComboBox);
            this.panel2.Controls.Add(this.label11);
            this.panel2.Controls.Add(this.label10);
            this.panel2.Controls.Add(this.endDateTimePicker);
            this.panel2.Controls.Add(this.label9);
            this.panel2.Controls.Add(this.startDateTimePicker);
            this.panel2.Controls.Add(this.licenseTextBox);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.actionComboBox);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.passwordTextBox);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.xmlTextBox);
            this.panel2.Dock = DockStyle.Fill;
            this.panel2.Location = new Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new Size(0x331, 0x191);
            this.panel2.TabIndex = 1;
            this.manageButton.Location = new Point(0x6a, 360);
            this.manageButton.Name = "manageButton";
            this.manageButton.Size = new Size(0x69, 0x1b);
            this.manageButton.TabIndex = 0x13;
            this.manageButton.Text = "&Ok";
            this.manageButton.Click += new EventHandler(this.manageButton_Click);
            this.retypePwdTextBox.Location = new Point(0x70, 0x13a);
            this.retypePwdTextBox.Name = "retypePwdTextBox";
            this.retypePwdTextBox.PasswordChar = '*';
            this.retypePwdTextBox.Size = new Size(0xb0, 0x16);
            this.retypePwdTextBox.TabIndex = 0x12;
            this.retypePwdTextBox.Text = "";
            this.label13.Location = new Point(10, 0x13a);
            this.label13.Name = "label13";
            this.label13.Size = new Size(0x69, 0x1c);
            this.label13.TabIndex = 0x11;
            this.label13.Text = "Retype pwd:";
            this.label13.TextAlign = ContentAlignment.MiddleLeft;
            this.newPasswordTextBox.Location = new Point(0x70, 0x115);
            this.newPasswordTextBox.Name = "newPasswordTextBox";
            this.newPasswordTextBox.PasswordChar = '*';
            this.newPasswordTextBox.Size = new Size(0xb0, 0x16);
            this.newPasswordTextBox.TabIndex = 0x10;
            this.newPasswordTextBox.Text = "";
            this.label12.Location = new Point(10, 0x115);
            this.label12.Name = "label12";
            this.label12.Size = new Size(0x69, 0x1c);
            this.label12.TabIndex = 15;
            this.label12.Text = "New password:";
            this.label12.TextAlign = ContentAlignment.MiddleLeft;
            this.statusComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            this.statusComboBox.Location = new Point(0x70, 240);
            this.statusComboBox.Name = "statusComboBox";
            this.statusComboBox.Size = new Size(0xb0, 0x18);
            this.statusComboBox.TabIndex = 14;
            this.label11.Location = new Point(10, 240);
            this.label11.Name = "label11";
            this.label11.Size = new Size(0x60, 0x1c);
            this.label11.TabIndex = 13;
            this.label11.Text = "Status:";
            this.label11.TextAlign = ContentAlignment.MiddleLeft;
            this.label10.Location = new Point(10, 0xcb);
            this.label10.Name = "label10";
            this.label10.Size = new Size(0x60, 0x1c);
            this.label10.TabIndex = 12;
            this.label10.Text = "End:";
            this.label10.TextAlign = ContentAlignment.MiddleLeft;
            this.endDateTimePicker.Location = new Point(0x70, 0xcb);
            this.endDateTimePicker.Name = "endDateTimePicker";
            this.endDateTimePicker.Size = new Size(0xb0, 0x16);
            this.endDateTimePicker.TabIndex = 11;
            this.label9.Location = new Point(10, 0xa6);
            this.label9.Name = "label9";
            this.label9.Size = new Size(0x60, 0x1c);
            this.label9.TabIndex = 10;
            this.label9.Text = "Start:";
            this.label9.TextAlign = ContentAlignment.MiddleLeft;
            this.startDateTimePicker.Location = new Point(0x70, 0xa6);
            this.startDateTimePicker.Name = "startDateTimePicker";
            this.startDateTimePicker.Size = new Size(0xb0, 0x16);
            this.startDateTimePicker.TabIndex = 9;
            this.licenseTextBox.Location = new Point(0x70, 0x81);
            this.licenseTextBox.Name = "licenseTextBox";
            this.licenseTextBox.Size = new Size(0xb0, 0x16);
            this.licenseTextBox.TabIndex = 8;
            this.licenseTextBox.Text = "";
            this.label8.Location = new Point(10, 0x81);
            this.label8.Name = "label8";
            this.label8.Size = new Size(0x60, 0x1c);
            this.label8.TabIndex = 7;
            this.label8.Text = "License:";
            this.label8.TextAlign = ContentAlignment.MiddleLeft;
            this.actionComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            this.actionComboBox.Location = new Point(0x70, 0x5c);
            this.actionComboBox.Name = "actionComboBox";
            this.actionComboBox.Size = new Size(0xb0, 0x18);
            this.actionComboBox.Sorted = true;
            this.actionComboBox.TabIndex = 6;
            this.actionComboBox.SelectedIndexChanged += new EventHandler(this.actionComboBox_SelectedIndexChanged);
            this.label7.Location = new Point(10, 0x5c);
            this.label7.Name = "label7";
            this.label7.Size = new Size(0x60, 0x1c);
            this.label7.TabIndex = 5;
            this.label7.Text = "Action:";
            this.label7.TextAlign = ContentAlignment.MiddleLeft;
            this.passwordTextBox.Location = new Point(0x70, 0x37);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.PasswordChar = '*';
            this.passwordTextBox.Size = new Size(0xb0, 0x16);
            this.passwordTextBox.TabIndex = 4;
            this.passwordTextBox.Text = "";
            this.label6.Location = new Point(10, 0x37);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x60, 0x1c);
            this.label6.TabIndex = 3;
            this.label6.Text = "Password:";
            this.label6.TextAlign = ContentAlignment.MiddleLeft;
            this.label5.Location = new Point(10, 0x12);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x60, 0x1c);
            this.label5.TabIndex = 1;
            this.label5.Text = "Vendor code:";
            this.label5.TextAlign = ContentAlignment.MiddleLeft;
            this.xmlTextBox.Dock = DockStyle.Right;
            this.xmlTextBox.Location = new Point(0x134, 0);
            this.xmlTextBox.MaxLength = 0x5f5e0ff;
            this.xmlTextBox.Multiline = true;
            this.xmlTextBox.Name = "xmlTextBox";
            this.xmlTextBox.ScrollBars = ScrollBars.Both;
            this.xmlTextBox.Size = new Size(0x1fd, 0x191);
            this.xmlTextBox.TabIndex = 0;
            this.xmlTextBox.Text = "";
            this.xmlTextBox.WordWrap = false;
            this.panel1.Controls.Add(this.urlTextBox);
            this.panel1.Dock = DockStyle.Bottom;
            this.panel1.Location = new Point(0, 0x191);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x331, 0x1c);
            this.panel1.TabIndex = 0;
            this.urlTextBox.Dock = DockStyle.Fill;
            this.urlTextBox.Location = new Point(0, 0);
            this.urlTextBox.Multiline = true;
            this.urlTextBox.Name = "urlTextBox";
            this.urlTextBox.ReadOnly = true;
            this.urlTextBox.Size = new Size(0x331, 0x1c);
            this.urlTextBox.TabIndex = 0;
            this.urlTextBox.Text = "";
            this.licenseTabPage.Controls.Add(this.userIDTextBox);
            this.licenseTabPage.Controls.Add(this.label14);
            this.licenseTabPage.Controls.Add(this.vendorCodeTextBox2);
            this.licenseTabPage.Controls.Add(this.label15);
            this.licenseTabPage.Controls.Add(this.label4);
            this.licenseTabPage.Controls.Add(this.queryMaxSeconds);
            this.licenseTabPage.Controls.Add(this.label3);
            this.licenseTabPage.Controls.Add(this.startButton);
            this.licenseTabPage.Controls.Add(this.label2);
            this.licenseTabPage.Controls.Add(this.queryMinutes);
            this.licenseTabPage.Controls.Add(this.label1);
            this.licenseTabPage.Location = new Point(4, 0x19);
            this.licenseTabPage.Name = "licenseTabPage";
            this.licenseTabPage.Size = new Size(0x331, 0x1ad);
            this.licenseTabPage.TabIndex = 0;
            this.licenseTabPage.Text = "License";
            this.userIDTextBox.Location = new Point(0xad, 0x37);
            this.userIDTextBox.Name = "userIDTextBox";
            this.userIDTextBox.Size = new Size(0x99, 0x16);
            this.userIDTextBox.TabIndex = 2;
            this.userIDTextBox.Text = "";
            this.label14.Location = new Point(0x26, 0x37);
            this.label14.Name = "label14";
            this.label14.Size = new Size(0x60, 0x1c);
            this.label14.TabIndex = 11;
            this.label14.Text = "User ID:";
            this.label14.TextAlign = ContentAlignment.MiddleLeft;
            this.vendorCodeTextBox2.Location = new Point(0xad, 0x12);
            this.vendorCodeTextBox2.Name = "vendorCodeTextBox2";
            this.vendorCodeTextBox2.Size = new Size(0x99, 0x16);
            this.vendorCodeTextBox2.TabIndex = 1;
            this.vendorCodeTextBox2.Text = "";
            this.label15.Location = new Point(0x26, 0x12);
            this.label15.Name = "label15";
            this.label15.Size = new Size(0x60, 0x1c);
            this.label15.TabIndex = 9;
            this.label15.Text = "Vendor code:";
            this.label15.TextAlign = ContentAlignment.MiddleLeft;
            this.label4.Location = new Point(240, 0x81);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x3a, 0x1b);
            this.label4.TabIndex = 5;
            this.label4.Text = "seconds";
            this.label4.TextAlign = ContentAlignment.MiddleLeft;
            this.queryMaxSeconds.Location = new Point(0xad, 0x81);
            int[] bits = new int[4];
            bits[0] = 1;
            this.queryMaxSeconds.Minimum = new decimal(bits);
            this.queryMaxSeconds.Name = "queryMaxSeconds";
            this.queryMaxSeconds.Size = new Size(0x39, 0x16);
            this.queryMaxSeconds.TabIndex = 4;
            bits = new int[4];
            bits[0] = 3;
            this.queryMaxSeconds.Value = new decimal(bits);
            this.label3.Location = new Point(0x26, 0x81);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x7d, 0x1b);
            this.label3.TabIndex = 4;
            this.label3.Text = "Max. process time:";
            this.label3.TextAlign = ContentAlignment.MiddleLeft;
            this.startButton.Location = new Point(0x6a, 0xb9);
            this.startButton.Name = "startButton";
            this.startButton.Size = new Size(90, 0x1a);
            this.startButton.TabIndex = 5;
            this.startButton.Text = "&Start";
            this.startButton.Click += new EventHandler(this.startButton_Click);
            this.label2.Location = new Point(240, 0x5c);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x3a, 0x1b);
            this.label2.TabIndex = 2;
            this.label2.Text = "minutes";
            this.label2.TextAlign = ContentAlignment.MiddleLeft;
            this.queryMinutes.Location = new Point(0xad, 0x5c);
            bits = new int[4];
            bits[0] = 1;
            this.queryMinutes.Minimum = new decimal(bits);
            this.queryMinutes.Name = "queryMinutes";
            this.queryMinutes.Size = new Size(0x39, 0x16);
            this.queryMinutes.TabIndex = 3;
            bits = new int[4];
            bits[0] = 5;
            this.queryMinutes.Value = new decimal(bits);
            this.label1.Location = new Point(0x26, 0x5c);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x7d, 0x1b);
            this.label1.TabIndex = 0;
            this.label1.Text = "Query license every";
            this.label1.TextAlign = ContentAlignment.MiddleLeft;
            this.traceTabPage.Controls.Add(this.traceLog);
            this.traceTabPage.Location = new Point(4, 0x19);
            this.traceTabPage.Name = "traceTabPage";
            this.traceTabPage.Size = new Size(0x331, 0x1ad);
            this.traceTabPage.TabIndex = 1;
            this.traceTabPage.Text = "Trace";
            this.traceLog.Dock = DockStyle.Fill;
            this.traceLog.Location = new Point(0, 0);
            this.traceLog.ModuleName = "";
            this.traceLog.Name = "traceLog";
            this.traceLog.Size = new Size(0x331, 0x1ad);
            this.traceLog.TabIndex = 0;
            this.traceLog.ToFileOnly = false;
            this.vendorCodeComboBox.Location = new Point(0x70, 0x10);
            this.vendorCodeComboBox.Name = "vendorCodeComboBox";
            this.vendorCodeComboBox.Size = new Size(0xb0, 0x18);
            this.vendorCodeComboBox.TabIndex = 2;
            base.AcceptButton = this.manageButton;
            this.AutoScaleBaseSize = new Size(6, 15);
            base.ClientSize = new Size(0x339, 0x1ca);
            base.Controls.Add(this.tabControl);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.MinimumSize = new Size(0x2b8, 0x1a8);
            base.Name = "MainForm";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "TradeMagic";
            base.SizeChanged += new EventHandler(this.MainForm_SizeChanged);
            base.Load += new EventHandler(this.MainForm_Load);
            this.tabControl.ResumeLayout(false);
            this.vendor.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.licenseTabPage.ResumeLayout(false);
            this.queryMaxSeconds.EndInit();
            this.queryMinutes.EndInit();
            this.traceTabPage.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        [STAThread]
        private static void Main()
        {
            mainForm = new MainForm();
            mainForm.traceLog.ModuleName = "Misc";
            string[] commandLineArgs = Environment.GetCommandLineArgs();
            int index = 1;
            while (index < commandLineArgs.Length)
            {
                string str = commandLineArgs[index];
                if (str.ToLower() == "/register")
                {
                    mainForm.traceLog.ModuleName = "Install";
                    TradeMagic.Cbi.Connection connection = new TradeMagic.Cbi.Connection();
                    RegistrationServices services = new RegistrationServices();
                    TMProviderInterface interface2 = new TMProviderInterface();
                    if (!services.RegisterAssembly(connection.GetType().Assembly, AssemblyRegistrationFlags.SetCodeBase))
                    {
                        throw new TMException(ErrorCode.Panic, "Failed to register " + connection.GetType().Assembly.FullName + " for COM interop");
                    }
                    Trace.WriteLine("TradeMagic.Cbi.dll registered successfully");
                    if (!services.RegisterAssembly(interface2.GetType().Assembly, AssemblyRegistrationFlags.SetCodeBase))
                    {
                        throw new TMException(ErrorCode.Panic, "Failed to register " + interface2.GetType().Assembly.FullName + " for COM interop");
                    }
                    Trace.WriteLine("TradeMagic.Com.dll registered successfully");
                    RegisterDtn();
                    return;
                }
                if (str.ToLower() == "/unregister")
                {
                    mainForm.traceLog.ModuleName = "Install";
                    TradeMagic.Cbi.Connection connection2 = new TradeMagic.Cbi.Connection();
                    RegistrationServices services2 = new RegistrationServices();
                    TMProviderInterface interface3 = new TMProviderInterface();
                    if (!services2.UnregisterAssembly(connection2.GetType().Assembly))
                    {
                        throw new TMException(ErrorCode.Panic, "Failed to unregister " + connection2.GetType().Assembly.FullName + " for COM interop");
                    }
                    Trace.WriteLine("TradeMagic.Cbi.dll unregistered successfully");
                    if (!services2.UnregisterAssembly(interface3.GetType().Assembly))
                    {
                        throw new TMException(ErrorCode.Panic, "Failed to unregister " + interface3.GetType().Assembly.FullName + " for COM interop");
                    }
                    Trace.WriteLine("TradeMagic.Com.dll unregistered successfully");
                    UnregisterDtn();
                    return;
                }
                MessageBox.Show("Unknown command line parameter '" + str + "'", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                Application.Exit();
                return;
            }
            Application.Run(mainForm);
        }

        private void MainForm_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            base.Hide();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            TradeMagic.Cbi.Globals.AppId = "TradeMagic Misc";
            if (TradeMagic.Cbi.Globals.UserId != "bea017e7f83b2c593858b19d10f2c450")
            {
                this.queryMinutes.Value = 15M;
                this.queryMinutes.Enabled = false;
                this.queryMaxSeconds.Value = 3M;
                this.queryMaxSeconds.Enabled = false;
            }
            try
            {
                string installDir = TradeMagic.Cbi.Globals.InstallDir;
                Trace.Assert(System.IO.File.Exists(installDir + @"\Config.xml"), "TradeMagic is not installed properly, Config.xml file is missing");
                XmlDocument document = new XmlDocument();
                XmlTextReader reader = new XmlTextReader(installDir + @"\Config.xml");
                document.Load(reader);
                reader.Close();
                try
                {
                    this.userIDTextBox.Text = document["TradeMagic"]["User"]["Id"].InnerText;
                }
                catch (Exception)
                {
                }
                foreach (XmlNode node in document["TradeMagic"]["Vendor"]["VendorCodes"])
                {
                    this.vendorCodeComboBox.Items.Add(node.InnerText);
                }
                if (this.vendorCodeComboBox.Items.Count > 0)
                {
                    this.vendorCodeComboBox.Text = (string) this.vendorCodeComboBox.Items[0];
                }
            }
            catch (Exception)
            {
            }
            this.actionComboBox.Items.Add("Activity");
            this.actionComboBox.Items.Add("ChangePassword");
            this.actionComboBox.Items.Add("Check");
            this.actionComboBox.Items.Add("LicenseInfo");
            this.actionComboBox.Items.Add("LicenseSummary");
            this.actionComboBox.Items.Add("Report");
            this.actionComboBox.Items.Add("UpdateLicense");
            this.actionComboBox.SelectedItem = "Report";
            this.statusComboBox.Items.Add("Active");
            this.statusComboBox.Items.Add("Free");
            this.statusComboBox.SelectedItem = "Active";
            this.startDateTimePicker.Format = DateTimePickerFormat.Custom;
            this.startDateTimePicker.CustomFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
            this.endDateTimePicker.Format = DateTimePickerFormat.Custom;
            this.endDateTimePicker.CustomFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
            this.endDateTimePicker.Value = TradeMagic.Cbi.Globals.MaxDate;
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            if (base.WindowState == FormWindowState.Minimized)
            {
                base.Hide();
            }
        }

        private void manageButton_Click(object sender, EventArgs e)
        {
            char[] chArray;
            byte[] buffer;
            string str6;
            MD5 md = new MD5CryptoServiceProvider();
            string str = "";
            if (this.vendorCodeComboBox.Text.Length > 0)
            {
                chArray = this.vendorCodeComboBox.Text.ToCharArray();
                buffer = new byte[2 * this.vendorCodeComboBox.Text.Length];
                for (int j = 0; j < chArray.Length; j++)
                {
                    buffer[2 * j] = BitConverter.GetBytes(chArray[j])[0];
                    buffer[(2 * j) + 1] = BitConverter.GetBytes(chArray[j])[1];
                }
                str = BitConverter.ToString(md.ComputeHash(buffer)).Replace("-", "").ToLower();
            }
            chArray = this.passwordTextBox.Text.ToCharArray();
            buffer = new byte[2 * this.passwordTextBox.Text.Length];
            for (int i = 0; i < chArray.Length; i++)
            {
                buffer[2 * i] = BitConverter.GetBytes(chArray[i])[0];
                buffer[(2 * i) + 1] = BitConverter.GetBytes(chArray[i])[1];
            }
            string str2 = BitConverter.ToString(md.ComputeHash(buffer)).Replace("-", "").ToLower();
            string requestUriString = "http://www.trademagic.net/tools/ManageLicense.php?" + ((str.Length > 0) ? ("vc=" + str) : "") + "&pw=" + str2 + "&ac=" + this.actionComboBox.Text;
            if (this.actionComboBox.Text == "Activity")
            {
                str6 = requestUriString;
                requestUriString = str6 + "&fr=" + this.startDateTimePicker.Value.ToString("yyyy-MM-dd00:00:00") + "&to=" + this.endDateTimePicker.Value.ToString("yyyy-MM-dd23:59:59");
            }
            else if (this.actionComboBox.Text == "ChangePassword")
            {
                if (this.newPasswordTextBox.Text != this.retypePwdTextBox.Text)
                {
                    MessageBox.Show("Retyped new password does not match new password. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    this.retypePwdTextBox.Focus();
                }
                chArray = this.newPasswordTextBox.Text.ToCharArray();
                buffer = new byte[2 * this.newPasswordTextBox.Text.Length];
                for (int k = 0; k < chArray.Length; k++)
                {
                    buffer[2 * k] = BitConverter.GetBytes(chArray[k])[0];
                    buffer[(2 * k) + 1] = BitConverter.GetBytes(chArray[k])[1];
                }
                string str4 = BitConverter.ToString(md.ComputeHash(buffer)).Replace("-", "").ToLower();
                requestUriString = requestUriString + "&np=" + str4;
            }
            else if (this.actionComboBox.Text == "Check")
            {
                str6 = requestUriString;
                requestUriString = str6 + "&fr=" + this.startDateTimePicker.Value.ToString("yyyy-MM-dd00:00:00") + "&to=" + this.endDateTimePicker.Value.ToString("yyyy-MM-dd23:59:59");
            }
            else if (this.actionComboBox.Text == "LicenseInfo")
            {
                requestUriString = requestUriString + "&lc=" + this.licenseTextBox.Text;
            }
            else if (this.actionComboBox.Text != "LicenseSummary")
            {
                if (this.actionComboBox.Text == "Report")
                {
                    str6 = requestUriString;
                    requestUriString = str6 + "&fr=" + this.startDateTimePicker.Value.ToString("yyyy-MM-dd00:00:00") + "&to=" + this.endDateTimePicker.Value.ToString("yyyy-MM-dd23:59:59");
                }
                else if (this.actionComboBox.Text == "UpdateLicense")
                {
                    str6 = requestUriString;
                    requestUriString = str6 + "&lc=" + this.licenseTextBox.Text + "&fr=" + this.startDateTimePicker.Value.ToString("yyyy-MM-dd") + "&to=" + this.endDateTimePicker.Value.ToString("yyyy-MM-dd") + "&st=" + this.statusComboBox.Text;
                }
            }
            this.urlTextBox.Text = requestUriString;
            this.xmlTextBox.Text = "Processing. Please wait ...";
            Application.DoEvents();
            XmlDocument document = new XmlDocument();
            XmlTextReader reader = new XmlTextReader(TradeMagic.Cbi.Globals.InstallDir + @"\Config.xml");
            document.Load(reader);
            reader.Close();
            WebRequest request = WebRequest.Create(requestUriString);
            request.Timeout = 0x2710;
            WebResponse response = request.GetResponse();
            string str5 = new StreamReader(response.GetResponseStream()).ReadToEnd();
            this.xmlTextBox.Text = str5.Substring(0, str5.IndexOf("</TradeMagic>") + "</TradeMagic>".Length);
            response.Close();
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            base.Show();
            if (base.WindowState == FormWindowState.Minimized)
            {
                base.WindowState = FormWindowState.Normal;
            }
        }

        private static void RegisterDtn()
        {
            if (DtnInstallDir.Length > 0)
            {
                Trace.WriteLine("Existing DTN installation found. DTN components are not registered.");
            }
            else
            {
                Process process;
                foreach (string str in dtnDlls)
                {
                    process = new Process();
                    process.StartInfo.Arguments = "/s \"" + TradeMagic.Cbi.Globals.InstallDir + @"bin\Dtn\" + str + "\"";
                    process.StartInfo.FileName = "regsvr32.exe";
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.WorkingDirectory = TradeMagic.Cbi.Globals.InstallDir + @"bin\Dtn";
                    process.Start();
                    process.WaitForExit();
                    Trace.WriteLine("DTN dll '" + TradeMagic.Cbi.Globals.InstallDir + @"bin\Dtn\" + str + "' registered.");
                }
                process = new Process();
                process.StartInfo.Arguments = "/RegServer";
                process.StartInfo.FileName = TradeMagic.Cbi.Globals.InstallDir + @"bin\Dtn\IQFeed_Level2.exe";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.WorkingDirectory = TradeMagic.Cbi.Globals.InstallDir + @"bin\Dtn";
                process.Start();
                process.WaitForExit();
                Trace.WriteLine("DTN exe '" + TradeMagic.Cbi.Globals.InstallDir + @"bin\Dtn\IQFeed_Level2.exe' registered.");
            }
        }

        private void Start(int everyMinutes, int maxWaitSeconds)
        {
            if (this.timer != null)
            {
                this.timer.Close();
            }
            this.maxProcessSeconds = maxWaitSeconds;
            this.meanSeconds = 0.0;
            this.testCount = 0;
            this.timer = new System.Timers.Timer((everyMinutes * 1000.0) * 60.0);
            this.timer.Elapsed += new ElapsedEventHandler(this.timer_Elapsed);
            this.timer.Start();
            Trace.WriteLine(string.Concat(new object[] { "License test started. Execute every ", everyMinutes, " minutes, max wait ", maxWaitSeconds, " seconds" }));
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            this.Start((int) this.queryMinutes.Value, (int) this.queryMaxSeconds.Value);
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            TradeMagic.Cbi.Globals.UserId = this.userIDTextBox.Text;
            DateTime now = DateTime.Now;
            TradeMagic.Cbi.Globals.VendorCode = this.vendorCodeTextBox2.Text;
            if (new TradeMagic.Cbi.Connection().GetLicense("Misc").Id != LicenseTypeId.Professional)
            {
                Trace.WriteLine("Failed to get professional license");
            }
            double totalSeconds = DateTime.Now.Subtract(now).TotalSeconds;
            this.meanSeconds = ((this.meanSeconds * this.testCount) + totalSeconds) / ((double) (++this.testCount));
            if (totalSeconds <= this.maxProcessSeconds)
            {
                Trace.WriteLine("License check ok: " + totalSeconds.ToString("0.0") + "/" + this.meanSeconds.ToString("0.0"));
            }
            else
            {
                Trace.WriteLine(string.Concat(new object[] { "License check exceeded max time (", this.maxProcessSeconds, "): ", totalSeconds.ToString("0.0"), "/", this.meanSeconds.ToString("0.0") }));
            }
        }

        private static void UnregisterDtn()
        {
            if (DtnInstallDir.Length > 0)
            {
                Trace.WriteLine("Existing DTN installation found. DTN components are not unregistered.");
            }
            else
            {
                Process process;
                foreach (string str in dtnDlls)
                {
                    process = new Process();
                    process.StartInfo.Arguments = "/s /u \"" + TradeMagic.Cbi.Globals.InstallDir + @"bin\Dtn\" + str + "\"";
                    process.StartInfo.FileName = "regsvr32.exe";
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.WorkingDirectory = TradeMagic.Cbi.Globals.InstallDir + @"bin\Dtn";
                    process.Start();
                    process.WaitForExit();
                    Trace.WriteLine("DTN dll '" + TradeMagic.Cbi.Globals.InstallDir + @"bin\Dtn\" + str + "' unregistered.");
                }
                process = new Process();
                process.StartInfo.Arguments = "/UnregServer";
                process.StartInfo.FileName = TradeMagic.Cbi.Globals.InstallDir + @"bin\Dtn\IQFeed_Level2.exe";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.WorkingDirectory = TradeMagic.Cbi.Globals.InstallDir + @"bin\Dtn";
                process.Start();
                process.WaitForExit();
                Trace.WriteLine("DTN exe '" + TradeMagic.Cbi.Globals.InstallDir + @"bin\Dtn\IQFeed_Level2.exe' unregistered.");
            }
        }

        private static string DtnInstallDir
        {
            get
            {
                try
                {
                    RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE");
                    if (key != null)
                    {
                        RegistryKey key2 = key.OpenSubKey("DTN");
                        if (key2 != null)
                        {
                            key2 = key2.OpenSubKey("IQFeed");
                            if (key2 != null)
                            {
                                return (string) key2.GetValue("EXEDIR");
                            }
                        }
                    }
                }
                catch (Exception)
                {
                }
                return "";
            }
        }
    }
}

