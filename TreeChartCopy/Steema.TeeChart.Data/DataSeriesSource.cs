namespace Steema.TeeChart.Data
{
    using Steema.TeeChart.Styles;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Data;
    using System.Data.Common;
    using System.Reflection;
    using System.Windows.Forms;

    [ToolboxItem(false)]
    public abstract class DataSeriesSource : SeriesSource
    {
        protected DataSeriesSource()
        {
        }

        private static void AddColumns(DataColumnCollection co, ListControl combo, ArrayList combos)
        {
            foreach (DataColumn column in co)
            {
                if (combo is ListBox)
                {
                    ((ListBox) combo).Items.Add(column.ColumnName);
                }
                else if (combo is ComboBox)
                {
                    ((ComboBox) combo).Items.Add(column.ColumnName);
                }
                if (combos != null)
                {
                    foreach (ComboBox box in combos)
                    {
                        box.Items.Add(column.ColumnName);
                    }
                    continue;
                }
            }
        }

        private static void AddFromAdapter(Series s, IDataAdapter d)
        {
            DataSet dataSet = new DataSet();
            d.Fill(dataSet);
            if (dataSet.Tables.Count > 0)
            {
                AddTable(s, dataSet.Tables[0]);
            }
        }

        private static void AddTable(Series s, DataTable t)
        {
            s.BeginUpdate();
            s.Clear();
            s.Add(t);
            s.EndUpdate();
        }

        internal static void CheckSeriesSource(Series s, ComboBox c)
        {
            ArrayList list = new ArrayList();
            DataTable table = null;
            if (s.DataSource is DataView)
            {
                DataView dataSource = (DataView) s.DataSource;
                if (dataSource.Table != null)
                {
                    table = dataSource.Table;
                }
                list.Add(table);
            }
            else if (s.DataSource is DataTable)
            {
                table = (DataTable) s.DataSource;
                list.Add(table);
            }
            else if (s.DataSource is DataSet)
            {
                DataSet set = (DataSet) s.DataSource;
                foreach (DataTable table2 in set.Tables)
                {
                    list.Add(table2);
                }
            }
            else if (s.DataSource is IDbDataAdapter)
            {
                IDbDataAdapter adapter = (IDbDataAdapter) s.DataSource;
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);
                foreach (DataTable table3 in dataSet.Tables)
                {
                    list.Add(table3);
                }
            }
            foreach (DataTable table4 in list)
            {
                foreach (object obj2 in c.Items)
                {
                    if (table4 == obj2)
                    {
                        c.SelectedItem = obj2;
                        return;
                    }
                }
            }
        }

        internal static void FillFields(object source, ListControl combo, ArrayList combos)
        {
            if (source is IDataReader)
            {
                AddColumns((source as IDataReader).GetSchemaTable().Columns, combo, combos);
            }
            else if (source is DataAdapter)
            {
                DataAdapter adapter = source as DataAdapter;
                DataSet dataSet = new DataSet();
                DataTable[] tableArray = adapter.FillSchema(dataSet, SchemaType.Mapped);
                if (tableArray.Length > 0)
                {
                    AddColumns(tableArray[0].Columns, combo, combos);
                }
            }
            else if (source is DataSet)
            {
                DataTableCollection tables = (source as DataSet).Tables;
                if (tables.Count > 0)
                {
                    AddColumns(tables[0].Columns, combo, combos);
                }
            }
            else if (source is DataTable)
            {
                AddColumns(((DataTable) source).Columns, combo, combos);
            }
            else if (source is DataView)
            {
                AddColumns((source as DataView).Table.Columns, combo, combos);
            }
        }

        internal static void FillSeries(Series s, IDataReader r)
        {
            if (!r.IsClosed)
            {
                int ordinal;
                if (s.mandatory.DataMember.Length == 0)
                {
                    ordinal = 0;
                }
                else
                {
                    ordinal = r.GetOrdinal(s.mandatory.DataMember);
                }
                while (r.Read())
                {
                    if (r.IsDBNull(ordinal))
                    {
                        s.Add();
                    }
                    else
                    {
                        s.Add(Convert.ToDouble(r.GetValue(ordinal)));
                    }
                }
            }
        }

        internal static void FillSources(Series s, ComboBox c)
        {
            c.Items.Clear();
            IContainer chartContainer = s.chart.ChartContainer;
            if (chartContainer != null)
            {
                foreach (object obj2 in chartContainer.Components)
                {
                    if (!IsValidSource(obj2))
                    {
                        continue;
                    }
                    if ((obj2 is DataSet) && (((DataSet) obj2).Tables.Count > 0))
                    {
                        foreach (DataTable table in ((DataSet) obj2).Tables)
                        {
                            if (c.Items.IndexOf(table) == -1)
                            {
                                c.Items.Add(table);
                            }
                        }
                        if (c.Items.IndexOf(obj2) == -1)
                        {
                            c.Items.Add(obj2);
                        }
                        continue;
                    }
                    c.Items.Add(obj2);
                }
            }
            else
            {
                FillSourcesReflection(s, c);
            }
        }

        private static void FillSourcesReflection(Series s, ComboBox c)
        {
            if (s.chart.parent != null)
            {
                Form form = s.Chart.parent.FindParentForm();
                if (form != null)
                {
                    System.Type baseType = form.GetType();
                    do
                    {
                        foreach (FieldInfo info in baseType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                        {
                            object obj2 = info.GetValue(form);
                            if (IsValidSource(obj2))
                            {
                                if ((obj2 is DataSet) && (((DataSet) obj2).Tables.Count > 0))
                                {
                                    foreach (DataTable table in ((DataSet) obj2).Tables)
                                    {
                                        if (c.Items.IndexOf(table) == -1)
                                        {
                                            c.Items.Add(table);
                                        }
                                    }
                                }
                                else if (obj2 is DataTable)
                                {
                                    if (c.Items.IndexOf((DataTable) obj2) == -1)
                                    {
                                        c.Items.Add((DataTable) obj2);
                                    }
                                }
                                else if (c.Items.IndexOf(obj2) == -1)
                                {
                                    c.Items.Add(obj2);
                                }
                            }
                        }
                        baseType = baseType.BaseType;
                    }
                    while (!baseType.Equals(typeof(Form)));
                }
                if (c.Items.Count > 0)
                {
                    CheckSeriesSource(s, c);
                }
            }
        }

        public static bool IsValidSource(object c)
        {
            return ((((c is DataSet) || (c is DataAdapter)) || (c is DataTable)) || (c is DataView));
        }

        internal static bool TryRefreshData(Series s)
        {
            object dataSource = s.DataSource;
            if (dataSource is DataTable)
            {
                AddTable(s, (DataTable) dataSource);
                return true;
            }
            if (dataSource is DataSet)
            {
                DataSet set = (DataSet) dataSource;
                if (set.Tables.Count > 0)
                {
                    AddTable(s, set.Tables[0]);
                }
                return true;
            }
            if (dataSource is DataView)
            {
                s.BeginUpdate();
                s.Clear();
                s.Add((DataView) dataSource);
                s.EndUpdate();
                return true;
            }
            if (dataSource is IDataReader)
            {
                s.BeginUpdate();
                s.Clear();
                s.Add((IDataReader) dataSource);
                s.EndUpdate();
                return true;
            }
            if (dataSource is IBindingList)
            {
                IBindingList list1 = (IBindingList) dataSource;
            }
            else
            {
                if (dataSource is IList)
                {
                    s.BeginUpdate();
                    s.Clear();
                    s.Add((IList) dataSource);
                    s.EndUpdate();
                    return true;
                }
                if (dataSource is IDbDataAdapter)
                {
                    IDbDataAdapter d = (IDbDataAdapter) dataSource;
                    if (d.SelectCommand != null)
                    {
                        AddFromAdapter(s, d);
                    }
                    return true;
                }
                if (dataSource is DataAdapter)
                {
                    AddFromAdapter(s, (DataAdapter) dataSource);
                    return true;
                }
            }
            return false;
        }
    }
}

