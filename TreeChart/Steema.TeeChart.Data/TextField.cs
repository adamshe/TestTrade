namespace Steema.TeeChart.Data
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Design.Serialization;
    using System.Globalization;

    [Serializable, ToolboxItem(false), TypeConverter(typeof(Converter))]
    public sealed class TextField
    {
        [NonSerialized]
        internal object data;
        private int index;
        private string name;

        public TextField()
        {
            this.index = -1;
            this.name = "";
        }

        public TextField(int index, string name) : this()
        {
            this.index = index;
            this.name = name;
        }

        public int Index
        {
            get
            {
                return this.index;
            }
            set
            {
                this.index = value;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        internal class Converter : TypeConverter
        {
            public override bool CanConvertTo(ITypeDescriptorContext context, Type destType)
            {
                return ((destType == typeof(InstanceDescriptor)) || base.CanConvertTo(context, destType));
            }

            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                if (destType == typeof(InstanceDescriptor))
                {
                    TextField field = (TextField) value;
                    return new InstanceDescriptor(typeof(TextField).GetConstructor(new Type[] { typeof(int), typeof(string) }), new object[] { field.Index, field.Name }, true);
                }
                return base.ConvertTo(context, culture, value, destType);
            }
        }
    }
}

