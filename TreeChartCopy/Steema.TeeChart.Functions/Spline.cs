namespace Steema.TeeChart.Functions
{
    using System;
    using System.Drawing;
    using System.Reflection;

    public class Spline
    {
        private bool build;
        private int capacity;
        private int fragments;
        private bool interpolate;
        private bool[] knuckleList;
        private double[][] Matrix;
        private int noPoints;
        private int noVertices;
        private PointF[] pointList;
        private PointF[] vertexList;

        public Spline()
        {
            this.Clear();
        }

        public void AddPoint(double X, double Y)
        {
            if (this.noPoints == this.capacity)
            {
                this.SetCapacity(this.capacity + 0x19);
            }
            this.noPoints++;
            PointF empty = PointF.Empty;
            empty.X = (float) X;
            empty.Y = (float) Y;
            this[this.noPoints - 1] = empty;
            this.Build = false;
        }

        public void Clear()
        {
            if (this.NumberOfVertices > 0)
            {
                this.ClearVertexList();
            }
            this.noPoints = 0;
            this.noVertices = 0;
            this.Build = false;
            this.SetCapacity(0);
            this.interpolate = false;
            this.fragments = 100;
        }

        private void ClearVertexList()
        {
            this.vertexList = null;
        }

        private void DoInterpolate()
        {
            if ((this.noVertices < 250) && (this.noVertices > 2))
            {
                PointF[] tfArray = new PointF[this.noVertices + 2];
                for (int i = 1; i <= this.noVertices; i++)
                {
                    for (int k = i + 1; k <= this.noVertices; k++)
                    {
                        double num = this.Matrix[k][i] / this.Matrix[i][i];
                        for (int m = 1; m <= this.noVertices; m++)
                        {
                            this.Matrix[k][m] -= num * this.Matrix[i][m];
                        }
                        this.vertexList[k].X -= (float) (num * this.vertexList[k - 1].X);
                        this.vertexList[k].Y -= (float) (num * this.vertexList[k - 1].Y);
                    }
                }
                tfArray[this.noVertices].X = (float) (((double) this.vertexList[this.noVertices].X) / this.Matrix[this.noVertices][this.noVertices]);
                tfArray[this.noVertices].Y = (float) (((double) this.vertexList[this.noVertices].Y) / this.Matrix[this.noVertices][this.noVertices]);
                for (int j = this.noVertices - 1; j >= 1; j--)
                {
                    tfArray[j].X = (float) ((1.0 / this.Matrix[j][j]) * (this.vertexList[j].X - (this.Matrix[j][j + 1] * tfArray[j + 1].X)));
                    tfArray[j].Y = (float) ((1.0 / this.Matrix[j][j]) * (this.vertexList[j].Y - (this.Matrix[j][j + 1] * tfArray[j + 1].Y)));
                }
                this.ClearVertexList();
                this.vertexList = tfArray;
            }
        }

        private void FillMatrix()
        {
            if ((this.noVertices > 2) && (this.noVertices <= 250))
            {
                int num;
                for (num = 2; num < this.noVertices; num++)
                {
                    this.Matrix[num][num - 1] = 0.16666666666666666;
                    this.Matrix[num][num] = 0.66666666666666663;
                    this.Matrix[num][num + 1] = 0.16666666666666666;
                }
                this.Matrix[1][1] = 1.0;
                this.Matrix[this.noVertices][this.noVertices] = 1.0;
                num = 3;
                while (num < (this.noVertices - 1))
                {
                    if (((Math.Abs((float) (this.vertexList[num].X - this.vertexList[num - 1].X)) < 1E-05) && (Math.Abs((float) (this.vertexList[num + 1].X - this.vertexList[num].X)) < 1E-05)) && ((Math.Abs((float) (this.vertexList[num].Y - this.vertexList[num - 1].Y)) < 1E-05) && (Math.Abs((float) (this.vertexList[num + 1].Y - this.vertexList[num].Y)) < 1E-05)))
                    {
                        for (int i = num - 1; i <= (num + 1); i++)
                        {
                            this.Matrix[i][i - 1] = 0.0;
                            this.Matrix[i][i] = 1.0;
                            this.Matrix[i][i + 1] = 0.0;
                        }
                        num += 2;
                    }
                    else
                    {
                        num++;
                    }
                }
            }
        }

        private bool GetKnuckle(int index)
        {
            return (((index != 0) && (index != (this.noPoints - 1))) && this.knuckleList[index]);
        }

        private void PhantomPoints()
        {
            if (this.NumberOfVertices > 1)
            {
                int index = 0;
                this.vertexList[index].X = (2f * this.vertexList[index + 1].X) - this.vertexList[index + 2].X;
                this.vertexList[index].Y = (2f * this.vertexList[index + 1].Y) - this.vertexList[index + 2].Y;
                this.vertexList[this.NumberOfVertices + 1].X = (2f * this.vertexList[this.NumberOfVertices].X) - this.vertexList[this.NumberOfVertices - 1].X;
                this.vertexList[this.NumberOfVertices + 1].Y = (2f * this.vertexList[this.NumberOfVertices].Y) - this.vertexList[this.NumberOfVertices - 1].Y;
            }
        }

        public void Rebuild()
        {
            if (this.noPoints > 1)
            {
                this.ClearVertexList();
                this.noVertices = 0;
                for (int i = 0; i < this.noPoints; i++)
                {
                    if (this.GetKnuckle(i))
                    {
                        this.noVertices += 3;
                    }
                    else
                    {
                        this.noVertices++;
                    }
                }
                this.vertexList = new PointF[this.noVertices + 2];
                int num2 = 0;
                for (int j = 0; j < this.noPoints; j++)
                {
                    PointF tf = this[j];
                    if (this.GetKnuckle(j))
                    {
                        this.vertexList[num2 + 1] = tf;
                        this.vertexList[num2 + 2] = tf;
                        num2 += 2;
                    }
                    this.vertexList[num2 + 1] = this.pointList[j];
                    num2++;
                }
                if (this.interpolate)
                {
                    this.Matrix = new double[this.noVertices + 1][];
                    for (int k = 1; k <= this.noVertices; k++)
                    {
                        this.Matrix[k] = new double[this.noVertices + 1];
                    }
                    this.FillMatrix();
                    this.DoInterpolate();
                    this.Matrix = null;
                }
            }
            this.build = true;
            this.PhantomPoints();
        }

        private void SetCapacity(int value)
        {
            if (value != this.capacity)
            {
                int capacity = this.capacity;
                int num2 = value;
                PointF[] pointList = this.pointList;
                bool[] knuckleList = this.knuckleList;
                this.pointList = null;
                this.knuckleList = null;
                if (value > 0)
                {
                    this.pointList = new PointF[num2];
                    this.knuckleList = new bool[value];
                    if (this.capacity != 0)
                    {
                        pointList.CopyTo(this.pointList, 0);
                        knuckleList.CopyTo(this.knuckleList, 0);
                    }
                }
                if (capacity != 0)
                {
                    pointList = null;
                    knuckleList = null;
                }
                this.capacity = value;
            }
        }

        public void SetKnuckle(int index, bool value)
        {
            this.knuckleList[index] = value;
            this.Build = false;
        }

        public PointF Value(double Parameter)
        {
            PointF empty = PointF.Empty;
            if (this.noPoints >= 2)
            {
                if (!this.build)
                {
                    this.Rebuild();
                }
                double num = ((this.NumberOfVertices - 1) * Parameter) + 1.0;
                int num2 = Math.Max(0, ((int) num) - 1);
                int num3 = num2 + 3;
                if (num2 > (this.noVertices + 1))
                {
                    num2 = this.noVertices + 1;
                }
                for (int i = num2; i <= num3; i++)
                {
                    double num5 = Math.Abs((double) (i - num));
                    if (num5 < 2.0)
                    {
                        double num6 = (num5 < 1.0) ? ((0.66666666666666663 - (num5 * num5)) + (((0.5 * num5) * num5) * num5)) : ((((2.0 - num5) * (2.0 - num5)) * (2.0 - num5)) / 6.0);
                        empty.X += (float) (this.vertexList[i].X * num6);
                        empty.Y += (float) (this.vertexList[i].Y * num6);
                    }
                }
            }
            return empty;
        }

        public bool Build
        {
            get
            {
                return this.build;
            }
            set
            {
                if (!value)
                {
                    if (this.build)
                    {
                        this.ClearVertexList();
                    }
                    this.noVertices = 0;
                }
                this.build = value;
            }
        }

        public int Fragments
        {
            get
            {
                return this.fragments;
            }
            set
            {
                if (this.fragments != value)
                {
                    this.fragments = Math.Min(value, 600);
                }
            }
        }

        public bool Interpolated
        {
            get
            {
                return this.interpolate;
            }
            set
            {
                if (value != this.interpolate)
                {
                    this.interpolate = value;
                    this.Build = false;
                }
            }
        }

        public PointF this[int index]
        {
            get
            {
                return this.pointList[index];
            }
            set
            {
                this.pointList[index] = value;
                this.Build = false;
            }
        }

        public int NumberOfVertices
        {
            get
            {
                if (!this.build)
                {
                    this.Rebuild();
                }
                return this.noVertices;
            }
        }
    }
}

