namespace iTrading.Gui
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Resources;
    using System.Windows.Forms;
    using iTrading.Core.Kernel;

    internal class MoreConnectionOptionsForm : Form
    {
        private Button cancel;
        private CheckBox CheckForMarketData;
        private NumericUpDown clientId;
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private Container components = null;
        private CheckBox connectToRunningTws;
        private TextBox host;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private NumericUpDown logLevel;
        private NumericUpDown maxMarketDataStreams;
        private Button ok;
        private NumericUpDown port;
        private CheckBox UseSsl;
        private CheckBox UseUserSettings;
        private NumericUpDown waitMilliSecondsRequest;

        /// <summary></summary>
        public MoreConnectionOptionsForm()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Initializes the Gui according the data value.
        /// </summary>
        /// <param name="options">Initial data values.</param>
        public void Data2Gui(IBOptions options)
        {
            this.CheckForMarketData.Checked = options.CheckForMarketData;
            this.clientId.Value = options.ClientId;
            this.connectToRunningTws.Checked = options.Connect2RunningTws;
            this.host.Text = options.Host;
            this.logLevel.Value = (decimal) options.LogLevel;
            this.maxMarketDataStreams.Value = options.MaxMarketDataStreams;
            this.port.Value = options.Port;
            this.UseUserSettings.Checked = options.UseUserSettings;
            this.UseSsl.Checked = options.UseSsl;
            this.waitMilliSecondsRequest.Value = options.WaitMilliSecondsRequest;
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
        /// Returns the data values according the current Gui settings.
        /// </summary>
        public void Gui2Data(IBOptions options)
        {
            options.CheckForMarketData = this.CheckForMarketData.Checked;
            options.ClientId = (int) this.clientId.Value;
            options.Connect2RunningTws = this.connectToRunningTws.Checked;
            options.Host = this.host.Text;
            options.LogLevel = (IBLogLevel) ((int) this.logLevel.Value);
            options.MaxMarketDataStreams = (int) this.maxMarketDataStreams.Value;
            options.Port = (int) this.port.Value;
            options.UseUserSettings = this.UseUserSettings.Checked;
            options.UseSsl = this.UseSsl.Checked;
            options.WaitMilliSecondsRequest = (int) this.waitMilliSecondsRequest.Value;
        }

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            ResourceManager manager = new ResourceManager(typeof(MoreConnectionOptionsForm));
            this.label1 = new Label();
            this.host = new TextBox();
            this.label2 = new Label();
            this.port = new NumericUpDown();
            this.clientId = new NumericUpDown();
            this.label3 = new Label();
            this.ok = new Button();
            this.cancel = new Button();
            this.UseSsl = new CheckBox();
            this.connectToRunningTws = new CheckBox();
            this.logLevel = new NumericUpDown();
            this.label4 = new Label();
            this.CheckForMarketData = new CheckBox();
            this.maxMarketDataStreams = new NumericUpDown();
            this.label5 = new Label();
            this.label6 = new Label();
            this.waitMilliSecondsRequest = new NumericUpDown();
            this.UseUserSettings = new CheckBox();
            this.port.BeginInit();
            this.clientId.BeginInit();
            this.logLevel.BeginInit();
            this.maxMarketDataStreams.BeginInit();
            this.waitMilliSecondsRequest.BeginInit();
            base.SuspendLayout();
            this.label1.Location = new Point(0x18, 0x10);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x38, 0x17);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Host:";
            this.label1.TextAlign = ContentAlignment.MiddleLeft;
            this.host.Location = new Point(0x68, 0x10);
            this.host.Name = "host";
            this.host.TabIndex = 1;
            this.host.Text = "localhost";
            this.label2.Location = new Point(0x18, 0x30);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x38, 0x17);
            this.label2.TabIndex = 2;
            this.label2.Text = "&Port:";
            this.label2.TextAlign = ContentAlignment.MiddleLeft;
            this.port.Location = new Point(0x68, 0x30);
            int[] bits = new int[4];
            bits[0] = 0x98967f;
            this.port.Maximum = new decimal(bits);
            this.port.Name = "port";
            this.port.Size = new Size(0x68, 20);
            this.port.TabIndex = 2;
            this.clientId.Location = new Point(0x68, 80);
            bits = new int[4];
            bits[0] = 0x540be3ff;
            bits[1] = 2;
            this.clientId.Maximum = new decimal(bits);
            this.clientId.Name = "clientId";
            this.clientId.Size = new Size(0x68, 20);
            this.clientId.TabIndex = 3;
            this.label3.Location = new Point(0x18, 80);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x38, 0x17);
            this.label3.TabIndex = 4;
            this.label3.Text = "&Client ID:";
            this.label3.TextAlign = ContentAlignment.MiddleLeft;
            this.ok.DialogResult = DialogResult.OK;
            this.ok.Location = new Point(120, 0xb8);
            this.ok.Name = "ok";
            this.ok.TabIndex = 11;
            this.ok.Text = "&Ok";
            this.ok.Click += new EventHandler(this.Ok_Click);
            this.cancel.DialogResult = DialogResult.Cancel;
            this.cancel.Location = new Point(0x100, 0xb8);
            this.cancel.Name = "cancel";
            this.cancel.TabIndex = 12;
            this.cancel.Text = "&Cancel";
            this.UseSsl.Location = new Point(240, 0x70);
            this.UseSsl.Name = "UseSsl";
            this.UseSsl.RightToLeft = RightToLeft.Yes;
            this.UseSsl.Size = new Size(0x98, 0x18);
            this.UseSsl.TabIndex = 9;
            this.UseSsl.Text = "&Use SSL";
            this.UseSsl.TextAlign = ContentAlignment.MiddleRight;
            this.connectToRunningTws.Checked = true;
            this.connectToRunningTws.CheckState = CheckState.Checked;
            this.connectToRunningTws.Location = new Point(240, 80);
            this.connectToRunningTws.Name = "connectToRunningTws";
            this.connectToRunningTws.RightToLeft = RightToLeft.Yes;
            this.connectToRunningTws.Size = new Size(0x98, 0x18);
            this.connectToRunningTws.TabIndex = 8;
            this.connectToRunningTws.Text = "&Use running TWS";
            this.connectToRunningTws.TextAlign = ContentAlignment.MiddleRight;
            this.logLevel.Location = new Point(0x68, 0x70);
            bits = new int[4];
            bits[0] = 5;
            this.logLevel.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 1;
            this.logLevel.Minimum = new decimal(bits);
            this.logLevel.Name = "logLevel";
            this.logLevel.Size = new Size(0x68, 20);
            this.logLevel.TabIndex = 4;
            bits = new int[4];
            bits[0] = 2;
            this.logLevel.Value = new decimal(bits);
            this.label4.Location = new Point(0x18, 0x70);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x38, 0x17);
            this.label4.TabIndex = 9;
            this.label4.Text = "&Log level:";
            this.label4.TextAlign = ContentAlignment.MiddleLeft;
            this.CheckForMarketData.Location = new Point(240, 0x90);
            this.CheckForMarketData.Name = "CheckForMarketData";
            this.CheckForMarketData.RightToLeft = RightToLeft.Yes;
            this.CheckForMarketData.Size = new Size(0x98, 0x18);
            this.CheckForMarketData.TabIndex = 10;
            this.CheckForMarketData.Text = "&Check for market data";
            this.CheckForMarketData.TextAlign = ContentAlignment.MiddleRight;
            this.maxMarketDataStreams.Location = new Point(0x178, 0x10);
            bits = new int[4];
            bits[0] = 0x3e8;
            this.maxMarketDataStreams.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 1;
            this.maxMarketDataStreams.Minimum = new decimal(bits);
            this.maxMarketDataStreams.Name = "maxMarketDataStreams";
            this.maxMarketDataStreams.Size = new Size(0x30, 20);
            this.maxMarketDataStreams.TabIndex = 6;
            bits = new int[4];
            bits[0] = 40;
            this.maxMarketDataStreams.Value = new decimal(bits);
            this.label5.Location = new Point(240, 0x10);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x88, 0x17);
            this.label5.TabIndex = 11;
            this.label5.Text = "Ma&x market data streams";
            this.label5.TextAlign = ContentAlignment.MiddleLeft;
            this.label6.Location = new Point(240, 0x30);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x88, 0x17);
            this.label6.TabIndex = 13;
            this.label6.Text = "&Min request delay (ms):";
            this.label6.TextAlign = ContentAlignment.MiddleLeft;
            this.waitMilliSecondsRequest.Location = new Point(0x178, 0x30);
            bits = new int[4];
            bits[0] = 0x3e8;
            this.waitMilliSecondsRequest.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 40;
            this.waitMilliSecondsRequest.Minimum = new decimal(bits);
            this.waitMilliSecondsRequest.Name = "waitMilliSecondsRequest";
            this.waitMilliSecondsRequest.Size = new Size(0x30, 20);
            this.waitMilliSecondsRequest.TabIndex = 7;
            bits = new int[4];
            bits[0] = 40;
            this.waitMilliSecondsRequest.Value = new decimal(bits);
            this.UseUserSettings.Location = new Point(0x18, 0x90);
            this.UseUserSettings.Name = "UseUserSettings";
            this.UseUserSettings.RightToLeft = RightToLeft.Yes;
            this.UseUserSettings.Size = new Size(0x80, 0x18);
            this.UseUserSettings.TabIndex = 5;
            this.UseUserSettings.Text = "U&ser user settings";
            this.UseUserSettings.TextAlign = ContentAlignment.MiddleRight;
            base.AcceptButton = this.ok;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this.cancel;
            base.ClientSize = new Size(0x1ba, 0xdf);
            base.Controls.Add(this.UseUserSettings);
            base.Controls.Add(this.label6);
            base.Controls.Add(this.waitMilliSecondsRequest);
            base.Controls.Add(this.label5);
            base.Controls.Add(this.maxMarketDataStreams);
            base.Controls.Add(this.CheckForMarketData);
            base.Controls.Add(this.logLevel);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.connectToRunningTws);
            base.Controls.Add(this.UseSsl);
            base.Controls.Add(this.cancel);
            base.Controls.Add(this.ok);
            base.Controls.Add(this.clientId);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.port);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.host);
            base.Controls.Add(this.label1);
            base.FormBorderStyle = FormBorderStyle.FixedSingle;
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "MoreConnectionOptionsForm";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "More connect options";
            this.port.EndInit();
            this.clientId.EndInit();
            this.logLevel.EndInit();
            this.maxMarketDataStreams.EndInit();
            this.waitMilliSecondsRequest.EndInit();
            base.ResumeLayout(false);
        }

        private void Ok_Click(object sender, EventArgs e)
        {
            base.Close();
        }
    }
}

