namespace Receiver.Service.Helpers
{
    using System.Threading.Tasks;

    public interface IHttpClientBase
    {
        Task<TResult> GetAsync<TResult>(string requestUri);
        Task<TResult> PostJsonAsync<TResult, TValue>(string requestUri, TValue value);
        Task<TResult> PutJsonAsync<TResult, TValue>(string requestUri, TValue value);
        Task<TResult> PostAsync<TResult, TValue>(string requestUri, TValue value);
        Task<TResult> PutAsync<TResult, TValue>(string requestUri, TValue value);
        Task PostJsonAsync<TValue>(string requestUri, TValue value);
        Task PutJsonAsync<TValue>(string requestUri, TValue value);
        Task PostAsync<TValue>(string requestUri, TValue value);
        Task PutAsync<TValue>(string requestUri, TValue value);
        Task<TResult> DeleteAsync<TResult>(string requestUri);
        Task DeleteAsync(string requestUri);
    }
}
