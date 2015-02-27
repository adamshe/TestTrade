using System.Windows.Forms;

namespace iTrading.Core.Kernel
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using iTrading.Core.Interface;

    /// <summary>
    /// Dictionary of symbols.
    /// </summary>
    [ClassInterface(ClassInterfaceType.None), Guid("461404DD-7600-463f-8394-6B21AAF6CDA0")]
    public class SymbolDictionary : DictionaryBase, IComSymbolDictionary
    {
        private Connection connection;

        internal SymbolDictionary(Connection connection)
        {
            this.connection = connection;
        }

        internal void Add(Symbol symbol)
        {
            base.Dictionary.Add(symbol, symbol);
        }

        /// <summary>
        /// Removes all symbols from the local application dictionary and cancels all associated
        /// <see cref="T:iTrading.Core.Kernel.MarketData" /> and <see cref="T:iTrading.Core.Kernel.MarketDepth" /> streams.
        /// </summary>
        public  void Clear()
        {
            this.connection.SynchronizeInvoke.Invoke(new MethodInvoker(this.ClearNow), null);
        }

        private void ClearNow()
        {
            try
            {
                lock (this.connection.Symbols)
                {
                    if (Globals.TraceSwitch.SymbolLookup)
                    {
                        Trace.WriteLine("(" + this.connection.IdPlus + ") Cbi.SymbolDictionary.Clear");
                    }
                    try
                    {
                        this.connection.operation = Operation.Update;
                        this.connection.adapter.Clear();
                    }
                    catch
                    {
                    }
                    ArrayList list = new ArrayList();
                    foreach (Symbol symbol in this.connection.Symbols.Values)
                    {
                        list.Add(symbol);
                    }
                    foreach (Symbol symbol2 in list)
                    {
                        symbol2.MarketData.Cancel();
                        symbol2.MarketDepth.Cancel();
                    }
                    this.connection.Symbols.Clear();
                }
            }
            catch (Exception exception)
            {
                this.connection.ProcessEventArgs(new ITradingErrorEventArgs(this.connection, ErrorCode.Panic, "", "Cbi.Symbol.ClearNow: exception caught: " + exception.Message));
            }
        }

        /// <summary>
        /// Checks if the symbol exists in this container.
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public bool Contains(Symbol symbol)
        {
            return base.Dictionary.Contains(symbol);
        }

        /// <summary>
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(Symbol[] array, int index)
        {
            base.CopyTo(array, index);
        }

        /// <summary>
        /// </summary>
        /// <param name="symbol"></param>
        public void Remove(Symbol symbol)
        {
            base.Dictionary.Remove(symbol);
        }

        /// <summary>
        /// Same as <see cref="M:iTrading.Core.Kernel.SymbolCollection.Remove(iTrading.Core.Kernel.Symbol)" />, but <see cref="P:iTrading.Core.Kernel.Symbol.Expiry" /> is not signifcant.
        /// </summary>
        /// <param name="symbol"></param>
        public void RemoveFamily(Symbol symbol)
        {
            ArrayList list = new ArrayList();
            foreach (Symbol symbol2 in base.Dictionary.Values)
            {
                if (symbol2.IsEqualFamily(symbol))
                {
                    list.Add(symbol2);
                }
            }
            foreach (Symbol symbol3 in list)
            {
                base.Dictionary.Remove(symbol3);
            }
        }

        internal Symbol this[Symbol symbol]
        {
            get
            {
                return (Symbol) base.Dictionary[symbol];
            }
        }

        /// <summary>
        /// The collection of available <see cref="T:iTrading.Core.Kernel.Symbol" /> instances.
        /// </summary>
        public ICollection Values
        {
            get
            {
                return base.Dictionary.Values;
            }
        }

        /// <summary>
        /// The Collection of available <see cref="T:iTrading.Core.Kernel.Symbol" /> instances.
        /// Used by COM clients to enumerate the <see cref="T:iTrading.Core.Kernel.SymbolDictionary" />
        /// because Interop does not allow enumeration (for..each) of Dictionaries.
        /// </summary>
        public iTrading.Core.Kernel.ValuesCollection ValuesCollection
        {
            get
            {
                return new iTrading.Core.Kernel.ValuesCollection(base.Dictionary.Values);
            }
        }
    }
}

