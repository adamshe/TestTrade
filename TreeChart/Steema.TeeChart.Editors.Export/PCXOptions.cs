namespace Steema.TeeChart.Editors.Export
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class PCXOptions : Form
    {
        private Container components = null;

        public PCXOptions()
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
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0xd4, 0xb0);
            base.Name = "PCXOptions";
        }
    }
}

