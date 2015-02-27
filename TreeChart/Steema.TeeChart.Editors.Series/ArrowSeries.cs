namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class ArrowSeries : Form
    {
        private ButtonColor BArrowColor;
        private Button BBrush;
        private ButtonPen BPen;
        private CheckBox CBColorEach;
        private Container components;
        private GroupBox groupBox1;
        private Label label1;
        private Label label2;
        private Arrow series;
        private NumericUpDown udHeight;
        private NumericUpDown udWidth;

        public ArrowSeries()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public ArrowSeries(Series s) : this()
        {
            this.series = (Arrow) s;
            this.udWidth.Value = this.series.ArrowWidth;
            this.udHeight.Value = this.series.ArrowHeight;
            this.CBColorEach.Checked = this.series.ColorEach;
            this.BArrowColor.Color = this.series.Color;
            this.BPen.Pen = this.series.Pointer.Pen;
        }

        private void BArrowColor_Click(object sender, EventArgs e)
        {
            this.series.Color = this.BArrowColor.Color;
        }

        private void BBrush_Click(object sender, EventArgs e)
        {
            BrushEditor.Edit(this.series.Pointer.Brush);
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
            this.BPen = new ButtonPen();
            this.groupBox1 = new GroupBox();
            this.BArrowColor = new ButtonColor();
            this.CBColorEach = new CheckBox();
            this.BBrush = new Button();
            this.label1 = new Label();
            this.label2 = new Label();
            this.udWidth = new NumericUpDown();
            this.udHeight = new NumericUpDown();
            this.groupBox1.SuspendLayout();
            this.udWidth.BeginInit();
            this.udHeight.BeginInit();
            base.SuspendLayout();
            this.BPen.FlatStyle = FlatStyle.Flat;
            this.BPen.Location = new Point(8, 8);
            this.BPen.Name = "BPen";
            this.BPen.TabIndex = 0;
            this.BPen.Text = "&Border...";
            this.groupBox1.Controls.Add(this.BArrowColor);
            this.groupBox1.Controls.Add(this.CBColorEach);
            this.groupBox1.Location = new Point(8, 0x21);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(120, 0x3f);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.BArrowColor.Color = Color.Empty;
            this.BArrowColor.Location = new Point(8, 0x20);
            this.BArrowColor.Name = "BArrowColor";
            this.BArrowColor.TabIndex = 1;
            this.BArrowColor.Text = "&Color...";
            this.BArrowColor.Click += new EventHandler(this.BArrowColor_Click);
            this.CBColorEach.FlatStyle = FlatStyle.Flat;
            this.CBColorEach.Location = new Point(8, 10);
            this.CBColorEach.Name = "CBColorEach";
            this.CBColorEach.Size = new Size(0x68, 0x10);
            this.CBColorEach.TabIndex = 0;
            this.CBColorEach.Text = "Color &Each";
            this.CBColorEach.CheckedChanged += new EventHandler(this.CBColorEach_CheckedChanged);
            this.BBrush.FlatStyle = FlatStyle.Flat;
            this.BBrush.Location = new Point(0x9e, 8);
            this.BBrush.Name = "BBrush";
            this.BBrush.TabIndex = 2;
            this.BBrush.Text = "&Pattern...";
            this.BBrush.Click += new EventHandler(this.BBrush_Click);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x91, 0x2e);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x24, 0x10);
            this.label1.TabIndex = 3;
            this.label1.Text = "&Width:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x91, 0x4a);
            this.label2.Name = "label2";
            this.label2.Size = new Size(40, 0x10);
            this.label2.TabIndex = 5;
            this.label2.Text = "&Height:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.udWidth.BorderStyle = BorderStyle.FixedSingle;
            this.udWidth.Location = new Point(0xb8, 0x2c);
            int[] bits = new int[4];
            bits[0] = 1;
            this.udWidth.Minimum = new decimal(bits);
            this.udWidth.Name = "udWidth";
            this.udWidth.Size = new Size(0x30, 20);
            this.udWidth.TabIndex = 4;
            this.udWidth.TextAlign = HorizontalAlignment.Right;
            bits = new int[4];
            bits[0] = 1;
            this.udWidth.Value = new decimal(bits);
            this.udWidth.TextChanged += new EventHandler(this.udWidth_ValueChanged);
            this.udWidth.ValueChanged += new EventHandler(this.udWidth_ValueChanged);
            this.udHeight.BorderStyle = BorderStyle.FixedSingle;
            this.udHeight.Location = new Point(0xb8, 0x48);
            bits = new int[4];
            bits[0] = 1;
            this.udHeight.Minimum = new decimal(bits);
            this.udHeight.Name = "udHeight";
            this.udHeight.Size = new Size(0x30, 20);
            this.udHeight.TabIndex = 6;
            this.udHeight.TextAlign = HorizontalAlignment.Right;
            bits = new int[4];
            bits[0] = 1;
            this.udHeight.Value = new decimal(bits);
            this.udHeight.TextChanged += new EventHandler(this.udHeight_ValueChanged);
            this.udHeight.ValueChanged += new EventHandler(this.udHeight_ValueChanged);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0xef, 0x69);
            base.Controls.Add(this.udHeight);
            base.Controls.Add(this.udWidth);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.BBrush);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.BPen);
            base.Name = "ArrowSeries";
            this.groupBox1.ResumeLayout(false);
            this.udWidth.EndInit();
            this.udHeight.EndInit();
            base.ResumeLayout(false);
        }

        private void udHeight_ValueChanged(object sender, EventArgs e)
        {
            if (this.series != null)
            {
                this.series.ArrowHeight = (int) this.udHeight.Value;
            }
        }

        private void udWidth_ValueChanged(object sender, EventArgs e)
        {
            if (this.series != null)
            {
                this.series.ArrowWidth = (int) this.udWidth.Value;
            }
        }
    }
}

