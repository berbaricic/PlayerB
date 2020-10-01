using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using RabbitMqEventBus;
using SessionControl.Models;
using StackExchange.Redis;

namespace SessionControl.Controllers
{
    [Route("[controller]")]
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

            IntegrationEvent cacheSizeChangedEvent = new CacheSizeChangedIntegrationEvent(rowNumber -1);
            _eventBus.Publish(cacheSizeChangedEvent);
        }

    }
}
