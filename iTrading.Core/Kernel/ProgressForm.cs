namespace iTrading.Core.Kernel
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Resources;
    using System.Threading;
    using System.Windows.Forms;

    internal class ProgressForm : Form, IProgress
    {
        private bool abortable = true;
        private Button abortButton;
        private Container components = null;
        private bool isAborted = false;
        private Label label;
        private int maxSteps = 0;
        private string message = "";
        private ProgressBar progressBar;

        public event AbortEventHandler Aborted;

        internal ProgressForm()
        {
            this.InitializeComponent();
            base.CreateControl();
        }

        private void abortButton_Click(object sender, EventArgs e)
        {
            this.isAborted = true;
            if (this.Aborted != null)
            {
                this.Aborted(this, new EventArgs());
            }
            this.Terminate();
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

        public void Initialise(int maxSteps, bool abortable)
        {
            this.abortable = abortable;
            this.isAborted = false;
            this.maxSteps = maxSteps;
            this.message = "";
            base.Show();
            base.Invoke(new MethodInvoker(this.InitialiseNow), null);
        }

        private void InitialiseNow()
        {
            this.abortButton.Enabled = this.abortable;
            if (this.maxSteps == 0)
            {
                this.progressBar.Hide();
            }
            else
            {
                this.progressBar.Minimum = 0;
                this.progressBar.Maximum = this.maxSteps;
                this.progressBar.Value = 0;
                this.progressBar.Show();
            }
            Monitor.Enter(typeof(ProgressForm));
            base.Show();
            Application.DoEvents();
        }

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.label = new System.Windows.Forms.Label();
            this.abortButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(0, 72);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(288, 8);
            this.progressBar.TabIndex = 0;
            // 
            // label
            // 
            this.label.Location = new System.Drawing.Point(8, 24);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(280, 40);
            this.label.TabIndex = 1;
            this.label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // abortButton
            // 
            this.abortButton.Location = new System.Drawing.Point(103, 100);
            this.abortButton.Name = "abortButton";
            this.abortButton.Size = new System.Drawing.Size(88, 23);
            this.abortButton.TabIndex = 2;
            this.abortButton.Text = "&Abort";
            this.abortButton.Click += new System.EventHandler(this.abortButton_Click);
            // 
            // ProgressForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(290, 135);
            this.ControlBox = false;
            this.Controls.Add(this.abortButton);
            this.Controls.Add(this.label);
            this.Controls.Add(this.progressBar);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProgressForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Please wait. Processing ...";
            this.ResumeLayout(false);

        }

        public void PerformStep()
        {
            base.Invoke(new MethodInvoker(this.PerformStepNow), null);
        }

        private void PerformStepNow()
        {
            if ((this.progressBar.Value + 1) < this.progressBar.Maximum)
            {
                this.progressBar.Value++;
            }
            Application.DoEvents();
        }

        private void SetMessageNow()
        {
            this.label.Text = this.message;
            Application.DoEvents();
        }

        public void Terminate()
        {
            Monitor.Exit(typeof(ProgressForm));
            this.Aborted  = null;
            base.Hide();
        }

        public bool IsAborted
        {
            get
            {
                Application.DoEvents();
                return this.isAborted;
            }
        }

        public string Message
        {
            get
            {
                return this.message;
            }
            set
            {
                this.message = value;
                base.Invoke(new MethodInvoker(this.SetMessageNow), null);
            }
        }
    }
}

