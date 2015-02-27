namespace iTrading.Gui
{
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Diagnostics;
    using System.Drawing;
    using System.Windows.Forms;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Lookup symbols at the repository.
    /// </summary>
    public class LookupSymbol : UserControl
    {
        private const string columnDescription = "Description";
        private const string columnName = "Name";
        private const string columnPointValue = "Point value";
        private const string columnRight = "Right";
        private const string columnRollOver = "Roll over";
        private const string columnStrikePrice = "Strike price";
        private iTrading.Gui.ColumnStyle[] columnStyles = new iTrading.Gui.ColumnStyle[] { new iTrading.Gui.ColumnStyle("Name", typeof(string), 1, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("Type", typeof(string), 1, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("Description", typeof(string), 4, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("Strike price", typeof(string), 1, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("Right", typeof(string), 1, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("Roll over", typeof(string), 1, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("Point value", typeof(string), 1, true, false, HorizontalAlignment.Center), new iTrading.Gui.ColumnStyle("Tick size", typeof(string), 1, true, false, HorizontalAlignment.Center) };
        private const string columnTickSize = "Tick size";
        private const string columnType = "Type";
        private Label companyNameLabel;
        private TextBox companyNameTextBox;
        private Container components = null;
        private Connection connection = null;
        private Label foundLabel;
        private Label label2;
        private Label label4;
        private Button lookupButton;
        private Panel panel1;
        private Label symbolLabel;
        private TMDataGrid symbolsDataGrid;
        private TextBox symbolTextBox;
        private ComboBox symbolTypeComboBox;

        /// <summary>
        /// </summary>
        public LookupSymbol()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// </summary>
        /// <param name="disposing"></param>
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
            this.symbolTextBox = new TextBox();
            this.symbolLabel = new Label();
            this.symbolTypeComboBox = new ComboBox();
            this.label2 = new Label();
            this.companyNameLabel = new Label();
            this.companyNameTextBox = new TextBox();
            this.lookupButton = new Button();
            this.symbolsDataGrid = new TMDataGrid();
            this.panel1 = new Panel();
            this.foundLabel = new Label();
            this.label4 = new Label();
            this.symbolsDataGrid.BeginInit();
            this.panel1.SuspendLayout();
            base.SuspendLayout();
            this.symbolTextBox.Location = new Point(0, 0x18);
            this.symbolTextBox.Name = "symbolTextBox";
            this.symbolTextBox.Size = new Size(0x40, 20);
            this.symbolTextBox.TabIndex = 0;
            this.symbolTextBox.Text = "*";
            this.symbolLabel.Location = new Point(0, 0);
            this.symbolLabel.Name = "symbolLabel";
            this.symbolLabel.Size = new Size(0x40, 0x17);
            this.symbolLabel.TabIndex = 1;
            this.symbolLabel.Text = "Symbol";
            this.symbolLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.symbolTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            this.symbolTypeComboBox.Location = new Point(0x38, 0x18);
            this.symbolTypeComboBox.Name = "symbolTypeComboBox";
            this.symbolTypeComboBox.Size = new Size(0x48, 0x15);
            this.symbolTypeComboBox.Sorted = true;
            this.symbolTypeComboBox.TabIndex = 2;
            this.label2.Location = new Point(0x38, 0);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x48, 0x17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Type";
            this.label2.TextAlign = ContentAlignment.MiddleCenter;
            this.companyNameLabel.Location = new Point(0x80, 0);
            this.companyNameLabel.Name = "companyNameLabel";
            this.companyNameLabel.Size = new Size(80, 0x17);
            this.companyNameLabel.TabIndex = 5;
            this.companyNameLabel.Text = "Description";
            this.companyNameLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.companyNameTextBox.Location = new Point(0x80, 0x18);
            this.companyNameTextBox.Name = "companyNameTextBox";
            this.companyNameTextBox.Size = new Size(80, 20);
            this.companyNameTextBox.TabIndex = 4;
            this.companyNameTextBox.Text = "*";
            this.lookupButton.Location = new Point(0xd8, 0x18);
            this.lookupButton.Name = "lookupButton";
            this.lookupButton.Size = new Size(0x40, 20);
            this.lookupButton.TabIndex = 6;
            this.lookupButton.Text = "&Lookup";
            this.lookupButton.Click += new EventHandler(this.LookupButton_Click);
            this.symbolsDataGrid.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.symbolsDataGrid.CaptionVisible = false;
            this.symbolsDataGrid.ColumnStyles = null;
            this.symbolsDataGrid.DataMember = "";
            this.symbolsDataGrid.HeaderForeColor = SystemColors.ControlText;
            this.symbolsDataGrid.Location = new Point(0, 0x38);
            this.symbolsDataGrid.Name = "symbolsDataGrid";
            this.symbolsDataGrid.Size = new Size(0x2f0, 440);
            this.symbolsDataGrid.TabIndex = 10;
            this.panel1.Controls.Add(this.foundLabel);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.companyNameLabel);
            this.panel1.Controls.Add(this.companyNameTextBox);
            this.panel1.Controls.Add(this.symbolTypeComboBox);
            this.panel1.Controls.Add(this.symbolTextBox);
            this.panel1.Controls.Add(this.symbolLabel);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.lookupButton);
            this.panel1.Dock = DockStyle.Top;
            this.panel1.Location = new Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x2f0, 0x38);
            this.panel1.TabIndex = 8;
            this.foundLabel.Location = new Point(0x290, 0x18);
            this.foundLabel.Name = "foundLabel";
            this.foundLabel.Size = new Size(0x20, 0x17);
            this.foundLabel.TabIndex = 8;
            this.foundLabel.Text = "0";
            this.foundLabel.TextAlign = ContentAlignment.MiddleRight;
            this.label4.Location = new Point(0x2b0, 0x18);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x30, 0x17);
            this.label4.TabIndex = 7;
            this.label4.Text = "Symbols";
            this.label4.TextAlign = ContentAlignment.MiddleRight;
            base.Controls.Add(this.panel1);
            base.Controls.Add(this.symbolsDataGrid);
            base.Name = "LookupSymbol";
            base.Size = new Size(0x2f0, 0x1f0);
            base.Load += new EventHandler(this.LookupSymbol_Load);
            this.symbolsDataGrid.EndInit();
            this.panel1.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void LookupButton_Click(object sender, EventArgs e)
        {
            string companyName = null;
            string name = null;
            SymbolType symbolType = null;
            if (this.symbolTypeComboBox.Text != "<Any>")
            {
                symbolType = this.connection.SymbolTypes.FindByName(this.symbolTypeComboBox.Text);
            }
            if (this.symbolTextBox.Text.Length > 0)
            {
                name = this.symbolTextBox.Text.Replace("*", "%").Replace("?", "_");
            }
            if (this.companyNameTextBox.Text.Length > 0)
            {
                companyName = this.companyNameTextBox.Text.Replace("*", "%").Replace("?", "_");
            }
            SymbolCollection symbols = Globals.DB.Select(name, null, companyName, Globals.MaxDate, symbolType, null, 0.0, RightId.Unknown, null);
            this.symbolsDataGrid.DataTable.BeginInit();
            this.symbolsDataGrid.DataTable.Rows.Clear();
            int num = 0;
            foreach (Symbol symbol in symbols)
            {
                if (!(symbol.GetProviderName(this.connection.Options.Provider.Id) == symbol.NoProviderName))
                {
                    DataRow row = this.symbolsDataGrid.DataTable.NewRow();
                    this.symbolsDataGrid.DataTable.Rows.Add(row);
                    row["Name"] = symbol.Name;
                    row["Description"] = symbol.CompanyName;
                    row["Roll over"] = symbol.RolloverMonths;
                    row["Point value"] = symbol.PointValue;
                    row["Right"] = symbol.Right.Name;
                    row["Strike price"] = symbol.StrikePrice;
                    row["Tick size"] = symbol.TickSize;
                    row["Type"] = symbol.SymbolType.Name;
                    num++;
                }
            }
            this.symbolsDataGrid.DataTable.EndInit();
            this.foundLabel.Text = num.ToString();
        }

        private void LookupSymbol_Load(object sender, EventArgs e)
        {
            if (Application.ExecutablePath.ToLower().IndexOf("devenv.exe") <= -1)
            {
                this.symbolsDataGrid.RowHeadersVisible = false;
                this.symbolsDataGrid.ColumnStyles = this.columnStyles;
                this.symbolsDataGrid.DataView.AllowNew = false;
                this.symbolTypeComboBox.Items.Add("<Any>");
                foreach (SymbolType type in this.connection.SymbolTypes.Values)
                {
                    if (type.Id != SymbolTypeId.Unknown)
                    {
                        this.symbolTypeComboBox.Items.Add(type.Name);
                    }
                }
                this.symbolTypeComboBox.SelectedIndex = 0;
                ToolTip tip = new ToolTip();
                tip.InitialDelay = 0x3e8;
                tip.ReshowDelay = 0;
                tip.ShowAlways = true;
                tip.SetToolTip(this.symbolLabel, "Enter symbol name pattern. Wildcards '*' and '?' may be applied");
                tip.SetToolTip(this.symbolTextBox, "Enter symbol name pattern. Wildcards '*' and '?' may be applied");
                tip = new ToolTip();
                tip.InitialDelay = 0x3e8;
                tip.ReshowDelay = 0;
                tip.ShowAlways = true;
                tip.SetToolTip(this.companyNameLabel, "Enter symbol description pattern. Wildcards '*' and '?' may be applied");
                tip.SetToolTip(this.companyNameTextBox, "Enter symbol description pattern. Wildcards '*' and '?' may be applied");
                tip = new ToolTip();
                tip.InitialDelay = 0x3e8;
                tip.ReshowDelay = 0;
                tip.ShowAlways = true;
                tip.SetToolTip(this.symbolsDataGrid, "Double click to select your symbol");
            }
        }

        /// <summary>
        /// Get/set the connection. Set the connection before this control is loaded.
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

        /// <summary>
        /// Get the selected symbol.
        /// </summary>
        public Symbol Symbol
        {
            get
            {
                for (int i = 0; i < this.symbolsDataGrid.DataTable.Rows.Count; i++)
                {
                    if (!this.symbolsDataGrid.IsSelected(i))
                    {
                        continue;
                    }
                    string name = (string) this.symbolsDataGrid[this.symbolsDataGrid.CurrentCell.RowNumber, 0];
                    string str2 = (string) this.symbolsDataGrid[this.symbolsDataGrid.CurrentCell.RowNumber, 1];
                    string str3 = (string) this.symbolsDataGrid[this.symbolsDataGrid.CurrentCell.RowNumber, 4];
                    RightId rightId = iTrading.Core.Kernel.Right.All.Find(str3).Id;
                    double strikePrice = Convert.ToDouble((string) this.symbolsDataGrid[this.symbolsDataGrid.CurrentCell.RowNumber, 3]);
                    SymbolType symbolType = this.connection.SymbolTypes.FindByName(str2);
                    SymbolCollection symbols = Globals.DB.Select(name, null, null, Globals.MaxDate, symbolType, null, strikePrice, rightId, null);
                    Trace.Assert(symbols.Count > 0, string.Concat(new object[] { "Gui.LookupSymbol.Symbol: name='", name, "' symbolType=", symbolType, " strikePrice=", strikePrice, " rightId=", rightId }));
                    Trace.Assert(symbols[0].Exchanges.Count > 0, string.Concat(new object[] { "Gui.LookupSymbol.Symbol: name='", name, "' symbolType=", symbolType, " strikePrice=", strikePrice, " rightId=", rightId }));
                    Exchange exchange = null;
                    foreach (Exchange exchange2 in symbols[0].Exchanges.Values)
                    {
                        if (((symbolType.Id == SymbolTypeId.Future) || (symbolType.Id == SymbolTypeId.Option)) && ((exchange2.Id != ExchangeId.Default) && (this.connection.Exchanges[exchange2.Id] != null)))
                        {
                            exchange = exchange2;
                            break;
                        }
                        if (((symbolType.Id == SymbolTypeId.Index) || (symbolType.Id == SymbolTypeId.Stock)) && ((exchange2.Id == ExchangeId.Default) && (this.connection.Exchanges[exchange2.Id] != null)))
                        {
                            exchange = exchange2;
                            break;
                        }
                    }
                    if (exchange == null)
                    {
                        foreach (Exchange exchange3 in symbols[0].Exchanges.Values)
                        {
                            if (this.connection.Exchanges[exchange3.Id] != null)
                            {
                                exchange = exchange3;
                            }
                        }
                    }
                    if (exchange == null)
                    {
                        return null;
                    }
                    DateTime expiry = ((symbolType.Id == SymbolTypeId.Future) || (symbolType.Id == SymbolTypeId.Option)) ? symbols[0].NearestExpiry(DateTime.Now) : Globals.MaxDate;
                    return this.connection.GetSymbol(name, expiry, symbolType, exchange, strikePrice, rightId, LookupPolicyId.RepositoryOnly);
                }
                return null;
            }
        }

        /// <summary>
        /// </summary>
        public TMDataGrid SymbolsDataGrid
        {
            get
            {
                return this.symbolsDataGrid;
            }
        }
    }
}

