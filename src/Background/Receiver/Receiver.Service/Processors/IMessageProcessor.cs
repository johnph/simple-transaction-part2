namespace Receiver.Service.Processors
{
    using Publisher.Framework.Commands;
    using System.Threading.Tasks;

    public interface IMessageProcessor
    {
        Task ProcessMessageAsync(ICommand command);
    }
}
