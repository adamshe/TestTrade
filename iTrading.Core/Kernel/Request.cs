using iTrading.Core.Data;

namespace iTrading.Core.Kernel
{
    using System;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Base class for all requests. <see cref="T:iTrading.Core.Kernel.Request" /> should not be constructed or called directly.
    /// The class is mainly for internal use. Please call the appropriate methods of e.g. <see cref="T:iTrading.Core.Kernel.Connection" />,
    /// <see cref="T:iTrading.Core.Kernel.Account" /> or <see cref="T:iTrading.Core.Kernel.Order" />.
    /// </summary>
    [ClassInterface(ClassInterfaceType.None), Guid("654834B5-2431-400b-BF16-9A419A0F6E65")]
    public class Request : IComRequest, ITradingSerializable
    {
        private object adapterLink;
        internal iTrading.Core.Kernel.Connection connection;
        private object customLink;
        private int id;
        private int mapId;
        internal static int nextId = 0;
        internal iTrading.Core.Kernel.Operation operation;

        /// <summary>For internal use only.</summary>
        protected Request(iTrading.Core.Kernel.Connection currentConnection)
        {
            this.adapterLink = null;
            this.customLink = null;
            this.connection = null;
            this.id = 0;
            this.mapId = -1;
            this.operation = iTrading.Core.Kernel.Operation.Insert;
            this.connection = currentConnection;
            lock (typeof(Request))
            {
                this.id = nextId++;
            }
        }

        /// <summary></summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public Request(Bytes bytes, int version)
        {
            this.adapterLink = null;
            this.customLink = null;
            this.connection = null;
            this.id = 0;
            this.mapId = -1;
            this.operation = iTrading.Core.Kernel.Operation.Insert;
            this.connection = bytes.Connection;
            this.id = bytes.ReadInt32();
            this.operation = (iTrading.Core.Kernel.Operation) bytes.ReadInt32();
        }

        /// <summary>
        /// Serialize the current object.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public virtual void Serialize(Bytes bytes, int version)
        {
            bytes.Write(this.id);
            bytes.Write((int) this.operation);
        }

        /// <summary>For internal use only. Do not access or change this property.
        /// Use <see cref="P:iTrading.Core.Kernel.Request.CustomLink" /> to link an object to this request.</summary>
        public object AdapterLink
        {
            get
            {
                return this.adapterLink;
            }
            set
            {
                this.adapterLink = value;
            }
        }

        /// <summary>
        /// Gets <see cref="P:iTrading.Core.Kernel.Request.ClassId" /> of current object.
        /// </summary>
        public virtual iTrading.Core.Kernel.ClassId ClassId
        {
            get
            {
                return iTrading.Core.Kernel.ClassId.Request;
            }
        }

        /// <summary>
        /// Connection where market data is requested from.
        /// </summary>
        public iTrading.Core.Kernel.Connection Connection
        {
            get
            {
                return this.connection;
            }
        }

        /// <summary>
        /// Custom link. This property may be used to attach any object to the request.
        /// </summary>
        public object CustomLink
        {
            get
            {
                return this.customLink;
            }
            set
            {
                this.customLink = value;
            }
        }

        /// <summary>
        /// Identifies the request.
        /// </summary>
        public int Id
        {
            get
            {
                return this.id;
            }
        }

        /// <summary>For internal use only. Do not access or change this property.</summary>
        public int MapId
        {
            get
            {
                return this.mapId;
            }
            set
            {
                this.mapId = value;
            }
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        public iTrading.Core.Kernel.Operation Operation
        {
            get
            {
                return this.operation;
            }
        }

        /// <summary>
        /// Version number.
        /// </summary>
        public virtual int Version
        {
            get
            {
                return 1;
            }
        }
    }
}

