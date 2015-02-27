namespace Steema.TeeChart.Editors.Export
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Export;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    public class ExportEditor : Form
    {
        private Button bSend;
        private Button button1;
        private Button button2;
        private Button button4;
        private Chart chart;
        private CheckBox checkBox1;
        private CheckBox checkBox2;
        private CheckBox checkBox3;
        private CheckBox checkHeader;
        private CheckBox checkPIndex;
        private CheckBox checkPLabels;
        private ComboBox comboDelimiter;
        private ComboBox comboSeries;
        private Container components;
        private DataExportFormat dataFormat;
        public string EmailName;
        private ExportEditors exportEditor;
        private ImageExportFormat format;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private ListBox listBox1;
        private NumericUpDown numericUpDown1;
        private NumericUpDown numericUpDown2;
        protected Form options;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private RadioButton radioButton1;
        private RadioButton radioButton2;
        private RadioButton radioButton3;
        private RadioButton radioButton4;
        private SaveFileDialog saveFileDialog1;
        private Splitter splitter1;
        public string standardFilters;
        private TabControl tabControl1;
        private TabControl tabControl2;
        private TabPage tabData;
        protected string[] tabDelimiters;
        private TabPage tabNative;
        private TabPage tabOptions;
        private TabPage tabPicture;
        private TabPage tabSize;
        private TextBox textDelimiter;

        public ExportEditor()
        {
            this.components = null;
            this.tabDelimiters = new string[] { " ", "\t", ",", ":" };
            this.EmailName = "TeeChart";
            this.InitializeComponent();
        }

        public ExportEditor(Chart c, Control parent) : this()
        {
            this.chart = c;
            if (this.Format == null)
            {
                this.tabControl1.SelectedIndex = 0;
            }
            this.button4.Hide();
            EditorUtils.InsertForm(this, parent);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.tabControl1.SelectedTab == this.tabPicture)
            {
                this.exportEditor.SetOptions(this.format);
                this.Format.CopyToClipboard();
            }
            else
            {
                this.dataFormat = null;
                this.SetDataExportOptions();
                this.dataFormat.CopyToClipboard();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.tabControl1.SelectedTab == this.tabPicture)
            {
                if (this.Format is MetafileFormat)
                {
                    this.exportEditor.SetOptions(this.Format);
                }
                this.saveFileDialog1.DefaultExt = this.Format.FileExtension;
                this.saveFileDialog1.FileName = "Chart1." + this.saveFileDialog1.DefaultExt;
                this.saveFileDialog1.Filter = this.Format.FilterFiles() + "|" + this.standardFilters;
                if (this.saveFileDialog1.ShowDialog(this) == DialogResult.OK)
                {
                    this.SavePictureToFile(this.saveFileDialog1.FileName);
                }
            }
            else if (this.tabControl1.SelectedTab == this.tabNative)
            {
                this.saveFileDialog1.DefaultExt = this.chart.Export.Template.FileExtension;
                this.saveFileDialog1.Filter = this.chart.Export.Template.FilterFiles() + "| All files (*.*)|*.*";
                this.saveFileDialog1.FileName = "Chart1." + this.saveFileDialog1.DefaultExt;
                if (this.saveFileDialog1.ShowDialog(this) == DialogResult.OK)
                {
                    this.SaveNativeToFile(this.saveFileDialog1.FileName);
                }
            }
            else
            {
                this.dataFormat = null;
                this.saveFileDialog1.DefaultExt = this.DataFormat.FileExtension;
                this.saveFileDialog1.Filter = this.DataFormat.FilterFiles() + "|" + this.standardFilters;
                this.saveFileDialog1.FileName = "Chart1." + this.saveFileDialog1.DefaultExt;
                if (this.saveFileDialog1.ShowDialog(this) == DialogResult.OK)
                {
                    this.SaveDataToFile(this.saveFileDialog1.FileName);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.SetAspectRatio(true);
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox3.Checked)
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    this.chart.Export.Template.Serialize(stream);
                    this.label3.Text = Convert.ToString(stream.Length);
                }
            }
        }

        public DataExportFormat CreateDataFormat()
        {
            int num = 6;
            if (this.radioButton1.Checked)
            {
                this.dataFormat = new TextFormat(this.chart.Chart);
                this.saveFileDialog1.FilterIndex = num + 1;
            }
            else if (this.radioButton2.Checked)
            {
                this.dataFormat = new XMLFormat(this.chart.Chart);
                this.saveFileDialog1.FilterIndex = num + 2;
            }
            else if (this.radioButton3.Checked)
            {
                this.dataFormat = new HTMLFormat(this.chart.Chart);
                this.saveFileDialog1.FilterIndex = num + 3;
            }
            else
            {
                this.dataFormat = new ExcelFormat(this.chart.Chart);
                this.saveFileDialog1.FilterIndex = num + 4;
            }
            return this.dataFormat;
        }

        public ImageExportFormat CreateFormat(int index)
        {
            switch (index)
            {
                case 1:
                    this.exportEditor = new MetafileEditor();
                    return new MetafileFormat(this.chart.Chart);

                case 2:
                    this.exportEditor = new JPEGEditor();
                    return new JPEGFormat(this.chart.Chart);

                case 3:
                    this.exportEditor = new PNGEditor();
                    return new PNGFormat(this.chart.Chart);

                case 4:
                    this.exportEditor = new GIFEditor();
                    return new GIFFormat(this.chart.Chart);

                case 5:
                    this.exportEditor = new TIFFEditor();
                    return new TIFFFormat(this.chart.Chart);
            }
            this.exportEditor = new BitmapEditor();
            return new BitmapFormat(this.chart.Chart);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void ExportEditor_Load(object sender, EventArgs e)
        {
            base.Icon = EditorUtils.TChartIcon();
            this.bSend.Visible = false;
            this.listBox1.Items.AddRange(ImageFormats.FormatDescriptions);
            this.standardFilters = "Data files |*.csv;*.txt;*.xml;*.xml;*.xls;*.htm;*.html|Image files |*.bmp;*.emf;*.wmf;*.jpg;*.jpeg;*.png;*.gif;*.tif;*.tiff|" + TemplateExport.FileFilter() + "|All files (*.*)|*.*";
            this.comboSeries.Items.Add(Texts.All);
            if (this.chart != null)
            {
                foreach (Series series in this.chart.Series)
                {
                    this.comboSeries.Items.Add(series.ToString());
                }
                if (this.comboSeries.Items.Count > 0)
                {
                    this.comboSeries.SelectedIndex = 0;
                }
            }
            this.comboDelimiter.SelectedIndex = 1;
            this.SetTabs();
        }

        public void GetDataExportOptions()
        {
            this.dataFormat = null;
            this.checkHeader.Enabled = !(this.DataFormat is XMLFormat);
        }

        protected internal int ImageHeight()
        {
            return (int) this.numericUpDown2.Value;
        }

        protected internal int ImageWidth()
        {
            return (int) this.numericUpDown1.Value;
        }

        private void InitializeComponent()
        {
            this.tabPicture = new TabPage();
            this.splitter1 = new Splitter();
            this.tabControl2 = new TabControl();
            this.tabOptions = new TabPage();
            this.tabSize = new TabPage();
            this.checkBox1 = new CheckBox();
            this.numericUpDown2 = new NumericUpDown();
            this.label2 = new Label();
            this.numericUpDown1 = new NumericUpDown();
            this.label1 = new Label();
            this.groupBox1 = new GroupBox();
            this.listBox1 = new ListBox();
            this.tabData = new TabPage();
            this.textDelimiter = new TextBox();
            this.comboDelimiter = new ComboBox();
            this.groupBox3 = new GroupBox();
            this.checkHeader = new CheckBox();
            this.checkPLabels = new CheckBox();
            this.checkPIndex = new CheckBox();
            this.groupBox2 = new GroupBox();
            this.radioButton4 = new RadioButton();
            this.radioButton3 = new RadioButton();
            this.radioButton2 = new RadioButton();
            this.radioButton1 = new RadioButton();
            this.comboSeries = new ComboBox();
            this.label4 = new Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button4 = new Button();
            this.bSend = new Button();
            this.button2 = new Button();
            this.button1 = new Button();
            this.tabControl1 = new TabControl();
            this.tabNative = new TabPage();
            this.label3 = new Label();
            this.checkBox3 = new CheckBox();
            this.checkBox2 = new CheckBox();
            this.saveFileDialog1 = new SaveFileDialog();
            this.tabPicture.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabSize.SuspendLayout();
            this.numericUpDown2.BeginInit();
            this.numericUpDown1.BeginInit();
            this.groupBox1.SuspendLayout();
            this.tabData.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabNative.SuspendLayout();
            base.SuspendLayout();
            this.tabPicture.Controls.AddRange(new Control[] { this.splitter1, this.tabControl2, this.groupBox1 });
            this.tabPicture.Location = new Point(4, 0x16);
            this.tabPicture.Name = "tabPicture";
            this.tabPicture.Size = new Size(0x188, 0xa4);
            this.tabPicture.TabIndex = 0;
            this.tabPicture.Text = "Picture";
            this.splitter1.Location = new Point(0x80, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new Size(3, 0xa4);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            this.tabControl2.Controls.AddRange(new Control[] { this.tabOptions, this.tabSize });
            this.tabControl2.Dock = DockStyle.Fill;
            this.tabControl2.HotTrack = true;
            this.tabControl2.Location = new Point(0x80, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new Size(0x108, 0xa4);
            this.tabControl2.TabIndex = 1;
            this.tabOptions.Location = new Point(4, 0x16);
            this.tabOptions.Name = "tabOptions";
            this.tabOptions.Size = new Size(0x100, 0x8a);
            this.tabOptions.TabIndex = 0;
            this.tabOptions.Text = "Options";
            this.tabSize.Controls.AddRange(new Control[] { this.checkBox1, this.numericUpDown2, this.label2, this.numericUpDown1, this.label1 });
            this.tabSize.Location = new Point(4, 0x16);
            this.tabSize.Name = "tabSize";
            this.tabSize.Size = new Size(0x100, 0x8a);
            this.tabSize.TabIndex = 1;
            this.tabSize.Text = "Size";
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = CheckState.Checked;
            this.checkBox1.FlatStyle = FlatStyle.Flat;
            this.checkBox1.Location = new Point(0x20, 80);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new Size(0x88, 0x18);
            this.checkBox1.TabIndex = 4;
            this.checkBox1.Text = "&Keep aspect ratio";
            this.checkBox1.CheckedChanged += new EventHandler(this.checkBox1_CheckedChanged);
            this.numericUpDown2.Location = new Point(0x40, 0x30);
            int[] bits = new int[4];
            bits[0] = 0x2710;
            this.numericUpDown2.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 1;
            this.numericUpDown2.Minimum = new decimal(bits);
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new Size(0x37, 20);
            this.numericUpDown2.TabIndex = 3;
            this.numericUpDown2.TextAlign = HorizontalAlignment.Right;
            bits = new int[4];
            bits[0] = 1;
            this.numericUpDown2.Value = new decimal(bits);
            this.numericUpDown2.Leave += new EventHandler(this.numericUpDown2_Leave);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x18, 50);
            this.label2.Name = "label2";
            this.label2.Size = new Size(40, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "&Height:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.numericUpDown1.Location = new Point(0x40, 0x16);
            bits = new int[4];
            bits[0] = 0x2710;
            this.numericUpDown1.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 1;
            this.numericUpDown1.Minimum = new decimal(bits);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new Size(0x37, 20);
            this.numericUpDown1.TabIndex = 1;
            this.numericUpDown1.TextAlign = HorizontalAlignment.Right;
            bits = new int[4];
            bits[0] = 1;
            this.numericUpDown1.Value = new decimal(bits);
            this.numericUpDown1.Leave += new EventHandler(this.numericUpDown1_Leave);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x18, 0x18);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x24, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Width:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            this.groupBox1.Controls.AddRange(new Control[] { this.listBox1 });
            this.groupBox1.Dock = DockStyle.Left;
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x80, 0xa4);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "&Format:";
            this.listBox1.BorderStyle = BorderStyle.FixedSingle;
            this.listBox1.Dock = DockStyle.Fill;
            this.listBox1.IntegralHeight = false;
            this.listBox1.Location = new Point(3, 0x10);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new Size(0x7a, 0x91);
            this.listBox1.TabIndex = 0;
            this.listBox1.SelectedIndexChanged += new EventHandler(this.listBox1_SelectedIndexChanged);
            this.tabData.Controls.AddRange(new Control[] { this.textDelimiter, this.comboDelimiter, this.groupBox3, this.groupBox2, this.comboSeries, this.label4 });
            this.tabData.Location = new Point(4, 0x16);
            this.tabData.Name = "tabData";
            this.tabData.Size = new Size(0x188, 0xa4);
            this.tabData.TabIndex = 2;
            this.tabData.Text = "Data";
            this.textDelimiter.Location = new Point(280, 0x80);
            this.textDelimiter.Name = "textDelimiter";
            this.textDelimiter.Size = new Size(0x30, 20);
            this.textDelimiter.TabIndex = 5;
            this.textDelimiter.Text = "";
            this.textDelimiter.TextChanged += new EventHandler(this.textDelimiter_TextChanged);
            this.comboDelimiter.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboDelimiter.Items.AddRange(new object[] { "Space", "Tab", "Comma", "Colon", "Custom" });
            this.comboDelimiter.Location = new Point(0xc0, 0x80);
            this.comboDelimiter.Name = "comboDelimiter";
            this.comboDelimiter.Size = new Size(80, 0x15);
            this.comboDelimiter.TabIndex = 4;
            this.groupBox3.Controls.AddRange(new Control[] { this.checkHeader, this.checkPLabels, this.checkPIndex });
            this.groupBox3.Location = new Point(0xb8, 8);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new Size(0xa8, 0x60);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "&Include:";
            this.checkHeader.Checked = true;
            this.checkHeader.CheckState = CheckState.Checked;
            this.checkHeader.FlatStyle = FlatStyle.Flat;
            this.checkHeader.Location = new Point(8, 0x40);
            this.checkHeader.Name = "checkHeader";
            this.checkHeader.Size = new Size(0x98, 0x18);
            this.checkHeader.TabIndex = 2;
            this.checkHeader.Text = "&Header";
            this.checkPLabels.Checked = true;
            this.checkPLabels.CheckState = CheckState.Checked;
            this.checkPLabels.FlatStyle = FlatStyle.Flat;
            this.checkPLabels.Location = new Point(8, 40);
            this.checkPLabels.Name = "checkPLabels";
            this.checkPLabels.Size = new Size(0x98, 0x18);
            this.checkPLabels.TabIndex = 1;
            this.checkPLabels.Text = "Point &Labels";
            this.checkPIndex.FlatStyle = FlatStyle.Flat;
            this.checkPIndex.Location = new Point(8, 0x10);
            this.checkPIndex.Name = "checkPIndex";
            this.checkPIndex.Size = new Size(0x98, 0x18);
            this.checkPIndex.TabIndex = 0;
            this.checkPIndex.Text = "Point &Index";
            this.groupBox2.Controls.AddRange(new Control[] { this.radioButton4, this.radioButton3, this.radioButton2, this.radioButton1 });
            this.groupBox2.Location = new Point(0x10, 40);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(160, 0x72);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Format:";
            this.radioButton4.FlatStyle = FlatStyle.Flat;
            this.radioButton4.Location = new Point(8, 0x58);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new Size(0x90, 0x18);
            this.radioButton4.TabIndex = 3;
            this.radioButton4.Text = "&Excel";
            this.radioButton4.CheckedChanged += new EventHandler(this.radioButton4_CheckedChanged);
            this.radioButton3.FlatStyle = FlatStyle.Flat;
            this.radioButton3.Location = new Point(8, 0x40);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new Size(0x90, 0x18);
            this.radioButton3.TabIndex = 2;
            this.radioButton3.Text = "&HTML Table";
            this.radioButton3.CheckedChanged += new EventHandler(this.radioButton3_CheckedChanged);
            this.radioButton2.FlatStyle = FlatStyle.Flat;
            this.radioButton2.Location = new Point(8, 40);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new Size(0x90, 0x18);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.Text = "&XML";
            this.radioButton2.CheckedChanged += new EventHandler(this.radioButton2_CheckedChanged);
            this.radioButton1.Checked = true;
            this.radioButton1.FlatStyle = FlatStyle.Flat;
            this.radioButton1.Location = new Point(8, 0x10);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new Size(0x90, 0x18);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "&Text";
            this.radioButton1.CheckedChanged += new EventHandler(this.radioButton1_CheckedChanged);
            this.comboSeries.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboSeries.Location = new Point(0x38, 13);
            this.comboSeries.Name = "comboSeries";
            this.comboSeries.Size = new Size(0x79, 0x15);
            this.comboSeries.TabIndex = 1;
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0x10, 0x10);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x27, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "&Series:";
            this.label4.TextAlign = ContentAlignment.TopRight;
            this.panel1.Controls.AddRange(new Control[] { this.panel2, this.bSend, this.button2, this.button1 });
            this.panel1.Dock = DockStyle.Bottom;
            this.panel1.Location = new Point(0, 190);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(400, 40);
            this.panel1.TabIndex = 0;
            this.panel2.Controls.AddRange(new Control[] { this.button4 });
            this.panel2.Dock = DockStyle.Right;
            this.panel2.Location = new Point(0x130, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new Size(0x60, 40);
            this.panel2.TabIndex = 3;
            this.button4.DialogResult = DialogResult.Cancel;
            this.button4.FlatStyle = FlatStyle.Flat;
            this.button4.Location = new Point(8, 8);
            this.button4.Name = "button4";
            this.button4.TabIndex = 0;
            this.button4.Text = "Close";
            this.bSend.FlatStyle = FlatStyle.Flat;
            this.bSend.Location = new Point(0xc0, 8);
            this.bSend.Name = "bSend";
            this.bSend.TabIndex = 2;
            this.bSend.Text = "S&end...";
            this.bSend.Click += new EventHandler(this.button3_Click);
            this.button2.FlatStyle = FlatStyle.Flat;
            this.button2.Location = new Point(0x60, 8);
            this.button2.Name = "button2";
            this.button2.TabIndex = 1;
            this.button2.Text = "&Save...";
            this.button2.Click += new EventHandler(this.button2_Click);
            this.button1.FlatStyle = FlatStyle.Flat;
            this.button1.Location = new Point(8, 8);
            this.button1.Name = "button1";
            this.button1.TabIndex = 0;
            this.button1.Text = "&Copy";
            this.button1.Click += new EventHandler(this.button1_Click);
            this.tabControl1.Controls.AddRange(new Control[] { this.tabPicture, this.tabNative, this.tabData });
            this.tabControl1.Dock = DockStyle.Fill;
            this.tabControl1.HotTrack = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new Size(400, 190);
            this.tabControl1.TabIndex = 0;
            this.tabNative.Controls.AddRange(new Control[] { this.label3, this.checkBox3, this.checkBox2 });
            this.tabNative.Location = new Point(4, 0x16);
            this.tabNative.Name = "tabNative";
            this.tabNative.Size = new Size(0x188, 0xa4);
            this.tabNative.TabIndex = 1;
            this.tabNative.Text = "Native";
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0xc0, 0x40);
            this.label3.Name = "label3";
            this.label3.Size = new Size(10, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "?";
            this.label3.UseMnemonic = false;
            this.checkBox3.FlatStyle = FlatStyle.Flat;
            this.checkBox3.Location = new Point(0x20, 60);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new Size(0x98, 0x18);
            this.checkBox3.TabIndex = 1;
            this.checkBox3.Text = "File &Size:";
            this.checkBox3.CheckedChanged += new EventHandler(this.checkBox3_CheckedChanged);
            this.checkBox2.Checked = true;
            this.checkBox2.CheckState = CheckState.Checked;
            this.checkBox2.FlatStyle = FlatStyle.Flat;
            this.checkBox2.Location = new Point(0x20, 0x18);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new Size(0xe8, 0x18);
            this.checkBox2.TabIndex = 0;
            this.checkBox2.Text = "Include Series &Data";
            this.saveFileDialog1.FileName = "Chart1";
            this.saveFileDialog1.Title = "Save Chart to file";
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this.button4;
            base.ClientSize = new Size(400, 230);
            base.Controls.AddRange(new Control[] { this.tabControl1, this.panel1 });
            base.MaximizeBox = false;
            base.Name = "ExportEditor";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "TeeChart Export";
            base.Load += new EventHandler(this.ExportEditor_Load);
            this.tabPicture.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabSize.ResumeLayout(false);
            this.numericUpDown2.EndInit();
            this.numericUpDown1.EndInit();
            this.groupBox1.ResumeLayout(false);
            this.tabData.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabNative.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.numericUpDown1.Value = this.chart.Width;
            this.numericUpDown2.Value = this.chart.Height;
            this.format = null;
            if (this.options != null)
            {
                this.options.Dispose();
            }
            this.format = this.CreateFormat(this.listBox1.SelectedIndex);
            this.options = this.exportEditor.Options();
            this.tabControl2.TabPages.Clear();
            if (this.options != null)
            {
                this.tabControl2.TabPages.Add(this.tabOptions);
            }
            this.tabControl2.TabPages.Add(this.tabSize);
            if (this.options != null)
            {
                EditorUtils.InsertForm(this.options, this.tabOptions);
                EditorUtils.Translate(this.options);
            }
        }

        private void numericUpDown1_Leave(object sender, EventArgs e)
        {
            this.SetAspectRatio(true);
        }

        private void numericUpDown2_Leave(object sender, EventArgs e)
        {
            this.SetAspectRatio(false);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            this.GetDataExportOptions();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            this.GetDataExportOptions();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            this.GetDataExportOptions();
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            this.GetDataExportOptions();
        }

        private void SaveDataToFile(string filename)
        {
            this.SetDataExportOptions();
            this.DataFormat.Save(filename);
        }

        private void SaveNativeToFile(string filename)
        {
            Stream stream = File.Create(filename);
            try
            {
                this.chart.Export.Template.Serialize(stream);
            }
            finally
            {
                stream.Close();
            }
        }

        private void SavePictureToFile(string filename)
        {
            this.exportEditor.SetOptions(this.format);
            this.format.Save(filename);
        }

        private void SetAspectRatio(bool horiz)
        {
            if (this.checkBox1.Checked)
            {
                if (horiz)
                {
                    this.numericUpDown2.Value = this.numericUpDown1.Value / ((decimal) this.format.chartWidthHeightRatio);
                }
                else
                {
                    this.numericUpDown1.Value = this.numericUpDown2.Value * ((decimal) this.format.chartWidthHeightRatio);
                }
            }
        }

        public void SetDataExportOptions()
        {
            this.DataFormat.IncludeHeader = this.checkHeader.Checked;
            this.dataFormat.IncludeLabels = this.checkPLabels.Checked;
            this.dataFormat.IncludeIndex = this.checkPIndex.Checked;
            if (this.comboSeries.SelectedIndex == 0)
            {
                this.dataFormat.Series = null;
            }
            else
            {
                this.dataFormat.Series = this.chart[this.comboSeries.SelectedIndex - 1];
            }
            if (this.dataFormat is TextFormat)
            {
                if (this.comboDelimiter.SelectedIndex == 4)
                {
                    ((TextFormat) this.dataFormat).TextDelimiter = this.textDelimiter.Text;
                }
                else
                {
                    ((TextFormat) this.dataFormat).TextDelimiter = this.tabDelimiters[this.comboDelimiter.SelectedIndex];
                }
            }
        }

        private void SetTabs()
        {
            if (this.format != null)
            {
                this.tabControl1.SelectedTab = this.tabPicture;
                for (int i = 0; i < 8; i++)
                {
                    if (this.Format.GetType() == ImageFormats.Formats[i])
                    {
                        this.listBox1.SelectedIndex = i;
                        return;
                    }
                }
            }
            else if (this.dataFormat != null)
            {
                this.tabControl1.SelectedTab = this.tabData;
                if (this.DataFormat is TextFormat)
                {
                    this.radioButton1.Checked = true;
                }
                else if (this.DataFormat is XMLFormat)
                {
                    this.radioButton2.Checked = true;
                }
                else if (this.DataFormat is HTMLFormat)
                {
                    this.radioButton3.Checked = true;
                }
                else if (this.DataFormat is ExcelFormat)
                {
                    this.radioButton4.Checked = true;
                }
            }
            else
            {
                this.tabControl1.SelectedTab = this.tabNative;
            }
        }

        public static bool ShowModal(Chart c)
        {
            using (ExportEditor editor = new ExportEditor())
            {
                editor.chart = c;
                if (editor.Format == null)
                {
                    editor.tabControl1.SelectedIndex = 0;
                }
                EditorUtils.Translate(editor);
                return (editor.ShowDialog() == DialogResult.OK);
            }
        }

        public static bool ShowModal(Chart c, ExportFormat exportFmt)
        {
            using (ExportEditor editor = new ExportEditor())
            {
                editor.chart = c;
                if (exportFmt is ImageExportFormat)
                {
                    editor.format = (ImageExportFormat) exportFmt;
                }
                else if (exportFmt is DataExportFormat)
                {
                    editor.dataFormat = (DataExportFormat) exportFmt;
                }
                EditorUtils.Translate(editor);
                return (editor.ShowDialog() == DialogResult.OK);
            }
        }

        private void textDelimiter_TextChanged(object sender, EventArgs e)
        {
            this.comboDelimiter.SelectedIndex = 4;
        }

        public DataExportFormat DataFormat
        {
            get
            {
                if (this.dataFormat == null)
                {
                    this.dataFormat = this.CreateDataFormat();
                }
                return this.dataFormat;
            }
        }

        private int FilterIndex
        {
            get
            {
                return this.saveFileDialog1.FilterIndex;
            }
        }

        public ImageExportFormat Format
        {
            get
            {
                if (this.format == null)
                {
                    this.format = this.CreateFormat(this.listBox1.SelectedIndex);
                }
                return this.format;
            }
        }
    }
}

