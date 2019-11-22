namespace Statement.Framework.Services
{
    using Microsoft.Extensions.Options;
    using Statement.Framework.Documents;
    using Statement.Framework.Repositories;
    using System;
    using System.Threading.Tasks;

    public class StatementService : IStatementService
    {
        private readonly IDocumentRepository<AccountStatement> _documentRepository;
        private readonly ICacheRepository<AccountStatement> _cacheRepository;
        private readonly int _cacheExpiryInSeconds;

        public StatementService(IDocumentRepository<AccountStatement> documentRepository, ICacheRepository<AccountStatement> cacheRepository, IOptions<CacheExpiry> options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _documentRepository = documentRepository ?? throw new ArgumentNullException(nameof(documentRepository));
            _cacheRepository = cacheRepository ?? throw new ArgumentNullException(nameof(cacheRepository));
            _cacheExpiryInSeconds = options.Value.ExpiryInSeconds;
        }

        public async Task<AccountStatement> GetAsync(int accountNumber, string month)
        {
            var key = $"{accountNumber}-{month}";

            if (_cacheRepository.KeyExistsAsync(key).Result)
            {
                return await _cacheRepository.GetAsync(key);
            }
            else
            {
                var document = await _documentRepository.GetAsync(key);

                if(document != null)
                {
                    await _cacheRepository.SetAsync(key, document, TimeSpan.FromSeconds(_cacheExpiryInSeconds));
                }

                return document;
            }
        }
    }
}
