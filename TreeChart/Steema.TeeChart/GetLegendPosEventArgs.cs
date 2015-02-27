namespace Steema.TeeChart
{
    using System;

    public class GetLegendPosEventArgs : EventArgs
    {
        private readonly int idx;
        private int x;
        private int xColor;
        private int y;

        public GetLegendPosEventArgs(int index, int x, int y, int xColor)
        {
            this.idx = index;
            this.x = x;
            this.y = y;
            this.xColor = xColor;
        }

        public int Index
        {
            get
            {
                return this.idx;
            }
        }

        public int X
        {
            get
            {
                return this.x;
            }
            set
            {
                this.x = value;
            }
        }

        public int XColor
        {
            get
            {
                return this.xColor;
            }
            set
            {
                this.xColor = value;
            }
        }

        public int Y
        {
            get
            {
                return this.y;
            }
            set
            {
                this.y = value;
            }
        }
    }
}

