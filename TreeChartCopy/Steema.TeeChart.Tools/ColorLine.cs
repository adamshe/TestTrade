namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    [Description("Displays a draggable line across axes."), ToolboxBitmap(typeof(ColorLine), "ToolsIcons.ColorLine.bmp")]
    public class ColorLine : ToolAxis
    {
        private bool allowDrag;
        private bool dragging;
        private bool dragRepaint;
        private bool draw3D;
        private bool drawBehind;
        private double lineValue;
        private bool noLimitDrag;

        public event EventHandler DragLine;

        public event ColorLineToolOnDragEventHandler EndDragLine;

        public ColorLine() : this(null)
        {
        }

        public ColorLine(Chart c) : base(c)
        {
            this.allowDrag = true;
            this.dragRepaint = false;
            this.noLimitDrag = false;
            this.draw3D = true;
            this.drawBehind = false;
            this.lineValue = 0.0;
            this.dragging = false;
        }

        protected internal override void ChartEvent(EventArgs e)
        {
            base.ChartEvent(e);
            if ((base.iAxis != null) && ((e is BeforeDrawSeriesEventArgs) || (e is AfterDrawEventArgs)))
            {
                base.chart.graphics3D.Pen = base.Pen;
                this.DrawColorLine(e is BeforeDrawSeriesEventArgs);
            }
        }

        private bool Clicked(int x, int y)
        {
            int num = base.iAxis.Horizontal ? x : y;
            if (Math.Abs((int) (num - base.iAxis.CalcPosValue(this.lineValue))) >= Steema.TeeChart.Tools.Tool.ClickTolerance)
            {
                return false;
            }
            Rectangle chartRect = base.chart.ChartRect;
            if (!base.iAxis.Horizontal)
            {
                return ((x >= chartRect.X) && (x <= chartRect.Right));
            }
            return ((y >= chartRect.Y) && (y <= chartRect.Bottom));
        }

        protected virtual void DoDragLine()
        {
            if (this.DragLine != null)
            {
                this.DragLine(this, EventArgs.Empty);
            }
        }

        protected virtual void DoEndDragLine()
        {
            if (this.EndDragLine != null)
            {
                this.EndDragLine(this);
            }
        }

        private void DrawColorLine(bool Back)
        {
            Graphics3D graphicsd = base.chart.graphics3D;
            Rectangle chartRect = base.chart.ChartRect;
            if (!graphicsd.Pen.Visible)
            {
                graphicsd.Pen.DrawingPen.Color = Color.Empty;
            }
            int num = base.chart.Aspect.Width3D;
            int y = base.iAxis.CalcPosValue(this.lineValue);
            if (Back)
            {
                if (!base.iAxis.Horizontal)
                {
                    if (this.draw3D)
                    {
                        graphicsd.ZLine(chartRect.X, y, 0, num);
                    }
                    if (this.draw3D || this.drawBehind)
                    {
                        graphicsd.HorizontalLine(chartRect.X, chartRect.Right, y, num);
                    }
                }
                else
                {
                    if (this.draw3D)
                    {
                        graphicsd.ZLine(y, chartRect.Bottom, 0, num);
                    }
                    if (this.draw3D || this.drawBehind)
                    {
                        graphicsd.VerticalLine(y, chartRect.Y, chartRect.Bottom, num);
                    }
                }
            }
            else if ((base.chart.Aspect.View3D || !this.dragging) || this.dragRepaint)
            {
                if (base.iAxis.Horizontal)
                {
                    if (this.draw3D)
                    {
                        graphicsd.ZLine(y, chartRect.Y, 0, num);
                    }
                    if (!this.drawBehind)
                    {
                        graphicsd.VerticalLine(y, chartRect.Y, chartRect.Bottom, 0);
                    }
                }
                else
                {
                    if (this.draw3D)
                    {
                        graphicsd.ZLine(chartRect.Right, y, 0, num);
                    }
                    if (!this.drawBehind)
                    {
                        graphicsd.HorizontalLine(chartRect.X, chartRect.Right, y, 0);
                    }
                }
            }
        }

        protected internal override void MouseEvent(MouseEventKinds kind, MouseEventArgs e, ref Cursor c)
        {
            bool flag = false;
            if (this.allowDrag && (base.iAxis != null))
            {
                double num3;
                switch (kind)
                {
                    case MouseEventKinds.Down:
                        this.dragging = this.Clicked(e.X, e.Y);
                        base.chart.CancelMouse = this.dragging;
                        return;

                    case MouseEventKinds.Move:
                    {
                        if (!this.dragging)
                        {
                            if (this.Clicked(e.X, e.Y))
                            {
                                c = base.iAxis.Horizontal ? Cursors.VSplit : Cursors.HSplit;
                                base.chart.CancelMouse = true;
                            }
                            return;
                        }
                        if (!this.dragRepaint)
                        {
                            this.Invalidate();
                        }
                        int num2 = base.iAxis.Horizontal ? e.X : e.Y;
                        num3 = base.Axis.CalcPosPoint(num2);
                        if (!this.noLimitDrag)
                        {
                            double num;
                            if (!base.iAxis.Horizontal)
                            {
                                num = base.iAxis.CalcPosPoint(base.iAxis.IEndPos);
                                if (num3 < num)
                                {
                                    num3 = num;
                                }
                                else
                                {
                                    num = base.iAxis.CalcPosPoint(base.iAxis.IStartPos);
                                    if (num3 > num)
                                    {
                                        num3 = num;
                                    }
                                }
                                break;
                            }
                            num = base.iAxis.CalcPosPoint(base.iAxis.IStartPos);
                            if (num3 < num)
                            {
                                num3 = num;
                            }
                            else
                            {
                                num = base.iAxis.CalcPosPoint(base.iAxis.IEndPos);
                                if (num3 > num)
                                {
                                    num3 = num;
                                }
                            }
                        }
                        break;
                    }
                    case MouseEventKinds.Up:
                        this.dragging = false;
                        if (!this.dragRepaint)
                        {
                            this.Invalidate();
                        }
                        this.DoEndDragLine();
                        return;

                    default:
                        return;
                }
                if (this.dragRepaint)
                {
                    this.Value = num3;
                }
                else
                {
                    flag = this.lineValue != num3;
                    if (flag)
                    {
                        base.chart.graphics3D.Pen = base.Pen;
                        this.DrawColorLine(true);
                        this.DrawColorLine(false);
                        this.lineValue = num3;
                    }
                }
                base.chart.CancelMouse = true;
                this.DoDragLine();
                if (flag)
                {
                    this.DrawColorLine(true);
                    this.DrawColorLine(false);
                }
            }
        }

        [DefaultValue(true), Description("")]
        public bool AllowDrag
        {
            get
            {
                return this.allowDrag;
            }
            set
            {
                base.SetBooleanProperty(ref this.allowDrag, value);
            }
        }

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.ColorLineTool;
            }
        }

        [DefaultValue(false), Description("")]
        public bool DragRepaint
        {
            get
            {
                return this.dragRepaint;
            }
            set
            {
                base.SetBooleanProperty(ref this.dragRepaint, value);
            }
        }

        [DefaultValue(true), Description("Draws ColorLine in 3D when True.")]
        public bool Draw3D
        {
            get
            {
                return this.draw3D;
            }
            set
            {
                base.SetBooleanProperty(ref this.draw3D, value);
            }
        }

        [Description("Draws the ColorLine behind the series values."), DefaultValue(false)]
        public bool DrawBehind
        {
            get
            {
                return this.drawBehind;
            }
            set
            {
                base.SetBooleanProperty(ref this.drawBehind, value);
            }
        }

        [Description("Allows drag of ColorLine outside of the Chart rectangle."), DefaultValue(false)]
        public bool NoLimitDrag
        {
            get
            {
                return this.noLimitDrag;
            }
            set
            {
                base.SetBooleanProperty(ref this.noLimitDrag, value);
            }
        }

        [Description("Gets detailed descriptive text.")]
        public override string Summary
        {
            get
            {
                return Texts.ColorLineSummary;
            }
        }

        [DefaultValue((double) 0.0), Description("Determines Axis position where the ColorLine has to be drawn.")]
        public double Value
        {
            get
            {
                return this.lineValue;
            }
            set
            {
                base.SetDoubleProperty(ref this.lineValue, value);
            }
        }
    }
}

