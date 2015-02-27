namespace iTrading.Test
{
    using NUnit.Framework;
    using System;
    using System.Collections;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Executes the complete test set for release.
    /// </summary>
    [TestFixture]
    public class Release : TestBase
    {
        /// <summary>
        /// Creates an instance of this class.
        /// </summary>
        /// <returns></returns>
        protected override TestBase CreateInstance()
        {
            return new Release();
        }

        /// <summary>
        /// Nothing to do here.
        /// </summary>
        protected override void DoTest()
        {
        }

        /// <summary>
        /// Run all tests (<see cref="M: iTrading.Test.TestBase.Multiple" />) as random sequence.
        /// </summary>
        [Test]
        public override void Multiple()
        {
            Random random = new Random(((int) DateTime.Now.Ticks) + base.Connection.ClientId);
            ArrayList tests = this.Tests;
            while (tests.Count > 0)
            {
                TestBase base2 = (TestBase) tests[random.Next(tests.Count - 1)];
                tests.Remove(base2);
                base2.Broker = base.Broker;
                base2.Environment = base.Environment;
                base2.Mode = base.Mode;
                if (((base2.Broker == null) || (base2.Broker.Id == ProviderTypeId.InteractiveBrokers)) || !(base2 is IBConnect))
                {
                    base2.Multiple();
                }
            }
        }

        /// <summary>
        /// Execute test sequence <see cref="M: iTrading.Test.TestBase.Multiple" /> in an infinite loop. 
        /// Please note: This method will not terminate, unless an error occurs.
        /// </summary>
        [Test]
        public override void MultipleInfinite()
        {
            while (true)
            {
                this.Multiple();
            }
        }

        /// <summary>
        /// Run all tests (<see cref="M: iTrading.Test.TestBase.Single" />) as random sequence.
        /// </summary>
        [Test]
        public override void Single()
        {
            Random random = new Random(((int) DateTime.Now.Ticks) + base.Connection.ClientId);
            ArrayList tests = this.Tests;
            while (tests.Count > 0)
            {
                TestBase base2 = (TestBase) tests[random.Next(tests.Count - 1)];
                tests.Remove(base2);
                base2.Broker = base.Broker;
                base2.Environment = base.Environment;
                base2.Mode = base.Mode;
                if (((base2.Broker == null) || (base2.Broker.Id == ProviderTypeId.InteractiveBrokers)) || !(base2 is IBConnect))
                {
                    base2.Single();
                }
            }
        }

        /// <summary>
        /// Execute test sequence <see cref="M: iTrading.Test.TestBase.Single" /> in an infinite loop. 
        /// Please note: This method will not terminate, unless an error occurs.
        /// </summary>
        [Test]
        public override void SingleInfinite()
        {
            while (true)
            {
                this.Single();
            }
        }

        private ArrayList Tests
        {
            get
            {
                ArrayList list = new ArrayList();
                list.Add(new Connect());
                list.Add(new DataTest());
                list.Add(new Quotes());
                list.Add(new  iTrading.Test.Order());
                return list;
            }
        }
    }
}

