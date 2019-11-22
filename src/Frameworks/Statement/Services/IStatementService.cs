namespace Statement.Framework.Services
{
    using Statement.Framework.Documents;
    using System.Threading.Tasks;

    public interface IStatementService
    {
        Task<AccountStatement> GetAsync(int accountNumber, string month);
    }
}
