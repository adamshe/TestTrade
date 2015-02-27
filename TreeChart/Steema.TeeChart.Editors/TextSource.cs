namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart;
    using Steema.TeeChart.Data;
    using Steema.TeeChart.Styles;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class TextSource : BaseSourceEditor
    {
        private Button BBrowse;
        private ComboBox CBSep;
        private IContainer components;
        private TextBox EFile;
        private TextBox EWeb;
        private ArrayList FieldTexts;
        private GroupBox groupBox1;
        private Label label1;
        private Label label2;
        private Label labelText;
        private NumericUpDown numericUpDown1;
        private OpenFileDialog openFileDialog1;
        private RadioButton RBFile;
        private RadioButton RBWeb;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TextBox textField;

        public TextSource()
        {
            this.components = null;
            this.FieldTexts = new ArrayList();
            this.InitializeComponent();
            base.CBSources.Visible = false;
            base.labelSource.Visible = false;
        }

        public TextSource(Steema.TeeChart.Styles.Series s)
            : this()
        {
            base.series = s;
        }

        protected override void ApplyChanges()
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (base.series.DataSource is Steema.TeeChart.Data.TextSource)
                {
                    this.SetOptions((Steema.TeeChart.Data.TextSource) base.series.DataSource);
                }
                else
                {
                    base.series.DataSource = null;
                    Steema.TeeChart.Data.TextSource source = new Steema.TeeChart.Data.TextSource();
                    this.SetOptions(source);
                    base.series.DataSource = source;
                    IContainer chartContainer = base.series.Chart.ChartContainer;
                    if (chartContainer != null)
                    {
                        source.AddToContainer(chartContainer);
                    }
                }
                base.BApply.Enabled = false;
                base.series.CheckDataSource();
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void BBrowse_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                this.EFile.Text = this.openFileDialog1.FileName;
                this.EFile.Focus();
                base.BApply.Enabled = true;
            }
        }

        private int CountFields()
        {
            int num = (this.textField.Text.Length == 0) ? 0 : 1;
            foreach (TextBox box in this.FieldTexts)
            {
                if ((box != null) && (box.Text.Length != 0))
                {
                    num++;
                }
            }
            return num;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void EFile_TextChanged(object sender, EventArgs e)
        {
            base.BApply.Enabled = true;
        }

        private void InitializeComponent()
        {
            this.tabControl1 = new TabControl();
            this.tabPage1 = new TabPage();
            this.groupBox1 = new GroupBox();
            this.textField = new TextBox();
            this.labelText = new Label();
            this.CBSep = new ComboBox();
            this.label2 = new Label();
            this.numericUpDown1 = new NumericUpDown();
            this.label1 = new Label();
            this.tabPage2 = new TabPage();
            this.EWeb = new TextBox();
            this.BBrowse = new Button();
            this.EFile = new TextBox();
            this.RBWeb = new RadioButton();
            this.RBFile = new RadioButton();
            this.openFileDialog1 = new OpenFileDialog();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.numericUpDown1.BeginInit();
            this.tabPage2.SuspendLayout();
            base.SuspendLayout();
            base.CBSources.Name = "CBSources";
            base.CBSources.Size = new Size(0xb8, 0x15);
            base.BApply.Name = "BApply";
            base.labelSource.Name = "labelSource";
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = DockStyle.Fill;
            this.tabControl1.HotTrack = true;
            this.tabControl1.Location = new Point(0, 30);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new Size(0x184, 0xa7);
            this.tabControl1.TabIndex = 1;
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.CBSep);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.numericUpDown1);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new Point(4, 0x16);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new Size(380, 0x8d);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Fields";
            this.groupBox1.Controls.Add(this.textField);
            this.groupBox1.Controls.Add(this.labelText);
            this.groupBox1.Location = new Point(0xb0, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0xc0, 0x6b);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "&Fields:";
            this.textField.BorderStyle = BorderStyle.FixedSingle;
            this.textField.Location = new Point(0x73, 15);
            this.textField.Name = "textField";
            this.textField.Size = new Size(0x30, 20);
            this.textField.TabIndex = 1;
            this.textField.Text = "";
            this.textField.TextAlign = HorizontalAlignment.Right;
            this.textField.TextChanged += new EventHandler(this.textField_TextChanged);
            this.labelText.AutoSize = true;
            this.labelText.Location = new Point(80, 0x12);
            this.labelText.Name = "labelText";
            this.labelText.Size = new Size(0x1d, 0x10);
            this.labelText.TabIndex = 0;
            this.labelText.Text = "&Text:";
            this.labelText.TextAlign = ContentAlignment.TopRight;
            this.CBSep.Items.AddRange(new object[] { "Comma", "Space", "Tab" });
            this.CBSep.Location = new Point(0x10, 80);
            this.CBSep.Name = "CBSep";
            this.CBSep.Size = new Size(0x79, 0x15);
            this.CBSep.TabIndex = 3;
            this.CBSep.TextChanged += new EventHandler(this.EFile_TextChanged);
            this.CBSep.SelectedIndexChanged += new EventHandler(this.EFile_TextChanged);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x10, 60);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x39, 0x10);
            this.label2.TabIndex = 2;
            this.label2.Text = "&Separator:";
            this.numericUpDown1.BorderStyle = BorderStyle.FixedSingle;
            this.numericUpDown1.Location = new Point(0x10, 0x1f);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new Size(0x38, 20);
            this.numericUpDown1.TabIndex = 1;
            this.numericUpDown1.TextAlign = HorizontalAlignment.Right;
            this.numericUpDown1.ValueChanged += new EventHandler(this.EFile_TextChanged);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x10, 11);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x7f, 0x10);
            this.label1.TabIndex = 0;
            this.label1.Text = "Number of &Header lines:";
            this.tabPage2.Controls.Add(this.EWeb);
            this.tabPage2.Controls.Add(this.BBrowse);
            this.tabPage2.Controls.Add(this.EFile);
            this.tabPage2.Controls.Add(this.RBWeb);
            this.tabPage2.Controls.Add(this.RBFile);
            this.tabPage2.Location = new Point(4, 0x16);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new Size(380, 0x8d);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Source";
            this.EWeb.BorderStyle = BorderStyle.FixedSingle;
            this.EWeb.Location = new Point(0x68, 0x38);
            this.EWeb.Name = "EWeb";
            this.EWeb.Size = new Size(200, 20);
            this.EWeb.TabIndex = 4;
            this.EWeb.Text = "http://www.steema.com/test.txt";
            this.EWeb.TextChanged += new EventHandler(this.EFile_TextChanged);
            this.BBrowse.FlatStyle = FlatStyle.Flat;
            this.BBrowse.Location = new Point(0x138, 0x17);
            this.BBrowse.Name = "BBrowse";
            this.BBrowse.Size = new Size(0x20, 0x17);
            this.BBrowse.TabIndex = 3;
            this.BBrowse.Text = "...";
            this.BBrowse.Click += new EventHandler(this.BBrowse_Click);
            this.EFile.BorderStyle = BorderStyle.FixedSingle;
            this.EFile.Location = new Point(0x68, 0x18);
            this.EFile.Name = "EFile";
            this.EFile.Size = new Size(200, 20);
            this.EFile.TabIndex = 2;
            this.EFile.Text = "";
            this.EFile.TextChanged += new EventHandler(this.EFile_TextChanged);
            this.RBWeb.FlatStyle = FlatStyle.Flat;
            this.RBWeb.Location = new Point(0x10, 0x36);
            this.RBWeb.Name = "RBWeb";
            this.RBWeb.Size = new Size(0x58, 0x18);
            this.RBWeb.TabIndex = 1;
            this.RBWeb.Text = "&Web URL:";
            this.RBWeb.CheckedChanged += new EventHandler(this.radioButton1_CheckedChanged);
            this.RBFile.FlatStyle = FlatStyle.Flat;
            this.RBFile.Location = new Point(0x10, 0x16);
            this.RBFile.Name = "RBFile";
            this.RBFile.Size = new Size(0x58, 0x18);
            this.RBFile.TabIndex = 0;
            this.RBFile.Text = "&File:";
            this.RBFile.CheckedChanged += new EventHandler(this.radioButton1_CheckedChanged);
            this.openFileDialog1.DefaultExt = "txt";
            this.openFileDialog1.Title = "Open text file";
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x184, 0xc5);
            base.Controls.Add(this.tabControl1);
            base.Name = "TextSource";
            base.Load += new EventHandler(this.TextSource_Load);
            base.Controls.SetChildIndex(this.tabControl1, 0);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.numericUpDown1.EndInit();
            this.tabPage2.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            this.EFile.Enabled = this.RBFile.Checked;
            this.RBWeb.Checked = !this.RBFile.Checked;
            this.EWeb.Enabled = !this.EFile.Enabled;
            this.BBrowse.Enabled = this.EFile.Enabled;
            this.EFile_TextChanged(sender, e);
        }

        private void SetOptions(Steema.TeeChart.Data.TextSource source)
        {
            if (this.RBWeb.Checked)
            {
                source.FileName = this.EWeb.Text;
            }
            else
            {
                source.FileName = this.EFile.Text;
            }
            source.HeaderLines = (int) this.numericUpDown1.Value;
            switch (this.CBSep.SelectedIndex)
            {
                case 0:
                    source.Separator = ',';
                    break;

                case 1:
                    source.Separator = ' ';
                    break;

                case 2:
                    source.Separator = '\t';
                    break;

                default:
                    source.Separator = Convert.ToChar(this.CBSep.Text);
                    break;
            }
            source.Fields.Clear();
            if (this.textField.Text.Length != 0)
            {
                source.Fields.Add(Convert.ToInt32(this.textField.Text), Texts.Text);
            }
            foreach (TextBox box in this.FieldTexts)
            {
                if ((box != null) && (box.Text.Length != 0))
                {
                    source.Fields.Add(Convert.ToInt32(box.Text), ((Steema.TeeChart.Styles.ValueList) box.Tag).Name);
                }
            }
        }

        private void SetupControls()
        {
            this.EWeb.Text = Texts.TextSrcURL;
            for (int i = 0; i < base.series.ValuesLists.Count; i++)
            {
                Steema.TeeChart.Styles.ValueList list = base.series.ValuesLists[i];
                Label label = new Label();
                label.Text = list.Name;
                label.AutoSize = true;
                label.Left = this.labelText.Left;
                label.Top = this.labelText.Top + (20 * (i + 1));
                label.UseMnemonic = false;
                this.groupBox1.Controls.Add(label);
                TextBox box = new TextBox();
                box.Top = label.Top;
                box.Left = this.textField.Left;
                box.TextAlign = HorizontalAlignment.Right;
                box.BorderStyle = BorderStyle.FixedSingle;
                box.Tag = list;
                box.Width = this.textField.Width;
                if (base.series.DataSource is Steema.TeeChart.Data.TextSource)
                {
                    Steema.TeeChart.Data.TextSource dataSource = (Steema.TeeChart.Data.TextSource) base.series.DataSource;
                    if (dataSource.Fields != null)
                    {
                        foreach (TextField field in dataSource.Fields)
                        {
                            if (field.Name == list.Name)
                            {
                                box.Text = field.Index.ToString();
                            }
                        }
                    }
                }
                box.TextChanged += new EventHandler(this.textField_TextChanged);
                this.groupBox1.Controls.Add(box);
                this.FieldTexts.Add(box);
            }
            if (base.series.DataSource is Steema.TeeChart.Data.TextSource)
            {
                Steema.TeeChart.Data.TextSource source2 = (Steema.TeeChart.Data.TextSource) base.series.DataSource;
                if (source2.IsURL())
                {
                    this.EWeb.Text = source2.FileName;
                    this.EFile.Text = "";
                    this.RBWeb.Checked = true;
                }
                else
                {
                    this.EFile.Text = source2.FileName;
                    this.EWeb.Text = "";
                    this.RBFile.Checked = true;
                }
                this.numericUpDown1.Value = source2.HeaderLines;
                char separator = source2.Separator;
                if (separator == '\t')
                {
                    this.CBSep.SelectedIndex = 2;
                }
                else if (separator != ' ')
                {
                    if (separator != ',')
                    {
                        this.CBSep.Text = source2.Separator.ToString();
                    }
                    else
                    {
                        this.CBSep.SelectedIndex = 0;
                    }
                }
                else
                {
                    this.CBSep.SelectedIndex = 1;
                }
                this.tabControl1.SelectedTab = this.tabPage1;
                if (source2.Fields != null)
                {
                    foreach (TextField field2 in source2.Fields)
                    {
                        if (field2.Name == Texts.Text)
                        {
                            this.textField.Text = field2.Index.ToString();
                            break;
                        }
                    }
                }
            }
            else
            {
                this.CBSep.SelectedIndex = 0;
                this.tabControl1.SelectedTab = this.tabPage2;
                this.RBFile.Checked = true;
                this.EFile.Focus();
            }
            this.openFileDialog1.Filter = Texts.TextFile + " (*.txt)|*.txt|" + Texts.TextFile + " (*.csv)|*.csv";
            base.BApply.Enabled = !(base.series.DataSource is Steema.TeeChart.Data.TextSource);
        }

        private void textField_TextChanged(object sender, EventArgs e)
        {
            base.BApply.Enabled = true;
        }

        private void TextSource_Load(object sender, EventArgs e)
        {
            if (base.series != null)
            {
                this.SetupControls();
            }
        }
    }
}

