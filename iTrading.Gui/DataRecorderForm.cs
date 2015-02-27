namespace iTrading.Gui
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Xml;
    using iTrading.Core.Kernel;

    /// <summary>
    /// </summary>
    public class DataRecorderForm : Form
    {
        private Button addButton;
        private Container components = null;
        private Connection connection = null;
        private CheckBox marketDepthCheckBox;
        private ListBox recordedSymbols;
        private Button removeButton;
        private bool running = false;
        private SelectSymbol selectSymbol;
        private Button startButton;

        /// <summary>
        /// Form to record market data (including market depth data).
        /// </summary>
        public DataRecorderForm()
        {
            this.InitializeComponent();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            Symbol symbol = this.selectSymbol.Symbol;
            if (symbol == null)
            {
                this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.NoSuchSymbol, "", "Provider does not support this symbol"));
            }
            else
            {
                string item = symbol.ToString();
                for (int i = 0; i < this.recordedSymbols.Items.Count; i++)
                {
                    if (((string) this.recordedSymbols.Items[i]) == item)
                    {
                        this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.UnableToPerformAction, "", "Symbols already is recorded"));
                        return;
                    }
                }
                this.recordedSymbols.Items.Add(item);
            }
        }

        private void DataRecorderForm_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            base.Hide();
            this.Save();
        }

        private void DataRecorderForm_Load(object sender, EventArgs e)
        {
            try
            {
                XmlDocument document = new XmlDocument();
                XmlTextReader reader = new XmlTextReader(Globals.InstallDir + @"\Config.xml");
                document.Load(reader);
                reader.Close();
                this.marketDepthCheckBox.Checked = Convert.ToBoolean(document["TradeMagic"]["Gui"]["DataRecorder"]["MarketDepth"].InnerText);
                this.recordedSymbols.Items.Clear();
                foreach (XmlNode node in document["TradeMagic"]["Gui"]["DataRecorder"]["Symbols"])
                {
                    this.recordedSymbols.Items.Add(node.InnerText);
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
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
            this.selectSymbol = new SelectSymbol();
            this.addButton = new Button();
            this.removeButton = new Button();
            this.recordedSymbols = new ListBox();
            this.marketDepthCheckBox = new CheckBox();
            this.startButton = new Button();
            base.SuspendLayout();
            this.selectSymbol.Connection = null;
            this.selectSymbol.Location = new Point(8, 8);
            this.selectSymbol.Name = "selectSymbol";
            this.selectSymbol.Size = new Size(0x1a8, 0x30);
            this.selectSymbol.Symbol = null;
            this.selectSymbol.TabIndex = 0;
            this.addButton.Location = new Point(0x60, 0x40);
            this.addButton.Name = "addButton";
            this.addButton.TabIndex = 1;
            this.addButton.Text = "&Add";
            this.addButton.Click += new EventHandler(this.addButton_Click);
            this.removeButton.Location = new Point(0x100, 0x40);
            this.removeButton.Name = "removeButton";
            this.removeButton.TabIndex = 2;
            this.removeButton.Text = "&Remove";
            this.removeButton.Click += new EventHandler(this.removeButton_Click);
            this.recordedSymbols.Location = new Point(8, 0x60);
            this.recordedSymbols.Name = "recordedSymbols";
            this.recordedSymbols.SelectionMode = SelectionMode.MultiExtended;
            this.recordedSymbols.Size = new Size(0x1a0, 0xc7);
            this.recordedSymbols.Sorted = true;
            this.recordedSymbols.TabIndex = 3;
            this.marketDepthCheckBox.Location = new Point(0x10, 0x130);
            this.marketDepthCheckBox.Name = "marketDepthCheckBox";
            this.marketDepthCheckBox.Size = new Size(0xa8, 0x18);
            this.marketDepthCheckBox.TabIndex = 4;
            this.marketDepthCheckBox.Text = "including market depth data";
            this.startButton.Location = new Point(0xa8, 0x150);
            this.startButton.Name = "startButton";
            this.startButton.Size = new Size(0x58, 0x17);
            this.startButton.TabIndex = 5;
            this.startButton.Text = "&Start";
            this.startButton.Click += new EventHandler(this.startButton_Click);
            base.AcceptButton = this.startButton;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x1b2, 0x177);
            base.Controls.Add(this.startButton);
            base.Controls.Add(this.marketDepthCheckBox);
            base.Controls.Add(this.recordedSymbols);
            base.Controls.Add(this.removeButton);
            base.Controls.Add(this.addButton);
            base.Controls.Add(this.selectSymbol);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "DataRecorderForm";
            this.Text = "Data recorder";
            base.Closing += new CancelEventHandler(this.DataRecorderForm_Closing);
            base.Load += new EventHandler(this.DataRecorderForm_Load);
            base.ResumeLayout(false);
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            while (this.recordedSymbols.SelectedItem != null)
            {
                this.recordedSymbols.Items.Remove(this.recordedSymbols.SelectedItem);
            }
        }

        private void Save()
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
            document["TradeMagic"]["Gui"].AppendChild(document.CreateElement("DataRecorder"));
            document["TradeMagic"]["Gui"]["DataRecorder"].AppendChild(document.CreateElement("MarketDepth"));
            document["TradeMagic"]["Gui"]["DataRecorder"]["MarketDepth"].InnerText = this.marketDepthCheckBox.Checked.ToString();
            document["TradeMagic"]["Gui"]["DataRecorder"].AppendChild(document.CreateElement("Symbols"));
            foreach (string str in this.recordedSymbols.Items)
            {
                XmlElement newChild = document.CreateElement("Symbol");
                newChild.InnerText = str;
                document["TradeMagic"]["Gui"]["DataRecorder"]["Symbols"].AppendChild(newChild);
            }
            document.Save(Globals.InstallDir + @"\Config.xml");
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            foreach (string str in this.recordedSymbols.Items)
            {
                Symbol symbolByName = this.connection.GetSymbolByName(str);
                if (symbolByName == null)
                {
                    this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.NoSuchSymbol, "", "Symbol '" + str + "' not supported by this broker"));
                }
                else
                {
                    if (!this.running)
                    {
                        symbolByName.MarketData.StartRecorder();
                        if (this.marketDepthCheckBox.Checked)
                        {
                            symbolByName.MarketDepth.StartRecorder();
                        }
                        continue;
                    }
                    symbolByName.MarketData.CancelRecorder();
                    symbolByName.MarketDepth.CancelRecorder();
                }
            }
            if (!this.running)
            {
                this.startButton.Text = "&Stop";
            }
            else
            {
                this.startButton.Text = "&Start";
            }
            this.running = !this.running;
            this.Save();
        }

        /// <summary>
        /// The current connection.
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
                this.selectSymbol.Connection = value;
            }
        }
    }
}

