namespace iTrading.Track
{
    using System;
    using System.Diagnostics;
    using iTrading.Core.Kernel;

    internal class NewsVendorsRequest : iTrading.Track.Request
    {
        internal NewsVendorsRequest(Adapter adapter) : base(adapter)
        {
        }

        internal override void Process(MessageCode msgCode, ByteReader reader)
        {
            if (Globals.TraceSwitch.News)
            {
                Trace.WriteLine("(" + base.Adapter.connection.IdPlus + ") Track.NewsVendorsRequest.Process");
            }
            reader.ReadByte();
            reader.ReadByte();
            string mapId = reader.ReadString(3).TrimEnd(new char[0]).TrimStart(new char[0]);
            reader.ReadString(2).TrimEnd(new char[0]).TrimStart(new char[0]);
            string str2 = reader.ReadString(14).TrimEnd(new char[0]).TrimStart(new char[0]);
            if (base.Adapter.connection.NewsItemTypes.FindByMapId(mapId) == null)
            {
                Trace.WriteLine("WARNING: News vendor '" + str2 + "' (" + mapId + ") not supported");
            }
        }

        internal override iTrading.Track.ErrorCode Send()
        {
            if (Globals.TraceSwitch.News)
            {
                Trace.WriteLine("(" + base.Adapter.connection.IdPlus + ") Track.NewsVendorsRequest.Send");
            }
            iTrading.Track.ErrorCode code = (iTrading.Track.ErrorCode) Api.RequestNewsVendors(base.Rqn);
            if (code != iTrading.Track.ErrorCode.NoError)
            {
                base.Adapter.connection.ProcessEventArgs(new ITradingErrorEventArgs(base.Adapter.connection, iTrading.Core.Kernel.ErrorCode.Panic, "", "Track.NewsVendorsRequest.Send: error " + code));
            }
            return code;
        }
    }
}

