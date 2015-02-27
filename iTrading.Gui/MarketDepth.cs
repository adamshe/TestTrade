namespace iTrading.Gui
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Globalization;
    using System.Windows.Forms;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Control for displaying level II data.
    /// </summary>
    public class MarketDepth : UserControl
    {
        private ListBox askListBox;
        private ListBox bidListBox;
        private Container components = null;
        private bool isRunning = false;
        private bool resizedAfterData = false;
        private iTrading.Gui.SelectSymbol selectSymbol;
        private Button startButton;
        private Symbol symbol = null;

        /// <summary>
        /// This event will be thrown, when the "Start/Stop" button was clicked.
        /// </summary>
        public event EventHandler StartButtonClick
        {
            add
            {
                this.startButton.Click += value;
            }
            remove
            {
                this.startButton.Click -= value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public MarketDepth()
        {
            this.InitializeComponent();
            base.Disposed += new EventHandler(this.MarketDepth_Disposed);
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
            this.selectSymbol = new iTrading.Gui.SelectSymbol();
            this.startButton = new Button();
            this.bidListBox = new ListBox();
            this.askListBox = new ListBox();
            base.SuspendLayout();
            this.selectSymbol.Connection = null;
            this.selectSymbol.Location = new Point(0, 0);
            this.selectSymbol.Name = "selectSymbol";
            this.selectSymbol.Size = new Size(0x1a0, 0x30);
            this.selectSymbol.Symbol = null;
            this.selectSymbol.TabIndex = 0;
            this.startButton.Location = new Point(440, 0x18);
            this.startButton.Name = "startButton";
            this.startButton.Size = new Size(70, 20);
            this.startButton.TabIndex = 1;
            this.startButton.Text = "S&tart";
            this.startButton.Click += new EventHandler(this.StartButton_Click);
            this.bidListBox.Font = new Font("Courier New", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.bidListBox.ItemHeight = 0x10;
            this.bidListBox.Location = new Point(0, 0x38);
            this.bidListBox.Name = "bidListBox";
            this.bidListBox.Size = new Size(0x138, 0x144);
            this.bidListBox.TabIndex = 2;
            this.askListBox.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.askListBox.Font = new Font("Courier New", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.askListBox.ItemHeight = 0x10;
            this.askListBox.Location = new Point(0x138, 0x38);
            this.askListBox.Name = "askListBox";
            this.askListBox.Size = new Size(0x138, 0x144);
            this.askListBox.TabIndex = 3;
            base.Controls.Add(this.selectSymbol);
            base.Controls.Add(this.askListBox);
            base.Controls.Add(this.bidListBox);
            base.Controls.Add(this.startButton);
            base.Name = "MarketDepth";
            base.Size = new Size(0x270, 0x188);
            base.Load += new EventHandler(this.MarketDepth_Load);
            base.SizeChanged += new EventHandler(this.MarketDepth_SizeChanged);
            base.ResumeLayout(false);
        }

        /// <summary>
        /// Catch the enter key.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MarketData_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                e.Handled = true;
                this.StartButton_Click(this, new EventArgs());
            }
        }

        private void MarketDepth_Disposed(object Sender, EventArgs Args)
        {
            if (this.symbol != null)
            {
                this.symbol.MarketDepth.MarketDepthItem -= new MarketDepthItemEventHandler(this.MarketDepth_MarketDepthItem);
            }
        }

        private void MarketDepth_Load(object sender, EventArgs e)
        {
            this.selectSymbol.SymbolName.KeyPress += new KeyPressEventHandler(this.MarketData_KeyPress);
            this.selectSymbol.SymbolType.KeyPress += new KeyPressEventHandler(this.MarketData_KeyPress);
            this.selectSymbol.Exchange.KeyPress += new KeyPressEventHandler(this.MarketData_KeyPress);
            this.selectSymbol.Expiry.KeyPress += new KeyPressEventHandler(this.MarketData_KeyPress);
        }

        private void MarketDepth_MarketDepthItem(object sender, MarketDepthEventArgs e)
        {
            ListBox askListBox;
            if (e.Error == ErrorCode.NoError)
            {
                askListBox = null;
                if (e.MarketDataType.Id == MarketDataTypeId.Ask)
                {
                    askListBox = this.askListBox;
                    goto Label_003E;
                }
                if (e.MarketDataType.Id == MarketDataTypeId.Bid)
                {
                    askListBox = this.bidListBox;
                    goto Label_003E;
                }
            }
            return;
        Label_003E:
            if (e.Operation == Operation.Delete)
            {
                askListBox.Items.RemoveAt(e.Position);
            }
            else
            {
                string item = string.Format("{0, -6}{1, 10}{2, 10}{3, 10}", new object[] { e.MarketMaker, this.Symbol.FormatPrice(e.Price), e.Volume, e.Time.ToString(CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern) });
                if (e.Operation == Operation.Insert)
                {
                    askListBox.Items.Insert(e.Position, item);
                }
                else
                {
                    askListBox.Items[e.Position] = item;
                }
                if (!this.resizedAfterData)
                {
                    this.MarketDepth_SizeChanged(this, new EventArgs());
                    this.resizedAfterData = true;
                }
            }
        }

        private void MarketDepth_SizeChanged(object sender, EventArgs e)
        {
            this.bidListBox.Size = new Size(base.Width / 2, base.Height - this.bidListBox.Location.Y);
            this.askListBox.Size = new Size(base.Width / 2, base.Height - this.askListBox.Location.Y);
            this.askListBox.Location = new Point((base.Width / 2) + 1, this.askListBox.Location.Y);
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            if (this.symbol == null)
            {
                this.symbol = this.selectSymbol.Symbol;
                if (this.symbol == null)
                {
                    this.selectSymbol.Connection.ProcessEventArgs(new ITradingErrorEventArgs(this.selectSymbol.Connection, ErrorCode.NoSuchSymbol, "", "Provider does not support this symbol"));
                }
                else
                {
                    this.symbol.MarketDepth.MarketDepthItem += new MarketDepthItemEventHandler(this.MarketDepth_MarketDepthItem);
                    this.startButton.Text = "S&top";
                    this.isRunning = true;
                }
            }
            else
            {
                this.symbol.MarketDepth.MarketDepthItem -= new MarketDepthItemEventHandler(this.MarketDepth_MarketDepthItem);
                this.symbol = null;
                this.startButton.Text = "S&tart";
                this.isRunning = false;
            }
        }

        /// <summary>
        /// Get/set the connection for retrieving level II data. Set the connection before this control is loaded.
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

        /// <summary>
        /// Indicates the status.
        /// </summary>
        public bool IsRunning
        {
            get
            {
                return this.isRunning;
            }
        }

        /// <summary>
        /// Returns the <see cref="T:iTrading.Gui.SelectSymbol" /> component.
        /// </summary>
        public iTrading.Gui.SelectSymbol SelectSymbol
        {
            get
            {
                return this.selectSymbol;
            }
        }

        /// <summary>
        /// Set or Gets the symbol for the Market Depth.
        /// Setting the symbol will start the data stream.
        /// </summary>
        public Symbol Symbol
        {
            get
            {
                return this.selectSymbol.Symbol;
            }
            set
            {
                if (value != null)
                {
                    this.selectSymbol.Symbol = value;
                    this.StartButton_Click(this, new EventArgs());
                }
            }
        }
    }
}

