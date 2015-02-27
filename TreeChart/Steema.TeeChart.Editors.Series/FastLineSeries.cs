namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class FastLineSeries : PenEditor
    {
        private CheckBox CBDrawAll;
        private CheckBox cbInvStairs;
        private CheckBox cbNulls;
        private CheckBox cbStairs;
        private CheckBox checkBox1;
        private IContainer components;
        private GroupBox groupBox1;
        private FastLine series;

        public FastLineSeries()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public FastLineSeries(Series s) : this()
        {
            this.series = (FastLine) s;
            base.SetPen(this.series.LinePen);
            this.CBDrawAll.Checked = this.series.DrawAllPoints;
            this.checkBox1.Checked = this.series.ColorEach;
            this.cbStairs.Checked = this.series.Stairs;
            this.cbInvStairs.Checked = this.series.InvertedStairs;
            this.cbInvStairs.Enabled = this.cbStairs.Checked;
            this.cbNulls.Checked = this.series.IgnoreNulls;
            base.OkButton.Visible = false;
            base.VisibleCheckBox.Visible = false;
        }

        private void CBDrawAll_CheckedChanged(object sender, EventArgs e)
        {
            this.series.DrawAllPoints = this.CBDrawAll.Checked;
        }

        private void cbIgnoreNulls_CheckedChanged(object sender, EventArgs e)
        {
            this.series.IgnoreNulls = this.cbNulls.Checked;
        }

        private void cbInvStairs_CheckedChanged(object sender, EventArgs e)
        {
            this.series.InvertedStairs = this.cbInvStairs.Checked;
        }

        private void cbStairs_CheckedChanged(object sender, EventArgs e)
        {
            this.series.Stairs = this.cbStairs.Checked;
            this.cbInvStairs.Enabled = this.cbStairs.Checked;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.series.ColorEach = this.checkBox1.Checked;
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
            this.CBDrawAll = new CheckBox();
            this.checkBox1 = new CheckBox();
            this.groupBox1 = new GroupBox();
            this.cbInvStairs = new CheckBox();
            this.cbStairs = new CheckBox();
            this.cbNulls = new CheckBox();
            this.groupBox1.SuspendLayout();
            base.SuspendLayout();
            this.CBDrawAll.FlatStyle = FlatStyle.Flat;
            this.CBDrawAll.Location = new Point(13, 0x9f);
            this.CBDrawAll.Name = "CBDrawAll";
            this.CBDrawAll.Size = new Size(0x67, 0x12);
            this.CBDrawAll.TabIndex = 9;
            this.CBDrawAll.Text = "Draw &All";
            this.CBDrawAll.CheckedChanged += new EventHandler(this.CBDrawAll_CheckedChanged);
            this.checkBox1.FlatStyle = FlatStyle.Flat;
            this.checkBox1.Location = new Point(0xbb, 0x84);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new Size(110, 0x17);
            this.checkBox1.TabIndex = 13;
            this.checkBox1.Text = "Color &Each";
            this.checkBox1.CheckedChanged += new EventHandler(this.checkBox1_CheckedChanged);
            this.groupBox1.Controls.Add(this.cbInvStairs);
            this.groupBox1.Controls.Add(this.cbStairs);
            this.groupBox1.Location = new Point(8, 0x70);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x98, 40);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Line mode:";
            this.cbInvStairs.FlatStyle = FlatStyle.Flat;
            this.cbInvStairs.Location = new Point(0x4a, 0x10);
            this.cbInvStairs.Name = "cbInvStairs";
            this.cbInvStairs.Size = new Size(70, 0x10);
            this.cbInvStairs.TabIndex = 1;
            this.cbInvStairs.Text = "&Inverted";
            this.cbInvStairs.CheckedChanged += new EventHandler(this.cbInvStairs_CheckedChanged);
            this.cbStairs.FlatStyle = FlatStyle.Flat;
            this.cbStairs.Location = new Point(8, 0x10);
            this.cbStairs.Name = "cbStairs";
            this.cbStairs.Size = new Size(0x40, 0x10);
            this.cbStairs.TabIndex = 0;
            this.cbStairs.Text = "&Stairs";
            this.cbStairs.CheckedChanged += new EventHandler(this.cbStairs_CheckedChanged);
            this.cbNulls.FlatStyle = FlatStyle.Flat;
            this.cbNulls.Location = new Point(0x80, 160);
            this.cbNulls.Name = "cbNulls";
            this.cbNulls.Size = new Size(0x68, 0x10);
            this.cbNulls.TabIndex = 15;
            this.cbNulls.Text = "Ignore &Nulls";
            this.cbNulls.CheckedChanged += new EventHandler(this.cbIgnoreNulls_CheckedChanged);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x135, 0xba);
            base.Controls.Add(this.cbNulls);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.checkBox1);
            base.Controls.Add(this.CBDrawAll);
            base.Name = "FastLineSeries";
            base.Controls.SetChildIndex(this.CBDrawAll, 0);
            base.Controls.SetChildIndex(this.checkBox1, 0);
            base.Controls.SetChildIndex(this.groupBox1, 0);
            base.Controls.SetChildIndex(this.cbNulls, 0);
            this.groupBox1.ResumeLayout(false);
            base.ResumeLayout(false);
        }
    }
}

