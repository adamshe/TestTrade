namespace iTrading.Yahoo
{
    using System;
    using iTrading.Core.Kernel;

    [Serializable]
    public class Loader : ILoader
    {
        public IAdapter Create(Connection connection)
        {
            return new Adapter(connection);
        }
    }
}

