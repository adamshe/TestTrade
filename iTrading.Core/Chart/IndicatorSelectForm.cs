using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;
using iTrading.Core.Gui;
using iTrading.Core.IndicatorBase;
using iTrading.Core.Kernel;
using iTrading.Core.Data;

namespace iTrading.Core.Chart
{
    internal class IndicatorSelectForm : Form
    {
        private Button acceptButton;
        internal Button applyButton;
        private ComboBox basedOnComboBox;
        private Label basedOnLabel;
        private Button closeButton;
        private Container components = null;
        private DataGrid dataGrid;
        private DataGridComboBoxColumn dataGridComboBoxColumn = new DataGridComboBoxColumn();
        private Hashtable indicatorClasses = new Hashtable();
        private IndicatorCollection indicators = new IndicatorCollection();
        private Label paramLabel0;
        private Label paramLabel1;
        private Label paramLabel2;
        private Label paramLabel3;
        private NumericUpDown paramNumericUpDown0;
        private NumericUpDown paramNumericUpDown1;
        private NumericUpDown paramNumericUpDown2;
        private NumericUpDown paramNumericUpDown3;
        private Quotes quotes = null;
        private bool selecting = false;

        internal IndicatorSelectForm()
        {
            this.InitializeComponent();
        }

        private void acceptButton_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.OK;
            base.Close();
        }

