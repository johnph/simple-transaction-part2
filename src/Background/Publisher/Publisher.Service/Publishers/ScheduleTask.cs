namespace Publisher.Service.Publishers
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Publisher.Framework.Services;
    using System;
    using System.Threading.Tasks;

    public class ScheduleTask : ScheduledPublisher
    {
        private readonly IPublishService _publishService;

        public ScheduleTask(IServiceScopeFactory serviceScopeFactory, ILogger<ScheduleTask> logger, IPublishService publishService) : base(serviceScopeFactory, logger)
        {
            _publishService = publishService ?? throw new ArgumentNullException(nameof(publishService));
        }

        protected override string Schedule => "0 0 1 * *";

        public override Task ProcessInScope(IServiceProvider serviceProvider)
        {
            Console.WriteLine("Processing starts here");
            _publishService.PublishAsync(DateTime.Now.AddDays(-1).ToString("MMM-yyyy"));
            return Task.CompletedTask;
        }
    }
}
