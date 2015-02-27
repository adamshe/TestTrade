namespace iTrading.Core.Kernel
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Represents time in force. Please note, that the pool of available time in force values (see <see cref="P:iTrading.Core.Kernel.Connection.TimeInForce" />)
    /// varies between brokers/data providers.
    /// </summary>
    [ClassInterface(ClassInterfaceType.None), Guid("0ED38F45-CE17-445a-B6FC-B06EBB31ADD8")]
    public class TimeInForce : IComTimeInForce
    {
        private static TimeInForceDictionary all = null;
        private TimeInForceId id;
        private string mapId;

        internal TimeInForce(TimeInForceId id, string mapId)
        {
            this.id = id;
            this.mapId = mapId;
        }

        /// <summary>
        /// Get a collection of all available action item types.
        /// See <see cref="P:iTrading.Core.Kernel.Connection.TimeInForce" /> for a collection of <see cref="T:iTrading.Core.Kernel.TimeInForce" /> objects supported
        /// by the current broker.
        /// </summary>
        public static TimeInForceDictionary All
        {
            get
            {
                lock (typeof(Globals))
                {
                    if (all == null)
                    {
                        all = new TimeInForceDictionary();
                        all.Add(new TimeInForce(TimeInForceId.Day, ""));
                        all.Add(new TimeInForce(TimeInForceId.Gtc, ""));
                    }
                }
                return all;
            }
        }

        /// <summary>
        /// The TradeMagic id.
        /// </summary>
        public TimeInForceId Id
        {
            get
            {
                return this.id;
            }
        }

        /// <summary>
        /// The broker dependent id.
        /// </summary>
        public string MapId
        {
            get
            {
                return this.mapId;
            }
        }

        /// <summary>
        /// </summary>
        public string Name
        {
            get
            {
                switch (this.id)
                {
                    case TimeInForceId.Day:
                        return "Day";

                    case TimeInForceId.Gtc:
                        return "Gtc";
                }
                return "Gtc";
            }
        }
    }
}

