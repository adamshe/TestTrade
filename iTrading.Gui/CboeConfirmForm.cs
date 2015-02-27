namespace iTrading.Gui
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Resources;
    using System.Windows.Forms;

    /// <summary>
    /// </summary>
    public class CboeConfirmForm : Form
    {
        /// <summary>
        /// </summary>
        private Container components = null;
        private Label label1;
        private Label label2;
        private LinkLabel linkLabel1;
        private Button noButton;
        private Button yesButton;

        /// <summary>
        /// </summary>
        public CboeConfirmForm()
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
            ResourceManager manager = new ResourceManager(typeof(CboeConfirmForm));
            this.yesButton = new Button();
            this.noButton = new Button();
            this.label1 = new Label();
            this.linkLabel1 = new LinkLabel();
            this.label2 = new Label();
            base.SuspendLayout();
            this.yesButton.Location = new Point(0x38, 0x60);
            this.yesButton.Name = "yesButton";
            this.yesButton.TabIndex = 0;
            this.yesButton.Text = "&Yes";
            this.yesButton.Click += new EventHandler(this.yesButton_Click);
            this.noButton.DialogResult = DialogResult.Cancel;
            this.noButton.Location = new Point(160, 0x60);
            this.noButton.Name = "noButton";
            this.noButton.TabIndex = 1;
            this.noButton.Text = "&No";
            this.label1.Location = new Point(0x10, 0x10);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x100, 0x20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Please confirm to submit an CBOE option order";
            this.label1.TextAlign = ContentAlignment.MiddleCenter;
            this.linkLabel1.Location = new Point(0x90, 0x38);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new Size(0x60, 0x17);
            this.linkLabel1.TabIndex = 4;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "CBOE Rule 6.8A";
            this.linkLabel1.TextAlign = ContentAlignment.MiddleLeft;
            this.linkLabel1.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            this.label2.Location = new Point(40, 0x38);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x60, 0x17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Read more here:";
            this.label2.TextAlign = ContentAlignment.MiddleRight;
            base.AcceptButton = this.yesButton;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this.noButton;
            base.ClientSize = new Size(0x124, 0x85);
            base.Controls.Add(this.linkLabel1);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.noButton);
            base.Controls.Add(this.yesButton);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "CboeConfirmForm";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Please confirm";
            base.ResumeLayout(false);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("IExplore.exe", "www.trademagic.net/phpBB2/viewtopic.php?t=110");
        }

        private void yesButton_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Yes;
            base.Close();
        }
    }
}

