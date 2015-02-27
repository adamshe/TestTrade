namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.Drawing;

    [ToolboxBitmap(typeof(BarJoin), "SeriesIcons.BarJoin.bmp")]
    public class BarJoin : Bar
    {
        private bool firstPoint;
        private ChartPen joinPen;
        private Rectangle OldBarBounds;

        public BarJoin() : this(null)
        {
        }

        public BarJoin(Chart c) : base(c)
        {
        }

        protected internal override void DoBeforeDrawChart()
        {
            base.DoBeforeDrawChart();
            this.firstPoint = true;
        }

        public override void DrawBar(int BarIndex, int StartPos, int EndPos)
        {
            base.DrawBar(BarIndex, StartPos, EndPos);
            if (!this.firstPoint)
            {
                int right;
                int left;
                switch (base.BarStyle)
                {
                    case BarStyles.Pyramid:
                    case BarStyles.Ellipse:
                    case BarStyles.Arrow:
                    case BarStyles.Cone:
                        right = (this.OldBarBounds.Left + this.OldBarBounds.Right) / 2;
                        left = (base.BarBounds.Left + base.BarBounds.Right) / 2;
                        break;

                    default:
                        right = this.OldBarBounds.Right;
                        left = base.BarBounds.Left;
                        break;
                }
                int top = this.OldBarBounds.Top;
                int y = base.BarBounds.Top;
                if (!this.DrawValuesForward())
                {
                    right = base.BarBounds.Right;
                    left = this.OldBarBounds.Left;
                    int num5 = top;
                    top = y;
                    y = num5;
                }
                Graphics3D graphicsd = base.chart.graphics3D;
                graphicsd.Pen = this.JoinPen;
                if (base.chart.Aspect.View3D)
                {
                    graphicsd.MoveTo(right, top, base.MiddleZ);
                    graphicsd.LineTo(left, y, base.MiddleZ);
                }
                else
                {
                    graphicsd.MoveTo(right, top);
                    graphicsd.LineTo(left, y);
                }
            }
            this.firstPoint = false;
            this.OldBarBounds = base.BarBounds;
        }

        protected internal override int NumSampleValues()
        {
            return 3;
        }

        internal override void PrepareForGallery(bool IsEnabled)
        {
            base.PrepareForGallery(IsEnabled);
            this.JoinPen.Color = Color.Blue;
            this.joinPen.Width = 2;
            base.FillSampleValues(2);
        }

        public override string Description
        {
            get
            {
                return Texts.GalleryBarJoin;
            }
        }

        public ChartPen JoinPen
        {
            get
            {
                if (this.joinPen == null)
                {
                    this.joinPen = new ChartPen(base.chart, Color.Blue);
                }
                return this.joinPen;
            }
        }
    }
}

