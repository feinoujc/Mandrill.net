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
        public static Uri BaseUrl = new Uri("https://mandrillapp.com/api/1.0/");
        private MandrillMessagesApi _messages;
        private MandrillTemplatesApi _templates;

        public MandrillApi(string apiKey)
        {
            if (apiKey == null) throw new ArgumentNullException("apiKey");
            ApiKey = apiKey;
            HttpClient = new HttpClient {BaseAddress = BaseUrl};
            HttpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mandrill.Net", 
                typeof (MandrillApi).Assembly.GetName().Version.ToString(2)));
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public string ApiKey { get; private set; }

        public HttpClient HttpClient { get; private set; }

        public IMandrillMessagesApi Messages
        {
            get { return _messages ?? (_messages = new MandrillMessagesApi(this)); }
        }

        public IMandrillTemplatesApi Templates
        {
            get { return _templates ?? (_templates = new MandrillTemplatesApi(this)); }
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