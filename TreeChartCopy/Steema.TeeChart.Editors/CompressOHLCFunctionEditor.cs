namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart.Functions;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class CompressOHLCFunctionEditor : Form
    {
        private ComboBox CBPeriod;
        private Container components;
        internal Control controlToEnable;
        private NumericUpDown ENum;
        private CompressOHLC function;
        private RadioButton radioButton1;
        private RadioButton radioButton2;

        public CompressOHLCFunctionEditor()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public CompressOHLCFunctionEditor(Function f) : this()
        {
            this.function = (CompressOHLC) f;
            switch (this.function.Compress)
            {
                case CompressionPeriod.ocDay:
                    this.CBPeriod.SelectedIndex = 0;
                    break;

                case CompressionPeriod.ocWeek:
                    this.CBPeriod.SelectedIndex = 1;
                    break;

                case CompressionPeriod.ocMonth:
                    this.CBPeriod.SelectedIndex = 2;
                    break;

                case CompressionPeriod.ocBiMonth:
                    this.CBPeriod.SelectedIndex = 3;
                    break;

                case CompressionPeriod.ocQuarter:
                    this.CBPeriod.SelectedIndex = 4;
                    break;

                case CompressionPeriod.ocYear:
                    this.CBPeriod.SelectedIndex = 5;
                    break;
            }
            this.ENum.Value = (decimal) this.function.Period;
            this.radioButton1.Checked = this.ENum.Value == 0M;
            this.radioButton2.Checked = !this.radioButton1.Checked;
        }

        private void CBPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.CBPeriod.SelectedIndex)
            {
                case 0:
                    this.function.Compress = CompressionPeriod.ocDay;
                    break;

                case 1:
                    this.function.Compress = CompressionPeriod.ocWeek;
                    break;

                case 2:
                    this.function.Compress = CompressionPeriod.ocMonth;
                    break;

                case 3:
                    this.function.Compress = CompressionPeriod.ocBiMonth;
                    break;

                case 4:
                    this.function.Compress = CompressionPeriod.ocQuarter;
                    break;

                case 5:
                    this.function.Compress = CompressionPeriod.ocYear;
                    break;
            }
            this.Changed();
        }

        private void Changed()
        {
            if (this.controlToEnable != null)
            {
                this.controlToEnable.Enabled = true;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void ENum_ValueChanged(object sender, EventArgs e)
        {
            this.function.Period = (double) this.ENum.Value;
            this.Changed();
        }

        private void InitializeComponent()
        {
            this.radioButton1 = new RadioButton();
            this.radioButton2 = new RadioButton();
            this.CBPeriod = new ComboBox();
            this.ENum = new NumericUpDown();
            this.ENum.BeginInit();
            base.SuspendLayout();
            this.radioButton1.Checked = true;
            this.radioButton1.FlatStyle = FlatStyle.Flat;
            this.radioButton1.Location = new Point(9, 0x18);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new Size(0x75, 0x18);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "&DateTime period:";
            this.radioButton1.Click += new EventHandler(this.radioButton1_Click);
            this.radioButton2.FlatStyle = FlatStyle.Flat;
            this.radioButton2.Location = new Point(9, 0x36);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new Size(120, 0x18);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.Text = "&Number of points:";
            this.radioButton2.Click += new EventHandler(this.radioButton2_Click);
            this.CBPeriod.Items.AddRange(new object[] { "Daily", "Weekly", "Monthly", "Bi-Monthly", "Quarterly", "Yearly" });
            this.CBPeriod.Location = new Point(0x8a, 0x1a);
            this.CBPeriod.Name = "CBPeriod";
            this.CBPeriod.Size = new Size(0x61, 0x15);
            this.CBPeriod.TabIndex = 2;
            this.CBPeriod.SelectedIndexChanged += new EventHandler(this.CBPeriod_SelectedIndexChanged);
            this.ENum.Location = new Point(0x89, 0x3a);
            this.ENum.Name = "ENum";
            this.ENum.Size = new Size(0x60, 20);
            this.ENum.TabIndex = 3;
            int[] bits = new int[4];
            bits[0] = 1;
            this.ENum.Value = new decimal(bits);
            this.ENum.ValueChanged += new EventHandler(this.ENum_ValueChanged);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(240, 0x6d);
            base.Controls.Add(this.ENum);
            base.Controls.Add(this.CBPeriod);
            base.Controls.Add(this.radioButton2);
            base.Controls.Add(this.radioButton1);
            base.Name = "CompressOHLCFunctionEditor";
            this.Text = "CompressOHLCFunctionEditor";
            this.ENum.EndInit();
            base.ResumeLayout(false);
        }

        private void radioButton1_Click(object sender, EventArgs e)
        {
            this.ENum.Value = 0M;
            this.CBPeriod.Focus();
            this.Changed();
        }

        private void radioButton2_Click(object sender, EventArgs e)
        {
            this.ENum.Focus();
            this.Changed();
        }
    }
}

