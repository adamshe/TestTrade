namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart.Functions;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;

    public class CCIFunctionEditor : Form
    {
        private Container components;
        internal Control controlToEnable;
        private TextBox EConst;
        private NumericUpDown EPeriod;
        private CCIFunction function;
        private Label label1;
        private Label label2;

        public CCIFunctionEditor()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public CCIFunctionEditor(Function f) : this()
        {
            this.function = (CCIFunction) f;
            this.EConst.Text = this.function.Constant.ToString();
            this.EPeriod.Value = (decimal) this.function.Period;
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

        private void EConst_TextChanged(object sender, EventArgs e)
        {
            if (!Regex.IsMatch(this.EConst.Text, "[^0-9.,]") && (this.EConst.Text != ""))
            {
                try
                {
                    this.function.Constant = Convert.ToDouble(this.EConst.Text);
                    this.Changed();
                }
                catch
                {
                    throw;
                }
            }
        }

        private void EPeriod_ValueChanged(object sender, EventArgs e)
        {
            this.function.Period = (double) this.EPeriod.Value;
            this.Changed();
        }

        private void InitializeComponent()
        {
            this.label1 = new Label();
            this.EConst = new TextBox();
            this.label2 = new Label();
            this.EPeriod = new NumericUpDown();
            this.EPeriod.BeginInit();
            base.SuspendLayout();
            this.label1.Location = new Point(0x10, 0x18);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x38, 0x17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Constant:";
            this.EConst.Location = new Point(0x48, 0x16);
            this.EConst.Name = "EConst";
            this.EConst.TabIndex = 1;
            this.EConst.Text = "";
            this.EConst.TextChanged += new EventHandler(this.EConst_TextChanged);
            this.label2.Location = new Point(0x10, 0x40);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x30, 0x17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Period:";
            this.EPeriod.Location = new Point(0x47, 0x3e);
            this.EPeriod.Name = "EPeriod";
            this.EPeriod.Size = new Size(0x68, 20);
            this.EPeriod.TabIndex = 3;
            this.EPeriod.ValueChanged += new EventHandler(this.EPeriod_ValueChanged);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(240, 0x6d);
            base.Controls.Add(this.EPeriod);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.EConst);
            base.Controls.Add(this.label1);
            base.Name = "CCIFunctionEditor";
            this.Text = "CCIFunctionEditor";
            this.EPeriod.EndInit();
            base.ResumeLayout(false);
        }
    }
}

