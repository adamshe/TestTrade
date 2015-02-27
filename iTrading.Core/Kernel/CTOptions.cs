using iTrading.Core.Data;

namespace iTrading.Core.Kernel
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Holds CyberTrader specific connection options.
    /// </summary>
    [Guid("E6389E05-6C47-49c3-8235-0E28F0542BD2"), ClassInterface(ClassInterfaceType.None)]
    public class CTOptions : OptionsBase, IComCTOptions, ITradingSerializable
    {
        private string authorizationCode;
        private string ipAddress;
        private string ipAddressAlternate;
        private int newsDaysBack;

        /// <summary>
        /// Creates an instance of class <see cref="T:iTrading.Core.Kernel.CTOptions" /> with default settings.
        /// </summary>
        public CTOptions()
        {
            this.authorizationCode = "";
            this.ipAddress = "";
            this.ipAddressAlternate = "";
            this.newsDaysBack = 1;
            base.Mode = ModeType.All[ModeTypeId.Demo];
            base.Provider = ProviderType.All[ProviderTypeId.CyberTrader];
        }

        /// <summary>
        /// Creates and initializes a new instance.
        /// </summary>
        /// <param name="options"></param>
        public CTOptions(OptionsBase options) : base(options)
        {
            this.authorizationCode = "";
            this.ipAddress = "";
            this.ipAddressAlternate = "";
            this.newsDaysBack = 1;
        }

        /// <summary></summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public CTOptions(Bytes bytes, int version) : base(bytes, version)
        {
            this.authorizationCode = "";
            this.ipAddress = "";
            this.ipAddressAlternate = "";
            this.newsDaysBack = 1;
            base.Provider = ProviderType.All[ProviderTypeId.CyberTrader];
            this.authorizationCode = bytes.ReadString();
            this.ipAddress = bytes.ReadString();
            this.ipAddressAlternate = bytes.ReadString();
            this.newsDaysBack = bytes.ReadInt32();
        }

        /// <summary>
        /// Serialize current object.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public override void Serialize(Bytes bytes, int version)
        {
            base.Serialize(bytes, version);
            bytes.Write(this.authorizationCode);
            bytes.Write(this.ipAddress);
            bytes.Write(this.ipAddressAlternate);
            bytes.Write(this.newsDaysBack);
        }

        /// <summary>
        /// CyberTrader authorization code for the data API.
        /// </summary>
        public string AuthorizationCode
        {
            get
            {
                return this.authorizationCode;
            }
            set
            {
                this.authorizationCode = value;
            }
        }

        /// <summary>
        /// Gets <see cref="P:iTrading.Core.Kernel.CTOptions.ClassId" /> of current object.
        /// </summary>
        public override iTrading.Core.Kernel.ClassId ClassId
        {
            get
            {
                return iTrading.Core.Kernel.ClassId.CTOptions;
            }
        }

        /// <summary>
        /// CyberTrader IP Address.
        /// </summary>
        public string IPAddress
        {
            get
            {
                return this.ipAddress;
            }
            set
            {
                this.ipAddress = value;
            }
        }

        /// <summary>
        /// Alternate CyberTrader IP Address.
        /// </summary>
        public string IPAddressAlternate
        {
            get
            {
                return this.ipAddressAlternate;
            }
            set
            {
                this.ipAddressAlternate = value;
            }
        }

        /// <summary>
        /// Retrieve news n days back.
        /// Default = 1.
        /// </summary>
        public int NewsDaysBack
        {
            get
            {
                return this.newsDaysBack;
            }
            set
            {
                this.newsDaysBack = value;
            }
        }
    }
}

