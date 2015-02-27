namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Reflection;
    using System.Runtime.InteropServices;

    public class OHLC : Custom
    {
        protected ValueList vHighValues;
        protected ValueList vLowValues;
        protected ValueList vOpenValues;

        public OHLC() : this(null)
        {
        }

        public OHLC(Chart c) : base(c)
        {
            base.XValues.DateTime = true;
            base.XValues.Name = Texts.ValuesDate;
            base.YValues.Name = Texts.ValuesClose;
            this.vHighValues = new ValueList(this, Texts.ValuesHigh);
            this.vLowValues = new ValueList(this, Texts.ValuesLow);
            this.vOpenValues = new ValueList(this, Texts.ValuesOpen);
        }

        public override void Add(DataView view)
        {
            int index = -1;
            int num2 = -1;
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
                        Color color2 = (Color) row[num2];
                    }
                    if (index != -1)
                    {
                        Convert.ToString(row[index]);
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
                    base.Add(this.DateValues.TempValue, this.CloseValues.TempValue);
                }
            }
        }

        public int Add(double open, double high, double low, double close)
        {
            return this.Add(base.Count, open, high, low, close);
        }

        public int Add(DateTime aDate, double open, double high, double low, double close)
        {
            return this.Add(Utils.DateTime(aDate), open, high, low, close);
        }

        public int Add(double index, double open, double high, double low, double close)
        {
            this.vHighValues.TempValue = high;
            this.vLowValues.TempValue = low;
            this.vOpenValues.TempValue = open;
            return base.Add(index, close);
        }

        public int Add(int index, double open, double high, double low, double close)
        {
            return this.Add(Convert.ToDouble(index), open, high, low, close);
        }

        protected override void AddSampleValues(int numValues)
        {
            Series.SeriesRandom r = base.RandomBounds(numValues);
            double aOpen = r.MinY + Utils.Round((double) (r.DifY * r.Random()));
            for (int i = 1; i <= numValues; i++)
            {
                double num2;
                double num3;
                double num4;
                GetRandomOHLC(r, aOpen, out num4, out num2, out num3, r.DifY);
                this.Add(r.tmpX, aOpen, num2, num3, num4);
                r.tmpX += r.StepX;
                aOpen = (num4 + (10.0 * r.Random())) - 5.0;
            }
        }

        private static void GetRandomOHLC(Series.SeriesRandom r, double aOpen, out double aClose, out double aHigh, out double aLow, double YRange)
        {
            int num = Math.Abs(Utils.Round((double) (YRange / 400.0)));
            aClose = (aOpen + (Utils.Round((double) (YRange / 25.0)) * r.Random())) - (YRange / 50.0);
            double num2 = 3 * Utils.Round((double) (Math.Abs((double) (aClose - aOpen)) / 10.0));
            if (aClose > aOpen)
            {
                aHigh = (aClose + num2) + (num * r.Random());
                aLow = (aOpen - num2) - (num * r.Random());
            }
            else
            {
                aHigh = (aOpen + num2) + (num * r.Random());
                aLow = (aClose - num2) - (num * r.Random());
            }
        }

        public override bool IsValidSourceOf(Series value)
        {
            return (value is OHLC);
        }

        public override double MaxYValue()
        {
            return Math.Max(Math.Max(Math.Max(this.CloseValues.Maximum, this.vHighValues.Maximum), this.vLowValues.Maximum), this.vOpenValues.Maximum);
        }

        public override double MinYValue()
        {
            return Math.Min(Math.Min(Math.Min(this.CloseValues.Minimum, this.vHighValues.Minimum), this.vLowValues.Minimum), this.vOpenValues.Minimum);
        }

        protected internal override int NumSampleValues()
        {
            return 40;
        }

        [Description("Gets and sets all Stock market Close values.")]
        public ValueList CloseValues
        {
            get
            {
                return base.YValues;
            }
            set
            {
                base.SetValueList(base.YValues, value);
            }
        }

        [Description("Gets and sets all Stock market Date values.")]
        public ValueList DateValues
        {
            get
            {
                return base.XValues;
            }
            set
            {
                base.SetValueList(base.XValues, value);
            }
        }

        [Description("Gets and sets all Stock market High values.")]
        public ValueList HighValues
        {
            get
            {
                return this.vHighValues;
            }
            set
            {
                base.SetValueList(this.vHighValues, value);
            }
        }

        [Description("Point characteristics")]
        public new SeriesOHLCPoint this[int index]
        {
            get
            {
                return new SeriesOHLCPoint(this, index);
            }
        }

        [Description("Gets and sets all Stock market Low values.")]
        public ValueList LowValues
        {
            get
            {
                return this.vLowValues;
            }
            set
            {
                base.SetValueList(this.vLowValues, value);
            }
        }

        [Description("Gets and sets all Stock market Open values.")]
        public ValueList OpenValues
        {
            get
            {
                return this.vOpenValues;
            }
            set
            {
                base.SetValueList(this.vOpenValues, value);
            }
        }
    }
}

