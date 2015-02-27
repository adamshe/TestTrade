namespace Steema.TeeChart.Data
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.Common;
    using System.Drawing;

    [ToolboxItem(false)]
    public class SeriesDataAdapter : DbDataAdapter
    {
        private Steema.TeeChart.Chart chart;
        private Steema.TeeChart.Styles.Series series;

        public SeriesDataAdapter() : this((Steema.TeeChart.Styles.Series) null)
        {
        }

        public SeriesDataAdapter(Steema.TeeChart.Chart c)
        {
            this.chart = c;
        }

        public SeriesDataAdapter(Steema.TeeChart.Styles.Series s)
        {
            this.series = s;
        }

        private void AddColumns(DataTable d)
        {
            d.Clear();
            d.Columns.Clear();
            DataColumn column = new DataColumn(Texts.Index);
            column.AllowDBNull = false;
            column.AutoIncrement = true;
            column.AutoIncrementSeed = 0L;
            column.AutoIncrementStep = 1L;
            column.ReadOnly = true;
            column.Unique = true;
            d.Columns.Add(column);
            d.Columns.Add(Texts.Text, typeof(string));
            d.Columns.Add(Texts.Colors, typeof(Color));
            if (this.series != null)
            {
                foreach (ValueList list in this.series.ValuesLists)
                {
                    if (list.DateTime)
                    {
                        d.Columns.Add(list.Name, typeof(DateTime));
                    }
                    else
                    {
                        d.Columns.Add(list.Name, typeof(double));
                    }
                }
            }
        }

        protected override RowUpdatedEventArgs CreateRowUpdatedEvent(DataRow dataRow, IDbCommand command, StatementType statementType, DataTableMapping tableMapping)
        {
            return null;
        }

        protected override RowUpdatingEventArgs CreateRowUpdatingEvent(DataRow dataRow, IDbCommand command, StatementType statementType, DataTableMapping tableMapping)
        {
            return null;
        }

        public override int Fill(DataSet dataSet)
        {
            if (this.series == null)
            {
                return 0;
            }
            dataSet.Tables.Clear();
            DataTable d = dataSet.Tables.Add(this.series.ToString());
            this.AddColumns(d);
            d.Rows.Clear();
            object[] values = new object[3 + this.series.ValuesLists.Count];
            for (int i = 0; i < this.series.Count; i++)
            {
                values[0] = i;
                values[1] = this.series.Labels[i];
                values[2] = this.series.ValueColor(i);
                for (int j = 0; j < this.series.ValuesLists.Count; j++)
                {
                    if (this.series.ValuesLists[j].DateTime)
                    {
                        values[3 + j] = this.series.ValuesLists[j].AsDateTime(i);
                    }
                    else
                    {
                        values[3 + j] = this.series.ValuesLists[j].Value[i];
                    }
                }
                d.Rows.Add(values);
            }
            d.RowChanged += new DataRowChangeEventHandler(this.RowChanged);
            return this.series.Count;
        }

        public override DataTable[] FillSchema(DataSet dataSet, SchemaType schemaType)
        {
            DataTable[] tableArray = new DataTable[] { new DataTable("Table") };
            this.AddColumns(tableArray[0]);
            return tableArray;
        }

        protected override void OnRowUpdated(RowUpdatedEventArgs value)
        {
        }

        protected override void OnRowUpdating(RowUpdatingEventArgs value)
        {
        }

        private void RowChanged(object sender, DataRowChangeEventArgs e)
        {
            if (e.Action == DataRowAction.Change)
            {
                int num = (int) e.Row[Texts.Index];
                this.series.Labels[num] = (string) e.Row[Texts.Text];
                this.series.Colors[num] = (Color) e.Row[Texts.Colors];
                foreach (ValueList list in this.series.ValuesLists)
                {
                    if (list.DateTime)
                    {
                        list[num] = ((DateTime) e.Row[list.Name]).ToOADate();
                    }
                    else
                    {
                        list[num] = (double) e.Row[list.Name];
                    }
                }
                this.series.Invalidate();
            }
        }

        protected override bool ShouldSerializeTableMappings()
        {
            return false;
        }

        public Steema.TeeChart.Chart Chart
        {
            get
            {
                return this.chart;
            }
            set
            {
                this.chart = value;
            }
        }

        public Steema.TeeChart.Styles.Series Series
        {
            get
            {
                return this.series;
            }
            set
            {
                this.series = value;
            }
        }
    }
}

