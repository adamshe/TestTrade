namespace iTrading.IB
{
    using System;
    using iTrading.Core.Kernel;

    internal abstract class StreamingRequest : iTrading.IB.Request
    {
        internal StreamingRequest(iTrading.Core .Kernel .Request Request) : base(Request)
        {
        }

        internal void Cancel()
        {
            this.DoCancel();
        }

        protected abstract void DoCancel();
    }
}

