namespace iTrading.Test
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Reflection;
    using System.Resources;
    using System.Threading;
    using System.Windows.Forms;
    using System.Xml;
    using iTrading.Core.Kernel;

    /// <summary>
    /// </summary>
    public class MainForm : Form
    {
        private ComboBox broker;
        private Button cancelButton;
        private Container components = null;
        private ComboBox environment;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private ComboBox mode;
        private NumericUpDown multipleClients;
        private int numClients = 0;
        private Button okButton;
        private ComboBox testCase;
        private ComboBox testSuite;

        /// <summary>
        /// </summary>
        public MainForm()
        {
            this.InitializeComponent();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Process.GetCurrentProcess().Kill();
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

        private void environment_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (((string) this.environment.SelectedItem))
            {
                case "Local":
                    this.multipleClients.Value = 0M;
                    this.multipleClients.Enabled = false;
                    break;

                case "Server":
                    this.multipleClients.Enabled = true;
                    break;
            }
        }

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            ResourceManager manager = new ResourceManager(typeof(MainForm));
            this.label1 = new Label();
            this.testSuite = new ComboBox();
            this.broker = new ComboBox();
            this.label2 = new Label();
            this.environment = new ComboBox();
            this.label3 = new Label();
            this.mode = new ComboBox();
            this.label4 = new Label();
            this.okButton = new Button();
            this.cancelButton = new Button();
            this.testCase = new ComboBox();
            this.label5 = new Label();
            this.multipleClients = new NumericUpDown();
            this.label6 = new Label();
            this.multipleClients.BeginInit();
            base.SuspendLayout();
            this.label1.Location = new Point(0x10, 0x10);
            this.label1.Name = "label1";
            this.label1.Size = new Size(80, 0x17);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Test:";
            this.label1.TextAlign = ContentAlignment.MiddleLeft;
            this.testSuite.DropDownStyle = ComboBoxStyle.DropDownList;
            this.testSuite.Location = new Point(0x68, 0x10);
            this.testSuite.Name = "testSuite";
            this.testSuite.Size = new Size(0x70, 0x15);
            this.testSuite.Sorted = true;
            this.testSuite.TabIndex = 1;
            this.broker.DropDownStyle = ComboBoxStyle.DropDownList;
            this.broker.Location = new Point(0x68, 80);
            this.broker.Name = "broker";
            this.broker.Size = new Size(0x70, 0x15);
            this.broker.Sorted = true;
            this.broker.TabIndex = 3;
            this.label2.Location = new Point(0x10, 80);
            this.label2.Name = "label2";
            this.label2.Size = new Size(80, 0x17);
            this.label2.TabIndex = 2;
            this.label2.Text = "&Broker:";
            this.label2.TextAlign = ContentAlignment.MiddleLeft;
            this.environment.DropDownStyle = ComboBoxStyle.DropDownList;
            this.environment.Enabled = false;
            this.environment.Location = new Point(0x68, 0x70);
            this.environment.Name = "environment";
            this.environment.Size = new Size(0x70, 0x15);
            this.environment.Sorted = true;
            this.environment.TabIndex = 4;
            this.environment.SelectedIndexChanged += new EventHandler(this.environment_SelectedIndexChanged);
            this.label3.Location = new Point(0x10, 0x70);
            this.label3.Name = "label3";
            this.label3.Size = new Size(80, 0x17);
            this.label3.TabIndex = 4;
            this.label3.Text = "&Environment:";
            this.label3.TextAlign = ContentAlignment.MiddleLeft;
            this.mode.DropDownStyle = ComboBoxStyle.DropDownList;
            this.mode.Location = new Point(0x68, 0x90);
            this.mode.Name = "mode";
            this.mode.Size = new Size(0x70, 0x15);
            this.mode.Sorted = true;
            this.mode.TabIndex = 5;
            this.label4.Location = new Point(0x10, 0x90);
            this.label4.Name = "label4";
            this.label4.Size = new Size(80, 0x17);
            this.label4.TabIndex = 6;
            this.label4.Text = "&Mode:";
            this.label4.TextAlign = ContentAlignment.MiddleLeft;
            this.okButton.Location = new Point(0x20, 0xd8);
            this.okButton.Name = "okButton";
            this.okButton.TabIndex = 7;
            this.okButton.Text = "&Ok";
            this.okButton.Click += new EventHandler(this.okButton_Click);
            this.cancelButton.DialogResult = DialogResult.Cancel;
            this.cancelButton.Location = new Point(0x80, 0xd8);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.TabIndex = 8;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.Click += new EventHandler(this.cancelButton_Click);
            this.testCase.DropDownStyle = ComboBoxStyle.DropDownList;
            this.testCase.Location = new Point(0x68, 0x30);
            this.testCase.Name = "testCase";
            this.testCase.Size = new Size(0x70, 0x15);
            this.testCase.Sorted = true;
            this.testCase.TabIndex = 2;
            this.label5.Location = new Point(0x10, 0x30);
            this.label5.Name = "label5";
            this.label5.Size = new Size(80, 0x17);
            this.label5.TabIndex = 10;
            this.label5.Text = "Test c&ase:";
            this.label5.TextAlign = ContentAlignment.MiddleLeft;
            this.multipleClients.Location = new Point(0x68, 0xb0);
            this.multipleClients.Name = "multipleClients";
            this.multipleClients.Size = new Size(0x40, 20);
            this.multipleClients.TabIndex = 6;
            this.label6.Location = new Point(0x10, 0xb0);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x58, 0x17);
            this.label6.TabIndex = 12;
            this.label6.Text = "M&ultiple clients:";
            this.label6.TextAlign = ContentAlignment.MiddleLeft;
            base.AcceptButton = this.okButton;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this.cancelButton;
            base.ClientSize = new Size(0xea, 0xff);
            base.Controls.Add(this.label6);
            base.Controls.Add(this.multipleClients);
            base.Controls.Add(this.testCase);
            base.Controls.Add(this.label5);
            base.Controls.Add(this.cancelButton);
            base.Controls.Add(this.okButton);
            base.Controls.Add(this.mode);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.environment);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.broker);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.testSuite);
            base.Controls.Add(this.label1);
            base.FormBorderStyle = FormBorderStyle.FixedSingle;
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "MainForm";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "TradeMagic Test";
            base.Load += new EventHandler(this.MainForm_Load);
            this.multipleClients.EndInit();
            base.ResumeLayout(false);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.testSuite.Items.Add("Connect");
            this.testSuite.Items.Add("Data");
            this.testSuite.Items.Add("IBConnect");
            this.testSuite.Items.Add("Indicator");
            this.testSuite.Items.Add("Quotes");
            this.testSuite.Items.Add("Order");
            this.testSuite.Items.Add("Release");
            this.testSuite.Items.Add("SimOrder");
            this.testSuite.SelectedItem = "Order";
            this.testCase.Items.Add("Single");
            this.testCase.Items.Add("SingleInfinite");
            this.testCase.SelectedItem = "Single";
            foreach (ProviderType type in ProviderType.All.Values)
            {
                if (type.Id != ProviderTypeId.Simulation)
                {
                    this.broker.Items.Add(type.Name);
                }
            }
            this.broker.SelectedItem = ProviderType.All[ProviderTypeId.InteractiveBrokers].Name;
            foreach (ModeType type2 in ModeType.All.Values)
            {
                this.mode.Items.Add(type2.Name);
            }
            this.mode.SelectedItem = ModeType.All[ModeTypeId.Demo].Name;
            this.environment.Items.Add("Local");
            this.environment.Items.Add("Server");
            this.environment.SelectedItem = "Local";
            try
            {
                XmlDocument document = new XmlDocument();
                XmlTextReader reader = new XmlTextReader(iTrading.Core.Kernel.Globals.InstallDir + @"\Config.xml");
                document.Load(reader);
                reader.Close();
                this.broker.SelectedItem = document["TradeMagic"]["Test"]["Last"]["Broker"].InnerText;
                this.environment.SelectedItem = document["TradeMagic"]["Test"]["Last"]["Environment"].InnerText;
                this.mode.SelectedItem = document["TradeMagic"]["Test"]["Last"]["Mode"].InnerText;
                this.multipleClients.Value = Convert.ToInt32(document["TradeMagic"]["Test"]["Last"]["MultipleClients"].InnerText);
                this.testCase.SelectedItem = document["TradeMagic"]["Test"]["Last"]["TestCase"].InnerText;
                this.testSuite.SelectedItem = document["TradeMagic"]["Test"]["Last"]["TestSuite"].InnerText;
            }
            catch (Exception)
            {
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            XmlDocument document = new XmlDocument();
            XmlTextReader reader = new XmlTextReader(iTrading.Core.Kernel.Globals.InstallDir + @"\Config.xml");
            document.Load(reader);
            reader.Close();
            if (document["TradeMagic"] == null)
            {
                document.AppendChild(document.CreateElement("TradeMagic"));
            }
            if (document["TradeMagic"]["Test"] == null)
            {
                document["TradeMagic"].AppendChild(document.CreateElement("Test"));
            }
            if (document["TradeMagic"]["Test"]["Last"] == null)
            {
                document["TradeMagic"]["Test"].AppendChild(document.CreateElement("Last"));
            }
            if (document["TradeMagic"]["Test"]["Last"]["TestSuite"] == null)
            {
                document["TradeMagic"]["Test"]["Last"].AppendChild(document.CreateElement("TestSuite"));
            }
            document["TradeMagic"]["Test"]["Last"]["TestSuite"].InnerText = (string) this.testSuite.SelectedItem;
            if (document["TradeMagic"]["Test"]["Last"]["Broker"] == null)
            {
                document["TradeMagic"]["Test"]["Last"].AppendChild(document.CreateElement("Broker"));
            }
            document["TradeMagic"]["Test"]["Last"]["Broker"].InnerText = (string) this.broker.SelectedItem;
            if (document["TradeMagic"]["Test"]["Last"]["Environment"] == null)
            {
                document["TradeMagic"]["Test"]["Last"].AppendChild(document.CreateElement("Environment"));
            }
            document["TradeMagic"]["Test"]["Last"]["Environment"].InnerText = (string) this.environment.SelectedItem;
            if (document["TradeMagic"]["Test"]["Last"]["Mode"] == null)
            {
                document["TradeMagic"]["Test"]["Last"].AppendChild(document.CreateElement("Mode"));
            }
            document["TradeMagic"]["Test"]["Last"]["Mode"].InnerText = (string) this.mode.SelectedItem;
            if (document["TradeMagic"]["Test"]["Last"]["TestCase"] == null)
            {
                document["TradeMagic"]["Test"]["Last"].AppendChild(document.CreateElement("TestCase"));
            }
            document["TradeMagic"]["Test"]["Last"]["TestCase"].InnerText = (string) this.testCase.SelectedItem;
            if (document["TradeMagic"]["Test"]["Last"]["MultipleClients"] == null)
            {
                document["TradeMagic"]["Test"]["Last"].AppendChild(document.CreateElement("MultipleClients"));
            }
            document["TradeMagic"]["Test"]["Last"]["MultipleClients"].InnerText = Convert.ToString((int) this.multipleClients.Value);
            document.Save(iTrading.Core.Kernel.Globals.InstallDir + @"\Config.xml");
            base.Hide();
            if (this.multipleClients.Value > 0M)
            {
                this.numClients = (int) this.multipleClients.Value;
                 iTrading.Test.Globals.startProcesses = new Thread(new ThreadStart(this.StartProcesses));
                 iTrading.Test.Globals.startProcesses.Name = "TM start processes";
                 iTrading.Test.Globals.startProcesses.Start();
                if ( iTrading.Test.Globals.terminateForm != null)
                {
                     iTrading.Test.Globals.terminateForm.Close();
                }
                 iTrading.Test.Globals.terminateForm = new TerminateForm();
                 iTrading.Test.Globals.terminateForm.exitAppOnClick = false;
                 iTrading.Test.Globals.terminateForm.ShowDialog();
            }
            else
            {
                 iTrading.Test.Globals.StartFromConfigFile(true);
            }
            base.Show();
        }

        private void StartProcesses()
        {
             iTrading.Test.Globals.processes.Clear();
             iTrading.Test.Globals.StartServer();
            base.Hide();
            for (int i = 0; i < this.numClients; i++)
            {
                Process process = new Process();
                process.StartInfo.Arguments = "/StartFromConfigFile /NoCleanUpOnConnect /NoServerStartUp";
                process.StartInfo.FileName = Assembly.GetExecutingAssembly().Location;
                process.StartInfo.UseShellExecute = false;
                process.Start();
                process.WaitForInputIdle();
                 iTrading.Test.Globals.processes.Add(process);
            }
            for (int j = 0; j <  iTrading.Test.Globals.processes.Count; j++)
            {
                ((Process)  iTrading.Test.Globals.processes[j]).WaitForExit();
            }
             iTrading.Test.Globals.processes.Clear();
             iTrading.Test.Globals.TerminateServer();
            if ( iTrading.Test.Globals.terminateForm != null)
            {
                 iTrading.Test.Globals.terminateForm.Close();
                 iTrading.Test.Globals.terminateForm = null;
            }
        }
    }
}

