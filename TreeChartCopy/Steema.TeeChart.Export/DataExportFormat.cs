namespace Steema.TeeChart.Export
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    public class DataExportFormat : ExportFormat
    {
        internal Steema.TeeChart.Chart Chart;
        internal bool hasColors;
        internal bool hasLabels;
        internal bool hasMarkPositions;
        internal bool hasNoMandatory;
        private bool includeHeader;
        private bool includeIndex;
        private bool includeLabels;
        private Steema.TeeChart.Styles.Series series;
        private string textLineSeparator;

        public DataExportFormat(Steema.TeeChart.Chart c)
        {
            this.includeLabels = true;
            this.textLineSeparator = "\n";
            this.Chart = c;
        }

        public DataExportFormat(Steema.TeeChart.Chart c, Steema.TeeChart.Styles.Series s)
        {
            this.includeLabels = true;
            this.textLineSeparator = "\n";
            this.Chart = c;
            this.Series = s;
        }

        public virtual void CopyToClipboard()
        {
            Clipboard.SetDataObject(new DataObject(this.DataFormat, this.GetContent()), true);
        }

        internal virtual string GetContent()
        {
            this.Prepare();
            string str = "";
            int num = this.MaxSeriesCount();
            for (int i = 0; i < num; i++)
            {
                str = str + this.PointToString(i) + this.TextLineSeparator;
            }
            return str;
        }

        private bool HasColors(Steema.TeeChart.Styles.Series aSeries)
        {
            Steema.TeeChart.Styles.Series series = aSeries;
            Color color = series.Color;
            int num = ((series.Count - 1) < 0x2710) ? (series.Count - 1) : 0x2710;
            for (int i = 0; i <= num; i++)
            {
                Color color2 = series.ValueColor(i);
                if (!color2.IsEmpty && (color2 != color))
                {
                    return true;
                }
            }
            return false;
        }

        private bool HasLabels(Steema.TeeChart.Styles.Series aSeries)
        {
            if (aSeries.Labels.Count > 0)
            {
                int num = ((aSeries.Count - 1) < 0x2710) ? (aSeries.Count - 1) : 0x2710;
                for (int i = 0; i <= num; i++)
                {
                    if (aSeries.Labels[i].Length != 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool HasNoMandatoryValues(Steema.TeeChart.Styles.Series aSeries)
        {
            Steema.TeeChart.Styles.Series series = aSeries;
            if (series.Count > 0)
            {
                ValueList notMandatory = series.notMandatory;
                if ((notMandatory.First != 0.0) || (notMandatory.Last != (series.Count - 1)))
                {
                    return true;
                }
                int num = ((series.Count - 1) < 0x2710) ? (series.Count - 1) : 0x2710;
                for (int i = 0; i <= num; i++)
                {
                    if (notMandatory[i] != i)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        internal int MaxSeriesCount()
        {
            if (this.Series != null)
            {
                return this.Series.Count;
            }
            int count = -1;
            foreach (Steema.TeeChart.Styles.Series series in this.Chart.Series)
            {
                if (series.Count > count)
                {
                    count = series.Count;
                }
            }
            return count;
        }

        internal virtual string PointToString(int index)
        {
            return "";
        }

        internal void Prepare()
        {
            Steema.TeeChart.Styles.Series series;
            if (this.Series != null)
            {
                series = this.Series;
            }
            else if (this.Chart.Series.Count > 0)
            {
                series = this.Chart[0];
            }
            else
            {
                series = null;
            }
            if (series != null)
            {
                this.SeriesGuessContents(series);
            }
            if (!this.IncludeLabels)
            {
                this.hasLabels = false;
            }
        }

        public virtual void Save(Stream stream)
        {
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(this.GetContent());
            writer.Flush();
        }

        public void Save(string fileName)
        {
            using (FileStream stream = new FileStream(fileName, FileMode.Create))
            {
                this.Save(stream);
                stream.Close();
            }
        }

        private void SeriesGuessContents(Steema.TeeChart.Styles.Series aSeries)
        {
            this.hasNoMandatory = this.HasNoMandatoryValues(aSeries);
            this.hasColors = this.HasColors(aSeries);
            this.hasLabels = this.HasLabels(aSeries);
            this.hasMarkPositions = aSeries.Marks.Positions.ExistCustom();
        }

        protected virtual string DataFormat
        {
            get
            {
                return DataFormats.Text;
            }
        }

        public bool IncludeHeader
        {
            get
            {
                return this.includeHeader;
            }
            set
            {
                this.includeHeader = value;
            }
        }

        public bool IncludeIndex
        {
            get
            {
                return this.includeIndex;
            }
            set
            {
                this.includeIndex = value;
            }
        }

        public bool IncludeLabels
        {
            get
            {
                return this.includeLabels;
            }
            set
            {
                this.includeLabels = value;
            }
        }

        public Steema.TeeChart.Styles.Series Series
        {
            get
            {
                return this.series;
            }
            set
            {
                this.series = value;
            }
        }

        public string TextLineSeparator
        {
            get
            {
                return this.textLineSeparator;
            }
            set
            {
                this.textLineSeparator = value;
            }
        }
    }
}

