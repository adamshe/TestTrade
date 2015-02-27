namespace iTrading.Db
{
    using System;
    using System.Collections;
    using System.Data;
    using System.Diagnostics;
    using System.Text;
    using iTrading.Core.Kernel;

    internal class Account
    {
        private static IDbCommand insertAccount = null;
        private static IDbCommand selectAccount = null;
        private static IDbCommand updateAccount = null;

        internal static bool Delete(iTrading.Core.Kernel.Account account)
        {
            if (Globals.TraceSwitch.DataBase)
            {
                Trace.WriteLine("(Db) Db.Account.Delete account='" + account.Name + "'");
            }
            Adapter.BeginTransactionNow();
            int accountId = GetAccountId(account);
            if (accountId >= 0)
            {
                ArrayList list = new ArrayList();
                IDataReader reader = iTrading.Db.Db.NewCommand("select token from tm_orders where account = " + accountId, Adapter.iDbTransaction).ExecuteReader();
                while (reader.Read())
                {
                    list.Add((string) reader["token"]);
                }
                reader.Close();
                foreach (string str in list)
                {
                    iTrading.Db.Db.NewCommand("delete from tm_orderhistories where token = '" + str + "'", Adapter.iDbTransaction).ExecuteNonQuery();
                }
                iTrading.Db.Db.NewCommand("delete from tm_orders where account = " + accountId, Adapter.iDbTransaction).ExecuteNonQuery();
                iTrading.Db.Db.NewCommand("delete from tm_executions where account = " + accountId, Adapter.iDbTransaction).ExecuteNonQuery();
                Adapter.CommitTransactionNow();
            }
            return true;
        }

        internal static int GetAccountId(iTrading.Core.Kernel.Account account)
        {
            if (Globals.TraceSwitch.DataBase)
            {
                Trace.WriteLine("(Db) Db.Adapter.GetAccountId account='" + account.Name + "'");
            }
            if (selectAccount == null)
            {
                selectAccount = iTrading.Db.Db.NewCommand("select id, numbytes, customtext from tm_accounts where account = @account and broker = @broker and mode = @mode", Adapter.iDbTransaction);
                iTrading.Db.Db.AddParameter(selectAccount, "@account", "VarChar", 50);
                iTrading.Db.Db.AddParameter(selectAccount, "@broker", "Integer");
                iTrading.Db.Db.AddParameter(selectAccount, "@mode", "Integer");
                selectAccount.Prepare();
            }
            selectAccount.Transaction = Adapter.iDbTransaction;
            iTrading.Db.Db.GetParameter(selectAccount, "@account").Value = account.Name;
            iTrading.Db.Db.GetParameter(selectAccount, "@broker").Value = account.Connection.Options.Provider.Id;
            iTrading.Db.Db.GetParameter(selectAccount, "@mode").Value = account.Connection.Options.Mode.Id;
            IDataReader reader = selectAccount.ExecuteReader();
            if (reader.Read())
            {
                int num = (int) reader["id"];
                reader.Close();
                return num;
            }
            reader.Close();
            Adapter.BeginTransactionNow();
            insertAccount = iTrading.Db.Db.NewCommand("insert into tm_accounts(account, broker, simulation, mode, numbytes, customtext) values (@account, @broker, @simulation, @mode, @numbytes, @customtext)", Adapter.iDbTransaction);
            iTrading.Db.Db.AddParameter(insertAccount, "@account", "VarChar", 50);
            iTrading.Db.Db.AddParameter(insertAccount, "@broker", "Integer");
            iTrading.Db.Db.AddParameter(insertAccount, "@simulation", "Integer");
            iTrading.Db.Db.AddParameter(insertAccount, "@mode", "Integer");
            iTrading.Db.Db.AddParameter(insertAccount, "@numbytes", "Integer");
            iTrading.Db.Db.AddParameter(insertAccount, "@customtext", "Binary", 0);
            iTrading.Db.Db.GetParameter(insertAccount, "@account").Value = account.Name;
            iTrading.Db.Db.GetParameter(insertAccount, "@broker").Value = account.Connection.Options.Provider.Id;
            iTrading.Db.Db.GetParameter(insertAccount, "@simulation").Value = account.IsSimulation ? 1 : 0;
            iTrading.Db.Db.GetParameter(insertAccount, "@mode").Value = account.Connection.Options.Mode.Id;
            iTrading.Db.Db.GetParameter(insertAccount, "@numbytes").Value = 0;
            iTrading.Db.Db.GetParameter(insertAccount, "@customtext").Value = new byte[0];
            insertAccount.ExecuteNonQuery();
            Adapter.CommitTransactionNow();
            reader = selectAccount.ExecuteReader();
            reader.Read();
            int num2 = (int) reader["id"];
            reader.Close();
            return num2;
        }

        internal static void Init()
        {
            insertAccount = null;
            selectAccount = null;
            updateAccount = null;
        }

        internal static void Restore(iTrading.Core.Kernel.Account account)
        {
            if (Globals.TraceSwitch.DataBase)
            {
                Trace.WriteLine("(Db) Db.Account.Restore account='" + account.Name + "'");
            }
            GetAccountId(account);
            IDataReader reader = selectAccount.ExecuteReader();
            reader.Read();
            byte[] buffer = new byte[(int) reader["numbytes"]];
            if (buffer.Length > 0)
            {
                reader.GetBytes(2, 0L, buffer, 0, buffer.Length);
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < buffer.Length; i += 2)
                {
                    builder.Append(BitConverter.ToChar(buffer, i));
                }
                account.CustomText = builder.ToString();
            }
            reader.Close();
        }

        internal static void Update(iTrading.Core.Kernel.Account account)
        {
            if (Globals.TraceSwitch.DataBase)
            {
                Trace.WriteLine("(Db) Db.Account.Update account='" + account.Name + "'");
            }
            Adapter.BeginTransactionNow();
            GetAccountId(account);
            char[] chArray = account.CustomText.ToCharArray();
            byte[] buffer = new byte[chArray.Length * 2];
            for (int i = 0; i < chArray.Length; i++)
            {
                buffer[2 * i] = BitConverter.GetBytes(chArray[i])[0];
                buffer[(2 * i) + 1] = BitConverter.GetBytes(chArray[i])[1];
            }
            updateAccount = iTrading.Db.Db.NewCommand("update tm_accounts set numbytes = @numbytes, customtext = @customtext where account = @account and broker = @broker and mode = @mode", Adapter.iDbTransaction);
            iTrading.Db.Db.AddParameter(updateAccount, "@numbytes", "Integer");
            iTrading.Db.Db.AddParameter(updateAccount, "@customtext", "Binary", buffer.Length);
            iTrading.Db.Db.AddParameter(updateAccount, "@account", "VarChar", 50);
            iTrading.Db.Db.AddParameter(updateAccount, "@broker", "Integer");
            iTrading.Db.Db.AddParameter(updateAccount, "@mode", "Integer");
            iTrading.Db.Db.GetParameter(updateAccount, "@numbytes").Value = buffer.Length;
            iTrading.Db.Db.GetParameter(updateAccount, "@customtext").Value = buffer;
            iTrading.Db.Db.GetParameter(updateAccount, "@account").Value = account.Name;
            iTrading.Db.Db.GetParameter(updateAccount, "@broker").Value = account.Connection.Options.Provider.Id;
            iTrading.Db.Db.GetParameter(updateAccount, "@mode").Value = account.Connection.Options.Mode.Id;
            updateAccount.ExecuteNonQuery();
            Adapter.CommitTransactionNow();
        }
    }
}

