namespace iTrading.Test
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Resources;
    using System.Windows.Forms;

    /// <summary>
    /// </summary>
    public class TerminateForm : Form
    {
        private Container components = null;
        internal bool exitAppOnClick = true;
        private Label label1;
        private Button okButton;

        /// <summary>
        /// </summary>
        public TerminateForm()
        {
            this.InitializeComponent();
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
            ResourceManager manager = new ResourceManager(typeof(TerminateForm));
            this.label1 = new Label();
            this.okButton = new Button();
            base.SuspendLayout();
            this.label1.Location = new Point(8, 0x10);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0xd0, 0x17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Terminate test suite ?";
            this.label1.TextAlign = ContentAlignment.MiddleCenter;
            this.okButton.Location = new Point(0x48, 0x38);
            this.okButton.Name = "okButton";
            this.okButton.TabIndex = 1;
            this.okButton.Text = "&Ok";
            this.okButton.Click += new EventHandler(this.okButton_Click);
            base.AcceptButton = this.okButton;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0xda, 0x5f);
            base.Controls.Add(this.okButton);
            base.Controls.Add(this.label1);
            base.FormBorderStyle = FormBorderStyle.FixedSingle;
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "TerminateForm";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Terminate ?";
            base.ResumeLayout(false);
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (this.exitAppOnClick)
            {
                Globals.TerminateServer();
                Process.GetCurrentProcess().Kill();
            }
            else
            {
                for (int i = 0; i < Globals.processes.Count; i++)
                {
                    try
                    {
                        ((Process) Globals.processes[i]).Kill();
                    }
                    catch (Exception)
                    {
                    }
                }
                Globals.processes.Clear();
                Globals.TerminateServer();
                if (Globals.startProcesses != null)
                {
                    Globals.startProcesses.Abort();
                    Globals.startProcesses = null;
                }
                base.Close();
            }
        }
    }
}

