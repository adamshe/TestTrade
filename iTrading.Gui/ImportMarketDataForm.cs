namespace iTrading.Gui
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Import market data.
    /// </summary>
    public class ImportMarketDataForm : Form
    {
        private Container components = null;
        private Button fileButton;
        private TextBox fileTextBox;
        private Label label1;
        private Button okButton;
        private SelectSymbol selectSymbol;

        /// <summary>
        /// </summary>
        public ImportMarketDataForm()
        {
            this.InitializeComponent();
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

        private void fileButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = @"c:\";
            dialog.RestoreDirectory = true;
            dialog.ShowDialog();
            if (dialog.FileName.Length != 0)
            {
                this.fileTextBox.Text = dialog.FileName;
            }
        }

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new Label();
            this.fileTextBox = new TextBox();
            this.fileButton = new Button();
            this.okButton = new Button();
            this.selectSymbol = new SelectSymbol();
            base.SuspendLayout();
            this.label1.Location = new Point(0x48, 80);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x40, 0x17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Import file:";
            this.label1.TextAlign = ContentAlignment.MiddleLeft;
            this.fileTextBox.Location = new Point(0x88, 80);
            this.fileTextBox.Name = "fileTextBox";
            this.fileTextBox.Size = new Size(0x98, 20);
            this.fileTextBox.TabIndex = 1;
            this.fileTextBox.Text = "";
            this.fileButton.Location = new Point(0x128, 80);
            this.fileButton.Name = "fileButton";
            this.fileButton.Size = new Size(0x20, 20);
            this.fileButton.TabIndex = 2;
            this.fileButton.Text = "...";
            this.fileButton.Click += new EventHandler(this.fileButton_Click);
            this.okButton.Location = new Point(0xb0, 120);
            this.okButton.Name = "okButton";
            this.okButton.TabIndex = 3;
            this.okButton.Text = "&Ok";
            this.okButton.Click += new EventHandler(this.okButton_Click);
            this.selectSymbol.Connection = null;
            this.selectSymbol.Location = new Point(8, 8);
            this.selectSymbol.Name = "selectSymbol";
            this.selectSymbol.Size = new Size(0x1a0, 0x30);
            this.selectSymbol.Symbol = null;
            this.selectSymbol.TabIndex = 0;
            base.AcceptButton = this.okButton;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x1b2, 0x9f);
            base.Controls.Add(this.selectSymbol);
            base.Controls.Add(this.okButton);
            base.Controls.Add(this.fileButton);
            base.Controls.Add(this.fileTextBox);
            base.Controls.Add(this.label1);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "ImportMarketDataForm";
            base.ShowInTaskbar = false;
            this.Text = "Import market data";
            base.ResumeLayout(false);
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (this.fileTextBox.Text.Length == 0)
            {
                MessageBox.Show("Please select a file name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                this.fileTextBox.Focus();
            }
            else if (!File.Exists(this.fileTextBox.Text))
            {
                MessageBox.Show("File '" + this.fileTextBox.Text + "' does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                this.fileTextBox.Focus();
            }
            else
            {
                Symbol symbol = this.selectSymbol.Symbol;
                if (symbol == null)
                {
                    MessageBox.Show("Please select a valid symbol", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    this.selectSymbol.Focus();
                }
                else
                {
                    try
                    {
                        symbol.MarketData.Load(this.fileTextBox.Text);
                        MessageBox.Show("Data successfully imported", "TradeMagic", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                    catch (Exception exception)
                    {
                        this.Connection.ProcessEventArgs(new ITradingErrorEventArgs(this.Connection, ErrorCode.Panic, "", exception.Message));
                    }
                    base.Close();
                }
            }
        }

        /// <summary>
        /// Get/set the connection to export market data. Set the connection before this form is loaded.
        /// </summary>
        public Connection Connection
        {
            get
            {
                return this.selectSymbol.Connection;
            }
            set
            {
                this.selectSymbol.Connection = value;
            }
        }
    }
}

