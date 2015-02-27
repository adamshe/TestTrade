namespace Steema.TeeChart
{
    using System;
    using System.Collections;
    using System.Reflection;

    public sealed class CustomAxes : CollectionBase
    {
        internal Steema.TeeChart.Chart Chart;

        public Axis Add(Axis axis)
        {
            if (base.List.IndexOf(axis) == -1)
            {
                base.List.Add(axis);
            }
            axis.Chart = this.Chart;
            return axis;
        }

        public int IndexOf(Axis a)
        {
            return base.List.IndexOf(a);
        }

        public Axis New()
        {
            return this.Add(new Axis(this.Chart));
        }

        protected override void OnInsertComplete(int index, object value)
        {
            base.OnInsertComplete(index, value);
            ((Axis) value).Chart = this.Chart;
        }

        public void Remove(Axis a)
        {
            int index = this.IndexOf(a);
            if (index != -1)
            {
                base.RemoveAt(index);
            }
        }

        public void RemoveAll()
        {
            while (base.Count > 0)
            {
                base.RemoveAt(0);
            }
        }

        public Axis this[int index]
        {
            get
            {
                return (Axis) base.List[index];
            }
            set
            {
                base.List[index] = value;
            }
        }
    }
}

