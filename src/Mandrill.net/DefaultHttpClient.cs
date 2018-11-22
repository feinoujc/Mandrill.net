using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;

namespace Mandrill
{
    internal class DefaultHttpClient : HttpClient
    {
        private DefaultHttpClient()
        {

        }
        private static readonly Lazy<Version> UserAgentVersionLazy = new Lazy<Version>(() => new AssemblyName(typeof(MandrillRequest).GetTypeInfo().Assembly.FullName).Version);
        private static readonly Uri BaseUrl = new Uri("https://mandrillapp.com/api/1.0/");

        public static HttpClient CreateDefault()
        {
            var httpClient = new DefaultHttpClient();
            return ApplyDefaults(httpClient);
        }
        public static HttpClient ApplyDefaults(HttpClient httpClient)
        {
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));

            if (httpClient.BaseAddress == null)
            {
                httpClient.BaseAddress = BaseUrl;
            }

            if (httpClient.DefaultRequestHeaders.UserAgent.Count == 0)
            {
                httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mandrill.net", UserAgentVersionLazy.Value.ToString(3)));

            }
            if (httpClient.DefaultRequestHeaders.Accept.Count == 0)
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            }
            return httpClient;
        }
    }
}
