namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using System;
    using System.Drawing;

    [ToolboxBitmap(typeof(Error), "SeriesIcons.Error.bmp")]
    public class Error : CustomError
    {
        public Error() : this(null)
        {
        }

        public Error(Chart c) : base(c)
        {
        }

        public override string Description
        {
            get
            {
                return Texts.GalleryError;
            }
        }
    }
}

