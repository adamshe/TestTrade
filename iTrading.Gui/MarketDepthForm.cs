namespace iTrading.Gui
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Resources;
    using System.Windows.Forms;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Form to select a symbol and display it's market depth data.
    /// </summary>
    public class MarketDepthForm : Form
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private Container components = null;
        private iTrading.Gui.MarketDepth marketDepth;

        /// <summary>
        /// 
        /// </summary>
        public MarketDepthForm()
        {
            this.InitializeComponent();
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
            ResourceManager manager = new ResourceManager(typeof(MarketDepthForm));
            this.marketDepth = new iTrading.Gui.MarketDepth();
            base.SuspendLayout();
            this.marketDepth.Connection = null;
            this.marketDepth.Dock = DockStyle.Fill;
            this.marketDepth.Location = new Point(0, 0);
            this.marketDepth.Name = "marketDepth";
            this.marketDepth.Size = new Size(0x2d8, 0x18d);
            this.marketDepth.Symbol = null;
            this.marketDepth.TabIndex = 0;
            this.marketDepth.Load += new EventHandler(this.MarketDepth_Load);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x2d8, 0x18d);
            base.Controls.Add(this.marketDepth);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "MarketDepthForm";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Market Depth";
            base.ResumeLayout(false);
        }

        private void MarketDepth_Load(object sender, EventArgs e)
        {
            this.marketDepth.StartButtonClick += new EventHandler(this.MarketDepth_StartButtonClick);
        }

        private void MarketDepth_StartButtonClick(object sender, EventArgs e)
        {
            this.Text = "Market Depth" + (this.marketDepth.IsRunning ? (" '" + this.marketDepth.SelectSymbol.Symbol.FullName + "'") : "");
        }

        /// <summary>
        /// Get/set the connection for retrieving market depth data. Set the connection before this control is loaded.
        /// </summary>
        public Connection Connection
        {
            get
            {
                return this.marketDepth.Connection;
            }
            set
            {
                this.marketDepth.Connection = value;
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
                return this.marketDepth.Symbol;
            }
            set
            {
                this.marketDepth.Symbol = value;
                this.Text = "Market Depth" + (this.marketDepth.IsRunning ? (" '" + value.FullName + "'") : "");
            }
        }
    }
}

