namespace iTrading.Core.Kernel
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Represents an account item type. Please note, that the pool of available account item types (see <see cref="P:iTrading.Core.Kernel.Connection.AccountItemTypes" />)
    /// varies between brokers/data providers.
    /// </summary>
    [ClassInterface(ClassInterfaceType.None), Guid("D1D6E9CC-105C-42ad-A8B4-D2CBA1662886")]
    public class AccountItemType : IComAccountItemType
    {
        private static AccountItemTypeDictionary all = null;
        private AccountItemTypeId id;
        private string mapId;

        internal AccountItemType(AccountItemTypeId id, string mapId)
        {
            this.id = id;
            this.mapId = mapId;
        }

        /// <summary>
        /// Get a collection of all available account item types.
        /// See <see cref="P:iTrading.Core.Kernel.Connection.AccountItemTypes" /> for a collection of <see cref="T:iTrading.Core.Kernel.AccountItemType" /> objects supported
        /// by the current broker.
        /// </summary>
        public static AccountItemTypeDictionary All
        {
            get
            {
                lock (typeof(Globals))
                {
                    if (all == null)
                    {
                        all = new AccountItemTypeDictionary();
                        all.Add(new AccountItemType(AccountItemTypeId.BuyingPower, ""));
                        all.Add(new AccountItemType(AccountItemTypeId.CashValue, ""));
                        all.Add(new AccountItemType(AccountItemTypeId.ExcessEquity, ""));
                        all.Add(new AccountItemType(AccountItemTypeId.InitialMargin, ""));
                        all.Add(new AccountItemType(AccountItemTypeId.InitialMarginOvernight, ""));
                        all.Add(new AccountItemType(AccountItemTypeId.MaintenanceMargin, ""));
                        all.Add(new AccountItemType(AccountItemTypeId.MaintenanceMarginOvernight, ""));
                        all.Add(new AccountItemType(AccountItemTypeId.NetLiquidation, ""));
                        all.Add(new AccountItemType(AccountItemTypeId.NetLiquidationByCurrency, ""));
                        all.Add(new AccountItemType(AccountItemTypeId.RealizedProfitLoss, ""));
                        all.Add(new AccountItemType(AccountItemTypeId.TotalCashBalance, ""));
                        all.Add(new AccountItemType(AccountItemTypeId.Unknown, ""));
                    }
                }
                return all;
            }
        }

        /// <summary>
        /// The TradeMagic id of the AccountItemType. This id is independent from the underlying broker system.
        /// </summary>
        public AccountItemTypeId Id
        {
            get
            {
                return this.id;
            }
        }

        /// <summary>
        /// The broker dependent id of the AccountItemType. 
        /// </summary>
        public string MapId
        {
            get
            {
                return this.mapId;
            }
        }

        /// <summary>
        /// The name of the AccountItemType.
        /// </summary>
        public string Name
        {
            get
            {
                switch (this.id)
                {
                    case AccountItemTypeId.BuyingPower:
                        return "Buying Power";

                    case AccountItemTypeId.CashValue:
                        return "Cash Value";

                    case AccountItemTypeId.ExcessEquity:
                        return "Excess Equity";

                    case AccountItemTypeId.InitialMargin:
                        return "Initial Margin";

                    case AccountItemTypeId.InitialMarginOvernight:
                        return "Initial Margin Overnight";

                    case AccountItemTypeId.MaintenanceMargin:
                        return "Maintenance Margin";

                    case AccountItemTypeId.MaintenanceMarginOvernight:
                        return "Maintenance Margin Overnight";

                    case AccountItemTypeId.NetLiquidation:
                        return "Net Liquidation Value";

                    case AccountItemTypeId.NetLiquidationByCurrency:
                        return "Net Liquidation by Currency";

                    case AccountItemTypeId.RealizedProfitLoss:
                        return "Realized Profit/Loss";

                    case AccountItemTypeId.TotalCashBalance:
                        return "Total Cash Balance";

                    case AccountItemTypeId.Unknown:
                        return "Unknown";
                }
                return "Unknown";
            }
        }
    }
}

