namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    public class GalleryByType : Form
    {
        private Button bCancel;
        private Button bOk;
        private TChart c = new TChart();
        private ChartListBox chartListBox1;
        private CheckBox checkBox1;
        private CheckBox checkBox2;
        private ComboBox comboBox1;
        private IContainer components = null;
        private GalleryPanel galleryPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel6;
        private Splitter splitter1;

        public GalleryByType()
        {
            this.InitializeComponent();
        }

        private void chartListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Series selectedSeries = this.chartListBox1.SelectedSeries;
            this.galleryPanel1.AddSubCharts(selectedSeries);
            this.bOk.Enabled = false;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.galleryPanel1.View3D = this.checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox2.Checked)
            {
                this.galleryPanel1.Smooth = SmoothingMode.HighQuality;
            }
            else
            {
                this.galleryPanel1.Smooth = SmoothingMode.HighSpeed;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.c.Series.Clear();
            for (int i = 0; i < 0x2c; i++)
            {
                if (Utils.SeriesGalleryCount[i] > 0)
                {
                    string str = GalleryPanel.GalleryPages(Utils.SeriesGalleryPage[i]);
                    if (this.comboBox1.SelectedItem.ToString() == str)
                    {
                        Series series = this.c.Series.Add(Utils.SeriesTypesOf[i]);
                        series.Title = series.Description;
                    }
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void GalleryByType_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < 0x2c; i++)
            {
                if (Utils.SeriesGalleryCount[i] > 0)
                {
                    string str = GalleryPanel.GalleryPages(Utils.SeriesGalleryPage[i]);
                    if (this.comboBox1.Items.IndexOf(str) == -1)
                    {
                        this.comboBox1.Items.Add(str);
                    }
                }
            }
            this.comboBox1.SelectedIndex = 0;
            this.chartListBox1.Chart = this.c;
        }

        private void GalleryByType_SizeChanged(object sender, EventArgs e)
        {
            this.galleryPanel1.Size = new Size(this.panel6.Width, this.panel6.Height - this.panel2.Height);
        }

        private void galleryPanel1_OnChangeChart(object sender, EventArgs e)
        {
            this.bOk.Enabled = true;
        }

        private void galleryPanel1_OnSelectedChart(object sender, EventArgs e)
        {
            this.galleryPanel1.SubSelected(sender, e);
            base.DialogResult = DialogResult.OK;
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.panel3 = new System.Windows.Forms.Panel();
            this.chartListBox1 = new ChartListBox(this.components);
            this.panel4 = new System.Windows.Forms.Panel();
            this.comboBox1 = new ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.galleryPanel1 = new GalleryPanel();
            this.splitter1 = new Splitter();
            this.panel2 = new System.Windows.Forms.Panel();
            this.checkBox2 = new CheckBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.bCancel = new Button();
            this.bOk = new Button();
            this.checkBox1 = new CheckBox();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel5.SuspendLayout();
            base.SuspendLayout();
            this.panel3.Controls.Add(this.chartListBox1);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Dock = DockStyle.Left;
            this.panel3.Location = new Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new Size(0x88, 0x163);
            this.panel3.TabIndex = 2;
            this.chartListBox1.Dock = DockStyle.Fill;
            this.chartListBox1.IntegralHeight = false;
            this.chartListBox1.Location = new Point(0, 40);
            this.chartListBox1.Name = "chartListBox1";
            this.chartListBox1.ShowActiveCheck = false;
            this.chartListBox1.ShowSeriesColor = false;
            this.chartListBox1.Size = new Size(0x88, 0x13b);
            this.chartListBox1.TabIndex = 1;
            this.chartListBox1.SelectedIndexChanged += new EventHandler(this.chartListBox1_SelectedIndexChanged);
            this.panel4.Controls.Add(this.comboBox1);
            this.panel4.Dock = DockStyle.Top;
            this.panel4.Location = new Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new Size(0x88, 40);
            this.panel4.TabIndex = 0;
            this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBox1.Location = new Point(8, 8);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new Size(0x79, 0x15);
            this.comboBox1.TabIndex = 0;
            this.comboBox1.SelectedIndexChanged += new EventHandler(this.comboBox1_SelectedIndexChanged);
            this.panel1.Controls.Add(this.panel6);
            this.panel1.Dock = DockStyle.Fill;
            this.panel1.Location = new Point(0x88, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x188, 0x163);
            this.panel1.TabIndex = 3;
            this.panel6.Controls.Add(this.galleryPanel1);
            this.panel6.Dock = DockStyle.Fill;
            this.panel6.Location = new Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new Size(0x188, 0x163);
            this.panel6.TabIndex = 0;
            this.galleryPanel1.DisplaySub = false;
            this.galleryPanel1.Location = new Point(0, 0);
            this.galleryPanel1.Name = "galleryPanel1";
            this.galleryPanel1.Size = new Size(0x188, 0x120);
            this.galleryPanel1.TabIndex = 0;
            this.galleryPanel1.OnSelectedChart += new EventHandler(this.galleryPanel1_OnSelectedChart);
            this.galleryPanel1.OnChangeChart += new EventHandler(this.galleryPanel1_OnChangeChart);
            this.splitter1.Location = new Point(0x88, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new Size(3, 0x163);
            this.splitter1.TabIndex = 4;
            this.splitter1.TabStop = false;
            this.panel2.Controls.Add(this.checkBox2);
            this.panel2.Controls.Add(this.panel5);
            this.panel2.Controls.Add(this.checkBox1);
            this.panel2.Dock = DockStyle.Bottom;
            this.panel2.Location = new Point(0x8b, 0x13b);
            this.panel2.Name = "panel2";
            this.panel2.Size = new Size(0x185, 40);
            this.panel2.TabIndex = 5;
            this.checkBox2.FlatStyle = FlatStyle.Flat;
            this.checkBox2.Location = new Point(0x70, 8);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new Size(0x58, 0x18);
            this.checkBox2.TabIndex = 1;
            this.checkBox2.Text = "&Smooth";
            this.checkBox2.CheckedChanged += new EventHandler(this.checkBox2_CheckedChanged);
            this.panel5.Controls.Add(this.bCancel);
            this.panel5.Controls.Add(this.bOk);
            this.panel5.Dock = DockStyle.Right;
            this.panel5.Location = new Point(0xd5, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new Size(0xb0, 40);
            this.panel5.TabIndex = 2;
            this.bCancel.DialogResult = DialogResult.Cancel;
            this.bCancel.FlatStyle = FlatStyle.Flat;
            this.bCancel.Location = new Point(0x60, 8);
            this.bCancel.Name = "bCancel";
            this.bCancel.TabIndex = 1;
            this.bCancel.Text = "Cancel";
            this.bOk.DialogResult = DialogResult.OK;
            this.bOk.Enabled = false;
            this.bOk.FlatStyle = FlatStyle.Flat;
            this.bOk.Location = new Point(8, 8);
            this.bOk.Name = "bOk";
            this.bOk.TabIndex = 0;
            this.bOk.Text = "OK";
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = CheckState.Checked;
            this.checkBox1.FlatStyle = FlatStyle.Flat;
            this.checkBox1.Location = new Point(8, 8);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "View &3D";
            this.checkBox1.CheckedChanged += new EventHandler(this.checkBox1_CheckedChanged);
            base.AcceptButton = this.bOk;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this.bCancel;
            base.ClientSize = new Size(0x210, 0x163);
            base.Controls.Add(this.panel2);
            base.Controls.Add(this.splitter1);
            base.Controls.Add(this.panel1);
            base.Controls.Add(this.panel3);
            base.Name = "GalleryByType";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "TeeChart Gallery";
            base.SizeChanged += new EventHandler(this.GalleryByType_SizeChanged);
            base.Load += new EventHandler(this.GalleryByType_Load);
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        public System.Type SeriesType
        {
            get
            {
                return this.galleryPanel1.SeriesType;
            }
        }

        public int SubIndex
        {
            get
            {
                return this.galleryPanel1.SubIndex;
            }
        }
    }
}

