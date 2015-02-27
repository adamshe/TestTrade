namespace Steema.TeeChart.Editors.Tools
{
    using Steema.TeeChart.Styles;
    using Steema.TeeChart.Tools;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class GridTransposeEditor : Form
    {
        private Button bTranspose;
        private ComboBox cbSeries;
        private Container components;
        private Label label1;
        private GridTranspose tool;

        public GridTransposeEditor()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public GridTransposeEditor(Steema.TeeChart.Tools.Tool s) : this()
        {
            this.tool = (GridTranspose) s;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.tool.Transpose();
        }

        private void cbSeries_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.tool.Series = (Custom3DGrid) this.cbSeries.SelectedItem;
            this.bTranspose.Enabled = true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void GridTransposeEditor_Load(object sender, EventArgs e)
        {
            if ((this.tool != null) && (this.tool.Chart != null))
            {
                this.cbSeries.BeginUpdate();
                this.cbSeries.Items.Clear();
                foreach (Series series in this.tool.Chart.Series)
                {
                    if (series is Custom3DGrid)
                    {
                        this.cbSeries.Items.Add(series);
                    }
                }
                this.cbSeries.EndUpdate();
                if (this.tool.Series != null)
                {
                    this.cbSeries.SelectedItem = this.tool.Series;
                }
                else if (this.cbSeries.Items.Count > 0)
                {
                    this.cbSeries.SelectedIndex = 0;
                }
                this.bTranspose.Enabled = this.cbSeries.SelectedItem != null;
            }
        }

        private void InitializeComponent()
        {
            this.label1 = new Label();
            this.cbSeries = new ComboBox();
            this.bTranspose = new Button();
            base.SuspendLayout();
            this.label1.AutoSize = true;
            this.label1.Location = new Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x51, 0x10);
            this.label1.TabIndex = 0;
            this.label1.Text = "Grid 3D &Series:";
            this.cbSeries.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbSeries.Location = new Point(8, 0x20);
            this.cbSeries.Name = "cbSeries";
            this.cbSeries.Size = new Size(0x79, 0x15);
            this.cbSeries.TabIndex = 1;
            this.cbSeries.SelectedIndexChanged += new EventHandler(this.cbSeries_SelectedIndexChanged);
            this.bTranspose.FlatStyle = FlatStyle.Flat;
            this.bTranspose.Location = new Point(8, 0x48);
            this.bTranspose.Name = "bTranspose";
            this.bTranspose.Size = new Size(120, 0x17);
            this.bTranspose.TabIndex = 2;
            this.bTranspose.Text = "&Transpose now";
            this.bTranspose.Click += new EventHandler(this.button1_Click);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(160, 0x7d);
            base.Controls.Add(this.bTranspose);
            base.Controls.Add(this.cbSeries);
            base.Controls.Add(this.label1);
            base.Name = "GridTransposeEditor";
            this.Text = "GridTransposeEditor";
            base.Load += new EventHandler(this.GridTransposeEditor_Load);
            base.ResumeLayout(false);
        }
    }
}

