namespace iTrading.Core.Interface
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Data;

    /// <summary>
    /// Represents a <see cref="T:TradeMagic.Data.PeriodType" /> type.
    /// </summary>
    [Guid("091E27E6-B330-4dc9-8480-922FED05392A")]
    public interface IComPeriodType
    {
        /// <summary>
        /// The TradeMagic id of the quotes size type.
        /// </summary>
        PeriodTypeId Id { get; }
        /// <summary>
        /// The name of the <see cref="T:TradeMagic.Data.PeriodType" />.
        /// </summary>
        string Name { get; }
    }
}

