namespace Steema.TeeChart
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Reflection;

    [Description("Custom labels list")]
    public class AxisLabelsItems : ArrayList
    {
        internal Axis iAxis;

        public AxisLabelsItems(Axis a)
        {
            this.iAxis = a;
        }

        public AxisLabelItem Add(double value)
        {
            AxisLabelItem item = new AxisLabelItem(this.iAxis.Chart);
            item.iAxisLabelsItems = this;
            item.Transparent = true;
            item.Value = value;
            base.Add(item);
            return item;
        }

        public AxisLabelItem Add(double value, string text)
        {
            AxisLabelItem item = this.Add(value);
            item.Text = text;
            return item;
        }

        public override void Clear()
        {
            for (int i = 0; i < base.Count; i++)
            {
                ((AxisLabelItem) base[i]).Dispose();
            }
            base.Clear();
            this.iAxis.Chart.Invalidate();
        }

        public void CopyFrom(AxisLabelsItems Source)
        {
            this.Clear();
            for (int i = 0; i < Source.Count; i++)
            {
                this.Add(Source[i].Value, Source[i].Text);
            }
        }

        public AxisLabelItem this[int index]
        {
            get
            {
                return (AxisLabelItem) base[index];
            }
        }
    }
}

