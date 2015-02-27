namespace Steema.TeeChart.Editors.Export
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class EmfOptions : Form
    {
        private CheckBox CBEnhanced;
        private Container components = null;

        public EmfOptions()
        {
            this.InitializeComponent();
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
            this.CBEnhanced = new CheckBox();
            base.SuspendLayout();
            this.CBEnhanced.Checked = true;
            this.CBEnhanced.CheckState = CheckState.Checked;
            this.CBEnhanced.FlatStyle = FlatStyle.Flat;
            this.CBEnhanced.Location = new Point(0x10, 0x10);
            this.CBEnhanced.Name = "CBEnhanced";
            this.CBEnhanced.Size = new Size(0x68, 0x10);
            this.CBEnhanced.TabIndex = 0;
            this.CBEnhanced.Text = "&Enhanced";
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x98, 0x4b);
            base.Controls.AddRange(new Control[] { this.CBEnhanced });
            base.Name = "EmfOptions";
            base.ResumeLayout(false);
        }

        public bool Enhanced
        {
            get
            {
                return this.CBEnhanced.Checked;
            }
        }
    }
}

