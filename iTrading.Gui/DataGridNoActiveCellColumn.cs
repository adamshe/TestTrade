namespace iTrading.Gui
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    /// This class disables editing of cells and allows entire rows to be selected.
    /// </summary>
    public class DataGridNoActiveCellColumn : DataGridTextBoxColumn
    {
        private int SelectedRow = -1;

        /// <summary>
        /// Disables Editing and selects entire rows.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="rowNum"></param>
        /// <param name="bounds"></param>
        /// <param name="readOnly"></param>
        /// <param name="instantText"></param>
        /// <param name="cellIsVisible"></param>
        protected override void Edit(CurrencyManager source, int rowNum, Rectangle bounds, bool readOnly, string instantText, bool cellIsVisible)
        {
            if ((this.SelectedRow > -1) && (this.SelectedRow < source.List.Count))
            {
                this.DataGridTableStyle.DataGrid.UnSelect(this.SelectedRow);
            }
            this.SelectedRow = rowNum;
            this.DataGridTableStyle.DataGrid.Select(this.SelectedRow);
        }
    }
}

