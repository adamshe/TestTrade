namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Reflection;

    public abstract class Custom3D : Series
    {
        private int timesZOrder;
        protected ValueList vzValues;

        protected Custom3D() : this(null)
        {
        }

        protected Custom3D(Chart c) : base(c)
        {
            this.timesZOrder = 3;
            base.HasZValues = true;
            base.calcVisiblePoints = false;
            this.vzValues = new ValueList(this, "Z");
            base.vxValues.Order = ValueListOrder.None;
        }

        public override void Add(DataView view)
        {
            int index = -1;
            int num2 = -1;
            Color empty = Color.Empty;
            string text = "";
            int[] numArray = new int[base.ValuesLists.Count];
            int num3 = 0;
            foreach (ValueList list in base.ValuesLists)
            {
                if (list.DataMember.Length != 0)
                {
                    numArray[base.ValuesLists.IndexOf(list)] = view.Table.Columns.IndexOf(list.DataMember);
                    num3++;
                }
            }
            if (base.labelMember.Length != 0)
            {
                index = view.Table.Columns.IndexOf(base.labelMember);
            }
            if (base.colorMember.Length != 0)
            {
                num2 = view.Table.Columns.IndexOf(base.colorMember);
            }
            if (num3 == base.ValuesLists.Count)
            {
                foreach (DataRowView view2 in view)
                {
                    DataRow row = view2.Row;
                    if (num2 != -1)
                    {
                        empty = (Color) row[num2];
                    }
                    if (index != -1)
                    {
                        text = Convert.ToString(row[index]);
                    }
                    foreach (ValueList list2 in base.ValuesLists)
                    {
                        int num4 = numArray[base.ValuesLists.IndexOf(list2)];
                        if (row[num4] is DateTime)
                        {
                            list2.TempValue = Utils.DateTime((DateTime) row[num4]);
                        }
                        else
                        {
                            list2.TempValue = Convert.ToDouble(row[num4]);
                        }
                    }
                    this.Add(base.vxValues.TempValue, base.YValues.TempValue, this.vzValues.TempValue, text, empty);
                }
            }
        }

        [Description("Adds the X, Y and Z arrays.")]
        public void Add(Array xValues, Array yValues, Array zValues)
        {
            int length = yValues.GetLength(0);
            this.vzValues.Count = length;
            this.vzValues.Value = base.ConvertArray(zValues, length);
            this.vzValues.statsOk = false;
            base.Add(xValues, yValues);
        }

        public int Add(double x, double y, double z)
        {
            return this.Add(x, y, z, "", Color.Empty);
        }

        public int Add(double x, double y, double z, Color color)
        {
            return this.Add(x, y, z, "", color);
        }

        public int Add(double x, double y, double z, string text)
        {
            return this.Add(x, y, z, text, Color.Empty);
        }

        public int Add(DateTime aDate, double y, double z, string text, Color color)
        {
            this.vzValues.TempValue = z;
            return base.Add(Utils.DateTime(aDate), y, text, color);
        }

        public int Add(double x, double y, double z, string text, Color color)
        {
            this.vzValues.TempValue = z;
            return base.Add(x, y, text, color);
        }

        protected internal override void CalcZOrder()
        {
            base.CalcZOrder();
            base.chart.maxZOrder = this.timesZOrder;
        }

        protected int CalcZPos(int valueIndex)
        {
            return base.chart.axes.depth.CalcYPosValue(this.vzValues.Value[valueIndex]);
        }

        protected internal override void DrawMark(int valueIndex, string s, SeriesMarks.Position position)
        {
            base.Marks.ZPosition = this.CalcZPos(valueIndex);
            base.Marks.ApplyArrowLength(ref position);
            base.DrawMark(valueIndex, s, position);
        }

        public override bool IsValidSourceOf(Series value)
        {
            return (value is Custom3D);
        }

        public override double MaxZValue()
        {
            return this.vzValues.Maximum;
        }

        public override double MinZValue()
        {
            return this.vzValues.Minimum;
        }

        protected override void PrepareLegendCanvas(Graphics3D g, int valueIndex, ref Color backColor, ref ChartBrush aBrush)
        {
            base.PrepareLegendCanvas(g, valueIndex, ref backColor, ref aBrush);
            if (base.chart.Legend.Symbol.continuous)
            {
                g.Pen.Visible = false;
            }
        }

        [Description("XYZ Point characteristics")]
        public SeriesXYZPoint this[int index]
        {
            get
            {
                return new SeriesXYZPoint(this, index);
            }
        }

        [DefaultValue(3)]
        public int TimesZOrder
        {
            get
            {
                return this.timesZOrder;
            }
            set
            {
                base.SetIntegerProperty(ref this.timesZOrder, value);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Values defining Z grid point positions.")]
        public ValueList ZValues
        {
            get
            {
                return this.vzValues;
            }
        }
    }
}

