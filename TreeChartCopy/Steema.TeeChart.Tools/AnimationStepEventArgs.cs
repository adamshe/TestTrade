namespace Steema.TeeChart.Tools
{
    using System;

    public class AnimationStepEventArgs : EventArgs
    {
        private readonly int s;

        public AnimationStepEventArgs(int stepNum)
        {
            this.s = stepNum;
        }

        public int Step
        {
            get
            {
                return this.s;
            }
        }
    }
}

