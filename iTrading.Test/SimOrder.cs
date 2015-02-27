using iTrading.Core.Kernel;

namespace iTrading.Test
{
    using System;

    /// <summary>
    /// Test simulated order placement. Orders are executed only on the simulation account.
    /// </summary>
    public class SimOrder : iTrading.Test.Order
    {
        /// <summary>
        /// Creates an instance of this class.
        /// </summary>
        /// <returns></returns>
        protected override TestBase CreateInstance()
        {
            return new SimOrder();
        }

        /// <summary>
        /// Execute test.
        /// </summary>
        protected override void DoTest()
        {
            Account account = null;
            lock (base.Connection.Accounts)
            {
                foreach (Account account2 in base.Connection.Accounts)
                {
                    if (account2.IsSimulation)
                    {
                        account = account2;
                    }
                }
            }
             iTrading.Test.Globals.Assert(base.GetType().FullName + " 000", account != null);
            base.Account = account;
            base.DoTest();
        }
    }
}

