namespace Steema.TeeChart
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    public class Scroll : ZoomScroll
    {
        private ScrollModes allow;
        private MouseButtons mouseButton;

        public Scroll(Chart c) : base(c)
        {
            this.allow = ScrollModes.Both;
            this.mouseButton = MouseButtons.Right;
        }

        [Description("Enables/Disables selected ScrollModes."), DefaultValue(3)]
        public ScrollModes Allow
        {
            get
            {
                return this.allow;
            }
            set
            {
                this.allow = value;
            }
        }

        [Description("Sets the Mousebutton to use for the scroll action."), DefaultValue(0x200000)]
        public MouseButtons MouseButton
        {
            get
            {
                return this.mouseButton;
            }
            set
            {
                this.mouseButton = value;
            }
        }
    }
}

