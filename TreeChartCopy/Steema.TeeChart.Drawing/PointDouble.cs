namespace Steema.TeeChart.Drawing
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct PointDouble
    {
        public double X;
        public double Y;
        public PointDouble(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}

