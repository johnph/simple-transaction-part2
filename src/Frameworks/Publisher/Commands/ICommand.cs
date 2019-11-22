namespace Publisher.Framework.Commands
{
    using System;

    public interface ICommand
    {
        Guid CorrelationId { get; set; }
        DateTime ReceivedOn { get; set; }
    }

    public interface ICommand<T> : ICommand
    {
        object Payload { get; set; }
        T GetBody();
    }
}
