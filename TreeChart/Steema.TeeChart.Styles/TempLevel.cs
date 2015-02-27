namespace Steema.TeeChart.Styles
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal struct TempLevel
    {
        public double UpToValue;
        public System.Drawing.Color Color;
        public int Count;
        public int Allocated;
        public LevelLine[] Line;
    }
}

