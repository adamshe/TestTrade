namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using System;

    public class Custom2DPolar : CustomPolar
    {
        public Custom2DPolar() : this(null)
        {
        }

        public Custom2DPolar(Chart c) : base(c)
        {
        }

        protected internal override void GalleryChanged3D(bool Is3D)
        {
            base.GalleryChanged3D(Is3D);
            base.chart.Aspect.View3D = false;
        }

        internal override void PrepareForGallery(bool IsEnabled)
        {
            base.PrepareForGallery(IsEnabled);
            base.CircleLabelsFont.Size = 6;
            base.Pointer.HorizSize = 2;
            base.Pointer.VertSize = 2;
        }
    }
}

