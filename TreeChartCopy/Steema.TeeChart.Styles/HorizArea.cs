namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;

    [ToolboxBitmap(typeof(HorizArea), "SeriesIcons.HorizArea.bmp")]
    public class HorizArea : Area
    {
        public HorizArea() : this(null)
        {
        }

        public HorizArea(Chart c) : base(c)
        {
            base.SetHorizontal();
            base.XValues.Order = ValueListOrder.None;
            base.YValues.Order = ValueListOrder.Ascending;
            base.Gradient.Direction = LinearGradientMode.Horizontal;
        }

        protected internal override void DrawMark(int valueIndex, string st, SeriesMarks.Position aPosition)
        {
            int num = aPosition.Height / 2;
            int num2 = base.Marks.Callout.Length + base.Marks.Callout.Distance;
            aPosition.LeftTop.Y = aPosition.ArrowTo.Y - num;
            aPosition.LeftTop.X += num2 + (aPosition.Width / 2);
            aPosition.ArrowTo.X += num2;
            aPosition.ArrowFrom.Y = aPosition.ArrowTo.Y;
            aPosition.ArrowFrom.X += base.Marks.Callout.Distance;
            base.DrawMark(valueIndex, st, aPosition);
        }

        protected internal override int NumSampleValues()
        {
            return 10;
        }

        public override string Description
        {
            get
            {
                return Texts.HorizAreaSeries;
            }
        }
    }
}

