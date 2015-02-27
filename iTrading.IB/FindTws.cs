namespace iTrading.IB
{
    using System;
    using System.Collections;
    using System.Text;

    public class FindTws : HandlePopup
    {
        private ArrayList ignoreTwsInstances = new ArrayList();
        internal const string logonCaption = "New Login";
        private ArrayList runningTwsInstances = new ArrayList();

        protected override bool HandleWindow(int handle)
        {
            StringBuilder buf = new StringBuilder(0x100);
            WinApi.GetClassName(handle, buf, 0x100);
            if ((buf.ToString() == "jclient.gp") && WinApi.IsWindowVisible(handle))
            {
                StringBuilder builder2 = new StringBuilder(0x100);
                WinApi.GetWindowText(handle, builder2, 0x100);
                if (!this.ignoreTwsInstances.Contains(handle))
                {
                    this.runningTwsInstances.Add(handle);
                }
            }
            return true;
        }

        public override bool Run()
        {
            base.Run();
            return (this.runningTwsInstances.Count > 0);
        }

        public override void StartUp()
        {
            this.runningTwsInstances.Clear();
        }

        public ArrayList IgnoreTwsInstances
        {
            get
            {
                return (ArrayList) this.ignoreTwsInstances.Clone();
            }
            set
            {
                this.ignoreTwsInstances = value;
            }
        }

        public ArrayList RunningTwsInstances
        {
            get
            {
                return (ArrayList) this.runningTwsInstances.Clone();
            }
        }
    }
}

