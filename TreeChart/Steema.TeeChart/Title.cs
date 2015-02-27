namespace Steema.TeeChart
{
    using Steema.TeeChart.Drawing;
    using Steema.TeeChart.Editors;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;

    [Editor(typeof(TitleComponentEditor), typeof(UITypeEditor)), Description("Title properties.")]
    public class Title : TextShapePosition
    {
        private bool adjustFrame;
        private StringAlignment alignment;
        private int FontH;
        protected bool onTop;
        private const int TitleFootDistance = 5;
        private int tmpFrameWidth;
        private int tmpMargin;
        private int tmpXPosTitle;

        public Title(Chart c) : base(c)
        {
            this.adjustFrame = true;
            this.alignment = StringAlignment.Center;
            this.onTop = true;
            base.Color = Color.Silver;
            base.Brush.defaultColor = Color.Silver;
            base.drawText = false;
            base.bTransparent = true;
        }

        public virtual bool Clicked(Point p)
        {
            return (base.visible && base.ShapeBounds.Contains(p.X, p.Y));
        }

        public virtual bool Clicked(int x, int y)
        {
            return (base.visible && base.ShapeBounds.Contains(x, y));
        }

        internal void DoDraw(Graphics3D g, ref Rectangle rect, bool CustomOnly)
        {
            if (base.bCustomPosition == CustomOnly)
            {
                this.Draw(g, ref rect);
            }
        }

        internal void Draw(Graphics3D g, ref Rectangle rect)
        {
            int num2;
            int linesLength = base.LinesLength;
            if (!base.visible || (linesLength <= 0))
            {
                goto Label_02BF;
            }
            if (base.Pen.Visible && !base.Pen.Color.IsEmpty)
            {
                this.tmpFrameWidth = base.Pen.Width;
            }
            else
            {
                this.tmpFrameWidth = 0;
            }
            if (base.Bevel.Inner != BevelStyles.None)
            {
                this.tmpFrameWidth += base.Bevel.Width;
            }
            if (!base.bCustomPosition)
            {
                base.ShapeBounds = rect;
                if (this.onTop)
                {
                    base.iTop += this.tmpFrameWidth;
                }
            }
            g.Font = base.Font;
            g.TextAlign = StringAlignment.Near;
            this.FontH = g.FontHeight;
            if (this.onTop || base.bCustomPosition)
            {
                base.iBottom = (base.iTop + (linesLength * this.FontH)) + this.tmpFrameWidth;
            }
            else
            {
                base.iTop = (base.iBottom - (linesLength * this.FontH)) - this.tmpFrameWidth;
            }
            this.tmpMargin = Utils.Round(g.TextWidth("W"));
            if (this.adjustFrame)
            {
                int num = 0;
                for (int j = 0; j < linesLength; j++)
                {
                    num2 = Utils.Round(g.TextWidth(base.Lines[j]));
                    if (num2 > num)
                    {
                        num = num2;
                    }
                }
                num += this.tmpMargin + this.tmpFrameWidth;
                switch (this.alignment)
                {
                    case StringAlignment.Near:
                        base.iRight = base.iLeft + num;
                        goto Label_01F2;

                    case StringAlignment.Far:
                        base.iLeft = base.iRight - num;
                        goto Label_01F2;
                }
                if (base.bCustomPosition)
                {
                    base.iRight = base.iLeft + num;
                }
                num2 = (base.iLeft + base.iRight) / 2;
                base.iLeft = num2 - (num / 2);
                base.iRight = num2 + (num / 2);
            }
        Label_01F2:
            base.Paint(g, base.ShapeBounds);
            if (this.alignment == StringAlignment.Near)
            {
                this.tmpXPosTitle = base.ShapeBounds.Left + (this.tmpMargin / 2);
            }
            for (int i = 0; i < linesLength; i++)
            {
                this.DrawTitleLine(i, g);
            }
            if (!base.bCustomPosition)
            {
                num2 = 5 + this.tmpFrameWidth;
                if (!base.Transparent && base.Shadow.Visible)
                {
                    num2 += base.Shadow.Height;
                }
                if (this.onTop)
                {
                    int y = rect.Y;
                    rect.Y = base.iBottom + num2;
                    rect.Height -= rect.Y - y;
                }
                else
                {
                    rect.Height -= num2 + (linesLength * this.FontH);
                }
            }
        Label_02BF:
            base.Chart.RecalcWidthHeight(ref rect);
        }

        private void DrawTitleLine(int AIndex, Graphics3D g)
        {
            string text = base.Lines[AIndex];
            if (text != null)
            {
                int y = (AIndex * this.FontH) + (this.tmpFrameWidth / 2);
                if (this.onTop)
                {
                    y += base.ShapeBounds.Top;
                }
                else
                {
                    y = (base.ShapeBounds.Bottom - this.FontH) - y;
                }
                if (this.alignment == StringAlignment.Far)
                {
                    this.tmpXPosTitle = (base.iRight - Utils.Round(g.TextWidth(text))) - (this.tmpMargin / 2);
                }
                else if (this.alignment == StringAlignment.Center)
                {
                    this.tmpXPosTitle = Utils.Round((float) (((base.iLeft + base.iRight) / 2) - (g.TextWidth(text) / 2f)));
                }
                g.TextOut(this.tmpXPosTitle, y, text);
            }
        }

        protected override bool ShouldSerializeTransparent()
        {
            return !base.bTransparent;
        }

        [DefaultValue(true), Description("Resizes Header and Footer frames to full Chart dimensions when True.")]
        public bool AdjustFrame
        {
            get
            {
                return this.adjustFrame;
            }
            set
            {
                base.SetBooleanProperty(ref this.adjustFrame, value);
            }
        }

        [Description("Sets how Header and Footer text will be aligned within Chart rectangle."), DefaultValue(1)]
        public StringAlignment Alignment
        {
            get
            {
                return this.alignment;
            }
            set
            {
                if (this.alignment != value)
                {
                    this.alignment = value;
                    this.Invalidate();
                }
            }
        }

        [Description("Gets/Sets Left position of Header or Footer Text.")]
        public new int Left
        {
            get
            {
                return base.Left;
            }
            set
            {
                base.Left = value;
            }
        }

        [Description("Gets/Sets Top position of Header or Footer Text.")]
        public int Top
        {
            get
            {
                return base.Top;
            }
            set
            {
                base.Top = value;
            }
        }

        internal class TitleComponentEditor : UITypeEditor
        {
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                Title t = (Title) value;
                bool flag = EditorUtils.ShowFormModal(new TitleEditor(t, null));
                if ((context != null) && flag)
                {
                    context.OnComponentChanged();
                }
                return value;
            }

            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.Modal;
            }
        }
    }
}

