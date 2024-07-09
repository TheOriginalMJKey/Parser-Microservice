using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Caching
{
    public class RedisCacheService
    {
        private readonly IDistributedCache _cache;

        public RedisCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan expiration)
        {
            var serializedValue = JsonConvert.SerializeObject(value);
            var bytes = System.Text.Encoding.UTF8.GetBytes(serializedValue);

            await _cache.SetAsync(key, bytes, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration
            });
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var bytes = await _cache.GetAsync(key);

            if (bytes == null || bytes.Length == 0)
            {
                return default(T);
            }

            var serializedValue = System.Text.Encoding.UTF8.GetString(bytes);
            return JsonConvert.DeserializeObject<T>(serializedValue);
        }
    }
}