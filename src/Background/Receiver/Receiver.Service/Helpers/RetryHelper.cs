namespace Receiver.Service.Helpers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class RetryHelper : IRetryHelper
    {
        public async Task Do<T>(Func<T, Task> action, T message, TimeSpan retryInterval, int maxAttemptCount = 3)
        {
            for (int attempted = 0; attempted < maxAttemptCount; attempted++)
            {
                try
                {
                    if (attempted > 0)
                    {
                        Thread.Sleep(retryInterval);
                    }

                    await action(message).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    if (attempted >= maxAttemptCount)
                        throw ex;
                }
            }
        }
    }
}
