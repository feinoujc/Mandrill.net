using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Mandrill.Model;
using Mandrill.Serialization;
using Newtonsoft.Json;

namespace Mandrill
{
    public class MandrillApi
    {
        private static readonly Uri BaseUrl = new Uri("https://mandrillapp.com/api/1.0/");

        private static readonly string UserAgent =
            $"Mandrill.net/{typeof (MandrillApi).Assembly.GetName().Version.ToString(3)}";

        private MandrillInboundApi _inbound;
        private MandrillMessagesApi _messages;
        private MandrillRejectsApi _rejects;
        private MandrillSendersApi _senders;
        private MandrillSubaccountsApi _subaccounts;
        private MandrillTagsApi _tags;
        private MandrillTemplatesApi _templates;
        private MandrillUsersApi _users;
        private MandrillWebHooksApi _webhooks;
        private MandrillWhitelistsApi _whitelists;

        public MandrillApi(string apiKey)
        {
            if (apiKey == null) throw new ArgumentNullException(nameof(apiKey));
            ApiKey = apiKey;
        }

        public string ApiKey { get; private set; }
        public IMandrillMessagesApi Messages => _messages ?? (_messages = new MandrillMessagesApi(this));

        public IMandrillTagsApi Tags => _tags ?? (_tags = new MandrillTagsApi(this));

        public IMandrillTemplatesApi Templates => _templates ?? (_templates = new MandrillTemplatesApi(this));

        public IMandrillRejectsApi Rejects => _rejects ?? (_rejects = new MandrillRejectsApi(this));

        public IMandrillUsersApi Users => _users ?? (_users = new MandrillUsersApi(this));

        public IMandrillSendersApi Senders => _senders ?? (_senders = new MandrillSendersApi(this));

        public IMandrillWhitelistsApi Whitelists => _whitelists ?? (_whitelists = new MandrillWhitelistsApi(this));

        public IMandrillSubaccountsApi Subaccounts => _subaccounts ?? (_subaccounts = new MandrillSubaccountsApi(this));

        public IMandrillInboundApi Inbound => _inbound ?? (_inbound = new MandrillInboundApi(this));

        public IMandrillWebHooksApi WebHooks => _webhooks ?? (_webhooks = new MandrillWebHooksApi(this));

        internal async Task<TResponse> PostAsync<TRequest, TResponse>(string requestUri, TRequest value)
            where TRequest : MandrillRequestBase
        {
            value.Key = ApiKey;
            
            var request = CreateHttpWebRequest(requestUri);
            using (var inputStream = new MemoryStream())
            using (var jsonWriter = new JsonTextWriter(new StreamWriter(inputStream)))
            {
                MandrillSerializer<TRequest>.Serialize(jsonWriter, value);
                jsonWriter.Flush();
                inputStream.Seek(0, SeekOrigin.Begin);
                using (var requestStream = await request.GetRequestStreamAsync())
                {
                    await inputStream.CopyToAsync(requestStream);
                }
            }

            try
            {
                using (var response = (HttpWebResponse) await request.GetResponseAsync())
                using (var responseStream = response.GetResponseStream())
                using (var jsonReader = new JsonTextReader(new StreamReader(responseStream)))
                {
                    return MandrillSerializer<TResponse>.Deserialize(jsonReader);
                }
            }
            catch (WebException webException)
            {
                throw ExtractMandrillErrorResponse(requestUri, webException);
            }
        }

        internal TResponse Post<TRequest, TResponse>(string requestUri, TRequest value)
            where TRequest : MandrillRequestBase
        {
            value.Key = ApiKey;

            var request = CreateHttpWebRequest(requestUri);
            using (var inputStream = new MemoryStream())
            using (var jsonWriter = new JsonTextWriter(new StreamWriter(inputStream)))
            {
                MandrillSerializer<TRequest>.Serialize(jsonWriter, value);
                jsonWriter.Flush();
                inputStream.Seek(0, SeekOrigin.Begin);
                using (var requestStream = request.GetRequestStream())
                {
                    inputStream.CopyToAsync(requestStream);
                }
            }

            try
            {
                using (var response = (HttpWebResponse)request.GetResponse())
                using (var responseStream = response.GetResponseStream())
                using (var jsonReader = new JsonTextReader(new StreamReader(responseStream)))
                {
                    return MandrillSerializer<TResponse>.Deserialize(jsonReader);
                }
            }
            catch (WebException webException)
            {
                throw ExtractMandrillErrorResponse(requestUri, webException);
            }
        }

        private static MandrillException ExtractMandrillErrorResponse(string requestUri, WebException webException)
        {
            MandrillErrorResponse error = null;
            var webResponse = webException.Response as HttpWebResponse;
            if (webResponse != null)
            {
                try
                {
                    using (var response = webResponse)
                    using (var responseStream = response.GetResponseStream())
                    using (var jsonReader = new JsonTextReader(new StreamReader(responseStream)))
                    {
                        error = MandrillSerializer<MandrillErrorResponse>.Deserialize(jsonReader);
                    }
                }
                catch (Exception)
                {
                    // ignored
                }

                if (error != null)
                {
                    return new MandrillException(error, webException);
                }
            }
            return new MandrillException($"{requestUri} failed", webException);
        }

        protected virtual HttpWebRequest CreateHttpWebRequest(string relativeUri)
        {
            var request = WebRequest.CreateHttp(new Uri(BaseUrl, relativeUri));
            request.UserAgent = UserAgent;
            request.Accept = "application/json";
            request.ContentType = "application/json";
            request.Method = "POST";
            return request;
        }
    }
}