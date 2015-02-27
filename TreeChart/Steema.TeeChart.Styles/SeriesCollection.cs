namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Drawing.Design;
    using System.Reflection;
    using System.Windows.Forms;

    [Editor(typeof(Editor), typeof(UITypeEditor))]
    public sealed class SeriesCollection : CollectionBase
    {
        private bool applyZOrder = true;
        internal Steema.TeeChart.Chart chart;

        public SeriesCollection(Steema.TeeChart.Chart c)
        {
            this.chart = c;
        }

        public bool ActiveUseAxis()
        {
            foreach (Series series in this)
            {
                if (series.Active)
                {
                    return series.UseAxis;
                }
            }
            return true;
        }

        public Series Add(Series s)
        {
            s.Chart = this.chart;
            if (base.List.IndexOf(s) == -1)
            {
                base.List.Add(s);
                this.chart.BroadcastEvent(s, SeriesEventStyle.Add);
            }
            return s;
        }

        public Series Add(System.Type type)
        {
            return this.Add(Series.NewFromType(type));
        }

        public void Clear()
        {
            this.Clear(true);
        }

        public void Clear(bool dispose)
        {
            while (base.Count > 0)
            {
                Series series = this[0];
                base.RemoveAt(0);
                series.OnDisposing();
                if (dispose)
                {
                    series.Dispose();
                }
            }
            base.Clear();
            this.chart.Invalidate();
            this.chart.BroadcastEvent(null, SeriesEventStyle.RemoveAll);
        }

        public void Exchange(int series1, int series2)
        {
            Series series = this[series1];
            Series series3 = this[series2];
            this[series1] = series3;
            this[series2] = series;
            this.chart.BroadcastEvent(null, SeriesEventStyle.Swap);
            this.chart.Invalidate();
        }

        public void FillItems(ListBox.ObjectCollection items)
        {
            foreach (Series series in this)
            {
                items.Add(series);
            }
        }

        public int IndexOf(Series s)
        {
            return base.List.IndexOf(s);
        }

        internal void InternalAdd(Series s)
        {
            base.List.Add(s);
        }

        public void MoveTo(Series s, int index)
        {
            int num = this.IndexOf(s);
            if (num != index)
            {
                base.List.Insert(index, s);
                if (num > index)
                {
                    num++;
                }
                base.InnerList.RemoveAt(num);
            }
        }

        protected override void OnInsertComplete(int index, object value)
        {
            ((Series) value).Chart = this.chart;
        }

        public void Remove(Series s)
        {
            int index = this.IndexOf(s);
            if (index > -1)
            {
                this.chart.BroadcastEvent(s, SeriesEventStyle.Remove);
                base.RemoveAt(index);
                this.chart.Invalidate();
            }
        }

        public void RemoveAllSeries()
        {
            while (base.Count > 0)
            {
                base.RemoveAt(0);
            }
        }

        public Series WithTitle(string title)
        {
            foreach (Series series in this)
            {
                if (series.ToString() == title)
                {
                    return series;
                }
            }
            return null;
        }

        [DefaultValue(true), Description("Sets multiple Series on same Chart in different Z spaces when true.")]
        public bool ApplyZOrder
        {
            get
            {
                return this.applyZOrder;
            }
            set
            {
                this.applyZOrder = value;
                if (this.chart != null)
                {
                    this.chart.Invalidate();
                }
            }
        }

        [Description("Defines the Chart component."), Browsable(false)]
        public Steema.TeeChart.Chart Chart
        {
            get
            {
                return this.chart;
            }
        }

        public Series this[int index]
        {
            get
            {
                return (Series) base.List[index];
            }
            set
            {
                base.List[index] = value;
            }
        }

        internal sealed class Editor : CollectionEditor
        {
            public Editor(System.Type type) : base(type)
            {
            }

            protected override object CreateInstance(System.Type itemType)
            {
                Chart c = null;
                if (base.Context.Instance is Chart)
                {
                    c = (Chart) base.Context.Instance;
                }
                else if (base.Context.Instance is TChart)
                {
                    TChart instance = (TChart) base.Context.Instance;
                    c = instance.Chart;
                }
                if (c == null)
                {
                    return null;
                }
                IContainer chartContainer = c.ChartContainer;
                Series series = ChartGallery.CreateNew(c, null);
                if (series != null)
                {
                    if (chartContainer != null)
                    {
                        series.AddToContainer(chartContainer);
                        return series;
                    }
                    series.Added();
                }
                return series;
            }
        }
    }
}

