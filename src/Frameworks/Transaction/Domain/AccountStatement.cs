using System;
using System.Collections.Generic;
using System.Text;
using Transaction.Framework.Types;

namespace Transaction.Framework.Domain
{
    public class AccountStatement
    {
        public int AccountNumber { get; set; }
        public Currency Currency { get; set; }
        public StatementDate Date { get; set; }
        public IEnumerable<StatementTransaction> TransactionDetails { get; set; }
    }

    public class StatementTransaction
    {
        public TransactionType TransactionType { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public Money Amount { get; set; }
        public Money CurrentBalance { get; set; }
    }
}
