namespace Steema.TeeChart
{
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;

    [Description("Custom label")]
    public class AxisLabelItem : TextShape
    {
        internal AxisLabelsItems iAxisLabelsItems;
        private double value;

        public AxisLabelItem(Chart c) : base(c)
        {
        }

        public void Repaint()
        {
            this.iAxisLabelsItems.iAxis.Chart.Invalidate();
        }

        private void SetValue(double v)
        {
            if (v != this.value)
            {
                this.value = v;
                this.Repaint();
            }
        }

        public double Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.SetValue(value);
            }
        }
    }
}

