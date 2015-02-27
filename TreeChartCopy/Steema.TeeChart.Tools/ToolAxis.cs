namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    public class ToolAxis : Tool
    {
        protected Steema.TeeChart.Axis iAxis;

        public ToolAxis() : this((Chart) null)
        {
        }

        public ToolAxis(Steema.TeeChart.Axis a) : this(a.Chart)
        {
            this.iAxis = a;
        }

        public ToolAxis(Chart c) : base(c)
        {
        }

        [Description("Sets the axis to which a Tool will belong."), DefaultValue((string) null)]
        public Steema.TeeChart.Axis Axis
        {
            get
            {
                return this.iAxis;
            }
            set
            {
                if (this.iAxis != value)
                {
                    this.iAxis = value;
                    this.Invalidate();
                }
            }
        }

        [Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Element Pen characteristics.")]
        public ChartPen Pen
        {
            get
            {
                if (base.pPen == null)
                {
                    base.pPen = new ChartPen(base.chart, Color.Black);
                }
                return base.pPen;
            }
        }
    }
}

