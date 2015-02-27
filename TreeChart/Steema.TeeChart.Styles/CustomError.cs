namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    public class CustomError : Bar
    {
        protected bool bDrawBar;
        private Drawer d;
        private ChartPen errorPen;
        private ValueList errorValues;
        private int errorWidth;
        private Steema.TeeChart.Styles.ErrorWidthUnits errorWidthUnits;
        protected ErrorStyles iErrorStyle;

        public CustomError() : this(null)
        {
        }

        public CustomError(Chart c) : base(c)
        {
            this.iErrorStyle = ErrorStyles.TopBottom;
            this.errorWidth = 100;
            this.errorWidthUnits = Steema.TeeChart.Styles.ErrorWidthUnits.Percent;
            this.bDrawBar = false;
            this.d = new Drawer();
            this.errorValues = new ValueList(this, Texts.ValuesStdError);
            base.Marks.Visible = false;
        }

        public int Add(double x, double y, double errorValue)
        {
            return this.Add(x, y, errorValue, "", Color.Empty);
        }

        public int Add(double x, double y, double errorValue, Color color)
        {
            return this.Add(x, y, errorValue, "", color);
        }

        public int Add(double x, double y, double errorValue, string text)
        {
            return this.Add(x, y, errorValue, text, Color.Empty);
        }

        public int Add(double x, double y, double errorValue, string text, Color color)
        {
            this.errorValues.TempValue = errorValue;
            return base.Add(x, y, text, color);
        }

        protected override void AddSampleValues(int numValues)
        {
            Series.SeriesRandom random = base.RandomBounds(numValues);
            for (int i = 1; i <= numValues; i++)
            {
                this.Add(random.tmpX, (double) Utils.Round((double) (random.DifY * random.Random())), random.DifY / (20.0 + random.Random()));
                random.tmpX += random.StepX;
            }
        }

        public int AddValue(double y, double errorValue)
        {
            return this.Add((double) base.Count, y, errorValue, "", Color.Empty);
        }

        protected internal override void CalcHorizMargins(ref int LeftMargin, ref int RightMargin)
        {
            base.CalcHorizMargins(ref LeftMargin, ref RightMargin);
            int num = Utils.Round((float) this.ErrorPen.Width);
            if ((this.iErrorStyle == ErrorStyles.Left) || (this.iErrorStyle == ErrorStyles.LeftRight))
            {
                LeftMargin = Math.Max(LeftMargin, num);
            }
            if ((this.iErrorStyle == ErrorStyles.Right) || (this.iErrorStyle == ErrorStyles.LeftRight))
            {
                RightMargin = Math.Max(RightMargin, num);
            }
        }

        protected internal override void CalcVerticalMargins(ref int TopMargin, ref int BottomMargin)
        {
            base.CalcVerticalMargins(ref TopMargin, ref BottomMargin);
            int num = Utils.Round((float) this.ErrorPen.Width);
            if ((this.iErrorStyle == ErrorStyles.Top) || (this.iErrorStyle == ErrorStyles.TopBottom))
            {
                TopMargin = Math.Max(TopMargin, num);
            }
            if ((this.iErrorStyle == ErrorStyles.Bottom) || (this.iErrorStyle == ErrorStyles.TopBottom))
            {
                BottomMargin = Math.Max(BottomMargin, num);
            }
        }

        public override void DrawBar(int BarIndex, int StartPos, int EndPos)
        {
            if (this.bDrawBar)
            {
                base.DrawBar(BarIndex, StartPos, EndPos);
            }
            if (this.ErrorPen.Visible)
            {
                double num = Math.Abs(this.errorValues[BarIndex]);
                if (num != 0.0)
                {
                    int errorWidth;
                    int num3;
                    int width = base.BarBounds.Width;
                    if (this.errorWidth == 0)
                    {
                        errorWidth = width;
                    }
                    else if (this.errorWidthUnits == Steema.TeeChart.Styles.ErrorWidthUnits.Percent)
                    {
                        errorWidth = Utils.Round((double) ((this.errorWidth * width) * 0.01));
                    }
                    else
                    {
                        errorWidth = this.errorWidth;
                    }
                    int y = base.CalcYPosValue(base.vyValues[BarIndex]);
                    switch (this.iErrorStyle)
                    {
                        case ErrorStyles.Left:
                        case ErrorStyles.Right:
                        case ErrorStyles.LeftRight:
                            num3 = base.CalcXSizeValue(num);
                            break;

                        default:
                            num3 = base.CalcYSizeValue(num);
                            break;
                    }
                    if (this.bDrawBar && (base.vyValues[BarIndex] < base.Origin))
                    {
                        num3 = -num3;
                    }
                    this.PrepareErrorPen(base.chart.graphics3D, BarIndex);
                    this.DrawError(base.chart.graphics3D, (base.BarBounds.Right + base.BarBounds.Left) / 2, y, errorWidth, num3, base.chart.Aspect.View3D);
                }
            }
        }

        private void DrawError(Graphics3D g, int X, int Y, int AWidth, int AHeight, bool draw3D)
        {
            this.d.g = g;
            this.d.X = X;
            this.d.Y = Y;
            this.d.MiddleZ = base.MiddleZ;
            this.d.AWidth = AWidth;
            this.d.draw3D = draw3D;
            switch (this.iErrorStyle)
            {
                case ErrorStyles.Left:
                    this.d.DrawHoriz(X - AHeight);
                    return;

                case ErrorStyles.Right:
                    this.d.DrawHoriz(X + AHeight);
                    return;

                case ErrorStyles.LeftRight:
                    this.d.DrawHoriz(X - AHeight);
                    this.d.DrawHoriz(X + AHeight);
                    return;

                case ErrorStyles.Top:
                    this.d.DrawVert(Y - AHeight);
                    return;

                case ErrorStyles.Bottom:
                    this.d.DrawVert(Y + AHeight);
                    return;

                case ErrorStyles.TopBottom:
                    this.d.DrawVert(Y - AHeight);
                    this.d.DrawVert(Y + AHeight);
                    return;
            }
        }

        protected override void DrawLegendShape(Graphics3D g, int valueIndex, Rectangle r)
        {
            this.PrepareErrorPen(g, valueIndex);
            this.DrawError(g, (r.X + r.Right) / 2, (r.Y + r.Bottom) / 2, r.Width, (r.Height / 2) - 1, false);
        }

        public override double MaxYValue()
        {
            double num3 = this.bDrawBar ? base.MaxYValue() : 0.0;
            for (int i = 0; i < base.Count; i++)
            {
                double num;
                if (this.bDrawBar)
                {
                    double num2 = this.errorValues[i];
                    num = base.vyValues[i];
                    if (num < 0.0)
                    {
                        num -= num2;
                    }
                    else
                    {
                        num += num2;
                    }
                    if (num > num3)
                    {
                        num3 = num;
                    }
                }
                else
                {
                    num = base.vyValues[i] + this.errorValues[i];
                    num3 = (i == 0) ? num : Math.Max(num3, num);
                }
            }
            return num3;
        }

        public override double MinYValue()
        {
            double num3 = this.bDrawBar ? base.MinYValue() : 0.0;
            for (int i = 0; i < base.Count; i++)
            {
                double num;
                if (this.bDrawBar)
                {
                    double num2 = this.errorValues[i];
                    num = base.vyValues[i];
                    if (num < 0.0)
                    {
                        num -= num2;
                    }
                    else
                    {
                        num += num2;
                    }
                    if (num < num3)
                    {
                        num3 = num;
                    }
                }
                else
                {
                    num = base.vyValues[i] - this.errorValues[i];
                    num3 = (i == 0) ? num : Math.Min(num3, num);
                }
            }
            return num3;
        }

        private void PrepareErrorPen(Graphics3D g, int valueIndex)
        {
            g.Pen = this.ErrorPen;
            if ((valueIndex != -1) && !this.bDrawBar)
            {
                g.Pen.Color = this.ValueColor(valueIndex);
            }
        }

        internal override void PrepareForGallery(bool IsEnabled)
        {
            base.PrepareForGallery(IsEnabled);
            this.ErrorPen.Color = IsEnabled ? Color.Red : Color.White;
            base.Color = IsEnabled ? Color.Blue : Color.Silver;
        }

        protected override void SetSeriesColor(Color c)
        {
            base.SetSeriesColor(c);
            if (!this.bDrawBar)
            {
                this.ErrorPen.Color = c;
            }
        }

        [Description("Defines Pen to draw Error T on top of Error Bars."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ChartPen ErrorPen
        {
            get
            {
                if (this.errorPen == null)
                {
                    this.errorPen = new ChartPen(base.chart);
                }
                return this.errorPen;
            }
        }

        [Description("Defines the Error Series Style."), DefaultValue(5)]
        public ErrorStyles ErrorStyle
        {
            get
            {
                return this.iErrorStyle;
            }
            set
            {
                if (this.iErrorStyle != value)
                {
                    this.iErrorStyle = value;
                    this.Invalidate();
                }
            }
        }

        [Description("Gets and sets the Error value for each Bar."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ValueList ErrorValues
        {
            get
            {
                return this.errorValues;
            }
        }

        [Description("Sets the horizontal size of the Error T."), DefaultValue(100)]
        public int ErrorWidth
        {
            get
            {
                return this.errorWidth;
            }
            set
            {
                base.SetIntegerProperty(ref this.errorWidth, value);
            }
        }

        [DefaultValue(0), Description("Sets ErrorWidth in pixels or % of Bar width.")]
        public Steema.TeeChart.Styles.ErrorWidthUnits ErrorWidthUnits
        {
            get
            {
                return this.errorWidthUnits;
            }
            set
            {
                if (this.errorWidthUnits != value)
                {
                    this.errorWidthUnits = value;
                    this.Invalidate();
                }
            }
        }

        private class Drawer
        {
            internal int AWidth;
            internal bool draw3D;
            internal Graphics3D g;
            internal int MiddleZ;
            internal int X;
            internal int Y;

            internal void DrawHoriz(int XPos)
            {
                if (this.draw3D)
                {
                    this.g.HorizontalLine(this.X, XPos, this.Y, this.MiddleZ);
                    this.g.VerticalLine(XPos, this.Y - (this.AWidth / 2), this.Y + (this.AWidth / 2), this.MiddleZ);
                }
                else
                {
                    this.g.HorizontalLine(this.X, XPos, this.Y);
                    this.g.VerticalLine(XPos, this.Y - (this.AWidth / 2), this.Y + (this.AWidth / 2));
                }
            }

            internal void DrawVert(int YPos)
            {
                if (this.draw3D)
                {
                    this.g.VerticalLine(this.X, this.Y, YPos, this.MiddleZ);
                    this.g.HorizontalLine(this.X - (this.AWidth / 2), this.X + (this.AWidth / 2), YPos, this.MiddleZ);
                }
                else
                {
                    this.g.VerticalLine(this.X, this.Y, YPos);
                    this.g.HorizontalLine(this.X - (this.AWidth / 2), this.X + (this.AWidth / 2), YPos);
                }
            }
        }
    }
}

