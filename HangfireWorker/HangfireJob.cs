using HangfireWorker.StorageWork;
using Newtonsoft.Json;
using SessionLibrary;
using StackExchange.Redis;
using System;

namespace HangfireWorker
{
    public class HangfireJob
    {
        private readonly IDatabase _cache;
        private readonly ISqlDatabaseWork sqlDatabase;
        public HangfireJob(ISqlDatabaseWork sqlDatabase)
        {
            this.sqlDatabase = sqlDatabase;
            IConnectionMultiplexer redis = ConnectionMultiplexer.Connect("redis");
            _cache = redis.GetDatabase();
        }

        public void PersistDataToDatabase()
        {
            int timestampNow;
            int timestampLimit;
            //time when background worker start checking
            timestampNow = (int)new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();
            //sessions with time before this (timestampLimit) are invalid
            timestampLimit = timestampNow - 60;

            //command - ZRANGEBYSCORE key min(-inf) max(timestampLimit)
            //Expired values
            RedisValue[] expireValues = _cache.SortedSetRangeByScore("SortedSetOfRequestsTime", stop: timestampLimit);

            foreach (var expired in expireValues)
            {
                //stringGet(item) --> value
                var cachedSession = _cache.StringGet(expired.ToString());
                var session = JsonConvert.DeserializeObject<Session>(cachedSession);
                //value save to sql db
                this.sqlDatabase.SaveToDatabase(session);
                //After saving to db, remove key from cache
                _cache.KeyDelete(expired.ToString());
                _cache.SortedSetRemove("SortedSetOfRequestsTime", expired);
            }

        }

        public void SendEMail(string name)
        {
            Console.WriteLine("Sending e-mail with name of Youtube video. Name: " + name);
        }

        public void SendNotice(string name)
        {
            Console.WriteLine("Hey you. Your video {0} has been watched. Be proud!", name);
        }

        public void DoSomeWorkAfterSendingEMail()
        {
            Console.WriteLine("Some work after sending email to client.");
        }


    }
}
