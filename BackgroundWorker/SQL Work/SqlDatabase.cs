using Dapper;
using Microsoft.Extensions.Configuration;
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

        public SqlDatabase(IConnectionSql connectionSql)
        {
            this.connectionSql = connectionSql;
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
