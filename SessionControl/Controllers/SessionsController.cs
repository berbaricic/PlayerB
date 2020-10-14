using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMqEventBus;
using SessionControl.Models;
using SessionLibrary;
using StackExchange.Redis;

namespace SessionControl.Controllers
{
    [ApiController]
    public class SessionsController : ControllerBase
    {
        private readonly IDatabase _cache;
        private readonly IEventBus _eventBus;

        public SessionsController(IDatabase cache, IEventBus eventBus)
        {
            _cache = cache;
            this._eventBus = eventBus;
        }

        // POST: Sessions
        [Route("/Sessions")]
        [HttpPost]
        public void PostSession([FromBody]Session session)
        {
            //get timestamp of request
            session.RequestTime = RedisStore.GetTimestamp();
            //get key for cache
            string key = RedisStore.GenerateKey();
            //save object in Key-Value pairs and SortedSet
            _cache.StringSetAsync(key, JsonConvert.SerializeObject(session));
            _cache.SortedSetAddAsync("SortedSetOfRequestsTime", key, session.RequestTime);

            //get lenght of sortedset   
            long rowNumber = _cache.SortedSetLength("SortedSetOfRequestsTime");

            IntegrationEvent cacheSizeChangedEvent = new CacheSizeChangedIntegrationEvent(rowNumber);
            _eventBus.Publish(cacheSizeChangedEvent);
        }

        ////POST: Video
        //[Route("/Video")]
        //[HttpPost]
        //public void SendEmailToConsumer([FromBody]Session session)
        //{
        //    string idVideo = session.IdVideo;

        //    backgroundJobClient.Schedule<HangfireJob>(worker => worker.PersistDataToDatabase(), TimeSpan.FromMinutes(1));
        //    backgroundJobClient.Enqueue(() => Console.WriteLine("Hello from controller!"));

        //    //var id1 = _backgroundJobClient.Schedule<HangfireJob>(sender => sender.SendEMail(idVideo), TimeSpan.FromMinutes(5));

        //    //_backgroundJobClient.ContinueJobWith<HangfireJob>(id1, setter => setter.DoSomeWorkAfterSendingEMail());
        //    ////fires every Sunday at 19:30
        //    //_recurringJobManager.AddOrUpdate(idVideo, () => Console.WriteLine("Cron weekly"), Cron.Weekly(0, 19, 30));

        //    ////fire at 12:00 PM (noon) every day
        //    //_recurringJobManager.AddOrUpdate(idVideo + "test", () => Console.WriteLine("Cron expression"), "0 0 12 * * ?");
        //}


    }
}
