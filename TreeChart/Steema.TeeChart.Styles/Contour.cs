namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;

    [ToolboxBitmap(typeof(Contour), "SeriesIcons.Contour.bmp")]
    public class Contour : Custom3DGrid
    {
        private bool automaticLevels;
        private ContourDraw cDraw;
        protected internal bool iModifyingLevels;
        private ContourLevels levels;
        private int numLevels;
        private double yPosition;
        private bool yPositionLevel;

        public event GetLevelEventHandler GetLevel;

        public Contour() : this(null)
        {
        }

        public Contour(Chart c) : base(c)
        {
            this.automaticLevels = true;
            this.levels = new ContourLevels();
            this.numLevels = 10;
            if (this.cDraw == null)
            {
                this.cDraw = new ContourDraw(this);
            }
            base.ColorEach = true;
        }

        protected override void AddSampleValues(int numValues)
        {
            base.AddSampleValues(numValues);
            this.yPosition = 0.5 * (base.vyValues.Maximum + base.vyValues.Minimum);
        }

        protected internal override int CountLegendItems()
        {
            return this.levels.Count;
        }

        internal void CreateAutoLevels()
        {
            if (this.automaticLevels && (this.numLevels > 0))
            {
                this.iModifyingLevels = true;
                base.chart.AutoRepaint = false;
                double num = base.YValues.Range / ((double) this.numLevels);
                this.levels.Clear();
                for (int i = 0; i < this.numLevels; i++)
                {
                    ContourLevel level = new ContourLevel(this);
                    this.levels.Add(level);
                    level.UpToValue = base.vyValues.Minimum + (num * i);
                    level.Color = base.ColorEach ? this.ValueColor(i) : base.GetValueColorValue(level.UpToValue);
                    if (this.GetLevel != null)
                    {
                        GetLevelEventArgs e = new GetLevelEventArgs(i, level.UpToValue, level.Color);
                        this.GetLevel(this, e);
                        level.UpToValue = e.Value;
                        level.Color = e.Color;
                    }
                }
                this.iModifyingLevels = false;
                base.chart.AutoRepaint = true;
            }
        }

        protected internal override void CreateSubGallery(Series.SubGalleryEventHandler AddSubChart)
        {
            base.CreateSubGallery(AddSubChart);
            AddSubChart(Texts.Colors);
            AddSubChart(Texts.Positions);
        }

        protected internal override void DoBeforeDrawChart()
        {
            base.DoBeforeDrawChart();
            if (base.Count > 0)
            {
                this.CreateAutoLevels();
            }
        }

        protected internal override void Draw()
        {
            if (base.Count > 0)
            {
                this.cDraw.Draw();
            }
        }

        private int GetNumLevels()
        {
            if (this.AutomaticLevels)
            {
                return this.numLevels;
            }
            return this.Levels.Count;
        }

        public override double MaxYValue()
        {
            if (!base.chart.Aspect.View3D)
            {
                return base.vzValues.Maximum;
            }
            return base.MaxYValue();
        }

        public override double MinYValue()
        {
            if (!base.chart.Aspect.View3D)
            {
                return base.vzValues.Minimum;
            }
            return base.MinYValue();
        }

        private void SetNumLevels(int value)
        {
            base.SetIntegerProperty(ref this.numLevels, value);
            if (this.AutomaticLevels)
            {
                this.levels.Clear();
                this.AutomaticLevels = true;
            }
        }

        protected internal override void SetSubGallery(int index)
        {
            if (index == 2)
            {
                base.ColorEach = true;
            }
            else if (index == 3)
            {
                this.YPositionLevel = true;
            }
            else
            {
                base.SetSubGallery(index);
            }
        }

        [DefaultValue(true), Description("Sets ContourSeries Automatic Levels.")]
        public bool AutomaticLevels
        {
            get
            {
                return this.automaticLevels;
            }
            set
            {
                base.SetBooleanProperty(ref this.automaticLevels, value);
            }
        }

        public override string Description
        {
            get
            {
                return Texts.GalleryContour;
            }
        }

        [Description("Accesses ContourLevel characteristics by Level index.")]
        public ContourLevels Levels
        {
            get
            {
                return this.levels;
            }
            set
            {
                this.levels = value;
            }
        }

        [DefaultValue(10), Description("Sets the number of levels for the ContourSeries.")]
        public int NumLevels
        {
            get
            {
                return this.numLevels;
            }
            set
            {
                this.SetNumLevels(value);
            }
        }

        [Description("Sets the Y-Axis height of the Contour Series.")]
        public double YPosition
        {
            get
            {
                return this.yPosition;
            }
            set
            {
                base.SetDoubleProperty(ref this.yPosition, value);
            }
        }

        [Description("Enables/disables YPosition to be set."), DefaultValue(false)]
        public bool YPositionLevel
        {
            get
            {
                return this.yPositionLevel;
            }
            set
            {
                base.SetBooleanProperty(ref this.yPositionLevel, value);
            }
        }

        public class ContourDraw : TeeBase
        {
            private int[] cellX;
            private int[] cellZ;
            private Contour cs;
            private double[] difY;
            private TempLevel[] iLevels;
            private static byte[,,] Sides = new byte[,,] { { { 0, 0, 8 }, { 0, 2, 5 }, { 7, 6, 9 } }, { { 0, 3, 4 }, { 1, 3, 1 }, { 4, 3, 0 } }, { { 9, 6, 7 }, { 5, 2, 0 }, { 8, 0, 0 } } };
            private TempLevel tLevel;
            private int tmpNumLevels;

            public ContourDraw(Contour cSeries) : base(cSeries.chart)
            {
                this.difY = new double[5];
                this.cellX = new int[5];
                this.cellZ = new int[5];
                this.cs = cSeries;
            }

            private void CalcLevel(int theLevel)
            {
                int num;
                int[] numArray = new int[5];
                for (num = 0; num <= 4; num++)
                {
                    if (this.difY[num] > 0.0)
                    {
                        numArray[num] = 1;
                    }
                    else if (this.difY[num] < 0.0)
                    {
                        numArray[num] = -1;
                    }
                    else
                    {
                        numArray[num] = 0;
                    }
                }
                int num2 = numArray[0];
                for (num = 1; num <= 4; num++)
                {
                    int index = num;
                    int num4 = (num == 4) ? 1 : (num + 1);
                    int side = Sides[1 + numArray[index], 1 + num2, 1 + numArray[num4]];
                    if (side != 0)
                    {
                        this.CalcLinePoints(index, num4, side, theLevel);
                    }
                }
            }

            private void CalcLinePoints(int m1, int m3, int side, int theLevel)
            {
                TempLevel level = this.iLevels[theLevel];
                if (level.Count >= level.Allocated)
                {
                    level.Allocated += 0x3e8;
                    level.Line = new LevelLine[level.Allocated];
                }
                LevelLine line = level.Line[level.Count];
                switch (side)
                {
                    case 1:
                        line.x1 = this.cellX[m1];
                        line.z1 = this.cellZ[m1];
                        line.x2 = this.cellX[0];
                        line.z2 = this.cellZ[0];
                        break;

                    case 2:
                        line.x1 = this.cellX[0];
                        line.z1 = this.cellZ[0];
                        line.x2 = this.cellX[m3];
                        line.z2 = this.cellZ[m3];
                        break;

                    case 3:
                        line.x1 = this.cellX[m3];
                        line.z1 = this.cellZ[m3];
                        line.x2 = this.cellX[m1];
                        line.z2 = this.cellZ[m1];
                        break;

                    case 4:
                        line.x1 = this.cellX[m1];
                        line.z1 = this.cellZ[m1];
                        this.PointSect(0, m3, ref line.x2, ref line.z2);
                        break;

                    case 5:
                        line.x1 = this.cellX[0];
                        line.z1 = this.cellZ[0];
                        this.PointSect(m3, m1, ref line.x2, ref line.z2);
                        break;

                    case 6:
                        line.x1 = this.cellX[m3];
                        line.z1 = this.cellZ[m3];
                        this.PointSect(m1, 0, ref line.x2, ref line.z2);
                        break;

                    case 7:
                        this.PointSect(m1, 0, ref line.x1, ref line.z1);
                        this.PointSect(0, m3, ref line.x2, ref line.z2);
                        break;

                    case 8:
                        this.PointSect(0, m3, ref line.x1, ref line.z1);
                        this.PointSect(m3, m1, ref line.x2, ref line.z2);
                        break;

                    case 9:
                        this.PointSect(m3, m1, ref line.x1, ref line.z1);
                        this.PointSect(m1, 0, ref line.x2, ref line.z2);
                        break;
                }
                level.Line[level.Count] = line;
                level.Count++;
                this.iLevels[theLevel] = level;
            }

            protected internal void Draw()
            {
                Axis depth = null;
                if (this.cs.Count > 0)
                {
                    this.cs.iNextXCell = 1;
                    this.cs.iNextZCell = 1;
                    if (this.cs.chart.Aspect.View3D)
                    {
                        depth = this.cs.chart.Axes.Depth;
                    }
                    else
                    {
                        depth = this.cs.GetVertAxis;
                    }
                    this.tmpNumLevels = this.cs.NumLevels;
                    this.iLevels = new TempLevel[this.tmpNumLevels];
                    ContourLevel level = null;
                    int index = 0;
                    while (index < this.tmpNumLevels)
                    {
                        level = this.cs.Levels[index];
                        this.iLevels[index].UpToValue = level.UpToValue;
                        this.iLevels[index].Color = level.Color;
                        this.iLevels[index].Count = 0;
                        this.iLevels[index].Allocated = 0;
                        index++;
                    }
                    for (int i = 0; i < this.cs.iNumZValues; i++)
                    {
                        for (int j = 0; j < this.cs.iNumXValues; j++)
                        {
                            if (this.cs.ExistFourGridIndex(j, i))
                            {
                                this.cellZ[1] = depth.CalcYPosValue(this.cs.vzValues[this.cs.valueIndex0]);
                                this.cellZ[3] = depth.CalcYPosValue(this.cs.vzValues[this.cs.valueIndex3]);
                                this.cellZ[2] = this.cellZ[1];
                                this.cellZ[4] = this.cellZ[3];
                                this.cellZ[0] = (this.cellZ[1] + this.cellZ[3]) / 2;
                                Axis getHorizAxis = this.cs.GetHorizAxis;
                                this.cellX[1] = getHorizAxis.CalcXPosValue(this.cs.XValues[this.cs.valueIndex0]);
                                this.cellX[2] = getHorizAxis.CalcXPosValue(this.cs.XValues[this.cs.valueIndex1]);
                                this.cellX[3] = this.cellX[2];
                                this.cellX[4] = this.cellX[1];
                                this.cellX[0] = (this.cellX[1] + this.cellX[2]) / 2;
                                ValueList yValues = this.cs.YValues;
                                double num6 = yValues[this.cs.valueIndex0];
                                double num7 = yValues[this.cs.valueIndex3];
                                double num8 = yValues[this.cs.valueIndex1];
                                double num9 = yValues[this.cs.valueIndex2];
                                double num4 = num6;
                                if (num7 < num4)
                                {
                                    num4 = num7;
                                }
                                if (num8 < num4)
                                {
                                    num4 = num8;
                                }
                                if (num9 < num4)
                                {
                                    num4 = num9;
                                }
                                if (num4 <= this.iLevels[this.tmpNumLevels - 1].UpToValue)
                                {
                                    double num5 = num6;
                                    if (num7 > num5)
                                    {
                                        num5 = num7;
                                    }
                                    if (num8 > num5)
                                    {
                                        num5 = num8;
                                    }
                                    if (num9 > num5)
                                    {
                                        num5 = num9;
                                    }
                                    double num10 = 0.25 * (((num6 + num7) + num8) + num9);
                                    if (num5 >= this.iLevels[0].UpToValue)
                                    {
                                        for (index = 0; index < this.tmpNumLevels; index++)
                                        {
                                            this.tLevel = this.iLevels[index];
                                            if ((this.tLevel.UpToValue >= num4) && (this.tLevel.UpToValue <= num5))
                                            {
                                                this.difY[1] = num6 - this.tLevel.UpToValue;
                                                this.difY[2] = num8 - this.tLevel.UpToValue;
                                                this.difY[3] = num9 - this.tLevel.UpToValue;
                                                this.difY[4] = num7 - this.tLevel.UpToValue;
                                                this.difY[0] = num10 - this.tLevel.UpToValue;
                                                this.CalcLevel(index);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    this.DrawLevelLines();
                }
            }

            private void DrawLevelLines()
            {
                Graphics3D graphicsd = this.cs.chart.Graphics3D;
                int y = this.cs.GetVertAxis.CalcYPosValue(this.cs.yPosition);
                for (int i = 0; i < this.tmpNumLevels; i++)
                {
                    TempLevel level = this.iLevels[i];
                    graphicsd.Pen = this.cs.Pen;
                    graphicsd.Pen.Color = level.Color;
                    if (this.cs.yPositionLevel)
                    {
                        y = this.cs.GetVertAxis.CalcYPosValue(level.UpToValue);
                    }
                    for (int j = 0; j < level.Count; j++)
                    {
                        LevelLine line = level.Line[j];
                        if (this.cs.chart.aspect.View3D)
                        {
                            graphicsd.MoveTo(line.x1, y, line.z1);
                            graphicsd.LineTo(line.x2, y, line.z2);
                        }
                        else
                        {
                            graphicsd.Line(line.x1, line.z1, line.x2, line.z2);
                        }
                    }
                }
            }

            private void PointSect(int p1, int p2, ref int aX, ref int aZ)
            {
                double num = this.difY[p2] - this.difY[p1];
                if (num != 0.0)
                {
                    num = 1.0 / num;
                    aX = (int) Math.Round((double) (((this.difY[p2] * this.cellX[p1]) - (this.difY[p1] * this.cellX[p2])) * num));
                    aZ = (int) Math.Round((double) (((this.difY[p2] * this.cellZ[p1]) - (this.difY[p1] * this.cellZ[p2])) * num));
                }
                else
                {
                    aX = Utils.Round((float) (this.cellX[p2] - this.cellX[p1]));
                    aZ = Utils.Round((float) (this.cellZ[p2] - this.cellZ[p1]));
                }
            }
        }

        public class GetLevelEventArgs : EventArgs
        {
            private System.Drawing.Color color;
            private readonly int lIndex;
            private double lValue;

            public GetLevelEventArgs(int LevelIndex, double Value, System.Drawing.Color Color)
            {
                this.lIndex = LevelIndex;
                this.lValue = Value;
                this.color = Color;
            }

            [Description("Sets the Colour of the ContourSeries Level.")]
            public System.Drawing.Color Color
            {
                get
                {
                    return this.color;
                }
                set
                {
                    this.color = value;
                }
            }

            [Description("Accesses the  ContourLevel characteristics selecting Level by index.")]
            public int LevelIndex
            {
                get
                {
                    return this.lIndex;
                }
            }

            [Description("Gets or Sets the values in the Value array.")]
            public double Value
            {
                get
                {
                    return this.lValue;
                }
                set
                {
                    this.lValue = value;
                }
            }
        }

        public delegate void GetLevelEventHandler(Contour sender, Contour.GetLevelEventArgs e);
    }
}

