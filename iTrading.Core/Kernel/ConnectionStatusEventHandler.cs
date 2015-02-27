namespace iTrading.Core.Kernel
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Call back for indicate connection status updates. <seealso cref="M:iTrading.Core.Kernel.Connection.Connect(iTrading.Core.Kernel.OptionsBase)" />.
    /// </summary>
    [ComVisible(false)]
    public delegate void ConnectionStatusEventHandler(object sender, ConnectionStatusEventArgs e);
}

