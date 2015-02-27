namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class AxisIncrement : Form
    {
        private Button BitBtn1;
        private Button BitBtn2;
        private CheckBox cbExact;
        private ComboBox cbSteps;
        private Container components = null;
        protected internal double dIncrement;
        private TextBox eCustom;
        private GroupBox groupBox1;
        protected internal bool isDateTime;
        protected internal bool isExact;
        protected internal DateTimeSteps iStep;
        private Label label1;
        private RadioButton radioButton1;
        private RadioButton radioButton2;

        public AxisIncrement()
        {
            this.InitializeComponent();
        }

        private void AxisIncrement_Load(object sender, EventArgs e)
        {
            this.cbExact.Visible = this.isDateTime;
            this.cbExact.Enabled = this.radioButton1.Checked;
            this.cbExact.Checked = this.isExact;
            if (this.isDateTime)
            {
                this.SetEditText();
                if (this.isExact && (this.iStep != DateTimeSteps.None))
                {
                    this.radioButton1.Checked = true;
                    this.cbSteps.SelectedIndex = (int) this.iStep;
                    this.eCustom.Text = "";
                    this.eCustom.Enabled = false;
                    this.cbSteps.Focus();
                }
                else
                {
                    this.radioButton2.Checked = true;
                    this.cbSteps.SelectedIndex = -1;
                }
            }
            else
            {
                this.cbSteps.Visible = false;
                this.groupBox1.Visible = false;
                this.radioButton2.Checked = true;
                this.eCustom.Text = this.dIncrement.ToString();
                int top = this.eCustom.Top;
                int num2 = this.label1.Top;
                this.eCustom.Top = this.BitBtn1.Top;
                this.label1.Top = this.eCustom.Top;
                this.label1.Visible = true;
                this.eCustom.Focus();
            }
            EditorUtils.Translate(this);
        }

        private void BitBtn1_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.isDateTime)
                {
                    if (this.radioButton1.Checked)
                    {
                        this.dIncrement = Utils.DateTimeStep[this.cbSteps.SelectedIndex];
                    }
                    else
                    {
                        int index = this.eCustom.Text.IndexOf(" ");
                        if (index == 0)
                        {
                            try
                            {
                                try
                                {
                                    this.dIncrement = Convert.ToDouble(this.eCustom.Text);
                                }
                                catch
                                {
                                    this.dIncrement = Convert.ToDouble(Convert.ToDateTime(this.eCustom.Text).TimeOfDay);
                                }
                            }
                            catch
                            {
                                this.dIncrement = Convert.ToDouble(this.eCustom.Text);
                            }
                        }
                        else
                        {
                            this.dIncrement = Convert.ToDouble(this.eCustom.Text.Substring(1, index - 1)) + Convert.ToDouble(Convert.ToDateTime(this.eCustom.Text.Substring(index + 1, 0xff)).TimeOfDay);
                        }
                    }
                }
                else
                {
                    this.dIncrement = Convert.ToDouble(this.eCustom.Text);
                }
            }
            catch
            {
            }
        }

        private void cbExact_CheckedChanged(object sender, EventArgs e)
        {
            this.isExact = this.cbExact.Checked;
            if (!this.isExact)
            {
                this.radioButton1.Checked = false;
                this.eCustom.Enabled = true;
                this.SetEditText();
                this.eCustom.Focus();
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

        private void DoEnableControls(bool value)
        {
            this.eCustom.Enabled = !value;
            this.cbExact.Checked = value;
            this.isExact = value;
            this.cbExact.Enabled = value;
            this.cbSteps.Enabled = value;
        }

        private static double Frac(double value)
        {
            return (value - ((int) value));
        }

        private static string GetDateTimeStepText(DateTimeSteps tmp)
        {
            using (AxisIncrement increment = new AxisIncrement())
            {
                return increment.cbSteps.Items[(int) tmp].ToString();
            }
        }

        public static string GetIncrementText(Control parent, double increment, bool isDateTime, bool exactDateTime, string aFormat)
        {
            if (!isDateTime)
            {
                return increment.ToString(aFormat);
            }
            DateTimeSteps tmp = Axis.FindDateTimeStep(increment);
            if (exactDateTime && (tmp != DateTimeSteps.None))
            {
                return GetDateTimeStepText(tmp);
            }
            if (increment <= 0.0)
            {
                return Utils.TimeToStr(Utils.DateTimeStep[1]);
            }
            if (increment <= 1.0)
            {
                return Utils.TimeToStr(increment);
            }
            string str = Utils.Round(increment).ToString();
            if (Frac(increment) != 0.0)
            {
                str = str + ' ' + Utils.TimeToStr(Frac(increment));
            }
            return str;
        }

        private void InitializeComponent()
        {
            this.cbExact = new CheckBox();
            this.groupBox1 = new GroupBox();
            this.radioButton2 = new RadioButton();
            this.radioButton1 = new RadioButton();
            this.BitBtn1 = new Button();
            this.BitBtn2 = new Button();
            this.eCustom = new TextBox();
            this.cbSteps = new ComboBox();
            this.label1 = new Label();
            this.groupBox1.SuspendLayout();
            base.SuspendLayout();
            this.cbExact.FlatStyle = FlatStyle.Flat;
            this.cbExact.Location = new Point(0x68, 8);
            this.cbExact.Name = "cbExact";
            this.cbExact.Size = new Size(0xa6, 0x10);
            this.cbExact.TabIndex = 1;
            this.cbExact.Text = "&Exact Date Time";
            this.cbExact.TextAlign = ContentAlignment.TopLeft;
            this.cbExact.CheckedChanged += new EventHandler(this.cbExact_CheckedChanged);
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Location = new Point(8, 0x20);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0xea, 80);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Increment:";
            this.radioButton2.FlatStyle = FlatStyle.Flat;
            this.radioButton2.Location = new Point(8, 0x30);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new Size(0x58, 0x15);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.Text = "&Custom :";
            this.radioButton1.FlatStyle = FlatStyle.Flat;
            this.radioButton1.Location = new Point(8, 0x10);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new Size(0x58, 0x15);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.Text = "&Standard :";
            this.radioButton1.CheckedChanged += new EventHandler(this.radioButton1_CheckedChanged);
            this.BitBtn1.DialogResult = DialogResult.OK;
            this.BitBtn1.FlatStyle = FlatStyle.Flat;
            this.BitBtn1.Location = new Point(0xf8, 0x30);
            this.BitBtn1.Name = "BitBtn1";
            this.BitBtn1.TabIndex = 5;
            this.BitBtn1.Text = "OK";
            this.BitBtn1.Click += new EventHandler(this.BitBtn1_Click);
            this.BitBtn2.DialogResult = DialogResult.Cancel;
            this.BitBtn2.FlatStyle = FlatStyle.Flat;
            this.BitBtn2.Location = new Point(0xf8, 80);
            this.BitBtn2.Name = "BitBtn2";
            this.BitBtn2.TabIndex = 6;
            this.BitBtn2.Text = "Cancel";
            this.eCustom.BorderStyle = BorderStyle.FixedSingle;
            this.eCustom.Location = new Point(0x68, 80);
            this.eCustom.Name = "eCustom";
            this.eCustom.Size = new Size(0x81, 20);
            this.eCustom.TabIndex = 4;
            this.eCustom.Text = "";
            this.cbSteps.Items.AddRange(new object[] { 
                "One Millisecond", "One Second", "Five Seconds", "Ten Seconds", "Fifteen Seconds", "Thirty Seconds", "One Minute", "Five Minutes", "Ten Minutes", "Fifteen Minutes", "Thirty Minutes", "One Hour", "Two Hours", "Six Hours", "Twelve Hours", "One Day", 
                "Two Days", "Three Days", "One Week", "Half Month", "One Month", "Two Months", "Three Months", "Four Months", "Six Months", "One Year"
             });
            this.cbSteps.Location = new Point(0x68, 0x30);
            this.cbSteps.Name = "cbSteps";
            this.cbSteps.Size = new Size(0x81, 0x15);
            this.cbSteps.TabIndex = 3;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x3a, 0x10);
            this.label1.TabIndex = 0;
            this.label1.Text = "Increment:";
            this.label1.UseMnemonic = false;
            this.label1.Visible = false;
            base.AcceptButton = this.BitBtn1;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this.BitBtn2;
            base.ClientSize = new Size(0x147, 0x72);
            base.Controls.Add(this.eCustom);
            base.Controls.Add(this.cbSteps);
            base.Controls.Add(this.BitBtn2);
            base.Controls.Add(this.BitBtn1);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.cbExact);
            base.Controls.Add(this.label1);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Name = "AxisIncrement";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Axis Increment";
            base.Load += new EventHandler(this.AxisIncrement_Load);
            this.groupBox1.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            this.UpdateControls();
        }

        private void SetEditText()
        {
            if (this.dIncrement <= 0.0)
            {
                this.eCustom.Text = Utils.TimeToStr(Utils.DateTimeStep[1]);
            }
            else if (this.dIncrement >= 1.0)
            {
                this.eCustom.Text = ((int) this.dIncrement).ToString();
                if ((this.dIncrement - ((int) this.dIncrement)) != 0.0)
                {
                    this.eCustom.Text = this.eCustom.Text + ' ' + ((this.dIncrement - ((int) this.dIncrement))).ToString();
                }
            }
            else
            {
                this.eCustom.Text = this.dIncrement.ToString();
            }
        }

        private void UpdateControls()
        {
            switch (this.radioButton1.Checked)
            {
                case false:
                    this.DoEnableControls(false);
                    this.SetEditText();
                    this.eCustom.Focus();
                    return;

                case true:
                    this.DoEnableControls(true);
                    if (this.iStep == DateTimeSteps.None)
                    {
                        this.cbSteps.SelectedIndex = 0;
                        break;
                    }
                    this.cbSteps.SelectedIndex = (int) this.iStep;
                    break;

                default:
                    return;
            }
            this.cbSteps.Focus();
        }

        public double Increment
        {
            get
            {
                return this.dIncrement;
            }
        }
    }
}

