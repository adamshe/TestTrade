namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    [ToolboxBitmap(typeof(TriSurface), "SeriesIcons.TriSurface.bmp")]
    public class TriSurface : Custom3DPalette
    {
        private double ARMN;
        private double ARMX;
        private ChartPen border;
        public bool CacheTriangles;
        private Color[] Colors;
        private double DSQ12;
        private double DSQI;
        private double DSQMN;
        private double DSQMX;
        private double DXMN;
        private double DXMX;
        private double DYMN;
        private double DYMX;
        private bool hide;
        private bool ICreated;
        protected bool ImprovedTriangles;
        private int ip1;
        private int ip2;
        private int[] IPL;
        private int ipl1;
        private int ipl2;
        private int IPMN1;
        private int IPMN2;
        private int[] IPT;
        private int IPTI1;
        private int IPTI2;
        private int[] ITF;
        private int ITT3;
        private int[] IWL;
        private int[] IWP;
        private int JPMN;
        private int JPMX;
        private int JWL;
        private int NDP0;
        private int NDPM1;
        private int NLF;
        private int NLN;
        private int NLNT3;
        private int NLT3;
        private const int NRep = 100;
        private int NSH;
        private int NTT3;
        private int numLines;
        public int NumTriangles;
        private Triangle3D Points;
        private const double Ratio = 1E-06;
        private int tmp;
        private int tmpCount;
        private bool tmpForward;
        private Triangle Triangles;
        private double[] WK;
        private double xd1;
        private double xd2;
        private double yd1;
        private double yd2;

        public TriSurface() : this(null)
        {
        }

        public TriSurface(Chart c) : base(c)
        {
            this.hide = true;
            this.ITF = new int[3];
            this.Colors = new Color[3];
            this.ImprovedTriangles = true;
        }

        private void AddByZ(ref Triangle ATriangle)
        {
            Triangle triangle2 = null;
            for (Triangle triangle = this.Triangles; triangle != null; triangle = triangle.Next)
            {
                if ((this.tmpForward && (ATriangle.Z > triangle.Z)) || (!this.tmpForward && (triangle.Z > ATriangle.Z)))
                {
                    if (triangle.Prev != null)
                    {
                        ATriangle.Prev = triangle.Prev;
                        triangle.Prev.Next = ATriangle;
                    }
                    else
                    {
                        this.Triangles = ATriangle;
                    }
                    triangle.Prev = ATriangle;
                    ATriangle.Next = triangle;
                    return;
                }
                triangle2 = triangle;
            }
            if (triangle2 != null)
            {
                triangle2.Next = ATriangle;
                ATriangle.Prev = triangle2;
            }
            else
            {
                this.Triangles = ATriangle;
            }
        }

        private void AddFirst()
        {
            this.ip1 = this.IPMN1;
            this.ip2 = this.IPMN2;
            int num = this.IWP[3];
            if (this.Side(base.XValues[this.ip1], base.vzValues[this.ip1], base.XValues[this.ip2], base.vzValues[this.ip2], base.XValues[num], base.vzValues[num]) < 0.0)
            {
                this.ip1 = this.IPMN2;
                this.ip2 = this.IPMN1;
            }
            this.NumTriangles = 1;
            this.NTT3 = 3;
            this.IPT[1] = this.ip1;
            this.IPT[2] = this.ip2;
            this.IPT[3] = num;
            this.numLines = 3;
            this.NLT3 = 9;
            this.IPL[1] = this.ip1;
            this.IPL[2] = this.ip2;
            this.IPL[3] = 1;
            this.IPL[4] = this.ip2;
            this.IPL[5] = num;
            this.IPL[6] = 1;
            this.IPL[7] = num;
            this.IPL[8] = this.ip1;
            this.IPL[9] = 1;
        }

        protected override void AddSampleValues(int numValues)
        {
            Random random = new Random();
            for (int i = 1; i <= (4 + numValues); i++)
            {
                double x = (1000.0 * random.NextDouble()) * 0.001;
                double z = (1000.0 * random.NextDouble()) * 0.001;
                base.Add(x, Utils.Sqr(Math.Exp(z)) * Math.Cos(x * z), z);
            }
        }

        private void AddTriangles()
        {
            this.JWL = 0;
            this.NLNT3 = 0;
            for (int i = this.JPMX; i <= this.numLines; i++)
            {
                int index = i * 3;
                this.ipl1 = this.IPL[index - 2];
                this.ipl2 = this.IPL[index - 1];
                int num3 = this.IPL[index];
                this.NumTriangles++;
                this.NTT3 += 3;
                this.IPT[this.NTT3 - 2] = this.ipl2;
                this.IPT[this.NTT3 - 1] = this.ipl1;
                this.IPT[this.NTT3] = this.ip1;
                if (i == this.JPMX)
                {
                    this.IPL[index - 1] = this.ip1;
                    this.IPL[index] = this.NumTriangles;
                }
                if (i == this.numLines)
                {
                    this.NLN = this.JPMX + 1;
                    this.NLNT3 = this.NLN * 3;
                    this.IPL[this.NLNT3 - 2] = this.ip1;
                    this.IPL[this.NLNT3 - 1] = this.IPL[1];
                    this.IPL[this.NLNT3] = this.NumTriangles;
                }
                this.ITT3 = num3 * 3;
                int num2 = this.IPT[this.ITT3 - 2];
                if ((num2 == this.ipl1) || (num2 == this.ipl2))
                {
                    num2 = this.IPT[this.ITT3 - 1];
                    if ((num2 == this.ipl1) || (num2 == this.ipl2))
                    {
                        num2 = this.IPT[this.ITT3];
                    }
                }
                if (this.IDxchg(this.ip1, num2, this.ipl1, this.ipl2) != 0)
                {
                    this.IPT[this.ITT3 - 2] = num2;
                    this.IPT[this.ITT3 - 1] = this.ipl1;
                    this.IPT[this.ITT3] = this.ip1;
                    this.IPT[this.NTT3 - 1] = num2;
                    if (i == this.JPMX)
                    {
                        this.IPL[index] = num3;
                    }
                    if ((i == this.numLines) && (this.IPL[3] == num3))
                    {
                        this.IPL[3] = this.NumTriangles;
                    }
                    this.JWL += 4;
                    this.IWL[this.JWL - 3] = this.ipl1;
                    this.IWL[this.JWL - 2] = num2;
                    this.IWL[this.JWL - 1] = num2;
                    this.IWL[this.JWL] = this.ipl2;
                }
            }
        }

        private void CalcBorder()
        {
            for (int i = 1; i <= (this.NLT3 / 3); i++)
            {
                int index = i * 3;
                int num2 = this.IPL[index - 2];
                int num3 = this.IPL[index - 1];
                if (((num2 == this.ipl1) && (num3 == this.IPTI2)) || ((num3 == this.ipl1) && (num2 == this.IPTI2)))
                {
                    this.IPL[index] = this.ITF[1];
                }
                if (((num2 == this.ipl2) && (num3 == this.IPTI1)) || ((num3 == this.ipl2) && (num2 == this.IPTI1)))
                {
                    this.IPL[index] = this.ITF[2];
                }
            }
        }

        private void CalcPoint(ref Point3D p, int APoint, int Index)
        {
            p.X = this.CalcXPos(Index);
            p.Y = this.CalcYPos(Index);
            p.Z = base.CalcZPos(Index);
            this.Colors[APoint] = this.ValueColor(Index);
        }

        private void CalcTriangle(int jp1)
        {
            this.ip1 = this.IWP[jp1];
            this.xd1 = base.XValues[this.ip1];
            this.yd1 = base.vzValues[this.ip1];
            this.ip2 = this.IPL[1];
            this.JPMN = 1;
            this.xd2 = base.XValues[this.ip2];
            this.yd2 = base.vzValues[this.ip2];
            this.DXMN = this.xd2 - this.xd1;
            this.DYMN = this.yd2 - this.yd1;
            this.DSQMN = Utils.Sqr(this.DXMN) + Utils.Sqr(this.DYMN);
            this.ARMN = this.DSQMN * 1E-06;
            this.JPMX = 1;
            this.DXMX = this.DXMN;
            this.DYMX = this.DYMN;
            this.DSQMX = this.DSQMN;
            this.ARMX = this.ARMN;
            this.Part1();
            if (this.JPMX < this.JPMN)
            {
                this.JPMX += this.numLines;
            }
            this.NSH = this.JPMN - 1;
            if (this.NSH > 0)
            {
                this.ShiftIPLArray();
            }
            this.AddTriangles();
            this.numLines = this.NLN;
            this.NLT3 = this.NLNT3;
            this.NLF = this.JWL / 2;
            if ((this.NLF != 0) && this.ImprovedTriangles)
            {
                this.ImproveTriangles();
            }
        }

        private void CheckColinear()
        {
            double num = this.DSQ12 * 1E-06;
            this.xd1 = base.XValues[this.IPMN1];
            this.yd1 = base.vzValues[this.IPMN1];
            double num2 = base.XValues[this.IPMN2] - this.xd1;
            double num3 = base.vzValues[this.IPMN2] - this.yd1;
            int num6 = 0;
            int index = 3;
            double num4 = 0.0;
            while ((index <= this.NDP0) && (num4 <= num))
            {
                num6 = this.IWP[index];
                num4 = Math.Abs((double) (((base.vzValues[num6] - this.yd1) * num2) - ((base.XValues[num6] - this.xd1) * num3)));
                index++;
            }
            index--;
            if (index == this.NDP0)
            {
                throw new TeeChartException(Texts.TriSurfaceAllColinear);
            }
            if (index != 3)
            {
                int num7 = index;
                index = num7 + 1;
                for (int i = 4; i <= num7; i++)
                {
                    index--;
                    this.IWP[index] = this.IWP[index - 1];
                }
                this.IWP[3] = num6;
            }
        }

        public override void Clear()
        {
            base.Clear();
            this.ICreated = false;
        }

        private void CreateTriangles()
        {
            this.tmpCount = base.Count;
            this.NDP0 = this.tmpCount - 1;
            this.NDPM1 = this.NDP0 - 1;
            if (this.FindClosestPair())
            {
                throw new TeeChartException(Texts.TriSurfaceSimilar);
            }
            this.IPT = new int[(6 * this.tmpCount) - 15];
            this.IPL = new int[6 * this.tmpCount];
            this.IWL = new int[0x12 * this.tmpCount];
            this.IWP = new int[this.tmpCount];
            this.WK = new double[this.tmpCount];
            this.DSQ12 = this.DSQMN;
            this.SortRest();
            this.CheckColinear();
            this.AddFirst();
            for (int i = 4; i <= this.NDP0; i++)
            {
                this.CalcTriangle(i);
            }
            this.ICreated = true;
        }

        protected override void DoBeforeDrawValues()
        {
            base.DoBeforeDrawValues();
            if (base.Count < 4)
            {
                this.NumTriangles = 0;
                throw new TeeChartException(Texts.TriSurfaceLess);
            }
            if (!this.CacheTriangles || !this.ICreated)
            {
                this.CreateTriangles();
            }
        }

        protected internal override void Draw()
        {
            Graphics3D graphicsd = base.chart.graphics3D;
            if (base.Pen.Visible || base.Brush.Visible)
            {
                graphicsd.Brush = base.Brush;
                graphicsd.Pen = base.Pen;
                if ((this.HideTriangles && !graphicsd.SupportsFullRotation) && base.Brush.Solid)
                {
                    this.tmpForward = !base.chart.axes.Depth.inverted;
                    this.Triangles = null;
                    for (int i = 1; i <= this.NumTriangles; i++)
                    {
                        this.tmp = 3 * i;
                        this.CalcPoint(ref this.Points.p0, 0, this.IPT[this.tmp - 2]);
                        this.CalcPoint(ref this.Points.p1, 1, this.IPT[this.tmp - 1]);
                        this.CalcPoint(ref this.Points.p2, 2, this.IPT[this.tmp]);
                        Triangle aTriangle = new Triangle();
                        aTriangle.Next = null;
                        aTriangle.Prev = null;
                        aTriangle.Z = Math.Max(base.vzValues[this.IPT[this.tmp]], Math.Max(base.vzValues[this.IPT[this.tmp - 1]], base.vzValues[this.IPT[this.tmp - 2]]));
                        aTriangle.P[0] = base.chart.graphics3D.Calc3DPoint(this.Points.p0);
                        aTriangle.P[1] = base.chart.graphics3D.Calc3DPoint(this.Points.p1);
                        aTriangle.P[2] = base.chart.graphics3D.Calc3DPoint(this.Points.p2);
                        aTriangle.Color = this.Colors[0];
                        this.AddByZ(ref aTriangle);
                    }
                    for (Triangle triangle2 = this.Triangles; triangle2 != null; triangle2 = triangle2.Next)
                    {
                        graphicsd.Brush.Color = triangle2.Color;
                        graphicsd.Polygon(triangle2.P);
                    }
                }
                else
                {
                    for (int j = 1; j <= this.NumTriangles; j++)
                    {
                        this.tmp = 3 * j;
                        this.CalcPoint(ref this.Points.p0, 0, this.IPT[this.tmp - 2]);
                        this.CalcPoint(ref this.Points.p1, 1, this.IPT[this.tmp - 1]);
                        this.CalcPoint(ref this.Points.p2, 2, this.IPT[this.tmp]);
                        graphicsd.Brush.Color = this.Colors[0];
                        graphicsd.Triangle(this.Points);
                    }
                }
            }
            if ((this.border != null) && this.border.bVisible)
            {
                graphicsd.Pen = this.border;
                for (int k = 1; k <= this.numLines; k++)
                {
                    this.CalcPoint(ref this.Points.p0, 0, this.IPL[(3 * k) - 2]);
                    graphicsd.MoveTo(this.Points.p0);
                    this.CalcPoint(ref this.Points.p1, 1, this.IPL[(3 * k) - 1]);
                    graphicsd.LineTo(this.Points.p1);
                }
            }
        }

        protected internal override void DrawMark(int valueIndex, string s, SeriesMarks.Position position)
        {
            base.Marks.ZPosition = base.CalcZPos(valueIndex);
            base.Marks.ApplyArrowLength(ref position);
            base.DrawMark(valueIndex, s, position);
        }

        private bool FindClosestPair()
        {
            bool flag = false;
            this.DSQMN = Utils.Sqr(base.XValues[1] - base.XValues[0]) + Utils.Sqr(base.vzValues[1] - base.vzValues[0]);
            this.IPMN1 = 1;
            this.IPMN2 = 2;
            this.ip1 = 1;
            while ((this.ip1 <= this.NDPM1) && !flag)
            {
                this.xd1 = base.XValues[this.ip1];
                this.yd1 = base.vzValues[this.ip1];
                this.ip2 = this.ip1 + 1;
                while ((this.ip2 <= this.NDP0) && !flag)
                {
                    this.xd2 = base.XValues[this.ip2];
                    this.yd2 = base.vzValues[this.ip2];
                    this.DSQI = Utils.Sqr(this.xd2 - this.xd1) + Utils.Sqr(this.yd2 - this.yd1);
                    if (this.DSQI == 0.0)
                    {
                        for (int i = this.ip2; i < this.NDP0; i++)
                        {
                            base.XValues[i] = base.XValues[i + 1];
                            base.vzValues[i] = base.vzValues[i + 1];
                        }
                        this.tmpCount--;
                        this.NDP0--;
                        this.NDPM1--;
                        this.ip2--;
                        flag = true;
                    }
                    else if (this.DSQI < this.DSQMN)
                    {
                        this.DSQMN = this.DSQI;
                        this.IPMN1 = this.ip1;
                        this.IPMN2 = this.ip2;
                    }
                    this.ip2++;
                }
                this.ip1++;
            }
            return flag;
        }

        private int IDxchg(int I1, int I2, int I3, int I4)
        {
            int num23 = 0;
            double num = base.XValues[I1];
            double num5 = base.vzValues[I1];
            double num2 = base.XValues[I2];
            double num6 = base.vzValues[I2];
            double num3 = base.XValues[I3];
            double num7 = base.vzValues[I3];
            double num4 = base.XValues[I4];
            double num8 = base.vzValues[I4];
            double num11 = ((num6 - num7) * (num - num3)) - ((num2 - num3) * (num5 - num7));
            double num12 = ((num5 - num8) * (num2 - num4)) - ((num - num4) * (num6 - num8));
            if ((num11 * num12) > 0.0)
            {
                double num9 = ((num7 - num5) * (num4 - num)) - ((num3 - num) * (num8 - num5));
                double num10 = ((num8 - num6) * (num3 - num2)) - ((num4 - num2) * (num7 - num6));
                double num13 = Utils.Sqr(num - num3) + Utils.Sqr(num5 - num7);
                double num15 = Utils.Sqr(num4 - num) + Utils.Sqr(num8 - num5);
                double num17 = Utils.Sqr(num3 - num4) + Utils.Sqr(num7 - num8);
                double num14 = Utils.Sqr(num2 - num4) + Utils.Sqr(num6 - num8);
                double num16 = Utils.Sqr(num3 - num2) + Utils.Sqr(num7 - num6);
                double num18 = Utils.Sqr(num2 - num) + Utils.Sqr(num6 - num5);
                double num19 = Utils.Sqr(num9) / (num17 * Math.Max(num13, num15));
                double num20 = Utils.Sqr(num10) / (num17 * Math.Max(num14, num16));
                double num21 = Utils.Sqr(num11) / (num18 * Math.Max(num16, num13));
                double num22 = Utils.Sqr(num12) / (num18 * Math.Max(num15, num14));
                if (Math.Min(num19, num20) < Math.Min(num21, num22))
                {
                    num23 = 1;
                }
            }
            return num23;
        }

        private void ImproveTriangles()
        {
            int num10 = this.NTT3 + 3;
            for (int i = 1; i <= 100; i++)
            {
                for (int j = 1; j <= this.NLF; j++)
                {
                    this.ipl1 = this.IWL[(j * 2) - 1];
                    this.ipl2 = this.IWL[j * 2];
                    int index = 0;
                    bool flag = true;
                    this.tmp = 3;
                    while (flag && (this.tmp <= this.NTT3))
                    {
                        this.ITT3 = num10 - this.tmp;
                        int num3 = this.IPT[this.ITT3 - 2];
                        int num4 = this.IPT[this.ITT3 - 1];
                        int num5 = this.IPT[this.ITT3];
                        if ((((this.ipl1 == num3) || (this.ipl1 == num4)) || (this.ipl1 == num5)) && (((this.ipl2 == num3) || (this.ipl2 == num4)) || (this.ipl2 == num5)))
                        {
                            index++;
                            this.ITF[index] = this.ITT3 / 3;
                            if (index == 2)
                            {
                                flag = false;
                            }
                        }
                        this.tmp += 3;
                    }
                    if (index >= 2)
                    {
                        int num7 = this.ITF[1] * 3;
                        this.IPTI1 = this.IPT[num7 - 2];
                        if ((this.IPTI1 == this.ipl1) || (this.IPTI1 == this.ipl2))
                        {
                            this.IPTI1 = this.IPT[num7 - 1];
                            if ((this.IPTI1 == this.ipl1) || (this.IPTI1 == this.ipl2))
                            {
                                this.IPTI1 = this.IPT[num7];
                            }
                        }
                        int num8 = this.ITF[2] * 3;
                        this.IPTI2 = this.IPT[num8 - 2];
                        if ((this.IPTI2 == this.ipl1) || (this.IPTI2 == this.ipl2))
                        {
                            this.IPTI2 = this.IPT[num8 - 1];
                            if ((this.IPTI2 == this.ipl1) || (this.IPTI2 == this.ipl2))
                            {
                                this.IPTI2 = this.IPT[num8];
                            }
                        }
                        if (this.IDxchg(this.IPTI1, this.IPTI2, this.ipl1, this.ipl2) != 0)
                        {
                            this.IPT[num7 - 2] = this.IPTI1;
                            this.IPT[num7 - 1] = this.IPTI2;
                            this.IPT[num7] = this.ipl1;
                            this.IPT[num8 - 2] = this.IPTI2;
                            this.IPT[num8 - 1] = this.IPTI1;
                            this.IPT[num8] = this.ipl2;
                            this.JWL += 8;
                            this.IWL[this.JWL - 7] = this.ipl1;
                            this.IWL[this.JWL - 6] = this.IPTI1;
                            this.IWL[this.JWL - 5] = this.IPTI1;
                            this.IWL[this.JWL - 4] = this.ipl2;
                            this.IWL[this.JWL - 3] = this.ipl2;
                            this.IWL[this.JWL - 2] = this.IPTI2;
                            this.IWL[this.JWL - 1] = this.IPTI2;
                            this.IWL[this.JWL] = this.ipl1;
                            this.CalcBorder();
                        }
                    }
                }
                this.tmp = this.NLF;
                this.NLF = this.JWL / 2;
                if (this.NLF == this.tmp)
                {
                    return;
                }
                this.JWL = 0;
                this.tmp = (this.tmp + 1) * 2;
                int num2 = 2 * this.NLF;
                while (this.tmp <= num2)
                {
                    this.JWL += 2;
                    this.IWL[this.JWL - 1] = this.IWL[this.tmp - 1];
                    this.IWL[this.JWL] = this.IWL[this.tmp];
                    this.tmp += 2;
                }
                this.NLF = this.JWL / 2;
            }
        }

        protected internal override int NumSampleValues()
        {
            return 15;
        }

        private void Part1()
        {
            for (int i = 2; i <= this.numLines; i++)
            {
                this.ip2 = this.IPL[(3 * i) - 2];
                this.xd2 = base.XValues[this.ip2];
                this.yd2 = base.vzValues[this.ip2];
                double num2 = this.xd2 - this.xd1;
                double num3 = this.yd2 - this.yd1;
                double num = (num3 * this.DXMN) - (num2 * this.DYMN);
                if (num <= this.ARMN)
                {
                    this.DSQI = Utils.Sqr(num2) + Utils.Sqr(num3);
                    if ((num < -this.ARMN) || (this.DSQI < this.DSQMN))
                    {
                        this.JPMN = i;
                        this.DXMN = num2;
                        this.DYMN = num3;
                        this.DSQMN = this.DSQI;
                        this.ARMN = this.DSQMN * 1E-06;
                    }
                }
                num = (num3 * this.DXMX) - (num2 * this.DYMX);
                if (num >= -this.ARMX)
                {
                    this.DSQI = Utils.Sqr(num2) + Utils.Sqr(num3);
                    if ((num > this.ARMX) || (this.DSQI < this.DSQMX))
                    {
                        this.JPMX = i;
                        this.DXMX = num2;
                        this.DYMX = num3;
                        this.DSQMX = this.DSQI;
                        this.ARMX = this.DSQMX * 1E-06;
                    }
                }
            }
        }

        private void ShiftIPLArray()
        {
            int num;
            for (int i = 1; i <= this.NSH; i++)
            {
                this.tmp = i * 3;
                num = this.tmp + this.NLT3;
                this.IPL[num - 2] = this.IPL[this.tmp - 2];
                this.IPL[num - 1] = this.IPL[this.tmp - 1];
                this.IPL[num] = this.IPL[this.tmp];
            }
            for (int j = 1; j <= (this.NLT3 / 3); j++)
            {
                this.tmp = j * 3;
                num = this.tmp + (this.NSH * 3);
                this.IPL[this.tmp - 2] = this.IPL[num - 2];
                this.IPL[this.tmp - 1] = this.IPL[num - 1];
                this.IPL[this.tmp] = this.IPL[num];
            }
            this.JPMX -= this.NSH;
        }

        private double Side(double u1, double v1, double u2, double v2, double u3, double v3)
        {
            return (((v3 - v1) * (u2 - u1)) - ((u3 - u1) * (v2 - v1)));
        }

        private void SortRest()
        {
            double num3;
            double num = (base.XValues[this.IPMN1] + base.XValues[this.IPMN2]) * 0.5;
            double num2 = (base.vzValues[this.IPMN1] + base.vzValues[this.IPMN2]) * 0.5;
            int index = 2;
            for (int i = 1; i <= this.NDP0; i++)
            {
                if ((i != this.IPMN1) && (i != this.IPMN2))
                {
                    index++;
                    this.IWP[index] = i;
                    num3 = Utils.Sqr(base.XValues[i] - num) + Utils.Sqr(base.vzValues[i] - num2);
                    this.WK[index] = num3;
                }
            }
            for (index = 3; index <= this.NDPM1; index++)
            {
                this.DSQMN = this.WK[index];
                this.JPMN = index;
                for (int j = index; j <= this.NDP0; j++)
                {
                    num3 = this.WK[j];
                    if (num3 < this.DSQMN)
                    {
                        this.DSQMN = num3;
                        this.JPMN = j;
                    }
                }
                int num4 = this.IWP[index];
                this.IWP[index] = this.IWP[this.JPMN];
                this.IWP[this.JPMN] = num4;
                this.WK[this.JPMN] = this.WK[index];
            }
        }

        public override string Description
        {
            get
            {
                return Texts.GalleryTriSurface;
            }
        }

        [Description(""), DefaultValue(true)]
        public bool HideTriangles
        {
            get
            {
                return this.hide;
            }
            set
            {
                base.SetBooleanProperty(ref this.hide, value);
            }
        }

        [Description("Accesses the TChartHiddenPen object representing the color and style of TriSurface border.")]
        public ChartPen Outline
        {
            get
            {
                if (this.border == null)
                {
                    this.border = new ChartPen(base.chart, Color.White, false);
                }
                return this.border;
            }
        }

        private sealed class Triangle
        {
            public System.Drawing.Color Color;
            public TriSurface.Triangle Next;
            public Point[] P = new Point[3];
            public TriSurface.Triangle Prev;
            public double Z;
        }
    }
}

