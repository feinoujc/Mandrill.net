using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mandrill.Serialization;
using Newtonsoft.Json;

namespace Mandrill.Http
{
    internal static class HttpClientExtensions
    {
        public static async Task<T> ReadAsJsonAsync<T>(this HttpContent content)
        {
            using (var reader = new JsonTextReader(new StreamReader(await content.ReadAsStreamAsync())))
            {
                return MandrillSerializer.Instance.Deserialize<T>(reader);
            }
        }

        public static Task<HttpResponseMessage> PostAsJsonAsync<T>(this HttpClient client, string requestUri, T value,
            CancellationToken c = default(CancellationToken))
        {
            return client.PostAsync(requestUri, GetJsonHttpContent(value), c);
        }

        public static Task<HttpResponseMessage> PostAsJsonAsync<T>(this HttpClient client, Uri requestUri, T value,
            CancellationToken c = default(CancellationToken))
        {
            return client.PostAsync(requestUri, GetJsonHttpContent(value), c);
        }

        private static HttpContent GetJsonHttpContent(object value)
        {
            return new MandrillJsonContent(value);
        }
    }
}