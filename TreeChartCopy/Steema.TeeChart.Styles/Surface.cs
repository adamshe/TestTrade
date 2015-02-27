namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    [ToolboxBitmap(typeof(Surface), "SeriesIcons.Surface.bmp")]
    public class Surface : Custom3DGrid
    {
        protected internal bool bWaterFall;
        private bool dotFrame;
        private Point[] points;
        private bool sameBrush;
        private ChartBrush sideBrush;
        private bool smoothPalette;
        private ChartPen waterLines;
        private bool wireFrame;

        public Surface() : this(null)
        {
        }

        public Surface(Chart c) : base(c)
        {
            this.points = new Point[4];
            base.iNextXCell = -1;
            base.iNextZCell = -1;
        }

        private int CalcOnePoint(int tmpRow, int t, ref Point p0, ref Point p1, int tmpYOrigin)
        {
            int index = base[tmpRow, t];
            p0 = this.CalcPointPos(index);
            p1.X = p0.X;
            p1.Y = tmpYOrigin;
            return base.CalcZPos(index);
        }

        private Point CalcPointPos(int index)
        {
            return new Point(base.GetHorizAxis.CalcXPosValue(base.vxValues.Value[index]), base.GetVertAxis.CalcYPosValue(base.vyValues.Value[index]));
        }

        protected internal override void CreateSubGallery(Series.SubGalleryEventHandler AddSubChart)
        {
            base.CreateSubGallery(AddSubChart);
            AddSubChart(Texts.WireFrame);
            AddSubChart(Texts.DotFrame);
            AddSubChart(Texts.Sides);
            AddSubChart(Texts.NoBorder);
        }

        protected internal override void Draw()
        {
            if (base.Count > 0)
            {
                this.PrepareCanvas();
                if (base.chart.graphics3D.SupportsFullRotation)
                {
                    this.FastDraw();
                }
                else
                {
                    this.DrawCells();
                }
                if ((this.sideBrush != null) && this.sideBrush.visible)
                {
                    this.DrawSides();
                }
            }
        }

        private void DrawCell(int x, int z)
        {
            int num = 0;
            bool flag = false;
            if (this.bWaterFall)
            {
                base.valueIndex0 = base[x, z];
                if (base.valueIndex0 > -1)
                {
                    base.valueIndex1 = base[x + base.iNextXCell, z];
                    if (base.valueIndex1 > -1)
                    {
                        this.points[0] = this.CalcPointPos(base.valueIndex0);
                        this.points[1] = this.CalcPointPos(base.valueIndex1);
                        flag = true;
                    }
                }
            }
            else if (this.FourGridIndex(x, z))
            {
                num = base.CalcZPos(base.valueIndex2);
                flag = true;
            }
            if (flag)
            {
                Color baseColor = this.ValueColor(base.valueIndex0);
                if (baseColor != Color.Transparent)
                {
                    int num2 = base.CalcZPos(base.valueIndex0);
                    if (this.sameBrush)
                    {
                        this.DrawTheCell(num2, num);
                    }
                    else
                    {
                        if (this.smoothPalette)
                        {
                            baseColor = base.GetValueColorValue((((base.vyValues.Value[base.valueIndex0] + base.vyValues.Value[base.valueIndex1]) + base.vyValues.Value[base.valueIndex2]) + base.vyValues.Value[base.valueIndex3]) * 0.25);
                        }
                        if (this.wireFrame)
                        {
                            if (base.Pen.Color.A < 0xff)
                            {
                                baseColor = Color.FromArgb(base.Pen.Color.A, baseColor);
                            }
                            base.chart.graphics3D.Pen.Color = baseColor;
                        }
                        else
                        {
                            if (base.Brush.Color.A < 0xff)
                            {
                                baseColor = Color.FromArgb(base.Brush.Color.A, baseColor);
                            }
                            base.chart.graphics3D.Brush.Color = baseColor;
                        }
                        this.DrawTheCell(num2, num);
                    }
                }
            }
        }

        private void DrawCells()
        {
            int num = this.bWaterFall ? 0 : 1;
            if (base.chart.Axes.Depth.Inverted)
            {
                base.iNextZCell = 1;
                if (!this.DrawValuesForward())
                {
                    base.iNextXCell = 1;
                    for (int i = base.iNumXValues - 1; i >= 1; i--)
                    {
                        for (int j = 1; j <= (base.iNumZValues - num); j++)
                        {
                            this.DrawCell(i, j);
                        }
                    }
                }
                else
                {
                    base.iNextXCell = -1;
                    for (int k = 2; k <= base.iNumXValues; k++)
                    {
                        for (int m = 1; m <= (base.iNumZValues - num); m++)
                        {
                            this.DrawCell(k, m);
                        }
                    }
                }
            }
            else
            {
                base.iNextZCell = -1;
                if (!this.DrawValuesForward())
                {
                    base.iNextXCell = 1;
                    for (int n = base.iNumXValues - 1; n >= 1; n--)
                    {
                        for (int num7 = base.iNumZValues; num7 >= (1 + num); num7--)
                        {
                            this.DrawCell(n, num7);
                        }
                    }
                }
                else
                {
                    base.iNextXCell = -1;
                    for (int num8 = 2; num8 <= base.iNumXValues; num8++)
                    {
                        for (int num9 = base.iNumZValues; num9 >= (1 + num); num9--)
                        {
                            this.DrawCell(num8, num9);
                        }
                    }
                }
            }
        }

        private void DrawSideCell(int a, int b, int c, int d, int tmpYOrigin, Graphics3D g)
        {
            Point[] p = new Point[4];
            int z = this.CalcOnePoint(a, b, ref p[0], ref p[1], tmpYOrigin);
            int num2 = this.CalcOnePoint(c, d, ref p[3], ref p[2], tmpYOrigin);
            p[0] = g.Calc3DPoint(p[0], z);
            p[1] = g.Calc3DPoint(p[1], z);
            p[2] = g.Calc3DPoint(p[2], num2);
            p[3] = g.Calc3DPoint(p[3], num2);
            if (Graphics3D.Cull(p))
            {
                g.Polygon(p);
            }
        }

        private void DrawSides()
        {
            Graphics3D g = base.chart.graphics3D;
            g.Brush = this.sideBrush;
            g.Pen.Visible = false;
            int tmpYOrigin = base.CalcYPosValue(base.GetVertAxis.Inverted ? base.vyValues.Maximum : base.vyValues.Minimum);
            int a = base.GetHorizAxis.Inverted ? 1 : base.iNumXValues;
            for (int i = base.iNumZValues; i > 1; i--)
            {
                this.DrawSideCell(a, i, a, i - 1, tmpYOrigin, g);
            }
            a = base.chart.axes.Depth.Inverted ? base.iNumZValues : 1;
            for (int j = 2; j <= base.iNumXValues; j++)
            {
                this.DrawSideCell(j, a, j - 1, a, tmpYOrigin, g);
            }
            Point point = new Point(0, 0);
            Point point2 = new Point(0, 0);
            int z = this.CalcOnePoint(base.iNumXValues, a, ref point, ref point2, tmpYOrigin);
            g.Pen.Visible = true;
            g.VerticalLine(point.X, point.Y, tmpYOrigin, z);
        }

        private void DrawTheCell(int z0, int z1)
        {
            Graphics3D graphicsd = base.chart.graphics3D;
            if (this.bWaterFall)
            {
                int iEndPos = base.GetVertAxis.IEndPos;
                if (!this.wireFrame)
                {
                    graphicsd.Pen.Visible = false;
                    graphicsd.Plane(this.points[0], this.points[1], new Point(this.points[1].X, iEndPos), new Point(this.points[0].X, iEndPos), z0);
                }
                graphicsd.Pen = base.Pen;
                graphicsd.Line(this.points[0].X, this.points[0].Y, this.points[1].X, this.points[1].Y, z0);
                if ((this.waterLines != null) && this.waterLines.Visible)
                {
                    graphicsd.Pen = this.waterLines;
                    graphicsd.VerticalLine(this.points[0].X, this.points[0].Y, iEndPos, z0);
                    graphicsd.VerticalLine(this.points[1].X, this.points[1].Y, iEndPos, z0);
                }
            }
            else if (this.dotFrame)
            {
                graphicsd.Pixel(this.points[0].X, this.points[0].Y, z0, this.ValueColor(base.valueIndex0));
                graphicsd.Pixel(this.points[1].X, this.points[1].Y, z0, this.ValueColor(base.valueIndex1));
                graphicsd.Pixel(this.points[2].X, this.points[2].Y, z1, this.ValueColor(base.valueIndex2));
                graphicsd.Pixel(this.points[3].X, this.points[3].Y, z1, this.ValueColor(base.valueIndex3));
            }
            else
            {
                graphicsd.PlaneFour3D(z0, z1, this.points);
            }
        }

        private void FastDraw()
        {
        }

        private bool FourGridIndex(int x, int z)
        {
            bool flag = base.ExistFourGridIndex(x, z);
            if (flag)
            {
                this.points[0] = this.CalcPointPos(base.valueIndex0);
                this.points[1] = this.CalcPointPos(base.valueIndex1);
                this.points[2] = this.CalcPointPos(base.valueIndex2);
                this.points[3] = this.CalcPointPos(base.valueIndex3);
            }
            return flag;
        }

        private void PrepareCanvas()
        {
            Graphics3D g = base.chart.graphics3D;
            this.PreparePen(g);
            g.Brush = base.Brush;
            g.Brush.Color = base.Color;
            this.sameBrush = (!base.bUseColorRange && !base.bUsePalette) || (g.Brush.Image != null);
            if (this.wireFrame || this.dotFrame)
            {
                g.Brush.visible = false;
            }
        }

        internal override void PrepareForGallery(bool IsEnabled)
        {
            base.PrepareForGallery(IsEnabled);
            base.iInGallery = true;
            base.CreateValues(10, 10);
        }

        protected override void PrepareLegendCanvas(Graphics3D g, int valueIndex, ref Color backColor, ref ChartBrush aBrush)
        {
            base.PrepareLegendCanvas(g, valueIndex, ref backColor, ref aBrush);
            this.PreparePen(g);
        }

        private void PreparePen(Graphics3D g)
        {
            if ((!base.Pen.Visible && !this.wireFrame) && !this.dotFrame)
            {
                g.Pen.Visible = false;
            }
            else
            {
                g.Pen = base.Pen;
            }
        }

        protected override void SetChart(Chart c)
        {
            base.SetChart(c);
            if (this.sideBrush != null)
            {
                this.sideBrush.Chart = c;
            }
            if (this.waterLines != null)
            {
                this.waterLines.Chart = c;
            }
        }

        protected internal override void SetSubGallery(int index)
        {
            switch (index)
            {
                case 2:
                    this.WireFrame = true;
                    return;

                case 3:
                    this.DotFrame = true;
                    return;

                case 4:
                    this.SideBrush.Visible = true;
                    return;

                case 5:
                    base.Pen.Visible = false;
                    return;
            }
            base.SetSubGallery(index);
        }

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.GallerySurface;
            }
        }

        [DefaultValue(false), Description("Sets SurfaceSeries as a grid of dots (pixels).")]
        public bool DotFrame
        {
            get
            {
                return this.dotFrame;
            }
            set
            {
                if (value)
                {
                    base.Pen.Visible = true;
                    this.wireFrame = false;
                }
                base.SetBooleanProperty(ref this.dotFrame, value);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Determines the Brush to fill the sides of a Surface Series.")]
        public ChartBrush SideBrush
        {
            get
            {
                if (this.sideBrush == null)
                {
                    this.sideBrush = new ChartBrush(base.chart, false);
                }
                return this.sideBrush;
            }
        }

        [DefaultValue(false), Description("Determine the cell Colors of a Surface Series.")]
        public bool SmoothPalette
        {
            get
            {
                return this.smoothPalette;
            }
            set
            {
                base.SetBooleanProperty(ref this.smoothPalette, value);
            }
        }

        [DefaultValue(false), Description("Enables/disables the display as a waterfall.")]
        public bool WaterFall
        {
            get
            {
                return this.bWaterFall;
            }
            set
            {
                base.SetBooleanProperty(ref this.bWaterFall, value);
            }
        }

        [Description("Sets Pen to draw valuelines."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ChartPen WaterLines
        {
            get
            {
                if (this.waterLines == null)
                {
                    this.waterLines = new ChartPen(base.chart, Color.Black);
                }
                return this.waterLines;
            }
        }

        [DefaultValue(false), Description("Shows Surface polygons as wire frame when true.")]
        public bool WireFrame
        {
            get
            {
                return this.wireFrame;
            }
            set
            {
                if (value)
                {
                    base.Pen.Visible = true;
                    this.dotFrame = false;
                }
                base.SetBooleanProperty(ref this.wireFrame, value);
            }
        }
    }
}

