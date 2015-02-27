namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart;
    using Steema.TeeChart.Data;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class SingleSourceEditor : BaseSourceEditor
    {
        private IContainer components;
        private System.Windows.Forms.Panel panel1;
        private SelectListForm select;

        public SingleSourceEditor()
        {
            this.components = null;
            this.InitializeComponent();
            int right = base.labelSource.Right;
            base.labelSource.Text = Texts.AskDataSet;
            base.labelSource.Left = right - base.labelSource.Width;
        }

        public SingleSourceEditor(Steema.TeeChart.Styles.Series s)
            : this()
        {
            base.series = s;
        }

        protected override void ApplyChanges()
        {
            if (base.series.DataSource is SingleRecordSource)
            {
                SingleRecordSource dataSource = (SingleRecordSource) base.series.DataSource;
                dataSource.ValueMembers = this.GetFields();
                dataSource.DataSource = base.CBSources.SelectedItem;
                base.series.CheckDataSource();
            }
            else
            {
                base.series.DataSource = null;
                SingleRecordSource source2 = new SingleRecordSource(base.CBSources.SelectedItem, this.GetFields());
                base.series.DataSource = source2;
                IContainer chartContainer = base.series.Chart.ChartContainer;
                if (chartContainer != null)
                {
                    source2.AddToContainer(chartContainer);
                }
            }
        }

        protected override void CBSourcesSelectedIndexChanged(object sender, EventArgs e)
        {
            this.SetFields();
            base.BApply.Enabled = true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private string[] GetFields()
        {
            string[] destination = new string[this.select.ToList.Items.Count];
            this.select.ToList.Items.CopyTo(destination, 0);
            return destination;
        }

        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            base.SuspendLayout();
            base.CBSources.ItemHeight = 13;
            base.CBSources.Name = "CBSources";
            base.CBSources.Size = new Size(0xb8, 0x15);
            base.CBSources.SelectedIndexChanged += new EventHandler(this.CBSourcesSelectedIndexChanged);
            base.BApply.Name = "BApply";
            base.labelSource.Name = "labelSource";
            this.panel1.Dock = DockStyle.Fill;
            this.panel1.Location = new Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x184, 0xc5);
            this.panel1.TabIndex = 1;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x184, 0xc5);
            base.Controls.Add(this.panel1);
            base.Name = "SingleSourceEditor";
            base.Load += new EventHandler(this.SingleSourceEditor_Load);
            base.Controls.SetChildIndex(this.panel1, 0);
            base.ResumeLayout(false);
        }

        protected override bool IsValid(object c)
        {
            return DataSeriesSource.IsValidSource(c);
        }

        private void SetCBSource()
        {
            if (base.series.DataSource == null)
            {
                base.CBSources.SelectedItem = null;
            }
            else
            {
                base.CBSources.SelectedItem = ((SingleRecordSource) base.series.DataSource).DataSource;
            }
        }

        private void SetFields()
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                this.select.FromList.Items.Clear();
                DataSeriesSource.FillFields(base.CBSources.SelectedItem, this.select.FromList, null);
                this.select.ToList.Items.Clear();
                if (base.series.DataSource is SingleRecordSource)
                {
                    SingleRecordSource dataSource = (SingleRecordSource) base.series.DataSource;
                    if (dataSource.ValueMembers != null)
                    {
                        foreach (string str in dataSource.ValueMembers)
                        {
                            if (this.select.FromList.Items.IndexOf(str) != -1)
                            {
                                this.select.ToList.Items.Add(str);
                            }
                        }
                    }
                }
                foreach (string str2 in this.select.ToList.Items)
                {
                    int index = this.select.FromList.Items.IndexOf(str2);
                    if (index != -1)
                    {
                        this.select.FromList.Items.RemoveAt(index);
                    }
                }
                this.select.EnableButtons();
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void SingleSourceEditor_Load(object sender, EventArgs e)
        {
            this.select = new SelectListForm();
            this.select.controlToEnable = base.BApply;
            EditorUtils.InsertForm(this.select, this.panel1);
            EditorUtils.Translate(this);
            if (base.series != null)
            {
                DataSeriesSource.FillSources(base.series, base.CBSources);
                this.SetCBSource();
            }
        }
    }
}

