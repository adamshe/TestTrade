namespace iTrading.Test
{
    //using NUnit.Framework;
    using Assertion = Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.IO;
    using System.Threading;
    using System.Windows.Forms;
    using System.Xml;
    using iTrading.Core.Kernel;
    using iTrading.Gui;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    /// <summary>
    /// Global settings and startup method.
    /// </summary>
    public sealed class Globals
    {
        internal static bool drivenByNUnit = true;
        internal static bool noCleanUpOnConnect = false;
        internal static bool noServerStartUp = false;
        internal static ArrayList processes = new ArrayList();
        internal static Process serverProcess = null;
        internal static bool startFromConfigFile = false;
        internal static Thread startProcesses = null;
        internal static TerminateForm terminateForm = null;
        private static TraceLogForm traceForm = null;

        private Globals()
        {
        }

        /// <summary>
        /// Check test assertion and trace test fail.
        /// </summary>
        /// <param name="remark"></param>
        /// <param name="condition"></param>
        public static void Assert(string remark, bool condition)
        {
            if (!condition)
            {
                Trace.WriteLine("Test failed: " + remark);
                if (drivenByNUnit)
                {
                    Assertion.Assert.Fail(remark);
                }
                else
                {
                    Debugger.Break();
                }
            }
        }

        /// <summary>
        /// Check test assertion and trace test fail.
        /// </summary>
        /// <param name="remark"></param>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        /// <param name="delta"></param>
        [TestMethod]
        public static void AssertEquals(string remark, double actual, double expected, double delta)
        {
            try
            {
                AssertEquals(remark, expected, actual, delta);
            }
            catch
            {
                Trace.WriteLine("Test failed: " + remark);
                if (drivenByNUnit)
                {
                    Assertion.Assert.Fail(remark);
                }
                else
                {
                    Debugger.Break();
                }
            }
        }

        /// <summary>
        /// Compare two text files for identity.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="refFile"></param>
        /// <returns></returns>
        public static bool CompareFiles(string file, string refFile)
        {
            StreamReader reader = File.OpenText(file);
            StreamReader reader2 = File.OpenText(refFile);
            while ((reader.Peek() != -1) && (reader2.Peek() != -1))
            {
                if (reader.ReadLine() != reader2.ReadLine())
                {
                    reader.Close();
                    reader2.Close();
                    return false;
                }
            }
            if (reader.Peek() == reader2.Peek())
            {
                reader.Close();
                reader2.Close();
                return true;
            }
            reader.Close();
            reader2.Close();
            return false;
        }

        /// <summary>
        /// Startup method.
        /// </summary>
        /// <param name="args"></param>
        [STAThread]
        private static void Main(string[] args)
        {
            drivenByNUnit = false;
            Thread.CurrentThread.Name = "TM Test";
            iTrading.Core.Kernel.Globals.AppId = "TradeMagic Test";
            foreach (string str in System.Environment.GetCommandLineArgs())
            {
                switch (str)
                {
                    case "/NoCleanUpOnConnect":
                        noCleanUpOnConnect = true;
                        break;

                    case "/NoServerStartUp":
                        noServerStartUp = true;
                        break;

                    case "/StartFromConfigFile":
                        startFromConfigFile = true;
                        break;
                }
            }
            if (startFromConfigFile)
            {
                StartFromConfigFile(false);
                Application.Exit();
            }
            else
            {
                Application.Run(new MainForm());
            }
        }

        /// <summary>
        /// Create temporary file.
        /// </summary>
        /// <returns></returns>
        public static string MakeTempFile()
        {
            string str2;
            string str = (string) System.Environment.GetEnvironmentVariables()["TEMP"];
            Random random = new Random(DateTime.Now.Millisecond);
            do
            {
                str2 = str + @"\" + random.Next();
            }
            while (File.Exists(str2) || Directory.Exists(str2));
            File.Create(str2).Close();
            return str2;
        }

        internal static void Sleep(int milliSeconds)
        {
            int millisecondsTimeout = 10;
            while (milliSeconds > 0)
            {
                Thread.Sleep(millisecondsTimeout);
                Application.DoEvents();
                milliSeconds -= millisecondsTimeout;
            }
        }

        internal static void StartFromConfigFile(bool showTerminateForm)
        {
            XmlDocument document = new XmlDocument();
            XmlTextReader reader = new XmlTextReader(iTrading.Core.Kernel.Globals.InstallDir + @"\Config.xml");
            document.Load(reader);
            reader.Close();
            string str = "TradeMagic Test \"";
            TestBase base2 = null;
            switch (document["TradeMagic"]["Test"]["Last"]["TestSuite"].InnerText)
            {
                case "Connect":
                    base2 = new Connect();
                    goto Label_0121;

                case "Data":
                    base2 = new DataTest();
                    goto Label_0121;

                case "IBConnect":
                    base2 = new IBConnect();
                    goto Label_0121;

                case "Indicator":
                    base2 = new IndicatorTest();
                    goto Label_0121;

                case "Quotes":
                    base2 = new Quotes();
                    break;

                case "Order":
                    base2 = new  iTrading.Test.Order();
                    break;

                case "Release":
                    base2 = new Release();
                    break;

                case "SimOrder":
                    base2 = new SimOrder();
                    break;
            }
        Label_0121:
            str = str + document["TradeMagic"]["Test"]["Last"]["TestSuite"].InnerText;
            base2.Broker = ProviderType.All.Find(document["TradeMagic"]["Test"]["Last"]["Broker"].InnerText);
            str = str + "/" + base2.Broker.Name;
            string str2 = "";
            switch (document["TradeMagic"]["Test"]["Last"]["Environment"].InnerText)
            {
                case "Local":
                    base2.Environment =  iTrading.Test.Environment.Local;
                    str2 = "Local";
                    break;

                case "Server":
                    base2.Environment =  iTrading.Test.Environment.Server;
                    str2 = "Server";
                    break;
            }
            str = str + "/" + str2;
            base2.Mode = ModeType.All.Find(document["TradeMagic"]["Test"]["Last"]["Mode"].InnerText);
            object obj2 = str + "/" + base2.Mode.Name;
            str = string.Concat(new object[] { obj2, "/", document["TradeMagic"]["Test"]["Last"]["TestCase"].InnerText, '"' });
            if (traceForm == null)
            {
                traceForm = new TraceLogForm();
                traceForm.AddToTray = true;
                traceForm.ModuleName = "Test";
                traceForm.Text = str;
                traceForm.Show();
            }
            if (showTerminateForm && !drivenByNUnit)
            {
                if (terminateForm != null)
                {
                    terminateForm.Close();
                }
                terminateForm = new TerminateForm();
                terminateForm.Show();
            }
            Trace.WriteLine("Test " + str + " started");
            try
            {
                switch (document["TradeMagic"]["Test"]["Last"]["TestCase"].InnerText)
                {
                    case "Multiple":
                        base2.Multiple();
                        break;

                    case "MultipleInfinite":
                        base2.MultipleInfinite();
                        break;

                    case "Single":
                        base2.Single();
                        break;

                    case "SingleInfinite":
                        base2.SingleInfinite();
                        break;
                }
            }
            catch (Exception exception)
            {
                string str3 = "";
                string remark = "Exception (" + exception.GetType().Name + "): " + exception.Message + "\r\n";
                StackTrace trace = new StackTrace(exception, true);
                for (int i = 0; i < trace.FrameCount; i++)
                {
                    StackFrame frame = trace.GetFrame(i);
                    obj2 = remark;
                    remark = string.Concat(new object[] { obj2, str3, frame.GetMethod().ToString(), ": ", frame.GetFileLineNumber(), ", ", frame.GetFileName(), "\r\n" });
                    str3 = str3 + "  ";
                }
                Assert(remark, false);
            }
            Trace.WriteLine("Test " + str + " terminated");
        }

        internal static void StartServer()
        {
            serverProcess = new Process();
            serverProcess.StartInfo.FileName = "TradeMagic.Server.exe";
            serverProcess.Start();
            serverProcess.WaitForInputIdle();
            Sleep(0x1388);
        }

        internal static void TerminateServer()
        {
            if (serverProcess != null)
            {
                try
                {
                    serverProcess.Kill();
                    serverProcess.WaitForExit();
                }
                catch (Exception)
                {
                }
            }
        }

        internal static int MilliSeconds2Sleep
        {
            get
            {
                return 500;
            }
        }
    }
}

