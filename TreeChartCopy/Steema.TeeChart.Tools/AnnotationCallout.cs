namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart.Styles;
    using System;
    using System.Drawing;

    public class AnnotationCallout : Callout
    {
        private int x;
        private int y;
        private int z;

        public AnnotationCallout(Series s) : base(s)
        {
            this.x = 0;
            this.y = 0;
            this.z = 0;
            base.Arrow.Visible = false;
            base.Visible = false;
        }

        internal Point CloserPoint(Rectangle R, Point P)
        {
            int right;
            int bottom;
            if (P.X > R.Right)
            {
                right = R.Right;
            }
            else if (P.X < R.Left)
            {
                right = R.Left;
            }
            else
            {
                right = (R.Left + R.Right) / 2;
            }
            if (P.Y > R.Bottom)
            {
                bottom = R.Bottom;
            }
            else if (P.Y < R.Top)
            {
                bottom = R.Top;
            }
            else
            {
                bottom = (R.Top + R.Bottom) / 2;
            }
            return new Point(right, bottom);
        }

        public int XPosition
        {
            get
            {
                return this.x;
            }
            set
            {
                if (this.x != value)
                {
                    this.x = value;
                    this.Invalidate();
                }
            }
        }

        public int YPosition
        {
            get
            {
                return this.y;
            }
            set
            {
                if (this.y != value)
                {
                    this.y = value;
                    this.Invalidate();
                }
            }
        }

        public int ZPosition
        {
            get
            {
                return this.z;
            }
            set
            {
                if (this.z != value)
                {
                    this.z = value;
                    this.Invalidate();
                }
            }
        }
    }
}

