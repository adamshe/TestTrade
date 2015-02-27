namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class StackBarSeries : Form
    {
        private CustomBar bar;
        private CheckBox CBYOrigin;
        private Container components;
        private TextBox EYOrigin;
        private GroupBox groupBox1;
        private Label label1;
        private RadioButton radioButton1;
        private RadioButton radioButton2;
        private RadioButton radioButton3;
        private RadioButton radioButton4;
        private RadioButton radioButton5;
        private RadioButton radioButton6;
        private NumericUpDown UDGroup;

        public StackBarSeries()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public StackBarSeries(CustomBar b, Control parent) : this()
        {
            this.bar = b;
            EditorUtils.InsertForm(this, parent);
        }

        private void CBYOrigin_CheckedChanged(object sender, EventArgs e)
        {
            this.bar.UseOrigin = this.CBYOrigin.Checked;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void EYOrigin_TextChanged(object sender, EventArgs e)
        {
            this.bar.Origin = Convert.ToDouble(this.EYOrigin.Text);
        }

        private void InitializeComponent()
        {
            this.groupBox1 = new GroupBox();
            this.radioButton6 = new RadioButton();
            this.radioButton5 = new RadioButton();
            this.radioButton4 = new RadioButton();
            this.radioButton3 = new RadioButton();
            this.radioButton2 = new RadioButton();
            this.radioButton1 = new RadioButton();
            this.CBYOrigin = new CheckBox();
            this.EYOrigin = new TextBox();
            this.label1 = new Label();
            this.UDGroup = new NumericUpDown();
            this.groupBox1.SuspendLayout();
            this.UDGroup.BeginInit();
            base.SuspendLayout();
            this.groupBox1.Controls.Add(this.radioButton6);
            this.groupBox1.Controls.Add(this.radioButton5);
            this.groupBox1.Controls.Add(this.radioButton4);
            this.groupBox1.Controls.Add(this.radioButton3);
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Location = new Point(8, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(160, 0x7c);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "&Multiple Bar:";
            this.radioButton6.FlatStyle = FlatStyle.Flat;
            this.radioButton6.Location = new Point(8, 0x65);
            this.radioButton6.Name = "radioButton6";
            this.radioButton6.Size = new Size(0x80, 0x11);
            this.radioButton6.TabIndex = 5;
            this.radioButton6.Text = "S&elf Stack";
            this.radioButton6.CheckedChanged += new EventHandler(this.radioButton6_CheckedChanged);
            this.radioButton5.FlatStyle = FlatStyle.Flat;
            this.radioButton5.Location = new Point(8, 0x54);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new Size(0x80, 0x11);
            this.radioButton5.TabIndex = 4;
            this.radioButton5.Text = "S&ide All";
            this.radioButton5.CheckedChanged += new EventHandler(this.radioButton5_CheckedChanged);
            this.radioButton4.FlatStyle = FlatStyle.Flat;
            this.radioButton4.Location = new Point(8, 0x43);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new Size(0x80, 0x11);
            this.radioButton4.TabIndex = 3;
            this.radioButton4.Text = "St&acked 100%";
            this.radioButton4.CheckedChanged += new EventHandler(this.radioButton4_CheckedChanged);
            this.radioButton3.FlatStyle = FlatStyle.Flat;
            this.radioButton3.Location = new Point(8, 50);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new Size(0x80, 0x11);
            this.radioButton3.TabIndex = 2;
            this.radioButton3.Text = "S&tacked";
            this.radioButton3.TextAlign = ContentAlignment.TopLeft;
            this.radioButton3.CheckedChanged += new EventHandler(this.radioButton3_CheckedChanged);
            this.radioButton2.FlatStyle = FlatStyle.Flat;
            this.radioButton2.Location = new Point(8, 0x21);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new Size(0x80, 0x11);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.Text = "&Side";
            this.radioButton2.CheckedChanged += new EventHandler(this.radioButton2_CheckedChanged);
            this.radioButton1.FlatStyle = FlatStyle.Flat;
            this.radioButton1.Location = new Point(8, 0x10);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new Size(0x80, 0x11);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.Text = "&None";
            this.radioButton1.CheckedChanged += new EventHandler(this.radioButton1_CheckedChanged);
            this.CBYOrigin.FlatStyle = FlatStyle.Flat;
            this.CBYOrigin.Location = new Point(0xb8, 8);
            this.CBYOrigin.Name = "CBYOrigin";
            this.CBYOrigin.Size = new Size(0x58, 0x10);
            this.CBYOrigin.TabIndex = 1;
            this.CBYOrigin.Text = "Use &Origin:";
            this.CBYOrigin.CheckedChanged += new EventHandler(this.CBYOrigin_CheckedChanged);
            this.EYOrigin.BorderStyle = BorderStyle.FixedSingle;
            this.EYOrigin.Location = new Point(0xb6, 0x1c);
            this.EYOrigin.Name = "EYOrigin";
            this.EYOrigin.Size = new Size(0x48, 20);
            this.EYOrigin.TabIndex = 2;
            this.EYOrigin.Text = "0";
            this.EYOrigin.TextAlign = HorizontalAlignment.Right;
            this.EYOrigin.TextChanged += new EventHandler(this.EYOrigin_TextChanged);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0xb6, 0x48);
            this.label1.Name = "label1";
            this.label1.Size = new Size(70, 0x10);
            this.label1.TabIndex = 3;
            this.label1.Text = "Stack &Group:";
            this.UDGroup.BorderStyle = BorderStyle.FixedSingle;
            this.UDGroup.Location = new Point(0xb8, 0x5b);
            this.UDGroup.Name = "UDGroup";
            this.UDGroup.Size = new Size(0x31, 20);
            this.UDGroup.TabIndex = 4;
            this.UDGroup.TextAlign = HorizontalAlignment.Right;
            this.UDGroup.TextChanged += new EventHandler(this.UDGroup_ValueChanged);
            this.UDGroup.ValueChanged += new EventHandler(this.UDGroup_ValueChanged);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x13a, 0x85);
            base.Controls.Add(this.UDGroup);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.EYOrigin);
            base.Controls.Add(this.CBYOrigin);
            base.Controls.Add(this.groupBox1);
            base.Name = "StackBarSeries";
            base.Load += new EventHandler(this.StackBarSeries_Load);
            this.groupBox1.ResumeLayout(false);
            this.UDGroup.EndInit();
            base.ResumeLayout(false);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            this.bar.MultiBar = MultiBars.None;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            this.bar.MultiBar = MultiBars.Side;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            this.bar.MultiBar = MultiBars.Stacked;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            this.bar.MultiBar = MultiBars.Stacked100;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            this.bar.MultiBar = MultiBars.SideAll;
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            this.bar.MultiBar = MultiBars.SelfStack;
        }

        private void StackBarSeries_Load(object sender, EventArgs e)
        {
            if (this.bar != null)
            {
                this.CBYOrigin.Checked = this.bar.UseOrigin;
                this.EYOrigin.Text = Convert.ToString(this.bar.Origin);
                this.UDGroup.Value = this.bar.StackGroup;
                switch (this.bar.MultiBar)
                {
                    case MultiBars.Side:
                        this.radioButton2.Checked = true;
                        return;

                    case MultiBars.Stacked:
                        this.radioButton3.Checked = true;
                        return;

                    case MultiBars.Stacked100:
                        this.radioButton4.Checked = true;
                        return;

                    case MultiBars.SideAll:
                        this.radioButton5.Checked = true;
                        return;

                    case MultiBars.SelfStack:
                        this.radioButton6.Checked = true;
                        return;
                }
                this.radioButton1.Checked = true;
            }
        }

        private void UDGroup_ValueChanged(object sender, EventArgs e)
        {
            this.bar.StackGroup = (int) this.UDGroup.Value;
        }
    }
}

