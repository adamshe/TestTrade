namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    [ToolboxBitmap(typeof(Bar), "SeriesIcons.Bar.bmp")]
    public class Bar : CustomBar
    {
        public Bar() : this(null)
        {
        }

        public Bar(Chart c) : base(c)
        {
        }

        protected internal override void CalcHorizMargins(ref int LeftMargin, ref int RightMargin)
        {
            base.CalcHorizMargins(ref LeftMargin, ref RightMargin);
            base.InternalApplyBarMargin(ref LeftMargin, ref RightMargin);
        }

        protected internal override void CalcVerticalMargins(ref int TopMargin, ref int BottomMargin)
        {
            base.CalcVerticalMargins(ref TopMargin, ref BottomMargin);
            int num = base.CalcMarkLength(0);
            if (num > 0)
            {
                num++;
                if (base.bUseOrigin && (base.MinYValue() < base.dOrigin))
                {
                    if (base.GetVertAxis.Inverted)
                    {
                        TopMargin += num;
                    }
                    else
                    {
                        BottomMargin += num;
                    }
                }
                if (!base.bUseOrigin || (base.MaxYValue() > base.dOrigin))
                {
                    if (base.GetVertAxis.Inverted)
                    {
                        BottomMargin += num;
                    }
                    else
                    {
                        TopMargin += num;
                    }
                }
            }
        }

        public override int CalcXPos(int valueIndex)
        {
            int num;
            if (base.iMultiBar == MultiBars.SideAll)
            {
                num = base.GetHorizAxis.CalcXPosValue((double) (base.IPreviousCount + valueIndex)) - (base.IBarSize / 2);
            }
            else if (base.iMultiBar == MultiBars.SelfStack)
            {
                num = base.CalcXPosValue(this.MinXValue()) - (base.IBarSize / 2);
            }
            else
            {
                num = base.CalcXPos(valueIndex);
                if (base.iMultiBar != MultiBars.None)
                {
                    num += Utils.Round((double) (base.IBarSize * ((base.IOrderPos - (base.INumBars * 0.5)) - 1.0)));
                }
                else
                {
                    num -= base.IBarSize / 2;
                }
            }
            return base.ApplyBarOffset(num);
        }

        public override int CalcYPos(int valueIndex)
        {
            switch (base.iMultiBar)
            {
                case MultiBars.None:
                case MultiBars.Side:
                case MultiBars.SideAll:
                    return base.CalcYPos(valueIndex);
            }
            double num2 = base.vyValues[valueIndex] + this.PointOrigin(valueIndex, false);
            if ((base.iMultiBar == MultiBars.Stacked) || (base.iMultiBar == MultiBars.SelfStack))
            {
                return base.CalcYPosValue(num2);
            }
            double num3 = this.PointOrigin(valueIndex, true);
            return ((num3 != 0.0) ? base.CalcYPosValue((num2 * 100.0) / num3) : 0);
        }

        public virtual void DrawBar(int barIndex, int startPos, int endPos)
        {
            base.SetPenBrushBar(base.NormalBarColor);
            int barBoundsMidX = base.BarBoundsMidX;
            BarStyles styles = base.DoGetBarStyle(barIndex);
            Graphics3D graphicsd = base.chart.graphics3D;
            Rectangle iBarBounds = base.iBarBounds;
            if (base.chart.Aspect.View3D)
            {
                switch (styles)
                {
                    case BarStyles.Rectangle:
                        graphicsd.Cube(iBarBounds.X, startPos, iBarBounds.Right, endPos, base.StartZ, base.EndZ, base.bDark3D);
                        return;

                    case BarStyles.Pyramid:
                        graphicsd.Pyramid(true, iBarBounds.X, startPos, iBarBounds.Right, endPos, base.StartZ, base.EndZ, base.bDark3D);
                        return;

                    case BarStyles.InvPyramid:
                        graphicsd.Pyramid(true, iBarBounds.X, endPos, iBarBounds.Right, startPos, base.StartZ, base.EndZ, base.bDark3D);
                        return;

                    case BarStyles.Cylinder:
                        graphicsd.Cylinder(true, base.iBarBounds, base.StartZ, base.EndZ, base.bDark3D);
                        return;

                    case BarStyles.Ellipse:
                        graphicsd.Ellipse(base.iBarBounds, base.MiddleZ);
                        return;

                    case BarStyles.Arrow:
                        graphicsd.Arrow(true, new Point(barBoundsMidX, endPos), new Point(barBoundsMidX, startPos), iBarBounds.Width, iBarBounds.Width / 2, base.MiddleZ);
                        return;

                    case BarStyles.RectGradient:
                        graphicsd.Cube(iBarBounds.X, startPos, iBarBounds.Right, endPos, base.StartZ, base.EndZ, base.bDark3D);
                        if (graphicsd.SupportsFullRotation || base.chart.Aspect.Orthogonal)
                        {
                            base.DoGradient3D(barIndex, graphicsd.Calc3DPoint(iBarBounds.X, startPos, base.StartZ), graphicsd.Calc3DPoint(iBarBounds.Right, endPos, base.StartZ));
                        }
                        return;

                    case BarStyles.Cone:
                        graphicsd.Cone(true, base.iBarBounds, base.StartZ, base.EndZ, base.bDark3D, base.conePercent);
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
                        pointArray = new Point[] { new Point(iBarBounds.X, endPos), new Point(barBoundsMidX, startPos), new Point(iBarBounds.Right, endPos) };
                        graphicsd.Polygon(pointArray);
                        return;

                    case BarStyles.InvPyramid:
                        pointArray = new Point[] { new Point(iBarBounds.X, startPos), new Point(barBoundsMidX, endPos), new Point(iBarBounds.Right, startPos) };
                        graphicsd.Polygon(pointArray);
                        return;

                    case BarStyles.Ellipse:
                        graphicsd.Ellipse(base.iBarBounds);
                        return;

                    case BarStyles.Arrow:
                        graphicsd.Arrow(true, new Point(barBoundsMidX, endPos), new Point(barBoundsMidX, startPos), iBarBounds.Width, iBarBounds.Width / 2, base.MiddleZ);
                        return;

                    case BarStyles.RectGradient:
                        base.DoBarGradient(barIndex, new Rectangle(iBarBounds.X, startPos, iBarBounds.Right - iBarBounds.X, endPos - startPos));
                        return;

                    default:
                        return;
                }
            }
        }

        protected internal override void DrawMark(int valueIndex, string s, SeriesMarks.Position p)
        {
            int num = base.IBarSize / 2;
            int num2 = base.Marks.Callout.Length + base.Marks.Callout.Distance;
            if (p.ArrowFrom.Y > this.GetOriginPos(valueIndex))
            {
                num2 = -num2 - p.Height;
                p.ArrowFrom.Y += base.Marks.Callout.Distance;
            }
            else
            {
                p.ArrowFrom.Y -= base.Marks.Callout.Distance;
            }
            p.LeftTop.X += num;
            p.LeftTop.Y -= num2;
            p.ArrowTo.X += num;
            p.ArrowTo.Y -= num2;
            p.ArrowFrom.X += num;
            if (base.AutoMarkPosition)
            {
                base.Marks.AntiOverlap(base.firstVisible, valueIndex, p);
            }
            base.DrawMark(valueIndex, s, p);
        }

        protected override bool DrawSeriesForward(int valueIndex)
        {
            switch (base.iMultiBar)
            {
                case MultiBars.None:
                case MultiBars.Side:
                case MultiBars.SideAll:
                    return true;

                case MultiBars.Stacked:
                case MultiBars.SelfStack:
                {
                    bool flag = base.vyValues[valueIndex] >= base.dOrigin;
                    if (base.GetVertAxis.Inverted)
                    {
                        flag = !flag;
                    }
                    return flag;
                }
            }
            return !base.GetVertAxis.Inverted;
        }

        public override void DrawValue(int valueIndex)
        {
            base.DrawValue(valueIndex);
            base.NormalBarColor = this.ValueColor(valueIndex);
            if (base.NormalBarColor != Color.Empty)
            {
                Rectangle rectangle = new Rectangle(0, 0, 0, 0);
                rectangle.X = this.CalcXPos(valueIndex);
                if ((base.barSizePercent == 100) && (valueIndex < (base.Count - 1)))
                {
                    rectangle.Width = this.CalcXPos(valueIndex + 1) - rectangle.X;
                }
                else
                {
                    rectangle.Width = base.IBarSize + 1;
                }
                rectangle.Y = this.CalcYPos(valueIndex);
                rectangle.Height = this.GetOriginPos(valueIndex) - rectangle.Y;
                if (!base.Pen.Visible)
                {
                    if (rectangle.Bottom > rectangle.Y)
                    {
                        rectangle.Height++;
                    }
                    else
                    {
                        rectangle.Y++;
                    }
                    rectangle.Width++;
                }
                base.iBarBounds = rectangle;
                if (rectangle.Bottom > rectangle.Y)
                {
                    this.DrawBar(valueIndex, rectangle.Y, rectangle.Bottom);
                }
                else
                {
                    this.DrawBar(valueIndex, rectangle.Bottom, rectangle.Y);
                }
            }
        }

        public int GetOriginPos(int valueIndex)
        {
            return base.InternalGetOriginPos(valueIndex, base.GetVertAxis.IEndPos);
        }

        protected override int InternalCalcMarkLength(int valueIndex)
        {
            return base.chart.graphics3D.FontHeight;
        }

        protected override bool InternalClicked(int valueIndex, Point point)
        {
            bool flag = false;
            int num = this.CalcXPos(valueIndex);
            if ((point.X < num) || (point.X > (num + base.IBarSize)))
            {
                return flag;
            }
            int num2 = this.CalcYPos(valueIndex);
            int originPos = this.GetOriginPos(valueIndex);
            if (originPos < num2)
            {
                int num4 = originPos;
                originPos = num2;
                num2 = num4;
            }
            switch (base.BarStyle)
            {
                case BarStyles.Pyramid:
                case BarStyles.Cone:
                    return Graphics3D.PointInTriangle(point, num, num + base.IBarSize, originPos, num2);

                case BarStyles.InvPyramid:
                    return Graphics3D.PointInTriangle(point, num, num + base.IBarSize, num2, originPos);

                case BarStyles.Ellipse:
                    return Graphics3D.PointInEllipse(point, new Rectangle(num, num2, num + base.IBarSize, originPos));
            }
            return ((point.Y >= num2) && (point.Y <= originPos));
        }

        public override double MaxXValue()
        {
            if (base.iMultiBar == MultiBars.SelfStack)
            {
                return this.MinXValue();
            }
            if (base.iMultiBar != MultiBars.SideAll)
            {
                return base.MaxXValue();
            }
            return (double) ((base.IPreviousCount + base.Count) - 1);
        }

        public override double MaxYValue()
        {
            return base.MaxMandatoryValue(base.MaxYValue());
        }

        public override double MinXValue()
        {
            if (base.iMultiBar == MultiBars.SelfStack)
            {
                return (double) base.Chart.Series.IndexOf(this);
            }
            return base.MinXValue();
        }

        public override double MinYValue()
        {
            return base.MinMandatoryValue(base.MinYValue());
        }

        protected override bool MoreSameZOrder()
        {
            return ((base.iMultiBar != MultiBars.SideAll) && base.MoreSameZOrder());
        }

        [Description("Determines the percent of total Bar width used."), DefaultValue(70)]
        public int BarWidthPercent
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

        public override string Description
        {
            get
            {
                return Texts.GalleryBar;
            }
        }
    }
}

