namespace Steema.TeeChart.Data
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Net;

    [Serializable, ToolboxBitmap(typeof(TextSource), "Images.TextSource.bmp"), Description("Fills a Series with data from text or csv files or string streams.")]
    public class TextSource : SeriesSource
    {
        private TextFieldCollection fields;
        private string fileName;
        private int header;
        private char separator;

        public TextSource()
        {
            this.fields = new TextFieldCollection();
            this.separator = ',';
            this.fileName = "";
        }

        public TextSource(string fileName) : this()
        {
            this.fileName = fileName;
        }

        public void Add(StreamReader r)
        {
            bool flag = false;
            if (this.fields != null)
            {
                for (int i = 0; i < this.fields.Count; i++)
                {
                    TextField field = this.fields[i];
                    if (field.Name.ToUpper() == Texts.Text.ToUpper())
                    {
                        field.data = base.Series.Labels;
                    }
                    else if (field.Name.ToUpper() == base.Series.notMandatory.Name.ToUpper())
                    {
                        flag = true;
                        field.data = base.Series.notMandatory;
                    }
                    else
                    {
                        field.data = base.Series.GetYValueList(field.Name);
                    }
                }
                base.Series.Clear();
                if (this.header > 0)
                {
                    int num2 = 0;
                    for (string str = r.ReadLine(); str != null; str = r.ReadLine())
                    {
                        num2++;
                        if (num2 >= this.header)
                        {
                            break;
                        }
                    }
                }
                string text = "";
                string str3 = "";
                for (string str4 = r.ReadLine(); str4 != null; str4 = r.ReadLine())
                {
                    if (str4.Trim().Length != 0)
                    {
                        double count;
                        for (int j = 0; j < base.Series.ValuesLists.Count; j++)
                        {
                            base.Series.ValuesLists[j].TempValue = 0.0;
                        }
                        text = "";
                        string[] strArray = str4.Split(new char[] { this.separator });
                        for (int k = 0; k < this.fields.Count; k++)
                        {
                            TextField field2 = this.fields[k];
                            if ((field2.Index > -1) && (field2.Index < strArray.Length))
                            {
                                str3 = strArray[field2.Index];
                                if (field2.data == base.Series.Labels)
                                {
                                    text = str3;
                                }
                                else if (field2.data is ValueList)
                                {
                                    double num3;
                                    ValueList data = (ValueList) field2.data;
                                    if (data.DateTime)
                                    {
                                        num3 = Convert.ToDateTime(str3).ToOADate();
                                    }
                                    else
                                    {
                                        num3 = Convert.ToDouble(str3);
                                    }
                                    data.TempValue = num3;
                                }
                            }
                        }
                        if (!flag)
                        {
                            count = base.Series.Count;
                        }
                        else
                        {
                            count = base.Series.notMandatory.TempValue;
                        }
                        base.Series.Add(count, base.Series.mandatory.TempValue, text);
                    }
                }
                base.Series.RefreshSeries();
            }
        }

        public bool IsURL()
        {
            return this.FileName.ToUpper(CultureInfo.CurrentCulture).StartsWith("HTTP://");
        }

        public override void RefreshData()
        {
            if (this.fileName.Length != 0)
            {
                Stream stream;
                if (this.IsURL())
                {
                    WebClient client = new WebClient();
                    stream = new MemoryStream(client.DownloadData(this.fileName));
                }
                else
                {
                    stream = System.IO.File.Open(this.fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                }
                try
                {
                    StreamReader r = new StreamReader(stream);
                    try
                    {
                        this.Add(r);
                    }
                    finally
                    {
                        r.Close();
                    }
                }
                finally
                {
                    stream.Close();
                }
            }
        }

        [Description("Mapping text fields to series value lists."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TextFieldCollection Fields
        {
            get
            {
                return this.fields;
            }
        }

        [Description("File name to load to fill series."), DefaultValue("")]
        public string FileName
        {
            get
            {
                return this.fileName;
            }
            set
            {
                this.fileName = value;
            }
        }

        [Description("Number of text lines to skip processing."), DefaultValue(0)]
        public int HeaderLines
        {
            get
            {
                return this.header;
            }
            set
            {
                this.header = value;
            }
        }

        [DefaultValue(','), Description("Character to use as field separator.")]
        public char Separator
        {
            get
            {
                return this.separator;
            }
            set
            {
                this.separator = value;
            }
        }
    }
}

