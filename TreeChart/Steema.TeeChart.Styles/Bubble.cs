namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;

    [ToolboxBitmap(typeof(Bubble), "SeriesIcons.Bubble.bmp")]
    public class Bubble : Points
    {
        private ValueList radiusValues;
        private bool squared;

        public Bubble() : this(null)
        {
        }

        public Bubble(Chart c) : base(c)
        {
            this.squared = true;
            this.radiusValues = new ValueList(this, Texts.ValuesBubbleRadius);
            base.point.InflateMargins = false;
            base.point.Style = PointerStyles.Circle;
            base.point.AllowChangeSize = false;
            base.Marks.Pen.Visible = false;
            base.Marks.Pen.defaultVisible = false;
            base.Marks.Transparent = true;
            base.bColorEach = true;
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
                    this.Add(base.vxValues.TempValue, base.YValues.TempValue, this.radiusValues.TempValue, text, empty);
                }
            }
        }

        public int Add(double x, double y, double radius)
        {
            return this.Add(x, y, radius, "", Color.Empty);
        }

        public int Add(double x, double y, double radius, Color color)
        {
            return this.Add(x, y, radius, "", color);
        }

        public int Add(double x, double y, double radius, string text)
        {
            return this.Add(x, y, radius, text, Color.Empty);
        }

        public int Add(double x, double y, double radius, string text, Color color)
        {
            this.radiusValues.TempValue = radius;
            return base.Add(x, y, text, color);
        }

        protected override void AddSampleValues(int numValues)
        {
            Series.SeriesRandom random = base.RandomBounds(numValues);
            for (int i = 1; i <= numValues; i++)
            {
                this.Add(random.tmpX, (double) Utils.Round((double) (random.DifY * random.Random())), (random.DifY / 15.0) + Utils.Round((double) (random.DifY / (10.0 + (15.0 * random.Random())))));
                random.tmpX += random.StepX;
            }
        }

        private double ApplyRadius(double value, ValueList aList, bool Increment)
        {
            double num = value;
            for (int i = 0; i < (base.Count - 1); i++)
            {
                if (Increment)
                {
                    num = Math.Max(num, aList[i] + this.radiusValues[i]);
                }
                else
                {
                    num = Math.Min(num, aList[i] - this.radiusValues[i]);
                }
            }
            return num;
        }

        protected override void DrawLegendShape(Graphics3D g, int valueIndex, Rectangle r)
        {
            int num = Math.Min(r.Width, r.Height);
            base.point.HorizSize = num;
            base.point.VertSize = num;
            base.DrawLegendShape(g, valueIndex, r);
        }

        public override void DrawValue(int valueIndex)
        {
            int num = base.CalcYSizeValue(this.radiusValues[valueIndex]);
            base.point.HorizSize = this.squared ? num : base.CalcXSizeValue(this.radiusValues[valueIndex]);
            base.point.VertSize = num;
            base.DrawPointer(this.CalcXPos(valueIndex), this.CalcYPos(valueIndex), this.ValueColor(valueIndex), valueIndex);
        }

        public override bool IsValidSourceOf(Series value)
        {
            return (value is Bubble);
        }

        public override double MaxYValue()
        {
            return this.ApplyRadius(base.MaxYValue(), base.vyValues, true);
        }

        public override double MaxZValue()
        {
            if (base.point.Draw3D)
            {
                return this.radiusValues.Maximum;
            }
            return base.MaxZValue();
        }

        public override double MinYValue()
        {
            return this.ApplyRadius(base.MinYValue(), base.vyValues, false);
        }

        public override double MinZValue()
        {
            if (base.point.Draw3D)
            {
                return -this.radiusValues.Maximum;
            }
            return base.MinZValue();
        }

        protected internal override int NumSampleValues()
        {
            return 8;
        }

        [DefaultValue(true), Description("Controls which color will be drawn on the bubbles.")]
        public bool ColorEach
        {
            get
            {
                return base.bColorEach;
            }
            set
            {
                base.ColorEach = value;
            }
        }

        public override string Description
        {
            get
            {
                return Texts.GalleryBubble;
            }
        }

        [Description("A TList object that stores each Bubble point Radius value.")]
        public ValueList RadiusValues
        {
            get
            {
                return this.radiusValues;
            }
            set
            {
                base.SetValueList(this.radiusValues, value);
            }
        }

        [Description("Determines how the Bubble size is calculated."), DefaultValue(true)]
        public bool Squared
        {
            get
            {
                return this.squared;
            }
            set
            {
                base.SetBooleanProperty(ref this.squared, value);
            }
        }
    }
}

