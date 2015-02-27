namespace Steema.TeeChart.Export
{
    using Steema.TeeChart;
    using System;

    public class DataExport
    {
        internal Chart chart;
        private ExcelFormat excelFormat;
        private HTMLFormat htmlFormat;
        private TextFormat txtFormat;
        private XMLFormat xmlFormat;

        public DataExport(Chart c)
        {
            this.chart = c;
        }

        public ExcelFormat Excel
        {
            get
            {
                if (this.excelFormat == null)
                {
                    this.excelFormat = new ExcelFormat(this.chart);
                }
                return this.excelFormat;
            }
        }

        public HTMLFormat HTML
        {
            get
            {
                if (this.htmlFormat == null)
                {
                    this.htmlFormat = new HTMLFormat(this.chart);
                }
                return this.htmlFormat;
            }
        }

        public TextFormat Text
        {
            get
            {
                if (this.txtFormat == null)
                {
                    this.txtFormat = new TextFormat(this.chart);
                }
                return this.txtFormat;
            }
        }

        public XMLFormat XML
        {
            get
            {
                if (this.xmlFormat == null)
                {
                    this.xmlFormat = new XMLFormat(this.chart);
                }
                return this.xmlFormat;
            }
        }
    }
}

