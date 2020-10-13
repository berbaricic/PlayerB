using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace HangfireWorker.StorageConnection
{
    public class SqlDatabaseConnection : ISqlDatabaseConnection
    {
        public SqlConnection CreateConnection()
        {
            SqlConnection connection = new SqlConnection("Server = database; Database = SessionDatabase; User = sa; Password = Pa&&word2020");
            connection.Open();
            return connection;
        }
    }
}
