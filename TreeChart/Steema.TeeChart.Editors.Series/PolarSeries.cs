namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class PolarSeries : Form
    {
        private Button BBrush;
        private Button BFont;
        private Button BPen;
        private Button BPiePen;
        private ButtonColor button1;
        private CheckBox CBAngleLabels;
        private CheckBox CBClockWise;
        private CheckBox CBClose;
        private CheckBox CBColorEach;
        private CheckBox CBInside;
        private CheckBox CBLabelsRot;
        private CircledSeries circledEditor;
        private Container components;
        private GroupBox groupBox1;
        internal Label label1;
        private Label label2;
        private Label label3;
        private Steema.TeeChart.Editors.SeriesPointer pointerEditor;
        internal CustomPolar polar;
        private NumericUpDown UDAngleInc;
        private NumericUpDown UDRadiusInc;
        private NumericUpDown UDTransp;

        public PolarSeries()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public PolarSeries(Series s) : this()
        {
            if (s is WindRose)
            {
                this.UDAngleInc.Visible = false;
                this.label1.Visible = false;
            }
            this.SetPolar((CustomPolar) s);
        }

        private void BBrush_Click(object sender, EventArgs e)
        {
            BrushEditor.Edit(this.polar.Brush);
        }

        private void BFont_Click(object sender, EventArgs e)
        {
            EditorUtils.EditFont(this.polar.CircleLabelsFont);
        }

        private void BPen_Click(object sender, EventArgs e)
        {
            PenEditor.Edit(this.polar.Pen);
        }

        private void BPiePen_Click(object sender, EventArgs e)
        {
            PenEditor.Edit(this.polar.CirclePen);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.polar.Color = this.button1.Color;
        }

        private void CBAngleLabels_CheckedChanged(object sender, EventArgs e)
        {
            this.polar.CircleLabels = this.CBAngleLabels.Checked;
            this.EnableLabels();
        }

        private void CBClockWise_CheckedChanged(object sender, EventArgs e)
        {
            this.polar.ClockWiseLabels = this.CBClockWise.Checked;
        }

        private void CBClose_CheckedChanged(object sender, EventArgs e)
        {
            this.polar.CloseCircle = this.CBClose.Checked;
        }

        private void CBColorEach_CheckedChanged(object sender, EventArgs e)
        {
            this.polar.ColorEach = this.CBColorEach.Checked;
        }

        private void CBInside_CheckedChanged(object sender, EventArgs e)
        {
            this.polar.CircleLabelsInside = this.CBInside.Checked;
        }

        private void CBLabelsRot_CheckedChanged(object sender, EventArgs e)
        {
            this.polar.CircleLabelsRotated = this.CBLabelsRot.Checked;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void EnableLabels()
        {
            this.CBLabelsRot.Enabled = this.CBAngleLabels.Enabled;
            this.CBClockWise.Enabled = this.CBAngleLabels.Enabled;
            this.BFont.Enabled = this.CBAngleLabels.Enabled;
        }

        private void InitializeComponent()
        {
            this.label1 = new Label();
            this.label2 = new Label();
            this.label3 = new Label();
            this.BPen = new Button();
            this.BBrush = new Button();
            this.BPiePen = new Button();
            this.CBClose = new CheckBox();
            this.UDAngleInc = new NumericUpDown();
            this.UDRadiusInc = new NumericUpDown();
            this.UDTransp = new NumericUpDown();
            this.groupBox1 = new GroupBox();
            this.BFont = new Button();
            this.CBClockWise = new CheckBox();
            this.CBLabelsRot = new CheckBox();
            this.CBInside = new CheckBox();
            this.CBAngleLabels = new CheckBox();
            this.button1 = new ButtonColor();
            this.CBColorEach = new CheckBox();
            this.UDAngleInc.BeginInit();
            this.UDRadiusInc.BeginInit();
            this.UDTransp.BeginInit();
            this.groupBox1.SuspendLayout();
            base.SuspendLayout();
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x17, 0x4f);
            this.label1.Name = "label1";
            this.label1.Size = new Size(90, 0x10);
            this.label1.TabIndex = 4;
            this.label1.Text = "Angle &Increment:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x11, 0x68);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x60, 0x10);
            this.label2.TabIndex = 6;
            this.label2.Text = "&Radius Increment:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0x25, 130);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x4d, 0x10);
            this.label3.TabIndex = 8;
            this.label3.Text = "Transparenc&y:";
            this.label3.TextAlign = ContentAlignment.TopRight;
            this.BPen.FlatStyle = FlatStyle.Flat;
            this.BPen.Location = new Point(8, 9);
            this.BPen.Name = "BPen";
            this.BPen.TabIndex = 0;
            this.BPen.Text = "&Pen...";
            this.BPen.Click += new EventHandler(this.BPen_Click);
            this.BBrush.FlatStyle = FlatStyle.Flat;
            this.BBrush.Location = new Point(8, 40);
            this.BBrush.Name = "BBrush";
            this.BBrush.TabIndex = 2;
            this.BBrush.Text = "Pa&ttern...";
            this.BBrush.Click += new EventHandler(this.BBrush_Click);
            this.BPiePen.FlatStyle = FlatStyle.Flat;
            this.BPiePen.Location = new Point(0x60, 40);
            this.BPiePen.Name = "BPiePen";
            this.BPiePen.TabIndex = 3;
            this.BPiePen.Text = "&Circle...";
            this.BPiePen.Click += new EventHandler(this.BPiePen_Click);
            this.CBClose.Checked = true;
            this.CBClose.CheckState = CheckState.Checked;
            this.CBClose.FlatStyle = FlatStyle.Flat;
            this.CBClose.Location = new Point(0x60, 12);
            this.CBClose.Name = "CBClose";
            this.CBClose.Size = new Size(0x68, 0x11);
            this.CBClose.TabIndex = 1;
            this.CBClose.Text = "C&lose Circle";
            this.CBClose.CheckedChanged += new EventHandler(this.CBClose_CheckedChanged);
            this.UDAngleInc.BorderStyle = BorderStyle.FixedSingle;
            this.UDAngleInc.Location = new Point(0x77, 0x4d);
            int[] bits = new int[4];
            bits[0] = 360;
            this.UDAngleInc.Maximum = new decimal(bits);
            this.UDAngleInc.Name = "UDAngleInc";
            this.UDAngleInc.Size = new Size(0x4e, 20);
            this.UDAngleInc.TabIndex = 5;
            this.UDAngleInc.TextAlign = HorizontalAlignment.Right;
            this.UDAngleInc.ValueChanged += new EventHandler(this.UDAngleInc_ValueChanged);
            this.UDRadiusInc.BorderStyle = BorderStyle.FixedSingle;
            this.UDRadiusInc.Location = new Point(0x77, 0x66);
            bits = new int[4];
            bits[0] = 0x7fff;
            this.UDRadiusInc.Maximum = new decimal(bits);
            this.UDRadiusInc.Name = "UDRadiusInc";
            this.UDRadiusInc.Size = new Size(0x4e, 20);
            this.UDRadiusInc.TabIndex = 7;
            this.UDRadiusInc.TextAlign = HorizontalAlignment.Right;
            this.UDRadiusInc.ValueChanged += new EventHandler(this.UDRadiusInc_ValueChanged);
            this.UDTransp.BorderStyle = BorderStyle.FixedSingle;
            this.UDTransp.Location = new Point(0x77, 0x80);
            this.UDTransp.Name = "UDTransp";
            this.UDTransp.Size = new Size(60, 20);
            this.UDTransp.TabIndex = 9;
            this.UDTransp.TextAlign = HorizontalAlignment.Right;
            this.UDTransp.TextChanged += new EventHandler(this.UDTransp_TextChanged);
            this.UDTransp.ValueChanged += new EventHandler(this.UDTransp_ValueChanged);
            this.groupBox1.Controls.Add(this.BFont);
            this.groupBox1.Controls.Add(this.CBClockWise);
            this.groupBox1.Controls.Add(this.CBLabelsRot);
            this.groupBox1.Controls.Add(this.CBInside);
            this.groupBox1.Controls.Add(this.CBAngleLabels);
            this.groupBox1.Location = new Point(0xca, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0xa8, 0x86);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Labels:";
            this.BFont.FlatStyle = FlatStyle.Flat;
            this.BFont.Location = new Point(0x11, 0x60);
            this.BFont.Name = "BFont";
            this.BFont.TabIndex = 4;
            this.BFont.Text = "&Font...";
            this.BFont.Click += new EventHandler(this.BFont_Click);
            this.CBClockWise.FlatStyle = FlatStyle.Flat;
            this.CBClockWise.Location = new Point(0x11, 0x45);
            this.CBClockWise.Name = "CBClockWise";
            this.CBClockWise.Size = new Size(0x6f, 0x11);
            this.CBClockWise.TabIndex = 3;
            this.CBClockWise.Text = "Clock&Wise";
            this.CBClockWise.CheckedChanged += new EventHandler(this.CBClockWise_CheckedChanged);
            this.CBLabelsRot.FlatStyle = FlatStyle.Flat;
            this.CBLabelsRot.Location = new Point(0x11, 0x2b);
            this.CBLabelsRot.Name = "CBLabelsRot";
            this.CBLabelsRot.Size = new Size(120, 0x11);
            this.CBLabelsRot.TabIndex = 2;
            this.CBLabelsRot.Text = "R&otated";
            this.CBLabelsRot.CheckedChanged += new EventHandler(this.CBLabelsRot_CheckedChanged);
            this.CBInside.FlatStyle = FlatStyle.Flat;
            this.CBInside.Location = new Point(0x55, 0x12);
            this.CBInside.Name = "CBInside";
            this.CBInside.Size = new Size(0x4d, 0x10);
            this.CBInside.TabIndex = 1;
            this.CBInside.Text = "I&nside";
            this.CBInside.CheckedChanged += new EventHandler(this.CBInside_CheckedChanged);
            this.CBAngleLabels.FlatStyle = FlatStyle.Flat;
            this.CBAngleLabels.Location = new Point(0x11, 0x12);
            this.CBAngleLabels.Name = "CBAngleLabels";
            this.CBAngleLabels.Size = new Size(60, 0x10);
            this.CBAngleLabels.TabIndex = 0;
            this.CBAngleLabels.Text = "&Visible";
            this.CBAngleLabels.CheckedChanged += new EventHandler(this.CBAngleLabels_CheckedChanged);
            this.button1.Color = Color.Empty;
            this.button1.Location = new Point(120, 160);
            this.button1.Name = "button1";
            this.button1.TabIndex = 10;
            this.button1.Text = "&Color...";
            this.button1.Click += new EventHandler(this.button1_Click);
            this.CBColorEach.FlatStyle = FlatStyle.Flat;
            this.CBColorEach.Location = new Point(0xcb, 0x90);
            this.CBColorEach.Name = "CBColorEach";
            this.CBColorEach.Size = new Size(120, 0x18);
            this.CBColorEach.TabIndex = 12;
            this.CBColorEach.Text = "Color &Each";
            this.CBColorEach.CheckedChanged += new EventHandler(this.CBColorEach_CheckedChanged);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x178, 0xc9);
            base.Controls.Add(this.CBColorEach);
            base.Controls.Add(this.button1);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.UDTransp);
            base.Controls.Add(this.UDRadiusInc);
            base.Controls.Add(this.UDAngleInc);
            base.Controls.Add(this.CBClose);
            base.Controls.Add(this.BPiePen);
            base.Controls.Add(this.BBrush);
            base.Controls.Add(this.BPen);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.Name = "PolarSeries";
            base.Load += new EventHandler(this.PolarSeries_Load);
            this.UDAngleInc.EndInit();
            this.UDRadiusInc.EndInit();
            this.UDTransp.EndInit();
            this.groupBox1.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void PolarSeries_Load(object sender, EventArgs e)
        {
            if (this.polar != null)
            {
                if (this.pointerEditor == null)
                {
                    this.pointerEditor = Steema.TeeChart.Editors.SeriesPointer.InsertPointer(base.Parent, this.polar.Pointer);
                }
                if (this.circledEditor == null)
                {
                    this.circledEditor = CircledSeries.InsertForm(base.Parent, this.polar);
                }
            }
        }

        protected void SetPolar(CustomPolar s)
        {
            this.polar = s;
            this.CBAngleLabels.Checked = this.polar.CircleLabels;
            this.CBClockWise.Checked = this.polar.ClockWiseLabels;
            this.CBClose.Checked = this.polar.CloseCircle;
            this.CBColorEach.Checked = this.polar.ColorEach;
            this.CBInside.Checked = this.polar.CircleLabelsInside;
            this.CBLabelsRot.Checked = this.polar.CircleLabelsRotated;
            this.UDTransp.Value = this.polar.Transparency;
            this.button1.Color = this.polar.Color;
        }

        private void UDAngleInc_ValueChanged(object sender, EventArgs e)
        {
            this.polar.AngleIncrement = (int) this.UDAngleInc.Value;
        }

        private void UDRadiusInc_ValueChanged(object sender, EventArgs e)
        {
            this.polar.RadiusIncrement = (int) this.UDRadiusInc.Value;
        }

        private void UDTransp_TextChanged(object sender, EventArgs e)
        {
            this.UDTransp_ValueChanged(sender, e);
        }

        private void UDTransp_ValueChanged(object sender, EventArgs e)
        {
            this.polar.Transparency = (int) this.UDTransp.Value;
        }
    }
}

