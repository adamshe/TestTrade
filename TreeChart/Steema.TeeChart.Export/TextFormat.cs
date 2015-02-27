namespace Steema.TeeChart.Export
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;

    public class TextFormat : DataExportFormat
    {
        public string TextDelimiter;

        public TextFormat(Chart c) : base(c)
        {
            this.TextDelimiter = Texts.TabDelimiter;
            base.FileExtension = "txt";
        }

        private void Add(string st, ref string tmpResult)
        {
            tmpResult = (tmpResult.Length == 0) ? st : (tmpResult + this.TextDelimiter + st);
        }

        private void DoSeries(int index, Series aSeries, ref int tmpNum, ref string tmpResult)
        {
            double num2;
            tmpNum++;
            if ((tmpNum == 1) && base.hasLabels)
            {
                if (aSeries.Count > index)
                {
                    this.Add(aSeries.Labels[index], ref tmpResult);
                }
                else
                {
                    this.Add("", ref tmpResult);
                }
            }
            if (base.hasNoMandatory)
            {
                if (aSeries.Count > index)
                {
                    num2 = aSeries.notMandatory[index];
                    this.Add(num2.ToString(), ref tmpResult);
                }
                else
                {
                    this.Add("", ref tmpResult);
                }
            }
            if (aSeries.Count > index)
            {
                num2 = aSeries.mandatory[index];
                this.Add(num2.ToString(), ref tmpResult);
            }
            else
            {
                this.Add("", ref tmpResult);
            }
            for (int i = 2; i < aSeries.valuesList.Count; i++)
            {
                if (aSeries.Count > index)
                {
                    tmpResult = tmpResult + this.TextDelimiter + aSeries.valuesList[i][index].ToString();
                }
                else
                {
                    tmpResult = tmpResult + this.TextDelimiter + "";
                }
            }
        }

        internal override string FilterFiles()
        {
            return Texts.TextFilter;
        }

        internal override string GetContent()
        {
            base.Prepare();
            string str = base.IncludeHeader ? (this.Header() + base.TextLineSeparator) : "";
            return (str + base.GetContent() + base.TextLineSeparator);
        }

        private string Header()
        {
            string str = base.IncludeIndex ? Texts.Index : "";
            if (base.hasLabels)
            {
                if (str.Length != 0)
                {
                    str = str + this.TextDelimiter;
                }
                str = str + Texts.Text;
            }
            if (str.Length != 0)
            {
                str = str + this.TextDelimiter;
            }
            if (base.Series != null)
            {
                return (str + this.HeaderSeries(base.Series));
            }
            if (base.Chart.Series.Count > 0)
            {
                str = str + this.HeaderSeries(base.Chart[0]);
                for (int i = 1; i < base.Chart.Series.Count; i++)
                {
                    str = str + this.TextDelimiter + this.HeaderSeries(base.Chart[i]);
                }
            }
            return str;
        }

        private string HeaderSeries(Series aSeries)
        {
            string str = "";
            if (base.hasNoMandatory)
            {
                str = str + aSeries.notMandatory.Name;
            }
            if (aSeries.valuesList.Count == 2)
            {
                return (str + aSeries.ToString());
            }
            str = str + aSeries.mandatory.Name;
            for (int i = 2; i < aSeries.valuesList.Count; i++)
            {
                str = str + this.TextDelimiter + aSeries.valuesList[i].Name;
            }
            return str;
        }

        internal override string PointToString(int index)
        {
            string tmpResult = base.IncludeIndex ? index.ToString() : "";
            int tmpNum = 0;
            if (base.Series != null)
            {
                this.DoSeries(index, base.Series, ref tmpNum, ref tmpResult);
                return tmpResult;
            }
            foreach (Series series in base.Chart.Series)
            {
                this.DoSeries(index, series, ref tmpNum, ref tmpResult);
            }
            return tmpResult;
        }
    }
}

