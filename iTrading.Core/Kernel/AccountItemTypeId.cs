namespace iTrading.Core.Kernel
{
    using System;

    /// <summary>
    /// Identifies the account item type.
    /// </summary>
    public enum AccountItemTypeId
    {
        BuyingPower,
        CashValue,
        ExcessEquity,
        InitialMargin,
        InitialMarginOvernight,
        MaintenanceMargin,
        MaintenanceMarginOvernight,
        NetLiquidation,
        NetLiquidationByCurrency,
        RealizedProfitLoss,
        TotalCashBalance,
        Unknown
    }
}

