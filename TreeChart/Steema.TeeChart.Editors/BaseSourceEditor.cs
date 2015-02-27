namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class BaseSourceEditor : Form
    {
        protected Button BApply;
        protected ComboBox CBSources;
        private Container components = null;
        protected Label labelSource;
        private Panel Pan;
        protected Steema.TeeChart.Styles.Series series;

        public BaseSourceEditor()
        {
            this.InitializeComponent();
        }

        protected void AddComponentDataSource(object c, ComboBox items, bool addCurrent)
        {
            if (this.IsValid(c) && this.series.chart.IsValidDataSource(this.series, c))
            {
                object obj2;
                if (c is Steema.TeeChart.Styles.Series)
                {
                    obj2 = ((Steema.TeeChart.Styles.Series)c).ToString();
                }
                else
                {
                    obj2 = c;
                }
                items.Items.Add(obj2);
            }
        }

        protected virtual void ApplyChanges()
        {
        }

        private void BApply_Click(object sender, EventArgs e)
        {
            this.ApplyChanges();
            this.BApply.Enabled = false;
        }

        protected virtual void CBSourcesSelectedIndexChanged(object sender, EventArgs e)
        {
            this.BApply.Enabled = true;
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
            this.Pan = new Panel();
            this.BApply = new Button();
            this.CBSources = new ComboBox();
            this.labelSource = new Label();
            this.Pan.SuspendLayout();
            base.SuspendLayout();
            this.Pan.Controls.Add(this.BApply);
            this.Pan.Controls.Add(this.CBSources);
            this.Pan.Controls.Add(this.labelSource);
            this.Pan.Dock = DockStyle.Top;
            this.Pan.Location = new Point(0, 0);
            this.Pan.Name = "Pan";
            this.Pan.Size = new Size(0x184, 30);
            this.Pan.TabIndex = 0;
            this.BApply.FlatStyle = FlatStyle.Flat;
            this.BApply.Location = new Point(0x128, 4);
            this.BApply.Name = "BApply";
            this.BApply.Size = new Size(0x40, 0x17);
            this.BApply.TabIndex = 2;
            this.BApply.Text = "&Apply";
            this.BApply.Click += new EventHandler(this.BApply_Click);
            this.CBSources.DropDownStyle = ComboBoxStyle.DropDownList;
            this.CBSources.Location = new Point(0x60, 4);
            this.CBSources.Name = "CBSources";
            this.CBSources.Size = new Size(0xb8, 0x15);
            this.CBSources.TabIndex = 1;
            this.CBSources.SelectedIndexChanged += new EventHandler(this.CBSourcesSelectedIndexChanged);
            this.labelSource.AutoSize = true;
            this.labelSource.Location = new Point(0x30, 6);
            this.labelSource.Name = "labelSource";
            this.labelSource.Size = new Size(0x2b, 0x10);
            this.labelSource.TabIndex = 0;
            this.labelSource.Text = "&Source:";
            this.labelSource.TextAlign = ContentAlignment.TopRight;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x184, 0xc5);
            base.Controls.Add(this.Pan);
            base.Name = "BaseSourceEditor";
            this.Pan.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        protected virtual bool IsValid(object c)
        {
            return false;
        }

        protected void SetCBSource()
        {
            if (this.series.DataSource != null)
            {
                foreach (object obj2 in this.CBSources.Items)
                {
                    if (obj2 == this.series.DataSource)
                    {
                        this.CBSources.SelectedIndex = this.CBSources.Items.IndexOf(obj2);
                        break;
                    }
                }
            }
        }
    }
}

