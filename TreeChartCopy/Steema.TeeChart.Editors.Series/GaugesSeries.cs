namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class GaugesSeries : Form
    {
        private Button BCenter;
        private Button BEndPoint;
        private Button BFont;
        private ButtonPen BPAxis;
        private ButtonPen BPLine;
        private ButtonPen BPMinor;
        private ButtonPen BPTicks;
        private CheckBox CBInside;
        private CheckBox CBLabels;
        private ComboBox CBStyle;
        private Container components;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private NumericUpDown numericUpDown1;
        private NumericUpDown numericUpDown2;
        private NumericUpDown numericUpDown3;
        private NumericUpDown numericUpDown4;
        private Gauges series;
        private TabControl tabControl1;
        private TabPage tabLabels;
        private TabPage tabOptions;
        private TabPage tabTicks;
        private TextBox textBox1;
        private NumericUpDown UDDistance;
        private NumericUpDown UDMax;
        private NumericUpDown UDMin;
        private NumericUpDown UDTotalAngle;
        private NumericUpDown UDValue;

        public GaugesSeries()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public GaugesSeries(Series s) : this()
        {
            this.series = (Gauges) s;
            this.BPLine.Pen = this.series.Pen;
            this.BPAxis.Pen = this.series.GetVertAxis.AxisPen;
            switch (this.series.HandStyle)
            {
                case HandStyle.Line:
                    this.CBStyle.SelectedIndex = 0;
                    break;

                case HandStyle.Triangle:
                    this.CBStyle.SelectedIndex = 1;
                    break;
            }
            this.UDValue.Value = (decimal) this.series.Value;
            this.UDTotalAngle.Value = (decimal) this.series.TotalAngle;
            this.UDDistance.Value = this.series.HandDistance;
            this.CBLabels.Checked = this.series.GetVertAxis.Labels.Visible;
            this.CBInside.Checked = this.series.LabelsInside;
            this.textBox1.Text = this.series.GetVertAxis.Labels.ValueFormat;
            this.numericUpDown1.Value = this.series.GetVertAxis.Ticks.Length;
            this.numericUpDown2.Value = this.series.MinorTickDistance;
            this.numericUpDown3.Value = this.series.GetVertAxis.MinorTickCount;
            this.numericUpDown4.Value = this.series.GetVertAxis.MinorTicks.Length;
            this.UDMax.Value = (decimal) this.series.Maximum;
            this.UDMin.Value = (decimal) this.series.Minimum;
        }

        private void BAxis_Click(object sender, EventArgs e)
        {
        }

        private void BCenter_Click(object sender, EventArgs e)
        {
            Steema.TeeChart.Editors.SeriesPointer.ShowModal(this.series.Center);
        }

        private void BEndPoint_Click(object sender, EventArgs e)
        {
            Steema.TeeChart.Editors.SeriesPointer.ShowModal(this.series.EndPoint);
        }

        private void BFont_Click(object sender, EventArgs e)
        {
            EditorUtils.EditFont(this.series.GetVertAxis.Labels.Font);
        }

        private void BPMinor_Click(object sender, EventArgs e)
        {
        }

        private void CBInside_Click(object sender, EventArgs e)
        {
            this.series.LabelsInside = this.CBInside.Checked;
        }

        private void CBLabels_Click(object sender, EventArgs e)
        {
            this.series.GetVertAxis.Labels.Visible = this.CBLabels.Checked;
        }

        private void CBStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.CBStyle.SelectedIndex)
            {
                case 0:
                    this.series.HandStyle = HandStyle.Line;
                    return;

                case 1:
                    this.series.HandStyle = HandStyle.Triangle;
                    return;
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
            this.tabControl1 = new TabControl();
            this.tabOptions = new TabPage();
            this.BPAxis = new ButtonPen();
            this.BPLine = new ButtonPen();
            this.BEndPoint = new Button();
            this.UDDistance = new NumericUpDown();
            this.UDTotalAngle = new NumericUpDown();
            this.UDValue = new NumericUpDown();
            this.label3 = new Label();
            this.label2 = new Label();
            this.label1 = new Label();
            this.BCenter = new Button();
            this.CBStyle = new ComboBox();
            this.tabTicks = new TabPage();
            this.numericUpDown4 = new NumericUpDown();
            this.numericUpDown3 = new NumericUpDown();
            this.numericUpDown2 = new NumericUpDown();
            this.numericUpDown1 = new NumericUpDown();
            this.UDMin = new NumericUpDown();
            this.UDMax = new NumericUpDown();
            this.label6 = new Label();
            this.label5 = new Label();
            this.BPMinor = new ButtonPen();
            this.BPTicks = new ButtonPen();
            this.tabLabels = new TabPage();
            this.textBox1 = new TextBox();
            this.label4 = new Label();
            this.BFont = new Button();
            this.CBInside = new CheckBox();
            this.CBLabels = new CheckBox();
            this.tabControl1.SuspendLayout();
            this.tabOptions.SuspendLayout();
            this.UDDistance.BeginInit();
            this.UDTotalAngle.BeginInit();
            this.UDValue.BeginInit();
            this.tabTicks.SuspendLayout();
            this.numericUpDown4.BeginInit();
            this.numericUpDown3.BeginInit();
            this.numericUpDown2.BeginInit();
            this.numericUpDown1.BeginInit();
            this.UDMin.BeginInit();
            this.UDMax.BeginInit();
            this.tabLabels.SuspendLayout();
            base.SuspendLayout();
            this.tabControl1.Controls.Add(this.tabOptions);
            this.tabControl1.Controls.Add(this.tabTicks);
            this.tabControl1.Controls.Add(this.tabLabels);
            this.tabControl1.Dock = DockStyle.Fill;
            this.tabControl1.Location = new Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new Size(0x158, 0xb6);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new EventHandler(this.tabControl1_SelectedIndexChanged);
            this.tabOptions.Controls.Add(this.BPAxis);
            this.tabOptions.Controls.Add(this.BPLine);
            this.tabOptions.Controls.Add(this.BEndPoint);
            this.tabOptions.Controls.Add(this.UDDistance);
            this.tabOptions.Controls.Add(this.UDTotalAngle);
            this.tabOptions.Controls.Add(this.UDValue);
            this.tabOptions.Controls.Add(this.label3);
            this.tabOptions.Controls.Add(this.label2);
            this.tabOptions.Controls.Add(this.label1);
            this.tabOptions.Controls.Add(this.BCenter);
            this.tabOptions.Controls.Add(this.CBStyle);
            this.tabOptions.Location = new Point(4, 0x16);
            this.tabOptions.Name = "tabOptions";
            this.tabOptions.Size = new Size(0x150, 0x9c);
            this.tabOptions.TabIndex = 0;
            this.tabOptions.Text = "Options";
            this.BPAxis.FlatStyle = FlatStyle.Flat;
            this.BPAxis.Location = new Point(0x18, 0x30);
            this.BPAxis.Name = "BPAxis";
            this.BPAxis.TabIndex = 13;
            this.BPAxis.Text = "Axis...";
            this.BPLine.FlatStyle = FlatStyle.Flat;
            this.BPLine.Location = new Point(0x18, 0x10);
            this.BPLine.Name = "BPLine";
            this.BPLine.TabIndex = 12;
            this.BPLine.Text = "Line...";
            this.BEndPoint.FlatStyle = FlatStyle.Flat;
            this.BEndPoint.Location = new Point(0xa8, 0x65);
            this.BEndPoint.Name = "BEndPoint";
            this.BEndPoint.TabIndex = 10;
            this.BEndPoint.Text = "End Point...";
            this.BEndPoint.Click += new EventHandler(this.BEndPoint_Click);
            this.UDDistance.Location = new Point(0x58, 0x7e);
            this.UDDistance.Name = "UDDistance";
            this.UDDistance.Size = new Size(0x40, 20);
            this.UDDistance.TabIndex = 9;
            this.UDDistance.ValueChanged += new EventHandler(this.UDDistance_ValueChanged);
            this.UDTotalAngle.Location = new Point(0x58, 0x66);
            int[] bits = new int[4];
            bits[0] = 360;
            this.UDTotalAngle.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 1;
            this.UDTotalAngle.Minimum = new decimal(bits);
            this.UDTotalAngle.Name = "UDTotalAngle";
            this.UDTotalAngle.Size = new Size(0x40, 20);
            this.UDTotalAngle.TabIndex = 8;
            bits = new int[4];
            bits[0] = 1;
            this.UDTotalAngle.Value = new decimal(bits);
            this.UDTotalAngle.ValueChanged += new EventHandler(this.UDTotalAngle_ValueChanged);
            this.UDValue.Location = new Point(0x58, 0x4e);
            this.UDValue.Name = "UDValue";
            this.UDValue.Size = new Size(0x40, 20);
            this.UDValue.TabIndex = 7;
            this.UDValue.ValueChanged += new EventHandler(this.UDValue_ValueChanged);
            this.label3.Location = new Point(0x18, 0x80);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x40, 0x10);
            this.label3.TabIndex = 6;
            this.label3.Text = "Distance";
            this.label2.Location = new Point(0x17, 0x67);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x41, 0x11);
            this.label2.TabIndex = 5;
            this.label2.Text = "Total Angle:";
            this.label1.Location = new Point(0x18, 80);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x38, 0x10);
            this.label1.TabIndex = 4;
            this.label1.Text = "Value";
            this.BCenter.FlatStyle = FlatStyle.Flat;
            this.BCenter.Location = new Point(120, 0x30);
            this.BCenter.Name = "BCenter";
            this.BCenter.TabIndex = 3;
            this.BCenter.Text = "Center...";
            this.BCenter.Click += new EventHandler(this.BCenter_Click);
            this.CBStyle.Items.AddRange(new object[] { "Line", "Triangle" });
            this.CBStyle.Location = new Point(120, 0x11);
            this.CBStyle.Name = "CBStyle";
            this.CBStyle.Size = new Size(0x79, 0x15);
            this.CBStyle.TabIndex = 2;
            this.CBStyle.SelectedIndexChanged += new EventHandler(this.CBStyle_SelectedIndexChanged);
            this.tabTicks.Controls.Add(this.numericUpDown4);
            this.tabTicks.Controls.Add(this.numericUpDown3);
            this.tabTicks.Controls.Add(this.numericUpDown2);
            this.tabTicks.Controls.Add(this.numericUpDown1);
            this.tabTicks.Controls.Add(this.UDMin);
            this.tabTicks.Controls.Add(this.UDMax);
            this.tabTicks.Controls.Add(this.label6);
            this.tabTicks.Controls.Add(this.label5);
            this.tabTicks.Controls.Add(this.BPMinor);
            this.tabTicks.Controls.Add(this.BPTicks);
            this.tabTicks.Location = new Point(4, 0x16);
            this.tabTicks.Name = "tabTicks";
            this.tabTicks.Size = new Size(0x150, 0x9c);
            this.tabTicks.TabIndex = 2;
            this.tabTicks.Text = "Ticks";
            this.numericUpDown4.Location = new Point(0xd0, 50);
            this.numericUpDown4.Name = "numericUpDown4";
            this.numericUpDown4.Size = new Size(0x40, 20);
            this.numericUpDown4.TabIndex = 9;
            this.numericUpDown4.ValueChanged += new EventHandler(this.numericUpDown4_ValueChanged);
            this.numericUpDown3.Location = new Point(0x80, 0x31);
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new Size(0x40, 20);
            this.numericUpDown3.TabIndex = 8;
            this.numericUpDown3.ValueChanged += new EventHandler(this.numericUpDown3_ValueChanged);
            this.numericUpDown2.Location = new Point(0xd0, 0x12);
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new Size(0x40, 20);
            this.numericUpDown2.TabIndex = 7;
            this.numericUpDown2.ValueChanged += new EventHandler(this.numericUpDown2_ValueChanged);
            this.numericUpDown1.Location = new Point(0x80, 0x12);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new Size(0x40, 20);
            this.numericUpDown1.TabIndex = 6;
            this.numericUpDown1.ValueChanged += new EventHandler(this.numericUpDown1_ValueChanged);
            this.UDMin.Location = new Point(0x4f, 0x6f);
            bits = new int[4];
            bits[0] = 0x3e8;
            this.UDMin.Maximum = new decimal(bits);
            this.UDMin.Name = "UDMin";
            this.UDMin.Size = new Size(0x40, 20);
            this.UDMin.TabIndex = 5;
            this.UDMin.ValueChanged += new EventHandler(this.UDMin_ValueChanged);
            this.UDMax.Location = new Point(0x4f, 0x57);
            bits = new int[4];
            bits[0] = 0x3e8;
            this.UDMax.Maximum = new decimal(bits);
            this.UDMax.Name = "UDMax";
            this.UDMax.Size = new Size(0x40, 20);
            this.UDMax.TabIndex = 4;
            this.UDMax.ValueChanged += new EventHandler(this.UDMax_ValueChanged);
            this.label6.Location = new Point(0x18, 0x70);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x38, 0x18);
            this.label6.TabIndex = 3;
            this.label6.Text = "Minimum:";
            this.label5.Location = new Point(0x18, 0x58);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x38, 0x17);
            this.label5.TabIndex = 2;
            this.label5.Text = "Maximum:";
            this.BPMinor.FlatStyle = FlatStyle.Flat;
            this.BPMinor.Location = new Point(0x18, 0x2d);
            this.BPMinor.Name = "BPMinor";
            this.BPMinor.TabIndex = 1;
            this.BPMinor.Text = "Minor";
            this.BPMinor.Click += new EventHandler(this.BPMinor_Click);
            this.BPTicks.FlatStyle = FlatStyle.Flat;
            this.BPTicks.Location = new Point(0x18, 0x10);
            this.BPTicks.Name = "BPTicks";
            this.BPTicks.TabIndex = 0;
            this.BPTicks.Text = "Ticks";
            this.tabLabels.Controls.Add(this.textBox1);
            this.tabLabels.Controls.Add(this.label4);
            this.tabLabels.Controls.Add(this.BFont);
            this.tabLabels.Controls.Add(this.CBInside);
            this.tabLabels.Controls.Add(this.CBLabels);
            this.tabLabels.Location = new Point(4, 0x16);
            this.tabLabels.Name = "tabLabels";
            this.tabLabels.Size = new Size(0x150, 0x9c);
            this.tabLabels.TabIndex = 1;
            this.tabLabels.Text = "Labels";
            this.textBox1.Location = new Point(0x20, 0x72);
            this.textBox1.Name = "textBox1";
            this.textBox1.TabIndex = 9;
            this.textBox1.Text = "";
            this.textBox1.TextChanged += new EventHandler(this.textBox1_TextChanged);
            this.label4.Location = new Point(0x18, 0x62);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x68, 0x10);
            this.label4.TabIndex = 8;
            this.label4.Text = "Format:";
            this.BFont.FlatStyle = FlatStyle.Flat;
            this.BFont.Location = new Point(0x18, 0x42);
            this.BFont.Name = "BFont";
            this.BFont.TabIndex = 7;
            this.BFont.Text = "Font...";
            this.BFont.Click += new EventHandler(this.BFont_Click);
            this.CBInside.FlatStyle = FlatStyle.Flat;
            this.CBInside.Location = new Point(0x18, 0x29);
            this.CBInside.Name = "CBInside";
            this.CBInside.Size = new Size(0x68, 0x10);
            this.CBInside.TabIndex = 6;
            this.CBInside.Text = "Inside";
            this.CBInside.Click += new EventHandler(this.CBInside_Click);
            this.CBLabels.FlatStyle = FlatStyle.Flat;
            this.CBLabels.Location = new Point(0x18, 0x12);
            this.CBLabels.Name = "CBLabels";
            this.CBLabels.TabIndex = 5;
            this.CBLabels.Text = "Show Labels";
            this.CBLabels.Click += new EventHandler(this.CBLabels_Click);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x158, 0xb6);
            base.Controls.Add(this.tabControl1);
            base.Name = "GaugesSeries";
            this.Text = "GaugesEditor";
            this.tabControl1.ResumeLayout(false);
            this.tabOptions.ResumeLayout(false);
            this.UDDistance.EndInit();
            this.UDTotalAngle.EndInit();
            this.UDValue.EndInit();
            this.tabTicks.ResumeLayout(false);
            this.numericUpDown4.EndInit();
            this.numericUpDown3.EndInit();
            this.numericUpDown2.EndInit();
            this.numericUpDown1.EndInit();
            this.UDMin.EndInit();
            this.UDMax.EndInit();
            this.tabLabels.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            this.series.GetVertAxis.Ticks.Length = (int) this.numericUpDown1.Value;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            this.series.MinorTickDistance = (int) this.numericUpDown2.Value;
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            this.series.GetVertAxis.MinorTickCount = (int) this.numericUpDown3.Value;
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            this.series.GetVertAxis.MinorTicks.Length = (int) this.numericUpDown4.Value;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.series.GetVertAxis.Labels.ValueFormat = this.textBox1.Text;
        }

        private void UDDistance_ValueChanged(object sender, EventArgs e)
        {
            this.series.HandDistance = (int) this.UDDistance.Value;
        }

        private void UDMax_ValueChanged(object sender, EventArgs e)
        {
            this.series.Maximum = (double) this.UDMax.Value;
        }

        private void UDMin_ValueChanged(object sender, EventArgs e)
        {
            this.series.Minimum = (double) this.UDMin.Value;
        }

        private void UDTotalAngle_ValueChanged(object sender, EventArgs e)
        {
            this.series.TotalAngle = (double) this.UDTotalAngle.Value;
        }

        private void UDValue_ValueChanged(object sender, EventArgs e)
        {
            this.series.Value = (double) this.UDValue.Value;
        }
    }
}

