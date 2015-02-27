namespace iTrading.Core.Kernel
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Dictionary of LicenseTypes.
    /// </summary>
    [Guid("799FA3AE-0B0A-42bd-885D-0CA941EE9713"), ClassInterface(ClassInterfaceType.None)]
    public class LicenseTypeDictionary : DictionaryBase, IComLicenseTypeDictionary
    {
        internal void Add(LicenseType licenseType)
        {
            base.Dictionary.Add(licenseType.Id, licenseType);
        }

        /// <summary>
        /// Checks if the LicenseType exists in this container.
        /// </summary>
        /// <param name="licenseType"></param>
        /// <returns></returns>
        public bool Contains(LicenseType licenseType)
        {
            return base.Dictionary.Contains(licenseType.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(LicenseType[] array, int index)
        {
            base.CopyTo(array, index);
        }

        /// <summary>
        /// Retrieves an LicenseType object by its name.
        /// </summary>
        /// <param name="name"></param>
        public LicenseType Find(string name)
        {
            foreach (LicenseType type in base.Dictionary.Values)
            {
                if (type.Name == name)
                {
                    return type;
                }
            }
            return null;
        }

        /// <summary>
        /// Retrieves an LicenseType object by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public LicenseType this[LicenseTypeId id]
        {
            get
            {
                return (LicenseType) base.Dictionary[id];
            }
        }

        /// <summary>
        /// The collection of available <see cref="T:iTrading.Core.Kernel.LicenseType" /> instances.
        /// </summary>
        public ICollection Values
        {
            get
            {
                return base.Dictionary.Values;
            }
        }

        /// <summary>
        /// The Collection of available <see cref="T:iTrading.Core.Kernel.LicenseType" /> instances.
        /// Used by COM clients to enumerate the <see cref="T:iTrading.Core.Kernel.LicenseTypeDictionary" />
        /// because Interop does not allow enumeration (for..each) of Dictionaries.
        /// </summary>
        public iTrading.Core.Kernel.ValuesCollection ValuesCollection
        {
            get
            {
                return new iTrading.Core.Kernel.ValuesCollection(base.Dictionary.Values);
            }
        }
    }
}

