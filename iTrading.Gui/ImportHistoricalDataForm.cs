namespace iTrading.Gui
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Resources;
    using System.Windows.Forms;
    using iTrading.Core.Kernel;
    using iTrading.Core.Data;

    /// <summary>
    /// Form to import historical data.
    /// </summary>
    public class ImportHistoricalDataForm : Form
    {
        private Container components = null;
        private TextBox fileTextBox;
        private Button importButton;
        private Label label3;
        private Label label5;
        private ComboBox period;
        private Button selectFileButton;
        private SelectSymbol selectSymbol;

        /// <summary></summary>
        public ImportHistoricalDataForm()
        {
            this.InitializeComponent();
        }

        /// <summary></summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void HistoricalDataForm_Load(object sender, EventArgs e)
        {
            foreach (PeriodType type in PeriodType.All.Values)
            {
                if ((type.Id == PeriodTypeId.Day) && (this.selectSymbol.Connection.FeatureTypes[FeatureTypeId.QuotesDaily] != null))
                {
                    this.period.Items.Add(type.Name);
                }
                else
                {
                    if ((type.Id == PeriodTypeId.Minute) && (this.selectSymbol.Connection.FeatureTypes[FeatureTypeId.Quotes1Minute] != null))
                    {
                        this.period.Items.Add(type.Name);
                        continue;
                    }
                    if ((type.Id == PeriodTypeId.Tick) && (this.selectSymbol.Connection.FeatureTypes[FeatureTypeId.QuotesTick] != null))
                    {
                        this.period.Items.Add(type.Name);
                    }
                }
            }
            this.period.SelectedIndex = 0;
        }

        private void importButton_Click(object sender, EventArgs e)
        {
            Symbol symbol = this.selectSymbol.Symbol;
            if (symbol == null)
            {
                MessageBox.Show("Not a valid symbol", "TradeMagic", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else if (this.fileTextBox.Text.Length == 0)
            {
                MessageBox.Show("Please enter file name", "TradeMagic", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else
            {
                PeriodType type = PeriodType.All.Find((string) this.period.SelectedItem);
                Trace.Assert(type != null, "Gui.ImportHistoricalDataForm.importButton_Click");
                try
                {
                    symbol.LoadQuotes(this.fileTextBox.Text, ' ', type.Id);
                    MessageBox.Show("Data successfully imported", "TradeMagic", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, "TradeMagic", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            ResourceManager manager = new ResourceManager(typeof(ImportHistoricalDataForm));
            this.selectSymbol = new SelectSymbol();
            this.importButton = new Button();
            this.selectFileButton = new Button();
            this.fileTextBox = new TextBox();
            this.label5 = new Label();
            this.label3 = new Label();
            this.period = new ComboBox();
            base.SuspendLayout();
            this.selectSymbol.Connection = null;
            this.selectSymbol.Location = new Point(8, 8);
            this.selectSymbol.Name = "selectSymbol";
            this.selectSymbol.Size = new Size(0x1a0, 0x30);
            this.selectSymbol.Symbol = null;
            this.selectSymbol.TabIndex = 0;
            this.importButton.Location = new Point(0xa8, 0x90);
            this.importButton.Name = "importButton";
            this.importButton.TabIndex = 5;
            this.importButton.Text = "&Import";
            this.importButton.Click += new EventHandler(this.importButton_Click);
            this.selectFileButton.Location = new Point(0x148, 0x68);
            this.selectFileButton.Name = "selectFileButton";
            this.selectFileButton.Size = new Size(0x20, 20);
            this.selectFileButton.TabIndex = 4;
            this.selectFileButton.Text = "...";
            this.selectFileButton.Click += new EventHandler(this.selectFileButton_Click);
            this.fileTextBox.Location = new Point(0x68, 0x68);
            this.fileTextBox.Name = "fileTextBox";
            this.fileTextBox.Size = new Size(0xd8, 20);
            this.fileTextBox.TabIndex = 3;
            this.fileTextBox.Text = "";
            this.label5.Location = new Point(40, 0x68);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x40, 0x17);
            this.label5.TabIndex = 10;
            this.label5.Text = "Import &file:";
            this.label5.TextAlign = ContentAlignment.MiddleLeft;
            this.label3.Location = new Point(40, 0x48);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x30, 0x17);
            this.label3.TabIndex = 12;
            this.label3.Text = "&Period:";
            this.label3.TextAlign = ContentAlignment.MiddleLeft;
            this.period.DropDownStyle = ComboBoxStyle.DropDownList;
            this.period.Location = new Point(0x68, 0x48);
            this.period.Name = "period";
            this.period.Size = new Size(80, 0x15);
            this.period.Sorted = true;
            this.period.TabIndex = 2;
            base.AcceptButton = this.importButton;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x1b2, 0xb7);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.period);
            base.Controls.Add(this.selectFileButton);
            base.Controls.Add(this.fileTextBox);
            base.Controls.Add(this.label5);
            base.Controls.Add(this.importButton);
            base.Controls.Add(this.selectSymbol);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "ImportHistoricalDataForm";
            this.Text = "Export historical data";
            base.Load += new EventHandler(this.HistoricalDataForm_Load);
            base.ResumeLayout(false);
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
        /// Get/set the connection for importing historical data. Set the connection before this form is loaded.
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

