namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    [ToolboxBitmap(typeof(Arrow), "SeriesIcons.Arrow.bmp")]
    public class Arrow : Points
    {
        private ValueList endXValues;
        private ValueList endYValues;

        public Arrow() : this(null)
        {
        }

        public Arrow(Chart c) : base(c)
        {
            base.calcVisiblePoints = false;
            this.endXValues = new ValueList(this, Texts.ValuesArrowEndX);
            this.endYValues = new ValueList(this, Texts.ValuesArrowEndY);
            base.point.InflateMargins = false;
            base.Marks.Pen.Visible = false;
            base.Marks.Transparent = true;
        }

        public int Add(double x0, double y0, double x1, double y1)
        {
            return this.Add(x0, y0, x1, y1, "", Color.Empty);
        }

        public int Add(double x0, double y0, double x1, double y1, Color color)
        {
            return this.Add(x0, y0, x1, y1, "", color);
        }

        public int Add(double x0, double y0, double x1, double y1, string text)
        {
            return this.Add(x0, y0, x1, y1, text, Color.Empty);
        }

        public int Add(double x0, double y0, double x1, double y1, string text, Color color)
        {
            this.endXValues.TempValue = x1;
            this.endYValues.TempValue = y1;
            return base.Add(x0, y0, text, color);
        }

        protected override void AddSampleValues(int numValues)
        {
            Series.SeriesRandom random = base.RandomBounds(numValues);
            int num3 = Utils.Round(random.DifY);
            int num4 = Utils.Round((double) (random.StepX * numValues));
            for (int i = 1; i <= numValues; i++)
            {
                double num = random.tmpX + (num4 * random.Random());
                double num2 = random.MinY + (num3 * random.Random());
                this.Add(num, num2, num + (num4 * random.Random()), num2 + (num3 * random.Random()));
            }
        }

        [Description("Called internally by TeeChart to draw the ValueIndex point of the Series.")]
        public override void DrawValue(int valueIndex)
        {
            Point fromPoint = new Point(this.CalcXPos(valueIndex), this.CalcYPos(valueIndex));
            Point toPoint = new Point(base.CalcXPosValue(this.endXValues[valueIndex]), base.CalcYPosValue(this.endYValues[valueIndex]));
            Color colorValue = this.ValueColor(valueIndex);
            Graphics3D g = base.chart.graphics3D;
            if (base.chart.Aspect.View3D)
            {
                base.Pointer.PrepareCanvas(g, colorValue);
            }
            else
            {
                g.Pen = base.Pointer.Pen;
                g.Pen.Color = colorValue;
            }
            g.Arrow(base.chart.Aspect.View3D, fromPoint, toPoint, base.Pointer.HorizSize, base.Pointer.VertSize, base.MiddleZ);
        }

        [Description("Returns the Maximum Value of the Series X Values List.")]
        public override double MaxXValue()
        {
            return Math.Max(base.MaxXValue(), this.endXValues.Maximum);
        }

        [Description("Returns the Maximum Value of the Series Y Values List.")]
        public override double MaxYValue()
        {
            return Math.Max(base.MaxYValue(), this.endYValues.Maximum);
        }

        [Description("Returns the Minimum Value of the Series X Values List.")]
        public override double MinXValue()
        {
            return Math.Min(base.MinXValue(), this.endXValues.Minimum);
        }

        [Description("Returns the Minimum Value of the Series Y Values List.")]
        public override double MinYValue()
        {
            return Math.Min(base.MinYValue(), this.endYValues.Minimum);
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Description("Determines the vertical arrow head size in pixels.")]
        public int ArrowHeight
        {
            get
            {
                return base.point.VertSize;
            }
            set
            {
                base.point.VertSize = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Description("Determines the horizontal arrow head size in pixels.")]
        public int ArrowWidth
        {
            get
            {
                return base.point.HorizSize;
            }
            set
            {
                base.point.HorizSize = value;
            }
        }

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.GalleryArrow;
            }
        }

        [Description("Sets X1 values for Arrows.")]
        public ValueList EndXValues
        {
            get
            {
                return this.endXValues;
            }
            set
            {
                base.SetValueList(this.endXValues, value);
            }
        }

        [Description("Sets Y1 values for Arrows.")]
        public ValueList EndYValues
        {
            get
            {
                return this.endYValues;
            }
            set
            {
                base.SetValueList(this.endYValues, value);
            }
        }

        [Description("Sets X0 values for Arrows.")]
        public ValueList StartXValues
        {
            get
            {
                return base.vxValues;
            }
            set
            {
                base.SetValueList(base.vxValues, value);
            }
        }

        [Description("Sets Y0 values for Arrows.")]
        public ValueList StartYValues
        {
            get
            {
                return base.vyValues;
            }
            set
            {
                base.SetValueList(base.vyValues, value);
            }
        }
    }
}

