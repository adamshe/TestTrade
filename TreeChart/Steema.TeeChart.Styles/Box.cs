namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using System;
    using System.Drawing;

    [ToolboxBitmap(typeof(Box), "SeriesIcons.Box.bmp")]
    public class Box : CustomBox
    {
        public Box() : this(null)
        {
        }

        public Box(Chart c) : base(c)
        {
        }

        public override double MaxXValue()
        {
            return base.dPosition;
        }

        public override double MinXValue()
        {
            return base.dPosition;
        }

        public override string Description
        {
            get
            {
                return Texts.GalleryBoxPlot;
            }
        }
    }
}

