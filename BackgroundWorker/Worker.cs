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
                RedisValue[] values = _cache.SortedSetRangeByScore("SortedSetOfRequestsTime", stop: timestampLimit);

                foreach (var item in values)
                {
                    //stringGet(item) --> value
                    var cachedSession = _cache.StringGet(item.ToString());
                    var session = JsonConvert.DeserializeObject<Session>(cachedSession);

                    //value save to sql db
                    string sqlSessionInsert = "INSERT INTO Session VALUES ('" + session.Id + "','" + session.Status + "','" +
                        session.UserAdress + "','" + session.IdVIdeo + "'," + session.RequestTime + ");";
                    using (IDbConnection db = this.OpenConnection())
                    {
                        var rows = db.Execute(sqlSessionInsert);
                    }

                    //After saving to db, remove key from cache
                    _cache.KeyDelete(item.ToString());
                    _cache.SortedSetRemove("SortedSetOfRequestsTime", item);
                }

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
