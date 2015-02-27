namespace Steema.TeeChart.Styles
{
    using System;
    using System.Collections;
    using System.Reflection;

    public sealed class StringList : ArrayList
    {
        public StringList(int capacity) : base(capacity)
        {
        }

        internal void Exchange(int a, int b)
        {
            string str = this[a];
            this[a] = this[b];
            this[b] = str;
        }

        public string this[int index]
        {
            get
            {
                if (index >= this.Count)
                {
                    return "";
                }
                return base[index].ToString();
            }
            set
            {
                while (this.Count <= index)
                {
                    this.Add("");
                }
                base[index] = value;
            }
        }
    }
}

