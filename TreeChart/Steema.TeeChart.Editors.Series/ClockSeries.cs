namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    public class ClockSeries : PolarSeries
    {
        private IContainer components;
        private Clock series;

        public ClockSeries()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public ClockSeries(Series s) : this()
        {
            this.series = (Clock) s;
            base.SetPolar(this.series);
        }

        private void ClockEditor_Load(object sender, EventArgs e)
        {
            base.label1.Visible = false;
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
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x188, 0xc5);
            base.Name = "ClockSeries";
            base.Load += new EventHandler(this.ClockEditor_Load);
        }
    }
}

