namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart;
    using Steema.TeeChart.Data;
    using Steema.TeeChart.Styles;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Windows.Forms;

    public class DatabaseEditor : BaseSourceEditor
    {
        private CheckBox cbDate;
        private ComboBox cbLabels;
        private ComboBox cbManda;
        private IContainer components;
        private Label labelLabel;
        private Label labelManda;
        private bool updating;
        private ArrayList ValueListCombos;

        public DatabaseEditor()
        {
            this.components = null;
            this.ValueListCombos = new ArrayList();
            this.InitializeComponent();
            int right = base.labelSource.Right;
            base.labelSource.Text = Texts.AskDataSet;
            base.labelSource.Left = right - base.labelSource.Width;
        }

        public DatabaseEditor(Steema.TeeChart.Styles.Series s)
            : this()
        {
            base.series = s;
        }

        protected override void ApplyChanges()
        {
            base.series.DataSource = null;
            this.ClearMembers();
            this.SetNewMembers();
            if ((base.CBSources.SelectedItem is DataTable) && (((DataTable) base.CBSources.SelectedItem).DataSet != null))
            {
                int index = ((DataTable) base.CBSources.SelectedItem).DataSet.Tables.IndexOf((DataTable) base.CBSources.SelectedItem);
                base.series.DataSource = ((DataTable) base.CBSources.SelectedItem).DataSet.Tables[index];
            }
            else
            {
                base.series.DataSource = base.CBSources.SelectedItem;
            }
        }

        private void cbDate_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox box = (CheckBox) sender;
            ((Steema.TeeChart.Styles.ValueList) box.Tag).DateTime = box.Checked;
        }

        private void cbLabels_SelectedIndexChanged(object sender, EventArgs e)
        {
            base.BApply.Enabled = true;
        }

        private void cbManda_SelectedIndexChanged(object sender, EventArgs e)
        {
            base.BApply.Enabled = true;
        }

        protected override void CBSourcesSelectedIndexChanged(object sender, EventArgs e)
        {
            base.CBSourcesSelectedIndexChanged(sender, e);
            this.SetFields();
        }

        protected void ClearMembers()
        {
            base.series.labelMember = "";
            foreach (Steema.TeeChart.Styles.ValueList list in base.series.ValuesLists)
            {
                list.valueSource = "";
            }
        }

        private void CreateCombos()
        {
            this.ValueListCombos.Clear();
            this.cbManda.Name = base.series.mandatory.Name;
            this.cbManda.Tag = base.series.mandatory;
            this.ValueListCombos.Add(this.cbManda);
            this.labelManda.TextAlign = ContentAlignment.TopRight;
            this.labelManda.Text = base.series.mandatory.Name + ":";
            this.labelManda.Left = this.labelLabel.Right - this.labelManda.Width;
            this.cbDate.Tag = base.series.mandatory;
            int num = 0;
            foreach (Steema.TeeChart.Styles.ValueList list in base.series.valuesList)
            {
                if (base.series.mandatory != list)
                {
                    ComboBox box = new ComboBox();
                    box.Name = list.Name;
                    box.Left = this.cbLabels.Left;
                    box.DropDownStyle = ComboBoxStyle.DropDown;
                    box.Width = this.cbLabels.Width;
                    box.Top = (2 + this.cbManda.Top) + (this.cbManda.Height * (num + 1));
                    box.SelectedIndexChanged += new EventHandler(this.cbManda_SelectedIndexChanged);
                    box.TextChanged += new EventHandler(this.cbManda_SelectedIndexChanged);
                    box.SelectedValueChanged += new EventHandler(this.cbManda_SelectedIndexChanged);
                    box.Tag = list;
                    this.ValueListCombos.Add(box);
                    this.cbLabels.Parent.Controls.Add(box);
                    Label label = new Label();
                    label.Text = list.Name + ":";
                    label.TextAlign = ContentAlignment.MiddleRight;
                    label.Top = box.Top;
                    label.Left = this.labelLabel.Left;
                    label.Width = this.labelLabel.Width;
                    this.cbLabels.Parent.Controls.Add(label);
                    CheckBox box2 = new CheckBox();
                    box2.Left = this.cbDate.Left;
                    box2.Top = box.Top;
                    box2.Text = Texts.DateTime;
                    box2.Width = this.cbDate.Width;
                    box2.FlatStyle = FlatStyle.Flat;
                    box2.Tag = list;
                    this.cbLabels.Parent.Controls.Add(box2);
                    box2.CheckedChanged += new EventHandler(this.cbDate_CheckedChanged);
                    box2.Checked = list.DateTime;
                    num++;
                }
            }
        }

        private void DatabaseEditor_Load(object sender, EventArgs e)
        {
            if (base.series != null)
            {
                this.updating = true;
                try
                {
                    DataSeriesSource.FillSources(base.series, base.CBSources);
                    base.SetCBSource();
                    this.CreateCombos();
                }
                finally
                {
                    this.updating = false;
                    this.SetFields();
                }
            }
            base.BApply.Enabled = false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private string GetComboText(ComboBox c)
        {
            if (c.SelectedIndex == -1)
            {
                return c.Text;
            }
            return c.SelectedItem.ToString();
        }

        private void InitializeComponent()
        {
            this.labelLabel = new Label();
            this.cbLabels = new ComboBox();
            this.cbManda = new ComboBox();
            this.labelManda = new Label();
            this.cbDate = new CheckBox();
            base.SuspendLayout();
            base.CBSources.ItemHeight = 13;
            base.CBSources.Name = "CBSources";
            base.CBSources.Size = new Size(0xb8, 0x15);
            base.CBSources.SelectedIndexChanged += new EventHandler(this.CBSourcesSelectedIndexChanged);
            base.BApply.Name = "BApply";
            base.labelSource.Name = "labelSource";
            this.labelLabel.AutoSize = true;
            this.labelLabel.Location = new Point(0x38, 0x26);
            this.labelLabel.Name = "labelLabel";
            this.labelLabel.Size = new Size(0x29, 0x10);
            this.labelLabel.TabIndex = 1;
            this.labelLabel.Text = "&Labels:";
            this.labelLabel.TextAlign = ContentAlignment.TopRight;
            this.cbLabels.Location = new Point(0x68, 0x24);
            this.cbLabels.Name = "cbLabels";
            this.cbLabels.Size = new Size(0x8f, 0x15);
            this.cbLabels.TabIndex = 2;
            this.cbLabels.TextChanged += new EventHandler(this.cbLabels_SelectedIndexChanged);
            this.cbLabels.SelectedValueChanged += new EventHandler(this.cbLabels_SelectedIndexChanged);
            this.cbLabels.SelectedIndexChanged += new EventHandler(this.cbLabels_SelectedIndexChanged);
            this.cbManda.Location = new Point(0x68, 0x40);
            this.cbManda.Name = "cbManda";
            this.cbManda.Size = new Size(0x8f, 0x15);
            this.cbManda.TabIndex = 4;
            this.cbManda.TextChanged += new EventHandler(this.cbManda_SelectedIndexChanged);
            this.cbManda.SelectedValueChanged += new EventHandler(this.cbManda_SelectedIndexChanged);
            this.cbManda.SelectedIndexChanged += new EventHandler(this.cbManda_SelectedIndexChanged);
            this.labelManda.AutoSize = true;
            this.labelManda.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.labelManda.Location = new Point(0x52, 0x42);
            this.labelManda.Name = "labelManda";
            this.labelManda.Size = new Size(15, 0x10);
            this.labelManda.TabIndex = 3;
            this.labelManda.Text = "&Y:";
            this.labelManda.TextAlign = ContentAlignment.TopRight;
            this.cbDate.FlatStyle = FlatStyle.Flat;
            this.cbDate.Location = new Point(0x108, 0x3e);
            this.cbDate.Name = "cbDate";
            this.cbDate.TabIndex = 5;
            this.cbDate.Text = "&DateTime";
            this.cbDate.CheckedChanged += new EventHandler(this.cbDate_CheckedChanged);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x184, 0xc5);
            base.Controls.Add(this.cbDate);
            base.Controls.Add(this.cbManda);
            base.Controls.Add(this.labelManda);
            base.Controls.Add(this.cbLabels);
            base.Controls.Add(this.labelLabel);
            base.Name = "DatabaseEditor";
            base.Load += new EventHandler(this.DatabaseEditor_Load);
            base.Controls.SetChildIndex(this.labelLabel, 0);
            base.Controls.SetChildIndex(this.cbLabels, 0);
            base.Controls.SetChildIndex(this.labelManda, 0);
            base.Controls.SetChildIndex(this.cbManda, 0);
            base.Controls.SetChildIndex(this.cbDate, 0);
            base.ResumeLayout(false);
        }

        protected override bool IsValid(object c)
        {
            return DataSeriesSource.IsValidSource(c);
        }

        private void SetCombosSelected()
        {
            foreach (Steema.TeeChart.Styles.ValueList list in base.series.valuesList)
            {
                if ((list.DataMember != null) && (list.DataMember.Length != 0))
                {
                    foreach (ComboBox box in this.ValueListCombos)
                    {
                        if (box.Name == list.Name)
                        {
                            box.SelectedIndex = box.Items.IndexOf(list.DataMember);
                        }
                    }
                    continue;
                }
            }
            this.cbLabels.SelectedIndex = this.cbLabels.Items.IndexOf(base.series.labelMember);
        }

        private void SetFields()
        {
            if (!this.updating)
            {
                this.cbLabels.Items.Clear();
                this.cbLabels.Enabled = base.CBSources.SelectedIndex != -1;
                foreach (ComboBox box in this.ValueListCombos)
                {
                    box.Items.Clear();
                    box.Enabled = this.cbLabels.Enabled;
                }
                if (base.CBSources.SelectedIndex != -1)
                {
                    this.Cursor = Cursors.WaitCursor;
                    try
                    {
                        DataSeriesSource.FillFields(base.CBSources.SelectedItem, this.cbLabels, this.ValueListCombos);
                    }
                    finally
                    {
                        this.Cursor = Cursors.Default;
                    }
                }
                this.SetCombosSelected();
            }
        }

        private void SetNewMembers()
        {
            if ((this.cbLabels.SelectedItem != null) && (this.cbLabels.SelectedItem.ToString().Length > 0))
            {
                base.series.labelMember = this.cbLabels.SelectedItem.ToString();
            }
            foreach (ComboBox box in this.ValueListCombos)
            {
                if (((box.SelectedItem != null) && (box.SelectedItem.ToString().Length > 0)) && (box.SelectedIndex != -1))
                {
                    ((Steema.TeeChart.Styles.ValueList) box.Tag).valueSource = this.GetComboText(box);
                }
            }
        }
    }
}

