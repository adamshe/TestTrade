namespace iTrading.Core.Kernel
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Represents a single row of the level II display.
    /// </summary>
    [Guid("ACA4C4D8-1D3D-402f-82F4-B4CFC6E38EFD"), ClassInterface(ClassInterfaceType.None)]
    public class MarketDepthRow : IComMarketDepthRow
    {
        private object adapterLink = null;
        internal string marketMaker;
        internal double price;
        internal DateTime time;
        internal int volume;

        internal MarketDepthRow(string marketMaker, double price, int volume, DateTime time)
        {
            this.marketMaker = marketMaker;
            this.price = price;
            this.time = time;
            this.volume = volume;
        }

        /// <summary>
        /// For internal use only. To not set this property.
        /// </summary>
        public object AdapterLink
        {
            get
            {
                return this.adapterLink;
            }
            set
            {
                this.adapterLink = value;
            }
        }

        /// <summary>
        /// Market maker id.
        /// </summary>
        public string MarketMaker
        {
            get
            {
                return this.marketMaker;
            }
        }

        /// <summary>
        /// Price of position.
        /// </summary>
        public double Price
        {
            get
            {
                return this.price;
            }
        }

        /// <summary>
        /// Time last update.
        /// </summary>
        public DateTime Time
        {
            get
            {
                return this.time;
            }
        }

        /// <summary>
        /// Siez of position.
        /// </summary>
        public int Volume
        {
            get
            {
                return this.volume;
            }
        }
    }
}

