using System;
using Transaction.Framework.Types;

namespace Transaction.Framework.Domain
{
    public class AccountTransaction
    {
        public int AccountNumber { get; set; }
        public TransactionType TransactionType { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public Money Amount { get; set; }
        public Money CurrentBalance { get; set; }
    }
}
