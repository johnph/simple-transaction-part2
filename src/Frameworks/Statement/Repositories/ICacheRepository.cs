namespace Statement.Framework.Repositories
{
    using System;
    using System.Threading.Tasks;

    public interface ICacheRepository<TValue> where TValue : class
    {
        Task<bool> KeyExistsAsync(string key);
        Task<TValue> GetAsync(string key);
        Task SetAsync(string key, TValue value, TimeSpan? expiry);
    }
}
