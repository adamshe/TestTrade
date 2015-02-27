namespace Steema.TeeChart
{
    using System;
    using System.ComponentModel;

    public class DepthAxis : Axis
    {
        public DepthAxis(bool horiz, bool isOtherSide, Chart c) : base(horiz, isOtherSide, c)
        {
            base.IsDepthAxis = true;
            base.bVisible = false;
        }

        [Description("Show/hide Axis."), Category("Axis"), DefaultValue(false)]
        public bool Visible
        {
            get
            {
                return base.bVisible;
            }
            set
            {
                base.Visible = value;
            }
        }
    }
}

