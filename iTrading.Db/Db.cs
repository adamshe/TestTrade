namespace iTrading.Db
{
    using System;
    using System.Data;
    using System.Data.Odbc;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Text.RegularExpressions;
    using iTrading.Core.Kernel;

    internal class Db
    {
        private static DateTime jetStartDate = new DateTime(0x76b, 12, 30);
        private static Regex replaceAt = new Regex("@[A-Za-z0-9_]*");

        internal static void AddParameter(IDbCommand cmd, string name, string type)
        {
            AddParameter(cmd, name, type, 0);
        }

        internal static void AddParameter(IDbCommand cmd, string name, string type, int length)
        {
            if (Adapter.connectionType == ConnectionTypeId.MySql)
            {
                switch (type)
                {
                    case "Binary":
                        ((OdbcCommand) cmd).Parameters.Add(name, OdbcType.Binary);
                        return;

                    case "DateTime":
                        ((OdbcCommand) cmd).Parameters.Add(name, OdbcType.DateTime);
                        return;

                    case "Double":
                        ((OdbcCommand) cmd).Parameters.Add(name, OdbcType.Double);
                        return;

                    case "Integer":
                        ((OdbcCommand) cmd).Parameters.Add(name, OdbcType.Int);
                        return;

                    case "LongVarChar":
                        ((OdbcCommand) cmd).Parameters.Add(name, OdbcType.NText, length);
                        return;

                    case "VarChar":
                        ((OdbcCommand) cmd).Parameters.Add(name, OdbcType.NVarChar, length);
                        return;
                }
                Trace.Assert(false, "Db.Adapter.DbAddParameter: unknown type '" + type + "'");
            }
            else if (Adapter.connectionType == ConnectionTypeId.OleDb)
            {
                switch (type)
                {
                    case "Binary":
                        ((OleDbCommand) cmd).Parameters.Add(name, OleDbType.Binary, length);
                        return;

                    case "DateTime":
                        ((OleDbCommand) cmd).Parameters.Add(name, OleDbType.Double);
                        return;

                    case "Double":
                        ((OleDbCommand) cmd).Parameters.Add(name, OleDbType.Double);
                        return;

                    case "Integer":
                        ((OleDbCommand) cmd).Parameters.Add(name, OleDbType.Integer);
                        return;

                    case "LongVarChar":
                        ((OleDbCommand) cmd).Parameters.Add(name, OleDbType.LongVarChar, length);
                        return;

                    case "VarChar":
                        ((OleDbCommand) cmd).Parameters.Add(name, OleDbType.VarChar, length);
                        return;
                }
                Trace.Assert(false, "Db.Adapter.DbAddParameter: unknown type '" + type + "'");
            }
            else if (Adapter.connectionType == ConnectionTypeId.SqlClient)
            {
                switch (type)
                {
                    case "Binary":
                        ((SqlCommand) cmd).Parameters.Add(name, SqlDbType.Binary);
                        return;

                    case "DateTime":
                        ((SqlCommand) cmd).Parameters.Add(name, SqlDbType.DateTime);
                        return;

                    case "Double":
                        ((SqlCommand) cmd).Parameters.Add(name, SqlDbType.Float);
                        return;

                    case "Integer":
                        ((SqlCommand) cmd).Parameters.Add(name, SqlDbType.Int);
                        return;

                    case "LongVarChar":
                        ((SqlCommand) cmd).Parameters.Add(name, SqlDbType.NVarChar, length);
                        return;

                    case "VarChar":
                        ((SqlCommand) cmd).Parameters.Add(name, SqlDbType.NVarChar, length);
                        return;
                }
                Trace.Assert(false, "Db.Adapter.DbAddParameter: unknown type '" + type + "'");
            }
            else
            {
                Trace.Assert(false, "Db.Adapter.DbAddParameter: unknown connection type " + ((int) Adapter.connectionType));
            }
        }

        internal static object ConvertDateTime(DateTime val)
        {
            if (val < Globals.MinDate)
            {
                val = Globals.MinDate;
            }
            switch (Adapter.connectionType)
            {
                case ConnectionTypeId.MySql:
                    return val;

                case ConnectionTypeId.OleDb:
                    return val.Subtract(jetStartDate).TotalDays;

                case ConnectionTypeId.SqlClient:
                    return val;
            }
            Trace.Assert(false, "Db.Adapter.DbDropIndex: unknown connection type " + ((int) Adapter.connectionType));
            return val;
        }

        internal static void Create(string cmdString)
        {
            switch (Adapter.connectionType)
            {
                case ConnectionTypeId.MySql:
                    cmdString = cmdString.Replace("%alter%", "modify");
                    cmdString = cmdString.Replace("%binary%", "longtext");
                    cmdString = cmdString.Replace("%counter%", "int not null auto_increment");
                    cmdString = cmdString.Replace("%double%", "double");
                    cmdString = cmdString.Replace("%long%", "int");
                    cmdString = cmdString.Replace("%memo%", "text");
                    cmdString = cmdString.Replace("%varchar%", "varchar");
                    break;

                case ConnectionTypeId.OleDb:
                    cmdString = cmdString.Replace("%alter%", "alter");
                    cmdString = cmdString.Replace("%binary%", "image");
                    cmdString = cmdString.Replace("%counter%", "counter");
                    cmdString = cmdString.Replace("%double%", "double");
                    cmdString = cmdString.Replace("%long%", "long");
                    cmdString = cmdString.Replace("%memo%", "memo");
                    cmdString = cmdString.Replace("%varchar%", "varchar");
                    break;

                case ConnectionTypeId.SqlClient:
                    cmdString = cmdString.Replace("%alter%", "alter");
                    cmdString = cmdString.Replace("%binary%", "image");
                    cmdString = cmdString.Replace("%counter%", "int identity");
                    cmdString = cmdString.Replace("%double%", "float");
                    cmdString = cmdString.Replace("%long%", "int");
                    cmdString = cmdString.Replace("%memo%", "nvarchar(4000)");
                    cmdString = cmdString.Replace("%varchar%", "nvarchar");
                    break;

                default:
                    Trace.Assert(false, "Db.Adapter.DbCreate: unknown connection type " + ((int) Adapter.connectionType));
                    return;
            }
            NewCommand(cmdString).ExecuteNonQuery();
        }

        internal static void DropIndex(string table, string index)
        {
            switch (Adapter.connectionType)
            {
                case ConnectionTypeId.MySql:
                    Create("drop index " + index + " on " + table);
                    return;

                case ConnectionTypeId.OleDb:
                    Create("drop index " + index + " on " + table);
                    return;

                case ConnectionTypeId.SqlClient:
                    Create("drop index " + table + "." + index);
                    return;
            }
            Trace.Assert(false, "Db.Adapter.DbDropIndex: unknown connection type " + ((int) Adapter.connectionType));
        }

        internal static IDataParameter GetParameter(IDbCommand cmd, string parameter)
        {
            switch (Adapter.connectionType)
            {
                case ConnectionTypeId.MySql:
                    return ((OdbcCommand) cmd).Parameters[parameter];

                case ConnectionTypeId.OleDb:
                    return ((OleDbCommand) cmd).Parameters[parameter];

                case ConnectionTypeId.SqlClient:
                    return ((SqlCommand) cmd).Parameters[parameter];
            }
            Trace.Assert(false, "Db.Adapter.DbGetParameter: unknown connection type " + ((int) Adapter.connectionType));
            return null;
        }

        internal static IDbCommand NewCommand(string cmd)
        {
            switch (Adapter.connectionType)
            {
                case ConnectionTypeId.MySql:
                    cmd = replaceAt.Replace(cmd, "?");
                    return new OdbcCommand(cmd, (OdbcConnection) Adapter.iDbConnection);

                case ConnectionTypeId.OleDb:
                    return new OleDbCommand(cmd, (OleDbConnection) Adapter.iDbConnection);

                case ConnectionTypeId.SqlClient:
                    return new SqlCommand(cmd, (SqlConnection) Adapter.iDbConnection);
            }
            Trace.Assert(false, "Db.Adapter.DbCreateCommand: unknown connection type " + ((int) Adapter.connectionType));
            return null;
        }

        internal static IDbCommand NewCommand(string cmd, IDbTransaction transaction)
        {
            switch (Adapter.connectionType)
            {
                case ConnectionTypeId.MySql:
                    cmd = replaceAt.Replace(cmd, "?");
                    return new OdbcCommand(cmd, (OdbcConnection) Adapter.iDbConnection, (OdbcTransaction) transaction);

                case ConnectionTypeId.OleDb:
                    return new OleDbCommand(cmd, (OleDbConnection) Adapter.iDbConnection, (OleDbTransaction) transaction);

                case ConnectionTypeId.SqlClient:
                    return new SqlCommand(cmd, (SqlConnection) Adapter.iDbConnection, (SqlTransaction) transaction);
            }
            Trace.Assert(false, "Db.Adapter.DbCreateCommand: unknown connection type " + ((int) Adapter.connectionType));
            return null;
        }

        internal static IDbConnection NewConnection(string connectionString)
        {
            switch (Adapter.connectionType)
            {
                case ConnectionTypeId.MySql:
                    return new OdbcConnection(connectionString);

                case ConnectionTypeId.OleDb:
                    return new OleDbConnection(connectionString);

                case ConnectionTypeId.SqlClient:
                    return new SqlConnection(connectionString);
            }
            Trace.Assert(false, "Db.Adapter.DbCreateConnection: unknown connection type " + ((int) Adapter.connectionType));
            return null;
        }

        internal static bool TableExists(string tableName)
        {
            switch (Adapter.connectionType)
            {
                case ConnectionTypeId.MySql:
                {
                    IDataReader reader = NewCommand("show tables like '" + tableName + "'").ExecuteReader();
                    if (!reader.Read())
                    {
                        reader.Close();
                        return false;
                    }
                    reader.Close();
                    return true;
                }
                case ConnectionTypeId.OleDb:
                {
                    object[] restrictions = new object[4];
                    restrictions[2] = tableName;
                    restrictions[3] = "TABLE";
                    return (((OleDbConnection) Adapter.iDbConnection).GetOleDbSchemaTable(OleDbSchemaGuid.Tables, restrictions).Rows.Count > 0);
                }
                case ConnectionTypeId.SqlClient:
                {
                    IDataReader reader2 = NewCommand("select * from sysobjects where type='U' and name='" + tableName + "'").ExecuteReader();
                    if (!reader2.Read())
                    {
                        reader2.Close();
                        return false;
                    }
                    reader2.Close();
                    return true;
                }
            }
            Trace.Assert(false, "Db.Adapter.DbTableExists: unknown connection type " + ((int) Adapter.connectionType));
            return false;
        }
    }
}

