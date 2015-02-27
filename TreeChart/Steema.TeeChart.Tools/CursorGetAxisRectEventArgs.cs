namespace Steema.TeeChart.Tools
{
    using System;
    using System.Drawing;

    public class CursorGetAxisRectEventArgs : EventArgs
    {
        private System.Drawing.Rectangle r;

        public CursorGetAxisRectEventArgs(System.Drawing.Rectangle rect)
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

