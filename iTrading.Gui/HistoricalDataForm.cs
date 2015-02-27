using iTrading.Core.Data;

namespace iTrading.Gui
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Globalization;
    using System.Resources;
    using System.Text;
    using System.Windows.Forms;
    using iTrading.Core.Kernel;
    using iTrading.Core.Data;

    /// <summary>
    /// Form to retrieve historical data.
    /// </summary>
    public class HistoricalDataForm : Form
    {
        private Container components = null;
        private TextBox data;
        private DateTimePicker from;
        private Button goButton;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private NumericUpDown number;
        private ComboBox period;
        private SelectSymbol selectSymbol;
        private DateTimePicker to;

        /// <summary></summary>
        public HistoricalDataForm()
        {
            this.InitializeComponent();
        }

        private void Connection_Bar(object sender, BarUpdateEventArgs e)
        {
            if (e.Error == ErrorCode.NoSuchSymbol)
            {
                this.data.Text = "No data found for this symbol";
            }
            else if (e.Error != ErrorCode.NoError)
            {
                this.data.Text = string.Concat(new object[] { e.NativeError, " (", e.Error, ")" });
            }
            else if (e.Operation == Operation.Insert)
            {
                if (e.Quotes.Bars.Count == 0)
                {
                    this.data.Text = "No historical data found";
                }
                else
                {
                    StringBuilder builder = new StringBuilder();
                    for (int i = e.First; i <= e.Last; i++)
                    {
                        object[] objArray;
                        if (e.Quotes.Period.Id == PeriodTypeId.Tick)
                        {
                            objArray = new object[6];
                            DateTime time = e.Quotes.Time[i];
                            objArray[0] = time.ToString("yyyy-MM-dd HH:mm:ss");
                            objArray[1] = " ";
                            objArray[2] = e.Quotes.Symbol.FormatPrice(e.Quotes.Open[i]);
                            objArray[3] = " ";
                            objArray[4] = e.Quotes.Volume[i];
                            objArray[5] = "\r\n";
                            builder.Append(string.Concat(objArray));
                        }
                        else
                        {
                            objArray = new object[] { e.Quotes.Time[i].ToString((e.Quotes.Period.Id == PeriodTypeId.Day) ? "yyyy-MM-dd" : "yyyy-MM-dd HH:mm:ss"), " ", e.Quotes.Symbol.FormatPrice(e.Quotes.Open[i]), " ", e.Quotes.Symbol.FormatPrice(e.Quotes.High[i]), " ", e.Quotes.Symbol.FormatPrice(e.Quotes.Low[i]), " ", e.Quotes.Symbol.FormatPrice(e.Quotes.Close[i]), " ", e.Quotes.Volume[i], "\r\n" };
                            builder.Append(string.Concat(objArray));
                        }
                    }
                    this.data.Text = builder.ToString();
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

        private void goButton_Click(object sender, EventArgs e)
        {
            Symbol symbol = this.selectSymbol.Symbol;
            if (symbol == null)
            {
                this.data.Text = "Not a valid symbol";
            }
            else if (this.from.Value.Date > this.to.Value.Date)
            {
                this.data.Text = "'from' date must be smaller/equal 'to' date";
            }
            else
            {
                this.data.Text = "Retrieving historical data. Please wait ...";
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
            ResourceManager manager = new ResourceManager(typeof(HistoricalDataForm));
            this.selectSymbol = new SelectSymbol();
            this.from = new DateTimePicker();
            this.label1 = new Label();
            this.label2 = new Label();
            this.to = new DateTimePicker();
            this.period = new ComboBox();
            this.label3 = new Label();
            this.number = new NumericUpDown();
            this.label4 = new Label();
            this.goButton = new Button();
            this.data = new TextBox();
            this.number.BeginInit();
            base.SuspendLayout();
            this.selectSymbol.Connection = null;
            this.selectSymbol.Location = new Point(8, 8);
            this.selectSymbol.Name = "selectSymbol";
            this.selectSymbol.Size = new Size(0x1a0, 0x30);
            this.selectSymbol.Symbol = null;
            this.selectSymbol.TabIndex = 0;
            this.from.Location = new Point(8, 0x58);
            this.from.Name = "from";
            this.from.Size = new Size(0x58, 20);
            this.from.TabIndex = 1;
            this.label1.Location = new Point(8, 0x40);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x58, 0x17);
            this.label1.TabIndex = 2;
            this.label1.Text = "&From";
            this.label1.TextAlign = ContentAlignment.MiddleCenter;
            this.label2.Location = new Point(0x60, 0x40);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x58, 0x17);
            this.label2.TabIndex = 4;
            this.label2.Text = "T&o";
            this.label2.TextAlign = ContentAlignment.MiddleCenter;
            this.to.Location = new Point(0x60, 0x58);
            this.to.Name = "to";
            this.to.Size = new Size(0x58, 20);
            this.to.TabIndex = 3;
            this.period.DropDownStyle = ComboBoxStyle.DropDownList;
            this.period.Location = new Point(0xb8, 0x58);
            this.period.Name = "period";
            this.period.Size = new Size(80, 0x15);
            this.period.Sorted = true;
            this.period.TabIndex = 5;
            this.label3.Location = new Point(0xb8, 0x40);
            this.label3.Name = "label3";
            this.label3.Size = new Size(80, 0x17);
            this.label3.TabIndex = 6;
            this.label3.Text = "&Period";
            this.label3.TextAlign = ContentAlignment.MiddleCenter;
            this.number.Location = new Point(0x108, 0x58);
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
            this.label4.Location = new Point(0x108, 0x40);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x40, 0x17);
            this.label4.TabIndex = 8;
            this.label4.Text = "&#";
            this.label4.TextAlign = ContentAlignment.MiddleCenter;
            this.goButton.Location = new Point(0x158, 0x58);
            this.goButton.Name = "goButton";
            this.goButton.Size = new Size(0x4b, 0x15);
            this.goButton.TabIndex = 9;
            this.goButton.Text = "&Go";
            this.goButton.Click += new EventHandler(this.goButton_Click);
            this.data.Location = new Point(8, 0x88);
            this.data.MaxLength = 0x5f5e0ff;
            this.data.Multiline = true;
            this.data.Name = "data";
            this.data.ScrollBars = ScrollBars.Vertical;
            this.data.Size = new Size(0x1a0, 280);
            this.data.TabIndex = 10;
            this.data.Text = "";
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x1b2, 0x1a5);
            base.Controls.Add(this.data);
            base.Controls.Add(this.goButton);
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
            base.Name = "HistoricalDataForm";
            this.Text = "Historical data";
            base.Load += new EventHandler(this.HistoricalDataForm_Load);
            this.number.EndInit();
            base.ResumeLayout(false);
        }

        /// <summary>
        /// Get/set the connection for retrieving historical data. Set the connection before this control is loaded.
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
                this.selectSymbol.Connection.Bar += new BarUpdateEventHandler(this.Connection_Bar);
            }
        }
    }
}

