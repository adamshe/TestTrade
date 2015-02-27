namespace iTrading.Gui
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Opens a connection. Assign a connection (<see cref="P:iTrading.Gui.ConnectForm.Connection" />) before loading this form.
    /// </summary>
    public class ConnectForm : Form
    {
        private Button cancel;
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private Container components = null;
        private Connection connection = null;
        private iTrading.Gui.ConnectOptions ConnectOptions;
        private Button ok;

        /// <summary></summary>
        public ConnectForm()
        {
            this.InitializeComponent();
        }

        private void ConnectForm_Closing(object sender, CancelEventArgs e)
        {
            this.connection.ConnectionStatus -= new ConnectionStatusEventHandler(this.ConnectionStatus);
        }

        private void ConnectionStatus(object sender, ConnectionStatusEventArgs e)
        {
            if (e.Connection.ConnectionStatusId == ConnectionStatusId.Connected)
            {
                base.Close();
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

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.ConnectOptions = new iTrading.Gui.ConnectOptions();
            this.ok = new Button();
            this.cancel = new Button();
            base.SuspendLayout();
            this.ConnectOptions.Location = new Point(0x10, 8);
            this.ConnectOptions.Name = "ConnectOptions";
            this.ConnectOptions.Size = new Size(0xd0, 0x98);
            this.ConnectOptions.TabIndex = 0;
            this.ok.DialogResult = DialogResult.OK;
            this.ok.Location = new Point(0x20, 0xb0);
            this.ok.Name = "ok";
            this.ok.TabIndex = 1;
            this.ok.Text = "&Ok";
            this.ok.Click += new EventHandler(this.Ok_Click);
            this.cancel.DialogResult = DialogResult.Cancel;
            this.cancel.Location = new Point(0x88, 0xb0);
            this.cancel.Name = "cancel";
            this.cancel.TabIndex = 2;
            this.cancel.Text = "&Cancel";
            base.AcceptButton = this.ok;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this.cancel;
            base.ClientSize = new Size(0xf2, 0xcf);
            base.Controls.Add(this.cancel);
            base.Controls.Add(this.ok);
            base.Controls.Add(this.ConnectOptions);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "ConnectForm";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "TradeMagic connect";
            base.Closing += new CancelEventHandler(this.ConnectForm_Closing);
            base.ResumeLayout(false);
        }

        private void Ok_Click(object sender, EventArgs e)
        {
            base.Enabled = false;
            Application.DoEvents();
            this.connection.ConnectionStatus += new ConnectionStatusEventHandler(this.ConnectionStatus);
            OptionsBase options = this.ConnectOptions.Gui2Data();
            options.Save();
            this.connection.Connect(options);
        }

        /// <summary>
        /// Gets/sets the established connection. Call <see cref="P:Connection.ConnectionStatusId" /> to check
        /// that the connection is established.
        /// Set the connection before this control is loaded.
        /// </summary>
        public Connection Connection
        {
            get
            {
                return this.connection;
            }
            set
            {
                this.connection = value;
            }
        }
    }
}

