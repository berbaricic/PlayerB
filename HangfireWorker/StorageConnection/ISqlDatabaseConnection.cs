using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace HangfireWorker.StorageConnection
{
    public interface ISqlDatabaseConnection
    {
        SqlConnection CreateConnection();
    }
}
