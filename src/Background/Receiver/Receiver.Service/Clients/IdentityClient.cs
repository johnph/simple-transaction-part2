namespace Receiver.Service.Clients
{
    using Receiver.Service.Helpers;
    using Receiver.Service.Models;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class IdentityClient : HttpClientBase, IIdentityClient
    {
        public IdentityClient(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }        

        public async Task<IEnumerable<int>> GetAccountNumbers()
        {
            var relativeUri = "api/internal/accountnumbers";
            var result = await GetAsync<IEnumerable<int>>(relativeUri);
            return result;
        }

        public async Task<IEnumerable<UserAccount>> GetUserAccounts()
        {
            var relativeUri = "api/internal/useraccounts";
            var result = await GetAsync<IEnumerable<UserAccount>>(relativeUri);
            return result;
        }

        public async Task<UserAccount> GetUserAccount(int accountnumber)
        {
            var relativeUri = $"api/internal/useraccount/{accountnumber}";
            var result = await GetAsync<UserAccount>(relativeUri);
            return result;
        }

        protected override HttpClient GetScopedHttpClient()
        {
            var httpClient = _httpClientFactory.CreateClient(NamedHttpClients.IdentityServiceClient);
            return httpClient;
        }
    }
}
