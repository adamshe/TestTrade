namespace iTrading.Gui
{
    using System;
    using System.Diagnostics;
    using System.IO;

    internal class TraceListener : DefaultTraceListener
    {
        private StreamWriter logFile;
        private TraceLog traceLog;

        internal TraceListener(TraceLog traceLog, StreamWriter logFile)
        {
            this.logFile = logFile;
            this.traceLog = traceLog;
        }

        public override void Close()
        {
            try
            {
                if (this.logFile != null)
                {
                    this.logFile.Close();
                    this.logFile = null;
                }
                base.Close();
            }
            catch (Exception)
            {
            }
        }

        public override void Write(string msg)
        {
            msg = DateTime.Now.ToString("HH:mm:ss:fff ") + msg;
            base.Write(msg);
            this.logFile.Write(msg);
            this.logFile.Flush();
            if (!this.traceLog.ToFileOnly)
            {
                this.traceLog.Trace(msg);
            }
        }

        public override void WriteLine(string msg)
        {
            msg = DateTime.Now.ToString("HH:mm:ss:fff ") + msg;
            base.WriteLine(msg);
            this.logFile.WriteLine(msg);
            this.logFile.Flush();
            if (!this.traceLog.ToFileOnly)
            {
                this.traceLog.TraceLine(msg);
            }
        }
    }
}

