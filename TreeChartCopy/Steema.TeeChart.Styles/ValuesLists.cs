namespace Steema.TeeChart.Styles
{
    using System;
    using System.Collections;
    using System.Reflection;

    [Serializable]
    public sealed class ValuesLists : ArrayList
    {
        public Steema.TeeChart.Styles.ValueList this[int i]
        {
            get
            {
                return (Steema.TeeChart.Styles.ValueList) base[i];
            }
        }
    }
}

