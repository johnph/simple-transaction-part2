namespace Receiver.Service.Receivers
{
    using EasyNetQ;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Publisher.Framework.Commands;
    using Publisher.Framework.Configurations;
    using Publisher.Framework.Messages;
    using Receiver.Service.Helpers;
    using Receiver.Service.Processors;
    using System;
    using System.Threading.Tasks;

    public class TriggerReceiver : ScopedReceiver
    {
        private readonly IBus _bus;
        private readonly ILogger _logger;
        private readonly Func<string, IMessageProcessor> _processor;
        private readonly QueueNames _queueNames;

        public TriggerReceiver(IBus bus, Func<string, IMessageProcessor> processor, IOptions<QueueNames> options, IServiceScopeFactory serviceScopeFactory, IRetryHelper retryHelper, ILogger<TriggerReceiver> logger) : base(serviceScopeFactory, retryHelper, logger)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (serviceScopeFactory == null)
            {
                throw new ArgumentNullException(nameof(serviceScopeFactory));
            }

            if (retryHelper == null)
            {
                throw new ArgumentNullException(nameof(retryHelper));
            }

            _bus = bus ?? throw new ArgumentNullException(nameof(bus));
            _processor = processor ?? throw new ArgumentNullException(nameof(processor));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _queueNames = options.Value;
        }

        public override async Task OnMessageReceived(ICommand qMessage)
        {
            await _processor("trigger").ProcessMessageAsync(qMessage);
        }

        public override async Task ProcessInScope(IServiceProvider serviceProvider)
        {
            await Task.Run(() => _bus.Receive<TriggerMessage>(_queueNames.Trigger, async (qMessage) => {
                await Execute(qMessage);
            }));
        }
    }
}
