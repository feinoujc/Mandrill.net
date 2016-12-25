using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Mandrill.Model;
using Mandrill.Serialization;
using Newtonsoft.Json;

namespace Mandrill
{
    internal class MandrillRequest
    {
        public HttpClient HttpClient { get; } = new HttpClient();
        public string ApiKey { get; }
        private static readonly Lazy<Version> UserAgentVersionLazy = new Lazy<Version>(()=> new AssemblyName(typeof(MandrillRequest).GetTypeInfo().Assembly.FullName).Version);
        private static readonly Uri BaseUrl = new Uri("https://mandrillapp.com/api/1.0/");
        public MandrillRequest(string apiKey)
        { 
            ApiKey = apiKey;
            HttpClient.BaseAddress = BaseUrl;
            HttpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mandrill.net", UserAgentVersionLazy.Value.ToString(3)));
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
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
                    throw new MandrillException(string.Format("Request failed"), inner);
                }
            }
            return response;
        }

        private async Task<T> ReadAsJsonAsync<T>(HttpContent content)
        {
            using (var reader = new JsonTextReader(new StreamReader(await content.ReadAsStreamAsync().ConfigureAwait(false))))
            {
                return MandrillSerializer.Instance.Deserialize<T>(reader);
            }
        }

        private Task<HttpResponseMessage> PostAsJsonAsync<T>(string requestUri, T value,
            CancellationToken c = default(CancellationToken))
        {
            return HttpClient.PostAsync(requestUri, new MandrillJsonContent(value), c);
        }


        private class MandrillJsonContent : HttpContent
        {
            protected MemoryStream Stream { get; private set; }

            public MandrillJsonContent(object value)
            {
                Stream = new MemoryStream();
                var jw = new JsonTextWriter(new StreamWriter(Stream));
                MandrillSerializer.Instance.Serialize(jw, value);
                jw.Flush();
                Stream.Seek(0, SeekOrigin.Begin);
                Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

            protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
            {
                return Stream.CopyToAsync(stream);
            }

            protected override bool TryComputeLength(out long length)
            {
                length = Stream.Length;
                return true;
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    using (Stream)
                    {
                    }
                }
                base.Dispose(disposing);
            }
        }
    }

}