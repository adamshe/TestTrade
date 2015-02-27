namespace iTrading.IB
{
    using System;

    internal class SocketClosedException : Exception
    {
        internal SocketClosedException(string msg) : base(msg)
        {
        }
    }
}

