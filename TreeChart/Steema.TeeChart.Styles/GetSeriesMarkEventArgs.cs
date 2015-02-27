namespace Steema.TeeChart.Styles
{
    using System;

    public class GetSeriesMarkEventArgs : EventArgs
    {
        private string mText;
        private readonly int vIndex;

        public GetSeriesMarkEventArgs(int valueIndex, string markText)
        {
            this.vIndex = valueIndex;
            this.mText = markText;
        }

        public string MarkText
        {
            get
            {
                return this.mText;
            }
            set
            {
                this.mText = value;
            }
        }

        public int ValueIndex
        {
            get
            {
                return this.vIndex;
            }
        }
    }
}

