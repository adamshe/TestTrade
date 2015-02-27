namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class WaterfallSeries : SurfaceSeries
    {
        private Button bLines;
        private Container components;
        private Waterfall waterfall;

        public WaterfallSeries()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public WaterfallSeries(Series s) : this()
        {
            this.waterfall = (Waterfall) s;
            base.series = this.waterfall;
        }

        private void bLines_Click(object sender, EventArgs e)
        {
            PenEditor.Edit(base.series.WaterLines);
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
            this.bLines = new Button();
            base.SuspendLayout();
            this.bLines.FlatStyle = FlatStyle.Flat;
            this.bLines.Location = new Point(11, 0x63);
            this.bLines.Name = "bLines";
            this.bLines.TabIndex = 0;
            this.bLines.Text = "&Lines...";
            this.bLines.Click += new EventHandler(this.bLines_Click);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x177, 0x84);
            base.Controls.AddRange(new Control[] { this.bLines });
            base.Name = "WaterfallSeries";
            base.ResumeLayout(false);
        }
    }
}

