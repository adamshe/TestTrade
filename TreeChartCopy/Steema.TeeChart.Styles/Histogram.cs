namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    [ToolboxBitmap(typeof(Histogram), "SeriesIcons.Histogram.bmp")]
    public class Histogram : BaseLine
    {
        private ChartPen linesPen;
        private int previous;

        public Histogram() : this(null)
        {
        }

        public Histogram(Chart c) : base(c)
        {
            base.calcVisiblePoints = false;
            base.pLinePen.Color = Color.Black;
        }

        protected internal override void CalcHorizMargins(ref int LeftMargin, ref int RightMargin)
        {
            base.CalcHorizMargins(ref LeftMargin, ref RightMargin);
            int num = this.VisiblePoints();
            if (num > 0)
            {
                num = (base.GetHorizAxis.IAxisSize / this.VisiblePoints()) / 2;
            }
            LeftMargin += num;
            RightMargin += num;
            if (base.LinePen.Visible)
            {
                RightMargin += base.LinePen.Width;
            }
        }

        protected internal override void CalcVerticalMargins(ref int TopMargin, ref int BottomMargin)
        {
            base.CalcVerticalMargins(ref TopMargin, ref BottomMargin);
            if (base.LinePen.Visible)
            {
                TopMargin += base.LinePen.Width;
            }
        }

        protected internal override void CreateSubGallery(Series.SubGalleryEventHandler AddSubChart)
        {
            base.CreateSubGallery(AddSubChart);
            AddSubChart(Texts.Hollow);
            AddSubChart(Texts.NoBorder);
            AddSubChart(Texts.Lines);
            AddSubChart(Texts.Transparency);
        }

        public override void DrawValue(int valueIndex)
        {
            Rectangle r = new Rectangle();
            int num = (base.GetHorizAxis.IAxisSize / this.VisiblePoints()) / 2;
            if (valueIndex == base.firstVisible)
            {
                r.X = this.CalcXPos(valueIndex) - num;
                r.Width = 2 * num;
            }
            else
            {
                r.X = this.previous;
                r.Width = (this.CalcXPos(valueIndex) + num) - r.X;
            }
            this.previous = r.Right;
            r.Y = this.CalcYPos(valueIndex);
            r.Height = base.GetVertAxis.Inverted ? (base.GetVertAxis.IStartPos - r.Y) : (base.GetVertAxis.IEndPos - r.Y);
            Graphics3D graphicsd = base.chart.graphics3D;
            graphicsd.Pen.Visible = false;
            if (this.Brush.Visible)
            {
                graphicsd.Brush = this.Brush;
                graphicsd.Brush.Color = this.ValueColor(valueIndex);
                if (base.GetVertAxis.Inverted)
                {
                    r.Y++;
                }
                if (base.chart.aspect.View3D)
                {
                    graphicsd.Rectangle(r.X, r.Y, r.Right - 1, r.Bottom, base.MiddleZ);
                }
                else
                {
                    graphicsd.Rectangle(r);
                }
                if (base.GetVertAxis.Inverted)
                {
                    r.Y--;
                }
            }
            if (base.LinePen.Visible)
            {
                graphicsd.Pen = base.LinePen;
                if (valueIndex == base.firstVisible)
                {
                    this.VerticalLine(r.X, r.Bottom, r.Y);
                }
                else
                {
                    this.VerticalLine(r.X, r.Y, this.CalcYPos(valueIndex - 1));
                }
                this.HorizLine(r.X, r.Right, r.Y);
                if (valueIndex == base.lastVisible)
                {
                    this.VerticalLine(r.Right - 1, r.Y, r.Bottom);
                }
            }
            if (((valueIndex > base.firstVisible) && (this.linesPen != null)) && this.linesPen.Visible)
            {
                num = this.CalcYPos(valueIndex - 1);
                num = base.GetVertAxis.Inverted ? Math.Min(r.Y, num) : Math.Max(r.Y, num);
                if (!base.LinePen.Visible)
                {
                    num--;
                }
                graphicsd.Pen = this.linesPen;
                this.VerticalLine(r.X, r.Bottom, num);
            }
        }

        private void HorizLine(int X0, int X1, int Y)
        {
            if (base.chart.aspect.view3D)
            {
                base.chart.graphics3D.HorizontalLine(X0, X1, Y, base.MiddleZ);
            }
            else
            {
                base.chart.graphics3D.HorizontalLine(X0, X1, Y);
            }
        }

        protected override void SetChart(Chart c)
        {
            base.SetChart(c);
            if (this.linesPen != null)
            {
                this.linesPen.Chart = c;
            }
        }

        protected internal override void SetSubGallery(int index)
        {
            switch (index)
            {
                case 1:
                    this.Brush.Visible = false;
                    return;

                case 2:
                    base.LinePen.Visible = false;
                    return;

                case 3:
                    this.LinesPen.Visible = true;
                    return;

                case 4:
                    this.Transparency = 30;
                    return;
            }
            base.SetSubGallery(index);
        }

        private void VerticalLine(int X, int Y0, int Y1)
        {
            if (base.chart.aspect.view3D)
            {
                base.chart.graphics3D.VerticalLine(X, Y0, Y1, base.MiddleZ);
            }
            else
            {
                base.chart.graphics3D.VerticalLine(X, Y0, Y1);
            }
        }

        private int VisiblePoints()
        {
            int maxPointsPerPage = base.chart.Page.MaxPointsPerPage;
            if (maxPointsPerPage != 0)
            {
                return maxPointsPerPage;
            }
            return base.Count;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance")]
        public ChartBrush Brush
        {
            get
            {
                return base.bBrush;
            }
        }

        public override string Description
        {
            get
            {
                return Texts.GalleryHistogram;
            }
        }

        [Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ChartPen LinesPen
        {
            get
            {
                if (this.linesPen == null)
                {
                    this.linesPen = new ChartPen(base.chart, Color.Black);
                }
                return this.linesPen;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Category("Appearance"), Description("Sets Transparency level from 0 to 100%."), DefaultValue(0)]
        public int Transparency
        {
            get
            {
                return base.bBrush.Transparency;
            }
            set
            {
                base.bBrush.Transparency = value;
            }
        }
    }
}

