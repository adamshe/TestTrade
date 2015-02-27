using System.Diagnostics;
using System.Runtime.InteropServices;
using iTrading.Core.Kernel;
using iTrading.Core.Interface;

namespace iTrading.Core.Data
{
    /// <summary>
    /// Represents an update operation of a <see cref="P:iTrading.Core.Data.BarUpdateEventArgs.Quotes" /> object. An instance of this class will be passed as argument 
    /// to a <see cref="T:iTrading.Core.Data.BarUpdateEventArgs" />.
    /// </summary>
    [ClassInterface(ClassInterfaceType.None), Guid("996E2C88-AAC2-4c04-B66A-045E9E63883C")]
    public class BarUpdateEventArgs : ITradingBaseEventArgs, IComBarUpdateEventArgs
    {
        private int first;
        private int last;
        private iTrading.Core.Kernel.Operation operation;
        private Quotes quotes;

        /// <summary></summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public BarUpdateEventArgs(Bytes bytes, int version) : base(bytes, version)
        {
            this.first = bytes.ReadInt32();
            this.last = bytes.ReadInt32();
            this.operation = (iTrading.Core.Kernel.Operation) bytes.ReadInt32();
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        /// <param name="currentConnection"></param>
        /// <param name="errorCode"></param>
        /// <param name="nativeError"></param>
        /// <param name="operation"></param>
        /// <param name="quotes"></param>
        /// <param name="first"></param>
        /// <param name="last"></param>
        public BarUpdateEventArgs(Connection currentConnection, ErrorCode errorCode, string nativeError, iTrading.Core.Kernel.Operation operation, Quotes quotes, int first, int last) : base(currentConnection, errorCode, nativeError)
        {
            this.first = first;
            this.last = last;
            this.operation = operation;
            this.quotes = quotes;
        }

        /// <summary>For internal use only.</summary>
        protected internal  override void Process()
        {
            Trace.Assert(base.Request == base.Request.Connection, "Cbi.BarUpdateEventArgs.Process");
            if (Globals.TraceSwitch.Quotes)
            {
                Trace.WriteLine(string.Concat(new object[] { "(", base.Request.Connection.IdPlus, ") Cbi.BarUpdateEventArgs.Process: symbol='", this.Quotes.Symbol.FullName, "' first=", this.first, " last=", this.last, " error=", base.Error, " nativeError='", base.NativeError, "'" }));
            }
          
            base.Request.Connection.OnBarChange( base.Request.Connection, this);
            this.quotes.initialized = true;
        }

        /// <summary>
        /// Serialize the current object.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="version"></param>
        public override void Serialize(Bytes bytes, int version)
        {
            base.Serialize(bytes, version);
            bytes.Write(this.first);
            bytes.Write(this.last);
            bytes.Write((int) this.operation);
        }

        /// <summary>
        /// Gets <see cref="P:TradeMagic.Data.BarUpdateEventArgs.ClassId" /> of current object.
        /// </summary>
        public override iTrading.Core.Kernel.ClassId ClassId
        {
            get
            {
                return iTrading.Core.Kernel.ClassId.BarUpdateEventArgs;
            }
        }

        /// <summary>
        /// Index of last bar inserted/updated. <seealso cref="P:TradeMagic.Data.BarUpdateEventArgs.Operation" />.
        /// </summary>
        public int First
        {
            get
            {
                return this.first;
            }
        }

        /// <summary>
        /// Index of last bar inserted/updated. <seealso cref="P:TradeMagic.Data.BarUpdateEventArgs.Operation" />.
        /// </summary>
        public int Last
        {
            get
            {
                return this.last;
            }
        }

        /// <summary>
        /// The update operation. 
        /// <see cref="F:iTrading.Core.Kernel.Operation.Insert" />: <see cref="P:TradeMagic.Data.BarUpdateEventArgs.Last" /> holds the index of the first bar inserted,
        /// <see cref="P:TradeMagic.Data.BarUpdateEventArgs.Last" /> holds the index of the last bar inserted.
        /// <see cref="F:iTrading.Core.Kernel.Operation.Update" />: <see cref="P:TradeMagic.Data.BarUpdateEventArgs.Last" /> holds the index of the bar updated.
        /// </summary>
        public iTrading.Core.Kernel.Operation Operation
        {
            get
            {
                return this.operation;
            }
        }

        /// <summary>
        /// The <see cref="P:TradeMagic.Data.BarUpdateEventArgs.Quotes" /> object the <see cref="T:TradeMagic.Data.Bar" /> belongs to.
        /// </summary>
        public Quotes Quotes
        {
            get
            {
                return this.quotes;
            }
        }
    }
}