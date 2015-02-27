using iTrading.Core.Kernel;
using System;
using TradeMagic.Client;

namespace iTrading.Client
{
    [Serializable]
    public class Loader : ILoader
    {
        public IAdapter Create(Connection connection)
        {
            return new Adapter(connection);
        }
    }
}