namespace iTrading.Core.Kernel
{
    using Microsoft.Win32;
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.IO;
    using System.Management;
    using System.Net;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Security.Cryptography;
    using System.Windows.Forms;
    using System.Xml;

    /// <summary>
    /// Holder for global settings, constants and bootstrap methods.
    /// </summary>
    [ComVisible(false)]
    public static class Globals
    {
        private static string appId = "Default";
        private static IDb db = null;
        /// <summary>
        /// This is the default <see cref="T:iTrading.Core.Kernel.ErrorArgsEventHandler" />. It pops up a messagebox and displays
        /// an error message.
        /// </summary>
        public static readonly ErrorArgsEventHandler DefaultErrorArgs = new ErrorArgsEventHandler(Globals.DefaultErrorArgsCallback);
        private static Assembly indicatorAssembly = null;
        private static string installDir = "";
        private static IProgress iProgress = new ProgressForm();
        private static bool licenseCheckFailed = false;
        private static LicenseType licenseType = null;
        private static string machineId = "";
        /// <summary>
        /// TradeMagic internal max. date. Valid dates have to be smaller than MaxDate. Take the first day of month
        /// - and not the last one - to avoid problem using the DateTimePicker control.
        /// </summary>
        public static readonly DateTime MaxDate = new DateTime(2099, 12, 1);
        /// <summary>
        /// TradeMagic internal min. date. Valid dates have to be smaller than MinDate. Take the first day of month
        /// - and not the last one - to avoid problem using the DateTimePicker control.
        /// </summary>
        public static readonly DateTime MinDate = new DateTime(1900, 1, 1);
        internal static double recorderIntervalSeconds = 0.25;
        private static TMTraceSwitch traceSwitch = new TMTraceSwitch("Trace", "TradeMagic trace switch");
        private static string userId = "";
        private static string vendorCode = "";

        private static void DefaultErrorArgsCallback(object sender, iTrading.Core.Kernel.ITradingErrorEventArgs e)
        {
            MessageBox.Show(e.Message, (e.Error >= ErrorCode.NoError) ? "TradeMagic info" : "TradeMagic error", MessageBoxButtons.OK, (e.Error >= ErrorCode.NoError) ? MessageBoxIcon.Asterisk : MessageBoxIcon.Hand);
        }

        /// <summary>
        /// The actual license.
        /// </summary>
        public static LicenseType GetLicense(string moduleName)
        {
            lock (typeof(Globals))
            {
                if (licenseCheckFailed && (licenseType != null))
                {
                    return licenseType;
                }
                string str = "";
                if (vendorCode.Length > 0)
                {
                    char[] chArray = vendorCode.ToCharArray();
                    byte[] buffer = new byte[2 * vendorCode.Length];
                    for (int i = 0; i < chArray.Length; i++)
                    {
                        buffer[2 * i] = BitConverter.GetBytes(chArray[i])[0];
                        buffer[(2 * i) + 1] = BitConverter.GetBytes(chArray[i])[1];
                    }
                    MD5 md = new MD5CryptoServiceProvider();
                    str = BitConverter.ToString(md.ComputeHash(buffer)).Replace("-", "").ToLower();
                }
                LicenseType type = LicenseType.All[LicenseTypeId.NotRegistered];
                lock (typeof(Globals))
                {
                    if (UserId.Length == 0)
                    {
                        try
                        {
                            string installDir = InstallDir;
                            Trace.Assert(System.IO.File.Exists(installDir + @"\Config.xml"), "TradeMagic is not installed properly, Config.xml file is missing");
                            XmlDocument document = new XmlDocument();
                            XmlTextReader reader = new XmlTextReader(installDir + @"\Config.xml");
                            document.Load(reader);
                            reader.Close();
                            userId = document["TradeMagic"]["User"]["Id"].InnerText;
                        }
                        catch (Exception)
                        {
                            licenseCheckFailed = true;
                            return (licenseType = type);
                        }
                    }
                    try
                    {
                        WebRequest request = WebRequest.Create("http://www.trademagic.net/tools/LicenseCheck.php?id=" + UserId + "&vc=" + ((str.Length > 0) ? str : "unknown") + "&ap=" + ((AppId.Length > 0) ? AppId : "unknown") + "&md=" + ((moduleName.Length > 0) ? moduleName : "unknown") + "&vs=" + Assembly.GetAssembly(typeof(iTrading.Core.Kernel.Connection)).GetName().Version.ToString() + "&mc=" + MachineId);
                        request.Timeout = 5000;
                        HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                        string s = new StreamReader(response.GetResponseStream()).ReadToEnd();
                        s = s.Substring(0, s.IndexOf("</TradeMagic>") + "</TradeMagic>".Length);
                        response.Close();
                        XmlDocument document2 = new XmlDocument();
                        XmlTextReader reader2 = new XmlTextReader(new StringReader(s));
                        document2.Load(reader2);
                        reader2.Close();
                        LicenseTypeId id = (LicenseTypeId) Convert.ToInt32(document2["TradeMagic"]["License"].InnerText);
                        if (id == LicenseTypeId.NotRegistered)
                        {
                            Trace.WriteLine("Registration error: " + document2["TradeMagic"]["Message"].InnerText);
                        }
                        type = LicenseType.All[id];
                        if (type == null)
                        {
                            licenseCheckFailed = true;
                            type = LicenseType.All[LicenseTypeId.Professional];
                        }
                        response.Close();
                    }
                    catch (WebException)
                    {
                        licenseCheckFailed = true;
                        type = LicenseType.All[LicenseTypeId.Professional];
                    }
                    catch (Exception exception)
                    {
                        licenseCheckFailed = true;
                        type = LicenseType.All[LicenseTypeId.Professional];
                        Trace.WriteLine("Unable to check license: " + exception.Message);
                    }
                }
                return (licenseType = type);
            }
        }

