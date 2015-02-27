namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using Steema.TeeChart.Editors;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;
    using System.Reflection;

    [Editor(typeof(MarksComponentEditor), typeof(UITypeEditor))]
    public class SeriesMarks : TextShape
    {
        private double angle;
        protected internal bool bClip;
        private MarksCallout callout;
        public bool CheckMarkArrowColor;
        protected internal int defaultArrowLength;
        private int drawEvery;
        protected internal Steema.TeeChart.Styles.Series iSeries;
        private MarksItems items;
        protected internal MarksStyles markerStyle;
        private bool multiLine;
        protected MarkPositions pPositions;
        internal int zPosition;

        public SeriesMarks(Steema.TeeChart.Styles.Series s) : base(s.chart)
        {
            this.pPositions = new MarkPositions();
            this.defaultArrowLength = 10;
            this.drawEvery = 1;
            this.zPosition = -1;
            this.markerStyle = MarksStyles.Label;
            this.iSeries = s;
            base.Color = Color.LightYellow;
            base.Brush.defaultColor = Color.LightYellow;
            base.Shadow.Visible = true;
            base.Shadow.defaultVisible = true;
            base.Shadow.Width = 1;
            base.Shadow.Height = 1;
            base.shadow.defaultSize = 1;
            base.shadow.Brush.defaultColor = Color.Gray;
            base.shadow.Color = Color.Gray;
            base.Visible = false;
            base.defaultVisible = false;
            this.callout = new MarksCallout(s);
            this.callout.Length = 10;
            this.items = new MarksItems(this);
        }

        protected internal bool AllSeriesVisible()
        {
            if (base.chart == null)
            {
                return base.Visible;
            }
            foreach (Steema.TeeChart.Styles.Series series in base.chart.Series)
            {
                if (!series.Marks.Visible)
                {
                    return false;
                }
            }
            return true;
        }

        protected internal void AntiOverlap(int first, int valueIndex, Position aPos)
        {
            Rectangle b = this.TotalBounds(valueIndex, aPos);
            for (int i = first; i < valueIndex; i++)
            {
                if (this.Positions[i] != null)
                {
                    Rectangle a = this.TotalBounds(i, this.Positions[i]);
                    if (!Rectangle.Intersect(a, b).IsEmpty)
                    {
                        int num2;
                        if (b.Top < a.Top)
                        {
                            num2 = b.Bottom - a.Y;
                        }
                        else
                        {
                            num2 = b.Y - a.Bottom;
                        }
                        aPos.LeftTop.Y -= num2;
                        aPos.ArrowTo.Y -= num2;
                    }
                }
            }
        }

        protected internal void ApplyArrowLength(ref Position aPos)
        {
            int num = this.Callout.Length + this.Callout.Distance;
            aPos.LeftTop.Y -= num;
            aPos.ArrowTo.Y -= num;
            aPos.ArrowFrom.Y -= this.Callout.Distance;
        }

        public void Clear()
        {
            this.pPositions.Clear();
            if (!this.Items.ILoadingCustom)
            {
                this.Items.Clear();
            }
        }

        public virtual int Clicked(Point p)
        {
            return this.Clicked(p.X, p.Y);
        }

        public virtual int Clicked(int x, int y)
        {
            if (this.iSeries.chart != null)
            {
                this.iSeries.chart.graphics3D.Calculate2DPosition(ref x, ref y, this.ZPosition);
            }
            int num = 0;
            foreach (Position position in this.pPositions)
            {
                if ((((num % this.drawEvery) == 0) && (position != null)) && position.Bounds.Contains(x, y))
                {
                    return num;
                }
                num++;
            }
            return -1;
        }

        protected internal void ConvertTo2D(Position aPos, ref Point p)
        {
            Point point = new Point(0, 0);
            if (base.Chart.aspect.View3D && !base.Chart.Graphics3D.Supports3DText)
            {
                int num = aPos.ArrowTo.X - p.X;
                int num2 = aPos.ArrowTo.Y - p.Y;
                base.Chart.Graphics3D.Calc3DPos(ref point, aPos.ArrowTo, this.ZPosition);
                p.X = point.X - num;
                p.Y = point.Y - num2;
            }
        }

        private void DrawText(Rectangle r, string st)
        {
            int tmpNumRow = 0;
            int fontHeight = this.iSeries.chart.graphics3D.FontHeight;
            int index = st.IndexOf('\n');
            if (index != -1)
            {
                string lineSt = st;
                do
                {
                    this.DrawTextLine(lineSt.Substring(0, index), r, tmpNumRow, fontHeight);
                    lineSt = lineSt.Substring(index + 1, lineSt.Length - (index + 1));
                    tmpNumRow++;
                    index = lineSt.IndexOf('\n');
                }
                while (index != -1);
                if (lineSt.Length != 0)
                {
                    this.DrawTextLine(lineSt, r, tmpNumRow, fontHeight);
                }
            }
            else
            {
                this.DrawTextLine(st, r, tmpNumRow, fontHeight);
            }
        }

        private void DrawTextLine(string lineSt, Rectangle r, int tmpNumRow, int tmpRowHeight)
        {
            int num;
            int num2;
            Point point = new Point(0, 0);
            Graphics3D.RectCenter(r, out num, out num2);
            if (this.Angle != 0.0)
            {
                double a = this.Angle * Utils.PiStep;
                double num4 = Math.Sin(a);
                double num5 = Math.Cos(a);
                int num6 = (tmpNumRow * tmpRowHeight) - (r.Bottom - num2);
                point.X = num + Utils.Round((double) (num6 * num4));
                if (this.Angle == 90.0)
                {
                    point.X += 2;
                }
                point.Y = num2 + Utils.Round((double) (num6 * num5));
            }
            else
            {
                point.X = num;
                point.Y = r.Y + (tmpNumRow * tmpRowHeight);
                if (base.Pen.Visible)
                {
                    point.X += base.Pen.Width;
                    point.Y += base.Pen.Width;
                }
            }
            Graphics3D graphicsd = this.iSeries.chart.graphics3D;
            graphicsd.TextAlign = StringAlignment.Center;
            if (graphicsd.Supports3DText)
            {
                if (this.Angle == 0.0)
                {
                    graphicsd.TextOut(point.X, point.Y, this.ZPosition, lineSt);
                }
                else
                {
                    graphicsd.RotateLabel(point.X, point.Y, this.ZPosition, lineSt, (double) ((float) this.angle));
                }
            }
            else if (this.Angle == 0.0)
            {
                graphicsd.TextOut(point.X, point.Y, lineSt);
            }
            else
            {
                graphicsd.RotateLabel(point.X, point.Y, lineSt, (double) ((float) this.angle));
            }
        }

        protected internal void InternalDraw(int index, Color aColor, string st, Position aPos)
        {
            this.UsePosition(index, aPos);
            Chart chart = this.iSeries.chart;
            Graphics3D g = chart.graphics3D;
            TextShape shape = this.MarkItem(index);
            bool flag1 = chart.aspect.View3D;
            if (this.Callout.Visible || this.Callout.Arrow.Visible)
            {
                this.Callout.Draw(aColor, aPos.ArrowFrom, aPos.ArrowTo, this.ZPosition);
            }
            if (shape.Transparent)
            {
                g.Brush.Visible = false;
            }
            else
            {
                g.Brush = shape.Brush;
            }
            g.Pen = shape.Pen;
            this.ConvertTo2D(aPos, ref aPos.LeftTop);
            Rectangle rect = new Rectangle(aPos.LeftTop.X, aPos.LeftTop.Y, aPos.Width + 2, aPos.Height);
            shape.DrawRectRotated(g, rect, (int) (this.angle % 360.0), this.ZPosition);
            g.Brush.Visible = false;
            Rectangle bounds = aPos.Bounds;
            Rectangle r = new Rectangle(bounds.X, bounds.Y, bounds.Width, bounds.Height);
            this.DrawText(r, st);
        }

        protected internal TextShape MarkItem(int valueIndex)
        {
            TextShape shape = this;
            if ((this.Items.Count > valueIndex) && (this.Items[valueIndex] != null))
            {
                shape = this.Items[valueIndex];
            }
            return shape;
        }

        public string PercentString(int valueIndex, bool addTotal)
        {
            Steema.TeeChart.Styles.ValueList mandatory = this.iSeries.mandatory;
            string str = ((mandatory.TotalABS != 0.0) ? (Math.Abs(this.iSeries.GetMarkValue(valueIndex)) / mandatory.TotalABS) : 100.0).ToString(this.iSeries.percentFormat);
            if (addTotal)
            {
                string format = this.multiLine ? Texts.DefaultPercentOf2 : Texts.DefaultPercentOf;
                str = string.Format(format, str, mandatory.TotalABS.ToString(this.iSeries.valueFormat));
            }
            return str;
        }

        public void ResetPositions()
        {
            foreach (Position position in this.pPositions)
            {
                if (position != null)
                {
                    position.Custom = false;
                }
            }
            this.Invalidate();
        }

        protected virtual bool ShouldSerializeArrowLength()
        {
            return (this.ArrowLength != this.defaultArrowLength);
        }

        public int TextWidth(int valueIndex)
        {
            string text = "";
            int num = 0;
            string markText = this.iSeries.GetMarkText(valueIndex);
            int index = markText.IndexOf('\n');
            if (index > 0)
            {
                do
                {
                    text = markText.Substring(0, index);
                    num = Math.Max(num, Convert.ToInt32(this.iSeries.chart.graphics3D.TextWidth(text)));
                    markText = markText.Substring(index + 1);
                    index = markText.IndexOf('\n');
                }
                while (index != -1);
            }
            if (markText.Length != 0)
            {
                num = Math.Max(num, (int) this.iSeries.chart.graphics3D.TextWidth(markText));
            }
            return num;
        }

        private Rectangle TotalBounds(int valueIndex, Position aPos)
        {
            Rectangle bounds = aPos.Bounds;
            TextShape shape = this.MarkItem(valueIndex);
            if (shape.Pen.Visible)
            {
                bounds.Width += shape.Pen.Width;
                bounds.Height += shape.Pen.Width;
            }
            if (shape.Shadow.Width > 0)
            {
                bounds.Width += shape.Shadow.Width;
            }
            else if (shape.Shadow.Width < 0)
            {
                bounds.X -= shape.Shadow.Width;
            }
            if (shape.Shadow.Height > 0)
            {
                bounds.Height += shape.Shadow.Height;
            }
            else if (shape.Shadow.Height < 0)
            {
                bounds.Y -= shape.Shadow.Height;
            }
            Point location = bounds.Location;
            this.ConvertTo2D(aPos, ref location);
            int num = bounds.X - location.X;
            bounds.X -= num;
            bounds.Width -= num;
            num = bounds.Y - location.Y;
            bounds.Y -= num;
            bounds.Height -= num;
            return bounds;
        }

        private void UsePosition(int index, Position markPosition)
        {
            Position position;
            while (index >= this.pPositions.Count)
            {
                this.pPositions.Add(null);
            }
            if (this.pPositions[index] == null)
            {
                position = new Position();
                position.Custom = false;
                this.pPositions[index] = position;
            }
            position = this.pPositions[index];
            if (position.Custom)
            {
                if (markPosition.ArrowFix)
                {
                    Point arrowFrom = markPosition.ArrowFrom;
                    markPosition.Assign(position);
                    markPosition.ArrowFrom = arrowFrom;
                }
                else
                {
                    markPosition.Assign(position);
                }
            }
            else
            {
                position.Assign(markPosition);
            }
        }

        [Description("Sets angle from 0 to 360\x00ba to rotate Marks."), DefaultValue((double) 0.0)]
        public double Angle
        {
            get
            {
                return this.angle;
            }
            set
            {
                base.SetDoubleProperty(ref this.angle, value);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Defines Pen for lines connecting Marks to Series points.")]
        public ChartPen Arrow
        {
            get
            {
                return this.Callout.Arrow;
            }
        }

        [Description("Sets length in pixels for the line connecting the Mark to Series point.")]
        public int ArrowLength
        {
            get
            {
                return this.Callout.Length;
            }
            set
            {
                this.Callout.Length = value;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never), Obsolete("Use the Color property instead.")]
        public Color BackColor
        {
            get
            {
                return base.Color;
            }
            set
            {
                base.Color = value;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MarksCallout Callout
        {
            get
            {
                return this.callout;
            }
        }

        [DefaultValue(false), Description("Restricts Marks to Chart axes space, when true.")]
        public bool Clip
        {
            get
            {
                return this.bClip;
            }
            set
            {
                base.SetBooleanProperty(ref this.bClip, value);
            }
        }

        [DefaultValue(1), Description("Sets the number of Marks to skip when displayed. ")]
        public int DrawEvery
        {
            get
            {
                return this.drawEvery;
            }
            set
            {
                base.SetIntegerProperty(ref this.drawEvery, value);
            }
        }

        [Obsolete("Use the Pen property instead."), EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public ChartPen Frame
        {
            get
            {
                return base.Pen;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MarksItems Items
        {
            get
            {
                return this.items;
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string[] Lines
        {
            get
            {
                return base.Lines;
            }
        }

        [Description("Characters in Mark texts are split into multiple lines, when true."), DefaultValue(false)]
        public bool MultiLine
        {
            get
            {
                return this.multiLine;
            }
            set
            {
                base.SetBooleanProperty(ref this.multiLine, value);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public MarkPositions Positions
        {
            get
            {
                return this.pPositions;
            }
        }

        [Browsable(false), Description("")]
        public Steema.TeeChart.Styles.Series Series
        {
            get
            {
                return this.iSeries;
            }
        }

        [Description("Defines how Series Marks are constructed."), DefaultValue(2)]
        public MarksStyles Style
        {
            get
            {
                return this.markerStyle;
            }
            set
            {
                if (this.markerStyle != value)
                {
                    this.markerStyle = value;
                    this.Invalidate();
                }
            }
        }

        [Description("Sets Position in pixels on the Z axis."), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ZPosition
        {
            get
            {
                return this.zPosition;
            }
            set
            {
                base.SetIntegerProperty(ref this.zPosition, value);
            }
        }

        public class MarkPositions : ArrayList
        {
            public bool ExistCustom()
            {
                foreach (SeriesMarks.Position position in this)
                {
                    if ((position != null) && position.Custom)
                    {
                        return true;
                    }
                }
                return false;
            }

            public SeriesMarks.Position this[int index]
            {
                get
                {
                    if (index >= this.Count)
                    {
                        return null;
                    }
                    return (SeriesMarks.Position) base[index];
                }
                set
                {
                    base[index] = value;
                }
            }
        }

        internal class MarksComponentEditor : UITypeEditor
        {
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                using (SeriesMarksEditor editor = new SeriesMarksEditor((SeriesMarks) value, null))
                {
                    bool flag = EditorUtils.ShowFormModal(editor);
                    if ((context != null) && flag)
                    {
                        context.OnComponentChanged();
                    }
                    return value;
                }
            }

            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.Modal;
            }
        }

        public class Position
        {
            public bool ArrowFix;
            public Point ArrowFrom;
            public Point ArrowTo;
            public bool Custom;
            public int Height;
            public Point LeftTop;
            public int Width;

            public void Assign(SeriesMarks.Position source)
            {
                this.ArrowFrom.X = source.ArrowFrom.X;
                this.ArrowFrom.Y = source.ArrowFrom.Y;
                this.ArrowTo.X = source.ArrowTo.X;
                this.ArrowTo.Y = source.ArrowTo.Y;
                this.LeftTop.X = source.LeftTop.X;
                this.LeftTop.Y = source.LeftTop.Y;
                this.Height = source.Height;
                this.Width = source.Width;
            }

            [Description("Returns the bounding rectangle of the indexed Mark.")]
            public Rectangle Bounds
            {
                get
                {
                    return new Rectangle(this.LeftTop, new Size(this.Width, this.Height));
                }
            }
        }
    }
}

