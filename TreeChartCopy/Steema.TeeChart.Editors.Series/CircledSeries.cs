namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class CircledSeries : Form
    {
        private ButtonColor BBack;
        private Button BBGrad;
        private CheckBox CB3D;
        private CheckBox CBAutoXR;
        private CheckBox CBAutoYR;
        private CheckBox CBCircled;
        private Circular circled;
        private Container components;
        private GroupBox groupBox1;
        private Label label1;
        private Label label2;
        private Label label3;
        private NumericUpDown UDRot;
        private NumericUpDown UDX;
        private NumericUpDown UDY;

        public CircledSeries()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public CircledSeries(Circular s, Control parent) : this()
        {
            this.circled = s;
        }

        private void BBack_Click(object sender, EventArgs e)
        {
            this.circled.CircleBackColor = this.BBack.Color;
        }

        private void BBGrad_Click(object sender, EventArgs e)
        {
            GradientEditor.Edit(this.circled.CircleGradient);
        }

        private void CB3D_CheckedChanged(object sender, EventArgs e)
        {
            this.circled.chart.aspect.View3D = this.CB3D.Checked;
        }

        private void CBAutoXR_CheckedChanged(object sender, EventArgs e)
        {
            this.circled.CustomXRadius = 0;
        }

        private void CBAutoYR_CheckedChanged(object sender, EventArgs e)
        {
            this.circled.CustomYRadius = 0;
        }

        private void CBCircled_CheckedChanged(object sender, EventArgs e)
        {
            this.circled.Circled = this.CBCircled.Checked;
        }

        private void CircledSeries_Load(object sender, EventArgs e)
        {
            if (this.circled != null)
            {
                this.CB3D.Checked = this.circled.chart.Aspect.View3D;
                this.CBCircled.Checked = this.circled.Circled;
                this.UDRot.Value = this.circled.RotationAngle;
                this.UDX.Value = this.circled.CustomXRadius;
                this.UDY.Value = this.circled.CustomYRadius;
                this.BBack.Color = this.circled.CircleBackColor;
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
            this.CBCircled = new CheckBox();
            this.CB3D = new CheckBox();
            this.BBack = new ButtonColor();
            this.label1 = new Label();
            this.groupBox1 = new GroupBox();
            this.UDY = new NumericUpDown();
            this.UDX = new NumericUpDown();
            this.CBAutoYR = new CheckBox();
            this.CBAutoXR = new CheckBox();
            this.label3 = new Label();
            this.label2 = new Label();
            this.UDRot = new NumericUpDown();
            this.BBGrad = new Button();
            this.groupBox1.SuspendLayout();
            this.UDY.BeginInit();
            this.UDX.BeginInit();
            this.UDRot.BeginInit();
            base.SuspendLayout();
            this.CBCircled.FlatStyle = FlatStyle.Flat;
            this.CBCircled.Location = new Point(10, 8);
            this.CBCircled.Name = "CBCircled";
            this.CBCircled.Size = new Size(0x3e, 0x10);
            this.CBCircled.TabIndex = 0;
            this.CBCircled.Text = "&Circled";
            this.CBCircled.CheckedChanged += new EventHandler(this.CBCircled_CheckedChanged);
            this.CB3D.FlatStyle = FlatStyle.Flat;
            this.CB3D.Location = new Point(10, 30);
            this.CB3D.Name = "CB3D";
            this.CB3D.Size = new Size(0x5e, 0x10);
            this.CB3D.TabIndex = 3;
            this.CB3D.Text = "&3 Dimensions";
            this.CB3D.CheckedChanged += new EventHandler(this.CB3D_CheckedChanged);
            this.BBack.Color = Color.Empty;
            this.BBack.Location = new Point(0x1b, 0x38);
            this.BBack.Name = "BBack";
            this.BBack.TabIndex = 4;
            this.BBack.Text = "C&olor...";
            this.BBack.Click += new EventHandler(this.BBack_Click);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x70, 7);
            this.label1.Name = "label1";
            this.label1.Size = new Size(50, 0x10);
            this.label1.TabIndex = 1;
            this.label1.Text = "&Rotation:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            this.groupBox1.Controls.Add(this.UDY);
            this.groupBox1.Controls.Add(this.UDX);
            this.groupBox1.Controls.Add(this.CBAutoYR);
            this.groupBox1.Controls.Add(this.CBAutoXR);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new Point(6, 100);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0xd0, 0x4e);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Radius:";
            this.UDY.BorderStyle = BorderStyle.FixedSingle;
            int[] bits = new int[4];
            bits[0] = 5;
            this.UDY.Increment = new decimal(bits);
            this.UDY.Location = new Point(0x4a, 0x2d);
            bits = new int[4];
            bits[0] = 0x7d0;
            this.UDY.Maximum = new decimal(bits);
            this.UDY.Name = "UDY";
            this.UDY.Size = new Size(50, 20);
            this.UDY.TabIndex = 3;
            this.UDY.TextAlign = HorizontalAlignment.Right;
            this.UDY.TextChanged += new EventHandler(this.UDY_TextChanged);
            this.UDY.ValueChanged += new EventHandler(this.UDY_ValueChanged);
            this.UDX.BorderStyle = BorderStyle.FixedSingle;
            bits = new int[4];
            bits[0] = 5;
            this.UDX.Increment = new decimal(bits);
            this.UDX.Location = new Point(0x4a, 0x12);
            bits = new int[4];
            bits[0] = 0x7d0;
            this.UDX.Maximum = new decimal(bits);
            this.UDX.Name = "UDX";
            this.UDX.Size = new Size(50, 20);
            this.UDX.TabIndex = 1;
            this.UDX.TextAlign = HorizontalAlignment.Right;
            this.UDX.TextChanged += new EventHandler(this.UDX_TextChanged);
            this.UDX.ValueChanged += new EventHandler(this.UDX_ValueChanged);
            this.CBAutoYR.FlatStyle = FlatStyle.Flat;
            this.CBAutoYR.Location = new Point(0x88, 0x2f);
            this.CBAutoYR.Name = "CBAutoYR";
            this.CBAutoYR.Size = new Size(0x40, 0x10);
            this.CBAutoYR.TabIndex = 5;
            this.CBAutoYR.Text = "A&uto";
            this.CBAutoYR.CheckedChanged += new EventHandler(this.CBAutoYR_CheckedChanged);
            this.CBAutoXR.FlatStyle = FlatStyle.Flat;
            this.CBAutoXR.Location = new Point(0x88, 20);
            this.CBAutoXR.Name = "CBAutoXR";
            this.CBAutoXR.Size = new Size(0x40, 0x10);
            this.CBAutoXR.TabIndex = 4;
            this.CBAutoXR.Text = "&Auto";
            this.CBAutoXR.CheckedChanged += new EventHandler(this.CBAutoXR_CheckedChanged);
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0x1d, 0x2f);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x2d, 0x10);
            this.label3.TabIndex = 2;
            this.label3.Text = "&Vertical:";
            this.label3.TextAlign = ContentAlignment.TopRight;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x10, 20);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x3a, 0x10);
            this.label2.TabIndex = 0;
            this.label2.Text = "&Horizontal:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.UDRot.BorderStyle = BorderStyle.FixedSingle;
            bits = new int[4];
            bits[0] = 5;
            this.UDRot.Increment = new decimal(bits);
            this.UDRot.Location = new Point(0xa1, 5);
            bits = new int[4];
            bits[0] = 360;
            this.UDRot.Maximum = new decimal(bits);
            this.UDRot.Name = "UDRot";
            this.UDRot.Size = new Size(0x38, 20);
            this.UDRot.TabIndex = 2;
            this.UDRot.TextAlign = HorizontalAlignment.Right;
            this.UDRot.TextChanged += new EventHandler(this.UDRot_TextChanged);
            this.UDRot.ValueChanged += new EventHandler(this.UDRot_ValueChanged);
            this.BBGrad.FlatStyle = FlatStyle.Flat;
            this.BBGrad.Location = new Point(0x77, 0x39);
            this.BBGrad.Name = "BBGrad";
            this.BBGrad.TabIndex = 6;
            this.BBGrad.Text = "Gradient...";
            this.BBGrad.Click += new EventHandler(this.BBGrad_Click);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0xd9, 0xb6);
            base.Controls.Add(this.BBGrad);
            base.Controls.Add(this.UDRot);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.BBack);
            base.Controls.Add(this.CB3D);
            base.Controls.Add(this.CBCircled);
            base.Name = "CircledSeries";
            base.Load += new EventHandler(this.CircledSeries_Load);
            this.groupBox1.ResumeLayout(false);
            this.UDY.EndInit();
            this.UDX.EndInit();
            this.UDRot.EndInit();
            base.ResumeLayout(false);
        }

        public static CircledSeries InsertForm(Control parent, Circular s)
        {
            TabControl control = (TabControl) ((TabPage) parent).Parent;
            TabPage page = new TabPage(Texts.GalleryCircled);
            control.TabPages.Add(page);
            CircledSeries f = new CircledSeries(s, page);
            EditorUtils.InsertForm(f, page);
            TabPage page2 = control.TabPages[1];
            control.TabPages[1] = page;
            control.TabPages[control.TabCount - 1] = page2;
            return f;
        }

        private void UDRot_TextChanged(object sender, EventArgs e)
        {
            this.UDRot_ValueChanged(sender, e);
        }

        private void UDRot_ValueChanged(object sender, EventArgs e)
        {
            if (this.UDRot.Value == 360M)
            {
                this.UDRot.Value = 0M;
            }
            this.circled.RotationAngle = (int) this.UDRot.Value;
        }

        private void UDX_TextChanged(object sender, EventArgs e)
        {
            this.UDX_ValueChanged(sender, e);
        }

        private void UDX_ValueChanged(object sender, EventArgs e)
        {
            this.circled.CustomXRadius = (int) this.UDX.Value;
        }

        private void UDY_TextChanged(object sender, EventArgs e)
        {
            this.UDY_ValueChanged(sender, e);
        }

        private void UDY_ValueChanged(object sender, EventArgs e)
        {
            this.circled.CustomYRadius = (int) this.UDY.Value;
        }
    }
}

