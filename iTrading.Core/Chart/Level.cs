using Steema.TeeChart.Drawing;

namespace iTrading.Core.Chart
{
    /// <summary>
    /// A horizontal line on a chart.
    /// </summary>
    public class Level
    {
        private ChartPen pen;
        private double val;

        /// <summary>
        /// </summary>
        /// <param name="pen">Pen to draw the chart level</param>
        /// <param name="levelValue">Value at which the level is painted</param>
        public Level(ChartPen pen, double levelValue)
        {
            this.pen = pen;
            this.val = levelValue;
        }

        /// <summary>
        /// Get the pen to draw the chart level.
        /// </summary>
        public ChartPen Pen
        {
            get
            {
                return this.pen;
            }
        }

        /// <summary>
        /// Get the value at which the level is painted.
        /// </summary>
        public double Value
        {
            get
            {
                return this.val;
            }
        }
    }
}