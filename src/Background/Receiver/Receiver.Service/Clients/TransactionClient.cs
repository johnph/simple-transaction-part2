namespace Receiver.Service.Clients
{
    using Receiver.Service.Helpers;
    using Receiver.Service.Models;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class TransactionClient : HttpClientBase, ITransactionClient
    {
        public TransactionClient(IHttpClientFactory httpClientFactory) : base(httpClientFactory) {  }

        public async Task<StatementResultModel> GetStatement(int accountNumber, string month)
        {
            var relativeUri = $"api/internal/{accountNumber}/statement/{month}";
            var result = await GetAsync<StatementResultModel>(relativeUri);
            return result;
        }

        protected override HttpClient GetScopedHttpClient()
        {
            var httpClient = _httpClientFactory.CreateClient(NamedHttpClients.TransactionServiceClient);
            return httpClient;
        }
    }
}
