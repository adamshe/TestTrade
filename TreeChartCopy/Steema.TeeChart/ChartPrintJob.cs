namespace Steema.TeeChart
{
    using System;
    using System.Drawing;

    public class ChartPrintJob
    {
        private Steema.TeeChart.Chart chart;
        private Rectangle chartRect;

        public ChartPrintJob(Steema.TeeChart.Chart c, Rectangle r)
        {
            this.chart = c;
            this.chartRect = r;
        }

        public Steema.TeeChart.Chart Chart
        {
            get
            {
                return this.chart;
            }
        }

        public Rectangle ChartRect
        {
            get
            {
                return this.chartRect;
            }
        }
    }
}

