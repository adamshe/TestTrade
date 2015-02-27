namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;

    [ToolboxBitmap(typeof(Smith), "SeriesIcons.Smith.bmp")]
    public class Smith : Circular
    {
        private ChartPen circlePen;
        private static double[] defaultR = new double[] { 0.0, 0.2, 0.5, 1.0, 2.0, 5.0, 10.0 };
        private static double[] defaultX = new double[] { 0.0, 0.1, 0.3, 0.5, 0.8, 1.0, 1.5, 2.0, 3.0, 5.0, 7.0, 10.0 };
        private string imagSymbol;
        private int OldX;
        private int OldY;
        private ChartPen pen;
        private SeriesPointer pointer;

        public Smith() : this(null)
        {
        }

        public Smith(Chart c) : base(c)
        {
            base.XValues.Name = Texts.SmithResistance;
            base.XValues.Order = ValueListOrder.None;
            base.YValues.Name = Texts.SmithReactance;
            this.pointer = new SeriesPointer(base.chart, this);
            this.circlePen = new ChartPen(Color.Black);
            this.imagSymbol = "i";
            base.CircleBackColor = Color.Empty;
        }

        protected override void AddSampleValues(int numValues)
        {
            Series.SeriesRandom random = base.RandomBounds(numValues);
            for (int i = 0; i <= numValues; i++)
            {
                base.Add((double) ((6.5 * i) / ((double) numValues)), (double) (((i * random.Random()) + 3.8) / ((double) numValues)));
            }
        }

        public override int CalcXPos(int valueIndex)
        {
            int num;
            int num2;
            this.ZToPos(base.vxValues[valueIndex], base.vyValues[valueIndex], out num, out num2);
            return num;
        }

        public override int CalcYPos(int valueIndex)
        {
            int num;
            int num2;
            this.ZToPos(base.vxValues[valueIndex], base.vyValues[valueIndex], out num2, out num);
            return num;
        }

        public override int Clicked(int x, int y)
        {
            if (base.chart != null)
            {
                base.chart.graphics3D.Calculate2DPosition(ref x, ref y, base.StartZ);
            }
            int num = base.Clicked(x, y);
            if (((num == -1) && (base.firstVisible > -1)) && ((base.lastVisible > -1) && this.pointer.Visible))
            {
                for (int i = base.firstVisible; i <= base.lastVisible; i++)
                {
                    if ((Math.Abs((int) (this.CalcXPos(i) - x)) < this.pointer.HorizSize) && (Math.Abs((int) (this.CalcYPos(i) - y)) < this.pointer.VertSize))
                    {
                        return i;
                    }
                }
            }
            return num;
        }

        protected override void DoBeforeDrawValues()
        {
            bool flag = false;
            foreach (Series series in base.chart.Series)
            {
                if (series.Active && (series is Smith))
                {
                    if (series == this)
                    {
                        if (!flag && this.CLabels)
                        {
                            Rectangle chartRect = base.chart.ChartRect;
                            chartRect.Inflate(-((int) base.chart.graphics3D.TextWidth("360")), -base.chart.graphics3D.FontHeight + 2);
                            base.chart.ChartRect = chartRect;
                        }
                        break;
                    }
                    flag = true;
                }
            }
            base.DoBeforeDrawValues();
            flag = false;
            foreach (Series series2 in base.chart.Series)
            {
                if (series2.Active && (series2 is Smith))
                {
                    if (series2 == this)
                    {
                        if (!flag)
                        {
                            this.DrawCircle();
                            if (base.chart.Axes.Visible && base.chart.Axes.DrawBehind)
                            {
                                this.DrawAxis();
                            }
                        }
                        break;
                    }
                    flag = true;
                }
            }
        }

        protected internal override void Draw()
        {
            base.Draw();
            if (this.pointer.Visible)
            {
                for (int i = base.firstVisible; i <= base.lastVisible; i++)
                {
                    Color colorValue = this.ValueColor(i);
                    this.pointer.PrepareCanvas(base.chart.graphics3D, colorValue);
                    this.pointer.Draw(this.CalcXPos(i), this.CalcYPos(i), colorValue);
                }
            }
        }

        private void DrawAxis()
        {
            if (base.GetVertAxis.Visible)
            {
                this.DrawXCircleGrid();
            }
            base.Chart.Graphics3D.UnClip();
            if (base.GetHorizAxis.Visible)
            {
                this.DrawRCircleGrid();
            }
        }

        private void DrawCircle()
        {
            Graphics3D graphicsd = base.chart.graphics3D;
            if (base.CircleBackColor.IsEmpty && (base.CalcCircleGradient() == null))
            {
                graphicsd.Brush.Visible = false;
            }
            else
            {
                graphicsd.Brush.Visible = true;
                graphicsd.Brush.Solid = true;
                graphicsd.Brush.Color = base.CalcCircleBackColor();
                graphicsd.Brush.Gradient = base.CircleGradient;
            }
            int num = base.CircleWidth / 2;
            int num2 = base.CircleHeight / 2;
            graphicsd.Pen = this.CirclePen;
            graphicsd.Ellipse(base.CircleXCenter - num, base.CircleYCenter - num2, base.CircleXCenter + num, base.CircleYCenter + num2, base.EndZ);
        }

        private void DrawRCircle(double value, int z, bool showLabel)
        {
            if (value != -1.0)
            {
                double num = 1.0 / (1.0 + value);
                int num2 = Utils.Round((double) (num * base.XRadius));
                int num3 = Utils.Round((double) (num * base.YRadius));
                base.chart.graphics3D.Ellipse(base.CircleRect.Right - (2 * num2), base.CircleYCenter - num3, base.CircleRect.Right, base.CircleYCenter + num3, z);
                if (showLabel)
                {
                    this.DrawRCircleLabel(value, base.CircleRect.Right - (2 * num2), base.CircleYCenter);
                }
            }
        }

        private void DrawRCircleGrid()
        {
            base.chart.graphics3D.Pen = this.RCirclePen;
            for (int i = 0; i <= 6; i++)
            {
                this.DrawRCircle(defaultR[i], base.MiddleZ, this.RLabels);
            }
        }

        private void DrawRCircleLabel(double rVal, int x, int y)
        {
            if (base.GetHorizAxis.Visible)
            {
                base.chart.graphics3D.Font = this.RLabelsFont;
                base.chart.graphics3D.TextAlign = StringAlignment.Center;
                base.chart.graphics3D.TextOut(x, y, base.EndZ, rVal.ToString());
            }
        }

        public override void DrawValue(int valueIndex)
        {
            int num;
            int num2;
            this.ZToPos(base.vxValues[valueIndex], base.vyValues[valueIndex], out num, out num2);
            this.LinePrepareCanvas(valueIndex);
            if (valueIndex == base.firstVisible)
            {
                base.chart.graphics3D.MoveTo(num, num2, base.StartZ);
            }
            else if ((num != this.OldX) || (num2 != this.OldY))
            {
                base.chart.graphics3D.LineTo(num, num2, base.StartZ);
            }
            this.OldX = num;
            this.OldY = num2;
        }

        private void DrawXCircle(double Value, int Z, bool ShowLabel)
        {
            int x;
            int right;
            int circleYCenter;
            if (Value != 0.0)
            {
                int num3;
                int num4;
                int num7;
                int num8;
                double num11 = 1.0 / Value;
                this.ZToPos(0.0, Value, out num4, out num8);
                if (ShowLabel)
                {
                    this.DrawXCircleLabel(Value, num4, num8);
                }
                this.ZToPos(100.0, Value, out num3, out num7);
                int num9 = Utils.Round((double) (num11 * base.XRadius));
                int num10 = Utils.Round((double) (num11 * base.YRadius));
                x = base.CircleRect.Right - num9;
                right = base.CircleRect.Right + num9;
                circleYCenter = base.CircleYCenter;
                int num6 = circleYCenter - (2 * num10);
                base.chart.Graphics3D.ClipEllipse(base.CircleRect);
                if (!base.chart.Aspect.View3D || base.chart.Aspect.Orthogonal)
                {
                    base.chart.graphics3D.Arc(x, num6, right, circleYCenter, 0f, 360f);
                }
                this.ZToPos(0.0, -Value, out num4, out num8);
                if (!base.chart.Aspect.View3D || base.chart.Aspect.Orthogonal)
                {
                    base.chart.graphics3D.Arc(x, num6 + (2 * num10), right, circleYCenter + (2 * num10), 0f, 360f);
                }
                base.chart.Graphics3D.UnClip();
                if (ShowLabel)
                {
                    this.DrawXCircleLabel(-Value, num4, num8);
                }
            }
            else
            {
                x = base.CircleRect.X;
                right = base.CircleRect.Right;
                circleYCenter = base.CircleYCenter;
                base.chart.graphics3D.Line(x, circleYCenter, right, circleYCenter, base.MiddleZ);
                if (ShowLabel)
                {
                    this.DrawXCircleLabel(0.0, x, circleYCenter);
                }
            }
        }

        private void DrawXCircleGrid()
        {
            base.chart.graphics3D.Pen = this.CCirclePen;
            for (int i = 0; i <= 11; i++)
            {
                this.DrawXCircle(defaultX[i], base.MiddleZ, this.CLabels);
            }
        }

        private void DrawXCircleLabel(double XVal, int X, int Y)
        {
            if (base.GetVertAxis.Visible)
            {
                base.chart.graphics3D.Font = this.CLabelsFont;
                int fontHeight = base.chart.graphics3D.FontHeight;
                string xCircleLabel = this.GetXCircleLabel(XVal);
                double num2 = base.PointToAngle(X, Y) * 57.29577;
                if (num2 >= 360.0)
                {
                    num2 -= 360.0;
                }
                if ((num2 == 0.0) || (num2 == 180.0))
                {
                    Y -= fontHeight / 2;
                }
                else if ((num2 > 0.0) && (num2 < 180.0))
                {
                    Y -= fontHeight;
                }
                if ((num2 == 90.0) || (num2 == 270.0))
                {
                    base.chart.graphics3D.TextAlign = StringAlignment.Center;
                }
                else
                {
                    base.chart.graphics3D.TextAlign = ((num2 > 90.0) && (num2 < 270.0)) ? StringAlignment.Far : StringAlignment.Near;
                }
                int num3 = ((int) base.chart.graphics3D.TextWidth("0")) / 2;
                switch (num2)
                {
                    case 0.0:
                        X += num3;
                        break;

                    case 180.0:
                        X -= num3;
                        break;
                }
                base.chart.graphics3D.TextOut(X, Y, base.EndZ, xCircleLabel);
            }
        }

        private string GetXCircleLabel(double reactance)
        {
            return (reactance.ToString() + this.imagSymbol);
        }

        private void LinePrepareCanvas(int valueIndex)
        {
            Graphics3D graphicsd = base.chart.graphics3D;
            if (this.Pen.Visible)
            {
                graphicsd.Pen = this.Pen;
                graphicsd.Pen.Color = (valueIndex == -1) ? base.Color : this.ValueColor(valueIndex);
            }
            else
            {
                graphicsd.Pen.Visible = false;
            }
        }

        private void PosToZ(int X, int Y, out double Resist, out double React)
        {
            X -= base.CircleXCenter;
            Y = base.CircleYCenter - Y;
            double num = X / base.XRadius;
            double num2 = Y / base.YRadius;
            double num3 = (num * num) + (num2 * num2);
            double num4 = 1.0 / ((num3 - (2.0 * num)) + 1.0);
            Resist = (1.0 - num3) * num4;
            React = (2.0 * num2) * num4;
        }

        internal override void PrepareForGallery(bool isEnabled)
        {
            base.PrepareForGallery(isEnabled);
            base.chart.Aspect.Chart3DPercent = 5;
            base.chart.Axes.Right.Labels.Visible = false;
            base.chart.Axes.Top.Labels.Visible = false;
            base.chart.Aspect.Orthogonal = false;
            base.chart.Aspect.Elevation = 360;
            base.chart.Aspect.Zoom = 90;
        }

        protected override void SetChart(Chart c)
        {
            base.SetChart(c);
            if (this.pointer != null)
            {
                this.pointer.Chart = c;
            }
            if ((base.chart != null) && base.DesignMode)
            {
                base.chart.Aspect.View3D = false;
            }
        }

        private void ZToPos(double Resist, double React, out int X, out int Y)
        {
            double num = (Resist * Resist) + (React * React);
            double num2 = 1.0 / ((num + (2.0 * Resist)) + 1.0);
            double num3 = (num - 1.0) * num2;
            double num4 = (2.0 * React) * num2;
            X = base.CircleXCenter + Utils.Round((double) (num3 * base.XRadius));
            Y = base.CircleYCenter - Utils.Round((double) (num4 * base.YRadius));
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Defines Brush to fill Chart."), Category("Appearance")]
        public ChartBrush Brush
        {
            get
            {
                return base.bBrush;
            }
        }

        [Description("Defines Pen to draw CCircles of the Smith Series.")]
        public Axis.GridPen CCirclePen
        {
            get
            {
                return base.GetVertAxis.Grid;
            }
        }

        [Description("Defines Pen to draw external Circle of the Smith Series."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ChartPen CirclePen
        {
            get
            {
                if (this.circlePen == null)
                {
                    this.circlePen = new ChartPen(base.chart);
                }
                return this.circlePen;
            }
        }

        [Description("Shows/hides the constant reactance labels.")]
        public bool CLabels
        {
            get
            {
                return base.GetVertAxis.Labels.Visible;
            }
            set
            {
                base.GetVertAxis.Labels.Visible = value;
            }
        }

        [Description("xCircle labels font.")]
        public ChartFont CLabelsFont
        {
            get
            {
                return base.GetVertAxis.Labels.Font;
            }
        }

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.GallerySmith;
            }
        }

        public string ImagSymbol
        {
            get
            {
                return this.imagSymbol;
            }
            set
            {
                base.SetStringProperty(ref this.imagSymbol, value);
            }
        }

        [Description("Defines pen to draw SmithSeries Chart."), Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ChartPen Pen
        {
            get
            {
                if (this.pen == null)
                {
                    this.pen = new ChartPen(base.chart);
                }
                return this.pen;
            }
        }

        [Description("Defines pen to draw SmithSeries Chart."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance")]
        public SeriesPointer Pointer
        {
            get
            {
                if (this.pointer == null)
                {
                    this.pointer = new SeriesPointer(base.chart, this);
                }
                return this.pointer;
            }
        }

        [Description("Defines Pen to draw RCircles of the Smith Series.")]
        public Axis.GridPen RCirclePen
        {
            get
            {
                return base.GetHorizAxis.Grid;
            }
        }

        [Description("Shows/hides the constant resistance labels.")]
        public bool RLabels
        {
            get
            {
                return base.GetHorizAxis.Labels.Visible;
            }
            set
            {
                base.GetHorizAxis.Labels.Visible = value;
            }
        }

        [Description("rCircle labels font.")]
        public ChartFont RLabelsFont
        {
            get
            {
                return base.GetHorizAxis.Labels.Font;
            }
        }
    }
}

