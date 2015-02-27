namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.Drawing;

    [ToolboxBitmap(typeof(LinePoint), "SeriesIcons.LinePoint.bmp")]
    public class LinePoint : Points
    {
        public LinePoint() : this(null)
        {
        }

        public LinePoint(Chart c) : base(c)
        {
            base.Pointer.Draw3D = false;
            base.Pointer.Style = PointerStyles.Diamond;
            base.LinePen.Color = Color.Red;
        }

        public override void DrawValue(int valueIndex)
        {
            int x = this.CalcXPos(valueIndex);
            int y = this.CalcYPos(valueIndex);
            Graphics3D graphicsd = base.chart.graphics3D;
            graphicsd.Brush.Visible = false;
            graphicsd.Pen = base.pLinePen;
            graphicsd.MoveTo(base.GetVertAxis.Position, y, base.StartZ);
            graphicsd.LineTo(x, y, base.StartZ);
            graphicsd.LineTo(x, base.GetHorizAxis.Position, base.StartZ);
            base.DrawValue(valueIndex);
        }

        public override string Description
        {
            get
            {
                return Texts.GalleryLinePoint;
            }
        }
    }
}

