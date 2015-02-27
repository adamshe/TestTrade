namespace iTrading.IB
{
    using System;

    internal class AutoOpenOrdersRequest : Request
    {
        private bool autoBind;

        internal AutoOpenOrdersRequest(Adapter adapter, bool autoBind) : base(adapter.connection)
        {
            this.autoBind = false;
            this.autoBind = autoBind;
        }

        internal override void Send(Adapter adapter)
        {
            adapter.ibSocket.Send(15);
            adapter.ibSocket.Send(1);
            adapter.ibSocket.Send(this.autoBind ? 1 : 0);
        }
    }
}

