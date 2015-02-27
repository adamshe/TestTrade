namespace iTrading.Core.Kernel
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Resources;
    using System.Windows.Forms;

    /// <summary>
    /// Zusammenfassung für RegisterForm.
    /// </summary>
    public class RegisterForm : Form
    {
        private Container components = null;
        private Label label1;
        private Label label10;
        private Label label11;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private LinkLabel linkLabel1;
        private PictureBox pictureBox1;
        private Button registerLater;
        private Button registerNow;
        private TextBox userId;

        /// <summary>
        /// </summary>
        public RegisterForm()
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

        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.userId = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.registerNow = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.registerLater = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(88, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(176, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Registration requires these steps:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(32, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "Register at";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // linkLabel1
            // 
            this.linkLabel1.Location = new System.Drawing.Point(88, 88);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(96, 23);
            this.linkLabel1.TabIndex = 2;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "TradeMagic forum";
            this.linkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // userId
            // 
            this.userId.Location = new System.Drawing.Point(32, 168);
            this.userId.Name = "userId";
            this.userId.Size = new System.Drawing.Size(288, 20);
            this.userId.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(32, 200);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(192, 23);
            this.label4.TabIndex = 5;
            this.label4.Text = "Press the \"Register now\" button";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(16, 120);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(16, 23);
            this.label5.TabIndex = 6;
            this.label5.Text = "2)";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(16, 88);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(16, 23);
            this.label6.TabIndex = 7;
            this.label6.Text = "1)";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // registerNow
            // 
            this.registerNow.Location = new System.Drawing.Point(56, 240);
            this.registerNow.Name = "registerNow";
            this.registerNow.Size = new System.Drawing.Size(96, 23);
            this.registerNow.TabIndex = 8;
            this.registerNow.Text = "&Register now";
            this.registerNow.Click += new System.EventHandler(this.registerNow_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(304, 8);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(40, 32);
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(16, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(88, 23);
            this.label7.TabIndex = 11;
            this.label7.Text = "TradeMagic 3 is";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.ForeColor = System.Drawing.Color.Red;
            this.label8.Location = new System.Drawing.Point(96, 16);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(40, 23);
            this.label8.TabIndex = 12;
            this.label8.Text = "FREE";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(128, 16);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(176, 23);
            this.label9.TabIndex = 13;
            this.label9.Text = "for personal, non-commercial use";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // registerLater
            // 
            this.registerLater.Location = new System.Drawing.Point(193, 240);
            this.registerLater.Name = "registerLater";
            this.registerLater.Size = new System.Drawing.Size(96, 23);
            this.registerLater.TabIndex = 14;
            this.registerLater.Text = "Register &later";
            this.registerLater.Click += new System.EventHandler(this.registerLater_Click);
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(32, 120);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(296, 23);
            this.label10.TabIndex = 15;
            this.label10.Text = "Go to \'profile\' section in TradeMagic forum and copy your";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(32, 144);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(112, 23);
            this.label11.TabIndex = 16;
            this.label11.Text = "TradeMagic ID here:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(16, 200);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(16, 23);
            this.label3.TabIndex = 17;
            this.label3.Text = "3)";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // RegisterForm
            // 
            this.AcceptButton = this.registerNow;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(346, 279);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.registerLater);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.registerNow);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.userId);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RegisterForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "TradeMagic registration";
            this.Load += new System.EventHandler(this.RegisterForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("IExplore.exe", "www.trademagic.net/phpbb2/index.php");
        }

        private void RegisterForm_Load(object sender, EventArgs e)
        {
            string userId = Globals.UserId;
            this.userId.Text = (userId.Length == 0) ? "enter-your-TradeMagic-ID-here" : userId;
        }

        private void registerLater_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("TradeMagic will not start without prior registration. Do you really want to register later ?", "TradeMagic", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                base.DialogResult = DialogResult.Cancel;
                base.Close();
            }
        }

        private void registerNow_Click(object sender, EventArgs e)
        {
            string text = this.userId.Text;
            if (text.Length == 0)
            {
                MessageBox.Show("The forum user name mustn't be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                this.userId.Focus();
                base.DialogResult = DialogResult.Retry;
            }
            else
            {
                try
                {
                    TMInstaller.Register(text, LicenseTypeId.Professional, "");
                }
                catch (TMException exception)
                {
                    MessageBox.Show(exception.Message, "Registration error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    base.DialogResult = DialogResult.Retry;
                    return;
                }
                MessageBox.Show("Registration successfully completed", "TradeMagic", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                base.DialogResult = DialogResult.OK;
                base.Close();
            }
        }
    }
}

