namespace Steema.TeeChart.Data
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.Drawing;
    using System.Globalization;
    using System.Xml;

    public class XmlSource : SeriesSource
    {
        private Steema.TeeChart.Chart iChart;
        private string sDataMember;
        private string sSeriesNode;

        public XmlSource()
        {
            this.sSeriesNode = "";
            this.sDataMember = "";
        }

        public XmlSource(Steema.TeeChart.Chart c) : this()
        {
            this.Chart = c;
        }

        public XmlSource(Series s) : this()
        {
            base.Series = s;
        }

        private Color HexToColor(string s)
        {
            s = s.Trim();
            if (s.Substring(1) == "#")
            {
                return Color.FromArgb(Convert.ToInt32('$' + s.Substring(2, 2)), Convert.ToInt32('$' + s.Substring(4, 2)), Convert.ToInt32('$' + s.Substring(6, 2)));
            }
            return Color.Empty;
        }

        public void Load(string fileName)
        {
            XmlDocument d = new XmlDocument();
            d.Load(fileName);
            this.Load(d);
        }

        public void Load(XmlDocument d)
        {
            if ((this.Chart != null) || (base.Series != null))
            {
                XmlNodeList elementsByTagName = d.GetElementsByTagName("series");
                if (elementsByTagName == null)
                {
                    this.XMLError("No <series> nodes.");
                }
                else
                {
                    if (base.Series == null)
                    {
                        this.Chart.Series.Clear(true);
                    }
                    if (this.SeriesNode.Length == 0)
                    {
                        if (elementsByTagName.Count > 0)
                        {
                            for (int i = 0; i < elementsByTagName.Count; i++)
                            {
                                this.LoadSeriesNode(elementsByTagName[i]);
                                if (base.Series != null)
                                {
                                    return;
                                }
                            }
                        }
                        else
                        {
                            this.XMLError("Empty <series> node.");
                        }
                    }
                    else
                    {
                        bool flag = false;
                        for (int j = 0; j < elementsByTagName.Count; j++)
                        {
                            XmlNode namedItem = elementsByTagName[j].Attributes.GetNamedItem("title");
                            if ((namedItem != null) && (namedItem.Value.ToUpper(CultureInfo.CurrentCulture) == this.SeriesNode.ToUpper()))
                            {
                                this.LoadSeriesNode(elementsByTagName[j]);
                                flag = true;
                                break;
                            }
                        }
                        if (!flag)
                        {
                            this.XMLError("Series " + this.SeriesNode + " not found");
                        }
                    }
                }
            }
        }

        private void LoadSeriesNode(XmlNode node)
        {
            XmlNode node6;
            string valueSource;
            Series series = base.Series;
            if (series != null)
            {
                series.Clear();
            }
            else
            {
                Type type = null;
                XmlNode namedItem = node.Attributes.GetNamedItem("type");
                if (namedItem != null)
                {
                    valueSource = namedItem.Value.ToUpper();
                    foreach (Type type2 in Utils.SeriesTypesOf)
                    {
                        if (type2.ToString().ToUpper() == valueSource)
                        {
                            type = type2;
                            break;
                        }
                    }
                    if (type == null)
                    {
                        type = Utils.SeriesTypesOf[5];
                    }
                    series = this.Chart.Series.Add(type);
                    namedItem = node.Attributes.GetNamedItem("title");
                    if (namedItem != null)
                    {
                        series.Title = namedItem.Value;
                    }
                    node6 = node.Attributes.GetNamedItem("color");
                    if (node6 != null)
                    {
                        series.Color = this.HexToColor(node6.Value);
                    }
                }
            }
            XmlNodeList list = node.SelectNodes("//points");
            if (list != null)
            {
                XmlNodeList list2 = list[0].SelectNodes("//point");
                if (list2 == null)
                {
                    this.XMLError("No <point> nodes.");
                }
                else
                {
                    valueSource = series.mandatory.valueSource;
                    if (valueSource.Length == 0)
                    {
                        valueSource = this.DataMember;
                    }
                    if (valueSource.Length == 0)
                    {
                        valueSource = series.mandatory.Name;
                    }
                    string name = series.notMandatory.valueSource;
                    if (name.Length == 0)
                    {
                        name = series.notMandatory.Name;
                    }
                    for (int i = 0; i < list2.Count; i++)
                    {
                        XmlNode node2;
                        string str3;
                        Color empty;
                        XmlAttributeCollection attributes = list2[i].Attributes;
                        if (attributes == null)
                        {
                            this.XMLError("<point> node has no data.");
                            return;
                        }
                        XmlNode node4 = attributes.GetNamedItem("text");
                        if (node4 == null)
                        {
                            str3 = "";
                        }
                        else
                        {
                            str3 = node4.Value;
                        }
                        node6 = attributes.GetNamedItem("color");
                        if (node6 == null)
                        {
                            empty = Color.Empty;
                        }
                        else
                        {
                            empty = this.HexToColor(node6.Value);
                        }
                        for (int j = 2; j < series.ValuesLists.Count; j++)
                        {
                            string str4 = series.ValuesLists[j].valueSource;
                            if (str4.Length == 0)
                            {
                                str4 = series.ValuesLists[j].Name;
                            }
                            node2 = attributes.GetNamedItem(str4);
                            if (node2 != null)
                            {
                                series.ValuesLists[j].TempValue = Convert.ToDouble(node2.Value);
                            }
                        }
                        node2 = attributes.GetNamedItem(valueSource);
                        XmlNode node3 = attributes.GetNamedItem(name);
                        if (node2 == null)
                        {
                            if (node3 == null)
                            {
                                series.Add(str3);
                            }
                            else
                            {
                                series.Add(Convert.ToDouble(node3.Value), (double) 0.0, str3);
                            }
                        }
                        else if (node3 == null)
                        {
                            series.Add(Convert.ToDouble(node2.Value), str3, empty);
                        }
                        else
                        {
                            series.Add(Convert.ToDouble(node3.Value), Convert.ToDouble(node2.Value), str3, empty);
                        }
                    }
                }
            }
            else
            {
                this.XMLError("No <points> node.");
            }
        }

        private void XMLError(string error)
        {
            throw new Exception(error);
        }

        public Steema.TeeChart.Chart Chart
        {
            get
            {
                return this.iChart;
            }
            set
            {
                this.iChart = value;
            }
        }

        public string DataMember
        {
            get
            {
                return this.sDataMember;
            }
            set
            {
                this.sDataMember = value;
            }
        }

        public string SeriesNode
        {
            get
            {
                return this.sSeriesNode;
            }
            set
            {
                this.sSeriesNode = value;
            }
        }
    }
}

