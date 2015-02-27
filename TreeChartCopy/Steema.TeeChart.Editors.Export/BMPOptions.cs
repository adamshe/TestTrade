namespace Steema.TeeChart.Editors.Export
{
    using Steema.TeeChart.Export;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class BMPOptions : Form
    {
        private ComboBox CBColors;
        private Container components = null;
        private Label label1;

        public BMPOptions()
        {
            this.InitializeComponent();
        }

        private void BMPOptions_Load(object sender, EventArgs e)
        {
            this.CBColors.Items.AddRange(Enum.GetNames(typeof(BitmapFormat.BMPColorReduction)));
            this.CBColors.SelectedItem = BitmapFormat.BMPColorReduction.Default.ToString();
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
            this.label1 = new Label();
            this.CBColors = new ComboBox();
            base.SuspendLayout();
            this.label1.AutoSize = true;
            this.label1.Location = new Point(8, 30);
            this.label1.Name = "label1";
            this.label1.Size = new Size(40, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Colors:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            this.CBColors.DropDownStyle = ComboBoxStyle.DropDownList;
            this.CBColors.Location = new Point(0x33, 0x19);
            this.CBColors.Name = "CBColors";
            this.CBColors.Size = new Size(0x66, 0x15);
            this.CBColors.TabIndex = 1;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0xa2, 0x5c);
            base.Controls.AddRange(new Control[] { this.CBColors, this.label1 });
            base.Name = "BMPOptions";
            base.Load += new EventHandler(this.BMPOptions_Load);
            base.ResumeLayout(false);
        }

        public int ColorReduction
        {
            get
            {
                return this.CBColors.SelectedIndex;
            }
        }
    }
}

