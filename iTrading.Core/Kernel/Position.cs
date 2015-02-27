using iTrading.Core.Kernel;

namespace iTrading.Core.Kernel
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Represents an open position in an account.
    /// </summary>
    [ClassInterface(ClassInterfaceType.None), Guid("53C1F8CE-71FB-4f4e-8851-580F526E7EA7")]
    public class Position : IComPosition
    {
        private Account account;
        private double avgPrice;
        private iTrading.Core.Kernel.Currency currency;
        private iTrading.Core.Kernel.MarketPosition marketPosition;
        private int quantity;
        private iTrading.Core.Kernel.Symbol symbol;

        internal Position(Account account, iTrading.Core.Kernel.Symbol symbol, iTrading.Core.Kernel.MarketPosition marketPosition, int quantity, iTrading.Core.Kernel.Currency currency, double avgPrice)
        {
            this.account = account;
            this.avgPrice = avgPrice;
            this.currency = currency;
            this.quantity = quantity;
            this.marketPosition = marketPosition;
            this.symbol = symbol;
        }

        internal void Update(PositionUpdateEventArgs eventArgs)
        {
            this.avgPrice = eventArgs.AvgPrice;
            this.currency = eventArgs.Currency;
            this.marketPosition = eventArgs.MarketPosition;
            this.quantity = eventArgs.Quantity;
        }

        /// <summary>
        /// Account where the position is belonging to.
        /// </summary>
        public Account Account
        {
            get
            {
                return this.account;
            }
        }

        /// <summary>
        /// Average cost per unit.
        /// </summary>
        public double AvgPrice
        {
            get
            {
                return this.avgPrice;
            }
        }

        /// <summary>
        /// Currency.
        /// </summary>
        public iTrading.Core.Kernel.Currency Currency
        {
            get
            {
                return this.currency;
            }
        }

        /// <summary>
        /// Type of position item.
        /// </summary>
        public iTrading.Core.Kernel.MarketPosition MarketPosition
        {
            get
            {
                return this.marketPosition;
            }
        }

        /// <summary>
        /// Number of shares/units/contracts.
        /// </summary>
        public int Quantity
        {
            get
            {
                return this.quantity;
            }
        }

        /// <summary>
        /// Symbol.
        /// </summary>
        public iTrading.Core.Kernel.Symbol Symbol
        {
            get
            {
                return this.symbol;
            }
        }
    }
}

