namespace Steema.TeeChart.Tools
{
    using System;

    public class GanttDragEventArgs : EventArgs
    {
        public int Bar;

        public GanttDragEventArgs(int bar)
        {
            this.Bar = bar;
        }
    }
}

