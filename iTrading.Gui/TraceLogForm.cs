namespace iTrading.Gui
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Resources;
    using System.Windows.Forms;

    /// <summary>
    /// </summary>
    public class TraceLogForm : Form
    {
        private IContainer components;
        private NotifyIcon notifyIcon;
        private TraceLog traceLog;

        /// <summary>
        /// 
        /// </summary>
        public TraceLogForm()
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
            this.components = new Container();
            ResourceManager manager = new ResourceManager(typeof(TraceLogForm));
            this.notifyIcon = new NotifyIcon(this.components);
            this.traceLog = new TraceLog();
            base.SuspendLayout();
            this.notifyIcon.Icon = (Icon) manager.GetObject("notifyIcon.Icon");
            this.notifyIcon.Text = "TradeMagic trace log";
            this.notifyIcon.DoubleClick += new EventHandler(this.NotifyIcon_Click);
            this.traceLog.Dock = DockStyle.Fill;
            this.traceLog.Location = new Point(0, 0);
            this.traceLog.ModuleName = "";
            this.traceLog.Name = "traceLog";
            this.traceLog.Size = new Size(0x278, 0x16d);
            this.traceLog.TabIndex = 0;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x278, 0x16d);
            base.Controls.Add(this.traceLog);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "TraceLogForm";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Trace log";
            base.Closing += new CancelEventHandler(this.TraceLogForm_Closing);
            base.ResumeLayout(false);
        }

        private void NotifyIcon_Click(object sender, EventArgs e)
        {
            if (base.WindowState == FormWindowState.Minimized)
            {
                base.WindowState = FormWindowState.Normal;
            }
            base.Show();
        }

        private void TraceLogForm_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            base.Hide();
        }

        /// <summary>
        /// Shows a icon in the system tray.
        /// </summary>
        public bool AddToTray
        {
            get
            {
                return base.ShowInTaskbar;
            }
            set
            {
                base.ShowInTaskbar = value;
                this.notifyIcon.Visible = value;
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
                return this.traceLog.ModuleName;
            }
            set
            {
                this.traceLog.ModuleName = value;
            }
        }

        /// <summary>
        /// </summary>
        public bool ToFileOnly
        {
            get
            {
                return this.traceLog.ToFileOnly;
            }
            set
            {
                this.traceLog.ToFileOnly = value;
            }
        }
    }
}

