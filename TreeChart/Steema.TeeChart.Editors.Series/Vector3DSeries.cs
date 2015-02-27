namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class Vector3DSeries : Form
    {
        private ButtonPen BPEnd;
        private ButtonPen BPStart;
        private Container components;
        private Label label1;
        private Label label2;
        private Vector3D series;
        private NumericUpDown UDHeight;
        private NumericUpDown UDWidth;

        public Vector3DSeries()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public Vector3DSeries(Series s) : this()
        {
            this.series = (Vector3D) s;
            this.UDWidth.Value = this.series.ArrowWidth;
            this.UDHeight.Value = this.series.ArrowHeight;
            this.BPStart.Pen = this.series.StartArrow;
            this.BPEnd.Pen = this.series.EndArrow;
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
            this.label1 = new Label();
            this.label2 = new Label();
            this.UDWidth = new NumericUpDown();
            this.UDHeight = new NumericUpDown();
            this.BPStart = new ButtonPen();
            this.BPEnd = new ButtonPen();
            this.UDWidth.BeginInit();
            this.UDHeight.BeginInit();
            base.SuspendLayout();
            this.label1.Location = new Point(0x18, 0x10);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x48, 0x10);
            this.label1.TabIndex = 0;
            this.label1.Text = "Arrow Width:";
            this.label2.Location = new Point(0x18, 40);
            this.label2.Name = "label2";
            this.label2.Size = new Size(80, 0x10);
            this.label2.TabIndex = 1;
            this.label2.Text = "Arrow Height:";
            this.UDWidth.Location = new Point(0x5f, 14);
            this.UDWidth.Name = "UDWidth";
            this.UDWidth.Size = new Size(0x49, 20);
            this.UDWidth.TabIndex = 2;
            this.UDWidth.ValueChanged += new EventHandler(this.UDWidth_ValueChanged);
            this.UDHeight.Location = new Point(0x5f, 0x26);
            this.UDHeight.Name = "UDHeight";
            this.UDHeight.Size = new Size(0x49, 20);
            this.UDHeight.TabIndex = 3;
            this.UDHeight.ValueChanged += new EventHandler(this.UDHeight_ValueChanged);
            this.BPStart.FlatStyle = FlatStyle.Flat;
            this.BPStart.Location = new Point(0x18, 80);
            this.BPStart.Name = "BPStart";
            this.BPStart.TabIndex = 4;
            this.BPStart.Text = "Start...";
            this.BPEnd.FlatStyle = FlatStyle.Flat;
            this.BPEnd.Location = new Point(120, 80);
            this.BPEnd.Name = "BPEnd";
            this.BPEnd.TabIndex = 5;
            this.BPEnd.Text = "End...";
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(240, 0x7e);
            base.Controls.Add(this.BPEnd);
            base.Controls.Add(this.BPStart);
            base.Controls.Add(this.UDHeight);
            base.Controls.Add(this.UDWidth);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.Name = "Vector3DSeries";
            this.Text = "Vector3DEditor";
            this.UDWidth.EndInit();
            this.UDHeight.EndInit();
            base.ResumeLayout(false);
        }

        private void UDHeight_ValueChanged(object sender, EventArgs e)
        {
            this.series.ArrowHeight = (int) this.UDHeight.Value;
        }

        private void UDWidth_ValueChanged(object sender, EventArgs e)
        {
            this.series.ArrowWidth = (int) this.UDWidth.Value;
        }
    }
}

