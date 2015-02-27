namespace iTrading.Core.Interface
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// Defines the <see cref="T:iTrading.Core.Kernel.LicenseTypeDictionary" /> public methods for early binding/intellisense.
    /// </summary>
    [Guid("E19D55BC-0120-477c-A59A-6C9E6CCC102D")]
    public interface IComLicenseTypeDictionary
    {
        /// <summary>
        /// Checks if the LicenseType exists in this container.
        /// </summary>
        /// <param name="licenseType"></param>
        /// <returns></returns>
        bool Contains(LicenseType licenseType);
        /// <summary>
        /// Retrieves an LicenseType object by its name.
        /// </summary>
        /// <param name="name"></param>
        LicenseType Find(string name);
        /// <summary>
        /// Retrieves an LicenseType object by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        LicenseType this[LicenseTypeId id] { get; }
        /// <summary>
        /// The collection of available <see cref="T:iTrading.Core.Kernel.LicenseType" /> instances.
        /// </summary>
        iTrading.Core.Kernel.ValuesCollection ValuesCollection { get; }
    }
}

