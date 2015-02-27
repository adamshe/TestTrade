namespace Steema.TeeChart
{
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;

    public class GetAxisLabelEventArgs : EventArgs
    {
        private string lText;
        private readonly Steema.TeeChart.Styles.Series series;
        private readonly int vIndex;

        public GetAxisLabelEventArgs(Steema.TeeChart.Styles.Series s, int valueIndex, string labelText)
        {
            this.series = s;
            this.vIndex = valueIndex;
            this.lText = labelText;
        }

        [Description("Modifies the Axis Label text. ")]
        public string LabelText
        {
            get
            {
                return this.lText;
            }
            set
            {
                this.lText = value;
            }
        }

        [Description("Read only. Label Series")]
        public Steema.TeeChart.Styles.Series Series
        {
            get
            {
                return this.series;
            }
        }

        [Description("Read only. Index of Label")]
        public int ValueIndex
        {
            get
            {
                return this.vIndex;
            }
        }
    }
}

