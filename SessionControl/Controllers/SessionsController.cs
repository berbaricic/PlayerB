using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SessionControl.Models;
using StackExchange.Redis;

namespace SessionControl.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SessionsController : ControllerBase
    {
        private readonly IDatabase _cache;

        public SessionsController(IDatabase cache)
        {
            _cache = cache;
        }

        //////GET: Sessions/Id
        //[HttpGet("{sessionId}")]
        //public Session GetSession(string sessionId)
        //{
        //    //get key for cache
        //    string key = RedisStore.GenerateKey();
        //    //get object from cache
        //    var cachedSession = _cache.StringGet(key);
        //    //convert JSON text to .NET object
        //    var session = JsonConvert.DeserializeObject<Session>(cachedSession);
        //    //get timestamp of request
        //    session.RequestTime = RedisStore.GetTimestamp();
        //    //update object in SortedSet
        //    _cache.SortedSetAdd("SortedSetOfRequestsTime", key, session.RequestTime);
        //    return session;
        //}

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
        }

        ////PUT: Sessions/Id
        //[HttpPut("{sessionId}")]
        //public void PutSession(string sessionId, [FromBody] Session session)
        //{
        //    //get timestamp of request
        //    session.RequestTime = RedisStore.GetTimestamp();
        //    //get key ofr cache
        //    string key = RedisStore.GenerateKey();
        //    //update object in Key-Value pairs and SortedSet
        //    _cache.StringSet(key, JsonConvert.SerializeObject(session));
        //    _cache.SortedSetAdd("SortedSetOfRequestsTime", key, session.RequestTime);
        //}
    }
}
