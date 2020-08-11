using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SessionControl.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace BackgroundWorker
{
    public class SqlDatabase : ISqlDatabase
    {
        private readonly IConnectionSql connectionSql;
        private readonly ILogger<Worker> logger;

        public SqlDatabase(IConnectionSql connectionSql, ILogger<Worker> logger)
        {
            this.connectionSql = connectionSql;
            this.logger = logger;
        }

        public void SaveToDatabase(Session session)
        {
            string sqlSessionInsert = "INSERT INTO Session VALUES ('" + session.Id + "','" + session.Status + "','" +
                            session.UserAdress + "','" + session.IdVideo + "'," + session.RequestTime + ");";
            using (IDbConnection db = this.connectionSql.OpenConnection())
            {
                var rows = db.Execute(sqlSessionInsert);
            }
        }
    }
}
