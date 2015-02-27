using iTrading.Core.Kernel;

namespace iTrading.Gui
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Control to display account values like margin requirements, cash value etc.
    /// </summary>
    public class AccountValues : UserControl
    {
        private Account account = null;
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private Container components = null;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Label label1;
        private Label label10;
        private Label label11;
        private Label label12;
        private Label label13;
        private Label label14;
        private Label label15;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private Label labelBP;
        private Label labelCash;
        private Label labelExcessQuity;
        private Label labelInitMargin;
        private Label labelLongs;
        private Label labelMaintMargin;
        private Label labelNetLiquidation;
        private Label labelNetLiquidationByCurrency;
        private Label labelONMaintMargin;
        private Label labelONMargin;
        private Label labelRProfit;
        private Label labelShorts;
        private Label labelTotalCashBalance;
        private Label labelUProfit;
        private Panel panel2;
        private Panel panel3;
        private Splitter splitter1;

        /// <summary>
        /// 
        /// </summary>
        public AccountValues()
        {
            this.InitializeComponent();
            base.Disposed += new EventHandler(this.AccountValues_Disposed);
        }

        private void AccountValues_AccountUpdate(object sender, AccountUpdateEventArgs e)
        {
            this.UpdateAccount(e.ItemType, e.Currency);
        }

        private void AccountValues_Disposed(object Sender, EventArgs Args)
        {
            this.Account.AccountUpdate -= new AccountUpdateEventHandler(this.AccountValues_AccountUpdate);
        }

        private void AccountValues_Load(object sender, EventArgs e)
        {
            if (Application.ExecutablePath.ToLower().IndexOf("devenv.exe") <= -1)
            {
                if (this.account == null)
                {
                    throw new TMException(ErrorCode.GuiNotInitialized, "AccountValues.Account property is NULL. Control is not initialized properly.");
                }
                if (this.account.Connection.ConnectionStatusId == ConnectionStatusId.Connected)
                {
                    this.Account.AccountUpdate += new AccountUpdateEventHandler(this.AccountValues_AccountUpdate);
                    this.account.PositionUpdate += new PositionUpdateEventHandler(this.AccountValues_PositionUpdate);
                    foreach (Currency currency in this.account.Currencies.Values)
                    {
                        foreach (AccountItemType type in AccountItemType.All.Values)
                        {
                            if (type.Id != AccountItemTypeId.Unknown)
                            {
                                this.UpdateAccount(type, currency);
                            }
                        }
                    }
                    this.label1.Text = "Account Statistics for: " + this.account.Name;
                    this.UpdatePositions();
                }
            }
        }

        private void AccountValues_PositionUpdate(object sender, PositionUpdateEventArgs e)
        {
            if (e.Operation != Operation.Update)
            {
                this.UpdatePositions();
            }
        }

        private void AccountValues_SizeChanged(object sender, EventArgs e)
        {
            this.panel2.Width = base.Width / 2;
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
            this.label1 = new Label();
            this.panel2 = new Panel();
            this.groupBox1 = new GroupBox();
            this.labelNetLiquidationByCurrency = new Label();
            this.label9 = new Label();
            this.labelNetLiquidation = new Label();
            this.label7 = new Label();
            this.labelONMaintMargin = new Label();
            this.labelMaintMargin = new Label();
            this.labelONMargin = new Label();
            this.labelInitMargin = new Label();
            this.labelBP = new Label();
            this.label15 = new Label();
            this.label6 = new Label();
            this.label5 = new Label();
            this.label4 = new Label();
            this.label3 = new Label();
            this.splitter1 = new Splitter();
            this.panel3 = new Panel();
            this.groupBox2 = new GroupBox();
            this.labelTotalCashBalance = new Label();
            this.label10 = new Label();
            this.labelExcessQuity = new Label();
            this.label2 = new Label();
            this.labelUProfit = new Label();
            this.labelRProfit = new Label();
            this.label12 = new Label();
            this.label11 = new Label();
            this.labelShorts = new Label();
            this.label14 = new Label();
            this.labelLongs = new Label();
            this.label13 = new Label();
            this.labelCash = new Label();
            this.label8 = new Label();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            base.SuspendLayout();
            this.label1.Dock = DockStyle.Top;
            this.label1.Font = new Font("Microsoft Sans Serif", 10.04651f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label1.Location = new Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x250, 0x30);
            this.label1.TabIndex = 5;
            this.label1.Text = "Account Statistics for: ";
            this.label1.TextAlign = ContentAlignment.MiddleCenter;
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Dock = DockStyle.Left;
            this.panel2.Location = new Point(0, 0x30);
            this.panel2.Name = "panel2";
            this.panel2.Size = new Size(280, 0x128);
            this.panel2.TabIndex = 7;
            this.groupBox1.Controls.Add(this.labelNetLiquidationByCurrency);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.labelNetLiquidation);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.labelONMaintMargin);
            this.groupBox1.Controls.Add(this.labelMaintMargin);
            this.groupBox1.Controls.Add(this.labelONMargin);
            this.groupBox1.Controls.Add(this.labelInitMargin);
            this.groupBox1.Controls.Add(this.labelBP);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Dock = DockStyle.Fill;
            this.groupBox1.Location = new Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(280, 0x128);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Summary";
            this.labelNetLiquidationByCurrency.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.labelNetLiquidationByCurrency.Location = new Point(0xa8, 0xe8);
            this.labelNetLiquidationByCurrency.Name = "labelNetLiquidationByCurrency";
            this.labelNetLiquidationByCurrency.Size = new Size(0x68, 0x10);
            this.labelNetLiquidationByCurrency.TabIndex = 15;
            this.label9.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.label9.Location = new Point(8, 0xe8);
            this.label9.Name = "label9";
            this.label9.Size = new Size(0x98, 0x10);
            this.label9.TabIndex = 14;
            this.label9.Text = "Net Liquidation by currency";
            this.labelNetLiquidation.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.labelNetLiquidation.Location = new Point(0xa8, 200);
            this.labelNetLiquidation.Name = "labelNetLiquidation";
            this.labelNetLiquidation.Size = new Size(0x68, 0x10);
            this.labelNetLiquidation.TabIndex = 13;
            this.label7.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.label7.Location = new Point(8, 200);
            this.label7.Name = "label7";
            this.label7.Size = new Size(0x98, 0x10);
            this.label7.TabIndex = 12;
            this.label7.Text = "Net Liquidation";
            this.labelONMaintMargin.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.labelONMaintMargin.Location = new Point(0xa8, 0xa8);
            this.labelONMaintMargin.Name = "labelONMaintMargin";
            this.labelONMaintMargin.Size = new Size(0x68, 0x10);
            this.labelONMaintMargin.TabIndex = 11;
            this.labelMaintMargin.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.labelMaintMargin.Location = new Point(0xa8, 0x88);
            this.labelMaintMargin.Name = "labelMaintMargin";
            this.labelMaintMargin.Size = new Size(0x68, 0x10);
            this.labelMaintMargin.TabIndex = 10;
            this.labelONMargin.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.labelONMargin.Location = new Point(0xa8, 0x68);
            this.labelONMargin.Name = "labelONMargin";
            this.labelONMargin.Size = new Size(0x68, 0x10);
            this.labelONMargin.TabIndex = 9;
            this.labelInitMargin.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.labelInitMargin.Location = new Point(0xa8, 0x48);
            this.labelInitMargin.Name = "labelInitMargin";
            this.labelInitMargin.Size = new Size(0x68, 0x10);
            this.labelInitMargin.TabIndex = 8;
            this.labelBP.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.labelBP.Location = new Point(0xa8, 40);
            this.labelBP.Name = "labelBP";
            this.labelBP.Size = new Size(0x68, 0x10);
            this.labelBP.TabIndex = 7;
            this.label15.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.label15.Location = new Point(8, 0xa8);
            this.label15.Name = "label15";
            this.label15.Size = new Size(0x98, 0x10);
            this.label15.TabIndex = 5;
            this.label15.Text = "Maint. Overnight Margin";
            this.label6.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.label6.Location = new Point(8, 0x68);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x98, 0x10);
            this.label6.TabIndex = 4;
            this.label6.Text = "Init Overnight Margin";
            this.label5.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.label5.Location = new Point(8, 0x48);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x98, 0x10);
            this.label5.TabIndex = 3;
            this.label5.Text = "Initial Margin";
            this.label4.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.label4.Location = new Point(8, 0x88);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x98, 0x10);
            this.label4.TabIndex = 2;
            this.label4.Text = "Maintenance Margin";
            this.label3.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.label3.Location = new Point(8, 40);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x98, 0x10);
            this.label3.TabIndex = 1;
            this.label3.Text = "Buying Power";
            this.splitter1.Location = new Point(280, 0x30);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new Size(8, 0x128);
            this.splitter1.TabIndex = 8;
            this.splitter1.TabStop = false;
            this.panel3.Controls.Add(this.groupBox2);
            this.panel3.Dock = DockStyle.Fill;
            this.panel3.Location = new Point(0x120, 0x30);
            this.panel3.Name = "panel3";
            this.panel3.Size = new Size(0x130, 0x128);
            this.panel3.TabIndex = 9;
            this.groupBox2.Controls.Add(this.labelTotalCashBalance);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.labelExcessQuity);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.labelUProfit);
            this.groupBox2.Controls.Add(this.labelRProfit);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.labelShorts);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.labelLongs);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.labelCash);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Dock = DockStyle.Fill;
            this.groupBox2.Location = new Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(0x130, 0x128);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Totals";
            this.labelTotalCashBalance.Location = new Point(160, 0xe8);
            this.labelTotalCashBalance.Name = "labelTotalCashBalance";
            this.labelTotalCashBalance.Size = new Size(0x70, 0x10);
            this.labelTotalCashBalance.TabIndex = 0x17;
            this.label10.Location = new Point(8, 0xe8);
            this.label10.Name = "label10";
            this.label10.Size = new Size(0x88, 0x10);
            this.label10.TabIndex = 0x16;
            this.label10.Text = "Total cash balance";
            this.labelExcessQuity.Location = new Point(160, 200);
            this.labelExcessQuity.Name = "labelExcessQuity";
            this.labelExcessQuity.Size = new Size(0x70, 0x10);
            this.labelExcessQuity.TabIndex = 0x15;
            this.label2.Location = new Point(8, 200);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x88, 0x10);
            this.label2.TabIndex = 20;
            this.label2.Text = "Excess equity";
            this.labelUProfit.Location = new Point(160, 0x88);
            this.labelUProfit.Name = "labelUProfit";
            this.labelUProfit.Size = new Size(0x70, 0x10);
            this.labelUProfit.TabIndex = 0x13;
            this.labelRProfit.Location = new Point(160, 0xa8);
            this.labelRProfit.Name = "labelRProfit";
            this.labelRProfit.Size = new Size(0x70, 0x10);
            this.labelRProfit.TabIndex = 0x12;
            this.label12.Location = new Point(8, 0x88);
            this.label12.Name = "label12";
            this.label12.Size = new Size(0x88, 0x10);
            this.label12.TabIndex = 0x11;
            this.label12.Text = "Unrealized Profit";
            this.label11.Location = new Point(8, 0xa8);
            this.label11.Name = "label11";
            this.label11.Size = new Size(0x88, 0x10);
            this.label11.TabIndex = 0x10;
            this.label11.Text = "Realized Profit";
            this.labelShorts.Location = new Point(160, 0x68);
            this.labelShorts.Name = "labelShorts";
            this.labelShorts.Size = new Size(0x70, 0x10);
            this.labelShorts.TabIndex = 15;
            this.label14.Location = new Point(8, 0x68);
            this.label14.Name = "label14";
            this.label14.Size = new Size(0x88, 0x10);
            this.label14.TabIndex = 14;
            this.label14.Text = "Short Positions";
            this.labelLongs.Location = new Point(160, 0x48);
            this.labelLongs.Name = "labelLongs";
            this.labelLongs.Size = new Size(0x70, 0x10);
            this.labelLongs.TabIndex = 13;
            this.label13.Location = new Point(8, 0x48);
            this.label13.Name = "label13";
            this.label13.Size = new Size(0x88, 0x10);
            this.label13.TabIndex = 12;
            this.label13.Text = "Long Positions";
            this.labelCash.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.labelCash.Location = new Point(160, 40);
            this.labelCash.Name = "labelCash";
            this.labelCash.Size = new Size(0x70, 0x10);
            this.labelCash.TabIndex = 8;
            this.label8.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.label8.Location = new Point(8, 40);
            this.label8.Name = "label8";
            this.label8.Size = new Size(0x88, 0x10);
            this.label8.TabIndex = 3;
            this.label8.Text = "Cash value";
            base.Controls.Add(this.panel3);
            base.Controls.Add(this.splitter1);
            base.Controls.Add(this.panel2);
            base.Controls.Add(this.label1);
            base.Name = "AccountValues";
            base.Size = new Size(0x250, 0x158);
            base.Load += new EventHandler(this.AccountValues_Load);
            base.SizeChanged += new EventHandler(this.AccountValues_SizeChanged);
            this.panel2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void UpdateAccount(AccountItemType itemType, Currency currency)
        {
            switch (itemType.Id)
            {
                case AccountItemTypeId.BuyingPower:
                    this.labelBP.Text = this.account.GetItem(itemType, currency).ToString();
                    return;

                case AccountItemTypeId.CashValue:
                    this.labelCash.Text = this.account.GetItem(itemType, currency).ToString();
                    return;

                case AccountItemTypeId.ExcessEquity:
                    this.labelExcessQuity.Text = this.account.GetItem(itemType, currency).ToString();
                    return;

                case AccountItemTypeId.InitialMargin:
                    this.labelInitMargin.Text = this.account.GetItem(itemType, currency).ToString();
                    return;

                case AccountItemTypeId.InitialMarginOvernight:
                    this.labelONMargin.Text = this.account.GetItem(itemType, currency).ToString();
                    return;

                case AccountItemTypeId.MaintenanceMargin:
                    this.labelMaintMargin.Text = this.account.GetItem(itemType, currency).ToString();
                    return;

                case AccountItemTypeId.MaintenanceMarginOvernight:
                    this.labelONMaintMargin.Text = this.account.GetItem(itemType, currency).ToString();
                    return;

                case AccountItemTypeId.NetLiquidation:
                    this.labelNetLiquidation.Text = this.account.GetItem(itemType, currency).ToString();
                    return;

                case AccountItemTypeId.NetLiquidationByCurrency:
                    this.labelNetLiquidationByCurrency.Text = this.account.GetItem(itemType, currency).ToString();
                    return;

                case AccountItemTypeId.RealizedProfitLoss:
                    this.labelRProfit.Text = this.account.GetItem(itemType, currency).ToString();
                    return;

                case AccountItemTypeId.TotalCashBalance:
                    this.labelTotalCashBalance.Text = this.account.GetItem(itemType, currency).ToString();
                    return;
            }
            this.account.Connection.ProcessEventArgs(new ITradingErrorEventArgs(this.account.Connection, ErrorCode.FeatureNotSupported, "", "AccountItemType not supported: " + itemType.Name));
        }

        private void UpdatePositions()
        {
            int num = 0;
            int num2 = 0;
            foreach (Position position in this.account.Positions)
            {
                if (position.MarketPosition.Id == MarketPositionId.Long)
                {
                    num++;
                }
                else if (position.MarketPosition.Id == MarketPositionId.Short)
                {
                    num2++;
                }
            }
            this.labelLongs.Text = num.ToString();
            this.labelShorts.Text = num2.ToString();
        }

        /// <summary>
        /// Get/set the account. Set the account before this control is loaded.
        /// </summary>
        public Account Account
        {
            get
            {
                return this.account;
            }
            set
            {
                this.account = value;
            }
        }
    }
}

