namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;

    [ToolboxBitmap(typeof(FastLine), "SeriesIcons.FastLine.bmp")]
    public class FastLine : BaseLine
    {
        private bool autoRepaint;
        private bool drawAll;
        private bool ignoreNulls;
        private bool internal3D;
        private Graphics3D internalG;
        private bool invertedStairs;
        private int oldX;
        private int oldY;
        private bool stairs;

        public FastLine() : this(null)
        {
        }

        public FastLine(Chart c) : base(c)
        {
            this.autoRepaint = true;
            this.drawAll = true;
            this.ignoreNulls = true;
            base.pLinePen.usesVisible = false;
        }

        private void CalcPosition(int index, ref int x, ref int y)
        {
            x = base.GetHorizAxis.CalcXPosValue(base.XValues.Value[index]);
            y = base.GetVertAxis.CalcYPosValue(base.YValues.Value[index]);
        }

        public override int Clicked(int x, int y)
        {
            if ((base.firstVisible > -1) && (base.lastVisible > -1))
            {
                if (base.chart != null)
                {
                    base.chart.graphics3D.Calculate2DPosition(ref x, ref y, base.MiddleZ);
                }
                int qx = 0;
                int qy = 0;
                Point p = new Point(x, y);
                for (int i = base.firstVisible; i <= base.lastVisible; i++)
                {
                    int px = this.CalcXPos(i);
                    int py = this.CalcYPos(i);
                    if ((px == x) && (py == y))
                    {
                        return i;
                    }
                    if ((i > base.firstVisible) && Graphics3D.PointInLineTolerance(p, px, py, qx, qy, 3))
                    {
                        return (i - 1);
                    }
                    qx = px;
                    qy = py;
                }
            }
            return -1;
        }

        protected internal override void CreateSubGallery(Series.SubGalleryEventHandler AddSubChart)
        {
            base.CreateSubGallery(AddSubChart);
            AddSubChart(Texts.Marks);
            AddSubChart(Texts.Dotted);
            AddSubChart(Texts.Stairs);
        }

        private void DoMove(int X, int Y)
        {
            if (this.internal3D)
            {
                this.internalG.MoveTo(X, Y, base.middleZ);
            }
            else
            {
                this.internalG.MoveTo(X, Y);
            }
        }

        protected internal override void Draw()
        {
            this.PrepareCanvas();
            int firstVisible = base.firstVisible;
            if (firstVisible > 0)
            {
                this.CalcPosition(firstVisible - 1, ref this.oldX, ref this.oldY);
            }
            else
            {
                this.CalcPosition(firstVisible, ref this.oldX, ref this.oldY);
            }
            this.internalG = base.chart.graphics3D;
            this.internal3D = base.chart.Aspect.View3D;
            this.DoMove(this.oldX, this.oldY);
            if (firstVisible >= 0)
            {
                this.DrawValue(firstVisible);
            }
            for (int i = firstVisible + 1; i <= base.lastVisible; i++)
            {
                this.DrawValue(i);
            }
        }

        protected override void DrawLegendShape(Graphics3D g, int valueIndex, Rectangle rect)
        {
            this.PrepareCanvas(g);
            g.HorizontalLine(rect.X, rect.Right, (rect.Y + rect.Bottom) / 2);
        }

        protected internal override void DrawMark(int valueIndex, string st, SeriesMarks.Position aPosition)
        {
            base.Marks.ApplyArrowLength(ref aPosition);
            base.DrawMark(valueIndex, st, aPosition);
        }

        public override void DrawValue(int index)
        {
            int x = 0;
            int y = 0;
            this.CalcPosition(index, ref x, ref y);
            if (((x != this.oldX) || (this.drawAll && (y != this.oldY))) || (this.ValueColor(index) == Color.Transparent))
            {
                if (this.ignoreNulls || (this.ValueColor(index) != Color.Transparent))
                {
                    if (base.bColorEach)
                    {
                        this.internalG.Pen.Color = this.ValueColor(index);
                    }
                    if (this.internal3D)
                    {
                        if (this.stairs)
                        {
                            if (this.invertedStairs)
                            {
                                this.internalG.LineTo(this.oldX, y, base.middleZ);
                            }
                            else
                            {
                                this.internalG.LineTo(x, this.oldY, base.middleZ);
                            }
                        }
                        this.internalG.LineTo(x, y, base.middleZ);
                    }
                    else
                    {
                        if (this.stairs)
                        {
                            if (this.invertedStairs)
                            {
                                this.internalG.LineTo(this.oldX, y);
                            }
                            else
                            {
                                this.internalG.LineTo(x, this.oldY);
                            }
                        }
                        this.internalG.LineTo(x, y);
                    }
                }
                else if ((index + 1) < base.Count)
                {
                    this.CalcPosition(index + 1, ref x, ref y);
                    this.DoMove(x, y);
                }
                else
                {
                    return;
                }
                this.oldX = x;
                this.oldY = y;
            }
        }

        private void linePenColorChanged(object o, EventArgs e)
        {
            base.Color = base.pLinePen.Color;
        }

        private void PrepareCanvas()
        {
            this.PrepareCanvas(base.chart.graphics3D);
        }

        private void PrepareCanvas(Graphics3D g)
        {
            g.Pen = base.pLinePen;
            g.Pen.Color = base.Color;
        }

        protected override void SetChart(Chart value)
        {
            base.SetChart(value);
            base.pLinePen.chart = value;
            base.pLinePen.ColorChanged = (EventHandler) Delegate.Combine(base.pLinePen.ColorChanged, new EventHandler(this.linePenColorChanged));
            base.AllowSinglePoint = false;
            base.DrawBetweenPoints = true;
        }

        protected override void SetSeriesColor(Color value)
        {
            base.SetSeriesColor(value);
            base.pLinePen.Color = value;
        }

        protected internal override void SetSubGallery(int index)
        {
            switch (index)
            {
                case 1:
                    base.Marks.Visible = true;
                    return;

                case 2:
                    base.LinePen.Style = DashStyle.Dot;
                    return;

                case 3:
                    this.Stairs = true;
                    return;
            }
        }

        [DefaultValue(true)]
        public bool AutoRepaint
        {
            get
            {
                return this.autoRepaint;
            }
            set
            {
                this.autoRepaint = value;
            }
        }

        public override string Description
        {
            get
            {
                return Texts.GalleryFastLine;
            }
        }

        [DefaultValue(true)]
        public bool DrawAllPoints
        {
            get
            {
                return this.drawAll;
            }
            set
            {
                base.SetBooleanProperty(ref this.drawAll, value);
            }
        }

        [DefaultValue(true)]
        public bool IgnoreNulls
        {
            get
            {
                return this.ignoreNulls;
            }
            set
            {
                base.SetBooleanProperty(ref this.ignoreNulls, value);
            }
        }

        [DefaultValue(false)]
        public bool InvertedStairs
        {
            get
            {
                return this.invertedStairs;
            }
            set
            {
                base.SetBooleanProperty(ref this.invertedStairs, value);
            }
        }

        [DefaultValue(false)]
        public bool Stairs
        {
            get
            {
                return this.stairs;
            }
            set
            {
                base.SetBooleanProperty(ref this.stairs, value);
            }
        }

        [Category("Appearance"), DefaultValue(0), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Description("Sets Transparency level from 0 to 100%.")]
        public int Transparency
        {
            get
            {
                return base.pLinePen.Transparency;
            }
            set
            {
                base.pLinePen.Transparency = value;
            }
        }
    }
}

