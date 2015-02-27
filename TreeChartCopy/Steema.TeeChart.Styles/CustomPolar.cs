namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Runtime.InteropServices;

    public class CustomPolar : Circular
    {
        private bool circleLabels;
        private ChartFont circleLabelsFont;
        private bool circleLabelsInside;
        private bool circleLabelsRot;
        private ChartPen circlePen;
        private bool clockWiseLabels;
        private bool closeCircle;
        private ChartFont font;
        protected int IMaxValuesCount;
        protected SeriesPointer iPointer;
        private int OldX;
        private int OldY;
        private ChartPen pen;

        public CustomPolar() : this(null)
        {
        }

        public CustomPolar(Chart c) : base(c)
        {
            this.circleLabels = false;
            this.circleLabelsInside = false;
            this.circleLabelsRot = false;
            this.clockWiseLabels = false;
            this.closeCircle = true;
            this.IMaxValuesCount = 0;
            this.iPointer = new SeriesPointer(base.chart, this);
            this.circleLabelsFont = new ChartFont(base.chart);
            this.circlePen = new ChartPen(base.chart, Color.Black);
        }

        public override int CalcXPos(int valueIndex)
        {
            int num;
            int num2;
            this.CalcXYPos(valueIndex, (double) base.XRadius, out num2, out num);
            return num2;
        }

        private void CalcXYPos(int valueIndex, double aRadius, out int x, out int y)
        {
            double num = base.GetVertAxis.Maximum - base.GetVertAxis.Minimum;
            double num2 = base.vyValues[valueIndex] - base.GetVertAxis.Minimum;
            if ((num == 0.0) || (num2 < 0.0))
            {
                x = base.CircleXCenter;
                y = base.CircleYCenter;
            }
            else
            {
                double aXRadius = (num2 * aRadius) / num;
                base.AngleToPos(Utils.PiStep * this.GetXValue(valueIndex), aXRadius, aXRadius, out x, out y);
            }
        }

        public override int CalcYPos(int valueIndex)
        {
            int num;
            int num2;
            this.CalcXYPos(valueIndex, (double) base.XRadius, out num, out num2);
            return num2;
        }

        protected override void DoAfterDrawValues()
        {
            if (!base.chart.Axes.DrawBehind)
            {
                bool flag = false;
                for (int i = base.chart.Series.IndexOf(this) + 1; i < base.chart.Series.Count; i++)
                {
                    if (base.chart[i] is CustomPolar)
                    {
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    this.DrawAxis();
                }
            }
            base.DoAfterDrawValues();
        }

        protected override void DoBeforeDrawValues()
        {
            bool flag = false;
            for (int i = 0; i < base.chart.Series.Count; i++)
            {
                if (base.chart[i].Active && (base.chart[i] is CustomPolar))
                {
                    if (base.chart[i] == this)
                    {
                        if ((!flag && this.circleLabels) && !this.circleLabelsInside)
                        {
                            base.chart.graphics3D.Font = this.circleLabelsFont;
                            int num = base.chart.graphics3D.FontHeight + 2;
                            Rectangle chartRect = new Rectangle();
                            chartRect = base.chart.ChartRect;
                            chartRect.Y += num;
                            chartRect.Height -= 2 * num;
                            num = Utils.Round(base.chart.graphics3D.TextWidth("360"));
                            chartRect.X += num;
                            chartRect.Width -= 2 * num;
                            base.chart.ChartRect = chartRect;
                        }
                        break;
                    }
                    flag = true;
                }
            }
            base.DoBeforeDrawValues();
            flag = false;
            for (int j = 0; j < base.chart.Series.Count; j++)
            {
                if (base.chart[j].Active && (base.chart[j] is CustomPolar))
                {
                    if (base.chart[j] == this)
                    {
                        if (!flag)
                        {
                            this.DrawCircle();
                            if (base.chart.Axes.DrawBehind)
                            {
                                this.DrawAxis();
                                return;
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
            if (this.iPointer.Visible)
            {
                for (int i = base.firstVisible; i <= base.lastVisible; i++)
                {
                    Color colorValue = this.ValueColor(i);
                    this.iPointer.PrepareCanvas(base.chart.graphics3D, colorValue);
                    this.iPointer.Draw(this.CalcXPos(i), this.CalcYPos(i), colorValue);
                }
            }
        }

        private void DrawAngleLabel(double angle, int index)
        {
            int num;
            int num2;
            Graphics3D graphicsd = base.chart.graphics3D;
            graphicsd.Font = this.circleLabelsFont;
            int fontHeight = graphicsd.FontHeight;
            if (angle >= 360.0)
            {
                angle -= 360.0;
            }
            string circleLabel = this.GetCircleLabel(angle, index);
            int xRadius = base.XRadius;
            int yRadius = base.YRadius;
            if (this.CircleLabelsInside)
            {
                xRadius -= Utils.Round(graphicsd.TextWidth("   "));
                yRadius -= Utils.Round(graphicsd.TextHeight(circleLabel));
            }
            base.AngleToPos(angle * 0.017453292519943295, (double) xRadius, (double) yRadius, out num, out num2);
            angle += base.RotationAngle;
            double a = angle * 0.017453292519943295;
            if (this.circleLabelsRot)
            {
                if ((angle > 90.0) && (angle < 270.0))
                {
                    num += Utils.Round((double) ((0.5 * fontHeight) * Math.Sin(angle * 0.017453292519943295)));
                    num2 += Utils.Round((double) ((0.5 * fontHeight) * Math.Cos(angle * 0.017453292519943295)));
                }
                else
                {
                    num -= Utils.Round((double) ((0.5 * fontHeight) * Math.Sin(a)));
                    num2 -= Utils.Round((double) ((0.5 * fontHeight) * Math.Cos(a)));
                }
            }
            if (angle >= 360.0)
            {
                angle -= 360.0;
            }
            if (this.circleLabelsRot)
            {
                if ((angle > 90.0) && (angle < 270.0))
                {
                    graphicsd.TextAlign = StringAlignment.Far;
                    angle += 180.0;
                }
                else
                {
                    graphicsd.TextAlign = StringAlignment.Near;
                }
                graphicsd.RotateLabel(num, num2, base.EndZ, circleLabel, angle);
            }
            else
            {
                if ((angle == 0.0) || (angle == 180.0))
                {
                    num2 -= fontHeight / 2;
                }
                else if ((angle > 0.0) && (angle < 180.0))
                {
                    num2 -= fontHeight;
                }
                if ((angle == 90.0) || (angle == 270.0))
                {
                    graphicsd.TextAlign = StringAlignment.Center;
                }
                else if (this.circleLabelsInside)
                {
                    if ((angle > 90.0) && (angle < 270.0))
                    {
                        graphicsd.TextAlign = StringAlignment.Near;
                    }
                    else
                    {
                        graphicsd.TextAlign = StringAlignment.Far;
                    }
                }
                else if ((angle > 90.0) && (angle < 270.0))
                {
                    graphicsd.TextAlign = StringAlignment.Far;
                }
                else
                {
                    graphicsd.TextAlign = StringAlignment.Near;
                }
                int num3 = Utils.Round((float) (graphicsd.TextWidth("0") / 2f));
                if (angle == 0.0)
                {
                    num += num3;
                }
                else if (angle == 180.0)
                {
                    num -= num3;
                }
                graphicsd.TextOut(num, num2, base.EndZ, circleLabel);
            }
        }

        private void DrawAxis()
        {
            this.DrawXGrid();
            this.DrawYGrid();
            if (base.chart.Axes.Visible)
            {
                int num;
                if (base.chart.Axes.Right.Visible)
                {
                    num = base.CircleXCenter + base.chart.Axes.Right.SizeTickAxis();
                    base.chart.Axes.Right.Draw(num, num + base.chart.Axes.Right.SizeLabels(), base.CircleXCenter, false, base.chart.Axes.Left.Minimum, base.chart.Axes.Left.Maximum, base.chart.Axes.Left.Increment, base.CircleYCenter - base.YRadius, base.CircleYCenter);
                }
                if (this.IMaxValuesCount == 0)
                {
                    Axis left = base.chart.Axes.Left;
                    if (left.Visible)
                    {
                        left.InternalSetInverted(true);
                        num = base.CircleXCenter - left.SizeTickAxis();
                        left.Draw(num, num - left.SizeLabels(), base.CircleXCenter, false, base.CircleYCenter, base.CircleYCenter + base.YRadius);
                        left.InternalSetInverted(false);
                    }
                    if (base.chart.Axes.Top.Visible)
                    {
                        base.chart.Axes.Top.InternalSetInverted(true);
                        num = base.CircleYCenter - base.chart.Axes.Top.SizeTickAxis();
                        base.chart.Axes.Top.Draw(num, num - base.chart.Axes.Top.SizeLabels(), base.CircleYCenter, false, left.Minimum, left.Maximum, left.Increment, base.CircleXCenter - base.XRadius, base.CircleXCenter);
                        base.chart.Axes.Top.InternalSetInverted(false);
                    }
                    if (base.chart.Axes.Bottom.Visible)
                    {
                        num = base.CircleYCenter + base.chart.Axes.Bottom.SizeTickAxis();
                        base.chart.Axes.Bottom.Draw(num, num + base.chart.Axes.Bottom.SizeLabels(), base.CircleYCenter, false, left.Minimum, left.Maximum, left.Increment, base.CircleXCenter, base.CircleXCenter + base.XRadius);
                    }
                }
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
            graphicsd.Pen = this.CirclePen;
            this.DrawPolarCircle(base.CircleWidth / 2, base.CircleHeight / 2, base.EndZ);
        }

        protected override void DrawLegendShape(Graphics3D g, int valueIndex, Rectangle rect)
        {
            if (this.Pen.Visible)
            {
                this.LinePrepareCanvas(valueIndex);
                g.HorizontalLine(rect.X, rect.Right, (rect.Y + rect.Bottom) / 2);
            }
            if (this.iPointer.Visible)
            {
                Color color = (valueIndex == -1) ? base.Color : this.ValueColor(valueIndex);
                this.iPointer.DrawLegendShape(g, color, rect, this.Pen.Visible);
            }
            else if (!this.Pen.Visible)
            {
                base.DrawLegendShape(g, valueIndex, rect);
            }
        }

        protected internal override void DrawMark(int valueIndex, string s, SeriesMarks.Position position)
        {
            base.Marks.ApplyArrowLength(ref position);
            base.DrawMark(valueIndex, s, position);
        }

        private void DrawPolarCircle(int HalfWidth, int HalfHeight, int Z)
        {
            if (this.IMaxValuesCount == 0)
            {
                base.chart.graphics3D.Ellipse(base.CircleXCenter - HalfWidth, base.CircleYCenter - HalfHeight, base.CircleXCenter + HalfWidth, base.CircleYCenter + HalfHeight, Z);
            }
            else
            {
                int num2;
                int num3;
                double num = (Utils.PiStep * 360.0) / ((double) this.IMaxValuesCount);
                base.AngleToPos(0.0, (double) HalfWidth, (double) HalfHeight, out num2, out num3);
                base.chart.graphics3D.MoveTo(num2, num3, Z);
                for (int i = 0; i <= this.IMaxValuesCount; i++)
                {
                    int num4;
                    int num5;
                    base.AngleToPos(i * num, (double) HalfWidth, (double) HalfHeight, out num4, out num5);
                    if (base.chart.graphics3D.Brush.Visible)
                    {
                        this.FillTriangle(num2, num3, num4, num5, Z);
                    }
                    base.chart.graphics3D.LineTo(num4, num5, Z);
                    num2 = num4;
                    num3 = num5;
                }
            }
        }

        public void DrawRing(double value, int z)
        {
            double num = base.GetVertAxis.Maximum - base.GetVertAxis.Minimum;
            if (num != 0.0)
            {
                num = (value - base.GetVertAxis.Minimum) / num;
                this.DrawPolarCircle(Utils.Round((double) (num * base.XRadius)), Utils.Round((double) (num * base.YRadius)), z);
            }
        }

        public override void DrawValue(int valueIndex)
        {
            int x = this.CalcXPos(valueIndex);
            int y = this.CalcYPos(valueIndex);
            this.LinePrepareCanvas(valueIndex);
            if (valueIndex == base.firstVisible)
            {
                base.chart.graphics3D.MoveTo(x, y, base.StartZ);
            }
            else
            {
                if ((x != this.OldX) || (y != this.OldY))
                {
                    this.TryFillTriangle(valueIndex, x, y);
                    base.chart.graphics3D.LineTo(x, y, base.StartZ);
                }
                if ((valueIndex == base.lastVisible) && this.CloseCircle)
                {
                    if (base.ColorEach)
                    {
                        this.Pen.Color = this.ValueColor(0);
                    }
                    this.OldX = x;
                    this.OldY = y;
                    x = this.CalcXPos(0);
                    y = this.CalcYPos(0);
                    this.TryFillTriangle(valueIndex, x, y);
                    base.chart.graphics3D.LineTo(x, y, base.StartZ);
                    x = this.OldX;
                    y = this.OldY;
                }
            }
            this.OldX = x;
            this.OldY = y;
        }

        private void DrawXGrid()
        {
            if (base.GetHorizAxis.Grid.Visible || this.circleLabels)
            {
                double increment = base.GetHorizAxis.Increment;
                if (increment <= 0.0)
                {
                    increment = 10.0;
                }
                this.SetGridCanvas(base.GetHorizAxis);
                int index = 0;
                double angle = 0.0;
                while (angle < 360.0)
                {
                    if (this.circleLabels)
                    {
                        this.DrawAngleLabel(angle, index);
                    }
                    if (base.GetHorizAxis.Grid.Visible)
                    {
                        int num;
                        int num2;
                        base.AngleToPos(Utils.PiStep * angle, (double) base.XRadius, (double) base.YRadius, out num, out num2);
                        base.chart.graphics3D.Line(base.CircleXCenter, base.CircleYCenter, num, num2, base.EndZ);
                    }
                    angle += increment;
                    index++;
                }
            }
        }

        private void DrawYGrid()
        {
            if (base.GetVertAxis.Grid.Visible)
            {
                double calcIncrement = base.GetVertAxis.CalcIncrement;
                if (calcIncrement > 0.0)
                {
                    this.SetGridCanvas(base.GetVertAxis);
                    double maximum = base.GetVertAxis.Maximum / calcIncrement;
                    if ((Math.Abs(maximum) < 2147483647.0) && (Math.Abs((double) ((base.GetVertAxis.Maximum - base.GetVertAxis.Minimum) / calcIncrement)) < 10000.0))
                    {
                        if (base.GetVertAxis.Labels.RoundFirstLabel)
                        {
                            maximum = calcIncrement * ((int) maximum);
                        }
                        else
                        {
                            maximum = base.GetVertAxis.Maximum;
                        }
                        if (!base.GetVertAxis.Labels.OnAxis)
                        {
                            while (maximum >= base.GetVertAxis.Maximum)
                            {
                                maximum -= calcIncrement;
                            }
                            while (maximum > base.GetVertAxis.Minimum)
                            {
                                this.DrawRing(maximum, base.EndZ);
                                maximum -= calcIncrement;
                            }
                        }
                        else
                        {
                            while (maximum > base.GetVertAxis.Maximum)
                            {
                                maximum -= calcIncrement;
                            }
                            while (maximum >= base.GetVertAxis.Minimum)
                            {
                                this.DrawRing(maximum, base.EndZ);
                                maximum -= calcIncrement;
                            }
                        }
                    }
                }
            }
        }

        public void DrawZone(double Min, double Max, int z)
        {
            double num = base.GetVertAxis.Maximum - base.GetVertAxis.Minimum;
            if (num != 0.0)
            {
                num = (Max - base.GetVertAxis.Minimum) / num;
                this.DrawPolarCircle(Utils.Round((double) (num * base.XRadius)), Utils.Round((double) (num * base.YRadius)), z);
            }
            num = base.GetVertAxis.Maximum - base.GetVertAxis.Minimum;
            if (num != 0.0)
            {
                num = (Min - base.GetVertAxis.Minimum) / num;
                int num2 = Utils.Round((double) (num * base.XRadius));
                int num3 = Utils.Round((double) (num * base.YRadius));
                if (this.IMaxValuesCount == 0)
                {
                    base.chart.graphics3D.TransparentEllipse(base.CircleXCenter - num2, base.CircleYCenter - num3, base.CircleXCenter + num2, base.CircleYCenter + num3, z);
                }
            }
        }

        private void FillTriangle(int aX, int aY, int bX, int bY, int z)
        {
            DashStyle style = base.chart.graphics3D.Pen.Style;
            base.chart.graphics3D.Pen.Visible = false;
            base.chart.graphics3D.Triangle(new Point(aX, aY), new Point(bX, bY), new Point(base.CircleXCenter, base.CircleYCenter), z);
            base.chart.graphics3D.Pen.Style = style;
        }

        private double GetAngleIncrement()
        {
            if (base.chart == null)
            {
                return 10.0;
            }
            double increment = base.GetHorizAxis.Increment;
            if (increment == 0.0)
            {
                increment = 10.0;
            }
            return increment;
        }

        protected virtual string GetCircleLabel(double angle, int index)
        {
            double num = this.clockWiseLabels ? (360.0 - angle) : angle;
            if (num == 360.0)
            {
                num = 0.0;
            }
            return (num.ToString() + "\x00ba");
        }

        private double GetRadiusIncrement()
        {
            if (base.chart != null)
            {
                return base.GetVertAxis.Increment;
            }
            return 0.0;
        }

        protected virtual double GetXValue(int valueIndex)
        {
            return base.vxValues[valueIndex];
        }

        private void LinePrepareCanvas(int valueIndex)
        {
            if (this.Pen.Visible)
            {
                Color color;
                if (valueIndex == -1)
                {
                    color = base.Color;
                }
                else if (this.Pen.Color.IsEmpty)
                {
                    color = this.ValueColor(valueIndex);
                }
                else
                {
                    color = this.Pen.Color;
                }
                base.chart.graphics3D.Pen = this.Pen;
                base.chart.graphics3D.Pen.Color = color;
            }
            else
            {
                base.chart.graphics3D.Pen.Visible = false;
            }
        }

        internal override void PrepareForGallery(bool IsEnabled)
        {
            base.PrepareForGallery(IsEnabled);
            base.Circled = true;
            base.GetHorizAxis.Increment = 90.0;
            base.chart.aspect.Chart3DPercent = 5;
            base.chart.axes.Right.Labels.Visible = false;
            base.chart.axes.Top.Labels.Visible = false;
            base.chart.aspect.Orthogonal = false;
            base.chart.aspect.Elevation = 360;
            base.chart.aspect.Zoom = 90;
        }

        protected override void SetChart(Chart c)
        {
            base.SetChart(c);
            if (this.iPointer != null)
            {
                this.iPointer.Chart = base.chart;
            }
            if ((base.chart != null) && base.DesignMode)
            {
                base.chart.aspect.view3D = false;
            }
            if (this.font != null)
            {
                this.font.Chart = base.chart;
            }
            if (this.pen != null)
            {
                this.pen.Chart = base.chart;
            }
            if (this.circlePen != null)
            {
                this.circlePen.Chart = base.chart;
            }
            if (this.circleLabelsFont != null)
            {
                this.circleLabelsFont.Chart = base.chart;
            }
        }

        private void SetGridCanvas(Axis axis)
        {
            Graphics3D graphicsd = base.chart.graphics3D;
            graphicsd.Brush.Visible = false;
            graphicsd.Pen = axis.Grid;
            if (graphicsd.Pen.Color.IsEmpty)
            {
                graphicsd.Pen.Color = Color.Gray;
            }
        }

        protected override void SetSeriesColor(Color c)
        {
            base.SetSeriesColor(c);
            this.Pen.Color = c;
        }

        private void TryFillTriangle(int valueIndex, int x, int y)
        {
            if (this.Brush.Visible)
            {
                base.chart.SetBrushCanvas(this.ValueColor(valueIndex), this.Brush, this.Brush.Color);
                this.FillTriangle(this.OldX, this.OldY, x, y, base.StartZ);
                this.LinePrepareCanvas(valueIndex);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Description("Sets angle in degrees to draw the dividing grid lines.")]
        public double AngleIncrement
        {
            get
            {
                return this.GetAngleIncrement();
            }
            set
            {
                if (base.chart != null)
                {
                    if (base.GetHorizAxis == null)
                    {
                        base.RecalcGetAxis();
                    }
                    base.GetHorizAxis.Increment = value;
                }
            }
        }

        [Description("Gets list of angle values for each polar point.")]
        public ValueList AngleValues
        {
            get
            {
                return base.vxValues;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Sets Polar Back Brush."), Category("Appearance")]
        public ChartBrush Brush
        {
            get
            {
                return base.bBrush;
            }
        }

        [DefaultValue(false), Description("Sets CicleLabel properties.")]
        public bool CircleLabels
        {
            get
            {
                return this.circleLabels;
            }
            set
            {
                base.SetBooleanProperty(ref this.circleLabels, value);
            }
        }

        public ChartFont CircleLabelsFont
        {
            get
            {
                return this.circleLabelsFont;
            }
        }

        [DefaultValue(false), Description("Displays the axis labels inside the circle area.")]
        public bool CircleLabelsInside
        {
            get
            {
                return this.circleLabelsInside;
            }
            set
            {
                base.SetBooleanProperty(ref this.circleLabelsInside, value);
            }
        }

        [DefaultValue(false)]
        public bool CircleLabelsRotated
        {
            get
            {
                return this.circleLabelsRot;
            }
            set
            {
                base.SetBooleanProperty(ref this.circleLabelsRot, value);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Determines the pen used to draw the outmost circle.")]
        public ChartPen CirclePen
        {
            get
            {
                return this.circlePen;
            }
        }

        [DefaultValue(false), Description("Displays of the circle labels clockwise.")]
        public bool ClockWiseLabels
        {
            get
            {
                return this.clockWiseLabels;
            }
            set
            {
                base.SetBooleanProperty(ref this.clockWiseLabels, value);
            }
        }

        [DefaultValue(true), Description("Draws a Line between the last and first coordinates.")]
        public bool CloseCircle
        {
            get
            {
                return this.closeCircle;
            }
            set
            {
                base.SetBooleanProperty(ref this.closeCircle, value);
            }
        }

        [Description("Sets the label font."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ChartFont Font
        {
            get
            {
                if (this.font == null)
                {
                    this.font = new ChartFont(base.chart);
                }
                return this.font;
            }
        }

        [Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Pen used to draw the Line connecting PolarSeries points.")]
        public ChartPen Pen
        {
            get
            {
                if (this.pen == null)
                {
                    this.pen = new ChartPen(base.chart, Color.Black);
                }
                return this.pen;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("")]
        public SeriesPointer Pointer
        {
            get
            {
                return this.iPointer;
            }
        }

        [Description("Determines the increment used to draw the ring grid lines."), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double RadiusIncrement
        {
            get
            {
                return this.GetRadiusIncrement();
            }
            set
            {
                if (base.chart != null)
                {
                    base.GetVertAxis.Increment = value;
                }
            }
        }

        [Description("Gets list of radius values for each polar point.")]
        public ValueList RadiusValues
        {
            get
            {
                return base.vyValues;
            }
        }

        [Category("Appearance"), DefaultValue(0), Description("Sets Transparency level from 0 to 100%."), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

