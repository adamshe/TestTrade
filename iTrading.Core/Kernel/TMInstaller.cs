namespace iTrading.Core.Kernel
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Configuration.Install;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Windows.Forms;
    using System.Xml;

    /// <summary>
    /// TradeMagic Cbi installer.
    /// </summary>
    [RunInstaller(true)]
    public class TMInstaller : Installer
    {
        private Container components = null;

        /// <summary>
        /// </summary>
        public TMInstaller()
        {
            this.InitializeComponent();
        }

        /// <summary> 
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Erforderliche Methode f|r die Designerunterst|tzung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor gedndert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new Container();
        }

        /// <summary>
        /// Installer method.
        /// </summary>
        /// <param name="stateSaver"></param>
        public override void Install(IDictionary stateSaver)
        {
            if (!System.IO.File.Exists(Globals.InstallDir + @"db\Symbols.txt"))
            {
                System.IO.File.Copy(Globals.InstallDir + @"db\backup\Symbols.txt", Globals.InstallDir + @"db\Symbols.txt");
            }
            if (Globals.UserId.Length == 0)
            {
                RegisterForm form;
                do
                {
                    form = new RegisterForm();
                    form.WindowState = FormWindowState.Normal;
                }
                while (form.ShowDialog() == DialogResult.Retry);
            }
            Process process = new Process();
            process.StartInfo.Arguments = "/Register";
            process.StartInfo.FileName = Globals.InstallDir + @"bin\TradeMagic.Misc.exe";
            process.StartInfo.UseShellExecute = false;
            process.Start();
            process.WaitForExit();
        }

        /// <summary>
        /// Register user.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="license"></param>
        /// <param name="vendorCode">Optional, could be ""</param>
        /// <returns></returns>
        public static void Register(string userId, LicenseTypeId license, string vendorCode)
        {
            if (vendorCode.Length == 0)
            {
                vendorCode = "none";
            }
            string message = "";
            try
            {
                WebRequest request = WebRequest.Create(string.Concat(new object[] { "http://www.trademagic.net/tools/RegisterLicense.php?id=", userId, "&lc=", (int) license, "&vc=", vendorCode, "&mc=", Globals.MachineId }));
                request.Timeout = 0x2710;
                WebResponse response = request.GetResponse();
                string s = new StreamReader(response.GetResponseStream()).ReadToEnd();
                s = s.Substring(0, s.IndexOf("</TradeMagic>") + "</TradeMagic>".Length);
                response.Close();
                XmlDocument document = new XmlDocument();
                XmlTextReader reader = new XmlTextReader(new StringReader(s));
                document.Load(reader);
                reader.Close();
                message = document["TradeMagic"].InnerText;
            }
            catch (WebException)
            {
                MessageBox.Show("Unable to register now. Please try again later", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            catch (Exception exception)
            {
                throw new TMException(ErrorCode.Panic, "Failed to register id '" + userId + "': " + exception.Message);
            }
            if (message.Length > 0)
            {
                throw new TMException(ErrorCode.NotRegistered, message);
            }
            try
            {
                string installDir = Globals.InstallDir;
                Trace.Assert(System.IO.File.Exists(installDir + @"\Config.xml"), "TradeMagic is not installed properly, Config.xml file is missing");
                XmlDocument document2 = new XmlDocument();
                XmlTextReader reader2 = new XmlTextReader(installDir + @"\Config.xml");
                document2.Load(reader2);
                reader2.Close();
                if (document2["TradeMagic"] == null)
                {
                    document2.AppendChild(document2.CreateElement("TradeMagic"));
                }
                if (document2["TradeMagic"]["User"] == null)
                {
                    document2["TradeMagic"].AppendChild(document2.CreateElement("User"));
                }
                if (document2["TradeMagic"]["User"]["Id"] == null)
                {
                    document2["TradeMagic"]["User"].AppendChild(document2.CreateElement("Id"));
                }
                document2["TradeMagic"]["User"]["Id"].InnerText = userId;
                document2.Save(installDir + @"\Config.xml");
            }
            catch (Exception exception2)
            {
                throw new TMException(ErrorCode.Panic, "Failed to register id '" + userId + "': " + exception2.Message);
            }
        }

        /// <summary>
        /// Uninstaller method.
        /// </summary>
        /// <param name="savedState"></param>
        public override void Uninstall(IDictionary savedState)
        {
            Process process = new Process();
            process.StartInfo.Arguments = "/Unregister";
            process.StartInfo.FileName = Globals.InstallDir + @"bin\TradeMagic.Misc.exe";
            process.StartInfo.UseShellExecute = false;
            process.Start();
            process.WaitForExit();
        }
    }
}

