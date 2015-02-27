namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class Point3DSeries : Form
    {
        private ButtonColor BColor;
        private ButtonPen Button1;
        private CheckBox CBColorEach;
        private Container components;
        private GroupBox groupBox1;
        private Label label1;
        private Steema.TeeChart.Editors.SeriesPointer pointerEditor;
        private Points3D series;
        private NumericUpDown UDPointDepth;

        public Point3DSeries()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public Point3DSeries(Points3D s) : this()
        {
            this.series = s;
            this.CBColorEach.Checked = this.series.ColorEach;
            this.UDPointDepth.Value = (decimal) this.series.DepthSize;
            this.BColor.Enabled = !this.series.ColorEach;
            this.BColor.Color = this.series.Color;
            this.Button1.Pen = this.series.LinePen;
        }

        private void BColor_Click(object sender, EventArgs e)
        {
            this.series.Color = this.BColor.Color;
            this.series.ColorEach = false;
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
            this.groupBox1 = new GroupBox();
            this.CBColorEach = new CheckBox();
            this.BColor = new ButtonColor();
            this.Button1 = new ButtonPen();
            this.UDPointDepth = new NumericUpDown();
            this.label1 = new Label();
            this.groupBox1.SuspendLayout();
            this.UDPointDepth.BeginInit();
            base.SuspendLayout();
            this.groupBox1.Controls.Add(this.CBColorEach);
            this.groupBox1.Controls.Add(this.BColor);
            this.groupBox1.Location = new Point(8, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x80, 80);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.CBColorEach.FlatStyle = FlatStyle.Flat;
            this.CBColorEach.Location = new Point(0x10, 0x38);
            this.CBColorEach.Name = "CBColorEach";
            this.CBColorEach.Size = new Size(0x68, 0x10);
            this.CBColorEach.TabIndex = 1;
            this.CBColorEach.Text = "Color &Each";
            this.CBColorEach.CheckedChanged += new EventHandler(this.CBColorEach_CheckedChanged);
            this.BColor.Color = Color.Empty;
            this.BColor.Location = new Point(0x10, 0x10);
            this.BColor.Name = "BColor";
            this.BColor.TabIndex = 0;
            this.BColor.Text = "&Color...";
            this.BColor.Click += new EventHandler(this.BColor_Click);
            this.Button1.FlatStyle = FlatStyle.Flat;
            this.Button1.Location = new Point(0x98, 0x10);
            this.Button1.Name = "Button1";
            this.Button1.TabIndex = 1;
            this.Button1.Text = "&Line...";
            this.UDPointDepth.BorderStyle = BorderStyle.FixedSingle;
            this.UDPointDepth.Location = new Point(0xb3, 0x38);
            this.UDPointDepth.Name = "UDPointDepth";
            this.UDPointDepth.Size = new Size(0x30, 20);
            this.UDPointDepth.TabIndex = 3;
            this.UDPointDepth.TextAlign = HorizontalAlignment.Right;
            this.UDPointDepth.ValueChanged += new EventHandler(this.UDPointDepth_ValueChanged);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x88, 60);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x26, 0x10);
            this.label1.TabIndex = 2;
            this.label1.Text = "Depth:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0xf1, 0x57);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.UDPointDepth);
            base.Controls.Add(this.Button1);
            base.Controls.Add(this.groupBox1);
            base.Name = "Point3DSeries";
            base.Load += new EventHandler(this.Point3DSeries_Load);
            this.groupBox1.ResumeLayout(false);
            this.UDPointDepth.EndInit();
            base.ResumeLayout(false);
        }

        private void Point3DSeries_Load(object sender, EventArgs e)
        {
            if ((this.series != null) && (this.pointerEditor == null))
            {
                this.pointerEditor = Steema.TeeChart.Editors.SeriesPointer.InsertPointer(base.Parent, this.series.Pointer);
            }
        }

        private void UDPointDepth_ValueChanged(object sender, EventArgs e)
        {
            this.series.DepthSize = (double) this.UDPointDepth.Value;
        }
    }
}

