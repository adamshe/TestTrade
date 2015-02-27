namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    [Description("Moves Series marks when dragging them with mouse."), ToolboxBitmap(typeof(DragMarks), "ToolsIcons.DragMarks.bmp")]
    public class DragMarks : ToolSeries
    {
        private int oldX;
        private int oldY;
        private SeriesMarks.Position position;

        public DragMarks() : this(null)
        {
        }

        public DragMarks(Chart c) : base(c)
        {
        }

        private void CheckCursor(ref Cursor c, int x, int y)
        {
            bool flag = false;
            if (base.iSeries != null)
            {
                flag = this.CheckCursorSeries(base.iSeries, x, y);
            }
            else
            {
                for (int i = base.chart.Series.Count - 1; i >= 0; i--)
                {
                    flag = this.CheckCursorSeries(base.chart[i], x, y);
                    if (flag)
                    {
                        break;
                    }
                }
            }
            if (flag)
            {
                base.chart.CancelMouse = true;
                c = Cursors.Hand;
            }
        }

        private bool CheckCursorSeries(Series s, int x, int y)
        {
            return ((s.Active && s.Marks.Visible) && (s.Marks.Clicked(x, y) != -1));
        }

        private int CheckSeries(Series s, int x, int y)
        {
            int num = -1;
            if (s.Active)
            {
                num = s.Marks.Clicked(x, y);
                if (num != -1)
                {
                    this.position = s.Marks.Positions[num];
                    return num;
                }
            }
            return num;
        }

        private void MouseDown(MouseEventArgs e)
        {
            if (base.iSeries != null)
            {
                this.CheckSeries(base.iSeries, e.X, e.Y);
            }
            else
            {
                for (int i = base.chart.Series.Count - 1; i >= 0; i--)
                {
                    if (this.CheckSeries(base.chart[i], e.X, e.Y) != -1)
                    {
                        break;
                    }
                }
            }
            if (this.position != null)
            {
                this.oldX = e.X;
                this.oldY = e.Y;
            }
        }

        protected internal override void MouseEvent(MouseEventKinds kind, MouseEventArgs e, ref Cursor c)
        {
            if (kind == MouseEventKinds.Up)
            {
                this.position = null;
            }
            else if (kind == MouseEventKinds.Down)
            {
                this.MouseDown(e);
                if (this.position != null)
                {
                    base.chart.CancelMouse = true;
                }
            }
            else if (kind == MouseEventKinds.Move)
            {
                this.MouseMove(e, ref c);
            }
        }

        private void MouseMove(MouseEventArgs e, ref Cursor c)
        {
            if (this.position == null)
            {
                this.CheckCursor(ref c, e.X, e.Y);
            }
            else
            {
                int num = e.X - this.oldX;
                int num2 = e.Y - this.oldY;
                this.position.Custom = true;
                this.position.LeftTop.X += num;
                this.position.LeftTop.Y += num2;
                this.position.ArrowTo.X += num;
                this.position.ArrowTo.Y += num2;
                this.oldX = e.X;
                this.oldY = e.Y;
                base.chart.CancelMouse = true;
                this.Invalidate();
            }
        }

        public override string Description
        {
            get
            {
                return Texts.DragMarksTool;
            }
        }

        [Description("Gets detailed descriptive text.")]
        public override string Summary
        {
            get
            {
                return Texts.DragMarksSummary;
            }
        }
    }
}

