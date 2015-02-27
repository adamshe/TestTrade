namespace Steema.TeeChart.Functions
{
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;

    public class CompressGetDateEventArgs : EventArgs
    {
        internal DateTime date;
        internal Series source;
        internal int valueIndex;

        public CompressGetDateEventArgs(Series source, int valueIndex, DateTime date)
        {
            this.source = source;
            this.valueIndex = valueIndex;
            this.date = date;
        }

        [Description("The CompressOHLC Function ValueIndex Date.")]
        public DateTime Date
        {
            get
            {
                return this.date;
            }
        }

        [Description("The CompressOHLC Function Source series.")]
        public Series Source
        {
            get
            {
                return this.source;
            }
        }

        [Description("The CompressOHLC Function ValueIndex.")]
        public int ValueIndex
        {
            get
            {
                return this.valueIndex;
            }
        }
    }
}

