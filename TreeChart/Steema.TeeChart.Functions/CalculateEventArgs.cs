namespace Steema.TeeChart.Functions
{
    using System;
    using System.ComponentModel;

    public class CalculateEventArgs : EventArgs
    {
        internal double x;
        public double Y;

        public CalculateEventArgs(double x, double y)
        {
            this.x = x;
            this.Y = y;
        }

        [Description("Returns the current X parameter.")]
        public double X
        {
            get
            {
                return this.x;
            }
        }
    }
}

