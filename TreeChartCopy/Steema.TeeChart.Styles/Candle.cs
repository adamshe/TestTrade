namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    [ToolboxBitmap(typeof(Candle), "SeriesIcons.Candle.bmp")]
    public class Candle : OHLC
    {
        private int candleWidth;
        private const int DefaultCandleWidth = 4;
        private Color downCloseColor;
        private Point OldP;
        private bool showCloseTick;
        private bool showOpenTick;
        private CandleStyles style;
        private Color upCloseColor;

        public Candle() : this(null)
        {
        }

        public Candle(Chart c) : base(c)
        {
            this.upCloseColor = Color.White;
            this.downCloseColor = Color.Red;
            this.candleWidth = 4;
            this.style = CandleStyles.CandleStick;
            this.showOpenTick = true;
            this.showCloseTick = true;
            base.Pointer.Draw3D = false;
        }

        private Color CalculateColor(int valueIndex)
        {
            Color color = this.ValueColor(valueIndex);
            if (!(color == base.Color))
            {
                return color;
            }
            if (base.vOpenValues[valueIndex] > base.CloseValues[valueIndex])
            {
                return this.downCloseColor;
            }
            if (base.vOpenValues[valueIndex] < base.CloseValues[valueIndex])
            {
                return this.upCloseColor;
            }
            if (valueIndex == 0)
            {
                return this.upCloseColor;
            }
            if (base.CloseValues[valueIndex - 1] > base.CloseValues[valueIndex])
            {
                return this.downCloseColor;
            }
            if (base.CloseValues[valueIndex - 1] < base.CloseValues[valueIndex])
            {
                return this.upCloseColor;
            }
            return this.ValueColor(valueIndex - 1);
        }

        protected internal override void CreateSubGallery(Series.SubGalleryEventHandler AddSubChart)
        {
            base.CreateSubGallery(AddSubChart);
            AddSubChart(Texts.CandleBar);
            AddSubChart(Texts.CandleNoOpen);
            AddSubChart(Texts.CandleNoClose);
            AddSubChart(Texts.NoBorder);
            AddSubChart(Texts.Line);
        }

        public override void DrawValue(int valueIndex)
        {
            PointerStyles rectangle = PointerStyles.Rectangle;
            base.OnGetPointerStyle(valueIndex, ref rectangle);
            Graphics3D g = base.chart.graphics3D;
            base.point.PrepareCanvas(g, Color.Empty);
            int x = base.CalcXPosValue(base.DateValues[valueIndex]);
            int y = base.CalcYPosValue(base.vOpenValues[valueIndex]);
            int bottom = base.CalcYPosValue(base.vHighValues[valueIndex]);
            int top = base.CalcYPosValue(base.vLowValues[valueIndex]);
            int num5 = base.CalcYPosValue(base.CloseValues[valueIndex]);
            int num6 = this.candleWidth / 2;
            int num7 = this.candleWidth - num6;
            if ((this.style == CandleStyles.CandleStick) || (this.style == CandleStyles.OpenClose))
            {
                if (!base.chart.Aspect.View3D || !base.point.Draw3D)
                {
                    if (this.style == CandleStyles.CandleStick)
                    {
                        if (base.chart.Aspect.View3D)
                        {
                            g.VerticalLine(x, top, bottom, base.MiddleZ);
                        }
                        else
                        {
                            g.VerticalLine(x, top, bottom);
                        }
                    }
                    if (y == num5)
                    {
                        num5--;
                    }
                    g.Brush.Color = this.CalculateColor(valueIndex);
                    if (base.Transparency > 0)
                    {
                        g.Brush.Transparency = base.Transparency;
                    }
                    if (base.chart.aspect.view3D)
                    {
                        g.Rectangle(new Rectangle(x - num6, y, num6 + num7, num5 - y), base.MiddleZ);
                    }
                    else
                    {
                        if (!base.point.Pen.Visible)
                        {
                            if (y < num5)
                            {
                                y--;
                            }
                            else
                            {
                                num5--;
                            }
                        }
                        g.Rectangle(x - num6, y, (x + num7) + 1, num5);
                    }
                }
                else
                {
                    int num8;
                    int num9;
                    if (num5 > y)
                    {
                        num8 = y;
                        num9 = num5;
                    }
                    else
                    {
                        num8 = num5;
                        num9 = y;
                    }
                    if (this.style == CandleStyles.CandleStick)
                    {
                        g.VerticalLine(x, num9, top, base.MiddleZ);
                    }
                    g.Brush.Color = this.CalculateColor(valueIndex);
                    if (y == num5)
                    {
                        g.Pen.Color = this.CalculateColor(valueIndex);
                    }
                    if (base.Transparency > 0)
                    {
                        g.Brush.Transparency = base.Transparency;
                    }
                    g.Cube(x - num6, num8, x + num7, num9, base.StartZ, base.EndZ, base.point.Dark3D);
                    if (this.style == CandleStyles.CandleStick)
                    {
                        g.VerticalLine(x, num8, bottom, base.MiddleZ);
                    }
                }
            }
            else if (this.style == CandleStyles.CandleBar)
            {
                g.Pen = base.point.Pen;
                g.Pen.Color = this.CalculateColor(valueIndex);
                if (!base.chart.Aspect.View3D)
                {
                    g.VerticalLine(x, top, bottom);
                    if (this.showOpenTick)
                    {
                        g.HorizontalLine(x, (x - num6) - 1, y);
                    }
                    if (this.showCloseTick)
                    {
                        g.HorizontalLine(x, (x + num7) + 1, num5);
                    }
                }
                else
                {
                    g.VerticalLine(x, top, bottom, base.MiddleZ);
                    if (this.showOpenTick)
                    {
                        g.HorizontalLine(x, (x - num6) - 1, y, base.MiddleZ);
                    }
                    if (this.showCloseTick)
                    {
                        g.HorizontalLine(x, (x + num7) + 1, num5, base.MiddleZ);
                    }
                }
            }
            else
            {
                Point point = new Point(x, num5);
                int num10 = this.DrawValuesForward() ? base.firstVisible : base.lastVisible;
                if ((valueIndex != num10) && !base.IsNull(valueIndex))
                {
                    g.Pen = this.Pen;
                    g.Pen.Color = this.CalculateColor(valueIndex);
                    if (base.chart.aspect.view3D)
                    {
                        g.Line(this.OldP, point, base.MiddleZ);
                    }
                    else
                    {
                        g.Line(this.OldP, point);
                    }
                }
                this.OldP = point;
            }
        }

        internal override void PrepareForGallery(bool IsEnabled)
        {
            base.PrepareForGallery(IsEnabled);
            base.FillSampleValues(4);
            base.ColorEach = true;
            if (IsEnabled)
            {
                this.upCloseColor = Color.Blue;
            }
            else
            {
                this.upCloseColor = Color.Silver;
                this.downCloseColor = Color.Silver;
                base.point.Pen.Color = Color.Gray;
            }
            base.point.Pen.Width = 2;
            this.candleWidth = 12;
        }

        protected internal override void SetSubGallery(int index)
        {
            switch (index)
            {
                case 1:
                    this.Style = CandleStyles.CandleBar;
                    return;

                case 2:
                    this.Pen.Visible = true;
                    this.Style = CandleStyles.CandleBar;
                    this.showOpenTick = false;
                    return;

                case 3:
                    this.Pen.Visible = true;
                    this.Style = CandleStyles.CandleBar;
                    this.showCloseTick = false;
                    return;

                case 4:
                    this.Style = CandleStyles.CandleStick;
                    this.Pen.Visible = false;
                    return;

                case 5:
                    this.Style = CandleStyles.Line;
                    return;
            }
            base.SetSubGallery(index);
        }

        [DefaultValue(4), Description("Sets the horizontal Candle Size.")]
        public int CandleWidth
        {
            get
            {
                return this.candleWidth;
            }
            set
            {
                base.SetIntegerProperty(ref this.candleWidth, value);
            }
        }

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.GalleryCandle;
            }
        }

        [Description("Candle color when Close value is greater than Open value."), DefaultValue(typeof(Color), "Red")]
        public Color DownCloseColor
        {
            get
            {
                return this.downCloseColor;
            }
            set
            {
                base.SetColorProperty(ref this.downCloseColor, value);
            }
        }

        [Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ChartPen Pen
        {
            get
            {
                return base.Pointer.Pen;
            }
        }

        [DefaultValue(true), Description("Determines whether Close prices will be displayed.")]
        public bool ShowClose
        {
            get
            {
                return this.showCloseTick;
            }
            set
            {
                base.SetBooleanProperty(ref this.showCloseTick, value);
            }
        }

        [Description("Determines whether Open prices will be displayed."), DefaultValue(true)]
        public bool ShowOpen
        {
            get
            {
                return this.showOpenTick;
            }
            set
            {
                base.SetBooleanProperty(ref this.showOpenTick, value);
            }
        }

        [Description("Determines how  the Candle points will be drawn."), DefaultValue(0)]
        public CandleStyles Style
        {
            get
            {
                return this.style;
            }
            set
            {
                if (this.style != value)
                {
                    this.style = value;
                    this.Invalidate();
                }
            }
        }

        [Description("Candle color fill when Open value is greater than Close."), DefaultValue(typeof(Color), "White")]
        public Color UpCloseColor
        {
            get
            {
                return this.upCloseColor;
            }
            set
            {
                base.SetColorProperty(ref this.upCloseColor, value);
            }
        }
    }
}

