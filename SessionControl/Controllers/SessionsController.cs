using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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

        // GET: Sessions?id=id&Status=status&UserAdress=UserAdress&IdVIdeo=IdVIdeo
        [HttpGet]
        public Session GetSessions([FromQuery]Session session)
        {
            //temporary key generation
            Random rnd = new Random();
            int broj = rnd.Next(0,1000);

            //put object into cache
            _cache.StringSet("Key:" + broj.ToString(), JsonConvert.SerializeObject(session));

            //return object from cache
            var cachedSession = _cache.StringGet("Key:" + broj.ToString());
            Session sess = JsonConvert.DeserializeObject<Session>(cachedSession);

            return sess;
        }

        //PUT: Sessions/key:x
        [HttpPut("{key}")]
        public void PutSessions(string key, [FromBody] Session session)
        {
            //save session changes
            _cache.StringSet(key, JsonConvert.SerializeObject(session));
        }
    }
}
