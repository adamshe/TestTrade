namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    [ToolboxBitmap(typeof(Annotation), "ToolsIcons.Annotation.bmp"), Description("Displays custom text at any location inside Chart.")]
    public class Annotation : Steema.TeeChart.Tools.Tool
    {
        private AnnotationCallout callout;
        private System.Windows.Forms.Cursor cursor;
        private AnnotationPositions position;
        private TextShapePosition shape;
        private StringAlignment textAlign;

        public event MouseEventHandler Click;

        public Annotation() : this(null)
        {
        }

        public Annotation(Chart c) : base(c)
        {
            this.position = AnnotationPositions.LeftTop;
            this.textAlign = StringAlignment.Near;
            this.cursor = Cursors.Default;
            this.Shape.drawText = false;
            this.shape.Shadow.Visible = true;
            this.Callout.Chart = base.chart;
        }

        protected internal override void ChartEvent(EventArgs e)
        {
            base.ChartEvent(e);
            if (e is AfterDrawEventArgs)
            {
                this.DrawText();
            }
        }

        private bool Clicked(int x, int y)
        {
            return this.Shape.ShapeBounds.Contains(x, y);
        }

        private void DrawText()
        {
            int num2;
            int num5;
            int num6;
            string text = this.Text;
            if (text.Length == 0)
            {
                text = " ";
            }
            Graphics3D graphicsd = base.chart.graphics3D;
            graphicsd.Font = this.shape.Font;
            int fontHeight = graphicsd.FontHeight;
            int num3 = base.chart.MultiLineTextWidth(text, out num2);
            int num4 = num2 * fontHeight;
            if (this.shape.CustomPosition)
            {
                num5 = this.shape.ShapeBounds.Left + 4;
                num6 = this.shape.ShapeBounds.Top + 2;
            }
            else
            {
                int num7 = (base.chart.Width - num3) - 8;
                int num8 = (base.chart.Height - num4) - 8;
                switch (this.position)
                {
                    case AnnotationPositions.LeftTop:
                        num5 = 10;
                        num6 = 10;
                        goto Label_00F4;

                    case AnnotationPositions.LeftBottom:
                        num5 = 10;
                        num6 = num8;
                        goto Label_00F4;

                    case AnnotationPositions.RightTop:
                        num5 = num7;
                        num6 = 10;
                        goto Label_00F4;
                }
                num5 = num7;
                num6 = num8;
            }
        Label_00F4:
            this.shape.ShapeBounds = new Rectangle(num5 - 4, num6 - 2, ((num5 + num3) + 2) - (num5 - 4), ((num6 + num4) + 2) - (num6 - 2));
            if (this.shape.Visible)
            {
                this.shape.Paint();
            }
            graphicsd.Brush.Visible = false;
            graphicsd.TextAlign = this.textAlign;
            if (this.textAlign == StringAlignment.Center)
            {
                num5 = 2 + ((this.shape.ShapeBounds.X + this.shape.ShapeBounds.Right) / 2);
            }
            else if (this.textAlign == StringAlignment.Far)
            {
                num5 = this.shape.ShapeBounds.Right - 2;
            }
            string[] strArray = text.Split(new char[] { '\n' });
            for (int i = 1; i <= num2; i++)
            {
                graphicsd.TextOut(num5, num6 + ((i - 1) * fontHeight), strArray[i - 1]);
            }
            if (this.callout.Visible || this.callout.Arrow.Visible)
            {
                Point p = new Point(this.callout.XPosition, this.callout.YPosition);
                Point aFrom = this.callout.CloserPoint(this.shape.ShapeBounds, p);
                if (this.callout.Distance != 0)
                {
                    p = Utils.PointAtDistance(aFrom, p, this.callout.Distance);
                }
                this.callout.Draw(Color.Empty, p, aFrom, this.callout.ZPosition);
            }
        }

        protected virtual string GetInnerText()
        {
            return this.shape.Text;
        }

        protected internal override void MouseEvent(MouseEventKinds kind, MouseEventArgs e, ref System.Windows.Forms.Cursor c)
        {
            if (kind == MouseEventKinds.Down)
            {
                if ((this.Click != null) && this.Clicked(e.X, e.Y))
                {
                    this.Click(this, e);
                }
            }
            else if (((kind == MouseEventKinds.Move) && (this.cursor != Cursors.Default)) && this.Clicked(e.X, e.Y))
            {
                c = this.cursor;
                base.Chart.CancelMouse = true;
            }
        }

        protected override void SetChart(Chart c)
        {
            base.SetChart(c);
            this.Shape.Chart = base.chart;
            this.Callout.Chart = base.chart;
        }

        private bool ShouldSerializeLeft()
        {
            return this.shape.CustomPosition;
        }

        private bool ShouldSerializeTop()
        {
            return this.shape.CustomPosition;
        }

        public AnnotationCallout Callout
        {
            get
            {
                if (this.callout == null)
                {
                    this.callout = new AnnotationCallout(null);
                }
                return this.callout;
            }
        }

        [Category("Appearance"), DefaultValue(typeof(Cursors), "Default")]
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
                return Texts.AnnotationTool;
            }
        }

        [Description("Sets horizontal displacement in pixels of text box from Chart's left edge.")]
        public int Left
        {
            get
            {
                return this.shape.Left;
            }
            set
            {
                this.shape.Left = value;
                this.shape.CustomPosition = true;
            }
        }

        [DefaultValue(0), Description("Sets the position of Annotation Tool text box and text.")]
        public AnnotationPositions Position
        {
            get
            {
                return this.position;
            }
            set
            {
                if (this.position != value)
                {
                    this.position = value;
                    this.shape.CustomPosition = false;
                    this.Invalidate();
                }
            }
        }

        [Description("Sets characteristics of the Annotation Tool text and text box Shape."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TextShapePosition Shape
        {
            get
            {
                if (this.shape == null)
                {
                    this.shape = new TextShapePosition(base.chart);
                }
                return this.shape;
            }
        }

        [Description("Gets detailed descriptive text.")]
        public override string Summary
        {
            get
            {
                return Texts.AnnotationSummary;
            }
        }

        [DefaultValue(""), Description("Defines the text for the Annotation Tool.")]
        public string Text
        {
            get
            {
                return this.GetInnerText();
            }
            set
            {
                this.shape.Text = value;
            }
        }

        [DefaultValue(0), Description("Horizontal alignment of displayed text.")]
        public StringAlignment TextAlign
        {
            get
            {
                return this.textAlign;
            }
            set
            {
                if (this.textAlign != value)
                {
                    this.textAlign = value;
                    this.Invalidate();
                }
            }
        }

        [Description("Sets vertical displacement in pixels of text box from Chart's top edge.")]
        public int Top
        {
            get
            {
                return this.shape.Top;
            }
            set
            {
                this.shape.Top = value;
                this.shape.CustomPosition = true;
            }
        }
    }
}

