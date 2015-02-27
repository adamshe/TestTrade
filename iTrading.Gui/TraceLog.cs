namespace iTrading.Gui
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;
    using System.Xml;
    using iTrading.Core.Kernel;

    /// <summary>
    /// </summary>
    public class TraceLog : UserControl
    {
        private Container components = null;
        private Label label3;
        private LogHandler logHandler = null;
        private string moduleName = "";
        private Control processMessage = null;
        private Button setTraceLevelButton;
        private CheckBox toFileOnlyCheckBox;
        private ListBox traceLevelListBox;
        private iTrading.Gui.TraceListener traceListener = null;
        private RichTextBox traceTextBox;

        /// <summary>
        /// Use this control to setup and save TradeMagic trace levels and to visualize trace logs.
        /// Creating this control removes any listener from "Trace.Listeners" and inserts a new listener 
        /// derived from "DefaultTraceListener".
        /// </summary>
        public TraceLog()
        {
            this.InitializeComponent();
            this.Init();
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
            System.Diagnostics.Trace.Listeners.Clear();
            if (this.traceListener != null)
            {
                this.traceListener.Close();
            }
            this.traceListener = null;
        }

        private TraceLevelIds Gui2TraceLevelId()
        {
            TraceLevelIds none = TraceLevelIds.None;
            foreach (iTrading.Core.Kernel.TraceLevel level in iTrading.Core.Kernel.TraceLevel.All.Values)
            {
                if (this.traceLevelListBox.SelectedItems.Contains(level.Name))
                {
                    none |= level.Id;
                }
            }
            return none;
        }

        private void Init()
        {
            this.logHandler = new LogHandler(this.ProcessLog);
            this.processMessage = new Control();
            this.processMessage.CreateControl();
            if (this.traceListener != null)
            {
                this.traceListener.Close();
            }
            this.traceLevelListBox.Items.Clear();
            foreach (iTrading.Core.Kernel.TraceLevel level in iTrading.Core.Kernel.TraceLevel.All.Values)
            {
                if ((level.Id != TraceLevelIds.None) && (level.Id != TraceLevelIds.All))
                {
                    this.traceLevelListBox.Items.Add(level.Name);
                }
            }
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
                if (this.ModuleName.Length == 0)
                {
                    this.TraceLevelId2Gui((TraceLevelIds) Convert.ToInt32(document["TradeMagic"]["TraceLevel"].InnerText, 10));
                    this.toFileOnlyCheckBox.Checked = Convert.ToBoolean(document["TradeMagic"]["Trace2FileOnly"].InnerText);
                }
                else
                {
                    this.TraceLevelId2Gui((TraceLevelIds) Convert.ToInt32(document["TradeMagic"][this.ModuleName]["TraceLevel"].InnerText, 10));
                    this.toFileOnlyCheckBox.Checked = Convert.ToBoolean(document["TradeMagic"][this.ModuleName]["Trace2FileOnly"].InnerText);
                }
                Globals.TraceSwitch.Level = this.Gui2TraceLevelId();
            }
            catch (Exception)
            {
            }
            string path = "";
            StreamWriter logFile = null;
            for (int i = 0; (i < 100) && (logFile == null); i++)
            {
                path = Globals.InstallDir + @"trace\" + ((this.moduleName.Length != 0) ? this.moduleName : "TradeMagic") + ((i == 0) ? "" : i.ToString()) + ".txt";
                try
                {
                    logFile = new StreamWriter(path);
                }
                catch (Exception)
                {
                }
            }
            if (logFile == null)
            {
                throw new TMException(ErrorCode.NoError, "Unable to open log file '" + path + "'. This file may be locked by another process.");
            }
            this.traceListener = new iTrading.Gui.TraceListener(this, logFile);
            System.Diagnostics.Trace.Listeners.Clear();
            System.Diagnostics.Trace.Listeners.Add(this.traceListener);
        }

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.setTraceLevelButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.traceLevelListBox = new System.Windows.Forms.ListBox();
            this.toFileOnlyCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // setTraceLevelButton
            // 
            this.setTraceLevelButton.Location = new System.Drawing.Point(24, 312);
            this.setTraceLevelButton.Name = "setTraceLevelButton";
            this.setTraceLevelButton.Size = new System.Drawing.Size(75, 23);
            this.setTraceLevelButton.TabIndex = 3;
            this.setTraceLevelButton.Text = "&Set";
            this.setTraceLevelButton.Click += new System.EventHandler(this.setTraceLevelButton_Click);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(8, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 23);
            this.label3.TabIndex = 5;
            this.label3.Text = "Trace levels";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // traceLevelListBox
            // 
            this.traceLevelListBox.Location = new System.Drawing.Point(8, 34);
            this.traceLevelListBox.Name = "traceLevelListBox";
            this.traceLevelListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.traceLevelListBox.Size = new System.Drawing.Size(120, 238);
            this.traceLevelListBox.Sorted = true;
            this.traceLevelListBox.TabIndex = 1;
            // 
            // toFileOnlyCheckBox
            // 
            this.toFileOnlyCheckBox.Location = new System.Drawing.Point(8, 280);
            this.toFileOnlyCheckBox.Name = "toFileOnlyCheckBox";
            this.toFileOnlyCheckBox.Size = new System.Drawing.Size(104, 24);
            this.toFileOnlyCheckBox.TabIndex = 2;
            this.toFileOnlyCheckBox.Text = "to &file only";
            this.toFileOnlyCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TraceLog
            // 
            this.Controls.Add(this.toFileOnlyCheckBox);
            this.Controls.Add(this.setTraceLevelButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.traceLevelListBox);
            this.Name = "TraceLog";
            this.Size = new System.Drawing.Size(520, 376);
            this.Load += new System.EventHandler(this.TraceLog_Load);
            this.ResumeLayout(false);

        }

        private void ProcessLog(string msg)
        {
            if (!this.traceTextBox.IsDisposed)
            {
                this.traceTextBox.AppendText(msg);
            }
        }

        private void setTraceLevelButton_Click(object sender, EventArgs e)
        {
            Globals.TraceSwitch.Level = this.Gui2TraceLevelId();
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
            if (this.ModuleName.Length != 0)
            {
                if (document["TradeMagic"][this.ModuleName] == null)
                {
                    document["TradeMagic"].AppendChild(document.CreateElement(this.ModuleName));
                }
                if (document["TradeMagic"][this.ModuleName]["TraceLevel"] == null)
                {
                    document["TradeMagic"][this.ModuleName].AppendChild(document.CreateElement("TraceLevel"));
                }
                document["TradeMagic"][this.ModuleName]["TraceLevel"].InnerText = ((int) this.Gui2TraceLevelId()).ToString();
                if (document["TradeMagic"][this.ModuleName]["Trace2FileOnly"] == null)
                {
                    document["TradeMagic"][this.ModuleName].AppendChild(document.CreateElement("Trace2FileOnly"));
                }
                document["TradeMagic"][this.ModuleName]["Trace2FileOnly"].InnerText = this.ToFileOnly.ToString();
            }
            else
            {
                if (document["TradeMagic"]["TraceLevel"] == null)
                {
                    document["TradeMagic"].AppendChild(document.CreateElement("TraceLevel"));
                }
                document["TradeMagic"]["TraceLevel"].InnerText = ((int) this.Gui2TraceLevelId()).ToString();
                if (document["TradeMagic"]["Trace2FileOnly"] == null)
                {
                    document["TradeMagic"].AppendChild(document.CreateElement("Trace2FileOnly"));
                }
                document["TradeMagic"]["Trace2FileOnly"].InnerText = this.ToFileOnly.ToString();
            }
            document.Save(installDir + @"\Config.xml");
        }

        internal void Trace(string msg)
        {
            if (this.traceListener == null)
            {
                this.Init();
            }
            this.processMessage.BeginInvoke(this.logHandler, new object[] { msg });
        }

        private void TraceLevelId2Gui(TraceLevelIds traceLevelId)
        {
            this.traceLevelListBox.ClearSelected();
            foreach (iTrading.Core .Kernel.TraceLevel level in iTrading.Core .Kernel .TraceLevel.All.Values)
            {
                if (((level.Id != TraceLevelIds.None) && (level.Id != TraceLevelIds.None)) && ((traceLevelId & level.Id) == level.Id))
                {
                    for (int i = 0; i < this.traceLevelListBox.Items.Count; i++)
                    {
                        if (((string) this.traceLevelListBox.Items[i]) == level.Name)
                        {
                            this.traceLevelListBox.SetSelected(i, true);
                        }
                    }
                }
            }
        }

        internal void TraceLine(string msg)
        {
            if (this.traceListener == null)
            {
                this.Init();
            }
            this.processMessage.BeginInvoke(this.logHandler, new object[] { msg + "\r\n" });
        }

        private void TraceLog_Load(object sender, EventArgs e)
        {
            if (this.traceListener == null)
            {
                this.Init();
            }
        }

        /// <summary>
        /// Name of the "Config.xml" section , where the "TraceLevel" key is stored.
        /// When this name is "", "TraceLevel" is located in the "TradeMagic" top-level section of "Config.xml".
        /// This name also is used to create the log file in the "TradeMagic-InstallDir"\Log subdirectory. 
        /// When "" the filename will be "TradeMagic.txt".
        /// </summary>
        public string ModuleName
        {
            get
            {
                return this.moduleName;
            }
            set
            {
                this.moduleName = value;
                this.Init();
            }
        }

        /// <summary>
        /// </summary>
        public bool ToFileOnly
        {
            get
            {
                return this.toFileOnlyCheckBox.Checked;
            }
            set
            {
                this.toFileOnlyCheckBox.Checked = value;
            }
        }

        private delegate void LogHandler(string message);
    }
}

