namespace Steema.TeeChart.Styles
{
    using System;

    public class MarksCallout : Callout
    {
        private int length;

        public MarksCallout(Series s) : base(s)
        {
            this.length = 8;
            base.Visible = false;
        }

        public int Length
        {
            get
            {
                return this.length;
            }
            set
            {
                if (this.length != value)
                {
                    this.length = value;
                    this.Invalidate();
                }
            }
        }
    }
}

