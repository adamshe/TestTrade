namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Printing;
    using System.Windows.Forms;

    public class PrintPreview : Form
    {
        private Button bFirst;
        private Button bLast;
        private Button bNext;
        private Button bPrevious;
        private Button button1;
        private Button button2;
        private Button button3;
        private ComboBox cbMarginUnits;
        private CheckBox cbPrintBackground;
        private CheckBox cbQuality;
        private Chart chart;
        private CheckBox checkBox2;
        private CheckBox checkBox3;
        private ComboBox comboBox1;
        private Container components;
        private GroupBox groupBox1;
        private GroupBox groupMargins;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label labelDetail;
        private Label lbResolution;
        private PageSetupDialog pageSetupDialog1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private PrintDocument printDocument1;
        private PrintPreviewControl printPreviewControl1;
        private Printer prn;
        private RadioButton radioButton1;
        private RadioButton radioButton2;
        private bool setting;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TrackBar tbResolution;
        private TrackBar trackBar1;
        private NumericUpDown upDBottom;
        private NumericUpDown upDLeft;
        private NumericUpDown upDRight;
        private NumericUpDown upDTop;

        public PrintPreview()
        {
            this.components = null;
            this.setting = false;
            this.InitializeComponent();
        }

        public PrintPreview(Chart c, Printer p) : this()
        {
            this.chart = c;
            this.prn = p;
            this.printDocument1 = this.prn.PrintDocument;
            if (this.prn.Landscape)
            {
                this.radioButton2.Checked = true;
            }
            else
            {
                this.radioButton1.Checked = true;
            }
        }

        public PrintPreview(Chart c, Control parent) : this(c, c.Printer)
        {
            this.button3.Hide();
            EditorUtils.InsertForm(this, parent);
        }

        private void bFirst_Click(object sender, EventArgs e)
        {
            this.chart.Page.Current = 0;
            this.RefreshView();
        }

        private void bLast_Click(object sender, EventArgs e)
        {
            this.chart.Page.Current = this.chart.Page.Count - 1;
            this.RefreshView();
        }

        private void bNext_Click(object sender, EventArgs e)
        {
            this.chart.Page.Next();
            this.RefreshView();
        }

        private void bPrevious_Click(object sender, EventArgs e)
        {
            this.chart.Page.Previous();
            this.RefreshView();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.pageSetupDialog1.PageSettings.Landscape = this.prn.Landscape;
            this.pageSetupDialog1.ShowDialog();
            this.prn.Landscape = this.pageSetupDialog1.PageSettings.Landscape;
            if (this.prn.Landscape)
            {
                this.radioButton2.Checked = true;
            }
            else
            {
                this.radioButton1.Checked = true;
            }
            this.prn.PrintDocument.DefaultPageSettings.PaperSize = this.pageSetupDialog1.PageSettings.PaperSize;
            this.RefreshView();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (this.prn.isPartial)
                {
                    this.prn.PrintDocument.Print();
                }
                else
                {
                    this.prn.Print();
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void cbMarginUnits_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.prn.MarginUnits = (PrintMarginUnits) this.cbMarginUnits.SelectedIndex;
                this.prn.GetOptions();
                this.RefreshView();
            }
        }

        private void cbMaxDetail_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void cbPrintBackground_CheckedChanged(object sender, EventArgs e)
        {
            this.prn.PrintPanelBackground = this.cbPrintBackground.Checked;
            this.RefreshView();
        }

        private void cbQuality_CheckedChanged(object sender, EventArgs e)
        {
            this.printPreviewControl1.UseAntiAlias = this.cbQuality.Checked;
            this.prn.UseAntiAlias = this.cbQuality.Checked;
            this.RefreshView();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.prn.Proportional = this.checkBox2.Checked;
                this.RefreshView();
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            this.prn.Grayscale = this.checkBox3.Checked;
            this.printPreviewControl1.Document.DefaultPageSettings.Color = !this.checkBox3.Checked;
            this.RefreshView();
        }

        private void CheckPageButtons()
        {
            this.bFirst.Visible = this.chart.Page.MaxPointsPerPage != 0;
            this.bLast.Visible = this.bFirst.Visible;
            this.bPrevious.Visible = this.bFirst.Visible;
            this.bNext.Visible = this.bFirst.Visible;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.printDocument1.PrinterSettings.PrinterName = this.comboBox1.SelectedItem.ToString();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private int GetMarginVal(int marginValue, bool isWidth)
        {
            int width;
            if (isWidth)
            {
                width = this.prn.PrintDocument.DefaultPageSettings.PaperSize.Width;
            }
            else
            {
                width = this.prn.PrintDocument.DefaultPageSettings.PaperSize.Height;
            }
            return ((marginValue * width) / 100);
        }

        private void InitializeComponent()
        {
            this.pageSetupDialog1 = new PageSetupDialog();
            this.printDocument1 = new PrintDocument();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button3 = new Button();
            this.button2 = new Button();
            this.button1 = new Button();
            this.comboBox1 = new ComboBox();
            this.label1 = new Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tabControl1 = new TabControl();
            this.tabPage1 = new TabPage();
            this.bNext = new Button();
            this.bPrevious = new Button();
            this.bLast = new Button();
            this.groupMargins = new GroupBox();
            this.label3 = new Label();
            this.cbMarginUnits = new ComboBox();
            this.upDBottom = new NumericUpDown();
            this.upDRight = new NumericUpDown();
            this.upDLeft = new NumericUpDown();
            this.upDTop = new NumericUpDown();
            this.label2 = new Label();
            this.trackBar1 = new TrackBar();
            this.groupBox1 = new GroupBox();
            this.radioButton2 = new RadioButton();
            this.radioButton1 = new RadioButton();
            this.bFirst = new Button();
            this.tabPage2 = new TabPage();
            this.lbResolution = new Label();
            this.labelDetail = new Label();
            this.tbResolution = new TrackBar();
            this.checkBox3 = new CheckBox();
            this.checkBox2 = new CheckBox();
            this.cbQuality = new CheckBox();
            this.cbPrintBackground = new CheckBox();
            this.printPreviewControl1 = new PrintPreviewControl();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupMargins.SuspendLayout();
            this.upDBottom.BeginInit();
            this.upDRight.BeginInit();
            this.upDLeft.BeginInit();
            this.upDTop.BeginInit();
            this.trackBar1.BeginInit();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tbResolution.BeginInit();
            base.SuspendLayout();
            this.pageSetupDialog1.Document = this.printDocument1;
            this.printDocument1.DocumentName = "TeeChart";
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = DockStyle.Top;
            this.panel1.Location = new Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x1f0, 0x20);
            this.panel1.TabIndex = 0;
            this.button3.DialogResult = DialogResult.Cancel;
            this.button3.FlatStyle = FlatStyle.Flat;
            this.button3.Location = new Point(400, 4);
            this.button3.Name = "button3";
            this.button3.TabIndex = 4;
            this.button3.Text = "Close";
            this.button2.FlatStyle = FlatStyle.Flat;
            this.button2.Location = new Point(0x138, 4);
            this.button2.Name = "button2";
            this.button2.TabIndex = 3;
            this.button2.Text = "&Print";
            this.button2.Click += new EventHandler(this.button2_Click);
            this.button1.FlatStyle = FlatStyle.Flat;
            this.button1.Location = new Point(0xe0, 4);
            this.button1.Name = "button1";
            this.button1.TabIndex = 2;
            this.button1.Text = "&Setup...";
            this.button1.Click += new EventHandler(this.button1_Click);
            this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBox1.DropDownWidth = 0xb0;
            this.comboBox1.Location = new Point(0x37, 4);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new Size(0xa1, 0x15);
            this.comboBox1.TabIndex = 1;
            this.comboBox1.SelectedIndexChanged += new EventHandler(this.comboBox1_SelectedIndexChanged);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(13, 7);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x29, 0x10);
            this.label1.TabIndex = 0;
            this.label1.Text = "Pr&inter:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            this.panel2.Controls.Add(this.tabControl1);
            this.panel2.Dock = DockStyle.Left;
            this.panel2.Location = new Point(0, 0x20);
            this.panel2.Name = "panel2";
            this.panel2.Size = new Size(0x80, 0x14d);
            this.panel2.TabIndex = 1;
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = DockStyle.Fill;
            this.tabControl1.HotTrack = true;
            this.tabControl1.Location = new Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new Size(0x80, 0x14d);
            this.tabControl1.TabIndex = 0;
            this.tabPage1.Controls.Add(this.bNext);
            this.tabPage1.Controls.Add(this.bPrevious);
            this.tabPage1.Controls.Add(this.bLast);
            this.tabPage1.Controls.Add(this.groupMargins);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.trackBar1);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.bFirst);
            this.tabPage1.Location = new Point(4, 0x16);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new Size(120, 0x133);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Page";
            this.bNext.FlatStyle = FlatStyle.Flat;
            this.bNext.Location = new Point(60, 270);
            this.bNext.Name = "bNext";
            this.bNext.Size = new Size(0x17, 0x17);
            this.bNext.TabIndex = 6;
            this.bNext.Text = ">";
            this.bNext.Click += new EventHandler(this.bNext_Click);
            this.bPrevious.FlatStyle = FlatStyle.Flat;
            this.bPrevious.Location = new Point(0x22, 270);
            this.bPrevious.Name = "bPrevious";
            this.bPrevious.Size = new Size(0x17, 0x17);
            this.bPrevious.TabIndex = 5;
            this.bPrevious.Text = "<";
            this.bPrevious.Click += new EventHandler(this.bPrevious_Click);
            this.bLast.FlatStyle = FlatStyle.Flat;
            this.bLast.Location = new Point(0x55, 270);
            this.bLast.Name = "bLast";
            this.bLast.Size = new Size(0x1b, 0x17);
            this.bLast.TabIndex = 7;
            this.bLast.Text = ">>";
            this.bLast.Click += new EventHandler(this.bLast_Click);
            this.groupMargins.Controls.Add(this.label3);
            this.groupMargins.Controls.Add(this.cbMarginUnits);
            this.groupMargins.Controls.Add(this.upDBottom);
            this.groupMargins.Controls.Add(this.upDRight);
            this.groupMargins.Controls.Add(this.upDLeft);
            this.groupMargins.Controls.Add(this.upDTop);
            this.groupMargins.Location = new Point(3, 0x70);
            this.groupMargins.Name = "groupMargins";
            this.groupMargins.Size = new Size(0x70, 0x90);
            this.groupMargins.TabIndex = 3;
            this.groupMargins.TabStop = false;
            this.groupMargins.Text = "&Margins";
            this.label3.AutoSize = true;
            this.label3.Location = new Point(5, 0x60);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x47, 0x10);
            this.label3.TabIndex = 4;
            this.label3.Text = "Margin &Units:";
            this.cbMarginUnits.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbMarginUnits.Items.AddRange(new object[] { "Percent", "1/100 Inch" });
            this.cbMarginUnits.Location = new Point(5, 0x73);
            this.cbMarginUnits.Name = "cbMarginUnits";
            this.cbMarginUnits.Size = new Size(0x63, 0x15);
            this.cbMarginUnits.TabIndex = 5;
            this.cbMarginUnits.SelectedIndexChanged += new EventHandler(this.cbMarginUnits_SelectedIndexChanged);
            this.upDBottom.BorderStyle = BorderStyle.FixedSingle;
            this.upDBottom.Location = new Point(0x20, 0x42);
            int[] bits = new int[4];
            bits[0] = 500;
            this.upDBottom.Maximum = new decimal(bits);
            this.upDBottom.Name = "upDBottom";
            this.upDBottom.Size = new Size(0x30, 20);
            this.upDBottom.TabIndex = 3;
            this.upDBottom.TextAlign = HorizontalAlignment.Right;
            this.upDBottom.TextChanged += new EventHandler(this.upDBottom_TextChanged);
            this.upDBottom.ValueChanged += new EventHandler(this.upDBottom_ValueChanged);
            this.upDRight.BorderStyle = BorderStyle.FixedSingle;
            this.upDRight.Location = new Point(0x38, 40);
            bits = new int[4];
            bits[0] = 500;
            this.upDRight.Maximum = new decimal(bits);
            this.upDRight.Name = "upDRight";
            this.upDRight.Size = new Size(0x30, 20);
            this.upDRight.TabIndex = 2;
            this.upDRight.TextAlign = HorizontalAlignment.Right;
            this.upDRight.TextChanged += new EventHandler(this.upDRight_TextChanged);
            this.upDRight.ValueChanged += new EventHandler(this.upDRight_ValueChanged);
            this.upDLeft.BorderStyle = BorderStyle.FixedSingle;
            this.upDLeft.Location = new Point(8, 40);
            bits = new int[4];
            bits[0] = 500;
            this.upDLeft.Maximum = new decimal(bits);
            this.upDLeft.Name = "upDLeft";
            this.upDLeft.Size = new Size(0x30, 20);
            this.upDLeft.TabIndex = 1;
            this.upDLeft.TextAlign = HorizontalAlignment.Right;
            this.upDLeft.TextChanged += new EventHandler(this.upDLeft_TextChanged);
            this.upDLeft.ValueChanged += new EventHandler(this.upDLeft_ValueChanged);
            this.upDTop.BorderStyle = BorderStyle.FixedSingle;
            this.upDTop.Location = new Point(0x20, 0x10);
            bits = new int[4];
            bits[0] = 500;
            this.upDTop.Maximum = new decimal(bits);
            this.upDTop.Name = "upDTop";
            this.upDTop.Size = new Size(0x30, 20);
            this.upDTop.TabIndex = 0;
            this.upDTop.TextAlign = HorizontalAlignment.Right;
            this.upDTop.TextChanged += new EventHandler(this.upDTop_TextChanged);
            this.upDTop.ValueChanged += new EventHandler(this.upDTop_ValueChanged);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(14, 0x47);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x24, 0x10);
            this.label2.TabIndex = 1;
            this.label2.Text = "&Zoom:";
            this.trackBar1.AutoSize = false;
            this.trackBar1.LargeChange = 20;
            this.trackBar1.Location = new Point(6, 0x57);
            this.trackBar1.Maximum = 200;
            this.trackBar1.Minimum = 1;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new Size(0x68, 0x11);
            this.trackBar1.SmallChange = 10;
            this.trackBar1.TabIndex = 2;
            this.trackBar1.TickFrequency = 10;
            this.trackBar1.Value = 100;
            this.trackBar1.Scroll += new EventHandler(this.trackBar1_Scroll);
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Location = new Point(3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x70, 60);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Orientation:";
            this.radioButton2.FlatStyle = FlatStyle.Flat;
            this.radioButton2.Location = new Point(8, 0x21);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new Size(0x60, 0x18);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.Text = "&Landscape";
            this.radioButton2.CheckedChanged += new EventHandler(this.radioButton2_CheckedChanged);
            this.radioButton1.FlatStyle = FlatStyle.Flat;
            this.radioButton1.Location = new Point(8, 12);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new Size(0x60, 0x18);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.Text = "P&ortrait";
            this.radioButton1.CheckedChanged += new EventHandler(this.radioButton1_CheckedChanged);
            this.bFirst.FlatStyle = FlatStyle.Flat;
            this.bFirst.Location = new Point(3, 270);
            this.bFirst.Name = "bFirst";
            this.bFirst.Size = new Size(0x1d, 0x17);
            this.bFirst.TabIndex = 4;
            this.bFirst.Text = "<<";
            this.bFirst.Click += new EventHandler(this.bFirst_Click);
            this.tabPage2.Controls.Add(this.lbResolution);
            this.tabPage2.Controls.Add(this.labelDetail);
            this.tabPage2.Controls.Add(this.tbResolution);
            this.tabPage2.Controls.Add(this.checkBox3);
            this.tabPage2.Controls.Add(this.checkBox2);
            this.tabPage2.Controls.Add(this.cbQuality);
            this.tabPage2.Controls.Add(this.cbPrintBackground);
            this.tabPage2.Location = new Point(4, 0x16);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new Size(120, 0x133);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Format";
            this.lbResolution.AutoSize = true;
            this.lbResolution.BackColor = Color.Transparent;
            this.lbResolution.Location = new Point(13, 0xc6);
            this.lbResolution.Name = "lbResolution";
            this.lbResolution.Size = new Size(10, 0x10);
            this.lbResolution.TabIndex = 6;
            this.lbResolution.Text = "1";
            this.lbResolution.UseMnemonic = false;
            this.labelDetail.AutoSize = true;
            this.labelDetail.Location = new Point(13, 140);
            this.labelDetail.Name = "labelDetail";
            this.labelDetail.Size = new Size(0x5d, 0x10);
            this.labelDetail.TabIndex = 4;
            this.labelDetail.Text = "&Detail Resolution:";
            this.tbResolution.AutoSize = false;
            this.tbResolution.LargeChange = 10;
            this.tbResolution.Location = new Point(5, 0xa4);
            this.tbResolution.Maximum = 100;
            this.tbResolution.Minimum = 1;
            this.tbResolution.Name = "tbResolution";
            this.tbResolution.SmallChange = 5;
            this.tbResolution.TabIndex = 5;
            this.tbResolution.TickFrequency = 10;
            this.tbResolution.Value = 1;
            this.tbResolution.ValueChanged += new EventHandler(this.tbResolution_Scroll);
            this.tbResolution.Scroll += new EventHandler(this.tbResolution_Scroll);
            this.checkBox3.FlatStyle = FlatStyle.Flat;
            this.checkBox3.Location = new Point(8, 0x68);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new Size(0x68, 0x10);
            this.checkBox3.TabIndex = 3;
            this.checkBox3.Text = "&Grayscale";
            this.checkBox3.CheckedChanged += new EventHandler(this.checkBox3_CheckedChanged);
            this.checkBox2.FlatStyle = FlatStyle.Flat;
            this.checkBox2.Location = new Point(8, 0x48);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.TabIndex = 2;
            this.checkBox2.Text = "P&roportional";
            this.checkBox2.CheckedChanged += new EventHandler(this.checkBox2_CheckedChanged);
            this.cbQuality.FlatStyle = FlatStyle.Flat;
            this.cbQuality.Location = new Point(8, 40);
            this.cbQuality.Name = "cbQuality";
            this.cbQuality.TabIndex = 1;
            this.cbQuality.Text = "&Quality";
            this.cbQuality.CheckedChanged += new EventHandler(this.cbQuality_CheckedChanged);
            this.cbPrintBackground.FlatStyle = FlatStyle.Flat;
            this.cbPrintBackground.Location = new Point(8, 8);
            this.cbPrintBackground.Name = "cbPrintBackground";
            this.cbPrintBackground.Size = new Size(120, 0x18);
            this.cbPrintBackground.TabIndex = 0;
            this.cbPrintBackground.Text = "Print &Background";
            this.cbPrintBackground.CheckedChanged += new EventHandler(this.cbPrintBackground_CheckedChanged);
            this.printPreviewControl1.AutoZoom = false;
            this.printPreviewControl1.Dock = DockStyle.Fill;
            this.printPreviewControl1.Document = this.printDocument1;
            this.printPreviewControl1.Location = new Point(0x80, 0x20);
            this.printPreviewControl1.Name = "printPreviewControl1";
            this.printPreviewControl1.Size = new Size(0x170, 0x14d);
            this.printPreviewControl1.TabIndex = 1;
            this.printPreviewControl1.Zoom = 0.28;
            this.printPreviewControl1.Click += new EventHandler(this.printPreviewControl1_Click);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this.button3;
            base.ClientSize = new Size(0x1f0, 0x16d);
            base.Controls.Add(this.printPreviewControl1);
            base.Controls.Add(this.panel2);
            base.Controls.Add(this.panel1);
            base.Name = "PrintPreview";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Print Preview";
            base.Closing += new CancelEventHandler(this.PrintPreview_Closing);
            base.Load += new EventHandler(this.PrintPreview_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupMargins.ResumeLayout(false);
            this.upDBottom.EndInit();
            this.upDRight.EndInit();
            this.upDLeft.EndInit();
            this.upDTop.EndInit();
            this.trackBar1.EndInit();
            this.groupBox1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tbResolution.EndInit();
            base.ResumeLayout(false);
        }

        private void PrintPreview_Closing(object sender, CancelEventArgs e)
        {
            this.prn.isPartial = false;
        }

        private void PrintPreview_Load(object sender, EventArgs e)
        {
            this.setting = true;
            if (this.prn != null)
            {
                this.prn.GetOptions();
                PrinterSettings.StringCollection installedPrinters = PrinterSettings.InstalledPrinters;
                for (int i = 0; i < installedPrinters.Count; i++)
                {
                    this.comboBox1.Items.Add(installedPrinters[i]);
                }
                this.comboBox1.SelectedIndex = this.comboBox1.Items.IndexOf(this.printDocument1.PrinterSettings.PrinterName);
                if (this.prn.Landscape)
                {
                    this.radioButton2.Checked = true;
                }
                else
                {
                    this.radioButton1.Checked = true;
                }
                this.cbQuality.Checked = this.prn.UseAntiAlias;
                this.checkBox2.Checked = this.prn.Proportional;
                this.checkBox3.Checked = this.prn.Grayscale;
                Steema.TeeChart.Margins margins = this.prn.Margins;
                this.setting = false;
                this.upDLeft.Value = margins.Left;
                this.upDTop.Value = margins.Top;
                this.upDRight.Value = margins.Right;
                this.upDBottom.Value = margins.Bottom;
                this.setting = true;
                this.CheckPageButtons();
                this.cbPrintBackground.Checked = this.prn.PrintPanelBackground;
                this.cbMarginUnits.SelectedIndex = (int) this.prn.MarginUnits;
                this.tbResolution.Value = this.prn.Resolution;
            }
            this.setting = false;
        }

        private void printPreviewControl1_Click(object sender, EventArgs e)
        {
            if (this.trackBar1.Value < 100)
            {
                this.trackBar1.Value *= 2;
            }
            else
            {
                this.trackBar1.Value /= 2;
            }
            this.trackBar1_Scroll(this.trackBar1, new EventArgs());
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.prn.PrintDocument.DefaultPageSettings.Landscape = false;
                this.prn.Landscape = false;
                this.RefreshView();
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            this.prn.PrintDocument.DefaultPageSettings.Landscape = true;
            this.prn.Landscape = true;
            this.RefreshView();
        }

        internal void RefreshView()
        {
            this.CheckPageButtons();
            this.printPreviewControl1.Document = this.printDocument1;
            this.printPreviewControl1.Refresh();
            this.printPreviewControl1.Invalidate();
        }

        public static void ShowModal(Chart c)
        {
            ShowModal(c, c.Printer);
        }

        public static void ShowModal(Chart c, Printer p)
        {
            using (PrintPreview preview = new PrintPreview(c, p))
            {
                EditorUtils.Translate(preview);
                preview.ShowDialog();
            }
        }

        private void tbResolution_Scroll(object sender, EventArgs e)
        {
            this.prn.Resolution = this.tbResolution.Value;
            this.lbResolution.Text = this.tbResolution.Value.ToString();
            this.RefreshView();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            double num = ((double) this.trackBar1.Value) / 100.0;
            if (num > 0.0)
            {
                this.printPreviewControl1.Zoom = num;
            }
            this.RefreshView();
        }

        private void upDBottom_TextChanged(object sender, EventArgs e)
        {
            this.upDBottom_ValueChanged(sender, e);
        }

        private void upDBottom_ValueChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.prn.Margins.Bottom = (int) this.upDBottom.Value;
                this.prn.PrintDocument.DefaultPageSettings.Margins.Bottom = (this.prn.MarginUnits == PrintMarginUnits.Percent) ? this.GetMarginVal((int) this.upDBottom.Value, false) : ((int) this.upDBottom.Value);
                this.RefreshView();
            }
        }

        private void upDLeft_TextChanged(object sender, EventArgs e)
        {
            this.upDLeft_ValueChanged(sender, e);
        }

        private void upDLeft_ValueChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.prn.Margins.Left = (int) this.upDLeft.Value;
                this.prn.PrintDocument.DefaultPageSettings.Margins.Left = (this.prn.MarginUnits == PrintMarginUnits.Percent) ? this.GetMarginVal((int) this.upDLeft.Value, true) : ((int) this.upDLeft.Value);
                this.RefreshView();
            }
        }

        private void upDRight_TextChanged(object sender, EventArgs e)
        {
            this.upDRight_ValueChanged(sender, e);
        }

        private void upDRight_ValueChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.prn.Margins.Right = (int) this.upDRight.Value;
                this.prn.PrintDocument.DefaultPageSettings.Margins.Right = (this.prn.MarginUnits == PrintMarginUnits.Percent) ? this.GetMarginVal((int) this.upDRight.Value, true) : ((int) this.upDRight.Value);
                this.RefreshView();
            }
        }

        private void upDTop_TextChanged(object sender, EventArgs e)
        {
            this.upDTop_ValueChanged(sender, e);
        }

        private void upDTop_ValueChanged(object sender, EventArgs e)
        {
            if (!this.setting)
            {
                this.prn.Margins.Top = (int) this.upDTop.Value;
                this.prn.PrintDocument.DefaultPageSettings.Margins.Top = (this.prn.MarginUnits == PrintMarginUnits.Percent) ? this.GetMarginVal((int) this.upDTop.Value, false) : ((int) this.upDTop.Value);
                this.RefreshView();
            }
        }
    }
}

