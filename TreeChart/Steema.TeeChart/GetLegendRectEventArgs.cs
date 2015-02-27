namespace Steema.TeeChart
{
    using System;
    using System.Drawing;

    public class GetLegendRectEventArgs : EventArgs
    {
        private System.Drawing.Rectangle r;

        public GetLegendRectEventArgs(System.Drawing.Rectangle rect)
        {
            this.r = rect;
        }

        public System.Drawing.Rectangle Rectangle
        {
            get
            {
                return this.r;
            }
            set
            {
                this.r = value;
            }
        }
    }
}

