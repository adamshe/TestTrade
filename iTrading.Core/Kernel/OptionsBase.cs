using iTrading.Core.Data;

namespace iTrading.Core.Kernel
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Xml;
    using iTrading.Core.Interface;

    /// <summary>
    /// Base class for connecting options. Provider specific options are derived from this class.
    /// Holds common settings to open a new connection. <seealso cref="M:iTrading.Core.Kernel.Connection.Connect(iTrading.Core.Kernel.OptionsBase)" />
    /// </summary>
    [Guid("47CBB2F6-B0EE-41e3-9BE6-BF8909B21D6D"), ClassInterface(ClassInterfaceType.None)]
    public class OptionsBase : IComOptionsBase, ITradingSerializable
    {
        private int historyMaintained;
        internal LicenseType license;
        private ModeType mode;
        private string password;
        private ProviderType provider;
        private int recorderWriteMinutes;
        private bool runAtServer;
        private bool simOcaPartialFills;
        private int timerDelayHours;
        private int timerIntervalMilliSeconds;
        private const int tmDefaultPort = 0x223d;
        private string tmHost;
        private int tmPort;
        private string user;
        private string vendorCode;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public OptionsBase()
        {
            this.provider = ProviderType.All[ProviderTypeId.InteractiveBrokers];
            this.historyMaintained = 30;
            this.license = LicenseType.All[LicenseTypeId.Professional];
            this.mode = ModeType.All[ModeTypeId.Demo];
            this.password = "";
            this.simOcaPartialFills = true;
            this.recorderWriteMinutes = 1;
            this.runAtServer = false;
            this.timerDelayHours = 0;
            this.timerIntervalMilliSeconds = 0x3e8;
            this.tmHost = "localhost";
            this.tmPort = 0x223d;
            this.user = "";
            this.vendorCode = "";
            this.vendorCode = Globals.VendorCode;
        }

        /// <summary>
        /// Creates and initializes a new instance.
        /// </summary>
        /// <param name="options"></param>
        public OptionsBase(OptionsBase options)
        {
            this.provider = ProviderType.All[ProviderTypeId.InteractiveBrokers];
            this.historyMaintained = 30;
            this.license = LicenseType.All[LicenseTypeId.Professional];
            this.mode = ModeType.All[ModeTypeId.Demo];
            this.password = "";
            this.simOcaPartialFills = true;
            this.recorderWriteMinutes = 1;
            this.runAtServer = false;
            this.timerDelayHours = 0;
            this.timerIntervalMilliSeconds = 0x3e8;
            this.tmHost = "localhost";
            this.tmPort = 0x223d;
            this.user = "";
            this.vendorCode = "";
            this.Provider = options.Provider;
            this.HistoryMaintained = options.HistoryMaintained;
            this.TMHost = options.TMHost;
            this.license = options.License;
            this.Mode = options.Mode;
            this.Password = options.Password;
            this.TMPort = options.TMPort;
            this.RunAtServer = options.RunAtServer;
            this.TimerDelayHours = options.TimerDelayHours;
            this.User = options.User;
            this.vendorCode = options.VendorCode;
        }

        /// <summary></summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public OptionsBase(Bytes bytes, int version)
        {
            this.provider = ProviderType.All[ProviderTypeId.InteractiveBrokers];
            this.historyMaintained = 30;
            this.license = LicenseType.All[LicenseTypeId.Professional];
            this.mode = ModeType.All[ModeTypeId.Demo];
            this.password = "";
            this.simOcaPartialFills = true;
            this.recorderWriteMinutes = 1;
            this.runAtServer = false;
            this.timerDelayHours = 0;
            this.timerIntervalMilliSeconds = 0x3e8;
            this.tmHost = "localhost";
            this.tmPort = 0x223d;
            this.user = "";
            this.vendorCode = "";
            this.Provider = bytes.ReadProvider();
            this.HistoryMaintained = bytes.ReadInt32();
            this.TMHost = bytes.ReadString();
            this.license = bytes.ReadLicenseType();
            this.Mode = bytes.ReadModeType();
            this.Password = bytes.ReadString();
            this.TMPort = bytes.ReadInt32();
            if (version > 2)
            {
                this.RecorderWriteMinutes = bytes.ReadInt32();
            }
            this.RunAtServer = bytes.ReadBoolean();
            if (version > 1)
            {
                this.SimOcaPartialFills = bytes.ReadBoolean();
            }
            this.TimerDelayHours = bytes.ReadInt32();
            if (version > 4)
            {
                this.TimerIntervalMilliSeconds = bytes.ReadInt32();
            }
            this.User = bytes.ReadString();
            this.vendorCode = bytes.ReadString();
        }

        /// <summary>
        /// Restore <see cref="T:iTrading.Core.Kernel.OptionsBase" /> object from "Config.xml".
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static OptionsBase Restore(ProviderType provider, ModeType mode)
        {
            string installDir = Globals.InstallDir;
            Trace.Assert(File.Exists(installDir + @"\Config.xml"), "TradeMagic is not installed properly, Config.xml file is missing");
            XmlDocument document = new XmlDocument();
            XmlTextReader reader = new XmlTextReader(installDir + @"\Config.xml");
            document.Load(reader);
            reader.Close();
            try
            {
                Bytes bytes = new Bytes();
                bytes.Reset(Convert.FromBase64String(document["TradeMagic"][provider.Id.ToString()]["Last"][mode.Id.ToString()].InnerText));
                return (OptionsBase) bytes.ReadSerializable();
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Checks if two instances of <see cref="T:iTrading.Core.Kernel.OptionsBase" /> will establish the same connection.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public virtual bool SameConnection(OptionsBase options)
        {
            if (options.provider.Id != this.provider.Id)
            {
                return false;
            }
            if (options.mode.Id != this.mode.Id)
            {
                return false;
            }
            if (options.runAtServer != this.runAtServer)
            {
                return false;
            }
            if (options.user != this.user)
            {
                return false;
            }
            if (options.password != this.password)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Save <see cref="T:iTrading.Core.Kernel.OptionsBase" /> object to "Config.xml" file.
        /// </summary>
        public void Save()
        {
            string installDir = Globals.InstallDir;
            Trace.Assert(File.Exists(installDir + @"\Config.xml"), "TradeMagic is not installed properly, Config.xml file is missing");
            XmlDocument document = new XmlDocument();
            XmlTextReader reader = new XmlTextReader(installDir + @"\Config.xml");
            document.Load(reader);
            reader.Close();
            if (document["TradeMagic"] == null)
            {
                document.AppendChild(document.CreateElement("TradeMagic"));
            }
            if (document["TradeMagic"][this.Provider.Id.ToString()] == null)
            {
                document["TradeMagic"].AppendChild(document.CreateElement(this.Provider.Id.ToString()));
            }
            if (document["TradeMagic"][this.Provider.Id.ToString()]["Last"] == null)
            {
                document["TradeMagic"][this.Provider.Id.ToString()].AppendChild(document.CreateElement("Last"));
            }
            if (document["TradeMagic"][this.Provider.Id.ToString()]["Last"][this.Mode.Id.ToString()] == null)
            {
                document["TradeMagic"][this.Provider.Id.ToString()]["Last"].AppendChild(document.CreateElement(this.Mode.Id.ToString()));
            }
            Bytes bytes = new Bytes();
            bytes.WriteSerializable(this);
            document["TradeMagic"][this.Provider.Id.ToString()]["Last"][this.Mode.Id.ToString()].InnerText = Convert.ToBase64String(bytes.Out, 0, bytes.OutLength);
            document.Save(installDir + @"\Config.xml");
        }

        /// <summary>
        /// Serialize the current object.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public virtual void Serialize(Bytes bytes, int version)
        {
            bytes.Write(this.Provider);
            bytes.Write(this.HistoryMaintained);
            bytes.Write(this.TMHost);
            bytes.Write(this.license);
            bytes.Write(this.Mode);
            bytes.Write(this.Password);
            bytes.Write(this.TMPort);
            bytes.Write(this.RecorderWriteMinutes);
            bytes.Write(this.RunAtServer);
            bytes.Write(this.SimOcaPartialFills);
            bytes.Write(this.TimerDelayHours);
            bytes.Write(this.TimerIntervalMilliSeconds);
            bytes.Write(this.User);
            bytes.Write(this.VendorCode);
        }

        /// <summary>
        /// Gets <see cref="P:iTrading.Core.Kernel.OptionsBase.ClassId" /> of current object.
        /// </summary>
        public virtual iTrading.Core.Kernel.ClassId ClassId
        {
            get
            {
                return iTrading.Core.Kernel.ClassId.OptionsBase;
            }
        }

        /// <summary>
        /// TradeMagic maintained order history this many days back. Executions and orders themselves will not be
        /// deleted.
        /// Default = 30. Maximum = 1000.
        /// </summary>
        public int HistoryMaintained
        {
            get
            {
                return this.historyMaintained;
            }
            set
            {
                this.historyMaintained = Math.Min(value, 0x3e8);
            }
        }

        /// <summary>
        /// Identifies the license.
        /// </summary>
        public LicenseType License
        {
            get
            {
                return this.license;
            }
        }

        /// <summary>
        /// Identifies the mode.
        /// </summary>
        public ModeType Mode
        {
            get
            {
                return this.mode;
            }
            set
            {
                this.mode = value;
            }
        }

        /// <summary>
        /// Password to connect to provider or data provider. Default = "".
        /// </summary>
        public string Password
        {
            get
            {
                return this.password;
            }
            set
            {
                this.password = value;
            }
        }

        /// <summary>
        /// Identifies the broker or data provider. Default = <see cref="F:iTrading.Core.Kernel.ProviderTypeId.InteractiveBrokers" />
        /// </summary>
        public ProviderType Provider
        {
            get
            {
                return this.provider;
            }
            set
            {
                this.provider = value;
            }
        }

        /// <summary>
        /// Get/set max gap (in minutes) between subsequent write calls to store the recorded data. Value must be greater than 0.
        /// </summary>
        public int RecorderWriteMinutes
        {
            get
            {
                return this.recorderWriteMinutes;
            }
            set
            {
                if (value <= 0)
                {
                    throw new TMException(ErrorCode.Panic, "Cbi.OptionsBase.RecorderWriteMinutes: value must be greater than 0");
                }
                this.recorderWriteMinutes = value;
            }
        }

        /// <summary>
        /// TRUE: Connect to TradeMagic server. FALSE: Establish connection in current application scope.
        /// </summary>
        public bool RunAtServer
        {
            get
            {
                return this.runAtServer;
            }
            set
            {
                this.runAtServer = value;
            }
        }

        /// <summary>
        /// This property only is effective if the provider does not support native OCA orders.
        /// TRUE: Partial fills reduce the order quantity of the paired OCA orders, when OCA handling is simulated.
        /// FALSE: OCA simulation works on complete fills only.
        /// Default: TRUE.
        /// </summary>
        public bool SimOcaPartialFills
        {
            get
            {
                return this.simOcaPartialFills;
            }
            set
            {
                this.simOcaPartialFills = value;
            }
        }

        /// <summary>
        /// Difference between local time and providers server time in hours.
        /// This property may be used if a provider does not support heart beat clock sychronisation.
        /// Default = 0.
        /// </summary>
        public int TimerDelayHours
        {
            get
            {
                return this.timerDelayHours;
            }
            set
            {
                this.timerDelayHours = value;
            }
        }

        /// <summary>
        /// Internal for timer events in milliseconds. Default = 1000.
        /// Please note: Becomes effective on connecting. Changes while connected won't have any effect.
        /// </summary>
        public int TimerIntervalMilliSeconds
        {
            get
            {
                return this.timerIntervalMilliSeconds;
            }
            set
            {
                this.timerIntervalMilliSeconds = value;
            }
        }

        /// <summary>
        /// Default port for establishing a connection using TradeMagic server.
        /// Value = 8765.
        /// </summary>
        public int TMDefaultPort
        {
            get
            {
                return 0x223d;
            }
        }

        /// <summary>
        /// Name of host to connect to. Do not enter an IP address string, but the hostname itself. Default = "localhost".
        /// </summary>
        public string TMHost
        {
            get
            {
                return this.tmHost;
            }
            set
            {
                this.tmHost = value;
            }
        }

        /// <summary>
        /// Number of port to connect to. Default = <see cref="P:iTrading.Core.Kernel.OptionsBase.TMDefaultPort" />
        /// </summary>
        public int TMPort
        {
            get
            {
                return this.tmPort;
            }
            set
            {
                this.tmPort = value;
            }
        }

        /// <summary>
        /// User id to connect to provider or data provider. Default = "".
        /// </summary>
        public string User
        {
            get
            {
                return this.user;
            }
            set
            {
                this.user = value;
            }
        }

        /// <summary>
        /// Vendor code to identify TradeMagic third party vendor. <seealso cref="P:iTrading.Core.Kernel.Globals.VendorCode" />
        /// </summary>
        public string VendorCode
        {
            get
            {
                return this.vendorCode;
            }
        }

        /// <summary>
        /// Version number.
        /// </summary>
        public int Version
        {
            get
            {
                return 7;
            }
        }
    }
}

