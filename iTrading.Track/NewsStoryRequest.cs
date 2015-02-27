namespace iTrading.Track
{
    using System;
    using System.Diagnostics;
    using iTrading.Core.Kernel;

    internal class NewsStoryRequest : iTrading.Track.Request
    {
        private string headLine;
        private NewsItemType itemType;
        private bool sent;
        private string storyId;
        private string text;
        private DateTime time;

        internal NewsStoryRequest(Adapter adapter, string headLine, string storyId, NewsItemType itemType, DateTime time) : base(adapter)
        {
            this.sent = false;
            this.text = "";
            this.headLine = headLine;
            this.itemType = itemType;
            this.storyId = storyId;
            this.time = time;
        }

        internal override void Process(MessageCode msgCode, ByteReader reader)
        {
            if (Globals.TraceSwitch.News)
            {
                Trace.WriteLine("(" + base.Adapter.connection.IdPlus + ") Track.NewsStoryRequest.Process");
            }
            iTrading.Track.ErrorCode code = (iTrading.Track.ErrorCode) reader.ReadByte();
            if (code != iTrading.Track.ErrorCode.NoError)
            {
                base.Adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(base.Adapter.connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Track.NewsStoryRequest.Process: error " + code));
            }
            else
            {
                bool flag = reader.ReadByte() == 0;
                int length = reader.ReadByte();
                string str = reader.ReadString(length);
                if (str.StartsWith("***  end"))
                {
                    str = "";
                }
                this.text = this.text + str;
                if (!flag && !this.sent)
                {
                    this.text = base.Adapter.regExProvider.Replace(this.text, "");
                    if (this.text.StartsWith("*** Story Not Found"))
                    {
                        this.text = this.headLine;
                    }
                    if (this.text.Replace(" ", "").Length == 0)
                    {
                        this.text = this.headLine;
                    }
                    base.Adapter.connection.ProcessEventArgs(new NewsEventArgs(base.Adapter.connection, iTrading.Core.Kernel.ErrorCode.NoError, "", this.storyId, this.itemType, this.time, this.headLine, this.text));
                    this.sent = true;
                }
            }
        }

        internal override iTrading.Track.ErrorCode Send()
        {
            if (Globals.TraceSwitch.News)
            {
                Trace.WriteLine("(" + base.Adapter.connection.IdPlus + ") Track.NewsStoryRequest.Send");
            }
            iTrading.Track.ErrorCode code = (iTrading.Track.ErrorCode) Api.RequestNewsStory(base.Rqn, this.storyId);
            if (code != iTrading.Track.ErrorCode.NoError)
            {
                base.Adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(base.Adapter.connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Track.NewsStoryRequest.Send: error " + code));
            }
            return code;
        }
    }
}

