namespace iTrading.Core.Kernel
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Represents an Currency type.
    /// </summary>
    [ClassInterface(ClassInterfaceType.None), Guid("6C6B53E8-2099-49a5-A0F7-CFD49E252786")]
    public class ModeType : IComModeType
    {
        private static ModeTypeDictionary all = null;
        private ModeTypeId id;

        internal ModeType(ModeTypeId id)
        {
            this.id = id;
        }

        /// <summary>
        /// Get a collection of all available mode types.
        /// </summary>
        public static ModeTypeDictionary All
        {
            get
            {
                lock (typeof(Globals))
                {
                    if (all == null)
                    {
                        all = new ModeTypeDictionary();
                        all.Add(new ModeType(ModeTypeId.Demo));
                        all.Add(new ModeType(ModeTypeId.Live));
                        all.Add(new ModeType(ModeTypeId.Simulation));
                        all.Add(new ModeType(ModeTypeId.Test));
                    }
                }
                return all;
            }
        }

        /// <summary>
        /// The TradeMagic id of the ModeType.
        /// </summary>
        public ModeTypeId Id
        {
            get
            {
                return this.id;
            }
        }

        /// <summary>
        /// The name of the ModeType.
        /// </summary>
        public string Name
        {
            get
            {
                switch (this.id)
                {
                    case ModeTypeId.Demo:
                        return "Demo";

                    case ModeTypeId.Live:
                        return "Live";

                    case ModeTypeId.Simulation:
                        return "Simulation";

                    case ModeTypeId.Test:
                        return "Test";
                }
                return "Demo";
            }
        }
    }
}

