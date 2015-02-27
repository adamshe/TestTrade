namespace Steema.TeeChart
{
    using System;
    using System.ComponentModel;

    public class SerializeEventArgs : EventArgs
    {
        private System.Type type;
        private string typeName;

        public SerializeEventArgs(string tName, System.Type t)
        {
            this.type = t;
            this.typeName = tName;
        }

        [Description("Sets Type to add to Serialization.")]
        public System.Type Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
            }
        }

        [Description("Name of Type to Serialize.")]
        public string TypeName
        {
            get
            {
                return this.typeName;
            }
        }
    }
}

