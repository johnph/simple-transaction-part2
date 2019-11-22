using System;
using System.Collections.Generic;

namespace SimpleBanking.ConsoleApp
{
    public static class Models
    {
        public class TransactionData
        {
            public DateTime Date { get; set; }
            public string TransactionDetails { get; set; }
            public decimal? Withdrawals { get; set; }
            public decimal? Deposits { get; set; }
            public decimal Balance { get; set; }
        }

        public class TransactionInput
        {
            public DateTime Date { get; set; }
            public TransactionType  TransactionType { get; set; }
            public decimal Amount { get; set; }
            public string Description { get; set; }
        }

        public class TransactionResult
        {
            public int? AccountNumber { get; set; }
            public bool IsSuccessful { get; set; }
            public decimal? Balance { get; set; }
            public string Currency { get; set; }
            public string Message { get; set; }
        }

        public class StatementResult
        {
            public string Key { get; set; }
            public string Name { get; set; }
            public int? AccountNumber { get; set; }
            public string Currency { get; set; }
            public string StartDate { get; set; }
            public string EndDate { get; set; }
            public decimal? OpeningBalance { get; set; }
            public decimal? ClosingBalance { get; set; }
            public IEnumerable<AccountTransaction> TransactionDetails { get; set; }
            public string Message { get; set; }
        }

        public class AccountTransaction
        {
            public string Date { get; set; }
            public string TransactionDetail { get; set; }
            public string Withdrawal { get; set; }
            public string Deposit { get; set; }
            public decimal Balance { get; set; }
        }

        public class SecurityToken
        {
            public string auth_token { get; set; }
        }

        public class Login
        {
            public string UserName { get; set; }
            public string Password { get; set; }
        }

        public enum TransactionType
        {
            Balance =0,
            Deposit = 1,
            Withdrawal = 2
        }
    }
}
