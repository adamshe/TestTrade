namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    public class ChartGallery : Form
    {
        private Button bCancel;
        private Button bOk;
        private Chart chart;
        private CheckBox checkBox1;
        private CheckBox checkBox2;
        private Container components;
        private GalleryPanel gallery;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private TabControl tabFuncs;
        private TabPage tabFunctions;
        private TabPage tabPage1;
        private TabControl tabPages;
        private TabPage tabSeries;
        private TabPage tabStandard;
        private TabControl tabTypes;

        public ChartGallery()
        {
            this.components = null;
            this.gallery = new GalleryPanel();
            this.InitializeComponent();
        }

        public ChartGallery(Chart c) : this()
        {
            this.chart = c;
        }

        private void AddTabPages(TabControl tab)
        {
            string str;
            tab.TabPages.Clear();
            if (this.tabTypes.SelectedIndex == 0)
            {
                for (int i = 0; i < 0x2c; i++)
                {
                    if (Utils.SeriesGalleryCount[i] > 0)
                    {
                        str = GalleryPanel.GalleryPages(Utils.SeriesGalleryPage[i]);
                        if (this.TabIndexOf(this.tabPages, str) == -1)
                        {
                            tab.TabPages.Add(new TabPage(str));
                        }
                    }
                }
            }
            else
            {
                for (int j = 0; j < 0x1b; j++)
                {
                    str = GalleryPanel.GalleryPages(Utils.FunctionGalleryPage[j]);
                    if (this.TabIndexOf(this.tabFuncs, str) == -1)
                    {
                        tab.TabPages.Add(new TabPage(str));
                    }
                }
            }
        }

        private void ChangedChart(object sender, EventArgs e)
        {
            this.bOk.Enabled = true;
        }

        public static System.Type ChangeSeriesType(Chart c, ref Series s)
        {
            Series newSeries = CreateNew(c, s);
            if (newSeries != null)
            {
                Series.AssignDispose(ref s, newSeries);
                return s.GetType();
            }
            return null;
        }

        private void ChartGallery_Load(object sender, EventArgs e)
        {
            this.gallery.OnSelectedChart += new EventHandler(this.SelectedChart);
            this.gallery.OnChangeChart += new EventHandler(this.ChangedChart);
            this.tabTypes.SelectedIndex = 0;
            this.tabTypes_SelectedIndexChanged(this, EventArgs.Empty);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.gallery.View3D = this.checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            this.gallery.Smooth = this.checkBox2.Checked ? SmoothingMode.HighQuality : SmoothingMode.HighSpeed;
        }

        private void CheckStandard(TabControl tab)
        {
            int num = this.TabIndexOf(tab, Texts.GalleryStandard);
            if (num > 0)
            {
                TabPage page = tab.TabPages[0];
                tab.TabPages[0] = tab.TabPages[num];
                tab.TabPages[num] = page;
            }
        }

        public static Series CreateNew(Chart c, Series old)
        {
            System.Type type = null;
            System.Type aFunction = null;
            int subIndex = 0;
            if (ShowModal(c, ref type, ref aFunction, ref subIndex) == DialogResult.OK)
            {
                return Series.CreateNewSeries(c, type, aFunction, subIndex);
            }
            return null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.checkBox1 = new CheckBox();
            this.tabSeries = new TabPage();
            this.tabPages = new TabControl();
            this.tabStandard = new TabPage();
            this.tabFunctions = new TabPage();
            this.tabFuncs = new TabControl();
            this.tabPage1 = new TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.checkBox2 = new CheckBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.bCancel = new Button();
            this.bOk = new Button();
            this.tabTypes = new TabControl();
            this.tabSeries.SuspendLayout();
            this.tabPages.SuspendLayout();
            this.tabFunctions.SuspendLayout();
            this.tabFuncs.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabTypes.SuspendLayout();
            base.SuspendLayout();
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = CheckState.Checked;
            this.checkBox1.FlatStyle = FlatStyle.Flat;
            this.checkBox1.Location = new Point(8, 8);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "View &3D";
            this.checkBox1.CheckedChanged += new EventHandler(this.checkBox1_CheckedChanged);
            this.tabSeries.Controls.Add(this.tabPages);
            this.tabSeries.Location = new Point(4, 0x16);
            this.tabSeries.Name = "tabSeries";
            this.tabSeries.Size = new Size(0x199, 0x121);
            this.tabSeries.TabIndex = 0;
            this.tabSeries.Text = "Series";
            this.tabPages.Controls.Add(this.tabStandard);
            this.tabPages.Dock = DockStyle.Fill;
            this.tabPages.HotTrack = true;
            this.tabPages.Location = new Point(0, 0);
            this.tabPages.Name = "tabPages";
            this.tabPages.SelectedIndex = 0;
            this.tabPages.Size = new Size(0x199, 0x121);
            this.tabPages.TabIndex = 0;
            this.tabPages.SelectedIndexChanged += new EventHandler(this.tabControl2_SelectedIndexChanged);
            this.tabStandard.Location = new Point(4, 0x16);
            this.tabStandard.Name = "tabStandard";
            this.tabStandard.Size = new Size(0x191, 0x107);
            this.tabStandard.TabIndex = 0;
            this.tabStandard.Text = "Standard";
            this.tabFunctions.Controls.Add(this.tabFuncs);
            this.tabFunctions.Location = new Point(4, 0x16);
            this.tabFunctions.Name = "tabFunctions";
            this.tabFunctions.Size = new Size(0x199, 0x121);
            this.tabFunctions.TabIndex = 1;
            this.tabFunctions.Text = "Functions";
            this.tabFuncs.Controls.Add(this.tabPage1);
            this.tabFuncs.Dock = DockStyle.Fill;
            this.tabFuncs.HotTrack = true;
            this.tabFuncs.Location = new Point(0, 0);
            this.tabFuncs.Name = "tabFuncs";
            this.tabFuncs.SelectedIndex = 0;
            this.tabFuncs.Size = new Size(0x199, 0x121);
            this.tabFuncs.TabIndex = 0;
            this.tabFuncs.SelectedIndexChanged += new EventHandler(this.tabFuncs_SelectedIndexChanged);
            this.tabPage1.Location = new Point(4, 0x16);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new Size(0x191, 0x107);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Standard";
            this.panel1.Controls.Add(this.checkBox2);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.checkBox1);
            this.panel1.Dock = DockStyle.Bottom;
            this.panel1.Location = new Point(0, 0x13b);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x1a1, 40);
            this.panel1.TabIndex = 1;
            this.checkBox2.FlatStyle = FlatStyle.Flat;
            this.checkBox2.Location = new Point(0x70, 8);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.TabIndex = 1;
            this.checkBox2.Text = "&Smooth";
            this.checkBox2.CheckedChanged += new EventHandler(this.checkBox2_CheckedChanged);
            this.panel2.Controls.Add(this.bCancel);
            this.panel2.Controls.Add(this.bOk);
            this.panel2.Dock = DockStyle.Right;
            this.panel2.Location = new Point(0xf1, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new Size(0xb0, 40);
            this.panel2.TabIndex = 2;
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
            this.tabTypes.Controls.Add(this.tabSeries);
            this.tabTypes.Controls.Add(this.tabFunctions);
            this.tabTypes.Dock = DockStyle.Fill;
            this.tabTypes.HotTrack = true;
            this.tabTypes.Location = new Point(0, 0);
            this.tabTypes.Name = "tabTypes";
            this.tabTypes.SelectedIndex = 0;
            this.tabTypes.Size = new Size(0x1a1, 0x13b);
            this.tabTypes.TabIndex = 0;
            this.tabTypes.SelectedIndexChanged += new EventHandler(this.tabTypes_SelectedIndexChanged);
            base.AcceptButton = this.bOk;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this.bCancel;
            base.ClientSize = new Size(0x1a1, 0x163);
            base.Controls.Add(this.tabTypes);
            base.Controls.Add(this.panel1);
            base.Name = "ChartGallery";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "TeeChart Gallery";
            base.Load += new EventHandler(this.ChartGallery_Load);
            this.tabSeries.ResumeLayout(false);
            this.tabPages.ResumeLayout(false);
            this.tabFunctions.ResumeLayout(false);
            this.tabFuncs.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.tabTypes.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void SelectedChart(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.OK;
        }

        public static DialogResult ShowModal(Chart c, ref System.Type type, ref System.Type aFunction, ref int subIndex)
        {
            Form form;
            DialogResult result2;
            bool flag = DesignTimeOptions.GalleryByType();
            subIndex = 0;
            if (!flag)
            {
                form = new ChartGallery(c);
            }
            else
            {
                form = new GalleryByType();
            }
            try
            {
                EditorUtils.Translate(form);
                DialogResult result = form.ShowDialog(c.parent.GetControl());
                if (result == DialogResult.OK)
                {
                    if (flag)
                    {
                        type = ((GalleryByType) form).SeriesType;
                        aFunction = null;
                        subIndex = ((GalleryByType) form).SubIndex;
                    }
                    else
                    {
                        type = ((ChartGallery) form).SeriesType;
                        aFunction = ((ChartGallery) form).FunctionType;
                        subIndex = ((ChartGallery) form).gallery.SubIndex;
                    }
                }
                result2 = result;
            }
            finally
            {
                form.Dispose();
            }
            return result2;
        }

        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.gallery.FunctionsVisible = false;
            this.gallery.View3D = this.checkBox1.Checked;
            if (this.tabPages.SelectedTab != null)
            {
                this.gallery.CreateGalleryPage(this.tabPages.SelectedTab.Text, false);
                this.gallery.Parent = this.tabPages.SelectedTab;
                this.gallery.Dock = DockStyle.Fill;
                this.gallery.Show();
                this.gallery.Focus();
            }
        }

        private void tabFuncs_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.gallery.FunctionsVisible = true;
            this.gallery.View3D = this.checkBox1.Checked;
            if (this.tabFuncs.SelectedTab != null)
            {
                this.gallery.CreateGalleryPage(this.tabFuncs.SelectedTab.Text, true);
                this.gallery.Parent = this.tabFuncs.SelectedTab;
                this.gallery.Dock = DockStyle.Fill;
                this.gallery.Show();
            }
        }

        private int TabIndexOf(TabControl pages, string GalleryPage)
        {
            for (int i = 0; i < pages.TabCount; i++)
            {
                if (pages.TabPages[i].Text == GalleryPage)
                {
                    return i;
                }
            }
            return -1;
        }

        private void tabTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabControl tab = (this.tabTypes.SelectedIndex == 0) ? this.tabPages : this.tabFuncs;
            this.AddTabPages(tab);
            this.CheckStandard(tab);
            tab.SelectedIndex = -1;
            tab.SelectedIndex = 0;
        }

        public System.Type FunctionType
        {
            get
            {
                return this.gallery.FunctionType;
            }
        }

        public System.Type SeriesType
        {
            get
            {
                return this.gallery.SeriesType;
            }
        }

        public int SubIndex
        {
            get
            {
                return this.gallery.SubIndex;
            }
        }
    }
}

