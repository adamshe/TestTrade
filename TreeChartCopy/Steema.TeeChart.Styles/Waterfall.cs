namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using System;
    using System.Drawing;

    [ToolboxBitmap(typeof(Waterfall), "SeriesIcons.Waterfall.bmp")]
    public class Waterfall : Surface
    {
        public Waterfall() : this(null)
        {
        }

        public Waterfall(Chart c) : base(c)
        {
            base.bWaterFall = true;
            base.WaterLines.Visible = true;
        }

        protected internal override void CreateSubGallery(Series.SubGalleryEventHandler AddSubChart)
        {
            base.CreateSubGallery(AddSubChart);
            AddSubChart(Texts.NoLines);
        }

        protected internal override void SetSubGallery(int index)
        {
            if (index == 6)
            {
                base.WaterLines.Visible = false;
            }
            else
            {
                base.SetSubGallery(index);
            }
        }

        public override string Description
        {
            get
            {
                return Texts.GalleryWaterFall;
            }
        }
    }
}

