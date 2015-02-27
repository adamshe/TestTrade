namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class Custom3DGrid : Custom3DPalette
    {
        protected PCellsRow gridIndex;
        protected bool iInGallery;
        protected internal int iNextXCell;
        protected internal int iNextZCell;
        protected internal int iNumXValues;
        protected internal int iNumZValues;
        private bool irregularGrid;
        private const int MaxAllowedCells = 0x7d0;
        protected internal int valueIndex0;
        protected internal int valueIndex1;
        protected internal int valueIndex2;
        protected internal int valueIndex3;

        public event GetYValueEventHandler GetYValue;

        public Custom3DGrid() : this(null)
        {
        }

        public Custom3DGrid(Chart c) : base(c)
        {
            this.gridIndex = new PCellsRow();
            this.iNumXValues = 10;
            this.iNumZValues = 10;
            this.gridIndex.Capacity = 0x7d0;
            for (int i = 0; i < 0x7d0; i++)
            {
                this.gridIndex.Add(null);
            }
        }

        protected override void AddSampleValues(int numValues)
        {
            if (numValues > 0)
            {
                bool iInGallery = this.iInGallery;
                this.iInGallery = true;
                try
                {
                    int numX = Math.Min(0x7d0, numValues);
                    this.CreateValues(numX, numX);
                }
                finally
                {
                    this.iInGallery = iInGallery;
                }
            }
        }

        internal bool CanCreateValues()
        {
            if (!base.DesignMode && !this.iInGallery)
            {
                return (this.GetYValue != null);
            }
            return true;
        }

        public override void Clear()
        {
            base.Clear();
            base.vxValues.Order = ValueListOrder.None;
            this.ClearGridIndex();
        }

        private void ClearGridIndex()
        {
            for (int i = 0; i < 0x7d0; i++)
            {
                this.gridIndex[i] = null;
            }
        }

        public void CreateValues(int numX, int numZ)
        {
            if (this.CanCreateValues())
            {
                this.iNumXValues = numX;
                this.iNumZValues = numZ;
                base.BeginUpdate();
                this.Clear();
                for (int i = 1; i <= numZ; i++)
                {
                    for (int j = 1; j <= numX; j++)
                    {
                        base.Add((double) j, this.GetXZValue(j, i), (double) i);
                    }
                }
                base.EndUpdate();
                base.CreateDefaultPalette(base.iPaletteSteps);
            }
        }

        protected internal override void DoBeforeDrawChart()
        {
            base.DoBeforeDrawChart();
            if (base.Count > 0)
            {
                this.FillGridIndex();
            }
        }

        protected void DoGetYValue(int x, int z, ref double Value)
        {
            if (this.GetYValue != null)
            {
                GetYValueEventArgs e = new GetYValueEventArgs(x, z, Value);
                this.GetYValue(this, e);
                Value = e.Value;
            }
        }

        protected internal bool ExistFourGridIndex(int x, int z)
        {
            if ((this.gridIndex[x] != null) && (this.gridIndex[x + this.iNextXCell] != null))
            {
                this.valueIndex0 = this.gridIndex[x][z];
                if (this.valueIndex0 > -1)
                {
                    this.valueIndex1 = this.gridIndex[x + this.iNextXCell][z];
                    if (this.valueIndex1 > -1)
                    {
                        this.valueIndex2 = this.gridIndex[x + this.iNextXCell][z + this.iNextZCell];
                        if (this.valueIndex2 > -1)
                        {
                            this.valueIndex3 = this.gridIndex[x][z + this.iNextZCell];
                            return (this.valueIndex3 > -1);
                        }
                    }
                }
            }
            return false;
        }

        private void FillGridIndex()
        {
            int num3;
            int num4;
            double minimum = base.vxValues.Minimum;
            double minZ = base.vzValues.Minimum;
            if (this.irregularGrid)
            {
                this.FillIrregularGrid(out num3, out num4, minimum, minZ);
            }
            else
            {
                this.FillRegularGrid(out num3, out num4, minimum, minZ);
            }
            if (num3 != this.iNumXValues)
            {
                this.iNumXValues = num3;
            }
            if (num4 != this.iNumZValues)
            {
                this.iNumZValues = num4;
            }
        }

        private void FillIrregularGrid(out int xCount, out int zCount, double minX, double minZ)
        {
            double[] values = new double[0x7d0];
            double[] numArray2 = new double[0x7d0];
            xCount = 1;
            values[0] = minX;
            zCount = 1;
            numArray2[0] = minZ;
            for (int i = 0; i < base.Count; i++)
            {
                this.SearchValue(ref xCount, values, base.vxValues.Value[i]);
                this.SearchValue(ref zCount, numArray2, base.vzValues.Value[i]);
            }
            this.SortValues(xCount, values);
            this.SortValues(zCount, numArray2);
            for (int j = 0; j < base.Count; j++)
            {
                this.InternalSetGridIndex(this.ValuePosition(xCount, values, base.vxValues.Value[j]), this.ValuePosition(zCount, numArray2, base.vzValues[j]), j);
            }
        }

        private void FillRegularGrid(out int xCount, out int zCount, double minX, double minZ)
        {
            xCount = (int) (1.0 + Math.Round((double) (base.vxValues.Maximum - minX)));
            zCount = (int) (1.0 + Math.Round((double) (base.vzValues.Maximum - minZ)));
            for (int i = 0; i < base.Count; i++)
            {
                this.InternalSetGridIndex((int) (1.0 + Math.Round((double) (base.vxValues.Value[i] - minX))), (int) (1.0 + Math.Round((double) (base.vzValues.Value[i] - minZ))), i);
            }
        }

        private double GetXZValue(int x, int z)
        {
            if (this.GetYValue != null)
            {
                double num = 0.0;
                GetYValueEventArgs e = new GetYValueEventArgs(x, z, num);
                this.GetYValue(this, e);
                return e.Value;
            }
            if (!base.DesignMode && !this.iInGallery)
            {
                return 0.0;
            }
            return (((0.5 * Math.Pow(Math.Cos(((double) x) / (this.iNumXValues * 0.2)), 2.0)) + Math.Pow(Math.Cos(((double) z) / (this.iNumZValues * 0.2)), 2.0)) - Math.Cos(((double) z) / (this.iNumZValues * 0.5)));
        }

        private void InternalSetGridIndex(int x, int z, int value)
        {
            if (this.gridIndex[x] == null)
            {
                this.gridIndex[x] = new CellsRow();
                for (int i = 0; i < 0x7d0; i++)
                {
                    this.gridIndex[x].Add(-1);
                }
            }
            this.gridIndex[x][z] = value;
        }

        protected override bool IsValidSeriesSource(Series value)
        {
            return (value is Custom3DGrid);
        }

        protected internal override int NumSampleValues()
        {
            return this.iNumXValues;
        }

        private void ReCreateValues()
        {
            this.CreateValues(this.iNumXValues, this.iNumZValues);
        }

        private void SearchValue(ref int aCount, double[] values, double aValue)
        {
            int index = 0;
            while (index < aCount)
            {
                if (values[index] == aValue)
                {
                    return;
                }
                index++;
                if (index == aCount)
                {
                    values[index] = aValue;
                    aCount++;
                }
            }
        }

        private void SetNumZValues(int value)
        {
            if (value != this.iNumZValues)
            {
                this.iNumZValues = value;
                this.ReCreateValues();
            }
        }

        private void SortValues(int aCount, double[] values)
        {
            for (int i = 1; i < (aCount - 1); i++)
            {
                double num2 = values[i];
                int index = i;
                for (int j = i + 1; j < aCount; j++)
                {
                    if (values[j] < num2)
                    {
                        num2 = values[j];
                        index = j;
                    }
                    if (index != i)
                    {
                        values[index] = values[i];
                        values[i] = num2;
                    }
                }
            }
        }

        private int ValuePosition(int aCount, double[] values, double aValue)
        {
            int index = 0;
            while ((aValue != values[index]) && (index < aCount))
            {
                index++;
            }
            index++;
            return index;
        }

        [Description("Determine if X and Z values are equi-distant or not."), DefaultValue(false)]
        public bool IrregularGrid
        {
            get
            {
                return this.irregularGrid;
            }
            set
            {
                base.SetBooleanProperty(ref this.irregularGrid, value);
            }
        }

        public int this[int x, int z]
        {
            get
            {
                if (this.gridIndex[x] == null)
                {
                    return -1;
                }
                return this.gridIndex[x][z];
            }
            set
            {
                if (((x >= 0) && (x < 0x7d0)) && ((z >= 0) && (z < 0x7d0)))
                {
                    this.InternalSetGridIndex(x, z, value);
                }
            }
        }

        [Description("Determines the Surface's horizontal size in number of points."), DefaultValue(10)]
        public int NumXValues
        {
            get
            {
                return this.iNumXValues;
            }
            set
            {
                if (this.iNumXValues != value)
                {
                    this.iNumXValues = value;
                    this.ReCreateValues();
                }
            }
        }

        [Description("Determines the Surface's depth size in number of points."), DefaultValue(10)]
        public int NumZValues
        {
            get
            {
                return this.iNumZValues;
            }
            set
            {
                if (this.iNumZValues != value)
                {
                    this.iNumZValues = value;
                    this.ReCreateValues();
                }
            }
        }

        protected sealed class CellsRow : ArrayList
        {
            public CellsRow() : base(0x7d0)
            {
            }

            public int this[int index]
            {
                get
                {
                    return (int) base[index];
                }
                set
                {
                    base[index] = value;
                }
            }
        }

        public class GetYValueEventArgs : EventArgs
        {
            private double lValue;
            private readonly int x;
            private readonly int z;

            public GetYValueEventArgs(int X, int Z, double Value)
            {
                this.x = X;
                this.z = Z;
                this.lValue = Value;
            }

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

            public int X
            {
                get
                {
                    return this.x;
                }
            }

            public int Z
            {
                get
                {
                    return this.z;
                }
            }
        }

        public delegate void GetYValueEventHandler(Series sender, Custom3DGrid.GetYValueEventArgs e);

        protected sealed class PCellsRow : ArrayList
        {
            public Custom3DGrid.CellsRow this[int index]
            {
                get
                {
                    return (Custom3DGrid.CellsRow) base[index];
                }
                set
                {
                    base[index] = value;
                }
            }
        }
    }
}

