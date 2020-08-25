using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;

namespace SessionControl.Models
{
    public static class RedisStore
    {            
        public static string GenerateKey()
        {
            Guid guid = Guid.NewGuid();
            return $"session:{guid}";
        }
        public static int GetTimestamp()
        {
            int timestamp = (int)new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();
            return timestamp;
        }

    }
}
