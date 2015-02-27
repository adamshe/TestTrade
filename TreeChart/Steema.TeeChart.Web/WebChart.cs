namespace Steema.TeeChart.Web
{
    using Microsoft.Win32;
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Editors.Export;
    using Steema.TeeChart.Export;
    using Steema.TeeChart.Styles;
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Design;
    using System.Drawing.Printing;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.Serialization.Formatters;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.Design;
    using System.Web.UI.WebControls;
    using System.Windows.Forms;
    using System.Xml;

    [Serializable, ToolboxData("<{0}:WebChart runat=server></{0}:WebChart>"), Designer(typeof(Designer)), ToolboxBitmap(typeof(WebChart), "Images.WebChart.bmp"), DefaultProperty("Chart")]
    public class WebChart : WebControl, IComponent, IDisposable, INamingContainer, IPostBackEventHandler, IPostBackDataHandler, IChart
    {
        private bool autoPostback = false;
        private int clickedX;
        private int clickedY;
        private string config;
        protected internal string designPath = "";
        protected internal Steema.TeeChart.Chart iChart = new Steema.TeeChart.Chart();
        private string iGetChartFile = "GetChart.aspx";
        private bool isObjectevent = false;
        private PictureFormats pictureFormat = PictureFormats.PNG;
        private bool refreshing = false;
        private string shareURL = "";
        private TempChartStyle tempChart = TempChartStyle.File;
        private string tmpFolder = "_chart_temp";

        public event PaintChartEventHandler AfterDraw;

        public event PaintChartEventHandler BeforeDraw;

        public event PaintChartEventHandler BeforeDrawAxes;

        public event PaintChartEventHandler BeforeDrawSeries;

        public event ClickEventHandler ClickAxis;

        public event ClickEventHandler ClickBackground;

        public event ClickEventHandler ClickLegend;

        public event SeriesEventHandler ClickSeries;

        public event ClickEventHandler ClickTitle;

        public event GetAxisLabelEventHandler GetAxisLabel;

        public event GetLegendPosEventHandler GetLegendPos;

        public event GetLegendRectEventHandler GetLegendRect;

        public event GetLegendTextEventHandler GetLegendText;

        public event GetNextAxisLabelEventHandler GetNextAxisLabel;

        public WebChart()
        {
            if (this.TempChart == TempChartStyle.File)
            {
                this.GetShareFolder();
            }
            this.Chart.parent = this;
        }

        internal string CreateDesignPictureFile()
        {
            ImageExportFormat format = this.iChart.Export.Image.FromFormat(this.pictureFormat);
            format.Width = (int) this.Width.Value;
            format.Height = (int) this.Height.Value;
            string str = "";
            if (base.Site != null)
            {
                str = this.ID + "." + format.FileExtension;
                format.Save(Path.GetTempPath() + @"\" + str);
                return ("<IMG SRC=\"" + Path.GetTempPath() + @"\" + str + "\">");
            }
            return "";
        }

        internal void CreatePictureFile(HtmlTextWriter writer)
        {
            this.Chart.parent = this;
            ImageExportFormat format = this.iChart.Export.Image.FromFormat(this.pictureFormat);
            format.Width = (int) this.Width.Value;
            format.Height = (int) this.Height.Value;
            string str = DateTime.Now.ToOADate().ToString();
            string name = this.ID.ToString() + this.Context.Request.UserHostAddress.ToString().Replace(".", "") + str.Replace(",", "").Replace("/", "").Replace(":", "").Replace(" ", "").Replace(".", "");
            string str3 = name + "." + format.FileExtension;
            if (this.isObjectevent)
            {
                this.Chart.Bitmap(format.Width, format.Height);
                MouseEventArgs e = new MouseEventArgs(MouseButtons.Left, 1, this.clickedX, this.clickedY, 0);
                this.Chart.DoMouseDown(false, e);
                this.isObjectevent = false;
            }
            string fileName = this.designPath + @"\" + this.tmpFolder + @"\" + str3;
            if ((this.tempChart == TempChartStyle.Session) && (this.Context.Session == null))
            {
                this.tempChart = TempChartStyle.File;
            }
            if (this.tempChart == TempChartStyle.Session)
            {
                MemoryStream stream = new MemoryStream();
                format.Save(stream);
                this.Context.Session.Add(name, stream);
            }
            else
            {
                this.MakeShareFolder();
                format.Save(fileName);
            }
            string str5 = "";
            foreach (string str6 in base.Attributes.Keys)
            {
                str5 = str5 + str6 + "=\"" + base.Attributes[str6];
            }
            string str7 = "";
            if (this.AutoPostback)
            {
                str7 = "<input type=\"image\" ";
            }
            else
            {
                str7 = "<IMG ";
            }
            if (this.tempChart == TempChartStyle.Session)
            {
                writer.Write(string.Concat(new object[] { 
                    str7, "SRC=\"", this.iGetChartFile, "?Chart=", name, "\" id=\"", this.ID.ToString(), "\" Name=\"", this.ID.ToString(), "\" ", str5, "; height:", format.Height, this.GetUnitType(this.Height), "; width:", format.Width, 
                    this.GetUnitType(this.Width), "\">"
                 }));
            }
            else
            {
                writer.Write(string.Concat(new object[] { 
                    str7, "SRC=\"", this.shareURL, "/", this.tmpFolder, "/", str3, "\" id=\"", this.ID.ToString(), "\" Name=\"", this.ID.ToString(), "\" ", str5, "; height:", format.Height, this.GetUnitType(this.Height), 
                    "; width:", format.Width, this.GetUnitType(this.Width), "\">"
                 }));
            }
        }

        protected internal void CreatePictureStream(HtmlTextWriter writer)
        {
            StringBuilder builder = new StringBuilder();
            ImageExportFormat format = this.iChart.Export.Image.FromFormat(this.pictureFormat);
            format.Width = (int) this.Width.Value;
            format.Height = (int) this.Height.Value;
            builder.Append("TeeChartImgGen.ashx");
            writer.WriteBeginTag("img");
            writer.WriteAttribute("src", builder.ToString() + "?Width=" + format.Width.ToString() + "&Height=" + format.Height.ToString() + "&ChartID=" + this.ID.ToString());
            if (this.ID != null)
            {
                writer.WriteAttribute("id", this.ClientID);
            }
            writer.Write('>');
        }

        protected internal MemoryStream DecodeBase64(string base64String)
        {
            byte[] buffer;
            MemoryStream stream = new MemoryStream();
            stream.Position = 0L;
            try
            {
                buffer = Convert.FromBase64String(base64String);
                stream.Write(buffer, 0, buffer.Length);
                stream.Flush();
            }
            catch (Exception exception)
            {
                Console.WriteLine("{0}", exception.Message);
            }
            buffer = null;
            stream.Position = 0L;
            return stream;
        }

        public override void Dispose()
        {
            this.iChart.Dispose();
            base.Dispose();
        }

        protected string GetBase64(Stream mstr)
        {
            byte[] buffer;
            string str = "";
            mstr.Position = 0L;
            try
            {
                buffer = new byte[mstr.Length];
                mstr.Read(buffer, 0, (int) mstr.Length);
                mstr.Flush();
                mstr.Close();
            }
            catch (Exception exception)
            {
                Console.WriteLine("{0}", exception.Message);
                return null;
            }
            str = Convert.ToBase64String(buffer, 0, buffer.Length);
            buffer = null;
            return str;
        }

        private void GetShareFolder()
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"Software\Steema Software\TeeChart.Net");
            if (key != null)
            {
                this.shareURL = (string) key.GetValue("VirtualShare");
                this.designPath = (string) key.GetValue("ShareFolder");
            }
        }

        private string GetUnitType(Unit unit)
        {
            UnitType type = unit.Type;
            if (type != UnitType.Pixel)
            {
                if (type == UnitType.Percentage)
                {
                    return "%";
                }
                return "px";
            }
            return "px";
        }

        protected void InternalLoadViewState(MemoryStream savedState, ref Steema.TeeChart.Chart chart)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            savedState.Position = 0L;
            formatter.AssemblyFormat = FormatterAssemblyStyle.Simple;
            SerializeBinder binder = new SerializeBinder();
            binder.BindToType("TeeChart", "Chart");
            formatter.Binder = binder;
            try
            {
                savedState.Position = 0L;
                savedState.Flush();
                try
                {
                    object obj2 = formatter.Deserialize(savedState);
                    chart = (Steema.TeeChart.Chart) obj2;
                    savedState.Position = 0L;
                    savedState.Flush();
                    savedState.Close();
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
                formatter = null;
            }
            catch (Exception exception2)
            {
                Console.WriteLine(exception2.Message);
            }
        }

        protected object InternalSaveViewState(Steema.TeeChart.Chart chart)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.AssemblyFormat = FormatterAssemblyStyle.Simple;
            MemoryStream serializationStream = new MemoryStream();
            formatter.Serialize(serializationStream, chart);
            return serializationStream;
        }

        public virtual bool LoadPostData(string postDataKey, NameValueCollection postCollection)
        {
            IEnumerator enumerator = postCollection.GetEnumerator();
            string str = postDataKey;
            while (str.IndexOf(":") != -1)
            {
                str = str.Substring(str.IndexOf(":") + 1);
            }
            while (enumerator.MoveNext())
            {
                string str2 = enumerator.Current.ToString();
                if (str2.LastIndexOf(".") != -1)
                {
                    str2 = str2.Substring(0, str2.LastIndexOf("."));
                }
                if (str2 == str)
                {
                    this.isObjectevent = true;
                    this.clickedX = Convert.ToInt32(postCollection[str + ".x"]);
                    this.clickedY = Convert.ToInt32(postCollection[str + ".y"]);
                    break;
                }
            }
            return false;
        }

        private void MakeShareFolder()
        {
            if (!Directory.Exists(this.designPath + @"\" + this.tmpFolder))
            {
                Directory.CreateDirectory(this.designPath + @"\" + this.tmpFolder);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.Config != null)
            {
                try
                {
                    this.InternalLoadViewState(this.DecodeBase64(this.Config), ref this.iChart);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            this.Page.RegisterRequiresPostBack(this);
        }

        public void RaisePostBackEvent(string eventArgument)
        {
        }

        public virtual void RaisePostDataChangedEvent()
        {
        }

        protected internal MemoryStream ReadFromFile(string chartID, string filePath, string extension)
        {
            string str = filePath;
            string url = str + chartID + "." + extension;
            XmlTextReader reader = new XmlTextReader(url);
            reader.WhitespaceHandling = WhitespaceHandling.None;
            string innerText = "";
            XmlDocument document = new XmlDocument();
            try
            {
                document.Load(url);
                foreach (XmlNode node in document.GetElementsByTagName(chartID))
                {
                    innerText = node.InnerText;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            reader.Close();
            document = null;
            return this.DecodeBase64(innerText);
        }

        protected override void Render(HtmlTextWriter output)
        {
            this.CreatePictureFile(output);
            if (this.AutoPostback)
            {
                this.Page.GetPostBackClientEvent(this, "");
            }
        }

        void IChart.CheckBackground(object sender, MouseEventArgs e)
        {
            if (this.ClickBackground != null)
            {
                this.iChart.CancelMouse = true;
                ImageClickEventArgs args = new ImageClickEventArgs(e.X, e.Y);
                this.ClickBackground(sender, args);
                this.iChart.IClicked = this.iChart.CancelMouse;
            }
        }

        bool IChart.CheckClickSeries()
        {
            return (this.ClickSeries != null);
        }

        bool IChart.CheckGetAxisLabelAssigned()
        {
            return (this.GetAxisLabel != null);
        }

        void IChart.CheckTitle(Title ATitle, MouseEventArgs e)
        {
            if (this.ClickTitle != null)
            {
                this.iChart.CancelMouse = true;
                ImageClickEventArgs args = new ImageClickEventArgs(e.X, e.Y);
                this.ClickTitle(ATitle, args);
                this.iChart.IClicked = this.iChart.CancelMouse;
            }
            if (!this.iChart.IClicked)
            {
                this.iChart.CheckZoomPanning(e);
            }
        }

        void IChart.DoAfterDraw()
        {
            if (this.AfterDraw != null)
            {
                this.AfterDraw(this, this.iChart.graphics3D);
            }
        }

        bool IChart.DoAllowScroll(Axis a, double Delta, ref double Min, ref double Max)
        {
            return true;
        }

        void IChart.DoBeforeDraw()
        {
            if (this.BeforeDraw != null)
            {
                this.BeforeDraw(this, this.iChart.graphics3D);
            }
        }

        void IChart.DoBeforeDrawAxes()
        {
            if (this.BeforeDrawAxes != null)
            {
                this.BeforeDrawAxes(this, this.iChart.graphics3D);
            }
        }

        void IChart.DoBeforeDrawSeries()
        {
            if (this.BeforeDrawSeries != null)
            {
                this.BeforeDrawSeries(this, this.iChart.graphics3D);
            }
        }

        void IChart.DoChartPrint(object sender, PrintPageEventArgs e)
        {
        }

        void IChart.DoClickAxis(Axis a, MouseEventArgs e)
        {
            if (this.ClickAxis != null)
            {
                this.iChart.CancelMouse = true;
                ImageClickEventArgs args = new ImageClickEventArgs(e.X, e.Y);
                this.ClickAxis(a, args);
            }
        }

        void IChart.DoClickLegend(object sender, MouseEventArgs e)
        {
            if (this.ClickLegend != null)
            {
                this.iChart.CancelMouse = true;
                ImageClickEventArgs args = new ImageClickEventArgs(e.X, e.Y);
                this.ClickLegend(sender, args);
                this.iChart.IClicked = this.iChart.CancelMouse;
            }
        }

        void IChart.DoClickSeries(object sender, Series s, int valueIndex, MouseEventArgs e)
        {
            if (this.ClickSeries != null)
            {
                this.ClickSeries(this, s, valueIndex, e);
            }
        }

        void IChart.DoGetAxisLabel(object sender, Series s, int valueIndex, ref string labelText)
        {
            if (this.GetAxisLabel != null)
            {
                GetAxisLabelEventArgs e = new GetAxisLabelEventArgs(s, valueIndex, labelText);
                this.GetAxisLabel(sender, e);
                labelText = e.LabelText;
            }
        }

        void IChart.DoGetLegendPos(object sender, int index, ref int X, ref int Y, ref int XColor)
        {
            if (this.GetLegendPos != null)
            {
                GetLegendPosEventArgs e = new GetLegendPosEventArgs(index, X, Y, XColor);
                this.GetLegendPos(sender, e);
                X = e.X;
                Y = e.Y;
                XColor = e.XColor;
            }
        }

        void IChart.DoGetLegendRectangle(object sender, ref Rectangle rect)
        {
            if (this.GetLegendRect != null)
            {
                GetLegendRectEventArgs e = new GetLegendRectEventArgs(rect);
                this.GetLegendRect(sender, e);
                rect = e.Rectangle;
            }
        }

        void IChart.DoGetLegendText(object sender, LegendStyles LegendStyle, int Index, ref string Text)
        {
            if (this.GetLegendText != null)
            {
                GetLegendTextEventArgs e = new GetLegendTextEventArgs(LegendStyle, Index, Text);
                this.GetLegendText(sender, e);
                Text = e.Text;
            }
        }

        void IChart.DoGetNextAxisLabel(object sender, int labelIndex, ref double labelValue, ref bool doStop)
        {
            if (this.GetNextAxisLabel != null)
            {
                GetNextAxisLabelEventArgs e = new GetNextAxisLabelEventArgs(labelIndex, labelValue, doStop);
                this.GetNextAxisLabel(sender, e);
                labelValue = e.LabelValue;
                doStop = e.Stop;
            }
        }

        void IChart.DoInvalidate()
        {
            if (!this.refreshing)
            {
                this.refreshing = true;
            }
            try
            {
                if (base.Site != null)
                {
                    IComponentChangeService service = (IComponentChangeService) base.Site.GetService(typeof(IComponentChangeService));
                    if (service != null)
                    {
                        service.OnComponentChanged(this, null, null, null);
                    }
                }
            }
            finally
            {
                this.refreshing = false;
            }
        }

        void IChart.DoScroll(object sender, EventArgs e)
        {
        }

        void IChart.DoSetControlStyle()
        {
        }

        void IChart.DoUndoneZoom(object sender, EventArgs e)
        {
        }

        void IChart.DoZoomed(object sender, EventArgs e)
        {
        }

        Form IChart.FindParentForm()
        {
            return null;
        }

        IContainer IChart.GetContainer()
        {
            return null;
        }

        System.Windows.Forms.Control IChart.GetControl()
        {
            return null;
        }

        Cursor IChart.GetCursor()
        {
            return null;
        }

        Point IChart.PointToScreen(Point p)
        {
            return p;
        }

        void IChart.RefreshControl()
        {
        }

        void IChart.SetChart(Steema.TeeChart.Chart c)
        {
            this.iChart = c;
        }

        void IChart.SetCursor(Cursor c)
        {
        }

        [Description("When True TeeChart's WebChart will respond to user mouseclick events.")]
        public bool AutoPostback
        {
            get
            {
                return this.autoPostback;
            }
            set
            {
                this.autoPostback = value;
            }
        }

        [Editor(typeof(WebChartEditor), typeof(UITypeEditor)), NotifyParentProperty(true), Description("Chart configuration."), Bindable(true), RefreshProperties(RefreshProperties.Repaint)]
        public Steema.TeeChart.Chart Chart
        {
            get
            {
                if (this.iChart.parent == null)
                {
                    this.iChart.parent = this;
                }
                return this.iChart;
            }
        }

        [Browsable(false), PersistenceMode(PersistenceMode.Attribute), DefaultValue((string) null)]
        public string Config
        {
            get
            {
                return this.config;
            }
            set
            {
                this.config = value;
            }
        }

        [Description("Chart Image Format"), DefaultValue(3)]
        public PictureFormats PictureFormat
        {
            get
            {
                return this.pictureFormat;
            }
            set
            {
                this.pictureFormat = value;
            }
        }

        [Description("When set to 'Session' TeeChart streams the Chart to a Session variable for direct retrieval from the WebForm (no temporary file). Requires script page 'GetChart'. See Documentation for details.")]
        public TempChartStyle TempChart
        {
            get
            {
                return this.tempChart;
            }
            set
            {
                this.tempChart = value;
            }
        }

        public delegate void ClickEventHandler(object sender, ImageClickEventArgs e);

        internal sealed class Designer : ControlDesigner
        {
            public Designer()
            {
                this.AddVerbs();
            }

            private void aboutEvent(object sender, EventArgs e)
            {
                WebChart component = (WebChart) base.Component;
                AboutBox.ShowModal();
            }

            private void AddVerbs()
            {
                DesignTimeOptions.InitLanguage(true, true);
                this.Verbs.Add(new DesignerVerb(Texts.About, new EventHandler(this.aboutEvent)));
                this.Verbs.Add(new DesignerVerb(Texts.Edit, new EventHandler(this.editorEvent)));
                this.Verbs.Add(new DesignerVerb(Texts.ExportChart, new EventHandler(this.exportEvent)));
                this.Verbs.Add(new DesignerVerb(Texts.ImportChart, new EventHandler(this.importEvent)));
                this.Verbs.Add(new DesignerVerb(Texts.PrintPreview, new EventHandler(this.previewEvent)));
                this.Verbs.Add(new DesignerVerb(Texts.Options, new EventHandler(this.optionsEvent)));
                this.Verbs.Add(new DesignerVerb(Texts.OnlineSupport, new EventHandler(this.supportEvent)));
            }

            private void editorEvent(object sender, EventArgs e)
            {
                WebChart component = (WebChart) base.Component;
                component.iChart.parent = component;
                ChartEditor.ShowModal(component.iChart);
                base.IsDirty = true;
                this.UpdateConfig();
                this.UpdateDesignTimeHtml();
            }

            private void exportEvent(object sender, EventArgs e)
            {
                WebChart component = (WebChart) base.Component;
                ExportEditor.ShowModal(component.Chart);
            }

            public override string GetDesignTimeHtml()
            {
                WebChart component = (WebChart) base.Component;
                return component.CreateDesignPictureFile();
            }

            private void importEvent(object sender, EventArgs e)
            {
                WebChart component = (WebChart) base.Component;
                ImportEditor.ShowModal(component.Chart);
                this.UpdateConfig();
                this.UpdateDesignTimeHtml();
            }

            private void optionsEvent(object sender, EventArgs e)
            {
                WebChart component = (WebChart) base.Component;
                using (DesignTimeOptions options = new DesignTimeOptions(component.Chart))
                {
                    if (EditorUtils.ShowFormModal(options))
                    {
                        options.StoreSettings();
                    }
                }
            }

            private void previewEvent(object sender, EventArgs e)
            {
                WebChart component = (WebChart) base.Component;
                PrintPreview.ShowModal(component.Chart);
                base.RaiseComponentChanged(null, null, null);
            }

            private void supportEvent(object sender, EventArgs e)
            {
                Process.Start("http://www.teechart.net/support/index.php");
            }

            internal void UpdateConfig()
            {
                WebChart component = (WebChart) base.Component;
                string oldValue = "";
                component.Config = component.GetBase64((MemoryStream) component.InternalSaveViewState(component.Chart));
                base.IsDirty = true;
                PropertyDescriptor member = TypeDescriptor.GetProperties(component).Find("Config", true);
                base.RaiseComponentChanged(member, oldValue, component.Config);
            }
        }

        public delegate void SeriesEventHandler(object sender, Series s, int valueIndex, EventArgs e);

        internal sealed class WebChartEditor : UITypeEditor
        {
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                bool flag = ChartEditor.ShowModal((Chart) value);
                if ((context != null) && flag)
                {
                    context.OnComponentChanged();
                }
                return value;
            }

            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.Modal;
            }
        }
    }
}

