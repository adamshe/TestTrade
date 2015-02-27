namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    [Description("Allows rotating Chart dragging with mouse button."), ToolboxBitmap(typeof(Rotate), "ToolsIcons.Rotate.bmp")]
    public class Rotate : Steema.TeeChart.Tools.Tool
    {
        private MouseButtons button;
        private bool dragging;
        private bool IFirstTime;
        private bool inverted;
        private bool IOldRepaint;
        private int IOldX;
        private int IOldY;
        private RotateStyles style;

        public Rotate() : this(null)
        {
        }

        public Rotate(Chart c) : base(c)
        {
            this.button = MouseButtons.Left;
            this.style = RotateStyles.All;
        }

        private int CalcAngleChange(int angle, int change)
        {
            if (change > 0)
            {
                return Math.Min(360, angle + change);
            }
            int num = base.chart.graphics3D.SupportsFullRotation ? 0 : 270;
            return Math.Max(num, angle + change);
        }

        private Point CalcPoint(Graphics3D g, int x, int y, int z)
        {
            return base.chart.parent.PointToScreen(g.Calc3DPoint(x, y, z));
        }

        private int CorrectAngle(int angle)
        {
            int num = angle;
            if (num > 360)
            {
                return (num - 360);
            }
            if (num < 0)
            {
                num += 360;
            }
            return num;
        }

        private void DrawCubeOutline()
        {
            Graphics3D g = base.chart.graphics3D;
            g.Pen = this.Pen;
            int z = base.chart.Aspect.Width3D;
            Rectangle chartRect = base.chart.ChartRect;
            Point start = this.CalcPoint(g, chartRect.Left, chartRect.Top, 0);
            Point end = this.CalcPoint(g, chartRect.Left, chartRect.Bottom, 0);
            Point point3 = this.CalcPoint(g, chartRect.Right, chartRect.Bottom, 0);
            Point point4 = this.CalcPoint(g, chartRect.Right, chartRect.Top, 0);
            Point point5 = this.CalcPoint(g, chartRect.Left, chartRect.Top, z);
            Point point6 = this.CalcPoint(g, chartRect.Left, chartRect.Bottom, z);
            Point point7 = this.CalcPoint(g, chartRect.Right, chartRect.Bottom, z);
            Point point8 = this.CalcPoint(g, chartRect.Right, chartRect.Top, z);
            Color backColor = this.Pen.Color;
            ControlPaint.DrawReversibleLine(start, end, backColor);
            ControlPaint.DrawReversibleLine(end, point3, backColor);
            ControlPaint.DrawReversibleLine(point3, point4, backColor);
            ControlPaint.DrawReversibleLine(point4, start, backColor);
            ControlPaint.DrawReversibleLine(point5, point6, backColor);
            ControlPaint.DrawReversibleLine(point6, point7, backColor);
            ControlPaint.DrawReversibleLine(point7, point8, backColor);
            ControlPaint.DrawReversibleLine(point8, point5, backColor);
            ControlPaint.DrawReversibleLine(start, point5, backColor);
            ControlPaint.DrawReversibleLine(end, point6, backColor);
            ControlPaint.DrawReversibleLine(point3, point7, backColor);
            ControlPaint.DrawReversibleLine(point4, point8, backColor);
        }

        internal static Pie FirstSeriesPie(Chart c)
        {
            foreach (Series series in c.Series)
            {
                if ((series.GetType() == typeof(Pie)) && series.Active)
                {
                    return (Pie) series;
                }
            }
            return null;
        }

        protected internal override void MouseEvent(MouseEventKinds kind, MouseEventArgs e, ref Cursor c)
        {
            if (kind == MouseEventKinds.Up)
            {
                if (this.dragging)
                {
                    this.dragging = false;
                    if (this.Pen.Visible)
                    {
                        base.chart.AutoRepaint = this.IOldRepaint;
                        this.Invalidate();
                    }
                }
            }
            else if (kind == MouseEventKinds.Move)
            {
                if (this.dragging)
                {
                    this.MouseMove(e);
                }
            }
            else if ((kind == MouseEventKinds.Down) && (e.Button == this.button))
            {
                this.dragging = true;
                this.IOldX = e.X;
                this.IOldY = e.Y;
                this.IFirstTime = true;
                base.chart.CancelMouse = true;
            }
        }

        private void MouseMove(MouseEventArgs e)
        {
            Aspect aspect = base.chart.Aspect;
            if (this.Pen.Visible)
            {
                if (this.IFirstTime)
                {
                    this.IOldRepaint = base.chart.AutoRepaint;
                    base.chart.AutoRepaint = false;
                    this.IFirstTime = false;
                }
                else
                {
                    this.DrawCubeOutline();
                }
            }
            aspect.view3D = true;
            aspect.Orthogonal = false;
            int change = Utils.Round((double) ((90.0 * (e.X - this.IOldX)) / ((double) base.chart.Width)));
            if (this.inverted)
            {
                change = -change;
            }
            int num2 = Utils.Round((double) ((90.0 * (this.IOldY - e.Y)) / ((double) base.chart.Height)));
            if (this.inverted)
            {
                num2 = -num2;
            }
            if (base.chart.graphics3D.SupportsFullRotation)
            {
                if ((this.style == RotateStyles.Rotation) || (this.style == RotateStyles.All))
                {
                    aspect.Rotation = this.CorrectAngle(aspect.Rotation + change);
                }
                if ((this.style == RotateStyles.Elevation) || (this.style == RotateStyles.All))
                {
                    aspect.Elevation = this.CorrectAngle(aspect.Elevation + num2);
                }
            }
            else
            {
                if ((this.style == RotateStyles.Rotation) || (this.style == RotateStyles.All))
                {
                    Pie pie = FirstSeriesPie(base.chart);
                    if (pie != null)
                    {
                        aspect.Rotation = 360;
                        if (!base.chart.graphics3D.SupportsFullRotation)
                        {
                            aspect.Perspective = 0;
                        }
                        if (change != 0)
                        {
                            pie.RotationAngle = this.CorrectAngle(pie.RotationAngle + change);
                        }
                    }
                    else
                    {
                        aspect.Rotation = this.CalcAngleChange(aspect.Rotation, change);
                    }
                }
                if ((this.style == RotateStyles.Elevation) || (this.style == RotateStyles.All))
                {
                    aspect.Elevation = this.CalcAngleChange(aspect.Elevation, num2);
                }
            }
            this.IOldX = e.X;
            this.IOldY = e.Y;
            base.chart.CancelMouse = true;
            if (this.Pen.Visible)
            {
                base.chart.graphics3D.aspect.orthogonal = false;
                base.chart.graphics3D.CalcTrigValues();
                base.chart.graphics3D.CalcPerspective(base.chart.ChartRect);
                this.DrawCubeOutline();
            }
        }

        [Description("Defines which mousebutton activates the TTeeCustomTool."), DefaultValue(0x100000)]
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

        [Description("Gets descriptive text.")]
        public override string Description
        {
            get
            {
                return Texts.RotateTool;
            }
        }

        [DefaultValue(false), Description("Inverts the direction of Rotation and Elevation.")]
        public bool Inverted
        {
            get
            {
                return this.inverted;
            }
            set
            {
                this.inverted = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Element Pen characteristics."), Category("Appearance")]
        public ChartPen Pen
        {
            get
            {
                if (base.pPen == null)
                {
                    base.pPen = new ChartPen(base.chart, Color.White);
                    base.pPen.bVisible = false;
                    base.pPen.defaultVisible = false;
                }
                return base.pPen;
            }
        }

        [DefaultValue(0), Description("Determines whether mouse action applies to Rotation, Elevation or Both.")]
        public RotateStyles Style
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
                return Texts.RotateSummary;
            }
        }
    }
}

