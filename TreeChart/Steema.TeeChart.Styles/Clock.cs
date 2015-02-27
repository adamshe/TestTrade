namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Timers;

    [ToolboxBitmap(typeof(Clock), "SeriesIcons.Clock.bmp")]
    public class Clock : Custom2DPolar
    {
        private ChartPen penHours;
        private ChartPen penMinutes;
        private ChartPen penSeconds;
        private static string[] RomanNumber = new string[] { "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII" };
        private ClockStyles style;
        private System.Timers.Timer timer;

        public Clock() : this(null)
        {
        }

        public Clock(Chart c) : base(c)
        {
            this.style = ClockStyles.Roman;
            this.timer = new System.Timers.Timer(1000.0);
            this.penHours = new ChartPen(c, Color.Black);
            this.penMinutes = new ChartPen(c, Color.Black);
            this.penSeconds = new ChartPen(c, Color.Black);
            base.Pointer.Visible = false;
            base.Pointer.defaultVisible = false;
            base.ShowInLegend = false;
            this.style = ClockStyles.Roman;
            base.Brush.Visible = true;
            base.Brush.Solid = true;
            base.CircleLabels = true;
            base.RotationAngle = 90;
            base.Circled = true;
            base.Add(0);
            this.timer.Elapsed += new ElapsedEventHandler(this.TimerElapsed);
            this.timer.Enabled = true;
        }

        private void CalcPos(double aAngle, double aSize, out int x, out int y)
        {
            base.AngleToPos(aAngle * 0.017453292519943295, (aSize * base.XRadius) / 2.0, (aSize * base.YRadius) / 2.0, out x, out y);
        }

        protected override void Dispose(bool disposing)
        {
            this.timer.Enabled = false;
            base.Dispose(disposing);
        }

        protected internal override void Draw()
        {
            int num5;
            int num6;
            DateTime now = DateTime.Now;
            int hour = now.Hour;
            int minute = now.Minute;
            int second = now.Second;
            int millisecond = now.Millisecond;
            Graphics3D g = base.chart.graphics3D;
            g.Brush = base.Brush;
            g.Brush.Color = base.Color;
            if (this.penHours.Visible)
            {
                g.Pen = this.penHours;
                this.CalcPos(360.0 - ((360.0 * ((60.0 * hour) + minute)) / 720.0), 1.3, out num5, out num6);
                g.Arrow(true, new Point(base.CircleXCenter, base.CircleYCenter), new Point(num5, num6), 14, 20, base.EndZ);
            }
            if (this.penMinutes.Visible)
            {
                g.Pen = this.penMinutes;
                this.CalcPos(360.0 - ((360.0 * minute) / 60.0), 1.7, out num5, out num6);
                g.Arrow(true, new Point(base.CircleXCenter, base.CircleYCenter), new Point(num5, num6), 10, 0x10, base.EndZ);
            }
            this.CalcPos(360.0 - ((360.0 * (second + (((double) millisecond) / 1000.0))) / 60.0), 1.8, out num5, out num6);
            if (this.penSeconds.Visible)
            {
                g.Pen = this.penSeconds;
                g.MoveTo(base.CircleXCenter, base.CircleYCenter, base.EndZ);
                g.LineTo(num5, num6, base.EndZ);
            }
            if (base.iPointer.Visible)
            {
                Color colorValue = base.iPointer.Brush.Color;
                base.iPointer.PrepareCanvas(g, colorValue);
                base.iPointer.Draw(num5, num6, colorValue);
            }
        }

        protected override string GetCircleLabel(double angle, int index)
        {
            int num = Utils.Round((double) ((360.0 - angle) / 30.0));
            if (this.style != ClockStyles.Decimal)
            {
                return RomanNumber[num - 1];
            }
            return num.ToString();
        }

        protected internal override int NumSampleValues()
        {
            return 1;
        }

        protected override void SetChart(Chart c)
        {
            base.SetChart(c);
            base.AngleIncrement = 30.0;
            if (base.chart != null)
            {
                base.chart.Axes.Visible = false;
            }
            this.penHours.Chart = base.chart;
            this.penMinutes.Chart = base.chart;
            this.penSeconds.Chart = base.chart;
            if (base.chart == null)
            {
                this.timer.Enabled = false;
            }
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            this.timer.Enabled = false;
            base.Repaint();
            this.timer.Enabled = true;
        }

        public override string Description
        {
            get
            {
                return Texts.GalleryClock;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance"), Description("Sets the Pen for the Clock's hour hand.")]
        public ChartPen PenHours
        {
            get
            {
                return this.penHours;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Sets the Pen for the Clock's minute hand."), Category("Appearance")]
        public ChartPen PenMinutes
        {
            get
            {
                return this.penMinutes;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance"), Description("Sets the Pen for the Clock's second hand.")]
        public ChartPen PenSeconds
        {
            get
            {
                return this.penSeconds;
            }
        }

        [DefaultValue(1), Description("Sets the style of the clock figures, Roman or Decimal.")]
        public ClockStyles Style
        {
            get
            {
                return this.style;
            }
            set
            {
                if (this.style != value)
                {
                    this.style = value;
                    this.Invalidate();
                }
            }
        }

        [Description("")]
        public System.Timers.Timer Timer
        {
            get
            {
                return this.timer;
            }
        }
    }
}

