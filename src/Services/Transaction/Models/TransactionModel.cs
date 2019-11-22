using System;
using Transaction.Framework.Types;

namespace Transaction.WebApi.Models
{
    public class TransactionModel
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }
}
