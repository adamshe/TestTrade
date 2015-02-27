namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart.Drawing;
    using System;
    using System.Drawing;

    public class Callout : SeriesPointer
    {
        private ChartPen arrow;
        private ArrowHeadStyles arrowHead;
        private int arrowHeadSize;
        private int distance;
        private const bool TeeCheckMarkArrowColor = false;

        public Callout() : this(null)
        {
        }

        public Callout(Series s) : base((s != null) ? s.chart : null, s)
        {
            this.arrowHead = ArrowHeadStyles.None;
            this.distance = 0;
            this.arrowHeadSize = 8;
            base.Style = PointerStyles.Rectangle;
            base.Color = Color.Black;
            base.Draw3D = false;
            base.Visible = true;
        }

        protected internal void Draw(Color c, Point pFrom, Point pTo, int z)
        {
            Graphics3D g = base.chart.Graphics3D;
            if (this.Arrow.Visible)
            {
                base.PrepareCanvas(g, base.Color);
                g.Pen = this.Arrow;
                switch (this.ArrowHead)
                {
                    case ArrowHeadStyles.None:
                        if (!base.Chart.aspect.view3D)
                        {
                            g.Line(pFrom, pTo);
                            break;
                        }
                        g.MoveTo(pFrom, z);
                        g.LineTo(pTo, z);
                        break;

                    case ArrowHeadStyles.Line:
                        g.Arrow(false, pFrom, pTo, this.ArrowHeadSize, this.ArrowHeadSize, z);
                        break;

                    case ArrowHeadStyles.Solid:
                        g.Arrow(true, pFrom, pTo, this.ArrowHeadSize, this.ArrowHeadSize, z);
                        break;
                }
            }
            if ((this.ArrowHead == ArrowHeadStyles.None) && base.Visible)
            {
                base.PrepareCanvas(g, base.Color);
                Point point = pFrom;
                if (base.Chart.aspect.View3D)
                {
                    point = g.Calc3DPoint(pFrom, z);
                }
                base.Draw(g, base.Chart.aspect.View3D, point.X, point.Y, base.HorizSize, base.VertSize, base.Color, base.Style);
            }
        }

        public ChartPen Arrow
        {
            get
            {
                if (this.arrow == null)
                {
                    this.arrow = new ChartPen(base.chart, Color.White);
                }
                return this.arrow;
            }
            set
            {
                this.Arrow.Assign(value);
            }
        }

        public ArrowHeadStyles ArrowHead
        {
            get
            {
                return this.arrowHead;
            }
            set
            {
                if (this.arrowHead != value)
                {
                    this.arrowHead = value;
                    this.Invalidate();
                }
            }
        }

        public int ArrowHeadSize
        {
            get
            {
                return this.arrowHeadSize;
            }
            set
            {
                if (this.arrowHeadSize != value)
                {
                    this.arrowHeadSize = value;
                    this.Invalidate();
                }
            }
        }

        public int Distance
        {
            get
            {
                return this.distance;
            }
            set
            {
                if (this.distance != value)
                {
                    this.distance = value;
                    this.Invalidate();
                }
            }
        }
    }
}

