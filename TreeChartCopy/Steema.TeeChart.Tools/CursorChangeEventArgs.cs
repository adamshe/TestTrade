namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart.Styles;
    using System;

    public class CursorChangeEventArgs : EventArgs
    {
        public Steema.TeeChart.Styles.Series Series;
        public int SnapPoint;
        public int ValueIndex;
        public int x;
        public double XValue;
        public int y;
        public double YValue;
    }
}

