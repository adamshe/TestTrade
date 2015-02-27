namespace iTrading.Core.Kernel
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Represents an Tracelevel type.
    /// </summary>
    [Guid("8A0360F3-4607-4ea4-BB8C-A7307E52D576"), ClassInterface(ClassInterfaceType.None)]
    public class TraceLevel : IComTraceLevel
    {
        private TraceLevelIds id;
        private static TraceLevelDictionary traceLevels = null;

        internal TraceLevel(TraceLevelIds id)
        {
            this.id = id;
        }

        /// <summary>
        /// Get a collection of all available trace levels.
        /// </summary>
        public static TraceLevelDictionary All
        {
            get
            {
                lock (typeof(Globals))
                {
                    if (traceLevels == null)
                    {
                        traceLevels = new TraceLevelDictionary();
                        traceLevels.Add(new TraceLevel(TraceLevelIds.All));
                        traceLevels.Add(new TraceLevel(TraceLevelIds.Com));
                        traceLevels.Add(new TraceLevel(TraceLevelIds.Connect));
                        traceLevels.Add(new TraceLevel(TraceLevelIds.ComClient));
                        traceLevels.Add(new TraceLevel(TraceLevelIds.DataBase));
                        traceLevels.Add(new TraceLevel(TraceLevelIds.Indicator));
                        traceLevels.Add(new TraceLevel(TraceLevelIds.MarketData));
                        traceLevels.Add(new TraceLevel(TraceLevelIds.MarketDepth));
                        traceLevels.Add(new TraceLevel(TraceLevelIds.Native));
                        traceLevels.Add(new TraceLevel(TraceLevelIds.News));
                        traceLevels.Add(new TraceLevel(TraceLevelIds.None));
                        traceLevels.Add(new TraceLevel(TraceLevelIds.Order));
                        traceLevels.Add(new TraceLevel(TraceLevelIds.Quotes));
                        traceLevels.Add(new TraceLevel(TraceLevelIds.Strict));
                        traceLevels.Add(new TraceLevel(TraceLevelIds.SymbolLookup));
                        traceLevels.Add(new TraceLevel(TraceLevelIds.Test));
                        traceLevels.Add(new TraceLevel(TraceLevelIds.Timer));
                        traceLevels.Add(new TraceLevel(TraceLevelIds.Types));
                    }
                }
                return traceLevels;
            }
        }

        /// <summary>
        /// The id of the TraceLevel.
        /// </summary>
        public TraceLevelIds Id
        {
            get
            {
                return this.id;
            }
        }

        /// <summary>
        /// The name of the TraceLevel.
        /// </summary>
        public string Name
        {
            get
            {
                switch (this.id)
                {
                    case TraceLevelIds.None:
                        return "None";

                    case TraceLevelIds.Com:
                        return "COM";

                    case TraceLevelIds.Connect:
                        return "Connect";

                    case TraceLevelIds.ComClient:
                        return "COMClient";

                    case TraceLevelIds.DataBase:
                        return "DataBase";

                    case TraceLevelIds.MarketData:
                        return "MarketData";

                    case TraceLevelIds.MarketDepth:
                        return "MarketDepth";

                    case TraceLevelIds.Order:
                        return "Order";

                    case TraceLevelIds.SymbolLookup:
                        return "SymbolLookup";

                    case TraceLevelIds.Native:
                        return "Native";

                    case TraceLevelIds.News:
                        return "News";

                    case TraceLevelIds.Test:
                        return "Test";

                    case TraceLevelIds.Types:
                        return "Types";

                    case TraceLevelIds.Quotes:
                        return "Quotes";

                    case TraceLevelIds.Timer:
                        return "Timer";

                    case TraceLevelIds.Indicator:
                        return "Indicator";

                    case TraceLevelIds.Strict:
                        return "Strict";

                    case TraceLevelIds.All:
                        return "All";
                }
                return "None";
            }
        }
    }
}

