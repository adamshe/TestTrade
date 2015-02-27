namespace iTrading.Gui
{
    using System;
    using System.Runtime.CompilerServices;
    using iTrading.Core.Kernel;

    /// <summary>
    /// This event will be thrown when the selected symbol changed.
    /// </summary>
    public delegate void SelectedSymbolChangedEventHandler(object sender, SymbolEventArgs e);
}

