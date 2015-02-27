namespace iTrading.Track
{
    using System;
    using System.Collections.Specialized;

    internal abstract class Request
    {
        private iTrading.Track.Adapter adapter;
        private int rqn;

        internal Request(iTrading.Track.Adapter adapter)
        {
            this.adapter = adapter;
            lock (typeof(Request))
            {
                this.rqn = adapter.nextRqn++;
            }
            lock (adapter.requests)
            {
                adapter.requests.Add(this);
            }
        }

        internal double CalculateFormatValue(double toFormat, byte format)
        {
            int num = 1;
            if (format < 10)
            {
                num = (int) Math.Pow(10.0, (double) format);
            }
            else
            {
                num = (int) Math.Pow(2.0, (double) (format - 10));
            }
            return (toFormat / ((double) num));
        }

        internal abstract void Process(MessageCode msgCode, ByteReader reader);
        internal abstract ErrorCode Send();
        internal static string ToString(StringCollection fields)
        {
            string str = "";
            foreach (string str2 in fields)
            {
                str = str + ((str.Length > 0) ? "|" : "") + str2;
            }
            return str;
        }

        internal iTrading.Track.Adapter Adapter
        {
            get
            {
                return this.adapter;
            }
        }

        internal int Rqn
        {
            get
            {
                return this.rqn;
            }
        }
    }
}

