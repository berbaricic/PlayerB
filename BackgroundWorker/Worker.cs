using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using Newtonsoft.Json;
using SessionControl.Models;
using System.Configuration;
using Microsoft.Extensions.Configuration;

namespace BackgroundWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> logger;
        private readonly ISqlDatabase sqlDatabase;
        private IDatabase _cache;

        public Worker(ILogger<Worker> logger, ISqlDatabase sqlDatabase, IConfiguration configuration)
        {
            this.logger = logger;
            this.sqlDatabase = sqlDatabase;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            //Connection to Redis Cache
            string configString = Configuration.GetConnectionString("redis");
            var options = ConfigurationOptions.Parse(configString);
            IConnectionMultiplexer redis = ConnectionMultiplexer.Connect(options);
            _cache = redis.GetDatabase();
            return base.StartAsync(cancellationToken);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //await Task.Delay(60 * 1000, stoppingToken);
            int timestampNow;
            int timestampLimit;
            while (!stoppingToken.IsCancellationRequested)
            {
                //time when background worker start checking 
                timestampNow = (int)new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();
                logger.LogInformation("Vrijeme paljenja: " + timestampNow.ToString());
                //sessions with time before this (timestampLimit) are invalid
                timestampLimit = timestampNow - 60;
                logger.LogInformation("Vrijeme limit: " + timestampLimit.ToString());

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

                //command - ZRANGEBYSCORE key min(timestampLimit) max(+inf)
                //Finished values (status = finished)
                RedisValue[] finishedValues = _cache.SortedSetRangeByScore("SortedSetOfRequestsTime", start: timestampLimit + 1);

                foreach (var finished in finishedValues)
                {
                    //stringGet(item) --> value
                    var cachedSession = _cache.StringGet(finished.ToString());
                    var session = JsonConvert.DeserializeObject<Session>(cachedSession);
                    if (session.Status == "FINISHED")
                    {
                        //value save to sql db
                        this.sqlDatabase.SaveToDatabase(session);
                        //After saving to db, remove key from cache
                        _cache.KeyDelete(finished.ToString());
                        _cache.SortedSetRemove("SortedSetOfRequestsTime", finished);
                    }
                    
                }
                //Wait 60 seconds and then repeat
                await Task.Delay(60*1000, stoppingToken);               
            }
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }
    }
}
