namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class ContourSeries : Form
    {
        private ButtonPen Button2;
        private CheckBox CBAutoLevels;
        private CheckBox CBColorEach;
        private CheckBox CBYPosLevel;
        private Container components;
        private ContourLevel contourLevel;
        private TextBox EValue;
        private Grid3DSeries grid3DEditor;
        private GroupBox groupBox1;
        private Label label1;
        private Label label2;
        private Label label3;
        private Contour series;
        private NumericUpDown UDLevel;
        private NumericUpDown UDNum;
        private NumericUpDown UDYPos;

        public ContourSeries()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public ContourSeries(Series s) : this()
        {
            this.series = (Contour) s;
            this.UDYPos.Value = Convert.ToDecimal(this.series.YPosition);
            this.UDLevel.Value = 0M;
            this.UDNum.Value = Convert.ToDecimal(this.series.NumLevels);
            this.CBYPosLevel.Checked = this.series.YPositionLevel;
            this.CBAutoLevels.Checked = this.series.AutomaticLevels;
            this.UDNum.Enabled = this.CBAutoLevels.Checked;
            this.CBColorEach.Checked = this.series.ColorEach;
            this.CBColorEach.Enabled = this.CBAutoLevels.Checked;
            this.Button2.Pen = this.series.Pen;
        }

        private void CBAutoLevels_CheckedChanged(object sender, EventArgs e)
        {
            this.series.AutomaticLevels = this.CBAutoLevels.Checked;
            this.UDNum.Enabled = this.CBAutoLevels.Checked;
            this.CBColorEach.Enabled = this.CBAutoLevels.Checked;
        }

        private void CBColorEach_CheckedChanged(object sender, EventArgs e)
        {
            this.series.ColorEach = this.CBColorEach.Checked;
        }

        private void CBYPosLevel_CheckedChanged(object sender, EventArgs e)
        {
            this.series.YPositionLevel = this.CBYPosLevel.Checked;
        }

        private void ContourSeries_Load(object sender, EventArgs e)
        {
            if ((this.series != null) && (this.grid3DEditor == null))
            {
                this.grid3DEditor = new Grid3DSeries(this.series, this);
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

        private void EValue_TextChanged(object sender, EventArgs e)
        {
            this.contourLevel.UpToValue = Convert.ToDouble(this.EValue.Text);
            this.series.Invalidate();
        }

        private void InitializeComponent()
        {
            this.Button2 = new ButtonPen();
            this.groupBox1 = new GroupBox();
            this.EValue = new TextBox();
            this.label3 = new Label();
            this.UDLevel = new NumericUpDown();
            this.label2 = new Label();
            this.UDNum = new NumericUpDown();
            this.CBColorEach = new CheckBox();
            this.CBAutoLevels = new CheckBox();
            this.label1 = new Label();
            this.CBYPosLevel = new CheckBox();
            this.UDYPos = new NumericUpDown();
            this.groupBox1.SuspendLayout();
            this.UDLevel.BeginInit();
            this.UDNum.BeginInit();
            this.UDYPos.BeginInit();
            base.SuspendLayout();
            this.Button2.FlatStyle = FlatStyle.Flat;
            this.Button2.Location = new Point(4, 8);
            this.Button2.Name = "Button2";
            this.Button2.TabIndex = 0;
            this.Button2.Text = "&Pen...";
            this.groupBox1.Controls.Add(this.EValue);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.UDLevel);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.UDNum);
            this.groupBox1.Controls.Add(this.CBColorEach);
            this.groupBox1.Controls.Add(this.CBAutoLevels);
            this.groupBox1.Location = new Point(4, 0x48);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x15c, 0x58);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Levels:";
            this.EValue.BorderStyle = BorderStyle.FixedSingle;
            this.EValue.Location = new Point(0x98, 0x35);
            this.EValue.Name = "EValue";
            this.EValue.Size = new Size(0x60, 20);
            this.EValue.TabIndex = 6;
            this.EValue.Text = "";
            this.EValue.TextChanged += new EventHandler(this.EValue_TextChanged);
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0x2e, 0x37);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x22, 0x10);
            this.label3.TabIndex = 5;
            this.label3.Text = "&Level:";
            this.label3.TextAlign = ContentAlignment.TopRight;
            this.UDLevel.BorderStyle = BorderStyle.FixedSingle;
            this.UDLevel.Location = new Point(0x58, 0x35);
            int[] bits = new int[4];
            bits[0] = 10;
            this.UDLevel.Maximum = new decimal(bits);
            this.UDLevel.Name = "UDLevel";
            this.UDLevel.Size = new Size(0x38, 20);
            this.UDLevel.TabIndex = 4;
            this.UDLevel.TextAlign = HorizontalAlignment.Right;
            this.UDLevel.TextChanged += new EventHandler(this.UDLevel_ValueChanged);
            this.UDLevel.ValueChanged += new EventHandler(this.UDLevel_ValueChanged);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x6b, 0x12);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x30, 0x10);
            this.label2.TabIndex = 3;
            this.label2.Text = "&Number:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.UDNum.BorderStyle = BorderStyle.FixedSingle;
            this.UDNum.Location = new Point(160, 0x10);
            bits = new int[4];
            bits[0] = 150;
            this.UDNum.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 1;
            this.UDNum.Minimum = new decimal(bits);
            this.UDNum.Name = "UDNum";
            this.UDNum.Size = new Size(0x38, 20);
            this.UDNum.TabIndex = 2;
            this.UDNum.TextAlign = HorizontalAlignment.Right;
            bits = new int[4];
            bits[0] = 1;
            this.UDNum.Value = new decimal(bits);
            this.UDNum.TextChanged += new EventHandler(this.UDNum_ValueChanged);
            this.UDNum.ValueChanged += new EventHandler(this.UDNum_ValueChanged);
            this.CBColorEach.FlatStyle = FlatStyle.Flat;
            this.CBColorEach.Location = new Point(0xdf, 0x13);
            this.CBColorEach.Name = "CBColorEach";
            this.CBColorEach.Size = new Size(0x79, 13);
            this.CBColorEach.TabIndex = 1;
            this.CBColorEach.Text = "&Color Each";
            this.CBColorEach.CheckedChanged += new EventHandler(this.CBColorEach_CheckedChanged);
            this.CBAutoLevels.FlatStyle = FlatStyle.Flat;
            this.CBAutoLevels.Location = new Point(6, 0x13);
            this.CBAutoLevels.Name = "CBAutoLevels";
            this.CBAutoLevels.Size = new Size(0x62, 13);
            this.CBAutoLevels.TabIndex = 0;
            this.CBAutoLevels.Text = "&Automatic";
            this.CBAutoLevels.CheckedChanged += new EventHandler(this.CBAutoLevels_CheckedChanged);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x1a, 0x29);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x59, 0x10);
            this.label1.TabIndex = 2;
            this.label1.Text = "&Vertical Position:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            this.CBYPosLevel.FlatStyle = FlatStyle.Flat;
            this.CBYPosLevel.Location = new Point(0xc6, 0x27);
            this.CBYPosLevel.Name = "CBYPosLevel";
            this.CBYPosLevel.Size = new Size(0x7a, 20);
            this.CBYPosLevel.TabIndex = 3;
            this.CBYPosLevel.Text = "&Levels position";
            this.CBYPosLevel.CheckedChanged += new EventHandler(this.CBYPosLevel_CheckedChanged);
            this.UDYPos.BorderStyle = BorderStyle.FixedSingle;
            this.UDYPos.Location = new Point(0x76, 0x27);
            bits = new int[4];
            bits[0] = 0x7530;
            this.UDYPos.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 0x7530;
            bits[3] = -2147483648;
            this.UDYPos.Minimum = new decimal(bits);
            this.UDYPos.Name = "UDYPos";
            this.UDYPos.Size = new Size(0x48, 20);
            this.UDYPos.TabIndex = 4;
            this.UDYPos.TextAlign = HorizontalAlignment.Right;
            this.UDYPos.TextChanged += new EventHandler(this.UDYPos_ValueChanged);
            this.UDYPos.ValueChanged += new EventHandler(this.UDYPos_ValueChanged);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x160, 0xa6);
            base.Controls.Add(this.UDYPos);
            base.Controls.Add(this.CBYPosLevel);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.Button2);
            base.Name = "ContourSeries";
            base.Load += new EventHandler(this.ContourSeries_Load);
            this.groupBox1.ResumeLayout(false);
            this.UDLevel.EndInit();
            this.UDNum.EndInit();
            this.UDYPos.EndInit();
            base.ResumeLayout(false);
        }

        private void UDLevel_ValueChanged(object sender, EventArgs e)
        {
            this.contourLevel = this.series.Levels[Convert.ToInt32(this.UDLevel.Value)];
            this.EValue.Text = this.contourLevel.UpToValue.ToString();
        }

        private void UDNum_ValueChanged(object sender, EventArgs e)
        {
            if (this.series != null)
            {
                this.series.NumLevels = Convert.ToInt32(this.UDNum.Value);
                this.series.CreateAutoLevels();
            }
        }

        private void UDYPos_ValueChanged(object sender, EventArgs e)
        {
            this.series.YPosition = Convert.ToDouble(this.UDYPos.Value);
        }
    }
}

