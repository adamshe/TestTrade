namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart;
    using Steema.TeeChart.Functions;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class PeriodEditor : Form
    {
        private Button BChange;
        private CheckBox CBAll;
        private Container components;
        internal Control controlToEnable;
        private NumericUpDown ENum;
        private Function function;
        private GroupBox groupBox1;
        private Label label1;
        private Label label2;
        private RadioButton radioButton3;
        private RadioButton radioButton4;
        private RadioButton radioButton5;
        private RadioButton RBNum;
        private RadioButton RBRange;

        public PeriodEditor()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public PeriodEditor(Function f) : this()
        {
            this.function = f;
            if (this.function.PeriodStyle == PeriodStyles.NumPoints)
            {
                this.RBNum.Checked = true;
                this.CBAll.Checked = f.Period == 0.0;
            }
            else
            {
                this.RBRange.Checked = true;
            }
            if (this.function.PeriodAlign == PeriodAligns.First)
            {
                this.radioButton3.Checked = true;
            }
            else if (this.function.PeriodAlign == PeriodAligns.Center)
            {
                this.radioButton4.Checked = true;
            }
            else
            {
                this.radioButton5.Checked = true;
            }
        }

        public void Apply()
        {
        }

        private void BChange_Click(object sender, EventArgs e)
        {
            this.RBRange.Checked = true;
            AxisIncrement increment = new AxisIncrement();
            increment.isExact = true;
            increment.dIncrement = this.function.Period;
            increment.iStep = Axis.FindDateTimeStep(this.function.Period);
            increment.Text = Texts.PeriodRange;
            if (increment.ShowDialog(this) == DialogResult.OK)
            {
                this.ENum.Text = increment.Increment.ToString();
                this.label2.Text = this.ENum.Text;
            }
            increment.Dispose();
        }

        private void CBAll_CheckedChanged(object sender, EventArgs e)
        {
            if (this.CBAll.Checked)
            {
                this.function.Period = 0.0;
                this.ENum.Value = 0M;
            }
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
            this.function.Period = (int) this.ENum.Value;
            this.CBAll.Checked = this.function.Period == 0.0;
            this.label2.Text = this.ENum.Text;
        }

        private void InitializeComponent()
        {
            this.label1 = new Label();
            this.RBNum = new RadioButton();
            this.RBRange = new RadioButton();
            this.groupBox1 = new GroupBox();
            this.radioButton5 = new RadioButton();
            this.radioButton4 = new RadioButton();
            this.radioButton3 = new RadioButton();
            this.label2 = new Label();
            this.CBAll = new CheckBox();
            this.BChange = new Button();
            this.ENum = new NumericUpDown();
            this.groupBox1.SuspendLayout();
            this.ENum.BeginInit();
            base.SuspendLayout();
            this.label1.AutoSize = true;
            this.label1.Location = new Point(8, 3);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x55, 0x10);
            this.label1.TabIndex = 0;
            this.label1.Text = "Calculate every:";
            this.RBNum.FlatStyle = FlatStyle.Flat;
            this.RBNum.Location = new Point(8, 0x15);
            this.RBNum.Name = "RBNum";
            this.RBNum.Size = new Size(0x70, 0x10);
            this.RBNum.TabIndex = 1;
            this.RBNum.Text = "&Number of points:";
            this.RBNum.CheckedChanged += new EventHandler(this.RBNum_CheckedChanged);
            this.RBRange.FlatStyle = FlatStyle.Flat;
            this.RBRange.Location = new Point(8, 0x2e);
            this.RBRange.Name = "RBRange";
            this.RBRange.Size = new Size(0x70, 0x10);
            this.RBRange.TabIndex = 4;
            this.RBRange.Text = "&Range of values:";
            this.RBRange.CheckedChanged += new EventHandler(this.RBRange_CheckedChanged);
            this.groupBox1.Controls.Add(this.radioButton5);
            this.groupBox1.Controls.Add(this.radioButton4);
            this.groupBox1.Controls.Add(this.radioButton3);
            this.groupBox1.Location = new Point(8, 70);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x120, 0x22);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Alignment:";
            this.radioButton5.FlatStyle = FlatStyle.Flat;
            this.radioButton5.Location = new Point(0xd0, 14);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new Size(0x40, 0x10);
            this.radioButton5.TabIndex = 2;
            this.radioButton5.Text = "&Last";
            this.radioButton5.CheckedChanged += new EventHandler(this.radioButton5_CheckedChanged);
            this.radioButton4.FlatStyle = FlatStyle.Flat;
            this.radioButton4.Location = new Point(0x70, 14);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new Size(0x40, 0x10);
            this.radioButton4.TabIndex = 1;
            this.radioButton4.Text = "C&enter";
            this.radioButton4.CheckedChanged += new EventHandler(this.radioButton4_CheckedChanged);
            this.radioButton3.FlatStyle = FlatStyle.Flat;
            this.radioButton3.Location = new Point(0x10, 14);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new Size(0x40, 0x10);
            this.radioButton3.TabIndex = 0;
            this.radioButton3.Text = "&First";
            this.radioButton3.CheckedChanged += new EventHandler(this.radioButton3_CheckedChanged);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(120, 0x2f);
            this.label2.Name = "label2";
            this.label2.Size = new Size(10, 0x10);
            this.label2.TabIndex = 5;
            this.label2.Text = "0";
            this.label2.UseMnemonic = false;
            this.CBAll.Checked = true;
            this.CBAll.CheckState = CheckState.Checked;
            this.CBAll.FlatStyle = FlatStyle.Flat;
            this.CBAll.Location = new Point(0xd0, 0x15);
            this.CBAll.Name = "CBAll";
            this.CBAll.Size = new Size(0x48, 0x10);
            this.CBAll.TabIndex = 3;
            this.CBAll.Text = "All &points";
            this.CBAll.CheckedChanged += new EventHandler(this.CBAll_CheckedChanged);
            this.BChange.FlatStyle = FlatStyle.Flat;
            this.BChange.Location = new Point(0xd1, 0x29);
            this.BChange.Name = "BChange";
            this.BChange.TabIndex = 6;
            this.BChange.Text = "&Change...";
            this.BChange.Click += new EventHandler(this.BChange_Click);
            this.ENum.BorderStyle = BorderStyle.FixedSingle;
            this.ENum.Location = new Point(120, 0x13);
            this.ENum.Name = "ENum";
            this.ENum.Size = new Size(0x48, 20);
            this.ENum.TabIndex = 2;
            this.ENum.TextAlign = HorizontalAlignment.Right;
            this.ENum.TextChanged += new EventHandler(this.ENum_ValueChanged);
            this.ENum.ValueChanged += new EventHandler(this.ENum_ValueChanged);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x131, 0x74);
            base.Controls.Add(this.ENum);
            base.Controls.Add(this.BChange);
            base.Controls.Add(this.CBAll);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.RBRange);
            base.Controls.Add(this.RBNum);
            base.Name = "PeriodEditor";
            base.Load += new EventHandler(this.PeriodEditor_Load);
            this.groupBox1.ResumeLayout(false);
            this.ENum.EndInit();
            base.ResumeLayout(false);
        }

        private void PeriodEditor_Load(object sender, EventArgs e)
        {
            if (this.function != null)
            {
                this.ENum.Value = (int) this.function.Period;
                this.CBAll.Checked = this.function.Period == 0.0;
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            this.function.PeriodAlign = PeriodAligns.First;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            this.function.PeriodAlign = PeriodAligns.Center;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            this.function.PeriodAlign = PeriodAligns.Last;
        }

        private void RBNum_CheckedChanged(object sender, EventArgs e)
        {
            this.ENum.Focus();
        }

        private void RBRange_CheckedChanged(object sender, EventArgs e)
        {
            this.BChange.Focus();
        }
    }
}

