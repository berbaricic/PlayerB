using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SessionControl.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<Session> SaveToDatabase(Session session)
        {
            string sQuery = "SaveToDatabase_StoredProcedure";
            DynamicParameters param = new DynamicParameters();
            param.Add("@SessionId", session.Id);
            param.Add("@Status", session.Status);
            param.Add("@UserAdress", session.UserAdress);
            param.Add("@IdVideo", session.IdVideo);
            param.Add("@RequestTime", session.RequestTime);

            using (IDbConnection db = this.connectionSql.OpenConnection())
            {
                var result = await db.QueryAsync<Session>(sQuery, param, commandType: CommandType.StoredProcedure);
                return result.FirstOrDefault();
            }
        }
    }
}
