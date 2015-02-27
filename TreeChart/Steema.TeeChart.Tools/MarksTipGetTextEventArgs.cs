namespace Steema.TeeChart.Tools
{
    using System;

    public class MarksTipGetTextEventArgs : EventArgs
    {
        private string text;

        public MarksTipGetTextEventArgs(string text)
        {
            this.text = text;
        }

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

