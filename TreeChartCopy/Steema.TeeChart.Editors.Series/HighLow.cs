namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class HighLow : Form
    {
        private Button BBrush;
        private ButtonColor BColor;
        private ButtonPen BHighPen;
        private Button BLowBrush;
        private ButtonPen BLowPen;
        private ButtonPen BPen;
        private CheckBox CBColorEach;
        private Container components;
        private Steema.TeeChart.Styles.HighLow series;

        public HighLow()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public HighLow(Series s) : this()
        {
            this.series = (Steema.TeeChart.Styles.HighLow) s;
        }

        private void BBrush_Click(object sender, EventArgs e)
        {
            BrushEditor.Edit(this.series.HighBrush);
        }

        private void BColor_Click(object sender, EventArgs e)
        {
            this.series.Color = this.BColor.Color;
        }

        private void BLowBrush_Click(object sender, EventArgs e)
        {
            BrushEditor.Edit(this.series.LowBrush);
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

        private void HighLow_Load(object sender, EventArgs e)
        {
            if (this.series != null)
            {
                this.BPen.Pen = this.series.Pen;
                this.BHighPen.Pen = this.series.HighPen;
                this.BLowPen.Pen = this.series.LowPen;
                this.BColor.Color = this.series.Color;
            }
        }

        private void InitializeComponent()
        {
            this.BPen = new ButtonPen();
            this.BLowPen = new ButtonPen();
            this.BColor = new ButtonColor();
            this.BHighPen = new ButtonPen();
            this.BBrush = new Button();
            this.BLowBrush = new Button();
            this.CBColorEach = new CheckBox();
            base.SuspendLayout();
            this.BPen.FlatStyle = FlatStyle.Flat;
            this.BPen.Location = new Point(13, 15);
            this.BPen.Name = "BPen";
            this.BPen.TabIndex = 0;
            this.BPen.Text = "&Lines...";
            this.BLowPen.FlatStyle = FlatStyle.Flat;
            this.BLowPen.Location = new Point(0x68, 0x2f);
            this.BLowPen.Name = "BLowPen";
            this.BLowPen.TabIndex = 1;
            this.BLowPen.Text = "&Low...";
            this.BColor.Color = Color.Empty;
            this.BColor.Location = new Point(13, 0x2f);
            this.BColor.Name = "BColor";
            this.BColor.TabIndex = 2;
            this.BColor.Text = "&Color...";
            this.BColor.Click += new EventHandler(this.BColor_Click);
            this.BHighPen.FlatStyle = FlatStyle.Flat;
            this.BHighPen.Location = new Point(0x68, 15);
            this.BHighPen.Name = "BHighPen";
            this.BHighPen.TabIndex = 3;
            this.BHighPen.Text = "&High...";
            this.BBrush.FlatStyle = FlatStyle.Flat;
            this.BBrush.Location = new Point(0xc3, 15);
            this.BBrush.Name = "BBrush";
            this.BBrush.TabIndex = 4;
            this.BBrush.Text = "&Pattern...";
            this.BBrush.Click += new EventHandler(this.BBrush_Click);
            this.BLowBrush.FlatStyle = FlatStyle.Flat;
            this.BLowBrush.Location = new Point(0xc3, 0x2f);
            this.BLowBrush.Name = "BLowBrush";
            this.BLowBrush.TabIndex = 5;
            this.BLowBrush.Text = "P&attern...";
            this.BLowBrush.Click += new EventHandler(this.BLowBrush_Click);
            this.CBColorEach.FlatStyle = FlatStyle.Flat;
            this.CBColorEach.Location = new Point(13, 0x58);
            this.CBColorEach.Name = "CBColorEach";
            this.CBColorEach.Size = new Size(0x68, 0x10);
            this.CBColorEach.TabIndex = 6;
            this.CBColorEach.Text = "&Color Each";
            this.CBColorEach.CheckedChanged += new EventHandler(this.CBColorEach_CheckedChanged);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x119, 0x74);
            base.Controls.Add(this.CBColorEach);
            base.Controls.Add(this.BLowBrush);
            base.Controls.Add(this.BBrush);
            base.Controls.Add(this.BHighPen);
            base.Controls.Add(this.BColor);
            base.Controls.Add(this.BLowPen);
            base.Controls.Add(this.BPen);
            base.Name = "HighLow";
            base.Load += new EventHandler(this.HighLow_Load);
            base.ResumeLayout(false);
        }
    }
}

