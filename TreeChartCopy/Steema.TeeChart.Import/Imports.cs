namespace Steema.TeeChart.Import
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Export;
    using Steema.TeeChart.Functions;
    using Steema.TeeChart.Styles;
    using Steema.TeeChart.Tools;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Reflection;
    using System.Runtime.Serialization;

    public class Imports
    {
        protected Chart chart;
        private TemplateImport template;

        public Imports()
        {
        }

        public Imports(Chart c)
        {
            this.chart = c;
        }

        public virtual void DeserializeFrom(SerializationInfo info, StreamingContext context)
        {
            SerializationInfoEnumerator enumerator = info.GetEnumerator();
            while (enumerator.MoveNext())
            {
                SerializationEntry current = enumerator.Current;
                string name = current.Name;
                if (!name.StartsWith("."))
                {
                    int index = name.IndexOf(".");
                    if (index >= 0)
                    {
                        string str2 = name.Substring(0, index);
                        name = name.Remove(0, index);
                        if (TypeDescriptor.GetProperties(this.chart).Find(str2, true) != null)
                        {
                            string typeName = current.Value.ToString();
                            Type type = this.FindType(typeName);
                            object[] args = new object[] { this.chart };
                            object o = Activator.CreateInstance(type, args);
                            if (type == typeof(Axis))
                            {
                                this.chart.Axes.Custom.Add((Axis) o);
                            }
                            this.TryCustomSerialization(o, info);
                        }
                    }
                }
            }
            enumerator = info.GetEnumerator();
            while (enumerator.MoveNext())
            {
                SerializationEntry entry2 = enumerator.Current;
                bool flag = true;
                string str4 = entry2.Name;
                if (str4.StartsWith("."))
                {
                    str4 = str4.Remove(0, 1);
                    object chart = this.chart;
                    do
                    {
                        string str5;
                        PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(chart);
                        int length = str4.IndexOf(".");
                        if (length >= 0)
                        {
                            str5 = str4.Substring(0, length);
                            PropertyDescriptor descriptor2 = properties.Find(str5, true);
                            if (descriptor2 == null)
                            {
                                if (chart is SeriesCollection)
                                {
                                    chart = ((SeriesCollection) chart)[Convert.ToInt32(str5)];
                                }
                                else if (chart is CustomAxes)
                                {
                                    chart = ((CustomAxes) chart)[Convert.ToInt32(str5)];
                                }
                                else if (chart is ToolsCollection)
                                {
                                    chart = ((ToolsCollection) chart)[Convert.ToInt32(str5)];
                                }
                            }
                            else
                            {
                                chart = descriptor2.GetValue(chart);
                            }
                            str4 = str4.Remove(0, length + 1);
                            if (chart == null)
                            {
                                flag = false;
                            }
                        }
                        else
                        {
                            str5 = str4;
                            PropertyDescriptor descriptor3 = properties.Find(str5, true);
                            if (descriptor3 != null)
                            {
                                if (descriptor3.PropertyType == typeof(object))
                                {
                                    if (entry2.Value is Array)
                                    {
                                        object[] objArray2 = new object[((Array) entry2.Value).Length];
                                        int num3 = 0;
                                        foreach (object obj4 in (Array) entry2.Value)
                                        {
                                            objArray2.SetValue(this.GetLinkObject(obj4.ToString()), num3);
                                            num3++;
                                        }
                                        descriptor3.SetValue(chart, objArray2);
                                    }
                                    else if (entry2.Value.GetType() == typeof(string))
                                    {
                                        descriptor3.SetValue(chart, this.GetLinkObject(entry2.Value.ToString()));
                                    }
                                }
                                else if (descriptor3.PropertyType == typeof(Axis))
                                {
                                    if (entry2.Value.GetType() == typeof(string))
                                    {
                                        descriptor3.SetValue(chart, this.GetLinkObject(entry2.Value.ToString()));
                                    }
                                }
                                else if (descriptor3.PropertyType == typeof(Series))
                                {
                                    if (entry2.Value.GetType() == typeof(string))
                                    {
                                        descriptor3.SetValue(chart, this.GetLinkObject(entry2.Value.ToString()));
                                    }
                                }
                                else if (descriptor3.PropertyType == typeof(Tool))
                                {
                                    if (entry2.Value.GetType() == typeof(string))
                                    {
                                        descriptor3.SetValue(chart, this.GetLinkObject(entry2.Value.ToString()));
                                    }
                                }
                                else if (descriptor3.PropertyType == typeof(StringList))
                                {
                                    StringList list = new StringList(((Array) entry2.Value).Length);
                                    list.AddRange((Array) entry2.Value);
                                    descriptor3.SetValue(chart, list);
                                }
                                else if (descriptor3.PropertyType == typeof(ColorList))
                                {
                                    ColorList list2 = new ColorList(((Array) entry2.Value).Length);
                                    list2.AddRange((Array) entry2.Value);
                                    descriptor3.SetValue(chart, list2);
                                }
                                else if (descriptor3.PropertyType == typeof(ArrayList))
                                {
                                    ArrayList list3 = new ArrayList(((Array) entry2.Value).Length);
                                    list3.AddRange((Array) entry2.Value);
                                    descriptor3.SetValue(chart, list3);
                                }
                                else if (descriptor3.PropertyType == typeof(Image))
                                {
                                    descriptor3.SetValue(chart, entry2.Value);
                                }
                                else if (descriptor3.PropertyType == entry2.ObjectType)
                                {
                                    descriptor3.SetValue(chart, entry2.Value);
                                }
                                else if (descriptor3.PropertyType == typeof(Function))
                                {
                                    Type f = Type.GetType(entry2.Value.ToString(), true);
                                    descriptor3.SetValue(chart, Function.NewInstance(f));
                                }
                                else
                                {
                                    descriptor3.SetValue(chart, Convert.ChangeType(entry2.Value, descriptor3.PropertyType));
                                }
                            }
                            else if (chart is Steema.TeeChart.Styles.ValueList)
                            {
                                ((Steema.TeeChart.Styles.ValueList) chart).Deserialize(str5, entry2.Value);
                            }
                            flag = false;
                        }
                    }
                    while (flag);
                }
            }
            if (this.chart is IChart)
            {
                this.TryCustomSerialization((this.chart as IChart).GetControl(), info);
            }
        }

        private Type FindType(string typeName)
        {
            Type type = this.InternalFindType(typeName, base.GetType().Assembly);
            if (type == null)
            {
                type = this.InternalFindType(typeName, Assembly.GetExecutingAssembly());
            }
            if (type == null)
            {
                type = this.InternalFindType(typeName, Assembly.GetEntryAssembly());
            }
            if (type != null)
            {
                return type;
            }
            foreach (AssemblyName name in Assembly.GetExecutingAssembly().GetReferencedAssemblies())
            {
                Type type2 = this.InternalFindType(typeName, Assembly.Load(name));
                if (type2 != null)
                {
                    return type2;
                }
            }
            return null;
        }

        private object GetLinkObject(string s)
        {
            int index = s.IndexOf(".");
            if (index >= 0)
            {
                string str = s.Substring(0, index).ToUpper();
                int num2 = Convert.ToInt32(s.Remove(0, index + 1));
                switch (str)
                {
                    case "SERIES":
                        return this.chart.series[num2];

                    case "AXIS":
                        return this.chart.axes[num2];

                    case "CUSTOMAXES":
                        return this.chart.axes.custom[num2];
                }
            }
            return null;
        }

        private Type InternalFindType(string typeName, Assembly a)
        {
            foreach (Type type in a.GetExportedTypes())
            {
                if (type.FullName == typeName)
                {
                    return type;
                }
            }
            return null;
        }

        public void ShowImportDialog()
        {
            ImportEditor.ShowModal(this.chart);
        }

        private void testMethod(ref Type t)
        {
            t = base.GetType();
        }

        private void TryCustomSerialization(object o, SerializationInfo info)
        {
            if (o is TemplateExport.ICustomSerialization)
            {
                (o as TemplateExport.ICustomSerialization).DeSerialize(info);
            }
        }

        public TemplateImport Template
        {
            get
            {
                if (this.template == null)
                {
                    this.template = new TemplateImport(this.chart);
                }
                return this.template;
            }
        }
    }
}

