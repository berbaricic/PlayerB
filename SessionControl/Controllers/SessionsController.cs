using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Moq;
using Newtonsoft.Json;
using SessionControl.Models;
using SessionControl.SignalR;
using StackExchange.Redis;

namespace SessionControl.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SessionsController : ControllerBase
    {
        private readonly IDatabase _cache;
        private readonly IHubContext<SessionHub> sessionHub;

        public SessionsController(IDatabase cache, IHubContext<SessionHub> sessionHub)
        {
            _cache = cache;
            this.sessionHub = sessionHub;
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
            var rowNumber = _cache.SortedSetLength("SortedSetOfRequestsTime");
            sessionHub.Clients.All.SendAsync("ShowNumber", rowNumber);           
        }

        //protected virtual void OnCacheChanged(CacheChangedEventArgs e)
        //{
        //    sessionHub.OnCacheChange?.Invoke(this, e);
        //}

    }
}
