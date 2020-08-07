using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;

namespace BackgroundWorker
{
    class ConnectionSql : IConnectionSql
    {
        public ConnectionSql(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public SqlConnection OpenConnection()
        {
            SqlConnection connection = new SqlConnection
            {
                ConnectionString = Configuration.GetConnectionString("database")
            };
            connection.Open();
            return connection;
        }

     
    }
}
