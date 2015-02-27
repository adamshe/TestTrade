namespace iTrading.Test
{
    using System;

    /// <summary>
    /// Test connection establishment.
    /// </summary>
    public class Connect : TestBase
    {
        /// <summary>
        /// Creates an instance of this class.
        /// </summary>
        /// <returns></returns>
        protected override TestBase CreateInstance()
        {
            return new Connect();
        }

        /// <summary>
        /// Nothing to do here.
        /// </summary>
        protected override void DoTest()
        {
            Globals.Sleep(0x7d0);
        }
    }
}

