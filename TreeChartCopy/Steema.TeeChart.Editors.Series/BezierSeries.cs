namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class BezierSeries : Form
    {
        private ButtonPen button1;
        private Container components;
        private Steema.TeeChart.Editors.SeriesPointer pointEditor;
        private Bezier series;

        public BezierSeries()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public BezierSeries(Series s) : this()
        {
            this.series = (Bezier) s;
        }

        private void BezierSeries_Load(object sender, EventArgs e)
        {
            if (this.series != null)
            {
                if (this.pointEditor == null)
                {
                    this.pointEditor = Steema.TeeChart.Editors.SeriesPointer.InsertPointer(base.Parent, this.series.Pointer);
                }
                this.button1.Pen = this.series.LinePen;
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

        private void InitializeComponent()
        {
            this.button1 = new ButtonPen();
            base.SuspendLayout();
            this.button1.FlatStyle = FlatStyle.Flat;
            this.button1.Location = new Point(8, 0x10);
            this.button1.Name = "button1";
            this.button1.TabIndex = 0;
            this.button1.Text = "&Border...";
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0xd8, 0x6d);
            base.Controls.Add(this.button1);
            base.Name = "BezierSeries";
            base.Load += new EventHandler(this.BezierSeries_Load);
            base.ResumeLayout(false);
        }
    }
}

