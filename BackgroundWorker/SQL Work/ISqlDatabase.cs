using SessionControl.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace BackgroundWorker
{
    public interface ISqlDatabase
    {
        void SaveToDatabase(Session session);
    }
}
