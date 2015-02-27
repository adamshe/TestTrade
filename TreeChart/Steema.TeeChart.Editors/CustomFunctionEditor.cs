namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart.Functions;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class CustomFunctionEditor : Form
    {
        private Container components;
        internal Control controlToEnable;
        private Custom function;
        private Label lbNumPoints;
        private Label lbStartX;
        private Label lbStep;
        private NumericUpDown numericUpDown1;
        private TextBox tbStartX;
        private TextBox tbStep;

        public CustomFunctionEditor()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public CustomFunctionEditor(Function f) : this()
        {
            this.function = (Custom) f;
            this.tbStartX.Text = this.function.StartX.ToString();
            this.tbStep.Text = this.function.Period.ToString();
            this.numericUpDown1.Value = this.function.NumPoints;
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
            this.lbStartX = new Label();
            this.lbStep = new Label();
            this.lbNumPoints = new Label();
            this.tbStartX = new TextBox();
            this.tbStep = new TextBox();
            this.numericUpDown1 = new NumericUpDown();
            this.numericUpDown1.BeginInit();
            base.SuspendLayout();
            this.lbStartX.AutoSize = true;
            this.lbStartX.Location = new Point(0x38, 0x10);
            this.lbStartX.Name = "lbStartX";
            this.lbStartX.Size = new Size(0x2a, 0x10);
            this.lbStartX.TabIndex = 0;
            this.lbStartX.Text = "&Start X:";
            this.lbStartX.TextAlign = ContentAlignment.TopRight;
            this.lbStep.AutoSize = true;
            this.lbStep.Location = new Point(0x43, 0x2b);
            this.lbStep.Name = "lbStep";
            this.lbStep.Size = new Size(0x1f, 0x10);
            this.lbStep.TabIndex = 2;
            this.lbStep.Text = "S&tep:";
            this.lbStep.TextAlign = ContentAlignment.TopRight;
            this.lbNumPoints.AutoSize = true;
            this.lbNumPoints.Location = new Point(0x21, 0x48);
            this.lbNumPoints.Name = "lbNumPoints";
            this.lbNumPoints.Size = new Size(0x41, 0x10);
            this.lbNumPoints.TabIndex = 4;
            this.lbNumPoints.Text = "&Num points:";
            this.lbNumPoints.TextAlign = ContentAlignment.TopRight;
            this.tbStartX.BorderStyle = BorderStyle.FixedSingle;
            this.tbStartX.Location = new Point(0x68, 14);
            this.tbStartX.Name = "tbStartX";
            this.tbStartX.Size = new Size(120, 20);
            this.tbStartX.TabIndex = 1;
            this.tbStartX.Text = "0";
            this.tbStartX.TextAlign = HorizontalAlignment.Right;
            this.tbStartX.TextChanged += new EventHandler(this.tbStartX_TextChanged);
            this.tbStep.BorderStyle = BorderStyle.FixedSingle;
            this.tbStep.Location = new Point(0x68, 0x2a);
            this.tbStep.Name = "tbStep";
            this.tbStep.Size = new Size(120, 20);
            this.tbStep.TabIndex = 3;
            this.tbStep.Text = "1";
            this.tbStep.TextAlign = HorizontalAlignment.Right;
            this.tbStep.TextChanged += new EventHandler(this.tbStep_TextChanged);
            this.numericUpDown1.BorderStyle = BorderStyle.FixedSingle;
            this.numericUpDown1.Location = new Point(0x68, 70);
            int[] bits = new int[4];
            bits[0] = 0x989680;
            this.numericUpDown1.Maximum = new decimal(bits);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new Size(0x48, 20);
            this.numericUpDown1.TabIndex = 5;
            this.numericUpDown1.TextAlign = HorizontalAlignment.Right;
            this.numericUpDown1.TextChanged += new EventHandler(this.numericUpDown1_ValueChanged);
            this.numericUpDown1.ValueChanged += new EventHandler(this.numericUpDown1_ValueChanged);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(240, 0x6d);
            base.Controls.Add(this.numericUpDown1);
            base.Controls.Add(this.tbStep);
            base.Controls.Add(this.tbStartX);
            base.Controls.Add(this.lbNumPoints);
            base.Controls.Add(this.lbStep);
            base.Controls.Add(this.lbStartX);
            base.Name = "CustomFunctionEditor";
            this.Text = "CustomFunctionEditor";
            this.numericUpDown1.EndInit();
            base.ResumeLayout(false);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            this.function.NumPoints = (int) this.numericUpDown1.Value;
            this.Changed();
        }

        private void tbStartX_TextChanged(object sender, EventArgs e)
        {
            this.function.StartX = Convert.ToDouble(this.tbStartX.Text);
            this.Changed();
        }

        private void tbStep_TextChanged(object sender, EventArgs e)
        {
            this.function.Period = Convert.ToDouble(this.tbStep.Text);
            this.Changed();
        }
    }
}

