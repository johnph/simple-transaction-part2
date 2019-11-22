using System;
using System.Collections.Generic;
using System.Text;

namespace Publisher.Framework.Commands
{
    public abstract class Command : ICommand
    {
        public Command()
        {
            this.CorrelationId = Guid.NewGuid();
            this.ReceivedOn = DateTime.UtcNow;
        }

        public Guid CorrelationId { get; set; }
        public DateTime ReceivedOn { get; set; }

    }

    public class Command<T> : Command, ICommand<T>
    {
        public Command(T body) : base()
        {
            Payload = body;
        }

        public object Payload { get; set; }

        public static ICommand Create(object oBody)
        {
            return new Command<T>((T)oBody);
        }

        public T GetBody()
        {
            return (T)Payload;
        }

        public override string ToString()
        {
            return string.Format("CreatedDate={0}, Id={1}, Type={2}", this.ReceivedOn, this.CorrelationId.ToString("N"), typeof(T).Name);
        }
    }
}
