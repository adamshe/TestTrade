namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;

    [ToolboxBitmap(typeof(HorizBar), "SeriesIcons.HorizBar.bmp")]
    public class HorizBar : CustomBar
    {
        public HorizBar() : this(null)
        {
        }

        public HorizBar(Chart c) : base(c)
        {
            base.SetHorizontal();
            base.notMandatory.Order = ValueListOrder.Ascending;
            base.mandatory.Order = ValueListOrder.None;
            base.Gradient.Direction = LinearGradientMode.Horizontal;
        }

        protected internal override void CalcHorizMargins(ref int leftMargin, ref int rightMargin)
        {
            base.CalcHorizMargins(ref leftMargin, ref rightMargin);
            int num = base.CalcMarkLength(-1);
            if (num > 0)
            {
                num++;
            }
            if (base.bUseOrigin && (base.MinXValue() < base.dOrigin))
            {
                leftMargin += num;
            }
            if (!base.bUseOrigin || (base.MaxXValue() > base.dOrigin))
            {
                if (base.GetHorizAxis.Inverted)
                {
                    leftMargin += num;
                }
                else
                {
                    rightMargin += num;
                }
            }
        }

        protected internal override void CalcVerticalMargins(ref int topMargin, ref int bottomMargin)
        {
            base.CalcVerticalMargins(ref topMargin, ref bottomMargin);
            base.InternalApplyBarMargin(ref topMargin, ref bottomMargin);
        }

        public override int CalcXPos(int valueIndex)
        {
            if (((base.iMultiBar == MultiBars.None) || (base.iMultiBar == MultiBars.Side)) || (base.iMultiBar == MultiBars.SideAll))
            {
                return base.CalcXPos(valueIndex);
            }
            double num = base.vxValues[valueIndex] + this.PointOrigin(valueIndex, false);
            if ((base.iMultiBar == MultiBars.Stacked) || (base.iMultiBar == MultiBars.SelfStack))
            {
                return base.CalcXPosValue(num);
            }
            double num2 = this.PointOrigin(valueIndex, true);
            if (num2 == 0.0)
            {
                return 0;
            }
            return base.CalcXPosValue((num * 100.0) / num2);
        }

        public override int CalcYPos(int valueIndex)
        {
            int num;
            if (base.iMultiBar == MultiBars.SideAll)
            {
                num = base.GetVertAxis.CalcYPosValue((double) (base.IPreviousCount + valueIndex)) - (base.IBarSize / 2);
            }
            else if (base.iMultiBar == MultiBars.SelfStack)
            {
                num = base.CalcYPosValue(this.MinYValue()) - (base.IBarSize / 2);
            }
            else
            {
                num = base.CalcYPos(valueIndex);
                if (base.iMultiBar != MultiBars.None)
                {
                    num += Utils.Round((double) (base.IBarSize * ((base.INumBars * 0.5) - ((1 + base.INumBars) - base.IOrderPos))));
                }
                else
                {
                    num -= base.IBarSize / 2;
                }
            }
            return base.ApplyBarOffset(num);
        }

        private void DrawBar(int barIndex, int startPos, int endPos)
        {
            Graphics3D graphicsd = base.chart.graphics3D;
            base.SetPenBrushBar(base.NormalBarColor);
            Rectangle iBarBounds = base.iBarBounds;
            int y = (iBarBounds.Y + iBarBounds.Bottom) / 2;
            BarStyles styles = base.DoGetBarStyle(barIndex);
            if (base.chart.Aspect.View3D)
            {
                switch (styles)
                {
                    case BarStyles.Rectangle:
                        graphicsd.Cube(startPos, iBarBounds.Y, endPos, iBarBounds.Bottom, base.StartZ, base.EndZ, base.bDark3D);
                        return;

                    case BarStyles.Pyramid:
                        graphicsd.Pyramid(false, startPos, iBarBounds.Y, endPos, iBarBounds.Bottom, base.StartZ, base.EndZ, base.bDark3D);
                        return;

                    case BarStyles.InvPyramid:
                        graphicsd.Pyramid(false, endPos, iBarBounds.Y, startPos, iBarBounds.Bottom, base.StartZ, base.EndZ, base.bDark3D);
                        return;

                    case BarStyles.Cylinder:
                        graphicsd.Cylinder(false, iBarBounds, base.StartZ, base.EndZ, base.bDark3D);
                        return;

                    case BarStyles.Ellipse:
                        graphicsd.Ellipse(base.iBarBounds, base.MiddleZ);
                        return;

                    case BarStyles.Arrow:
                        graphicsd.Arrow(true, new Point(startPos, y), new Point(endPos, y), iBarBounds.Height, iBarBounds.Height / 2, base.MiddleZ);
                        return;

                    case BarStyles.RectGradient:
                        graphicsd.Cube(startPos, iBarBounds.Y, endPos, iBarBounds.Bottom, base.StartZ, base.EndZ, base.bDark3D);
                        if (graphicsd.SupportsFullRotation || base.chart.Aspect.Orthogonal)
                        {
                            base.DoGradient3D(barIndex, graphicsd.Calc3DPoint(startPos, iBarBounds.Y, base.StartZ), graphicsd.Calc3DPoint(endPos, iBarBounds.Bottom, base.StartZ));
                        }
                        return;

                    case BarStyles.Cone:
                        graphicsd.Cone(false, base.iBarBounds, base.StartZ, base.EndZ, base.bDark3D, base.conePercent);
                        return;
                }
            }
            else
            {
                Point[] pointArray;
                switch (styles)
                {
                    case BarStyles.Rectangle:
                    case BarStyles.Cylinder:
                        base.BarRectangle(base.NormalBarColor, base.iBarBounds);
                        return;

                    case BarStyles.Pyramid:
                    case BarStyles.Cone:
                        pointArray = new Point[] { new Point(startPos, iBarBounds.Y), new Point(endPos, y), new Point(startPos, iBarBounds.Bottom) };
                        graphicsd.Polygon(pointArray);
                        return;

                    case BarStyles.InvPyramid:
                        pointArray = new Point[] { new Point(endPos, iBarBounds.Y), new Point(startPos, y), new Point(endPos, iBarBounds.Bottom) };
                        graphicsd.Polygon(pointArray);
                        return;

                    case BarStyles.Ellipse:
                        graphicsd.Ellipse(base.iBarBounds);
                        return;

                    case BarStyles.Arrow:
                        graphicsd.Arrow(true, new Point(startPos, y), new Point(endPos, y), iBarBounds.Height, iBarBounds.Height / 2, base.MiddleZ);
                        return;

                    case BarStyles.RectGradient:
                        base.DoBarGradient(barIndex, new Rectangle(startPos, iBarBounds.Y, endPos - startPos, iBarBounds.Height));
                        return;

                    default:
                        return;
                }
            }
        }

        protected internal override void DrawMark(int valueIndex, string s, SeriesMarks.Position position)
        {
            int num = base.IBarSize / 2;
            int num2 = base.Marks.Callout.Length + base.Marks.Callout.Distance;
            bool flag = position.ArrowFrom.X < this.GetOriginPos(valueIndex);
            if (flag)
            {
                num2 = -num2 - position.Width;
            }
            position.LeftTop.X += num2 + (position.Width / 2);
            position.LeftTop.Y += num + (position.Height / 2);
            position.ArrowTo.X += num2;
            position.ArrowTo.Y += num;
            position.ArrowFrom.Y += num;
            if (flag)
            {
                position.ArrowFrom.X -= base.Marks.Callout.Distance;
            }
            else
            {
                position.ArrowFrom.X += base.Marks.Callout.Distance;
            }
            base.DrawMark(valueIndex, s, position);
        }

        protected override bool DrawSeriesForward(int valueIndex)
        {
            switch (base.iMultiBar)
            {
                case MultiBars.None:
                    return true;

                case MultiBars.Side:
                case MultiBars.SideAll:
                    return false;

                case MultiBars.Stacked:
                case MultiBars.SelfStack:
                {
                    bool flag = base.mandatory[valueIndex] >= 0.0;
                    if (base.GetHorizAxis.Inverted)
                    {
                        flag = !flag;
                    }
                    return flag;
                }
            }
            return !base.GetHorizAxis.Inverted;
        }

        public override void DrawValue(int valueIndex)
        {
            base.DrawValue(valueIndex);
            base.NormalBarColor = this.ValueColor(valueIndex);
            Rectangle rectangle = new Rectangle(0, 0, 0, 0);
            if (base.NormalBarColor != Color.Empty)
            {
                rectangle.Y = this.CalcYPos(valueIndex);
            }
            if ((base.barSizePercent == 100) && (valueIndex > 0))
            {
                rectangle.Height = this.CalcYPos(valueIndex - 1) - rectangle.Y;
            }
            else
            {
                rectangle.Height = base.IBarSize + 1;
            }
            rectangle.X = this.GetOriginPos(valueIndex);
            rectangle.Width = this.CalcXPos(valueIndex) - rectangle.X;
            if (!base.Pen.Visible)
            {
                if (rectangle.Right > rectangle.X)
                {
                    rectangle.Width++;
                }
                else
                {
                    rectangle.X++;
                }
                rectangle.Height++;
            }
            base.iBarBounds = rectangle;
            if (rectangle.Right > rectangle.X)
            {
                this.DrawBar(valueIndex, rectangle.X, rectangle.Right);
            }
            else
            {
                this.DrawBar(valueIndex, rectangle.Right, rectangle.X);
            }
        }

        protected virtual int GetOriginPos(int valueIndex)
        {
            return base.InternalGetOriginPos(valueIndex, base.GetHorizAxis.IStartPos);
        }

        protected override int InternalCalcMarkLength(int valueIndex)
        {
            if (valueIndex == -1)
            {
                return base.MaxMarkWidth();
            }
            return base.Marks.TextWidth(valueIndex);
        }

        protected override bool InternalClicked(int valueIndex, Point point)
        {
            int tmpY = this.CalcYPos(valueIndex);
            if (!base.chart.Aspect.View3D && ((point.Y < tmpY) || (point.Y > (tmpY + base.IBarSize))))
            {
                return false;
            }
            int num2 = this.CalcXPos(valueIndex);
            int originPos = this.GetOriginPos(valueIndex);
            if (originPos < num2)
            {
                int num4 = num2;
                num2 = originPos;
                originPos = num4;
            }
            switch (base.BarStyle)
            {
                case BarStyles.Pyramid:
                case BarStyles.Cone:
                    return this.InTriangle(num2, originPos, tmpY, point);

                case BarStyles.InvPyramid:
                    return this.InTriangle(originPos, num2, tmpY, point);
            }
            if (base.chart.Aspect.View3D)
            {
                int x = point.X;
                int y = point.Y;
                base.chart.graphics3D.Calculate2DPosition(ref x, ref y, base.StartZ);
                point.X = x;
                point.Y = y;
                return (((point.Y >= tmpY) && (point.Y <= (tmpY + base.IBarSize))) && this.OtherClicked(point, num2, originPos, tmpY));
            }
            return this.OtherClicked(point, num2, originPos, tmpY);
        }

        private bool InTriangle(int x1, int x2, int tmpY, Point p)
        {
            Point[] poly = new Point[3];
            Graphics3D graphicsd = base.chart.graphics3D;
            if (base.chart.Aspect.View3D)
            {
                poly[0] = graphicsd.Calc3DPoint(x1, tmpY, base.StartZ);
                poly[1] = graphicsd.Calc3DPoint(x2, tmpY + (base.IBarSize / 2), base.MiddleZ);
                poly[2] = graphicsd.Calc3DPoint(x1, tmpY + base.IBarSize, base.StartZ);
                return Graphics3D.PointInPolygon(p, poly);
            }
            return Graphics3D.PointInHorizTriangle(p, tmpY, tmpY + base.IBarSize, x1, x2);
        }

        public override double MaxXValue()
        {
            return base.MaxMandatoryValue(base.MaxXValue());
        }

        public override double MaxYValue()
        {
            if (base.iMultiBar == MultiBars.SelfStack)
            {
                return this.MinYValue();
            }
            if (base.iMultiBar != MultiBars.SideAll)
            {
                return base.MaxYValue();
            }
            return (double) ((base.IPreviousCount + base.Count) - 1);
        }

        public override double MinXValue()
        {
            return base.MinMandatoryValue(base.MinXValue());
        }

        public override double MinYValue()
        {
            if (base.iMultiBar == MultiBars.SelfStack)
            {
                return (double) base.Chart.Series.IndexOf(this);
            }
            return base.MinYValue();
        }

        private bool OtherClicked(Point p, int tmpX, int endX, int tmpY)
        {
            if (base.BarStyle == BarStyles.Ellipse)
            {
                return Graphics3D.PointInEllipse(p, tmpX, tmpY, endX, tmpY + base.IBarSize);
            }
            return ((p.X >= tmpX) && (p.X <= endX));
        }

        [Description("Defines the percent of bar Height"), DefaultValue(70)]
        public int BarHeightPercent
        {
            get
            {
                return base.barSizePercent;
            }
            set
            {
                base.SetBarSizePercent(value);
            }
        }

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.GalleryHorizBar;
            }
        }
    }
}

