using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace BackgroundWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private IDatabase _cache;
        

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            IConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");
            _cache = redis.GetDatabase();
            return base.StartAsync(cancellationToken);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int timestampNow=0;
            int timestampLimit=0;
            while (!stoppingToken.IsCancellationRequested)
            {
                //time when background worker start checking 
                timestampNow = (int)new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();
                //sessions with time before this (timestampLimit) are invalid
                timestampLimit = timestampNow - 60;

                //command - ZRANGEBYSCORE key min(start) max(stop)
                RedisValue[] values = _cache.SortedSetRangeByScore("SortedSetOfRequestsTime", stop: timestampLimit);

                foreach (var item in values)
                {
                    _logger.LogInformation("Values:" + item);

                    //stringGet(item) --> value
                    //value save to sql db
                    //KeyDelete(RedisKey key, CommandFlags flags = CommandFlags.None) OR -->
                }
                //KeyDelete(RedisKey[] keys, CommandFlags flags = CommandFlags.None)



                await Task.Delay(60*1000, stoppingToken);
            }
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }
    }
}
