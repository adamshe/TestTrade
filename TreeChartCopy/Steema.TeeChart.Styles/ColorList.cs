namespace Steema.TeeChart.Styles
{
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Reflection;

    public sealed class ColorList : ArrayList
    {
        public ColorList(int capacity) : base(capacity)
        {
        }

        internal void Exchange(int a, int b)
        {
            Color color = this[a];
            this[a] = this[b];
            this[b] = color;
        }

        public Color this[int index]
        {
            get
            {
                if (index >= this.Count)
                {
                    return Color.Empty;
                }
                return (Color) base[index];
            }
            set
            {
                while (this.Count <= index)
                {
                    this.Add(Color.Empty);
                }
                base[index] = value;
            }
        }
    }
}

