using StackExchange.Redis;
using StoreInventorySystem.Application.Interfaces;
using System.Text.Json;

namespace StoreInventorySystem.Infrastructure.Caching
{
    public class RedisCacheService : ICacheService
    {
        private readonly IConnectionMultiplexer _redis;

        public RedisCacheService(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }
            
        public async Task<T?> GetAsync<T>(string key)
        {
            var db = _redis.GetDatabase();

            var value = await db.StringGetAsync(key);

            if(!value.HasValue)
                return default;
            
            return JsonSerializer.Deserialize<T>(value.ToString());
        }

        public async Task RemoveAsync(string key)
        {
            var db = _redis.GetDatabase();

            await db.KeyDeleteAsync(key);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan ttl)
        {
            var db = _redis.GetDatabase();

            await db.StringSetAsync(key, JsonSerializer.Serialize<T>(value), ttl);
        }
    }
}
