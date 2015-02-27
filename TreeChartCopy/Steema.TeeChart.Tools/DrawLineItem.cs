namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    public class DrawLineItem
    {
        public PointDouble EndPos;
        public PointDouble StartPos;
        public DrawLineStyle Style;
        private DrawLine tool;

        public DrawLineItem(DrawLine owner)
        {
            this.tool = owner;
            this.tool.Lines.Add(this);
        }

        public void DrawHandles()
        {
            Graphics3D graphicsd = this.tool.chart.graphics3D;
            graphicsd.Brush.Visible = true;
            graphicsd.Brush.Solid = true;
            graphicsd.Brush.Color = (this.tool.chart.Panel.Color == Color.Black) ? Color.Silver : Color.Black;
            graphicsd.Pen.Visible = false;
            graphicsd.Rectangle(this.StartHandle, 0);
            graphicsd.Rectangle(this.EndHandle, 0);
        }

        private Rectangle RectangleFromPoint(Point p)
        {
            return new Rectangle(p.X - 3, p.Y - 3, 6, 6);
        }

        [Description("Returns Rect of the DrawLine end handle.")]
        public Rectangle EndHandle
        {
            get
            {
                return this.RectangleFromPoint(this.tool.AxisPoint(this.EndPos));
            }
        }

        [Description("Returns Rect of the DrawLine start handle.")]
        public Rectangle StartHandle
        {
            get
            {
                return this.RectangleFromPoint(this.tool.AxisPoint(this.StartPos));
            }
        }
    }
}

