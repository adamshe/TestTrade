namespace Steema.TeeChart.Export
{
    using Steema.TeeChart;
    using Steema.TeeChart.Data;
    using Steema.TeeChart.Functions;
    using Steema.TeeChart.Styles;
    using Steema.TeeChart.Tools;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters;
    using System.Runtime.Serialization.Formatters.Binary;

    public sealed class TemplateExport : ExportFormat
    {
        internal Chart chart;
        private bool includeData = true;

        public TemplateExport(Chart c)
        {
            this.chart = c;
            base.FileExtension = Texts.TeeFilesExtension;
        }

        internal static string FileFilter()
        {
            return (Texts.TeeFiles + " (" + Texts.TeeFilesExtension + ")|*." + Texts.TeeFilesExtension);
        }

        internal override string FilterFiles()
        {
            return (Texts.TeeFiles + " (" + Texts.TeeFilesExtension + ")|*." + Texts.TeeFilesExtension);
        }

        public void Save(Stream stream)
        {
            this.Serialize(stream);
        }

        public void Save(string FileName)
        {
            using (FileStream stream = new FileStream(FileName, FileMode.Create))
            {
                this.Save(stream);
                stream.Flush();
                stream.Close();
            }
        }

        public void Serialize(Stream stream)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.AssemblyFormat = FormatterAssemblyStyle.Simple;
            formatter.Serialize(stream, this.chart);
        }

        public void Serialize(SerializationInfo info, StreamingContext context)
        {
            this.SerializeObject("", this.chart, info);
        }

        private void SerializeObject(string Prefix, object value, SerializationInfo info)
        {
            if (value != null)
            {
                if (value is CollectionBase)
                {
                    CollectionBase base2 = (CollectionBase) value;
                    Prefix = Prefix.Remove(0, 1);
                    int num = 0;
                    foreach (object obj2 in base2)
                    {
                        string name = Prefix + "." + num.ToString();
                        info.AddValue(name, obj2.GetType().ToString());
                        this.SerializeObject("." + name, obj2, info);
                        num++;
                    }
                }
                else if (value is Array)
                {
                    info.AddValue(Prefix, value);
                }
                else if (value is ArrayList)
                {
                    if (this.IncludeData)
                    {
                        Array array = ((ArrayList) value).ToArray();
                        info.AddValue(Prefix, array);
                    }
                }
                else if (value is Image)
                {
                    info.AddValue(Prefix, value);
                }
                else
                {
                    if ((value is Steema.TeeChart.Styles.ValueList) && !this.IncludeData)
                    {
                        return;
                    }
                    if (value is Steema.TeeChart.Styles.ValueList)
                    {
                        Steema.TeeChart.Styles.ValueList list = (Steema.TeeChart.Styles.ValueList) value;
                        list.Trim();
                        info.AddValue(Prefix + ".Value", list.Value);
                        info.AddValue(Prefix + ".Count", list.Count);
                    }
                    foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(value))
                    {
                        this.SerializeProperty(Prefix, value, descriptor, info);
                    }
                }
                if (value is ICustomSerialization)
                {
                    (value as ICustomSerialization).Serialize(info);
                }
            }
        }

        private void SerializeProperty(string Title, object value, PropertyDescriptor i, SerializationInfo info)
        {
            if (i.ShouldSerializeValue(value) && (i.SerializationVisibility != DesignerSerializationVisibility.Hidden))
            {
                string name = Title + "." + i.DisplayName;
                if (i.PropertyType == typeof(string))
                {
                    object obj2 = i.GetValue(value);
                    info.AddValue(name, obj2, obj2.GetType());
                }
                else if (i.PropertyType.IsClass)
                {
                    object obj3 = i.GetValue(value);
                    if (obj3 is Series)
                    {
                        info.AddValue(name, "Series." + this.chart.series.IndexOf((Series) obj3));
                    }
                    else if ((value is Tool) && (obj3 is Axis))
                    {
                        info.AddValue(name, "Axis." + this.chart.axes.IndexOf((Axis) obj3));
                    }
                    else if ((value is Series) && (obj3 is Axis))
                    {
                        info.AddValue(name, "CustomAxes." + this.chart.axes.IndexOf((Axis) obj3));
                    }
                    else if (obj3 is Function)
                    {
                        info.AddValue(name, obj3.GetType().ToString());
                        this.SerializeObject(name, i.GetValue(value), info);
                    }
                    else if ((obj3 is object[]) && (i.Name == "DataSource"))
                    {
                        string[] strArray = new string[((object[]) obj3).Length];
                        Array array = (Array) obj3;
                        int index = 0;
                        foreach (object obj4 in array)
                        {
                            strArray.SetValue("Series." + this.chart.Series.IndexOf((Series) obj4).ToString(), index);
                            index++;
                        }
                        info.AddValue(name, strArray);
                    }
                    else if (!(obj3 is SeriesSource))
                    {
                        this.SerializeObject(name, i.GetValue(value), info);
                    }
                }
                else
                {
                    object obj5 = i.GetValue(value);
                    info.AddValue(name, obj5, obj5.GetType());
                }
            }
        }

        public bool IncludeData
        {
            get
            {
                return this.includeData;
            }
            set
            {
                if (this.includeData != value)
                {
                    this.includeData = value;
                }
            }
        }

        public interface ICustomSerialization
        {
            void DeSerialize(SerializationInfo info);
            void Serialize(SerializationInfo info);
        }
    }
}

