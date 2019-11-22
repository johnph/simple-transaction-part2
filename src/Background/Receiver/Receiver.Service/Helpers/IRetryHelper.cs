namespace Receiver.Service.Helpers
{
    using System;
    using System.Threading.Tasks;

    public interface IRetryHelper
    {
        Task Do<T>(Func<T, Task> action, T message, TimeSpan retryInterval, int maxAttemptCount = 3);
    }
}
