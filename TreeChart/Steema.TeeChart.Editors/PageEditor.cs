namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart;
    using Steema.TeeChart.Tools;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class PageEditor : Form
    {
        private Button bFirst;
        private Button bLast;
        private Button bNext;
        private Button bPrevious;
        private CheckBox checkBox4;
        private CheckBox checkBox5;
        private CheckBox checkBox6;
        private Container components;
        private Label label2;
        private Label labelPages;
        private NumericUpDown numericUpDown1;
        private Page page;

        public PageEditor()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public PageEditor(Page p, Control parent) : this()
        {
            this.page = p;
            this.checkBox4.Checked = this.page.ScaleLastPage;
            this.checkBox5.Checked = this.page.chart.Legend.CurrentPage;
            this.numericUpDown1.Value = this.page.MaxPointsPerPage;
            this.SetLabelPages();
            Steema.TeeChart.Tools.Tool tool = this.PageNumTool(false);
            this.checkBox6.Checked = (tool != null) && tool.Active;
            EditorUtils.InsertForm(this, parent);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.page.Current = 1;
            this.SetLabelPages();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.page.Previous();
            this.SetLabelPages();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.page.Next();
            this.SetLabelPages();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.page.Current = this.page.Count;
            this.SetLabelPages();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            this.page.ScaleLastPage = this.checkBox4.Checked;
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            this.page.chart.Legend.CurrentPage = this.checkBox5.Checked;
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            this.PageNumTool(true).Active = this.checkBox6.Checked;
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
            this.label2 = new Label();
            this.numericUpDown1 = new NumericUpDown();
            this.checkBox4 = new CheckBox();
            this.checkBox5 = new CheckBox();
            this.labelPages = new Label();
            this.checkBox6 = new CheckBox();
            this.bFirst = new Button();
            this.bPrevious = new Button();
            this.bNext = new Button();
            this.bLast = new Button();
            this.numericUpDown1.BeginInit();
            base.SuspendLayout();
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x20, 0x12);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x58, 0x10);
            this.label2.TabIndex = 0;
            this.label2.Text = "&Points per Page:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.numericUpDown1.BorderStyle = BorderStyle.FixedSingle;
            this.numericUpDown1.Location = new Point(120, 0x10);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new Size(0x45, 20);
            this.numericUpDown1.TabIndex = 1;
            this.numericUpDown1.TextAlign = HorizontalAlignment.Right;
            this.numericUpDown1.ValueChanged += new EventHandler(this.numericUpDown1_ValueChanged);
            this.checkBox4.FlatStyle = FlatStyle.Flat;
            this.checkBox4.Location = new Point(0x20, 0x48);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new Size(0xf1, 0x18);
            this.checkBox4.TabIndex = 3;
            this.checkBox4.Text = "Scale &Last Page";
            this.checkBox4.CheckedChanged += new EventHandler(this.checkBox4_CheckedChanged);
            this.checkBox5.Checked = true;
            this.checkBox5.CheckState = CheckState.Checked;
            this.checkBox5.FlatStyle = FlatStyle.Flat;
            this.checkBox5.Location = new Point(0x20, 0x60);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new Size(0xf1, 0x18);
            this.checkBox5.TabIndex = 4;
            this.checkBox5.Text = "&Current Page Legend";
            this.checkBox5.CheckedChanged += new EventHandler(this.checkBox5_CheckedChanged);
            this.labelPages.AutoSize = true;
            this.labelPages.Location = new Point(120, 0x30);
            this.labelPages.Name = "labelPages";
            this.labelPages.Size = new Size(0x3e, 0x10);
            this.labelPages.TabIndex = 2;
            this.labelPages.Text = "Page 1 of 1";
            this.checkBox6.FlatStyle = FlatStyle.Flat;
            this.checkBox6.Location = new Point(0x20, 120);
            this.checkBox6.Name = "checkBox6";
            this.checkBox6.Size = new Size(0xf1, 0x18);
            this.checkBox6.TabIndex = 5;
            this.checkBox6.Text = "&Show Page Number";
            this.checkBox6.CheckedChanged += new EventHandler(this.checkBox6_CheckedChanged);
            this.bFirst.FlatStyle = FlatStyle.Flat;
            this.bFirst.Location = new Point(0x30, 0x98);
            this.bFirst.Name = "bFirst";
            this.bFirst.Size = new Size(40, 0x17);
            this.bFirst.TabIndex = 6;
            this.bFirst.Text = "<<";
            this.bFirst.Click += new EventHandler(this.button1_Click);
            this.bPrevious.FlatStyle = FlatStyle.Flat;
            this.bPrevious.Location = new Point(0x60, 0x98);
            this.bPrevious.Name = "bPrevious";
            this.bPrevious.Size = new Size(40, 0x17);
            this.bPrevious.TabIndex = 7;
            this.bPrevious.Text = "<";
            this.bPrevious.Click += new EventHandler(this.button2_Click);
            this.bNext.FlatStyle = FlatStyle.Flat;
            this.bNext.Location = new Point(0x98, 0x98);
            this.bNext.Name = "bNext";
            this.bNext.Size = new Size(40, 0x17);
            this.bNext.TabIndex = 8;
            this.bNext.Text = ">";
            this.bNext.Click += new EventHandler(this.button3_Click);
            this.bLast.FlatStyle = FlatStyle.Flat;
            this.bLast.Location = new Point(200, 0x98);
            this.bLast.Name = "bLast";
            this.bLast.Size = new Size(40, 0x17);
            this.bLast.TabIndex = 9;
            this.bLast.Text = ">>";
            this.bLast.Click += new EventHandler(this.button4_Click);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x130, 0xbd);
            base.Controls.Add(this.bLast);
            base.Controls.Add(this.bNext);
            base.Controls.Add(this.bPrevious);
            base.Controls.Add(this.bFirst);
            base.Controls.Add(this.checkBox6);
            base.Controls.Add(this.labelPages);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.checkBox5);
            base.Controls.Add(this.checkBox4);
            base.Controls.Add(this.numericUpDown1);
            base.Name = "PageEditor";
            this.Text = "Page Editor";
            this.numericUpDown1.EndInit();
            base.ResumeLayout(false);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            this.page.MaxPointsPerPage = (int) this.numericUpDown1.Value;
            this.SetLabelPages();
        }

        private Steema.TeeChart.Tools.Tool PageNumTool(bool createTool)
        {
            PageNumber number;
            foreach (Steema.TeeChart.Tools.Tool tool in this.page.Chart.Tools)
            {
                if (tool is PageNumber)
                {
                    return tool;
                }
            }
            if (!createTool)
            {
                return null;
            }
            this.page.Chart.Tools.Add(number = new PageNumber());
            if (this.page.chart.ChartContainer != null)
            {
                this.page.chart.ChartContainer.Add(number);
            }
            return number;
        }

        private void SetLabelPages()
        {
            this.labelPages.Text = string.Format(Texts.PageOfPages, this.page.Current, this.page.Count);
            this.bFirst.Enabled = (this.page.Count > 0) && (this.page.Current > 1);
            this.bPrevious.Enabled = this.bFirst.Enabled;
            this.bNext.Enabled = (this.page.Count > 0) && (this.page.Current < this.page.Count);
            this.bLast.Enabled = this.bNext.Enabled;
        }
    }
}

