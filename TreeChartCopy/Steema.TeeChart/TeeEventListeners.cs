namespace Steema.TeeChart
{
    using System;
    using System.Collections;
    using System.Reflection;

    public class TeeEventListeners : ArrayList
    {
        public ITeeEventListener this[int index]
        {
            get
            {
                return (ITeeEventListener) base[index];
            }
            set
            {
                base[index] = value;
            }
        }
    }
}

