namespace iTrading.Gui
{
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Resources;
    using System.Windows.Forms;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Manage symbol data.
    /// </summary>
    public class SymbolsForm : Form
    {
        private Button AddButton;
        private Button AddUpdateSymbolButton;
        private TMDataGrid BrokerNamesDataGrid;
        private const string columnBroker = "Broker";
        private iTrading.Gui.ColumnStyle[] columnBrokerNames = new iTrading.Gui.ColumnStyle[] { new iTrading.Gui.ColumnStyle("Broker", typeof(string), 1, true, true, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("SymbolName", typeof(string), 2, false, false, HorizontalAlignment.Center) };
        private const string columnDate = "Date";
        private const string columnDividend = "Dividend";
        private iTrading.Gui.ColumnStyle[] columnDividends = new iTrading.Gui.ColumnStyle[] { new iTrading.Gui.ColumnStyle("Date", typeof(DateTime), 1, false, true, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("Dividend", typeof(double), 2, false, false, HorizontalAlignment.Center) };
        private const string columnSplitFactor = "Split factor";
        private iTrading.Gui.ColumnStyle[] columnSplits = new iTrading.Gui.ColumnStyle[] { new iTrading.Gui.ColumnStyle("Date", typeof(DateTime), 1, false, true, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("Split factor", typeof(double), 2, false, false, HorizontalAlignment.Center) };
        private const string columnSymbolName = "SymbolName";
        private NumericUpDown CommissionNumericUpDown;
        private TextBox CompanyNameTextBox;
        private Container components = null;
        private ComboBox CurrencyComboBox;
        private TabPage CustomTextTabPage;
        private TextBox CustomTextTextBox;
        private TMDataGrid DividendsDataGrid;
        private TabPage DividendsTabPage;
        private ComboBox ExchangesComboBox;
        private ListBox ExchangesListBox;
        private TabPage GeneralTabPage;
        private GroupBox groupBox1;
        private Label label1;
        private Label label10;
        private Label label11;
        private Label label12;
        private Label label13;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private NumericUpDown MarginNumericUpDown;
        private TabPage NamesTabPage;
        private TextBox NameTextBox;
        private TextBox PointValueTextBox;
        private Button RemoveButton;
        private Button RemoveSymbolButton;
        private ComboBox RightComboBox;
        private NumericUpDown RollOverNumericUpDown;
        private iTrading.Gui.SelectSymbol SelectSymbol;
        private NumericUpDown SlippageNumericUpDown;
        private TMDataGrid SplitsDataGrid;
        private TabPage SplitsTabPage;
        private NumericUpDown StrikePriceNumericUpDown;
        private ComboBox SymbolTypeComboBox;
        private TabControl tabControl1;
        private TextBox TickSizeTextBox;
        private TextBox UrlTextBox;

        /// <summary>
        /// </summary>
        public SymbolsForm()
        {
            this.InitializeComponent();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            if (!this.ExchangesListBox.Items.Contains(this.ExchangesComboBox.SelectedItem))
            {
                this.ExchangesListBox.Items.Add(this.ExchangesComboBox.SelectedItem);
            }
        }

        private void AddUpdateSymbolButton_Click(object sender, EventArgs e)
        {
            Symbol symbol = this.Gui2Data(false);
            if (symbol != null)
            {
                Globals.DB.Update(symbol, true);
            }
        }

        private void Clear()
        {
            this.CommissionNumericUpDown.Value = 0M;
            this.CompanyNameTextBox.Text = "";
            this.CustomTextTextBox.Text = "";
            this.MarginNumericUpDown.Value = 0M;
            this.NameTextBox.Text = "";
            this.PointValueTextBox.Text = "";
            this.RollOverNumericUpDown.Value = 0M;
            this.SlippageNumericUpDown.Value = 0M;
            this.TickSizeTextBox.Text = "";
            this.UrlTextBox.Text = "";
            this.BrokerNamesDataGrid.DataTable.Rows.Clear();
            this.DividendsDataGrid.DataTable.Rows.Clear();
            this.ExchangesListBox.Items.Clear();
            this.SelectSymbol.Clear();
            this.SplitsDataGrid.DataTable.Rows.Clear();
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

        private Symbol Gui2Data(bool forDelete)
        {
            Exchange exchange = null;
            ExchangeDictionary exchanges = new ExchangeDictionary();
            foreach (string str in this.ExchangesListBox.Items)
            {
                if (exchange == null)
                {
                    exchange = this.SelectSymbol.Connection.Exchanges.Find(str);
                }
                exchanges.Add(this.SelectSymbol.Connection.Exchanges.Find(str));
            }
            if (exchange == null)
            {
                if (!forDelete)
                {
                    MessageBox.Show("Please add an exchange", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    this.ExchangesListBox.Focus();
                    return null;
                }
                exchange = Exchange.All[ExchangeId.Default];
            }
            double pointValue = 0.0;
            double tickSize = 0.0;
            try
            {
                pointValue = Convert.ToDouble(this.PointValueTextBox.Text);
            }
            catch (Exception)
            {
                if (!forDelete)
                {
                    MessageBox.Show("Invalid point value", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    this.PointValueTextBox.Focus();
                    return null;
                }
            }
            try
            {
                tickSize = Convert.ToDouble(this.TickSizeTextBox.Text);
            }
            catch (Exception)
            {
                if (!forDelete)
                {
                    MessageBox.Show("Invalid tick size", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    this.TickSizeTextBox.Focus();
                    return null;
                }
            }
            SymbolType type = SymbolType.All.Find((string) this.SymbolTypeComboBox.SelectedItem);
            if (((this.RollOverNumericUpDown.Value == 0M) && (type.Id == SymbolTypeId.Future)) && !forDelete)
            {
                MessageBox.Show("Roll over period must be greater than 0 for futures", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                this.RollOverNumericUpDown.Focus();
                return null;
            }
            if ((type.Id != SymbolTypeId.Stock) && ((this.SplitsDataGrid.DataTable.Rows.Count > 0) || (this.DividendsDataGrid.DataTable.Rows.Count > 0)))
            {
                MessageBox.Show("Only stocks may have splits or dividends. Please remove and split/dividend.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return null;
            }
            DividendDictionary dividends = new DividendDictionary();
            foreach (DataRow row in this.DividendsDataGrid.DataTable.Rows)
            {
                dividends.Add((DateTime) row["Date"], (double) row["Dividend"]);
            }
            SplitDictionary splits = new SplitDictionary();
            foreach (DataRow row2 in this.SplitsDataGrid.DataTable.Rows)
            {
                splits.Add((DateTime) row2["Date"], (double) row2["Split factor"]);
            }
            Symbol symbol = new Symbol(this.SelectSymbol.Connection, this.NameTextBox.Text, new DateTime(0x7d3, 3, 1), SymbolType.All.Find((string)this.SymbolTypeComboBox.SelectedItem), exchange, (double)this.StrikePriceNumericUpDown.Value, iTrading.Core .Kernel.Right.All.Find((string)this.RightComboBox.SelectedItem).Id, Currency.All.Find((string)this.CurrencyComboBox.SelectedItem), tickSize, pointValue, this.CompanyNameTextBox.Text, exchanges, splits, dividends);
            symbol.Commission = (double) this.CommissionNumericUpDown.Value;
            symbol.CustomText = this.CustomTextTextBox.Text;
            symbol.Margin = (double) this.MarginNumericUpDown.Value;
            symbol.RolloverMonths = (int) this.RollOverNumericUpDown.Value;
            symbol.Slippage = (double) this.SlippageNumericUpDown.Value;
            symbol.Url = this.UrlTextBox.Text;
            foreach (DataRow row3 in this.BrokerNamesDataGrid.DataTable.Rows)
            {
                symbol.SetProviderName(ProviderType.All.Find((string) row3["Broker"]).Id, (string) row3["SymbolName"]);
            }
            return symbol;
        }

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            ResourceManager manager = new ResourceManager(typeof(SymbolsForm));
            this.tabControl1 = new TabControl();
            this.GeneralTabPage = new TabPage();
            this.label13 = new Label();
            this.MarginNumericUpDown = new NumericUpDown();
            this.label12 = new Label();
            this.SlippageNumericUpDown = new NumericUpDown();
            this.label11 = new Label();
            this.CommissionNumericUpDown = new NumericUpDown();
            this.StrikePriceNumericUpDown = new NumericUpDown();
            this.label10 = new Label();
            this.label9 = new Label();
            this.RightComboBox = new ComboBox();
            this.groupBox1 = new GroupBox();
            this.RemoveButton = new Button();
            this.AddButton = new Button();
            this.ExchangesComboBox = new ComboBox();
            this.ExchangesListBox = new ListBox();
            this.label8 = new Label();
            this.RollOverNumericUpDown = new NumericUpDown();
            this.label7 = new Label();
            this.TickSizeTextBox = new TextBox();
            this.label6 = new Label();
            this.PointValueTextBox = new TextBox();
            this.label5 = new Label();
            this.CurrencyComboBox = new ComboBox();
            this.label2 = new Label();
            this.UrlTextBox = new TextBox();
            this.label1 = new Label();
            this.CompanyNameTextBox = new TextBox();
            this.label3 = new Label();
            this.SymbolTypeComboBox = new ComboBox();
            this.label4 = new Label();
            this.NameTextBox = new TextBox();
            this.DividendsTabPage = new TabPage();
            this.DividendsDataGrid = new TMDataGrid();
            this.SplitsTabPage = new TabPage();
            this.SplitsDataGrid = new TMDataGrid();
            this.NamesTabPage = new TabPage();
            this.BrokerNamesDataGrid = new TMDataGrid();
            this.CustomTextTabPage = new TabPage();
            this.CustomTextTextBox = new TextBox();
            this.SelectSymbol = new iTrading.Gui.SelectSymbol();
            this.AddUpdateSymbolButton = new Button();
            this.RemoveSymbolButton = new Button();
            this.tabControl1.SuspendLayout();
            this.GeneralTabPage.SuspendLayout();
            this.MarginNumericUpDown.BeginInit();
            this.SlippageNumericUpDown.BeginInit();
            this.CommissionNumericUpDown.BeginInit();
            this.StrikePriceNumericUpDown.BeginInit();
            this.groupBox1.SuspendLayout();
            this.RollOverNumericUpDown.BeginInit();
            this.DividendsTabPage.SuspendLayout();
            this.DividendsDataGrid.BeginInit();
            this.SplitsTabPage.SuspendLayout();
            this.SplitsDataGrid.BeginInit();
            this.NamesTabPage.SuspendLayout();
            this.BrokerNamesDataGrid.BeginInit();
            this.CustomTextTabPage.SuspendLayout();
            base.SuspendLayout();
            this.tabControl1.Controls.Add(this.GeneralTabPage);
            this.tabControl1.Controls.Add(this.DividendsTabPage);
            this.tabControl1.Controls.Add(this.SplitsTabPage);
            this.tabControl1.Controls.Add(this.NamesTabPage);
            this.tabControl1.Controls.Add(this.CustomTextTabPage);
            this.tabControl1.Location = new Point(10, 0x5c);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new Size(0x1f3, 0x1a9);
            this.tabControl1.TabIndex = 0;
            this.GeneralTabPage.Controls.Add(this.label13);
            this.GeneralTabPage.Controls.Add(this.MarginNumericUpDown);
            this.GeneralTabPage.Controls.Add(this.label12);
            this.GeneralTabPage.Controls.Add(this.SlippageNumericUpDown);
            this.GeneralTabPage.Controls.Add(this.label11);
            this.GeneralTabPage.Controls.Add(this.CommissionNumericUpDown);
            this.GeneralTabPage.Controls.Add(this.StrikePriceNumericUpDown);
            this.GeneralTabPage.Controls.Add(this.label10);
            this.GeneralTabPage.Controls.Add(this.label9);
            this.GeneralTabPage.Controls.Add(this.RightComboBox);
            this.GeneralTabPage.Controls.Add(this.groupBox1);
            this.GeneralTabPage.Controls.Add(this.label8);
            this.GeneralTabPage.Controls.Add(this.RollOverNumericUpDown);
            this.GeneralTabPage.Controls.Add(this.label7);
            this.GeneralTabPage.Controls.Add(this.TickSizeTextBox);
            this.GeneralTabPage.Controls.Add(this.label6);
            this.GeneralTabPage.Controls.Add(this.PointValueTextBox);
            this.GeneralTabPage.Controls.Add(this.label5);
            this.GeneralTabPage.Controls.Add(this.CurrencyComboBox);
            this.GeneralTabPage.Controls.Add(this.label2);
            this.GeneralTabPage.Controls.Add(this.UrlTextBox);
            this.GeneralTabPage.Controls.Add(this.label1);
            this.GeneralTabPage.Controls.Add(this.CompanyNameTextBox);
            this.GeneralTabPage.Controls.Add(this.label3);
            this.GeneralTabPage.Controls.Add(this.SymbolTypeComboBox);
            this.GeneralTabPage.Controls.Add(this.label4);
            this.GeneralTabPage.Controls.Add(this.NameTextBox);
            this.GeneralTabPage.Location = new Point(4, 0x19);
            this.GeneralTabPage.Name = "GeneralTabPage";
            this.GeneralTabPage.Size = new Size(0x1eb, 0x18c);
            this.GeneralTabPage.TabIndex = 0;
            this.GeneralTabPage.Text = "General";
            this.label13.Location = new Point(280, 0x120);
            this.label13.Name = "label13";
            this.label13.Size = new Size(0x80, 0x1b);
            this.label13.TabIndex = 0x1f;
            this.label13.Text = "&Margin req. per unit:";
            this.label13.TextAlign = ContentAlignment.MiddleLeft;
            this.MarginNumericUpDown.Location = new Point(0x1a0, 0x120);
            int[] bits = new int[4];
            bits[0] = 0x98967f;
            this.MarginNumericUpDown.Maximum = new decimal(bits);
            this.MarginNumericUpDown.Name = "MarginNumericUpDown";
            this.MarginNumericUpDown.Size = new Size(0x40, 0x16);
            this.MarginNumericUpDown.TabIndex = 0x13;
            this.label12.Location = new Point(280, 0xf8);
            this.label12.Name = "label12";
            this.label12.Size = new Size(0x80, 0x1b);
            this.label12.TabIndex = 0x1d;
            this.label12.Text = "&Slippage as ticks:";
            this.label12.TextAlign = ContentAlignment.MiddleLeft;
            this.SlippageNumericUpDown.DecimalPlaces = 2;
            this.SlippageNumericUpDown.Location = new Point(0x1a0, 0xf8);
            this.SlippageNumericUpDown.Name = "SlippageNumericUpDown";
            this.SlippageNumericUpDown.Size = new Size(0x40, 0x16);
            this.SlippageNumericUpDown.TabIndex = 0x12;
            this.label11.Location = new Point(280, 0xd0);
            this.label11.Name = "label11";
            this.label11.Size = new Size(130, 0x1b);
            this.label11.TabIndex = 0x1b;
            this.label11.Text = "&Commission per unit:";
            this.label11.TextAlign = ContentAlignment.MiddleLeft;
            this.CommissionNumericUpDown.DecimalPlaces = 2;
            this.CommissionNumericUpDown.Location = new Point(0x1a0, 0xd0);
            this.CommissionNumericUpDown.Name = "CommissionNumericUpDown";
            this.CommissionNumericUpDown.Size = new Size(0x40, 0x16);
            this.CommissionNumericUpDown.TabIndex = 0x11;
            this.StrikePriceNumericUpDown.DecimalPlaces = 1;
            this.StrikePriceNumericUpDown.Location = new Point(0x73, 0x66);
            bits = new int[4];
            bits[0] = 0x3e8;
            this.StrikePriceNumericUpDown.Maximum = new decimal(bits);
            this.StrikePriceNumericUpDown.Name = "StrikePriceNumericUpDown";
            this.StrikePriceNumericUpDown.Size = new Size(0x7d, 0x16);
            this.StrikePriceNumericUpDown.TabIndex = 12;
            this.label10.Location = new Point(10, 0x66);
            this.label10.Name = "label10";
            this.label10.Size = new Size(0x69, 0x1a);
            this.label10.TabIndex = 0x19;
            this.label10.Text = "Stri&ke price:";
            this.label10.TextAlign = ContentAlignment.MiddleLeft;
            this.label9.Location = new Point(10, 0x8a);
            this.label9.Name = "label9";
            this.label9.Size = new Size(0x4c, 0x1b);
            this.label9.TabIndex = 0x17;
            this.label9.Text = "Ri&ght:";
            this.label9.TextAlign = ContentAlignment.MiddleLeft;
            this.RightComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            this.RightComboBox.Location = new Point(0x73, 0x8a);
            this.RightComboBox.Name = "RightComboBox";
            this.RightComboBox.Size = new Size(0x7d, 0x18);
            this.RightComboBox.TabIndex = 13;
            this.groupBox1.Controls.Add(this.RemoveButton);
            this.groupBox1.Controls.Add(this.AddButton);
            this.groupBox1.Controls.Add(this.ExchangesComboBox);
            this.groupBox1.Controls.Add(this.ExchangesListBox);
            this.groupBox1.Location = new Point(280, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0xca, 0xc0);
            this.groupBox1.TabIndex = 0x16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Exchanges";
            this.RemoveButton.Location = new Point(0x73, 0x37);
            this.RemoveButton.Name = "RemoveButton";
            this.RemoveButton.Size = new Size(0x43, 0x19);
            this.RemoveButton.TabIndex = 0x17;
            this.RemoveButton.Text = "Remo&ve";
            this.RemoveButton.Click += new EventHandler(this.RemoveButton_Click);
            this.AddButton.Location = new Point(0x13, 0x37);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new Size(0x43, 0x19);
            this.AddButton.TabIndex = 0x15;
            this.AddButton.Text = "A&dd";
            this.AddButton.Click += new EventHandler(this.AddButton_Click);
            this.ExchangesComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            this.ExchangesComboBox.Location = new Point(10, 0x12);
            this.ExchangesComboBox.Name = "ExchangesComboBox";
            this.ExchangesComboBox.Size = new Size(0xb6, 0x18);
            this.ExchangesComboBox.TabIndex = 20;
            this.ExchangesListBox.ItemHeight = 0x10;
            this.ExchangesListBox.Location = new Point(10, 0x5c);
            this.ExchangesListBox.Name = "ExchangesListBox";
            this.ExchangesListBox.ScrollAlwaysVisible = true;
            this.ExchangesListBox.SelectionMode = SelectionMode.MultiSimple;
            this.ExchangesListBox.Size = new Size(0xb6, 0x54);
            this.ExchangesListBox.TabIndex = 0x18;
            this.label8.Location = new Point(10, 0x11e);
            this.label8.Name = "label8";
            this.label8.Size = new Size(0x69, 0x1b);
            this.label8.TabIndex = 0x13;
            this.label8.Text = "&Rollover period:";
            this.label8.TextAlign = ContentAlignment.MiddleLeft;
            this.RollOverNumericUpDown.Location = new Point(0x73, 0x11e);
            this.RollOverNumericUpDown.Name = "RollOverNumericUpDown";
            this.RollOverNumericUpDown.Size = new Size(0x7d, 0x16);
            this.RollOverNumericUpDown.TabIndex = 0x11;
            this.label7.Location = new Point(10, 0xf9);
            this.label7.Name = "label7";
            this.label7.Size = new Size(0x4c, 0x1b);
            this.label7.TabIndex = 0x11;
            this.label7.Text = "Tic&k size:";
            this.label7.TextAlign = ContentAlignment.MiddleLeft;
            this.TickSizeTextBox.Location = new Point(0x73, 0xf9);
            this.TickSizeTextBox.Name = "TickSizeTextBox";
            this.TickSizeTextBox.Size = new Size(120, 0x16);
            this.TickSizeTextBox.TabIndex = 0x10;
            this.TickSizeTextBox.Text = "";
            this.label6.Location = new Point(10, 0xd4);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x4c, 0x1b);
            this.label6.TabIndex = 15;
            this.label6.Text = "&Point value:";
            this.label6.TextAlign = ContentAlignment.MiddleLeft;
            this.PointValueTextBox.Location = new Point(0x73, 0xd4);
            this.PointValueTextBox.Name = "PointValueTextBox";
            this.PointValueTextBox.Size = new Size(120, 0x16);
            this.PointValueTextBox.TabIndex = 15;
            this.PointValueTextBox.Text = "";
            this.label5.Location = new Point(10, 0xaf);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x4c, 0x1b);
            this.label5.TabIndex = 13;
            this.label5.Text = "Currenc&y:";
            this.label5.TextAlign = ContentAlignment.MiddleLeft;
            this.CurrencyComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            this.CurrencyComboBox.Location = new Point(0x73, 0xaf);
            this.CurrencyComboBox.Name = "CurrencyComboBox";
            this.CurrencyComboBox.Size = new Size(0x7d, 0x18);
            this.CurrencyComboBox.TabIndex = 14;
            this.label2.Location = new Point(10, 360);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x60, 0x1b);
            this.label2.TabIndex = 11;
            this.label2.Text = "&URL:";
            this.label2.TextAlign = ContentAlignment.MiddleLeft;
            this.UrlTextBox.Location = new Point(0x73, 360);
            this.UrlTextBox.Name = "UrlTextBox";
            this.UrlTextBox.Size = new Size(0x16d, 0x16);
            this.UrlTextBox.TabIndex = 0x15;
            this.UrlTextBox.Text = "";
            this.label1.Location = new Point(10, 0x143);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x69, 0x1b);
            this.label1.TabIndex = 9;
            this.label1.Text = "&Description:";
            this.label1.TextAlign = ContentAlignment.MiddleLeft;
            this.CompanyNameTextBox.Location = new Point(0x73, 0x143);
            this.CompanyNameTextBox.Name = "CompanyNameTextBox";
            this.CompanyNameTextBox.Size = new Size(0x16d, 0x16);
            this.CompanyNameTextBox.TabIndex = 20;
            this.CompanyNameTextBox.Text = "";
            this.label3.Location = new Point(10, 0x41);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x4c, 0x1a);
            this.label3.TabIndex = 7;
            this.label3.Text = "&Type:";
            this.label3.TextAlign = ContentAlignment.MiddleLeft;
            this.SymbolTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            this.SymbolTypeComboBox.Location = new Point(0x73, 0x41);
            this.SymbolTypeComboBox.Name = "SymbolTypeComboBox";
            this.SymbolTypeComboBox.Size = new Size(0x7d, 0x18);
            this.SymbolTypeComboBox.TabIndex = 11;
            this.label4.Location = new Point(10, 0x1c);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x4c, 0x1a);
            this.label4.TabIndex = 5;
            this.label4.Text = "&Name:";
            this.label4.TextAlign = ContentAlignment.MiddleLeft;
            this.NameTextBox.Location = new Point(0x73, 0x1c);
            this.NameTextBox.Name = "NameTextBox";
            this.NameTextBox.Size = new Size(120, 0x16);
            this.NameTextBox.TabIndex = 10;
            this.NameTextBox.Text = "";
            this.DividendsTabPage.Controls.Add(this.DividendsDataGrid);
            this.DividendsTabPage.Location = new Point(4, 0x19);
            this.DividendsTabPage.Name = "DividendsTabPage";
            this.DividendsTabPage.Size = new Size(0x1eb, 0x18c);
            this.DividendsTabPage.TabIndex = 5;
            this.DividendsTabPage.Text = "Dividends";
            this.DividendsDataGrid.CaptionVisible = false;
            this.DividendsDataGrid.ColumnStyles = null;
            this.DividendsDataGrid.DataMember = "";
            this.DividendsDataGrid.HeaderForeColor = SystemColors.ControlText;
            this.DividendsDataGrid.Location = new Point(10, 8);
            this.DividendsDataGrid.Name = "DividendsDataGrid";
            this.DividendsDataGrid.Size = new Size(470, 380);
            this.DividendsDataGrid.TabIndex = 1;
            this.SplitsTabPage.Controls.Add(this.SplitsDataGrid);
            this.SplitsTabPage.Location = new Point(4, 0x19);
            this.SplitsTabPage.Name = "SplitsTabPage";
            this.SplitsTabPage.Size = new Size(0x1eb, 0x18c);
            this.SplitsTabPage.TabIndex = 3;
            this.SplitsTabPage.Text = "Splits";
            this.SplitsDataGrid.CaptionVisible = false;
            this.SplitsDataGrid.ColumnStyles = null;
            this.SplitsDataGrid.DataMember = "";
            this.SplitsDataGrid.HeaderForeColor = SystemColors.ControlText;
            this.SplitsDataGrid.Location = new Point(10, 9);
            this.SplitsDataGrid.Name = "SplitsDataGrid";
            this.SplitsDataGrid.Size = new Size(470, 0x17b);
            this.SplitsDataGrid.TabIndex = 0;
            this.NamesTabPage.Controls.Add(this.BrokerNamesDataGrid);
            this.NamesTabPage.Location = new Point(4, 0x19);
            this.NamesTabPage.Name = "NamesTabPage";
            this.NamesTabPage.Size = new Size(0x1eb, 0x18c);
            this.NamesTabPage.TabIndex = 1;
            this.NamesTabPage.Text = "Names";
            this.BrokerNamesDataGrid.CaptionVisible = false;
            this.BrokerNamesDataGrid.ColumnStyles = null;
            this.BrokerNamesDataGrid.DataMember = "";
            this.BrokerNamesDataGrid.HeaderForeColor = SystemColors.ControlText;
            this.BrokerNamesDataGrid.Location = new Point(10, 9);
            this.BrokerNamesDataGrid.Name = "BrokerNamesDataGrid";
            this.BrokerNamesDataGrid.Size = new Size(470, 0x17b);
            this.BrokerNamesDataGrid.TabIndex = 0;
            this.CustomTextTabPage.Controls.Add(this.CustomTextTextBox);
            this.CustomTextTabPage.Location = new Point(4, 0x19);
            this.CustomTextTabPage.Name = "CustomTextTabPage";
            this.CustomTextTabPage.Size = new Size(0x1eb, 0x18c);
            this.CustomTextTabPage.TabIndex = 4;
            this.CustomTextTabPage.Text = "Custom text";
            this.CustomTextTextBox.Location = new Point(10, 9);
            this.CustomTextTextBox.Multiline = true;
            this.CustomTextTextBox.Name = "CustomTextTextBox";
            this.CustomTextTextBox.ScrollBars = ScrollBars.Both;
            this.CustomTextTextBox.Size = new Size(470, 0x17b);
            this.CustomTextTextBox.TabIndex = 0;
            this.CustomTextTextBox.Text = "";
            this.CustomTextTextBox.WordWrap = false;
            this.SelectSymbol.Connection = null;
            this.SelectSymbol.Location = new Point(10, 0x12);
            this.SelectSymbol.Name = "SelectSymbol";
            this.SelectSymbol.Size = new Size(0x1f3, 0x38);
            this.SelectSymbol.Symbol = null;
            this.SelectSymbol.TabIndex = 1;
            this.AddUpdateSymbolButton.Location = new Point(0x6a, 0x217);
            this.AddUpdateSymbolButton.Name = "AddUpdateSymbolButton";
            this.AddUpdateSymbolButton.Size = new Size(90, 0x1b);
            this.AddUpdateSymbolButton.TabIndex = 30;
            this.AddUpdateSymbolButton.Text = "&Add/Update";
            this.AddUpdateSymbolButton.Click += new EventHandler(this.AddUpdateSymbolButton_Click);
            this.RemoveSymbolButton.Location = new Point(0x133, 0x217);
            this.RemoveSymbolButton.Name = "RemoveSymbolButton";
            this.RemoveSymbolButton.Size = new Size(90, 0x1b);
            this.RemoveSymbolButton.TabIndex = 0x20;
            this.RemoveSymbolButton.Text = "&Remove";
            this.RemoveSymbolButton.Click += new EventHandler(this.RemoveSymbolButton_Click);
            this.AutoScaleBaseSize = new Size(6, 15);
            base.ClientSize = new Size(520, 0x23b);
            base.Controls.Add(this.RemoveSymbolButton);
            base.Controls.Add(this.AddUpdateSymbolButton);
            base.Controls.Add(this.SelectSymbol);
            base.Controls.Add(this.tabControl1);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "SymbolsForm";
            this.Text = "Symbols";
            base.Load += new EventHandler(this.SymbolsForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.GeneralTabPage.ResumeLayout(false);
            this.MarginNumericUpDown.EndInit();
            this.SlippageNumericUpDown.EndInit();
            this.CommissionNumericUpDown.EndInit();
            this.StrikePriceNumericUpDown.EndInit();
            this.groupBox1.ResumeLayout(false);
            this.RollOverNumericUpDown.EndInit();
            this.DividendsTabPage.ResumeLayout(false);
            this.DividendsDataGrid.EndInit();
            this.SplitsTabPage.ResumeLayout(false);
            this.SplitsDataGrid.EndInit();
            this.NamesTabPage.ResumeLayout(false);
            this.BrokerNamesDataGrid.EndInit();
            this.CustomTextTabPage.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            while (this.ExchangesListBox.SelectedItems.Count > 0)
            {
                this.ExchangesListBox.Items.Remove(this.ExchangesListBox.SelectedItems[0]);
            }
        }

        private void RemoveSymbolButton_Click(object sender, EventArgs e)
        {
            Symbol symbol = this.Gui2Data(true);
            if ((symbol != null) && (MessageBox.Show("Do you really want to delete this symbol ?", "Delete symbol", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK))
            {
                Globals.DB.Delete(symbol);
                this.Clear();
            }
        }

        private void SelectSymbol_SelectedSymbolChanged(object o, SymbolEventArgs e)
        {
            this.CommissionNumericUpDown.Value = (decimal) e.Symbol.Commission;
            this.CompanyNameTextBox.Text = e.Symbol.CompanyName;
            this.CurrencyComboBox.SelectedItem = e.Symbol.Currency.Name;
            this.CustomTextTextBox.Text = e.Symbol.CustomText;
            this.MarginNumericUpDown.Value = (decimal) e.Symbol.Margin;
            this.NameTextBox.Text = e.Symbol.Name;
            this.PointValueTextBox.Text = e.Symbol.PointValue.ToString();
            this.RightComboBox.SelectedItem = e.Symbol.Right.Name;
            this.RollOverNumericUpDown.Value = e.Symbol.RolloverMonths;
            this.SlippageNumericUpDown.Value = (decimal) e.Symbol.Slippage;
            this.StrikePriceNumericUpDown.Value = (decimal) e.Symbol.StrikePrice;
            this.SymbolTypeComboBox.SelectedItem = e.Symbol.SymbolType.Name;
            this.TickSizeTextBox.Text = e.Symbol.TickSize.ToString();
            this.UrlTextBox.Text = e.Symbol.Url;
            this.ExchangesListBox.Items.Clear();
            foreach (Exchange exchange in e.Symbol.Exchanges.Values)
            {
                this.ExchangesListBox.Items.Add(exchange.Name);
            }
            this.BrokerNamesDataGrid.DataTable.Rows.Clear();
            foreach (ProviderType type in ProviderType.All.Values)
            {
                DataRow row = this.BrokerNamesDataGrid.DataTable.NewRow();
                row["Broker"] = type.Name;
                row["SymbolName"] = e.Symbol.GetProviderName(type.Id);
                this.BrokerNamesDataGrid.DataTable.Rows.Add(row);
            }
            this.DividendsDataGrid.DataTable.Rows.Clear();
            foreach (DateTime time in e.Symbol.Dividends.Keys)
            {
                DataRow row2 = this.DividendsDataGrid.DataTable.NewRow();
                row2["Date"] = time;
                row2["Dividend"] = e.Symbol.Dividends[time];
                this.DividendsDataGrid.DataTable.Rows.Add(row2);
            }
            this.SplitsDataGrid.DataTable.Rows.Clear();
            foreach (DateTime time2 in e.Symbol.Splits.Keys)
            {
                DataRow row3 = this.SplitsDataGrid.DataTable.NewRow();
                row3["Date"] = time2;
                row3["Split factor"] = e.Symbol.Splits[time2];
                this.SplitsDataGrid.DataTable.Rows.Add(row3);
            }
        }

        private void SymbolsForm_Load(object sender, EventArgs e)
        {
            this.BrokerNamesDataGrid.RowHeadersVisible = false;
            this.BrokerNamesDataGrid.ColumnStyles = this.columnBrokerNames;
            this.BrokerNamesDataGrid.DataView.AllowEdit = true;
            this.BrokerNamesDataGrid.DataView.AllowNew = false;
            this.DividendsDataGrid.RowHeadersVisible = true;
            this.DividendsDataGrid.ColumnStyles = this.columnDividends;
            this.DividendsDataGrid.DataView.AllowDelete = true;
            this.DividendsDataGrid.DataView.AllowEdit = true;
            this.DividendsDataGrid.DataView.AllowNew = true;
            this.SplitsDataGrid.RowHeadersVisible = true;
            this.SplitsDataGrid.ColumnStyles = this.columnSplits;
            this.SplitsDataGrid.DataView.AllowDelete = true;
            this.SplitsDataGrid.DataView.AllowEdit = true;
            this.SplitsDataGrid.DataView.AllowNew = true;
            this.RightComboBox.Items.Clear();
            foreach (Right right in iTrading.Core.Kernel.Right.All.Values)
            {
                this.RightComboBox.Items.Add(right.Name);
            }
            this.RightComboBox.SelectedItem = iTrading.Core.Kernel.Right.All[RightId.Unknown].Name;
            this.SelectSymbol.SelectedSymbolChanged += new SelectedSymbolChangedEventHandler(this.SelectSymbol_SelectedSymbolChanged);
        }

        /// <summary>
        /// Get/set the connection for managing symbol data. Set the connection before this control is loaded.
        /// </summary>
        public Connection Connection
        {
            get
            {
                return this.SelectSymbol.Connection;
            }
            set
            {
                this.SelectSymbol.Connection = value;
                this.CurrencyComboBox.Items.Clear();
                foreach (Currency currency in this.SelectSymbol.Connection.Currencies.Values)
                {
                    this.CurrencyComboBox.Items.Add(currency.Name);
                }
                this.CurrencyComboBox.SelectedItem = Currency.All[CurrencyId.Unknown].Name;
                this.ExchangesComboBox.Items.Clear();
                foreach (Exchange exchange in this.SelectSymbol.Connection.Exchanges.Values)
                {
                    this.ExchangesComboBox.Items.Add(exchange.Name);
                }
                if (this.SelectSymbol.Connection.Exchanges[ExchangeId.Default] != null)
                {
                    this.ExchangesComboBox.SelectedItem = Exchange.All[ExchangeId.Default].Name;
                }
                else
                {
                    this.ExchangesComboBox.SelectedIndex = 0;
                }
                this.SymbolTypeComboBox.Items.Clear();
                foreach (SymbolType type in this.SelectSymbol.Connection.SymbolTypes.Values)
                {
                    this.SymbolTypeComboBox.Items.Add(type.Name);
                }
                if (this.SelectSymbol.Connection.SymbolTypes[SymbolTypeId.Unknown] != null)
                {
                    this.SymbolTypeComboBox.SelectedItem = SymbolType.All[SymbolTypeId.Unknown].Name;
                }
                else
                {
                    this.SymbolTypeComboBox.SelectedIndex = 0;
                }
            }
        }
    }
}

