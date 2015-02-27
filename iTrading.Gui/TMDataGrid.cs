namespace iTrading.Gui
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Data;
    using System.Windows.Forms;

    /// <summary>
    /// Special datagrid. Columns are described by <see cref="T:iTrading.Gui.ColumnStyle" />.
    /// </summary>
    public class TMDataGrid : DataGrid
    {
        private iTrading.Gui.ColumnStyle[] columnStyles = null;
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private Container components = null;
        private System.Data.DataTable dataTable = null;
        private System.Data.DataView dataView = null;

        /// <summary>
        /// This event will be thrown before a row is deleted. Set <see cref="P:iTrading.Gui.RowDeletingEventArgs.Cancel" />
        /// to TRUE, to abort the deletion.
        /// </summary>
        public event RowDeletingEventHandler RowDeleting;

        /// <summary>
        /// </summary>
        public TMDataGrid()
        {
            this.InitializeComponent();
            base.SizeChanged += new EventHandler(this.DataGrid_SizeChanged);
            base.VisibleChanged += new EventHandler(this.DataGrid_VisibleChanged);
        }

        private void DataGrid_SizeChanged(object sender, EventArgs e)
        {
            this.Rearrange();
        }

        private void DataGrid_VisibleChanged(object sender, EventArgs e)
        {
            this.Rearrange();
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

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new Container();
        }

        /// <summary></summary>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if ((keyData != Keys.Delete) || (this.RowDeleting == null))
            {
                return base.ProcessDialogKey(keyData);
            }
            ArrayList dataRows = new ArrayList();
            for (int i = 0; i < this.DataTable.Rows.Count; i++)
            {
                if (base.IsSelected(i))
                {
                    dataRows.Add(this.DataTable.Rows[i]);
                }
            }
            if (dataRows.Count > 0)
            {
                RowDeletingEventArgs e = new RowDeletingEventArgs(dataRows);
                this.RowDeleting(this, e);
                if (!e.Cancel)
                {
                    foreach (DataRow row in dataRows)
                    {
                        this.DataTable.Rows.Remove(row);
                    }
                }
            }
            return true;
        }

        private void Rearrange()
        {
            if (base.Visible && (this.DataTable != null))
            {
                int num = 0;
                int num2 = 0;
                int num3 = base.ClientSize.Width - (2 * SystemInformation.Border3DSize.Width);
                if (base.VisibleRowCount < this.DataView.Count)
                {
                    num3 -= SystemInformation.VerticalScrollBarWidth;
                }
                if (base.RowHeadersVisible)
                {
                    num3 -= base.RowHeaderWidth;
                }
                if (base.TableStyles.Contains(this.DataTable.TableName))
                {
                    foreach (iTrading.Gui.ColumnStyle style in this.ColumnStyles)
                    {
                        num += style.Width;
                    }
                    foreach (iTrading.Gui.ColumnStyle style2 in this.ColumnStyles)
                    {
                        int num4 = ((style2.Width * num3) / num) - 1;
                        base.TableStyles[this.DataTable.TableName].GridColumnStyles[style2.Name].Width = num4;
                        num2 += num4;
                    }
                    DataGridColumnStyle style1 = base.TableStyles[this.DataTable.TableName].GridColumnStyles[this.ColumnStyles[0].Name];
                    style1.Width += num3 - num2;
                }
            }
        }

        /// <summary>
        /// Get/set the columns styles.
        /// </summary>
        public iTrading.Gui.ColumnStyle[] ColumnStyles
        {
            get
            {
                return this.columnStyles;
            }
            set
            {
                if (value != null)
                {
                    this.columnStyles = value;
                    this.dataTable = new System.Data.DataTable();
                    DataGridTableStyle table = new DataGridTableStyle();
                    foreach (iTrading.Gui.ColumnStyle style2 in value)
                    {
                        DataColumn column = new DataColumn();
                        column.DataType = style2.Type;
                        column.ColumnName = style2.Name;
                        this.DataTable.Columns.Add(column);
                        DataGridTextBoxColumn column2 = style2.ReadOnly ? new DataGridNoActiveCellColumn() : new DataGridTextBoxColumn();
                        column2.Alignment = style2.Alignment;
                        column2.HeaderText = style2.Name;
                        column2.MappingName = style2.Name;
                        column2.NullText = "";
                        column2.ReadOnly = style2.ReadOnly;
                        column2.Width = style2.Width;
                        table.GridColumnStyles.Add(column2);
                    }
                    table.RowHeadersVisible = base.RowHeadersVisible;
                    base.TableStyles.Add(table);
                    this.dataView = this.DataTable.DefaultView;
                    base.DataSource = this.dataView;
                }
            }
        }

        /// <summary>
        /// Get the datatable of the <see cref="T:iTrading.Gui.TMDataGrid" />.
        /// </summary>
        public System.Data.DataTable DataTable
        {
            get
            {
                return this.dataTable;
            }
        }

        /// <summary>
        /// Get the dataview of the <see cref="T:iTrading.Gui.TMDataGrid" />.
        /// </summary>
        public System.Data.DataView DataView
        {
            get
            {
                return this.dataView;
            }
        }
    }
}

