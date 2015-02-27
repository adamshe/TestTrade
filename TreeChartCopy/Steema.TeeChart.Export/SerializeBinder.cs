namespace Steema.TeeChart.Export
{
    using System;
    using System.Runtime.Serialization;

    public class SerializeBinder : SerializationBinder
    {
        public override Type BindToType(string assemblyName, string typeName)
        {
            return Type.GetType(typeName);
        }
    }
}

