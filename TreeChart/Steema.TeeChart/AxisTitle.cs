namespace Steema.TeeChart
{
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;

    public sealed class AxisTitle : TextShape
    {
        private int angle;
        internal int customSize;
        private int iDefaultAngle;

        public AxisTitle(Chart c) : base(c)
        {
        }

        internal void SetInitialAngle(int a)
        {
            this.angle = a;
            this.iDefaultAngle = this.angle;
        }

        private bool ShouldSerializeAngle()
        {
            return (this.angle != this.iDefaultAngle);
        }

        [Description("Rotation degree applied to each Axis Label.")]
        public int Angle
        {
            get
            {
                return this.angle;
            }
            set
            {
                base.SetIntegerProperty(ref this.angle, value % 360);
            }
        }

        [DefaultValue(""), Description("Defines text for each Chart Axis.")]
        public string Caption
        {
            get
            {
                return base.Text;
            }
            set
            {
                if (value != null)
                {
                    base.Text = value;
                }
            }
        }

        [Description("Change spacing between the axis/labels and the outer panel edge."), DefaultValue(0)]
        public int CustomSize
        {
            get
            {
                return this.customSize;
            }
            set
            {
                base.SetIntegerProperty(ref this.customSize, value);
            }
        }
    }
}

