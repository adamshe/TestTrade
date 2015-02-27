namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    [ToolboxBitmap(typeof(GanttTool), "ToolsIcons.GanttTool.bmp"), Description("Allows dragging and resizing Gantt bars.")]
    public class GanttTool : ToolSeries
    {
        private bool allowDrag;
        private bool allowResize;
        private int bar;
        private GanttBarPart barPart;
        private Cursor cursorDrag;
        private Cursor cursorResize;
        private int minPixels;
        private double xOriginal;

        public event GanttDragEventHandler DragBar;

        public event GanttResizeEventHandler ResizeBar;

        public GanttTool() : this(null)
        {
        }

        public GanttTool(Chart c) : base(c)
        {
            this.allowDrag = true;
            this.allowResize = true;
            this.bar = -1;
            this.cursorDrag = Cursors.Hand;
            this.cursorResize = Cursors.SizeWE;
            this.minPixels = 5;
        }

        private void MouseDown(MouseEventArgs e, ref Cursor c)
        {
            this.bar = -1;
            if (this.allowResize)
            {
                int num = this.Gantt.Pointer.VertSize / 2;
                this.xOriginal = base.Chart.axes.Bottom.CalcPosPoint(e.X);
                for (int i = 0; i < this.Gantt.Count; i++)
                {
                    int num2 = this.Gantt.GetVertAxis.CalcPosValue(this.Gantt.YValues.Value[i]);
                    if ((e.Y >= (num2 - num)) && (e.Y <= (num2 + num)))
                    {
                        int num4 = this.Gantt.GetHorizAxis.CalcPosValue(this.Gantt.StartValues.Value[i]);
                        int num5 = this.Gantt.GetHorizAxis.CalcPosValue(this.Gantt.EndValues.Value[i]);
                        if (Math.Abs((int) (e.X - num4)) < this.minPixels)
                        {
                            this.bar = i;
                            this.barPart = GanttBarPart.Start;
                            break;
                        }
                        if (Math.Abs((int) (e.X - num5)) < this.minPixels)
                        {
                            this.bar = i;
                            this.barPart = GanttBarPart.End;
                            break;
                        }
                    }
                }
            }
            if ((this.bar == -1) && this.allowDrag)
            {
                this.bar = this.Gantt.Clicked(e.X, e.Y);
                if (this.bar != -1)
                {
                    this.barPart = GanttBarPart.All;
                }
            }
        }

        protected internal override void MouseEvent(MouseEventKinds kind, MouseEventArgs e, ref Cursor c)
        {
            if (this.Gantt != null)
            {
                if (kind == MouseEventKinds.Up)
                {
                    this.bar = -1;
                }
                else if (kind == MouseEventKinds.Move)
                {
                    this.MouseMove(e, ref c);
                }
                else if (kind == MouseEventKinds.Down)
                {
                    this.MouseDown(e, ref c);
                }
            }
        }

        private void MouseMove(MouseEventArgs e, ref Cursor c)
        {
            if (this.bar != -1)
            {
                double num4 = this.Gantt.GetHorizAxis.CalcPosPoint(e.X) - this.xOriginal;
                switch (this.barPart)
                {
                    case GanttBarPart.Start:
                        this.Gantt.StartValues.Value[this.bar] += num4;
                        if (this.Gantt.StartValues[this.bar] > this.Gantt.EndValues[this.bar])
                        {
                            this.Gantt.StartValues[this.bar] = this.Gantt.EndValues[this.bar];
                        }
                        if (this.ResizeBar != null)
                        {
                            this.ResizeBar(this, new GanttResizeEventArgs(this.bar, GanttBarPart.Start));
                        }
                        goto Label_035B;

                    case GanttBarPart.All:
                        this.Gantt.StartValues.Value[this.bar] += num4;
                        this.Gantt.EndValues.Value[this.bar] += num4;
                        if (this.DragBar != null)
                        {
                            this.DragBar(this, new GanttDragEventArgs(this.bar));
                        }
                        goto Label_035B;

                    case GanttBarPart.End:
                        this.Gantt.EndValues.Value[this.bar] += num4;
                        if (this.Gantt.EndValues[this.bar] < this.Gantt.StartValues[this.bar])
                        {
                            this.Gantt.EndValues[this.bar] = this.Gantt.StartValues[this.bar];
                        }
                        if (this.ResizeBar != null)
                        {
                            this.ResizeBar(this, new GanttResizeEventArgs(this.bar, GanttBarPart.End));
                        }
                        goto Label_035B;
                }
            }
            else
            {
                if (this.allowResize)
                {
                    int num = this.Gantt.Pointer.VertSize / 2;
                    for (int i = 0; i < this.Gantt.Count; i++)
                    {
                        int num3 = this.Gantt.GetVertAxis.CalcPosValue(this.Gantt.YValues.Value[i]);
                        if ((e.Y >= (num3 - num)) && (e.Y <= (num3 + num)))
                        {
                            num3 = this.Gantt.GetHorizAxis.CalcPosValue(this.Gantt.StartValues.Value[i]);
                            if (Math.Abs((int) (e.X - num3)) < this.minPixels)
                            {
                                c = this.cursorResize;
                                base.chart.CancelMouse = true;
                                break;
                            }
                            if (Math.Abs((int) (this.Gantt.GetHorizAxis.CalcPosValue(this.Gantt.EndValues.Value[i]) - e.X)) < this.minPixels)
                            {
                                c = this.cursorResize;
                                base.chart.CancelMouse = true;
                                break;
                            }
                        }
                    }
                }
                if ((this.allowDrag && (this.bar == -1)) && (this.Gantt.Clicked(e.X, e.Y) != -1))
                {
                    c = this.cursorDrag;
                    base.chart.CancelMouse = true;
                }
                return;
            }
        Label_035B:
            this.xOriginal = this.Gantt.GetHorizAxis.CalcPosPoint(e.X);
            this.Gantt.Invalidate();
        }

        [DefaultValue(true)]
        public bool AllowDrag
        {
            get
            {
                return this.allowDrag;
            }
            set
            {
                this.allowDrag = value;
            }
        }

        [DefaultValue(true)]
        public bool AllowResize
        {
            get
            {
                return this.allowResize;
            }
            set
            {
                this.allowResize = value;
            }
        }

        [DefaultValue(typeof(Cursor), "Hand"), Category("Appearance")]
        public Cursor CursorDrag
        {
            get
            {
                return this.cursorDrag;
            }
            set
            {
                this.cursorDrag = value;
            }
        }

        [Category("Appearance"), DefaultValue(typeof(Cursor), "SizeWE")]
        public Cursor CursorResize
        {
            get
            {
                return this.cursorResize;
            }
            set
            {
                this.cursorResize = value;
            }
        }

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.GanttTool;
            }
        }

        public Steema.TeeChart.Styles.Gantt Gantt
        {
            get
            {
                if ((base.iSeries != null) && (base.iSeries is Steema.TeeChart.Styles.Gantt))
                {
                    return (base.iSeries as Steema.TeeChart.Styles.Gantt);
                }
                return null;
            }
        }

        [DefaultValue(5)]
        public int MinPixels
        {
            get
            {
                return this.minPixels;
            }
            set
            {
                this.minPixels = value;
            }
        }

        [Description("Gets detailed descriptive text.")]
        public override string Summary
        {
            get
            {
                return Texts.GanttToolSummary;
            }
        }
    }
}

