using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Configuration;
using Newtonsoft.Json;
using SessionControl.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data.Common;

namespace BackgroundWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;
        private IDatabase _cache;
        

        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            //Connection to Redis Cache
            IConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");
            _cache = redis.GetDatabase();
            return base.StartAsync(cancellationToken);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int timestampNow;
            int timestampLimit;
            while (!stoppingToken.IsCancellationRequested)
            {
                //time when background worker start checking 
                timestampNow = (int)new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();
                //sessions with time before this (timestampLimit) are invalid
                timestampLimit = timestampNow - 60;

                //command - ZRANGEBYSCORE key min(-inf) max(timestampLimit)
                //Expired values
                RedisValue[] expireValues = _cache.SortedSetRangeByScore("SortedSetOfRequestsTime", stop: timestampLimit);
                //command - ZRANGEBYSCORE key min(timestampLimit) max(+inf)
                //Finished values (status = finished)
                RedisValue[] finishedValues = _cache.SortedSetRangeByScore("SortedSetOfRequestsTime", start: timestampLimit + 1);
                _logger.LogInformation("Slijedi posao provjere isteklih sesija.");
                foreach (var expired in expireValues)
                {
                    //stringGet(item) --> value
                    var cachedSession = _cache.StringGet(expired.ToString());
                    var session = JsonConvert.DeserializeObject<Session>(cachedSession);

                    //value save to sql db
                    string sqlSessionInsert = "INSERT INTO Session VALUES ('" + session.Id + "','" + session.Status + "','" +
                        session.UserAdress + "','" + session.IdVideo + "'," + session.RequestTime + ");";
                    using (IDbConnection db = this.OpenConnection())
                    {
                        var rows = db.Execute(sqlSessionInsert);
                    }

                    //After saving to db, remove key from cache
                    _cache.KeyDelete(expired.ToString());
                    _cache.SortedSetRemove("SortedSetOfRequestsTime", expired);
                }
                _logger.LogInformation("Slijedi posao provjere sesija sa statusom FINISHED.");
                foreach (var finished in finishedValues)
                {
                    //stringGet(item) --> value
                    var cachedSession = _cache.StringGet(finished.ToString());
                    var session = JsonConvert.DeserializeObject<Session>(cachedSession);
                    if (session.Status == "FINISHED")
                    {
                        //value save to sql db
                        string sqlSessionInsert = "INSERT INTO Session VALUES ('" + session.Id + "','" + session.Status + "','" +
                            session.UserAdress + "','" + session.IdVideo + "'," + session.RequestTime + ");";
                        using (IDbConnection db = this.OpenConnection())
                        {
                            var rows = db.Execute(sqlSessionInsert);
                        }

                        //After saving to db, remove key from cache
                        _cache.KeyDelete(finished.ToString());
                        _cache.SortedSetRemove("SortedSetOfRequestsTime", finished);
                    }
                    
                }
                //Wait 60 seconds and then repeat
                _logger.LogInformation("Hej! Odradio sam posao, moram odmorit 60 sekundi.");
                await Task.Delay(60*1000, stoppingToken);               
            }
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }
        private SqlConnection OpenConnection()
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = _configuration.GetConnectionString("SQLConnection");
            connection.Open();
            return connection;
        }
    }
}
