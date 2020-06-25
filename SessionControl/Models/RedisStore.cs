using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;

namespace SessionControl.Models
{
    public static class RedisStore
    {            
        public static string GenerateKey(string sessionId)
        {
            var test = Guid.NewGuid();
            var test1 = new Random().Next(100);
            return $"session:{sessionId}";
        }
        public static int GetTimestamp()
        {
            int timestamp = (int)new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();
            return timestamp;
        }

    }
}
