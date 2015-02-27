namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class AxisMaxMin : Form
    {
        private Button BitBtn1;
        private Button BitBtn2;
        private Container components = null;
        private TextBox EMaximum;
        private TextBox EMinimum;
        protected internal bool isDateTime;
        private Label label1;
        private Label label2;
        protected internal double maxMin;

        public AxisMaxMin()
        {
            this.InitializeComponent();
        }

        private void AxisMaxMin_Load(object sender, EventArgs e)
        {
            if (this.isDateTime)
            {
                DateTime time = DateTime.FromOADate(this.maxMin);
                if (this.maxMin >= 1.0)
                {
                    this.EMaximum.Text = time.ToShortDateString();
                }
                else
                {
                    this.label1.Visible = false;
                    this.EMaximum.Visible = false;
                }
                this.EMinimum.Text = time.TimeOfDay.ToString();
            }
            else
            {
                this.label1.Text = Texts.AxisDlgValue;
                this.EMaximum.Text = this.maxMin.ToString();
                this.label2.Visible = false;
                this.EMinimum.Visible = false;
            }
            EditorUtils.Translate(this);
        }

        private void BitBtn1_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.isDateTime)
                {
                    if (this.EMaximum.Visible)
                    {
                        this.maxMin = DateTime.Parse(this.EMaximum.Text + ' ' + this.EMinimum.Text).ToOADate();
                    }
                    else
                    {
                        this.maxMin = DateTime.Parse(this.EMaximum.Text).ToOADate();
                    }
                }
                else
                {
                    this.maxMin = Convert.ToDouble(this.EMaximum.Text);
                }
            }
            catch
            {
                MessageBox.Show(Texts.IncorrectMaxMinValue);
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
            this.label2 = new Label();
            this.EMinimum = new TextBox();
            this.EMaximum = new TextBox();
            this.BitBtn1 = new Button();
            this.BitBtn2 = new Button();
            base.SuspendLayout();
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x20, 0x11);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x1f, 0x10);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Date:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x20, 0x2e);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x21, 0x10);
            this.label2.TabIndex = 2;
            this.label2.Text = "&Time:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.EMinimum.BorderStyle = BorderStyle.FixedSingle;
            this.EMinimum.Location = new Point(0x48, 0x2c);
            this.EMinimum.Name = "EMinimum";
            this.EMinimum.Size = new Size(0x6c, 20);
            this.EMinimum.TabIndex = 3;
            this.EMinimum.Text = "";
            this.EMaximum.BorderStyle = BorderStyle.FixedSingle;
            this.EMaximum.Location = new Point(0x48, 15);
            this.EMaximum.Name = "EMaximum";
            this.EMaximum.Size = new Size(0x6c, 20);
            this.EMaximum.TabIndex = 1;
            this.EMaximum.Text = "";
            this.BitBtn1.DialogResult = DialogResult.OK;
            this.BitBtn1.FlatStyle = FlatStyle.Flat;
            this.BitBtn1.Location = new Point(0xbf, 11);
            this.BitBtn1.Name = "BitBtn1";
            this.BitBtn1.TabIndex = 4;
            this.BitBtn1.Text = "OK";
            this.BitBtn1.Click += new EventHandler(this.BitBtn1_Click);
            this.BitBtn2.DialogResult = DialogResult.Cancel;
            this.BitBtn2.FlatStyle = FlatStyle.Flat;
            this.BitBtn2.Location = new Point(0xbf, 0x2b);
            this.BitBtn2.Name = "BitBtn2";
            this.BitBtn2.TabIndex = 5;
            this.BitBtn2.Text = "Cancel";
            base.AcceptButton = this.BitBtn1;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x112, 0x4f);
            base.Controls.Add(this.BitBtn2);
            base.Controls.Add(this.BitBtn1);
            base.Controls.Add(this.EMaximum);
            base.Controls.Add(this.EMinimum);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Name = "AxisMaxMin";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Axis Maximum and Minimum";
            base.Load += new EventHandler(this.AxisMaxMin_Load);
            base.ResumeLayout(false);
        }
    }
}

