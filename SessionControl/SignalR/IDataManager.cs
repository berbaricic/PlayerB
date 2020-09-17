using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SessionControl.SignalR
{
    public interface IDataManager
    {
        public SqlConnection OpenConnection();

        public int GetNumberOfRows();
    }
}
