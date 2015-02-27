namespace Steema.TeeChart
{
    using Steema.TeeChart.Editors;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    [ToolboxBitmap(typeof(ButtonColor), "Images.ButtonColor.bmp")]
    public class ButtonColor : Button
    {
        private System.Drawing.Color color;
        private Rectangle r;

        public ButtonColor() : this(System.Drawing.Color.Empty)
        {
        }

        public ButtonColor(System.Drawing.Color c)
        {
            this.color = System.Drawing.Color.Empty;
            this.r = new Rectangle(3, 6, 10, 10);
            this.color = c;
            base.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        }

        protected override void OnClick(EventArgs e)
        {
            System.Drawing.Color color = ColorEditor.Choose(this.color, this);
            if (this.color != color)
            {
                this.Color = color;
            }
            base.OnClick(e);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            pe.Graphics.FillRectangle(new SolidBrush(this.color), this.r);
            pe.Graphics.DrawRectangle(Pens.Black, this.r);
        }

        [DefaultValue(typeof(System.Drawing.Color), "Empty")]
        public System.Drawing.Color Color
        {
            get
            {
                return this.color;
            }
            set
            {
                this.color = value;
                base.Invalidate();
            }
        }

        [DefaultValue(0)]
        public System.Windows.Forms.FlatStyle FlatStyle
        {
            get
            {
                return base.FlatStyle;
            }
            set
            {
                base.FlatStyle = value;
            }
        }
    }
}

