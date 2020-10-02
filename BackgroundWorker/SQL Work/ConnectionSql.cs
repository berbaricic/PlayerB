using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;

namespace BackgroundWorker
{
    class ConnectionSql : IConnectionSql
    {
        private readonly ILogger<Worker> logger;

        public ConnectionSql(IConfiguration configuration, ILogger<Worker> logger)
        {
            Configuration = configuration;
            this.logger = logger;
        }

        public IConfiguration Configuration { get; }

        public SqlConnection OpenConnection()
        {
            SqlConnection connection = new SqlConnection("Server = database; Database = SessionDatabase; User = sa; Password = Pa&&word2020");
            connection.Open();
            return connection;
        }

     
    }
}
