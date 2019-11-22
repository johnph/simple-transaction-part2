namespace Publisher.Framework.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPublishService
    {
        Task PublishAsync(string month);
        Task PublishAsync(string month, IEnumerable<int> accountNumbers);
    }
}
