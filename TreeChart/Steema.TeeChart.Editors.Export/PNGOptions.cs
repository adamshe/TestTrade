namespace Steema.TeeChart.Editors.Export
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class PNGOptions : Form
    {
        private CheckBox cbGrayScale;
        private Container components = null;
        private Label label1;
        private NumericUpDown UpDown1;

        public PNGOptions()
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
            this.label1 = new Label();
            this.UpDown1 = new NumericUpDown();
            this.cbGrayScale = new CheckBox();
            this.UpDown1.BeginInit();
            base.SuspendLayout();
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x30, 0x12);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x65, 0x10);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Compression level:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            this.label1.Visible = false;
            this.UpDown1.BorderStyle = BorderStyle.FixedSingle;
            this.UpDown1.Location = new Point(0x98, 0x10);
            int[] bits = new int[4];
            bits[0] = 9;
            this.UpDown1.Maximum = new decimal(bits);
            this.UpDown1.Name = "UpDown1";
            this.UpDown1.Size = new Size(0x25, 20);
            this.UpDown1.TabIndex = 1;
            this.UpDown1.TextAlign = HorizontalAlignment.Right;
            bits = new int[4];
            bits[0] = 9;
            this.UpDown1.Value = new decimal(bits);
            this.UpDown1.Visible = false;
            this.cbGrayScale.FlatStyle = FlatStyle.Flat;
            this.cbGrayScale.Location = new Point(0x48, 40);
            this.cbGrayScale.Name = "cbGrayScale";
            this.cbGrayScale.Size = new Size(120, 0x18);
            this.cbGrayScale.TabIndex = 2;
            this.cbGrayScale.Text = "&Gray Scale";
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0xd0, 0x4f);
            base.Controls.Add(this.cbGrayScale);
            base.Controls.Add(this.UpDown1);
            base.Controls.Add(this.label1);
            base.Name = "PNGOptions";
            this.UpDown1.EndInit();
            base.ResumeLayout(false);
        }

        public int Compression
        {
            get
            {
                return (int) this.UpDown1.Value;
            }
        }

        public bool GrayScale
        {
            get
            {
                return this.cbGrayScale.Checked;
            }
        }
    }
}

