namespace iTrading.Gui
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Resources;
    using System.Windows.Forms;
    using iTrading.Core.Kernel;

    internal class MorePatsConnectOptionsForm : Form
    {
        private TextBox ApplicationId;
        private Button cancel;
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private Container components = null;
        private NumericUpDown enable;
        private TextBox HostAddress;
        private NumericUpDown hostHandShakeInterval;
        private NumericUpDown hostHandShakeTimeout;
        private NumericUpDown HostPort;
        private Label label1;
        private Label label10;
        private Label label11;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private TextBox LicenseKey;
        private Button ok;
        private TextBox PriceAddress;
        private NumericUpDown priceHandShakeInterval;
        private NumericUpDown priceHandShakeTimeout;
        private NumericUpDown PricePort;
        private CheckBox superTas;

        /// <summary></summary>
        public MorePatsConnectOptionsForm()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Initializes the Gui according the data value.
        /// </summary>
        /// <param name="options">Initial data values.</param>
        public void Data2Gui(PatsOptions options)
        {
            this.ApplicationId.Text = options.ApplicationId;
            this.enable.Value = options.Enable;
            this.HostAddress.Text = options.HostAddress;
            this.hostHandShakeInterval.Value = options.HostHandShakeInterval;
            this.hostHandShakeTimeout.Value = options.HostHandShakeTimeout;
            this.HostPort.Value = options.HostPort;
            this.LicenseKey.Text = options.LicenseKey;
            this.PriceAddress.Text = options.PriceAddress;
            this.priceHandShakeInterval.Value = options.PriceHandShakeInterval;
            this.priceHandShakeTimeout.Value = options.PriceHandShakeTimeout;
            this.PricePort.Value = options.PricePort;
            this.superTas.Checked = options.SuperTas;
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
        public void Gui2Data(PatsOptions options)
        {
            options.ApplicationId = this.ApplicationId.Text;
            options.Enable = (int) this.enable.Value;
            options.HostAddress = this.HostAddress.Text;
            options.HostHandShakeInterval = (int) this.hostHandShakeInterval.Value;
            options.HostHandShakeTimeout = (int) this.hostHandShakeTimeout.Value;
            options.HostPort = (int) this.HostPort.Value;
            options.LicenseKey = this.LicenseKey.Text;
            options.PriceAddress = this.PriceAddress.Text;
            options.PriceHandShakeInterval = (int) this.priceHandShakeInterval.Value;
            options.PriceHandShakeTimeout = (int) this.priceHandShakeTimeout.Value;
            options.PricePort = (int) this.PricePort.Value;
            options.SuperTas = this.superTas.Checked;
        }

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            ResourceManager manager = new ResourceManager(typeof(MorePatsConnectOptionsForm));
            this.label1 = new Label();
            this.HostAddress = new TextBox();
            this.label2 = new Label();
            this.HostPort = new NumericUpDown();
            this.ok = new Button();
            this.cancel = new Button();
            this.PricePort = new NumericUpDown();
            this.label3 = new Label();
            this.PriceAddress = new TextBox();
            this.label4 = new Label();
            this.ApplicationId = new TextBox();
            this.label5 = new Label();
            this.LicenseKey = new TextBox();
            this.label6 = new Label();
            this.superTas = new CheckBox();
            this.hostHandShakeInterval = new NumericUpDown();
            this.label7 = new Label();
            this.hostHandShakeTimeout = new NumericUpDown();
            this.label8 = new Label();
            this.priceHandShakeTimeout = new NumericUpDown();
            this.label9 = new Label();
            this.priceHandShakeInterval = new NumericUpDown();
            this.label10 = new Label();
            this.enable = new NumericUpDown();
            this.label11 = new Label();
            this.HostPort.BeginInit();
            this.PricePort.BeginInit();
            this.hostHandShakeInterval.BeginInit();
            this.hostHandShakeTimeout.BeginInit();
            this.priceHandShakeTimeout.BeginInit();
            this.priceHandShakeInterval.BeginInit();
            this.enable.BeginInit();
            base.SuspendLayout();
            this.label1.Location = new Point(0x18, 0x10);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x60, 0x17);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Host address:";
            this.label1.TextAlign = ContentAlignment.MiddleLeft;
            this.HostAddress.Location = new Point(120, 0x10);
            this.HostAddress.Name = "HostAddress";
            this.HostAddress.Size = new Size(0x68, 20);
            this.HostAddress.TabIndex = 1;
            this.HostAddress.Text = "";
            this.label2.Location = new Point(0x18, 0x30);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x58, 0x17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Host &port:";
            this.label2.TextAlign = ContentAlignment.MiddleLeft;
            this.HostPort.Location = new Point(120, 0x30);
            int[] bits = new int[4];
            bits[0] = 0x98967f;
            this.HostPort.Maximum = new decimal(bits);
            this.HostPort.Name = "HostPort";
            this.HostPort.Size = new Size(0x68, 20);
            this.HostPort.TabIndex = 2;
            this.ok.DialogResult = DialogResult.OK;
            this.ok.Location = new Point(0x58, 0xe0);
            this.ok.Name = "ok";
            this.ok.TabIndex = 12;
            this.ok.Text = "&Ok";
            this.ok.Click += new EventHandler(this.Ok_Click);
            this.cancel.DialogResult = DialogResult.Cancel;
            this.cancel.Location = new Point(0x100, 0xe0);
            this.cancel.Name = "cancel";
            this.cancel.TabIndex = 13;
            this.cancel.Text = "&Cancel";
            this.PricePort.Location = new Point(120, 0x70);
            bits = new int[4];
            bits[0] = 0x98967f;
            this.PricePort.Maximum = new decimal(bits);
            this.PricePort.Name = "PricePort";
            this.PricePort.Size = new Size(0x68, 20);
            this.PricePort.TabIndex = 4;
            this.label3.Location = new Point(0x18, 0x70);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x58, 0x17);
            this.label3.TabIndex = 10;
            this.label3.Text = "Price po&rt:";
            this.label3.TextAlign = ContentAlignment.MiddleLeft;
            this.PriceAddress.Location = new Point(120, 80);
            this.PriceAddress.Name = "PriceAddress";
            this.PriceAddress.Size = new Size(0x68, 20);
            this.PriceAddress.TabIndex = 3;
            this.PriceAddress.Text = "";
            this.label4.Location = new Point(0x18, 80);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x60, 0x17);
            this.label4.TabIndex = 8;
            this.label4.Text = "Price &address:";
            this.label4.TextAlign = ContentAlignment.MiddleLeft;
            this.ApplicationId.Location = new Point(120, 0x90);
            this.ApplicationId.Name = "ApplicationId";
            this.ApplicationId.Size = new Size(0x68, 20);
            this.ApplicationId.TabIndex = 5;
            this.ApplicationId.Text = "";
            this.label5.Location = new Point(0x18, 0x90);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x60, 0x17);
            this.label5.TabIndex = 12;
            this.label5.Text = "Application ID:";
            this.label5.TextAlign = ContentAlignment.MiddleLeft;
            this.LicenseKey.Location = new Point(120, 0xb0);
            this.LicenseKey.Name = "LicenseKey";
            this.LicenseKey.Size = new Size(0x68, 20);
            this.LicenseKey.TabIndex = 6;
            this.LicenseKey.Text = "";
            this.label6.Location = new Point(0x18, 0xb0);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x60, 0x17);
            this.label6.TabIndex = 14;
            this.label6.Text = "License key:";
            this.label6.TextAlign = ContentAlignment.MiddleLeft;
            this.superTas.Location = new Point(0x100, 0xb0);
            this.superTas.Name = "superTas";
            this.superTas.RightToLeft = RightToLeft.Yes;
            this.superTas.Size = new Size(0x60, 0x18);
            this.superTas.TabIndex = 12;
            this.superTas.Text = "Super TAS";
            this.superTas.TextAlign = ContentAlignment.MiddleRight;
            this.hostHandShakeInterval.Location = new Point(0x150, 0x10);
            bits = new int[4];
            bits[0] = 900;
            this.hostHandShakeInterval.Maximum = new decimal(bits);
            this.hostHandShakeInterval.Name = "hostHandShakeInterval";
            this.hostHandShakeInterval.Size = new Size(0x38, 20);
            this.hostHandShakeInterval.TabIndex = 7;
            this.label7.Location = new Point(0x100, 0x10);
            this.label7.Name = "label7";
            this.label7.Size = new Size(0x48, 0x17);
            this.label7.TabIndex = 15;
            this.label7.Text = "Host interval:";
            this.label7.TextAlign = ContentAlignment.MiddleLeft;
            this.hostHandShakeTimeout.Location = new Point(0x150, 0x30);
            bits = new int[4];
            bits[0] = 0x708;
            this.hostHandShakeTimeout.Maximum = new decimal(bits);
            this.hostHandShakeTimeout.Name = "hostHandShakeTimeout";
            this.hostHandShakeTimeout.Size = new Size(0x38, 20);
            this.hostHandShakeTimeout.TabIndex = 8;
            this.label8.Location = new Point(0x100, 0x30);
            this.label8.Name = "label8";
            this.label8.Size = new Size(0x48, 0x17);
            this.label8.TabIndex = 0x11;
            this.label8.Text = "Host timeout:";
            this.label8.TextAlign = ContentAlignment.MiddleLeft;
            this.priceHandShakeTimeout.Location = new Point(0x150, 0x70);
            bits = new int[4];
            bits[0] = 0x708;
            this.priceHandShakeTimeout.Maximum = new decimal(bits);
            this.priceHandShakeTimeout.Name = "priceHandShakeTimeout";
            this.priceHandShakeTimeout.Size = new Size(0x38, 20);
            this.priceHandShakeTimeout.TabIndex = 10;
            this.label9.Location = new Point(0x100, 0x70);
            this.label9.Name = "label9";
            this.label9.Size = new Size(80, 0x17);
            this.label9.TabIndex = 0x15;
            this.label9.Text = "Price timeout:";
            this.label9.TextAlign = ContentAlignment.MiddleLeft;
            this.priceHandShakeInterval.Location = new Point(0x150, 80);
            bits = new int[4];
            bits[0] = 900;
            this.priceHandShakeInterval.Maximum = new decimal(bits);
            this.priceHandShakeInterval.Name = "priceHandShakeInterval";
            this.priceHandShakeInterval.Size = new Size(0x38, 20);
            this.priceHandShakeInterval.TabIndex = 9;
            this.label10.Location = new Point(0x100, 80);
            this.label10.Name = "label10";
            this.label10.Size = new Size(80, 0x17);
            this.label10.TabIndex = 0x13;
            this.label10.Text = "Price interval:";
            this.label10.TextAlign = ContentAlignment.MiddleLeft;
            this.enable.Location = new Point(0x150, 0x90);
            bits = new int[4];
            bits[0] = 0xff;
            this.enable.Maximum = new decimal(bits);
            this.enable.Name = "enable";
            this.enable.Size = new Size(0x38, 20);
            this.enable.TabIndex = 11;
            this.label11.Location = new Point(0x100, 0x90);
            this.label11.Name = "label11";
            this.label11.Size = new Size(80, 0x17);
            this.label11.TabIndex = 0x17;
            this.label11.Text = "Enable:";
            this.label11.TextAlign = ContentAlignment.MiddleLeft;
            base.AcceptButton = this.ok;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this.cancel;
            base.ClientSize = new Size(0x1a2, 0x107);
            base.Controls.Add(this.enable);
            base.Controls.Add(this.label11);
            base.Controls.Add(this.priceHandShakeTimeout);
            base.Controls.Add(this.label9);
            base.Controls.Add(this.priceHandShakeInterval);
            base.Controls.Add(this.label10);
            base.Controls.Add(this.hostHandShakeTimeout);
            base.Controls.Add(this.label8);
            base.Controls.Add(this.hostHandShakeInterval);
            base.Controls.Add(this.label7);
            base.Controls.Add(this.superTas);
            base.Controls.Add(this.LicenseKey);
            base.Controls.Add(this.ApplicationId);
            base.Controls.Add(this.PriceAddress);
            base.Controls.Add(this.HostAddress);
            base.Controls.Add(this.label6);
            base.Controls.Add(this.label5);
            base.Controls.Add(this.PricePort);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.cancel);
            base.Controls.Add(this.ok);
            base.Controls.Add(this.HostPort);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.FormBorderStyle = FormBorderStyle.FixedSingle;
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "MorePatsConnectOptionsForm";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "More Patsystems connect options";
            this.HostPort.EndInit();
            this.PricePort.EndInit();
            this.hostHandShakeInterval.EndInit();
            this.hostHandShakeTimeout.EndInit();
            this.priceHandShakeTimeout.EndInit();
            this.priceHandShakeInterval.EndInit();
            this.enable.EndInit();
            base.ResumeLayout(false);
        }

        private void Ok_Click(object sender, EventArgs e)
        {
            base.Close();
        }
    }
}

