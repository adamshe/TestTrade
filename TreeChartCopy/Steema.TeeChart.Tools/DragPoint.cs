namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    [Description("Moves a Series point when dragging it with mouse."), ToolboxBitmap(typeof(DragPoint), "ToolsIcons.DragPoint.bmp")]
    public class DragPoint : ToolSeries
    {
        private MouseButtons button;
        private System.Windows.Forms.Cursor cursor;
        private int dragging;
        private DragPointStyles style;
        private Series theSeries;

        public event DragPointEventHandler Drag;

        public DragPoint() : this((Chart) null)
        {
        }

        public DragPoint(Chart c) : base(c)
        {
            this.style = DragPointStyles.Both;
            this.button = MouseButtons.Left;
            this.cursor = Cursors.Hand;
            this.dragging = -1;
        }

        public DragPoint(Series s) : base(s)
        {
            this.style = DragPointStyles.Both;
            this.button = MouseButtons.Left;
            this.cursor = Cursors.Hand;
            this.dragging = -1;
        }

        private Series ClickedSeries(int x, int y)
        {
            if (base.iSeries == null)
            {
                foreach (Series series in base.chart.Series)
                {
                    if (series.Active && (series.Clicked(x, y) != -1))
                    {
                        return series;
                    }
                }
            }
            else if (base.iSeries.Clicked(x, y) != -1)
            {
                return base.iSeries;
            }
            return null;
        }

        protected internal override void MouseEvent(MouseEventKinds kind, MouseEventArgs e, ref System.Windows.Forms.Cursor c)
        {
            if (kind == MouseEventKinds.Up)
            {
                this.dragging = -1;
            }
            else if (kind == MouseEventKinds.Move)
            {
                if (this.dragging == -1)
                {
                    if (this.ClickedSeries(e.X, e.Y) != null)
                    {
                        c = this.cursor;
                        base.chart.CancelMouse = true;
                    }
                }
                else
                {
                    bool flag = false;
                    if ((this.style == DragPointStyles.X) || (this.style == DragPointStyles.Both))
                    {
                        this.theSeries.XValues[this.dragging] = this.theSeries.XScreenToValue(e.X);
                        flag = true;
                    }
                    if ((this.style == DragPointStyles.Y) || (this.style == DragPointStyles.Both))
                    {
                        this.theSeries.YValues[this.dragging] = this.theSeries.YScreenToValue(e.Y);
                        flag = true;
                    }
                    if (this.Drag != null)
                    {
                        this.Drag(this, this.dragging);
                    }
                    if (flag)
                    {
                        this.Invalidate();
                    }
                }
            }
            else if ((kind == MouseEventKinds.Down) && (e.Button == this.button))
            {
                this.theSeries = null;
                this.dragging = -1;
                if (base.iSeries == null)
                {
                    foreach (Series series2 in base.chart.Series)
                    {
                        if (series2.Active)
                        {
                            this.dragging = series2.Clicked(e.X, e.Y);
                            if (this.dragging != -1)
                            {
                                this.theSeries = series2;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    this.dragging = base.iSeries.Clicked(e.X, e.Y);
                    if (this.dragging != -1)
                    {
                        this.theSeries = base.iSeries;
                    }
                }
                if (this.dragging != -1)
                {
                    base.chart.CancelMouse = true;
                }
            }
        }

        [DefaultValue(0x100000), Description("Sets which mousebutton activates DragPoint.")]
        public MouseButtons Button
        {
            get
            {
                return this.button;
            }
            set
            {
                this.button = value;
            }
        }

        [Category("Appearance"), Description("Determines the type of DragPoint Cursor displayed."), DefaultValue(typeof(Cursors), "Hand")]
        public System.Windows.Forms.Cursor Cursor
        {
            get
            {
                return this.cursor;
            }
            set
            {
                this.cursor = value;
            }
        }

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.DragPoint;
            }
        }

        [DefaultValue(2), Description("")]
        public DragPointStyles Style
        {
            get
            {
                return this.style;
            }
            set
            {
                this.style = value;
            }
        }

        [Description("Gets detailed descriptive text.")]
        public override string Summary
        {
            get
            {
                return Texts.DragPointSummary;
            }
        }
    }
}

