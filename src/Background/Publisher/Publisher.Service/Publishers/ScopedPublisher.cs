using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Publisher.Service.Publishers
{
    public abstract class ScopedPublisher : BasePublisher
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ScopedPublisher(IServiceScopeFactory serviceScopeFactory, ILogger<ScopedPublisher> logger) : base(logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task Process()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                await ProcessInScope(scope.ServiceProvider);
            }
        }

        public abstract Task ProcessInScope(IServiceProvider serviceProvider);
    }
}
