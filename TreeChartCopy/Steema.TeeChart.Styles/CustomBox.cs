namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;

    public class CustomBox : Points
    {
        private int adjacentPoint1;
        private int adjacentPoint3;
        protected double dPosition;
        private SeriesPointer extrOut;
        private double innerFence1;
        private double innerFence3;
        protected bool IVertical;
        private double median;
        private ChartPen medianPen;
        private SeriesPointer mildOut;
        private double outerFence1;
        private double outerFence3;
        private double quartile1;
        private double quartile3;
        private bool useCustomValues;
        private double whiskerLength;
        private ChartPen whiskerPen;

        public CustomBox() : this(null)
        {
        }

        public CustomBox(Chart c) : base(c)
        {
            this.whiskerLength = 1.5;
            this.IVertical = true;
            this.useCustomValues = false;
            base.AllowSinglePoint = false;
            base.calcVisiblePoints = false;
            base.vxValues.Name = "";
            base.vyValues.Name = "Samples";
            base.marks.Visible = false;
            base.marks.defaultVisible = false;
            base.marks.ArrowLength = 0;
            base.marks.defaultArrowLength = 0;
            base.point.Draw3D = false;
            base.point.Pen.Width = 1;
            base.point.VertSize = 15;
            base.point.HorizSize = 15;
            base.point.Brush.Color = Color.White;
            this.mildOut = new SeriesPointer(base.chart, this);
            this.mildOut.Style = PointerStyles.Circle;
            this.extrOut = new SeriesPointer(base.chart, this);
            this.extrOut.Style = PointerStyles.Star;
        }

        protected override void AddSampleValues(int numValues)
        {
            int num = base.chart.Series.Count + 1;
            Random random = new Random();
            int num2 = num * (3 + random.Next(10));
            base.Add(-num2);
            for (int i = 2; i < (numValues - 1); i++)
            {
                base.Add((int) ((num2 * i) / numValues));
            }
            base.Add((int) (2 * num2));
        }

        private int CalcPos(double value)
        {
            if (!this.IVertical)
            {
                return base.CalcXPosValue(value);
            }
            return base.CalcYPosValue(value);
        }

        protected override void DoBeforeDrawValues()
        {
            base.DoBeforeDrawValues();
            if (!this.useCustomValues && (this.SampleValues.Count > 0))
            {
                int count = this.SampleValues.Count;
                double invN = 1.0 / ((double) count);
                int num3 = count / 2;
                if ((count % 2) == 0)
                {
                    this.median = this.SampleValues[num3];
                }
                else
                {
                    this.median = 0.5 * (this.SampleValues[num3 - 1] + this.SampleValues[num3]);
                }
                this.quartile1 = this.Percentile(0.25, invN);
                this.quartile3 = this.Percentile(0.75, invN);
                double num4 = this.quartile3 - this.quartile1;
                this.innerFence1 = this.quartile1 - (this.whiskerLength * num4);
                this.innerFence3 = this.quartile3 + (this.whiskerLength * num4);
                int num5 = 0;
                while (num5 < count)
                {
                    if (this.innerFence1 <= this.SampleValues[num5])
                    {
                        break;
                    }
                    num5++;
                }
                this.adjacentPoint1 = num5;
                num5 = num3;
                while (num5 < count)
                {
                    if (this.innerFence3 <= this.SampleValues[num5])
                    {
                        break;
                    }
                    num5++;
                }
                this.adjacentPoint3 = num5 - 1;
                this.outerFence1 = this.quartile1 - ((2.0 * this.whiskerLength) * num4);
                this.outerFence3 = this.quartile3 + ((2.0 * this.whiskerLength) * num4);
            }
        }

        protected internal override void Draw()
        {
            int num;
            int num2;
            int num3;
            int num4;
            int num6;
            int num7;
            int num8;
            int num10;
            base.Draw();
            int horizSize = base.Pointer.HorizSize;
            if (this.IVertical)
            {
                num = base.CalcXPosValue(this.dPosition) - horizSize;
                num3 = base.CalcXPosValue(this.dPosition) + horizSize;
                num2 = base.CalcYPosValue(this.quartile3);
                num4 = base.CalcYPosValue(this.quartile1);
                num7 = num4;
                num8 = num2;
            }
            else
            {
                num2 = base.CalcYPosValue(this.dPosition) - horizSize;
                num4 = base.CalcYPosValue(this.dPosition) + horizSize;
                num3 = base.CalcXPosValue(this.quartile3);
                num = base.CalcXPosValue(this.quartile1);
                num7 = num;
                num8 = num3;
            }
            if (base.GetHorizAxis.Inverted)
            {
                num10 = num;
                num = num3;
                num3 = num10;
            }
            if (base.GetVertAxis.Inverted)
            {
                num10 = num2;
                num2 = num4;
                num4 = num10;
            }
            Graphics3D g = base.chart.graphics3D;
            if (base.Pointer.Visible)
            {
                base.Pointer.PrepareCanvas(g, base.Pointer.Brush.Color);
                if (this.IVertical)
                {
                    num6 = (num4 - num2) / 2;
                    base.Pointer.Draw(g, base.chart.aspect.view3D, (num + horizSize) - 1, num2 + num6, base.Pointer.HorizSize - 1, num6 - 1, base.Pointer.Brush.Color, base.Pointer.Style);
                }
                else
                {
                    int num5 = (num3 - num) / 2;
                    base.Pointer.Draw(g, base.chart.aspect.view3D, num + num5, (num2 + horizSize) - 1, num5 - 1, base.Pointer.VertSize - 1, base.Pointer.Brush.Color, base.Pointer.Style);
                }
            }
            if (this.MedianPen.Visible)
            {
                g.Pen = this.medianPen;
                g.Brush.Visible = false;
                num6 = this.CalcPos(this.median);
                if (this.IVertical)
                {
                    if (base.chart.aspect.view3D)
                    {
                        g.HorizontalLine(num, num3, num6, base.StartZ);
                    }
                    else
                    {
                        g.HorizontalLine(num, num3, num6);
                    }
                }
                else if (base.chart.Aspect.View3D)
                {
                    g.VerticalLine(num6, num2, num4, base.StartZ);
                }
                else
                {
                    g.VerticalLine(num6, num2, num4);
                }
            }
            if (this.WhiskerPen.Visible)
            {
                int tmpZ = (base.Pointer.Visible && base.Pointer.Draw3D) ? base.MiddleZ : base.StartZ;
                g.Pen = this.whiskerPen;
                int num12 = (this.IVertical ? (num + num3) : (num2 + num4)) / 2;
                this.DrawWhisker(this.adjacentPoint1, num7, tmpZ, horizSize, num12);
                this.DrawWhisker(this.adjacentPoint3, num8, tmpZ, horizSize, num12);
            }
        }

        public override void DrawValue(int index)
        {
            SeriesPointer mildOut = null;
            double num = this.SampleValues[index];
            if ((num >= this.innerFence1) && (num <= this.innerFence3))
            {
                mildOut = null;
            }
            else if (((num >= this.innerFence3) && (num <= this.outerFence3)) || ((num <= this.innerFence1) && (num >= this.outerFence1)))
            {
                mildOut = this.mildOut;
            }
            else
            {
                mildOut = this.extrOut;
            }
            if ((mildOut != null) && mildOut.Visible)
            {
                Color colorValue = this.ValueColor(index);
                mildOut.PrepareCanvas(base.chart.graphics3D, colorValue);
                if (this.IVertical)
                {
                    mildOut.Draw(base.CalcXPosValue(this.dPosition), this.CalcYPos(index), colorValue);
                }
                else
                {
                    mildOut.Draw(this.CalcXPos(index), base.CalcYPosValue(this.dPosition), colorValue);
                }
            }
        }

        private void DrawWhisker(int AIndex, int Pos, int tmpZ, int tmp, int tmp1)
        {
            int bottom = this.CalcPos(this.SampleValues[AIndex]);
            Graphics3D graphicsd = base.chart.graphics3D;
            if (base.chart.aspect.view3D)
            {
                if (this.IVertical)
                {
                    graphicsd.VerticalLine(tmp1, Pos, bottom, tmpZ);
                    graphicsd.HorizontalLine(tmp1 - tmp, tmp1 + tmp, bottom, tmpZ);
                }
                else
                {
                    graphicsd.HorizontalLine(Pos, bottom, tmp1, tmpZ);
                    graphicsd.VerticalLine(bottom, tmp1 - tmp, tmp1 + tmp, tmpZ);
                }
            }
            else if (this.IVertical)
            {
                graphicsd.VerticalLine(tmp1, Pos, bottom);
                graphicsd.HorizontalLine(tmp1 - tmp, tmp1 + tmp, bottom);
            }
            else
            {
                graphicsd.HorizontalLine(Pos, bottom, tmp1);
                graphicsd.VerticalLine(bottom, tmp1 - tmp, tmp1 + tmp);
            }
        }

        private void InternalGallery()
        {
            this.dPosition = base.chart.Series.IndexOf(this) + 1;
            base.Pointer.HorizSize = 12;
            this.MildOut.HorizSize = 3;
            this.ExtrOut.VertSize = 3;
            base.FillSampleValues(Utils.Round((double) (10.0 * this.dPosition)));
        }

        private double Percentile(double P, double InvN)
        {
            double num = 0.0;
            double num2 = 0.0;
            int num3 = 0;
            while (num < P)
            {
                num2 = num;
                num = (0.5 + num3) * InvN;
                num3++;
            }
            double num4 = (P - num2) / (num - num2);
            return (this.SampleValues[num3 - 2] + ((this.SampleValues[num3 - 1] - this.SampleValues[num3 - 2]) * num4));
        }

        internal override void PrepareForGallery(bool IsEnabled)
        {
            base.PrepareForGallery(IsEnabled);
            foreach (Series series in base.chart.Series)
            {
                if (series is CustomBox)
                {
                    ((CustomBox) series).InternalGallery();
                }
            }
        }

        protected override void SetChart(Chart c)
        {
            base.SetChart(c);
            this.ExtrOut.chart = c;
            this.MildOut.chart = c;
            this.MedianPen.chart = c;
            this.WhiskerPen.chart = c;
        }

        [Description(""), DefaultValue(0)]
        public int AdjacentPoint1
        {
            get
            {
                return this.adjacentPoint1;
            }
            set
            {
                base.SetIntegerProperty(ref this.adjacentPoint1, value);
            }
        }

        [DefaultValue(0), Description("")]
        public int AdjacentPoint3
        {
            get
            {
                return this.adjacentPoint3;
            }
            set
            {
                base.SetIntegerProperty(ref this.adjacentPoint3, value);
            }
        }

        [Browsable(false), Description("Controls the appearance of CustomBoxSeries box.")]
        public SeriesPointer Box
        {
            get
            {
                return base.Pointer;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Controls the appearance of the extreme range of outer points.")]
        public SeriesPointer ExtrOut
        {
            get
            {
                return this.extrOut;
            }
        }

        [Description(""), DefaultValue((double) 0.0)]
        public double InnerFence1
        {
            get
            {
                return this.innerFence1;
            }
            set
            {
                base.SetDoubleProperty(ref this.innerFence1, value);
            }
        }

        [DefaultValue((double) 0.0), Description("")]
        public double InnerFence3
        {
            get
            {
                return this.innerFence3;
            }
            set
            {
                base.SetDoubleProperty(ref this.innerFence3, value);
            }
        }

        [DefaultValue((double) 0.0), Description("")]
        public double Median
        {
            get
            {
                return this.median;
            }
            set
            {
                base.SetDoubleProperty(ref this.median, value);
            }
        }

        [Description("Defines the Pen to draw the median line."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ChartPen MedianPen
        {
            get
            {
                if (this.medianPen == null)
                {
                    this.medianPen = new ChartPen(base.chart, Color.Black, true);
                    this.medianPen.Width = 1;
                    this.medianPen.Style = DashStyle.Dot;
                }
                return this.medianPen;
            }
        }

        [Description("Controls the appearance of the mid range of outer points."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public SeriesPointer MildOut
        {
            get
            {
                return this.mildOut;
            }
        }

        [DefaultValue((double) 0.0), Description("")]
        public double OuterFence1
        {
            get
            {
                return this.outerFence1;
            }
            set
            {
                base.SetDoubleProperty(ref this.outerFence1, value);
            }
        }

        [DefaultValue((double) 0.0), Description("")]
        public double OuterFence3
        {
            get
            {
                return this.outerFence3;
            }
            set
            {
                base.SetDoubleProperty(ref this.outerFence3, value);
            }
        }

        [Description("Specifies the position of box series."), DefaultValue((double) 0.0)]
        public double Position
        {
            get
            {
                return this.dPosition;
            }
            set
            {
                base.SetDoubleProperty(ref this.dPosition, value);
            }
        }

        [DefaultValue((double) 0.0), Description("")]
        public double Quartile1
        {
            get
            {
                return this.quartile1;
            }
            set
            {
                base.SetDoubleProperty(ref this.quartile1, value);
            }
        }

        [DefaultValue((double) 0.0), Description("")]
        public double Quartile3
        {
            get
            {
                return this.quartile3;
            }
            set
            {
                base.SetDoubleProperty(ref this.quartile3, value);
            }
        }

        [Description(""), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public ValueList SampleValues
        {
            get
            {
                return base.mandatory;
            }
        }

        [DefaultValue(false), Description("")]
        public bool UseCustomValues
        {
            get
            {
                return this.useCustomValues;
            }
            set
            {
                base.SetBooleanProperty(ref this.useCustomValues, value);
            }
        }

        [DefaultValue((double) 1.5), Description("Defines the whisker length as a function of the IQR.")]
        public double WhiskerLength
        {
            get
            {
                return this.whiskerLength;
            }
            set
            {
                base.SetDoubleProperty(ref this.whiskerLength, value);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Defines the Pen to draw the whisker lines.")]
        public ChartPen WhiskerPen
        {
            get
            {
                if (this.whiskerPen == null)
                {
                    this.whiskerPen = new ChartPen(base.chart, Color.Black, true);
                }
                return this.whiskerPen;
            }
        }
    }
}

