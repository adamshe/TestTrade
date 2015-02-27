namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart.Functions;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class OBVFunctionEditor : Form
    {
        private ComboBox CBVolume;
        private Container components;
        internal Control controlToEnable;
        private OBVFunction function;
        private Label label1;
        private Series series;

        public OBVFunctionEditor()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public OBVFunctionEditor(Series s, ListBox list) : this()
        {
            int index;
            this.series = s;
            this.function = (OBVFunction) this.series.Function;
            foreach (string str in list.Items)
            {
                Series dest = this.series.chart.series.WithTitle(str);
                this.series.CheckOtherSeries(dest);
                this.CBVolume.Items.Add(str);
            }
            if (this.function.Volume != null)
            {
                this.CBVolume.Items.Add("(none)");
                if (!this.CBVolume.Items.Contains(this.function.Volume.ToString()))
                {
                    index = this.CBVolume.Items.Add(this.function.Volume.ToString());
                }
                else
                {
                    index = this.CBVolume.Items.IndexOf(this.function.Volume.ToString());
                }
                this.CBVolume.SelectedIndex = index;
            }
            else
            {
                index = this.CBVolume.Items.Add("(none)");
                this.CBVolume.SelectedIndex = index;
            }
        }

        private void CBVolume_SelectedIndexChanged(object sender, EventArgs e)
        {
            Series dest = this.series.chart.series.WithTitle(this.CBVolume.Items[this.CBVolume.SelectedIndex].ToString());
            if (dest != null)
            {
                this.series.CheckOtherSeries(dest);
                this.function.Volume = dest;
            }
            else
            {
                this.function.Volume = null;
            }
            this.Changed();
        }

        private void Changed()
        {
            if (this.controlToEnable != null)
            {
                this.controlToEnable.Enabled = true;
            }
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
            this.CBVolume = new ComboBox();
            this.label1 = new Label();
            base.SuspendLayout();
            this.CBVolume.Location = new Point(0x18, 40);
            this.CBVolume.Name = "CBVolume";
            this.CBVolume.Size = new Size(0x8e, 0x15);
            this.CBVolume.TabIndex = 3;
            this.CBVolume.SelectedIndexChanged += new EventHandler(this.CBVolume_SelectedIndexChanged);
            this.label1.Location = new Point(0x10, 0x10);
            this.label1.Name = "label1";
            this.label1.TabIndex = 2;
            this.label1.Text = "Volume Series:";
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(240, 0x6d);
            base.Controls.Add(this.CBVolume);
            base.Controls.Add(this.label1);
            base.Name = "OBVFunctionEditor";
            this.Text = "OBVFunctionEditor";
            base.ResumeLayout(false);
        }
    }
}

