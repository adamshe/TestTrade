namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart.Functions;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class MovingAverageFunctionEditor : Form
    {
        private ComboBox CBStyle;
        private Container components;
        internal Control controlToEnable;
        private MovingAverage function;
        private Label label1;
        private Label label2;
        private NumericUpDown numericUpDown1;

        public MovingAverageFunctionEditor()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public MovingAverageFunctionEditor(Function f) : this()
        {
            this.function = (MovingAverage) f;
            this.numericUpDown1.Value = (decimal) this.function.Period;
            if (this.function.Weighted)
            {
                this.CBStyle.SelectedIndex = 1;
            }
            else if (this.function.WeightedIndex)
            {
                this.CBStyle.SelectedIndex = 2;
            }
            else
            {
                this.CBStyle.SelectedIndex = 0;
            }
        }

        private void CBStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.function.Weighted = this.CBStyle.SelectedIndex == 1;
            this.function.WeightedIndex = this.CBStyle.SelectedIndex == 2;
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

        private void InitializeComponent()
        {
            this.label1 = new Label();
            this.numericUpDown1 = new NumericUpDown();
            this.label2 = new Label();
            this.CBStyle = new ComboBox();
            this.numericUpDown1.BeginInit();
            base.SuspendLayout();
            this.label1.Location = new Point(0x40, 0x18);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x48, 0x17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Period:";
            this.numericUpDown1.Location = new Point(0x70, 0x16);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new Size(0x58, 20);
            this.numericUpDown1.TabIndex = 1;
            this.numericUpDown1.ValueChanged += new EventHandler(this.numericUpDown1_ValueChanged);
            this.label2.Location = new Point(0x36, 0x38);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x38, 0x17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Average:";
            this.CBStyle.Items.AddRange(new object[] { "Normal", "Weighted", "Weighted by Index" });
            this.CBStyle.Location = new Point(0x70, 0x35);
            this.CBStyle.Name = "CBStyle";
            this.CBStyle.Size = new Size(0x58, 0x15);
            this.CBStyle.TabIndex = 3;
            this.CBStyle.SelectedIndexChanged += new EventHandler(this.CBStyle_SelectedIndexChanged);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(240, 0x6d);
            base.Controls.Add(this.CBStyle);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.numericUpDown1);
            base.Controls.Add(this.label1);
            base.Name = "MovingAverageFunctionEditor";
            this.Text = "MovingAverageFunctionEditor";
            this.numericUpDown1.EndInit();
            base.ResumeLayout(false);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            this.function.Period = (double) this.numericUpDown1.Value;
            this.Changed();
        }
    }
}

