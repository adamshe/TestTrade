namespace Steema.TeeChart.Editors.Export
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class TIFFOptions : Form
    {
        private Container components = null;
        private RadioButton radioButton1;
        private RadioButton radioButton2;
        private GroupBox RGCompression;

        public TIFFOptions()
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
            this.RGCompression = new GroupBox();
            this.radioButton2 = new RadioButton();
            this.radioButton1 = new RadioButton();
            this.RGCompression.SuspendLayout();
            base.SuspendLayout();
            this.RGCompression.Controls.Add(this.radioButton2);
            this.RGCompression.Controls.Add(this.radioButton1);
            this.RGCompression.Location = new Point(20, 8);
            this.RGCompression.Name = "RGCompression";
            this.RGCompression.Size = new Size(0xa8, 40);
            this.RGCompression.TabIndex = 1;
            this.RGCompression.TabStop = false;
            this.RGCompression.Text = "&Compression:";
            this.radioButton2.FlatStyle = FlatStyle.Flat;
            this.radioButton2.Location = new Point(0x60, 0x10);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new Size(0x40, 0x10);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.Text = "&RLE";
            this.radioButton1.Checked = true;
            this.radioButton1.FlatStyle = FlatStyle.Flat;
            this.radioButton1.Location = new Point(0x10, 0x10);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new Size(0x40, 0x10);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "&LZW";
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0xd0, 0x66);
            base.Controls.Add(this.RGCompression);
            base.Name = "TIFFOptions";
            this.Text = "TIFF";
            this.RGCompression.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        public bool CompressionLZW
        {
            get
            {
                return this.radioButton1.Checked;
            }
        }
    }
}

