using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace BackgroundWorker
{
    public interface IConnectionSql
    {
        SqlConnection OpenConnection();
    }
}
