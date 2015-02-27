namespace iTrading.Core.Kernel
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Represents an license type.
    /// </summary>
    [ClassInterface(ClassInterfaceType.None), Guid("B338A90C-4A97-4090-B929-98B60E999575")]
    public class LicenseType : IComLicenseType
    {
        private static LicenseTypeDictionary all = null;
        private LicenseTypeId id;

        internal LicenseType(LicenseTypeId id)
        {
            this.id = id;
        }

        /// <summary>
        /// Get a collection of all available license types.
        /// </summary>
        public static LicenseTypeDictionary All
        {
            get
            {
                lock (typeof(Globals))
                {
                    if (all == null)
                    {
                        all = new LicenseTypeDictionary();
                        all.Add(new LicenseType(LicenseTypeId.NotRegistered));
                        all.Add(new LicenseType(LicenseTypeId.Professional));
                    }
                }
                return all;
            }
        }

        /// <summary>
        /// The TradeMagic id of the LicenseType.
        /// </summary>
        public LicenseTypeId Id
        {
            get
            {
                return this.id;
            }
        }

        /// <summary>
        /// The name of the LicenseType.
        /// </summary>
        public string Name
        {
            get
            {
                switch (this.id)
                {
                    case LicenseTypeId.NotRegistered:
                        return "Not Registered";

                    case LicenseTypeId.Professional:
                        return "License Registered";
                }
                return "Not Registered";
            }
        }
    }
}

