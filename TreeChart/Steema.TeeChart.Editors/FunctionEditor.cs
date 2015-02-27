namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart;
    using Steema.TeeChart.Functions;
    using Steema.TeeChart.Styles;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class FunctionEditor : Form
    {
        private Button bApply;
        private ComboBox cbFunctions;
        private CCIFunctionEditor cciFunction;
        private CLVFunctionEditor clvFunction;
        private Container components;
        private CompressOHLCFunctionEditor compressOHLC;
        private CustomFunctionEditor custom;
        private Label label1;
        private MovingAverageFunctionEditor movavgFunction;
        private OBVFunctionEditor obvFunction;
        private PeriodEditor options;
        private System.Windows.Forms.Panel panel1;
        private PVOFunctionEditor pvoFunction;
        private SelectListForm select;
        private Steema.TeeChart.Styles.Series series;
        private TabPage tabCCI;
        private TabPage tabCLV;
        private TabPage tabCompress;
        private TabControl tabControl1;
        private TabPage tabCustom;
        private TabPage tabMovAvg;
        private TabPage tabOBV;
        private TabPage tabOptions;
        private TabPage tabPage1;
        private TabPage tabPVO;

        public FunctionEditor()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public FunctionEditor(Steema.TeeChart.Styles.Series s)
            : this()
        {
            this.series = s;
        }

        private void AddPages(bool withOptions)
        {
            this.tabControl1.TabPages.Clear();
            if ((this.cbFunctions.SelectedIndex > 0) && (Utils.FunctionTypesOf[this.cbFunctions.SelectedIndex - 1] == typeof(Steema.TeeChart.Functions.Custom)))
            {
                if (this.series.Function == null)
                {
                    this.series.Function = new Steema.TeeChart.Functions.Custom();
                }
                this.tabControl1.TabPages.Add(this.tabCustom);
                this.tabControl1.SelectedTab = this.tabCustom;
                this.tabControl1_SelectedIndexChanged(this, new EventArgs());
            }
            else if ((this.cbFunctions.SelectedIndex > 0) && (Utils.FunctionTypesOf[this.cbFunctions.SelectedIndex - 1] == typeof(CompressOHLC)))
            {
                if (this.series.Function == null)
                {
                    this.series.Function = new CompressOHLC();
                }
                this.tabControl1.TabPages.Add(this.tabPage1);
                this.tabControl1.TabPages.Add(this.tabCompress);
                this.tabControl1.SelectedTab = this.tabPage1;
                this.tabControl1_SelectedIndexChanged(this, new EventArgs());
            }
            else if ((this.cbFunctions.SelectedIndex > 0) && (Utils.FunctionTypesOf[this.cbFunctions.SelectedIndex - 1] == typeof(CLVFunction)))
            {
                if (this.series.Function == null)
                {
                    this.series.Function = new CLVFunction();
                }
                this.tabControl1.TabPages.Add(this.tabPage1);
                this.tabControl1.TabPages.Add(this.tabCLV);
                this.tabControl1.SelectedTab = this.tabPage1;
                this.tabControl1_SelectedIndexChanged(this, new EventArgs());
            }
            else if ((this.cbFunctions.SelectedIndex > 0) && (Utils.FunctionTypesOf[this.cbFunctions.SelectedIndex - 1] == typeof(OBVFunction)))
            {
                if (this.series.Function == null)
                {
                    this.series.Function = new OBVFunction();
                }
                this.tabControl1.TabPages.Add(this.tabPage1);
                this.tabControl1.TabPages.Add(this.tabOBV);
                this.tabControl1.SelectedTab = this.tabPage1;
                this.tabControl1_SelectedIndexChanged(this, new EventArgs());
            }
            else if ((this.cbFunctions.SelectedIndex > 0) && (Utils.FunctionTypesOf[this.cbFunctions.SelectedIndex - 1] == typeof(CCIFunction)))
            {
                if (this.series.Function == null)
                {
                    this.series.Function = new CCIFunction();
                }
                this.tabControl1.TabPages.Add(this.tabPage1);
                this.tabControl1.TabPages.Add(this.tabCCI);
                this.tabControl1.SelectedTab = this.tabPage1;
                this.tabControl1_SelectedIndexChanged(this, new EventArgs());
            }
            else if ((this.cbFunctions.SelectedIndex > 0) && (Utils.FunctionTypesOf[this.cbFunctions.SelectedIndex - 1] == typeof(MovingAverage)))
            {
                if (this.series.Function == null)
                {
                    this.series.Function = new MovingAverage();
                }
                this.tabControl1.TabPages.Add(this.tabPage1);
                this.tabControl1.TabPages.Add(this.tabMovAvg);
                this.tabControl1.SelectedTab = this.tabPage1;
                this.tabControl1_SelectedIndexChanged(this, new EventArgs());
            }
            else if ((this.cbFunctions.SelectedIndex > 0) && (Utils.FunctionTypesOf[this.cbFunctions.SelectedIndex - 1] == typeof(PVOFunction)))
            {
                if (this.series.Function == null)
                {
                    this.series.Function = new PVOFunction();
                }
                this.tabControl1.TabPages.Add(this.tabPage1);
                this.tabControl1.TabPages.Add(this.tabPVO);
                this.tabControl1.SelectedTab = this.tabPage1;
                this.tabControl1_SelectedIndexChanged(this, new EventArgs());
            }
            else
            {
                this.tabControl1.TabPages.Add(this.tabPage1);
                if (withOptions)
                {
                    this.tabControl1.TabPages.Add(this.tabOptions);
                }
            }
        }

        private void bApply_Click(object sender, EventArgs e)
        {
            if (this.options != null)
            {
                this.options.Apply();
            }
            this.DoApply();
            this.bApply.Enabled = false;
        }

        private void cbFunctions_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.AddPages(this.FunctionType != null);
            this.bApply.Enabled = true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void DoApply()
        {
            if (this.select.ToList.Items.Count == 0)
            {
                this.series.DataSource = null;
            }
            else
            {
                foreach (string str in this.select.ToList.Items)
                {
                    Steema.TeeChart.Styles.Series dest = this.series.chart.series.WithTitle(str);
                    this.series.CheckOtherSeries(dest);
                }
                if (this.select.ToList.Items.Count == 1)
                {
                    this.series.DataSource = this.series.chart.series.WithTitle(this.select.ToList.Items[0].ToString());
                }
                else
                {
                    object[] objArray = new object[this.select.ToList.Items.Count];
                    int index = 0;
                    foreach (string str2 in this.select.ToList.Items)
                    {
                        Steema.TeeChart.Styles.Series series2 = this.series.chart.series.WithTitle(str2);
                        objArray.SetValue(series2, index);
                        index++;
                    }
                    this.series.DataSource = objArray;
                }
            }
            System.Type functionType = this.FunctionType;
            if (functionType != null)
            {
                if ((this.series.Function == null) || (this.series.Function.GetType() != functionType))
                {
                    this.series.Function = Function.NewInstance(functionType);
                }
                else
                {
                    this.series.CheckDataSource();
                }
            }
            else
            {
                this.series.Function = null;
            }
        }

        private void FillSources(bool AddCurrent)
        {
            this.select.FromList.BeginUpdate();
            this.select.FromList.Items.Clear();
            foreach (Steema.TeeChart.Styles.Series series in this.series.chart.Series)
            {
                if ((AddCurrent || !this.series.HasDataSource(series)) && this.series.Chart.IsValidDataSource(this.series, series))
                {
                    this.select.FromList.Items.Add(series.ToString());
                }
            }
            this.select.FromList.EndUpdate();
        }

        private void FunctionEditor_Load(object sender, EventArgs e)
        {
            if (this.series != null)
            {
                this.cbFunctions.Items.Add(Texts.FunctionNone);
                foreach (System.Type type in Utils.FunctionTypesOf)
                {
                    string str = Function.NewInstance(type).Description();
                    this.cbFunctions.Items.Add(str.Replace('\n', ' '));
                }
                this.tabControl1.SelectedTab = this.tabPage1;
                this.select = new SelectListForm();
                this.select.controlToEnable = this.bApply;
                EditorUtils.InsertForm(this.select, this.tabPage1);
                this.FillSources(false);
                this.select.ToList.BeginUpdate();
                this.select.ToList.Items.Clear();
                ArrayList list = this.series.DataSourceArray();
                if (list != null)
                {
                    foreach (object obj2 in list)
                    {
                        if (obj2 is Steema.TeeChart.Styles.Series)
                        {
                            Steema.TeeChart.Styles.Series series = (Steema.TeeChart.Styles.Series)obj2;
                            this.select.ToList.Items.Add(series.ToString());
                            int index = this.select.FromList.Items.IndexOf(series.ToString());
                            if (index != -1)
                            {
                                this.select.FromList.Items.RemoveAt(index);
                            }
                        }
                    }
                }
                this.select.ToList.EndUpdate();
                if (this.series.Function == null)
                {
                    this.cbFunctions.SelectedIndex = 0;
                }
                else
                {
                    int num2 = this.FunctionIndexOf(this.series.Function.GetType());
                    this.cbFunctions.SelectedIndex = num2 + 1;
                }
                this.select.EnableButtons();
            }
            this.bApply.Enabled = false;
        }

        private int FunctionIndexOf(System.Type f)
        {
            for (int i = 0; i < 0x1b; i++)
            {
                if (Utils.FunctionTypesOf[i] == f)
                {
                    return i;
                }
            }
            return -1;
        }

        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.bApply = new Button();
            this.cbFunctions = new ComboBox();
            this.label1 = new Label();
            this.tabOptions = new TabPage();
            this.tabPage1 = new TabPage();
            this.tabControl1 = new TabControl();
            this.tabCompress = new TabPage();
            this.tabMovAvg = new TabPage();
            this.tabCLV = new TabPage();
            this.tabPVO = new TabPage();
            this.tabCCI = new TabPage();
            this.tabCustom = new TabPage();
            this.tabOBV = new TabPage();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            base.SuspendLayout();
            this.panel1.Controls.Add(this.bApply);
            this.panel1.Controls.Add(this.cbFunctions);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = DockStyle.Top;
            this.panel1.Location = new Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x158, 0x19);
            this.panel1.TabIndex = 0;
            this.bApply.FlatStyle = FlatStyle.Flat;
            this.bApply.Location = new Point(0xf8, 1);
            this.bApply.Name = "bApply";
            this.bApply.TabIndex = 2;
            this.bApply.Text = "&Apply";
            this.bApply.Click += new EventHandler(this.bApply_Click);
            this.cbFunctions.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbFunctions.Location = new Point(0x5c, 1);
            this.cbFunctions.Name = "cbFunctions";
            this.cbFunctions.Size = new Size(140, 0x15);
            this.cbFunctions.TabIndex = 1;
            this.cbFunctions.SelectedIndexChanged += new EventHandler(this.cbFunctions_SelectedIndexChanged);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x22, 3);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x39, 0x10);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Functions:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            this.tabOptions.Location = new Point(4, 0x16);
            this.tabOptions.Name = "tabOptions";
            this.tabOptions.Size = new Size(0x150, 0x9a);
            this.tabOptions.TabIndex = 1;
            this.tabOptions.Text = "Options";
            this.tabPage1.Location = new Point(4, 0x16);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new Size(0x150, 0x9a);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Source Series";
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabCompress);
            this.tabControl1.Controls.Add(this.tabMovAvg);
            this.tabControl1.Controls.Add(this.tabCLV);
            this.tabControl1.Controls.Add(this.tabPVO);
            this.tabControl1.Controls.Add(this.tabOptions);
            this.tabControl1.Controls.Add(this.tabCCI);
            this.tabControl1.Controls.Add(this.tabCustom);
            this.tabControl1.Controls.Add(this.tabOBV);
            this.tabControl1.Dock = DockStyle.Fill;
            this.tabControl1.HotTrack = true;
            this.tabControl1.ItemSize = new Size(0x4e, 0x12);
            this.tabControl1.Location = new Point(0, 0x19);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new Size(0x158, 180);
            this.tabControl1.TabIndex = 1;
            this.tabControl1.SelectedIndexChanged += new EventHandler(this.tabControl1_SelectedIndexChanged);
            this.tabCompress.Location = new Point(4, 0x16);
            this.tabCompress.Name = "tabCompress";
            this.tabCompress.Size = new Size(0x150, 0x9a);
            this.tabCompress.TabIndex = 3;
            this.tabCompress.Text = "Compress";
            this.tabMovAvg.Location = new Point(4, 0x16);
            this.tabMovAvg.Name = "tabMovAvg";
            this.tabMovAvg.Size = new Size(0x150, 0x9a);
            this.tabMovAvg.TabIndex = 7;
            this.tabMovAvg.Text = "Mov. Avg.";
            this.tabCLV.Location = new Point(4, 0x16);
            this.tabCLV.Name = "tabCLV";
            this.tabCLV.Size = new Size(0x150, 0x9a);
            this.tabCLV.TabIndex = 4;
            this.tabCLV.Text = "CLV";
            this.tabPVO.Location = new Point(4, 0x16);
            this.tabPVO.Name = "tabPVO";
            this.tabPVO.Size = new Size(0x150, 0x9a);
            this.tabPVO.TabIndex = 8;
            this.tabPVO.Text = "PVO";
            this.tabCCI.Location = new Point(4, 0x16);
            this.tabCCI.Name = "tabCCI";
            this.tabCCI.Size = new Size(0x150, 0x9a);
            this.tabCCI.TabIndex = 6;
            this.tabCCI.Text = "CCI";
            this.tabCustom.Location = new Point(4, 0x16);
            this.tabCustom.Name = "tabCustom";
            this.tabCustom.Size = new Size(0x150, 0x9a);
            this.tabCustom.TabIndex = 2;
            this.tabCustom.Text = "Custom";
            this.tabOBV.Location = new Point(4, 0x16);
            this.tabOBV.Name = "tabOBV";
            this.tabOBV.Size = new Size(0x150, 0x9a);
            this.tabOBV.TabIndex = 5;
            this.tabOBV.Text = "OBV";
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x158, 0xcd);
            base.Controls.Add(this.tabControl1);
            base.Controls.Add(this.panel1);
            base.Name = "FunctionEditor";
            base.Load += new EventHandler(this.FunctionEditor_Load);
            this.panel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.tabControl1.SelectedTab == this.tabOptions)
            {
                if (this.bApply.Enabled)
                {
                    this.bApply_Click(this.bApply, EventArgs.Empty);
                }
                this.bApply.Enabled = false;
                if ((this.options == null) && (this.series.Function != null))
                {
                    this.options = new PeriodEditor(this.series.Function);
                    EditorUtils.InsertForm(this.options, this.tabOptions);
                }
            }
            else if ((this.tabControl1.SelectedTab == this.tabCustom) && (this.FunctionType == typeof(Steema.TeeChart.Functions.Custom)))
            {
                if (this.bApply.Enabled)
                {
                    this.bApply_Click(this.bApply, EventArgs.Empty);
                }
                this.bApply.Enabled = false;
                if (this.series.Function != null)
                {
                    this.custom = new CustomFunctionEditor(this.series.Function);
                    this.custom.controlToEnable = this.bApply;
                    EditorUtils.InsertForm(this.custom, this.tabCustom);
                }
            }
            else if ((this.tabControl1.SelectedTab == this.tabCompress) && (this.FunctionType == typeof(CompressOHLC)))
            {
                if (this.bApply.Enabled)
                {
                    this.bApply_Click(this.bApply, EventArgs.Empty);
                }
                this.bApply.Enabled = false;
                if (this.series.Function != null)
                {
                    this.compressOHLC = new CompressOHLCFunctionEditor(this.series.Function);
                    this.compressOHLC.controlToEnable = this.bApply;
                    EditorUtils.InsertForm(this.compressOHLC, this.tabCompress);
                }
            }
            else if ((this.tabControl1.SelectedTab == this.tabCLV) && (this.FunctionType == typeof(CLVFunction)))
            {
                if (this.bApply.Enabled)
                {
                    this.bApply_Click(this.bApply, EventArgs.Empty);
                }
                this.bApply.Enabled = false;
                if (this.series.Function != null)
                {
                    this.clvFunction = new CLVFunctionEditor(this.series, this.select.FromList);
                    this.clvFunction.controlToEnable = this.bApply;
                    EditorUtils.InsertForm(this.clvFunction, this.tabCLV);
                }
            }
            else if ((this.tabControl1.SelectedTab == this.tabOBV) && (this.FunctionType == typeof(OBVFunction)))
            {
                if (this.bApply.Enabled)
                {
                    this.bApply_Click(this.bApply, EventArgs.Empty);
                }
                this.bApply.Enabled = false;
                if (this.series.Function != null)
                {
                    this.obvFunction = new OBVFunctionEditor(this.series, this.select.FromList);
                    this.obvFunction.controlToEnable = this.bApply;
                    EditorUtils.InsertForm(this.obvFunction, this.tabOBV);
                }
            }
            else if ((this.tabControl1.SelectedTab == this.tabCCI) && (this.FunctionType == typeof(CCIFunction)))
            {
                if (this.bApply.Enabled)
                {
                    this.bApply_Click(this.bApply, EventArgs.Empty);
                }
                this.bApply.Enabled = false;
                if (this.series.Function != null)
                {
                    this.cciFunction = new CCIFunctionEditor(this.series.Function);
                    this.cciFunction.controlToEnable = this.bApply;
                    EditorUtils.InsertForm(this.cciFunction, this.tabCCI);
                }
            }
            else if ((this.tabControl1.SelectedTab == this.tabMovAvg) && (this.FunctionType == typeof(MovingAverage)))
            {
                if (this.bApply.Enabled)
                {
                    this.bApply_Click(this.bApply, EventArgs.Empty);
                }
                this.bApply.Enabled = false;
                if (this.series.Function != null)
                {
                    this.movavgFunction = new MovingAverageFunctionEditor(this.series.Function);
                    this.movavgFunction.controlToEnable = this.bApply;
                    EditorUtils.InsertForm(this.movavgFunction, this.tabMovAvg);
                }
            }
            else if ((this.tabControl1.SelectedTab == this.tabPVO) && (this.FunctionType == typeof(PVOFunction)))
            {
                if (this.bApply.Enabled)
                {
                    this.bApply_Click(this.bApply, EventArgs.Empty);
                }
                this.bApply.Enabled = false;
                if (this.series.Function != null)
                {
                    this.pvoFunction = new PVOFunctionEditor(this.series.Function);
                    this.pvoFunction.controlToEnable = this.bApply;
                    EditorUtils.InsertForm(this.pvoFunction, this.tabPVO);
                }
            }
        }

        private System.Type FunctionType
        {
            get
            {
                if (this.cbFunctions.SelectedIndex < 1)
                {
                    return null;
                }
                return Utils.FunctionTypesOf[this.cbFunctions.SelectedIndex - 1];
            }
        }
    }
}

