namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;

    [ToolboxBitmap(typeof(HighLow), "SeriesIcons.HighLow.bmp")]
    public class HighLow : Series
    {
        private ChartBrush highBrush;
        private ChartPen highPen;
        private ValueList low;
        private ChartBrush lowBrush;
        private ChartPen lowPen;
        private int OldX;
        private int OldY0;
        private int OldY1;
        private ChartPen pen;

        public HighLow() : this(null)
        {
        }

        public HighLow(Chart c) : base(c)
        {
            base.calcVisiblePoints = false;
            this.Pen.Color = Color.Empty;
            this.low = new ValueList(this, Texts.ValuesLow);
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
                    this.Add(base.vxValues.TempValue, this.HighValues.TempValue, this.LowValues.TempValue, text, empty);
                }
            }
        }

        public int Add(double x, double h, double l)
        {
            return this.Add(x, h, l, "", Color.Empty);
        }

        public int Add(double x, double h, double l, Color color)
        {
            return this.Add(x, h, l, "", color);
        }

        public int Add(double x, double h, double l, string text)
        {
            return this.Add(x, h, l, text, Color.Empty);
        }

        public int Add(double x, double h, double l, string text, Color color)
        {
            this.low.TempValue = l;
            return base.Add(x, h, text, color);
        }

        protected override void AddSampleValues(int numValues)
        {
            Series.SeriesRandom random = base.RandomBounds(numValues);
            double h = random.DifY * random.Random();
            for (int i = 1; i <= numValues; i++)
            {
                h += (random.Random() * Utils.Round((double) (random.DifY / 5.0))) - (random.DifY / 10.0);
                this.Add(random.tmpX, h, h - (random.Random() * Utils.Round((double) (random.DifY / 5.0))));
                random.tmpX += random.StepX;
            }
        }

        protected internal override void CreateSubGallery(Series.SubGalleryEventHandler AddSubChart)
        {
            base.CreateSubGallery(AddSubChart);
            AddSubChart(Texts.Filled);
            AddSubChart(Texts.NoLines);
            AddSubChart(Texts.NoHigh);
            AddSubChart(Texts.NoLow);
        }

        private void DrawLine(ChartPen APen, int BeginY, int EndY, int x)
        {
            Graphics3D graphicsd = base.chart.graphics3D;
            if (APen.Visible)
            {
                graphicsd.Pen = APen;
                if (base.chart.Aspect.View3D)
                {
                    graphicsd.MoveTo(this.OldX, BeginY, base.MiddleZ);
                    graphicsd.LineTo(x, EndY, base.MiddleZ);
                }
                else
                {
                    graphicsd.MoveTo(this.OldX, BeginY);
                    graphicsd.LineTo(x, EndY);
                }
            }
        }

        public override void DrawValue(int valueIndex)
        {
            Graphics3D graphicsd = base.chart.graphics3D;
            int x = this.CalcXPos(valueIndex);
            int y = this.CalcYPos(valueIndex);
            int num3 = base.CalcYPosValue(this.low[valueIndex]);
            Color color = this.ValueColor(valueIndex);
            if (valueIndex != base.firstVisible)
            {
                ChartBrush brush = (this.low[valueIndex] < this.HighValues[valueIndex]) ? this.highBrush : this.lowBrush;
                if ((brush != null) && brush.Visible)
                {
                    graphicsd.Pen.Visible = false;
                    int oldX = this.OldX;
                    if (this.Pen.Visible)
                    {
                        oldX += this.Pen.Width;
                    }
                    graphicsd.Brush = brush;
                    if (graphicsd.Brush.Color.IsEmpty)
                    {
                        graphicsd.Brush.Color = color;
                    }
                    graphicsd.Plane(new Point(oldX, this.OldY0), new Point(oldX, this.OldY1), new Point(x, num3), new Point(x, y), base.MiddleZ);
                }
                this.DrawLine(this.HighPen, this.OldY0, y, x);
                this.DrawLine(this.LowPen, this.OldY1, num3, x);
            }
            if (this.Pen.Visible)
            {
                Color color2 = this.pen.Color;
                if (color2.IsEmpty)
                {
                    color2 = color;
                }
                graphicsd.Pen = this.pen;
                graphicsd.Pen.Color = color2;
                if (base.chart.Aspect.View3D)
                {
                    graphicsd.VerticalLine(x, y, num3, base.MiddleZ);
                }
                else
                {
                    graphicsd.VerticalLine(x, y, num3);
                }
            }
            this.OldX = x;
            this.OldY0 = y;
            this.OldY1 = num3;
        }

        public override bool IsValidSourceOf(Series value)
        {
            return (value is HighLow);
        }

        public override double MaxYValue()
        {
            return Math.Max(base.MaxYValue(), this.low.Maximum);
        }

        public override double MinYValue()
        {
            return Math.Min(base.MinYValue(), this.low.Minimum);
        }

        protected override void SetChart(Chart c)
        {
            base.SetChart(c);
            if (this.pen != null)
            {
                this.pen.Chart = c;
            }
            if (this.highPen != null)
            {
                this.highPen.Chart = c;
            }
            if (this.lowPen != null)
            {
                this.lowPen.Chart = c;
            }
            if (this.highBrush != null)
            {
                this.highBrush.Chart = c;
            }
            if (this.lowBrush != null)
            {
                this.lowBrush.Chart = c;
            }
        }

        protected internal override void SetSubGallery(int index)
        {
            switch (index)
            {
                case 1:
                    this.HighBrush.Visible = true;
                    this.LowBrush.Visible = true;
                    return;

                case 2:
                    this.Pen.Visible = false;
                    return;

                case 3:
                    this.HighPen.Visible = false;
                    return;

                case 4:
                    this.LowPen.Visible = false;
                    return;
            }
            base.SetSubGallery(index);
        }

        public override string Description
        {
            get
            {
                return Texts.GalleryHighLow;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ChartBrush HighBrush
        {
            get
            {
                if (this.highBrush == null)
                {
                    this.highBrush = new ChartBrush(base.chart, Color.Empty, false);
                }
                return this.highBrush;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ChartPen HighPen
        {
            get
            {
                if (this.highPen == null)
                {
                    this.highPen = new ChartPen(base.chart, Color.Black);
                }
                return this.highPen;
            }
        }

        public ValueList HighValues
        {
            get
            {
                return base.vyValues;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ChartBrush LowBrush
        {
            get
            {
                if (this.lowBrush == null)
                {
                    this.lowBrush = new ChartBrush(base.chart, Color.Empty, false);
                }
                return this.lowBrush;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance")]
        public ChartPen LowPen
        {
            get
            {
                if (this.lowPen == null)
                {
                    this.lowPen = new ChartPen(base.chart, Color.Black);
                }
                return this.lowPen;
            }
        }

        public ValueList LowValues
        {
            get
            {
                return this.low;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance")]
        public ChartPen Pen
        {
            get
            {
                if (this.pen == null)
                {
                    this.pen = new ChartPen(base.chart);
                }
                return this.pen;
            }
        }
    }
}

