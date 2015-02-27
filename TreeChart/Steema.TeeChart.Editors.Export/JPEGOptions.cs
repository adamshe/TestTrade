namespace Steema.TeeChart.Editors.Export
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class JPEGOptions : Form
    {
        private CheckBox CBGray;
        private Container components = null;
        private Label label1;
        private NumericUpDown UpDown1;

        public JPEGOptions()
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
            this.CBGray = new CheckBox();
            this.label1 = new Label();
            this.UpDown1 = new NumericUpDown();
            this.UpDown1.BeginInit();
            base.SuspendLayout();
            this.CBGray.FlatStyle = FlatStyle.Flat;
            this.CBGray.Location = new Point(0x24, 0x10);
            this.CBGray.Name = "CBGray";
            this.CBGray.Size = new Size(100, 0x10);
            this.CBGray.TabIndex = 1;
            this.CBGray.Text = "&Gray scale";
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x19, 50);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x38, 0x10);
            this.label1.TabIndex = 2;
            this.label1.Text = "% &Quality:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            this.UpDown1.BorderStyle = BorderStyle.FixedSingle;
            this.UpDown1.Location = new Point(0x51, 0x30);
            int[] bits = new int[4];
            bits[0] = 1;
            this.UpDown1.Minimum = new decimal(bits);
            this.UpDown1.Name = "UpDown1";
            this.UpDown1.Size = new Size(0x38, 20);
            this.UpDown1.TabIndex = 3;
            this.UpDown1.TextAlign = HorizontalAlignment.Right;
            bits = new int[4];
            bits[0] = 0x5f;
            this.UpDown1.Value = new decimal(bits);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0xa1, 0x75);
            base.Controls.Add(this.UpDown1);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.CBGray);
            base.Name = "JPEGOptions";
            this.UpDown1.EndInit();
            base.ResumeLayout(false);
        }

        public bool Grayscale
        {
            get
            {
                return this.CBGray.Checked;
            }
        }

        public int Quality
        {
            get
            {
                return (int) this.UpDown1.Value;
            }
        }
    }
}

