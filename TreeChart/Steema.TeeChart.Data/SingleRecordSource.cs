namespace Steema.TeeChart.Data
{
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.Common;
    using System.Drawing;
    using System.Windows.Forms;

    [Description("Fills a Series with fields from a single dataset row."), ToolboxBitmap(typeof(SingleRecordSource), "Images.SingleRecordSource.bmp")]
    public class SingleRecordSource : DataSeriesSource
    {
        private object oDataSource;
        private int position;
        private CurrencyManager recordCurrency;
        private string[] valueMembers;

        public SingleRecordSource()
        {
        }

        public SingleRecordSource(object source, string[] fields) : this()
        {
            this.oDataSource = source;
            this.valueMembers = fields;
        }

        public SingleRecordSource(object source, string[] fields, CurrencyManager manager) : this(source, fields)
        {
            this.RecordCurrency = manager;
        }

        private void AddFromAdapter(IDataAdapter d)
        {
            DataSet dataSet = new DataSet();
            d.Fill(dataSet);
            if (dataSet.Tables.Count > 0)
            {
                this.AddFromTable(dataSet.Tables[0]);
            }
        }

        private void AddFromTable(DataTable d)
        {
            if ((this.valueMembers != null) && (this.valueMembers.Length > 0))
            {
                int num = (this.RecordCurrency != null) ? this.RecordCurrency.Position : this.position;
                if ((num >= 0) && (num < d.Rows.Count))
                {
                    DataRow row = d.Rows[num];
                    foreach (string str in this.valueMembers)
                    {
                        DataColumn column = d.Columns[str];
                        if (row.IsNull(column))
                        {
                            base.Series.Add(column.Caption);
                        }
                        else
                        {
                            base.Series.Add(Convert.ToDouble(row[column]), column.Caption);
                        }
                    }
                }
            }
        }

        public override void RefreshData()
        {
            base.Series.BeginUpdate();
            try
            {
                base.Series.Clear();
                if (this.DataSource is DataTable)
                {
                    this.AddFromTable((DataTable) this.DataSource);
                }
                else if (this.DataSource is DataSet)
                {
                    DataSet dataSource = (DataSet) this.DataSource;
                    if (dataSource.Tables.Count > 0)
                    {
                        this.AddFromTable(dataSource.Tables[0]);
                    }
                }
                else if (this.DataSource is DataView)
                {
                    this.AddFromTable(((DataView) this.DataSource).Table);
                }
                else if (this.DataSource is IDbDataAdapter)
                {
                    IDbDataAdapter d = (IDbDataAdapter) this.DataSource;
                    if (d.SelectCommand != null)
                    {
                        this.AddFromAdapter(d);
                    }
                }
                else if (this.DataSource is DataAdapter)
                {
                    this.AddFromAdapter((DataAdapter) this.DataSource);
                }
            }
            finally
            {
                base.Series.EndUpdate();
            }
        }

        [Description("Stores the Datasource associated with this SingleRecordSource")]
        public object DataSource
        {
            get
            {
                return this.oDataSource;
            }
            set
            {
                this.oDataSource = value;
            }
        }

        [Description("Indicates which dataset row to use to fill series."), DefaultValue(0)]
        public int Position
        {
            get
            {
                return this.position;
            }
            set
            {
                if (this.position != value)
                {
                    this.position = value;
                    if (base.Series != null)
                    {
                        base.Series.CheckDataSource();
                    }
                }
            }
        }

        [Description("Set RecordCurrency to the Currency Manager to track the Datasource position"), DefaultValue((string) null)]
        public CurrencyManager RecordCurrency
        {
            get
            {
                return this.recordCurrency;
            }
            set
            {
                this.recordCurrency = value;
            }
        }

        [Description("Stores field names for the SingleRecordSource"), DefaultValue((string) null)]
        public string[] ValueMembers
        {
            get
            {
                return this.valueMembers;
            }
            set
            {
                this.valueMembers = value;
            }
        }
    }
}

