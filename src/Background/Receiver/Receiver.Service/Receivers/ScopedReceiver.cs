namespace Receiver.Service.Receivers
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Publisher.Framework.Commands;
    using Receiver.Service.Helpers;
    using System;
    using System.Net.Http;

    using System.Threading.Tasks;
    public abstract class ScopedReceiver : BaseReceiver
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IRetryHelper _retryHelper;
        private readonly ILogger _logger;

        public ScopedReceiver(IServiceScopeFactory serviceScopeFactory, IRetryHelper retryHelper, ILogger<ScopedReceiver> logger) : base(logger)
        {
            _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
            _retryHelper = retryHelper ?? throw new ArgumentNullException(nameof(retryHelper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task Process()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                await ProcessInScope(scope.ServiceProvider);
            }
        }

        protected async Task Execute(ICommand qMessage)
        {
            try
            {
                await OnMessageReceived(qMessage);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("ScopedReceiver", ex, "qMessage processing failed in the first attempt");

                try
                {
                    await _retryHelper.Do(OnMessageReceived, qMessage, new TimeSpan(0, 0, 10));
                }
                catch (Exception e)
                {
                    // throwing the exception pushes the message in to error queue
                    throw e;
                }
            }
        }

        public abstract Task ProcessInScope(IServiceProvider serviceProvider);
        public abstract Task OnMessageReceived(ICommand qMessage);
    }
}
