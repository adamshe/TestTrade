using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace iTrading.Core.Gui
{
    /// <summary>
    /// </summary>
    public class DataGridComboBoxColumn : DataGridTextBoxColumn
    {
        private CurrencyManager cm = null;
        private System.Windows.Forms.ComboBox comboBox = new System.Windows.Forms.ComboBox();
        private int iCurrentRow;

        /// <summary>
        /// </summary>
        public DataGridComboBoxColumn()
        {
            this.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBox.Leave += new EventHandler(this.comboBox_Leave);
            this.comboBox.SelectedIndexChanged += new EventHandler(this.comboBox_SelectedIndexChanged);
        }

        private void comboBox_Leave(object sender, EventArgs e)
        {
            if (this.comboBox.SelectedItem != null)
            {
                ((DataView) this.cm.List).Table.Rows[this.iCurrentRow][base.MappingName] = this.comboBox.SelectedItem;
                this.Invalidate();
            }
            this.comboBox.Hide();
            this.DataGridTableStyle.DataGrid.Scroll -= new EventHandler(this.DataGrid_Scroll);
        }

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((this.comboBox.SelectedItem != null) && (this.iCurrentRow >= ((DataView) this.cm.List).Table.Rows.Count))
            {
                ((DataView) this.cm.List).Table.Rows.Add(((DataView) this.cm.List).Table.NewRow());
            }
        }

        private void DataGrid_Scroll(object sender, EventArgs e)
        {
            this.comboBox.Hide();
        }

        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="rowNum"></param>
        /// <param name="bounds"></param>
        /// <param name="readOnly"></param>
        /// <param name="instantText"></param>
        /// <param name="cellIsVisible"></param>
        protected override void Edit(CurrencyManager source, int rowNum, Rectangle bounds, bool readOnly, string instantText, bool cellIsVisible)
        {
            base.Edit(source, rowNum, bounds, readOnly, instantText, cellIsVisible);
            if (!readOnly && cellIsVisible)
            {
                this.iCurrentRow = rowNum;
                this.cm = source;
                this.DataGridTableStyle.DataGrid.Scroll += new EventHandler(this.DataGrid_Scroll);
                this.comboBox.Parent = this.TextBox.Parent;
                Rectangle currentCellBounds = this.DataGridTableStyle.DataGrid.GetCurrentCellBounds();
                this.comboBox.Location = currentCellBounds.Location;
                this.comboBox.Size = new Size(this.TextBox.Size.Width + 2, this.comboBox.Size.Height);
                this.comboBox.SelectedIndex = this.comboBox.FindStringExact(this.TextBox.Text);
                this.comboBox.Show();
                this.comboBox.BringToFront();
                this.comboBox.Focus();
            }
        }

        /// <summary>
        /// </summary>
        public System.Windows.Forms.ComboBox ComboBox
        {
            get
            {
                return this.comboBox;
            }
        }
    }
}