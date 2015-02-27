namespace Steema.TeeChart.Tools
{
    using Steema.TeeChart;
    using System;
    using System.Drawing;

    [ToolboxBitmap(typeof(PageNumber), "ToolsIcons.PageNumber.bmp")]
    public class PageNumber : Annotation
    {
        public PageNumber() : this(null)
        {
        }

        public PageNumber(Chart c) : base(c)
        {
            base.Text = Texts.PageOfPages;
        }

        protected override string GetInnerText()
        {
            if (!base.DesignMode)
            {
                try
                {
                    return string.Format(this.Format, base.chart.Page.Current, base.chart.Page.Count);
                }
                catch
                {
                    return this.Format;
                }
            }
            return this.Format;
        }

        public override string Description
        {
            get
            {
                return Texts.PageNumber;
            }
        }

        public string Format
        {
            get
            {
                return base.GetInnerText();
            }
            set
            {
                base.Text = value;
            }
        }
    }
}

