namespace Steema.TeeChart
{
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;

    public class ZoomScroll : TeeBase
    {
        private bool active;
        public int x0;
        public int x1;
        public int y0;
        public int y1;

        public ZoomScroll(Chart c) : base(c)
        {
        }

        public void Activate(int x, int y)
        {
            this.x0 = x;
            this.y0 = y;
            this.x1 = x;
            this.y1 = y;
            this.active = true;
        }

        public void Check()
        {
            int num;
            if (this.x0 > this.x1)
            {
                num = this.x0;
                this.x0 = this.x1;
                this.x1 = num;
            }
            if (this.y0 > this.y1)
            {
                num = this.y0;
                this.y0 = this.y1;
                this.y1 = num;
            }
        }

        [DefaultValue(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), Description("Returns the active state of Chart Zoom and Scroll.")]
        public bool Active
        {
            get
            {
                return this.active;
            }
            set
            {
                this.active = value;
            }
        }
    }
}

