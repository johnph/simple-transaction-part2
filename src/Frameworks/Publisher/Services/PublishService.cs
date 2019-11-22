namespace Publisher.Framework.Services
{
    using Microsoft.Extensions.Options;
    using Publisher.Framework.Commands;
    using Publisher.Framework.Configurations;
    using Publisher.Framework.Messages;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class PublishService : IPublishService
    {
        private readonly ICommandPublisher _commandPublisher;
        private readonly QueueNames _queueNames;

        public PublishService(ICommandPublisher commandPublisher, IOptions<QueueNames> options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _commandPublisher = commandPublisher ?? throw new ArgumentNullException(nameof(commandPublisher));
            _queueNames = options.Value;
        }

        public async Task PublishAsync(string month)
        {
            await _commandPublisher.PublishAsync(_queueNames.Trigger, new TriggerMessage() { Month = month });
        }

        public async Task PublishAsync(string month, IEnumerable<int> accountNumbers)
        {
            await _commandPublisher.PublishAsync(_queueNames.Trigger, new TriggerMessage() { Month = month, AccountNumbers = accountNumbers });
        }        
    }
}
