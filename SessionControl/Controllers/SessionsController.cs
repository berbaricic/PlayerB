using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        //GET: Sessions/sessionId
        [HttpGet("{sessionId}")]
        public async Task<ActionResult<Session>> GetSession(string sessionId)
        {
            string key = RedisStore.GenerateKey(sessionId);
            var cachedSession = _cache.StringGet(key);
            var session = JsonConvert.DeserializeObject<Session>(cachedSession);
            return session;
        }

        // POST
        [HttpPost]
        public void PostSession([FromBody]Session session)
        {
            string key = RedisStore.GenerateKey(session.Id);
            _cache.StringSet(key, JsonConvert.SerializeObject(session));
        }

        //PUT: Sessions/sessionId
        [HttpPut("{sessionId}")]
        public void PutSession(string sessionId, [FromBody] Session session)
        {
            string key = RedisStore.GenerateKey(sessionId);
            _cache.StringSet(key, JsonConvert.SerializeObject(session));
        }
    }
}
