using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Statement.Framework.Repositories
{
    public class CacheRepository<TValue> : ICacheRepository<TValue> where TValue : class
    {
        private readonly IDatabase _database;

        public CacheRepository(IDatabase database)
        {
            _database = database;
        }

        public async Task<bool> KeyExistsAsync(string key)
        {
            return await _database.KeyExistsAsync(key);
        }

       
        public async Task<TValue> GetAsync(string key)
        {
            var cacheValue = await _database.StringGetAsync(key);
            return JsonConvert.DeserializeObject<TValue>(cacheValue);
        }

        public async Task SetAsync(string key, TValue value, TimeSpan? expiry)
        {
            await _database.KeyDeleteAsync(key);
            await _database.StringSetAsync(key, JsonConvert.SerializeObject(value));
            await _database.KeyExpireAsync(key, expiry);
        }
    }
}
