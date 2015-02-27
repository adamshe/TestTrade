namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart.Drawing;
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Reflection;

    public class MarksItems : ArrayList
    {
        internal bool ILoadingCustom = false;
        internal TextShape IMarks;

        public MarksItems(SeriesMarks s)
        {
            this.IMarks = s;
        }

        public override void Clear()
        {
            base.Clear();
            this.IMarks.Invalidate();
        }

        public MarksItem this[int index]
        {
            get
            {
                while (index > (this.Count - 1))
                {
                    this.Add(null);
                }
                if (base[index] == null)
                {
                    MarksItem item = new MarksItem(this.IMarks.Chart);
                    item.Color = MarksItem.ChartMarkColor;
                    ((SeriesMarks) this.IMarks).Shadow.Width = 1;
                    ((SeriesMarks) this.IMarks).Shadow.Height = 1;
                    ((SeriesMarks) this.IMarks).Shadow.Color = Color.Gray;
                    base[index] = item;
                }
                return (MarksItem) base[index];
            }
        }
    }
}

