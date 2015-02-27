namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    [ToolboxBitmap(typeof(Area), "SeriesIcons.Area.bmp")]
    public class Area : Custom
    {
        private double origin;
        private bool useOrigin;

        public Area() : this(null)
        {
        }

        public Area(Chart c) : base(c)
        {
            base.drawArea = true;
            base.AllowSinglePoint = false;
            base.Pointer.Visible = false;
            base.Pointer.defaultVisible = false;
            base.pAreaLines = new ChartPen(base.chart, Color.Black);
            base.bAreaBrush = new ChartBrush(base.chart, Color.Empty);
        }

        protected internal override void CreateSubGallery(Series.SubGalleryEventHandler AddSubChart)
        {
            base.CreateSubGallery(AddSubChart);
            AddSubChart(Texts.Stairs);
            AddSubChart(Texts.Marks);
            AddSubChart(Texts.Colors);
            AddSubChart(Texts.Hollow);
            AddSubChart(Texts.NoLines);
            AddSubChart(Texts.Stack);
            AddSubChart(Texts.Stack100);
            AddSubChart(Texts.Points);
            AddSubChart(Texts.Gradient);
        }

        protected override int GetOriginPos(int valueIndex)
        {
            if (this.useOrigin)
            {
                return base.CalcPosValue(this.origin);
            }
            return base.GetOriginPos(valueIndex);
        }

        public override double MaxXValue()
        {
            double origin = base.MaxXValue();
            if ((!base.yMandatory && this.useOrigin) && (origin < this.origin))
            {
                origin = this.origin;
            }
            return origin;
        }

        public override double MaxYValue()
        {
            double origin = base.MaxYValue();
            if ((base.yMandatory && this.useOrigin) && (origin < this.origin))
            {
                origin = this.origin;
            }
            return origin;
        }

        public override double MinXValue()
        {
            double origin = base.MinXValue();
            if ((!base.yMandatory && this.useOrigin) && (origin > this.origin))
            {
                origin = this.origin;
            }
            return origin;
        }

        public override double MinYValue()
        {
            double origin = base.MinYValue();
            if ((base.yMandatory && this.useOrigin) && (origin > this.origin))
            {
                origin = this.origin;
            }
            return origin;
        }

        protected override void PrepareLegendCanvas(Graphics3D g, int valueIndex, ref Color backColor, ref ChartBrush aBrush)
        {
            backColor = base.GetAreaBrushColor(g.Brush.ForegroundColor);
            g.Pen = this.AreaLines;
            aBrush = this.AreaBrush;
        }

        protected internal override void SetSubGallery(int index)
        {
            switch (index)
            {
                case 1:
                    base.Stairs = true;
                    return;

                case 2:
                    base.Marks.Visible = true;
                    return;

                case 3:
                    base.ColorEach = true;
                    return;

                case 4:
                    this.AreaBrush.Visible = false;
                    return;

                case 5:
                    this.AreaLines.Visible = false;
                    return;

                case 6:
                    this.MultiArea = MultiAreas.Stacked;
                    return;

                case 7:
                    this.MultiArea = MultiAreas.Stacked100;
                    return;

                case 8:
                    base.Pointer.Visible = true;
                    return;

                case 9:
                    this.Gradient.Visible = true;
                    return;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Determines Brush to fill the background Area region.")]
        public ChartBrush AreaBrush
        {
            get
            {
                return base.bAreaBrush;
            }
        }

        [DefaultValue((string) null), Description("Determines Pen to draw AreaLines."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ChartPen AreaLines
        {
            get
            {
                return base.pAreaLines;
            }
        }

        [Browsable(false), Obsolete("Please use AreaLines property."), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue((string) null), EditorBrowsable(EditorBrowsableState.Never)]
        public ChartPen AreaLinesPen
        {
            get
            {
                return this.AreaLines;
            }
        }

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.GalleryArea;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Determines Gradient to fill the background Area region."), Category("Appearance")]
        public Steema.TeeChart.Drawing.Gradient Gradient
        {
            get
            {
                return this.AreaBrush.Gradient;
            }
        }

        [DefaultValue(0), Description("Determines how Multi-AreaSeries are displayed.")]
        public MultiAreas MultiArea
        {
            get
            {
                if (base.iStacked == CustomStack.Stack)
                {
                    return MultiAreas.Stacked;
                }
                if (base.iStacked == CustomStack.Stack100)
                {
                    return MultiAreas.Stacked100;
                }
                return MultiAreas.None;
            }
            set
            {
                if (value != this.MultiArea)
                {
                    if (value == MultiAreas.None)
                    {
                        base.Stacked = CustomStack.None;
                    }
                    else if (value == MultiAreas.Stacked)
                    {
                        base.Stacked = CustomStack.Stack;
                    }
                    else if (value == MultiAreas.Stacked100)
                    {
                        base.Stacked = CustomStack.Stack100;
                    }
                }
            }
        }

        [DefaultValue((double) 0.0), Description("Sets axis value as a common bottom for all AreaSeries points.")]
        public double Origin
        {
            get
            {
                return this.origin;
            }
            set
            {
                base.SetDoubleProperty(ref this.origin, value);
            }
        }

        [Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Determines to fill the top 3D Area region.")]
        public Steema.TeeChart.Drawing.Gradient TopGradient
        {
            get
            {
                return base.bBrush.Gradient;
            }
        }

        [Description("Aligns bottom of AreaSeries to the Origin property value."), DefaultValue(false)]
        public bool UseOrigin
        {
            get
            {
                return this.useOrigin;
            }
            set
            {
                base.SetBooleanProperty(ref this.useOrigin, value);
            }
        }
    }
}

