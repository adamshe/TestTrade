namespace iTrading.Gui
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Resources;
    using System.Windows.Forms;
    using System.Xml;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Form to display market data (quotes and trades) for multiple symbols.
    /// </summary>
    public class MarketDataForm : Form
    {
        private Container components = null;
        private iTrading.Gui.MarketData MarketData;

        /// <summary></summary>
        public MarketDataForm()
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

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            ResourceManager manager = new ResourceManager(typeof(MarketDataForm));
            this.MarketData = new iTrading.Gui.MarketData();
            base.SuspendLayout();
            this.MarketData.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.MarketData.Connection = null;
            this.MarketData.Location = new Point(0, 0);
            this.MarketData.Name = "MarketData";
            this.MarketData.Size = new Size(0x362, 0x199);
            this.MarketData.TabIndex = 0;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x360, 0x195);
            base.Controls.Add(this.MarketData);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "MarketDataForm";
            this.Text = "Market Data";
            base.Closing += new CancelEventHandler(this.MarketDataForm_Closing);
            base.Load += new EventHandler(this.MarketDataForm_Load);
            base.ResumeLayout(false);
        }

        private void MarketDataForm_Closing(object sender, CancelEventArgs e)
        {
            XmlDocument document = new XmlDocument();
            XmlTextReader reader = new XmlTextReader(Globals.InstallDir + @"\Config.xml");
            document.Load(reader);
            reader.Close();
            if (document["TradeMagic"] == null)
            {
                document.AppendChild(document.CreateElement("TradeMagic"));
            }
            if (document["TradeMagic"]["Gui"] == null)
            {
                document["TradeMagic"].AppendChild(document.CreateElement("Gui"));
            }
            document["TradeMagic"]["Gui"].RemoveAll();
            document["TradeMagic"]["Gui"].AppendChild(document.CreateElement("MarketDataForm"));
            document["TradeMagic"]["Gui"]["MarketDataForm"].AppendChild(document.CreateElement("Symbols"));
            foreach (MarketDataGridRow row in this.MarketData.GridRows)
            {
                XmlElement newChild = document.CreateElement("Symbol");
                newChild.InnerText = row.Symbol.ToString();
                document["TradeMagic"]["Gui"]["MarketDataForm"]["Symbols"].AppendChild(newChild);
            }
            document.Save(Globals.InstallDir + @"\Config.xml");
        }

        private void MarketDataForm_Load(object sender, EventArgs e)
        {
            try
            {
                XmlDocument document = new XmlDocument();
                XmlTextReader reader = new XmlTextReader(Globals.InstallDir + @"\Config.xml");
                document.Load(reader);
                reader.Close();
                foreach (XmlNode node in document["TradeMagic"]["Gui"]["MarketDataForm"]["Symbols"])
                {
                    Symbol symbolByName = this.Connection.GetSymbolByName(node.InnerText);
                    if (symbolByName != null)
                    {
                        this.MarketData.AddSymbol(symbolByName);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Get/set the connection for retrieving market data. Set the connection before this control is loaded.
        /// </summary>
        public Connection Connection
        {
            get
            {
                return this.MarketData.Connection;
            }
            set
            {
                this.MarketData.Connection = value;
            }
        }
    }
}

