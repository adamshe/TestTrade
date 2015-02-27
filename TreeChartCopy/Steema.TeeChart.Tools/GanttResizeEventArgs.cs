namespace Steema.TeeChart.Tools
{
    using System;

    public class GanttResizeEventArgs : GanttDragEventArgs
    {
        public GanttBarPart Part;

        public GanttResizeEventArgs(int bar, GanttBarPart part) : base(bar)
        {
            this.Part = part;
        }
    }
}

