namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    [Serializable, ToolboxBitmap(typeof(Points), "SeriesIcons.Points.bmp")]
    public class Points : CustomPoint
    {
        public Points() : this(null)
        {
        }

        public Points(Chart c) : base(c)
        {
            if (base.point == null)
            {
                base.Pointer.Color = Color.Empty;
            }
            base.Marks.defaultArrowLength = 0;
            base.Marks.Callout.Length = 0;
        }

        protected virtual bool CanDoExtra()
        {
            return true;
        }

        protected internal override void CreateSubGallery(Series.SubGalleryEventHandler AddSubChart)
        {
            base.CreateSubGallery(AddSubChart);
            AddSubChart(Texts.Colors);
            AddSubChart(Texts.Marks);
            AddSubChart(Texts.Hollow);
            AddSubChart(Texts.NoBorder);
            AddSubChart(Texts.Gradient);
            if (this.CanDoExtra())
            {
                AddSubChart(Texts.Point2D);
                AddSubChart(Texts.Triangle);
                AddSubChart(Texts.Star);
                AddSubChart(Texts.Circle);
                AddSubChart(Texts.DownTri);
                AddSubChart(Texts.Cross);
                AddSubChart(Texts.Diamond);
            }
        }

        protected override Color GetSeriesColor()
        {
            if (base.point != null)
            {
                return base.Pointer.Color;
            }
            return Color.Empty;
        }

        protected override void SetColorEach(bool value)
        {
            base.SetColorEach(value);
            if (value)
            {
                base.point.Brush.ForegroundColor = Color.Empty;
            }
        }

        protected override void SetSeriesColor(Color value)
        {
            base.SetSeriesColor(value);
            base.Pointer.Color = value;
        }

        protected internal override void SetSubGallery(int index)
        {
            switch (index)
            {
                case 1:
                    base.ColorEach = true;
                    return;

                case 2:
                    base.Marks.Visible = true;
                    return;

                case 3:
                    base.Pointer.Brush.Visible = false;
                    return;

                case 4:
                    base.Pointer.Pen.Visible = false;
                    return;

                case 5:
                    base.Pointer.Gradient.Visible = true;
                    return;

                default:
                    if (!this.CanDoExtra())
                    {
                        break;
                    }
                    switch (index)
                    {
                        case 6:
                            base.Pointer.Draw3D = false;
                            return;

                        case 7:
                            base.Pointer.Style = PointerStyles.Triangle;
                            return;

                        case 8:
                            base.Pointer.Style = PointerStyles.Star;
                            return;

                        case 9:
                            base.Pointer.Style = PointerStyles.Circle;
                            base.Pointer.HorizSize = 8;
                            base.Pointer.VertSize = 8;
                            return;

                        case 10:
                            base.Pointer.Style = PointerStyles.DownTriangle;
                            return;

                        case 11:
                            base.Pointer.Style = PointerStyles.Cross;
                            return;

                        case 12:
                            base.Pointer.Style = PointerStyles.Diamond;
                            break;
                    }
                    return;
            }
        }

        public override string Description
        {
            get
            {
                return Texts.GalleryPoint;
            }
        }

        [Browsable(false), Description("Sets the Pen for the Point connecting Lines.")]
        public ChartPen LinePen
        {
            get
            {
                return base.pLinePen;
            }
        }
    }
}

