namespace Steema.TeeChart.Editors.Series
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class ImageBarSeries : Form
    {
        private PictureBox Bevel1;
        private Button button1;
        private CheckBox checkBox1;
        private Container components = null;
        private GroupBox groupBox1;

        public ImageBarSeries()
        {
            this.InitializeComponent();
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
            this.groupBox1 = new GroupBox();
            this.Bevel1 = new PictureBox();
            this.checkBox1 = new CheckBox();
            this.button1 = new Button();
            this.groupBox1.SuspendLayout();
            base.SuspendLayout();
            this.groupBox1.Controls.Add(this.Bevel1);
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Location = new Point(7, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x120, 180);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Image:";
            this.Bevel1.BorderStyle = BorderStyle.FixedSingle;
            this.Bevel1.Location = new Point(160, 0x10);
            this.Bevel1.Name = "Bevel1";
            this.Bevel1.Size = new Size(0x70, 0x98);
            this.Bevel1.TabIndex = 2;
            this.Bevel1.TabStop = false;
            this.checkBox1.FlatStyle = FlatStyle.Flat;
            this.checkBox1.Location = new Point(0x18, 0x48);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new Size(0x68, 0x10);
            this.checkBox1.TabIndex = 1;
            this.checkBox1.Text = "&Tiled";
            this.button1.FlatStyle = FlatStyle.Flat;
            this.button1.Location = new Point(0x18, 0x20);
            this.button1.Name = "button1";
            this.button1.Size = new Size(80, 0x17);
            this.button1.TabIndex = 0;
            this.button1.Text = "&Browse...";
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x12b, 0xb8);
            base.Controls.Add(this.groupBox1);
            base.Name = "ImageBarSeries";
            this.groupBox1.ResumeLayout(false);
            base.ResumeLayout(false);
        }
    }
}

