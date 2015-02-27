using iTrading.Core.Data;

namespace iTrading.Core.Kernel
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// This object will be passed as argument to the <see cref="T:iTrading.Core.Kernel.ConnectionStatusEventHandler" />.
    /// </summary>
    [ComVisible(false)]
    public class ConnectionStatusEventArgs : ITradingBaseEventArgs, IComConnectionStatusEventArgs, ITradingSerializable
    {
        private int clientId;
        private iTrading.Core.Kernel.Connection connection;
        private iTrading.Core.Kernel.ConnectionStatusId connectionStatusId;
        private string customText;
        private iTrading.Core.Kernel.ConnectionStatusId oldConnectionStatusId;
        private iTrading.Core.Kernel.ConnectionStatusId oldSecondaryConnectionStatusId;
        private iTrading.Core.Kernel.ConnectionStatusId secondaryConnectionStatusId;

        /// <summary></summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public ConnectionStatusEventArgs(Bytes bytes, int version) : base(bytes, version)
        {
            this.customText = "";
            this.clientId = bytes.ReadInt32();
            this.connection = (iTrading.Core.Kernel.Connection) bytes.ReadSerializable();
            this.connectionStatusId = bytes.ReadConnectionStatus().Id;
            this.customText = bytes.ReadString();
            this.oldConnectionStatusId = (iTrading.Core.Kernel.ConnectionStatusId) bytes.ReadInt32();
            this.oldSecondaryConnectionStatusId = (iTrading.Core.Kernel.ConnectionStatusId) bytes.ReadInt32();
            this.secondaryConnectionStatusId = bytes.ReadConnectionStatus().Id;
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        /// <param name="currentConnection"></param>
        /// <param name="errorCode"></param>
        /// <param name="connectionStatusId"></param>
        /// <param name="secondaryConnectionStatusId"></param>
        /// <param name="nativeError"></param>
        /// <param name="clientId"></param>
        /// <param name="customText"></param>
        public ConnectionStatusEventArgs(iTrading.Core.Kernel.Connection currentConnection, ErrorCode errorCode, string nativeError, iTrading.Core.Kernel.ConnectionStatusId connectionStatusId, iTrading.Core.Kernel.ConnectionStatusId secondaryConnectionStatusId, int clientId, string customText) : base(currentConnection, errorCode, nativeError)
        {
            this.customText = "";
            this.clientId = (clientId == 0) ? currentConnection.ClientId : clientId;
            this.connection = currentConnection;
            this.connectionStatusId = connectionStatusId;
            this.customText = (customText.Length == 0) ? currentConnection.CustomText : customText;
            this.oldConnectionStatusId = this.connection.ConnectionStatusId;
            this.oldSecondaryConnectionStatusId = this.connection.SecondaryConnectionStatusId;
            this.secondaryConnectionStatusId = secondaryConnectionStatusId;
        }

        /// <summary>For internal use only.</summary>
        protected internal override void Process()
        {
            Trace.Assert(base.Request == base.Request.Connection, "Cbi.ConnectionStatusEventArgs.Process");
            if (Globals.TraceSwitch.Connect)
            {
                Trace.WriteLine("(" + base.Request.Connection.IdPlus + ") Cbi.ConnectionStatusEventArgs.Process: " + this.ConnectionStatusId.ToString() + " " + this.SecondaryConnectionStatusId.ToString());
            }
            
                this.Connection.OnConnectionStatusChange(this.Connection, this);
            
        }

        /// <summary>
        /// Serialize the current object.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public override void Serialize(Bytes bytes, int version)
        {
            base.Serialize(bytes, version);
            bytes.Write(this.clientId);
            bytes.WriteSerializable(this.connection);
            bytes.Write(ConnectionStatus.All[this.connectionStatusId]);
            bytes.Write(this.customText);
            bytes.Write((int) this.oldConnectionStatusId);
            bytes.Write((int) this.oldSecondaryConnectionStatusId);
            bytes.Write(ConnectionStatus.All[this.secondaryConnectionStatusId]);
        }

        /// <summary>
        /// Gets <see cref="P:iTrading.Core.Kernel.ConnectionStatusEventArgs.ClassId" /> of current object.
        /// </summary>
        public override iTrading.Core.Kernel.ClassId ClassId
        {
            get
            {
                return iTrading.Core.Kernel.ClassId.ConnectionStatusEventArgs;
            }
        }

        /// <summary>
        /// Identifies the client session at the TradeMagic server.
        /// 0, when broker adapter is executed locally.
        /// </summary>
        public int ClientId
        {
            get
            {
                return this.clientId;
            }
        }

        /// <summary>
        /// The affected connection.
        /// </summary>
        public iTrading.Core.Kernel.Connection Connection
        {
            get
            {
                return this.connection;
            }
        }

        /// <summary>
        /// Connection status.
        /// </summary>
        public iTrading.Core.Kernel.ConnectionStatusId ConnectionStatusId
        {
            get
            {
                return this.connectionStatusId;
            }
        }

        /// <summary>
        /// Custom text.
        /// </summary>
        public string CustomText
        {
            get
            {
                return this.customText;
            }
        }

        /// <summary>
        /// Old/previous connection status.
        /// </summary>
        public iTrading.Core.Kernel.ConnectionStatusId OldConnectionStatusId
        {
            get
            {
                return this.oldConnectionStatusId;
            }
        }

        /// <summary>
        /// Old/previous connection status or the secondary server(s), e.g. price feed server.
        /// </summary>
        public iTrading.Core.Kernel.ConnectionStatusId OldSecondaryConnectionStatusId
        {
            get
            {
                return this.oldSecondaryConnectionStatusId;
            }
        }

        /// <summary>
        /// Connection status of the secondars server(s), e.g. price feed server.
        /// </summary>
        public iTrading.Core.Kernel.ConnectionStatusId SecondaryConnectionStatusId
        {
            get
            {
                return this.secondaryConnectionStatusId;
            }
        }

        /// <summary>
        /// Version number.
        /// </summary>
        public override int Version
        {
            get
            {
                return 2;
            }
        }
    }
}

