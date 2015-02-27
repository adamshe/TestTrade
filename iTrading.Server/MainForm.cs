namespace TradeMagic.Server
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Resources;
    using System.Windows.Forms;
    using System.Xml;
    using iTrading.Core.Kernel ;

    public class MainForm : Form
    {
        private ClientHandler clientHandler = null;
        private GroupBox clientsGroupBox;
        private ListBox clientsListBox;
        private IContainer components;
        private GroupBox configurationGoupBox;
        private NumericUpDown connectionsNumericUpDown;
        private ContextMenu contextMenu;
        private ErrorEventArgsHandler errorEventArgsHandler = null;
        private TabPage errorsTabPage;
        private TextBox errorsTextBox;
        private Button exitButton;
        private MenuItem exitMenuItem;
        private TabPage generalTabPage;
        private Label label1;
        private Label label2;
        private NotifyIcon notifyIcon;
        private NumericUpDown portNumericUpDown;
        private Control processMessage = null;
        private Button SaveButton;
        private TradeMagic.Server.Server server;
        private MenuItem showMenuItem;
        private TabControl tabControl;
        private Button terminateButton;
        private iTrading.Gui.TraceLog TraceLog;
        private TabPage traceTabPage;

        internal MainForm(TradeMagic.Server.Server server)
        {
            this.InitializeComponent();
            this.clientHandler = new ClientHandler(this.ProcessClient);
            this.errorEventArgsHandler = new ErrorEventArgsHandler(this.ProcessErrorEventArgs);
            this.processMessage = new Control();
            this.server = server;
            this.TraceLog.ModuleName = "Server";
            this.processMessage.CreateControl();
            base.Hide();
            string installDir = Globals.InstallDir;
            if (!File.Exists(installDir + @"\Config.xml"))
            {
                throw new TMException(ErrorCode.Panic, "TradeMagic is not installed properly, Config.xml file is missing");
            }
            XmlDocument document = new XmlDocument();
            XmlTextReader reader = new XmlTextReader(installDir + @"\Config.xml");
            document.Load(reader);
            reader.Close();
            try
            {
                this.connectionsNumericUpDown.Value = Convert.ToInt32(document["TradeMagic"]["Server"]["Connections"].InnerText, 10);
                this.portNumericUpDown.Value = Convert.ToInt32(document["TradeMagic"]["Server"]["Port"].InnerText, 10);
                server.maxConnections = (int) this.connectionsNumericUpDown.Value;
                server.port = (int) this.portNumericUpDown.Value;
            }
            catch (Exception)
            {
                this.connectionsNumericUpDown.Value = server.maxConnections;
                this.portNumericUpDown.Value = server.port;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        internal void Error(ErrorCode errorCode, string nativeError, string message)
        {
            this.processMessage.Invoke(this.errorEventArgsHandler, new object[] { errorCode, nativeError, message });
        }

        private void Exit()
        {
            if ((this.server.clients.Count == 0) || (MessageBox.Show("Do you want to exit TradeMagic server and terminate any pending connection ?", "TradeMagic server", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes))
            {
                this.server.Exit();
            }
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            this.Exit();
        }

        private void exitMenuItem_Click(object sender, EventArgs e)
        {
            this.Exit();
        }

        internal void HandleClient(Client client, bool add)
        {
            this.processMessage.Invoke(this.clientHandler, new object[] { client, add });
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ResourceManager manager = new ResourceManager(typeof(MainForm));
            this.TraceLog = new iTrading.Gui.TraceLog();
            this.contextMenu = new ContextMenu();
            this.showMenuItem = new MenuItem();
            this.exitMenuItem = new MenuItem();
            this.notifyIcon = new NotifyIcon(this.components);
            this.generalTabPage = new TabPage();
            this.clientsGroupBox = new GroupBox();
            this.clientsListBox = new ListBox();
            this.terminateButton = new Button();
            this.configurationGoupBox = new GroupBox();
            this.connectionsNumericUpDown = new NumericUpDown();
            this.portNumericUpDown = new NumericUpDown();
            this.label1 = new Label();
            this.SaveButton = new Button();
            this.label2 = new Label();
            this.exitButton = new Button();
            this.errorsTabPage = new TabPage();
            this.errorsTextBox = new TextBox();
            this.tabControl = new TabControl();
            this.traceTabPage = new TabPage();
            this.generalTabPage.SuspendLayout();
            this.clientsGroupBox.SuspendLayout();
            this.configurationGoupBox.SuspendLayout();
            this.connectionsNumericUpDown.BeginInit();
            this.portNumericUpDown.BeginInit();
            this.errorsTabPage.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.traceTabPage.SuspendLayout();
            base.SuspendLayout();
            this.TraceLog.Dock = DockStyle.Fill;
            this.TraceLog.Location = new Point(0, 0);
            this.TraceLog.ModuleName = "";
            this.TraceLog.Name = "TraceLog";
            this.TraceLog.Size = new Size(0x288, 0x16b);
            this.TraceLog.TabIndex = 0;
            this.contextMenu.MenuItems.AddRange(new MenuItem[] { this.showMenuItem, this.exitMenuItem });
            this.showMenuItem.Index = 0;
            this.showMenuItem.Text = "&Show ...";
            this.showMenuItem.Click += new EventHandler(this.showMenuItem_Click);
            this.exitMenuItem.Index = 1;
            this.exitMenuItem.Text = "&Exit";
            this.exitMenuItem.Click += new EventHandler(this.exitMenuItem_Click);
            this.notifyIcon.ContextMenu = this.contextMenu;
            this.notifyIcon.Icon = (Icon) manager.GetObject("notifyIcon.Icon");
            this.notifyIcon.Text = "TradeMagic server";
            this.notifyIcon.Visible = true;
            this.notifyIcon.DoubleClick += new EventHandler(this.showMenuItem_Click);
            this.generalTabPage.Controls.Add(this.clientsGroupBox);
            this.generalTabPage.Controls.Add(this.configurationGoupBox);
            this.generalTabPage.Controls.Add(this.exitButton);
            this.generalTabPage.Location = new Point(4, 0x16);
            this.generalTabPage.Name = "generalTabPage";
            this.generalTabPage.Size = new Size(0x288, 0x16b);
            this.generalTabPage.TabIndex = 0;
            this.generalTabPage.Text = "General";
            this.clientsGroupBox.Controls.Add(this.clientsListBox);
            this.clientsGroupBox.Controls.Add(this.terminateButton);
            this.clientsGroupBox.Location = new Point(0x138, 8);
            this.clientsGroupBox.Name = "clientsGroupBox";
            this.clientsGroupBox.Size = new Size(0x148, 0x160);
            this.clientsGroupBox.TabIndex = 6;
            this.clientsGroupBox.TabStop = false;
            this.clientsGroupBox.Text = "Clients";
            this.clientsListBox.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.clientsListBox.Location = new Point(8, 0x10);
            this.clientsListBox.Name = "clientsListBox";
            this.clientsListBox.SelectionMode = SelectionMode.MultiSimple;
            this.clientsListBox.Size = new Size(0x138, 0x115);
            this.clientsListBox.TabIndex = 0;
            this.terminateButton.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.terminateButton.Location = new Point(0x80, 0x138);
            this.terminateButton.Name = "terminateButton";
            this.terminateButton.TabIndex = 2;
            this.terminateButton.Text = "&Terminate";
            this.terminateButton.Click += new EventHandler(this.terminateButton_Click);
            this.configurationGoupBox.Controls.Add(this.connectionsNumericUpDown);
            this.configurationGoupBox.Controls.Add(this.portNumericUpDown);
            this.configurationGoupBox.Controls.Add(this.label1);
            this.configurationGoupBox.Controls.Add(this.SaveButton);
            this.configurationGoupBox.Controls.Add(this.label2);
            this.configurationGoupBox.Location = new Point(8, 8);
            this.configurationGoupBox.Name = "configurationGoupBox";
            this.configurationGoupBox.Size = new Size(0x128, 0x98);
            this.configurationGoupBox.TabIndex = 5;
            this.configurationGoupBox.TabStop = false;
            this.configurationGoupBox.Text = "Configuration";
            this.connectionsNumericUpDown.Location = new Point(0x88, 0x40);
            int[] bits = new int[4];
            bits[0] = 0x186a0;
            this.connectionsNumericUpDown.Maximum = new decimal(bits);
            this.connectionsNumericUpDown.Name = "connectionsNumericUpDown";
            this.connectionsNumericUpDown.Size = new Size(0x58, 20);
            this.connectionsNumericUpDown.TabIndex = 8;
            this.portNumericUpDown.Location = new Point(0x88, 0x20);
            bits = new int[4];
            bits[0] = 0x186a0;
            this.portNumericUpDown.Maximum = new decimal(bits);
            this.portNumericUpDown.Name = "portNumericUpDown";
            this.portNumericUpDown.Size = new Size(0x58, 20);
            this.portNumericUpDown.TabIndex = 7;
            this.label1.Location = new Point(0x10, 0x40);
            this.label1.Name = "label1";
            this.label1.Size = new Size(80, 0x17);
            this.label1.TabIndex = 6;
            this.label1.Text = "# Connections:";
            this.label1.TextAlign = ContentAlignment.MiddleLeft;
            this.SaveButton.Location = new Point(0x60, 0x70);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.TabIndex = 5;
            this.SaveButton.Text = "&Save";
            this.SaveButton.Click += new EventHandler(this.SaveButton_Click);
            this.label2.Location = new Point(0x10, 0x20);
            this.label2.Name = "label2";
            this.label2.Size = new Size(80, 0x17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Port:";
            this.label2.TextAlign = ContentAlignment.MiddleLeft;
            this.exitButton.Location = new Point(0x68, 0xd0);
            this.exitButton.Name = "exitButton";
            this.exitButton.TabIndex = 9;
            this.exitButton.Text = "&Exit";
            this.exitButton.Click += new EventHandler(this.exitButton_Click);
            this.errorsTabPage.Controls.Add(this.errorsTextBox);
            this.errorsTabPage.Location = new Point(4, 0x16);
            this.errorsTabPage.Name = "errorsTabPage";
            this.errorsTabPage.Size = new Size(0x288, 0x16b);
            this.errorsTabPage.TabIndex = 1;
            this.errorsTabPage.Text = "Errors";
            this.errorsTextBox.Dock = DockStyle.Fill;
            this.errorsTextBox.Location = new Point(0, 0);
            this.errorsTextBox.Multiline = true;
            this.errorsTextBox.Name = "errorsTextBox";
            this.errorsTextBox.ScrollBars = ScrollBars.Both;
            this.errorsTextBox.Size = new Size(0x288, 0x16b);
            this.errorsTextBox.TabIndex = 0;
            this.errorsTextBox.Text = "";
            this.errorsTextBox.WordWrap = false;
            this.tabControl.Controls.Add(this.generalTabPage);
            this.tabControl.Controls.Add(this.errorsTabPage);
            this.tabControl.Controls.Add(this.traceTabPage);
            this.tabControl.Dock = DockStyle.Fill;
            this.tabControl.Location = new Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new Size(0x290, 0x185);
            this.tabControl.TabIndex = 0;
            this.traceTabPage.Controls.Add(this.TraceLog);
            this.traceTabPage.Location = new Point(4, 0x16);
            this.traceTabPage.Name = "traceTabPage";
            this.traceTabPage.Size = new Size(0x288, 0x16b);
            this.traceTabPage.TabIndex = 2;
            this.traceTabPage.Text = "Trace";
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x290, 0x185);
            base.Controls.Add(this.tabControl);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.MinimumSize = new Size(0x1f8, 320);
            base.Name = "MainForm";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "TradeMagic server";
            base.Closing += new CancelEventHandler(this.MainForm_Closing);
            this.generalTabPage.ResumeLayout(false);
            this.clientsGroupBox.ResumeLayout(false);
            this.configurationGoupBox.ResumeLayout(false);
            this.connectionsNumericUpDown.EndInit();
            this.portNumericUpDown.EndInit();
            this.errorsTabPage.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.traceTabPage.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void MainForm_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            base.Hide();
        }

        private void ProcessClient(Client client, bool add)
        {
            if (add)
            {
                this.clientsListBox.Items.Add(client.ListBoxItem);
            }
            else
            {
                this.clientsListBox.Items.Remove(client.ListBoxItem);
            }
        }

        private void ProcessErrorEventArgs(ErrorCode errorCode, string nativeError, string message)
        {
            string str = message + ((nativeError.Length == 0) ? "" : (": " + nativeError)) + " (" + errorCode.ToString("g") + ")";
            Trace.WriteLine(str);
            this.errorsTextBox.AppendText(DateTime.Now.ToString("HH:mm:ss ") + str + "\r\n");
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            string installDir = Globals.InstallDir;
            if (!File.Exists(installDir + @"\Config.xml"))
            {
                throw new TMException(ErrorCode.Panic, "TradeMagic is not installed properly, Config.xml file is missing");
            }
            XmlDocument document = new XmlDocument();
            XmlTextReader reader = new XmlTextReader(installDir + @"\Config.xml");
            document.Load(reader);
            reader.Close();
            if (document["TradeMagic"] == null)
            {
                XmlElement newChild = document.CreateElement("TradeMagic");
                document.AppendChild(newChild);
            }
            if (document["TradeMagic"]["Server"] == null)
            {
                XmlElement element2 = document.CreateElement("Server");
                document["TradeMagic"].AppendChild(element2);
            }
            if (document["TradeMagic"]["Server"]["Port"] == null)
            {
                XmlElement element3 = document.CreateElement("Port");
                document["TradeMagic"]["Server"].AppendChild(element3);
            }
            if (document["TradeMagic"]["Server"]["Connections"] == null)
            {
                XmlElement element4 = document.CreateElement("Connections");
                document["TradeMagic"]["Server"].AppendChild(element4);
            }
            document["TradeMagic"]["Server"]["Connections"].InnerText = this.connectionsNumericUpDown.Value.ToString();
            document["TradeMagic"]["Server"]["Port"].InnerText = this.portNumericUpDown.Value.ToString();
            document.Save(installDir + @"\Config.xml");
            MessageBox.Show("Settings saved. They become effective after TradeMagic server restart.", "TradeMagic server", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void showMenuItem_Click(object sender, EventArgs e)
        {
            if (base.WindowState == FormWindowState.Minimized)
            {
                base.WindowState = FormWindowState.Normal;
            }
            base.Show();
        }

        private void terminateButton_Click(object sender, EventArgs e)
        {
            lock (this.server.clients)
            {
                ArrayList list = new ArrayList();
                foreach (string str in this.clientsListBox.SelectedItems)
                {
                    foreach (Client client in this.server.clients)
                    {
                        if (str == client.ListBoxItem)
                        {
                            list.Add(client);
                        }
                    }
                }
                foreach (Client client2 in list)
                {
                    client2.Close();
                }
            }
        }

        private delegate void ClientHandler(Client client, bool add);

        private delegate void ErrorEventArgsHandler(ErrorCode errorCode, string nativeError, string message);

        private delegate void LogHandler(string message);
    }
}

