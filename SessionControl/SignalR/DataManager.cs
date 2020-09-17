using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Dapper;

namespace SessionControl.SignalR
{
    public class DataManager : IDataManager
    {
        public SqlConnection OpenConnection()
        {
            SqlConnection connection = new SqlConnection("Server = database; Database = SessionDatabase; User = sa; Password = Pa&&word2020");
            connection.Open();
            return connection;
        }

        public int GetNumberOfRows()
        {
            string sQuery = "SELECT COUNT(*) FROM Session;";

            using (IDbConnection  db = OpenConnection())
            {
                var number = db.Query<int>(sQuery);
                return number.FirstOrDefault();
            }
        }
    }
}
