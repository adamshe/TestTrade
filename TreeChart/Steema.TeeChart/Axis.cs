namespace Steema.TeeChart
{
    using Steema.TeeChart.Drawing;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Drawing;
    using System.Drawing.Design;
    using System.Drawing.Drawing2D;
    using System.Globalization;
    using System.Runtime.InteropServices;

    [ToolboxBitmap(typeof(Axis), "Images.Axis.bmp"), Description("Axis properties."), Designer(typeof(Designer)), Editor(typeof(Editor), typeof(UITypeEditor))]
    public class Axis : TeeBase
    {
        private bool automatic;
        private bool automaticMaximum;
        private bool automaticMinimum;
        public static int AxisClickGap = 3;
        internal AxisLinePen axispen;
        private AxisTitle axisTitle;
        protected bool bVisible;
        private double desiredIncrement;
        private double endPosition;
        protected internal AxisDraw FAxisDraw;
        private GridPen grid;
        internal bool horizontal;
        internal bool iAxisDateTime;
        private double iAxisLogSizeRange;
        public int IAxisSize;
        private double iAxisSizeRange;
        private int iCenterPos;
        public int IEndPos;
        protected internal bool iHideBackGrid;
        private double iLogMax;
        private double iLogMin;
        private double iMaximum;
        private double iMinAxisIncrement;
        private double iMinimum;
        internal bool inverted;
        internal double iRange;
        private double iRangelog;
        private bool iRangezero;
        public bool IsDepthAxis;
        private SeriesCollection iSeriesList;
        public int IStartPos;
        private int iZPos;
        private AxisLabels labels;
        private bool logarithmic;
        private double logarithmicBase;
        private const int MaxAxisTicks = 0x7d0;
        private int maximumOffset;
        private double maximumValue;
        private const int MaxPixelPos = 0x7fff;
        private const double minAxisRange = 1E-10;
        private int minimumOffset;
        private double minimumValue;
        private ChartPen minorGrid;
        private int minorTickCount;
        private TicksPen minorTicks;
        private bool otherSide;
        private int posAxis;
        private Steema.TeeChart.PositionUnits positionUnits;
        private int posTitle;
        private double relativePosition;
        private double startPosition;
        private bool tickonlabelsonly;
        private TicksPen ticks;
        private TicksPen ticksInner;
        private double zPosition;

        public Axis() : this((Chart) null)
        {
        }

        public Axis(Chart c) : base(c)
        {
            this.iMinAxisIncrement = 1E-12;
            this.automatic = true;
            this.automaticMaximum = true;
            this.automaticMinimum = true;
            this.logarithmicBase = 10.0;
            this.minorTickCount = 3;
            this.tickonlabelsonly = true;
            this.bVisible = true;
            this.endPosition = 100.0;
            this.positionUnits = Steema.TeeChart.PositionUnits.Percent;
            this.labels = new AxisLabels(this);
            this.FAxisDraw = new AxisDraw(this);
            this.axispen = new AxisLinePen(base.chart);
        }

        public Axis(IContainer container) : this()
        {
            container.Add(this);
        }

        public Axis(bool horiz, bool isOtherSide, Chart c) : this(c)
        {
            this.horizontal = horiz;
            this.otherSide = isOtherSide;
        }

        internal void AddToContainer(IContainer c)
        {
            c.Add(this);
        }

        public void AdjustMaxMin()
        {
            this.CalcMinMax(ref this.minimumValue, ref this.maximumValue);
            this.iMaximum = this.maximumValue;
            this.iMinimum = this.minimumValue;
            this.InternalCalcRange();
        }

        public void AdjustMaxMinRect(Rectangle rect)
        {
            double num;
            double num2;
            if (this.horizontal)
            {
                this.ReCalcAdjustedMinMax(rect.X, rect.Right, out num, out num2);
            }
            else
            {
                this.ReCalcAdjustedMinMax(rect.Y, rect.Bottom, out num, out num2);
            }
            this.InternalCalcPositions();
            this.iMaximum = num2;
            this.iMinimum = num;
            if (this.iMinimum > this.iMaximum)
            {
                double iMinimum = this.iMinimum;
                this.iMinimum = this.iMaximum;
                this.iMaximum = iMinimum;
            }
            this.InternalCalcRange();
        }

        private bool AnySeriesHasLessThan(int num)
        {
            bool flag = false;
            foreach (Series series in base.Chart.Series)
            {
                if (!series.bActive || ((!series.yMandatory || !this.Horizontal) && (series.yMandatory || this.Horizontal)))
                {
                    continue;
                }
                if (series.AssociatedToAxis(this))
                {
                    flag = series.Count <= num;
                    if (flag)
                    {
                        return flag;
                    }
                }
            }
            return flag;
        }

        private int ApplyPosition(int apos, Rectangle r)
        {
            int num;
            if (this.relativePosition == 0.0)
            {
                return apos;
            }
            if (this.positionUnits == Steema.TeeChart.PositionUnits.Percent)
            {
                num = this.horizontal ? r.Height : r.Width;
                num = Utils.Round((double) ((0.01 * this.relativePosition) * num));
            }
            else
            {
                num = (int) Math.Round(this.relativePosition);
            }
            if (this.otherSide)
            {
                num = -num;
            }
            if (this.horizontal)
            {
                num = -num;
            }
            return (apos + num);
        }

        private Rectangle AxisRect()
        {
            int iEndPos;
            int position;
            int iStartPos;
            int num4;
            if (this.IStartPos > this.IEndPos)
            {
                iEndPos = this.IEndPos;
                iStartPos = this.IStartPos;
            }
            else
            {
                iEndPos = this.IStartPos;
                iStartPos = this.IEndPos;
            }
            if (this.posAxis > this.labels.position)
            {
                position = this.labels.position;
                num4 = this.posAxis + AxisClickGap;
            }
            else
            {
                position = this.posAxis - AxisClickGap;
                num4 = this.labels.position;
            }
            if (this.horizontal)
            {
                return Rectangle.FromLTRB(iEndPos, position, iStartPos, num4);
            }
            return Rectangle.FromLTRB(position, iEndPos, num4, iStartPos);
        }

        private double CalcDateTimeIncrement(int maxNumLabels)
        {
            double aStep = Math.Max(this.desiredIncrement, Utils.DateTimeStep[0]);
            if ((aStep > 0.0) && (maxNumLabels > 0))
            {
                int num2;
                if ((this.iRange / aStep) > 1000000.0)
                {
                    aStep = this.iRange / 1000000.0;
                }
                do
                {
                    num2 = Utils.Round((double) (this.iRange / aStep));
                    if (num2 > maxNumLabels)
                    {
                        if (aStep < Utils.DateTimeStep[0x19])
                        {
                            aStep = this.NextDateTimeStep(aStep);
                        }
                        else
                        {
                            aStep *= 2.0;
                        }
                    }
                }
                while (num2 > maxNumLabels);
            }
            return Math.Max(aStep, Utils.DateTimeStep[0]);
        }

        private double CalcLabelsIncrement(int maxNumLabels)
        {
            if (maxNumLabels > 0)
            {
                if (this.iAxisDateTime)
                {
                    return this.CalcDateTimeIncrement(maxNumLabels);
                }
                return this.InternalCalcLabelsIncrement(maxNumLabels);
            }
            if (this.iAxisDateTime)
            {
                return Utils.DateTimeStep[0];
            }
            return this.iMinAxisIncrement;
        }

        private int CalcLabelsRect(int tmpSize, ref Rectangle r)
        {
            this.InflateAxisRect(tmpSize, ref r);
            return this.GetRectangleEdge(r);
        }

        public AxisLabelStyle CalcLabelStyle()
        {
            if (this.Labels.iStyle != AxisLabelStyle.Auto)
            {
                return this.labels.iStyle;
            }
            return this.InternalCalcLabelStyle();
        }

        public void CalcMinMax(ref double min, ref double max)
        {
            if (this.automatic || this.automaticMaximum)
            {
                max = base.chart.InternalMinMax(this, false, this.horizontal);
            }
            if (this.automatic || this.automaticMinimum)
            {
                min = base.chart.InternalMinMax(this, true, this.horizontal);
            }
        }

        public double CalcPosPoint(int value)
        {
            double iRangelog;
            if (this.logarithmic)
            {
                if (value == this.IStartPos)
                {
                    return this.InternalCalcPos(this.iMaximum, this.iMinimum);
                }
                if (value == this.IEndPos)
                {
                    return this.InternalCalcPos(this.iMinimum, this.iMaximum);
                }
                iRangelog = this.iRangelog;
                if (iRangelog == 0.0)
                {
                    return this.iMinimum;
                }
                if (this.inverted)
                {
                    iRangelog = ((this.IEndPos - value) * iRangelog) / ((double) this.IAxisSize);
                }
                else
                {
                    iRangelog = ((value - this.IStartPos) * iRangelog) / ((double) this.IAxisSize);
                }
                return Math.Exp(this.horizontal ? (this.iLogMin + iRangelog) : (this.iLogMax - iRangelog));
            }
            if (this.IAxisSize <= 0)
            {
                return 0.0;
            }
            iRangelog = this.inverted ? ((double) (this.IEndPos - value)) : ((double) (value - this.IStartPos));
            iRangelog *= this.iRange / ((double) this.IAxisSize);
            if (!this.horizontal)
            {
                return (this.iMaximum - iRangelog);
            }
            return (this.iMinimum + iRangelog);
        }

        public int CalcPosValue(double value)
        {
            if (!this.horizontal)
            {
                return this.CalcYPosValue(value);
            }
            return this.CalcXPosValue(value);
        }

        protected internal void CalcRect(ref Rectangle r, bool inflateChartRectangle)
        {
            this.iAxisDateTime = this.IsDateTime;
            if (inflateChartRectangle)
            {
                if (this.IsDepthAxis)
                {
                    this.posTitle = r.Right;
                }
                else if (((this.axisTitle != null) && this.axisTitle.Visible) && (this.axisTitle.Caption.Length != 0))
                {
                    this.posTitle = this.CalcLabelsRect(this.InternalCalcSize(this.axisTitle.Font, this.axisTitle.Angle, this.axisTitle.Caption, this.axisTitle.customSize), ref r);
                }
                if (this.labels.Visible)
                {
                    this.labels.position = this.CalcLabelsRect(this.SizeLabels(), ref r);
                }
                int num = this.SizeTickAxis() + base.chart.Walls.CalcWallSize(this);
                if (num > 0)
                {
                    this.InflateAxisRect(num, ref r);
                }
                this.posTitle = this.ApplyPosition(this.posTitle, r);
                this.labels.position = this.ApplyPosition(this.labels.position, r);
            }
            else
            {
                this.posAxis = this.ApplyPosition(this.GetRectangleEdge(r), r);
                this.labels.position = this.InflateAxisPos(this.posAxis, this.SizeTickAxis());
                this.posTitle = this.InflateAxisPos(this.labels.position, this.SizeLabels());
            }
        }

        public int CalcSizeValue(double value)
        {
            if (value <= 0.0)
            {
                return 0;
            }
            if (this.logarithmic)
            {
                if (this.iRangelog == 0.0)
                {
                    return 0;
                }
                return Utils.Round((double) (Math.Log(value) * this.iAxisLogSizeRange));
            }
            if (this.iRange == 0.0)
            {
                return 0;
            }
            return Utils.Round((double) (value * this.iAxisSizeRange));
        }

        public int CalcXPosValue(double value)
        {
            if (this.IsDepthAxis)
            {
                return this.InternalCalcDepthPosValue(value);
            }
            if (this.logarithmic)
            {
                return this.InternalCalcLogPosValue(true, value);
            }
            if (this.iRangezero)
            {
                return this.iCenterPos;
            }
            double num = (value - this.iMinimum) * this.iAxisSizeRange;
            num = this.inverted ? (this.IEndPos - num) : (this.IStartPos + num);
            if (num > 32767.0)
            {
                num = 32767.0;
            }
            else if (num < -32767.0)
            {
                num = -32767.0;
            }
            return (int) num;
        }

        public double CalcXYIncrement(int maxLabelSize)
        {
            int num;
            if (maxLabelSize > 0)
            {
                if (this.labels.iSeparation > 0)
                {
                    maxLabelSize += (int) Math.Round((double) ((0.01 * this.labels.iSeparation) * maxLabelSize));
                }
                num = (int) Math.Round((double) ((1.0 * this.IAxisSize) / ((double) maxLabelSize)));
            }
            else
            {
                num = 1;
            }
            return this.CalcLabelsIncrement(num);
        }

        public int CalcYPosValue(double value)
        {
            if (this.IsDepthAxis)
            {
                return this.InternalCalcDepthPosValue(value);
            }
            if (this.logarithmic)
            {
                return this.InternalCalcLogPosValue(false, value);
            }
            if (this.iRangezero)
            {
                return this.iCenterPos;
            }
            double num = (value - this.iMinimum) * this.iAxisSizeRange;
            num = this.inverted ? (this.IStartPos + num) : (this.IEndPos - num);
            if (num > 32767.0)
            {
                num = 32767.0;
            }
            else if (num < -32767.0)
            {
                num = -32767.0;
            }
            return (int) num;
        }

        private int CalcZPos()
        {
            int num = this.IsDepthAxis ? base.Chart.ChartBounds.Height : base.Chart.Aspect.Width3D;
            return (int) Math.Round((double) ((num * this.zPosition) * 0.01));
        }

        public bool Clicked(Point xy)
        {
            return this.Clicked(xy.X, xy.Y);
        }

        public bool Clicked(int x, int y)
        {
            return (base.chart.IsAxisVisible(this) && this.AxisRect().Contains(x, y));
        }

        public string DateTimeDefaultFormat(double astep)
        {
            DateTimeFormatInfo currentInfo = DateTimeFormatInfo.CurrentInfo;
            if (astep > 1.0)
            {
                return currentInfo.ShortDatePattern;
            }
            return currentInfo.ShortTimePattern;
        }

        public void DateTimeIncrement(bool isDateTime, bool increment, ref double value, double anIncrement, DateTimeSteps tmpWhichDatetime)
        {
            if (!isDateTime)
            {
                value += increment ? anIncrement : -anIncrement;
            }
            else
            {
                DateTime time = Utils.DateTime(value);
                int year = time.Year;
                int month = time.Month;
                int day = time.Day;
                switch (tmpWhichDatetime)
                {
                    case DateTimeSteps.HalfMonth:
                        if (day <= 15)
                        {
                            if (day > 1)
                            {
                                day = 1;
                            }
                            else
                            {
                                this.IncDecMonths(1, ref day, ref month, ref year, increment);
                                day = 15;
                            }
                            break;
                        }
                        day = 15;
                        break;

                    case DateTimeSteps.OneMonth:
                        this.IncDecMonths(1, ref day, ref month, ref year, increment);
                        break;

                    case DateTimeSteps.TwoMonths:
                        this.IncDecMonths(2, ref day, ref month, ref year, increment);
                        break;

                    case DateTimeSteps.ThreeMonths:
                        this.IncDecMonths(3, ref day, ref month, ref year, increment);
                        break;

                    case DateTimeSteps.FourMonths:
                        this.IncDecMonths(4, ref day, ref month, ref year, increment);
                        break;

                    case DateTimeSteps.SixMonths:
                        this.IncDecMonths(6, ref day, ref month, ref year, increment);
                        break;

                    case DateTimeSteps.OneYear:
                        if (!increment)
                        {
                            year--;
                            break;
                        }
                        year++;
                        break;

                    default:
                        if (increment)
                        {
                            value += anIncrement;
                            return;
                        }
                        value -= anIncrement;
                        return;
                }
                value = Utils.DateTime(new DateTime(year, month, day));
            }
        }

        private void DecMonths(int howMany, ref int day, ref int month, ref int year)
        {
            day = 1;
            if (month > howMany)
            {
                month -= howMany;
            }
            else
            {
                year--;
                month = 12 - (howMany - month);
            }
        }

        private void DoCalculation(int aStartPos, int aSize)
        {
            this.IStartPos = aStartPos + Utils.Round((double) ((0.01 * aSize) * this.startPosition));
            this.IEndPos = aStartPos + Utils.Round((double) ((0.01 * aSize) * this.endPosition));
        }

        public void Draw(bool calcPosAxis)
        {
            this.FAxisDraw.Draw(calcPosAxis);
        }

        public void Draw(int posLabels, int posTitle, int posAxis, bool gridVisible)
        {
            this.InternalCalcPositions();
            this.Draw(posLabels, posTitle, posAxis, gridVisible, this.IStartPos, this.IEndPos);
        }

        public void Draw(int posLabels, int posTitle, int posAxis, bool gridVisible, int aStartPos, int aEndPos)
        {
            this.Labels.position = posLabels;
            this.posTitle = posTitle;
            this.posAxis = posAxis;
            this.IStartPos = aStartPos;
            this.IEndPos = aEndPos;
            this.RecalcSizeCenter();
            bool visible = this.Grid.Visible;
            this.grid.Visible = gridVisible;
            this.Draw(false);
            this.grid.Visible = visible;
        }

        public void Draw(int posLabels, int posTitle, int posAxis, bool gridVisible, double aMinimum, double aMaximum, int aStartPosition, int aEndPosition)
        {
            this.Draw(posLabels, posTitle, posAxis, gridVisible, aMinimum, aMaximum, this.desiredIncrement, aStartPosition, aEndPosition);
        }

        public void Draw(int posLabels, int posTitle, int posAxis, bool gridVisible, double aMinimum, double aMaximum, double aIncrement, int aStartPos, int aEndPos)
        {
            double minimumValue = this.minimumValue;
            double maximumValue = this.maximumValue;
            double desiredIncrement = this.desiredIncrement;
            bool automatic = this.automatic;
            this.automatic = false;
            this.minimumValue = aMinimum;
            this.maximumValue = aMaximum;
            this.desiredIncrement = aIncrement;
            this.SetInternals();
            this.Draw(posLabels, posTitle, posAxis, gridVisible, aStartPos, aEndPos);
            this.minimumValue = minimumValue;
            this.maximumValue = maximumValue;
            this.desiredIncrement = desiredIncrement;
            this.automatic = automatic;
            this.SetInternals();
        }

        public void DrawAxisLabel(int x, int y, int angle, string st, TextShape labelItem)
        {
            this.DrawAxisLabel(this.labels.Font, x, y, angle, st, labelItem);
        }

        public void DrawAxisLabel(ChartFont f, int x, int y, int angle, string st, TextShape format)
        {
            StringAlignment center;
            Graphics3D g = base.chart.graphics3D;
            StringAlignment textAlign = g.TextAlign;
            if (format == null)
            {
                format = this.Labels;
            }
            else
            {
                f = format.Font;
            }
            g.Font = f;
            int num = g.FontHeight / 2;
            int iZPos = this.iZPos;
            int num3 = this.NumTextLines(st);
            int fontHeight = g.FontHeight;
            if (this.horizontal && this.otherSide)
            {
                if ((angle > 90) && (angle <= 180))
                {
                    y += fontHeight;
                    center = StringAlignment.Center;
                }
                else if ((angle > 0) && (angle <= 90))
                {
                    x -= fontHeight;
                    x -= (fontHeight * num3) / 2;
                    if (this.Labels.Align == AxisLabelAlign.Default)
                    {
                        center = StringAlignment.Near;
                    }
                    else
                    {
                        y -= this.MaxLabelsWidth();
                        center = StringAlignment.Far;
                    }
                }
                else if ((angle > 180) && (angle <= 270))
                {
                    x = (x + fontHeight) + ((fontHeight * num3) / 2);
                    if (this.Labels.Align == AxisLabelAlign.Default)
                    {
                        center = StringAlignment.Far;
                    }
                    else
                    {
                        y -= this.MaxLabelsWidth();
                        center = StringAlignment.Near;
                    }
                }
                else
                {
                    y -= fontHeight * (num3 + 1);
                    center = StringAlignment.Center;
                }
            }
            else if (this.horizontal)
            {
                if ((angle > 90) && (angle <= 180))
                {
                    y += fontHeight;
                    y += fontHeight * num3;
                    center = StringAlignment.Center;
                }
                else if ((angle > 0) && (angle <= 90))
                {
                    x -= fontHeight;
                    x -= (fontHeight * num3) / 2;
                    if (this.Labels.Align == AxisLabelAlign.Default)
                    {
                        center = StringAlignment.Far;
                    }
                    else
                    {
                        y += this.MaxLabelsWidth();
                        center = StringAlignment.Near;
                    }
                }
                else if ((angle > 180) && (angle <= 270))
                {
                    x = (x + fontHeight) + ((fontHeight * num3) / 2);
                    if (this.Labels.Align == AxisLabelAlign.Default)
                    {
                        center = StringAlignment.Near;
                    }
                    else
                    {
                        y += this.MaxLabelsWidth();
                        center = StringAlignment.Far;
                    }
                }
                else
                {
                    y -= fontHeight;
                    center = StringAlignment.Center;
                }
            }
            else if (this.otherSide)
            {
                if (((angle > 270) && (angle <= 360)) || (angle == 0))
                {
                    y -= fontHeight;
                    y -= (fontHeight * num3) / 2;
                    if (this.Labels.Align == AxisLabelAlign.Default)
                    {
                        center = StringAlignment.Near;
                    }
                    else
                    {
                        x += this.MaxLabelsWidth();
                        center = StringAlignment.Far;
                    }
                }
                else if ((angle > 90) && (angle <= 180))
                {
                    y += fontHeight;
                    y += (fontHeight * num3) / 2;
                    if (this.Labels.Align == AxisLabelAlign.Default)
                    {
                        center = StringAlignment.Far;
                    }
                    else
                    {
                        x += this.MaxLabelsWidth();
                        center = StringAlignment.Near;
                    }
                }
                else if ((angle > 0) && (angle <= 90))
                {
                    x -= fontHeight;
                    center = StringAlignment.Center;
                }
                else
                {
                    x += fontHeight * (num3 + 1);
                    center = StringAlignment.Center;
                }
            }
            else if (((angle > 270) && (angle <= 360)) || (angle == 0))
            {
                y -= fontHeight;
                y -= (fontHeight * num3) / 2;
                if (this.Labels.Align == AxisLabelAlign.Default)
                {
                    center = StringAlignment.Far;
                }
                else
                {
                    x -= this.MaxLabelsWidth();
                    center = StringAlignment.Near;
                }
            }
            else if ((angle > 90) && (angle <= 180))
            {
                y += fontHeight;
                y += (fontHeight * num3) / 2;
                if (this.Labels.Align == AxisLabelAlign.Default)
                {
                    center = StringAlignment.Near;
                }
                else
                {
                    x -= this.MaxLabelsWidth();
                    center = StringAlignment.Far;
                }
            }
            else if ((angle > 0) && (angle <= 90))
            {
                x -= fontHeight * (num3 + 1);
                center = StringAlignment.Center;
            }
            else
            {
                x += fontHeight;
                center = StringAlignment.Center;
            }
            if (!format.Transparent)
            {
                int num5;
                int num6 = ((int) g.TextWidth("W")) / 4;
                format.Left = x;
                format.Top = y + fontHeight;
                int num7 = base.chart.MultiLineTextWidth(st, out num5);
                num = g.FontHeight * num5;
                StringAlignment alignment2 = center;
                format.Bottom = format.Top + num;
                format.Right = format.Left + num7;
                switch (alignment2)
                {
                    case StringAlignment.Far:
                        format.Right = format.Left;
                        format.Left = format.Right - num7;
                        break;

                    case StringAlignment.Center:
                        num7 = (format.Right - format.Left) / 2;
                        format.Right -= num7;
                        format.Left -= num7;
                        break;
                }
                format.Left -= num6;
                format.Right += num6 / 2;
                if (iZPos != 0)
                {
                    format.ShapeBounds = g.CalcRect3D(format.ShapeBounds, iZPos);
                }
                format.DrawRectRotated(g, format.ShapeBounds, this.Labels.Angle, iZPos);
                g.Brush.Visible = false;
            }
            g.TextAlign = center;
            string str = st;
            string str2 = "";
            bool flag = base.chart.aspect.view3D;
            for (int i = 1; i <= num3; i++)
            {
                int index = str.IndexOf('\n');
                str2 = (index != -1) ? str.Substring(0, index) : str;
                if (angle == 0)
                {
                    y += fontHeight;
                    g.Font = f;
                    if (this.labels.Exponent)
                    {
                        this.DrawExponentLabel(x, y, iZPos, ref str2);
                    }
                    else if (flag)
                    {
                        g.TextOut(x, y, iZPos, str2);
                    }
                    else
                    {
                        g.TextOut(f, x, y, str2);
                    }
                }
                else
                {
                    if ((angle >= 0) && (angle <= 90))
                    {
                        x += fontHeight;
                    }
                    else if ((angle > 90) && (angle <= 180))
                    {
                        y -= fontHeight;
                    }
                    else if ((angle > 180) && (angle <= 270))
                    {
                        x -= fontHeight;
                    }
                    else if ((angle > 270) && (angle < 360))
                    {
                        y += fontHeight;
                    }
                    g.RotateLabel(x, y, iZPos, str2, (double) angle);
                }
                if (index >= 0)
                {
                    str = str.Remove(0, index + 1);
                }
            }
            g.TextAlign = textAlign;
        }

        private void DrawExponentLabel(int x, int y, int tmpZ, ref string tmpSt2)
        {
            Graphics3D graphicsd = base.chart.Graphics3D;
            int index = tmpSt2.ToUpper().IndexOf('E');
            if (index == -1)
            {
                graphicsd.TextOut(x, y, tmpZ, tmpSt2);
            }
            else
            {
                float num4;
                string text = "";
                string str2 = "";
                if ((index - 1) < 0)
                {
                    text = tmpSt2.Substring(0, 1);
                    str2 = tmpSt2.Substring(1);
                }
                else
                {
                    text = tmpSt2.Substring(0, index - 1);
                    str2 = tmpSt2.Substring(index + 1);
                }
                int size = graphicsd.Font.Size;
                int num3 = size - 1;
                if (graphicsd.TextAlign == StringAlignment.Near)
                {
                    graphicsd.TextOut(x, y, tmpZ, text);
                    num4 = graphicsd.TextWidth(text) + 1f;
                    ChartFont font = graphicsd.Font;
                    font.Size -= (int) (size * 0.25);
                    graphicsd.TextOut((int) (x + ((int) num4)), (y - ((int) (num3 * 0.5))) + 2, tmpZ, str2);
                    graphicsd.Font.Size = size;
                }
                else
                {
                    ChartFont font2 = graphicsd.Font;
                    font2.Size -= (int) (size * 0.25);
                    graphicsd.TextOut(x, (y - ((int) (num3 * 0.5))) + 2, tmpZ, str2);
                    num4 = graphicsd.TextWidth(str2) + 1f;
                    graphicsd.Font.Size = size;
                    graphicsd.TextOut((int) (x - ((int) num4)), y, tmpZ, text);
                }
            }
        }

        public void DrawTitle(int x, int y)
        {
            if (this.IsDepthAxis)
            {
                Graphics3D graphicsd = base.chart.graphics3D;
                graphicsd.TextAlign = StringAlignment.Near;
                graphicsd.Font = this.axisTitle.Font;
                graphicsd.TextOut(x, y, base.chart.aspect.Width3D / 2, this.axisTitle.Caption);
            }
            else
            {
                bool bExponent = this.labels.bExponent;
                this.labels.bExponent = false;
                this.DrawAxisLabel(this.axisTitle.Font, x, y, this.axisTitle.Angle, this.axisTitle.Caption, null);
                this.labels.bExponent = bExponent;
            }
        }

        public static DateTimeSteps FindDateTimeStep(double stepValue)
        {
            for (int i = 0x18; i >= 0; i--)
            {
                if (Math.Abs((double) (Utils.DateTimeStep[i] - stepValue)) < Utils.DateTimeStep[0])
                {
                    return (DateTimeSteps) i;
                }
            }
            return DateTimeSteps.None;
        }

        private int GetRectangleEdge(Rectangle r)
        {
            if (this.otherSide)
            {
                if (!this.horizontal)
                {
                    return r.Right;
                }
                return r.Y;
            }
            if (!this.horizontal)
            {
                return r.X;
            }
            return r.Bottom;
        }

        internal void IncDecDateTime(bool increment, ref double value, double anIncrement, DateTimeSteps tmpWhichDatetime)
        {
            this.DateTimeIncrement((this.labels.ExactDateTime && this.iAxisDateTime) && (tmpWhichDatetime >= DateTimeSteps.HalfMonth), increment, ref value, anIncrement, tmpWhichDatetime);
        }

        private void IncDecMonths(int howMany, ref int day, ref int month, ref int year, bool increment)
        {
            if (increment)
            {
                this.IncMonths(howMany, ref day, ref month, ref year);
            }
            else
            {
                this.DecMonths(howMany, ref day, ref month, ref year);
            }
        }

        private void IncMonths(int howMany, ref int day, ref int month, ref int year)
        {
            day = 1;
            month += howMany;
            if (month > 12)
            {
                year++;
                month -= 12;
            }
        }

        private int InflateAxisPos(int value, int amount)
        {
            int num = value;
            if (this.horizontal)
            {
                if (this.otherSide)
                {
                    return (num - amount);
                }
                return (num + amount);
            }
            if (this.otherSide)
            {
                return (num + amount);
            }
            return (num - amount);
        }

        private void InflateAxisRect(int value, ref Rectangle r)
        {
            int x = r.X;
            int y = r.Y;
            int right = r.Right;
            int bottom = r.Bottom;
            if (this.horizontal)
            {
                if (this.otherSide)
                {
                    y += value;
                }
                else
                {
                    bottom -= value;
                }
            }
            else if (this.otherSide)
            {
                right -= value;
            }
            else
            {
                x += value;
            }
            r.X = x;
            r.Y = y;
            r.Width = right - x;
            r.Height = bottom - y;
        }

        private int InternalCalcDepthPosValue(double value)
        {
            if (this.iRangezero)
            {
                return this.iCenterPos;
            }
            return (int) (this.iAxisSizeRange * (this.inverted ? (this.iMaximum - value) : (value - this.iMinimum)));
        }

        private double InternalCalcLabelsIncrement(int maxNumLabels)
        {
            double desiredIncrement;
            if (this.desiredIncrement <= 0.0)
            {
                if (this.iRange == 0.0)
                {
                    desiredIncrement = 1.0;
                }
                else
                {
                    desiredIncrement = Math.Abs(this.iRange) / ((double) (maxNumLabels + 1));
                    if (this.AnySeriesHasLessThan(maxNumLabels))
                    {
                        desiredIncrement = Math.Max(1.0, desiredIncrement);
                    }
                }
            }
            else
            {
                desiredIncrement = this.desiredIncrement;
            }
            int num2 = maxNumLabels + 1;
            bool flag = false;
            if (this.labels.iSeparation >= 0)
            {
                do
                {
                    double num3 = this.iRange / desiredIncrement;
                    if (Math.Abs(num3) < 2147483647.0)
                    {
                        num2 = (int) Math.Round(num3);
                        if (num2 > maxNumLabels)
                        {
                            desiredIncrement = this.NextStep(desiredIncrement);
                        }
                    }
                    else
                    {
                        desiredIncrement = this.NextStep(desiredIncrement);
                    }
                    flag = double.IsInfinity(desiredIncrement);
                }
                while (((num2 > maxNumLabels) && (desiredIncrement <= this.iRange)) && !flag);
            }
            if (!flag)
            {
                return Math.Max(desiredIncrement, this.iMinAxisIncrement);
            }
            return this.iRange;
        }

        private AxisLabelStyle InternalCalcLabelStyle()
        {
            AxisLabelStyle none = AxisLabelStyle.None;
            if (this.IsDepthAxis)
            {
                none = AxisLabelStyle.Text;
                foreach (Series series in base.chart.series)
                {
                    if (series.bActive && (series.HasZValues || (series.MinZValue() != series.MaxZValue())))
                    {
                        return AxisLabelStyle.Value;
                    }
                }
                return none;
            }
            foreach (Series series2 in base.chart.series)
            {
                if (!series2.bActive || !series2.AssociatedToAxis(this))
                {
                    continue;
                }
                none = AxisLabelStyle.Value;
                if (((this.horizontal && series2.yMandatory) || (!this.horizontal && !series2.yMandatory)) && ((series2.sLabels != null) && (series2.sLabels[0].Length != 0)))
                {
                    return AxisLabelStyle.Text;
                }
            }
            return none;
        }

        private int InternalCalcLogPosValue(bool isx, double value)
        {
            if (this.iRangelog == 0.0)
            {
                return this.iCenterPos;
            }
            if (value <= 0.0)
            {
                if ((!isx || !this.inverted) && (isx || this.inverted))
                {
                    return this.IStartPos;
                }
                return this.IEndPos;
            }
            int num = (int) Math.Round(this.inverted ? ((this.iLogMax - Math.Log(value)) * this.iAxisLogSizeRange) : ((Math.Log(value) - this.iLogMin) * this.iAxisLogSizeRange));
            if (!isx)
            {
                return (this.IEndPos - num);
            }
            return (this.IStartPos + num);
        }

        private double InternalCalcPos(double A, double B)
        {
            if (this.horizontal && this.inverted)
            {
                return A;
            }
            if (!this.horizontal && !this.inverted)
            {
                return A;
            }
            return B;
        }

        internal void InternalCalcPositions()
        {
            if (this.IsDepthAxis)
            {
                this.DoCalculation(0, base.chart.aspect.Width3D);
            }
            else
            {
                Rectangle chartRect = base.chart.ChartRect;
                if (this.horizontal)
                {
                    this.DoCalculation(chartRect.X, base.chart.ChartRectWidth);
                }
                else
                {
                    this.DoCalculation(chartRect.Y, base.chart.ChartRectHeight);
                }
            }
            this.RecalcSizeCenter();
        }

        public void InternalCalcRange()
        {
            this.iRange = this.iMaximum - this.iMinimum;
            this.iRangezero = this.iRange == 0.0;
            this.iAxisSizeRange = this.iRangezero ? 0.0 : (((double) this.IAxisSize) / this.iRange);
            if (this.logarithmic)
            {
                this.iLogMin = (this.iMinimum <= 0.0) ? 0.0 : Math.Log(this.iMinimum);
                this.iLogMax = (this.iMaximum <= 0.0) ? 0.0 : Math.Log(this.iMaximum);
                this.iRangelog = this.iLogMax - this.iLogMin;
                this.iAxisLogSizeRange = (this.iRangelog == 0.0) ? 0.0 : (((double) this.IAxisSize) / this.iRangelog);
            }
            this.iZPos = this.CalcZPos();
        }

        private int InternalCalcSize(ChartFont tmpFont, int tmpAngle, string tmpText, int tmpSize)
        {
            if (tmpSize != 0)
            {
                return tmpSize;
            }
            Graphics3D graphicsd = base.chart.Graphics3D;
            if (this.horizontal)
            {
                switch (tmpAngle)
                {
                    case 0:
                    case 180:
                        return graphicsd.FontTextHeight(tmpFont);
                }
                if (tmpText.Length == 0)
                {
                    graphicsd.Font = tmpFont;
                    return this.MaxLabelsWidth();
                }
                return (int) graphicsd.TextWidth(tmpFont, tmpText);
            }
            switch (tmpAngle)
            {
                case 90:
                case 270:
                    return graphicsd.FontTextHeight(tmpFont);
            }
            if (tmpText.Length == 0)
            {
                graphicsd.Font = tmpFont;
                return this.MaxLabelsWidth();
            }
            return (int) graphicsd.TextWidth(tmpFont, tmpText);
        }

        internal void InternalSetInverted(bool value)
        {
            this.inverted = value;
        }

        private void InternalSetMaximum(double value)
        {
            base.SetDoubleProperty(ref this.maximumValue, value);
        }

        private void InternalSetMinimum(double value)
        {
            base.SetDoubleProperty(ref this.minimumValue, value);
        }

        public bool IsCustom()
        {
            return (base.chart.axes.custom.IndexOf(this) != -1);
        }

        private int MaxLabelsValueWidth()
        {
            double iMinimum;
            double iMaximum;
            if ((this.IsDateTime && this.labels.ExactDateTime) || this.labels.RoundFirstLabel)
            {
                double calcIncrement = this.CalcIncrement;
                iMinimum = calcIncrement * Convert.ToInt64((double) (this.iMinimum / calcIncrement));
                iMaximum = calcIncrement * Convert.ToInt64((double) (this.iMaximum / calcIncrement));
            }
            else
            {
                iMinimum = this.iMinimum;
                iMaximum = this.iMaximum;
            }
            return (((int) base.chart.Graphics3D.TextWidth(" ")) + Math.Max(base.chart.MultiLineTextWidth(this.labels.LabelValue(iMinimum)), base.chart.MultiLineTextWidth(this.labels.LabelValue(iMaximum))));
        }

        public int MaxLabelsWidth()
        {
            if (this.Labels.Items.Count == 0)
            {
                switch (this.CalcLabelStyle())
                {
                    case AxisLabelStyle.Value:
                        return this.MaxLabelsValueWidth();

                    case AxisLabelStyle.Mark:
                        return base.chart.MaxMarkWidth();

                    case AxisLabelStyle.Text:
                        return base.chart.MaxTextWidth();
                }
                return 0;
            }
            int num = 0;
            for (int i = 0; i < this.Labels.Items.Count; i++)
            {
                base.Chart.Graphics3D.Font = this.Labels.Items[i].Font;
                num = Math.Max(num, base.Chart.MultiLineTextWidth(this.Labels.Items[i].Text));
            }
            return num;
        }

        private double NextDateTimeStep(double aStep)
        {
            for (int i = 0x18; i >= 0; i--)
            {
                if (aStep >= Utils.DateTimeStep[i])
                {
                    return Utils.DateTimeStep[i + 1];
                }
            }
            return Utils.DateTimeStep[0x19];
        }

        private double NextStep(double oldStep)
        {
            if (oldStep >= 10.0)
            {
                return (10.0 * this.NextStep(0.1 * oldStep));
            }
            if (oldStep < 1.0)
            {
                return (0.1 * this.NextStep(oldStep * 10.0));
            }
            if (oldStep < 2.0)
            {
                return 2.0;
            }
            if (oldStep < 5.0)
            {
                return 5.0;
            }
            return 10.0;
        }

        private int NumTextLines(string st)
        {
            int index = st.IndexOf('\n');
            if (index == -1)
            {
                return 1;
            }
            int num2 = 0;
            while (index != -1)
            {
                num2++;
                st = st.Remove(0, index + 1);
                index = st.IndexOf('\n');
            }
            if (st.Length != 0)
            {
                num2++;
            }
            return num2;
        }

        private void ReCalcAdjustedMinMax(int pos1, int pos2, out double tmpMin, out double tmpMax)
        {
            int iStartPos = this.IStartPos;
            int iEndPos = this.IEndPos;
            this.IStartPos += pos1;
            this.IEndPos -= pos2;
            this.IAxisSize = this.IEndPos - this.IStartPos;
            tmpMin = this.CalcPosPoint(iStartPos);
            tmpMax = this.CalcPosPoint(iEndPos);
        }

        private void RecalcSizeCenter()
        {
            this.IAxisSize = this.IEndPos - this.IStartPos;
            this.iCenterPos = (this.IStartPos + this.IEndPos) / 2;
            this.InternalCalcRange();
        }

        internal void RemoveFromContainer(IContainer c)
        {
            c.Remove(this);
        }

        public void Scroll(double offset, bool checkLimits)
        {
            if ((!checkLimits || ((offset > 0.0) && (this.maximumValue < base.chart.InternalMinMax(this, false, this.Horizontal)))) || ((offset < 0.0) && (this.minimumValue > base.chart.InternalMinMax(this, true, this.Horizontal))))
            {
                this.automatic = false;
                this.automaticMaximum = false;
                this.maximumValue += offset;
                this.automaticMinimum = false;
                this.minimumValue += offset;
                this.Invalidate();
            }
        }

        private void SetAutoMinMax(ref bool variable, bool var2, bool value)
        {
            base.SetBooleanProperty(ref variable, value);
            if (value)
            {
                if (var2)
                {
                    this.automatic = true;
                }
            }
            else
            {
                this.automatic = false;
            }
        }

        protected override void SetChart(Chart c)
        {
            base.SetChart(c);
            this.labels.Chart = base.chart;
            this.FAxisDraw.Chart = base.chart;
        }

        private void SetInternals()
        {
            this.iMaximum = this.maximumValue;
            this.iMinimum = this.minimumValue;
            this.InternalCalcRange();
        }

        public void SetMinMax(DateTime minDate, DateTime maxDate)
        {
            this.SetMinMax(Utils.DateTime(minDate), Utils.DateTime(maxDate));
        }

        public void SetMinMax(double min, double max)
        {
            this.automatic = false;
            this.automaticMinimum = false;
            this.automaticMaximum = false;
            if (min > max)
            {
                double num = min;
                min = max;
                max = num;
            }
            this.InternalSetMinimum(min);
            this.InternalSetMaximum(max);
            if ((this.maximumValue - this.minimumValue) < 1E-10)
            {
                this.InternalSetMaximum(this.minimumValue + 1E-10);
            }
        }

        protected bool ShouldSerializeHorizontal()
        {
            return this.IsCustom();
        }

        protected virtual bool ShouldSerializeMaximum()
        {
            return (!this.automatic && !this.automaticMaximum);
        }

        protected virtual bool ShouldSerializeMinimum()
        {
            return (!this.automatic && !this.automaticMinimum);
        }

        protected bool ShouldSerializeOtherSide()
        {
            return this.IsCustom();
        }

        protected bool ShouldSerializeZPosition()
        {
            if (this.IsDepthAxis)
            {
                return false;
            }
            return ((this.OtherSide && (this.zPosition != 100.0)) || (!this.OtherSide && (this.zPosition != 0.0)));
        }

        internal int SizeLabels()
        {
            return this.InternalCalcSize(this.labels.Font, this.labels.Angle, "", this.labels.CustomSize);
        }

        internal int SizeTickAxis()
        {
            int num = 0;
            if (this.Visible)
            {
                num = this.axispen.Width + 1;
            }
            if (this.Ticks.Visible)
            {
                num += this.ticks.length;
            }
            if (this.MinorTicks.Visible)
            {
                num = Math.Max(num, this.minorTicks.length);
            }
            return num;
        }

        public string TitleOrName()
        {
            string caption = this.Title.Caption;
            if (caption.Length != 0)
            {
                return caption;
            }
            if (this is DepthAxis)
            {
                return Texts.DepthAxis;
            }
            if (this.horizontal)
            {
                if (this.otherSide)
                {
                    return Texts.TopAxis;
                }
                return Texts.BottomAxis;
            }
            if (this.otherSide)
            {
                return Texts.RightAxis;
            }
            return Texts.LeftAxis;
        }

        [Category("Axis Scales"), DefaultValue(true), Description("Calculates Max & Min of axis scale based on dependent Series.")]
        public bool Automatic
        {
            get
            {
                return this.automatic;
            }
            set
            {
                base.SetBooleanProperty(ref this.automatic, value);
                this.automaticMinimum = value;
                this.automaticMaximum = value;
            }
        }

        [Description("Calculates Max of axis scale based on dependent Series."), DefaultValue(true), Category("Axis Scales")]
        public bool AutomaticMaximum
        {
            get
            {
                return this.automaticMaximum;
            }
            set
            {
                this.SetAutoMinMax(ref this.automaticMaximum, this.automaticMinimum, value);
            }
        }

        [Description("Calculates Min of axis scale based on dependent Series."), Category("Axis Scales"), DefaultValue(true)]
        public bool AutomaticMinimum
        {
            get
            {
                return this.automaticMinimum;
            }
            set
            {
                this.SetAutoMinMax(ref this.automaticMinimum, this.automaticMaximum, value);
            }
        }

        [Description("Pen used to draw the axis."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Axis Format")]
        public AxisLinePen AxisPen
        {
            get
            {
                return this.axispen;
            }
        }

        [Browsable(false), Description("Returns the calculated Axis Label increment.")]
        public double CalcIncrement
        {
            get
            {
                int num;
                if (this.horizontal)
                {
                    num = Math.Max(this.labels.LabelWidth(this.iMinimum), this.labels.LabelWidth(this.iMaximum));
                }
                else
                {
                    num = Math.Max(this.labels.LabelHeight(this.iMinimum), this.labels.LabelHeight(this.iMaximum));
                }
                return this.CalcXYIncrement(num);
            }
        }

        [Category("Axis Layout"), Description("Axis End position as percentage of Chart."), DefaultValue((double) 100.0)]
        public double EndPosition
        {
            get
            {
                return this.endPosition;
            }
            set
            {
                base.SetDoubleProperty(ref this.endPosition, value);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Axis Format"), Description("Accesses Pen for Chart back Grid.")]
        public GridPen Grid
        {
            get
            {
                if (this.grid == null)
                {
                    this.grid = new GridPen(base.chart);
                }
                return this.grid;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("Please use Axis.Grid.Centered property."), DefaultValue(false), Category("Axis Format")]
        public bool GridCentered
        {
            get
            {
                return this.Grid.centered;
            }
            set
            {
                this.Grid.Centered = value;
            }
        }

        [Browsable(false), Description("Sets a custom axis to be drawn horizontally.")]
        public bool Horizontal
        {
            get
            {
                return this.horizontal;
            }
            set
            {
                base.SetBooleanProperty(ref this.horizontal, value);
            }
        }

        [Description("Minimum step between axis labels."), DefaultValue((double) 0.0), Category("Axis Scales")]
        public double Increment
        {
            get
            {
                return this.desiredIncrement;
            }
            set
            {
                if (value < 0.0)
                {
                    throw new TeeChartException(Texts.AxisIncrementNeg);
                }
                base.SetDoubleProperty(ref this.desiredIncrement, value);
            }
        }

        [Description("Swaps Axis Minimum and Maximum scales."), Category("Axis Layout"), DefaultValue(false)]
        public bool Inverted
        {
            get
            {
                return this.inverted;
            }
            set
            {
                base.SetBooleanProperty(ref this.inverted, value);
            }
        }

        [DefaultValue(false), Description("Returns True if associated Series are date-time (Series.XValues.DateTime)."), Browsable(false)]
        public bool IsDateTime
        {
            get
            {
                foreach (Series series in base.chart.series)
                {
                    if (series.Active && series.AssociatedToAxis(this))
                    {
                        return (this.Horizontal ? series.XValues.DateTime : series.vyValues.DateTime);
                    }
                }
                return false;
            }
        }

        [Category("Axis"), Description("Axis Labels characteristics"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public AxisLabels Labels
        {
            get
            {
                return this.labels;
            }
        }

        [Browsable(false), DefaultValue(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never), Obsolete("Please use Labels.OnAxis property.")]
        public bool LabelsOnAxis
        {
            get
            {
                return this.labels.bOnAxis;
            }
            set
            {
                this.labels.OnAxis = value;
            }
        }

        [Category("Axis Scales"), DefaultValue(false), Description("Scales the Axis Logarithmically when True.")]
        public bool Logarithmic
        {
            get
            {
                return this.logarithmic;
            }
            set
            {
                if (value && this.IsDateTime)
                {
                    throw new TeeChartException(Texts.AxisLogDateTime);
                }
                if (value)
                {
                    this.AdjustMaxMin();
                    if ((this.iMinimum < 0.0) || (this.iMaximum < 0.0))
                    {
                        throw new TeeChartException(Texts.AxisLogNotPositive);
                    }
                }
                base.SetBooleanProperty(ref this.logarithmic, value);
            }
        }

        [DefaultValue((double) 10.0), Category("Axis Scales"), Description("Base used for Logarithmic scale.")]
        public double LogarithmicBase
        {
            get
            {
                return this.logarithmicBase;
            }
            set
            {
                base.SetDoubleProperty(ref this.logarithmicBase, value);
            }
        }

        [Description("Maximum Axis value."), Category("Axis Scales")]
        public double Maximum
        {
            get
            {
                return this.maximumValue;
            }
            set
            {
                this.InternalSetMaximum(value);
            }
        }

        [DefaultValue(0), Category("Axis Scales"), Description("Amount of pixels left as margin at axis max position.")]
        public int MaximumOffset
        {
            get
            {
                return this.maximumOffset;
            }
            set
            {
                base.SetIntegerProperty(ref this.maximumOffset, value);
            }
        }

        [Description("Maximum X value for the specified Axis."), Browsable(false)]
        public double MaxXValue
        {
            get
            {
                return base.chart.MaxXValue(this);
            }
        }

        [Browsable(false), Description("Maximum Y value for the specified Axis.")]
        public double MaxYValue
        {
            get
            {
                return base.chart.MaxYValue(this);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double MinAxisIncrement
        {
            get
            {
                return this.iMinAxisIncrement;
            }
            set
            {
                this.iMinAxisIncrement = value;
            }
        }

        [Description("Minimum Axis value."), Category("Axis Scales")]
        public double Minimum
        {
            get
            {
                return this.minimumValue;
            }
            set
            {
                this.InternalSetMinimum(value);
            }
        }

        [Description("Pixels separation at axis minimum."), Category("Axis Scales"), DefaultValue(0)]
        public int MinimumOffset
        {
            get
            {
                return this.minimumOffset;
            }
            set
            {
                base.SetIntegerProperty(ref this.minimumOffset, value);
            }
        }

        [Description("Sets characteristics of Minor Tick Grid."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Axis Minor")]
        public ChartPen MinorGrid
        {
            get
            {
                if (this.minorGrid == null)
                {
                    this.minorGrid = new ChartPen(base.chart, Color.Black, false);
                    this.minorGrid.Style = DashStyle.Dot;
                    this.minorGrid.defaultStyle = DashStyle.Dot;
                }
                return this.minorGrid;
            }
        }

        [Category("Axis Minor"), Description("Number of Axis minor ticks between major ticks."), DefaultValue(3)]
        public int MinorTickCount
        {
            get
            {
                return this.minorTickCount;
            }
            set
            {
                base.SetIntegerProperty(ref this.minorTickCount, value);
            }
        }

        [Category("Axis Minor"), Description("Pen used to draw Axis Minor ticks."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TicksPen MinorTicks
        {
            get
            {
                if (this.minorTicks == null)
                {
                    this.minorTicks = new TicksPen(base.chart);
                    this.minorTicks.length = 2;
                    this.minorTicks.defaultLength = 2;
                }
                return this.minorTicks;
            }
        }

        [Description("Minimum X value for the specified Axis."), Browsable(false)]
        public double MinXValue
        {
            get
            {
                return base.chart.MinXValue(this);
            }
        }

        [Description("Minimum Y value for the specified Axis."), Browsable(false)]
        public double MinYValue
        {
            get
            {
                return base.chart.MinYValue(this);
            }
        }

        [Category("Axis Layout"), Description("Sets the Axis to other side (Left or Right, Top or Bottom)")]
        public bool OtherSide
        {
            get
            {
                return this.otherSide;
            }
            set
            {
                base.SetBooleanProperty(ref this.otherSide, value);
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), Obsolete("Please use Position property")]
        public int PosAxis
        {
            get
            {
                return this.posAxis;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(0), Category("Axis Layout"), Description("The screen co-ordinate where axis is drawn.")]
        public int Position
        {
            get
            {
                return this.posAxis;
            }
        }

        [Description("Defines axis Position units (pixels or percentage)"), Category("Axis Layout"), DefaultValue(0)]
        public Steema.TeeChart.PositionUnits PositionUnits
        {
            get
            {
                return this.positionUnits;
            }
            set
            {
                if (this.positionUnits != value)
                {
                    this.positionUnits = value;
                    this.Invalidate();
                }
            }
        }

        [Description("Axis position as percent of Chart."), Category("Axis Layout"), DefaultValue((double) 0.0)]
        public double RelativePosition
        {
            get
            {
                return this.relativePosition;
            }
            set
            {
                base.SetDoubleProperty(ref this.relativePosition, value);
            }
        }

        [Category("Axis Layout"), Description("Axis Start position as percentage of Chart."), DefaultValue((double) 0.0)]
        public double StartPosition
        {
            get
            {
                return this.startPosition;
            }
            set
            {
                base.SetDoubleProperty(ref this.startPosition, value);
            }
        }

        [DefaultValue(true), Description(" Ticks only coincide at Labels."), Category("Axis Ticks")]
        public bool TickOnLabelsOnly
        {
            get
            {
                return this.tickonlabelsonly;
            }
            set
            {
                base.SetBooleanProperty(ref this.tickonlabelsonly, value);
            }
        }

        [Category("Axis Ticks"), Description("Pen to draw Axis Ticks along the Axis line."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TicksPen Ticks
        {
            get
            {
                if (this.ticks == null)
                {
                    this.ticks = new TicksPen(base.chart);
                    this.ticks.length = 4;
                    this.ticks.defaultLength = 4;
                }
                return this.ticks;
            }
        }

        [Description("Pen to draw Axis marks inside the Axis line."), Category("Axis Ticks"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TicksPen TicksInner
        {
            get
            {
                if (this.ticksInner == null)
                {
                    this.ticksInner = new TicksPen(base.chart);
                }
                return this.ticksInner;
            }
        }

        [Description("Title attributes."), Category("Axis"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public AxisTitle Title
        {
            get
            {
                if (this.axisTitle == null)
                {
                    this.axisTitle = new AxisTitle(base.chart);
                }
                return this.axisTitle;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(0), Obsolete("Please use Axis.Title.CustomSize property."), Category("Axis")]
        public int TitleSize
        {
            get
            {
                return this.Title.customSize;
            }
            set
            {
                this.Title.CustomSize = value;
            }
        }

        [DefaultValue(true), Description("Show/hide all Axes."), Category("Axis")]
        public bool Visible
        {
            get
            {
                return this.bVisible;
            }
            set
            {
                base.SetBooleanProperty(ref this.bVisible, value);
            }
        }

        [Category("Axis Layout"), Description("Determines the Z axis position along chart Depth")]
        public double ZPosition
        {
            get
            {
                return this.zPosition;
            }
            set
            {
                base.SetDoubleProperty(ref this.zPosition, value);
            }
        }

        internal protected class AxisDraw : TeeBase
        {
            internal Axis axis;
            private TicksGridDraw drawTicksAndGrid;
            private double iIncrement;
            private AxisLabelStyle tmpLabelStyle;
            internal int tmpNumTicks;
            internal int[] tmpTicks;
            internal double tmpValue;
            private DateTimeSteps tmpWhichDatetime;

            public AxisDraw(Axis a) : base(a.chart)
            {
                this.axis = a;
                this.drawTicksAndGrid = new TicksGridDraw(this.axis);
            }

            private void AddTick(int aPos)
            {
                this.tmpTicks[this.tmpNumTicks] = aPos;
                this.tmpNumTicks++;
            }

            private void AxisLabelsSeries(Rectangle rect)
            {
                int num3;
                int num4;
                int numLines = 0;
                string aLabel = "";
                double aValue = 0.0;
                this.CalcAllSeries();
                this.CalcFirstLastAllSeries(out num3, out num4, rect);
                if (num3 != 0x7fffffff)
                {
                    int num7 = -1;
                    int num8 = 0;
                    bool horizontal = this.axis.horizontal;
                    switch (this.axis.labels.iAngle)
                    {
                        case 90:
                        case 270:
                            horizontal = !horizontal;
                            break;
                    }
                    for (int i = num3; i <= num4; i++)
                    {
                        if ((this.GetAxisSeriesLabel(i, ref aValue, ref aLabel) && (aValue >= this.axis.iMinimum)) && (aValue <= this.axis.iMaximum))
                        {
                            int aPos = this.axis.CalcPosValue(aValue);
                            if (!this.axis.TickOnLabelsOnly)
                            {
                                this.AddTick(aPos);
                            }
                            if (this.axis.labels.Visible && (aLabel.Length != 0))
                            {
                                int num6 = this.axis.chart.MultiLineTextWidth(aLabel, out numLines);
                                if (!horizontal)
                                {
                                    num6 = base.chart.graphics3D.FontHeight * numLines;
                                }
                                if ((this.axis.labels.iSeparation != 0) && (num7 != -1))
                                {
                                    bool flag;
                                    num6 += (int) ((0.02 * num6) * this.axis.Labels.iSeparation);
                                    num6 /= 2;
                                    if (aPos >= num7)
                                    {
                                        flag = (aPos - num6) >= (num7 + num8);
                                    }
                                    else
                                    {
                                        flag = (aPos + num6) <= (num7 - num8);
                                    }
                                    if (flag)
                                    {
                                        this.DrawThisLabel(aPos, aLabel, null);
                                        num7 = aPos;
                                        num8 = num6;
                                    }
                                }
                                else
                                {
                                    this.DrawThisLabel(aPos, aLabel, null);
                                    num7 = aPos;
                                    num8 = num6 / 2;
                                }
                            }
                        }
                    }
                    this.axis.iSeriesList.Clear(false);
                }
            }

            private void CalcAllSeries()
            {
                if (this.axis.iSeriesList == null)
                {
                    this.axis.iSeriesList = new SeriesCollection(base.chart);
                }
                this.axis.iSeriesList.Clear();
                foreach (Series series in this.axis.chart.Series)
                {
                    if (series.Active && series.AssociatedToAxis(this.axis))
                    {
                        this.axis.iSeriesList.InternalAdd(series);
                    }
                }
            }

            private void CalcFirstLastAllSeries(out int tmpFirst, out int tmpLast, Rectangle rect)
            {
                tmpFirst = 0x7fffffff;
                tmpLast = -1;
                foreach (Series series in this.axis.iSeriesList)
                {
                    series.CalcFirstLastVisibleIndex();
                    if ((series.firstVisible < tmpFirst) && (series.firstVisible != -1))
                    {
                        tmpFirst = series.firstVisible;
                    }
                    if (series.lastVisible > tmpLast)
                    {
                        tmpLast = series.lastVisible;
                    }
                }
            }

            private void DepthAxisLabels()
            {
                if (this.axis.chart.CountActiveSeries() > 0)
                {
                    for (int i = (int) this.axis.iMinimum; i <= ((int) this.axis.iMaximum); i++)
                    {
                        int aPos = this.axis.CalcYPosValue((this.axis.iMaximum - i) - 0.5);
                        if (!this.axis.TickOnLabelsOnly)
                        {
                            this.AddTick(aPos);
                        }
                        if (this.axis.Labels.Visible)
                        {
                            string labelText = this.axis.chart.SeriesTitleLegend(i, true);
                            if (this.axis.chart.parent != null)
                            {
                                this.axis.chart.parent.DoGetAxisLabel(this.axis, null, i, ref labelText);
                            }
                            this.DrawThisLabel(aPos, labelText, null);
                        }
                    }
                }
            }

            private void DoCustomLabels()
            {
                int labelIndex = 0;
                bool stop = true;
                bool flag2 = false;
                this.tmpValue = this.axis.iMinimum;
            Label_0017:
                if (this.axis.chart.parent != null)
                {
                    this.axis.chart.parent.DoGetNextAxisLabel(this.axis, labelIndex, ref this.tmpValue, ref stop);
                }
                if (stop)
                {
                    if (labelIndex == 0)
                    {
                        this.DoNotCustomLabels();
                    }
                }
                else
                {
                    flag2 = (this.tmpValue >= (this.axis.iMinimum - 1E-07)) && (this.tmpValue <= (this.axis.iMaximum + 1E-07));
                    if (flag2)
                    {
                        this.InternalDrawLabel(false);
                    }
                    labelIndex++;
                    if ((flag2 || (labelIndex <= 0x7d0)) || !stop)
                    {
                        goto Label_0017;
                    }
                }
            }

            private void DoDefaultLabels()
            {
                double num = 0.0;
                this.tmpValue = this.axis.iMaximum / this.iIncrement;
                if (Math.Abs((double) (this.axis.iRange / this.iIncrement)) < 10000.0)
                {
                    if ((this.axis.iAxisDateTime && this.axis.Labels.ExactDateTime) && ((this.tmpWhichDatetime != DateTimeSteps.None) && (this.tmpWhichDatetime > DateTimeSteps.OneDay)))
                    {
                        this.tmpValue = Utils.DateTime(RoundDate(Utils.DateTime(this.axis.iMaximum), this.tmpWhichDatetime));
                    }
                    else if ((Convert.ToString(this.axis.iMinimum) == Convert.ToString(this.axis.iMaximum)) || !this.axis.labels.RoundFirstLabel)
                    {
                        this.tmpValue = this.axis.iMaximum;
                    }
                    else
                    {
                        this.tmpValue = this.iIncrement * Convert.ToDouble(decimal.Truncate(Convert.ToDecimal(this.tmpValue)));
                    }
                    while (this.tmpValue > this.axis.iMaximum)
                    {
                        this.axis.IncDecDateTime(false, ref this.tmpValue, this.iIncrement, this.tmpWhichDatetime);
                    }
                    if (this.axis.iRangezero)
                    {
                        this.InternalDrawLabel(false);
                    }
                    else
                    {
                        if (Math.Abs((double) (this.axis.iMaximum - this.axis.iMinimum)) >= this.axis.iMinAxisIncrement)
                        {
                            double num2 = this.tmpValue - this.iIncrement;
                            if (!(this.tmpValue.ToString() == num2.ToString()))
                            {
                                num = (this.axis.iMinimum - this.axis.iMinAxisIncrement) / (1.0 + this.axis.iMinAxisIncrement);
                                while (this.tmpValue >= num)
                                {
                                    this.InternalDrawLabel(true);
                                }
                                return;
                            }
                        }
                        this.InternalDrawLabel(false);
                    }
                }
            }

            private void DoDefaultLogLabels()
            {
                if (this.axis.iMinimum != this.axis.iMaximum)
                {
                    if (this.axis.iMinimum <= 0.0)
                    {
                        if (this.axis.iMinimum == 0.0)
                        {
                            this.axis.iMinimum = 0.1;
                        }
                        else
                        {
                            this.axis.iMinimum = 1E-10;
                        }
                        this.tmpValue = this.axis.iMinimum;
                    }
                    else
                    {
                        this.tmpValue = this.IntPower(this.axis.LogarithmicBase, (int) Math.Round(this.LogBaseN(this.axis.logarithmicBase, this.axis.iMinimum)));
                    }
                    if (this.axis.MinorGrid.Visible)
                    {
                        double tmpValue = this.tmpValue;
                        if (tmpValue >= this.axis.iMinimum)
                        {
                            tmpValue = this.IntPower(this.axis.logarithmicBase, ((int) Math.Round(this.LogBaseN(this.axis.logarithmicBase, this.axis.iMinimum))) - 1);
                        }
                        if (tmpValue < this.axis.iMinimum)
                        {
                            this.AddTick(this.axis.CalcPosValue(tmpValue));
                        }
                    }
                    if (this.axis.logarithmicBase > 1.0)
                    {
                        while (this.tmpValue <= this.axis.iMaximum)
                        {
                            if (this.tmpValue >= this.axis.iMinimum)
                            {
                                this.InternalDrawLabel(false);
                            }
                            this.tmpValue *= this.axis.logarithmicBase;
                        }
                    }
                    if (this.axis.MinorGrid.Visible && (this.tmpValue > this.axis.iMaximum))
                    {
                        this.AddTick(this.axis.CalcPosValue(this.tmpValue));
                    }
                }
            }

            private void DoNotCustomLabels()
            {
                if (this.axis.logarithmic && (this.axis.desiredIncrement == 0.0))
                {
                    this.DoDefaultLogLabels();
                }
                else
                {
                    this.DoDefaultLabels();
                }
            }

            public void Draw(bool calcPosAxis)
            {
                this.axis.iAxisDateTime = this.axis.IsDateTime;
                if (calcPosAxis)
                {
                    this.axis.posAxis = this.axis.ApplyPosition(this.axis.GetRectangleEdge(this.axis.chart.ChartRect), this.axis.chart.ChartRect);
                }
                this.DrawAxisTitle();
                this.tmpNumTicks = 0;
                this.tmpTicks = new int[0x7d0];
                if (this.axis.Labels.Items.Count != 0)
                {
                    this.DrawCustomLabels();
                }
                else
                {
                    this.tmpLabelStyle = this.axis.CalcLabelStyle();
                    if (this.tmpLabelStyle != AxisLabelStyle.None)
                    {
                        base.chart.graphics3D.Font = this.axis.labels.Font;
                        this.iIncrement = this.axis.CalcIncrement;
                        if ((this.axis.iAxisDateTime && this.axis.Labels.ExactDateTime) && (this.axis.desiredIncrement != 0.0))
                        {
                            this.tmpWhichDatetime = Axis.FindDateTimeStep(this.axis.desiredIncrement);
                            if (this.tmpWhichDatetime != DateTimeSteps.None)
                            {
                                while ((this.iIncrement > Utils.DateTimeStep[Convert.ToInt16((int) this.tmpWhichDatetime)]) && (this.tmpWhichDatetime != DateTimeSteps.OneYear))
                                {
                                    this.tmpWhichDatetime += 1;
                                }
                            }
                        }
                        else
                        {
                            this.tmpWhichDatetime = DateTimeSteps.None;
                        }
                        if (((this.iIncrement > 0.0) || ((this.tmpWhichDatetime >= DateTimeSteps.HalfMonth) && (this.tmpWhichDatetime <= DateTimeSteps.OneYear))) && (this.axis.iMaximum >= this.axis.iMinimum))
                        {
                            switch (this.tmpLabelStyle)
                            {
                                case AxisLabelStyle.Value:
                                    if ((this.axis.chart.parent == null) || this.axis.chart.parent.CheckGetAxisLabelAssigned())
                                    {
                                        this.DoNotCustomLabels();
                                    }
                                    else
                                    {
                                        this.DoCustomLabels();
                                    }
                                    goto Label_025B;

                                case AxisLabelStyle.Mark:
                                    this.AxisLabelsSeries(this.axis.chart.ChartRect);
                                    goto Label_025B;

                                case AxisLabelStyle.Text:
                                    if (!this.axis.IsDepthAxis)
                                    {
                                        this.AxisLabelsSeries(this.axis.chart.ChartRect);
                                    }
                                    else
                                    {
                                        this.DepthAxisLabels();
                                    }
                                    goto Label_025B;
                            }
                        }
                    }
                }
            Label_025B:
                this.drawTicksAndGrid.DrawTicksGrid(ref this.tmpTicks, ref this.tmpNumTicks, ref this.tmpValue);
            }

            private void DrawAxisTitle()
            {
                if (((this.axis.axisTitle != null) && this.axis.axisTitle.Visible) && (this.axis.axisTitle.Caption.Length != 0))
                {
                    Rectangle chartRect = this.axis.chart.ChartRect;
                    if (this.axis.IsDepthAxis)
                    {
                        this.axis.DrawTitle(this.axis.posTitle, chartRect.Bottom);
                    }
                    else if (this.axis.horizontal)
                    {
                        this.axis.DrawTitle(this.axis.iCenterPos, this.axis.posTitle);
                    }
                    else
                    {
                        this.axis.DrawTitle(this.axis.posTitle, this.axis.iCenterPos);
                    }
                }
            }

            private void DrawCustomLabels()
            {
                for (int i = 0; i < this.axis.Labels.Items.Count; i++)
                {
                    AxisLabelItem labelItem = this.axis.Labels.Items[i];
                    if ((labelItem.Value >= this.axis.Minimum) && (labelItem.Value <= this.axis.Maximum))
                    {
                        int aPos = this.axis.CalcPosValue(labelItem.Value);
                        if (!this.axis.TickOnLabelsOnly)
                        {
                            this.AddTick(aPos);
                        }
                        if (labelItem.Visible)
                        {
                            string text = labelItem.Text;
                            if (text == "")
                            {
                                text = this.axis.Labels.LabelValue(labelItem.Value);
                            }
                            this.DrawThisLabel(aPos, text, labelItem);
                        }
                    }
                }
            }

            private void DrawThisLabel(int labelPos, string tmpSt, TextShape labelItem)
            {
                if (this.axis.tickonlabelsonly)
                {
                    this.AddTick(labelPos);
                }
                Chart chart = this.axis.chart;
                Graphics3D graphicsd = chart.graphics3D;
                graphicsd.Font = (labelItem == null) ? this.axis.labels.Font : labelItem.Font;
                graphicsd.Brush.Visible = false;
                if (this.axis.IsDepthAxis)
                {
                    int num;
                    graphicsd.TextAlign = StringAlignment.Near;
                    if ((chart.Aspect.Rotation == 360) || chart.Aspect.Orthogonal)
                    {
                        num = labelPos + (graphicsd.FontHeight / 2);
                    }
                    else
                    {
                        num = labelPos;
                    }
                    graphicsd.TextOut(this.axis.labels.position, chart.ChartRectBottom, num, tmpSt);
                }
                else if (this.axis.horizontal)
                {
                    this.axis.DrawAxisLabel(labelPos, this.axis.labels.position, this.axis.labels.iAngle, tmpSt, labelItem);
                }
                else
                {
                    this.axis.DrawAxisLabel(this.axis.labels.position, labelPos, this.axis.labels.iAngle, tmpSt, labelItem);
                }
            }

            private bool GetAxisSeriesLabel(int aIndex, ref double aValue, ref string aLabel)
            {
                Chart chart = this.axis.chart;
                for (int i = 0; i < this.axis.iSeriesList.Count; i++)
                {
                    Series series = this.axis.iSeriesList[i];
                    if ((aIndex >= series.firstVisible) && (aIndex <= series.lastVisible))
                    {
                        switch (this.tmpLabelStyle)
                        {
                            case AxisLabelStyle.Mark:
                                aLabel = series.ValueMarkText(aIndex);
                                break;

                            case AxisLabelStyle.Text:
                                aLabel = series.Labels[aIndex];
                                break;
                        }
                        if (this.axis.chart.parent != null)
                        {
                            this.axis.chart.parent.DoGetAxisLabel(this.axis, this.axis.iSeriesList[i], aIndex, ref aLabel);
                        }
                        if (aLabel.Length != 0)
                        {
                            if (this.axis.horizontal)
                            {
                                aValue = series.XValues[aIndex];
                            }
                            else
                            {
                                aValue = series.YValues[aIndex];
                            }
                            return true;
                        }
                    }
                }
                return false;
            }

            private void InternalDrawLabel(bool decValue)
            {
                int aPos = this.axis.CalcPosValue(this.tmpValue);
                if (this.axis.labels.bOnAxis || ((aPos > this.axis.IStartPos) && (aPos < this.axis.IEndPos)))
                {
                    if (!this.axis.TickOnLabelsOnly)
                    {
                        this.AddTick(aPos);
                    }
                    if (this.axis.labels.Visible)
                    {
                        this.DrawThisLabel(aPos, this.axis.labels.LabelValue(this.tmpValue), null);
                    }
                }
                if (decValue)
                {
                    this.axis.IncDecDateTime(false, ref this.tmpValue, this.iIncrement, this.tmpWhichDatetime);
                }
            }

            private double IntPower(double numBase, int exponent)
            {
                return Math.Pow(numBase, (double) exponent);
            }

            private double LogBaseN(double numBase, double x)
            {
                return Math.Log(x, numBase);
            }

            private static DateTime RoundDate(DateTime aDate, DateTimeSteps aStep)
            {
                if (aDate <= DateTime.MinValue)
                {
                    return aDate;
                }
                int year = aDate.Year;
                int month = aDate.Month;
                int day = aDate.Day;
                switch (aStep)
                {
                    case DateTimeSteps.HalfMonth:
                        if (day < 15)
                        {
                            day = 1;
                            break;
                        }
                        day = 15;
                        break;

                    case DateTimeSteps.OneMonth:
                    case DateTimeSteps.TwoMonths:
                    case DateTimeSteps.ThreeMonths:
                    case DateTimeSteps.FourMonths:
                    case DateTimeSteps.SixMonths:
                        day = 1;
                        break;

                    case DateTimeSteps.OneYear:
                        day = 1;
                        month = 1;
                        break;
                }
                return new DateTime(year, month, day);
            }

            private sealed class TicksGridDraw
            {
                private Aspect a;
                internal Axis axis;
                private Chart c;
                private Graphics3D g;
                private bool is3D;
                private Rectangle r;
                private int tmpNumTicks;
                private int[] tmpTicks;
                private int tmpWallSize;

                public TicksGridDraw(Axis a)
                {
                    this.axis = a;
                    this.c = this.axis.chart;
                }

                private void AProc(int aPos, bool isGrid)
                {
                    if ((aPos > this.axis.IStartPos) && (aPos < this.axis.IEndPos))
                    {
                        if (isGrid)
                        {
                            this.DrawGridLine(aPos);
                        }
                        else
                        {
                            this.InternalDrawTick(aPos, 1, this.axis.minorTicks.length);
                        }
                    }
                }

                private void DrawAxisLine()
                {
                    if (this.axis.IsDepthAxis)
                    {
                        int num = (this.r.Bottom + this.c.Walls.CalcWallSize(this.c.axes.Bottom)) - this.axis.iZPos;
                        this.g.Line(this.axis.posAxis, num, this.axis.IStartPos, this.axis.posAxis, num, this.axis.IEndPos);
                    }
                    else if (this.axis.horizontal)
                    {
                        if (this.axis.otherSide)
                        {
                            this.g.HorizontalLine(this.axis.IStartPos, this.axis.IEndPos, this.axis.posAxis, this.axis.iZPos);
                        }
                        else
                        {
                            this.g.HorizontalLine(this.axis.IStartPos - this.c.Walls.CalcWallSize(this.c.Axes.Left), this.axis.IEndPos, this.axis.posAxis + this.tmpWallSize, this.axis.iZPos);
                        }
                    }
                    else
                    {
                        int num2 = this.axis.otherSide ? this.tmpWallSize : -this.tmpWallSize;
                        this.g.VerticalLine(this.axis.posAxis + num2, this.axis.IStartPos, this.axis.IEndPos + this.c.Walls.CalcWallSize(this.c.axes.Bottom), this.axis.iZPos);
                    }
                }

                private void DrawGridLine(int tmp)
                {
                    if ((tmp > this.axis.IStartPos) && (tmp < this.axis.IEndPos))
                    {
                        if (this.axis.IsDepthAxis)
                        {
                            this.g.VerticalLine(this.r.X, this.r.Y, this.r.Bottom, tmp);
                            this.g.HorizontalLine(this.r.X, this.r.Right, this.r.Bottom, tmp);
                        }
                        else if (this.axis.horizontal)
                        {
                            if (this.is3D)
                            {
                                if (this.axis.otherSide)
                                {
                                    this.g.VerticalLine(tmp, this.r.Y, this.r.Bottom, this.a.Width3D);
                                }
                                else if (this.c.axes.DrawBehind)
                                {
                                    this.g.ZLine(tmp, this.axis.posAxis, 0, this.a.Width3D);
                                    this.g.VerticalLine(tmp, this.r.Y, this.r.Bottom, this.a.Width3D);
                                }
                                else
                                {
                                    this.g.VerticalLine(tmp, this.r.Y, this.r.Bottom, 0);
                                }
                            }
                            else
                            {
                                this.g.VerticalLine(tmp, this.r.Y + 1, this.r.Bottom);
                            }
                        }
                        else if (this.is3D)
                        {
                            if (this.axis.otherSide)
                            {
                                this.g.HorizontalLine(this.r.X, this.r.Right, tmp, this.a.Width3D);
                            }
                            else if (!this.c.axes.DrawBehind)
                            {
                                this.g.HorizontalLine(this.r.X, this.r.Right, tmp, 0);
                            }
                            else
                            {
                                this.g.ZLine(this.axis.posAxis, tmp, 0, this.a.Width3D);
                                if (!this.axis.iHideBackGrid)
                                {
                                    this.g.HorizontalLine(this.r.X, this.r.Right, tmp, this.a.Width3D);
                                }
                            }
                        }
                        else
                        {
                            this.g.HorizontalLine(this.r.X + 1, this.r.Right, tmp);
                        }
                    }
                }

                private void DrawGrids()
                {
                    this.g.Pen = this.axis.Grid;
                    if (this.g.Pen.Color.IsEmpty)
                    {
                        this.g.Pen.Color = Color.WhiteSmoke;
                    }
                    for (int i = 0; i < this.tmpNumTicks; i++)
                    {
                        if (this.axis.grid.centered)
                        {
                            if (i > 0)
                            {
                                this.DrawGridLine((int) (0.5 * (this.tmpTicks[i] + this.tmpTicks[i - 1])));
                            }
                        }
                        else
                        {
                            this.DrawGridLine(this.tmpTicks[i]);
                        }
                    }
                }

                public void DrawTicksGrid(ref int[] tempTicks, ref int tempNumTicks, ref double tempValue)
                {
                    this.c = this.axis.chart;
                    this.g = this.c.graphics3D;
                    this.a = this.c.aspect;
                    this.is3D = this.a.view3D;
                    this.tmpTicks = tempTicks;
                    this.tmpNumTicks = tempNumTicks;
                    this.r = this.c.ChartRect;
                    this.g.Brush.Visible = false;
                    this.tmpWallSize = this.c.Walls.CalcWallSize(this.axis);
                    if (this.axis.axispen.Visible)
                    {
                        this.g.Pen = this.axis.axispen;
                        this.DrawAxisLine();
                    }
                    this.ProcessTicks(this.axis.Ticks, 1, this.axis.ticks.length);
                    if (this.axis.Grid.Visible)
                    {
                        this.DrawGrids();
                    }
                    this.ProcessTicks(this.axis.TicksInner, -1, -this.axis.ticksInner.length);
                    this.ProcessMinor(this.axis.MinorTicks, false);
                    this.ProcessMinor(this.axis.MinorGrid, true);
                }

                private void InternalDrawTick(int tmp, int delta, int tmpTickLength)
                {
                    if (this.axis.IsDepthAxis)
                    {
                        if (this.is3D)
                        {
                            this.g.HorizontalLine(this.axis.posAxis + delta, (this.axis.posAxis + delta) + tmpTickLength, this.r.Bottom, tmp);
                        }
                        else
                        {
                            this.g.HorizontalLine(this.axis.posAxis + delta, (this.axis.posAxis + delta) + tmpTickLength, this.r.Bottom);
                        }
                    }
                    else if (this.axis.otherSide)
                    {
                        if (this.axis.horizontal)
                        {
                            if (this.is3D)
                            {
                                this.g.VerticalLine(tmp, this.axis.posAxis - delta, (this.axis.posAxis - delta) - tmpTickLength, this.axis.iZPos);
                            }
                            else
                            {
                                this.g.VerticalLine(tmp, this.axis.posAxis - delta, (this.axis.posAxis - delta) - tmpTickLength);
                            }
                        }
                        else if (this.is3D)
                        {
                            this.g.HorizontalLine(this.axis.posAxis + delta, (this.axis.posAxis + delta) + tmpTickLength, tmp, this.axis.iZPos);
                        }
                        else
                        {
                            this.g.HorizontalLine(this.axis.posAxis + delta, (this.axis.posAxis + delta) + tmpTickLength, tmp);
                        }
                    }
                    else
                    {
                        delta += this.tmpWallSize;
                        if (this.axis.horizontal)
                        {
                            if (this.is3D)
                            {
                                this.g.VerticalLine(tmp, this.axis.posAxis + delta, (this.axis.posAxis + delta) + tmpTickLength, this.axis.iZPos);
                            }
                            else
                            {
                                this.g.VerticalLine(tmp, this.axis.posAxis + delta, (this.axis.posAxis + delta) + tmpTickLength);
                            }
                        }
                        else if (this.is3D)
                        {
                            this.g.HorizontalLine(this.axis.posAxis - delta, (this.axis.posAxis - delta) - tmpTickLength, tmp, this.axis.iZPos);
                        }
                        else
                        {
                            this.g.HorizontalLine(this.axis.posAxis - delta, (this.axis.posAxis - delta) - tmpTickLength, tmp);
                        }
                    }
                }

                private void ProcessMinor(ChartPen aPen, bool isGrid)
                {
                    if ((this.tmpNumTicks > 0) && aPen.Visible)
                    {
                        this.g.Pen = aPen;
                        this.ProcessMinorTicks(isGrid);
                    }
                }

                private void ProcessMinorTicks(bool isGrid)
                {
                    double num2 = 1.0 / ((double) (this.axis.minorTickCount + 1));
                    if (this.tmpNumTicks > 1)
                    {
                        double num;
                        if (!this.axis.logarithmic)
                        {
                            num = (1.0 * (this.tmpTicks[1] - this.tmpTicks[0])) * num2;
                            for (int j = 1; j <= this.axis.minorTickCount; j++)
                            {
                                this.AProc(this.tmpTicks[0] - ((int) (j * num)), isGrid);
                                this.AProc(this.tmpTicks[this.tmpNumTicks - 1] + ((int) (j * num)), isGrid);
                            }
                        }
                        for (int i = 1; i <= (this.tmpNumTicks - 1); i++)
                        {
                            if (this.axis.logarithmic)
                            {
                                double num5 = this.axis.CalcPosPoint(this.tmpTicks[i - 1]);
                                num = ((num5 * this.axis.LogarithmicBase) - num5) * num2;
                                for (int k = 1; k < this.axis.minorTickCount; k++)
                                {
                                    num5 += num;
                                    if (num5 > this.axis.iMaximum)
                                    {
                                        goto Label_016D;
                                    }
                                    this.AProc(this.axis.CalcPosValue(num5), isGrid);
                                }
                            Label_016D:;
                            }
                            else
                            {
                                num = (1.0 * (this.tmpTicks[i] - this.tmpTicks[i - 1])) * num2;
                                for (int m = 1; m <= this.axis.minorTickCount; m++)
                                {
                                    this.AProc(this.tmpTicks[i] - ((int) (m * num)), isGrid);
                                }
                            }
                        }
                    }
                }

                private void ProcessTicks(ChartPen aPen, int aOffset, int aLength)
                {
                    if (aPen.Visible && (aLength != 0))
                    {
                        this.g.Pen = aPen;
                        for (int i = 0; i < this.tmpNumTicks; i++)
                        {
                            this.InternalDrawTick(this.tmpTicks[i], aOffset, aLength);
                        }
                    }
                }
            }
        }

        public sealed class AxisLinePen : ChartPen
        {
            public AxisLinePen(Chart c) : base(c, Color.Black)
            {
                base.Width = 2;
            }

            [DefaultValue(2), Description("Sets the width of the AxisLinePen")]
            public override int Width
            {
                get
                {
                    return base.Width;
                }
                set
                {
                    base.Width = value;
                }
            }
        }

        internal sealed class Designer : ComponentDesigner
        {
            public Designer()
            {
                this.Verbs.Add(new DesignerVerb(Texts.Edit, new EventHandler(this.OnEdit)));
            }

            private void OnEdit(object sender, EventArgs e)
            {
                if (EditorUtils.ShowFormModal(new AxisEditor(this.Axis, null)))
                {
                    base.RaiseComponentChanged(null, null, null);
                }
            }

            private Steema.TeeChart.Axis Axis
            {
                get
                {
                    return (Steema.TeeChart.Axis) base.Component;
                }
            }
        }

        internal sealed class Editor : UITypeEditor
        {
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                Axis a = (Axis) value;
                bool flag = EditorUtils.ShowFormModal(new AxisEditor(a, null));
                if ((context != null) && flag)
                {
                    context.OnComponentChanged();
                }
                return value;
            }

            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.Modal;
            }
        }

        [Description("Pen to draw Grid lines at every Axis Label position.")]
        public class GridPen : ChartPen
        {
            internal bool centered;

            public GridPen(Chart c) : base(c)
            {
                base.defaultColor = Color.DarkGray;
                base.Color = base.defaultColor;
                this.Style = DashStyle.Dot;
                base.defaultStyle = DashStyle.Dot;
            }

            [DefaultValue(false), Description("Aligns the Grid to the centre.")]
            public bool Centered
            {
                get
                {
                    return this.centered;
                }
                set
                {
                    base.SetBooleanProperty(ref this.centered, value);
                }
            }

            [Description("Sets the style of the grid lines."), DefaultValue(2)]
            public override DashStyle Style
            {
                get
                {
                    return base.Style;
                }
                set
                {
                    base.Style = value;
                }
            }
        }

        [Description("Pen to draw Axis marks along the Axis line.")]
        public class TicksPen : ChartPen
        {
            internal int defaultLength;
            internal int length;

            public TicksPen(Chart c) : base(c)
            {
                base.defaultColor = Color.DarkGray;
                base.cColor = base.defaultColor;
            }

            private bool ShouldSerializeLength()
            {
                return (this.length != this.defaultLength);
            }

            [Description("Length of Axis Ticks in pixels.")]
            public int Length
            {
                get
                {
                    return this.length;
                }
                set
                {
                    base.SetIntegerProperty(ref this.length, value);
                }
            }
        }
    }
}

