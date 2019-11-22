namespace Publisher.Service.Publishers
{
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using System.Threading;
    using System.Threading.Tasks;

    public abstract class BasePublisher : IHostedService
    {
        private readonly ILogger _logger;

        public BasePublisher(ILogger<BasePublisher> logger)
        {
            _logger = logger;
        }

        public virtual Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Hosted Service is starting.");
            return ExecuteAsync(cancellationToken);
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Hosted Service is stopping.");
            await Task.CompletedTask;
        }

        protected virtual async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Hosted Service is Executing.");
            await Process();
        }

        protected abstract Task Process();
    }
}
