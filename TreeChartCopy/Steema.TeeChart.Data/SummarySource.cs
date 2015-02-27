namespace Steema.TeeChart.Data
{
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.Common;
    using System.Drawing;

    [Description("Fills a Series with specified summary fields from dataset."), ToolboxBitmap(typeof(SummarySource), "Images.SingleRecordSource.bmp")]
    public class SummarySource : DataSeriesSource
    {
        private object oDataSource;

        public SummarySource()
        {
        }

        public SummarySource(object source) : this()
        {
            this.oDataSource = source;
        }

        private void AddFromAdapter(IDataAdapter d)
        {
        }

        private void AddFromTable(DataTable d)
        {
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

        private DataGroup StrToDBGroup(string St)
        {
            St = St.ToUpper();
            if (St == "HOUR")
            {
                return DataGroup.Hour;
            }
            if (St == "DAY")
            {
                return DataGroup.Day;
            }
            if (St == "WEEK")
            {
                return DataGroup.Week;
            }
            if (St == "WEEKDAY")
            {
                return DataGroup.WeekDay;
            }
            if (St == "MONTH")
            {
                return DataGroup.Month;
            }
            if (St == "QUARTER")
            {
                return DataGroup.Quarter;
            }
            if (St == "YEAR")
            {
                return DataGroup.Year;
            }
            return DataGroup.None;
        }

        private ValueListOrder StrToDBOrder(string St)
        {
            St = St.ToUpper();
            if (St == "SORTASC")
            {
                return ValueListOrder.Ascending;
            }
            if (St == "SORTDES")
            {
                return ValueListOrder.Descending;
            }
            return ValueListOrder.None;
        }

        private string TeeGetDBPart(int Num, string St)
        {
            string str = "";
            if (St.StartsWith("#"))
            {
                St = St.Remove(0, 1);
                int index = St.IndexOf("#", 0);
                if (index <= 0)
                {
                    return str;
                }
                if (Num == 1)
                {
                    return St.Substring(0, index - 1);
                }
                if (Num == 2)
                {
                    str = St.Substring(index + 1, St.Length - index);
                }
            }
            return str;
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
    }
}

