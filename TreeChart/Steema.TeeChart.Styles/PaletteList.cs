namespace Steema.TeeChart.Styles
{
    using System;
    using System.Collections;
    using System.Reflection;

    public class PaletteList : ArrayList
    {
        public new GridPalette this[int index]
        {
            get
            {
                return (GridPalette) base[index];
            }
            set
            {
                base[index] = value;
            }
        }
    }
}

