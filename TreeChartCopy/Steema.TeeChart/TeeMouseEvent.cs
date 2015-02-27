namespace Steema.TeeChart
{
    using System.Windows.Forms;

    public class TeeMouseEvent : TeeEvent
    {
        public MouseButtons Button;
        public MouseEventKinds Event;
        public MouseEventArgs mArgs;
    }
}

