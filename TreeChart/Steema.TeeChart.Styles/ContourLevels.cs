namespace Steema.TeeChart.Styles
{
    using System;
    using System.Collections;
    using System.Reflection;

    public class ContourLevels : ArrayList
    {
        public new ContourLevel this[int index]
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

