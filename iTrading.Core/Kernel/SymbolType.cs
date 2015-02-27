namespace iTrading.Core.Kernel
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Represents a symbol type. Please note, that the pool of available symbol types (see <see cref="P:iTrading.Core.Kernel.Connection.SymbolTypes" />)
    /// varies between brokers/data providers.
    /// </summary>
    [Guid("C50331A7-074A-40c4-875B-025136CF56FB"), ClassInterface(ClassInterfaceType.None)]
    public class SymbolType : IComSymbolType
    {
        private static SymbolTypeDictionary all = null;
        private SymbolTypeId id;
        private string mapId;

        internal SymbolType(SymbolTypeId id, string mapId)
        {
            this.id = id;
            this.mapId = mapId;
        }

        /// <summary>
        /// Get a collection of all available action item types.
        /// See <see cref="P:iTrading.Core.Kernel.Connection.SymbolTypes" /> for a collection of <see cref="T:iTrading.Core.Kernel.SymbolType" /> objects supported
        /// by the current broker.
        /// </summary>
        public static SymbolTypeDictionary All
        {
            get
            {
                lock (typeof(Globals))
                {
                    if (all == null)
                    {
                        all = new SymbolTypeDictionary();
                        all.Add(new SymbolType(SymbolTypeId.Future, ""));
                        all.Add(new SymbolType(SymbolTypeId.Index, ""));
                        all.Add(new SymbolType(SymbolTypeId.Option, ""));
                        all.Add(new SymbolType(SymbolTypeId.Stock, ""));
                        all.Add(new SymbolType(SymbolTypeId.Unknown, ""));
                    }
                }
                return all;
            }
        }

        /// <summary>
        /// The TradeMagic id of the SymbolType. This id is independent from the underlying broker system.
        /// </summary>
        public SymbolTypeId Id
        {
            get
            {
                return this.id;
            }
        }

        /// <summary>
        /// The broker dependent id of the SymbolType. 
        /// </summary>
        public string MapId
        {
            get
            {
                return this.mapId;
            }
        }

        /// <summary>
        /// The name of the SymbolType.
        /// </summary>
        public string Name
        {
            get
            {
                switch (this.id)
                {
                    case SymbolTypeId.Future:
                        return "Future";

                    case SymbolTypeId.Stock:
                        return "Stock";

                    case SymbolTypeId.Unknown:
                        return "Unknown";

                    case SymbolTypeId.Index:
                        return "Index";

                    case SymbolTypeId.Option:
                        return "Option";
                }
                return "Any";
            }
        }
    }
}

