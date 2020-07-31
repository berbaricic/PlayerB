using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace BackgroundWorker
{
    class ConnectionSql : IConnectionSql
    {
        private readonly IConfiguration configuration;
        public ConnectionSql(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public SqlConnection OpenConnection()
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = configuration.GetConnectionString("sqlconnection");
            connection.Open();
            return connection;
        }

     
    }
}
