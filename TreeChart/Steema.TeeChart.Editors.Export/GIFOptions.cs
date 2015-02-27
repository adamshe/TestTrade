namespace Steema.TeeChart.Editors.Export
{
    using Steema.TeeChart.Export;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class GIFOptions : Form
    {
        private ComboBox CBReduction;
        private Container components = null;
        private Label label2;

        public GIFOptions()
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

        private void GIFOptions_Load(object sender, EventArgs e)
        {
            this.CBReduction.Items.AddRange(Enum.GetNames(typeof(GIFFormat.GIFColorReduction)));
            this.CBReduction.SelectedItem = GIFFormat.GIFColorReduction.None.ToString();
        }

        private void InitializeComponent()
        {
            this.label2 = new Label();
            this.CBReduction = new ComboBox();
            base.SuspendLayout();
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x10, 0x22);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x3a, 0x10);
            this.label2.TabIndex = 2;
            this.label2.Text = "&Reduction:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.CBReduction.DropDownStyle = ComboBoxStyle.DropDownList;
            this.CBReduction.Location = new Point(80, 0x20);
            this.CBReduction.Name = "CBReduction";
            this.CBReduction.Size = new Size(0x68, 0x15);
            this.CBReduction.TabIndex = 4;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0xd0, 0x66);
            base.Controls.Add(this.CBReduction);
            base.Controls.Add(this.label2);
            base.Name = "GIFOptions";
            base.Load += new EventHandler(this.GIFOptions_Load);
            base.ResumeLayout(false);
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
        }

        public int ColorReduction
        {
            get
            {
                return this.CBReduction.SelectedIndex;
            }
        }
    }
}

