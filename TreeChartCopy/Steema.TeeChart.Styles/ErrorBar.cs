namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    [ToolboxBitmap(typeof(ErrorBar), "SeriesIcons.ErrorBar.bmp")]
    public class ErrorBar : CustomError
    {
        public ErrorBar() : this(null)
        {
        }

        public ErrorBar(Chart c) : base(c)
        {
            base.bDrawBar = true;
            base.iErrorStyle = ErrorStyles.Top;
        }

        internal override void PrepareForGallery(bool IsEnabled)
        {
            base.ErrorPen.Width = 2;
            base.PrepareForGallery(IsEnabled);
        }

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.GalleryErrorBar;
            }
        }
    }
}

