using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Mandrill.Model;
using Mandrill.Serialization;
using Newtonsoft.Json;

namespace Mandrill
{
    internal class MandrillRequest
    {
        public HttpClient HttpClient { get; }
        public string ApiKey { get; }
        public MandrillRequest(string apiKey, HttpClient httpClient)
        {
            if (apiKey == null) throw new ArgumentNullException(nameof(apiKey));

            ApiKey = apiKey;
            this.HttpClient = httpClient;
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(string requestUri, TRequest value) where TRequest : MandrillRequestBase
        {
            value.Key = ApiKey;
            var response = await PostAsJsonAsync(requestUri, value).ConfigureAwait(false);
            await EnsureSuccessAsync(response).ConfigureAwait(false);
            return await ReadAsJsonAsync<TResponse>(response.Content).ConfigureAwait(false);
        }

        private async Task<HttpResponseMessage> EnsureSuccessAsync(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                MandrillErrorResponse error = null;
                //try to extract the error response json first, swallow if its not there
                try
                {
                    error = await ReadAsJsonAsync<MandrillErrorResponse>(response.Content).ConfigureAwait(false);
                }
                catch (Exception)
                {
                }

                //then call this to get the web exception to wrap
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

        private async Task<T> ReadAsJsonAsync<T>(HttpContent content)
        {
            using (var stream = await content.ReadAsStreamAsync().ConfigureAwait(false))
            using (var reader = new StreamReader(stream))
            using (var jsonReader = new JsonTextReader(reader))
            {
                return MandrillSerializer<T>.Deserialize(jsonReader);
            }
        }

        private async Task<HttpResponseMessage> PostAsJsonAsync<T>(string requestUri, T value,
            CancellationToken c = default(CancellationToken))
        {
            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream))
            using (var content = GetStreamContent(value, writer))
            {
                return await HttpClient.PostAsync(requestUri, content, c).ConfigureAwait(false);
            }
        }

        private StreamContent GetStreamContent<T>(T value, StreamWriter writer)
        {
            using (var jsonWriter = new JsonTextWriter(writer) { CloseOutput = false })
            {
                MandrillSerializer<T>.Serialize(jsonWriter, value);
                jsonWriter.Flush();
            }
            writer.BaseStream.Seek(0, SeekOrigin.Begin);
            var content = new StreamContent(writer.BaseStream);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return content;
        }
    }
}
