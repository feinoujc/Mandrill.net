using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Mandrill.Model;
using Mandrill.Serialization;

namespace Mandrill
{
    internal class MandrillRequest
    {
        public string ApiKey { get; }
        public HttpClient HttpClient { get; }
        public JsonSerializerOptions JsonOptions { get; }

        public MandrillRequest(string apiKey, HttpClient httpClient)
        {
            if (apiKey == null) throw new ArgumentNullException(nameof(apiKey));
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            ApiKey = apiKey;
            HttpClient = httpClient;
            JsonOptions = MandrillSerializer.Instance;
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(string requestUri, TRequest value, CancellationToken cancellationToken) where TRequest : MandrillRequestBase
        {
            value.Key = ApiKey;
            var response = await HttpClient.PostAsync(requestUri, JsonContent.Create(value, value.GetType(), options: JsonOptions), cancellationToken).ConfigureAwait(false);
            await EnsureSuccessAsync(response, cancellationToken).ConfigureAwait(false);
            return (await response.Content.ReadFromJsonAsync<TResponse>(JsonOptions, cancellationToken).ConfigureAwait(false))!;
        }

        private async Task<HttpResponseMessage> EnsureSuccessAsync(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            if (!response.IsSuccessStatusCode)
            {
                MandrillErrorResponse error = null;
                try
                {
                    error = await response.Content.ReadFromJsonAsync<MandrillErrorResponse>(JsonOptions, cancellationToken).ConfigureAwait(false);
                }
                catch (Exception) when (!cancellationToken.IsCancellationRequested)
                {
                }

                try
                {
                    response.EnsureSuccessStatusCode();
                }
                catch (HttpRequestException inner)
                {
                    if (error != null)
                    {
                        throw new MandrillException(error, inner);
                    }

                    throw new MandrillException("Request failed", inner);
                }
            }

            return response;
        }
    }
}
