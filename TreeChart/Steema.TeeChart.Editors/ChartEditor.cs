namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors.Export;
    using Steema.TeeChart.Editors.Tools;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Windows.Forms;

    public class ChartEditor : Form, ITeeEventListener
    {
        private AspectEditor aspectEditor;
        private AxesEditor axesEditor;
        private Button bHelp;
        private Button bSeriesDown;
        private Button bSeriesUp;
        private Button button1;
        private Button buttonAddSeries;
        private Button buttonChangeSeries;
        private Button buttonCloneSeries;
        private Button buttonDeleteSeries;
        private Button buttonTitleSeries;
        private ComboBox CBSeries;
        private Steema.TeeChart.Chart Chart;
        private ChartListBox chartListBox1;
        private CheckBox checkBox2;
        private IContainer components;
        private ExportEditor exportEditor;
        private GeneralEditor generalEditor;
        public string HelpFileName;
        private Label label4;
        private Label label7;
        private Steema.TeeChart.Editors.LegendEditor legendEditor;
        private LinkLabel linkLabel1;
        private Steema.TeeChart.Editors.PageEditor pageEditor;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.Panel panel12;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Panel panel9;
        private Steema.TeeChart.Editors.PanelEditor panelEditor;
        private System.Windows.Forms.Panel panelSeries;
        private PictureBox pictureBox2;
        private PrintPreview printPreview;
        private SeriesEditor seriesEditor;
        private TabPage tabAspect;
        private TabPage tabAxes;
        private TabPage tabChart;
        private TabControl tabControl1;
        private TabControl tabControl2;
        private TabControl tabControl3;
        private TabPage tabExport;
        private TabPage tabGeneral;
        private TabPage tabLegend;
        private TabPage tabPage10;
        private TabPage tabPage11;
        private TabPage tabPage3;
        private TabPage tabPage9;
        private TabPage tabPageSeries;
        private TabPage tabPaging;
        private TabPage tabPanel;
        private TabPage tabPrint;
        private TabPage tabSeries;
        private TabPage tabTitles;
        private TabPage tabTools;
        private TabPage tabWalls;
        private TitleEditor titlesEditor;
        private ToolsEditor toolsEditor;
        private Steema.TeeChart.Editors.WallEditor[] wallEditor;

        public ChartEditor()
        {
            this.wallEditor = new Steema.TeeChart.Editors.WallEditor[4];
            this.HelpFileName = "";
            base.SetStyle(ControlStyles.DoubleBuffer, true);
            this.InitializeComponent();
            this.chartListBox1.Chart = null;
        }

        public ChartEditor(Steema.TeeChart.Chart c)
        {
            this.wallEditor = new Steema.TeeChart.Editors.WallEditor[4];
            this.HelpFileName = "";
            this.Chart = c;
            if (this.Chart != null)
            {
                this.Chart.Listeners.Add(this);
            }
            base.SetStyle(ControlStyles.DoubleBuffer, true);
            this.InitializeComponent();
            EditorUtils.GetUpDown(this.bSeriesUp, this.bSeriesDown);
            base.Width = Convert.ToInt32(Texts.DefaultEditorSize);
            base.Height = Convert.ToInt32(Texts.DefaultEditorHeight);
            this.chartListBox1.SetChart(c);
            this.AddSeries();
            object[] destination = new object[this.CBSeries.Items.Count];
            this.CBSeries.Items.CopyTo(destination, 0);
            EditorUtils.Translate(this);
            this.CBSeries.Items.Clear();
            this.CBSeries.Items.AddRange(destination);
            if (this.Text.Length == 0)
            {
                this.Text = string.Format(Texts.Editing, c.ToString());
            }
        }

        private Steema.TeeChart.Styles.Series AddFromGallery()
        {
            Steema.TeeChart.Styles.Series s = ChartGallery.CreateNew(this.Chart, null);
            if (s != null)
            {
                this.AddToContainer(s);
                s.Chart = this.Chart;
                this.AddSeries();
                this.chartListBox1.SelectedSeries = s;
            }
            return s;
        }

        private void AddSeries()
        {
            this.CBSeries.Items.Clear();
            foreach (Steema.TeeChart.Styles.Series series in this.Chart.Series)
            {
                this.CBSeries.Items.Add(series.ToString());
            }
            this.chartListBox1.FillSeries(null);
            if (this.chartListBox1.Items.Count > 0)
            {
                this.chartListBox1.SelectedIndex = 0;
            }
            this.EnableButtons();
        }

        private void AddToContainer(Steema.TeeChart.Styles.Series s)
        {
            IContainer chartContainer = this.Chart.ChartContainer;
            if (chartContainer != null)
            {
                s.AddToContainer(chartContainer);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Steema.TeeChart.Styles.Series series = this.AddFromGallery();
            if ((series != null) && (series.Function != null))
            {
                this.tabControl1.SelectedTab = this.tabPageSeries;
                this.seriesEditor.ShowSeriesSource();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string url = (this.HelpFileName.Length == 0) ? "TeeChartNet1.chm" : this.HelpFileName;
            Help.ShowHelp(this, url);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int selectedIndex = this.chartListBox1.SelectedIndex;
            if (selectedIndex > 0)
            {
                this.Chart.Series.Exchange(selectedIndex, selectedIndex - 1);
                this.AddSeries();
                this.chartListBox1.ClearSelected();
                this.chartListBox1.SelectedIndex = selectedIndex - 1;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int selectedIndex = this.chartListBox1.SelectedIndex;
            if (selectedIndex < (this.Chart.Series.Count - 1))
            {
                this.Chart.Series.Exchange(selectedIndex, selectedIndex + 1);
                this.AddSeries();
                this.chartListBox1.ClearSelected();
                this.chartListBox1.SelectedIndex = selectedIndex + 1;
            }
        }

        private void buttonChangeSeries_Click(object sender, EventArgs e)
        {
            this.chartListBox1.ChangeTypeSeries();
            this.CBSeries_SelectedIndexChanged(sender, e);
        }

        private void buttonCloneSeries_Click(object sender, EventArgs e)
        {
            Steema.TeeChart.Styles.Series s = this.Series.Clone();
            this.AddToContainer(s);
            this.AddSeries();
            this.chartListBox1.SelectedSeries = s;
        }

        private void buttonDeleteSeries_Click(object sender, EventArgs e)
        {
            this.chartListBox1.DeleteSeries();
            this.EnableButtons();
        }

        private void buttonTitleSeries_Click(object sender, EventArgs e)
        {
            Steema.TeeChart.Styles.Series series = this.Series;
            string str = series.ToString();
            if (TextInput.Query(Texts.ChangeSeriesTitle, Texts.NewSeriesTitle, ref str))
            {
                series.Title = str;
                this.chartListBox1.Items[this.chartListBox1.SelectedIndex] = series.ToString();
                this.chartListBox1.SelectedSeries = series;
            }
        }

        private void CBSeries_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.chartListBox1.SelectedIndex != this.CBSeries.SelectedIndex)
            {
                this.chartListBox1.ClearSelected();
                this.chartListBox1.SelectedIndex = this.CBSeries.SelectedIndex;
            }
            this.panelSeries.Controls.Clear();
            if (this.CBSeries.SelectedIndex != -1)
            {
                this.seriesEditor = new SeriesEditor(this.Series, this.panelSeries);
                EditorUtils.Translate(this.seriesEditor);
                this.label7.BackColor = this.Series.Color;
                this.label7.Visible = this.Series.UseSeriesColor;
                this.label7.Enabled = this.label7.Visible;
                this.label4.Text = this.Series.ToString();
                this.pictureBox2.Image = this.Series.GetBitmapEditor();
            }
        }

        private void ChartEditor_Load(object sender, EventArgs e)
        {
            base.Icon = EditorUtils.TChartIcon();
            this.linkLabel1.Visible = ((this.Chart != null) && (this.Chart.parent != null)) && (this.Chart.parent.GetContainer() != null);
            this.bHelp.Visible = this.linkLabel1.Visible || (this.HelpFileName.Length != 0);
        }

        private void chartListBox1_DoubleClick(object sender, EventArgs e)
        {
            this.tabControl1.SelectedTab = this.tabPageSeries;
        }

        private void chartListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.EnableArrowButtons();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            this.Chart.Walls.Visible = this.checkBox2.Checked;
        }

        private Wall CurrentWall()
        {
            switch (this.tabControl3.SelectedIndex)
            {
                case 0:
                    return this.Chart.Walls.Left;

                case 1:
                    return this.Chart.Walls.Right;

                case 2:
                    return this.Chart.Walls.Back;
            }
            return this.Chart.Walls.Bottom;
        }

        protected override void Dispose(bool disposing)
        {
            if (this.Chart != null)
            {
                this.Chart.RemoveListener(this);
            }
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void EnableArrowButtons()
        {
            this.bSeriesUp.Enabled = this.chartListBox1.SelectedIndex > 0;
            this.bSeriesDown.Enabled = this.chartListBox1.SelectedIndex < (this.chartListBox1.Items.Count - 1);
        }

        private void EnableButtons()
        {
            bool flag = this.chartListBox1.SelectedIndex != -1;
            this.buttonDeleteSeries.Enabled = flag;
            this.buttonTitleSeries.Enabled = flag;
            this.buttonCloneSeries.Enabled = flag;
            this.buttonChangeSeries.Enabled = flag;
            this.EnableArrowButtons();
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.tabControl2 = new TabControl();
            this.tabSeries = new TabPage();
            this.panel12 = new System.Windows.Forms.Panel();
            this.chartListBox1 = new ChartListBox(this.components);
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.bSeriesDown = new Button();
            this.bSeriesUp = new Button();
            this.buttonChangeSeries = new Button();
            this.buttonCloneSeries = new Button();
            this.buttonTitleSeries = new Button();
            this.buttonDeleteSeries = new Button();
            this.buttonAddSeries = new Button();
            this.tabPanel = new TabPage();
            this.tabAxes = new TabPage();
            this.tabGeneral = new TabPage();
            this.tabTitles = new TabPage();
            this.tabWalls = new TabPage();
            this.tabControl3 = new TabControl();
            this.tabPage3 = new TabPage();
            this.tabPage9 = new TabPage();
            this.tabPage10 = new TabPage();
            this.tabPage11 = new TabPage();
            this.checkBox2 = new CheckBox();
            this.tabPaging = new TabPage();
            this.tabLegend = new TabPage();
            this.tabAspect = new TabPage();
            this.tabControl1 = new TabControl();
            this.tabChart = new TabPage();
            this.tabPageSeries = new TabPage();
            this.panelSeries = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.label4 = new Label();
            this.panel10 = new System.Windows.Forms.Panel();
            this.label7 = new Label();
            this.panel9 = new System.Windows.Forms.Panel();
            this.pictureBox2 = new PictureBox();
            this.CBSeries = new ComboBox();
            this.tabTools = new TabPage();
            this.tabExport = new TabPage();
            this.tabPrint = new TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.linkLabel1 = new LinkLabel();
            this.bHelp = new Button();
            this.button1 = new Button();
            this.tabControl2.SuspendLayout();
            this.tabSeries.SuspendLayout();
            this.panel12.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabWalls.SuspendLayout();
            this.tabControl3.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabChart.SuspendLayout();
            this.tabPageSeries.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel10.SuspendLayout();
            this.panel2.SuspendLayout();
            base.SuspendLayout();
            this.tabControl2.Controls.AddRange(new Control[] { this.tabSeries, this.tabPanel, this.tabAxes, this.tabGeneral, this.tabTitles, this.tabWalls, this.tabPaging, this.tabLegend, this.tabAspect });
            this.tabControl2.Dock = DockStyle.Fill;
            this.tabControl2.HotTrack = true;
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.ShowToolTips = true;
            this.tabControl2.Size = new Size(0x18e, 250);
            this.tabControl2.TabIndex = 0;
            this.tabControl2.SelectedIndexChanged += new EventHandler(this.tabControl2_SelectedIndexChanged);
            this.tabSeries.Controls.AddRange(new Control[] { this.panel12, this.panel3, this.panel1 });
            this.tabSeries.Location = new Point(4, 0x16);
            this.tabSeries.Name = "tabSeries";
            this.tabSeries.Size = new Size(390, 0xe0);
            this.tabSeries.TabIndex = 0;
            this.tabSeries.Text = "Series";
            this.panel12.Controls.AddRange(new Control[] { this.chartListBox1 });
            this.panel12.Dock = DockStyle.Fill;
            this.panel12.Location = new Point(0, 0x18);
            this.panel12.Name = "panel12";
            this.panel12.Size = new Size(0x11d, 200);
            this.panel12.TabIndex = 3;
            this.chartListBox1.Dock = DockStyle.Fill;
            this.chartListBox1.IntegralHeight = false;
            this.chartListBox1.Name = "chartListBox1";
            this.chartListBox1.Size = new Size(0x11d, 200);
            this.chartListBox1.TabIndex = 0;
            this.chartListBox1.DoubleClick += new EventHandler(this.chartListBox1_DoubleClick);
            this.chartListBox1.SelectedIndexChanged += new EventHandler(this.chartListBox1_SelectedIndexChanged);
            this.panel3.Dock = DockStyle.Top;
            this.panel3.Name = "panel3";
            this.panel3.Size = new Size(0x11d, 0x18);
            this.panel3.TabIndex = 0;
            this.panel1.Controls.AddRange(new Control[] { this.bSeriesDown, this.bSeriesUp, this.buttonChangeSeries, this.buttonCloneSeries, this.buttonTitleSeries, this.buttonDeleteSeries, this.buttonAddSeries });
            this.panel1.Dock = DockStyle.Right;
            this.panel1.Location = new Point(0x11d, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x69, 0xe0);
            this.panel1.TabIndex = 1;
            this.bSeriesDown.BackColor = Color.Silver;
            this.bSeriesDown.Enabled = false;
            this.bSeriesDown.FlatStyle = FlatStyle.Flat;
            this.bSeriesDown.Location = new Point(0x3a, 0x10);
            this.bSeriesDown.Name = "bSeriesDown";
            this.bSeriesDown.Size = new Size(0x18, 0x17);
            this.bSeriesDown.TabIndex = 1;
            this.bSeriesDown.Click += new EventHandler(this.button6_Click);
            this.bSeriesUp.BackColor = Color.Silver;
            this.bSeriesUp.Enabled = false;
            this.bSeriesUp.FlatStyle = FlatStyle.Flat;
            this.bSeriesUp.Location = new Point(0x18, 0x10);
            this.bSeriesUp.Name = "bSeriesUp";
            this.bSeriesUp.Size = new Size(0x19, 0x17);
            this.bSeriesUp.TabIndex = 0;
            this.bSeriesUp.Click += new EventHandler(this.button5_Click);
            this.buttonChangeSeries.Enabled = false;
            this.buttonChangeSeries.FlatStyle = FlatStyle.Flat;
            this.buttonChangeSeries.Location = new Point(0x12, 0xbf);
            this.buttonChangeSeries.Name = "buttonChangeSeries";
            this.buttonChangeSeries.TabIndex = 6;
            this.buttonChangeSeries.Text = "&Change...";
            this.buttonChangeSeries.Click += new EventHandler(this.buttonChangeSeries_Click);
            this.buttonCloneSeries.Enabled = false;
            this.buttonCloneSeries.FlatStyle = FlatStyle.Flat;
            this.buttonCloneSeries.Location = new Point(0x12, 0x9b);
            this.buttonCloneSeries.Name = "buttonCloneSeries";
            this.buttonCloneSeries.TabIndex = 5;
            this.buttonCloneSeries.Text = "Cl&one";
            this.buttonCloneSeries.Click += new EventHandler(this.buttonCloneSeries_Click);
            this.buttonTitleSeries.Enabled = false;
            this.buttonTitleSeries.FlatStyle = FlatStyle.Flat;
            this.buttonTitleSeries.Location = new Point(0x12, 0x77);
            this.buttonTitleSeries.Name = "buttonTitleSeries";
            this.buttonTitleSeries.TabIndex = 4;
            this.buttonTitleSeries.Text = "&Title...";
            this.buttonTitleSeries.Click += new EventHandler(this.buttonTitleSeries_Click);
            this.buttonDeleteSeries.Enabled = false;
            this.buttonDeleteSeries.FlatStyle = FlatStyle.Flat;
            this.buttonDeleteSeries.Location = new Point(0x12, 0x53);
            this.buttonDeleteSeries.Name = "buttonDeleteSeries";
            this.buttonDeleteSeries.TabIndex = 3;
            this.buttonDeleteSeries.Text = "&Delete...";
            this.buttonDeleteSeries.Click += new EventHandler(this.buttonDeleteSeries_Click);
            this.buttonAddSeries.FlatStyle = FlatStyle.Flat;
            this.buttonAddSeries.Location = new Point(0x12, 0x2f);
            this.buttonAddSeries.Name = "buttonAddSeries";
            this.buttonAddSeries.TabIndex = 2;
            this.buttonAddSeries.Text = "&Add...";
            this.buttonAddSeries.Click += new EventHandler(this.button2_Click);
            this.tabPanel.Location = new Point(4, 0x16);
            this.tabPanel.Name = "tabPanel";
            this.tabPanel.Size = new Size(390, 0xe0);
            this.tabPanel.TabIndex = 5;
            this.tabPanel.Text = "Panel";
            this.tabAxes.Location = new Point(4, 0x16);
            this.tabAxes.Name = "tabAxes";
            this.tabAxes.Size = new Size(390, 0xe0);
            this.tabAxes.TabIndex = 2;
            this.tabAxes.Text = "Axes";
            this.tabGeneral.Location = new Point(4, 0x16);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Size = new Size(390, 0xe0);
            this.tabGeneral.TabIndex = 1;
            this.tabGeneral.Text = "General";
            this.tabTitles.Location = new Point(4, 0x16);
            this.tabTitles.Name = "tabTitles";
            this.tabTitles.Size = new Size(390, 0xe0);
            this.tabTitles.TabIndex = 4;
            this.tabTitles.Text = "Titles";
            this.tabWalls.Controls.AddRange(new Control[] { this.tabControl3, this.checkBox2 });
            this.tabWalls.Location = new Point(4, 0x16);
            this.tabWalls.Name = "tabWalls";
            this.tabWalls.Size = new Size(390, 0xe0);
            this.tabWalls.TabIndex = 7;
            this.tabWalls.Text = "Walls";
            this.tabControl3.Controls.AddRange(new Control[] { this.tabPage3, this.tabPage9, this.tabPage10, this.tabPage11 });
            this.tabControl3.HotTrack = true;
            this.tabControl3.Location = new Point(0x3d, 0x20);
            this.tabControl3.Name = "tabControl3";
            this.tabControl3.SelectedIndex = 0;
            this.tabControl3.Size = new Size(0x116, 0xb7);
            this.tabControl3.TabIndex = 1;
            this.tabControl3.SelectedIndexChanged += new EventHandler(this.tabControl3_SelectedIndexChanged);
            this.tabPage3.Location = new Point(4, 0x16);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new Size(270, 0x9d);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "Left";
            this.tabPage9.Location = new Point(4, 0x16);
            this.tabPage9.Name = "tabPage9";
            this.tabPage9.Size = new Size(270, 0x9d);
            this.tabPage9.TabIndex = 1;
            this.tabPage9.Text = "Right";
            this.tabPage10.Location = new Point(4, 0x16);
            this.tabPage10.Name = "tabPage10";
            this.tabPage10.Size = new Size(270, 0x9d);
            this.tabPage10.TabIndex = 2;
            this.tabPage10.Text = "Back";
            this.tabPage11.Location = new Point(4, 0x16);
            this.tabPage11.Name = "tabPage11";
            this.tabPage11.Size = new Size(270, 0x9d);
            this.tabPage11.TabIndex = 3;
            this.tabPage11.Text = "Bottom";
            this.checkBox2.FlatStyle = FlatStyle.Flat;
            this.checkBox2.Location = new Point(7, 2);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new Size(0xe1, 0x18);
            this.checkBox2.TabIndex = 0;
            this.checkBox2.Text = "Visible Walls";
            this.checkBox2.CheckedChanged += new EventHandler(this.checkBox2_CheckedChanged);
            this.tabPaging.Location = new Point(4, 0x16);
            this.tabPaging.Name = "tabPaging";
            this.tabPaging.Size = new Size(390, 0xe0);
            this.tabPaging.TabIndex = 6;
            this.tabPaging.Text = "Paging";
            this.tabLegend.Location = new Point(4, 0x16);
            this.tabLegend.Name = "tabLegend";
            this.tabLegend.Size = new Size(390, 0xe0);
            this.tabLegend.TabIndex = 3;
            this.tabLegend.Text = "Legend";
            this.tabAspect.Location = new Point(4, 0x16);
            this.tabAspect.Name = "tabAspect";
            this.tabAspect.Size = new Size(390, 0xe0);
            this.tabAspect.TabIndex = 8;
            this.tabAspect.Text = "3D";
            this.tabControl1.Appearance = TabAppearance.FlatButtons;
            this.tabControl1.Controls.AddRange(new Control[] { this.tabChart, this.tabPageSeries, this.tabTools, this.tabExport, this.tabPrint });
            this.tabControl1.Dock = DockStyle.Fill;
            this.tabControl1.HotTrack = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.ShowToolTips = true;
            this.tabControl1.Size = new Size(0x196, 0x117);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new EventHandler(this.tabControl1_SelectedIndexChanged);
            this.tabChart.Controls.AddRange(new Control[] { this.tabControl2 });
            this.tabChart.Location = new Point(4, 0x19);
            this.tabChart.Name = "tabChart";
            this.tabChart.Size = new Size(0x18e, 250);
            this.tabChart.TabIndex = 0;
            this.tabChart.Text = "Chart";
            this.tabPageSeries.Controls.AddRange(new Control[] { this.panelSeries, this.panel8 });
            this.tabPageSeries.Location = new Point(4, 0x19);
            this.tabPageSeries.Name = "tabPageSeries";
            this.tabPageSeries.Size = new Size(0x18e, 250);
            this.tabPageSeries.TabIndex = 1;
            this.tabPageSeries.Text = "Series";
            this.panelSeries.Dock = DockStyle.Fill;
            this.panelSeries.Location = new Point(0, 0x20);
            this.panelSeries.Name = "panelSeries";
            this.panelSeries.Size = new Size(0x18e, 0xda);
            this.panelSeries.TabIndex = 1;
            this.panel8.Controls.AddRange(new Control[] { this.label4, this.panel10, this.panel9, this.pictureBox2, this.CBSeries });
            this.panel8.Dock = DockStyle.Top;
            this.panel8.Name = "panel8";
            this.panel8.Size = new Size(0x18e, 0x20);
            this.panel8.TabIndex = 0;
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0xce, 11);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0, 13);
            this.label4.TabIndex = 4;
            this.label4.UseMnemonic = false;
            this.panel10.Controls.AddRange(new Control[] { this.label7 });
            this.panel10.Dock = DockStyle.Right;
            this.panel10.Location = new Point(360, 0);
            this.panel10.Name = "panel10";
            this.panel10.Size = new Size(0x26, 0x20);
            this.panel10.TabIndex = 3;
            this.label7.BackColor = Color.Red;
            this.label7.BorderStyle = BorderStyle.Fixed3D;
            this.label7.Cursor = Cursors.Hand;
            this.label7.Enabled = false;
            this.label7.FlatStyle = FlatStyle.Flat;
            this.label7.Location = new Point(6, 5);
            this.label7.Name = "label7";
            this.label7.Size = new Size(0x18, 0x18);
            this.label7.TabIndex = 0;
            this.label7.UseMnemonic = false;
            this.label7.Click += new EventHandler(this.label7_Click);
            this.panel9.Location = new Point(0x134, 6);
            this.panel9.Name = "panel9";
            this.panel9.Size = new Size(1, 1);
            this.panel9.TabIndex = 2;
            this.pictureBox2.BorderStyle = BorderStyle.Fixed3D;
            this.pictureBox2.Location = new Point(0xa9, 5);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new Size(0x18, 0x18);
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            this.CBSeries.DropDownStyle = ComboBoxStyle.DropDownList;
            this.CBSeries.DropDownWidth = 160;
            this.CBSeries.Location = new Point(4, 6);
            this.CBSeries.Name = "CBSeries";
            this.CBSeries.Size = new Size(160, 0x15);
            this.CBSeries.TabIndex = 0;
            this.CBSeries.SelectedIndexChanged += new EventHandler(this.CBSeries_SelectedIndexChanged);
            this.tabTools.Location = new Point(4, 0x19);
            this.tabTools.Name = "tabTools";
            this.tabTools.Size = new Size(0x18e, 250);
            this.tabTools.TabIndex = 5;
            this.tabTools.Text = "Tools";
            this.tabExport.Location = new Point(4, 0x19);
            this.tabExport.Name = "tabExport";
            this.tabExport.Size = new Size(0x18e, 250);
            this.tabExport.TabIndex = 3;
            this.tabExport.Text = "Export";
            this.tabPrint.Location = new Point(4, 0x19);
            this.tabPrint.Name = "tabPrint";
            this.tabPrint.Size = new Size(0x18e, 250);
            this.tabPrint.TabIndex = 4;
            this.tabPrint.Text = "Print";
            this.panel2.BorderStyle = BorderStyle.Fixed3D;
            this.panel2.Controls.AddRange(new Control[] { this.linkLabel1, this.bHelp, this.button1 });
            this.panel2.Dock = DockStyle.Bottom;
            this.panel2.Location = new Point(0, 0x117);
            this.panel2.Name = "panel2";
            this.panel2.Size = new Size(0x196, 0x26);
            this.panel2.TabIndex = 1;
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.FlatStyle = FlatStyle.Flat;
            this.linkLabel1.Font = new Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.linkLabel1.Location = new Point(0x8e, 10);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new Size(0x70, 0x10);
            this.linkLabel1.TabIndex = 1;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "www.Steema.com";
            this.linkLabel1.UseMnemonic = false;
            this.linkLabel1.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            this.bHelp.FlatStyle = FlatStyle.Flat;
            this.bHelp.Location = new Point(0x18, 7);
            this.bHelp.Name = "bHelp";
            this.bHelp.TabIndex = 0;
            this.bHelp.Text = "&Help...";
            this.bHelp.Click += new EventHandler(this.button3_Click);
            this.button1.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.button1.DialogResult = DialogResult.Cancel;
            this.button1.FlatStyle = FlatStyle.Flat;
            this.button1.Location = new Point(310, 8);
            this.button1.Name = "button1";
            this.button1.TabIndex = 2;
            this.button1.Text = "Close";
            this.button1.Click += new EventHandler(this.button1_Click);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this.button1;
            base.ClientSize = new Size(0x196, 0x13d);
            base.Controls.AddRange(new Control[] { this.tabControl1, this.panel2 });
            base.HelpButton = true;
            base.Name = "ChartEditor";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "TeeChart Editor";
            base.Load += new EventHandler(this.ChartEditor_Load);
            this.tabControl2.ResumeLayout(false);
            this.tabSeries.ResumeLayout(false);
            this.panel12.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tabWalls.ResumeLayout(false);
            this.tabControl3.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabChart.ResumeLayout(false);
            this.tabPageSeries.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel10.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void label7_Click(object sender, EventArgs e)
        {
            this.label7.BackColor = ColorEditor.Choose(this.label7.BackColor, this);
            this.Series.Color = this.label7.BackColor;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.linkLabel1.Links[this.linkLabel1.Links.IndexOf(e.Link)].Visited = true;
            Process.Start(this.linkLabel1.Text);
        }

        internal void SetDefaultTab(ChartEditorTabs defaultTab)
        {
            switch (defaultTab)
            {
                case ChartEditorTabs.Main:
                case ChartEditorTabs.Data:
                    break;

                case ChartEditorTabs.General:
                    this.tabControl2.SelectedTab = this.tabGeneral;
                    return;

                case ChartEditorTabs.Axes:
                    this.tabControl2.SelectedTab = this.tabAxes;
                    return;

                case ChartEditorTabs.Legend:
                    this.tabControl2.SelectedTab = this.tabLegend;
                    return;

                case ChartEditorTabs.Walls:
                    this.tabControl2.SelectedTab = this.tabWalls;
                    return;

                case ChartEditorTabs.Aspect:
                    this.tabControl2.SelectedTab = this.tabAspect;
                    return;

                case ChartEditorTabs.Panel:
                    this.tabControl2.SelectedTab = this.tabPanel;
                    return;

                case ChartEditorTabs.Page:
                    this.tabControl2.SelectedTab = this.tabPaging;
                    return;

                case ChartEditorTabs.Series:
                    this.tabControl1.SelectedTab = this.tabPageSeries;
                    return;

                case ChartEditorTabs.Tools:
                    this.tabControl1.SelectedTab = this.tabTools;
                    return;

                case ChartEditorTabs.Export:
                    this.tabControl1.SelectedTab = this.tabExport;
                    return;

                case ChartEditorTabs.Print:
                    this.tabControl1.SelectedTab = this.tabPrint;
                    return;

                case ChartEditorTabs.SeriesDataSource:
                    this.tabControl1.SelectedTab = this.tabPageSeries;
                    this.seriesEditor.tabControl1.SelectedTab = this.seriesEditor.tabSource;
                    break;

                default:
                    return;
            }
        }

        public static bool ShowModal(Steema.TeeChart.Chart c)
        {
            return ShowModal(c, ChartEditorTabs.Main);
        }

        public static bool ShowModal(Steema.TeeChart.Styles.Series s)
        {
            return ShowModal(s, ChartEditorTabs.Main);
        }

        public static bool ShowModal(Steema.TeeChart.Chart c, ChartEditorTabs defaultTab)
        {
            using (ChartEditor editor = new ChartEditor(c))
            {
                editor.SetDefaultTab(defaultTab);
                return (editor.ShowDialog() == DialogResult.OK);
            }
        }

        public static bool ShowModal(Steema.TeeChart.Styles.Series s, ChartEditorTabs defaultTab)
        {
            using (ChartEditor editor = new ChartEditor(s.chart))
            {
                editor.ShowSeriesEditor(s);
                editor.SetDefaultTab(defaultTab);
                return (editor.ShowDialog() == DialogResult.OK);
            }
        }

        internal void ShowSeriesEditor(Steema.TeeChart.Styles.Series s)
        {
            this.chartListBox1.SelectedSeries = s;
            this.tabControl1.SelectedTab = this.tabPageSeries;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((this.tabControl1.SelectedTab == this.tabPrint) & (this.printPreview == null))
            {
                this.printPreview = new PrintPreview(this.Chart, this.tabPrint);
                EditorUtils.Translate(this.printPreview);
            }
            else if (this.tabControl1.SelectedTab == this.tabPrint)
            {
                this.printPreview.RefreshView();
            }
            else if ((this.tabControl1.SelectedTab == this.tabExport) & (this.exportEditor == null))
            {
                this.exportEditor = new ExportEditor(this.Chart, this.tabExport);
                EditorUtils.Translate(this.exportEditor);
            }
            else if (this.tabControl1.SelectedTab == this.tabTools)
            {
                if (this.toolsEditor == null)
                {
                    this.toolsEditor = new ToolsEditor(this.Chart, this.tabTools);
                    EditorUtils.Translate(this.toolsEditor);
                }
                else
                {
                    this.toolsEditor.Reload();
                }
            }
            else if ((this.tabControl1.SelectedTab == this.tabPageSeries) && (this.CBSeries.SelectedIndex != this.chartListBox1.SelectedIndex))
            {
                this.CBSeries.SelectedIndex = this.chartListBox1.SelectedIndex;
            }
        }

        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.tabControl2.SelectedTab == this.tabWalls)
            {
                this.checkBox2.Checked = this.Chart.Walls.Visible;
                this.tabControl3_SelectedIndexChanged(sender, e);
            }
            else if (this.tabControl2.SelectedTab == this.tabPanel)
            {
                this.panelEditor = new Steema.TeeChart.Editors.PanelEditor(this.Chart.Panel, this.tabPanel);
                EditorUtils.Translate(this.panelEditor);
            }
            else if ((this.tabControl2.SelectedTab == this.tabAxes) & (this.axesEditor == null))
            {
                this.axesEditor = new AxesEditor(this.Chart, this.tabAxes);
                EditorUtils.Translate(this.axesEditor);
            }
            else if ((this.tabControl2.SelectedTab == this.tabAspect) & (this.aspectEditor == null))
            {
                this.aspectEditor = new AspectEditor(this.Chart, this.tabAspect);
                EditorUtils.Translate(this.aspectEditor);
            }
            else if ((this.tabControl2.SelectedTab == this.tabLegend) & (this.legendEditor == null))
            {
                this.legendEditor = new Steema.TeeChart.Editors.LegendEditor(this.Chart, this.tabLegend);
                EditorUtils.Translate(this.legendEditor);
            }
            else if ((this.tabControl2.SelectedTab == this.tabGeneral) & (this.generalEditor == null))
            {
                this.generalEditor = new GeneralEditor(this.Chart, this.tabGeneral);
                EditorUtils.Translate(this.generalEditor);
            }
            else if ((this.tabControl2.SelectedTab == this.tabTitles) & (this.titlesEditor == null))
            {
                this.titlesEditor = new TitleEditor(this.Chart, this.tabTitles);
                EditorUtils.Translate(this.titlesEditor);
            }
            else if (this.tabControl2.SelectedTab == this.tabPaging)
            {
                this.pageEditor = new Steema.TeeChart.Editors.PageEditor(this.Chart.Page, this.tabPaging);
                EditorUtils.Translate(this.pageEditor);
            }
        }

        private void tabControl3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.wallEditor[this.tabControl3.SelectedIndex] == null)
            {
                this.wallEditor[this.tabControl3.SelectedIndex] = new Steema.TeeChart.Editors.WallEditor(this.CurrentWall(), this.tabControl3.SelectedTab);
                EditorUtils.Translate(this.wallEditor[this.tabControl3.SelectedIndex]);
            }
        }

        public void TeeEvent(Steema.TeeChart.TeeEvent e)
        {
            if ((e is SeriesEvent) && (((SeriesEvent) e).Event == SeriesEventStyle.ChangeColor))
            {
                this.label7.BackColor = ((SeriesEvent) e).Series.Color;
            }
        }

        private Steema.TeeChart.Styles.Series Series
        {
            get
            {
                if ((this.chartListBox1.Items.Count != 0) && (this.chartListBox1.SelectedItem != null))
                {
                    return this.Chart[this.chartListBox1.SelectedIndex];
                }
                return null;
            }
        }
    }
}

