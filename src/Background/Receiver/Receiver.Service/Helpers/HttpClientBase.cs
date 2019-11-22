namespace Receiver.Service.Helpers
{
    using Newtonsoft.Json;
    using System;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    public abstract class HttpClientBase : IHttpClientBase
    {
        protected readonly IHttpClientFactory _httpClientFactory;

        public HttpClientBase(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<TResult> GetAsync<TResult>(string requestUri)
        {
            TResult objResult = default(TResult);

            using (var client = GetScopedHttpClient())
            {
                using (var response = await client.GetAsync(requestUri))
                {
                    if (TryParse<TResult>(response, out objResult))
                    {
                        return objResult;
                    }

                    using (HttpContent content = response.Content)
                    {
                        throw new HttpRequestException(response.Content.ReadAsStringAsync().Result);
                    }
                }
            }
        }

        public async Task<TResult> PostJsonAsync<TResult, TValue>(string requestUri, TValue value)
        {
            TResult objResult = default(TResult);

            using (var client = GetScopedHttpClient())
            {
                using (var response = await client.PostAsJsonAsync(requestUri, value))
                {
                    if (TryParse<TResult>(response, out objResult))
                    {
                        return objResult;
                    }

                    using (HttpContent content = response.Content)
                    {
                        throw new HttpRequestException(response.Content.ReadAsStringAsync().Result);
                    }
                }
            }
        }

        public async Task<TResult> PutJsonAsync<TResult, TValue>(string requestUri, TValue value)
        {
            TResult objResult = default(TResult);

            using (var client = GetScopedHttpClient())
            {
                using (var response = await client.PutAsJsonAsync(requestUri, value))
                {
                    if (TryParse<TResult>(response, out objResult))
                    {
                        return objResult;
                    }

                    using (HttpContent content = response.Content)
                    {
                        throw new HttpRequestException(response.Content.ReadAsStringAsync().Result);
                    }
                }
            }
        }

        public async Task<TResult> PostAsync<TResult, TValue>(string requestUri, TValue value)
        {
            TResult objResult = default(TResult);
            var stringContent = GetStringContent<TValue>(value);

            using (var client = GetScopedHttpClient())
            {
                using (var response = await client.PostAsync(requestUri, stringContent))
                {
                    if (TryParse<TResult>(response, out objResult))
                    {
                        return objResult;
                    }

                    using (HttpContent content = response.Content)
                    {
                        throw new HttpRequestException(response.Content.ReadAsStringAsync().Result);
                    }
                }
            }
        }

        public async Task<TResult> PutAsync<TResult, TValue>(string requestUri, TValue value)
        {
            TResult objResult = default(TResult);
            var stringContent = GetStringContent<TValue>(value);

            using (var client = GetScopedHttpClient())
            {
                using (var response = await client.PutAsync(requestUri, stringContent))
                {
                    if (TryParse<TResult>(response, out objResult))
                    {
                        return objResult;
                    }

                    using (HttpContent content = response.Content)
                    {
                        throw new HttpRequestException(response.Content.ReadAsStringAsync().Result);
                    }
                }
            }
        }

        public async Task PostJsonAsync<TValue>(string requestUri, TValue value)
        {
            using (var client = GetScopedHttpClient())
            {
                using (var response = await client.PostAsJsonAsync(requestUri, value))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return;
                    }

                    using (HttpContent content = response.Content)
                    {
                        throw new HttpRequestException(response.Content.ReadAsStringAsync().Result);
                    }
                }
            }
        }

        public async Task PutJsonAsync<TValue>(string requestUri, TValue value)
        {
            using (var client = this.GetScopedHttpClient())
            {
                using (var response = await client.PutAsJsonAsync(requestUri, value))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return;
                    }

                    using (HttpContent content = response.Content)
                    {
                        throw new HttpRequestException(response.Content.ReadAsStringAsync().Result);
                    }
                }
            }
        }

        public async Task PostAsync<TValue>(string requestUri, TValue value)
        {
            var stringContent = GetStringContent<TValue>(value);

            using (var client = GetScopedHttpClient())
            {
                using (var response = await client.PostAsync(requestUri, stringContent))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return;
                    }

                    using (HttpContent content = response.Content)
                    {
                        throw new HttpRequestException(response.Content.ReadAsStringAsync().Result);
                    }
                }
            }
        }

        public async Task PutAsync<TValue>(string requestUri, TValue value)
        {
            var stringContent = GetStringContent<TValue>(value);

            using (var client = GetScopedHttpClient())
            {
                using (var response = await client.PutAsync(requestUri, stringContent))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return;
                    }

                    using (HttpContent content = response.Content)
                    {
                        throw new HttpRequestException(response.Content.ReadAsStringAsync().Result);
                    }
                }
            }
        }

        public async Task<TResult> DeleteAsync<TResult>(string requestUri)
        {
            TResult objResult = default(TResult);

            using (var client = GetScopedHttpClient())
            {
                using (var response = await client.DeleteAsync(requestUri))
                {
                    if (TryParse<TResult>(response, out objResult))
                    {
                        return objResult;
                    }

                    using (HttpContent content = response.Content)
                    {
                        throw new HttpRequestException(response.Content.ReadAsStringAsync().Result);
                    }
                }
            }
        }

        public async Task DeleteAsync(string requestUri)
        {
            using (var client = GetScopedHttpClient())
            {
                using (var response = await client.DeleteAsync(requestUri))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return;
                    }

                    using (HttpContent content = response.Content)
                    {
                        throw new HttpRequestException(response.Content.ReadAsStringAsync().Result);
                    }
                }
            }
        }

        protected abstract HttpClient GetScopedHttpClient();


        private StringContent GetStringContent<TValue>(TValue content)
        {
            string jsonString = JsonConvert.SerializeObject(content);

            return new StringContent(
                jsonString,
                 Encoding.UTF8,
                 Constants.MediaTypeAppJson);
        }

        private bool TryParse<TResult>(HttpResponseMessage response, out TResult t)
        {
            if (typeof(TResult).IsAssignableFrom(typeof(HttpResponseMessage)))
            {
                t = (TResult)Convert.ChangeType(response, typeof(TResult));
                return true;
            }

            if (response.IsSuccessStatusCode)
            {
                t = response.Content.ReadAsAsync<TResult>().Result;
                return true;
            }

            t = default(TResult);
            return false;
        }
    }
}
