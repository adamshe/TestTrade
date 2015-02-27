namespace Steema.TeeChart
{
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;

    [Serializable]
    public class LegendSymbol : TeeBase
    {
        internal bool continuous;
        private bool defaultPen;
        private Legend ILegend;
        private ChartPen iPen;
        internal LegendSymbolPosition position;
        private bool squared;
        private bool visible;
        private int width;
        private LegendSymbolSize widthUnits;

        public LegendSymbol(Legend legend) : base(legend.chart)
        {
            this.defaultPen = true;
            this.position = LegendSymbolPosition.Left;
            this.width = 20;
            this.widthUnits = LegendSymbolSize.Percent;
            this.visible = true;
            this.ILegend = legend;
        }

        internal int CalcWidth(int value)
        {
            if (!this.visible)
            {
                return 0;
            }
            if (this.squared)
            {
                return (this.ILegend.CalcItemHeight() - 5);
            }
            if (this.widthUnits == LegendSymbolSize.Percent)
            {
                return Utils.Round((double) ((this.width * value) * 0.01));
            }
            return this.width;
        }

        [DefaultValue(false), Description("Adjoins the different legend rectangles when true.")]
        public bool Continous
        {
            get
            {
                return this.continuous;
            }
            set
            {
                base.SetBooleanProperty(ref this.continuous, value);
            }
        }

        [DefaultValue(true), Description("Uses series pen properties to draw a border around the coloured box legend symbol, when true. ")]
        public bool DefaultPen
        {
            get
            {
                return this.defaultPen;
            }
            set
            {
                base.SetBooleanProperty(ref this.defaultPen, value);
            }
        }

        [Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Pen used to draw a border around the color box legend symbols.")]
        public ChartPen Pen
        {
            get
            {
                if (this.iPen == null)
                {
                    this.iPen = new ChartPen(this.ILegend.chart);
                }
                return this.iPen;
            }
        }

        [Description("Sets the position of the Legend color rectangles."), DefaultValue(0)]
        public LegendSymbolPosition Position
        {
            get
            {
                return this.position;
            }
            set
            {
                if (this.position != value)
                {
                    this.position = value;
                    this.Invalidate();
                }
            }
        }

        [DefaultValue(false), Description("When true, the legend symbol will be resized to square shaped.")]
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

        [Description("Shows or hides Legend symbols."), DefaultValue(true)]
        public bool Visible
        {
            get
            {
                return this.visible;
            }
            set
            {
                base.SetBooleanProperty(ref this.visible, value);
            }
        }

        [DefaultValue(20), Description("Defines the width of the color rectangles.")]
        public int Width
        {
            get
            {
                return this.width;
            }
            set
            {
                base.SetIntegerProperty(ref this.width, value);
            }
        }

        [Description("Defines the Width units for the width of Symbol."), DefaultValue(0)]
        public LegendSymbolSize WidthUnits
        {
            get
            {
                return this.widthUnits;
            }
            set
            {
                if (this.widthUnits != value)
                {
                    this.widthUnits = value;
                    this.Invalidate();
                }
            }
        }
    }
}

