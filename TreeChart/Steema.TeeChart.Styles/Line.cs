namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    [ToolboxBitmap(typeof(Line), "SeriesIcons.Line.bmp")]
    public class Line : Custom
    {
        public Line() : this(null)
        {
        }

        public Line(Chart c) : base(c)
        {
            base.drawLine = true;
            base.Pointer.Visible = false;
            base.Pointer.defaultVisible = false;
            base.AllowSinglePoint = false;
        }

        protected internal override void CreateSubGallery(Series.SubGalleryEventHandler AddSubChart)
        {
            base.CreateSubGallery(AddSubChart);
            AddSubChart(Texts.Stairs);
            AddSubChart(Texts.Points);
            AddSubChart(Texts.Height);
            AddSubChart(Texts.Hollow);
            AddSubChart(Texts.Colors);
            AddSubChart(Texts.Marks);
            AddSubChart(Texts.NoBorder);
        }

        protected override void PrepareLegendCanvas(Graphics3D g, int valueIndex, ref Color backColor, ref ChartBrush aBrush)
        {
            g.Pen = base.LinePen;
            g.Brush = aBrush;
        }

        protected internal override void SetSubGallery(int Index)
        {
            switch (Index)
            {
                case 1:
                    base.Stairs = true;
                    return;

                case 2:
                    base.Pointer.Visible = true;
                    return;

                case 3:
                    base.LineHeight = 5;
                    return;

                case 4:
                    base.Brush.Visible = false;
                    return;

                case 5:
                    base.ColorEach = true;
                    return;

                case 6:
                    base.Marks.Visible = true;
                    return;

                case 7:
                    base.LinePen.Visible = false;
                    return;
            }
        }

        public override string Description
        {
            get
            {
                return Texts.GalleryLine;
            }
        }

        [Description("Determines the Line Gradient."), Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Steema.TeeChart.Drawing.Gradient Gradient
        {
            get
            {
                return base.bBrush.Gradient;
            }
        }
    }
}

