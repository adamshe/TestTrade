namespace Steema.TeeChart
{
    using System;
    using System.Drawing;

    public class Header : Title
    {
        public Header(Chart c) : base(c)
        {
            base.Font.Color = Color.Blue;
            base.Font.Brush.defaultColor = Color.Blue;
        }
    }
}

