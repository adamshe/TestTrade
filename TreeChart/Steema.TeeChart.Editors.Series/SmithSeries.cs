namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class SmithSeries : Form
    {
        private ButtonPen BBorder;
        private Button BCFont;
        private ButtonPen BCircle;
        private ButtonPen BCPen;
        private Button BRFont;
        private ButtonPen BRPen;
        private CheckBox CBC;
        private CheckBox CBColorEach;
        private CheckBox CBR;
        private CircledSeries circledEditor;
        private Container components;
        private TextBox EImag;
        private Label label1;
        private Steema.TeeChart.Editors.SeriesPointer pointerEditor;
        private Smith series;

        public SmithSeries()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public SmithSeries(Series s) : this()
        {
            this.series = (Smith) s;
        }

        private void CBC_CheckedChanged(object sender, EventArgs e)
        {
            this.series.CLabels = this.CBC.Checked;
        }

        private void CBColorEach_CheckedChanged(object sender, EventArgs e)
        {
            this.series.ColorEach = this.CBColorEach.Checked;
        }

        private void CBR_CheckedChanged(object sender, EventArgs e)
        {
            this.series.RLabels = this.CBR.Checked;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void EImag_TextChanged(object sender, EventArgs e)
        {
            this.series.ImagSymbol = this.EImag.Text;
        }

        private void InitializeComponent()
        {
            this.CBC = new CheckBox();
            this.CBR = new CheckBox();
            this.BCPen = new ButtonPen();
            this.BRPen = new ButtonPen();
            this.BCFont = new Button();
            this.BRFont = new Button();
            this.BBorder = new ButtonPen();
            this.BCircle = new ButtonPen();
            this.CBColorEach = new CheckBox();
            this.label1 = new Label();
            this.EImag = new TextBox();
            base.SuspendLayout();
            this.CBC.FlatStyle = FlatStyle.Flat;
            this.CBC.Location = new Point(15, 0x10);
            this.CBC.Name = "CBC";
            this.CBC.Size = new Size(80, 0x10);
            this.CBC.TabIndex = 0;
            this.CBC.Text = "&C Labels";
            this.CBC.CheckedChanged += new EventHandler(this.CBC_CheckedChanged);
            this.CBR.FlatStyle = FlatStyle.Flat;
            this.CBR.Location = new Point(15, 0x30);
            this.CBR.Name = "CBR";
            this.CBR.Size = new Size(80, 0x10);
            this.CBR.TabIndex = 1;
            this.CBR.Text = "&R Labels";
            this.CBR.CheckedChanged += new EventHandler(this.CBR_CheckedChanged);
            this.BCPen.FlatStyle = FlatStyle.Flat;
            this.BCPen.Location = new Point(0x65, 12);
            this.BCPen.Name = "BCPen";
            this.BCPen.TabIndex = 2;
            this.BCPen.Text = "C &Pen...";
            this.BRPen.FlatStyle = FlatStyle.Flat;
            this.BRPen.Location = new Point(0x65, 0x2c);
            this.BRPen.Name = "BRPen";
            this.BRPen.TabIndex = 3;
            this.BRPen.Text = "R P&en...";
            this.BCFont.FlatStyle = FlatStyle.Flat;
            this.BCFont.Location = new Point(0xb7, 12);
            this.BCFont.Name = "BCFont";
            this.BCFont.TabIndex = 4;
            this.BCFont.Text = "&Font...";
            this.BRFont.FlatStyle = FlatStyle.Flat;
            this.BRFont.Location = new Point(0xb7, 0x2c);
            this.BRFont.Name = "BRFont";
            this.BRFont.TabIndex = 5;
            this.BRFont.Text = "Fo&nt...";
            this.BBorder.FlatStyle = FlatStyle.Flat;
            this.BBorder.Location = new Point(15, 0x70);
            this.BBorder.Name = "BBorder";
            this.BBorder.TabIndex = 6;
            this.BBorder.Text = "&Border...";
            this.BCircle.FlatStyle = FlatStyle.Flat;
            this.BCircle.Location = new Point(0x65, 0x70);
            this.BCircle.Name = "BCircle";
            this.BCircle.TabIndex = 7;
            this.BCircle.Text = "&Circle...";
            this.CBColorEach.FlatStyle = FlatStyle.Flat;
            this.CBColorEach.Location = new Point(0x10, 0x52);
            this.CBColorEach.Name = "CBColorEach";
            this.CBColorEach.Size = new Size(0x68, 0x10);
            this.CBColorEach.TabIndex = 8;
            this.CBColorEach.Text = "C&olor Each";
            this.CBColorEach.CheckedChanged += new EventHandler(this.CBColorEach_CheckedChanged);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x88, 0x51);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x4c, 0x10);
            this.label1.TabIndex = 9;
            this.label1.Text = "&Imag. Symbol:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            this.EImag.BorderStyle = BorderStyle.FixedSingle;
            this.EImag.Location = new Point(0xd8, 0x4f);
            this.EImag.Name = "EImag";
            this.EImag.Size = new Size(40, 20);
            this.EImag.TabIndex = 10;
            this.EImag.Text = "";
            this.EImag.TextChanged += new EventHandler(this.EImag_TextChanged);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x110, 0x94);
            base.Controls.Add(this.EImag);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.CBColorEach);
            base.Controls.Add(this.BCircle);
            base.Controls.Add(this.BBorder);
            base.Controls.Add(this.BRFont);
            base.Controls.Add(this.BCFont);
            base.Controls.Add(this.BRPen);
            base.Controls.Add(this.BCPen);
            base.Controls.Add(this.CBR);
            base.Controls.Add(this.CBC);
            base.Name = "SmithSeries";
            base.Load += new EventHandler(this.SmithSeries_Load);
            base.ResumeLayout(false);
        }

        private void SmithSeries_Load(object sender, EventArgs e)
        {
            if (this.series != null)
            {
                this.CBColorEach.Checked = this.series.ColorEach;
                this.CBR.Checked = this.series.RLabels;
                this.CBC.Checked = this.series.CLabels;
                this.EImag.Text = this.series.ImagSymbol;
                this.BRPen.Pen = this.series.RCirclePen;
                this.BCPen.Pen = this.series.CCirclePen;
                this.BCircle.Pen = this.series.CirclePen;
                this.BBorder.Pen = this.series.Pen;
                if (this.pointerEditor == null)
                {
                    this.pointerEditor = Steema.TeeChart.Editors.SeriesPointer.InsertPointer(base.Parent, this.series.Pointer);
                }
                if (this.circledEditor == null)
                {
                    this.circledEditor = CircledSeries.InsertForm(base.Parent, this.series);
                }
            }
        }
    }
}

