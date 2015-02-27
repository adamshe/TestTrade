using iTrading.Core.Data;

namespace iTrading.Gui
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Globalization;
    using System.Resources;
    using System.Windows.Forms;
    using iTrading.Core.Kernel;
    using iTrading.Core.Data;

    /// <summary>
    /// Form to export historical data.
    /// </summary>
    public class ExportHistoricalDataForm : Form
    {
        private Container components = null;
        private Button exportButton;
        private TextBox fileTextBox;
        private DateTimePicker from;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private NumericUpDown number;
        private ComboBox period;
        private Button selectFileButton;
        private SelectSymbol selectSymbol;
        private DateTimePicker to;

        /// <summary></summary>
        public ExportHistoricalDataForm()
        {
            this.InitializeComponent();
        }

        private void Connection_Bar(object sender, BarUpdateEventArgs e)
        {
            this.selectSymbol.Connection.Bar -= new BarUpdateEventHandler(this.Connection_Bar);
            if (e.Error == ErrorCode.NoSuchSymbol)
            {
                MessageBox.Show("No historical data found for this symbol", "TradeMagic", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else if (e.Error != ErrorCode.NoError)
            {
                MessageBox.Show(string.Concat(new object[] { e.NativeError, " (", e.Error, ")" }), "TradeMagic", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else if (e.Operation == Operation.Insert)
            {
                if (e.Quotes.Bars.Count == 0)
                {
                    MessageBox.Show("No historical data found", "TradeMagic", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else
                {
                    try
                    {
                        e.Quotes.Dump(this.fileTextBox.Text, ' ');
                        MessageBox.Show("Data successfully exported", "TradeMagic", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message, "TradeMagic", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }
                }
            }
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

        private void exportButton_Click(object sender, EventArgs e)
        {
            Symbol symbol = this.selectSymbol.Symbol;
            if (symbol == null)
            {
                MessageBox.Show("Not a valid symbol", "TradeMagic", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else if (this.from.Value.Date > this.to.Value.Date)
            {
                MessageBox.Show("'from' date must be smaller/equal 'to' date", "TradeMagic", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else if (this.fileTextBox.Text.Length == 0)
            {
                MessageBox.Show("Please enter file name", "TradeMagic", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else
            {
                this.selectSymbol.Connection.Bar += new BarUpdateEventHandler(this.Connection_Bar);
                try
                {
                    symbol.RequestQuotes(this.from.Value, this.to.Value, new Period(PeriodType.All.Find(this.period.Text).Id, (int) this.number.Value), true, LookupPolicyId.RepositoryAndProvider, null);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, "TradeMagic", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }

        private void HistoricalDataForm_Load(object sender, EventArgs e)
        {
            this.from.Format = DateTimePickerFormat.Custom;
            this.from.CustomFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
            this.to.Format = DateTimePickerFormat.Custom;
            this.to.CustomFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
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

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            ResourceManager manager = new ResourceManager(typeof(ExportHistoricalDataForm));
            this.selectSymbol = new SelectSymbol();
            this.from = new DateTimePicker();
            this.label1 = new Label();
            this.label2 = new Label();
            this.to = new DateTimePicker();
            this.period = new ComboBox();
            this.label3 = new Label();
            this.number = new NumericUpDown();
            this.label4 = new Label();
            this.exportButton = new Button();
            this.selectFileButton = new Button();
            this.fileTextBox = new TextBox();
            this.label5 = new Label();
            this.number.BeginInit();
            base.SuspendLayout();
            this.selectSymbol.Connection = null;
            this.selectSymbol.Location = new Point(8, 8);
            this.selectSymbol.Name = "selectSymbol";
            this.selectSymbol.Size = new Size(0x1a0, 0x30);
            this.selectSymbol.Symbol = null;
            this.selectSymbol.TabIndex = 0;
            this.from.Location = new Point(40, 0x58);
            this.from.Name = "from";
            this.from.Size = new Size(0x58, 20);
            this.from.TabIndex = 1;
            this.label1.Location = new Point(40, 0x40);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x58, 0x17);
            this.label1.TabIndex = 2;
            this.label1.Text = "&From";
            this.label1.TextAlign = ContentAlignment.MiddleCenter;
            this.label2.Location = new Point(0x80, 0x40);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x58, 0x17);
            this.label2.TabIndex = 4;
            this.label2.Text = "T&o";
            this.label2.TextAlign = ContentAlignment.MiddleCenter;
            this.to.Location = new Point(0x80, 0x58);
            this.to.Name = "to";
            this.to.Size = new Size(0x58, 20);
            this.to.TabIndex = 3;
            this.period.DropDownStyle = ComboBoxStyle.DropDownList;
            this.period.Location = new Point(0xd8, 0x58);
            this.period.Name = "period";
            this.period.Size = new Size(80, 0x15);
            this.period.Sorted = true;
            this.period.TabIndex = 5;
            this.label3.Location = new Point(0xd8, 0x40);
            this.label3.Name = "label3";
            this.label3.Size = new Size(80, 0x17);
            this.label3.TabIndex = 6;
            this.label3.Text = "&Period";
            this.label3.TextAlign = ContentAlignment.MiddleCenter;
            this.number.Location = new Point(0x128, 0x58);
            int[] bits = new int[4];
            bits[0] = 0x3e8;
            this.number.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 1;
            this.number.Minimum = new decimal(bits);
            this.number.Name = "number";
            this.number.Size = new Size(0x40, 20);
            this.number.TabIndex = 7;
            bits = new int[4];
            bits[0] = 1;
            this.number.Value = new decimal(bits);
            this.label4.Location = new Point(0x128, 0x40);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x40, 0x17);
            this.label4.TabIndex = 8;
            this.label4.Text = "&#";
            this.label4.TextAlign = ContentAlignment.MiddleCenter;
            this.exportButton.Location = new Point(0xa8, 0xb0);
            this.exportButton.Name = "exportButton";
            this.exportButton.TabIndex = 10;
            this.exportButton.Text = "&Export";
            this.exportButton.Click += new EventHandler(this.exportButton_Click);
            this.selectFileButton.Location = new Point(0x148, 0x88);
            this.selectFileButton.Name = "selectFileButton";
            this.selectFileButton.Size = new Size(0x20, 20);
            this.selectFileButton.TabIndex = 9;
            this.selectFileButton.Text = "...";
            this.selectFileButton.Click += new EventHandler(this.selectFileButton_Click);
            this.fileTextBox.Location = new Point(0x68, 0x88);
            this.fileTextBox.Name = "fileTextBox";
            this.fileTextBox.Size = new Size(0xd8, 20);
            this.fileTextBox.TabIndex = 8;
            this.fileTextBox.Text = "";
            this.label5.Location = new Point(40, 0x88);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x40, 0x17);
            this.label5.TabIndex = 10;
            this.label5.Text = "Export &file:";
            this.label5.TextAlign = ContentAlignment.MiddleLeft;
            base.AcceptButton = this.exportButton;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x1b2, 0xd7);
            base.Controls.Add(this.selectFileButton);
            base.Controls.Add(this.fileTextBox);
            base.Controls.Add(this.label5);
            base.Controls.Add(this.exportButton);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.number);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.period);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.to);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.from);
            base.Controls.Add(this.selectSymbol);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "ExportHistoricalDataForm";
            this.Text = "Export historical data";
            base.Load += new EventHandler(this.HistoricalDataForm_Load);
            this.number.EndInit();
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
        /// Get/set the connection for exporting historical data. Set the connection before this form is loaded.
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

