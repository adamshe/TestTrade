namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing.Design;
    using System.Reflection;

    public sealed class ValueList : TeeBase
    {
        public int Capacity;
        public int Count;
        internal bool dateTime;
        public static int DefaultCapacity;
        internal double maximum;
        internal double minimum;
        public string Name;
        private ValueListOrder order;
        internal Series series;
        internal bool statsOk;
        public double TempValue;
        internal double total;
        internal double totalABS;
        public double[] Value;
        internal string valueSource;

        public ValueList(Series s, string name) : this(s, name, DefaultCapacity)
        {
        }

        public ValueList(Series s, string name, int initialCapacity)
        {
            this.Value = new double[1];
            this.order = ValueListOrder.None;
            this.valueSource = "";
            this.series = s;
            this.series.valuesList.Add(this);
            this.Name = name;
            this.Capacity = initialCapacity;
        }

        internal int AddChartValue(double AValue)
        {
            int count;
            if (this.order == ValueListOrder.None)
            {
                count = this.Count;
                this.IncrementArray();
                this.Value[count] = AValue;
            }
            else
            {
                int index = this.Count - 1;
                if (((index == -1) || ((this.order == ValueListOrder.Ascending) && (AValue >= this[index]))) || ((this.order == ValueListOrder.Descending) && (AValue <= this[index])))
                {
                    count = this.Count;
                    this.IncrementArray();
                    this.Value[count] = AValue;
                }
                else
                {
                    if (this.order != ValueListOrder.Ascending)
                    {
                        while ((index >= 0) && (this[index] < AValue))
                        {
                            index--;
                        }
                    }
                    else
                    {
                        while ((index >= 0) && (this[index] > AValue))
                        {
                            index--;
                        }
                    }
                    count = index + 1;
                    this.IncrementArray();
                    for (index = this.Count - 1; index > count; index--)
                    {
                        this.Value[index] = this.Value[index - 1];
                    }
                    this.Value[count] = AValue;
                }
            }
            this.statsOk = false;
            return count;
        }

        public System.DateTime AsDateTime(int index)
        {
            return Utils.DateTime(this[index]);
        }

        internal void Assign(ValueList value)
        {
            this.order = value.order;
            this.dateTime = value.dateTime;
            this.Name = value.Name;
            this.valueSource = value.valueSource;
        }

        private void CalcStats()
        {
            int count = this.Count;
            if (count > 0)
            {
                this.minimum = this.Value[0];
                this.maximum = this.minimum;
                this.total = this.minimum;
                this.totalABS = Math.Abs(this.total);
                for (int i = 1; i < count; i++)
                {
                    double num2 = this.Value[i];
                    if (num2 < this.minimum)
                    {
                        this.minimum = num2;
                    }
                    if (num2 > this.maximum)
                    {
                        this.maximum = num2;
                    }
                    this.total += num2;
                    this.totalABS += Math.Abs(num2);
                }
            }
            else
            {
                this.minimum = 0.0;
                this.maximum = 0.0;
                this.total = 0.0;
                this.totalABS = 0.0;
            }
            this.statsOk = true;
        }

        public void Clear()
        {
            this.Count = 0;
            this.Value = new double[1];
        }

        private int CompareValueIndex(int a, int b)
        {
            int num = (this.Value[a] < this.Value[b]) ? -1 : ((this.Value[a] > this.Value[b]) ? 1 : 0);
            if (this.order == ValueListOrder.Descending)
            {
                num = -num;
            }
            return num;
        }

        internal void Deserialize(string field, object e)
        {
            if (field == "Value")
            {
                this.Value = (double[]) e;
            }
            else if (field == "Count")
            {
                this.Count = Convert.ToInt32(e);
            }
        }

        internal void Exchange(int Index1, int Index2)
        {
            double num = this.Value[Index1];
            this.Value[Index1] = this.Value[Index2];
            this.Value[Index2] = num;
        }

        public void FillSequence()
        {
            int count = this.Count;
            for (int i = 0; i < count; i++)
            {
                this.Value[i] = i;
            }
            this.statsOk = false;
        }

        private void IncrementArray()
        {
            this.Count++;
            int length = this.Value.Length;
            if (this.Count > length)
            {
                if (this.Capacity > 0)
                {
                    length += this.Capacity;
                }
                else if (this.Count > 3)
                {
                    length += this.Count / 4;
                }
                else
                {
                    length += 100;
                }
                double[] array = new double[length];
                this.Value.CopyTo(array, 0);
                this.Value = array;
            }
        }

        public int IndexOf(double value)
        {
            return Array.IndexOf(this.Value, value, 0, this.Count);
        }

        internal void InsertChartValue(int valueIndex, double value)
        {
            this.IncrementArray();
            for (int i = this.Count - 1; i > valueIndex; i--)
            {
                this.Value[i] = this.Value[i - 1];
            }
            this.Value[valueIndex] = value;
            this.statsOk = false;
        }

        [Obsolete("Use IndexOf method."), EditorBrowsable(EditorBrowsableState.Never)]
        public int Locate(double value)
        {
            return Array.IndexOf(this.Value, value, 0, this.Count);
        }

        public void RemoveAt(int index)
        {
            this.RemoveRange(index, 1);
        }

        public void RemoveRange(int index, int count)
        {
            this.Count -= count;
            for (int i = index; i < this.Count; i++)
            {
                this.Value[i] = this.Value[i + count];
            }
            this.statsOk = false;
        }

        public void Sort()
        {
            if (this.order != ValueListOrder.None)
            {
                Utils.Sort(0, this.Count - 1, new Utils.CompareEventHandler(this.CompareValueIndex), new Utils.SwapEventHandler(this.series.SwapValueIndex));
            }
        }

        internal void Trim()
        {
            double[] destinationArray = new double[this.Count];
            Array.Copy(this.Value, 0, destinationArray, 0, this.Count);
            this.Value = destinationArray;
        }

        [Description("Field to use as source for this value list."), DefaultValue(""), Editor(typeof(Series.LabelMemberEditor), typeof(UITypeEditor))]
        public string DataMember
        {
            get
            {
                return this.valueSource;
            }
            set
            {
                if (this.valueSource != value)
                {
                    this.valueSource = value;
                    if (this.series != null)
                    {
                        this.series.CheckDataSource();
                    }
                }
            }
        }

        [DefaultValue(false), Description("Allows values to be expressed either as numbers or as Date+Time values.")]
        public bool DateTime
        {
            get
            {
                return this.dateTime;
            }
            set
            {
                if (this.dateTime != value)
                {
                    this.dateTime = value;
                    this.series.Invalidate();
                }
            }
        }

        [Browsable(false), Description("Returns the First point value.")]
        public double First
        {
            get
            {
                return this.Value[0];
            }
        }

        public double this[int index]
        {
            get
            {
                return this.Value[index];
            }
            set
            {
                this.Value[index] = value;
                this.statsOk = false;
            }
        }

        [Description("Returns the Last point value."), Browsable(false)]
        public double Last
        {
            get
            {
                return this.Value[this.Count - 1];
            }
        }

        [Browsable(false), Description("Returns the highest of all values in the list.")]
        public double Maximum
        {
            get
            {
                if (!this.statsOk)
                {
                    this.CalcStats();
                }
                return this.maximum;
            }
        }

        [Browsable(false), Obsolete("Please use Maximum property."), EditorBrowsable(EditorBrowsableState.Never)]
        public double MaxValue
        {
            get
            {
                return this.Maximum;
            }
        }

        [Browsable(false), Description("Returns the lowest of all values in the list.")]
        public double Minimum
        {
            get
            {
                if (!this.statsOk)
                {
                    this.CalcStats();
                }
                return this.minimum;
            }
        }

        [Obsolete("Please use Minimum property."), Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public double MinValue
        {
            get
            {
                return this.Minimum;
            }
        }

        [Description("Determines if points are automatically sorted"), DefaultValue(0)]
        public ValueListOrder Order
        {
            get
            {
                return this.order;
            }
            set
            {
                if (this.order != value)
                {
                    this.order = value;
                    if (this.series != null)
                    {
                        this.series.CheckDataSource();
                    }
                }
            }
        }

        [Browsable(false), Description("")]
        public double Range
        {
            get
            {
                if (!this.statsOk)
                {
                    this.CalcStats();
                }
                return (this.maximum - this.minimum);
            }
        }

        [Browsable(false), Description("Maintains the sum of all IValueList values.")]
        public double Total
        {
            get
            {
                if (!this.statsOk)
                {
                    this.CalcStats();
                }
                return this.total;
            }
        }

        [Description("Returns the sum of all absolute values in the list."), Browsable(false)]
        public double TotalABS
        {
            get
            {
                if (!this.statsOk)
                {
                    this.CalcStats();
                }
                return this.totalABS;
            }
        }
    }
}

