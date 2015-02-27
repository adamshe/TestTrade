namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using System;
    using System.Drawing;
    using System.IO;
    using System.Reflection;

    [ToolboxBitmap(typeof(ImageBar), "SeriesIcons.ImageBar.bmp")]
    public class ImageBar : Bar
    {
        private Image image;
        private bool imageTiled;

        public ImageBar() : this(null)
        {
        }

        public ImageBar(Chart c) : base(c)
        {
            this.imageTiled = false;
            string name = "TeeChart.Images.ImageBar.Dollar.bmp";
            Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name);
            if (manifestResourceStream != null)
            {
                this.image = Image.FromStream(manifestResourceStream);
            }
        }

        public override void DrawBar(int BarIndex, int StartPos, int EndPos)
        {
            base.DrawBar(BarIndex, StartPos, EndPos);
            if (this.image != null)
            {
                int y;
                Rectangle barBounds = base.BarBounds;
                if (barBounds.Bottom < barBounds.Y)
                {
                    y = barBounds.Y;
                    barBounds.Y = barBounds.Bottom;
                    barBounds.Height = barBounds.Y - y;
                }
                if (base.Pen.Visible)
                {
                    y = base.Pen.Width;
                    if ((y > 1) && ((y % 2) == 0))
                    {
                        y--;
                    }
                    barBounds.X += y;
                    barBounds.Y += y;
                    if (!base.chart.Aspect.View3D)
                    {
                        barBounds.Width--;
                        barBounds.Height--;
                    }
                }
                barBounds = base.chart.graphics3D.CalcRect3D(barBounds, base.StartZ);
                if (this.imageTiled)
                {
                    this.DrawTiledImage(this.image, barBounds, base.BarBounds.Bottom < base.BarBounds.Top);
                }
                else
                {
                    base.chart.graphics3D.Draw(barBounds, this.image, false);
                }
            }
        }

        private void DrawTiledImage(Image aImage, Rectangle R, bool StartFromTop)
        {
            int width = aImage.Width;
            int height = aImage.Height;
            if ((width > 0) && (height > 0))
            {
                base.chart.graphics3D.ClipRectangle(R);
                int num3 = R.Width;
                int num4 = R.Height;
                for (int i = 0; i < num4; i += height)
                {
                    for (int j = 0; j < num3; j += width)
                    {
                        Rectangle rectangle;
                        if (StartFromTop)
                        {
                            rectangle = Rectangle.FromLTRB(R.X, R.Y + i, R.Right, (R.Y + i) + height);
                        }
                        else
                        {
                            rectangle = Rectangle.FromLTRB(R.X, (R.Bottom - i) - height, R.Right, R.Bottom - i);
                        }
                        base.chart.graphics3D.Draw(rectangle, this.image, false);
                    }
                }
                base.chart.graphics3D.UnClip();
            }
        }

        internal override void PrepareForGallery(bool IsEnabled)
        {
            base.PrepareForGallery(IsEnabled);
            base.FillSampleValues(2);
            base.chart.Aspect.Orthogonal = true;
        }

        public bool ImageTiled
        {
            get
            {
                return this.imageTiled;
            }
            set
            {
                base.SetBooleanProperty(ref this.imageTiled, value);
            }
        }
    }
}

