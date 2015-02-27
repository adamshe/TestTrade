namespace Steema.TeeChart
{
    using System;

    public class GetNextAxisLabelEventArgs : EventArgs
    {
        private readonly int lIndex;
        private double lValue;
        private bool stop;

        public GetNextAxisLabelEventArgs(int labelIndex, double labelValue, bool doStop)
        {
            this.lIndex = labelIndex;
            this.lValue = labelValue;
            this.stop = doStop;
        }

        public int LabelIndex
        {
            get
            {
                return this.lIndex;
            }
        }

        public double LabelValue
        {
            get
            {
                return this.lValue;
            }
            set
            {
                this.lValue = value;
            }
        }

        public bool Stop
        {
            get
            {
                return this.stop;
            }
            set
            {
                this.stop = value;
            }
        }
    }
}

