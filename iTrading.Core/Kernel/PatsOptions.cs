using iTrading.Core.Data;

namespace iTrading.Core.Kernel
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Holds Patsystems specific connection options.
    /// </summary>
    [ClassInterface(ClassInterfaceType.None), Guid("91CEFC33-FD95-44a6-83CB-72B2B82E908B")]
    public class PatsOptions : OptionsBase, IComPatsOptions, ITradingSerializable
    {
        private string applicationId;
        private int enable;
        private string hostAddress;
        private int hostHandShakeInterval;
        private int hostHandShakeTimeout;
        private int hostPort;
        private string licenseKey;
        private bool logoffOnConnectionClose;
        private int maxChangeMaskZeroSecs;
        private string newPassword;
        private string priceAddress;
        private int priceHandShakeInterval;
        private int priceHandShakeTimeout;
        private int pricePort;
        private bool superTas;

        /// <summary>
        /// Creates an instance of class <see cref="T:iTrading.Core.Kernel.PatsOptions" /> with default settings to connect
        /// to the local demo system.
        /// </summary>
        public PatsOptions()
        {
            this.applicationId = "";
            this.enable = 0;
            this.hostAddress = "";
            this.hostHandShakeInterval = 0;
            this.hostHandShakeTimeout = 0;
            this.hostPort = 0;
            this.licenseKey = "";
            this.logoffOnConnectionClose = false;
            this.maxChangeMaskZeroSecs = 0;
            this.newPassword = "";
            this.priceAddress = "";
            this.priceHandShakeInterval = 0;
            this.priceHandShakeTimeout = 0;
            this.pricePort = 0;
            this.superTas = false;
            base.Mode = ModeType.All[ModeTypeId.Demo];
            base.Provider = ProviderType.All[ProviderTypeId.Patsystems];
        }

        /// <summary>
        /// Creates and initializes a new instance.
        /// </summary>
        /// <param name="options"></param>
        public PatsOptions(OptionsBase options) : base(options)
        {
            this.applicationId = "";
            this.enable = 0;
            this.hostAddress = "";
            this.hostHandShakeInterval = 0;
            this.hostHandShakeTimeout = 0;
            this.hostPort = 0;
            this.licenseKey = "";
            this.logoffOnConnectionClose = false;
            this.maxChangeMaskZeroSecs = 0;
            this.newPassword = "";
            this.priceAddress = "";
            this.priceHandShakeInterval = 0;
            this.priceHandShakeTimeout = 0;
            this.pricePort = 0;
            this.superTas = false;
        }

        /// <summary></summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public PatsOptions(Bytes bytes, int version) : base(bytes, version)
        {
            this.applicationId = "";
            this.enable = 0;
            this.hostAddress = "";
            this.hostHandShakeInterval = 0;
            this.hostHandShakeTimeout = 0;
            this.hostPort = 0;
            this.licenseKey = "";
            this.logoffOnConnectionClose = false;
            this.maxChangeMaskZeroSecs = 0;
            this.newPassword = "";
            this.priceAddress = "";
            this.priceHandShakeInterval = 0;
            this.priceHandShakeTimeout = 0;
            this.pricePort = 0;
            this.superTas = false;
            base.Provider = ProviderType.All[ProviderTypeId.Patsystems];
            this.applicationId = bytes.ReadString();
            this.enable = bytes.ReadInt32();
            this.hostAddress = bytes.ReadString();
            this.hostHandShakeInterval = bytes.ReadInt32();
            this.hostHandShakeTimeout = bytes.ReadInt32();
            this.hostPort = bytes.ReadInt32();
            this.licenseKey = bytes.ReadString();
            this.logoffOnConnectionClose = bytes.ReadBoolean();
            if (version >= 7)
            {
                this.maxChangeMaskZeroSecs = bytes.ReadInt32();
            }
            this.newPassword = bytes.ReadString();
            this.priceAddress = bytes.ReadString();
            this.priceHandShakeInterval = bytes.ReadInt32();
            this.priceHandShakeTimeout = bytes.ReadInt32();
            this.pricePort = bytes.ReadInt32();
            this.superTas = bytes.ReadBoolean();
        }

        /// <summary>
        /// Checks if two instances of <see cref="T:iTrading.Core.Kernel.OptionsBase" /> will establish the same connection.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public override bool SameConnection(OptionsBase options)
        {
            if (!base.SameConnection(options))
            {
                return false;
            }
            if (!(options is PatsOptions))
            {
                return false;
            }
            PatsOptions options2 = (PatsOptions) options;
            if (options2.hostAddress != this.hostAddress)
            {
                return false;
            }
            if (options2.hostPort != this.hostPort)
            {
                return false;
            }
            if (options2.priceAddress != this.priceAddress)
            {
                return false;
            }
            if (options2.pricePort != this.pricePort)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Serialize current object.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public override void Serialize(Bytes bytes, int version)
        {
            base.Serialize(bytes, version);
            bytes.Write(this.applicationId);
            bytes.Write(this.enable);
            bytes.Write(this.hostAddress);
            bytes.Write(this.hostHandShakeInterval);
            bytes.Write(this.hostHandShakeTimeout);
            bytes.Write(this.hostPort);
            bytes.Write(this.licenseKey);
            bytes.Write(this.logoffOnConnectionClose);
            if (version >= 7)
            {
                bytes.Write(this.maxChangeMaskZeroSecs);
            }
            bytes.Write(this.newPassword);
            bytes.Write(this.priceAddress);
            bytes.Write(this.priceHandShakeInterval);
            bytes.Write(this.priceHandShakeTimeout);
            bytes.Write(this.pricePort);
            bytes.Write(this.superTas);
        }

        /// <summary>
        /// Application ID
        /// </summary>
        public string ApplicationId
        {
            get
            {
                return this.applicationId;
            }
            set
            {
                this.applicationId = value;
            }
        }

        /// <summary>
        /// Gets <see cref="P:iTrading.Core.Kernel.PatsOptions.ClassId" /> of current object.
        /// </summary>
        public override iTrading.Core.Kernel.ClassId ClassId
        {
            get
            {
                return iTrading.Core.Kernel.ClassId.PatsOptions;
            }
        }

        /// <summary>
        /// Get/set pats diagnostic level
        /// </summary>
        public int Enable
        {
            get
            {
                return this.enable;
            }
            set
            {
                this.enable = value;
            }
        }

        /// <summary>
        /// Name of host to connect to.
        /// </summary>
        public string HostAddress
        {
            get
            {
                return this.hostAddress;
            }
            set
            {
                this.hostAddress = value;
            }
        }

        /// <summary>
        /// Get/set the interval for host handshake protocol. 
        /// Will be set if != 0 and <see cref="P:iTrading.Core.Kernel.PatsOptions.HostHandShakeTimeout" /> != 0.
        /// </summary>
        public int HostHandShakeInterval
        {
            get
            {
                return this.hostHandShakeInterval;
            }
            set
            {
                this.hostHandShakeInterval = value;
            }
        }

        /// <summary>
        /// Get/set the interval for host handshake protocol. 
        /// Will be set if != 0 and <see cref="P:iTrading.Core.Kernel.PatsOptions.HostHandShakeInterval" /> != 0.
        /// </summary>
        public int HostHandShakeTimeout
        {
            get
            {
                return this.hostHandShakeTimeout;
            }
            set
            {
                this.hostHandShakeTimeout = value;
            }
        }

        /// <summary>
        /// Number of port to connect to.
        /// </summary>
        public int HostPort
        {
            get
            {
                return this.hostPort;
            }
            set
            {
                this.hostPort = value;
            }
        }

        /// <summary>
        /// License key.
        /// </summary>
        public string LicenseKey
        {
            get
            {
                return this.licenseKey;
            }
            set
            {
                this.licenseKey = value;
            }
        }

        /// <summary>
        /// TRUE = free any Patsystems ressources and force Patsystems API to save it's data locally. 
        /// TradeMagic can not reconnect to Patsystems servers. TradeMagic must be terminated and restarted.
        /// FALSE = TradeMagic may reconnect to Patsystems servers. Patsystems API will not save it's data.
        /// (Default: FALSE)
        /// </summary>
        public bool LogoffOnConnectionClose
        {
            get
            {
                return this.logoffOnConnectionClose;
            }
            set
            {
                this.logoffOnConnectionClose = value;
            }
        }

        /// <summary>
        /// The Patsystems API occationally "stops" sending data by (a) no longer setting the appropriate price change flag and (b) not updating the Pats internal
        /// price data structure. A <see cref="T:iTrading.Core.Kernel.MarketDataEventArgs" /> event, holding an error <see cref="F:iTrading.Core.Kernel.ErrorCode.UnexpectedDataStop" />, will be sent after
        /// n seconds, if only these "void" data events have been seen. n can be set/get by this parameter. If this parameter is 0, no such event is triggered.
        /// Default = 0.
        /// Note: TradeMagic does not unsubscribe you from the affected data stream, you need to unsubscribe by yourself.
        /// </summary>
        public int MaxChangeMaskZeroSecs
        {
            get
            {
                return this.maxChangeMaskZeroSecs;
            }
            set
            {
                this.maxChangeMaskZeroSecs = value;
            }
        }

        /// <summary>
        /// New password. Set a valid new password only if you want to change your password. 
        /// The new password will be valid after successful login. Default = "" (no password change).
        /// </summary>
        public string NewPassword
        {
            get
            {
                return this.newPassword;
            }
            set
            {
                this.newPassword = value;
            }
        }

        /// <summary>
        /// Name of price host to connect to.
        /// </summary>
        public string PriceAddress
        {
            get
            {
                return this.priceAddress;
            }
            set
            {
                this.priceAddress = value;
            }
        }

        /// <summary>
        /// Get/set the interval for price server handshake protocol. 
        /// Will be set if != 0 and <see cref="P:iTrading.Core.Kernel.PatsOptions.PriceHandShakeTimeout" /> != 0.
        /// </summary>
        public int PriceHandShakeInterval
        {
            get
            {
                return this.priceHandShakeInterval;
            }
            set
            {
                this.priceHandShakeInterval = value;
            }
        }

        /// <summary>
        /// Get/set the interval for price server handshake protocol. 
        /// Will be set if != 0 and <see cref="P:iTrading.Core.Kernel.PatsOptions.PriceHandShakeInterval" /> != 0.
        /// </summary>
        public int PriceHandShakeTimeout
        {
            get
            {
                return this.priceHandShakeTimeout;
            }
            set
            {
                this.priceHandShakeTimeout = value;
            }
        }

        /// <summary>
        /// Number of price host port to connect to.
        /// </summary>
        public int PricePort
        {
            get
            {
                return this.pricePort;
            }
            set
            {
                this.pricePort = value;
            }
        }

        /// <summary>
        /// Enable/disable SuperTAS support. Default: disabled.
        /// </summary>
        public bool SuperTas
        {
            get
            {
                return this.superTas;
            }
            set
            {
                this.superTas = value;
            }
        }
    }
}

