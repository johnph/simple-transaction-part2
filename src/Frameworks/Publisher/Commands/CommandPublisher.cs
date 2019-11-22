namespace Publisher.Framework.Commands
{
    using EasyNetQ;
    using System;
    using System.Threading.Tasks;

    public class CommandPublisher : ICommandPublisher
    {
        private readonly IBus _bus;

        public CommandPublisher(IBus bus)
        {
            _bus = bus ?? throw new ArgumentNullException(nameof(bus));
        }

        public async Task PublishAsync<T>(string queueName, T message) where T : Command
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            await _bus.SendAsync(queueName, message);
        }
    }
}
