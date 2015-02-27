namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class ColorGridSeries : Grid3DSeries
    {
        private Button bGrid;
        private CheckBox cbCentered;
        private ColorGrid colorGridSeries;
        private Container components;

        public ColorGridSeries()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public ColorGridSeries(Series s) : this()
        {
            this.colorGridSeries = (ColorGrid) s;
            base.series = this.colorGridSeries;
            base.SetProperties();
            this.cbCentered.Checked = this.colorGridSeries.CenteredPoints;
        }

        private void bGrid_Click(object sender, EventArgs e)
        {
            PenEditor.Edit(this.colorGridSeries.Pen);
        }

        private void cbCentered_CheckedChanged(object sender, EventArgs e)
        {
            this.colorGridSeries.CenteredPoints = this.cbCentered.Checked;
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
            this.bGrid = new Button();
            this.cbCentered = new CheckBox();
            base.SuspendLayout();
            this.bGrid.FlatStyle = FlatStyle.Flat;
            this.bGrid.Location = new Point(8, 0x85);
            this.bGrid.Name = "bGrid";
            this.bGrid.TabIndex = 6;
            this.bGrid.Text = "&Grid...";
            this.bGrid.Click += new EventHandler(this.bGrid_Click);
            this.cbCentered.FlatStyle = FlatStyle.Flat;
            this.cbCentered.Location = new Point(0x103, 120);
            this.cbCentered.Name = "cbCentered";
            this.cbCentered.Size = new Size(0x75, 0x18);
            this.cbCentered.TabIndex = 7;
            this.cbCentered.Text = "&Centered";
            this.cbCentered.CheckedChanged += new EventHandler(this.cbCentered_CheckedChanged);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x178, 160);
            base.Controls.Add(this.cbCentered);
            base.Controls.Add(this.bGrid);
            base.Name = "ColorGridSeries";
            this.Text = "ColorGridEdit";
            base.Controls.SetChildIndex(this.bGrid, 0);
            base.Controls.SetChildIndex(this.cbCentered, 0);
            base.ResumeLayout(false);
        }
    }
}

