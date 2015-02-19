using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Mandrill.Http;
using Mandrill.Model;

namespace Mandrill
{
    public class MandrillApi
    {
        private static readonly Uri BaseUrl = new Uri("https://mandrillapp.com/api/1.0/");

        private static readonly Func<HttpClient> DefaultHttpClientFactory = () =>
        {
#if DEBUG
// ReSharper disable once UseObjectOrCollectionInitializer
            var client = new HttpClient(new LoggingHandler(new HttpClientHandler()));
#else
            var client = new HttpClient();
#endif
            client.BaseAddress = BaseUrl;
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mandrill.Net",
                typeof (MandrillApi).Assembly.GetName().Version.ToString(2)));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        };

        private MandrillMessagesApi _messages;
        private MandrillRejectsApi _rejects;
        private MandrillTemplatesApi _templates;
        private MandrillUsersApi _users;
        private MandrillTagsApi _tags;

        public MandrillApi(string apiKey) : this(apiKey, DefaultHttpClientFactory())
        {
        }


        public MandrillApi(string apiKey, HttpClient httpClient)
        {
            if (apiKey == null) throw new ArgumentNullException("apiKey");
            ApiKey = apiKey;
            HttpClient = httpClient;
        }

        public string ApiKey { get; private set; }

        public HttpClient HttpClient { get; private set; }

        public IMandrillMessagesApi Messages
        {
            get { return _messages ?? (_messages = new MandrillMessagesApi(this)); }
        }

        public IMandrillTagsApi Tags
        {
            get { return _tags ?? (_tags = new MandrillTagsApi(this)); }
        }

        public IMandrillTemplatesApi Templates
        {
            get { return _templates ?? (_templates = new MandrillTemplatesApi(this)); }
        }

        public IMandrillRejectsApi Rejects
        {
            get { return _rejects ?? (_rejects = new MandrillRejectsApi(this)); }
        }

        public IMandrillUsersApi Users
        {
            get { return _users ?? (_users = new MandrillUsersApi(this)); }
        }

        internal async Task<TResponse> PostAsync<TRequest, TResponse>(string requestUri, TRequest value)
            where TRequest : MandrillRequestBase
        {
            value.Key = ApiKey;
            var response = await HttpClient.PostAsJsonAsync(requestUri, value);
            await EnsureSuccessAsync(response);
            return await response.Content.ReadAsJsonAsync<TResponse>();
        }

        private async Task<HttpResponseMessage> EnsureSuccessAsync(HttpResponseMessage response, [CallerMemberName] string caller = null)
        {
            if (!response.IsSuccessStatusCode)
            {
                MandrillErrorResponse error = null;
                //try to extract the error response json first, swallow if its not there
                try
                {
                    error = await response.Content.ReadAsJsonAsync<MandrillErrorResponse>();
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
                    throw new MandrillException(string.Format("{0} failed", caller), inner);
                }
            }
            return response;
        }
    }
}