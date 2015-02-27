namespace iTrading.Core.Interface
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.LicenseType" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("715F6BEB-6324-4427-B1FF-01526E3A8300")]
    public interface IComLicenseType
    {
        /// <summary>
        /// The TradeMagic id of the LicenseType.
        /// </summary>
        LicenseTypeId Id { get; }
        /// <summary>
        /// The name of the LicenseType.
        /// </summary>
        string Name { get; }
    }
}

