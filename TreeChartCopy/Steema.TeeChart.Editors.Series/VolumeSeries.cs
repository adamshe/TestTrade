namespace Steema.TeeChart.Editors.Series
{
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class VolumeSeries : PenEditor
    {
        private CheckBox CBColorEach;
        private CheckBox CBUseOrigin;
        private IContainer components;
        private Volume series;

        public VolumeSeries()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public VolumeSeries(Series s) : this()
        {
            this.series = (Volume) s;
            this.CBUseOrigin.Checked = this.series.UseOrigin;
            this.CBColorEach.Checked = this.series.ColorEach;
            base.SetPen(this.series.LinePen);
            base.OkButton.Visible = false;
        }

        private void CBColorEach_CheckedChanged(object sender, EventArgs e)
        {
            this.series.ColorEach = this.CBColorEach.Checked;
        }

        private void CBUseOrigin_CheckedChanged(object sender, EventArgs e)
        {
            this.series.UseOrigin = this.CBUseOrigin.Checked;
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
            this.CBColorEach = new CheckBox();
            this.CBUseOrigin = new CheckBox();
            base.SuspendLayout();
            this.CBColorEach.FlatStyle = FlatStyle.Flat;
            this.CBColorEach.Location = new Point(0x93, 0x61);
            this.CBColorEach.Name = "CBColorEach";
            this.CBColorEach.Size = new Size(0x6d, 0x16);
            this.CBColorEach.TabIndex = 9;
            this.CBColorEach.Text = "Color &Each";
            this.CBColorEach.CheckedChanged += new EventHandler(this.CBColorEach_CheckedChanged);
            this.CBUseOrigin.FlatStyle = FlatStyle.Flat;
            this.CBUseOrigin.Location = new Point(0x93, 0x84);
            this.CBUseOrigin.Name = "CBUseOrigin";
            this.CBUseOrigin.Size = new Size(0x5d, 20);
            this.CBUseOrigin.TabIndex = 10;
            this.CBUseOrigin.Text = "&Use Origin";
            this.CBUseOrigin.CheckedChanged += new EventHandler(this.CBUseOrigin_CheckedChanged);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x12b, 0xb2);
            base.Controls.Add(this.CBUseOrigin);
            base.Controls.Add(this.CBColorEach);
            base.Name = "VolumeSeries";
            base.Controls.SetChildIndex(this.CBColorEach, 0);
            base.Controls.SetChildIndex(this.CBUseOrigin, 0);
            base.ResumeLayout(false);
        }
    }
}

