namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class PieSeries : Form
    {
        private Button BPen;
        private Button BShadowColor;
        private CheckBox CBAutoMarkPosition;
        private CheckBox CBDark3d;
        private ComboBox CBOther;
        private CheckBox CBPatterns;
        private CheckBox cbShadowVisible;
        private CircledSeries circledEditor;
        private Container components;
        private TextBox EOtherLabel;
        private TextBox EOtherValue;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Pie pie;
        private NumericUpDown UDAngleSize;
        private NumericUpDown UDExpBig;
        private NumericUpDown UDShadowHoriz;
        private NumericUpDown UDShadowVert;

        public PieSeries()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public PieSeries(Series s) : this()
        {
            this.pie = (Pie) s;
        }

        public PieSeries(Pie s, Control parent, CircledSeries cEditor) : this()
        {
            this.pie = s;
            this.circledEditor = cEditor;
            EditorUtils.InsertForm(this, parent);
        }

        private void BPen_Click(object sender, EventArgs e)
        {
            PenEditor.Edit(this.pie.Pen);
        }

        private void BShadowColor_Click(object sender, EventArgs e)
        {
            BrushEditor.Edit(this.pie.Shadow.Brush);
        }

        private void CBAutoMarkPosition_CheckedChanged(object sender, EventArgs e)
        {
            this.pie.AutoMarkPosition = this.CBAutoMarkPosition.Checked;
        }

        private void CBDark3d_CheckedChanged(object sender, EventArgs e)
        {
            this.pie.Dark3D = this.CBDark3d.Checked;
        }

        private void CBOther_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.CBOther.SelectedIndex)
            {
                case 0:
                    this.pie.OtherSlice.Style = PieOtherStyles.None;
                    return;

                case 1:
                    this.pie.OtherSlice.Style = PieOtherStyles.BelowPercent;
                    return;

                case 2:
                    this.pie.OtherSlice.Style = PieOtherStyles.BelowValue;
                    return;
            }
        }

        private void CBPatterns_CheckedChanged(object sender, EventArgs e)
        {
            this.pie.UsePatterns = this.CBPatterns.Checked;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.pie.Shadow.Visible = this.cbShadowVisible.Checked;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void EOtherLabel_TextChanged(object sender, EventArgs e)
        {
            if (this.pie != null)
            {
                this.pie.OtherSlice.Text = this.EOtherLabel.Text;
            }
        }

        private void EOtherValue_TextChanged(object sender, EventArgs e)
        {
            if (this.EOtherValue.Text.Length != 0)
            {
                this.pie.OtherSlice.Value = Convert.ToDouble(this.EOtherValue.Text);
            }
        }

        private void InitializeComponent()
        {
            this.CBDark3d = new CheckBox();
            this.CBPatterns = new CheckBox();
            this.BPen = new Button();
            this.label1 = new Label();
            this.label2 = new Label();
            this.UDExpBig = new NumericUpDown();
            this.UDAngleSize = new NumericUpDown();
            this.groupBox1 = new GroupBox();
            this.EOtherLabel = new TextBox();
            this.EOtherValue = new TextBox();
            this.CBOther = new ComboBox();
            this.label5 = new Label();
            this.label4 = new Label();
            this.label3 = new Label();
            this.groupBox2 = new GroupBox();
            this.cbShadowVisible = new CheckBox();
            this.UDShadowVert = new NumericUpDown();
            this.UDShadowHoriz = new NumericUpDown();
            this.label7 = new Label();
            this.label6 = new Label();
            this.BShadowColor = new Button();
            this.CBAutoMarkPosition = new CheckBox();
            this.UDExpBig.BeginInit();
            this.UDAngleSize.BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.UDShadowVert.BeginInit();
            this.UDShadowHoriz.BeginInit();
            base.SuspendLayout();
            this.CBDark3d.FlatStyle = FlatStyle.Flat;
            this.CBDark3d.Location = new Point(0xae, 0x22);
            this.CBDark3d.Name = "CBDark3d";
            this.CBDark3d.Size = new Size(0x7a, 20);
            this.CBDark3d.TabIndex = 5;
            this.CBDark3d.Text = "&Dark 3D";
            this.CBDark3d.CheckedChanged += new EventHandler(this.CBDark3d_CheckedChanged);
            this.CBPatterns.FlatStyle = FlatStyle.Flat;
            this.CBPatterns.Location = new Point(290, 0x22);
            this.CBPatterns.Name = "CBPatterns";
            this.CBPatterns.Size = new Size(0x4b, 20);
            this.CBPatterns.TabIndex = 6;
            this.CBPatterns.Text = "Pa&tterns";
            this.CBPatterns.CheckedChanged += new EventHandler(this.CBPatterns_CheckedChanged);
            this.BPen.FlatStyle = FlatStyle.Flat;
            this.BPen.Location = new Point(0xae, 9);
            this.BPen.Name = "BPen";
            this.BPen.TabIndex = 4;
            this.BPen.Text = "&Border...";
            this.BPen.Click += new EventHandler(this.BPen_Click);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x1f, 13);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x58, 0x10);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Explode biggest:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x38, 0x2a);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x3f, 0x10);
            this.label2.TabIndex = 2;
            this.label2.Text = "Total angle:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.UDExpBig.BorderStyle = BorderStyle.FixedSingle;
            int[] bits = new int[4];
            bits[0] = 5;
            this.UDExpBig.Increment = new decimal(bits);
            this.UDExpBig.Location = new Point(0x77, 10);
            this.UDExpBig.Name = "UDExpBig";
            this.UDExpBig.Size = new Size(0x30, 20);
            this.UDExpBig.TabIndex = 1;
            this.UDExpBig.TextAlign = HorizontalAlignment.Right;
            this.UDExpBig.TextChanged += new EventHandler(this.UDExpBig_TextChanged);
            this.UDExpBig.ValueChanged += new EventHandler(this.UDExpBig_ValueChanged);
            this.UDAngleSize.BorderStyle = BorderStyle.FixedSingle;
            bits = new int[4];
            bits[0] = 5;
            this.UDAngleSize.Increment = new decimal(bits);
            this.UDAngleSize.Location = new Point(0x77, 40);
            bits = new int[4];
            bits[0] = 360;
            this.UDAngleSize.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 1;
            this.UDAngleSize.Minimum = new decimal(bits);
            this.UDAngleSize.Name = "UDAngleSize";
            this.UDAngleSize.Size = new Size(0x30, 20);
            this.UDAngleSize.TabIndex = 3;
            this.UDAngleSize.TextAlign = HorizontalAlignment.Right;
            bits = new int[4];
            bits[0] = 360;
            this.UDAngleSize.Value = new decimal(bits);
            this.UDAngleSize.ValueChanged += new EventHandler(this.UDAngleSize_ValueChanged);
            this.groupBox1.Controls.Add(this.EOtherLabel);
            this.groupBox1.Controls.Add(this.EOtherValue);
            this.groupBox1.Controls.Add(this.CBOther);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new Point(4, 0x52);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0xa4, 0x63);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Group slices:";
            this.EOtherLabel.BorderStyle = BorderStyle.FixedSingle;
            this.EOtherLabel.Location = new Point(0x40, 0x45);
            this.EOtherLabel.Name = "EOtherLabel";
            this.EOtherLabel.Size = new Size(0x60, 20);
            this.EOtherLabel.TabIndex = 5;
            this.EOtherLabel.Text = "Other";
            this.EOtherLabel.TextChanged += new EventHandler(this.EOtherLabel_TextChanged);
            this.EOtherValue.BorderStyle = BorderStyle.FixedSingle;
            this.EOtherValue.Location = new Point(0x40, 0x2d);
            this.EOtherValue.Name = "EOtherValue";
            this.EOtherValue.Size = new Size(0x38, 20);
            this.EOtherValue.TabIndex = 3;
            this.EOtherValue.Text = "0";
            this.EOtherValue.TextAlign = HorizontalAlignment.Right;
            this.EOtherValue.TextChanged += new EventHandler(this.EOtherValue_TextChanged);
            this.CBOther.DropDownStyle = ComboBoxStyle.DropDownList;
            this.CBOther.Items.AddRange(new object[] { "None", "Below %", "Below Value" });
            this.CBOther.Location = new Point(0x40, 0x15);
            this.CBOther.Name = "CBOther";
            this.CBOther.Size = new Size(0x60, 0x15);
            this.CBOther.TabIndex = 1;
            this.CBOther.SelectedIndexChanged += new EventHandler(this.CBOther_SelectedIndexChanged);
            this.label5.AutoSize = true;
            this.label5.Location = new Point(0x1c, 0x47);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x23, 0x10);
            this.label5.TabIndex = 4;
            this.label5.Text = "&Label:";
            this.label5.TextAlign = ContentAlignment.TopRight;
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0x1c, 0x2f);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x24, 0x10);
            this.label4.TabIndex = 2;
            this.label4.Text = "&Value:";
            this.label4.TextAlign = ContentAlignment.TopRight;
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0x1f, 0x17);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x21, 0x10);
            this.label3.TabIndex = 0;
            this.label3.Text = "&Style:";
            this.label3.TextAlign = ContentAlignment.TopRight;
            this.groupBox2.Controls.Add(this.cbShadowVisible);
            this.groupBox2.Controls.Add(this.UDShadowVert);
            this.groupBox2.Controls.Add(this.UDShadowHoriz);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.BShadowColor);
            this.groupBox2.Location = new Point(0xac, 0x52);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(0xbc, 0x63);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Shadow:";
            this.cbShadowVisible.FlatStyle = FlatStyle.Flat;
            this.cbShadowVisible.Location = new Point(10, 0x15);
            this.cbShadowVisible.Name = "cbShadowVisible";
            this.cbShadowVisible.Size = new Size(0x41, 0x10);
            this.cbShadowVisible.TabIndex = 0;
            this.cbShadowVisible.Text = "&Visible";
            this.cbShadowVisible.CheckedChanged += new EventHandler(this.checkBox1_CheckedChanged);
            this.UDShadowVert.BorderStyle = BorderStyle.FixedSingle;
            this.UDShadowVert.Location = new Point(0x7b, 70);
            bits = new int[4];
            bits[0] = 0x3e8;
            this.UDShadowVert.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 0x3e8;
            bits[3] = -2147483648;
            this.UDShadowVert.Minimum = new decimal(bits);
            this.UDShadowVert.Name = "UDShadowVert";
            this.UDShadowVert.Size = new Size(0x38, 20);
            this.UDShadowVert.TabIndex = 5;
            this.UDShadowVert.TextAlign = HorizontalAlignment.Right;
            this.UDShadowVert.TextChanged += new EventHandler(this.UDShadowVert_TextChanged);
            this.UDShadowVert.ValueChanged += new EventHandler(this.UDShadowVert_ValueChanged);
            this.UDShadowHoriz.BorderStyle = BorderStyle.FixedSingle;
            this.UDShadowHoriz.Location = new Point(0x7b, 0x2e);
            bits = new int[4];
            bits[0] = 0x3e8;
            this.UDShadowHoriz.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 0x3e8;
            bits[3] = -2147483648;
            this.UDShadowHoriz.Minimum = new decimal(bits);
            this.UDShadowHoriz.Name = "UDShadowHoriz";
            this.UDShadowHoriz.Size = new Size(0x38, 20);
            this.UDShadowHoriz.TabIndex = 3;
            this.UDShadowHoriz.TextAlign = HorizontalAlignment.Right;
            this.UDShadowHoriz.TextChanged += new EventHandler(this.UDShadowHoriz_TextChanged);
            this.UDShadowHoriz.ValueChanged += new EventHandler(this.UDShadowHoriz_ValueChanged);
            this.label7.AutoSize = true;
            this.label7.Location = new Point(0x37, 0x30);
            this.label7.Name = "label7";
            this.label7.Size = new Size(0x3e, 0x10);
            this.label7.TabIndex = 2;
            this.label7.Text = "&Horiz. Size:";
            this.label7.TextAlign = ContentAlignment.TopRight;
            this.label6.AutoSize = true;
            this.label6.Location = new Point(0x3d, 0x48);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x38, 0x10);
            this.label6.TabIndex = 4;
            this.label6.Text = "&Vert. Size:";
            this.label6.TextAlign = ContentAlignment.TopRight;
            this.BShadowColor.FlatStyle = FlatStyle.Flat;
            this.BShadowColor.Location = new Point(0x6f, 0x10);
            this.BShadowColor.Name = "BShadowColor";
            this.BShadowColor.Size = new Size(0x44, 0x17);
            this.BShadowColor.TabIndex = 1;
            this.BShadowColor.Text = "&Color...";
            this.BShadowColor.Click += new EventHandler(this.BShadowColor_Click);
            this.CBAutoMarkPosition.FlatStyle = FlatStyle.Flat;
            this.CBAutoMarkPosition.Location = new Point(0xae, 0x3a);
            this.CBAutoMarkPosition.Name = "CBAutoMarkPosition";
            this.CBAutoMarkPosition.Size = new Size(0xba, 20);
            this.CBAutoMarkPosition.TabIndex = 7;
            this.CBAutoMarkPosition.Text = "&Auto Mark Position";
            this.CBAutoMarkPosition.CheckedChanged += new EventHandler(this.CBAutoMarkPosition_CheckedChanged);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x170, 190);
            base.Controls.Add(this.CBAutoMarkPosition);
            base.Controls.Add(this.groupBox2);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.UDAngleSize);
            base.Controls.Add(this.UDExpBig);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.BPen);
            base.Controls.Add(this.CBPatterns);
            base.Controls.Add(this.CBDark3d);
            base.Name = "PieSeries";
            base.Load += new EventHandler(this.PieSeries_Load);
            this.UDExpBig.EndInit();
            this.UDAngleSize.EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.UDShadowVert.EndInit();
            this.UDShadowHoriz.EndInit();
            base.ResumeLayout(false);
        }

        private void PieSeries_Load(object sender, EventArgs e)
        {
            if (this.pie != null)
            {
                if (this.circledEditor == null)
                {
                    this.circledEditor = CircledSeries.InsertForm(base.Parent, this.pie);
                }
                this.CBDark3d.Checked = this.pie.Dark3D;
                this.CBPatterns.Checked = this.pie.UsePatterns;
                this.UDExpBig.Value = this.pie.ExplodeBiggest;
                this.UDAngleSize.Value = this.pie.AngleSize;
                this.cbShadowVisible.Checked = this.pie.Shadow.Visible;
                this.CBAutoMarkPosition.Checked = this.pie.AutoMarkPosition;
                if (this.pie.OtherSlice.Style == PieOtherStyles.None)
                {
                    this.CBOther.SelectedIndex = 0;
                }
                else if (this.pie.OtherSlice.Style == PieOtherStyles.BelowPercent)
                {
                    this.CBOther.SelectedIndex = 1;
                }
                else if (this.pie.OtherSlice.Style == PieOtherStyles.BelowValue)
                {
                    this.CBOther.SelectedIndex = 2;
                }
                this.EOtherValue.Text = this.pie.OtherSlice.Value.ToString();
                this.EOtherLabel.Text = this.pie.OtherSlice.Text;
                this.UDShadowHoriz.Value = this.pie.Shadow.Width;
                this.UDShadowVert.Value = this.pie.Shadow.Height;
            }
        }

        private void UDAngleSize_ValueChanged(object sender, EventArgs e)
        {
            this.pie.AngleSize = (int) this.UDAngleSize.Value;
        }

        private void UDExpBig_TextChanged(object sender, EventArgs e)
        {
            this.UDExpBig_ValueChanged(sender, e);
        }

        private void UDExpBig_ValueChanged(object sender, EventArgs e)
        {
            this.pie.ExplodeBiggest = (int) this.UDExpBig.Value;
        }

        private void UDShadowHoriz_TextChanged(object sender, EventArgs e)
        {
            this.UDShadowHoriz_ValueChanged(sender, e);
        }

        private void UDShadowHoriz_ValueChanged(object sender, EventArgs e)
        {
            this.pie.Shadow.Width = (int) this.UDShadowHoriz.Value;
        }

        private void UDShadowVert_TextChanged(object sender, EventArgs e)
        {
            this.UDShadowVert_ValueChanged(sender, e);
        }

        private void UDShadowVert_ValueChanged(object sender, EventArgs e)
        {
            this.pie.Shadow.Height = (int) this.UDShadowVert.Value;
        }
    }
}

