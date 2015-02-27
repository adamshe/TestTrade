namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    [ToolboxBitmap(typeof(Pyramid), "SeriesIcons.Pyramid.bmp")]
    public class Pyramid : Series
    {
        private ChartPen pen;
        private int size;

        public Pyramid() : this(null)
        {
        }

        public Pyramid(Chart c) : base(c)
        {
            this.size = 50;
            base.calcVisiblePoints = false;
            base.ColorEach = true;
        }

        private double AcumUpTo(int UpToIndex)
        {
            double num = 0.0;
            for (int i = 0; i <= UpToIndex; i++)
            {
                num += base.mandatory[i];
            }
            return num;
        }

        protected internal override void CalcHorizMargins(ref int LeftMargin, ref int RightMargin)
        {
            LeftMargin = 20;
            RightMargin = 20;
        }

        protected internal override void DrawMark(int valueIndex, string s, SeriesMarks.Position position)
        {
            position.LeftTop.Y = base.GetVertAxis.CalcPosValue(this.AcumUpTo(valueIndex));
            base.DrawMark(valueIndex, s, position);
        }

        public override void DrawValue(int valueIndex)
        {
            if (!base.IsNull(valueIndex))
            {
                base.chart.SetBrushCanvas(this.ValueColor(valueIndex), this.Brush, this.Brush.Color);
                base.chart.graphics3D.Pen = this.Pen;
                double num = this.AcumUpTo(valueIndex - 1);
                double num2 = 100.0 - ((num * 100.0) / base.mandatory.Total);
                int num3 = Utils.Round((double) ((this.SizePercent * base.GetHorizAxis.IAxisSize) * 0.005));
                int num4 = Utils.Round((double) ((num2 * num3) * 0.01));
                Rectangle r = new Rectangle();
                r.X = base.GetHorizAxis.CalcPosValue(this.MinXValue()) - num4;
                r.Width = 2 * num4;
                double num5 = 100.0 - num2;
                int num6 = (num5 > 0.0) ? Utils.Round((double) ((num5 * (base.EndZ - base.StartZ)) * 0.005)) : 0;
                r.Height = base.GetVertAxis.CalcPosValue(num);
                num += base.mandatory[valueIndex];
                r.Y = base.GetVertAxis.CalcPosValue(num);
                r.Height -= r.Y;
                num2 = 100.0 - ((num * 100.0) / base.mandatory.Total);
                int truncZ = (num2 < 100.0) ? Utils.Round((double) ((num2 * (base.EndZ - base.StartZ)) * 0.005)) : 0;
                base.chart.graphics3D.PyramidTrunc(r, base.StartZ + num6, base.EndZ - num6, Utils.Round((double) ((num2 * num3) * 0.01)), truncZ);
            }
        }

        public override bool DrawValuesForward()
        {
            return !base.GetVertAxis.Inverted;
        }

        public override double MaxXValue()
        {
            return this.MinXValue();
        }

        public override double MaxYValue()
        {
            return base.mandatory.TotalABS;
        }

        public override double MinXValue()
        {
            return (double) base.chart.Series.IndexOf(this);
        }

        public override double MinYValue()
        {
            return 0.0;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Sets Brush characteristics for the Pyramid Series."), Category("Appearance")]
        public ChartBrush Brush
        {
            get
            {
                return base.bBrush;
            }
        }

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.GalleryPyramid;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance"), Description("Element Pen characteristics.")]
        public ChartPen Pen
        {
            get
            {
                if (this.pen == null)
                {
                    this.pen = new ChartPen(base.chart, Color.Black);
                }
                return this.pen;
            }
        }

        [DefaultValue(50)]
        public int SizePercent
        {
            get
            {
                return this.size;
            }
            set
            {
                base.SetIntegerProperty(ref this.size, value);
            }
        }
    }
}

