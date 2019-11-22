namespace Transaction.Framework.Data.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Transaction.Framework.Data.Entities;

    public interface IAccountTransactionRepository
    {
        Task Create(AccountTransactionEntity accountTransactionEntity, AccountSummaryEntity accountSummaryEntity);
        Task<IEnumerable<AccountTransactionEntity>> Get(int accountNumber, DateTime startDate, DateTime endDate);
    }
}
