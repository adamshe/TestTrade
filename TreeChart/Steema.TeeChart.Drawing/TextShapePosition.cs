namespace Steema.TeeChart.Drawing
{
    using Steema.TeeChart;
    using System;
    using System.ComponentModel;

    public class TextShapePosition : TextShape
    {
        protected bool bCustomPosition;

        public TextShapePosition(Chart c) : base(c)
        {
        }

        protected override bool ShouldSerializeBottom()
        {
            return this.bCustomPosition;
        }

        protected override bool ShouldSerializeLeft()
        {
            return this.bCustomPosition;
        }

        protected override bool ShouldSerializeRight()
        {
            return this.bCustomPosition;
        }

        protected override bool ShouldSerializeTop()
        {
            return this.bCustomPosition;
        }

        [DefaultValue(false), Description("Set to True to permit custom positioning of Shape.")]
        public bool CustomPosition
        {
            get
            {
                return this.bCustomPosition;
            }
            set
            {
                base.SetBooleanProperty(ref this.bCustomPosition, value);
            }
        }
    }
}

