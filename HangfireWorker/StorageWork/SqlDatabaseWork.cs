using Dapper;
using HangfireWorker.StorageConnection;
using SessionLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangfireWorker.StorageWork
{
    public class SqlDatabaseWork : ISqlDatabaseWork
    {
        private readonly ISqlDatabaseConnection sqlDatabaseConnection;

        public SqlDatabaseWork(ISqlDatabaseConnection sqlDatabaseConnection)
        {
            this.sqlDatabaseConnection = sqlDatabaseConnection;
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

            using (IDbConnection db = this.sqlDatabaseConnection.CreateConnection())
            {
                var result = await db.QueryAsync<Session>(sQuery, param, commandType: CommandType.StoredProcedure);
                return result.FirstOrDefault();
            }
        }
    }
}
