namespace Steema.TeeChart.Export
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;

    public class HTMLFormat : DataExportFormat
    {
        private string emptyCell;

        public HTMLFormat(Chart c) : base(c)
        {
            this.emptyCell = "<td></td>";
            base.FileExtension = "htm";
        }

        private string Celldouble(double value)
        {
            return ("<td>" + value.ToString() + "</td>");
        }

        internal override string FilterFiles()
        {
            return Texts.HTMLFilter;
        }

        internal override string GetContent()
        {
            base.Prepare();
            string str = "<table border=\"1\">" + base.TextLineSeparator;
            if (base.IncludeHeader)
            {
                str = str + this.Header() + base.TextLineSeparator;
            }
            return (str + base.GetContent() + base.TextLineSeparator + "</table>");
        }

        private string GetPointString(int index)
        {
            string str = base.IncludeIndex ? ("<td>" + index.ToString() + "</td>") : "";
            if (base.Series != null)
            {
                return (str + this.GetPointStringSeries(base.Series, index));
            }
            foreach (Series series in base.Chart.Series)
            {
                str = str + this.GetPointStringSeries(series, index);
            }
            return str;
        }

        private string GetPointStringSeries(Series aSeries, int index)
        {
            int num;
            string str = "";
            if ((aSeries.Count - 1) < index)
            {
                if (base.hasLabels)
                {
                    str = str + this.emptyCell;
                }
                if (base.hasNoMandatory)
                {
                    str = str + this.emptyCell;
                }
                str = str + this.emptyCell;
                for (num = 2; num < aSeries.valuesList.Count; num++)
                {
                    str = str + this.emptyCell;
                }
                return str;
            }
            if (base.hasLabels)
            {
                str = str + "<td>" + aSeries.Labels[index] + "</td>";
            }
            if (base.hasNoMandatory)
            {
                str = str + this.Celldouble(aSeries.notMandatory[index]);
            }
            str = str + this.Celldouble(aSeries.mandatory[index]);
            for (num = 2; num < aSeries.valuesList.Count; num++)
            {
                str = str + this.Celldouble(aSeries.valuesList[num][index]);
            }
            return str;
        }

        private string Header()
        {
            string str = "<tr>";
            if (base.IncludeIndex)
            {
                str = str + "<td>" + Texts.Index + "</td>";
            }
            if (base.Series != null)
            {
                str = str + this.HeaderSeries(base.Series);
            }
            else
            {
                foreach (Series series in base.Chart.Series)
                {
                    str = str + this.HeaderSeries(series);
                }
            }
            return (str + "</tr>");
        }

        private string HeaderSeries(Series aSeries)
        {
            string str = "";
            if (base.hasLabels)
            {
                str = str + "<td>" + Texts.Text + "</td>";
            }
            if (base.hasNoMandatory)
            {
                str = str + "<td>" + aSeries.notMandatory.Name + "</td>";
            }
            if (aSeries.valuesList.Count == 2)
            {
                return (str + "<td>" + aSeries.ToString() + "</td>");
            }
            str = str + "<td>" + aSeries.mandatory.Name + "</td>";
            for (int i = 2; i < aSeries.valuesList.Count; i++)
            {
                str = str + "<td>" + aSeries.valuesList[i].Name + "</td>";
            }
            return str;
        }

        internal override string PointToString(int index)
        {
            return ("<tr>" + this.GetPointString(index) + "</tr>");
        }
    }
}

