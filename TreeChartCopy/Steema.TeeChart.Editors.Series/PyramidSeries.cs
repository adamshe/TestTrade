namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class PyramidSeries : Form
    {
        private ButtonColor BColor;
        private Button BPen;
        private Button Button1;
        private CheckBox CBColorEach;
        private Container components;
        private Label label1;
        private Pyramid series;
        private NumericUpDown UDSize;

        public PyramidSeries()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public PyramidSeries(Series s) : this()
        {
            this.series = (Pyramid) s;
            this.CBColorEach.Checked = this.series.ColorEach;
            this.UDSize.Value = this.series.SizePercent;
            this.BColor.Enabled = !this.series.ColorEach;
            this.BColor.Color = this.series.Color;
        }

        private void BColor_Click(object sender, EventArgs e)
        {
            this.series.Color = this.BColor.Color;
        }

        private void BPen_Click(object sender, EventArgs e)
        {
            PenEditor.Edit(this.series.Pen);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            BrushEditor.Edit(this.series.Brush);
        }

        private void CBColorEach_CheckedChanged(object sender, EventArgs e)
        {
            this.series.ColorEach = this.CBColorEach.Checked;
            this.BColor.Enabled = !this.series.ColorEach;
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
            this.BPen = new Button();
            this.BColor = new ButtonColor();
            this.Button1 = new Button();
            this.CBColorEach = new CheckBox();
            this.label1 = new Label();
            this.UDSize = new NumericUpDown();
            this.UDSize.BeginInit();
            base.SuspendLayout();
            this.BPen.FlatStyle = FlatStyle.Flat;
            this.BPen.Location = new Point(0x11, 0x11);
            this.BPen.Name = "BPen";
            this.BPen.TabIndex = 0;
            this.BPen.Text = "&Border...";
            this.BPen.Click += new EventHandler(this.BPen_Click);
            this.BColor.Color = Color.Empty;
            this.BColor.Location = new Point(0x77, 0x11);
            this.BColor.Name = "BColor";
            this.BColor.TabIndex = 1;
            this.BColor.Text = "&Color...";
            this.BColor.Click += new EventHandler(this.BColor_Click);
            this.Button1.FlatStyle = FlatStyle.Flat;
            this.Button1.Location = new Point(0x11, 0x33);
            this.Button1.Name = "Button1";
            this.Button1.TabIndex = 2;
            this.Button1.Text = "&Pattern...";
            this.Button1.Click += new EventHandler(this.Button1_Click);
            this.CBColorEach.FlatStyle = FlatStyle.Flat;
            this.CBColorEach.Location = new Point(0x77, 0x37);
            this.CBColorEach.Name = "CBColorEach";
            this.CBColorEach.Size = new Size(0x5e, 0x12);
            this.CBColorEach.TabIndex = 3;
            this.CBColorEach.Text = "Color &Each";
            this.CBColorEach.CheckedChanged += new EventHandler(this.CBColorEach_CheckedChanged);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x33, 0x61);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x2b, 0x10);
            this.label1.TabIndex = 4;
            this.label1.Text = "&Size %:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            this.UDSize.BorderStyle = BorderStyle.FixedSingle;
            int[] bits = new int[4];
            bits[0] = 5;
            this.UDSize.Increment = new decimal(bits);
            this.UDSize.Location = new Point(0x66, 0x5e);
            this.UDSize.Name = "UDSize";
            this.UDSize.Size = new Size(60, 20);
            this.UDSize.TabIndex = 5;
            this.UDSize.TextAlign = HorizontalAlignment.Right;
            this.UDSize.TextChanged += new EventHandler(this.UDSize_ValueChanged);
            this.UDSize.ValueChanged += new EventHandler(this.UDSize_ValueChanged);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0xdb, 0x7c);
            base.Controls.Add(this.UDSize);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.CBColorEach);
            base.Controls.Add(this.Button1);
            base.Controls.Add(this.BColor);
            base.Controls.Add(this.BPen);
            base.Name = "PyramidSeries";
            this.UDSize.EndInit();
            base.ResumeLayout(false);
        }

        private void UDSize_ValueChanged(object sender, EventArgs e)
        {
            this.series.SizePercent = (int) this.UDSize.Value;
        }
    }
}

