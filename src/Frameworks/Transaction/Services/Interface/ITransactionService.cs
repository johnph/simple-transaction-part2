namespace Transaction.Framework.Services.Interface
{
    using System.Threading.Tasks;
    using Transaction.Framework.Domain;
    using Transaction.Framework.Types;

    public interface ITransactionService
    {
        Task<TransactionResult> Balance(int accountNumber);
        Task<TransactionResult> Deposit(AccountTransaction accountTransaction);
        Task<TransactionResult> Withdraw(AccountTransaction accountTransaction);
        Task<AccountStatement> Statement(int accountNumber, StatementDate statementDate);
    }
}
