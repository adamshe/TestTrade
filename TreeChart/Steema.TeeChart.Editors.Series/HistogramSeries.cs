namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class HistogramSeries : Form
    {
        private Button BAreaColor;
        private Button BAreaLinePen;
        private Button BAreaLinesPen;
        private Button Button1;
        private CheckBox CBColorEach;
        private Container components;
        private GroupBox groupBox1;
        private Label label1;
        private Histogram series;
        private NumericUpDown UDTransp;

        public HistogramSeries()
        {
            this.components = null;
            this.InitializeComponent();
            EditorUtils.Translate(this);
        }

        public HistogramSeries(Series s) : this()
        {
            this.series = (Histogram) s;
            this.UDTransp.Value = this.series.Transparency;
            this.CBColorEach.Checked = this.series.ColorEach;
        }

        private void BAreaLinePen_Click(object sender, EventArgs e)
        {
            PenEditor.Edit(this.series.LinePen);
        }

        private void BAreaLinesPen_Click(object sender, EventArgs e)
        {
            PenEditor.Edit(this.series.LinesPen);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            BrushEditor.Edit(this.series.Brush);
        }

        private void CBColorEach_CheckedChanged(object sender, EventArgs e)
        {
            this.series.ColorEach = this.CBColorEach.Checked;
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
            this.BAreaLinePen = new Button();
            this.BAreaLinesPen = new Button();
            this.Button1 = new Button();
            this.groupBox1 = new GroupBox();
            this.BAreaColor = new Button();
            this.CBColorEach = new CheckBox();
            this.label1 = new Label();
            this.UDTransp = new NumericUpDown();
            this.groupBox1.SuspendLayout();
            this.UDTransp.BeginInit();
            base.SuspendLayout();
            this.BAreaLinePen.FlatStyle = FlatStyle.Flat;
            this.BAreaLinePen.Location = new Point(0x10, 11);
            this.BAreaLinePen.Name = "BAreaLinePen";
            this.BAreaLinePen.TabIndex = 0;
            this.BAreaLinePen.Text = "&Border...";
            this.BAreaLinePen.Click += new EventHandler(this.BAreaLinePen_Click);
            this.BAreaLinesPen.FlatStyle = FlatStyle.Flat;
            this.BAreaLinesPen.Location = new Point(0x10, 0x29);
            this.BAreaLinesPen.Name = "BAreaLinesPen";
            this.BAreaLinesPen.TabIndex = 1;
            this.BAreaLinesPen.Text = "&Lines...";
            this.BAreaLinesPen.Click += new EventHandler(this.BAreaLinesPen_Click);
            this.Button1.FlatStyle = FlatStyle.Flat;
            this.Button1.Location = new Point(0x10, 0x47);
            this.Button1.Name = "Button1";
            this.Button1.TabIndex = 2;
            this.Button1.Text = "&Pattern...";
            this.Button1.Click += new EventHandler(this.Button1_Click);
            this.groupBox1.Controls.Add(this.BAreaColor);
            this.groupBox1.Controls.Add(this.CBColorEach);
            this.groupBox1.Location = new Point(120, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x70, 0x4a);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Color:";
            this.BAreaColor.FlatStyle = FlatStyle.Flat;
            this.BAreaColor.Location = new Point(0x10, 40);
            this.BAreaColor.Name = "BAreaColor";
            this.BAreaColor.TabIndex = 1;
            this.BAreaColor.Text = "&Color...";
            this.CBColorEach.FlatStyle = FlatStyle.Flat;
            this.CBColorEach.Location = new Point(0x10, 0x10);
            this.CBColorEach.Name = "CBColorEach";
            this.CBColorEach.Size = new Size(0x55, 0x10);
            this.CBColorEach.TabIndex = 0;
            this.CBColorEach.Text = "Color &Each";
            this.CBColorEach.CheckedChanged += new EventHandler(this.CBColorEach_CheckedChanged);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x68, 0x60);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x4d, 0x10);
            this.label1.TabIndex = 4;
            this.label1.Text = "&Transparency:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            this.UDTransp.BorderStyle = BorderStyle.FixedSingle;
            this.UDTransp.Location = new Point(0xb8, 0x5c);
            this.UDTransp.Name = "UDTransp";
            this.UDTransp.Size = new Size(0x30, 20);
            this.UDTransp.TabIndex = 5;
            this.UDTransp.TextAlign = HorizontalAlignment.Right;
            this.UDTransp.ValueChanged += new EventHandler(this.UDTransp_ValueChanged);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0xf6, 0x86);
            base.Controls.Add(this.UDTransp);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.Button1);
            base.Controls.Add(this.BAreaLinesPen);
            base.Controls.Add(this.BAreaLinePen);
            base.Name = "HistogramSeries";
            this.groupBox1.ResumeLayout(false);
            this.UDTransp.EndInit();
            base.ResumeLayout(false);
        }

        private void UDTransp_ValueChanged(object sender, EventArgs e)
        {
            this.series.Transparency = (int) this.UDTransp.Value;
        }
    }
}

