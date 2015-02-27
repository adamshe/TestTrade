namespace iTrading.IB
{
    using System;
    using iTrading.Core.Kernel;

    internal abstract class Request
    {
        private iTrading.Core.Kernel.Request _TMRequest;

        protected Request(iTrading.Core.Kernel.Request Request)
        {
            this._TMRequest = Request;
            this._TMRequest.AdapterLink = this;
        }

        internal abstract void Send(Adapter Adapter);

        internal iTrading.Core.Kernel.Connection Connection
        {
            get
            {
                return this.TMRequest.Connection;
            }
        }

        internal iTrading.Core.Kernel.Request TMRequest
        {
            get
            {
                return this._TMRequest;
            }
        }
    }
}

