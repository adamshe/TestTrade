namespace iTrading.IB
{
    using System;

    internal class NextValidIdRequest : Request
    {
        private readonly int numIds;

        internal NextValidIdRequest(Adapter adapter, int intNumIds) : base(adapter.connection)
        {
            this.numIds = intNumIds;
        }

        internal override void Send(Adapter adapter)
        {
            adapter.ibSocket.Send(8);
            adapter.ibSocket.Send(1);
            adapter.ibSocket.Send(this.numIds);
        }
    }
}

