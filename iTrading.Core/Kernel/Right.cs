namespace iTrading.Core.Kernel
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Represents an option right.
    /// </summary>
    [Guid("C9837C9F-C5A8-463d-B619-25D992FDEBCB"), ClassInterface(ClassInterfaceType.None)]
    public class Right : IComRight
    {
        private static RightDictionary all = null;
        private RightId id;

        internal Right(RightId id)
        {
            this.id = id;
        }

        /// <summary>
        /// Get a collection of all available option right values.
        /// </summary>
        public static RightDictionary All
        {
            get
            {
                lock (typeof(Globals))
                {
                    if (all == null)
                    {
                        all = new RightDictionary();
                        all.Add(new Right(RightId.Call));
                        all.Add(new Right(RightId.Put));
                        all.Add(new Right(RightId.Unknown));
                    }
                }
                return all;
            }
        }

        /// <summary>
        /// The TradeMagic id of the option right.
        /// </summary>
        public RightId Id
        {
            get
            {
                return this.id;
            }
        }

        /// <summary>
        /// The name of the option right.
        /// </summary>
        public string Name
        {
            get
            {
                switch (this.id)
                {
                    case RightId.Call:
                        return "Call";

                    case RightId.Put:
                        return "Put";

                    case RightId.Unknown:
                        return "Unknown";
                }
                return "Unknown";
            }
        }
    }
}

