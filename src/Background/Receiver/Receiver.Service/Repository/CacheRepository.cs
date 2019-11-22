namespace Receiver.Service.Repository
{
    using Newtonsoft.Json;
    using StackExchange.Redis;
    using System;
    using System.Threading.Tasks;

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

        public async Task SetAsync(string key, TValue value, TimeSpan? expiry)
        {
            await _database.KeyDeleteAsync(key);
            await _database.StringSetAsync(key, JsonConvert.SerializeObject(value));
            await _database.KeyExpireAsync(key, expiry);
        }

        public async Task KeyDeleteAsync(string key)
        {
            await _database.KeyDeleteAsync(key);
        }
    }
}
