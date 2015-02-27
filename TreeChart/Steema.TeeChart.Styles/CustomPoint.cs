namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;

    public class CustomPoint : BaseLine
    {
        protected CustomStack iStacked;
        internal SeriesPointer point;

        public event ClickPointerEventHandler ClickPointer;

        public event GetPointerStyleEventHandler GetPointerStyle;

        public CustomPoint() : this(null)
        {
        }

        public CustomPoint(Chart c) : base(c)
        {
        }

        private int AxisPosition()
        {
            if (base.yMandatory)
            {
                return base.GetVertAxis.IEndPos;
            }
            return base.GetHorizAxis.IEndPos;
        }

        protected internal override void CalcHorizMargins(ref int LeftMargin, ref int RightMargin)
        {
            base.CalcHorizMargins(ref LeftMargin, ref RightMargin);
            this.Pointer.CalcHorizMargins(ref LeftMargin, ref RightMargin);
        }

        private int CalcStackedPos(int valueIndex, double value)
        {
            value += this.PointOrigin(valueIndex, false);
            if (this.iStacked == CustomStack.Stack)
            {
                return Math.Min(this.AxisPosition(), base.CalcPosValue(value));
            }
            double num = this.PointOrigin(valueIndex, true);
            if (num == 0.0)
            {
                return this.AxisPosition();
            }
            return base.CalcPosValue((value * 100.0) / num);
        }

        protected internal override void CalcVerticalMargins(ref int TopMargin, ref int BottomMargin)
        {
            base.CalcVerticalMargins(ref TopMargin, ref BottomMargin);
            this.Pointer.CalcVerticalMargins(ref TopMargin, ref BottomMargin);
        }

        public override int CalcXPos(int valueIndex)
        {
            if ((!base.yMandatory && (this.iStacked != CustomStack.None)) && (this.iStacked != CustomStack.Overlap))
            {
                return this.CalcStackedPos(valueIndex, base.vxValues[valueIndex]);
            }
            return base.CalcXPos(valueIndex);
        }

        public override int CalcYPos(int valueIndex)
        {
            if ((base.yMandatory && (this.iStacked != CustomStack.None)) && (this.iStacked != CustomStack.Overlap))
            {
                return this.CalcStackedPos(valueIndex, base.vyValues[valueIndex]);
            }
            return base.CalcYPos(valueIndex);
        }

        protected internal override void CalcZOrder()
        {
            if (this.iStacked == CustomStack.None)
            {
                base.CalcZOrder();
            }
            else
            {
                base.iZOrder = base.chart.maxZOrder;
            }
        }

        public override int Clicked(int x, int y)
        {
            if (base.chart != null)
            {
                base.chart.graphics3D.Calculate2DPosition(ref x, ref y, base.StartZ);
            }
            int num = base.Clicked(x, y);
            if (((num == -1) && (base.firstVisible > -1)) && (base.lastVisible > -1))
            {
                for (int i = base.firstVisible; i <= base.lastVisible; i++)
                {
                    if (this.ClickedPointer(i, this.CalcXPos(i), this.CalcYPos(i), x, y))
                    {
                        this.OnClickPointer(i, x, y);
                        return i;
                    }
                }
            }
            return num;
        }

        public virtual bool ClickedPointer(int valueIndex, int tmpX, int tmpY, int x, int y)
        {
            return ((Math.Abs((int) (tmpX - x)) < this.point.HorizSize) && (Math.Abs((int) (tmpY - y)) < this.point.VertSize));
        }

        protected override void DrawLegendShape(Graphics3D g, int valueIndex, Rectangle rect)
        {
            if (this.Pointer.Visible)
            {
                Color color = (valueIndex == -1) ? base.Color : this.ValueColor(valueIndex);
                this.point.DrawLegendShape(g, color, rect, false);
            }
            else
            {
                base.DrawLegendShape(g, valueIndex, rect);
            }
        }

        protected internal override void DrawMark(int valueIndex, string s, SeriesMarks.Position position)
        {
            base.Marks.ZPosition = base.StartZ;
            if (base.yMandatory)
            {
                base.Marks.ApplyArrowLength(ref position);
            }
            base.DrawMark(valueIndex, s, position);
        }

        public void DrawPointer(int aX, int aY, Color aColor, int valueIndex)
        {
            this.point.PrepareCanvas(base.chart.graphics3D, aColor);
            PointerStyles style = this.point.Style;
            this.OnGetPointerStyle(valueIndex, ref style);
            this.point.Draw(aX, aY, aColor, style);
        }

        public override void DrawValue(int valueIndex)
        {
            this.DrawPointer(this.CalcXPos(valueIndex), this.CalcYPos(valueIndex), this.ValueColor(valueIndex), valueIndex);
        }

        protected virtual int GetOriginPos(int valueIndex)
        {
            if ((this.iStacked != CustomStack.None) && (this.iStacked != CustomStack.Overlap))
            {
                return this.CalcStackedPos(valueIndex, 0.0);
            }
            if (base.yMandatory)
            {
                if (!base.GetVertAxis.Inverted)
                {
                    return base.GetVertAxis.IEndPos;
                }
                return base.GetVertAxis.IStartPos;
            }
            if (!base.GetHorizAxis.Inverted)
            {
                return base.GetHorizAxis.IStartPos;
            }
            return base.GetHorizAxis.IEndPos;
        }

        public override double MaxXValue()
        {
            double num = 0.0;
            if (base.yMandatory)
            {
                return base.MaxXValue();
            }
            if (this.iStacked == CustomStack.Stack100)
            {
                return 100.0;
            }
            num = base.MaxXValue();
            if (this.iStacked == CustomStack.Stack)
            {
                for (int i = 0; i < base.Count; i++)
                {
                    num = Math.Max(num, this.PointOrigin(i, false) + base.XValues[i]);
                }
            }
            return num;
        }

        public override double MaxYValue()
        {
            if (!base.yMandatory)
            {
                return base.MaxYValue();
            }
            if (this.iStacked == CustomStack.Stack100)
            {
                return 100.0;
            }
            double num = base.MaxYValue();
            if (this.iStacked == CustomStack.Stack)
            {
                for (int i = 0; i < base.Count; i++)
                {
                    num = Math.Max(num, this.PointOrigin(i, false) + base.vyValues[i]);
                }
            }
            return num;
        }

        public override double MinXValue()
        {
            if (!base.yMandatory && (this.iStacked == CustomStack.Stack100))
            {
                return 0.0;
            }
            return base.MinXValue();
        }

        public override double MinYValue()
        {
            if (base.yMandatory && (this.iStacked == CustomStack.Stack100))
            {
                return 0.0;
            }
            return base.MinYValue();
        }

        protected void OnClickPointer(int valueIndex, int x, int y)
        {
            if (this.ClickPointer != null)
            {
                this.ClickPointer(this, valueIndex, x, y);
            }
        }

        protected void OnGetPointerStyle(int valueIndex, ref PointerStyles style)
        {
            if (this.GetPointerStyle != null)
            {
                GetPointerStyleEventArgs e = new GetPointerStyleEventArgs(valueIndex, style);
                this.GetPointerStyle(this, e);
                style = e.Style;
            }
        }

        private double PointOrigin(int valueIndex, bool sumAll)
        {
            double num = 0.0;
            foreach (Series series in base.chart.Series)
            {
                if (!sumAll && (series == this))
                {
                    return num;
                }
                if ((series.Active && (series is CustomPoint)) && (series.Count > valueIndex))
                {
                    double originValue = series.GetOriginValue(valueIndex);
                    if (originValue > 0.0)
                    {
                        num += originValue;
                    }
                }
            }
            return num;
        }

        protected override void SetChart(Chart c)
        {
            base.SetChart(c);
            if (this.point != null)
            {
                this.point.Chart = base.chart;
            }
        }

        private void SetOtherStacked()
        {
            if (base.chart != null)
            {
                foreach (Series series in base.chart.Series)
                {
                    if (series is CustomPoint)
                    {
                        ((CustomPoint) series).iStacked = this.iStacked;
                    }
                }
            }
        }

        [DefaultValue((string) null), Description("Defines all necessary properties of the Series Pointer."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public SeriesPointer Pointer
        {
            get
            {
                if (this.point == null)
                {
                    this.point = new SeriesPointer(base.chart, this);
                }
                return this.point;
            }
        }

        [DefaultValue(0), Description("Defines how multiple series will be displayed.")]
        public CustomStack Stacked
        {
            get
            {
                return this.iStacked;
            }
            set
            {
                if (this.iStacked != value)
                {
                    this.iStacked = value;
                    this.SetOtherStacked();
                    this.Invalidate();
                }
            }
        }

        public delegate void ClickPointerEventHandler(CustomPoint series, int valueIndex, int x, int y);

        public class GetPointerStyleEventArgs : EventArgs
        {
            private PointerStyles style;
            private readonly int valueIndex;

            public GetPointerStyleEventArgs(int valueIndex, PointerStyles style)
            {
                this.valueIndex = valueIndex;
                this.style = style;
            }

            public PointerStyles Style
            {
                get
                {
                    return this.style;
                }
                set
                {
                    this.style = value;
                }
            }

            public int ValueIndex
            {
                get
                {
                    return this.valueIndex;
                }
            }
        }

        public delegate void GetPointerStyleEventHandler(CustomPoint series, CustomPoint.GetPointerStyleEventArgs e);
    }
}

