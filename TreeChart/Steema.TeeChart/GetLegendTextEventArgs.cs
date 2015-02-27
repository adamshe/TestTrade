namespace Steema.TeeChart
{
    using System;
    using System.ComponentModel;

    public class GetLegendTextEventArgs : EventArgs
    {
        private readonly int idx;
        private readonly LegendStyles lStyle;
        private string text;

        public GetLegendTextEventArgs(LegendStyles legendStyle, int index, string text)
        {
            this.lStyle = legendStyle;
            this.idx = index;
            this.text = text;
        }

        public int Index
        {
            get
            {
                return this.idx;
            }
        }

        public LegendStyles LegendStyle
        {
            get
            {
                return this.lStyle;
            }
        }

        [Description("Sets the Text for the Legend values.")]
        public string Text
        {
            get
            {
                return this.text;
            }
            set
            {
                this.text = value;
            }
        }
    }
}

