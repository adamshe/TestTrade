namespace Steema.TeeChart
{
    using Steema.TeeChart.Drawing;
    using Steema.TeeChart.Editors;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    [ToolboxBitmap(typeof(ButtonPen), "Images.ButtonPen.bmp")]
    public class ButtonPen : Button
    {
        private ChartPen pen;

        public ButtonPen() : this(null)
        {
        }

        public ButtonPen(ChartPen p)
        {
            this.pen = p;
            base.FlatStyle = FlatStyle.Flat;
            this.Text = Texts.Border;
        }

        protected override void OnClick(EventArgs e)
        {
            if ((this.pen != null) && PenEditor.Edit(this.pen))
            {
                base.Invalidate();
            }
            base.OnClick(e);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            if (this.pen != null)
            {
                int num = pe.ClipRectangle.Height / 2;
                pe.Graphics.DrawLine(this.pen.DrawingPen, 3, num, 10, num);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public ChartPen Pen
        {
            get
            {
                return this.pen;
            }
            set
            {
                this.pen = value;
                base.Invalidate();
            }
        }
    }
}

