namespace iTrading.IB
{
    using System;

    internal class OpenOrdersRequest : Request
    {
        internal OpenOrdersRequest(Adapter adapter) : base(adapter.connection)
        {
        }

        internal override void Send(Adapter adapter)
        {
            adapter.ibSocket.Send(5);
            adapter.ibSocket.Send(1);
        }
    }
}

