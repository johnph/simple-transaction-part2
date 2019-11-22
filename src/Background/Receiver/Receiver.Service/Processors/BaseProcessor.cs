namespace Receiver.Service.Processors
{
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using MongoDB.Driver;
    using Publisher.Framework.Commands;
    using Receiver.Service.Clients;
    using Receiver.Service.Documents;
    using Receiver.Service.Helpers;
    using Receiver.Service.Repository;
    using System;
    using System.Threading.Tasks;

    public abstract class BaseProcessor : IMessageProcessor
    {
        protected abstract Task<AccountStatement> ProcessAsync(ICommand command);

        protected readonly IIdentityClient _identityClient;
        protected readonly ITransactionClient _transactionClient;
        protected readonly IDocumentRepository<AccountStatement> _documentRepository;

        private readonly ICacheRepository<AccountStatement> _cacheRepository;
        private readonly int _cacheExpiryInSeconds;
        private readonly ILogger _logger;
        private readonly IRetryHelper _retryHelper;

        public BaseProcessor(IServiceProvider serviceProvider, ILogger<BaseProcessor> logger, IRetryHelper retryHelper)
        {
            _identityClient = (IIdentityClient)serviceProvider.GetService(typeof(IIdentityClient));
            _transactionClient = (ITransactionClient)serviceProvider.GetService(typeof(ITransactionClient));
            _cacheRepository = (ICacheRepository<AccountStatement>)serviceProvider.GetService(typeof(ICacheRepository<AccountStatement>));
            _documentRepository = (IDocumentRepository<AccountStatement>)serviceProvider.GetService(typeof(IDocumentRepository<AccountStatement>));
            _cacheExpiryInSeconds = ((IOptions<CacheExpiry>)serviceProvider.GetService(typeof(IOptions<CacheExpiry>))).Value.ExpiryInSeconds;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _retryHelper = retryHelper ?? throw new ArgumentNullException(nameof(retryHelper));
        }

        public async Task ProcessMessageAsync(ICommand command)
        {
            var document = await ProcessAsync(command);

            if (document != null)
            {
                try
                {
                    await UpdateDocument(document);
                }
                catch (MongoException ex)
                {
                    _logger.LogError("BaseProcessor", ex, "qMessage processing failed due to mongoDb exception");

                    try
                    {
                        await _retryHelper.Do(UpdateDocument, document, new TimeSpan(0, 0, 10), 10);
                    }
                    catch (Exception e)
                    {
                        // throwing the exception pushes the message in to error queue
                        throw e;
                    }
                }
            }
        }

        private async Task UpdateDocument(AccountStatement document)
        {
            await _documentRepository.UpdateAsync(document.Key, document);

            if (_cacheRepository.KeyExistsAsync(document.Key).Result)
            {
                await _cacheRepository.SetAsync(document.Key, document, TimeSpan.FromSeconds(_cacheExpiryInSeconds));
            }
        }
    }
}
