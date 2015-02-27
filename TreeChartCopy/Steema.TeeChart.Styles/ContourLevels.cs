namespace Steema.TeeChart.Styles
{
    using System;
    using System.Collections;
    using System.Reflection;

    public class ContourLevels : ArrayList
    {
        public ContourLevel this[int index]
        {
            get
            {
                return (ContourLevel) base[index];
            }
            set
            {
                base[index] = value;
            }
        }
    }
}

