using iTrading.Core.Data;

namespace iTrading.Core.Kernel
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// A news item. This object will be passed as argument to a <see cref="T:iTrading.Core.Kernel.NewsEventHandler" />.
    /// </summary>
    [ComVisible(false)]
    public class NewsEventArgs : ITradingBaseEventArgs, IComNewsEventArgs, ITradingSerializable
    {
        private string headLine;
        private string id;
        private NewsItemType itemType;
        private string text;
        private DateTime time;

        /// <summary></summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public NewsEventArgs(Bytes bytes, int version) : base(bytes, version)
        {
            this.headLine = bytes.ReadString();
            this.id = bytes.ReadString();
            this.itemType = bytes.ReadNewsItemType();
            this.text = bytes.ReadString();
            this.time = bytes.ReadDateTime();
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="errorCode"></param>
        /// <param name="nativeError"></param>
        /// <param name="id"></param>
        /// <param name="itemType"></param>
        /// <param name="newsText"></param>
        /// <param name="headLine"></param>
        /// <param name="time"></param>
        public NewsEventArgs(Connection connection, ErrorCode errorCode, string nativeError, string id, NewsItemType itemType, DateTime time, string headLine, string newsText) : base(connection, errorCode, nativeError)
        {
            this.headLine = headLine;
            this.id = id;
            this.itemType = itemType;
            this.text = newsText;
            this.time = time;
        }

        /// <summary>For internal use only.</summary>
        protected internal override void Process()
        {
            Trace.Assert(base.Request == base.Request.Connection, "Cbi.NewsEventArgs.Process");
            if (Globals.TraceSwitch.News)
            {
                Trace.WriteLine("(" + base.Request.Connection.IdPlus + ") Cbi.NewsEventArgs: " + this.id);
            }
            if (base.Request.Connection.News.newsEventHandler != null)
            {
                base.Request.Connection.News.newsEventHandler(base.Request, this);
            }
        }

        /// <summary>
        /// Serialize the current object.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public override void Serialize(Bytes bytes, int version)
        {
            base.Serialize(bytes, version);
            bytes.Write(this.headLine);
            bytes.Write(this.id);
            bytes.Write(this.itemType);
            bytes.Write(this.text);
            bytes.Write(this.time);
        }

        /// <summary>
        /// Gets <see cref="P:iTrading.Core.Kernel.NewsEventArgs.ClassId" /> of current object.
        /// </summary>
        public override iTrading.Core.Kernel.ClassId ClassId
        {
            get
            {
                return iTrading.Core.Kernel.ClassId.NewsEventArgs;
            }
        }

        /// <summary>
        /// News headline
        /// </summary>
        public string HeadLine
        {
            get
            {
                return this.headLine;
            }
        }

        /// <summary>
        /// Identifies the news item.
        /// </summary>
        public string Id
        {
            get
            {
                return this.id;
            }
        }

        /// <summary>
        /// Identifies the news item type.
        /// </summary>
        public NewsItemType ItemType
        {
            get
            {
                return this.itemType;
            }
        }

        /// <summary>
        /// Text of news item.
        /// </summary>
        public string Text
        {
            get
            {
                return this.text;
            }
        }

        /// <summary>
        /// Time of news item.
        /// </summary>
        public DateTime Time
        {
            get
            {
                return this.time;
            }
        }

        /// <summary>
        /// Version number.
        /// </summary>
        public override int Version
        {
            get
            {
                return 1;
            }
        }
    }
}

