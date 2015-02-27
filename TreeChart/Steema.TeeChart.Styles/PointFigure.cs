namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    [ToolboxBitmap(typeof(PointFigure), "SeriesIcons.PointFigure.bmp")]
    public class PointFigure : OHLC
    {
        private double boxSize;
        private SeriesPointer down;
        private double reversal;
        private SeriesPointer up;

        public PointFigure() : this(null)
        {
        }

        public PointFigure(Chart c) : base(c)
        {
            this.boxSize = 1.0;
            this.reversal = 3.0;
            this.up = new SeriesPointer(c, this);
            this.up.Style = PointerStyles.DiagCross;
            this.up.Brush.Color = Color.Green;
            this.down = new SeriesPointer(c, this);
            this.down.Style = PointerStyles.Circle;
            this.down.Brush.Color = Color.Red;
            base.vxValues.dateTime = false;
        }

        private int CalcMaxColumns(bool draw)
        {
            if (base.Count <= 0)
            {
                return 0;
            }
            double num = this.ReversalAmount * this.BoxSize;
            double fromValue = base.LowValues.Value[0];
            double toValue = base.HighValues.Value[0];
            int num4 = 0;
            int tmpX = 0;
            if (draw)
            {
                tmpX = base.CalcXPosValue((double) num4);
                this.DrawColumn(this.down, fromValue, toValue, tmpX);
            }
            bool flag = true;
            for (int i = 1; i < base.Count; i++)
            {
                double num5;
                if (flag)
                {
                    num5 = base.LowValues.Value[i];
                    if (num5 <= (fromValue - this.BoxSize))
                    {
                        if (draw)
                        {
                            this.DrawColumn(this.down, num5, fromValue - this.BoxSize, tmpX);
                        }
                        fromValue = num5;
                    }
                    else
                    {
                        num5 = base.HighValues.Value[i];
                        if (num5 >= (fromValue + num))
                        {
                            num4++;
                            toValue = num5;
                            if (draw)
                            {
                                tmpX = base.CalcXPosValue((double) num4);
                                this.DrawColumn(this.up, fromValue + this.BoxSize, toValue, tmpX);
                            }
                            flag = false;
                        }
                    }
                }
                else
                {
                    num5 = base.HighValues.Value[i];
                    if (num5 >= (toValue + this.BoxSize))
                    {
                        if (draw)
                        {
                            this.DrawColumn(this.up, toValue + this.BoxSize, num5, tmpX);
                        }
                        toValue = num5;
                    }
                    else
                    {
                        num5 = base.LowValues.Value[i];
                        if (num5 <= (toValue - num))
                        {
                            num4++;
                            fromValue = num5;
                            if (draw)
                            {
                                tmpX = base.CalcXPosValue((double) num4);
                                this.DrawColumn(this.down, fromValue, toValue - this.BoxSize, tmpX);
                            }
                            flag = true;
                        }
                    }
                }
            }
            return (num4 + 1);
        }

        protected internal override int CountLegendItems()
        {
            return 2;
        }

        protected internal override void Draw()
        {
            this.CalcMaxColumns(true);
        }

        private void DrawColumn(SeriesPointer pointer, double FromValue, double ToValue, int tmpX)
        {
            do
            {
                int py = base.CalcYPosValue(FromValue);
                pointer.PrepareCanvas(base.chart.graphics3D, pointer.Color);
                pointer.Draw(tmpX, py, pointer.Color);
                FromValue += this.BoxSize;
            }
            while (FromValue <= ToValue);
        }

        protected internal override Color LegendItemColor(int index)
        {
            if (index != 0)
            {
                return this.down.Brush.Color;
            }
            return this.up.Brush.Color;
        }

        public override string LegendString(int legendIndex, LegendTextStyles legendTextStyle)
        {
            if (legendIndex != 0)
            {
                return Texts.Down;
            }
            return Texts.Up;
        }

        public override double MaxXValue()
        {
            return (double) (this.CalcMaxColumns(false) - 1);
        }

        public override double MinXValue()
        {
            return 0.0;
        }

        internal override void PrepareForGallery(bool IsEnabled)
        {
            base.PrepareForGallery(IsEnabled);
            if (!IsEnabled)
            {
                this.DownSymbol.Color = Color.Silver;
                this.DownSymbol.Pen.Color = Color.Gray;
                this.UpSymbol.Color = Color.Silver;
                this.UpSymbol.Pen.Color = Color.Gray;
            }
        }

        protected override void SetChart(Chart value)
        {
            base.SetChart(value);
            if (this.up != null)
            {
                this.up.Chart = base.chart;
            }
            if (this.down != null)
            {
                this.down.Chart = base.chart;
            }
        }

        [DefaultValue((double) 1.0)]
        public double BoxSize
        {
            get
            {
                return this.boxSize;
            }
            set
            {
                base.SetDoubleProperty(ref this.boxSize, value);
            }
        }

        public override string Description
        {
            get
            {
                return Texts.GalleryPointFigure;
            }
        }

        public SeriesPointer DownSymbol
        {
            get
            {
                return this.down;
            }
        }

        [DefaultValue((double) 3.0)]
        public double ReversalAmount
        {
            get
            {
                return this.reversal;
            }
            set
            {
                base.SetDoubleProperty(ref this.reversal, value);
            }
        }

        public SeriesPointer UpSymbol
        {
            get
            {
                return this.up;
            }
        }
    }
}

