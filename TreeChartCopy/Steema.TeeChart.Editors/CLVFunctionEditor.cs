namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart.Functions;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class CLVFunctionEditor : Form
    {
        private CheckBox CBAccumulate;
        private ComboBox CBVolume;
        private Container components;
        internal Control controlToEnable;
        private CLVFunction function;
        private Label label1;
        private Series series;

        public CLVFunctionEditor()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public CLVFunctionEditor(Series s, ListBox list) : this()
        {
            int index;
            this.series = s;
            this.function = (CLVFunction) this.series.Function;
            foreach (string str in list.Items)
            {
                Series dest = this.series.chart.series.WithTitle(str);
                this.series.CheckOtherSeries(dest);
                this.CBVolume.Items.Add(str);
            }
            this.CBAccumulate.Checked = this.function.Accumulate;
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

        private void CBAccumulate_Click(object sender, EventArgs e)
        {
            this.function.Accumulate = this.CBAccumulate.Checked;
            this.Changed();
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
            this.label1 = new Label();
            this.CBVolume = new ComboBox();
            this.CBAccumulate = new CheckBox();
            base.SuspendLayout();
            this.label1.Location = new Point(0x10, 0x10);
            this.label1.Name = "label1";
            this.label1.TabIndex = 0;
            this.label1.Text = "Volume Series:";
            this.CBVolume.Location = new Point(0x12, 0x23);
            this.CBVolume.Name = "CBVolume";
            this.CBVolume.Size = new Size(0x8e, 0x15);
            this.CBVolume.TabIndex = 1;
            this.CBVolume.SelectedIndexChanged += new EventHandler(this.CBVolume_SelectedIndexChanged);
            this.CBAccumulate.Location = new Point(0x10, 0x48);
            this.CBAccumulate.Name = "CBAccumulate";
            this.CBAccumulate.Size = new Size(0x90, 0x18);
            this.CBAccumulate.TabIndex = 2;
            this.CBAccumulate.Text = "Accumulate";
            this.CBAccumulate.Click += new EventHandler(this.CBAccumulate_Click);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(240, 0x6d);
            base.Controls.Add(this.CBAccumulate);
            base.Controls.Add(this.CBVolume);
            base.Controls.Add(this.label1);
            base.Name = "CLVFunctionEditor";
            this.Text = "CLVFunctionEditor";
            base.ResumeLayout(false);
        }
    }
}

