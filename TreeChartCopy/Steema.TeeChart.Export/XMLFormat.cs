namespace Steema.TeeChart.Export
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;

    public class XMLFormat : DataExportFormat
    {
        public XMLFormat(Chart c) : base(c)
        {
            base.FileExtension = "xml";
        }

        private void AddResult(string st, ref string tmpResult)
        {
            tmpResult = (tmpResult.Length == 0) ? st : (tmpResult + st);
        }

        internal override string FilterFiles()
        {
            return Texts.XMLFilter;
        }

        private string Get(ValueList aList, int index)
        {
            string[] strArray = new string[] { " ", aList.Name, "=\"", aList[index].ToString(), "\"" };
            return string.Concat(strArray);
        }

        internal override string GetContent()
        {
            string str = "";
            base.Prepare();
            if (base.Series != null)
            {
                return (str + this.XMLSeries(base.Series));
            }
            str = str + "<chart>" + base.TextLineSeparator;
            foreach (Series series in base.Chart.Series)
            {
                str = str + this.XMLSeries(series);
            }
            return (str + "</chart>");
        }

        private string GetPointString(int index, Series aSeries)
        {
            string tmpResult = base.IncludeIndex ? ("index=\"" + index.ToString() + "\"") : "";
            if (base.hasLabels)
            {
                tmpResult = tmpResult + " text=\"" + aSeries.Labels[index] + "\"";
            }
            if (base.hasNoMandatory)
            {
                this.AddResult(this.Get(aSeries.notMandatory, index), ref tmpResult);
            }
            this.AddResult(this.Get(aSeries.mandatory, index), ref tmpResult);
            for (int i = 2; i < aSeries.valuesList.Count; i++)
            {
                tmpResult = tmpResult + this.Get(aSeries.valuesList[i], index);
            }
            return tmpResult;
        }

        private string SeriesPoints(Series aSeries)
        {
            string str = "";
            if (aSeries.Count > 0)
            {
                for (int i = 0; i < aSeries.Count; i++)
                {
                    string str2 = str;
                    str = str2 + "<point " + this.GetPointString(i, aSeries) + "/>" + base.TextLineSeparator;
                }
            }
            return str;
        }

        private string XMLSeries(Series aSeries)
        {
            return ("<series title=\"" + aSeries.ToString() + "\" type=\"" + aSeries.Description + "\">" + base.TextLineSeparator + "<points count=\"" + aSeries.Count.ToString() + "\">" + base.TextLineSeparator + this.SeriesPoints(aSeries) + "</points>" + base.TextLineSeparator + "</series>" + base.TextLineSeparator + base.TextLineSeparator);
        }
    }
}

