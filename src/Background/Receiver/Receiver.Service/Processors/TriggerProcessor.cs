namespace Receiver.Service.Processors
{
    using Microsoft.Extensions.Options;
    using Publisher.Framework.Commands;
    using Publisher.Framework.Configurations;
    using Publisher.Framework.Messages;
    using Receiver.Service.Clients;
    using System;
    using System.Threading.Tasks;

    public class TriggerProcessor : IMessageProcessor
    {
        private readonly IIdentityClient _identityClient;
        private readonly ICommandPublisher _commandPublisher;
        private readonly QueueNames _queueNames;

        public TriggerProcessor(ICommandPublisher commandPublisher, IOptions<QueueNames> options, IIdentityClient identityClient)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _commandPublisher = commandPublisher ?? throw new ArgumentNullException(nameof(commandPublisher));
            _identityClient = identityClient ?? throw new ArgumentNullException(nameof(identityClient));

            _queueNames = options.Value;
        }

        public async Task ProcessMessageAsync(ICommand command)
        {
            var qMessage = (TriggerMessage)command;

            if(qMessage.AccountNumbers == null)
            {
                // get the list of user accounts
                var userAccounts = await _identityClient.GetUserAccounts();

                // publish the message in to the queue for each user
                foreach (var userAccount in userAccounts)
                {
                    var statementMessage = new StatementMessage() { Name = userAccount.Name, AccountNumber = userAccount.AccountNumber, Month = qMessage.Month, Currency = userAccount.Currency };
                    await _commandPublisher.PublishAsync(_queueNames.Statement, statementMessage);
                }
            }
            else
            {
                foreach (var accountNumber in qMessage.AccountNumbers)
                {
                    var userAccount = await _identityClient.GetUserAccount(accountNumber);
                    var statementMessage = new StatementMessage() { Name = userAccount.Name, AccountNumber = userAccount.AccountNumber, Month = qMessage.Month, Currency = userAccount.Currency };
                    await _commandPublisher.PublishAsync(_queueNames.Statement, statementMessage);
                }
            }            
        }
    }
}
