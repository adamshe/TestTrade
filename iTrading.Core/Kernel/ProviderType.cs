namespace iTrading.Core.Kernel
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Represents a provider type.
    /// </summary>
    [Guid("6C132056-DE91-43fe-9F99-1DC4BF007139"), ClassInterface(ClassInterfaceType.None)]
    public class ProviderType : IComProviderType
    {
        private static ProviderTypeDictionary all = null;
        private ProviderTypeId id;

        internal ProviderType(ProviderTypeId id)
        {
            this.id = id;
        }

        /// <summary>
        /// Get a collection of all available provider types.
        /// </summary>
        public static ProviderTypeDictionary All
        {
            get
            {
                lock (typeof(Globals))
                {
                    if (all == null)
                    {
                        all = new ProviderTypeDictionary();
                        all.Add(new ProviderType(ProviderTypeId.CyberTrader));
                        all.Add(new ProviderType(ProviderTypeId.Dtn));
                        all.Add(new ProviderType(ProviderTypeId.ESignal));
                        all.Add(new ProviderType(ProviderTypeId.InteractiveBrokers));
                        all.Add(new ProviderType(ProviderTypeId.MBTrading));
                        all.Add(new ProviderType(ProviderTypeId.Patsystems));
                        all.Add(new ProviderType(ProviderTypeId.Simulation));
                        all.Add(new ProviderType(ProviderTypeId.TrackData));
                        all.Add(new ProviderType(ProviderTypeId.Yahoo));
                    }
                }
                return all;
            }
        }

        /// <summary>
        /// The TradeMagic id of the ProviderType.
        /// </summary>
        public ProviderTypeId Id
        {
            get
            {
                return this.id;
            }
        }

        /// <summary>
        /// The name of the ProviderType.
        /// </summary>
        public string Name
        {
            get
            {
                switch (this.id)
                {
                    case ProviderTypeId.InteractiveBrokers:
                        return "Interactive Brokers";

                    case ProviderTypeId.MBTrading:
                        return "MB Trading";

                    case ProviderTypeId.Patsystems:
                        return "Patsystems";

                    case ProviderTypeId.Simulation:
                        return "Simulation";

                    case ProviderTypeId.TrackData:
                        return "TrackData";

                    case ProviderTypeId.Dtn:
                        return "DTN";

                    case ProviderTypeId.ESignal:
                        return "eSignal";

                    case ProviderTypeId.Yahoo:
                        return "YAHOO";

                    case ProviderTypeId.CyberTrader:
                        return "CyberTrader";
                }
                return "Interactive Brokers";
            }
        }
    }
}

