namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart.Functions;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class PVOFunctionEditor : Form
    {
        private CheckBox CBPercent;
        private Container components;
        internal Control controlToEnable;
        private PVOFunction function;
        private Label label1;
        private Label label2;
        private NumericUpDown UDPeriod1;
        private NumericUpDown UDPeriod2;

        public PVOFunctionEditor()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public PVOFunctionEditor(Function f) : this()
        {
            this.function = (PVOFunction) f;
            this.UDPeriod2.Value = this.function.Period2;
            this.UDPeriod1.Value = (decimal) this.function.Period;
            this.CBPercent.Checked = this.function.Percentage;
        }

        private void CBPercent_Click(object sender, EventArgs e)
        {
            this.function.Percentage = this.CBPercent.Checked;
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
            this.UDPeriod1 = new NumericUpDown();
            this.label1 = new Label();
            this.label2 = new Label();
            this.UDPeriod2 = new NumericUpDown();
            this.CBPercent = new CheckBox();
            this.UDPeriod1.BeginInit();
            this.UDPeriod2.BeginInit();
            base.SuspendLayout();
            this.UDPeriod1.Location = new Point(0x58, 0x10);
            this.UDPeriod1.Name = "UDPeriod1";
            this.UDPeriod1.Size = new Size(80, 20);
            this.UDPeriod1.TabIndex = 0;
            this.UDPeriod1.ValueChanged += new EventHandler(this.UDPeriod1_ValueChanged);
            this.label1.Location = new Point(0x20, 20);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x38, 0x12);
            this.label1.TabIndex = 1;
            this.label1.Text = "Period1:";
            this.label2.Location = new Point(0x20, 50);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x38, 0x17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Period2:";
            this.UDPeriod2.Location = new Point(0x58, 0x30);
            this.UDPeriod2.Name = "UDPeriod2";
            this.UDPeriod2.Size = new Size(80, 20);
            this.UDPeriod2.TabIndex = 2;
            this.UDPeriod2.ValueChanged += new EventHandler(this.UDPeriod2_ValueChanged);
            this.CBPercent.Location = new Point(0x20, 0x52);
            this.CBPercent.Name = "CBPercent";
            this.CBPercent.Size = new Size(0x80, 0x10);
            this.CBPercent.TabIndex = 4;
            this.CBPercent.Text = "Percentage";
            this.CBPercent.Click += new EventHandler(this.CBPercent_Click);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(240, 0x6d);
            base.Controls.Add(this.CBPercent);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.UDPeriod2);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.UDPeriod1);
            base.Name = "PVOFunctionEditor";
            this.Text = "PVOFunctionEditor";
            this.UDPeriod1.EndInit();
            this.UDPeriod2.EndInit();
            base.ResumeLayout(false);
        }

        private void UDPeriod1_ValueChanged(object sender, EventArgs e)
        {
            this.function.Period = (double) this.UDPeriod1.Value;
            this.Changed();
        }

        private void UDPeriod2_ValueChanged(object sender, EventArgs e)
        {
            this.function.Period2 = (int) this.UDPeriod2.Value;
            this.Changed();
        }
    }
}

