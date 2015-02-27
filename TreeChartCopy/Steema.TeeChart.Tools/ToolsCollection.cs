namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors.Tools;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Drawing.Design;
    using System.Reflection;

    [Serializable, Editor(typeof(Editor), typeof(UITypeEditor))]
    public sealed class ToolsCollection : CollectionBase
    {
        public Chart chart;

        public ToolsCollection(Chart c)
        {
            this.chart = c;
        }

        public int Add(Tool tool)
        {
            tool.Chart = this.chart;
            int index = this.IndexOf(tool);
            if (index == -1)
            {
                index = base.List.Add(tool);
            }
            return index;
        }

        public int IndexOf(Tool s)
        {
            return base.List.IndexOf(s);
        }

        protected override void OnInsertComplete(int index, object value)
        {
            ((Tool) value).chart = this.chart;
        }

        public void Remove(Tool s)
        {
            int index = this.IndexOf(s);
            if (index != -1)
            {
                base.RemoveAt(index);
                this.chart.Invalidate();
            }
        }

        public Tool this[int index]
        {
            get
            {
                return (Tool) base.List[index];
            }
            set
            {
                base.List[index] = value;
            }
        }

        internal sealed class Editor : CollectionEditor
        {
            public Editor(Type type) : base(type)
            {
            }

            protected override object CreateInstance(Type itemType)
            {
                Chart instance = null;
                if (base.Context.Instance is Chart)
                {
                    instance = (Chart) base.Context.Instance;
                }
                else if (base.Context.Instance is TChart)
                {
                    instance = ((TChart) base.Context.Instance).Chart;
                }
                if (instance != null)
                {
                    return ToolsGallery.CreateNew(instance, base.Context.Container);
                }
                return null;
            }
        }
    }
}

