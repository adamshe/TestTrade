namespace iTrading.Core.Kernel
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// TradeMagic trace switch
    /// </summary>
    [ClassInterface(ClassInterfaceType.None), Guid("D89F8AE2-E3FC-4cac-BA42-50993DDE0B14")]
    public class TMTraceSwitch : Switch, IComTMTraceSwitch
    {
        /// <summary>
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="description"></param>
        public TMTraceSwitch(string displayName, string description) : base(displayName, description)
        {
        }

        /// <summary>
        /// Check if the <see cref="F:iTrading.Core.Kernel.TraceLevelIds.All" /> trace level it set.
        /// </summary>
        public bool All
        {
            get
            {
                return ((this.Level & TraceLevelIds.All) != TraceLevelIds.None);
            }
        }

        /// <summary>
        /// Check if the <see cref="F:iTrading.Core.Kernel.TraceLevelIds.Com" /> trace level it set.
        /// </summary>
        public bool Com
        {
            get
            {
                return ((this.Level & TraceLevelIds.Com) != TraceLevelIds.None);
            }
        }

        /// <summary>
        /// Check if the <see cref="F:iTrading.Core.Kernel.TraceLevelIds.ComClient" /> trace level it set.
        /// </summary>
        public bool ComClient
        {
            get
            {
                return ((this.Level & TraceLevelIds.ComClient) != TraceLevelIds.None);
            }
        }

        /// <summary>
        /// Check if the <see cref="F:iTrading.Core.Kernel.TraceLevelIds.Connect" /> trace level it set.
        /// </summary>
        public bool Connect
        {
            get
            {
                return ((this.Level & TraceLevelIds.Connect) != TraceLevelIds.None);
            }
        }

        /// <summary>
        /// Check if the <see cref="F:iTrading.Core.Kernel.TraceLevelIds.DataBase" /> trace level it set.
        /// </summary>
        public bool DataBase
        {
            get
            {
                return ((this.Level & TraceLevelIds.DataBase) != TraceLevelIds.None);
            }
        }

        /// <summary>
        /// Check if the <see cref="F:iTrading.Core.Kernel.TraceLevelIds.Indicator" /> trace level it set.
        /// </summary>
        public bool Indicator
        {
            get
            {
                return ((this.Level & TraceLevelIds.Indicator) != TraceLevelIds.None);
            }
        }

        /// <summary>
        /// Get/set trace level.
        /// </summary>
        public TraceLevelIds Level
        {
            get
            {
                return (TraceLevelIds) base.SwitchSetting;
            }
            set
            {
                base.SwitchSetting = (int) value;
            }
        }

        /// <summary>
        /// Check if the <see cref="F:iTrading.Core.Kernel.TraceLevelIds.MarketData" /> trace level it set.
        /// </summary>
        public bool MarketData
        {
            get
            {
                return ((this.Level & TraceLevelIds.MarketData) != TraceLevelIds.None);
            }
        }

        /// <summary>
        /// Check if the <see cref="F:iTrading.Core.Kernel.TraceLevelIds.MarketDepth" /> trace level it set.
        /// </summary>
        public bool MarketDepth
        {
            get
            {
                return ((this.Level & TraceLevelIds.MarketDepth) != TraceLevelIds.None);
            }
        }

        /// <summary>
        /// Check if the <see cref="F:iTrading.Core.Kernel.TraceLevelIds.Native" /> trace level it set.
        /// </summary>
        public bool Native
        {
            get
            {
                return ((this.Level & TraceLevelIds.Native) != TraceLevelIds.None);
            }
        }

        /// <summary>
        /// Check if the <see cref="F:iTrading.Core.Kernel.TraceLevelIds.News" /> trace level it set.
        /// </summary>
        public bool News
        {
            get
            {
                return ((this.Level & TraceLevelIds.News) != TraceLevelIds.None);
            }
        }

        /// <summary>
        /// Check if the <see cref="F:iTrading.Core.Kernel.TraceLevelIds.Order" /> trace level it set.
        /// </summary>
        public bool Order
        {
            get
            {
                return ((this.Level & TraceLevelIds.Order) != TraceLevelIds.None);
            }
        }

        /// <summary>
        /// Check if the <see cref="F:iTrading.Core.Kernel.TraceLevelIds.Quotes" /> trace level it set.
        /// </summary>
        public bool Quotes
        {
            get
            {
                return ((this.Level & TraceLevelIds.Quotes) != TraceLevelIds.None);
            }
        }

        /// <summary>
        /// Check if the <see cref="F:iTrading.Core.Kernel.TraceLevelIds.Strict" /> trace level it set.
        /// </summary>
        public bool Strict
        {
            get
            {
                return ((this.Level & TraceLevelIds.Strict) != TraceLevelIds.None);
            }
        }

        /// <summary>
        /// Check if the <see cref="F:iTrading.Core.Kernel.TraceLevelIds.SymbolLookup" /> trace level it set.
        /// </summary>
        public bool SymbolLookup
        {
            get
            {
                return ((this.Level & TraceLevelIds.SymbolLookup) != TraceLevelIds.None);
            }
        }

        /// <summary>
        /// Check if the <see cref="F:iTrading.Core.Kernel.TraceLevelIds.Test" /> trace level it set.
        /// </summary>
        public bool Test
        {
            get
            {
                return ((this.Level & TraceLevelIds.Test) != TraceLevelIds.None);
            }
        }

        /// <summary>
        /// Check if the <see cref="F:iTrading.Core.Kernel.TraceLevelIds.Timer" /> trace level it set.
        /// </summary>
        public bool Timer
        {
            get
            {
                return ((this.Level & TraceLevelIds.Timer) != TraceLevelIds.None);
            }
        }

        /// <summary>
        /// Check if the <see cref="F:iTrading.Core.Kernel.TraceLevelIds.Types" /> trace level it set.
        /// </summary>
        public bool Types
        {
            get
            {
                return ((this.Level & TraceLevelIds.Types) != TraceLevelIds.None);
            }
        }
    }
}

