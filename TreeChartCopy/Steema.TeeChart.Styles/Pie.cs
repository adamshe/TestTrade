namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Reflection;
    using System.Runtime.InteropServices;

    [ToolboxBitmap(typeof(Pie), "SeriesIcons.Pie.bmp")]
    public class Pie : Circular
    {
        public PieAngle[] Angles;
        private int angleSize;
        private bool autoMarkPosition;
        public const int BelongsToOther = -1;
        private bool dark3D;
        private bool darkPen;
        private int explodeBiggest;
        private ExplodedSliceList explodedSlice;
        protected int iDonutPercent;
        internal int IniX;
        internal int IniY;
        private bool IsExploded;
        private const int OtherFlag = 0x7fffffff;
        private PieOtherSlice otherSlice;
        private ChartPen pen;
        private PieShadow shadow;
        private int[] sortedSlice;
        private bool usePatterns;

        public Pie() : this(null)
        {
        }

        public Pie(Chart c) : base(c)
        {
            this.angleSize = 360;
            this.dark3D = true;
            this.explodedSlice = new ExplodedSliceList(0);
            this.autoMarkPosition = true;
            base.bColorEach = true;
            this.pen = new ChartPen(base.chart, Color.Black);
            base.Marks.Visible = true;
            base.marks.Arrow.Color = Color.Black;
            base.marks.Arrow.defaultColor = Color.Black;
            base.marks.Callout.Length = 8;
            base.marks.defaultArrowLength = 8;
            base.UseSeriesColor = false;
        }

        protected override void AddSampleValues(int numValues)
        {
            string[] strArray = new string[] { Texts.PieSample1, Texts.PieSample2, Texts.PieSample3, Texts.PieSample4, Texts.PieSample5, Texts.PieSample6, Texts.PieSample7, Texts.PieSample8 };
            Series.SeriesRandom random = base.RandomBounds(numValues);
            for (int i = 0; i < numValues; i++)
            {
                base.Add((double) (1 + Utils.Round((double) (1000.0 * random.Random()))), strArray[i % 8]);
            }
        }

        public bool BelongsToOtherSlice(int valueIndex)
        {
            return (base.vxValues.Value[valueIndex] == -1.0);
        }

        private void CalcAngles()
        {
            double num = (6.2831853071795862 * this.angleSize) / 360.0;
            double totalABS = base.YValues.TotalABS;
            double num3 = (totalABS != 0.0) ? (num / totalABS) : 0.0;
            this.Angles = new PieAngle[base.Count];
            double num4 = 0.0;
            for (int i = 0; i < base.Count; i++)
            {
                this.Angles[i] = new PieAngle();
                this.Angles[i].StartAngle = (i == 0) ? 0.0 : this.Angles[i - 1].EndAngle;
                if (totalABS != 0.0)
                {
                    if (!this.BelongsToOtherSlice(i))
                    {
                        num4 += Math.Abs(base.vyValues[i]);
                    }
                    if (num4 == totalABS)
                    {
                        this.Angles[i].EndAngle = num;
                    }
                    else
                    {
                        this.Angles[i].EndAngle = num4 * num3;
                    }
                    if ((this.Angles[i].EndAngle - this.Angles[i].StartAngle) > num)
                    {
                        this.Angles[i].EndAngle = this.Angles[i].StartAngle + num;
                    }
                }
                else
                {
                    this.Angles[i].EndAngle = num;
                }
                this.Angles[i].MidAngle = (this.Angles[i].StartAngle + this.Angles[i].EndAngle) * 0.5;
            }
        }

        private int CalcClickedPie(int x, int y)
        {
            if (base.Chart != null)
            {
                base.Chart.Graphics3D.Calculate2DPosition(ref x, ref y, base.Chart.Aspect.Width3D);
            }
            double angle = base.PointToAngle(x, y);
            for (int i = 0; i < base.Count; i++)
            {
                int num2;
                int num3;
                this.CalcExplodedOffset(i, out num2, out num3);
                if (((Math.Abs((int) (x - base.CircleXCenter)) <= (base.XRadius + num2)) && (Math.Abs((int) (y - base.CircleYCenter)) <= (base.YRadius + num3))) && this.Angles[i].Contains(angle))
                {
                    return i;
                }
            }
            return -1;
        }

        private void CalcExplodeBiggest()
        {
            int index = base.YValues.IndexOf(base.YValues.Maximum);
            if (index != -1)
            {
                this.explodedSlice[index] = this.explodeBiggest;
            }
        }

        private void CalcExplodedOffset(int valueIndex, out int offsetX, out int offsetY)
        {
            offsetX = 0;
            offsetY = 0;
            if (this.IsExploded)
            {
                double num = this.explodedSlice[valueIndex];
                if (num > 0.0)
                {
                    double midAngle = this.Angles[valueIndex].MidAngle;
                    if (base.chart.graphics3D.SupportsFullRotation)
                    {
                        midAngle += ((1.5707963267948966 * this.angleSize) / 360.0) + 3.1415926535897931;
                    }
                    double resultSin = 0.0;
                    double resultCos = 0.0;
                    Utils.SinCos(midAngle + base.rotDegree, out resultSin, out resultCos);
                    num *= 0.01;
                    offsetX = Utils.Round((double) ((base.iXRadius * num) * resultCos));
                    offsetY = Utils.Round((double) ((base.iYRadius * num) * resultSin));
                }
            }
        }

        protected void CalcExplodedRadius(int valueIndex, out int aXRadius, out int aYRadius)
        {
            double num = 1.0 + (this.explodedSlice[valueIndex] * 0.01);
            aXRadius = Utils.Round((double) (base.iXRadius * num));
            aYRadius = Utils.Round((double) (base.iYRadius * num));
        }

        public override int CalcXPos(int valueIndex)
        {
            if (base.vxValues[valueIndex] == 2147483647.0)
            {
                return 0;
            }
            return base.CalcXPos(valueIndex);
        }

        protected override void ClearLists()
        {
            base.ClearLists();
            this.explodedSlice.Clear();
        }

        public override int Clicked(int x, int y)
        {
            int num = base.Clicked(x, y);
            if (num == -1)
            {
                num = this.CalcClickedPie(x, y);
            }
            return num;
        }

        private int CompareSlice(int A, int B)
        {
            double totalAngle = (6.2831853071795862 * this.angleSize) / 360.0;
            double angleSlice = this.GetAngleSlice(this.sortedSlice[A], totalAngle);
            double num3 = this.GetAngleSlice(this.sortedSlice[B], totalAngle);
            if (angleSlice < num3)
            {
                return -1;
            }
            if (angleSlice > num3)
            {
                return 1;
            }
            return 0;
        }

        protected internal override int CountLegendItems()
        {
            int num = 0;
            for (int i = 0; i < base.Count; i++)
            {
                if (this.BelongsToOtherSlice(i))
                {
                    num++;
                }
            }
            if ((base.chart.Legend != null) && (base.chart.Legend == this.OtherSlice.Legend))
            {
                return num;
            }
            return (base.Count - num);
        }

        protected internal override void CreateSubGallery(Series.SubGalleryEventHandler AddSubChart)
        {
            base.CreateSubGallery(AddSubChart);
            AddSubChart(Texts.Patterns);
            AddSubChart(Texts.Exploded);
            AddSubChart(Texts.Shadow);
            AddSubChart(Texts.Marks);
            AddSubChart(Texts.SemiPie);
            AddSubChart(Texts.NoBorder);
            AddSubChart(Texts.DarkPen);
        }

        private void DisableRotation()
        {
            base.chart.aspect.Orthogonal = false;
            base.chart.aspect.Rotation = 0;
            base.chart.aspect.Elevation = 0x131;
        }

        protected internal override void DoBeforeDrawChart()
        {
            if (this.PieValues.Order != ValueListOrder.None)
            {
                this.PieValues.Sort();
            }
            for (int i = 0; i < base.Count; i++)
            {
                if (base.vxValues.Value[i] == 2147483647.0)
                {
                    base.Delete(i);
                    break;
                }
            }
            base.XValues.FillSequence();
            if (((this.otherSlice != null) && (this.otherSlice.Style != PieOtherStyles.None)) && (base.YValues.TotalABS > 0.0))
            {
                bool flag = false;
                double y = 0.0;
                for (int j = 0; j < base.Count; j++)
                {
                    double num = base.YValues[j];
                    if (this.otherSlice.Style == PieOtherStyles.BelowPercent)
                    {
                        num = (num * 100.0) / base.YValues.TotalABS;
                    }
                    if (num < this.otherSlice.Value)
                    {
                        y += base.YValues[j];
                        base.XValues[j] = -1.0;
                        flag = true;
                    }
                }
                if (flag)
                {
                    base.Add((double) 2147483647.0, y, this.otherSlice.Text, this.otherSlice.Color);
                    Steema.TeeChart.Styles.ValueList yValues = base.YValues;
                    yValues.totalABS -= y;
                    base.YValues.statsOk = true;
                }
            }
        }

        protected internal override void Draw()
        {
            if (this.explodeBiggest > 0)
            {
                this.CalcExplodeBiggest();
            }
            int valueIndex = -1;
            int num2 = 0;
            int count = base.Count;
            for (int i = 0; i < this.explodedSlice.Count; i++)
            {
                if (this.explodedSlice[i] > num2)
                {
                    num2 = Utils.Round((float) this.explodedSlice[i]);
                    valueIndex = i;
                }
            }
            this.CalcAngles();
            this.IsExploded = valueIndex != -1;
            if (this.IsExploded)
            {
                int num5;
                int num6;
                this.CalcExplodedOffset(valueIndex, out num5, out num6);
                base.CircleRect.Inflate(-Math.Abs(num5) / 2, -Math.Abs(num6) / 2);
                base.AdjustCircleRect();
                base.CalcRadius();
            }
            base.AngleToPos(0.0, (double) base.iXRadius, (double) base.iYRadius, out this.IniX, out this.IniY);
            Graphics3D graphicsd = base.chart.graphics3D;
            if ((((this.shadow != null) && this.shadow.bVisible) && !this.shadow.Color.IsEmpty) && ((this.shadow.Width != 0) || (this.shadow.Height != 0)))
            {
                graphicsd.Brush = this.shadow.Brush;
                graphicsd.Pen.Visible = false;
                Rectangle rCircleRect = base.rCircleRect;
                rCircleRect.Offset(this.shadow.Width, this.shadow.Height);
                graphicsd.Ellipse(rCircleRect, base.EndZ - 10);
            }
            Rectangle chartRect = base.chart.ChartRect;
            if ((this.OtherSlice.Legend != null) && this.OtherSlice.Legend.Visible)
            {
                Legend legend = base.chart.Legend;
                base.chart.legend = this.OtherSlice.Legend;
                base.chart.DoDrawLegend(ref chartRect);
                base.chart.legend = legend;
            }
            if ((base.chart.Aspect.View3D && (this.IsExploded || (this.iDonutPercent > 0))) && !graphicsd.SupportsFullRotation)
            {
                if (this.sortedSlice == null)
                {
                    this.sortedSlice = new int[count];
                }
                for (int j = 0; j < count; j++)
                {
                    this.sortedSlice[j] = j;
                }
                Utils.Sort(0, count - 1, new Utils.CompareEventHandler(this.CompareSlice), new Utils.SwapEventHandler(this.SwapSlice));
                for (int k = 0; k < count; k++)
                {
                    this.DrawValue(this.sortedSlice[k]);
                }
            }
            else
            {
                base.Draw();
            }
        }

        protected internal override void DrawMark(int valueIndex, string s, SeriesMarks.Position position)
        {
            if (!this.BelongsToOtherSlice(valueIndex))
            {
                int num;
                int num2;
                double midAngle;
                this.CalcExplodedRadius(valueIndex, out num, out num2);
                if (base.chart.graphics3D.SupportsFullRotation)
                {
                    midAngle = (this.Angles[valueIndex].MidAngle + 3.1415926535897931) + 1.5707963267948966;
                    base.Marks.zPosition = base.StartZ;
                }
                else
                {
                    midAngle = this.Angles[valueIndex].MidAngle;
                    base.Marks.zPosition = base.EndZ;
                }
                position.ArrowFix = true;
                int x = 0;
                int y = 0;
                int distance = base.Marks.Callout.Length + base.Marks.Callout.Distance;
                base.AngleToPos(midAngle, (double) (num + distance), (double) (num2 + distance), out x, out y);
                position.ArrowTo.X = x;
                position.ArrowTo.Y = y;
                distance = base.Marks.Callout.Distance;
                base.AngleToPos(midAngle, (double) (num + distance), (double) (num2 + distance), out x, out y);
                position.ArrowFrom.X = x;
                position.ArrowFrom.Y = y;
                if (position.ArrowTo.X > base.iCircleXCenter)
                {
                    position.LeftTop.X = position.ArrowTo.X;
                }
                else
                {
                    position.LeftTop.X = position.ArrowTo.X - position.Width;
                }
                if (position.ArrowTo.Y > base.iCircleYCenter)
                {
                    position.LeftTop.Y = position.ArrowTo.Y;
                }
                else
                {
                    position.LeftTop.Y = position.ArrowTo.Y - position.Height;
                }
                if (this.AutoMarkPosition)
                {
                    base.Marks.AntiOverlap(base.firstVisible, valueIndex, position);
                }
                base.DrawMark(valueIndex, s, position);
            }
        }

        protected internal void DrawPie(int valueIndex)
        {
            int num;
            int num2;
            this.CalcExplodedOffset(valueIndex, out num, out num2);
            Graphics3D graphicsd = base.chart.graphics3D;
            if (base.chart.Aspect.View3D || (this.iDonutPercent == 0))
            {
                graphicsd.Pie(base.iCircleXCenter + num, base.iCircleYCenter - num2, base.iXRadius, base.iYRadius, base.StartZ, base.EndZ, this.Angles[valueIndex].StartAngle + base.rotDegree, this.Angles[valueIndex].EndAngle + base.rotDegree, this.dark3D, this.IsExploded, this.iDonutPercent);
            }
            else
            {
                graphicsd.Donut(base.iCircleXCenter + num, base.iCircleYCenter - num2, base.iXRadius, base.iYRadius, this.Angles[valueIndex].StartAngle + base.rotDegree, this.Angles[valueIndex].EndAngle + base.rotDegree, (double) this.iDonutPercent);
            }
        }

        public override void DrawValue(int valueIndex)
        {
            if (((base.CircleWidth > 4) && (base.CircleHeight > 4)) && !this.BelongsToOtherSlice(valueIndex))
            {
                if (this.usePatterns || base.chart.graphics3D.Monochrome)
                {
                    base.bBrush.Style = Graphics3D.GetDefaultPattern(valueIndex);
                }
                else
                {
                    base.bBrush.Solid = true;
                }
                Color aColor = base.chart.graphics3D.Monochrome ? Color.Black : this.ValueColor(valueIndex);
                base.chart.SetBrushCanvas(aColor, base.bBrush, base.CalcCircleBackColor());
                this.PreparePiePen(base.chart.graphics3D, valueIndex);
                this.DrawPie(valueIndex);
            }
        }

        protected internal override void GalleryChanged3D(bool is3D)
        {
            base.GalleryChanged3D(is3D);
            this.DisableRotation();
            base.Circled = !base.chart.Aspect.View3D;
        }

        private double GetAngleSlice(int index, double TotalAngle)
        {
            double num = this.Angles[index].MidAngle + base.rotDegree;
            if (num > TotalAngle)
            {
                num -= TotalAngle;
            }
            if (num > (0.25 * TotalAngle))
            {
                num -= 0.25 * TotalAngle;
                if (num > 3.1415926535897931)
                {
                    num = TotalAngle - num;
                }
                return num;
            }
            return ((0.25 * TotalAngle) - num);
        }

        protected internal override int LegendToValueIndex(int legendIndex)
        {
            int num = -1;
            bool flag2 = (base.chart.Legend != null) && (base.chart.Legend == this.OtherSlice.Legend);
            for (int i = 0; i < base.Count; i++)
            {
                bool flag = this.BelongsToOtherSlice(i);
                if ((flag2 && flag) || (!flag2 && !flag))
                {
                    num++;
                    if (num == legendIndex)
                    {
                        return i;
                    }
                }
            }
            return legendIndex;
        }

        protected internal override int NumSampleValues()
        {
            return 8;
        }

        internal override void PrepareForGallery(bool IsEnabled)
        {
            base.PrepareForGallery(IsEnabled);
            base.FillSampleValues(8);
            base.chart.Aspect.Chart3DPercent = 0x4b;
            base.Marks.Callout.Length = 0;
            base.Marks.DrawEvery = 1;
            this.DisableRotation();
            this.ColorEach = IsEnabled;
        }

        protected override void PrepareLegendCanvas(Graphics3D g, int valueIndex, ref Color backColor, ref ChartBrush aBrush)
        {
            base.PrepareLegendCanvas(g, valueIndex, ref backColor, ref aBrush);
            this.PreparePiePen(g, valueIndex);
            if (this.usePatterns || g.Monochrome)
            {
                aBrush.Style = Graphics3D.GetDefaultPattern(valueIndex);
            }
            else
            {
                aBrush.Solid = true;
            }
        }

        private void PreparePiePen(Graphics3D g, int valueIndex)
        {
            g.Pen = this.pen;
            if (this.darkPen)
            {
                Color c = this.ValueColor(valueIndex);
                Graphics3D.ApplyDark(ref c, 0x80);
                g.Pen.Color = c;
            }
        }

        protected override void SetChart(Chart c)
        {
            base.SetChart(c);
            if (this.pen != null)
            {
                this.pen.Chart = base.chart;
            }
            if (this.shadow != null)
            {
                this.shadow.Chart = base.chart;
            }
        }

        protected void SetDonutPercent(int value)
        {
            base.SetIntegerProperty(ref this.iDonutPercent, value);
        }

        protected internal override void SetSubGallery(int index)
        {
            switch (index)
            {
                case 1:
                    this.UsePatterns = true;
                    return;

                case 2:
                    this.ExplodeBiggest = 30;
                    return;

                case 3:
                    this.Shadow.Visible = true;
                    this.Shadow.Width = 10;
                    this.Shadow.Height = 10;
                    return;

                case 4:
                    base.Marks.Visible = true;
                    this.Clear();
                    base.Add(30.0, "A");
                    base.Add(70.0, "B");
                    return;

                case 5:
                    this.AngleSize = 180;
                    return;

                case 6:
                    this.Pen.Visible = false;
                    return;

                case 7:
                    this.DarkPen = true;
                    return;
            }
        }

        private void SwapSlice(int a, int b)
        {
            int num = this.sortedSlice[a];
            this.sortedSlice[a] = this.sortedSlice[b];
            this.sortedSlice[b] = num;
        }

        internal override void SwapValueIndex(int a, int b)
        {
            base.SwapValueIndex(a, b);
            this.explodedSlice.Exchange(a, b);
        }

        [Description("Total angle in degrees (0 to 360) for all slices."), DefaultValue(360)]
        public int AngleSize
        {
            get
            {
                return this.angleSize;
            }
            set
            {
                base.SetIntegerProperty(ref this.angleSize, value);
            }
        }

        [DefaultValue(true), Description("If true, marks will be displayed trying to not overlap one to each other.")]
        public bool AutoMarkPosition
        {
            get
            {
                return this.autoMarkPosition;
            }
            set
            {
                base.SetBooleanProperty(ref this.autoMarkPosition, value);
            }
        }

        [Category("Appearance"), Description("Brush fill for PieSeries."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ChartBrush Brush
        {
            get
            {
                return base.bBrush;
            }
        }

        [Description("Draws points with different preset Colors."), DefaultValue(true)]
        public bool ColorEach
        {
            get
            {
                return base.ColorEach;
            }
            set
            {
                base.ColorEach = value;
            }
        }

        [DefaultValue(true), Category("Appearance"), Description("Darkens side of 3D pie section to add depth.")]
        public bool Dark3D
        {
            get
            {
                return this.dark3D;
            }
            set
            {
                base.SetBooleanProperty(ref this.dark3D, value);
            }
        }

        [Description("Darkens pie slice borders."), DefaultValue(false)]
        public bool DarkPen
        {
            get
            {
                return this.darkPen;
            }
            set
            {
                base.SetBooleanProperty(ref this.darkPen, value);
            }
        }

        public override string Description
        {
            get
            {
                return Texts.GalleryPie;
            }
        }

        [Description("Displaces the biggest slice from centre by value set."), DefaultValue(0)]
        public int ExplodeBiggest
        {
            get
            {
                return this.explodeBiggest;
            }
            set
            {
                base.SetIntegerProperty(ref this.explodeBiggest, value);
                this.CalcExplodeBiggest();
            }
        }

        [Description("Accesses the properties for exploding any Pie slice.")]
        public ExplodedSliceList ExplodedSlice
        {
            get
            {
                return this.explodedSlice;
            }
        }

        [Description("Accesses the OtherSlice properties.")]
        public PieOtherSlice OtherSlice
        {
            get
            {
                if (this.otherSlice == null)
                {
                    this.otherSlice = new PieOtherSlice(base.chart, this);
                }
                return this.otherSlice;
            }
        }

        [Description("Line pen for Pie."), Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ChartPen Pen
        {
            get
            {
                return this.pen;
            }
        }

        [Description("Stores the Pie slice values.")]
        public Steema.TeeChart.Styles.ValueList PieValues
        {
            get
            {
                return base.vyValues;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Defines the offset shadow of the PieSeries.")]
        public PieShadow Shadow
        {
            get
            {
                if (this.shadow == null)
                {
                    this.shadow = new PieShadow(base.chart);
                }
                return this.shadow;
            }
        }

        [Description("Fills Pie Sectors with different Brush pattern styles."), DefaultValue(false)]
        public bool UsePatterns
        {
            get
            {
                return this.usePatterns;
            }
            set
            {
                base.SetBooleanProperty(ref this.usePatterns, value);
            }
        }

        public class ExplodedSliceList : ArrayList
        {
            public ExplodedSliceList(int capacity) : base(capacity)
            {
            }

            internal void Exchange(int a, int b)
            {
                int num = this[a];
                this[a] = this[b];
                this[b] = num;
            }

            public int this[int index]
            {
                get
                {
                    if (index < this.Count)
                    {
                        return (int) base[index];
                    }
                    return 0;
                }
                set
                {
                    while (this.Count <= index)
                    {
                        this.Add(0);
                    }
                    base[index] = value;
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PieAngle
        {
            public double StartAngle;
            public double MidAngle;
            public double EndAngle;
            public bool Contains(double angle)
            {
                return ((angle >= this.StartAngle) && (angle <= this.EndAngle));
            }
        }

        public class PieOtherSlice : TeeBase
        {
            private double aValue;
            private System.Drawing.Color color;
            private Steema.TeeChart.Legend legend;
            private Series series;
            private PieOtherStyles style;
            private string text;

            public PieOtherSlice(Chart c, Series s) : base(c)
            {
                this.style = PieOtherStyles.None;
                if (this.series == null)
                {
                    this.series = s;
                }
            }

            private Steema.TeeChart.Legend GetLegend()
            {
                if (this.legend == null)
                {
                    this.legend = new Steema.TeeChart.Legend(this.series.Chart);
                    this.legend.Visible = false;
                    this.legend.Series = this.series;
                }
                return this.legend;
            }

            private void SetLegend(Steema.TeeChart.Legend l)
            {
                if (this.legend != null)
                {
                    this.legend = l;
                    this.legend.Series = this.series;
                }
            }

            [Description("Sets the Color of the OtherSlice."), Category("Appearance")]
            public System.Drawing.Color Color
            {
                get
                {
                    return this.color;
                }
                set
                {
                    base.SetColorProperty(ref this.color, value);
                }
            }

            [Description("PieOtherSlice Legend.")]
            public Steema.TeeChart.Legend Legend
            {
                get
                {
                    return this.GetLegend();
                }
                set
                {
                    this.SetLegend(value);
                }
            }

            [Description("Sets either value or percentage to group 'other' Pie slice.")]
            public PieOtherStyles Style
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

            [Description("Title for otherSlice.")]
            public string Text
            {
                get
                {
                    return this.text;
                }
                set
                {
                    base.SetStringProperty(ref this.text, value);
                }
            }

            [Description("Value (value or percentage) for Otherslice grouping.")]
            public double Value
            {
                get
                {
                    return this.aValue;
                }
                set
                {
                    base.SetDoubleProperty(ref this.aValue, value);
                }
            }
        }

        public class PieShadow : Shadow
        {
            public PieShadow(Chart c) : base(c)
            {
                base.bBrush.defaultColor = Color.DarkGray;
                base.bBrush.color = Color.DarkGray;
                base.defaultVisible = false;
                base.bVisible = false;
                base.Width = 20;
                base.Height = 20;
            }
        }
    }
}

