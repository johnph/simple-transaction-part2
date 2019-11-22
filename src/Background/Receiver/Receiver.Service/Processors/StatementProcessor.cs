namespace Receiver.Service.Processors
{
    using Microsoft.Extensions.Logging;
    using Publisher.Framework.Commands;
    using Publisher.Framework.Messages;
    using Receiver.Service.Documents;
    using Receiver.Service.Helpers;
    using Receiver.Service.Types;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class StatementProcessor : BaseProcessor, IMessageProcessor
    {
        private ILogger _logger;

        public StatementProcessor(IServiceProvider serviceProvider, ILogger<StatementProcessor> logger, IRetryHelper retryHelper) : base(serviceProvider, logger, retryHelper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task<AccountStatement> ProcessAsync(ICommand command)
        {
            var qMessage = (StatementMessage)command;

            var monthlyStatement = await _transactionClient.GetStatement(qMessage.AccountNumber, qMessage.Month);

            var statementDate = new StatementDate(qMessage.Month);

            const string DateFormat = "dd/MM/yyyy";

            var accountStatement = new AccountStatement()
            {
                Key = $"{monthlyStatement.AccountNumber}-{qMessage.Month}",
                Name = qMessage.Name,
                AccountNumber = monthlyStatement.AccountNumber,
                Currency = qMessage.Currency,
                StartDate = statementDate.StartDate.ToString(DateFormat),
                EndDate = statementDate.EndDate.ToString(DateFormat),
                // Logic used here to calculate opening & closing balance is not accurate and needs improvement
                OpeningBalance = monthlyStatement.TransactionDetails.OrderBy(i => i.Date).Select(i => i.CurrentBalance).FirstOrDefault(),
                ClosingBalance = monthlyStatement.TransactionDetails.OrderByDescending(i => i.Date).Select(i => 
                    i.TransactionType == TransactionType.Deposit.ToString() ? 
                        (i.CurrentBalance + i.Amount) :
                        (i.CurrentBalance - i.Amount)
                ).FirstOrDefault(),
                TransactionDetails = monthlyStatement.TransactionDetails.Select(i => new AccountTransaction()
                {
                    Date = i.Date.ToString(DateFormat),
                    TransactionDetail = i.Description,
                    Withdrawal = i.TransactionType == TransactionType.Withdrawal.ToString() ? i.Amount.ToString() : string.Empty,
                    Deposit = i.TransactionType == TransactionType.Deposit.ToString() ? i.Amount.ToString() : string.Empty,
                    Balance = i.TransactionType == TransactionType.Deposit.ToString() ?
                        (i.CurrentBalance + i.Amount) :
                        (i.CurrentBalance - i.Amount)
                })
            };

            return accountStatement;
        }
    }
}
