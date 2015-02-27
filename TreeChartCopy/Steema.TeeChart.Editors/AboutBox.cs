namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Reflection;
    using System.Windows.Forms;

    public class AboutBox : Form
    {
        private Button button1;
        private IContainer components;
        private Label label1;
        private Label label2;
        private Label labelE;
        private LinkLabel linkLabel1;
        private System.Windows.Forms.Panel panel1;
        private Label version;

        public AboutBox()
        {
            this.InitializeComponent();
        }

        private void AboutBox_Load(object sender, EventArgs e)
        {
            this.panel1.BackgroundImage = Utils.GetBitmapResource("Steema.TeeChart.Images.aboutback.bmp");
            this.version.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

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
            this.panel1 = new System.Windows.Forms.Panel();
            this.linkLabel1 = new LinkLabel();
            this.label2 = new Label();
            this.label1 = new Label();
            this.button1 = new Button();
            this.version = new Label();
            this.labelE = new Label();
            this.panel1.SuspendLayout();
            base.SuspendLayout();
            this.panel1.BorderStyle = BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.linkLabel1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new Point(10, 8);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x156, 0x80);
            this.panel1.TabIndex = 0;
            this.panel1.Paint += new PaintEventHandler(this.panel1_Paint);
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.BackColor = Color.Transparent;
            this.linkLabel1.FlatStyle = FlatStyle.Flat;
            this.linkLabel1.Font = new Font("Tahoma", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.linkLabel1.Location = new Point(0xcf, 0x68);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new Size(0x80, 0x13);
            this.linkLabel1.TabIndex = 2;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "www.teechart.net";
            this.linkLabel1.UseMnemonic = false;
            this.linkLabel1.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            this.label2.AutoSize = true;
            this.label2.BackColor = Color.Transparent;
            this.label2.Location = new Point(7, 0x6c);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x6b, 0x10);
            this.label2.TabIndex = 1;
            this.label2.Text = "All Rights Reserved.";
            this.label2.UseMnemonic = false;
            this.label1.AutoSize = true;
            this.label1.BackColor = Color.Transparent;
            this.label1.FlatStyle = FlatStyle.Flat;
            this.label1.Location = new Point(6, 0x5c);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0xb3, 0x10);
            this.label1.TabIndex = 0;
            this.label1.Text = "(c) 2001-2004 by Steema Software";
            this.label1.UseMnemonic = false;
            this.button1.DialogResult = DialogResult.OK;
            this.button1.FlatStyle = FlatStyle.Flat;
            this.button1.Location = new Point(0x90, 0x90);
            this.button1.Name = "button1";
            this.button1.TabIndex = 1;
            this.button1.Text = "Close";
            this.version.AutoSize = true;
            this.version.Location = new Point(13, 0x93);
            this.version.Name = "version";
            this.version.Size = new Size(0x29, 0x10);
            this.version.TabIndex = 2;
            this.version.Text = "version";
            this.labelE.BackColor = Color.Transparent;
            this.labelE.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.labelE.ForeColor = SystemColors.ControlText;
            this.labelE.Location = new Point(0xee, 0x91);
            this.labelE.Name = "labelE";
            this.labelE.Size = new Size(0x70, 0x10);
            this.labelE.TabIndex = 5;
            this.labelE.Text = "Evaluation Version";
            this.labelE.Visible = false;
            base.AcceptButton = this.button1;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x16a, 0xaf);
            base.Controls.Add(this.labelE);
            base.Controls.Add(this.version);
            base.Controls.Add(this.panel1);
            base.Controls.Add(this.button1);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.Name = "AboutBox";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "About TeeChart Pro .Net v1";
            base.Load += new EventHandler(this.AboutBox_Load);
            this.panel1.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.linkLabel1.Links[this.linkLabel1.Links.IndexOf(e.Link)].Visited = true;
            Process.Start(this.linkLabel1.Text);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            base.OnPaint(e);
            Image bitmapResource = Utils.GetBitmapResource("Steema.TeeChart.Images.tchartnetlogo.png");
            e.Graphics.DrawImageUnscaled(bitmapResource, (this.panel1.Width / 2) - (bitmapResource.Width / 2), 2, bitmapResource.Width, bitmapResource.Height);
        }

        public static void ShowModal()
        {
            using (AboutBox box = new AboutBox())
            {
                EditorUtils.Translate(box);
                box.ShowDialog();
            }
        }
    }
}

