namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class ErrorSeries : Form
    {
        private BarSeries barEditor;
        private ButtonPen BPen;
        private CheckBox CBColorEach;
        private Container components;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Label label1;
        private RadioButton radioButton1;
        private RadioButton radioButton2;
        private RadioButton radioButton3;
        private RadioButton radioButton4;
        private RadioButton radioButton5;
        private RadioButton radioButton6;
        private RadioButton rbPercent;
        private RadioButton rbPixels;
        private CustomError series;
        private NumericUpDown UDBarWidth;

        public ErrorSeries()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public ErrorSeries(Series s) : this()
        {
            this.series = (CustomError) s;
            if (this.series.ErrorWidthUnits == ErrorWidthUnits.Percent)
            {
                this.rbPercent.Checked = true;
            }
            else
            {
                this.rbPixels.Checked = true;
            }
            this.UDBarWidth.Value = this.series.ErrorWidth;
            this.BPen.Pen = this.series.ErrorPen;
            this.CBColorEach.Checked = this.series.ColorEach;
            switch (this.series.ErrorStyle)
            {
                case ErrorStyles.Left:
                    this.radioButton1.Checked = true;
                    return;

                case ErrorStyles.Right:
                    this.radioButton2.Checked = true;
                    return;

                case ErrorStyles.LeftRight:
                    this.radioButton3.Checked = true;
                    return;

                case ErrorStyles.Top:
                    this.radioButton4.Checked = true;
                    return;

                case ErrorStyles.Bottom:
                    this.radioButton5.Checked = true;
                    return;

                case ErrorStyles.TopBottom:
                    this.radioButton6.Checked = true;
                    return;
            }
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

        private void ErrorSeries_Load(object sender, EventArgs e)
        {
            if ((this.series is ErrorBar) && (this.barEditor == null))
            {
                this.barEditor = BarSeries.InsertEditor(base.Parent, this.series);
            }
        }

        private void InitializeComponent()
        {
            this.BPen = new ButtonPen();
            this.label1 = new Label();
            this.UDBarWidth = new NumericUpDown();
            this.CBColorEach = new CheckBox();
            this.groupBox1 = new GroupBox();
            this.rbPixels = new RadioButton();
            this.rbPercent = new RadioButton();
            this.groupBox2 = new GroupBox();
            this.radioButton6 = new RadioButton();
            this.radioButton5 = new RadioButton();
            this.radioButton4 = new RadioButton();
            this.radioButton3 = new RadioButton();
            this.radioButton2 = new RadioButton();
            this.radioButton1 = new RadioButton();
            this.UDBarWidth.BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            base.SuspendLayout();
            this.BPen.Location = new Point(12, 9);
            this.BPen.Name = "BPen";
            this.BPen.TabIndex = 0;
            this.BPen.Text = "&Border...";
            this.label1.AutoSize = true;
            this.label1.Location = new Point(12, 0x2d);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x41, 0x10);
            this.label1.TabIndex = 1;
            this.label1.Text = "Error &Width:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            this.UDBarWidth.BorderStyle = BorderStyle.FixedSingle;
            this.UDBarWidth.Location = new Point(0x4d, 0x2b);
            this.UDBarWidth.Name = "UDBarWidth";
            this.UDBarWidth.Size = new Size(0x33, 20);
            this.UDBarWidth.TabIndex = 2;
            this.UDBarWidth.TextAlign = HorizontalAlignment.Right;
            this.UDBarWidth.ValueChanged += new EventHandler(this.UDBarWidth_ValueChanged);
            this.CBColorEach.FlatStyle = FlatStyle.Flat;
            this.CBColorEach.Location = new Point(12, 0x4b);
            this.CBColorEach.Name = "CBColorEach";
            this.CBColorEach.Size = new Size(0x60, 0x10);
            this.CBColorEach.TabIndex = 3;
            this.CBColorEach.Text = "&Color Each";
            this.CBColorEach.CheckedChanged += new EventHandler(this.CBColorEach_CheckedChanged);
            this.groupBox1.Controls.Add(this.rbPixels);
            this.groupBox1.Controls.Add(this.rbPercent);
            this.groupBox1.Location = new Point(0x88, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x60, 0x40);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Width Units:";
            this.rbPixels.FlatStyle = FlatStyle.Flat;
            this.rbPixels.Location = new Point(12, 40);
            this.rbPixels.Name = "rbPixels";
            this.rbPixels.Size = new Size(0x48, 0x10);
            this.rbPixels.TabIndex = 1;
            this.rbPixels.Text = "P&ixels";
            this.rbPixels.CheckedChanged += new EventHandler(this.rbPixels_CheckedChanged);
            this.rbPercent.FlatStyle = FlatStyle.Flat;
            this.rbPercent.Location = new Point(12, 0x12);
            this.rbPercent.Name = "rbPercent";
            this.rbPercent.Size = new Size(0x48, 0x10);
            this.rbPercent.TabIndex = 0;
            this.rbPercent.Text = "P&ercent";
            this.rbPercent.CheckedChanged += new EventHandler(this.rbPercent_CheckedChanged);
            this.groupBox2.Controls.Add(this.radioButton6);
            this.groupBox2.Controls.Add(this.radioButton5);
            this.groupBox2.Controls.Add(this.radioButton4);
            this.groupBox2.Controls.Add(this.radioButton3);
            this.groupBox2.Controls.Add(this.radioButton2);
            this.groupBox2.Controls.Add(this.radioButton1);
            this.groupBox2.Location = new Point(5, 100);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(0xe3, 80);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Style:";
            this.radioButton6.FlatStyle = FlatStyle.Flat;
            this.radioButton6.Location = new Point(120, 0x38);
            this.radioButton6.Name = "radioButton6";
            this.radioButton6.Size = new Size(0x68, 0x10);
            this.radioButton6.TabIndex = 5;
            this.radioButton6.Text = "T&op and Bottom";
            this.radioButton6.CheckedChanged += new EventHandler(this.radioButton6_CheckedChanged);
            this.radioButton5.FlatStyle = FlatStyle.Flat;
            this.radioButton5.Location = new Point(120, 0x24);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new Size(80, 0x10);
            this.radioButton5.TabIndex = 4;
            this.radioButton5.Text = "&Bottom";
            this.radioButton5.CheckedChanged += new EventHandler(this.radioButton5_CheckedChanged);
            this.radioButton4.FlatStyle = FlatStyle.Flat;
            this.radioButton4.Location = new Point(120, 0x10);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new Size(80, 0x10);
            this.radioButton4.TabIndex = 3;
            this.radioButton4.Text = "&Top";
            this.radioButton4.CheckedChanged += new EventHandler(this.radioButton4_CheckedChanged);
            this.radioButton3.FlatStyle = FlatStyle.Flat;
            this.radioButton3.Location = new Point(0x10, 0x38);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new Size(0x60, 0x10);
            this.radioButton3.TabIndex = 2;
            this.radioButton3.Text = "Left &and Right";
            this.radioButton3.CheckedChanged += new EventHandler(this.radioButton3_CheckedChanged);
            this.radioButton2.FlatStyle = FlatStyle.Flat;
            this.radioButton2.Location = new Point(0x10, 0x24);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new Size(80, 0x10);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.Text = "&Right";
            this.radioButton2.CheckedChanged += new EventHandler(this.radioButton2_CheckedChanged);
            this.radioButton1.FlatStyle = FlatStyle.Flat;
            this.radioButton1.Location = new Point(0x10, 0x10);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new Size(80, 0x10);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.Text = "&Left";
            this.radioButton1.CheckedChanged += new EventHandler(this.radioButton1_CheckedChanged);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0xed, 0xba);
            base.Controls.Add(this.groupBox2);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.CBColorEach);
            base.Controls.Add(this.UDBarWidth);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.BPen);
            base.Name = "ErrorSeries";
            base.Load += new EventHandler(this.ErrorSeries_Load);
            this.UDBarWidth.EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            this.series.ErrorStyle = ErrorStyles.Left;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            this.series.ErrorStyle = ErrorStyles.Right;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            this.series.ErrorStyle = ErrorStyles.LeftRight;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            this.series.ErrorStyle = ErrorStyles.Top;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            this.series.ErrorStyle = ErrorStyles.Bottom;
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            this.series.ErrorStyle = ErrorStyles.TopBottom;
        }

        private void rbPercent_CheckedChanged(object sender, EventArgs e)
        {
            this.series.ErrorWidthUnits = ErrorWidthUnits.Percent;
        }

        private void rbPixels_CheckedChanged(object sender, EventArgs e)
        {
            this.series.ErrorWidthUnits = ErrorWidthUnits.Pixels;
        }

        private void UDBarWidth_ValueChanged(object sender, EventArgs e)
        {
            this.series.ErrorWidth = (int) this.UDBarWidth.Value;
        }
    }
}

