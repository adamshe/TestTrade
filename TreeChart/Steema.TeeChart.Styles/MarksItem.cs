namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.Drawing;

    public class MarksItem : TextShape
    {
        public static Color ChartMarkColor = Color.LightYellow;

        public MarksItem() : this(null)
        {
        }

        public MarksItem(Chart c) : base(c)
        {
            base.Color = ChartMarkColor;
        }
    }
}

