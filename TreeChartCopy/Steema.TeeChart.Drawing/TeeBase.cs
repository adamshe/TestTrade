namespace Steema.TeeChart.Drawing
{
    using Steema.TeeChart;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    [Serializable, ToolboxItem(false)]
    public class TeeBase : Component
    {
        [NonSerialized]
        internal Steema.TeeChart.Chart chart;

        public TeeBase()
        {
        }

        public TeeBase(Steema.TeeChart.Chart c)
        {
            this.chart = c;
        }

        [Description("Repaints chart asynchronously.")]
        public virtual void Invalidate()
        {
            if (((this.chart != null) && !this.chart.graphics3D.Dirty) && this.chart.AutoRepaint)
            {
                this.chart.graphics3D.Dirty = true;
                if (this.chart.parent != null)
                {
                    this.chart.parent.DoInvalidate();
                }
            }
        }

        protected void SetBooleanProperty(ref bool variable, bool value)
        {
            if (variable != value)
            {
                variable = value;
                this.Invalidate();
            }
        }

        protected virtual void SetChart(Steema.TeeChart.Chart value)
        {
            this.chart = value;
        }

        protected void SetColorProperty(ref Color variable, Color value)
        {
            if (variable != value)
            {
                variable = value;
                this.Invalidate();
            }
        }

        protected void SetDoubleProperty(ref double variable, double value)
        {
            if (variable != value)
            {
                variable = value;
                this.Invalidate();
            }
        }

        protected void SetIntegerProperty(ref int variable, int value)
        {
            if (variable != value)
            {
                variable = value;
                this.Invalidate();
            }
        }

        protected void SetStringProperty(ref string variable, string value)
        {
            if (variable != value)
            {
                variable = value;
                this.Invalidate();
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Steema.TeeChart.Chart Chart
        {
            get
            {
                return this.chart;
            }
            set
            {
                this.SetChart(value);
            }
        }
    }
}

