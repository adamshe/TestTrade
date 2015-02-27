namespace iTrading.Gui
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Control to select a symbol.
    /// </summary>
    public class SelectSymbol : UserControl
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private Container components = null;
        private Connection connection = null;
        private Symbol currentSymbol = null;
        private ComboBox exchange;
        private ComboBox expiry;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label limitPriceLabel;
        private const int monthsBack = 14;
        private Button querySymbolButton;
        private CheckBox right;
        private PriceUpDown strikePrice;
        private TextBox symbolName;
        private ComboBox symbolType;
        private bool updating = false;

        /// <summary>
        /// This event will be thrown when a <see cref="T:TradeMagic.Cbi.AccountItem" /> is updated.
        /// </summary>
        public event SelectedSymbolChangedEventHandler SelectedSymbolChanged;

        /// <summary>
        /// 
        /// </summary>
        public SelectSymbol()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Clear the "name" text box.
        /// </summary>
        public void Clear()
        {
            this.symbolName.Text = "";
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

        private void exchange_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.updating && (this.connection.FeatureTypes[FeatureTypeId.SymbolLookup] != null))
            {
                this.SymbolNameChanged();
            }
        }

        private void expiry_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.updating && (this.connection.FeatureTypes[FeatureTypeId.SymbolLookup] != null))
            {
                this.SymbolNameChanged();
            }
        }

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.symbolName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.symbolType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.exchange = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.expiry = new System.Windows.Forms.ComboBox();
            this.querySymbolButton = new System.Windows.Forms.Button();
            this.right = new System.Windows.Forms.CheckBox();
            this.limitPriceLabel = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Name";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // symbolName
            // 
            this.symbolName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.symbolName.Location = new System.Drawing.Point(0, 24);
            this.symbolName.Name = "symbolName";
            this.symbolName.Size = new System.Drawing.Size(80, 20);
            this.symbolName.TabIndex = 0;
            this.symbolName.Leave += new System.EventHandler(this.symbolName_Leave);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(80, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 23);
            this.label2.TabIndex = 2;
            this.label2.Text = "&Type";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // symbolType
            // 
            this.symbolType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.symbolType.Location = new System.Drawing.Point(80, 24);
            this.symbolType.Name = "symbolType";
            this.symbolType.Size = new System.Drawing.Size(72, 21);
            this.symbolType.Sorted = true;
            this.symbolType.TabIndex = 1;
            this.symbolType.SelectedIndexChanged += new System.EventHandler(this.symbolType_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(152, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 23);
            this.label3.TabIndex = 4;
            this.label3.Text = "&Exchange";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // exchange
            // 
            this.exchange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.exchange.Location = new System.Drawing.Point(152, 24);
            this.exchange.MaxDropDownItems = 16;
            this.exchange.Name = "exchange";
            this.exchange.Size = new System.Drawing.Size(88, 21);
            this.exchange.Sorted = true;
            this.exchange.TabIndex = 2;
            this.exchange.SelectedIndexChanged += new System.EventHandler(this.exchange_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(248, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 23);
            this.label4.TabIndex = 6;
            this.label4.Text = "E&xpiry";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // expiry
            // 
            this.expiry.CausesValidation = false;
            this.expiry.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.expiry.Location = new System.Drawing.Point(240, 24);
            this.expiry.Name = "expiry";
            this.expiry.Size = new System.Drawing.Size(70, 21);
            this.expiry.TabIndex = 3;
            this.expiry.SelectedIndexChanged += new System.EventHandler(this.expiry_SelectedIndexChanged);
            // 
            // querySymbolButton
            // 
            this.querySymbolButton.Location = new System.Drawing.Point(384, 24);
            this.querySymbolButton.Name = "querySymbolButton";
            this.querySymbolButton.Size = new System.Drawing.Size(32, 20);
            this.querySymbolButton.TabIndex = 6;
            this.querySymbolButton.Text = "...";
            this.querySymbolButton.Click += new System.EventHandler(this.querySymbolButton_Click);
            // 
            // right
            // 
            this.right.Location = new System.Drawing.Point(368, 26);
            this.right.Name = "right";
            this.right.Size = new System.Drawing.Size(16, 16);
            this.right.TabIndex = 5;
            this.right.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.right.CheckedChanged += new System.EventHandler(this.Right_CheckedChanged);
            // 
            // limitPriceLabel
            // 
            this.limitPriceLabel.Location = new System.Drawing.Point(304, 0);
            this.limitPriceLabel.Name = "limitPriceLabel";
            this.limitPriceLabel.Size = new System.Drawing.Size(56, 23);
            this.limitPriceLabel.TabIndex = 9;
            this.limitPriceLabel.Text = "Stri&ke";
            this.limitPriceLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(360, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(32, 23);
            this.label5.TabIndex = 10;
            this.label5.Text = "&Put";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SelectSymbol
            // 
            this.Controls.Add(this.label5);
            this.Controls.Add(this.limitPriceLabel);
            this.Controls.Add(this.right);
            this.Controls.Add(this.querySymbolButton);
            this.Controls.Add(this.expiry);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.exchange);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.symbolType);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.symbolName);
            this.Controls.Add(this.label1);
            this.Name = "SelectSymbol";
            this.Size = new System.Drawing.Size(416, 48);
            this.Load += new System.EventHandler(this.SelectSymbol_Load);
            this.Leave += new System.EventHandler(this.SelectSymbol_Leave);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void querySymbolButton_Click(object sender, EventArgs e)
        {
            LookupSymbolForm form = new LookupSymbolForm();
            form.LookupSymbol.Connection = this.connection;
            form.WindowState = FormWindowState.Normal;
            form.ShowDialog();
            Symbol symbol = form.LookupSymbol.Symbol;
            if (symbol != null)
            {
                this.updating = true;
                this.exchange.Text = symbol.Exchange.Name;
                this.expiry.SelectedValue = symbol.Expiry;
                this.right.Checked = symbol.Right.Id == RightId.Put;
                this.strikePrice.Value = (decimal) symbol.StrikePrice;
                this.symbolName.Text = symbol.Name;
                this.symbolType.Text = symbol.SymbolType.Name;
                this.updating = false;
                this.currentSymbol = this.Symbol;
                if (this.SelectedSymbolChanged != null)
                {
                    this.SelectedSymbolChanged(this, new SymbolEventArgs(this.Connection, ErrorCode.NoError, "", this.currentSymbol));
                }
            }
        }

        private void Right_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.updating && (this.connection.FeatureTypes[FeatureTypeId.SymbolLookup] != null))
            {
                this.SymbolNameChanged();
            }
        }

        private void SelectSymbol_Leave(object sender, EventArgs e)
        {
            if (!this.updating)
            {
                this.SymbolNameChanged();
            }
        }

        private void SelectSymbol_Load(object sender, EventArgs e)
        {
            if (Application.ExecutablePath.ToLower().IndexOf("devenv.exe") <= -1)
            {
                this.updating = true;
                DateTime today = DateTime.Today;
                today = today.AddDays((double) (1 - today.Day));
                ArrayList list = new ArrayList();
                list.Add(new Months("##/##", Globals.ContinousContractExpiry));
                for (int i = 0; i < 30; i++)
                {
                    DateTime itemValue = today.AddMonths(i - 14);
                    list.Add(new Months(itemValue.ToString("yy'/'MM"), itemValue));
                }
                list.Reverse();
                this.expiry.DataSource = list;
                this.expiry.DisplayMember = "ItemDisplay";
                this.expiry.ValueMember = "ItemValue";
                this.expiry.SelectedIndex = 14;
                foreach (Exchange exchange in this.connection.Exchanges.Values)
                {
                    this.exchange.Items.Add(exchange.Name);
                }
                if (this.connection.Exchanges[ExchangeId.Default] != null)
                {
                    this.exchange.SelectedItem = this.connection.Exchanges[ExchangeId.Default].Name;
                }
                else if (this.exchange.Items.Count > 0)
                {
                    this.exchange.SelectedIndex = 0;
                }
                foreach (iTrading.Core .Kernel .SymbolType type in this.connection.SymbolTypes.Values)
                {
                    if (type.Id != SymbolTypeId.Unknown)
                    {
                        this.symbolType.Items.Add(type.Name);
                    }
                }
                if (this.connection.SymbolTypes[SymbolTypeId.Stock] != null)
                {
                    this.symbolType.SelectedItem = this.connection.SymbolTypes[SymbolTypeId.Stock].Name;
                    this.expiry.Enabled = false;
                }
                else if (this.symbolType.Items.Count > 0)
                {
                    this.symbolType.SelectedIndex = 0;
                }
                this.right.Enabled = false;
                this.strikePrice.Enabled = false;
                this.updating = false;
            }
        }

        private void StrikePrice_ValueChanged(object sender, EventArgs e)
        {
            if (!this.updating && (this.connection.FeatureTypes[FeatureTypeId.SymbolLookup] != null))
            {
                this.SymbolNameChanged();
            }
        }

        private void symbolName_Leave(object sender, EventArgs e)
        {
            if (!this.updating && (this.connection.FeatureTypes[FeatureTypeId.SymbolLookup] != null))
            {
                this.SymbolNameChanged();
            }
        }

        private void SymbolNameChanged()
        {
            //todo check this
            RightId lRightID = this.right.Checked ? RightId.Put  : RightId.Call;
           
            if ((((((this.currentSymbol == null) || 
                (this.currentSymbol.Name != this.symbolName.Text)) || 
                ((this.currentSymbol.Exchange.Name != this.exchange.Text) || 
                (this.currentSymbol.SymbolType.Name != this.symbolType.Text))) ||
                (((this.currentSymbol.SymbolType.Id == SymbolTypeId.Future) || 
                (this.currentSymbol.SymbolType.Id == SymbolTypeId.Option)) && 
                ((this.expiry.SelectedValue == null) || 
                (this.currentSymbol.Expiry != ((DateTime) this.expiry.SelectedValue))))) || 
                ((this.currentSymbol.StrikePrice != ((double) this.strikePrice.Value)) && 
                (this.currentSymbol.SymbolType.Id == SymbolTypeId.Option))) ||
                ((this.currentSymbol.Right.Id != lRightID) && 
                (this.currentSymbol.SymbolType.Id == SymbolTypeId.Option)))
            {
                Symbol currentSymbol = this.currentSymbol;
                Symbol symbol = this.Symbol;
                this.currentSymbol = symbol;
                if (((((currentSymbol == null) && (symbol != null)) || ((currentSymbol != null) && (symbol == null))) || (((currentSymbol != null) && (symbol != null)) && (currentSymbol != symbol))) && (this.SelectedSymbolChanged != null))
                {
                    this.SelectedSymbolChanged(this, new SymbolEventArgs(this.Connection, ErrorCode.NoError, "", this.currentSymbol));
                }
            }
        }

        /// <summary>
        /// Disable the expiry Combobox if instrument is a stock.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void symbolType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((this.symbolType.SelectedIndex >= 0) && !this.updating)
            {
                SymbolType type = this.connection.SymbolTypes[SymbolTypeId.Future];
                SymbolType type2 = this.connection.SymbolTypes[SymbolTypeId.Option];
                if ((type != null) && (this.symbolType.SelectedItem.ToString() == type.Name))
                {
                    this.expiry.Enabled = true;
                    this.right.Enabled = false;
                    this.strikePrice.Enabled = false;
                }
                else if ((type2 != null) && (this.symbolType.SelectedItem.ToString() == type2.Name))
                {
                    this.expiry.Enabled = true;
                    this.right.Enabled = true;
                    this.strikePrice.Enabled = true;
                }
                else
                {
                    this.expiry.Enabled = false;
                    this.right.Enabled = false;
                    this.strikePrice.Enabled = false;
                }
                if (this.connection.FeatureTypes[FeatureTypeId.SymbolLookup] != null)
                {
                    this.SymbolNameChanged();
                }
            }
        }

        /// <summary>
        /// Get/set the connection for symbol lookup. Set the connection before this control is loaded.
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

        /// <summary></summary>
        public ComboBox Exchange
        {
            get
            {
                return this.exchange;
            }
        }

        /// <summary></summary>
        public ComboBox Expiry
        {
            get
            {
                return this.expiry;
            }
        }

        /// <summary>
        /// Returns an instance of class <see cref="T:TradeMagic.Cbi.Symbol" />, which matches the selected values.
        /// NULL, if no matching symbol could be found.
        /// </summary>
        public Symbol Symbol
        {
            get
            {
                if (Application.ExecutablePath.ToLower().IndexOf("devenv.exe") <= -1)
                {
                    Exchange exchange = this.connection.Exchanges.Find(this.exchange.Text);
                    RightId unknown = RightId.Unknown;
                    SymbolType symbolType = this.connection.SymbolTypes.Find(this.symbolType.Text);
                    if (this.connection == null)
                    {
                        throw new TMException(ErrorCode.GuiNotInitialized, "SelectSymbol.Connection property is NULL. Control is not initialized properly.");
                    }
                    if (this.symbolName.Text.Length == 0)
                    {
                        return null;
                    }
                    if (symbolType == null)
                    {
                        return null;
                    }
                    if (exchange == null)
                    {
                        return null;
                    }
                    if (symbolType.Id == SymbolTypeId.Option)
                    {
                        unknown = this.right.Checked ? RightId.Put : RightId.Call;
                    }
                    DateTime maxDate = Globals.MaxDate;
                    if (this.expiry.SelectedValue != null)
                    {
                        maxDate = (DateTime) this.expiry.SelectedValue;
                    }
                    Symbol symbol = this.connection.GetSymbol(this.symbolName.Text, maxDate, symbolType, exchange, (double) this.strikePrice.Value, unknown, LookupPolicyId.RepositoryAndProvider);
                    if (symbol != null)
                    {
                        return symbol;
                    }
                    if (this.connection.ConnectionStatusId != ConnectionStatusId.Connected)
                    {
                        this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.NotConnected, "", "Connection is closed. Unable to retrieve data."));
                        this.symbolName.Focus();
                        return null;
                    }
                }
                return null;
            }
            set
            {
                if (value != null)
                {
                    this.updating = true;
                    this.symbolName.Text = value.Name;
                    this.symbolType.SelectedItem = value.SymbolType.Name;
                    this.exchange.SelectedItem = value.Exchange.Name;
                    if ((value.SymbolType.Id == SymbolTypeId.Future) || (value.SymbolType.Id == SymbolTypeId.Option))
                    {
                        this.expiry.SelectedValue = value.Expiry;
                    }
                    if (value.SymbolType.Id == SymbolTypeId.Option)
                    {
                        this.strikePrice.Value = (decimal) value.StrikePrice;
                        this.right.Checked = value.Right.Id == RightId.Put;
                    }
                    this.updating = false;
                }
            }
        }

        /// <summary></summary>
        public TextBox SymbolName
        {
            get
            {
                return this.symbolName;
            }
        }

        /// <summary></summary>
        public ComboBox SymbolType
        {
            get
            {
                return this.symbolType;
            }
        }

        /// <summary>
        /// Used by the expiry ListBox
        /// </summary>
        private class Months
        {
            private string itemDisplay;
            private DateTime itemValue;

            public Months(string ItemDisplay, DateTime ItemValue)
            {
                this.itemDisplay = ItemDisplay;
                this.itemValue = ItemValue;
            }

            public string ItemDisplay
            {
                get
                {
                    return this.itemDisplay;
                }
            }

            public DateTime ItemValue
            {
                get
                {
                    return this.itemValue;
                }
            }
        }
    }
}

