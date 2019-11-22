using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Receiver.Service.Repository
{
    public interface ICacheRepository<TValue> where TValue : class
    {
        Task<bool> KeyExistsAsync(string key);
        Task SetAsync(string key, TValue value, TimeSpan? expiry);
        Task KeyDeleteAsync(string key);
    }
}
