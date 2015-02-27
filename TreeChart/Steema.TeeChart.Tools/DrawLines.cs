namespace Steema.TeeChart.Tools
{
    using System;
    using System.Collections;
    using System.Reflection;

    public class DrawLines : CollectionBase
    {
        internal DrawLine tool;

        public int Add(DrawLineItem l)
        {
            int index = base.List.IndexOf(l);
            if (index != -1)
            {
                return index;
            }
            return base.List.Add(l);
        }

        public void Clear()
        {
            base.Clear();
            this.tool.Invalidate();
        }

        public int IndexOf(DrawLineItem l)
        {
            return base.List.IndexOf(l);
        }

        public void Remove(DrawLineItem s)
        {
            base.RemoveAt(base.List.IndexOf(s));
            this.tool.Invalidate();
        }

        public DrawLineItem this[int index]
        {
            get
            {
                return (DrawLineItem) base.List[index];
            }
        }
    }
}

