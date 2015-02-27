namespace Steema.TeeChart.Styles
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct GridPalette
    {
        public double UpToValue;
        public System.Drawing.Color Color;
        public GridPalette(double aValue, System.Drawing.Color aColor)
        {
            this.UpToValue = aValue;
            this.Color = aColor;
        }
    }
}

