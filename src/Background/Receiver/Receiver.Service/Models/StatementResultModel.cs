using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Receiver.Service.Models
{
    public class StatementResultModel
    {
        public int AccountNumber { get; set; }
        public string Currency { get; set; }
        public string Date { get; set; }
        public IEnumerable<StatementTransactionModel> TransactionDetails { get; set; }
    }

    public class StatementTransactionModel
    {
        public string TransactionType { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public decimal CurrentBalance { get; set; }
    }
}
