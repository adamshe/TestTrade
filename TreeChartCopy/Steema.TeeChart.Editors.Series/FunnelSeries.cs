namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class FunnelSeries : Form
    {
        private ButtonColor AboveColor;
        private ButtonColor BelowColor;
        private Button Button1;
        private ButtonPen ButtonPen1;
        private ButtonPen ButtonPen2;
        private Container components;
        private NumericUpDown DifLimit;
        private Label label1;
        private Funnel series;
        private ButtonColor WithinColor;

        public FunnelSeries()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public FunnelSeries(Series s) : this()
        {
            this.series = (Funnel) s;
            this.DifLimit.Value = (decimal) this.series.DifferenceLimit;
            this.ButtonPen1.Pen = this.series.LinesPen;
            this.ButtonPen2.Pen = this.series.Pen;
            this.AboveColor.Color = this.series.AboveColor;
            this.WithinColor.Color = this.series.WithinColor;
            this.BelowColor.Color = this.series.BelowColor;
        }

        private void AboveColor_Click(object sender, EventArgs e)
        {
            this.series.AboveColor = this.AboveColor.Color;
        }

        private void BelowColor_Click(object sender, EventArgs e)
        {
            this.series.BelowColor = this.BelowColor.Color;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            BrushEditor.Edit(this.series.Brush);
        }

        private void DifLimit_ValueChanged(object sender, EventArgs e)
        {
            this.series.DifferenceLimit = (double) this.DifLimit.Value;
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
            this.AboveColor = new ButtonColor();
            this.WithinColor = new ButtonColor();
            this.BelowColor = new ButtonColor();
            this.ButtonPen1 = new ButtonPen();
            this.ButtonPen2 = new ButtonPen();
            this.Button1 = new Button();
            this.DifLimit = new NumericUpDown();
            this.label1 = new Label();
            this.DifLimit.BeginInit();
            base.SuspendLayout();
            this.AboveColor.Color = Color.Empty;
            this.AboveColor.Location = new Point(0x10, 0x10);
            this.AboveColor.Name = "AboveColor";
            this.AboveColor.TabIndex = 0;
            this.AboveColor.Text = "&Above...";
            this.AboveColor.Click += new EventHandler(this.AboveColor_Click);
            this.WithinColor.Color = Color.Empty;
            this.WithinColor.Location = new Point(0x10, 0x30);
            this.WithinColor.Name = "WithinColor";
            this.WithinColor.TabIndex = 1;
            this.WithinColor.Text = "&Within...";
            this.WithinColor.Click += new EventHandler(this.WithinColor_Click);
            this.BelowColor.Color = Color.Empty;
            this.BelowColor.Location = new Point(0x10, 80);
            this.BelowColor.Name = "BelowColor";
            this.BelowColor.TabIndex = 2;
            this.BelowColor.Text = "B&elow...";
            this.BelowColor.Click += new EventHandler(this.BelowColor_Click);
            this.ButtonPen1.FlatStyle = FlatStyle.Flat;
            this.ButtonPen1.Location = new Point(0x69, 0x30);
            this.ButtonPen1.Name = "ButtonPen1";
            this.ButtonPen1.TabIndex = 3;
            this.ButtonPen1.Text = "&Lines...";
            this.ButtonPen2.FlatStyle = FlatStyle.Flat;
            this.ButtonPen2.Location = new Point(0x69, 80);
            this.ButtonPen2.Name = "ButtonPen2";
            this.ButtonPen2.TabIndex = 4;
            this.ButtonPen2.Text = "&Border...";
            this.Button1.FlatStyle = FlatStyle.Flat;
            this.Button1.Location = new Point(0xc3, 0x30);
            this.Button1.Name = "Button1";
            this.Button1.TabIndex = 7;
            this.Button1.Text = "&Pattern...";
            this.Button1.Click += new EventHandler(this.Button1_Click);
            this.DifLimit.BorderStyle = BorderStyle.FixedSingle;
            this.DifLimit.Location = new Point(0xc6, 0x12);
            int[] bits = new int[4];
            bits[0] = 50;
            this.DifLimit.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 1;
            this.DifLimit.Minimum = new decimal(bits);
            this.DifLimit.Name = "DifLimit";
            this.DifLimit.Size = new Size(0x40, 20);
            this.DifLimit.TabIndex = 6;
            this.DifLimit.TextAlign = HorizontalAlignment.Right;
            bits = new int[4];
            bits[0] = 1;
            this.DifLimit.Value = new decimal(bits);
            this.DifLimit.ValueChanged += new EventHandler(this.DifLimit_ValueChanged);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x90, 0x16);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x33, 0x10);
            this.label1.TabIndex = 5;
            this.label1.Text = "&Dif. Limit:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x11c, 0x74);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.DifLimit);
            base.Controls.Add(this.Button1);
            base.Controls.Add(this.ButtonPen2);
            base.Controls.Add(this.ButtonPen1);
            base.Controls.Add(this.BelowColor);
            base.Controls.Add(this.WithinColor);
            base.Controls.Add(this.AboveColor);
            base.Name = "FunnelSeries";
            this.DifLimit.EndInit();
            base.ResumeLayout(false);
        }

        private void WithinColor_Click(object sender, EventArgs e)
        {
            this.series.WithinColor = this.WithinColor.Color;
        }
    }
}

