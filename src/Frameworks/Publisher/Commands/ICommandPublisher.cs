namespace Publisher.Framework.Commands
{
    using System.Threading.Tasks;

    public interface ICommandPublisher
    {
        Task PublishAsync<T>(string queueName, T message) where T : Command;
    }
}
