namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    [ToolboxBitmap(typeof(Donut), "SeriesIcons.Donut.bmp")]
    public class Donut : Pie
    {
        private const int DefaultDonutPercent = 50;

        public Donut() : this(null)
        {
        }

        public Donut(Chart c) : base(c)
        {
            base.SetDonutPercent(50);
        }

        protected internal override void GalleryChanged3D(bool Is3D)
        {
            base.GalleryChanged3D(Is3D);
            base.Circled = true;
        }

        public override string Description
        {
            get
            {
                return Texts.GalleryDonut;
            }
        }

        [DefaultValue(50)]
        public int DonutPercent
        {
            get
            {
                return base.iDonutPercent;
            }
            set
            {
                base.SetDonutPercent(value);
            }
        }
    }
}

