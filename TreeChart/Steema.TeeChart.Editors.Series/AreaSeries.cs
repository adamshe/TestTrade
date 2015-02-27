namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class AreaSeries : Form
    {
        private Area area;
        private ButtonColor BAreaColor;
        private ButtonPen BAreaLinePen;
        private ButtonPen BAreaLinesPen;
        private Button button1;
        private Button button2;
        private CheckBox CBColorEach;
        private CheckBox CBInvStairs;
        private CheckBox CBStairs;
        private CheckBox CBUseOrigin;
        private Container components;
        private GroupBox groupBox1;
        private GroupBox groupBox3;
        private GroupBox groupBox4;
        private GroupBox groupBox5;
        private Label label2;
        private RadioButton radioButton1;
        private RadioButton radioButton2;
        private RadioButton radioButton3;
        private NumericUpDown UDOrigin;

        public AreaSeries()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public AreaSeries(Series s) : this()
        {
            this.area = (Area) s;
            this.CBStairs.Checked = this.area.Stairs;
            this.CBInvStairs.Checked = this.area.InvertedStairs;
            this.CBInvStairs.Enabled = this.CBStairs.Checked;
            this.CBColorEach.Checked = this.area.ColorEach;
            this.CBUseOrigin.Checked = this.area.UseOrigin;
            this.UDOrigin.Value = Convert.ToDecimal(this.area.Origin);
            this.BAreaColor.Color = this.area.Color;
            this.BAreaLinePen.Pen = this.area.LinePen;
            this.BAreaLinesPen.Pen = this.area.AreaLines;
            if (this.area.MultiArea == MultiAreas.None)
            {
                this.radioButton1.Checked = true;
            }
            else if (this.area.MultiArea == MultiAreas.Stacked)
            {
                this.radioButton2.Checked = true;
            }
            else if (this.area.MultiArea == MultiAreas.Stacked100)
            {
                this.radioButton3.Checked = true;
            }
        }

        private void BAreaColor_Click(object sender, EventArgs e)
        {
            this.area.Color = this.BAreaColor.Color;
            this.CBColorEach.Checked = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BrushEditor.Edit(this.area.AreaBrush);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            BrushEditor.Edit(this.area.Brush);
        }

        private void CBColorEach_CheckedChanged(object sender, EventArgs e)
        {
            this.area.ColorEach = this.CBColorEach.Checked;
        }

        private void CBInvStairs_CheckedChanged(object sender, EventArgs e)
        {
            this.area.InvertedStairs = this.CBInvStairs.Checked;
        }

        private void CBStairs_CheckedChanged(object sender, EventArgs e)
        {
            this.area.Stairs = this.CBStairs.Checked;
            this.CBInvStairs.Enabled = this.CBStairs.Checked;
        }

        private void CBUseOrigin_CheckedChanged(object sender, EventArgs e)
        {
            this.area.UseOrigin = this.CBUseOrigin.Checked;
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
            this.groupBox1 = new GroupBox();
            this.button2 = new Button();
            this.button1 = new Button();
            this.CBInvStairs = new CheckBox();
            this.CBStairs = new CheckBox();
            this.BAreaLinesPen = new ButtonPen();
            this.BAreaLinePen = new ButtonPen();
            this.groupBox3 = new GroupBox();
            this.radioButton3 = new RadioButton();
            this.radioButton1 = new RadioButton();
            this.radioButton2 = new RadioButton();
            this.groupBox4 = new GroupBox();
            this.BAreaColor = new ButtonColor();
            this.CBColorEach = new CheckBox();
            this.groupBox5 = new GroupBox();
            this.UDOrigin = new NumericUpDown();
            this.label2 = new Label();
            this.CBUseOrigin = new CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.UDOrigin.BeginInit();
            base.SuspendLayout();
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.CBInvStairs);
            this.groupBox1.Controls.Add(this.CBStairs);
            this.groupBox1.Controls.Add(this.BAreaLinesPen);
            this.groupBox1.Controls.Add(this.BAreaLinePen);
            this.groupBox1.Location = new Point(4, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x129, 0x49);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Tag = "0";
            this.groupBox1.Text = "Area :";
            this.button2.FlatStyle = FlatStyle.Flat;
            this.button2.Location = new Point(0x70, 12);
            this.button2.Name = "button2";
            this.button2.TabIndex = 2;
            this.button2.Text = "&Pattern...";
            this.button2.Click += new EventHandler(this.button2_Click);
            this.button1.FlatStyle = FlatStyle.Flat;
            this.button1.Location = new Point(0x70, 40);
            this.button1.Name = "button1";
            this.button1.TabIndex = 4;
            this.button1.Text = "P&attern...";
            this.button1.Click += new EventHandler(this.button1_Click);
            this.CBInvStairs.FlatStyle = FlatStyle.Flat;
            this.CBInvStairs.Location = new Point(12, 0x2c);
            this.CBInvStairs.Name = "CBInvStairs";
            this.CBInvStairs.Size = new Size(0x5c, 0x10);
            this.CBInvStairs.TabIndex = 1;
            this.CBInvStairs.Text = "&Inverted";
            this.CBInvStairs.CheckedChanged += new EventHandler(this.CBInvStairs_CheckedChanged);
            this.CBStairs.FlatStyle = FlatStyle.Flat;
            this.CBStairs.Location = new Point(12, 0x15);
            this.CBStairs.Name = "CBStairs";
            this.CBStairs.Size = new Size(0x5c, 0x10);
            this.CBStairs.TabIndex = 0;
            this.CBStairs.Text = "&Stairs";
            this.CBStairs.CheckedChanged += new EventHandler(this.CBStairs_CheckedChanged);
            this.BAreaLinesPen.FlatStyle = FlatStyle.Flat;
            this.BAreaLinesPen.Location = new Point(0xc4, 40);
            this.BAreaLinesPen.Name = "BAreaLinesPen";
            this.BAreaLinesPen.Size = new Size(0x59, 0x17);
            this.BAreaLinesPen.TabIndex = 5;
            this.BAreaLinesPen.Text = "Area &Lines...";
            this.BAreaLinePen.FlatStyle = FlatStyle.Flat;
            this.BAreaLinePen.Location = new Point(0xc4, 12);
            this.BAreaLinePen.Name = "BAreaLinePen";
            this.BAreaLinePen.Size = new Size(0x59, 0x17);
            this.BAreaLinePen.TabIndex = 3;
            this.BAreaLinePen.Text = "&Border...";
            this.groupBox3.Controls.Add(this.radioButton3);
            this.groupBox3.Controls.Add(this.radioButton1);
            this.groupBox3.Controls.Add(this.radioButton2);
            this.groupBox3.Location = new Point(4, 80);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new Size(0x8d, 0x49);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "&Multiple Areas:";
            this.radioButton3.FlatStyle = FlatStyle.Flat;
            this.radioButton3.Location = new Point(0x18, 0x30);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new Size(0x60, 0x10);
            this.radioButton3.TabIndex = 2;
            this.radioButton3.Text = "Stac&k 100%";
            this.radioButton3.CheckedChanged += new EventHandler(this.radioButton3_CheckedChanged);
            this.radioButton1.FlatStyle = FlatStyle.Flat;
            this.radioButton1.Location = new Point(0x18, 0x10);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new Size(0x60, 0x10);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.Text = "&None";
            this.radioButton1.CheckedChanged += new EventHandler(this.radioButton1_CheckedChanged);
            this.radioButton2.FlatStyle = FlatStyle.Flat;
            this.radioButton2.Location = new Point(0x18, 0x20);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new Size(0x60, 0x10);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.Text = "S&tack";
            this.radioButton2.CheckedChanged += new EventHandler(this.radioButton2_CheckedChanged);
            this.groupBox4.Controls.Add(this.BAreaColor);
            this.groupBox4.Controls.Add(this.CBColorEach);
            this.groupBox4.Location = new Point(0xb3, 80);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new Size(0x79, 0x49);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Color:";
            this.BAreaColor.Color = Color.Empty;
            this.BAreaColor.Location = new Point(14, 40);
            this.BAreaColor.Name = "BAreaColor";
            this.BAreaColor.TabIndex = 1;
            this.BAreaColor.Text = "&Color...";
            this.BAreaColor.Click += new EventHandler(this.BAreaColor_Click);
            this.CBColorEach.FlatStyle = FlatStyle.Flat;
            this.CBColorEach.Location = new Point(14, 0x10);
            this.CBColorEach.Name = "CBColorEach";
            this.CBColorEach.Size = new Size(0x62, 0x10);
            this.CBColorEach.TabIndex = 0;
            this.CBColorEach.Text = "Color &Each";
            this.CBColorEach.CheckedChanged += new EventHandler(this.CBColorEach_CheckedChanged);
            this.groupBox5.Controls.Add(this.UDOrigin);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Controls.Add(this.CBUseOrigin);
            this.groupBox5.Location = new Point(4, 0x9c);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new Size(0x129, 0x27);
            this.groupBox5.TabIndex = 3;
            this.groupBox5.TabStop = false;
            this.UDOrigin.BorderStyle = BorderStyle.FixedSingle;
            this.UDOrigin.Location = new Point(0xc3, 13);
            int[] bits = new int[4];
            bits[0] = 0x540be400;
            bits[1] = 2;
            this.UDOrigin.Maximum = new decimal(bits);
            this.UDOrigin.Minimum = new decimal(new int[] { 0x540be400, 2, 0, -2147483648 });
            this.UDOrigin.Name = "UDOrigin";
            this.UDOrigin.Size = new Size(80, 20);
            this.UDOrigin.TabIndex = 2;
            this.UDOrigin.TextAlign = HorizontalAlignment.Right;
            this.UDOrigin.TextChanged += new EventHandler(this.UDOrigin_ValueChanged);
            this.UDOrigin.ValueChanged += new EventHandler(this.UDOrigin_ValueChanged);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x98, 15);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x26, 0x10);
            this.label2.TabIndex = 1;
            this.label2.Text = "&Origin:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.CBUseOrigin.FlatStyle = FlatStyle.Flat;
            this.CBUseOrigin.Location = new Point(8, 15);
            this.CBUseOrigin.Name = "CBUseOrigin";
            this.CBUseOrigin.Size = new Size(0x68, 0x10);
            this.CBUseOrigin.TabIndex = 0;
            this.CBUseOrigin.Text = "&Use Origin";
            this.CBUseOrigin.CheckedChanged += new EventHandler(this.CBUseOrigin_CheckedChanged);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x131, 0xc7);
            base.Controls.Add(this.groupBox5);
            base.Controls.Add(this.groupBox4);
            base.Controls.Add(this.groupBox3);
            base.Controls.Add(this.groupBox1);
            base.Name = "AreaSeries";
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.UDOrigin.EndInit();
            base.ResumeLayout(false);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            this.area.MultiArea = MultiAreas.None;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            this.area.MultiArea = MultiAreas.Stacked;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            this.area.MultiArea = MultiAreas.Stacked100;
        }

        private void UDOrigin_ValueChanged(object sender, EventArgs e)
        {
            this.area.Origin = Convert.ToDouble(this.UDOrigin.Value);
        }
    }
}

