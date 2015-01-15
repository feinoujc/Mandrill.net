using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mandrill.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mandrill.Http
{
    static class HttpClientExtensions
    {
        public static async Task<JToken> ReadAsJsonAsync(this HttpContent content)
        {
            using (var reader = new JsonTextReader(new StreamReader(await content.ReadAsStreamAsync())))
            {
                return JToken.Load(reader);
            }
        }

        public static async Task<T> ReadAsJsonAsync<T>(this HttpContent content)
        {
            var jtoken = await content.ReadAsJsonAsync();
            return jtoken.ToObject<T>(MandrillSerializer.Instance);
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
            var token = JToken.FromObject(value, MandrillSerializer.Instance);
            return new StringContent(token.ToString(), Encoding.UTF8, "application/json");
        }
    }
}