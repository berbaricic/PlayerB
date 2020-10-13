using SessionControl.Models;
using SessionLibrary;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundWorker
{
    public interface ISqlDatabase
    {
        Task<Session> SaveToDatabase(Session session);
    }
}
