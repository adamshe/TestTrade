namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using System;
    using System.Drawing;

    [ToolboxBitmap(typeof(Bezier), "SeriesIcons.Bezier.bmp")]
    public class Bezier : Custom
    {
        public Bezier() : this(null)
        {
        }

        public Bezier(Chart c) : base(c)
        {
        }

        protected internal override void Draw()
        {
            int count = base.Count;
            while (((count - 4) % 3) != 0)
            {
                count++;
            }
            Point[] p = new Point[count];
            for (int i = 0; i < base.Count; i++)
            {
                p[i].X = this.CalcXPos(i);
                p[i].Y = this.CalcYPos(i);
            }
            for (int j = base.Count; j < count; j++)
            {
                p[j] = p[base.Count - 1];
            }
            base.chart.graphics3D.Pen = base.LinePen;
            base.chart.graphics3D.Brush.Visible = false;
            if (base.chart.Aspect.View3D)
            {
                base.chart.graphics3D.DrawBeziers(base.MiddleZ, p);
            }
            else
            {
                base.chart.graphics3D.DrawBeziers(p);
            }
            if (base.point.Visible)
            {
                for (int k = 0; k < base.Count; k++)
                {
                    base.DrawPointer(p[k].X, p[k].Y, this.ValueColor(k), k);
                }
            }
        }

        internal override void PrepareForGallery(bool IsEnabled)
        {
            base.PrepareForGallery(IsEnabled);
            base.FillSampleValues(7);
            base.ColorEach = IsEnabled;
            base.Pointer.Draw3D = false;
        }

        protected override void SetSeriesColor(Color c)
        {
            base.SetSeriesColor(c);
            base.LinePen.Color = c;
        }

        public override string Description
        {
            get
            {
                return Texts.GalleryBezier;
            }
        }
    }
}

