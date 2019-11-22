using Receiver.Service.Models;
namespace Receiver.Service.Clients
{
    using System.Threading.Tasks;

    public interface ITransactionClient
    {
        Task<StatementResultModel> GetStatement(int accountNumber, string month);
    }
}
