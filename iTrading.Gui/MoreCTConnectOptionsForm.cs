namespace iTrading.Gui
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Resources;
    using System.Windows.Forms;
    using iTrading.Core.Kernel;

    internal class MoreCTConnectOptionsForm : Form
    {
        private TextBox AuthorizationCode;
        private Button cancel;
        private Container components = null;
        private TextBox IPAddress;
        private TextBox IPAddressAlternate;
        private Label label1;
        private Label label4;
        private Label label6;
        private Button ok;

        /// <summary></summary>
        public MoreCTConnectOptionsForm()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Initializes the Gui according the data value.
        /// </summary>
        /// <param name="options">Initial data values.</param>
        public void Data2Gui(CTOptions options)
        {
            this.AuthorizationCode.Text = options.AuthorizationCode;
            this.IPAddress.Text = options.IPAddress;
            this.IPAddressAlternate.Text = options.IPAddressAlternate;
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
        public void Gui2Data(CTOptions options)
        {
            options.AuthorizationCode = this.AuthorizationCode.Text;
            options.IPAddress = this.IPAddress.Text;
            options.IPAddressAlternate = this.IPAddressAlternate.Text;
        }

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            ResourceManager manager = new ResourceManager(typeof(MoreCTConnectOptionsForm));
            this.label1 = new Label();
            this.IPAddress = new TextBox();
            this.ok = new Button();
            this.cancel = new Button();
            this.IPAddressAlternate = new TextBox();
            this.label4 = new Label();
            this.AuthorizationCode = new TextBox();
            this.label6 = new Label();
            base.SuspendLayout();
            this.label1.Location = new Point(0x18, 0x40);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x73, 0x1b);
            this.label1.TabIndex = 0;
            this.label1.Text = "&IP address:";
            this.label1.TextAlign = ContentAlignment.MiddleLeft;
            this.IPAddress.Location = new Point(160, 0x40);
            this.IPAddress.Name = "IPAddress";
            this.IPAddress.Size = new Size(0x7d, 0x16);
            this.IPAddress.TabIndex = 2;
            this.IPAddress.Text = "";
            this.ok.DialogResult = DialogResult.OK;
            this.ok.Location = new Point(40, 160);
            this.ok.Name = "ok";
            this.ok.Size = new Size(90, 0x1b);
            this.ok.TabIndex = 12;
            this.ok.Text = "&Ok";
            this.ok.Click += new EventHandler(this.Ok_Click);
            this.cancel.DialogResult = DialogResult.Cancel;
            this.cancel.Location = new Point(0xb0, 160);
            this.cancel.Name = "cancel";
            this.cancel.Size = new Size(90, 0x1b);
            this.cancel.TabIndex = 13;
            this.cancel.Text = "&Cancel";
            this.IPAddressAlternate.Location = new Point(160, 0x68);
            this.IPAddressAlternate.Name = "IPAddressAlternate";
            this.IPAddressAlternate.Size = new Size(0x7d, 0x16);
            this.IPAddressAlternate.TabIndex = 3;
            this.IPAddressAlternate.Text = "";
            this.label4.Location = new Point(0x18, 0x68);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x73, 0x1b);
            this.label4.TabIndex = 8;
            this.label4.Text = "Alt. IP a&ddress:";
            this.label4.TextAlign = ContentAlignment.MiddleLeft;
            this.AuthorizationCode.Location = new Point(160, 0x18);
            this.AuthorizationCode.Name = "AuthorizationCode";
            this.AuthorizationCode.Size = new Size(0x7d, 0x16);
            this.AuthorizationCode.TabIndex = 1;
            this.AuthorizationCode.Text = "";
            this.label6.Location = new Point(0x18, 0x18);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x88, 0x1b);
            this.label6.TabIndex = 14;
            this.label6.Text = "&Authorization code:";
            this.label6.TextAlign = ContentAlignment.MiddleLeft;
            base.AcceptButton = this.ok;
            this.AutoScaleBaseSize = new Size(6, 15);
            base.CancelButton = this.cancel;
            base.ClientSize = new Size(0x13a, 0xcd);
            base.Controls.Add(this.AuthorizationCode);
            base.Controls.Add(this.IPAddressAlternate);
            base.Controls.Add(this.IPAddress);
            base.Controls.Add(this.label6);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.cancel);
            base.Controls.Add(this.ok);
            base.Controls.Add(this.label1);
            base.FormBorderStyle = FormBorderStyle.FixedSingle;
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "MoreCTConnectOptionsForm";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "CyberTrader options";
            base.ResumeLayout(false);
        }

        private void Ok_Click(object sender, EventArgs e)
        {
            base.Close();
        }
    }
}

