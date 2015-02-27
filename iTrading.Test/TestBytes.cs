using iTrading.Core.Data;
using iTrading.Core.Kernel;

namespace iTrading.Test
{

    internal class TestBytes : Bytes
    {
        internal TestBytes(Connection connection) : base(connection)
        {
        }

        /// <summary>
        /// Convert <see cref="T:iTrading.Core.Kernel.Order" /> to id.
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        protected override int ToId(iTrading.Core.Kernel.Order order)
        {
            return -1;
        }

        /// <summary>
        /// Convert <see cref="T:iTrading.Core.Kernel.Request" /> to id.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected override int ToId(Request request)
        {
            return -1;
        }

        /// <summary>
        /// Convert <see cref="T:iTrading.Core.Kernel.Request" /> to id.
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        protected override int ToId(Symbol symbol)
        {
            return -1;
        }

        /// <summary>
        /// Convert id to <see cref="T:iTrading.Core.Kernel.Order" />.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected override iTrading.Core.Kernel.Order ToOrder(int id)
        {
            return null;
        }

        /// <summary>
        /// Convert id to <see cref="T:iTrading.Core.Kernel.Request" />.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected override Request ToRequest(int id)
        {
            return null;
        }

        /// <summary>
        /// Convert id to <see cref="T:iTrading.Core.Kernel.Request" />.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected override Symbol ToSymbol(int id)
        {
            return null;
        }
    }
}

