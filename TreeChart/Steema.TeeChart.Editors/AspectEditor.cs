namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    public class AspectEditor : Form
    {
        private Steema.TeeChart.Drawing.Aspect Aspect;
        private CheckBox CBClipPoints;
        private ComboBox cbQuality;
        private CheckBox checkBox1;
        private CheckBox checkBox7;
        private CheckBox checkBox8;
        private Container components;
        private Label label11;
        private Label label13;
        private Label label2;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label9;
        private Label labelElevation;
        private Label labelHoriz;
        private Label labelPerspec;
        private Label labelRotation;
        private Label labelVert;
        private Label labelZoom;
        private NumericUpDown numericUpDown2;
        private TrackBar trackBar1;
        private TrackBar trackBar2;
        private TrackBar trackBar3;
        private TrackBar trackBar4;
        private TrackBar trackBar5;
        private TrackBar trackBar6;

        public AspectEditor()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public AspectEditor(Chart c, Control parent) : this()
        {
            this.Aspect = c.Aspect;
            this.checkBox1.Checked = this.Aspect.View3D;
            this.checkBox7.Checked = this.Aspect.Orthogonal;
            this.checkBox8.Checked = this.Aspect.ZoomText;
            this.trackBar1.Value = this.Aspect.Zoom;
            this.trackBar2.Value = this.Aspect.Rotation;
            this.trackBar3.Value = this.Aspect.Elevation;
            this.numericUpDown2.Value = this.Aspect.Chart3DPercent;
            this.trackBar2.Enabled = !this.Aspect.Orthogonal;
            this.trackBar3.Enabled = this.trackBar2.Enabled;
            this.trackBar5.Value = this.Aspect.HorizOffset;
            this.trackBar4.Value = this.Aspect.VertOffset;
            this.trackBar6.Value = this.Aspect.Perspective;
            this.labelRotation.Text = this.trackBar2.Value.ToString() + "%";
            this.labelElevation.Text = this.trackBar3.Value.ToString() + "%";
            this.labelZoom.Text = this.trackBar1.Value.ToString() + "%";
            this.labelHoriz.Text = this.Aspect.HorizOffset.ToString();
            this.labelVert.Text = this.Aspect.VertOffset.ToString();
            this.labelPerspec.Text = this.Aspect.Perspective.ToString() + "%";
            if (c.Graphics3D.SmoothingMode == SmoothingMode.HighSpeed)
            {
                this.cbQuality.SelectedIndex = 0;
            }
            else if (c.Graphics3D.SmoothingMode == SmoothingMode.HighQuality)
            {
                this.cbQuality.SelectedIndex = 1;
            }
            else
            {
                this.cbQuality.SelectedIndex = 2;
            }
            this.CBClipPoints.Checked = this.Aspect.ClipPoints;
            EditorUtils.InsertForm(this, parent);
        }

        private void CBClipPoints_CheckedChanged(object sender, EventArgs e)
        {
            this.Aspect.ClipPoints = this.CBClipPoints.Checked;
        }

        private void cbQuality_SelectedIndexChanged(object sender, EventArgs e)
        {
            Graphics3D graphicsd = this.Aspect.chart.graphics3D;
            switch (this.cbQuality.SelectedIndex)
            {
                case 0:
                    graphicsd.SmoothingMode = SmoothingMode.HighSpeed;
                    return;

                case 1:
                    graphicsd.SmoothingMode = SmoothingMode.HighQuality;
                    return;

                case 2:
                    graphicsd.SmoothingMode = SmoothingMode.AntiAlias;
                    return;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.Aspect.View3D = this.checkBox1.Checked;
            this.EnableControls();
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            this.Aspect.Orthogonal = this.checkBox7.Checked;
            this.trackBar2.Enabled = !this.Aspect.Orthogonal;
            this.trackBar3.Enabled = this.trackBar2.Enabled;
            this.trackBar6.Enabled = this.trackBar2.Enabled;
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            this.Aspect.ZoomText = this.checkBox8.Checked;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void EnableControls()
        {
            this.trackBar1.Enabled = this.checkBox1.Checked;
            if (this.checkBox1.Checked)
            {
                this.trackBar2.Enabled = !this.Aspect.Orthogonal;
                this.trackBar3.Enabled = this.trackBar2.Enabled;
                this.trackBar6.Enabled = this.trackBar2.Enabled;
            }
            else
            {
                this.trackBar2.Enabled = false;
                this.trackBar3.Enabled = this.trackBar2.Enabled;
                this.trackBar6.Enabled = this.trackBar2.Enabled;
            }
            this.trackBar4.Enabled = this.checkBox1.Checked;
            this.trackBar5.Enabled = this.checkBox1.Checked;
        }

        private void InitializeComponent()
        {
            this.labelElevation = new Label();
            this.trackBar3 = new TrackBar();
            this.label11 = new Label();
            this.labelRotation = new Label();
            this.trackBar2 = new TrackBar();
            this.label9 = new Label();
            this.labelZoom = new Label();
            this.trackBar1 = new TrackBar();
            this.label5 = new Label();
            this.checkBox8 = new CheckBox();
            this.checkBox7 = new CheckBox();
            this.numericUpDown2 = new NumericUpDown();
            this.label4 = new Label();
            this.checkBox1 = new CheckBox();
            this.labelVert = new Label();
            this.trackBar4 = new TrackBar();
            this.label2 = new Label();
            this.labelHoriz = new Label();
            this.trackBar5 = new TrackBar();
            this.label7 = new Label();
            this.labelPerspec = new Label();
            this.trackBar6 = new TrackBar();
            this.label13 = new Label();
            this.cbQuality = new ComboBox();
            this.label6 = new Label();
            this.CBClipPoints = new CheckBox();
            this.trackBar3.BeginInit();
            this.trackBar2.BeginInit();
            this.trackBar1.BeginInit();
            this.numericUpDown2.BeginInit();
            this.trackBar4.BeginInit();
            this.trackBar5.BeginInit();
            this.trackBar6.BeginInit();
            base.SuspendLayout();
            this.labelElevation.AutoSize = true;
            this.labelElevation.Location = new Point(0x148, 0x4c);
            this.labelElevation.Name = "labelElevation";
            this.labelElevation.Size = new Size(0x21, 0x10);
            this.labelElevation.TabIndex = 0x10;
            this.labelElevation.Text = "345%";
            this.labelElevation.UseMnemonic = false;
            this.trackBar3.AutoSize = false;
            this.trackBar3.Location = new Point(0xb8, 0x4c);
            this.trackBar3.Maximum = 360;
            this.trackBar3.Minimum = 270;
            this.trackBar3.Name = "trackBar3";
            this.trackBar3.Size = new Size(140, 20);
            this.trackBar3.TabIndex = 15;
            this.trackBar3.TickFrequency = 5;
            this.trackBar3.Value = 0x159;
            this.trackBar3.Scroll += new EventHandler(this.trackBar3_Scroll);
            this.label11.AutoSize = true;
            this.label11.Location = new Point(0x87, 0x4c);
            this.label11.Name = "label11";
            this.label11.Size = new Size(0x36, 0x10);
            this.label11.TabIndex = 14;
            this.label11.Text = "&Elevation:";
            this.label11.TextAlign = ContentAlignment.TopRight;
            this.labelRotation.AutoSize = true;
            this.labelRotation.Location = new Point(0x148, 0x2c);
            this.labelRotation.Name = "labelRotation";
            this.labelRotation.Size = new Size(0x21, 0x10);
            this.labelRotation.TabIndex = 13;
            this.labelRotation.Text = "345%";
            this.labelRotation.UseMnemonic = false;
            this.trackBar2.AutoSize = false;
            this.trackBar2.Location = new Point(0xb8, 0x2c);
            this.trackBar2.Maximum = 360;
            this.trackBar2.Minimum = 270;
            this.trackBar2.Name = "trackBar2";
            this.trackBar2.Size = new Size(140, 20);
            this.trackBar2.TabIndex = 12;
            this.trackBar2.TickFrequency = 5;
            this.trackBar2.Value = 0x159;
            this.trackBar2.Scroll += new EventHandler(this.trackBar2_Scroll);
            this.label9.AutoSize = true;
            this.label9.Location = new Point(0x8b, 0x2c);
            this.label9.Name = "label9";
            this.label9.Size = new Size(50, 0x10);
            this.label9.TabIndex = 11;
            this.label9.Text = "Rot&ation:";
            this.label9.TextAlign = ContentAlignment.TopRight;
            this.labelZoom.AutoSize = true;
            this.labelZoom.Location = new Point(0x148, 12);
            this.labelZoom.Name = "labelZoom";
            this.labelZoom.Size = new Size(0x21, 0x10);
            this.labelZoom.TabIndex = 10;
            this.labelZoom.Text = "100%";
            this.labelZoom.UseMnemonic = false;
            this.trackBar1.AutoSize = false;
            this.trackBar1.LargeChange = 50;
            this.trackBar1.Location = new Point(0xb8, 12);
            this.trackBar1.Maximum = 0x3e8;
            this.trackBar1.Minimum = 5;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new Size(140, 20);
            this.trackBar1.TabIndex = 9;
            this.trackBar1.TickFrequency = 50;
            this.trackBar1.Value = 100;
            this.trackBar1.Scroll += new EventHandler(this.trackBar1_Scroll);
            this.label5.AutoSize = true;
            this.label5.Location = new Point(0x99, 12);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x24, 0x10);
            this.label5.TabIndex = 8;
            this.label5.Text = "&Zoom:";
            this.label5.TextAlign = ContentAlignment.TopRight;
            this.checkBox8.Checked = true;
            this.checkBox8.CheckState = CheckState.Checked;
            this.checkBox8.FlatStyle = FlatStyle.Flat;
            this.checkBox8.Location = new Point(8, 0x71);
            this.checkBox8.Name = "checkBox8";
            this.checkBox8.Size = new Size(0x68, 0x13);
            this.checkBox8.TabIndex = 4;
            this.checkBox8.Text = "Z&oom Text";
            this.checkBox8.CheckedChanged += new EventHandler(this.checkBox8_CheckedChanged);
            this.checkBox7.Checked = true;
            this.checkBox7.CheckState = CheckState.Checked;
            this.checkBox7.FlatStyle = FlatStyle.Flat;
            this.checkBox7.Location = new Point(8, 0x49);
            this.checkBox7.Name = "checkBox7";
            this.checkBox7.TabIndex = 3;
            this.checkBox7.Text = "O&rthogonal";
            this.checkBox7.CheckedChanged += new EventHandler(this.checkBox7_CheckedChanged);
            this.numericUpDown2.BorderStyle = BorderStyle.FixedSingle;
            int[] bits = new int[4];
            bits[0] = 5;
            this.numericUpDown2.Increment = new decimal(bits);
            this.numericUpDown2.Location = new Point(0x3a, 0x25);
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new Size(0x31, 20);
            this.numericUpDown2.TabIndex = 2;
            this.numericUpDown2.TextAlign = HorizontalAlignment.Right;
            this.numericUpDown2.TextChanged += new EventHandler(this.numericUpDown2_ValueChanged);
            this.numericUpDown2.ValueChanged += new EventHandler(this.numericUpDown2_ValueChanged);
            this.label4.AutoSize = true;
            this.label4.FlatStyle = FlatStyle.Flat;
            this.label4.Location = new Point(20, 0x27);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x26, 0x10);
            this.label4.TabIndex = 1;
            this.label4.Text = "&3D % :";
            this.label4.TextAlign = ContentAlignment.TopRight;
            this.checkBox1.FlatStyle = FlatStyle.Flat;
            this.checkBox1.Location = new Point(8, 8);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new Size(0x70, 0x17);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "3 &Dimensions";
            this.checkBox1.CheckedChanged += new EventHandler(this.checkBox1_CheckedChanged);
            this.labelVert.AutoSize = true;
            this.labelVert.Location = new Point(0x148, 0x97);
            this.labelVert.Name = "labelVert";
            this.labelVert.Size = new Size(10, 0x10);
            this.labelVert.TabIndex = 0x16;
            this.labelVert.Text = "0";
            this.labelVert.UseMnemonic = false;
            this.trackBar4.AutoSize = false;
            this.trackBar4.LargeChange = 1;
            this.trackBar4.Location = new Point(0xb8, 0x93);
            this.trackBar4.Maximum = 0x5dc;
            this.trackBar4.Minimum = -1500;
            this.trackBar4.Name = "trackBar4";
            this.trackBar4.Size = new Size(140, 20);
            this.trackBar4.TabIndex = 0x15;
            this.trackBar4.TickFrequency = 150;
            this.trackBar4.Scroll += new EventHandler(this.trackBar4_Scroll);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x7d, 0x93);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x40, 0x10);
            this.label2.TabIndex = 20;
            this.label2.Text = "&Vert. Offset:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.labelHoriz.AutoSize = true;
            this.labelHoriz.Location = new Point(0x148, 0x77);
            this.labelHoriz.Name = "labelHoriz";
            this.labelHoriz.Size = new Size(10, 0x10);
            this.labelHoriz.TabIndex = 0x13;
            this.labelHoriz.Text = "0";
            this.labelHoriz.UseMnemonic = false;
            this.trackBar5.AutoSize = false;
            this.trackBar5.LargeChange = 1;
            this.trackBar5.Location = new Point(0xb8, 0x73);
            this.trackBar5.Maximum = 0x5dc;
            this.trackBar5.Minimum = -1500;
            this.trackBar5.Name = "trackBar5";
            this.trackBar5.Size = new Size(140, 20);
            this.trackBar5.TabIndex = 0x12;
            this.trackBar5.TickFrequency = 150;
            this.trackBar5.Scroll += new EventHandler(this.trackBar5_Scroll);
            this.label7.AutoSize = true;
            this.label7.Location = new Point(0x77, 0x73);
            this.label7.Name = "label7";
            this.label7.Size = new Size(70, 0x10);
            this.label7.TabIndex = 0x11;
            this.label7.Text = "&Horiz. Offset:";
            this.label7.TextAlign = ContentAlignment.TopRight;
            this.labelPerspec.AutoSize = true;
            this.labelPerspec.Location = new Point(0x148, 0xbf);
            this.labelPerspec.Name = "labelPerspec";
            this.labelPerspec.Size = new Size(10, 0x10);
            this.labelPerspec.TabIndex = 0x19;
            this.labelPerspec.Text = "0";
            this.labelPerspec.UseMnemonic = false;
            this.trackBar6.AutoSize = false;
            this.trackBar6.LargeChange = 10;
            this.trackBar6.Location = new Point(0xb8, 0xbb);
            this.trackBar6.Maximum = 100;
            this.trackBar6.Name = "trackBar6";
            this.trackBar6.Size = new Size(140, 20);
            this.trackBar6.TabIndex = 0x18;
            this.trackBar6.TickFrequency = 5;
            this.trackBar6.Value = 15;
            this.trackBar6.Scroll += new EventHandler(this.trackBar6_Scroll);
            this.label13.AutoSize = true;
            this.label13.Location = new Point(0x7a, 0xbb);
            this.label13.Name = "label13";
            this.label13.Size = new Size(0x43, 0x10);
            this.label13.TabIndex = 0x17;
            this.label13.Text = "&Perspective:";
            this.label13.TextAlign = ContentAlignment.TopRight;
            this.cbQuality.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbQuality.Items.AddRange(new object[] { "Best Speed", "Best Quality", "Anti-Alias" });
            this.cbQuality.Location = new Point(8, 160);
            this.cbQuality.Name = "cbQuality";
            this.cbQuality.Size = new Size(0x58, 0x15);
            this.cbQuality.TabIndex = 6;
            this.cbQuality.SelectedIndexChanged += new EventHandler(this.cbQuality_SelectedIndexChanged);
            this.label6.AutoSize = true;
            this.label6.Location = new Point(8, 0x90);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x2b, 0x10);
            this.label6.TabIndex = 5;
            this.label6.Text = "&Quality:";
            this.CBClipPoints.FlatStyle = FlatStyle.Flat;
            this.CBClipPoints.Location = new Point(8, 0xc0);
            this.CBClipPoints.Name = "CBClipPoints";
            this.CBClipPoints.Size = new Size(0x68, 0x10);
            this.CBClipPoints.TabIndex = 7;
            this.CBClipPoints.Text = "&Clip Points";
            this.CBClipPoints.CheckedChanged += new EventHandler(this.CBClipPoints_CheckedChanged);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x170, 0xe5);
            base.Controls.Add(this.CBClipPoints);
            base.Controls.Add(this.cbQuality);
            base.Controls.Add(this.label6);
            base.Controls.Add(this.labelPerspec);
            base.Controls.Add(this.label13);
            base.Controls.Add(this.labelVert);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.labelHoriz);
            base.Controls.Add(this.label7);
            base.Controls.Add(this.labelElevation);
            base.Controls.Add(this.label11);
            base.Controls.Add(this.labelRotation);
            base.Controls.Add(this.label9);
            base.Controls.Add(this.labelZoom);
            base.Controls.Add(this.label5);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.trackBar6);
            base.Controls.Add(this.trackBar4);
            base.Controls.Add(this.trackBar5);
            base.Controls.Add(this.trackBar3);
            base.Controls.Add(this.trackBar2);
            base.Controls.Add(this.trackBar1);
            base.Controls.Add(this.checkBox8);
            base.Controls.Add(this.checkBox7);
            base.Controls.Add(this.numericUpDown2);
            base.Controls.Add(this.checkBox1);
            base.Name = "AspectEditor";
            this.Text = "AspectEditor";
            this.trackBar3.EndInit();
            this.trackBar2.EndInit();
            this.trackBar1.EndInit();
            this.numericUpDown2.EndInit();
            this.trackBar4.EndInit();
            this.trackBar5.EndInit();
            this.trackBar6.EndInit();
            base.ResumeLayout(false);
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            this.Aspect.Chart3DPercent = (int) this.numericUpDown2.Value;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            this.Aspect.Zoom = this.trackBar1.Value;
            this.labelZoom.Text = this.trackBar1.Value.ToString() + "%";
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            this.Aspect.Rotation = this.trackBar2.Value;
            this.labelRotation.Text = this.trackBar2.Value.ToString() + "%";
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            this.Aspect.Elevation = this.trackBar3.Value;
            this.labelElevation.Text = this.trackBar3.Value.ToString() + "%";
        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            this.Aspect.VertOffset = this.trackBar4.Value;
            this.labelVert.Text = this.Aspect.VertOffset.ToString();
        }

        private void trackBar5_Scroll(object sender, EventArgs e)
        {
            this.Aspect.HorizOffset = this.trackBar5.Value;
            this.labelHoriz.Text = this.Aspect.HorizOffset.ToString();
        }

        private void trackBar6_Scroll(object sender, EventArgs e)
        {
            this.Aspect.Perspective = this.trackBar6.Value;
            this.labelPerspec.Text = this.Aspect.Perspective.ToString() + "%";
        }
    }
}

