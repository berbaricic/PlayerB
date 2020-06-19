using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace SessionControl.Models
{
    public static class RedisStore
    {            
        public static string GenerateKey(string idSesije)
        {
            return $"session:{idSesije}";
        }

    }
}
