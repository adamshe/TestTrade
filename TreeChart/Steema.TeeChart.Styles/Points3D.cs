namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;

    [ToolboxBitmap(typeof(Points3D), "SeriesIcons.Points3D.bmp")]
    public class Points3D : Custom3D
    {
        private double depthSize;
        private int IOldX;
        private int IOldY;
        private int IOldZ;
        private ChartPen linePen;
        private SeriesPointer pointer;

        public event GetPointerStyleEventHandler GetPointerStyle;

        public Points3D() : this(null)
        {
        }

        public Points3D(Chart c) : base(c)
        {
            this.depthSize = 0.0;
            this.pointer = new SeriesPointer(base.chart, this);
            this.linePen = new ChartPen(base.chart, System.Drawing.Color.Black);
        }

        protected override void AddSampleValues(int numValues)
        {
            Series.SeriesRandom random = base.RandomBounds(numValues);
            for (int i = 1; i <= numValues; i++)
            {
                base.Add((double) (100.0 * random.Random()), (double) (100.0 * random.Random()), (double) (100.0 * random.Random()));
            }
        }

        protected internal override void CalcHorizMargins(ref int LeftMargin, ref int RightMargin)
        {
            base.CalcHorizMargins(ref LeftMargin, ref RightMargin);
            this.pointer.CalcHorizMargins(ref LeftMargin, ref RightMargin);
        }

        protected internal override void CalcVerticalMargins(ref int TopMargin, ref int BottomMargin)
        {
            base.CalcVerticalMargins(ref TopMargin, ref BottomMargin);
            this.pointer.CalcVerticalMargins(ref TopMargin, ref BottomMargin);
        }

        private void CalcZPositions(int valueIndex)
        {
            base.middleZ = base.CalcZPos(valueIndex);
            int num = Math.Max(1, base.chart.Axes.Depth.CalcSizeValue(this.depthSize) / 2);
            base.startZ = base.middleZ - num;
            base.endZ = base.middleZ + num;
        }

        public override int Clicked(int x, int y)
        {
            int num = x;
            int num2 = y;
            int num5 = base.Clicked(x, y);
            if ((num5 == -1) && this.pointer.Visible)
            {
                for (int i = 0; i < base.Count; i++)
                {
                    int num3 = this.CalcXPos(i);
                    int num4 = this.CalcYPos(i);
                    x = num;
                    y = num2;
                    if (base.chart != null)
                    {
                        base.chart.graphics3D.Calculate2DPosition(ref x, ref y, base.CalcZPos(i));
                    }
                    if ((Math.Abs((int) (num3 - x)) < this.pointer.HorizSize) && (Math.Abs((int) (num4 - y)) < this.pointer.VertSize))
                    {
                        return i;
                    }
                }
            }
            return num5;
        }

        protected internal override void CreateSubGallery(Series.SubGalleryEventHandler AddSubChart)
        {
            base.CreateSubGallery(AddSubChart);
            AddSubChart(Texts.NoPoint);
            AddSubChart(Texts.NoLine);
            AddSubChart(Texts.Colors);
            AddSubChart(Texts.Marks);
            AddSubChart(Texts.Hollow);
            AddSubChart(Texts.NoBorder);
            AddSubChart(Texts.Point2D);
            AddSubChart(Texts.Triangle);
            AddSubChart(Texts.Star);
            AddSubChart(Texts.Circle);
            AddSubChart(Texts.DownTri);
            AddSubChart(Texts.Cross);
            AddSubChart(Texts.Diamond);
        }

        protected override void DrawLegendShape(Graphics3D g, int valueIndex, Rectangle r)
        {
            if (this.pointer.Visible)
            {
                System.Drawing.Color color = (valueIndex == -1) ? this.Color : this.ValueColor(valueIndex);
                this.pointer.DrawLegendShape(g, color, r, this.LinePen.Visible);
            }
            else
            {
                base.DrawLegendShape(g, valueIndex, r);
            }
        }

        protected internal override void DrawMark(int valueIndex, string s, SeriesMarks.Position position)
        {
            this.CalcZPositions(valueIndex);
            base.Marks.ZPosition = this.pointer.Visible ? ((base.StartZ + base.EndZ) / 2) : base.StartZ;
            base.Marks.ApplyArrowLength(ref position);
            base.DrawMark(valueIndex, s, position);
        }

        public override void DrawValue(int valueIndex)
        {
            this.CalcZPositions(valueIndex);
            int px = this.CalcXPos(valueIndex);
            int py = this.CalcYPos(valueIndex);
            if (this.pointer.Visible)
            {
                System.Drawing.Color colorValue = this.ValueColor(valueIndex);
                this.pointer.PrepareCanvas(base.chart.graphics3D, colorValue);
                PointerStyles style = this.pointer.Style;
                if (this.GetPointerStyle != null)
                {
                    CustomPoint.GetPointerStyleEventArgs e = new CustomPoint.GetPointerStyleEventArgs(valueIndex, style);
                    this.GetPointerStyle(this, e);
                    style = e.Style;
                }
                this.pointer.Draw(base.chart.graphics3D, base.chart.Aspect.View3D, px, py, this.pointer.HorizSize, this.pointer.VertSize, colorValue, style);
            }
            if ((valueIndex > base.firstVisible) && this.linePen.Visible)
            {
                base.chart.graphics3D.Pen = this.LinePen;
                base.chart.graphics3D.MoveTo(this.IOldX, this.IOldY, this.IOldZ);
                base.chart.graphics3D.LineTo(px, py, base.MiddleZ);
            }
            this.IOldX = px;
            this.IOldY = py;
            this.IOldZ = base.MiddleZ;
        }

        public override double MaxZValue()
        {
            return (base.vzValues.Maximum + this.depthSize);
        }

        internal override void PrepareForGallery(bool isEnabled)
        {
            base.PrepareForGallery(isEnabled);
            this.linePen.Color = System.Drawing.Color.Navy;
            base.chart.Aspect.Zoom = 60;
        }

        protected override void SetChart(Chart value)
        {
            base.SetChart(value);
            this.pointer.Chart = value;
            this.linePen.Chart = value;
        }

        protected internal override void SetSubGallery(int index)
        {
            switch (index)
            {
                case 1:
                    this.Pointer.Visible = false;
                    return;

                case 2:
                    this.LinePen.Visible = false;
                    return;

                case 3:
                    base.ColorEach = true;
                    return;

                case 4:
                    base.Marks.Visible = true;
                    return;

                case 5:
                    this.Pointer.Brush.Visible = false;
                    return;

                case 6:
                    this.Pointer.Pen.Visible = false;
                    return;

                case 7:
                    this.Pointer.Draw3D = false;
                    return;

                case 8:
                    this.Pointer.Style = PointerStyles.Triangle;
                    return;

                case 9:
                    this.Pointer.Style = PointerStyles.Star;
                    return;

                case 10:
                    this.Pointer.Style = PointerStyles.Circle;
                    return;

                case 11:
                    this.Pointer.Style = PointerStyles.DownTriangle;
                    return;

                case 12:
                    this.Pointer.Style = PointerStyles.Cross;
                    return;

                case 13:
                    this.Pointer.Style = PointerStyles.Diamond;
                    return;
            }
            base.SetSubGallery(index);
        }

        [Description("Default color for all points. See also: ColorEach property."), Category("Appearance"), DefaultValue(typeof(System.Drawing.Color), ""), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public System.Drawing.Color Color
        {
            get
            {
                return this.GetSeriesColor();
            }
            set
            {
                this.SetSeriesColor(value);
                if (this.pointer.Color != value)
                {
                    this.Pointer.Color = value;
                }
                this.Invalidate();
            }
        }

        [DefaultValue((double) 0.0), Description("Sets the Depth of each 3DPoint to the value of DepthSize.")]
        public double DepthSize
        {
            get
            {
                return this.depthSize;
            }
            set
            {
                base.SetDoubleProperty(ref this.depthSize, value);
            }
        }

        public override string Description
        {
            get
            {
                return Texts.GalleryPoint3D;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Sets the Pen for the Point3D connecting Lines."), Category("Appearance")]
        public ChartPen LinePen
        {
            get
            {
                return this.linePen;
            }
        }

        [DefaultValue((string) null), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Each point in a PointSeries is drawn using the Pointer properties.")]
        public SeriesPointer Pointer
        {
            get
            {
                if (this.pointer == null)
                {
                    this.pointer = new SeriesPointer(base.chart, this);
                }
                if (this.Color != this.pointer.Color)
                {
                    this.Color = this.pointer.Color;
                }
                this.Invalidate();
                return this.pointer;
            }
        }

        public delegate void GetPointerStyleEventHandler(Points3D series, CustomPoint.GetPointerStyleEventArgs e);
    }
}