        private void basedOnComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.selecting)
            {
                PropertyInfo propertyInfo = this.indicators[this.dataGrid.CurrentRowIndex].GetPropertyInfo(PropertyType.BasedOn);
                if (propertyInfo != null)
                {
                    propertyInfo.SetValue(this.indicators[this.dataGrid.CurrentRowIndex], PriceType.All.Find((string) this.basedOnComboBox.SelectedItem).Id, null);
                }
            }
        }

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.dataGridComboBoxColumn.ComboBox.SelectedItem != null)
            {
                if (this.dataGrid.CurrentRowIndex >= this.indicators.Count)
                {
                    this.indicators.Add(((IndicatorBase.IndicatorBase) this.indicatorClasses[(string) this.dataGridComboBoxColumn.ComboBox.SelectedItem]).CloneWithQuotes(this.quotes));
                }
                else if (((IndicatorBase.IndicatorBase) this.indicatorClasses[(string) this.dataGridComboBoxColumn.ComboBox.SelectedItem]).GetType() != this.indicators[this.dataGrid.CurrentRowIndex].GetType())
                {
                    this.indicators[this.dataGrid.CurrentRowIndex] = ((IndicatorBase.IndicatorBase) this.indicatorClasses[(string) this.dataGridComboBoxColumn.ComboBox.SelectedItem]).CloneWithQuotes(this.quotes);
                }
                this.Selected(this.indicators[this.dataGrid.CurrentRowIndex]);
            }
        }

        private void dataGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            if (this.dataGrid.CurrentRowIndex < ((DataTable) this.dataGrid.DataSource).Rows.Count)
            {
                this.Selected(this.indicators[this.dataGrid.CurrentRowIndex]);
            }
        }

        private void dataTable_RowDeleting(object sender, DataRowChangeEventArgs e)
        {
            for (int i = 0; i < ((DataTable) this.dataGrid.DataSource).Rows.Count; i++)
            {
                if (e.Row == ((DataTable) this.dataGrid.DataSource).Rows[i])
                {
                    this.indicators.RemoveAt(i);
                    return;
                }
            }
        }

        /// <summary>
        /// Die verwendeten Ressourcen bereinigen.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void HideControls()
        {
            this.basedOnLabel.Hide();
            this.basedOnComboBox.Hide();
            this.paramLabel0.Hide();
            this.paramLabel1.Hide();
            this.paramLabel2.Hide();
            this.paramLabel3.Hide();
            this.paramNumericUpDown0.Hide();
            this.paramNumericUpDown1.Hide();
            this.paramNumericUpDown2.Hide();
            this.paramNumericUpDown3.Hide();
        }

        private void IndicatorSelectForm_Load(object sender, EventArgs e)
        {
            Trace.Assert(this.quotes != null);
            DataGridTableStyle style = new DataGridTableStyle();
            DataTable table = new DataTable("table");
            this.dataGridComboBoxColumn.ComboBox.SelectedIndexChanged += new EventHandler(this.ComboBox_SelectedIndexChanged);
            this.dataGridComboBoxColumn.ComboBox.Sorted = true;
            this.dataGridComboBoxColumn.HeaderText = "Indicator";
            this.dataGridComboBoxColumn.NullText = "";
            this.dataGridComboBoxColumn.MappingName = "Indicator";
            this.dataGridComboBoxColumn.Width = (this.dataGrid.ClientRectangle.Width - this.dataGrid.RowHeaderWidth) - 4;
            style.GridColumnStyles.Add(this.dataGridComboBoxColumn);
            style.MappingName = "table";
            style.PreferredRowHeight = this.dataGridComboBoxColumn.ComboBox.Height + 2;
            DataColumn column = new DataColumn();
            column.ColumnName = "Indicator";
            column.DataType = typeof(string);
            table.Columns.Add(column);
            table.RowDeleting += new DataRowChangeEventHandler(this.dataTable_RowDeleting);
            this.dataGrid.DataSource = table;
            this.dataGrid.TableStyles.Add(style);
            foreach (System.Type type in Globals.IndicatorAssembly.GetTypes())
            {
                if (type.BaseType == typeof(IndicatorBase.IndicatorBase))
                {
                    IndicatorBase.IndicatorBase base2 = (IndicatorBase.IndicatorBase) Globals.IndicatorAssembly.CreateInstance(type.FullName, false, BindingFlags.CreateInstance, null, new object[] { this.quotes }, null, new object[0]);
                    if (this.indicatorClasses[base2.Name] != null)
                    {
                        this.quotes.Connection.ProcessEventArgs(new ITradingErrorEventArgs(this.quotes.Connection, ErrorCode.DuplicateIndicator, "name", "Two or more indicators have the same name/description"));
                    }
                    else
                    {
                        this.dataGridComboBoxColumn.ComboBox.Items.Add(base2.Name);
                        this.indicatorClasses.Add(base2.Name, base2);
                    }
                }
            }
            foreach (PriceType type2 in PriceType.All.Values)
            {
                this.basedOnComboBox.Items.Add(type2.Name);
            }
            foreach (IndicatorBase.IndicatorBase base3 in this.indicators)
            {
                DataRow row = table.NewRow();
                row["Indicator"] = base3.Name;
                table.Rows.Add(row);
            }
            this.HideControls();
        }

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            ResourceManager manager = new ResourceManager(typeof(IndicatorSelectForm));
            this.dataGrid = new DataGrid();
            this.acceptButton = new Button();
            this.applyButton = new Button();
            this.paramLabel0 = new Label();
            this.paramNumericUpDown0 = new NumericUpDown();
            this.paramNumericUpDown1 = new NumericUpDown();
            this.paramLabel1 = new Label();
            this.paramNumericUpDown2 = new NumericUpDown();
            this.paramLabel2 = new Label();
            this.paramNumericUpDown3 = new NumericUpDown();
            this.paramLabel3 = new Label();
            this.basedOnLabel = new Label();
            this.basedOnComboBox = new ComboBox();
            this.closeButton = new Button();
            this.dataGrid.BeginInit();
            this.paramNumericUpDown0.BeginInit();
            this.paramNumericUpDown1.BeginInit();
            this.paramNumericUpDown2.BeginInit();
            this.paramNumericUpDown3.BeginInit();
            base.SuspendLayout();
            this.dataGrid.CaptionVisible = false;
            this.dataGrid.DataMember = "";
            this.dataGrid.HeaderForeColor = SystemColors.ControlText;
            this.dataGrid.Location = new Point(0, 0);
            this.dataGrid.Name = "dataGrid";
            this.dataGrid.Size = new Size(0x130, 0xe8);
            this.dataGrid.TabIndex = 0;
            this.dataGrid.CurrentCellChanged += new EventHandler(this.dataGrid_CurrentCellChanged);
            this.acceptButton.Location = new Point(120, 0x100);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new Size(90, 0x1a);
            this.acceptButton.TabIndex = 10;
            this.acceptButton.Text = "&Accept";
            this.acceptButton.Click += new EventHandler(this.acceptButton_Click);
            this.applyButton.Location = new Point(0x100, 0x100);
            this.applyButton.Name = "applyButton";
            this.applyButton.Size = new Size(90, 0x1a);
            this.applyButton.TabIndex = 11;
            this.applyButton.Text = "&Apply";
            this.paramLabel0.Location = new Point(0x150, 0x40);
            this.paramLabel0.Name = "paramLabel0";
            this.paramLabel0.Size = new Size(0x70, 0x18);
            this.paramLabel0.TabIndex = 12;
            this.paramLabel0.Text = "Param0:";
            this.paramLabel0.TextAlign = ContentAlignment.MiddleLeft;
            this.paramNumericUpDown0.Location = new Point(0x1c8, 0x40);
            this.paramNumericUpDown0.Name = "paramNumericUpDown0";
            this.paramNumericUpDown0.Size = new Size(0x68, 0x16);
            this.paramNumericUpDown0.TabIndex = 2;
            this.paramNumericUpDown0.ValueChanged += new EventHandler(this.paramNumericUpDown0_ValueChanged);
            this.paramNumericUpDown0.Leave += new EventHandler(this.paramNumericUpDown0_ValueChanged);
            this.paramNumericUpDown1.Location = new Point(0x1c8, 0x68);
            this.paramNumericUpDown1.Name = "paramNumericUpDown1";
            this.paramNumericUpDown1.Size = new Size(0x68, 0x16);
            this.paramNumericUpDown1.TabIndex = 3;
            this.paramNumericUpDown1.ValueChanged += new EventHandler(this.paramNumericUpDown1_ValueChanged);
            this.paramNumericUpDown1.Leave += new EventHandler(this.paramNumericUpDown1_ValueChanged);
            this.paramLabel1.Location = new Point(0x150, 0x68);
            this.paramLabel1.Name = "paramLabel1";
            this.paramLabel1.Size = new Size(0x70, 0x17);
            this.paramLabel1.TabIndex = 14;
            this.paramLabel1.Text = "Param1:";
            this.paramLabel1.TextAlign = ContentAlignment.MiddleLeft;
            this.paramNumericUpDown2.Location = new Point(0x1c8, 0x90);
            this.paramNumericUpDown2.Name = "paramNumericUpDown2";
            this.paramNumericUpDown2.Size = new Size(0x68, 0x16);
            this.paramNumericUpDown2.TabIndex = 4;
            this.paramNumericUpDown2.ValueChanged += new EventHandler(this.paramNumericUpDown2_ValueChanged);
            this.paramNumericUpDown2.Leave += new EventHandler(this.paramNumericUpDown2_ValueChanged);
            this.paramLabel2.Location = new Point(0x150, 0x90);
            this.paramLabel2.Name = "paramLabel2";
            this.paramLabel2.Size = new Size(0x70, 0x17);
            this.paramLabel2.TabIndex = 0x10;
            this.paramLabel2.Text = "Param2:";
            this.paramLabel2.TextAlign = ContentAlignment.MiddleLeft;
            this.paramNumericUpDown3.Location = new Point(0x1c8, 0xb8);
            this.paramNumericUpDown3.Name = "paramNumericUpDown3";
            this.paramNumericUpDown3.Size = new Size(0x68, 0x16);
            this.paramNumericUpDown3.TabIndex = 5;
            this.paramNumericUpDown3.ValueChanged += new EventHandler(this.paramNumericUpDown3_ValueChanged);
            this.paramNumericUpDown3.Leave += new EventHandler(this.paramNumericUpDown3_ValueChanged);
            this.paramLabel3.Location = new Point(0x150, 0xb8);
            this.paramLabel3.Name = "paramLabel3";
            this.paramLabel3.Size = new Size(0x70, 0x17);
            this.paramLabel3.TabIndex = 0x12;
            this.paramLabel3.Text = "Param3:";
            this.paramLabel3.TextAlign = ContentAlignment.MiddleLeft;
            this.basedOnLabel.Location = new Point(0x150, 0x18);
            this.basedOnLabel.Name = "basedOnLabel";
            this.basedOnLabel.Size = new Size(0x70, 0x17);
            this.basedOnLabel.TabIndex = 20;
            this.basedOnLabel.Text = "Based on:";
            this.basedOnLabel.TextAlign = ContentAlignment.MiddleLeft;
            this.basedOnComboBox.Location = new Point(0x1c8, 0x18);
            this.basedOnComboBox.Name = "basedOnComboBox";
            this.basedOnComboBox.Size = new Size(0x68, 0x18);
            this.basedOnComboBox.Sorted = true;
            this.basedOnComboBox.TabIndex = 1;
            this.basedOnComboBox.SelectedIndexChanged += new EventHandler(this.basedOnComboBox_SelectedIndexChanged);
            
            this.closeButton.Location = new Point(0x188, 0x100);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new Size(90, 0x1a);
            this.closeButton.TabIndex = 12;
            this.closeButton.Text = "&Close";
            base.AcceptButton = this.acceptButton;
            this.AutoScaleBaseSize = new Size(6, 15);
            base.CancelButton = this.closeButton;
            base.ClientSize = new Size(0x24a, 0x12d);
            base.Controls.Add(this.closeButton);
            base.Controls.Add(this.basedOnComboBox);
            base.Controls.Add(this.basedOnLabel);
            base.Controls.Add(this.paramNumericUpDown3);
            base.Controls.Add(this.paramLabel3);
            base.Controls.Add(this.paramNumericUpDown2);
            base.Controls.Add(this.paramLabel2);
            base.Controls.Add(this.paramNumericUpDown1);
            base.Controls.Add(this.paramLabel1);
            base.Controls.Add(this.paramNumericUpDown0);
            base.Controls.Add(this.paramLabel0);
            base.Controls.Add(this.applyButton);
            base.Controls.Add(this.acceptButton);
            base.Controls.Add(this.dataGrid);
            
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "IndicatorSelectForm";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Indicators";
            base.Load += new EventHandler(this.IndicatorSelectForm_Load);
            this.dataGrid.EndInit();
            this.paramNumericUpDown0.EndInit();
            this.paramNumericUpDown1.EndInit();
            this.paramNumericUpDown2.EndInit();
            this.paramNumericUpDown3.EndInit();
            base.ResumeLayout(false);
        }

        private void paramNumericUpDown0_ValueChanged(object sender, EventArgs e)
        {
            if (!this.selecting)
            {
                PropertyInfo propertyInfo = this.indicators[this.dataGrid.CurrentRowIndex].GetPropertyInfo(PropertyType.Param0);
                if (propertyInfo != null)
                {
                    if (propertyInfo.PropertyType.Name == "Double")
                    {
                        propertyInfo.SetValue(this.indicators[this.dataGrid.CurrentRowIndex], (double) this.paramNumericUpDown0.Value, null);
                    }
                    else
                    {
                        propertyInfo.SetValue(this.indicators[this.dataGrid.CurrentRowIndex], (int) this.paramNumericUpDown0.Value, null);
                    }
                }
            }
        }

        private void paramNumericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (!this.selecting)
            {
                PropertyInfo propertyInfo = this.indicators[this.dataGrid.CurrentRowIndex].GetPropertyInfo(PropertyType.Param1);
                if (propertyInfo != null)
                {
                    if (propertyInfo.PropertyType.Name == "Double")
                    {
                        propertyInfo.SetValue(this.indicators[this.dataGrid.CurrentRowIndex], (double) this.paramNumericUpDown1.Value, null);
                    }
                    else
                    {
                        propertyInfo.SetValue(this.indicators[this.dataGrid.CurrentRowIndex], (int) this.paramNumericUpDown1.Value, null);
                    }
                }
            }
        }

        private void paramNumericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (!this.selecting)
            {
                PropertyInfo propertyInfo = this.indicators[this.dataGrid.CurrentRowIndex].GetPropertyInfo(PropertyType.Param2);
                if (propertyInfo != null)
                {
                    if (propertyInfo.PropertyType.Name == "Double")
                    {
                        propertyInfo.SetValue(this.indicators[this.dataGrid.CurrentRowIndex], (double) this.paramNumericUpDown2.Value, null);
                    }
                    else
                    {
                        propertyInfo.SetValue(this.indicators[this.dataGrid.CurrentRowIndex], (int) this.paramNumericUpDown2.Value, null);
                    }
                }
            }
        }

        private void paramNumericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            if (!this.selecting)
            {
                PropertyInfo propertyInfo = this.indicators[this.dataGrid.CurrentRowIndex].GetPropertyInfo(PropertyType.Param3);
                if (propertyInfo != null)
                {
                    if (propertyInfo.PropertyType.Name == "Double")
                    {
                        propertyInfo.SetValue(this.indicators[this.dataGrid.CurrentRowIndex], (double) this.paramNumericUpDown3.Value, null);
                    }
                    else
                    {
                        propertyInfo.SetValue(this.indicators[this.dataGrid.CurrentRowIndex], (int) this.paramNumericUpDown3.Value, null);
                    }
                }
            }
        }

        private void Selected(IndicatorBase.IndicatorBase indicator)
        {
            this.HideControls();
            this.selecting = true;
            PropertyInfo propertyInfo = indicator.GetPropertyInfo(PropertyType.BasedOn);
            int num = 0;
            int x = this.basedOnComboBox.Location.X;
            int num3 = this.basedOnLabel.Location.X;
            int y = this.basedOnComboBox.Location.Y;
            int num5 = this.basedOnLabel.Location.Y;
            if (propertyInfo != null)
            {
                this.basedOnLabel.Show();
                this.basedOnComboBox.Show();
                this.basedOnComboBox.SelectedItem = PriceType.All[(PriceTypeId) propertyInfo.GetValue(indicator, null)].Name;
                num += 40;
            }
            PropertyInfo info2 = indicator.GetPropertyInfo(PropertyType.Param0);
            if (info2 != null)
            {
                this.paramLabel0.Location = new Point(num3, num5 + num);
                this.paramLabel0.Show();
                this.paramLabel0.Text = indicator.GetParameterAttribute(PropertyType.Param0).Name + ":";
                this.paramNumericUpDown0.Show();
                this.paramNumericUpDown0.DecimalPlaces = (info2.PropertyType.Name == "Double") ? 2 : 0;
                this.paramNumericUpDown0.Increment = (info2.PropertyType.Name == "Double") ? ((decimal) 0.01) : ((decimal) 1.0);
                this.paramNumericUpDown0.Location = new Point(x, y + num);
                this.paramNumericUpDown0.Maximum = (decimal) indicator.GetParameterAttribute(PropertyType.Param0).MaxValue;
                this.paramNumericUpDown0.Minimum = (decimal) indicator.GetParameterAttribute(PropertyType.Param0).MinValue;
                this.paramNumericUpDown0.Value = Convert.ToDecimal(info2.GetValue(indicator, null), CultureInfo.InvariantCulture);
                num += 40;
            }
            PropertyInfo info3 = indicator.GetPropertyInfo(PropertyType.Param1);
            if (info3 != null)
            {
                this.paramLabel1.Location = new Point(num3, num5 + num);
                this.paramLabel1.Show();
                this.paramLabel1.Text = indicator.GetParameterAttribute(PropertyType.Param1).Name + ":";
                this.paramNumericUpDown1.Show();
                this.paramNumericUpDown1.DecimalPlaces = (info3.PropertyType.Name == "Double") ? 2 : 0;
                this.paramNumericUpDown1.Increment = (info3.PropertyType.Name == "Double") ? ((decimal) 0.01) : ((decimal) 1.0);
                this.paramNumericUpDown1.Location = new Point(x, y + num);
                this.paramNumericUpDown1.Maximum = (decimal) indicator.GetParameterAttribute(PropertyType.Param1).MaxValue;
                this.paramNumericUpDown1.Minimum = (decimal) indicator.GetParameterAttribute(PropertyType.Param1).MinValue;
                this.paramNumericUpDown1.Value = Convert.ToDecimal(info3.GetValue(indicator, null), CultureInfo.InvariantCulture);
                num += 40;
            }
            PropertyInfo info4 = indicator.GetPropertyInfo(PropertyType.Param2);
            if (info4 != null)
            {
                this.paramLabel2.Location = new Point(num3, num5 + num);
                this.paramLabel2.Show();
                this.paramLabel2.Text = indicator.GetParameterAttribute(PropertyType.Param2).Name + ":";
                this.paramNumericUpDown2.Show();
                this.paramNumericUpDown2.DecimalPlaces = (info4.PropertyType.Name == "Double") ? 2 : 0;
                this.paramNumericUpDown2.Increment = (info4.PropertyType.Name == "Double") ? ((decimal) 0.01) : ((decimal) 1.0);
                this.paramNumericUpDown2.Location = new Point(x, y + num);
                this.paramNumericUpDown2.Maximum = (decimal) indicator.GetParameterAttribute(PropertyType.Param2).MaxValue;
                this.paramNumericUpDown2.Minimum = (decimal) indicator.GetParameterAttribute(PropertyType.Param2).MinValue;
                this.paramNumericUpDown2.Value = Convert.ToDecimal(info4.GetValue(indicator, null), CultureInfo.InvariantCulture);
                num += 40;
            }
            PropertyInfo info5 = indicator.GetPropertyInfo(PropertyType.Param3);
            if (info5 != null)
            {
                this.paramLabel3.Location = new Point(num3, num5 + num);
                this.paramLabel3.Show();
                this.paramLabel3.Text = indicator.GetParameterAttribute(PropertyType.Param3).Name + ":";
                this.paramNumericUpDown3.Show();
                this.paramNumericUpDown3.DecimalPlaces = (info5.PropertyType.Name == "Double") ? 2 : 0;
                this.paramNumericUpDown3.Increment = (info5.PropertyType.Name == "Double") ? ((decimal) 0.01) : ((decimal) 1.0);
                this.paramNumericUpDown3.Location = new Point(x, y + num);
                this.paramNumericUpDown3.Maximum = (decimal) indicator.GetParameterAttribute(PropertyType.Param3).MaxValue;
                this.paramNumericUpDown3.Minimum = (decimal) indicator.GetParameterAttribute(PropertyType.Param3).MinValue;
                this.paramNumericUpDown3.Value = Convert.ToDecimal(info5.GetValue(indicator, null), CultureInfo.InvariantCulture);
                num += 40;
            }
            this.selecting = false;
        }

        /// <summary>
        /// </summary>
        public IndicatorCollection Indicators
        {
            get
            {
                return this.indicators;
            }
            set
            {
                this.indicators = value;
            }
        }

        internal Quotes Quotes
        {
            get
            {
                return this.quotes;
            }
            set
            {
                this.quotes = value;
            }
        }
    }
}