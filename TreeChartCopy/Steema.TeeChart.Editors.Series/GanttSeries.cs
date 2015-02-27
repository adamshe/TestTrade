namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class GanttSeries : Form
    {
        private ButtonColor BColor;
        private ButtonPen BConnLines;
        private CheckBox CBColorEach;
        private Container components;
        private GroupBox groupBox1;
        private Label label1;
        private Gantt series;
        private NumericUpDown UDPointVertSize;

        public GanttSeries()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public GanttSeries(Series s) : this()
        {
            this.series = (Gantt) s;
            this.CBColorEach.Checked = this.series.ColorEach;
            this.UDPointVertSize.Value = this.series.Pointer.VertSize;
            this.BColor.Color = this.series.Color;
            this.BConnLines.Pen = this.series.ConnectingPen;
        }

        private void BColor_Click(object sender, EventArgs e)
        {
            this.series.Color = this.BColor.Color;
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
            this.BConnLines = new ButtonPen();
            this.label1 = new Label();
            this.UDPointVertSize = new NumericUpDown();
            this.groupBox1 = new GroupBox();
            this.CBColorEach = new CheckBox();
            this.BColor = new ButtonColor();
            this.UDPointVertSize.BeginInit();
            this.groupBox1.SuspendLayout();
            base.SuspendLayout();
            this.BConnLines.Location = new Point(8, 8);
            this.BConnLines.Name = "BConnLines";
            this.BConnLines.Size = new Size(0x88, 0x17);
            this.BConnLines.TabIndex = 0;
            this.BConnLines.Text = "Co&nnecting Lines...";
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x1c, 0x2c);
            this.label1.Name = "label1";
            this.label1.Size = new Size(40, 0x10);
            this.label1.TabIndex = 1;
            this.label1.Text = "&Height:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            this.UDPointVertSize.BorderStyle = BorderStyle.FixedSingle;
            this.UDPointVertSize.Location = new Point(0x44, 40);
            int[] bits = new int[4];
            bits[0] = 0x3e8;
            this.UDPointVertSize.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 1;
            this.UDPointVertSize.Minimum = new decimal(bits);
            this.UDPointVertSize.Name = "UDPointVertSize";
            this.UDPointVertSize.Size = new Size(0x38, 20);
            this.UDPointVertSize.TabIndex = 2;
            this.UDPointVertSize.TextAlign = HorizontalAlignment.Right;
            bits = new int[4];
            bits[0] = 1;
            this.UDPointVertSize.Value = new decimal(bits);
            this.UDPointVertSize.ValueChanged += new EventHandler(this.UDPointVertSize_ValueChanged);
            this.groupBox1.Controls.Add(this.CBColorEach);
            this.groupBox1.Controls.Add(this.BColor);
            this.groupBox1.Location = new Point(12, 0x44);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x80, 80);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.CBColorEach.FlatStyle = FlatStyle.Flat;
            this.CBColorEach.Location = new Point(20, 0x34);
            this.CBColorEach.Name = "CBColorEach";
            this.CBColorEach.Size = new Size(0x60, 0x10);
            this.CBColorEach.TabIndex = 1;
            this.CBColorEach.Text = "Color &Each";
            this.CBColorEach.CheckedChanged += new EventHandler(this.CBColorEach_CheckedChanged);
            this.BColor.Color = Color.Empty;
            this.BColor.Location = new Point(0x18, 0x10);
            this.BColor.Name = "BColor";
            this.BColor.TabIndex = 0;
            this.BColor.Text = "&Color...";
            this.BColor.Click += new EventHandler(this.BColor_Click);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x98, 160);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.UDPointVertSize);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.BConnLines);
            base.Name = "GanttSeries";
            this.UDPointVertSize.EndInit();
            this.groupBox1.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void UDPointVertSize_ValueChanged(object sender, EventArgs e)
        {
            this.series.Pointer.VertSize = (int) this.UDPointVertSize.Value;
        }
    }
}

