namespace Receiver.Service.Clients
{
    using Receiver.Service.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IIdentityClient
    {
        Task<IEnumerable<int>> GetAccountNumbers();
        Task<IEnumerable<UserAccount>> GetUserAccounts();
        Task<UserAccount> GetUserAccount(int accountnumber);
    }
}
