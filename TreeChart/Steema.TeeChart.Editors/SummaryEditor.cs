namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart.Data;
    using Steema.TeeChart.Styles;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class SummaryEditor : BaseSourceEditor
    {
        private ComboBox CBAgg;
        private ComboBox CBGroup;
        private ComboBox CBSort;
        private ComboBox CBSortType;
        private ComboBox CBTimeStep;
        private ComboBox CBValue;
        private IContainer components;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private ArrayList ValueListCombos;

        public SummaryEditor()
        {
            this.components = null;
            this.ValueListCombos = new ArrayList();
            this.InitializeComponent();
        }

        public SummaryEditor(Steema.TeeChart.Styles.Series s)
            : this()
        {
            base.series = s;
        }

        protected override void ApplyChanges()
        {
        }

        private void CBAgg_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.CBValue.Enabled = (base.series.DataSource != null) && (this.CBAgg.SelectedIndex != 1);
            if (this.CBValue.SelectedIndex != -1)
            {
                this.CBValue.Text = this.CBValue.Items[this.CBValue.SelectedIndex].ToString();
            }
            else
            {
                this.CBValue.Text = "";
            }
            base.BApply.Enabled = true;
        }

        private void CBSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.CBSortType.Enabled = this.CBSort.SelectedIndex >= 0;
            base.BApply.Enabled = true;
        }

        private void CBSortType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.CBSortType.Enabled = this.CBSort.SelectedIndex >= 0;
            base.BApply.Enabled = true;
        }

        protected override void CBSourcesSelectedIndexChanged(object sender, EventArgs e)
        {
            this.EnableCombos();
            this.SetFields();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void EnableCombos()
        {
            if (base.series.DataSource != null)
            {
                this.CBAgg.Enabled = true;
                this.CBValue.Enabled = true;
                this.CBTimeStep.Enabled = true;
                this.CBGroup.Enabled = true;
            }
            else
            {
                this.CBAgg.Enabled = false;
                this.CBValue.Enabled = false;
                this.CBTimeStep.Enabled = false;
                this.CBGroup.Enabled = false;
            }
        }

        private void InitializeComponent()
        {
            this.label1 = new Label();
            this.CBAgg = new ComboBox();
            this.label2 = new Label();
            this.CBValue = new ComboBox();
            this.label3 = new Label();
            this.label4 = new Label();
            this.CBTimeStep = new ComboBox();
            this.CBSort = new ComboBox();
            this.CBGroup = new ComboBox();
            this.CBSortType = new ComboBox();
            base.SuspendLayout();
            base.CBSources.Name = "CBSources";
            base.CBSources.Size = new Size(0xb8, 0x15);
            base.BApply.Name = "BApply";
            base.labelSource.Name = "labelSource";
            this.label1.Location = new Point(0x15, 0x38);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x30, 0x10);
            this.label1.TabIndex = 1;
            this.label1.Text = "&Calc:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            this.CBAgg.Items.AddRange(new object[] { "Sum", "Count", "High", "Low", "Avg" });
            this.CBAgg.Location = new Point(0x49, 0x35);
            this.CBAgg.Name = "CBAgg";
            this.CBAgg.Size = new Size(0x61, 0x15);
            this.CBAgg.TabIndex = 2;
            this.CBAgg.SelectedIndexChanged += new EventHandler(this.CBAgg_SelectedIndexChanged);
            this.label2.Location = new Point(0xb8, 0x38);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x10, 0x10);
            this.label2.TabIndex = 3;
            this.label2.Text = "of";
            this.CBValue.Location = new Point(0xd7, 0x35);
            this.CBValue.Name = "CBValue";
            this.CBValue.Size = new Size(0x9d, 0x15);
            this.CBValue.TabIndex = 4;
            this.label3.Location = new Point(12, 0x58);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x38, 0x10);
            this.label3.TabIndex = 5;
            this.label3.Text = "&Group by:";
            this.label3.TextAlign = ContentAlignment.TopRight;
            this.label4.Location = new Point(7, 120);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x3e, 0x10);
            this.label4.TabIndex = 6;
            this.label4.Text = "&Sort by:";
            this.label4.TextAlign = ContentAlignment.TopRight;
            this.CBTimeStep.Items.AddRange(new object[] { "Hour", "Day", "Week", "WeekDay", "Month", "Quarter", "Year" });
            this.CBTimeStep.Location = new Point(0x49, 0x57);
            this.CBTimeStep.Name = "CBTimeStep";
            this.CBTimeStep.Size = new Size(0x61, 0x15);
            this.CBTimeStep.TabIndex = 7;
            this.CBSort.Items.AddRange(new object[] { "(none)", "Calculation", "Group" });
            this.CBSort.Location = new Point(0x49, 0x76);
            this.CBSort.Name = "CBSort";
            this.CBSort.Size = new Size(0x77, 0x15);
            this.CBSort.TabIndex = 8;
            this.CBSort.SelectedIndexChanged += new EventHandler(this.CBSort_SelectedIndexChanged);
            this.CBGroup.Location = new Point(0xd7, 0x57);
            this.CBGroup.Name = "CBGroup";
            this.CBGroup.Size = new Size(0x9d, 0x15);
            this.CBGroup.TabIndex = 9;
            this.CBSortType.Items.AddRange(new object[] { "Ascending", "Descending" });
            this.CBSortType.Location = new Point(0xd7, 0x76);
            this.CBSortType.Name = "CBSortType";
            this.CBSortType.Size = new Size(0x9d, 0x15);
            this.CBSortType.TabIndex = 10;
            this.CBSortType.SelectedIndexChanged += new EventHandler(this.CBSortType_SelectedIndexChanged);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x184, 0xc5);
            base.Controls.Add(this.CBSortType);
            base.Controls.Add(this.CBGroup);
            base.Controls.Add(this.CBSort);
            base.Controls.Add(this.CBTimeStep);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.CBValue);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.CBAgg);
            base.Controls.Add(this.label1);
            base.Name = "SummaryEditor";
            base.Load += new EventHandler(this.SummaryEditor_Load);
            base.Controls.SetChildIndex(this.label1, 0);
            base.Controls.SetChildIndex(this.CBAgg, 0);
            base.Controls.SetChildIndex(this.label2, 0);
            base.Controls.SetChildIndex(this.CBValue, 0);
            base.Controls.SetChildIndex(this.label3, 0);
            base.Controls.SetChildIndex(this.label4, 0);
            base.Controls.SetChildIndex(this.CBTimeStep, 0);
            base.Controls.SetChildIndex(this.CBSort, 0);
            base.Controls.SetChildIndex(this.CBGroup, 0);
            base.Controls.SetChildIndex(this.CBSortType, 0);
            base.ResumeLayout(false);
        }

        protected override bool IsValid(object c)
        {
            return DataSeriesSource.IsValidSource(c);
        }

        private void SetFields()
        {
            this.ValueListCombos.Clear();
            this.CBValue.Items.Clear();
            this.CBGroup.Items.Clear();
            this.ValueListCombos.Add(this.CBValue);
            this.ValueListCombos.Add(this.CBGroup);
            if (base.CBSources.SelectedIndex != -1)
            {
                this.Cursor = Cursors.WaitCursor;
                try
                {
                    DataSeriesSource.FillFields(base.CBSources.SelectedItem, null, this.ValueListCombos);
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }
        }

        private void SummaryEditor_Load(object sender, EventArgs e)
        {
        }
    }
}

