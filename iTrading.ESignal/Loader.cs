using System;
using iTrading.Core.Kernel;

namespace iTrading.ESignal
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

