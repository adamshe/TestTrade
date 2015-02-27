namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class SurfaceSeries : Form
    {
        private Button Button1;
        private Button Button2;
        private Button Button3;
        private CheckBox CBSmooth;
        private Container components;
        private Grid3DSeries grid3DEditor;
        private GroupBox groupBox1;
        private RadioButton radioButton1;
        private RadioButton radioButton2;
        private RadioButton radioButton3;
        protected internal Surface series;

        public SurfaceSeries()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public SurfaceSeries(Series s) : this()
        {
            this.series = (Surface) s;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            BrushEditor.Edit(this.series.SideBrush);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            PenEditor.Edit(this.series.Pen);
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            BrushEditor.Edit(this.series.Brush);
        }

        private void CBSmooth_CheckedChanged(object sender, EventArgs e)
        {
            this.series.SmoothPalette = this.CBSmooth.Checked;
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
            this.Button2 = new Button();
            this.Button3 = new Button();
            this.CBSmooth = new CheckBox();
            this.groupBox1 = new GroupBox();
            this.radioButton3 = new RadioButton();
            this.radioButton2 = new RadioButton();
            this.radioButton1 = new RadioButton();
            this.Button1 = new Button();
            this.groupBox1.SuspendLayout();
            base.SuspendLayout();
            this.Button2.FlatStyle = FlatStyle.Flat;
            this.Button2.Location = new Point(8, 8);
            this.Button2.Name = "Button2";
            this.Button2.TabIndex = 0;
            this.Button2.Text = "&Pen...";
            this.Button2.Click += new EventHandler(this.Button2_Click);
            this.Button3.FlatStyle = FlatStyle.Flat;
            this.Button3.Location = new Point(8, 40);
            this.Button3.Name = "Button3";
            this.Button3.TabIndex = 1;
            this.Button3.Text = "&Brush...";
            this.Button3.Click += new EventHandler(this.Button3_Click);
            this.CBSmooth.FlatStyle = FlatStyle.Flat;
            this.CBSmooth.Location = new Point(8, 0x48);
            this.CBSmooth.Name = "CBSmooth";
            this.CBSmooth.Size = new Size(0x98, 0x10);
            this.CBSmooth.TabIndex = 2;
            this.CBSmooth.Text = "&Smooth palette";
            this.CBSmooth.CheckedChanged += new EventHandler(this.CBSmooth_CheckedChanged);
            this.groupBox1.Controls.Add(this.radioButton3);
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Location = new Point(0x60, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x100, 40);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "&Drawing Mode:";
            this.radioButton3.FlatStyle = FlatStyle.Flat;
            this.radioButton3.Location = new Point(0xa8, 0x10);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new Size(80, 0x10);
            this.radioButton3.TabIndex = 2;
            this.radioButton3.Text = "Dot&Frame";
            this.radioButton3.CheckedChanged += new EventHandler(this.radioButton3_CheckedChanged);
            this.radioButton2.FlatStyle = FlatStyle.Flat;
            this.radioButton2.Location = new Point(0x4b, 0x10);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new Size(80, 0x10);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.Text = "&WireFrame";
            this.radioButton2.CheckedChanged += new EventHandler(this.radioButton2_CheckedChanged);
            this.radioButton1.Checked = true;
            this.radioButton1.FlatStyle = FlatStyle.Flat;
            this.radioButton1.Location = new Point(8, 0x10);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new Size(80, 0x10);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "S&olid";
            this.radioButton1.CheckedChanged += new EventHandler(this.radioButton1_CheckedChanged);
            this.Button1.FlatStyle = FlatStyle.Flat;
            this.Button1.Location = new Point(0xb3, 0x3d);
            this.Button1.Name = "Button1";
            this.Button1.Size = new Size(120, 0x17);
            this.Button1.TabIndex = 4;
            this.Button1.Text = "S&ide Brush...";
            this.Button1.Click += new EventHandler(this.Button1_Click);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x16b, 0x5d);
            base.Controls.Add(this.Button1);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.CBSmooth);
            base.Controls.Add(this.Button3);
            base.Controls.Add(this.Button2);
            base.Name = "SurfaceSeries";
            base.Load += new EventHandler(this.SurfaceSeries_Load);
            this.groupBox1.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            this.series.WireFrame = false;
            this.series.DotFrame = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            this.series.WireFrame = true;
            this.series.DotFrame = false;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            this.series.WireFrame = false;
            this.series.DotFrame = true;
        }

        private void SurfaceSeries_Load(object sender, EventArgs e)
        {
            if (this.series != null)
            {
                this.CBSmooth.Checked = this.series.SmoothPalette;
                if (this.series.WireFrame)
                {
                    this.radioButton2.Checked = true;
                }
                else if (this.series.DotFrame)
                {
                    this.radioButton3.Checked = true;
                }
                else
                {
                    this.radioButton1.Checked = true;
                }
                if (this.grid3DEditor == null)
                {
                    this.grid3DEditor = new Grid3DSeries(this.series, this);
                }
            }
        }
    }
}