        /// <summary>
        /// Application Id.
        /// Default = "Default".
        /// </summary>
        public static string AppId
        {
            get
            {
                return appId;
            }
            set
            {
                appId = value;
            }
        }

        /// <summary>
        /// Pseudo expiry for continous contract quotes data.
        /// </summary>
        public static DateTime ContinousContractExpiry
        {
            get
            {
                return MinDate;
            }
        }

        /// <summary>
        /// Get the global database object.
        /// </summary>
        public static IDb DB
        {
            get
            {
                lock (typeof(Globals))
                {
                    if (db == null)
                    {
                        db = (IDb) Assembly.LoadFrom(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\TradeMagic.Db.dll").CreateInstance("TradeMagic.Db.Adapter");
                    }
                    return db;
                }
            }
        }

        internal static Assembly IndicatorAssembly
        {
            get
            {
                lock (typeof(Globals))
                {
                    if (indicatorAssembly == null)
                    {
                        indicatorAssembly = Assembly.LoadFrom("iTrading.Indicator.dll");
                    }
                }
                return indicatorAssembly;
            }
        }

        /// <summary>
        /// Get/set TradeMagic installation directory. 
        /// Default = "".
        /// Please note: If the default is not overriden, TradeMagic looks up the registry at
        /// SOFTWARE/TradeMagic/TradeMagic 3/InstallDir.
        /// </summary>
        public static string InstallDir
        {
            get
            {
                lock (typeof(Globals))
                {
                    if ((installDir == null) || (installDir.Length == 0))
                    {
                        RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE");
                        Trace.Assert(key != null, "TradeMagic is not installed properly, registry key: 'SOFTWARE' is missing");
                        key = key.OpenSubKey("TradeMagic");
                        Trace.Assert(key != null, "TradeMagic is not installed properly, registry key: 'TradeMagic' is missing");
                        key = key.OpenSubKey("TradeMagic 3");
                        Trace.Assert(key != null, "TradeMagic is not installed properly, registry key: 'TradeMagic 3' is missing");
                        installDir = (string) key.GetValue("InstallDir");
                        Trace.Assert(key != null, "TradeMagic is not installed properly, registry key: 'InstallDir' is missing");
                    }
                }
                return installDir;
            }
            set
            {
                installDir = value;
            }
        }

        /// <summary>
        /// Get the machine ID.
        /// </summary>
        public static string MachineId
        {
            get
            {
                lock (typeof(Globals))
                {
                    if (machineId.Length == 0)
                    {
                        ArrayList list = new ArrayList();
                        ManagementObjectSearcher searcher = new ManagementObjectSearcher(new ObjectQuery("select * from Win32_Processor"));
                        foreach (ManagementObject obj2 in searcher.Get())
                        {
                            list.Add(obj2["Name"]);
                        }
                        list.Sort();
                        Trace.Assert(list.Count > 0, "Core.Globals.MachineId1: keys.Count=0");
                        string str = (string) list[0];
                        list.Clear();
                        searcher = new ManagementObjectSearcher(new ObjectQuery("select * from Win32_BIOS"));
                        foreach (ManagementObject obj3 in searcher.Get())
                        {
                            list.Add(obj3["Name"]);
                        }
                        list.Sort();
                        Trace.Assert(list.Count > 0, "Cbi.Globals.MachineId2: keys.Count=0");
                        str = str + ((string) list[0]);
                        char[] chArray = str.ToCharArray();
                        byte[] buffer = new byte[2 * str.Length];
                        for (int i = 0; i < chArray.Length; i++)
                        {
                            buffer[2 * i] = BitConverter.GetBytes(chArray[i])[0];
                            buffer[(2 * i) + 1] = BitConverter.GetBytes(chArray[i])[1];
                        }
                        MD5 md = new MD5CryptoServiceProvider();
                        machineId = BitConverter.ToString(md.ComputeHash(buffer)).Replace("-", "");
                    }
                    return machineId;
                }
            }
        }

        /// <summary>
        /// Get/set the <see cref="T:iTrading.Core.Kernel.IProgress" /> implementation.
        /// The default implementation is a dialog box poping up. 
        /// An exclusive lock is set too: <see cref="M:iTrading.Core.Kernel.IProgress.Initialise(System.Int32,System.Boolean)" /> may only be called
        /// once at a time, <see cref="M:iTrading.Core.Kernel.IProgress.Terminate" /> has to be called prios subsequent calls to 
        /// <see cref="M:iTrading.Core.Kernel.IProgress.Initialise(System.Int32,System.Boolean)" />.
        /// </summary>
        public static IProgress Progress
        {
            get
            {
                return iProgress;
            }
            set
            {
                if (iProgress != null)
                {
                    iProgress.Terminate();
                }
                iProgress = value;
            }
        }

        /// <summary>
        /// Get the ticksize value of 1/128.
        /// </summary>
        public static double TickSize128
        {
            get
            {
                return 0.0078125;
            }
        }

        /// <summary>
        /// Get the ticksize value of 1/32.
        /// </summary>
        public static double TickSize32
        {
            get
            {
                return 0.03125;
            }
        }

        /// <summary>
        /// Get the ticksize value of 1/64.
        /// </summary>
        public static double TickSize64
        {
            get
            {
                return 0.015625;
            }
        }

        /// <summary>
        /// Get the ticksize value of 1/8.
        /// </summary>
        public static double TickSize8
        {
            get
            {
                return 0.125;
            }
        }

        /// <summary>
        /// Get the TradeMagic trace switch.
        /// </summary>
        public static TMTraceSwitch TraceSwitch
        {
            get
            {
                return traceSwitch;
            }
        }

        /// <summary>
        /// Get/set user id. 
        /// If user id = "" on license check, then it will be read from the Config.xml file.
        /// </summary>
        public static string UserId
        {
            get
            {
                if (userId.Length == 0)
                {
                    try
                    {
                        string installDir = InstallDir;
                        Trace.Assert(System.IO.File.Exists(installDir + @"\Config.xml"), "TradeMagic is not installed properly, Config.xml file is missing");
                        XmlDocument document = new XmlDocument();
                        XmlTextReader reader = new XmlTextReader(installDir + @"\Config.xml");
                        document.Load(reader);
                        reader.Close();
                        userId = document["TradeMagic"]["User"]["Id"].InnerText;
                    }
                    catch (Exception)
                    {
                        userId = "";
                    }
                }
                return userId;
            }
            set
            {
                userId = value;
            }
        }

        /// <summary>
        /// Get/set vendor code. The vendor code should be set by TradeMagic partners at application startup.
        /// </summary>
        public static string VendorCode
        {
            get
            {
                return vendorCode;
            }
            set
            {
                if (vendorCode.Length == 0)
                {
                    vendorCode = value;
                }
            }
        }
    }
}

