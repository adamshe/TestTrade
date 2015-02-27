namespace iTrading.Gui
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Globalization;
    using System.Resources;
    using System.Windows.Forms;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Export recorded market data to a file.
    /// </summary>
    public class ExportMarketDepthForm : Form
    {
        private Container components = null;
        private TextBox fileTextBox;
        private DateTimePicker fromDateTimePicker;
        private Label label1;
        private Label label2;
        private Label label3;
        private Button okButton;
        private Button selectFileButton;
        private SelectSymbol selectSymbol;
        private DateTimePicker toDateTimePicker;

        /// <summary>
        /// </summary>
        public ExportMarketDepthForm()
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

        private void ExportMarketDepthForm_Load(object sender, EventArgs e)
        {
            this.fromDateTimePicker.Format = DateTimePickerFormat.Custom;
            this.fromDateTimePicker.CustomFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
            this.toDateTimePicker.Format = DateTimePickerFormat.Custom;
            this.toDateTimePicker.CustomFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
        }

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            ResourceManager manager = new ResourceManager(typeof(ExportMarketDepthForm));
            this.selectSymbol = new SelectSymbol();
            this.fromDateTimePicker = new DateTimePicker();
            this.label1 = new Label();
            this.label2 = new Label();
            this.toDateTimePicker = new DateTimePicker();
            this.label3 = new Label();
            this.fileTextBox = new TextBox();
            this.selectFileButton = new Button();
            this.okButton = new Button();
            base.SuspendLayout();
            this.selectSymbol.Connection = null;
            this.selectSymbol.Location = new Point(8, 0);
            this.selectSymbol.Name = "selectSymbol";
            this.selectSymbol.Size = new Size(0x1a0, 0x30);
            this.selectSymbol.Symbol = null;
            this.selectSymbol.TabIndex = 0;
            this.fromDateTimePicker.Location = new Point(0x88, 0x48);
            this.fromDateTimePicker.Name = "fromDateTimePicker";
            this.fromDateTimePicker.Size = new Size(0xb0, 20);
            this.fromDateTimePicker.TabIndex = 1;
            this.label1.Location = new Point(0x40, 0x48);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x30, 0x17);
            this.label1.TabIndex = 2;
            this.label1.Text = "From:";
            this.label1.TextAlign = ContentAlignment.MiddleLeft;
            this.label2.Location = new Point(0x40, 0x68);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x30, 0x17);
            this.label2.TabIndex = 4;
            this.label2.Text = "To:";
            this.label2.TextAlign = ContentAlignment.MiddleLeft;
            this.toDateTimePicker.Location = new Point(0x88, 0x68);
            this.toDateTimePicker.Name = "toDateTimePicker";
            this.toDateTimePicker.Size = new Size(0xb0, 20);
            this.toDateTimePicker.TabIndex = 3;
            this.label3.Location = new Point(0x40, 0x88);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x40, 0x17);
            this.label3.TabIndex = 5;
            this.label3.Text = "Export file:";
            this.label3.TextAlign = ContentAlignment.MiddleLeft;
            this.fileTextBox.Location = new Point(0x88, 0x88);
            this.fileTextBox.Name = "fileTextBox";
            this.fileTextBox.Size = new Size(0xb0, 20);
            this.fileTextBox.TabIndex = 6;
            this.fileTextBox.Text = "";
            this.selectFileButton.Location = new Point(320, 0x88);
            this.selectFileButton.Name = "selectFileButton";
            this.selectFileButton.Size = new Size(0x20, 20);
            this.selectFileButton.TabIndex = 7;
            this.selectFileButton.Text = "...";
            this.selectFileButton.Click += new EventHandler(this.selectFileButton_Click);
            this.okButton.Location = new Point(0xb0, 0xb8);
            this.okButton.Name = "okButton";
            this.okButton.TabIndex = 8;
            this.okButton.Text = "&Ok";
            this.okButton.Click += new EventHandler(this.okButton_Click);
            base.AcceptButton = this.okButton;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x1b2, 0xdf);
            base.Controls.Add(this.okButton);
            base.Controls.Add(this.selectFileButton);
            base.Controls.Add(this.fileTextBox);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.toDateTimePicker);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.fromDateTimePicker);
            base.Controls.Add(this.selectSymbol);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "ExportMarketDepthForm";
            base.ShowInTaskbar = false;
            this.Text = "Export market depth data";
            base.Load += new EventHandler(this.ExportMarketDepthForm_Load);
            base.ResumeLayout(false);
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (this.fromDateTimePicker.Value.Date > this.toDateTimePicker.Value.Date)
            {
                MessageBox.Show("'From' data must be smaller/equal 'To' date", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                this.fromDateTimePicker.Focus();
            }
            else if (this.fileTextBox.Text.Length == 0)
            {
                MessageBox.Show("Please select a file name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
                        symbol.MarketDepth.Dump(this.fromDateTimePicker.Value, this.toDateTimePicker.Value, this.fileTextBox.Text);
                        MessageBox.Show("Data successfully exported", "TradeMagic", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                    catch (Exception exception)
                    {
                        this.Connection.ProcessEventArgs(new ITradingErrorEventArgs(this.Connection, ErrorCode.Panic, "", exception.Message));
                    }
                    base.Close();
                }
            }
        }

        private void selectFileButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.CheckFileExists = false;
            dialog.InitialDirectory = @"c:\";
            dialog.RestoreDirectory = true;
            dialog.ShowDialog();
            if (dialog.FileName.Length != 0)
            {
                this.fileTextBox.Text = dialog.FileName;
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

