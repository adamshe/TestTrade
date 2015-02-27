namespace iTrading.Track
{
    using iTrading.Core.Kernel;
    using System;
    using System.Diagnostics;
    

    internal class NewsRequest : iTrading.Track.Request
    {
        internal NewsRequest(Adapter adapter) : base(adapter)
        {
        }

        internal iTrading.Track.ErrorCode Halt()
        {
            if (Globals.TraceSwitch.News)
            {
                Trace.WriteLine("(" + base.Adapter.connection.IdPlus + ") Track.NewsRequest.Halt");
            }
            iTrading.Track.ErrorCode code = (iTrading.Track.ErrorCode) Api.RequestNewsHeadlines(base.Rqn, '\x0002');
            if ((code != iTrading.Track.ErrorCode.NoError) && (code != iTrading.Track.ErrorCode.NotConnected))
            {
                base.Adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(base.Adapter.connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Track.NewsRequest.Process: error " + code));
            }
            return code;
        }

        internal override void Process(MessageCode msgCode, ByteReader reader)
        {
            if (Globals.TraceSwitch.News)
            {
                Trace.WriteLine("(" + base.Adapter.connection.IdPlus + ") Track.NewsRequest.Process");
            }
            int length = reader.ReadByte();
            string input = reader.ReadString(length);
            if (input.Length != 0)
            {
                string[] strArray = base.Adapter.regExSpaces.Replace(input, " ").Split(new char[] { ' ' });
                string storyId = strArray[0];
                int index = base.Adapter.regExDigits.Match(strArray[1]).Index;
                string str3 = "";
                string mapId = "";
                if (index > 0)
                {
                    mapId = strArray[1].Substring(0, index - 1);
                    str3 = strArray[1].Substring(index);
                }
                else
                {
                    mapId = strArray[1];
                    str3 = strArray[2];
                }
                NewsItemType itemType = base.Adapter.connection.NewsItemTypes.FindByMapId(mapId);
                if (itemType != null)
                {
                    DateTime date = base.Adapter.connection.Now.Date;
                    DateTime time = base.Adapter.connection.Now.Date;
                    try
                    {
                        if (str3.IndexOf(".") >= 0)
                        {
                            string[] strArray2 = str3.Split(new char[] { '.' });
                            time = new DateTime(date.Year, date.Month, date.Day, Convert.ToInt32(strArray2[0]), Convert.ToInt32(strArray2[1]), 0);
                        }
                        else
                        {
                            string[] strArray3 = str3.Split(new char[] { ':' });
                            int hour = Convert.ToInt32(strArray3[0]);
                            if (hour >= 12)
                            {
                                hour -= 12;
                            }
                            time = new DateTime(date.Year, date.Month, date.Day, hour, Convert.ToInt32(strArray3[1]), 0);
                        }
                    }
                    catch
                    {
                    }
                    if (base.Adapter.newsIds[storyId] == null)
                    {
                        NewsStoryRequest request = new NewsStoryRequest(base.Adapter, input, storyId, itemType, time);
                        base.Adapter.newsIds.Add(storyId, request);
                        request.Send();
                    }
                }
            }
        }

        internal override iTrading.Track.ErrorCode Send()
        {
            if (Globals.TraceSwitch.News)
            {
                Trace.WriteLine("(" + base.Adapter.connection.IdPlus + ") Track.NewsRequest.Send");
            }
            iTrading.Track.ErrorCode code = (iTrading.Track.ErrorCode) Api.RequestNewsHeadlines(base.Rqn, '\x0001');
            if (code != iTrading.Track.ErrorCode.NoError)
            {
                base.Adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(base.Adapter.connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Track.NewsRequest.Send: error " + code));
            }
            return code;
        }
    }
}

