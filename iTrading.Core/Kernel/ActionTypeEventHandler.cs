namespace iTrading.Core.Kernel
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    /// <summary>
    /// This delegate will be called once for every ActionType supported by the
    /// the actual connection.
    /// These calls will be executed immediately after a successful call of
    /// <see cref="M:iTrading.Core.Kernel.Connection.Connect(iTrading.Core.Kernel.OptionsBase)" />. 
    /// </summary>
    [ComVisible(false)]
    public delegate void ActionTypeEventHandler(object sender, ActionTypeEventArgs e);
}

