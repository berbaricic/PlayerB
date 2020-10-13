using SessionLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HangfireWorker.StorageWork
{
    public interface ISqlDatabaseWork
    {
        Task<Session> SaveToDatabase(Session session);
    }
}
