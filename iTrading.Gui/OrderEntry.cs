using iTrading.Core.Kernel;

namespace iTrading.Gui
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Control to enter orders.
    /// </summary>
    public class OrderEntry : UserControl
    {
        private Account account;
        private ComboBox actionType;
        private Order changedOrder;
        private Container components;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private PriceUpDown limitPrice;
        private Label limitPriceLabel;
        private const string noOcaGroup = "None";
        private ComboBox ocaGroup;
        private ComboBox orderType;
        private NumericUpDown quantity;
        private SelectSymbol selectSymbol;
        private SimpleQuote simpleQuote;
        private PriceUpDown stopPrice;
        private Label stopPriceLabel;
        private Button submitButton;
        private ComboBox tif;

        /// <summary>
        /// Broker Order entry screen for a new order.
        /// </summary>
        public OrderEntry()
        {
            this.components = null;
            this.account = null;
            this.InitializeComponent();
            this.submitButton.Text = "Submit";
        }

        /// <summary>
        /// Broker Order entry screen for updating an order.
        /// Do not use this constructor for new orders.
        /// </summary>
        public OrderEntry(Order order)
        {
            this.components = null;
            this.account = null;
            this.InitializeComponent();
            if (order.OrderId == null)
            {
                throw new TMException(ErrorCode.OrderRejected, "Order.OrderId is NULL.  You must use an open order with a valid OrderId");
            }
            if (order == null)
            {
                throw new TMException(ErrorCode.OrderRejected, "Order is NULL.");
            }
            if (order.Account == null)
            {
                throw new TMException(ErrorCode.OrderRejected, "Order.Account is NULL.");
            }
            this.changedOrder = order;
            this.account = order.Account;
            this.selectSymbol.Connection = this.account.Connection;
            this.simpleQuote.Connection = this.account.Connection;
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
        /// Erforderliche Methode f|r die Designerunterst|tzung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor gedndert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new Label();
            this.orderType = new ComboBox();
            this.limitPrice = new PriceUpDown();
            this.limitPriceLabel = new Label();
            this.stopPriceLabel = new Label();
            this.stopPrice = new PriceUpDown();
            this.submitButton = new Button();
            this.actionType = new ComboBox();
            this.label4 = new Label();
            this.label2 = new Label();
            this.quantity = new NumericUpDown();
            this.simpleQuote = new SimpleQuote();
            this.selectSymbol = new SelectSymbol();
            this.label3 = new Label();
            this.ocaGroup = new ComboBox();
            this.tif = new ComboBox();
            this.label5 = new Label();
            this.limitPrice.BeginInit();
            this.stopPrice.BeginInit();
            this.quantity.BeginInit();
            base.SuspendLayout();
            this.label1.Location = new Point(0x228, 0);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x4d, 0x17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Order T&ype";
            this.label1.TextAlign = ContentAlignment.MiddleCenter;
            this.orderType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.orderType.Location = new Point(0x228, 0x18);
            this.orderType.Name = "orderType";
            this.orderType.Size = new Size(0x55, 0x15);
            this.orderType.Sorted = true;
            this.orderType.TabIndex = 3;
            this.orderType.SelectedIndexChanged += new EventHandler(this.orderType_SelectedIndexChanged);
            this.limitPrice.DecimalPlaces = 2;
            this.limitPrice.Location = new Point(0x278, 0x18);
            int[] bits = new int[4];
            bits[0] = 0xf423f;
            this.limitPrice.Maximum = new decimal(bits);
            this.limitPrice.Name = "limitPrice";
            this.limitPrice.Size = new Size(0x48, 20);
            this.limitPrice.Symbol = null;
            this.limitPrice.TabIndex = 4;
            this.limitPriceLabel.Location = new Point(0x278, 0);
            this.limitPriceLabel.Name = "limitPriceLabel";
            this.limitPriceLabel.Size = new Size(0x48, 0x17);
            this.limitPriceLabel.TabIndex = 4;
            this.limitPriceLabel.Text = "&Limit";
            this.limitPriceLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.stopPriceLabel.Location = new Point(0x2c0, 0);
            this.stopPriceLabel.Name = "stopPriceLabel";
            this.stopPriceLabel.Size = new Size(0x48, 0x17);
            this.stopPriceLabel.TabIndex = 6;
            this.stopPriceLabel.Text = "Sto&p";
            this.stopPriceLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.stopPrice.DecimalPlaces = 2;
            this.stopPrice.Location = new Point(0x2c0, 0x18);
            bits = new int[4];
            bits[0] = 0xf423f;
            this.stopPrice.Maximum = new decimal(bits);
            this.stopPrice.Name = "stopPrice";
            this.stopPrice.Size = new Size(0x48, 20);
            this.stopPrice.Symbol = null;
            this.stopPrice.TabIndex = 5;
            this.submitButton.Location = new Point(0x390, 0x18);
            this.submitButton.Name = "submitButton";
            this.submitButton.Size = new Size(0x40, 0x15);
            this.submitButton.TabIndex = 8;
            this.submitButton.Text = "&Submit";
            this.submitButton.Click += new EventHandler(this.submitButton_Click);
            this.actionType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.actionType.Location = new Point(0x1d8, 0x18);
            this.actionType.Name = "actionType";
            this.actionType.Size = new Size(80, 0x15);
            this.actionType.Sorted = true;
            this.actionType.TabIndex = 2;
            this.label4.Location = new Point(480, 0);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x40, 0x17);
            this.label4.TabIndex = 8;
            this.label4.Text = "&Action";
            this.label4.TextAlign = ContentAlignment.MiddleCenter;
            this.label2.Location = new Point(0x1a8, 0);
            this.label2.Name = "label2";
            this.label2.Size = new Size(40, 0x17);
            this.label2.TabIndex = 10;
            this.label2.Text = "&#";
            this.label2.TextAlign = ContentAlignment.MiddleCenter;
            bits = new int[4];
            bits[0] = 100;
            this.quantity.Increment = new decimal(bits);
            this.quantity.Location = new Point(0x1a0, 0x18);
            bits = new int[4];
            bits[0] = 0xf423f;
            this.quantity.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 1;
            this.quantity.Minimum = new decimal(bits);
            this.quantity.Name = "quantity";
            this.quantity.Size = new Size(0x38, 20);
            this.quantity.TabIndex = 1;
            bits = new int[4];
            bits[0] = 100;
            this.quantity.Value = new decimal(bits);
            this.simpleQuote.Connection = null;
            this.simpleQuote.Dock = DockStyle.Bottom;
            this.simpleQuote.Location = new Point(0, 0x40);
            this.simpleQuote.Name = "simpleQuote";
            this.simpleQuote.Size = new Size(0x3d0, 40);
            this.simpleQuote.TabIndex = 11;
            this.simpleQuote.TabStop = false;
            this.selectSymbol.Connection = null;
            this.selectSymbol.Location = new Point(0, 0);
            this.selectSymbol.Name = "selectSymbol";
            this.selectSymbol.Size = new Size(0x1a0, 0x30);
            this.selectSymbol.Symbol = null;
            this.selectSymbol.TabIndex = 0;
            this.label3.Location = new Point(0x308, 0);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x38, 0x17);
            this.label3.TabIndex = 12;
            this.label3.Text = "Oca Gr.";
            this.label3.TextAlign = ContentAlignment.MiddleCenter;
            this.ocaGroup.DropDownStyle = ComboBoxStyle.DropDownList;
            this.ocaGroup.Location = new Point(0x308, 0x18);
            this.ocaGroup.Name = "ocaGroup";
            this.ocaGroup.Size = new Size(0x40, 0x15);
            this.ocaGroup.TabIndex = 6;
            this.tif.DropDownStyle = ComboBoxStyle.DropDownList;
            this.tif.Location = new Point(840, 0x18);
            this.tif.Name = "tif";
            this.tif.Size = new Size(0x38, 0x15);
            this.tif.TabIndex = 7;
            this.label5.Location = new Point(840, 0);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x30, 0x17);
            this.label5.TabIndex = 14;
            this.label5.Text = "Tif";
            this.label5.TextAlign = ContentAlignment.MiddleCenter;
            base.Controls.Add(this.tif);
            base.Controls.Add(this.label5);
            base.Controls.Add(this.ocaGroup);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.quantity);
            base.Controls.Add(this.actionType);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.submitButton);
            base.Controls.Add(this.stopPriceLabel);
            base.Controls.Add(this.stopPrice);
            base.Controls.Add(this.limitPriceLabel);
            base.Controls.Add(this.limitPrice);
            base.Controls.Add(this.orderType);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.simpleQuote);
            base.Controls.Add(this.selectSymbol);
            base.Name = "OrderEntry";
            base.Size = new Size(0x3d0, 0x68);
            base.Load += new EventHandler(this.OrderEntry_Load);
            this.limitPrice.EndInit();
            this.stopPrice.EndInit();
            this.quantity.EndInit();
            base.ResumeLayout(false);
        }

        private void OrderEntry_Load(object sender, EventArgs e)
        {
            if (Application.ExecutablePath.ToLower().IndexOf("devenv.exe") <= -1)
            {
                if (this.account == null)
                {
                    throw new TMException(ErrorCode.GuiNotInitialized, "OrderEntry.Account property is NULL. Control is not initialized properly.");
                }
                if (this.Account.Connection.ConnectionStatusId == ConnectionStatusId.Connected)
                {
                    if (this.account.IsSimulation)
                    {
                        foreach (ActionType type in ActionType.All.Values)
                        {
                            this.actionType.Items.Add(type.Name);
                        }
                        this.actionType.SelectedItem = ActionType.All[ActionTypeId.Buy].Name;
                        foreach (OrderType type2 in OrderType.All.Values)
                        {
                            if (type2.Id != OrderTypeId.Unknown)
                            {
                                this.orderType.Items.Add(type2.Name);
                            }
                        }
                        this.orderType.SelectedItem = OrderType.All[OrderTypeId.Market].Name;
                        foreach (TimeInForce force in TimeInForce.All.Values)
                        {
                            this.tif.Items.Add(force.Name);
                        }
                        if (TimeInForce.All[TimeInForceId.Day] != null)
                        {
                            this.tif.SelectedItem = TimeInForce.All[TimeInForceId.Day].Name;
                        }
                    }
                    else
                    {
                        foreach (ActionType type3 in this.account.Connection.ActionTypes.Values)
                        {
                            this.actionType.Items.Add(type3.Name);
                        }
                        this.actionType.SelectedItem = this.account.Connection.ActionTypes[ActionTypeId.Buy].Name;
                        foreach (OrderType type4 in this.account.Connection.OrderTypes.Values)
                        {
                            if (type4.Id != OrderTypeId.Unknown)
                            {
                                this.orderType.Items.Add(type4.Name);
                            }
                        }
                        this.orderType.SelectedItem = this.account.Connection.OrderTypes[OrderTypeId.Market].Name;
                        foreach (TimeInForce force2 in this.account.Connection.TimeInForces.Values)
                        {
                            this.tif.Items.Add(force2.Name);
                        }
                        if (this.account.Connection.TimeInForces[TimeInForceId.Day] != null)
                        {
                            this.tif.SelectedItem = this.account.Connection.TimeInForces[TimeInForceId.Day].Name;
                        }
                    }
                    this.ocaGroup.Items.Add("None");
                    for (int i = 1; i < 11; i++)
                    {
                        this.ocaGroup.Items.Add("Grp " + i);
                    }
                    this.ocaGroup.Text = "None";
                    this.selectSymbol.SymbolType.SelectedIndexChanged += new EventHandler(this.SymbolType_SelectedIndexChanged);
                    this.selectSymbol.SelectedSymbolChanged += new SelectedSymbolChangedEventHandler(this.SelectedSymbolChanged);
                    int selectedIndex = this.selectSymbol.SymbolType.SelectedIndex;
                    this.selectSymbol.SymbolType.SelectedIndex = -1;
                    this.selectSymbol.SymbolType.SelectedIndex = selectedIndex;
                    if (this.changedOrder != null)
                    {
                        this.selectSymbol.Symbol = this.changedOrder.Symbol;
                        this.actionType.Text = this.changedOrder.Action.Name;
                        this.orderType.Text = this.changedOrder.OrderType.Name;
                        this.quantity.Value = this.changedOrder.Quantity;
                        this.limitPrice.Value = (decimal) this.changedOrder.LimitPrice;
                        this.limitPrice.Value = (decimal) this.changedOrder.LimitPrice;
                        this.stopPrice.Value = (decimal) this.changedOrder.StopPrice;
                        this.stopPrice.Value = (decimal) this.changedOrder.StopPrice;
                        if (!this.ocaGroup.Items.Contains(this.changedOrder.OcaGroup))
                        {
                            this.ocaGroup.Items.Add(this.changedOrder.OcaGroup);
                        }
                        this.ocaGroup.Text = this.changedOrder.OcaGroup;
                        this.selectSymbol.Enabled = false;
                        this.actionType.Enabled = false;
                        this.orderType.Enabled = false;
                        this.tif.Enabled = false;
                        this.submitButton.Text = "Update";
                    }
                }
            }
        }

        /// <summary>
        /// Show/hide stop and limit textboxes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void orderType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((Application.ExecutablePath.ToLower().IndexOf("devenv.exe") <= -1) && (this.account != null)) && (this.account.Connection.ConnectionStatusId == ConnectionStatusId.Connected))
            {
                switch (OrderType.All.Find(this.orderType.Text).Id)
                {
                    case OrderTypeId.Market:
                        this.limitPrice.Enabled = false;
                        this.limitPrice.Value = 0M;
                        this.limitPrice.Value = 0M;
                        this.limitPriceLabel.Enabled = false;
                        this.stopPrice.Enabled = false;
                        this.stopPrice.Value = 0M;
                        this.stopPrice.Value = 0M;
                        this.stopPriceLabel.Enabled = false;
                        return;

                    case OrderTypeId.Limit:
                        this.limitPrice.Enabled = true;
                        this.limitPriceLabel.Enabled = true;
                        this.stopPrice.Enabled = false;
                        if (this.changedOrder == null)
                        {
                            this.stopPrice.Value = 0M;
                            this.stopPrice.Value = 0M;
                        }
                        this.stopPriceLabel.Enabled = false;
                        if (this.selectSymbol.Symbol != null)
                        {
                            this.limitPrice.Value = (this.selectSymbol.Symbol.MarketData.Last == null) ? ((decimal) 0.0) : ((decimal) this.selectSymbol.Symbol.MarketData.Last.Price);
                            this.limitPrice.Value = (this.selectSymbol.Symbol.MarketData.Last == null) ? ((decimal) 0.0) : ((decimal) this.selectSymbol.Symbol.MarketData.Last.Price);
                        }
                        return;

                    case OrderTypeId.Stop:
                        this.limitPrice.Enabled = false;
                        if (this.changedOrder == null)
                        {
                            this.limitPrice.Value = 0M;
                            this.limitPrice.Value = 0M;
                        }
                        this.limitPriceLabel.Enabled = false;
                        this.stopPrice.Enabled = true;
                        this.stopPriceLabel.Enabled = true;
                        if (this.selectSymbol.Symbol != null)
                        {
                            this.stopPrice.Value = (this.selectSymbol.Symbol.MarketData.Last == null) ? ((decimal) 0.0) : ((decimal) this.selectSymbol.Symbol.MarketData.Last.Price);
                            this.stopPrice.Value = (this.selectSymbol.Symbol.MarketData.Last == null) ? ((decimal) 0.0) : ((decimal) this.selectSymbol.Symbol.MarketData.Last.Price);
                        }
                        return;

                    case OrderTypeId.StopLimit:
                        this.limitPrice.Enabled = true;
                        this.limitPriceLabel.Enabled = true;
                        this.stopPrice.Enabled = true;
                        this.stopPriceLabel.Enabled = true;
                        if ((this.changedOrder == null) && (this.selectSymbol.Symbol != null))
                        {
                            this.stopPrice.Value = (this.selectSymbol.Symbol.MarketData.Last == null) ? ((decimal) 0.0) : ((decimal) this.selectSymbol.Symbol.MarketData.Last.Price);
                            this.limitPrice.Value = (this.selectSymbol.Symbol.MarketData.Last == null) ? ((decimal) 0.0) : ((decimal) this.selectSymbol.Symbol.MarketData.Last.Price);
                            this.stopPrice.Value = (this.selectSymbol.Symbol.MarketData.Last == null) ? ((decimal) 0.0) : ((decimal) this.selectSymbol.Symbol.MarketData.Last.Price);
                            this.limitPrice.Value = (this.selectSymbol.Symbol.MarketData.Last == null) ? ((decimal) 0.0) : ((decimal) this.selectSymbol.Symbol.MarketData.Last.Price);
                        }
                        return;
                }
            }
        }

        private void SelectedSymbolChanged(object sender, SymbolEventArgs e)
        {
            if (e.Symbol != null)
            {
                this.submitButton.Enabled = e.Symbol.SymbolType.Id != SymbolTypeId.Index;
                this.simpleQuote.SelectSymbol(e.Symbol);
                this.stopPrice.Symbol = this.limitPrice.Symbol = e.Symbol;
                this.orderType_SelectedIndexChanged(this, new EventArgs());
            }
        }

        /// <summary>
        /// Sends the order to the broker.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private void submitButton_Click(object sender, EventArgs e)
        {
            if (this.account == null)
            {
                throw new TMException(ErrorCode.GuiNotInitialized, "OrderEntry.Account property is NULL. Control is not initialized properly.");
            }
            if (this.changedOrder == null)
            {
                if (this.selectSymbol.Symbol == null)
                {
                    this.account.Connection.ProcessEventArgs(new ITradingErrorEventArgs(this.account.Connection, ErrorCode.NoSuchSymbol, "", "Provider does not support this symbol"));
                }
                else
                {
                    Order order = this.Account.CreateOrder(this.selectSymbol.Symbol, ActionType.All.Find(this.actionType.Text).Id, OrderType.All.Find(this.orderType.Text).Id, TimeInForce.All.Find(this.tif.Text).Id, (int) this.quantity.Value, (double) this.limitPrice.Value, (double) this.stopPrice.Value, (this.ocaGroup.Text != "None") ? this.ocaGroup.Text : "", null);
                    if (order != null)
                    {
                        if (order.Account.IsSimulation)
                        {
                            order.Submit(SimulationSymbolOptions.GetDefaultSimulationSymbolOptions(order.Symbol));
                        }
                        else
                        {
                            order.Submit();
                        }
                    }
                }
            }
            else
            {
                this.changedOrder.Quantity = (int) this.quantity.Value;
                this.changedOrder.LimitPrice = (double) this.limitPrice.Value;
                this.changedOrder.StopPrice = (double) this.stopPrice.Value;
                this.changedOrder.Change();
                base.ParentForm.Close();
            }
        }

        /// <summary>
        /// Sends the new symbol to simpleQuote.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private void SymbolType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.selectSymbol.SymbolType.SelectedIndex >= 0)
            {
                if (SymbolType.All.Find(this.selectSymbol.SymbolType.Text).Id == SymbolTypeId.Stock)
                {
                    this.quantity.Increment = 100M;
                    if (this.changedOrder == null)
                    {
                        this.quantity.Value = 100M;
                    }
                }
                else
                {
                    this.quantity.Increment = 1M;
                    if (this.changedOrder == null)
                    {
                        this.quantity.Value = 1M;
                    }
                }
            }
        }

        /// <summary>
        /// Get/set the account where the order should be placed. Set the account before this control is loaded.
        /// </summary>
        public Account Account
        {
            get
            {
                return this.account;
            }
            set
            {
                if (value != null)
                {
                    this.account = value;
                    this.selectSymbol.Connection = this.account.Connection;
                    this.simpleQuote.Connection = this.account.Connection;
                }
            }
        }

        /// <summary>
        /// Get LimitPrice control.
        /// </summary>
        public PriceUpDown LimitPrice
        {
            get
            {
                return this.limitPrice;
            }
        }

        /// <summary>
        /// Get StopPrice control.
        /// </summary>
        public PriceUpDown StopPrice
        {
            get
            {
                return this.stopPrice;
            }
        }
    }
}

