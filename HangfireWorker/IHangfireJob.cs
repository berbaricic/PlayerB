namespace HangfireWorker
{
    public interface IHangfireJob
    {
        public void DoSomeWorkAfterSendingEMail();
        public void PersistDataToDatabase();
        public void SendEMail(string name);
        public void SendNotice(string name);
    }
}