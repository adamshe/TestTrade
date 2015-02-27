namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    [Description("Displays a coloured rectangle (band) at the specified axis and position."), ToolboxBitmap(typeof(ColorBand), "ToolsIcons.ColorBand.bmp")]
    public class ColorBand : ToolAxis
    {
        private bool drawBehind;
        private double end;
        private double start;

        public ColorBand() : this(null)
        {
        }

        public ColorBand(Chart c) : base(c)
        {
            this.drawBehind = true;
        }

        protected internal override void ChartEvent(EventArgs e)
        {
            base.ChartEvent(e);
            if (((e is BeforeDrawSeriesEventArgs) && this.drawBehind) || ((e is AfterDrawEventArgs) && !this.drawBehind))
            {
                this.PaintBand();
            }
        }

        private void PaintBand()
        {
            if (base.iAxis != null)
            {
                bool flag;
                double num3;
                Rectangle chartRect = base.chart.ChartRect;
                double start = this.start;
                double end = this.end;
                if (base.iAxis.Inverted)
                {
                    if (start < end)
                    {
                        num3 = start;
                        start = end;
                        end = num3;
                    }
                    flag = (end <= base.iAxis.Maximum) && (start >= base.iAxis.Minimum);
                }
                else
                {
                    if (start > end)
                    {
                        num3 = start;
                        start = end;
                        end = num3;
                    }
                    flag = (start <= base.iAxis.Maximum) && (end >= base.iAxis.Minimum);
                }
                if (flag)
                {
                    if (base.iAxis.Horizontal)
                    {
                        chartRect.X = Math.Max(base.iAxis.IStartPos, base.iAxis.CalcPosValue(start));
                        chartRect.Width = Math.Min(base.iAxis.IEndPos, base.iAxis.CalcPosValue(end)) - chartRect.X;
                        if (!base.Pen.Visible)
                        {
                            chartRect.Width++;
                        }
                    }
                    else
                    {
                        chartRect.Y = Math.Max(base.iAxis.IStartPos, base.iAxis.CalcPosValue(end));
                        chartRect.Height = Math.Min(base.iAxis.IEndPos, base.iAxis.CalcPosValue(start)) - chartRect.Y;
                        chartRect.X++;
                        if (!base.Pen.Visible)
                        {
                            chartRect.Height++;
                            chartRect.Width++;
                        }
                    }
                    Graphics3D graphicsd = base.chart.graphics3D;
                    graphicsd.Brush = this.Brush;
                    graphicsd.Pen = base.Pen;
                    if (base.chart.aspect.view3D && this.drawBehind)
                    {
                        graphicsd.Rectangle(chartRect, base.chart.aspect.Width3D);
                    }
                    else
                    {
                        graphicsd.Rectangle(chartRect);
                    }
                }
            }
        }

        [Description("Element Brush characteristics."), Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ChartBrush Brush
        {
            get
            {
                if (base.bBrush == null)
                {
                    base.bBrush = new ChartBrush(base.chart);
                }
                return base.bBrush;
            }
        }

        [Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Description("Sets Band Color.")]
        public System.Drawing.Color Color
        {
            get
            {
                return this.Brush.Color;
            }
            set
            {
                this.Brush.Color = value;
            }
        }

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.ColorBandTool;
            }
        }

        [DefaultValue(true), Description("Draws the Colorband behind the series values.")]
        public bool DrawBehind
        {
            get
            {
                return this.drawBehind;
            }
            set
            {
                base.SetBooleanProperty(ref this.drawBehind, value);
            }
        }

        [Description("Sets End Axis value of colorband.")]
        public double End
        {
            get
            {
                return this.end;
            }
            set
            {
                base.SetDoubleProperty(ref this.end, value);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Category("Appearance"), Description("Sets Band colour gradient.")]
        public Steema.TeeChart.Drawing.Gradient Gradient
        {
            get
            {
                return this.Brush.Gradient;
            }
        }

        [Description("Sets Start Axis value of colorband.")]
        public double Start
        {
            get
            {
                return this.start;
            }
            set
            {
                base.SetDoubleProperty(ref this.start, value);
            }
        }

        [Description("Gets detailed descriptive text.")]
        public override string Summary
        {
            get
            {
                return Texts.ColorBandSummary;
            }
        }

        [DefaultValue(0), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Category("Appearance"), Description("Sets the Transparency of ColorBand as percentage.")]
        public int Transparency
        {
            get
            {
                return this.Brush.Transparency;
            }
            set
            {
                base.bBrush.Transparency = value;
            }
        }
    }
}

